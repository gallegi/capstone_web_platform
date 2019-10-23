using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace vnpost_ocr_system.Controllers.Document
{
    public class DisplayStatisticChartController : Controller
    {
        // GET: DisplayStatisticChart
        [Route("ho-so/thong-ke-tong-quat")]
        public ActionResult Index()
        {
            return View("/Views/Document/DisplayStatisticChart.cshtml");
        }
    }
}