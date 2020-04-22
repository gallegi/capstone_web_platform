using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace vnpost_ocr_system.Models
{
    public class OCRRaw
    {
        public int form_id { get; set; }
        public string raw_text { get; set; }
        public string[] province { get; set; }
        public string[] district { get; set; }
        public string[] public_administration { get; set; }
        public string[] profile { get; set; }
        public string[] appointment_letter_code { get; set; }
        public string[] name { get; set; }
        public string[] phone_number { get; set; }
        public string[] street { get; set; }
        public string[] personal_paper_type { get; set; }
        public string[] personal_paper_number { get; set; }
        public string[] issued_date { get; set; }
        public string[] issued_place { get; set; }
    }
}