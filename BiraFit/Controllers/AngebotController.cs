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
                        angebotList.Add(new AngebotViewModel()
                        {
                            Angebot = angebot,
                            Bedarf = bedarf
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

            if (Context.Bedarf.Single(i => i.Sportler_Id == sportlerId && i.OpenBedarf).Id != currentAngebot.Bedarf_Id)
            {
                return RedirectToAction("Index", "Home");
            }

            Context.Konversation.Add(new Konversation()
            {
                Sportler_Id = sportlerId,
                PersonalTrainer_Id = personalTrainerId
            });

            var angeboteToRemove = Context.Angebot.Where(i => i.Id != id && i.Bedarf_Id == currentAngebot.Bedarf_Id)
                .ToList();

            foreach (var angebot in angeboteToRemove)
            {
                Context.Angebot.Remove(angebot);
            }

            var bedarfId = Context.Bedarf.Single(i => i.Sportler_Id == sportlerId && i.OpenBedarf);
            Context.Bedarf.Remove(bedarfId);
            Context.SaveChanges();

            //SendMail();

            return RedirectToAction("Chat/" + Context.Konversation
                                        .Single(i => i.Sportler_Id == sportlerId &&
                                                     i.PersonalTrainer_Id == personalTrainerId)
                                        .Id, "Nachricht");
        }

        private void SendMail()
        {
            MailMessage msg = new MailMessage();
            System.Net.Mail.SmtpClient client = new System.Net.Mail.SmtpClient();
            try
            {
                msg.Subject = "Add Subject";
                msg.Body = "Add Email Body Part";
                msg.From = new MailAddress("fabjoos@gmail.com");
                msg.To.Add("enzo.berther@hsr.ch");
                msg.IsBodyHtml = true;
                client.Host = "smtp.gmail.com";
                System.Net.NetworkCredential basicauthenticationinfo = new System.Net.NetworkCredential("fabjoos@gmail.com", "Israel27");
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
                        angebotList.Add(new AngebotViewModel()
                        {
                            Angebot = angebot,
                            Bedarf = bedarf
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