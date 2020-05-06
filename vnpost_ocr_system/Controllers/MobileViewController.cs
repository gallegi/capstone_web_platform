using System.Web.Mvc;

namespace vnpost_ocr_system.Controllers
{
    public class MobileViewController : Controller
    {
        [Route("demo/mobile")]
        public ActionResult Index()
        {
            return View("/Views/MobileView/MobileView.cshtml");
        }
    }
}