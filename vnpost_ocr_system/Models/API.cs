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
    
    public partial class API
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public API()
        {
            this.APIInputParams = new HashSet<APIInputParam>();
            this.APIOutputParams = new HashSet<APIOutputParam>();
        }
    
        public int APIID { get; set; }
        public int APIMethodID { get; set; }
        public string APIUri { get; set; }
        public string APIDescription { get; set; }
        public System.DateTime LastMofifiedTime { get; set; }
        public string SampleRespone { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<APIInputParam> APIInputParams { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<APIOutputParam> APIOutputParams { get; set; }
        public virtual APIMethod APIMethod { get; set; }
    }
}
