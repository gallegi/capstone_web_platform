using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace vnpost_ocr_system.Controllers.InvitationCard
{
    public class SearchStatusController : Controller
    {
        [Route("giay-hen/tim-giay-hen")]
        [HttpGet]
        public ActionResult Index()
        {
            return View("/Views/InvitationCard/SearchStatus.cshtml");
        }

        [Route("giay-hen/tim-giay-hen")]
        [HttpPost]
        public void Search(string id)
        {
            DisplayStatusController ds = new DisplayStatusController();
            ds.Display(id);
        }
    }
}