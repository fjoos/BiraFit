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
        public int Id { get; set; }

        [Required]
        [Display(Name = "Nachricht")]
        public string Text { get; set; }

        [Required]
        public string Sender_Id { get; set; }

        [Required]
        public string Empfaenger_Id { get; set; }

        [Required]
        public DateTime Datum { get; set; }
    }
}