using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System;
using BiraFit.ViewModel;

namespace BiraFit.Models
{
    // Sie können Profildaten für den Benutzer durch Hinzufügen weiterer Eigenschaften zur ApplicationUser-Klasse hinzufügen. Weitere Informationen finden Sie unter "http://go.microsoft.com/fwlink/?LinkID=317594".
    [Table("AspNetUsers")]
    public class ApplicationUser : IdentityUser
    {
        [Required]
        public DateTime AnmeldeDatum { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Vorname { get; set; }

        [Required]
        public string Adresse { get; set; }

        public string ProfilBild { get; set; }

        [Required]
        public int Aktiv { get; set; }



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

        // : base("DefaultConnection", throwIfV1Schema: false)
        public ApplicationDbContext()
            : base("birafit", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}