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
    
    public partial class Payment
    {
        public long PaymentID { get; set; }
        public long OrderID { get; set; }
        public int PaymentMethodID { get; set; }
        public int PaymentStatusID { get; set; }
        public Nullable<System.DateTime> PaymentDate { get; set; }
        public string OtherDetails { get; set; }
    
        public virtual Order Order { get; set; }
        public virtual PaymentMethod PaymentMethod { get; set; }
        public virtual PaymentStatus PaymentStatu { get; set; }
    }
}
