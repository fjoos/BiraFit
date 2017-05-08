using System;
using System.Linq;
using System.Web.Mvc;
using BiraFit.Models;
using BiraFit.Controllers.Helpers;
using Microsoft.AspNet.Identity;

namespace BiraFit.Controllers
{
    public class BedarfController : BaseController
    {
        public ActionResult Index()
        {
            return RedirectToAction("Index", "Home");
        }

        public ActionResult New()
        {
            if (!IsSportler() || !IsLoggedIn())
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        [HttpPost]
        public ActionResult Create(Bedarf bedarf)
        {
            var sportlerId = AuthentificationHelper.AuthenticateSportler(User, Context).Id;

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
            return RedirectToAction("New", "Bedarf");
        }

        public ActionResult Edit(int id)
        {
            var sportlerId = AuthentificationHelper.AuthenticateSportler(User, Context).Id;

            if (IsBedarfOwner(id, sportlerId))
            {
                var bedarf = Context.Bedarf.Single(b => b.Id == id);
                return View(bedarf);
            }

            return HttpNotFound();
        }

        public ActionResult Delete(int id)
        {
            if (!IsSportler() || !IsLoggedIn())
            {
                return RedirectToAction("Index", "Home");
            }

            Bedarf bedarf = Context.Bedarf.Single(i => i.Id == id);

            if (GetSportlerAspNetUserId(bedarf.Sportler_Id) == User.Identity.GetUserId())
            {
                Context.Bedarf.Remove(bedarf);
                Context.SaveChanges();
            }

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public ActionResult Edit(Bedarf bedarf)
        {
            var bedarfInDb = Context.Bedarf.Single(c => c.Id == bedarf.Id);
            TryUpdateModel(bedarfInDb);
            Context.SaveChanges();
            return RedirectToAction("Index", "Home");
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


        [ChildActionOnly]
        public ActionResult bedarfNavigation()
        {
            if (IsSportler())
            {
                ViewBag.Type = "Sportler";
                var sportlerId = AuthentificationHelper.AuthenticateSportler(User, Context).Id;
                if (IsBedarfOpen(sportlerId))
                {
                    ViewBag.id = Context.Bedarf.Single(b => b.Sportler_Id == sportlerId).Id;
                }
            }
           
            return PartialView();
        }
    }
}