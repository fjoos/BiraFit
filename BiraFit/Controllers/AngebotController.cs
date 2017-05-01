using BiraFit.Controllers.Helpers;
using BiraFit.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using BiraFit.ViewModel;

namespace BiraFit.Controllers
{
    public class AngebotController : BaseController
    {
        // GET: Angebot
        public ActionResult Index()
        {
            if (!IsSportler() || !IsLoggedIn())
            {
                return RedirectToAction("Index", "Home");
            }


            var currentSportlerId = AuthentificationHelper.AuthenticateSportler(User, Context).Id;
            var currentBedarf = Context.Bedarf.Where(s => s.Sportler_Id == currentSportlerId).ToList();

            foreach (var bedarf in currentBedarf)
            {
                if (bedarf.OpenBedarf)
                {
                    //return View();
                    return View(new AngebotViewModel() { angebote = Context.Angebot.Where(b => b.Bedarf_Id == bedarf.Id).ToList(),bedarf = bedarf });
                }
            }

            //wird keine Angebote Meldung anzeigen redirect entfernen!
            return RedirectToAction("Index", "Home");
        }
        [HttpPost]
        public ActionResult Create(Angebot angebot)
        {
            Context.Angebot.Add(angebot);
            Context.SaveChanges();
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public ActionResult Accept(int angebotId)
        {
            return RedirectToAction("Index", "Home");
        }
    }
}