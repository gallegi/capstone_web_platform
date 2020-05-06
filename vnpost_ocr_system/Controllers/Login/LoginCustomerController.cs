using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using vnpost_ocr_system.Controllers.CustomController;
using vnpost_ocr_system.Models;
using vnpost_ocr_system.SupportClass;
using XCrypt;

namespace vnpost_ocr_system.Controllers.Login
{
    public class LoginCustomerController : BaseUserController
    {
        private VNPOST_AppointmentEntities db = new VNPOST_AppointmentEntities();
        // GET: LoginCustomer
        [Route("khach-hang/dang-nhap")]
        public ActionResult Index()
        {
            if (Session["userID"] != null) return Redirect("/");
            ViewBag.invalidcode = "";
            ViewBag.messe = "";
            
            if (Request.Browser.IsMobileDevice)
            {
                return View("/Views/MobileView/Login.cshtml");
            }
            else
            {
                return View("/Views/Login/Login_Customer.cshtml");
            }

        }
        public ActionResult Login(string user, string pass, string checkbox)
        {
            var custom = db.Customers.Where(x => x.Email.Equals(user) || x.Phone.Equals(user)).FirstOrDefault();
            bool check = true;
            if (custom != null)
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
                    if (check == false)
                        if (!custom.Email.Equals(user))
                        {
                            check = false;
                        }
                        else
                        {
                            check = true;
                        }
                }
                if (check == false) return Json(1, JsonRequestBehavior.AllowGet);
                string pass_temp = pass;
                pass = string.Concat(pass, custom.PasswordSalt.Substring(0, 6));
                string passXc = new XCryptEngine(XCryptEngine.AlgorithmType.MD5).Encrypt(pass, "pd");
                if (passXc.Equals(custom.PasswordHash))
                {
                    Session["userID"] = custom.CustomerID;
                    Session["userName"] = custom.FullName;
                    Session["Role"] = "0";
                    Session["url"] = "/";
                    if (!String.IsNullOrEmpty(checkbox))
                    {
                        if (checkbox.Equals("True") || Session["DeviceToken"] != null)
                        {
                            // Generate unique token
                            string AuthToken = Convert.ToBase64String(Guid.NewGuid().ToByteArray());
                            // Save Token to DB
                            AuthenticationToken au = new AuthenticationToken();
                            au.CustomerID = custom.CustomerID;
                            au.AuthToken = AuthToken;
                            au.Status = true;
                            au.CreatedTime = DateTime.Now;
                            au.ExpireDate = DateTime.Now.AddHours(12);
                            if (Session["DeviceToken"] != null)
                                au.FirebaseToken = Session["DeviceToken"].ToString();
                            db.AuthenticationTokens.Add(au);
                            db.SaveChanges();
                            // Save Token to cookie
                            HttpCookie AuthCookie = new HttpCookie("VNPostORCAuthToken");
                            AuthCookie.Value = AuthToken;
                            // Set the cookie expiration date.
                            AuthCookie.Expires = DateTime.Now.AddYears(1);
                            // Add the cookie.
                            Response.Cookies.Add(AuthCookie);
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
        public ActionResult DangKi(string tbName, string tbPhone, string tbValidCodePhone, string tbValidCodeEmail, string tbEmail, string tbPass, string distrint, string tbRePass, string group1)
        {
            try
            {
                if (!string.IsNullOrEmpty(tbPhone))
                {
                    if (!tbValidCodePhone.Equals("123456"))
                    {
                        ViewBag.invalidcode = "Mã xác thực điện thoại không chính xác";
                        if (Request.Browser.IsMobileDevice)
                        {
                            return View("/Views/MobileView/Login.cshtml");
                        }
                        else
                        {
                            return View("/Views/Login/Login_Customer.cshtml");
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
                            return View("/Views/Login/Login_Customer.cshtml");
                        }
                    }
                }
                var email_token = db.EmailVerifications.Where(x => x.Email.Equals(tbEmail) && x.VerificationCode.Equals(tbValidCodeEmail) && x.Status == false).ToList().LastOrDefault();
                if (!string.IsNullOrEmpty(tbEmail))
                {
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
                            return View("/Views/Login/Login_Customer.cshtml");
                        }
                    }
                    if (email_token == null)
                    {
                        ViewBag.invalidcode1 = "Mã xác thực email không chính xác";
                        if (Request.Browser.IsMobileDevice)
                        {
                            return View("/Views/MobileView/Login.cshtml");
                        }
                        else
                        {
                            return View("/Views/Login/Login_Customer.cshtml");
                        }
                    } else if (DateTime.Now.Subtract(email_token.CreatedTime).TotalMinutes > 15)
                    {
                        ViewBag.invalidcode1 = "Mã xác thực email đã hết hạn";
                        if (Request.Browser.IsMobileDevice)
                        {
                            return View("/Views/MobileView/Login.cshtml");
                        }
                        else
                        {
                            return View("/Views/Login/Login_Customer.cshtml");
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
                c.DOB = null;
                c.PostalDistrictID = distrint;
                db.Customers.Add(c);

                //Update verification code status to true
                email_token.Status = true;
                db.Entry(email_token).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                ViewBag.notifi = "Tạo tài khoản thành công";
                Session["userID"] = c.CustomerID;
                Session["userName"] = c.FullName;
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
                    return View("/Views/Login/Login_Customer.cshtml");
                }
            }
        }
        [Route("Logout")]
        public ActionResult Logout()
        {
            Session.Abandon();
            HttpCookie AuthCookie = Request.Cookies["VNPostORCAuthToken"];
            if (AuthCookie != null)
            {
                var AuthToken = db.AuthenticationTokens.Where(x => x.AuthToken.Equals(AuthCookie.Value) && x.Status.Equals(true)).FirstOrDefault();
                if (AuthToken != null)
                {
                    AuthToken.Status = false;
                    db.Entry(AuthToken).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                }
                // delete cookies
                AuthCookie.Expires = DateTime.Now.AddDays(-1d);
                Response.Cookies.Add(AuthCookie);
            }
            return Redirect("/");
        }

        public ActionResult EmailVerification(string email)
        {
            try
            {
                string email_norm = email.Trim().ToLower();
                //Check if email exist => Return error mail exist
                var user = db.Customers.Where(x => x.Email.Equals(email_norm)).FirstOrDefault();
                if (user == null)
                {//Email not exist

                    //Check if token exist for that email and not expired
                    int token;
                    var email_token = db.EmailVerifications.Where(x => x.Email.Equals(email_norm) && x.Status == false).ToList().LastOrDefault();
                    if (email_token != null && DateTime.Now.Subtract(email_token.CreatedTime).TotalMinutes <= 15)
                    {//Use old token if token not expired
                        token = Convert.ToInt32(email_token.VerificationCode);
                    }
                    else
                    {//Generate new token
                        Random r = new Random();
                        token = r.Next(100000, 999999);
                        EmailVerification ev = new EmailVerification();
                        ev.Email = email_norm;
                        ev.VerificationCode = token.ToString();
                        ev.Status = false;
                        ev.CreatedTime = DateTime.Now;
                        db.EmailVerifications.Add(ev);
                        db.SaveChanges();
                    }

                    MailService.sendMail(
                        email_norm,
                        "[VNPost] Mã xác thực email của bạn",
                        "Mã xác thực email của bạn là: " + token
                        ); ;
                }
                else
                {//Email already exist
                   return  Json(new { error = true, message = "Địa chỉ email đã tồn tại" });
                }

            } catch (Exception e)
            {//Unexpected error
                Console.WriteLine(e.StackTrace);
                return Json(new { error = true, message = "Có lỗi xảy ra" });
            }
            //Return email success
            return Json(new { error = false, message = "Mã xác nhận đã được gửi đến email của bạn"});
        }

        public ActionResult ResetPassword(string emailORphone)
        {
            string[] absolutepath = Request.Url.ToString().Split('/');
            string path = absolutepath[0] + "//" + absolutepath[2];
            var user = db.Customers.Where(x => x.Email.Equals(emailORphone) || x.Phone.Equals(emailORphone)).FirstOrDefault();
            if (user != null)
            {
                try
                {
                    int token;
                    var custom_token = db.ResetPasswordTokens.Where(x => x.CustomerID == user.CustomerID && x.Status == false).ToList().LastOrDefault();
                    if (custom_token != null && DateTime.Now.Subtract(custom_token.CreatedDate).TotalMinutes <= 15)
                    {
                        token = Convert.ToInt32(custom_token.Token);
                    }
                    else
                    {
                        Random r = new Random();
                        token = r.Next(100000, 999999);
                        ResetPasswordToken re = new ResetPasswordToken();
                        re.CustomerID = user.CustomerID;
                        re.Token = token.ToString();
                        re.Status = false;
                        re.CreatedDate = DateTime.Now;
                        db.ResetPasswordTokens.Add(re);
                        db.SaveChanges();
                    }
                    MailService.sendMail(
                        emailORphone, 
                        "Thay đổi mật khẩu", 
                        "Quý khách vui lòng không cung cấp mã cho người khác.</br> Mã Token của bạn là: " + token + ".</br> Hoặc bạn có thể click vào link sau để thay đổi mật khẩu của mình " + HttpContext.Request.Url.GetLeftPart(UriPartial.Authority)  + "/khach-hang/thay-doi-mat-khau?userid=" + user.CustomerID + "&token=" + token
                    );
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
            return Json(user.CustomerID, JsonRequestBehavior.AllowGet);
        }

        [Route("khach-hang/thay-doi-mat-khau")]
        public ActionResult PasswordForm(int userid, int token)
        {
            var user = db.ResetPasswordTokens.Where(x => x.Token.Equals(token.ToString()) && x.CustomerID == userid && x.Status == false).ToList().LastOrDefault();
            if (user != null && DateTime.Now.Subtract(user.CreatedDate).TotalMinutes <= 15)
            {
                ViewBag.userid = user.CustomerID;
                ViewBag.token = token;
                return View("/Views/Login/ResetPassword.cshtml");
            }
            return Redirect("/khach-hang/dang-nhap");
        }

        public ActionResult CheckToken(int token, int userid)
        {
            try
            {
                var checktoken = db.ResetPasswordTokens.Where(x => x.Token.Equals(token.ToString()) && x.CustomerID == userid && x.Status == false).ToList().LastOrDefault();
                if (checktoken == null) return Json(0, JsonRequestBehavior.AllowGet);
                if (DateTime.Now.Subtract(checktoken.CreatedDate).TotalMinutes > 15) return Json(-1, JsonRequestBehavior.AllowGet);
                return Json(1, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(-2, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult ChangePass(string password, int userid, int token)
        {
            try
            {
                Int64 id = Convert.ToInt64(userid);
                var user = db.Customers.Where(x => x.CustomerID == id).FirstOrDefault();
                var custom_token = db.ResetPasswordTokens.Where(x => x.CustomerID == user.CustomerID && x.Token.Equals(token.ToString()) && x.Status == false).ToList().LastOrDefault();
                custom_token.Status = true;
                db.Entry(custom_token).State = System.Data.Entity.EntityState.Modified;
                password = string.Concat(password, user.PasswordSalt.Substring(0, 6));
                string passXc = new XCryptEngine(XCryptEngine.AlgorithmType.MD5).Encrypt(password, "pd");
                user.PasswordHash = passXc;
                db.Entry(user).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return Json(1, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(0, JsonRequestBehavior.AllowGet);
            }
        }

    }
    public class login
    {
        public string username { get; set; }
        public string password { get; set; }
    }
}