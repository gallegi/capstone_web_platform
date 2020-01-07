using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace vnpost_ocr_system.Models
{
    public class MyOrderDetail : OrderStatusDetail
    {
        public int mi { get; set; }
        public int h { get; set; }
        public int y { get; set; }
        public int m { get; set; }
        public int d { get; set; }
        public string hour { get; set; }
        public string display { get; set; }
        public string StatusName { get; set; }
        public string PosName { get; set; }
    }
}