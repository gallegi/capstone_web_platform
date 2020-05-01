using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace vnpost_ocr_system.Models
{
    public class ReceivedDocument : Order
    {
        public string PostalProvinceName { get; set; }
        public string ProfileName { get; set; }
        public string PublicAdministrationName { get; set; }
        public string Phone { get; set; }
    }
}