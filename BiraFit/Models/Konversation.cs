using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BiraFit.Models
{
    [Table("Konversation")]
    public class Konversation
    {
        [Key]
        public int Id { get; set; }

        public List<Nachricht> Nachrichten { get; set; }

        public int Sportler_Id { get; set; }

        public int PersonalTrainer_Id { get; set; }

        public bool SportlerDeleted { get; set; }

        public bool TrainerDeleted { get; set; }
    }
}