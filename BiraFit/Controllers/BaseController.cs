using System.Linq;
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

        protected bool IsSportler()
        {
            var id = User.Identity.GetUserId();
            return Context.Sportler.Any(i => i.User_Id == id);
        }

        protected override void Dispose(bool disposing)
        {
            Context.Dispose();
        }

        protected string GetTrainerAspNetUserId(int trainerId)
        {
            return Context.PersonalTrainer.First(i => i.Id == trainerId).User_Id;
        }

        protected string GetSportlerAspNetUserId(int sportlerId)
        {
            return Context.Sportler.First(i => i.Id == sportlerId).User_Id;
        }

        protected int GetUserIdbyAspNetUserId(string id)
        {
            if (Context.Sportler.Any(i => i.User_Id == id))
            {
                return Context.Sportler.First(i => i.User_Id == id).Id;
            }

            return Context.PersonalTrainer.First(i => i.User_Id == id).Id;
        }


    }
}