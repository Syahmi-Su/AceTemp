using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace AceTC.Models
{
    public class CascadingClass
    {
        [DisplayName("Subject")]
        public string subject { get; set; }

        [DisplayName("Message")]
        public Outstanding message { get; set; }

        [DisplayName("Receiver")]
        public Parent receiver { get; set; }
    }
}