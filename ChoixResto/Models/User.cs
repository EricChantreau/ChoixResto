using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ChoixResto.Models
{
    public class User
    {
        public int Id { get; set; }
        [Required, MaxLength(80)]
        public string FirstName { get; set; }
        [Required]
        public string Password { get; set; }
    }
}