using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace vnpost_ocr_system.Controllers.InvitationCard
{
    public class InputInformationController : Controller
    {
        // GET: InputInformation
        public ActionResult Index()
        {
            return View("/Views/InvitationCard/InputInformation.cshtml");
        }
    }
}