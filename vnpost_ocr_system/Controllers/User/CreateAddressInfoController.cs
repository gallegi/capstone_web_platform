using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace vnpost_ocr_system.Controllers.User
{
    public class CreateAddressInfoController : Controller
    {
        // GET: CreateAddressInfo
        [Route("tai-khoan/tao-dia-chi-moi")]
        public ActionResult Index()
        {
            return View("/Views/User/CreateAddressInfo.cshtml");
        }
    }
}