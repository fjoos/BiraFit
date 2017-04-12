using BiraFit.Controllers.Helpers;
using BiraFit.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BiraFit.Controllers
{
    public class AngebotController : BaseController
    {
        // GET: Angebot
        public ActionResult Index()
        {
            var CurrentUserId = User.Identity.GetUserId();
            var BedarfList = Context.Bedarf.ToList();
            int CurrentSportlerId = AuthentificationHelper.AuthenticateSportler(User, Context).Id;
            List<Angebot> Angebote = new List<Angebot>();

            if (CurrentSportlerId > 0)
            {
                foreach (var Bedarf in BedarfList)
                {
                    if(Bedarf.Sportler_Id == CurrentSportlerId && Bedarf.OpenBedarf)
                    {
                        foreach (var Angebot in Context.Angebot.ToList())
                        {
                            if(Bedarf.Id == Angebot.Bedarf_Id)
                            {
                                Angebote.Add(Angebot);
                            }
                        }
                    }
                }
                return View(Angebote);
            }
            return RedirectToAction("Index", "Home");
        }
    }
}