namespace vnpost_ocr_system.Models
{
    public class ContactInfoDB : ContactInfo
    {
        public string PersonalPaperTypeName { get; set; }
        public string PersonalPaperIssuedDateString { get; set; }
        public string PostalDistrictName { get; set; }
        public string PostalProvinceCode { get; set; }
        public string PostalProvinceName { get; set; }
    }
}