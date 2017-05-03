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
                    bedarfViewModelList.Add(new BedarfViewModel() { Bedarf = bedarf, Sportler = null, Trainer = null, IsOwner = false });
                }

                return View(bedarfViewModelList);
            }

            if (IsSportler())
            {
                var currentId = User.Identity.GetUserId();

                foreach (var bedarf in bedarfList)
                {
                    if (currentId == GetSportlerAspNetUserId(bedarf.Sportler_Id))
                    {
                        bedarfViewModelList.Add(new BedarfViewModel()
                        {
                            Bedarf = bedarf,
                            Sportler = AuthentificationHelper.AuthenticateSportler(User, Context),
                            Trainer = null,
                            IsOwner = true
                        });
                    }
                    else
                    {
                        bedarfViewModelList.Add(new BedarfViewModel() { Bedarf = bedarf, Sportler = AuthentificationHelper.AuthenticateSportler(User, Context), Trainer = null, IsOwner = false });
                    }
                }
                return View(bedarfViewModelList);
            }

            foreach (var bedarf in bedarfList)
            {
                bedarfViewModelList.Add(new BedarfViewModel()
                {
                    Bedarf = bedarf,
                    Sportler = null,
                    Trainer = AuthentificationHelper.AuthenticatePersonalTrainer(User, Context),
                    IsOwner = false
                });
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