using System;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Hosting;
using System.Web.Mvc;
using vnpost_ocr_system.Controllers.CustomController;
using vnpost_ocr_system.Models;
using vnpost_ocr_system.SupportClass;

namespace vnpost_ocr_system.Controllers
{
    public class PaymentController : BaseUserController
    {
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
                if (!Regex.IsMatch(ProcedurerPhone, "^0[35789]\\d{8}$"))
                    return Json(new { success = false, message = "Số điện thoại không hợp lệ" });
                string ProcedurerPostalDistrictCode = Request["ProcedurerPostalDistrictCode"].ToString();
                string ProcedurerStreet = Request["ProcedurerStreet"].ToString();
                int ProcedurerPersonalPaperTypeID = int.Parse(Request["ProcedurerPersonalPaperTypeID"].ToString());
                string ProcedurerPersonalPaperNumber = Request["ProcedurerPersonalPaperNumber"].ToString();

                DateTime? ProcedurerPersonalPaperIssuedDate = null;
                DateTime temp;
                if (DateTime.TryParseExact(Request["ProcedurerPersonalPaperIssuedDate"].ToString(), "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out temp))
                {
                    ProcedurerPersonalPaperIssuedDate = temp;
                }
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
                                return Json(new { success = false, message = "Mã giấy hẹn này đã được tạo bởi 1 đơn hàng trước đó, vào mục tìm kiếm để xem đơn hàng tương ứng với mã giấy hẹn này" });
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
                            db.SaveChanges();

                            OrderStatusDetail detail = new OrderStatusDetail();
                            detail.OrderID = o.OrderID;
                            detail.StatusID = -3;
                            detail.CreatedTime = DateTime.Now;
                            db.OrderStatusDetails.Add(detail);
                            db.SaveChanges();

                            OrderImage image = new OrderImage();
                            image.ImageRealName = imgName;
                            image.ImageName = DateTime.Now.ToFileTime().ToString() + "." + imgName.Split('.')[imgName.Split('.').Length - 1];
                            image.OrderID = o.OrderID;
                            db.OrderImages.Add(image);
                            db.SaveChanges();

                            Payment payment = new Payment();
                            payment.OrderID = o.OrderID;
                            payment.PaymentMethodID = 1;
                            payment.PaymentStatusID = 0;
                            payment.PaymentDate = DateTime.Now;
                            db.Payments.Add(payment);

                            db.SaveChanges();
                            transaction.Commit();
                            ViewBag.id_raw = o.OrderID;
                            string path = "~/OrderImage/" + o.OrderID + "/";
                            if (!Directory.Exists(HostingEnvironment.MapPath(path)))
                            {
                                Directory.CreateDirectory(HostingEnvironment.MapPath(path));
                            }
                            if (sourceimage.Size != null)
                            {
                                sourceimage.Save(HostingEnvironment.MapPath(path + image.ImageName));
                            }
                            return Json(new { success = true, message = "Thêm thành công", data = o.OrderID });
                        }
                        catch (Exception e)
                        {
                            transaction.Rollback();
                            return Json(new { success = false, message = "Có lỗi xảy ra", debug = e.StackTrace});
                        }
                    }
                }
            }
            catch (Exception e)
            {
                return Json(new { success = false, message = "Có lỗi xảy ra", debug = e.StackTrace});
            }
        }
    }
}