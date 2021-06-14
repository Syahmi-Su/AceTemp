using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace AceTC.Models
{
    public class CascadingClass
    {
        public string parent_id { get; set; }
        public int outstanding_id { get; set; }

        [DisplayName("Receiver")]
        public string receiver { get; set; }

        [DisplayName("Subject")]
        public string subject { get; set; }

        [DisplayName("Message")]
        public string message { get; set; }


    }
}