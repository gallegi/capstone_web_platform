using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web.Mvc;
using vnpost_ocr_system.Controllers.CustomController;
using vnpost_ocr_system.Models;

namespace vnpost_ocr_system.Controllers.Mobile
{
    public class MobileAppController : BaseUserController
    {
        private VNPOST_AppointmentEntities db = new VNPOST_AppointmentEntities();
        private static string ServerKEY = "AAAA00Hwtuw:APA91bGni43m9ze5t4PbyJFps3XlKAr09KXf6JvJe643DBQSk2jckcz5peoNV4V1IAOZp7NLL-jO6gXjatdBkrqrLaoC7jKlxB5VRQSdTHns9Dy-Gb5V7SzTYAJPLMmPnXv2Z9f-oST7";
        private static string SenderID = "907344393964";

        [Route("api/mobile/device_token")]
        public ActionResult DeviceToken(string DeviceToken)
        {
            if (DeviceToken != null)
            {
                Session["DeviceToken"] = DeviceToken;
                return Redirect("~/khach-hang/dang-nhap");

                //return View("/Views/MobileView/Login.cshtml");
            }
            return Redirect("~/Home/index");
        }

        [Route("api/mobile/user_info")]
        [HttpPost]
        public ActionResult UserInfo(string VNPostORCAuthToken)
        {
            DateTime dateTimeNow = DateTime.Now;
            var AuthToken = db.AuthenticationTokens.Where(x => x.AuthToken.Equals(VNPostORCAuthToken) && x.Status.Equals(true) && x.ExpireDate >= dateTimeNow).FirstOrDefault();
            if (AuthToken == null)
            {
                return Json(new { Name = "", Email = "" }, JsonRequestBehavior.AllowGet);
            }

            Customer custom = db.Customers.Where(x => x.CustomerID.Equals(AuthToken.CustomerID)).FirstOrDefault();
            if (custom != null)
                return Json(new { Name = custom.FullName, Email = custom.Email }, JsonRequestBehavior.AllowGet);
            else
                return Json(new { Name = "", Email = "" }, JsonRequestBehavior.AllowGet);
        }

        [Route("api/mobile/all_notification")]
        [HttpPost]
        public ActionResult GetAllNotification(string VNPostORCAuthToken)
        {
            DateTime dateTimeNow = DateTime.Now;
            List<NotiMessage> notiMesses = new List<NotiMessage>();
            var AuthToken = db.AuthenticationTokens.Where(x => x.AuthToken.Equals(VNPostORCAuthToken) && x.Status.Equals(true) && x.ExpireDate >= dateTimeNow).FirstOrDefault();
            if (AuthToken == null)
            {
                notiMesses = new List<NotiMessage>
                {
                    new NotiMessage { orderID = -1, Title = "", ContentText = "", SentDate = "" }
                };
                return Json(notiMesses, JsonRequestBehavior.AllowGet);
            }

            notiMesses = (from n in db.NotificationMessages
                          join o in db.Orders on n.OrderID equals o.OrderID
                          where o.CustomerID.Equals(AuthToken.CustomerID)
                          orderby n.SentDate descending
                          select n).ToList().Select(x => new NotiMessage
                          {
                              orderID = x.OrderID,
                              Title = x.Title,
                              ContentText = x.ContentText,
                              SentDate = x.SentDate.ToString()
                          }).ToList();
            if (notiMesses.Count == 0)
            {
                notiMesses = new List<NotiMessage>
                {
                    new NotiMessage { orderID = -1, Title = "", ContentText = "", SentDate = "" }
                };
            }
            return Json(notiMesses, JsonRequestBehavior.AllowGet);
        }

