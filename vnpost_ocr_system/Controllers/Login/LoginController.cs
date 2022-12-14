using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using vnpost_ocr_system.Models;
using XCrypt;

namespace vnpost_ocr_system.Controllers.Login
{
    public class LoginController : Controller
    {
        private VNPOST_AppointmentEntities db = new VNPOST_AppointmentEntities();
        [Route("dang-nhap")]
        public ActionResult Index()
        {
            ViewBag.notifi = "";
            if (Session["useradminID"] != null) return Redirect("/ho-so/thong-ke-tong-quat");
            return View("/Views/Login/Login.cshtml");
        }
        public ActionResult Login(string username, string password)
        {
            try
            {
                var admin = db.Admins.Where(x => x.AdminUsername.Equals(username)).FirstOrDefault();
                if (admin != null)
                {
                    string pass_t = password;
                    password = string.Concat(password, admin.AdminPasswordSalt.Substring(0, 6));
                    string passXc = new XCryptEngine(XCryptEngine.AlgorithmType.MD5).Encrypt(password, "pd");
                    if (passXc.Equals(admin.AdminPasswordHash))
                    {

                        if (admin.IsActive == false)
                        {
                            ViewBag.notifi = "Tài khoản không được phép hoạt động";
                            return View("/Views/Login/Login.cshtml");
                        }
                        Session["useradminID"] = admin.AdminID;
                        Session["useradminName"] = admin.AdminName;
                        Session["adminRole"] = admin.Role;
                        Session["adminPro"] = admin.PostalProvinceCode;
                        Session["Role"] = admin.Role.ToString();
                        Session["url"] = "/ho-so/thong-ke-tong-quat";
                        //if (!String.IsNullOrEmpty(checkbox))
                        //{
                        //    if (checkbox.Equals("on"))
                        //    {
                        //        HttpCookie remme = new HttpCookie("remmemadmin");
                        //        remme["user"] = admin.AdminUsername;
                        //        remme["pass"] = pass_t;
                        //        remme.Expires = DateTime.Now.AddDays(365);
                        //        HttpContext.Response.Cookies.Add(remme);
                        //    }
                        //}
                        return Redirect("/ho-so/thong-ke-tong-quat");
                    }
                    else
                    {
                        ViewBag.notifi = "Mật khẩu không chính xác";
                        return View("/Views/Login/Login.cshtml");
                    }
                }
                else
                {
                    ViewBag.notifi = "Tên tài khoản không tồn tại";
                    return View("/Views/Login/Login.cshtml");
                }
            }
            catch (Exception e)
            {
                return View("/Views/Login/Login.cshtml");
            }
        }
        public ActionResult LogoutAdmin()
        {
            Session.Abandon();
            return RedirectToAction("Index");
        }
    }
}