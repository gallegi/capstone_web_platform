using Newtonsoft.Json.Linq;
using System;
using System.Data.Entity;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Linq.Dynamic;
using System.Net;
using System.Web.Mvc;
using vnpost_ocr_system.Controllers.Mobile;
using vnpost_ocr_system.Models;
using vnpost_ocr_system.SupportClass;

namespace vnpost_ocr_system.Controllers.Document
{
    public class DocumentNotReceivedDetailController : Controller
    {
        public static bool err;
        [Auther(Roles = "1,2,3,4")]
        [Route("ho-so/ho-so-cho-nhan/chi-tiet")]
        //Tuấn: Tôi tạm comment đoạn này đang bị lỗi lại để ae rebuild được, ô fix nhanh nhé
        //GET: DocumentNotReceivedDetail
        public ActionResult Detail(string id)
        {
            VNPOST_AppointmentEntities db = new VNPOST_AppointmentEntities();
            NotReceivedDocumentDetail order = new NotReceivedDocumentDetail();
            string sql = "select *, oi.ImageName, oi.ImageRealName ,pa.Phone as 'PAPhone',pa.Address as 'PAAddress', po.Phone as 'POPhone', po.Address as 'POAddress' from [Order] o join [Profile] p on o.ProfileID = p.ProfileID join " +
                        "PublicAdministration pa on p.PublicAdministrationLocationID = pa.PublicAdministrationLocationID " +
                        "join PostOffice po on pa.PosCode = po.PosCode join District d on po.DistrictCode = d.DistrictCode " +
                        "join Province pro on d.PostalProvinceCode = pro.PostalProvinceCode " +
                        "join PersonalPaperType ppt on o.ProcedurerPersonalPaperTypeID = ppt.PersonalPaperTypeID " +
                        "join OrderImage oi on o.[OrderID] = oi.[OrderID] " +
                        "where o.OrderID = @id";
            order = db.Database.SqlQuery<NotReceivedDocumentDetail>(sql, new SqlParameter("id", id)).FirstOrDefault();
            ViewBag.Order = order;
            ViewBag.letterid = id;
            ViewBag.imageUrl = id + "/" + order.ImageName;
            ViewBag.imageRUrl = order.ImageRealName;
            if (err == true)
            {
                ViewBag.error = "err";
                err = false;
            }
            return View("/Views/Document/DocumentNotReceivedDetail.cshtml");
        }
        [Auther(Roles = "1,2,3,4")]
        [Route("ho-so/ho-so-cho-nhan/chi-tiet/cap-nhat")]
        [HttpPost]
        public ActionResult Update()
        {
            string itemCode = Request["itemCode"];
            string status = Request["status"];
            string note = Request["note"];
            string id = Request["id"];
            string letterid = Request["letterid"];
            VNPOST_AppointmentEntities db = new VNPOST_AppointmentEntities();
            //da huy
            if (status.Equals("0"))
            {
                Order order = db.Orders.Where(x => x.OrderID.ToString().Equals(id)).FirstOrDefault();
                order.StatusID = 0;
                db.SaveChanges();

                //Send firebase message
                MobileAppController.SendFCMMessage(order.OrderID, long.Parse(status));

                return Json(new { message = "Cancelled" }, JsonRequestBehavior.AllowGet);
            }


            using (DbContextTransaction con = db.Database.BeginTransaction())
            {
                try
                {
                    Order order = db.Orders.Where(x => x.ItemCode.Equals(itemCode) && x.StatusID == -2).FirstOrDefault();
                    if (order != null)
                    {
                        return Json(new { message = "Exist" }, JsonRequestBehavior.AllowGet);
                    }
                    int conId = Convert.ToInt32(id);
                    OrderStatusDetail osd = new OrderStatusDetail();
                    osd.OrderID = Convert.ToInt64(id);
                    osd.StatusID = Convert.ToInt32(status);
                    osd.Note = note;
                    osd.CreatedTime = DateTime.Now;
                    db.OrderStatusDetails.Add(osd);
                    db.SaveChanges();
                    Order o = db.Orders.Where(x => x.OrderID == conId).FirstOrDefault();
                    o.ItemCode = itemCode;
                    o.Amount = Convert.ToDouble(getAllInfo(itemCode)["TongCuocChuyenPhat"]);
                    o.TotalAmountInWords = NumberToCurrencyWord.convert((int)o.Amount);
                    //processed
                    long usernameID = Convert.ToInt64((Session["useradminID"]).ToString());
                    o.ProcessedBy = usernameID;
                    //o.StatusID = Convert.ToInt32(status);
                    db.Entry(o).State = EntityState.Modified;
                    db.SaveChanges();
                    con.Commit();

                    //Send firebase message
                    MobileAppController.SendFCMMessage(osd.OrderID, osd.StatusID);
                    err = false;
                    return Json(new { message = "Success" }, JsonRequestBehavior.AllowGet);
                }
                catch (Exception e)
                {
                    e.Message.ToString();
                    con.Rollback();
                    err = true;
                    return Json(new { message = "Detail" }, JsonRequestBehavior.AllowGet);
                }
            }
        }



        public static JObject getAllInfo(string itemCode)
        {
            JArray jsonArray = null;
            string url = @"http://daotao-tiepnhanhoso.vnpost.vn/serviceApi/v1/LayDuLieuTheoMaBuuGui/" + itemCode;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.AutomaticDecompression = DecompressionMethods.GZip;

            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            using (Stream stream = response.GetResponseStream())
            using (StreamReader reader = new StreamReader(stream))
            {
                try
                {
                    jsonArray = JArray.Parse(reader.ReadToEnd());
                }
                catch (Exception ex)
                {
                    Console.WriteLine("no such item code from API: " + itemCode);
                    return null;
                }
                return (JObject)jsonArray.Last;
            }
        }
    }
}