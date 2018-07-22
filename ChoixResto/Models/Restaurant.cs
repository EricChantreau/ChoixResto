using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ChoixResto.Models
{
    public class Restaurant
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Restaurant name must not be empty")]
        public string Name { get; set; }
        [RegularExpression(@"^0[0-9]{9}$", ErrorMessage = "Restaurant phone is invalid")]
        public string Phone { get; set; }
    }
}