        [Route("api/mobile/order_status")]
        public ActionResult orderStatus(string orderID)
        {
            string cusid = "";
            if (Session["userID"] != null)
            {
                cusid = Session["userID"].ToString();
            }
            VNPOST_AppointmentEntities db = new VNPOST_AppointmentEntities();
            Order od = null;
            Order odb = null;

            odb = db.Database.SqlQuery<Order>("select o.* from [Order] o where o.OrderID = @id", new SqlParameter("id", orderID)).FirstOrDefault();

            if (odb == null)
            {
                return Redirect("/Home/index");
            }
            else
            {
                if (cusid != "")
                {
                    string checksql = "select o.*," +
                        "(case when o.StatusID = 0 then 0 else 1 end) as 'active' " +
                        "from [Order] o inner join Customer c on o.CustomerID = c.CustomerID " +
                        "where c.CustomerID = @cusid and o.OrderID = @oid " +
                        "order by active desc";
                    od = db.Database.SqlQuery<Order>(checksql, new SqlParameter("cusid", cusid), new SqlParameter("oid", orderID)).FirstOrDefault();
                }
                if (od == null) ViewBag.ck = 1;
                else ViewBag.ck = 0;

                string query = "select distinct YEAR(CreatedTime) as 'y', MONTH(CreatedTime) as 'm', DAY(CreatedTime) as 'd',CAST(CreatedTime AS date) as 'CreatedTime'  from OrderStatusDetail where OrderID = @id order by CreatedTime desc";
                List<OrderByDay> list = db.Database.SqlQuery<OrderByDay>(query, new SqlParameter("id", odb.OrderID)).ToList();
                query = "select os.*,s.StatusName,DATEPART(HOUR, CreatedTime) as 'h',DATEPART(MINUTE, CreatedTime) as 'mi',p.PosName,YEAR(CreatedTime) as 'y', MONTH(CreatedTime) as 'm', DAY(CreatedTime) as 'd' from OrderStatusDetail os inner join Status s on os.StatusID = s.StatusID left outer join PostOffice p on os.PosCode = p.PosCode where OrderID = @id order by CreatedTime desc";
                List<MyOrderDetail> listOrderDB = db.Database.SqlQuery<MyOrderDetail>(query, new SqlParameter("id", odb.OrderID)).ToList();
                foreach (var item in list)
                {
                    item.listOrder = new List<MyOrderDetail>();
                    foreach (var x in listOrderDB)
                    {
                        x.hour = x.h + ":" + x.mi;
                        if (x.y == item.y && x.m == item.m && x.d == item.d)
                        {
                            if (x.StatusID == 2) x.display = "Đã xác nhận đi khỏi bưu cục - " + x.PosCode + " - " + x.PosName;
                            else if (x.StatusID == 3) x.display = "Đã xác nhận đến bưu cục - " + x.PosCode + " - " + x.PosName;
                            else x.display = x.StatusName;

                            if (x.Note != null) x.display += " (" + x.Note + ")";
                            item.listOrder.Add(x);
                            item.dayOfWeek = x.CreatedTime.ToString("ddd");
                        }
                    }
                    switch (item.dayOfWeek)
                    {
                        case "Mon":
                            item.dayOfWeek = "Thứ hai";
                            break;
                        case "Tue":
                            item.dayOfWeek = "Thứ ba";
                            break;
                        case "Wed":
                            item.dayOfWeek = "Thứ tư";
                            break;
                        case "Thu":
                            item.dayOfWeek = "Thứ năm";
                            break;
                        case "Fri":
                            item.dayOfWeek = "Thứ sáu";
                            break;
                        case "Sat":
                            item.dayOfWeek = "Thứ bảy";
                            break;
                        case "Sun":
                            item.dayOfWeek = "Chủ nhật";
                            break;
                    }
                }
                ViewBag.list = list;
                string sql = @"select distinct o.*, pa.PublicAdministrationName, pa.Phone, pa.Address, pr.ProfileName, p.PosName, p.Address as 'Address_BC', p.Phone as 'Phone_BC', s.StatusName, 
                                 (case when o.StatusID = 0 then 0 else 1 end) as 'active'
                                 from [Order] o 
                                 inner join Profile pr on pr.ProfileID = o.ProfileID
                                 inner join PublicAdministration pa on pr.PublicAdministrationLocationID = pa.PublicAdministrationLocationID
                                 inner join PostOffice p on pa.PosCode = p.PosCode
                                 inner join District d on d.DistrictCode = p.DistrictCode
            inner join OrderStatusDetail os on o.OrderID = os.OrderID
            inner join Status s on o.StatusID = s.StatusID
                                 where o.OrderID = @id
                                 order by active desc";
                orderDB o = db.Database.SqlQuery<orderDB>(sql, new SqlParameter("id", orderID)).FirstOrDefault();
                if (o == null)
                {
                    sql = @"select distinct o.*, os.PosCode,
                             (case when o.StatusID = 0 then 0 else 1 end) as 'active'
                             from [Order] o left outer join Status s on o.StatusID = s.StatusID
                             inner join OrderStatusDetail os on o.OrderID = os.OrderID
                             where o.OrderID = @id
                             order by active desc";
                    o = db.Database.SqlQuery<orderDB>(sql, new SqlParameter("id", orderID)).FirstOrDefault();
                }
                else
                {
                    o.NgayCap = o.ProcedurerPersonalPaperIssuedDate.HasValue ? o.ProcedurerPersonalPaperIssuedDate.Value.ToString("dd/MM/yyyy") : null;
                    o.displayAmount = formatAmount(o.Amount);
                }

                if (o.StatusID == -3) o.step = 0;
                else if (o.StatusID == -2) o.step = 1;
                else if (o.StatusID == 1) o.step = 2;
                else if (o.StatusID == 5) o.step = 4;
                else if (o.StatusID == -1) o.step = 3;
                else o.step = 0;
                ViewBag.order = o;
                if (o.StatusID == 0)
                {
                    ViewBag.checkCancelled = true;
                }
                else
                {
                    ViewBag.checkCancelled = false;
                }
                return View("/Views/InvitationCard/DisplayStatus.cshtml");
            }
        }
        string formatAmount(double? i)
        {
            string s = "";
            if (i != null)
            {
                s = i + "";
                int temp = Convert.ToInt32(s);
                s = temp.ToString("C0");
                s = s.Substring(1);
            }
            return s;
        }

