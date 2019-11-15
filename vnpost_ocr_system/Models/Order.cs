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
    
    public partial class Order
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Order()
        {
            this.OrderStatusDetails = new HashSet<OrderStatusDetail>();
            this.Payments = new HashSet<Payment>();
        }
    
        public long OrderID { get; set; }
        public long CustomerID { get; set; }
        public System.DateTime OrderDate { get; set; }
        public Nullable<System.DateTime> ShipDate { get; set; }
        public int ProfileID { get; set; }
        public string AppointmentLetterCode { get; set; }
        public string AppointLetterImageLink { get; set; }
        public string ProcedurerFullName { get; set; }
        public string ProcedurerPhone { get; set; }
        public string ProcedurerPostalDistrictCode { get; set; }
        public string ProcedurerStreet { get; set; }
        public int ProcedurerPersonalPaperTypeID { get; set; }
        public string ProcedurerPersonalPaperNumber { get; set; }
        public System.DateTime ProcedurerPersonalPaperIssuedDate { get; set; }
        public string ProcedurerPersonalPaperIssuedPlace { get; set; }
        public string SenderFullName { get; set; }
        public string SenderPhone { get; set; }
        public string SenderPostalDistrictCode { get; set; }
        public string SenderStreet { get; set; }
        public string ReceiverFullName { get; set; }
        public string ReceiverPhone { get; set; }
        public string ReceiverPostalDistrictCode { get; set; }
        public string ReceiverStreet { get; set; }
        public string OrderNote { get; set; }
        public System.DateTime CreatedTime { get; set; }
        public Nullable<double> Amount { get; set; }
        public string TotalAmountInWords { get; set; }
        public int StatusID { get; set; }
        public string ItemCode { get; set; }
        public Nullable<long> ProcessedBy { get; set; }
        public Nullable<long> ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedTime { get; set; }
    
        public virtual Admin Admin { get; set; }
        public virtual Admin Admin1 { get; set; }
        public virtual Customer Customer { get; set; }
        public virtual District District { get; set; }
        public virtual District District1 { get; set; }
        public virtual District District2 { get; set; }
        public virtual PersonalPaperType PersonalPaperType { get; set; }
        public virtual Profile Profile { get; set; }
        public virtual Status Status { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OrderStatusDetail> OrderStatusDetails { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Payment> Payments { get; set; }
    }
}
