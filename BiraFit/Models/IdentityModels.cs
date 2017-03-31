using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace BiraFit.Models
{
    // Sie können Profildaten für den Benutzer durch Hinzufügen weiterer Eigenschaften zur ApplicationUser-Klasse hinzufügen. Weitere Informationen finden Sie unter "http://go.microsoft.com/fwlink/?LinkID=317594".
    public class ApplicationUser : IdentityUser
    {
        

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Beachten Sie, dass der "authenticationType" mit dem in "CookieAuthenticationOptions.AuthenticationType" definierten Typ übereinstimmen muss.
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Benutzerdefinierte Benutzeransprüche hier hinzufügen
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Bedarf> Bedarf { get; set; }
        public DbSet<Angebot> Angebot { get; set; }
        public DbSet<PersonalTrainer> PersonalTrainer { get; set; }
        public DbSet<Sportler> Sportler { get; set; }
        public DbSet<Konversation> Konversation { get; set; }
        public DbSet<Nachricht> Nachricht { get; set; }


        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}