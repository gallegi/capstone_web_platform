using System.Collections.Generic;

namespace vnpost_ocr_system.Models
{
    public class OrderByDay
    {
        public int y { get; set; }
        public int m { get; set; }
        public int d { get; set; }
        public int StatusID { get; set; }
        public List<MyOrderDetail> listOrder { get; set; }
        public string dayOfWeek { get; set; }

    }
}