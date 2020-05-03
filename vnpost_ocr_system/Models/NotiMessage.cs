using System;

namespace vnpost_ocr_system.Models
{
    public class NotiMessage
    {
        public long orderID { get; set; }
        public string Title { get; set; }
        public string ContentText { get; set; }
        private string _sendate;
        public string SentDate
        {
            //Apr 15 2020  4:25AM
            get
            {
                if (_sendate == "") return "";
                else return DateTime.Parse(_sendate).ToString("dd/MM/yyyy HH:mm:ss");
            }

            set { _sendate = value; }
        }
    }
}