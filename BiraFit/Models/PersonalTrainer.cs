using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BiraFit.Models
{
    [Table("Personaltrainer")]
    public class PersonalTrainer
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public float Bewertung { get; set; }

        [Required]
        public string User_Id { get; set; }

        [ForeignKey("User_Id")]
        public ApplicationUser User { get; set; }

        public List<Angebot> Angebote { get; set; }

        public List<Konversation> Konversationen { get; set; }

        public string Beschreibung { get; set; }

        public int AnzahlBewertungen { get; set; }
    }
}