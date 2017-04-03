using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace BiraFit.Models
{
    [Table("Sportler")]
    public class Sportler
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string User_Id { get; set; }

        [ForeignKey("User_Id")]
        public ApplicationUser User { get; set; }

        public List<Bedarf> Bedurfnisse { get; set; }

        public List<Konversation> Konversationen { get; set; }
    }
}