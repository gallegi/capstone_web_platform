using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace vnpost_ocr_system.Controllers.Login
{
    public class LoginCustomerController : Controller
    {
        // GET: LoginCustomer
        [Route("khach-hang/dang-nhap")]
        public ActionResult Index()
        {
            if (Request.Browser.IsMobileDevice)
            {
                return View("/Views/MobileView/Login.cshtml");
            }
            return View("/Views/Login/Login_Cutomer.cshtml");
        }
    }
}