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
            int CurrentUserId = AuthenticateSportler();
            if (CurrentUserId > 0)
            {
                string query = String.Format("INSERT INTO Bedarf (Titel,Beschreibung,Preis,Ort,OpenBedarf,Sportler_Id,Datum) VALUES ('{0}','{1}',{2},'{3}',{4},{5},'{6}')", Bedarf.Titel, Bedarf.Beschreibung, Bedarf.Preis, Bedarf.Ort, 1, CurrentUserId, DateTime.Now);
                _context.Database.ExecuteSqlCommand(query);
            }
            
            return View();
        }

        public int AuthenticateSportler()
        {
            var CurrentUserId = User.Identity.GetUserId();
            List<Sportler> SportlerList = _context.Sportler.ToList();
            foreach (var item in SportlerList)
            {
                if(item.User_Id == CurrentUserId)
                {
                    return item.Id;
                }
            }
            return -1;
        }

    }
}