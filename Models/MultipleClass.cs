using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AceTC.Models
{
    public class MultipleClass
    {
        public Student studentdetails { get; set; }
        public Parent parentdetails { get; set; }
        public Outstanding outstandingdetails { get; set; }
        public Payment paymentdetails { get; set; }
        public Status statusdetails { get; set; }
        public Package packagedetails { get; set; }
    }
}