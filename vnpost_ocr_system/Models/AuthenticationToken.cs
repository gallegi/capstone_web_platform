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
    
    public partial class AuthenticationToken
    {
        public long TokenID { get; set; }
        public long CustomerID { get; set; }
        public string Token { get; set; }
        public string FirebaseToken { get; set; }
        public bool Status { get; set; }
        public System.DateTime CreateDate { get; set; }
        public System.DateTime ExpireDate { get; set; }
    
        public virtual Customer Customer { get; set; }
    }
}
