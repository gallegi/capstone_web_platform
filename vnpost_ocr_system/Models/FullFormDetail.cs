using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace vnpost_ocr_system.Models
{
    public class FullFormDetail
    {
        public FullFormDetail() { }
        public FullFormDetail(FormTemplate ft, string image)
        {
            this.ft = ft;
            this.image = image;
        }

        public string image { get; set; }
        public FormTemplate ft { get; set; }
    }
}