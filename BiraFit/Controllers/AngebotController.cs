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

        public AngebotController()
        {
            _context = new ApplicationDbContext();
        }

        // GET: Angebot
        public ActionResult Index()
        {
            if (AuthenticateSportler() > 0)
            {
                return View();
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