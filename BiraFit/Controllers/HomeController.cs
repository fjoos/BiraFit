using System.Linq;
using System.Web.Mvc;
using BiraFit.Controllers.Helpers;
using BiraFit.ViewModel;

namespace BiraFit.Controllers
{
    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            if (!IsLoggedIn())
            {
                return View(new BedarfViewModel() { BedarfList = Context.Bedarf.ToList(), Sportler = null, Trainer = null });
            }
            if (IsSportler())
            {
                return View(new BedarfViewModel() { BedarfList = Context.Bedarf.ToList(), Sportler = AuthentificationHelper.AuthenticateSportler(User, Context), Trainer = null });
            }

            return View(new BedarfViewModel()
            {
                BedarfList = Context.Bedarf.ToList(),
                Sportler = null,
                Trainer = AuthentificationHelper.AuthenticatePersonalTrainer(User, Context)
            });
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