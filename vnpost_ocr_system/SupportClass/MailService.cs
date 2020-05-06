using System.Net.Mail;

namespace vnpost_ocr_system.SupportClass
{
    public static class MailService
    {
        static SmtpClient smtp;
        static MailService()
        {
            smtp = new SmtpClient();
            smtp.Host = "smtp.mailgun.org";
            smtp.Credentials = new System.Net.NetworkCredential("postmaster@mail.vnpost.tech", "b59cfe65aea2a1bbd9335115e1e14662-0afbfc6c-b6903395");
            smtp.Port = 587;
            smtp.EnableSsl = true;
        }

        public static void sendMail(string to, string subject, string body)
        {
            MailMessage mail = new MailMessage();
            mail.To.Add(to);
            mail.From = new MailAddress("support@vnpost.tech", "VNPost");
            mail.Subject = subject;
            mail.IsBodyHtml = true;
            mail.Body = body;

            smtp.Send(mail);
        }
    }
}