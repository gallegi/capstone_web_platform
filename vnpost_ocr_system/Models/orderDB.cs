namespace vnpost_ocr_system.Models
{
    public class orderDB : Order
    {
        public int active { get; set; }
        public string PersonalPaperTypeName { get; set; }
        public string NgayCap { get; set; }
        public string PublicAdministrationName { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string ProfileName { get; set; }
        public string PosName { get; set; }
        public string Address_BC { get; set; }
        public string Phone_BC { get; set; }
        public string displayAmount { get; set; }
        public string StatusName { get; set; }
        public int step { get; set; }
    }
}