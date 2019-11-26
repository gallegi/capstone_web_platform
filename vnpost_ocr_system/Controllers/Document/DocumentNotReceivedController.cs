using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Linq.Dynamic;
using System.Threading.Tasks;
using System.Web.Hosting;
using System.Web.Mvc;
using System.Web.Routing;
using vnpost_ocr_system.Models;
using vnpost_ocr_system.SupportClass;

namespace vnpost_ocr_system.Controllers.Document
{
    public class DocumentNotReceivedController : Controller
    {
        // GET: DocumentNotReceived
        [Auther(Roles = "1,2,3,4")]
        [Route("ho-so/ho-so-cho-nhan")]
        public ActionResult Index()
        {
            using (VNPOST_AppointmentEntities db = new VNPOST_AppointmentEntities())
            {
                List<Province> proList = db.Provinces.OrderBy(x => x.PostalProvinceName).ToList();
                ViewBag.proList = proList;
            }
            return View("/Views/Document/DocumentNotReceived.cshtml");
        }



        [Route("cho-tiep-nhan")]
        [HttpPost]
        public ActionResult Search(string province, string district, string coQuan, string profile, string dateFrom, string dateTo)
        {
            VNPOST_AppointmentEntities db = new VNPOST_AppointmentEntities();
            List<Non_revieve> searchList = null;
            int totalrows = 0;
            int totalrowsafterfiltering = 0;
            string query = "";
            DateTime from = DateTime.Today;
            DateTime to = DateTime.Today;
            try
            {
                int start = Convert.ToInt32(Request["start"]);
                int length = Convert.ToInt32(Request["length"]);
                string searchValue = Request["search[value]"];
                string sortColumnName = Request["columns[" + Request["order[0][column]"] + "][name]"];
                string sortDirection = Request["order[0][dir]"];
                if (!profile.Equals(""))
                {
                    query = "select o.*, p.ProfileName, p.PublicAdministrationLocationID from [Order] o join [Profile] p on o.ProfileID = p.ProfileID where o.StatusID = -3 and p.ProfileID = @profile";
                }
                else if (!coQuan.Equals(""))
                {
                    query = "select o.*, p.ProfileName, p.PublicAdministrationLocationID , pa.Phone as 'PAPhone' from [Order] o join [Profile] p on o.ProfileID = p.ProfileID join " +
                        "PublicAdministration pa on p.PublicAdministrationLocationID = pa.PublicAdministrationLocationID where o.StatusID = -3 and " +
                        "pa.PublicAdministrationLocationID = @coQuan";
                }
                else if (!district.Equals(""))
                {
                    query = "select o.*, p.ProfileName, p.PublicAdministrationLocationID ,pa.Phone as 'PAPhone' from [Order] o join [Profile] p on o.ProfileID = p.ProfileID join " +
                        "PublicAdministration pa on p.PublicAdministrationLocationID = pa.PublicAdministrationLocationID " +
                        "join PostOffice po on pa.PosCode = po.PosCode join District d on po.DistrictCode = d.DistrictCode where o.StatusID = -3 " +
                        "and d.PostalDistrictCode = @district";
                }
                else if (!province.Equals(""))
                {
                    query = "select o.*, p.ProfileName, p.PublicAdministrationLocationID ,pa.Phone as 'PAPhone' from [Order] o join [Profile] p on o.ProfileID = p.ProfileID join " +
                        "PublicAdministration pa on p.PublicAdministrationLocationID = pa.PublicAdministrationLocationID " +
                        "join PostOffice po on pa.PosCode = po.PosCode join District d on po.DistrictCode = d.DistrictCode " +
                        "join Province pro on d.PostalProvinceCode = pro.PostalProvinceCode where o.StatusID = -3 and pro.PostalProvinceCode = @province";
                }
                else
                {
                    query = "select o.*, p.ProfileName, p.PublicAdministrationLocationID ,pa.Phone as 'PAPhone' from [Order] o join [Profile] p on o.ProfileID = p.ProfileID join " +
                        "PublicAdministration pa on p.PublicAdministrationLocationID = pa.PublicAdministrationLocationID " +
                        "join PostOffice po on pa.PosCode = po.PosCode join District d on po.DistrictCode = d.DistrictCode " +
                        "join Province pro on d.PostalProvinceCode = pro.PostalProvinceCode where o.StatusID = -3";
                }
                if (!dateFrom.Equals("") && !dateTo.Equals(""))
                {
                    query += " and o.OrderDate between @dateFrom and @dateTo";
                    from = DateTime.ParseExact(dateFrom, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    to = DateTime.ParseExact(dateTo, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                }
                else
                {
                    if (!dateFrom.Equals(""))
                    {
                        query += " and o.OrderDate >= @dateFrom";
                        from = DateTime.ParseExact(dateFrom, "dd/MM/yyyy", CultureInfo.InvariantCulture);

                    }
                    else if (!dateTo.Equals(""))
                    {
                        query += " and o.OrderDate <= @dateTo";
                        to = DateTime.ParseExact(dateTo, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    }
                }
                searchList = db.Database.SqlQuery<Non_revieve>(query + " order by " + sortColumnName + " " + sortDirection + " OFFSET " + start + " ROWS FETCH NEXT " + length + " ROWS ONLY", new SqlParameter("profile", profile),
                                                                      new SqlParameter("coQuan", coQuan),
                                                                      new SqlParameter("district", district),
                                                                      new SqlParameter("province", province),
                                                                      new SqlParameter("dateFrom", from),
                                                                      new SqlParameter("dateTo", to)).ToList();
                db.Configuration.LazyLoadingEnabled = false;

                totalrows = db.Database.SqlQuery<int>("SELECT COUNT(*) FROM ( " + query + " ) as count"
                                                                    , new SqlParameter("profile", profile),
                                                                      new SqlParameter("coQuan", coQuan),
                                                                      new SqlParameter("district", district),
                                                                      new SqlParameter("province", province),
                                                                      new SqlParameter("dateFrom", from),
                                                                      new SqlParameter("dateTo", to)).FirstOrDefault();
                totalrowsafterfiltering = totalrows;

            }
            catch (Exception e)
            {

                e.Message.ToString();

            }
            return Json(new { data = searchList, draw = Request["draw"], recordsTotal = totalrows, recordsFiltered = totalrowsafterfiltering }, JsonRequestBehavior.AllowGet);

        }

    }
}