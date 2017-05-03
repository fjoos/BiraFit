using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace BiraFit.Models
{
    [Table("Angebot")]
    public class Angebot
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Beschreibung { get; set; }

        [Required]
        public int Preis { get; set; }

        [Required]
        public DateTime Datum { get; set; }

        public int Bedarf_Id { get; set; }

        public int PersonalTrainer_Id { get; set; }
    }
}