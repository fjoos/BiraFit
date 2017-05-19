using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using BiraFit.Controllers.Helpers;
using BiraFit.ViewModel;
using Microsoft.AspNet.Identity;
using BiraFit.Models;

namespace BiraFit.Controllers
{
    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            var bedarfList = Context.Bedarf.ToList();
            var bedarfViewModelList = new List<BedarfViewModel>();

            if (IsLoggedIn())
            {
                if (IsSportler())
                {
                    bedarfViewModelList = fillSportlerList(bedarfList);
                }
                else
                {
                    bedarfViewModelList = fillTrainerList(bedarfList);
                }
            }
            else
            {
                bedarfViewModelList = fillAnonymList(bedarfList);
            }

            return View(bedarfViewModelList);
        }

        private List<BedarfViewModel> fillSportlerList(List<Bedarf> bedarfList)
        {
            var result = new List<BedarfViewModel>();
            var currentId = User.Identity.GetUserId();
            bool owner = false;
            foreach (var bedarf in bedarfList)
            {
                var sportlerUserId = GetAspNetUserIdFromSportlerId(bedarf.Sportler_Id);
                var sportler = Context.Users.Single(s => s.Id == sportlerUserId);
                owner = (currentId == GetAspNetUserIdFromSportlerId(bedarf.Sportler_Id));
                result.Add(new BedarfViewModel()
                {
                    Bedarf = bedarf,
                    Sportler = AuthentificationHelper.AuthenticateSportler(User, Context),
                    Trainer = null,
                    IsOwner = owner,
                    OfferMade = false,
                    sportlerProfilbild = sportler.ProfilBild,
                    sportlerEmail = sportler.Email
                });
            }
            result.OrderBy(b => b.Bedarf.Datum);
            return result;
        }

        private List<BedarfViewModel> fillTrainerList(List<Bedarf> bedarfList)
        {
            var result = new List<BedarfViewModel>();
            var personalTrainerId = GetAspNetSpecificIdFromUserId(User.Identity.GetUserId());
            bool offermade = false;

            foreach (var bedarf in bedarfList)
            {
                var sportler = Context.Sportler.Single(s => s.Id == bedarf.Sportler_Id);
                var sportlerUser = Context.Users.Single(s => s.Id == sportler.User_Id);
                offermade = (Context.Angebot.Any(i => i.Bedarf_Id == bedarf.Id && i.PersonalTrainer_Id == personalTrainerId));
                result.Add(new BedarfViewModel()
                {
                    Bedarf = bedarf,
                    Sportler = sportler,
                    Trainer = AuthentificationHelper.AuthenticatePersonalTrainer(User, Context),
                    IsOwner = false,
                    OfferMade = offermade,
                    sportlerProfilbild = sportlerUser.ProfilBild,
                    sportlerEmail = sportlerUser.Email
                });
            }
            result.OrderBy(b => b.Bedarf.Datum);
            return result;
        }

        private List<BedarfViewModel> fillAnonymList(List<Bedarf> bedarfList)
        {
            var result = new List<BedarfViewModel>();
            foreach (var bedarf in bedarfList)
            {
                result.Add(new BedarfViewModel()
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
            result.OrderBy(b => b.Bedarf.Datum);
            return result;
        }

    }
}