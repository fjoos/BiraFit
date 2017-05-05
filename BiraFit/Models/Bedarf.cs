using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BiraFit.Models
{
    [Table("Bedarf")]
    public class Bedarf
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public bool OpenBedarf { get; set; }

        [Required]
        public string Titel { get; set; }

        [Required]
        public string Beschreibung { get; set; }

        [Required]
        public int Preis { get; set; }

        [Required]
        public string Ort { get; set; }

        [Required]
        public DateTime Datum { get; set; }

        public List<Angebot> Angebote { get; set; }

        public int Sportler_Id { get; set; }
    }
}