using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace vnpost_ocr_system.Models
{
    public class NotReceivedDocument : Order
    {
        public string PostalProvinceName { get; set; }
        public string ProfileName { get; set; }
        public string PublicAdministrationName { get; set; }
        public string Phone { get; set; }
    }
    public class NotReceivedDocumentDetail : NotReceivedDocument
    {
        public string PAPhone { get; set; }
        public string PAAddress { get; set; }
        public string POPhone { get; set; }
        public string POAddress { get; set; }
        public string PosName { get; set; }
        public string PersonalPaperTypeName { get; set; }
        public string ImageName { get; set; }
        public string ImageRealName { get; set; }
    }
}