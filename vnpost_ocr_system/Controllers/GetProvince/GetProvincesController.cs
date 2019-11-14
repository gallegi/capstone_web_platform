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
        // GET: GetProvinces
        [Route("GetDistrict")]
        [HttpPost]
        public ActionResult Index(string code)
        {
            VNPOST_AppointmentEntities db = new VNPOST_AppointmentEntities();
            List<District> list = db.Districts.Where(x => x.PostalProvinceCode.Equals(code)).ToList();
            return Json(new { data = list});
        }
    }
}