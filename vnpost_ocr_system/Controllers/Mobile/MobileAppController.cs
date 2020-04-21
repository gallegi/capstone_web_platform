using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using vnpost_ocr_system.Models;

namespace vnpost_ocr_system.Controllers.Mobile
{
    public class MobileAppController : Controller
    {
        private VNPOST_AppointmentEntities db = new VNPOST_AppointmentEntities();

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
            var AuthToken = db.AuthenticationTokens.Where(x => x.Token.Equals(VNPostORCAuthToken) && x.Status.Equals(true) && x.ExpireDate >= dateTimeNow).FirstOrDefault();
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
            var AuthToken = db.AuthenticationTokens.Where(x => x.Token.Equals(VNPostORCAuthToken) && x.Status.Equals(true) && x.ExpireDate >= dateTimeNow).FirstOrDefault();
            if (AuthToken == null)
            {
                return Json(new notiMessage { Title = "", ContentText = "", SentDate = "" }, JsonRequestBehavior.AllowGet);
            }

            List<notiMessage> notiMesses = (from n in db.NotificationMessages
                                            join o in db.Orders on n.OrderID equals o.OrderID
                                            where o.CustomerID.Equals(AuthToken.CustomerID)
                                            orderby n.SentDate 
                                            select n).ToList().Select(x => new notiMessage
                                            {
                                                orderID = x.OrderID,
                                                Title = x.Title,
                                                ContentText = x.ContentText,
                                                SentDate = x.SentDate.ToString()
                                            }).ToList();
            //List<notiMessage> notiMesses = db.NotificationMessages.Where(x => x.CustomerID.Equals(AuthToken.CustomerID))
            //                                            .OrderByDescending(x => x.SentDate)
            //                                            .Select(x => new notiMessage
            //                                            {
            //                                                Title = x.Title,
            //                                                ContentText = x.ContentText,
            //                                                SentDate = x.SentDate.ToString()
            //                                            }).ToList();
            if (notiMesses.Count == 0)
            {
                notiMesses = new List<notiMessage>();
                notiMesses.Add(new notiMessage { orderID = -1, Title = "", ContentText = "", SentDate = "" });
            }
            return Json(notiMesses, JsonRequestBehavior.AllowGet);
        }
    }

    public class notiMessage
    {
        public long orderID { get; set; }
        public string Title { get; set; }
        public string ContentText { get; set; }
        private string _sendate;
        public string SentDate
        {
            //Apr 15 2020  4:25AM
            get
            {
                if (_sendate == "") return "";
                else return DateTime.Parse(_sendate).ToString("dd/MM/yyyy HH:mm:ss");
            }

            set { _sendate = value; }
        }
    }

}