using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace vnpost_ocr_system.Models
{
    public class ContactInfoDB : ContactInfo
    {
        public string PersonalPaperTypeName { get; set; }
        public string PersonalPaperIssuedDateString { get; set; }
        public string PostalProvinceCode { get; set; }
    }
}