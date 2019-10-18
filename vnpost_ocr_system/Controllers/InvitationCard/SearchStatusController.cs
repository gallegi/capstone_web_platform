using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace vnpost_ocr_system.Controllers.InvitationCard
{
    public class SearchStatusController : Controller
    {
        // GET: SearchStatus
        public ActionResult Index()
        {
            return View("/Views/InvitationCard/SearchStatus.cshtml");
        }
    }
}