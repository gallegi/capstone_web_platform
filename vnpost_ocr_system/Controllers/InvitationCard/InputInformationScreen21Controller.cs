using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using vnpost_ocr_system.Models;

namespace vnpost_ocr_system.Controllers.InvitationCard
{
    public class InputInformationScreen21Controller : Controller
    {
        [Route("giay-hen/nhap-giay-hen/thong-tin-thu-tuc")]
        public ActionResult Index()
        {
            using (VNPOST_AppointmentEntities db = new VNPOST_AppointmentEntities())
            {
                List<Province> provinces = db.Provinces.ToList();
                ViewBag.provinces = provinces;

                List<ContactInfoDB> contactInfos = db.Database.SqlQuery<ContactInfoDB>(@"select ci.*, ppt.PersonalPaperTypeName from Customer c inner join ContactInfo ci
                    on c.CustomerID = ci.CustomerID
                    inner join PersonalPaperType ppt on ci.PersonalPaperTypeID = ppt.PersonalPaperTypeID
                    where c.CustomerID = @CustomerID", new SqlParameter("CustomerID", Session["userID"].ToString())).ToList();
                ViewBag.contactInfos = contactInfos;

                List<PersonalPaperType> papertypes = db.PersonalPaperTypes.ToList();
                ViewBag.papertypes = papertypes;
            }

            return View("/Views/InvitationCard/InputInformationScreen21.cshtml");
        }

        [Route("giay-hen/nhap-giay-hen/thong-tin-thu-tuc")]
        [HttpPost]
        public ActionResult Add()
        {
            try
            {
                string FullName = Request["FullName"];
                string Phone = Request["Phone"];
                string PostalDistrictCode = Request["PostalDistrictCode"];
                string Street = Request["Street"];
                string PersonalPaperTypeID = Request["PersonalPaperTypeID"];
                string PersonalPaperNumber = Request["PersonalPaperNumber"];
                string PersonalPaperIssuedDateString = Request["PersonalPaperIssuedDateString"];
                string PersonalPaperIssuedPlace = Request["PersonalPaperIssuedPlace"];
                string ContactInfoID = Request["ContactInfoID"];
                using (VNPOST_AppointmentEntities db = new VNPOST_AppointmentEntities())
                {
                    ContactInfo c = ContactInfoID == "" ? new ContactInfo() : db.ContactInfoes.Find(int.Parse(ContactInfoID));
                    if (ContactInfoID != "" && !c.CustomerID.Equals(long.Parse(Session["userID"].ToString()))) return Json(new { success = false, message = "Có lỗi xảy ra" });
                    c.FullName = FullName;
                    c.Phone = Phone;
                    c.PostalDistrictCode = PostalDistrictCode;
                    c.Street = Street;
                    c.PersonalPaperTypeID = int.Parse(PersonalPaperTypeID);
                    c.PersonalPaperNumber = PersonalPaperNumber;
                    c.PersonalPaperIssuedDate = DateTime.ParseExact(PersonalPaperIssuedDateString, "dd/MM/yyyy", null);
                    c.PersonalPaperIssuedPlace = PersonalPaperIssuedPlace;
                    c.CustomerID = long.Parse(Session["userID"].ToString());
                    if (ContactInfoID == "")
                    {
                        db.ContactInfoes.Add(c);
                    }
                    db.SaveChanges();
                }
                return Json(new { success = true, message = "Thêm mới thành công" });
            }
            catch (Exception)
            {
                return Json(new { success = false, message = "Có lỗi xảy ra" });
            }
        }

        [Route("giay-hen/nhap-giay-hen/thong-tin-thu-tuc/GetContactInfo")]
        [HttpPost]
        public ActionResult GetContactInfo(int id)
        {
            using (VNPOST_AppointmentEntities db = new VNPOST_AppointmentEntities())
            {
                ContactInfoDB info = db.Database.SqlQuery<ContactInfoDB>(@"select * from ContactInfo c inner join District d on c.PostalDistrictCode = d.PostalDistrictCode 
                        inner join Province p on d.PostalProvinceCode = p.PostalProvinceCode where ContactInfoID = @ContactInfoID",
                    new SqlParameter("ContactInfoID", id)).FirstOrDefault();
                if (!info.CustomerID.Equals(long.Parse(Session["userID"].ToString()))) return null;
                if (info == null) return null;

                List<District> districts = db.Database.SqlQuery<District>("select d.* from District d inner join Province p on d.PostalProvinceCode = p.PostalProvinceCode where p.PostalProvinceCode = @PostalProvinceCode", new SqlParameter("PostalProvinceCode", info.PostalProvinceCode)).ToList();

                info.PersonalPaperIssuedDateString = info.PersonalPaperIssuedDate == null ? "" : info.PersonalPaperIssuedDate.GetValueOrDefault().ToString("dd/MM/yyyy");
                return Json(new { info = info, list = districts });
            }
        }
    }
}