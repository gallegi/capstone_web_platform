using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace vnpost_ocr_system.Controllers
{
    public class PaymentController : Controller
    {
        // GET: Payment
        [Route("don-hang/thanh-toan")]
        public ActionResult Index()
        {
            return View("/Views/Payment/Payment.cshtml");
        }
    }
}