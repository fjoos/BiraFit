using BiraFit.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BiraFit.Controllers
{
    public class AngebotController : Controller
    {
        private ApplicationDbContext _context;
        public static List<Angebot> AngebotList;

        public AngebotController()
        {
            _context = new ApplicationDbContext();
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }

        // GET: Angebot
        public ActionResult Index()
        {
            var CurrentUserId = User.Identity.GetUserId();
            var BedarfList = _context.Bedarf.ToList();
            int CurrentSportlerId = AuthenticateSportler();
            List<Angebot> Angebote = new List<Angebot>();

            if (CurrentSportlerId > 0)
            {
                foreach (var Bedarf in BedarfList)
                {
                    if(Bedarf.Sportler_Id == CurrentSportlerId && Bedarf.OpenBedarf)
                    {
                        foreach (var Angebot in _context.Angebot.ToList())
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
    }
}