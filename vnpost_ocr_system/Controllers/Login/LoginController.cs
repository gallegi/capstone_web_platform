using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace vnpost_ocr_system.Controllers.Login
{
    public class LoginController : Controller
    {
        // GET: Login
        [Route("dang-nhap")]
        public ActionResult Index()
        {
            return View("/Views/Login/Login.cshtml");
        }
    }
}