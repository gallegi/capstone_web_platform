using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using XCrypt;
using vnpost_ocr_system.Models;
namespace vnpost_ocr_system.Controllers.Login
{
    public class LoginController : Controller
    {
        private VNPOST_AppointmentEntities db = new VNPOST_AppointmentEntities();
        [Route("dang-nhap")]
        public ActionResult Index()
        {
            ViewBag.notifi = "";
            if(Session["userID"] != null) return Redirect("/phan-quyen-tai-khoan");
            if (HttpContext.Request.Cookies["remmemadmin"] != null)
            {
                HttpCookie remme = HttpContext.Request.Cookies.Get("remmemadmin");
                login a = new login()
                {
                    username = remme.Values.Get("user"),
                    password = remme.Values.Get("pass")
                };
                ViewBag.login = a;
            }
            return View("/Views/Login/Login.cshtml");
        }
        public ActionResult Login(string username, string password, string checkbox)
        {
            try
            {
                string passXc = new XCryptEngine(XCryptEngine.AlgorithmType.MD5).Encrypt(password, "pl");
                var admin = db.Admins.Where(x => x.AdminUsername.Equals(username)).FirstOrDefault();
                if (admin != null)
                {
                    string pass = string.Concat(admin.AdminPasswordHash, admin.AdminPasswordSalt);
                    passXc = string.Concat(passXc, admin.AdminPasswordSalt);
                    if (passXc.Equals(pass))
                    {
                        Session["userID"] = admin.AdminID;
                        Session["userName"] = admin.AdminName;
                        if (!String.IsNullOrEmpty(checkbox))
                        {
                            if (checkbox.Equals("on"))
                            {
                                HttpCookie remme = new HttpCookie("remmemadmin");
                                remme["user"] = admin.AdminUsername;
                                remme["pass"] = password;
                                remme.Expires = DateTime.Now.AddDays(365);
                                HttpContext.Response.Cookies.Add(remme);
                            }
                        }
                        return Redirect("/phan-quyen-tai-khoan");
                    }
                    else
                    {
                        ViewBag.notifi = "Sai mật khẩu";
                        return View("/Views/Login/Login.cshtml");
                    }
                }
                else
                {
                    ViewBag.notifi = "Tên tài khoản không tồn tại";
                    return View("/Views/Login/Login.cshtml");
                }
            }
            catch(Exception e)
            {
                return View("/Views/Login/Login.cshtml");
            }
        }
    }
}