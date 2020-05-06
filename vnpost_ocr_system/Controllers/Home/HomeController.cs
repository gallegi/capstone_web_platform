using System.Web.Mvc;
using vnpost_ocr_system.Controllers.CustomController;

namespace vnpost_ocr_system.Controllers.Home
{
    public class HomeController : BaseUserController
    {
        // GET: Home

        public ActionResult Index()
        {
            ViewBag.video = "https://www.youtube.com/embed/OHR3gC3fsns";
            if (Request.Browser.IsMobileDevice)
            {
                return View("/Views/MobileView/Home.cshtml");
            }
            return View("/Views/Home/Home.cshtml");
        }
    }
}