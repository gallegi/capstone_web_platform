using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using vnpost_ocr_system.Models;
namespace vnpost_ocr_system.Controllers.Document
{
    public class DocumentReceivedDetailController : Controller
    {
        // GET: DocumentReceivedDetail
        [Route("ho-so/ho-so-da-nhan/chi-tiet")]
        public ActionResult Index()
        {
            String query = "select * from [Order] where OrderID=1";
            VNPOST_AppointmentEntities db = new VNPOST_AppointmentEntities();
            Order order = db.Orders.Find(1);
            ViewBag.statusID = order.StatusID;
            ViewBag.orderID = order.OrderID;
            ViewBag.order = order;
            return View("/Views/Document/DocumentReceivedDetail.cshtml");
        }
    }
}