using System.Collections.Generic;

using BiraFit.Models;
using System;

namespace BiraFit.ViewModel
{
    public class NachrichtViewModel
    {
        public List<Konversation> Konversationen { get; set; }
        public List<string> LastMessages { get; set; }
        public List<string> ProfileImages { get; set; }
        public List<DateTime> SendTimes { get; set; }
        public List<string> UserNames { get; set; }
    }
}