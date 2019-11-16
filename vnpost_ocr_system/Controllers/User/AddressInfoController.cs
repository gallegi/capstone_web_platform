using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Mvc;
using vnpost_ocr_system.Models;

namespace vnpost_ocr_system.Controllers.User
{
    public class AddressInfoController : Controller
    {
        // GET: AddressInfo
        [Route("tai-khoan/so-dia-chi")]
        public ActionResult Index()
        {
            //need function logIn to get session.
            VNPOST_AppointmentEntities db = new VNPOST_AppointmentEntities();
            List<ContactInfo> listContactInfo = db.ContactInfoes.Where(x => x.CustomerID == 1).ToList();
            List<Province> listProvince = db.Provinces.ToList<Province>();
            List<PersonalPaperType> listPaperType = db.PersonalPaperTypes.ToList<PersonalPaperType>();
            ViewBag.listContactInfo = listContactInfo;
            ViewBag.listProvince = listProvince;
            ViewBag.listPaperType = listPaperType;
            return View("/Views/User/AddressInfo.cshtml");
        }

        [Route("tai-khoan/so-dia-chi/xoa")]
        public ActionResult Delete(string Code)
        {
            VNPOST_AppointmentEntities db = new VNPOST_AppointmentEntities();
            using (DbContextTransaction transaction = db.Database.BeginTransaction())
            {
                try
                {
                    ContactInfo delete = db.ContactInfoes.Where(x => x.ContactInfoID.Equals(Code)).First();
                    db.ContactInfoes.Remove(delete);
                    db.SaveChanges();
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return new HttpStatusCodeResult(400);
                }
            }
            return new HttpStatusCodeResult(200);
        }

        [Route("tai-khoan/so-dia-chi/lay-chinh-sua")]
        public ActionResult getInfoEdit(string contactInfoId)
        {
            try
            {
                VNPOST_AppointmentEntities db = new VNPOST_AppointmentEntities();
                ContactInfoes contact = db.Database.SqlQuery<ContactInfoes>("select * from ContactInfo where ContactInfoID = @contacInfoId"
                    , new SqlParameter("contacInfoId", contactInfoId)).First();
                contact.stringDate = contact.PersonalPaperIssuedDate.Value.ToString("dd/MM/yyyy");
                return Json(contact);
            }
            catch (Exception ex)
            {

                return new HttpStatusCodeResult(400);
            }
        }

        [Route("tai-khoan/so-dia-chi/lay-quan-huyen")]
        public ActionResult getDistrict(string provinceID)
        {
            VNPOST_AppointmentEntities db = new VNPOST_AppointmentEntities();
            List<District> listDistrict = db.Districts.Where(x => x.PostalProvinceCode.Equals(provinceID)).ToList();
            listDistrict = listDistrict.Select(x => new District
            {
                PostalDistrictCode = x.PostalDistrictCode,
                PostalDistrictName = x.PostalDistrictName
            }).ToList();
            return Json(listDistrict);
        }

        [Route("tai-khoan/so-dia-chi/lay-tinh-thanh-pho")]
        public ActionResult getProvince()
        {
            VNPOST_AppointmentEntities db = new VNPOST_AppointmentEntities();
            List<Province> listProvince = db.Provinces.ToList<Province>();
            return Json(listProvince);
        }


        [Route("tai-khoan/so-dia-chi/chinh-sua")]
        public ActionResult Edit(string name, string phone, string address, string PaperTypeCode,
            string paperNumber, string districtCode, string date, string placeOfIssue, string id)
        {

            VNPOST_AppointmentEntities db = new VNPOST_AppointmentEntities();
            string query = "update ContactInfo set FullName = @name "
                    + " , Phone = @phone "
                    + " , PostalDistrictCode = @districtCode "
                    + " , Street = @address "
                    + " , PersonalPaperTypeID = @PaperTypeCode "
                    + " , PersonalPaperNumber = @paperNumber "
                    + " , PersonalPaperIssuedDate = @date "
                    + " , PersonalPaperIssuedPlace = @placeOfIssue "
                    + " where ContactInfoID = @id ";
            using (DbContextTransaction transaction = db.Database.BeginTransaction())
            {
                try
                {
                    db.Database.ExecuteSqlCommand(query
                        , new SqlParameter("name", name)
                        , new SqlParameter("phone", phone)
                        , new SqlParameter("districtCode", districtCode)
                        , new SqlParameter("address", address)
                        , new SqlParameter("PaperTypeCode", PaperTypeCode)
                        , new SqlParameter("paperNumber", paperNumber)
                        , new SqlParameter("date", date)
                        , new SqlParameter("placeOfIssue", placeOfIssue)
                        , new SqlParameter("id", id));
                    transaction.Commit();
                    return new HttpStatusCodeResult(200);
                    //return Redirect("tai-khoan/so-dia-chi");
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return new HttpStatusCodeResult(400);
                }
            }
        }
    }

    public class ContactInfoes : ContactInfo
    {
        public string stringDate { get; set; }
        public string StatusName { get; set; }
    }
}