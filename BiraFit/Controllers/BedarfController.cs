using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BiraFit.Models;
using BiraFit.ViewModel;
using Microsoft.AspNet.Identity;
using System.Security.Claims;

namespace BiraFit.Controllers
{
    public class BedarfController : Controller
    {
        public static List<Bedarf> BedarfList;
        private ApplicationDbContext _context;

        public BedarfController()
        {
            _context = new ApplicationDbContext();
        }

        protected override void Dispose(bool disposing)
        {
           _context.Dispose();
        }

        // GET: Bedarf
        public ActionResult Index()
        {
            var BedarfList = _context.Bedarf.ToList();
            return View(BedarfList);
        }

        public ActionResult New()
        {
            if (AuthenticateSportler() > 0)
            {
                return View();
            }
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public ActionResult Create(Bedarf Bedarf)
        {
            int SportlerId = AuthenticateSportler();
            if (!IsBedarfOpen(SportlerId))
            {
                if (SportlerId > 0)
                {
                    string query = String.Format("INSERT INTO Bedarf (Titel,Beschreibung,Preis,Ort,OpenBedarf,Sportler_Id,Datum) VALUES ('{0}','{1}',{2},'{3}',{4},{5},'{6}')", Bedarf.Titel, Bedarf.Beschreibung, Bedarf.Preis, Bedarf.Ort, 1, SportlerId, DateTime.Now);
                    _context.Database.ExecuteSqlCommand(query);
                    return RedirectToAction("Index", "Bedarf");
                }
                return RedirectToAction("Index", "Home");
            }

            //Redirect Fehler wegen Offenem Bedarf des Users -> Fehlermeldung ausgeben
            return RedirectToAction("New", "Bedarf");
        }

        public ActionResult Edit(int Id)
        {
            int SportlerId = AuthenticateSportler();
            
            if (IsBedarfOwner(Id,SportlerId))
            {
                var Bedarf = _context.Bedarf.Single(b => b.Id == Id);
                return View(Bedarf);
            }
            return HttpNotFound();
        }

        [HttpPost]
        public ActionResult Edit(Bedarf Bedarf)
        {
            var BedarfInDb = _context.Bedarf.Single(c => c.Id == Bedarf.Id);
            TryUpdateModel(BedarfInDb);
            _context.SaveChanges();
            return View();
        }

        public int AuthenticateSportler()
        {
            var CurrentUserId = User.Identity.GetUserId();
            List<Sportler> SportlerList = _context.Sportler.ToList();
            foreach (var Sportler in SportlerList)
            {
                if (Sportler.User_Id == CurrentUserId)
                {
                    return Sportler.Id;
                }
            }
            return -1;
        }

        public bool IsBedarfOpen(int SportlerId)
        {
            List<Bedarf> BedarfList = _context.Bedarf.ToList();

            foreach (var Bedarf in BedarfList)
            {
                if(Bedarf.Sportler_Id == SportlerId)
                {
                    if (Bedarf.OpenBedarf)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public bool IsBedarfOwner(int BedarfId, int SportlerId)
        {
            var Bedürfnisse = _context.Bedarf.ToList();
            
            foreach (var Bedarf in Bedürfnisse)
            {
                if(BedarfId == Bedarf.Id)
                {
                    if (Bedarf.Sportler_Id == SportlerId)
                    {
                        return true;
                    }
                }
                
            }
            return false;
        }

    }
}