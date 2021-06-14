using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AceTC.Models
{
    public class JoinClass
    {
        public Student studentdetails { get; set; }
        public Package packagedetails { get; set; }
        public studRegister studsubjdetails { get; set; }

        public Subject subjectlist { get; set; }
    }
}