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
    
    public partial class Query_Scope_1_Result
    {
        public string PostalDistrictCode { get; set; }
        public string PostalDistrictName { get; set; }
        public Nullable<double> DistrictDistance { get; set; }
        public long PublicAdministrationLocationID { get; set; }
        public string PublicAdministrationName { get; set; }
        public Nullable<double> PublicAdministractionDistance { get; set; }
        public long ProfileID { get; set; }
        public string ProfileName { get; set; }
        public Nullable<double> ProfileDistance { get; set; }
        public Nullable<double> TotalDistance { get; set; }
    }
}
