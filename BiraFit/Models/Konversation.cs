using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace BiraFit.Models
{
    [Table("Konversation")]
    public class Konversation
    {
        [Key]
        public int Id { get; set; }

        public List<Nachricht> Nachrichten { get; set; }
    }
}