using System;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;
using vnpost_ocr_system.Models;
using vnpost_ocr_system.SupportClass;

namespace vnpost_ocr_system.Controllers
{
    public class PaymentController : Controller
    {
        // GET: Payment

        //[Route("don-hang/thanh-toan")]
        //public ActionResult Index()
        //{
        //    return View("/Views/Payment/Payment.cshtml");
        //}

        [Auther(Roles = "0")]
        [HttpPost]
        [Route("don-hang/thanh-toan")]
        public ActionResult Add()
        {
            try
            {
                Image sourceimage = Image.FromStream(Request.Files["img"].InputStream, true, true);
                string imgName = Request["imgName"].ToString();
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
                    using (DbContextTransaction transaction = db.Database.BeginTransaction())
                    {
                        try
                        {
                            Order o = db.Database.SqlQuery<Order>("select * from [Order] where AppointmentLetterCode = @AppointmentLetterCode",
                                new SqlParameter("AppointmentLetterCode", AppointmentLetterCode)).FirstOrDefault();
                            if (o != null)
                                return Json(new { success = false, message = "Mã đơn hàng đã tồn tại" });
                            o = new Order();
                            long userID = Convert.ToInt64(Session["userID"].ToString());
                            o.CustomerID = userID;
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
                            o.StatusID = -3;
                            db.Orders.Add(o);

                            OrderStatusDetail detail = new OrderStatusDetail();
                            detail.OrderID = o.OrderID;
                            detail.StatusID = -3;
                            detail.CreatedTime = DateTime.Now;
                            db.OrderStatusDetails.Add(detail);

                            OrderImage image = new OrderImage();
                            image.ImageRealName = imgName;
                            image.ImageName = DateTime.Now.ToFileTime().ToString()+"."+imgName.Split('.')[imgName.Split('.').Length-1];
                            image.OrderID = o.OrderID;
                            db.OrderImages.Add(image);

                            Payment payment = new Payment();
                            payment.OrderID = o.OrderID;
                            payment.PaymentMethodID = 1;
                            payment.PaymentStatusID = 0;
                            db.Payments.Add(payment);

                            db.SaveChanges();
                            transaction.Commit();
                            ViewBag.id_raw = o.OrderID;
                            string path = "/OrderImage/";
                            if (!Directory.Exists(HostingEnvironment.MapPath(path)))
                            {
                                Directory.CreateDirectory(HostingEnvironment.MapPath(path));
                            }
                            if (sourceimage.Size != null)
                            {
                                sourceimage.Save(HostingEnvironment.MapPath(path + image.ImageName));
                            }
                            return Json(new { success = true, message = "Thêm thành công", data = o.AppointmentLetterCode });
                        }
                        catch (Exception e)
                        {
                            transaction.Rollback();
                            return Json(new { success = false, message = "Có lỗi xảy ra" });
                        }
                    }
                }
            }
            catch (Exception)
            {
                return Json(new { success = false, message = "Có lỗi xảy ra" });
            }
        }
    }
}