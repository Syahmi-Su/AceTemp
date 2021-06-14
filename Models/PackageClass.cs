using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace AceTC.Models
{
    public class PackageClass
    {
        
        public int package_id { get; set; }
        [Required]
        public string package_desc { get; set; }
        [Required]
        public string package_category { get; set; }
        [Required]
        public float package_price { get; set; }
    }
}