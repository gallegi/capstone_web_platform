using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace vnpost_ocr_system.Controllers.User
{
    public class AccountInfoController : Controller
    {
        // GET: AccountInfo
        [Route("tai-khoan/thong-tin-tai-khoan")]
        public ActionResult Index()
        {
            return View("/Views/User/AccountInfo.cshtml");
        }
    }
}