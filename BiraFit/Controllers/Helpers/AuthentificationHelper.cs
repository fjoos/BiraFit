using BiraFit.Models;
using Microsoft.AspNet.Identity;
using System.Linq;
using System.Security.Principal;

namespace BiraFit.Controllers.Helpers
{
    public static class AuthentificationHelper
    {
       
        public static Sportler AuthenticateSportler(IPrincipal user, ApplicationDbContext context)
        {
            var currentUserId = user.Identity.GetUserId();

            return context.Sportler
                .Where(s => s.User_Id == currentUserId)
                .FirstOrDefault();
        }

        public static PersonalTrainer AuthenticatePersonalTrainer(IPrincipal user, ApplicationDbContext context)
        {
            var currentUserId = user.Identity.GetUserId();

            return context.PersonalTrainer
                .Where(p => p.User_Id == currentUserId)
                .FirstOrDefault();

        }

        
    }
}