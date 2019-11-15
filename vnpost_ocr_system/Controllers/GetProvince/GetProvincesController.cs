using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using vnpost_ocr_system.Models;

namespace vnpost_ocr_system.Controllers.GetProvince
{
    public class GetProvincesController : Controller
    {
        [Route("GetDistrict")]
        [HttpPost]
        public ActionResult GetDistrictByProvinceCode(string code)
        {
            VNPOST_AppointmentEntities db = new VNPOST_AppointmentEntities();
            //3 cái đều chạy được, select để lọc data thừa trả về view cho nhẹ, dùng hay không thì tùy
            //List<District> list = db.Districts.Where(x => x.PostalProvinceCode.Equals(code)).Select(x => new District
            //{
            //    PostalDistrictCode = x.PostalDistrictCode,
            //    PostalDistrictName = x.PostalDistrictName
            //}).ToList();

            //List<District> list = (from d in db.Districts
            //                       where d.PostalProvinceCode.Equals(code)
            //                       select d).ToList().Select(x => new District
            //                       {
            //                           PostalDistrictCode = x.PostalDistrictCode,
            //                           PostalDistrictName = x.PostalDistrictName
            //                       }).ToList();
            List<District> list = db.Database.SqlQuery<District>("select * from District where PostalProvinceCode = @code", new SqlParameter("code", code)).ToList()
                .Select(x => new District {
                    PostalDistrictCode = x.PostalDistrictCode,
                    PostalDistrictName = x.PostalDistrictName
                }).ToList();
            return Json(list);
        }

        [Route("GetProfile")]
        [HttpPost]
        public ActionResult GetProfileByPAId(string code)
        {
            VNPOST_AppointmentEntities db = new VNPOST_AppointmentEntities();
            List<Profile> list = db.Profiles.Where(x => x.PublicAdministrationLocationID.Equals(code)).ToList().Select(x => new Profile
            {
                ProfileID = x.ProfileID,
                ProfileName = x.ProfileName
            }).ToList();
            return Json(list);
        }

        [Route("GetDetails")]
        [HttpPost]
        public ActionResult GetPublicAdministrationByDistrictCode(string code)
        {
            VNPOST_AppointmentEntities db = new VNPOST_AppointmentEntities();
            List<PublicAdministration> list = (from po in db.PostOffices.Where(x => x.DistrictCode.Equals(code))
                                               join pa in db.PublicAdministrations on po.PosCode equals pa.PosCode
                                               select pa).ToList().Select(x => new PublicAdministration
                                               {
                                                   PublicAdministrationLocationID = x.PublicAdministrationLocationID,
                                                   PublicAdministrationName = x.PublicAdministrationName
                                               }).ToList();
            return Json(list);
        }
    }
}