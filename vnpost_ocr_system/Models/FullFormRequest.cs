using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace vnpost_ocr_system.Models
{
    public class FullFormRequest
    {
        public FullFormRequest() { }
        public FullFormRequest(FormTemplate ft)
        {
            this.ft = ft;
        }
        public FullFormRequest(FormTemplate ft, string action)
        {
            this.action = action;
            this.ft = ft;
        }
        public string action { get; set; }
        public FormTemplate ft { get; set; }
    }
}