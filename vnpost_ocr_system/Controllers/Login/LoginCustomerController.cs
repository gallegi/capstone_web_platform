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
            ViewBag.invalidcode = "";
            return View("/Views/Login/Login_Cutomer.cshtml");
        }
        [HttpPost]
        public ActionResult DangKi(string tbName, string tbPhone, string tbValidCodePhone,string tbValidCodeEmail, string tbEmail, string tbPass, string tbRePass, string group1)
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
                    if(cus.Count > 0)
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
                ViewBag.messe = "Có lỗi xảy ra. Vui lòng thử lại";
                return View("/Views/Login/Login_Cutomer.cshtml");
            }
        }
    }
}