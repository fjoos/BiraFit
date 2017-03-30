using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BiraFit.Models
{
    public class Bedarf
    {
        [Required]
        [Display(Name = "Id")]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Open")]
        public bool Open { get; set; }

        [Required]
        [Display(Name = "SportlerID")]
        public int SportlerID { get; set; }

        [Required]
        [Display(Name = "Beschreibung")]
        public string Beschreibung { get; set; }

        [Required]
        [Display(Name = "Preis")]
        public int Preis { get; set; }

        [Required]
        [Display(Name = "Ort")]
        public string Ort { get; set; }


    }
}