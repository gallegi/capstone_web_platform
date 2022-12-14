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
    
    public partial class PublicAdministration
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public PublicAdministration()
        {
            this.FormTemplates = new HashSet<FormTemplate>();
            this.Profiles = new HashSet<Profile>();
        }
    
        public long PublicAdministrationLocationID { get; set; }
        public string PublicAdministrationName { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public long PosCode { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<FormTemplate> FormTemplates { get; set; }
        public virtual PostOffice PostOffice { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Profile> Profiles { get; set; }
    }
}
