//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace vnpost_ocr_system.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class PhoneVerification
    {
        public long PhoneVerificationID { get; set; }
        public string Phone { get; set; }
        public string VerificationCode { get; set; }
        public bool Status { get; set; }
        public System.DateTime CreatedTime { get; set; }
    }
}
