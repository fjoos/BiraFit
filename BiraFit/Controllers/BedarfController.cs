using System;
using System.Linq;
using System.Web.Mvc;
using BiraFit.Models;
using BiraFit.Controllers.Helpers;
using Microsoft.AspNet.Identity;
using BiraFit.ViewModel;

namespace BiraFit.Controllers
{
    public class BedarfController : BaseController
    {
        public ActionResult Index()
        {
            return RedirectToAction("Index", "Home");
        }

        //
        // GET: /Bedarf/New
        public ActionResult New()
        {
            if (IsSportler())
            {
                var sportlerId = GetAspNetSpecificIdFromUserId(User.Identity.GetUserId());
                if (IsBedarfOpen(sportlerId))
                {
                    return RedirectToAction("Index", "Home");
                }
                return View();
            }
            return RedirectToAction("Index", "Home");
        }

        //
        // POST: /Bedarf/New
        [HttpPost]
        public ActionResult Create(BedarfCreateViewModel model)
        {
            if(ModelState.IsValid)
            {
                Context.Bedarf.Add(new Bedarf()
                {
                    Titel = model.Titel,
                    Beschreibung = model.Beschreibung,
                    Preis = model.Preis,
                    Ort = model.Ort,
                    OpenBedarf = true,
                    Sportler_Id = GetAspNetSpecificIdFromUserId(User.Identity.GetUserId()),
                    Datum = DateTime.Now
                });
                Context.SaveChanges();
                return RedirectToAction("Index", "Home");
            }         
            return View(model);
        }

        //
        // GET: /Bedarf/Edit/1
        public ActionResult Edit(int bedarfId)
        {
            var sportlerId = GetAspNetSpecificIdFromUserId(User.Identity.GetUserId());

            if (IsBedarfOwner(bedarfId, sportlerId))
            {
                var bedarf = Context.Bedarf.Single(b => b.Id == bedarfId);
                return View(bedarf);
            }

            return HttpNotFound();
        }

        //
        // POST: /Bedarf/Delete/1
        public ActionResult Delete(int bedarfId)
        {
            if (IsSportler())
            {
                Bedarf bedarf = Context.Bedarf.Single(i => i.Id == bedarfId);

                if (GetAspNetUserIdFromSportlerId(bedarf.Sportler_Id) == User.Identity.GetUserId())
                {
                    deleteOpenAngebote(bedarf.Id);
                    Context.Bedarf.Remove(bedarf);
                    Context.SaveChanges();
                }
            }
            return RedirectToAction("Index", "Home");
        }

        private void deleteOpenAngebote(int bedarfId)
        {
            if(Context.Angebot.Any(s => s.Bedarf_Id == bedarfId))
            {
                var openAngebote = Context.Angebot.Where((s => s.Bedarf_Id == bedarfId));
                Context.Angebot.RemoveRange(openAngebote);
            }
        }

        //
        // POST: /Bedarf/Edit/1
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
        public ActionResult BedarfNavigation()
        {
            if (IsSportler())
            {
                ViewBag.Type = "Sportler";
                var sportlerId = AuthentificationHelper.AuthenticateSportler(User, Context).Id;
                if (IsBedarfOpen(sportlerId))
                {
                    ViewBag.Id = Context.Bedarf.Single(b => b.Sportler_Id == sportlerId).Id;
                }
            }
           
            return PartialView();
        }
    }
}