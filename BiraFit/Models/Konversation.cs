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
        [Display(Name = "Id")]
        public int Id { get; set; }

        [Required]
        [ForeignKey("Id")]
        [Display(Name = "SportlerID")]
        public Sportler SportlerID { get; set; }

        [Required]
        [ForeignKey("Id")]
        [Display(Name = "TrainerID")]
        public PersonalTrainer TrainerID { get; set; }
    }
}