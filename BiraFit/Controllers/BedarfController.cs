using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using BiraFit.Models;
using BiraFit.Controllers.Helpers;

namespace BiraFit.Controllers
{
    public class BedarfController : BaseController
    {
        public ActionResult Index()
        {
            return View(Context.Bedarf.ToList());
        }

        public ActionResult New()
        {
            if (AuthentificationHelper.AuthenticateSportler(User, Context).Id > 0)
            {
                return View();
            }

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public ActionResult Create(Bedarf Bedarf)
        {
            int SportlerId = AuthentificationHelper.AuthenticateSportler(User, Context).Id;

            if (!IsBedarfOpen(SportlerId))
            {
                if (SportlerId > 0)
                {
                    string query = string.Format("INSERT INTO Bedarf (Titel,Beschreibung,Preis,Ort,OpenBedarf,Sportler_Id,Datum) VALUES ('{0}','{1}',{2},'{3}',{4},{5},'{6}')", Bedarf.Titel, Bedarf.Beschreibung, Bedarf.Preis, Bedarf.Ort, 1, SportlerId, DateTime.Now);
                    Context.Database.ExecuteSqlCommand(query);
                    return RedirectToAction("Index", "Bedarf");
                }
                return RedirectToAction("Index", "Home");
            }

            //Redirect Fehler wegen Offenem Bedarf des Users -> Fehlermeldung ausgeben
            return RedirectToAction("New", "Bedarf");
        }

        public ActionResult Edit(int Id)
        {
            int SportlerId = AuthentificationHelper.AuthenticateSportler(User, Context).Id;
            
            if (IsBedarfOwner(Id,SportlerId))
            {
                var Bedarf = Context.Bedarf.Single(b => b.Id == Id);
                return View(Bedarf);
            }

            return HttpNotFound();
        }

        [HttpPost]
        public ActionResult Edit(Bedarf Bedarf)
        {
            var BedarfInDb = Context.Bedarf.Single(c => c.Id == Bedarf.Id);
            TryUpdateModel(BedarfInDb);
            Context.SaveChanges();
            return View();
        }
        
        public bool IsBedarfOpen(int SportlerId)
        {
            return Context.Bedarf.Any(b => b.Sportler_Id == SportlerId 
                && b.OpenBedarf);
        }

        public bool IsBedarfOwner(int bedarfId, int sportlerId)
        {
            return Context.Bedarf.Any(b => b.Id == bedarfId && 
                b.Sportler_Id == sportlerId);
        }
    }
}