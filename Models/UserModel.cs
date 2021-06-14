using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AceTC.Models
{
    public class UserModel
    {
        [DisplayName("User Name")]
        [Required(ErrorMessage = "This Field is Required")]
        public string Username { get; set; }

        [Required(ErrorMessage = "This Field is Required")]
        public string Password { get; set; }

        public string LoginErrorMsg { get; set; }
    }
}