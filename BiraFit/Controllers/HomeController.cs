using System.Linq;
using System.Web.Mvc;

namespace BiraFit.Controllers
{
    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            return View(Context.Bedarf.ToList());
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