using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace vnpost_ocr_system.Controllers.User
{
    public class AddressInfoController : Controller
    {
        // GET: AddressInfo
        [Route("tai-khoan/so-dia-chi")]
        public ActionResult Index()
        {
            return View("/Views/User/AddressInfo.cshtml");
        }
    }
}