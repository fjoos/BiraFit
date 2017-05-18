using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using BiraFit.Controllers.Helpers;
using BiraFit.ViewModel;
using Microsoft.AspNet.Identity;

namespace BiraFit.Controllers
{
    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            var bedarfList = Context.Bedarf.ToList();
            var bedarfViewModelList = new List<BedarfViewModel>();
            if (!IsLoggedIn())
            {
                foreach (var bedarf in bedarfList)
                {
                    bedarfViewModelList.Add(new BedarfViewModel()
                    {
                        Bedarf = bedarf,
                        Sportler = null,
                        Trainer = null,
                        IsOwner = false,
                        OfferMade = false,
                        sportlerProfilbild = null,
                        sportlerEmail = null
                    });
                }

                return View(bedarfViewModelList);
            }

            if (IsSportler())
            {
                var currentId = User.Identity.GetUserId();
                foreach (var bedarf in bedarfList)
                {
                    var sportler = Context.Sportler.Single(s => s.Id == bedarf.Sportler_Id);
                    var sportlerId = Context.Users.Single(s => s.Id == sportler.User_Id);
                    if (currentId == GetAspNetUserIdFromSportlerId(bedarf.Sportler_Id))
                    {
                        bedarfViewModelList.Add(new BedarfViewModel()
                        {
                            Bedarf = bedarf,
                            Sportler = AuthentificationHelper.AuthenticateSportler(User, Context),
                            Trainer = null,
                            IsOwner = true,
                            OfferMade = false,
                            sportlerProfilbild = sportlerId.ProfilBild,
                            sportlerEmail = sportlerId.Email
                        });
                    }
                    else
                    {
                        bedarfViewModelList.Add(new BedarfViewModel()
                        {
                            Bedarf = bedarf,
                            Sportler = AuthentificationHelper.AuthenticateSportler(User, Context),
                            Trainer = null,
                            IsOwner = false,
                            OfferMade = false,
                            sportlerProfilbild = sportlerId.ProfilBild,
                            sportlerEmail = sportlerId.Email
                        });
                    }
                }
                return View(bedarfViewModelList);
            }

            foreach (var bedarf in bedarfList)
            {
                var personalTrainerId = GetAspNetSpecificIdFromUserId(User.Identity.GetUserId());
                var sportler = Context.Sportler.Single(s => s.Id == bedarf.Sportler_Id);
                var sportlerId = Context.Users.Single(s => s.Id == sportler.User_Id);
                if (Context.Angebot.Any(i => i.Bedarf_Id == bedarf.Id && i.PersonalTrainer_Id == personalTrainerId))
                {
                   
                    bedarfViewModelList.Add(new BedarfViewModel()
                    {
                        Bedarf = bedarf,
                        Sportler = sportler,
                        Trainer = AuthentificationHelper.AuthenticatePersonalTrainer(User, Context),
                        IsOwner = false,
                        OfferMade = true,
                        sportlerProfilbild = sportlerId.ProfilBild,
                        sportlerEmail = sportlerId.Email
                    });
                }
                else
                {
                    bedarfViewModelList.Add(new BedarfViewModel()
                    {
                        Bedarf = bedarf,
                        Sportler = sportler,
                        Trainer = AuthentificationHelper.AuthenticatePersonalTrainer(User, Context),
                        IsOwner = false,
                        OfferMade = false,
                        sportlerProfilbild = sportlerId.ProfilBild,
                        sportlerEmail = sportlerId.Email
                    });
                }
            }
            return View(bedarfViewModelList);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}