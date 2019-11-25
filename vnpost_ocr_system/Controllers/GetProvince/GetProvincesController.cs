using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Dynamic;
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
            //3 cách lấy dữ liệu trong database, select để lọc data thừa trả về view cho nhẹ, dùng hay không thì tùy
            List<District> list = db.Database.SqlQuery<District>("select * from District where PostalProvinceCode = @code order by PostalDistrictName asc", new SqlParameter("code", code)).ToList()
                .Select(x => new District {
                    PostalDistrictCode = x.PostalDistrictCode,
                    PostalDistrictName = x.PostalDistrictName
                }).ToList();
            return Json(list);
        }

        [Route("GetProfile")]
        [HttpPost]
        public ActionResult GetProfileByPAId(int code)
        {
            VNPOST_AppointmentEntities db = new VNPOST_AppointmentEntities();
            List<Profile> list = db.Profiles.Where(x => x.PublicAdministrationLocationID.Equals(code)).OrderBy(x => x.ProfileName).ToList().Select(x => new Profile
            {
                ProfileID = x.ProfileID,
                ProfileName = x.ProfileName
            }).ToList();
            return Json(list);
        }

        [Route("GetAdmins")]
        [HttpPost]
        public ActionResult GetPublicAdministrationByDistrictCode(string code)
        {
            VNPOST_AppointmentEntities db = new VNPOST_AppointmentEntities();
            List<PublicAdministration> list = (from pd in db.Districts.Where(x => x.PostalDistrictCode.Equals(code))
                                               join po in db.PostOffices on pd.DistrictCode equals po.DistrictCode
                                               join pa in db.PublicAdministrations on po.PosCode equals pa.PosCode
                                               orderby pa.PublicAdministrationName
                                               select pa).ToList().Select(x => new PublicAdministration
                                               {
                                                   PublicAdministrationLocationID = x.PublicAdministrationLocationID,
                                                   PublicAdministrationName = x.PublicAdministrationName,
                                                   Address = x.Address
                                               }).ToList();
            return Json(list);
        }

        [Route("GetAddressOfPublicAdmins")]
        [HttpGet]
        public ActionResult GetAddressByPublicAdminCode(int code)
        {
            using (VNPOST_AppointmentEntities db = new VNPOST_AppointmentEntities())
            {
                return Json(db.PublicAdministrations.Find(code).PublicAdministrationName);
            }
        }
    }
}