        public static void SendFCMMessage(long orderID, long statusID)
        {
            List<string> deviceTokens = new List<string>();
            string title = "", contentText = "", statusName = "";
            using (VNPOST_AppointmentEntities db = new VNPOST_AppointmentEntities())
            {
                Order order = db.Orders.Where(x => x.OrderID.Equals(orderID)).FirstOrDefault();
                if (order == null) return;
                else
                {
                    deviceTokens = db.AuthenticationTokens.Where(x => x.CustomerID.Equals(order.CustomerID)
                                                                 && !x.FirebaseToken.Equals(null)
                                                                 && x.ExpireDate > DateTime.Now
                                                                 && x.Status.Equals(true)).ToList()
                                                                 .Select(x => x.FirebaseToken).ToList();

                    var status = db.Status.Where(x => x.StatusID == statusID).FirstOrDefault();
                    if (status != null)
                    {
                        statusName = status.StatusName;
                    }
                }
            }

            if (statusName == "") return;

            title = "Đơn hàng số " + orderID;
            contentText = "Tình trạng đơn hàng : " + statusName
                         + " vào lúc: " + DateTime.Now.ToString("dd-MM-yyyy hh:mm");


            //Save message to DB
            AddNotiMessage(orderID, title, contentText);

            //Send message to all device that user loging in
            foreach (string deviceToken in deviceTokens)
                SendFCMMessageToDevice(deviceToken, title, contentText, orderID.ToString());
        }
        private static void AddNotiMessage(long orderID, string title, string contentText)
        {
            NotificationMessage notiMess = new NotificationMessage
            {
                OrderID = orderID,
                Title = title,
                ContentText = contentText,
                SentDate = DateTime.Now
            };

            using (VNPOST_AppointmentEntities db = new VNPOST_AppointmentEntities())
            {
                db.NotificationMessages.Add(notiMess);
                db.SaveChanges();
            }
        }
        private static void SendFCMMessageToDevice(string deviceToken, string title, string contentText, string orderID)
        {
            WebRequest tRequest = WebRequest.Create("https://fcm.googleapis.com/fcm/send");
            tRequest.Method = "post";
            //serverKey - Key from Firebase cloud messaging server  
            tRequest.Headers.Add(string.Format("Authorization: key={0}", ServerKEY));
            //Sender Id - From firebase project setting  
            tRequest.Headers.Add(string.Format("Sender: id={0}", SenderID));
            tRequest.ContentType = "application/json";
            var payload = new
            {
                to = deviceToken,
                priority = "high",
                content_available = true,
                data = new
                {
                    body = contentText,
                    title = title,
                    orderID = orderID
                }
            };

            string postbody = JsonConvert.SerializeObject(payload).ToString();
            Byte[] byteArray = Encoding.UTF8.GetBytes(postbody);
            tRequest.ContentLength = byteArray.Length;
            using (Stream dataStream = tRequest.GetRequestStream())
            {
                dataStream.Write(byteArray, 0, byteArray.Length);
                using (WebResponse tResponse = tRequest.GetResponse())
                {
                    using (Stream dataStreamResponse = tResponse.GetResponseStream())
                    {
                        if (dataStreamResponse != null) using (StreamReader tReader = new StreamReader(dataStreamResponse))
                            {
                                String sResponseFromServer = tReader.ReadToEnd();
                                //Handling respond here if needed!
                            }
                    }
                }
            }
        }
    }
}

