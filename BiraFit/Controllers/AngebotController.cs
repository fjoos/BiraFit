using System;
using BiraFit.Controllers.Helpers;
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

            var currentSportlerId = AuthentificationHelper.AuthenticateSportler(User, Context).Id;
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
                        var sportlerId = User.Identity.GetUserId();
                        var sportler = Context.Users.Single(s => s.Id == sportlerId);
                        
                        angebotList.Add(new AngebotViewModel()
                        {
                            Angebot = angebot,
                            Bedarf = bedarf,
                            peronalTrainerId = trainer.Id,
                            trainerUsername = trainer.Email,
                            sportlerPicture = sportler.ProfilBild,
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
            var personalTrainerId = currentAngebot.PersonalTrainer_Id;
            List<Angebot> canceledAngebote = Context.Angebot.Where(s => s.Bedarf_Id == currentAngebot.Bedarf_Id).ToList();
            canceledAngebote.Remove(currentAngebot);
            if (Context.Bedarf.Single(i => i.Sportler_Id == sportlerId && i.OpenBedarf).Id != currentAngebot.Bedarf_Id)
            {
                return RedirectToAction("Index", "Home");
            }

            if(!Context.Konversation.Any(s => s.PersonalTrainer_Id == personalTrainerId && s.Sportler_Id == sportlerId))
            {
                Context.Konversation.Add(new Konversation()
                {
                    Sportler_Id = sportlerId,
                    PersonalTrainer_Id = personalTrainerId
                });
            }

            var angeboteToRemove = Context.Angebot.Where(i => i.Id != id && i.Bedarf_Id == currentAngebot.Bedarf_Id)
                .ToList();

            foreach (var angebot in angeboteToRemove)
            {
                Context.Angebot.Remove(angebot);
            }

            var bedarfId = Context.Bedarf.Single(i => i.Sportler_Id == sportlerId && i.OpenBedarf);
            Context.Bedarf.Remove(bedarfId);
            Context.SaveChanges();
            var ptId = GetTrainerAspNetUserId(personalTrainerId);
            var peronalTrainerEmail = Context.Users.Single(s => s.Id == ptId).Email;

            var spId = GetSportlerAspNetUserId(sportlerId);
            var sportlerEmail = Context.Users.Single(s => s.Id == spId).Email;

            createEmailForPersonaTrainer(peronalTrainerEmail);
            createEmailForSportler(sportlerEmail);
            foreach(Angebot cancelAngebot in canceledAngebote)
            {
                var cancelPersonalTrainer = GetTrainerAspNetUserId(cancelAngebot.PersonalTrainer_Id);
                var cancelPersonalTrainerId = Context.Users.Single(s => s.Id == cancelPersonalTrainer);
                createCancelEmails(cancelPersonalTrainerId.Email);
            }

            return RedirectToAction("Chat/" + Context.Konversation
                                        .Single(i => i.Sportler_Id == sportlerId &&
                                                     i.PersonalTrainer_Id == personalTrainerId)
                                        .Id, "Nachricht");
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

        private void createCancelEmails(string email)
        {
            var massage = "Ihr Angebot wurde leider abgelehnt. Probieren Sie es weiter!";
            SendMail(email, massage);
        }


        private void SendMail(string email, string massage)
        {
            MailMessage msg = new MailMessage();
            System.Net.Mail.SmtpClient client = new System.Net.Mail.SmtpClient();
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

        public bool AuthenticateOwner(int angebotId)
        {
            var currentAngebot = Context.Angebot.Single(i => i.Id == angebotId);
            var sportlerId = GetUserIdbyAspNetUserId(User.Identity.GetUserId());
            if (!Context.Bedarf.Any(i => i.Sportler_Id == sportlerId))
            {
                return false;
            }
            return Context.Bedarf.Single(i => i.Sportler_Id == sportlerId && i.OpenBedarf).Id ==
                   currentAngebot.Bedarf_Id;
        }

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

        public ActionResult AngebotTrainer()
        {
            if (IsSportler() || !IsLoggedIn())
            {
                return RedirectToAction("Index", "Home");
            }

            var currentPersonalTrainerId = AuthentificationHelper.AuthenticatePersonalTrainer(User, Context).Id;
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


        [ChildActionOnly]
        public ActionResult angebotNavigation()
        {
            if (IsSportler())
            {
                ViewBag.Type = "Sportler";
            }
            else
            {
               
            }
            return PartialView();
        }
    }
}