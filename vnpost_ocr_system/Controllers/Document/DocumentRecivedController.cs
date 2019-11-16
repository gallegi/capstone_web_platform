using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using vnpost_ocr_system.Models;
using System.Linq.Dynamic;

namespace vnpost_ocr_system.Controllers.Document
{
    public class DocumentRecivedController : Controller
    {
        // GET: DocumentRecived
        [Route("ho-so/ho-so-da-nhan")]
        public ActionResult Index()
        {
                using(VNPOST_AppointmentEntities db = new VNPOST_AppointmentEntities())
            {
                List<Province> proList = db.Provinces.ToList();
                ViewBag.proList = proList;

            }
                return View("/Views/Document/DocumentRecived.cshtml");
        }


        public class recieve : Order
        {
            public string PostalProvinceName { get; set; }
            public string ProfileName { get; set; }
            public string PublicAdministrationName { get; set; }
            public string Phone { get; set; }
        }

        
        [Route("da-tiep-nhan")]
        [HttpPost]
        public ActionResult Search(string province, string district, string organ, string profile, string dateFrom, string dateTo)
        {
            VNPOST_AppointmentEntities db = new VNPOST_AppointmentEntities();
            List<recieve> searchList = new List<recieve>();
            int totalrows = 0;
            int totalrowsafterfiltering = 0;
            string query = "";

            try
            {
                int start = Convert.ToInt32(Request["start"]);
                int length = Convert.ToInt32(Request["length"]);
                string searchValue = Request["search[value]"];
                string sortColumnName = Request["columns[" + Request["order[0][column]"] + "][name]"];
                string sortDirection = Request["order[0][dir]"];
                if (!profile.Equals(""))
                {
                    query = "select * from [Order] o join [Profile] p on o.ProfileID = p.ProfileID "+
"join PublicAdministration pa on p.PublicAdministrationLocationID = pa.PublicAdministrationLocationID "+
"where o.StatusID = 2 and p.ProfileID = @profile";
                }else if(!organ.Equals(""))
                {
                    query = "select * from [Order] o join [Profile] p on o.ProfileID = p.ProfileID " +
"join PublicAdministration pa on p.PublicAdministrationLocationID = pa.PublicAdministrationLocationID " +
"where o.StatusID = 2 and p.PublicAdministrationLocationID = @organ";
                }else if (!district.Equals(""))
                {
                    query = "select * from [Order] o join [Profile] p on o.ProfileID = p.ProfileID "+
"join PublicAdministration pa on p.PublicAdministrationLocationID = pa.PublicAdministrationLocationID "+
"join PostOffice po on pa.PosCode = po.PosCode join District d on d.DistrictCode = po.DistrictCode "+
"join Province pro on pro.PostalProvinceCode = d.PostalProvinceCode "+
"where o.StatusID = 2 and d.PostalDistrictCode = @district";
                }
                else if(!province.Equals(""))
                {
                    query = "select * from [Order] o join [Profile] p on o.ProfileID = p.ProfileID " +
"join PublicAdministration pa on p.PublicAdministrationLocationID = pa.PublicAdministrationLocationID " +
"join PostOffice po on pa.PosCode = po.PosCode join District d on d.DistrictCode = po.DistrictCode " +
"join Province pro on pro.PostalProvinceCode = d.PostalProvinceCode " +
"where o.StatusID = 2 and d.PostalProvinceCode = @province";
                }
                else
                {
                    query = "select * from [Order] o join [Profile] p on o.ProfileID = p.ProfileID " +
"join PublicAdministration pa on p.PublicAdministrationLocationID = pa.PublicAdministrationLocationID " +
"join PostOffice po on pa.PosCode = po.PosCode join District d on d.DistrictCode = po.DistrictCode " +
"join Province pro on pro.PostalProvinceCode = d.PostalProvinceCode " +
"where o.StatusID = 2";
                }

                if (!dateFrom.Equals("") && !dateTo.Equals(""))
                {
                    query += " and o.OrderDate between @dateFrom and @dateTo";
                }
                else
                {
                    if (!dateFrom.Equals(""))
                    {
                        query += " and o.OrderDate >= @dateFrom";
                    }
                    else if (!dateTo.Equals(""))
                    {
                        query += " and o.OrderDate <= @dateTo";
                    }
                }

                searchList = db.Database.SqlQuery<recieve>(query,new SqlParameter("profile",profile),
                                                                 new SqlParameter("organ",organ),
                                                                 new SqlParameter("district",district),
                                                                 new SqlParameter("province",province),
                                                                 new SqlParameter("dateFrom",dateFrom),
                                                                 new SqlParameter("dateTo", dateTo)).ToList();
                db.Configuration.LazyLoadingEnabled = false;

                totalrows = searchList.Count;

                totalrowsafterfiltering = searchList.Count;
                //sorting
                searchList = searchList.OrderBy(sortColumnName + " " + sortDirection).ToList<recieve>();
                //paging
                searchList = searchList.Skip(start).Take(length).ToList<recieve>();

            }
            catch(Exception e)
            {
                e.Message.ToString();
            }
            return Json(new { data = searchList, draw = Request["draw"], recordsTotal = totalrows, recordsFiltered = totalrowsafterfiltering }, JsonRequestBehavior.AllowGet);
        }
    }
}