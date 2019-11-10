using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace vnpost_ocr_system.Controllers.UserRegister
{
    public class UserRegisterController : Controller
    {
        // GET: UserRegister
        [Route("nguoi-dung/dang-ki")]
        public ActionResult Index()
        {
            return View("/Views/UserRegister/UserRegister.cshtml");
        }
    }
}