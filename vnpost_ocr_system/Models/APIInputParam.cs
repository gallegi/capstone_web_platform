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
    
    public partial class APIInputParam
    {
        public int APIID { get; set; }
        public string APIInputParamName { get; set; }
        public string APIInputParamType { get; set; }
        public string APIInputParamDescription { get; set; }
        public System.DateTime LastMofifiedTime { get; set; }
    
        public virtual API API { get; set; }
    }
}
