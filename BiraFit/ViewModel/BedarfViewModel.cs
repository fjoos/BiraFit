using BiraFit.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace BiraFit.ViewModel
{
    public class BedarfViewModel
    {
        public Sportler Sportler { get; set; }
        public PersonalTrainer Trainer { get; set; }
        public Bedarf Bedarf { get; set; }
        public bool IsOwner { get; set; }
        public bool OfferMade { get; set; }
        public int Preis { get; set; }
        public string Beschreibung { get; set; }
        public int Id { get; set; }
        public string sportlerProfilbild { get; set; }
        public string sportlerEmail { get; set; }
    }

    public class BedarfCreateViewModel
    {
            
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

            [Required]
            public int Sportler_Id { get; set; }


    }

    public class BedarfEditViewModel
    {
        [Required]
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

        [Required]
        public int Sportler_Id { get; }


    }

}