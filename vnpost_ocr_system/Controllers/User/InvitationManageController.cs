using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace vnpost_ocr_system.Controllers.User
{
    public class InvitationManageController : Controller
    {
        // GET: InvitationManage
        [Route("tai-khoan/quan-ly-giay-hen")]
        public ActionResult Index()
        {
            return View("/Views/User/InvitationManage.cshtml");
        }
    }
}