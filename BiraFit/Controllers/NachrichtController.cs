using BiraFit.Models;
using Microsoft.AspNet.Identity;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using BiraFit.ViewModel;
using BiraFit.Controllers.Helpers;

namespace BiraFit.Controllers
{
    public class NachrichtController : BaseController
    {
        private int _id;
        private Sportler _sportler;
        private PersonalTrainer _personalTrainer;



        // GET: Nachricht
        public ActionResult Index()
        {
            _sportler = AuthentificationHelper.AuthenticateSportler(User, Context);
            if (_sportler == null)
            {
                _personalTrainer = AuthentificationHelper.AuthenticatePersonalTrainer(User, Context);
            }
            _id = _sportler != null ? _sportler.Id : _personalTrainer.Id;

            if (_sportler != null)
            {
                
                if (_id > 0)
                {
                    var sportlerKonversationen = from b in Context.Konversation
                        where b.Sportler_Id == _id
                        select b;
                    List<Konversation> sportlerKonversationList = sportlerKonversationen.ToList<Konversation>();
                    return View(sportlerKonversationList);
                }

            }

            var trainerKonversationen = from b in Context.Konversation
                                        where b.PersonalTrainer_Id == _id
                                        select b;
            List<Konversation> trainerKonversationList = trainerKonversationen.ToList();
            return View(trainerKonversationList);
        }
        
        public ActionResult Chat(int id)
        {
            
            if (!IsLoggedIn())
            {
                return RedirectToAction("Index", "Home");
            }

            var chat = from k in Context.Konversation
                       where k.Id == id
                       from m in k.Nachrichten
                       orderby m.Datum
                       select m;
            List<Nachricht> chatList = chat.ToList();
            
            return View(new ChatViewModel { nachricht = chatList, id = User.Identity.GetUserId() });
        }

        [HttpPost]
        public ActionResult SendMessage(Nachricht message)
        {
            //continue here
            if (IsLoggedIn())
            {
                if (_sportler != null)
                {
                    var existingKonversation = from k in Context.Konversation
                        from n in Context.Nachricht
                        where k.Sportler_Id == _id
                        select n;
                    List<Nachricht> konversationList = existingKonversation.ToList();

                }
                else
                {
                    
                }
                
                /*
                string query =
                    $"INSERT INTO Bedarf (Titel,Beschreibung,Preis,Ort,OpenBedarf,Sportler_Id,Datum) VALUES ('{bedarf.Titel}','{bedarf.Beschreibung}',{bedarf.Preis},'{bedarf.Ort}',{1},{sportlerId},'{DateTime.Now}')";
                Context.Database.ExecuteSqlCommand(query);
                return RedirectToAction("Index", "Bedarf"); */
            }
            return RedirectToAction("Chat/" + message.Id, "Nachricht");
        }
        public int AuthenticateTrainer()
        {
            return 0;
        }

        
    }
}