//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace AceTC.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;

    public partial class Subject
    {
        [Required(ErrorMessage = "Subject Code Required")]
        public string subject_code { get; set; }

        [Required(ErrorMessage = "Subject Name Required")]
        public string subject_name { get; set; }

        [Required(ErrorMessage = "Subject Type Required")]
        public string subject_type { get; set; }
    }
}
