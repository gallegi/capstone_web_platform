using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace vnpost_ocr_system.Controllers.Login
{
    public class ManagementUserController : Controller
    {
        // GET: ManagementUser
        [Route("phan-quyen-tai-khoan")]
        public ActionResult Index()
        {
            return View("/Views/Login/ManagementUser.cshtml");
        }
    }
}