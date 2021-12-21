using System;
using System.ComponentModel.DataAnnotations;

namespace WeddingPlanner.Models
{
    public class LogUser
    {
        [Required]
        [EmailAddress]
        public string lemail {get;set;}

        [Required]
        [DataType(DataType.Password)]
        [MinLength(8)]
        public string lpassword {get;set;}
    }
}