using System.Collections.Generic;
using BiraFit.Models;

namespace BiraFit.ViewModel
{
    public class BedarfViewModel
    {
        public Sportler Sportler { get; set; }
        public PersonalTrainer Trainer { get; set; }
        public List<Bedarf> BedarfList { get; set; }
    }
}