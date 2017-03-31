using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace BiraFit.Models
{
    [Table("Nachricht")]
    public class Nachricht
    {
        [Key]
        [Display(Name = "Id")]
        public int Id { get; set; }

        [Required]
        [ForeignKey("Id")]
        [Display(Name = "KonversationID")]
        public Konversation KonversationID { get; set; }

        [Required]
        [Display(Name = "Text")]
        public string Text { get; set; }

        [Required]
        [Display(Name = "SenderID")]
        public int SenderID { get; set; }

        [Required]
        [Display(Name = "EmpfaengerID")]
        public int TrainerID { get; set; }

        [Required]
        [Display(Name = "Datum")]
        public DateTime Datum { get; set; }
    }
}