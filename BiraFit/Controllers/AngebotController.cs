using BiraFit.Controllers.Helpers;
using BiraFit.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

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
            
            int currentSportlerId = AuthentificationHelper.AuthenticateSportler(User, Context).Id;
            var currentBedarf = Context.Bedarf.Single(s => s.Sportler_Id == currentSportlerId && s.OpenBedarf);
            List<Angebot> angebotList = new List<Angebot>(Context.Angebot.Where(b => b.Bedarf_Id == currentBedarf.Id));
           
            return View(angebotList);
            
        }
    }
}