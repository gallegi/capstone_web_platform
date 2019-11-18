using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using vnpost_ocr_system.Models;

namespace vnpost_ocr_system.Controllers.User
{
    public class CreateAddressInfoController : Controller
    {
        // GET: CreateAddressInfo
        [Route("tai-khoan/tao-dia-chi-moi")]
        public ActionResult Index()
        {
            VNPOST_AppointmentEntities db = new VNPOST_AppointmentEntities();
            List<Province> listProvince = db.Provinces.ToList<Province>();
            List<PersonalPaperType> listPaperType = db.PersonalPaperTypes.ToList<PersonalPaperType>();
            ViewBag.listProvince = listProvince;
            ViewBag.listPaperType = listPaperType;
            return View("/Views/User/CreateAddressInfo.cshtml");
        }

        [Route("tai-khoan/tao-dia-chi-moi/them")]
        public ActionResult Add(string name, string phone, string province, string district
            , string address, string paperType, string code, string date, string placeOfIssue)
        {
            VNPOST_AppointmentEntities db = new VNPOST_AppointmentEntities();
            using (DbContextTransaction transaction = db.Database.BeginTransaction())
            {
                try
                {
                    ContactInfo c = new ContactInfo();
                    c.FullName = name;
                    c.Phone = phone;
                    c.PostalDistrictCode = district;
                    c.Street = address;
                    c.PersonalPaperTypeID = Convert.ToInt32(paperType);
                    c.PersonalPaperNumber = code;
                    c.PersonalPaperIssuedDate = DateTime.ParseExact(date, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    c.PersonalPaperIssuedPlace = placeOfIssue;

                    //function LogIn is in progress.
                    c.CustomerID  = Convert.ToInt32(Session["userID"].ToString()); ;
                    db.ContactInfoes.Add(c);
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

        [Route("tai-khoan/tao-dia-chi-moi/lay-quan-huyen")]
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
    }
}