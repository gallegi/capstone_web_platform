namespace vnpost_ocr_system.Models
{
    public class ErrorObject
    {
        public string status_code;
        public string status;
        public string msg;
        public ErrorObject(string status_code, string status, string msg)
        {
            this.status_code = status_code;
            this.status = status;
            this.msg = msg;
        }
    }
}