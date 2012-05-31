using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace TheChallenge.Models
{
    public class LoginViewModel
    {
        [Required, StringLength(50)]
        public String UserName { get; set; }

        [Required, MinLength(7)]
        public String Password { get; set; }
    }
}