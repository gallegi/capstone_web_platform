using System;
using System.Drawing;
using System.Web;
using System.Web.Mvc;
using vnpost_ocr_system.Models;

namespace vnpost_ocr_system.Controllers
{
    public class PaymentController : Controller
    {
        // GET: Payment

        [Route("don-hang/thanh-toan")]
        public ActionResult Index()
        {
            return View("/Views/Payment/Payment.cshtml");
        }

        [HttpPost]
        [Route("don-hang/thanh-toan")]
        public ActionResult Add(HttpPostedFileBase file)
        {
            Image sourceimage = Image.FromStream(Request.Files["img"].InputStream, true, true);
            int ProfileID = int.Parse(Request["ProfileID"].ToString());
            string AppointmentLetterCode = Request["AppointmentLetterCode"].ToString();
            string ProcedurerFullName = Request["ProcedurerFullName"].ToString();
            string ProcedurerPhone = Request["ProcedurerPhone"].ToString();
            string ProcedurerPostalDistrictCode = Request["ProcedurerPostalDistrictCode"].ToString();
            string ProcedurerStreet = Request["ProcedurerStreet"].ToString();
            int ProcedurerPersonalPaperTypeID = int.Parse(Request["ProcedurerPersonalPaperTypeID"].ToString());
            string ProcedurerPersonalPaperNumber = Request["ProcedurerPersonalPaperNumber"].ToString();
            DateTime ProcedurerPersonalPaperIssuedDate = DateTime.ParseExact(Request["ProcedurerPersonalPaperIssuedDate"].ToString(), "dd/MM/yyyy", null);
            string ProcedurerPersonalPaperIssuedPlace = Request["ProcedurerPersonalPaperIssuedPlace"].ToString();
            string SenderFullName = Request["SenderFullName"].ToString();
            string SenderPhone = Request["SenderPhone"].ToString();
            string SenderPostalDistrictCode = Request["SenderPostalDistrictCode"].ToString();
            string SenderStreet = Request["SenderStreet"].ToString();
            string ReceiverFullName = Request["ReceiverFullName"].ToString();
            string ReceiverPhone = Request["ReceiverPhone"].ToString();
            string ReceiverPostalDistrictCode = Request["ReceiverPostalDistrictCode"].ToString();
            string ReceiverStreet = Request["ReceiverStreet"].ToString();
            string OrderNote = Request["OrderNote"].ToString();
            using (VNPOST_AppointmentEntities db = new VNPOST_AppointmentEntities())
            {
                Order o = new Order();
                o.CustomerID = long.Parse(Session["userID"].ToString());
                o.OrderDate = DateTime.Now;
                o.ProfileID = ProfileID;
                o.AppointmentLetterCode = AppointmentLetterCode;
                o.ProcedurerFullName = ProcedurerFullName;
                o.ProcedurerPhone = ProcedurerPhone;
                o.ProcedurerPostalDistrictCode = ProcedurerPostalDistrictCode;
                o.ProcedurerStreet = ProcedurerStreet;
                o.ProcedurerPersonalPaperTypeID = ProcedurerPersonalPaperTypeID;
                o.ProcedurerPersonalPaperNumber = ProcedurerPersonalPaperNumber;
                o.ProcedurerPersonalPaperIssuedDate = ProcedurerPersonalPaperIssuedDate;
                o.ProcedurerPersonalPaperIssuedPlace = ProcedurerPersonalPaperIssuedPlace;
                o.SenderFullName = SenderFullName;
                o.SenderPhone = SenderPhone;
                o.SenderPostalDistrictCode = SenderPostalDistrictCode;
                o.SenderStreet = SenderStreet;
                o.ReceiverFullName = ReceiverFullName;
                o.ReceiverPhone = ReceiverPhone;
                o.ReceiverPostalDistrictCode = ReceiverPostalDistrictCode;
                o.ReceiverStreet = ReceiverStreet;
                o.OrderNote = OrderNote;
            }
            sourceimage.Save("C:/Users/1160/Desktop/temp.png");
            return View("/Views/Payment/Payment.cshtml");
        }
    }
}