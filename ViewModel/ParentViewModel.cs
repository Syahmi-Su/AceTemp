using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AceTC.ViewModel
{
    public class ParentViewModel
    {
        public int confirmation_id { get; set; }
        public string student_ic { get; set; }
        public string parent_ic { get; set; }
        public double payment_fee { get; set; }
        public string ref_num { get; set; }
        public int status_id { get; set; }
        public System.DateTime confirmation_date { get; set; }
        public System.DateTime payment_date { get; set; }
        public string payment_detail { get; set; }
        public string payment_feedetails { get; set; }
        public HttpPostedFileBase filename { get; set; }

        public int meal_fee { get; set; }

        public int transport_fee { get; set; }

        public int first_register { get; set; }

        public float lower_discount { get; set; }
    }
}