using System.Collections.Generic;
using BiraFit.Models;

namespace BiraFit.ViewModel
{
    public class ChatViewModel
    {
        public List<Nachricht> Nachrichten { get; set; }
        public string Nachricht { get; set; }
        public string Id { get; set; }
        public int KonversationId { get; set; }
        public string Empfänger { get; set; }
        public string EmpfängerId { get; set; }
        public bool IsSportler { get; set; }
        public string EmpfängerProfilBild { get; set; }
        public string SenderProfilBild { get; set; }
    }
}