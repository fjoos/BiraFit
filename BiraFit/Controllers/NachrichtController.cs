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
        private int _id;
        private bool _isSportler;
        public NachrichtController()
        {
            _isSportler = true;

        }
        // GET: Nachricht
        public ActionResult Index()
        {
            _id = AuthentificationHelper.AuthenticateSportler(User, Context).Id;
            //diesen bereich noch fixen!
            if (_id == -1)
            {
                _id = AuthenticateTrainer();
                _isSportler = false;
            }
            
            if (_isSportler)
            {

                var SportlerKonversationen = from b in Context.Konversation
                                             where b.Sportler_Id == _id
                                             select b;
                List<Konversation> SportlerKonversationList = SportlerKonversationen.ToList<Konversation>();
                return View(SportlerKonversationList);
            }

            var TrainerKonversationen = from b in Context.Konversation
                                        where b.PersonalTrainer_Id == _id
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