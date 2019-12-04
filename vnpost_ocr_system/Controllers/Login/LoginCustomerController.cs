using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using vnpost_ocr_system.Models;
using vnpost_ocr_system.SupportClass;
using XCrypt;
namespace vnpost_ocr_system.Controllers.Login
{
    public class LoginCustomerController : Controller
    {
        private VNPOST_AppointmentEntities db = new VNPOST_AppointmentEntities();
        // GET: LoginCustomer
        [Route("khach-hang/dang-nhap")]
        public ActionResult Index()
        {
            if (Session["userID"] != null) return Redirect("/");
            ViewBag.invalidcode = "";
            ViewBag.messe = "";
            if (HttpContext.Request.Cookies["remmem"] != null)
            {
                HttpCookie remme = HttpContext.Request.Cookies.Get("remmem");
                login a = new login()
                {
                    username = remme.Values.Get("user"),
                    password = remme.Values.Get("pass")
                };
                ViewBag.login = a;
            }
            if (Request.Browser.IsMobileDevice)
            {
                return View("/Views/MobileView/Login.cshtml");
            }
            else
            {
                return View("/Views/Login/Login_Cutomer.cshtml");
            }
        }
        public ActionResult Login(string user,string pass,string checkbox)
        {
            var custom = db.Customers.Where(x => x.Email.Equals(user) || x.Phone.Equals(user)).FirstOrDefault();
            bool check = true;
            if(custom != null)
            {
                if (!string.IsNullOrEmpty(custom.Phone))
                {
                    if (!custom.Phone.Equals(user))
                    {
                        check = false;
                    }
                }
                if (!string.IsNullOrEmpty(custom.Email))
                {
                    if(check == false)
                    if (!custom.Email.Equals(user))
                    {
                        check = false;
                    }
                    else
                    {
                        check = true;
                    }
                }
                if(check == false) return Json(1, JsonRequestBehavior.AllowGet);
                string pass_temp = pass;
                pass = string.Concat(pass, custom.PasswordSalt.Substring(0,6));
                string passXc = new XCryptEngine(XCryptEngine.AlgorithmType.MD5).Encrypt(pass, "pd");
                if (passXc.Equals(custom.PasswordHash))
                {
                    Session["userID"] = custom.CustomerID;
                    Session["userName"] = custom.FullName;
                    Session["Role"] = "0";
                    Session["url"] = "/";
                    if (!String.IsNullOrEmpty(checkbox))
                    {
                        if (checkbox.Equals("True"))
                        {
                            HttpCookie remme = new HttpCookie("remmem");
                            remme["user"] = user;
                            remme["pass"] = pass_temp;
                            remme.Expires = DateTime.Now.AddDays(365);
                            HttpContext.Response.Cookies.Add(remme);
                        }
                    }
                    return Json(3, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(2, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return Json(1, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public ActionResult DangKi(string tbName, string tbPhone, string tbValidCodePhone,string tbValidCodeEmail, string tbEmail, string tbPass,string distrint, string tbRePass, string group1)
        {
            try
            {
                if (!string.IsNullOrEmpty(tbPhone))
                {
                    if (!tbValidCodePhone.Equals("123456"))
                    {
                        ViewBag.invalidcode = "Mã xác thực không đúng";
                        if (Request.Browser.IsMobileDevice)
                        {
                            return View("/Views/MobileView/Login.cshtml");
                        }
                        else
                        {
                            return View("/Views/Login/Login_Cutomer.cshtml");
                        }
                    }
                    var cus = db.Customers.Where(x => x.Phone.Equals(tbPhone)).ToList();
                    if (cus.Count > 0)
                    {
                        ViewBag.messe = "Số điện thoại đã được đăng kí cho tài khoản khác";
                        if (Request.Browser.IsMobileDevice)
                        {
                            return View("/Views/MobileView/Login.cshtml");
                        }
                        else
                        {
                            return View("/Views/Login/Login_Cutomer.cshtml");
                        }
                    }
                }
                if (!string.IsNullOrEmpty(tbEmail))
                {
                    if (!tbValidCodeEmail.Equals("123456"))
                    {
                        ViewBag.invalidcode1 = "Mã xác thực không đúng";
                        if (Request.Browser.IsMobileDevice)
                        {
                            return View("/Views/MobileView/Login.cshtml");
                        }
                        else
                        {
                            return View("/Views/Login/Login_Cutomer.cshtml");
                        }
                    }
                    var cus = db.Customers.Where(x => x.Email.Equals(tbEmail)).ToList();
                    if (cus.Count > 0)
                    {
                        ViewBag.messe = "Địa chỉ email đã được đăng kí cho tài khoản khác";
                        if (Request.Browser.IsMobileDevice)
                        {
                            return View("/Views/MobileView/Login.cshtml");
                        }
                        else
                        {
                            return View("/Views/Login/Login_Cutomer.cshtml");
                        }
                    }
                }
                Random r = new Random();
                int salt = r.Next(100000, 999999);
                tbPass = string.Concat(tbPass, salt);
                string passXc = new XCryptEngine(XCryptEngine.AlgorithmType.MD5).Encrypt(tbPass, "pd");
                Customer c = new Customer();
                c.PasswordHash = passXc;
                c.PasswordSalt = salt.ToString();
                c.FullName = tbName;
                c.Gender = Convert.ToInt32(group1);
                c.Phone = tbPhone;
                c.Email = tbEmail;
                c.DOB = DateTime.Now;
                c.PostalDistrictID = distrint;
                db.Customers.Add(c);
                db.SaveChanges();
                ViewBag.notifi = "Tạo tài khoản thành công";
                var custom = db.Customers.Where(x => x.Email.Equals(tbEmail) || x.Phone.Equals(tbPhone)).FirstOrDefault();
                Session["userID"] = custom.CustomerID;
                Session["userName"] = custom.FullName;
                Session["Role"] = "0";
                Session["url"] = "/";
                return Redirect("/");
            }
            catch (Exception e)
            {
                ViewBag.messe = "Có lỗi xảy ra. Vui lòng thử lại";
                if (Request.Browser.IsMobileDevice)
                {
                    return View("/Views/MobileView/Login.cshtml");
                }
                else
                {
                    return View("/Views/Login/Login_Cutomer.cshtml");
                }
            }
        }
        [Route("Logout")]
        public ActionResult Logout()
        {
            Session.Abandon();
            return RedirectToAction("Index");
        }
        public ActionResult GetPro()
        {
            db.Configuration.ProxyCreationEnabled = false;
            var list = db.Provinces.ToList();
            return Json(list,JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetDis(string id)
        {
            db.Configuration.ProxyCreationEnabled = false;
            var list = db.Districts.Where(x => x.PostalProvinceCode == id).ToList();
            return Json(list, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ResetPassword(string emailORphone)
        {
            var user = db.Customers.Where(x => x.Email.Equals(emailORphone) || x.Phone.Equals(emailORphone)).FirstOrDefault();
            if(user != null)
            {
                try
                {
                    Random r = new Random();
                    int token = r.Next(100000,999999);
                    ResetPasswordToken re = new ResetPasswordToken();
                    re.CustomerID = user.CustomerID;
                    re.Token = token.ToString();
                    re.Status = true;
                    re.CreatedDate = DateTime.Now;
                    db.ResetPasswordTokens.Add(re);
                    db.SaveChanges();
                    MailMessage mail = new MailMessage();
                    mail.To.Add(emailORphone);
                    mail.From = new MailAddress("shidoundie@gmail.com");
                    mail.Subject = "Thay đổi mật khẩu";
                    mail.Body = "Click vào đường dẫn này để thay đổi mật khẩu: http://localhost:50796/khach-hang/thay-doi-mat-khau?customerid="+user.CustomerID+"&token="+ token;
                    mail.IsBodyHtml = true;
                    SmtpClient smtp = new SmtpClient();
                    smtp.Host = "smtp.gmail.com"; //Or Your SMTP Server Address
                    smtp.Credentials = new System.Net.NetworkCredential("shidoundie@gmail.com", "********");
                    smtp.Port = 587;
                    smtp.EnableSsl = true;
                    smtp.Send(mail);
                }
                catch (Exception ex)
                {
                    return Json(-1, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return Json(0, JsonRequestBehavior.AllowGet);
            }
            return Json(1, JsonRequestBehavior.AllowGet);
        }
        [Route("khach-hang/thay-doi-mat-khau")]
        public ActionResult PasswordForm(string customerid,int token)
        {
            var user = db.ResetPasswordTokens.Where(x => x.CustomerID.Equals(customerid) && x.Token.Equals(token.ToString()) && x.CreatedDate == DateTime.Now.Date).LastOrDefault();
            if(user != null)
            {
                return View("/Views/Login/Success.cshtml");
            }
            return View("/Views/Login/Success.cshtml");
        }
    }
    public class login
    {
        public string username { get; set; }
        public string password { get; set; }
    }
}