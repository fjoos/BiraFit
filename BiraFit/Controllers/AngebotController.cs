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
            var bedarfList = Context.Bedarf.ToList();
            int currentSportlerId = AuthentificationHelper.AuthenticateSportler(User, Context).Id;
            List<Angebot> angebote = new List<Angebot>();

            if (currentSportlerId > 0)
            {
                foreach (var bedarf in bedarfList)
                {
                    if(bedarf.Sportler_Id == currentSportlerId && bedarf.OpenBedarf)
                    {
                        foreach (var angebot in Context.Angebot.ToList())
                        {
                            if(bedarf.Id == angebot.Bedarf_Id)
                            {
                                angebote.Add(angebot);
                            }
                        }
                    }
                }
                return View(angebote);
            }
            return RedirectToAction("Index", "Home");
        }
    }
}