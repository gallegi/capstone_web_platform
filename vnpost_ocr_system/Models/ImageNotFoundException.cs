using System;

namespace vnpost_ocr_system.Models
{
    public class ImageNotFoundException : Exception
    {
        public string msg = "";
        public ImageNotFoundException() { }
        public ImageNotFoundException(string msg) : base(msg)
        {
            this.msg = msg;
        }
        public ImageNotFoundException(string message, Exception inner)
        : base(message, inner)
        {
        }
    }
}