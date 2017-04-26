using System;
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
        public ActionResult Create(Bedarf bedarf)
        {
            int sportlerId = AuthentificationHelper.AuthenticateSportler(User, Context).Id;

            if (!IsBedarfOpen(sportlerId))
            {
                if (sportlerId > 0)
                {
                    string query =
                        $"INSERT INTO Bedarf (Titel,Beschreibung,Preis,Ort,OpenBedarf,Sportler_Id,Datum) VALUES ('{bedarf.Titel}','{bedarf.Beschreibung}',{bedarf.Preis},'{bedarf.Ort}',{1},{sportlerId},'{DateTime.Now}')";
                    Context.Database.ExecuteSqlCommand(query);
                    return RedirectToAction("Index", "Bedarf");
                }
                return RedirectToAction("Index", "Home");
            }

            //Redirect Fehler wegen Offenem Bedarf des Users -> Fehlermeldung ausgeben
            return RedirectToAction("New", "Bedarf");
        }

        public ActionResult Edit(int id)
        {
            int sportlerId = AuthentificationHelper.AuthenticateSportler(User, Context).Id;
            
            if (IsBedarfOwner(id,sportlerId))
            {
                var bedarf = Context.Bedarf.Single(b => b.Id == id);
                return View(bedarf);
            }

            return HttpNotFound();
        }

        [HttpPost]
        public ActionResult Edit(Bedarf bedarf)
        {
            var bedarfInDb = Context.Bedarf.Single(c => c.Id == bedarf.Id);
            TryUpdateModel(bedarfInDb);
            Context.SaveChanges();
            return View();
        }
        
        public bool IsBedarfOpen(int sportlerId)
        {
            return Context.Bedarf.Any(b => b.Sportler_Id == sportlerId 
                && b.OpenBedarf);
        }

        public bool IsBedarfOwner(int bedarfId, int sportlerId)
        {
            return Context.Bedarf.Any(b => b.Id == bedarfId && 
                b.Sportler_Id == sportlerId);
        }
    }
}