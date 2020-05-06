using System.Linq;
using System.Web.Mvc;
using vnpost_ocr_system.Models;
using vnpost_ocr_system.SupportClass;

namespace vnpost_ocr_system.Form
{
    public class ListFormController : Controller
    {

        private VNPOST_AppointmentEntities db = new VNPOST_AppointmentEntities();
        // GET: ListForm
        [Auther(Roles = "1")]
        [Route("bieu-mau/list-bieu-mau")]
        public ActionResult Index()
        {
            var listForms = db.FormTemplates.ToList();
            ViewBag.Forms = listForms;
            return View("/Views/Form/ListFormView.cshtml");
        }
    }
}