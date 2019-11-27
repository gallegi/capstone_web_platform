using System;
using System.Collections.Generic;
using System.Linq;
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
                            remme["pass"] = pass;
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
                        return View("/Views/Login/Login_Cutomer.cshtml");
                    }
                    var cus = db.Customers.Where(x => x.Phone.Equals(tbPhone)).ToList();
                    if (cus.Count > 0)
                    {
                        ViewBag.messe = "Số điện thoại đã được đăng kí cho tài khoản khác";
                        return View("/Views/Login/Login_Cutomer.cshtml");
                    }
                }
                if (!string.IsNullOrEmpty(tbEmail))
                {
                    if (!tbValidCodeEmail.Equals("123456"))
                    {
                        ViewBag.invalidcode1 = "Mã xác thực không đúng";
                        return View("/Views/Login/Login_Cutomer.cshtml");
                    }
                    var cus = db.Customers.Where(x => x.Email.Equals(tbEmail)).ToList();
                    if (cus.Count > 0)
                    {
                        ViewBag.messe = "Địa chỉ email đã được đăng kí cho tài khoản khác";
                        return View("/Views/Login/Login_Cutomer.cshtml");
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
                return Redirect("/");
            }
            catch (Exception e)
            {
                ViewBag.messe = "Có lỗi xảy ra. Vui lòng thử lại";
                return View("/Views/Login/Login_Cutomer.cshtml");
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
    }
    public class login
    {
        public string username { get; set; }
        public string password { get; set; }
    }
}