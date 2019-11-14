using System;
using System.Collections.Generic;
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
            List<District> list = db.Districts.Where(x => x.PostalProvinceCode.Equals(code)).ToList();
            return Json(new { data = list});
        }

        [Route("GetPostOffice")]
        [HttpPost]
        public ActionResult GetPostOfficeByDistrictCode(string code)
        {
            VNPOST_AppointmentEntities db = new VNPOST_AppointmentEntities();
            List<PostOffice> list = db.PostOffices.Where(x => x.DistrictCode.Equals(code)).ToList();
            return Json(new { data = list });
        }

        [Route("GetDetails")]
        [HttpPost]
        public ActionResult GetPublicAdministrationByDistrictCode(string code)
        {
            VNPOST_AppointmentEntities db = new VNPOST_AppointmentEntities();
            List<PublicAdministration> list = (from po in db.PostOffices.Where(x => x.DistrictCode.Equals(code))
                                               join pa in db.PublicAdministrations on po.PosCode equals pa.PosCode
                                               select pa).ToList();
            return Json(new { data = list });
        }
    }
}