using BiraFit.Models;
using Microsoft.AspNet.Identity;
using System.Web.Mvc;

namespace BiraFit.Controllers
{
    public abstract class BaseController : Controller
    {
        protected BaseController()
        {
            Context = new ApplicationDbContext();
        }

        protected ApplicationDbContext Context
        {
            get;
        }

        protected bool IsLoggedIn()
        {
            return User.Identity.GetUserId() != null;
        }

        protected override void Dispose(bool disposing)
        {
            Context.Dispose();
        }


    }
}