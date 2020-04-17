using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace vnpost_ocr_system.Models
{
    public class OCRParsed
    {
        public string PostalProvinceCode { get; set; }
        public string PostalDistrictCode { get; set; }
        public long? PublicAdministrationLocationID { get; set; }
        public long? ProfileID { get; set; }
        public string AppointmentLetterCode { get; set; }
        public string ProcedurerFullName { get; set; }
        public string ProcedurerPhone { get; set; }
        public string ProcerdurerProvince { get; set; }
        public string ProcedurerDistrict { get; set; }
        public string ProcedurerStreet { get; set; }
        public int ProcedurerPersonalPaperType { get; set; }
        public string ProcedurerPersonalPaperNumber { get; set; }
        public string ProcedurerPersonalPaperIssuedDate { get; set; }
        public string ProcedurerPersonalPaperIssuedPlace { get; set; }
        public string SenderFullName { get; set; }
        public string SenderPhone { get; set; }
        public string SenderrProvince { get; set; }
        public string SenderDistrict { get; set; }
        public string SenderStreet { get; set; }
        public string ReceiverFullName { get; set; }
        public string ReceiverPhone { get; set; }
        public string ReceiverrProvince { get; set; }
        public string ReceiverDistrict { get; set; }
        public string ReceiverStreet { get; set; }
    }
}