using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace BiraFit.Models
{
    [Table("Bedarf")]
    public class Bedarf
    {
        [Key]
        [Display(Name = "Id")]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Open")]
        public bool Open { get; set; }

        [Required]
        [Display(Name = "Titel")]
        public string Titel { get; set; }

        [Required]
        [ForeignKey("Id")]
        [Display(Name = "SportlerID")]
        public Sportler SportlerID { get; set; }

        [Required]
        [Display(Name = "Beschreibung")]
        public string Beschreibung { get; set; }

        [Required]
        [Display(Name = "Preis")]
        public int Preis { get; set; }

        [Required]
        [Display(Name = "Ort")]
        public string Ort { get; set; }

        [Required]
        [Display(Name = "Datum")]
        public DateTime Datum { get; set; }


    }
}