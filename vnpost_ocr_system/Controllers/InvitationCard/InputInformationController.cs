using System.Web.Mvc;

namespace vnpost_ocr_system.Controllers.InvitationCard
{
    public class InputInformationController : Controller
    {
        // GET: InputInformation
        [Route("giay-hen/nhap-giay-hen")]
        public ActionResult Index()
        {
            return View("/Views/InvitationCard/InputInformation.cshtml");
        }
    }
}