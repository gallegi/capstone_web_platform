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
        public ActionResult UserInfo(string DeviceToken)
        {
            DateTime minCreateDate = DateTime.Now.AddHours(-12);
            var FBToken = db.FirebaseTokens.Where(x => x.FirebaseToken1.Equals(DeviceToken) && x.Status.Equals(true) && x.CreateDate >= minCreateDate).FirstOrDefault();
            if (FBToken == null)
            {
                return Json(new { Name = "", Email = "" }, JsonRequestBehavior.AllowGet);
            }

            var AuthToken = db.AuthenticationTokens.Where(x => x.TokenID.Equals(FBToken.AuthTokenID) && x.Status.Equals(true) && x.CreateDate >= minCreateDate).FirstOrDefault();
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
        public ActionResult GetAllNotification(string DeviceToken)
        {
            DateTime minCreateDate = DateTime.Now.AddHours(-12);
            var FBToken = db.FirebaseTokens.Where(x => x.FirebaseToken1.Equals(DeviceToken) && x.Status.Equals(true) && x.CreateDate >= minCreateDate).FirstOrDefault();
            if (FBToken == null)
            {
                return Json(new { Name = "", Email = "" }, JsonRequestBehavior.AllowGet);
            }

            var AuthToken = db.AuthenticationTokens.Where(x => x.TokenID.Equals(FBToken.AuthTokenID) && x.Status.Equals(true) && x.CreateDate >= minCreateDate).FirstOrDefault();
            if (AuthToken == null)
            {
                return Json(new { Name = "", Email = "" }, JsonRequestBehavior.AllowGet);
            }

            List<notiMessage> notiMesses = db.NotificationMessages.Where(x => x.CustomerID.Equals(AuthToken.CustomerID))
                                                        .OrderByDescending(x => x.SentDate)
                                                        .Select(x => new notiMessage
                                                        {
                                                            Title = x.Title,
                                                            ContentText = x.ContentText,
                                                            SentDate = x.SentDate.ToString()
                                                        }).ToList();
            if (notiMesses.Count > 0)
                return Json(notiMesses, JsonRequestBehavior.AllowGet);
            else
                return Json(new notiMessage { Title = "", ContentText = "", SentDate = "" }, JsonRequestBehavior.AllowGet);
        }
    }

    public class notiMessage
    {
        public string Title { get; set; }
        public string ContentText { get; set; }
        private string _sendate;
        public string SentDate
        {
            //Apr 15 2020  4:25AM
            get { return DateTime.Parse(_sendate).ToString("dd/MM/yyyy HH:mm:ss"); }

            set { _sendate = value; }
        }
    }

}