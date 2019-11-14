using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using vnpost_ocr_system.Models;
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
            if (Request.Browser.IsMobileDevice)
            {
                return View("/Views/MobileView/Login.cshtml");
            }else{
            }
            ViewBag.invalidcode = "";
            return View("/Views/Login/Login_Cutomer.cshtml");
        }
        [HttpPost]
        public ActionResult DangKi(string tbName, string tbPhone, string tbValidCode, string tbEmail, string tbPass, string tbRePass, string group1)
        {
            if (!tbValidCode.Equals("123456"))
            {
                ViewBag.invalidcode = "Mã xác thực không đúng";
                return View("/Views/Login/Login_Cutomer.cshtml");
            }
            try
            {
                string passXc = new XCryptEngine(XCryptEngine.AlgorithmType.MD5).Encrypt(tbPass, "pl");
                Random r = new Random();
                int salt = r.Next(100000, 999999);
                Customer c = new Customer();
                c.PasswordHash = passXc;
                c.PasswordSalt = salt.ToString();
                c.FullName = tbName;
                c.Gender = Convert.ToInt32(group1);
                c.Phone = tbPhone;
                c.Email = tbEmail;
                c.DOB = DateTime.Now;
                db.Customers.Add(c);
                db.SaveChanges();
                ViewBag.notifi = "Tạo tài khoản thành công";
                return Redirect("/tai-khoan/thong-tin-tai-khoan");
            }
            catch (Exception e)
            {
                ViewBag.notifi = "Có lỗi xảy ra. Vui lòng thử lại";
                return View("/Views/Login/Login_Cutomer.cshtml");
            }
        }
    }
}