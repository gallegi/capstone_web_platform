﻿using System;
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
        [Route("GetProvince")]
        [HttpPost]
        public ActionResult GetProvince()
        {
            VNPOST_AppointmentEntities db = new VNPOST_AppointmentEntities();
            List<Province> list = db.Database.SqlQuery<Province>("select * from Province order by PostalProvinceName asc").ToList()
                .Select(x => new Province
                {
                    PostalProvinceCode = x.PostalProvinceCode,
                    PostalProvinceName = x.PostalProvinceName
                }).ToList();
            return Json(list);
        }

        [Route("GetDistrict")]
        [HttpPost]
        public ActionResult GetDistrictByProvinceCode(string code)
        {
            VNPOST_AppointmentEntities db = new VNPOST_AppointmentEntities();
            List<District> list = db.Database.SqlQuery<District>("select * from District where PostalProvinceCode = @code order by PostalDistrictName asc", new SqlParameter("code", code)).ToList()
                .Select(x => new District
                {
                    PostalDistrictCode = x.PostalDistrictCode,
                    PostalDistrictName = x.PostalDistrictName
                }).ToList();
            return Json(list);
        }

        [Route("GetProfile")]
        [HttpPost]
        public ActionResult GetProfileByPAId(string code)
        {
            int code_number;
            if (!int.TryParse(code, out code_number))
                return null;
            VNPOST_AppointmentEntities db = new VNPOST_AppointmentEntities();
            List<Profile> list = db.Profiles.Where(x => x.PublicAdministrationLocationID.Equals(code_number)).OrderBy(x => x.ProfileName).ToList().Select(x => new Profile
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
            List<PublicAdministration> list = (from pd in db.Districts
                                               join po in db.PostOffices on pd.DistrictCode equals po.DistrictCode
                                               join pa in db.PublicAdministrations on po.PosCode equals pa.PosCode
                                               where pd.PostalDistrictCode.Equals(code)
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

        [Route("getProvinceDistrictPublicAdmins")]
        [HttpPost]
        public ActionResult getProvinceDistrictPublicAdmins(int ProfileID)
        {
            using (VNPOST_AppointmentEntities db = new VNPOST_AppointmentEntities())
            {
                var data = (from d in db.Districts
                            join po in db.PostOffices on d.DistrictCode equals po.DistrictCode
                            join pa in db.PublicAdministrations on po.PosCode equals pa.PosCode
                            join p in db.Profiles on pa.PublicAdministrationLocationID equals p.PublicAdministrationLocationID
                            where p.ProfileID.Equals(ProfileID)
                            select new
                            {
                                d.PostalProvinceCode,
                                d.PostalDistrictCode,
                                pa.PublicAdministrationLocationID
                            }).FirstOrDefault();
                return Json(new { success = data == null ? false : true, data = data});
            }
        }
    }
}