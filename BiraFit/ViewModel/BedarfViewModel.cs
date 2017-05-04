using System.Collections.Generic;
using BiraFit.Models;

namespace BiraFit.ViewModel
{
    public class BedarfViewModel
    {
        public Sportler Sportler { get; set; }
        public PersonalTrainer Trainer { get; set; }
        public Bedarf Bedarf { get; set; }
        public bool IsOwner { get; set; }
        public bool OfferMade { get; set; }
        public Angebot Angebot { get; set; }
    }
}