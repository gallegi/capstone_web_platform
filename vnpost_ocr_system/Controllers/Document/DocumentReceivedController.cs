using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using vnpost_ocr_system.Models;
using System.Linq.Dynamic;
using vnpost_ocr_system.SupportClass;
using System.Globalization;
using System.Web.Hosting;
using System.IO;
using OfficeOpenXml;

namespace vnpost_ocr_system.Controllers.Document
{
    public class DocumentReceivedController : Controller
    {
        public static List<ReceivedDocument> excelList = new List<ReceivedDocument>();
        // GET: DocumentRecived
        [Auther(Roles = "1,2,3,4")]
        [Route("ho-so/ho-so-da-nhan")]
        public ActionResult Index()
        {
            using (VNPOST_AppointmentEntities db = new VNPOST_AppointmentEntities())
            {
                string address = "";
                List<Province> proList = new List<Province>();
                if (Session["adminRole"] != null)
                {

                    if (Convert.ToInt32(Session["adminRole"]) == 1 || Convert.ToInt32(Session["adminRole"]) == 2)
                    {
                        proList = db.Provinces.OrderBy(x => x.PostalProvinceName).ToList();
                    }
                    else
                    {
                        if (Session["adminPro"] != null)
                        {
                            address = Session["adminPro"].ToString();
                            proList = db.Provinces.Where(x => x.PostalProvinceCode == address).OrderBy(x => x.PostalProvinceName).ToList();
                        }
                        else
                        {
                            proList = db.Provinces.OrderBy(x => x.PostalProvinceName).ToList();
                        }
                    }
                }
                ViewBag.proList = proList;
            }
            return View("/Views/Document/DocumentRecived.cshtml");
        }


        


        [Route("da-tiep-nhan")]
        [HttpPost]
        public ActionResult Search(string province, string district, string organ, string profile, string dateFrom, string dateTo)
        {
            VNPOST_AppointmentEntities db = new VNPOST_AppointmentEntities();
            List<ReceivedDocument> searchList = new List<ReceivedDocument>();
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

                query = "select o.*,pro.PostalProvinceName, p.ProfileName, p.PublicAdministrationLocationID,pa.PublicAdministrationName from [Order] o join [Profile] p on o.ProfileID = p.ProfileID " +
                        "join PublicAdministration pa on p.PublicAdministrationLocationID = pa.PublicAdministrationLocationID " +
                        "join PostOffice po on pa.PosCode = po.PosCode join District d on d.DistrictCode = po.DistrictCode " +
                        "join Province pro on pro.PostalProvinceCode = d.PostalProvinceCode " +
                        "where o.StatusID != 0 and o.StatusID != -3 and o.StatusID != 5";
                if (!province.Equals(""))
                {
                    query += " and pro.PostalProvinceCode = @province ";
                }
                if (!district.Equals(""))
                {
                    query += " and d.PostalDistrictCode = @district ";
                }
                if (!organ.Equals(""))
                {
                    query += " and pa.PublicAdministrationLocationID = @organ ";
                }
                if (!profile.Equals(""))
                {
                    query += " and p.ProfileID = @profile ";
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

                searchList = db.Database.SqlQuery<ReceivedDocument>(query + " order by "+ sortColumnName + " "+ sortDirection + " OFFSET " + start + " ROWS FETCH NEXT " + length + " ROWS ONLY",
                                                                 new SqlParameter("profile", profile),
                                                                 new SqlParameter("organ", organ),
                                                                 new SqlParameter("district", district),
                                                                 new SqlParameter("province", province),
                                                                 new SqlParameter("dateFrom", from),
                                                                 new SqlParameter("dateTo", to)).ToList();
                db.Configuration.LazyLoadingEnabled = false;

                totalrows = db.Database.SqlQuery<int>("SELECT COUNT(*) FROM ( " + query + " ) as count"
                                                                    , new SqlParameter("profile", profile),
                                                                      new SqlParameter("organ", organ),
                                                                      new SqlParameter("district", district),
                                                                      new SqlParameter("province", province),
                                                                      new SqlParameter("dateFrom", from),
                                                                      new SqlParameter("dateTo", to)).FirstOrDefault();
                totalrowsafterfiltering = totalrows;
                excelList = db.Database.SqlQuery<ReceivedDocument>(query, new SqlParameter("profile", profile),
                                                                 new SqlParameter("organ", organ),
                                                                 new SqlParameter("district", district),
                                                                 new SqlParameter("province", province),
                                                                 new SqlParameter("dateFrom", from),
                                                                 new SqlParameter("dateTo", to)).ToList();

            }
            catch (Exception e)
            {
                e.Message.ToString();
            }
            return Json(new { data = searchList, draw = Request["draw"], recordsTotal = totalrows, recordsFiltered = totalrowsafterfiltering }, JsonRequestBehavior.AllowGet);
        }
        [Route("ho-so-da-nhan/excel")]
        [HttpPost]
        public void ReturnExcel()
        {
            string path = HostingEnvironment.MapPath("/Excel/Ho-so-da-nhan.xlsx");
            FileInfo file = new FileInfo(path);
            using (ExcelPackage excelPackage = new ExcelPackage(file))
            {
                ExcelWorkbook excelWorkbook = excelPackage.Workbook;
                ExcelWorksheet excelWorksheet = excelWorkbook.Worksheets.First();

                int k = 2;
                for (int i = 0; i < excelList.Count; i++)
                {

                    excelWorksheet.Cells[k, 1].Value = i + 1;
                    excelWorksheet.Cells[k, 2].Value = excelList.ElementAt(i).AppointmentLetterCode;
                    excelWorksheet.Cells[k, 3].Value = excelList.ElementAt(i).ItemCode;
                    excelWorksheet.Cells[k, 4].Value = excelList.ElementAt(i).PostalProvinceName;
                    excelWorksheet.Cells[k, 5].Value = excelList.ElementAt(i).PublicAdministrationName;
                    excelWorksheet.Cells[k, 6].Value = excelList.ElementAt(i).ProfileName;
                    excelWorksheet.Cells[k, 7].Value = excelList.ElementAt(i).OrderDate != null ? excelList.ElementAt(i).OrderDate.ToString("dd/MM/yyyy") : "";
                    excelWorksheet.Cells[k, 8].Value = excelList.ElementAt(i).ReceiverFullName;
                    excelWorksheet.Cells[k, 9].Value = excelList.ElementAt(i).ReceiverStreet;
                    k++;
                }
                excelPackage.SaveAs(new FileInfo(HostingEnvironment.MapPath("/Excel/Download/Ho-so-da-nhan.xlsx")));
            }
        }
    }
}