using System;
using BiraFit.Models;
using Microsoft.AspNet.Identity;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using BiraFit.ViewModel;
using BiraFit.Controllers.Helpers;
using NUnit.Framework;

namespace BiraFit.Controllers
{
    public class NachrichtController : BaseController
    {
        // GET: Nachricht
        public ActionResult Index()
        {
            List<Konversation> konversationList;
            if (IsSportler())
            {
                var user = AuthentificationHelper.AuthenticateSportler(User, Context);
                var konversationen = from b in Context.Konversation
                    where b.Sportler_Id == user.Id
                    select b;
                konversationList = konversationen.ToList();
            }
            else
            {
                var user = AuthentificationHelper.AuthenticatePersonalTrainer(User, Context);
                var konversationen = from b in Context.Konversation
                    where b.PersonalTrainer_Id == user.Id
                    select b;
                konversationList = konversationen.ToList();
            }
            
            return View(konversationList);
        }
        
        // GET: Nachricht/Chat/<id>
        public ActionResult Chat(int id)
        {
            if (!CheckPermission(id))
            {
                return RedirectToAction("Index", "Home");
            }

            var chat = from k in Context.Konversation
                where k.Id == id
                from m in k.Nachrichten
                orderby m.Datum
                select m;

            List<Nachricht> chatList = chat.ToList();

            return View(new ChatViewModel { Nachrichten = chatList, KonversationId = id, Id = User.Identity.GetUserId() });
        }

        [HttpPost]
        public ActionResult SendMessage(ChatViewModel message)
        {
            var konversation = Context.Konversation.First(i => i.Id == message.KonversationId);
            string empfaengerId = User.Identity.GetUserId() == GetTrainerAspNetUserId(konversation.PersonalTrainer_Id) ? GetSportlerAspNetUserId(konversation.Sportler_Id) : GetTrainerAspNetUserId(konversation.PersonalTrainer_Id);
            string query =
                $"INSERT INTO Nachricht (Text,Sender_Id,Empfaenger_Id,Datum,Konversation_Id,Konversation_Id1) VALUES ('{message.Nachricht}','{User.Identity.GetUserId()}','{empfaengerId}',CONVERT(datetime, '{DateTime.Now}', 104),{message.KonversationId},{message.KonversationId})";
            Context.Database.ExecuteSqlCommand(query);
            return RedirectToAction("Chat/" + message.KonversationId, "Nachricht");
        }

        public bool CheckPermission(int konversationId)
        {
            if (!IsLoggedIn())
            {
                return false;
            }

            var konversation = GetKonversation(konversationId);

            if (IsSportler())
            {
                return konversation.Sportler_Id == GetUserIdbyAspNetUserId(User.Identity.GetUserId());
            }

            return konversation.PersonalTrainer_Id == GetUserIdbyAspNetUserId(User.Identity.GetUserId());
        }

        public Konversation GetKonversation(int konversationId)
        {
            return Context.Konversation.Single(k => k.Id == konversationId);
        }
    }
}