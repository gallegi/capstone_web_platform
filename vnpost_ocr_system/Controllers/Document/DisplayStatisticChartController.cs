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
        [Route("thong-ke/cho-tiep-nhan")]
        public ActionResult Index()
        {
            return View("/Views/Document/DisplayStatisticChart.cshtml");
        }
    }
}