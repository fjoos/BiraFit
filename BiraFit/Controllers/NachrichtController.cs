using BiraFit.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Security.Claims;
using BiraFit.ViewModel;
using BiraFit.Controllers.Helpers;

namespace BiraFit.Controllers
{
    public class NachrichtController : BaseController
    {
        private int Id;
        private bool IsSportler;
        public NachrichtController()
        {
            IsSportler = true;

        }
        // GET: Nachricht
        public ActionResult Index()
        {
            Id = AuthentificationHelper.AuthenticateSportler(User, Context).Id;
            //diesen bereich noch fixen!
            if (Id == -1)
            {
                Id = AuthenticateTrainer();
                IsSportler = false;
            }
            
            if (IsSportler)
            {

                var SportlerKonversationen = from b in Context.Konversation
                                             where b.Sportler_Id == Id
                                             select b;
                List<Konversation> SportlerKonversationList = SportlerKonversationen.ToList<Konversation>();
                return View(SportlerKonversationList);
            }

            var TrainerKonversationen = from b in Context.Konversation
                                        where b.PersonalTrainer_Id == Id
                                        select b;
            return View(TrainerKonversationen);
        }
        
        public ActionResult Chat(int id)
        {
            
            if (!IsLoggedIn())
            {
                return RedirectToAction("Index", "Home");
            }

            var Chat = from k in Context.Konversation
                       where k.Id == id
                       from m in k.Nachrichten
                       orderby m.Datum
                       select m;
            List<Nachricht> ChatList = Chat.ToList();
            
            return View(new ChatViewModel { nachricht = ChatList, id = User.Identity.GetUserId() });
        }

        public int AuthenticateTrainer()
        {
            return 0;
        }

        
    }
}