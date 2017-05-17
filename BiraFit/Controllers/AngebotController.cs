using System;
using BiraFit.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using BiraFit.ViewModel;
using Microsoft.AspNet.Identity;
using System.Net.Mail;

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
            var currentSportlerId = GetUserIdbyAspNetUserId(User.Identity.GetUserId());
            var currentBedarf = Context.Bedarf.Where(s => s.Sportler_Id == currentSportlerId).ToList();
            List<AngebotViewModel> angebotList = new List<AngebotViewModel>();
            foreach (var bedarf in currentBedarf)
            {
                if (bedarf.OpenBedarf)
                {
                    foreach (var angebot in Context.Angebot.Where(b => b.Bedarf_Id == bedarf.Id).ToList())
                    {
                        var trainerId = GetTrainerAspNetUserId(angebot.PersonalTrainer_Id);
                        var trainer = Context.Users.Single(s => s.Id == trainerId);
                        var userId = User.Identity.GetUserId();
                        var user = Context.Users.Single(s => s.Id == userId);
                        angebotList.Add(new AngebotViewModel()
                        {
                            Angebot = angebot,
                            Bedarf = bedarf,
                            peronalTrainerId = trainer.Id,
                            trainerUsername = trainer.Email,
                            sportlerPicture = user.ProfilBild,
                            trainerPicture = trainer.ProfilBild
                        });
                    }
                    return View(angebotList);
                }
            }
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public ActionResult Create(BedarfViewModel bedarfviewmodel)
        {
            Context.Angebot.Add(new Angebot()
            {
                Beschreibung = bedarfviewmodel.Beschreibung,
                Datum = DateTime.Now,
                PersonalTrainer_Id = GetUserIdbyAspNetUserId(User.Identity.GetUserId()),
                Preis = bedarfviewmodel.Preis,
                Bedarf_Id = bedarfviewmodel.Id
            });
            Context.SaveChanges();
            return RedirectToAction("Index", "Home");
        }

        // GET: Accept
        public ActionResult Accept(int id)
        {
            if (!IsSportler() || !IsLoggedIn() || !AuthenticateOwner(id))
            {
                return RedirectToAction("Index", "Home");
            }
            var currentAngebot = Context.Angebot.Single(i => i.Id == id);
            var sportlerId = GetUserIdbyAspNetUserId(User.Identity.GetUserId());
            var sportlerUserId = GetSportlerAspNetUserId(sportlerId);
            var personalTrainerId = currentAngebot.PersonalTrainer_Id;
            var personalTrainerUserId = GetTrainerAspNetUserId(personalTrainerId);
            var peronalTrainerEmail = Context.Users.Single(s => s.Id == personalTrainerUserId).Email;
            var sportlerEmail = Context.Users.Single(s => s.Id == sportlerUserId).Email;
            var bedarfId = Context.Bedarf.Single(i => i.Sportler_Id == sportlerId && i.OpenBedarf);

            var angeboteToRemove = Context.Angebot.Where(i => i.Id != id && i.Bedarf_Id == currentAngebot.Bedarf_Id).ToList();
            var canceledAngebote = Context.Angebot.Where(s => s.Bedarf_Id == currentAngebot.Bedarf_Id).ToList();
            canceledAngebote.Remove(currentAngebot);
            startConversation(personalTrainerId, sportlerId);
            /* Bruchts das?, isch miner meinig no scho gwährleischtet bim posta fu agebot.
            if (Context.Bedarf.Single(i => i.Sportler_Id == sportlerId && i.OpenBedarf).Id != currentAngebot.Bedarf_Id)
            {
                return RedirectToAction("Index", "Home");
            }
            */
            Context.Angebot.RemoveRange(angeboteToRemove);
            Context.Bedarf.Remove(bedarfId);
            Context.SaveChanges();

            createEmailForPersonaTrainer(peronalTrainerEmail);
            createEmailForSportler(sportlerEmail);
            createCancelEmails(canceledAngebote);


            return RedirectToAction("Chat/" + Context.Konversation.Single(i => i.Sportler_Id == sportlerId && i.PersonalTrainer_Id == personalTrainerId).Id, "Nachricht");
        }

        // GET: Reject Angebot
        public ActionResult Reject(int id)
        {
            if (!IsSportler() || !IsLoggedIn() || !AuthenticateOwner(id))
            {
                return RedirectToAction("Index", "Home");
            }

            var angebot = Context.Angebot.Single(i => i.Id == id);
            Context.Angebot.Remove(angebot);
            Context.SaveChanges();

            return RedirectToAction("Index", "Angebot");
        }

        // GET: Withdraw (delete)
        public ActionResult Withdraw(int id)
        {
            if (IsSportler() || !IsLoggedIn())
            {
                return RedirectToAction("Index", "Home");
            }
            var personalTrainerId = GetUserIdbyAspNetUserId(User.Identity.GetUserId());
            var angebotToRemove = Context.Angebot.Single(
                i => i.Bedarf_Id == id && i.PersonalTrainer_Id == personalTrainerId);
            Context.Angebot.Remove(angebotToRemove);
            Context.SaveChanges();
            return RedirectToAction("Index", "Home");
        }

        // GET: AngebotList as a Trainer
        public ActionResult AngebotTrainer()
        {
            if (IsSportler() || !IsLoggedIn())
            {
                return RedirectToAction("Index", "Home");
            }
            var currentPersonalTrainerId = GetUserIdbyAspNetUserId(User.Identity.GetUserId());
            var currentAngebot = Context.Angebot.Where(s => s.PersonalTrainer_Id == currentPersonalTrainerId).ToList();
            List<AngebotViewModel> angebotList = new List<AngebotViewModel>();
            foreach (var angebot in currentAngebot)
            {
                foreach (var bedarf in Context.Bedarf.Where(b => b.Id == angebot.Bedarf_Id).ToList())
                {
                    if (bedarf.OpenBedarf)
                    {
                        var sportlerId = GetSportlerAspNetUserId(bedarf.Sportler_Id);
                        var sportler = Context.Users.Single(s => s.Id == sportlerId);
                        angebotList.Add(new AngebotViewModel()
                        {
                            Angebot = angebot,
                            Bedarf = bedarf,
                            sportlerPicture = sportler.ProfilBild,
                            sportlerUsername = sportler.Email
                        });
                    }
                }
            }
            return View(angebotList);
        }

        private void startConversation(int trainerId, int sportlerId)
        {
            if (!Context.Konversation.Any(s => s.PersonalTrainer_Id == trainerId && s.Sportler_Id == sportlerId))
            {
                Context.Konversation.Add(new Konversation()
                {
                    Sportler_Id = sportlerId,
                    PersonalTrainer_Id = trainerId
                });
            }
        }

        private void createEmailForPersonaTrainer(string email)
        {
            var massage = "Ihr Angebot wurde angenommen. Eine neue Konversation ist nun verfügbar.";
            SendMail(email, massage);
        }

        private void createEmailForSportler(string email)
        {
            var massage = "Sie haben ein Angebot angenommen. Eine neue Konversation ist nun verfügbar.";
            SendMail(email, massage);
        }

        private void createCancelEmails(List<Angebot> receiverList)
        {
            var massage = "Ihr Angebot wurde leider abgelehnt. Probieren Sie es weiter!";
            foreach (Angebot cancelAngebot in receiverList)
            {
                var cancelPersonalTrainer = GetTrainerAspNetUserId(cancelAngebot.PersonalTrainer_Id);
                var receiverEmail = Context.Users.Single(s => s.Id == cancelPersonalTrainer).Email;                
                SendMail(receiverEmail, massage);
            }
        }

        private void SendMail(string email, string massage)
        {
            MailMessage msg = new MailMessage();
            SmtpClient client = new SmtpClient();
            try
            {
                msg.Subject = "Angebot angenommen";
                msg.Body = massage;
                msg.From = new MailAddress("birafit17@gmail.com");
                msg.To.Add(email);
                msg.IsBodyHtml = true;
                client.Host = "smtp.gmail.com";
                System.Net.NetworkCredential basicauthenticationinfo = new System.Net.NetworkCredential("birafit17@gmail.com", "Hsr-12345");
                client.Port = int.Parse("587");
                client.EnableSsl = true;
                client.UseDefaultCredentials = false;
                client.Credentials = basicauthenticationinfo;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.Send(msg);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public bool AuthenticateOwner(int angebotId)
        {
            var currentAngebot = Context.Angebot.Single(i => i.Id == angebotId);
            var sportlerId = GetUserIdbyAspNetUserId(User.Identity.GetUserId());
            if (Context.Bedarf.Any(i => i.Sportler_Id == sportlerId))
            {
                return Context.Bedarf.Single(i => i.Sportler_Id == sportlerId && i.OpenBedarf).Id ==
                   currentAngebot.Bedarf_Id;
            }
            return false;
        }

        [ChildActionOnly]
        public ActionResult angebotNavigation()
        {
            if (IsSportler())
            {
                ViewBag.Type = "Sportler";
            }
            else
            {
                ViewBag.Type = "Personaltrainer";
            }
            return PartialView();
        }
    }
}