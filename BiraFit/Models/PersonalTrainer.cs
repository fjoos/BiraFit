using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace BiraFit.Models
{
    [Table("Personaltrainer")]
    public class PersonalTrainer
    {
        [Key]
        [Display(Name = "Id")]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Bewertung")]
        public int Bewertung { get; set; }

        [Required]
        [Display(Name = "UserID")]
        public int userID { get; set; }
    }
}