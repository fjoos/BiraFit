using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace BiraFit.Models
{
    public class Angebot
    {
        [Key]
        [Display(Name = "Id")]
        public int Id { get; set; }

        [Required]
        [Display(Name = "TrainerID")]
        public int TrainerID { get; set; }

        [Required]
        [Display(Name = "Beschreibung")]
        public string Beschreibung { get; set; }

        [Required]
        [Display(Name = "Preis")]
        public int Preis { get; set; }

        [Required]
        [Display(Name = "Datum")]
        public DateTime Datum { get; set; }

        [Required]
        [ForeignKey("Id")]
        [Display(Name = "BedarfID")]
        public Bedarf BedarfID { get; set; }
    }
}