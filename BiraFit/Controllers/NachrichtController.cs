using BiraFit.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Security.Claims;

namespace BiraFit.Controllers
{
    public class NachrichtController : Controller
    {
        private ApplicationDbContext _context;
        private int Id;
        private bool IsSportler;
        public NachrichtController()
        {
            IsSportler = true;
            _context = new ApplicationDbContext();

        }
        // GET: Nachricht
        public ActionResult Index()
        {

            Id = AuthenticateSportler();
            if (Id == -1)
            {
                Id = AuthenticateTrainer();
                IsSportler = false;
            }
            var currentUserId = User.Identity.GetUserId();
            if (IsSportler)
            {

                var SportlerKonversationen = from b in _context.Konversation
                                                    where b.Sportler_Id == Id
                                                    select b;
                List<Konversation> SportlerKonversationList = SportlerKonversationen.ToList<Konversation>();
                return View(SportlerKonversationList);
            }

            var TrainerKonversationen = from b in _context.Konversation
                                 where b.PersonalTrainer_Id == Id
                                 select b;
            return View(TrainerKonversationen);
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

        public int AuthenticateTrainer()
        {
            return 0;
        }
    }
}