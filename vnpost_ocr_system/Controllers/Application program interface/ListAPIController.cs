using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using vnpost_ocr_system.Models;
using vnpost_ocr_system.SupportClass;

namespace vnpost_ocr_system.Controllers.Application_program_interface
{
    public class ListAPIController : Controller
    {
        // GET: ListAPI
        [Auther(Roles = "1")]
        [Route("api/danh-sach-api")]
        public ActionResult Index()
        {
            return View("/Views/API/ListAPI.cshtml");

        }
        [Route("api/danh-sach-api/getinformation")]
        [HttpPost]
        public ActionResult GetInformation()
        {
            using (VNPOST_AppointmentEntities db = new VNPOST_AppointmentEntities())
            {
                List<APIDB> listAPI = db.Database.SqlQuery<APIDB>("select APIID,APIUri,APIDescription" +
                    " from API").ToList();
                APIEdit api = db.Database.SqlQuery<APIEdit>("select  APIMethodID,APIUri,APIDescription,SampleResponse,Username," +
                   "Password from API where APIID=1").FirstOrDefault();
                List<APIInputParam> listAPIInputParam = db.Database.SqlQuery<APIInputParam>("select APIID,APIInputParamName,APIInputParamType,APIInputParamDescription," +
                    "LastMofifiedTime from APIInputParam where APIID=1").ToList();
                List<APIOutputParam> listAPIOutputParam = db.Database.SqlQuery<APIOutputParam>("select APIID,APIOutputParamName,APIOutputParamType,APIOutputParamDescription,LastMofifiedTime" +
                    " from APIOutputParam where APIID=1").ToList();
                return Json(new
                {
                    listAPI = listAPI,
                    api=api,
                    listAPIInputParam = listAPIInputParam,
                    listAPIOutputParam = listAPIOutputParam
                }, JsonRequestBehavior.AllowGet);

            }
        }
        [Auther(Roles = "1")]
        [Route("api/danh-sach-api/delete")]
        [HttpPost]
        public ActionResult Delete(int apiid)
        {

            using (VNPOST_AppointmentEntities db = new VNPOST_AppointmentEntities())
            {
                using (DbContextTransaction transaction = db.Database.BeginTransaction())
                {

                    try
                    {
                        db.Database.ExecuteSqlCommand("delete from APIInputParam where APIID=@apiid; " +
                        "delete from APIOutputParam where APIID = @apiid1; " +
                        "DELETE FROM API WHERE APIID = @apiid2; ",new SqlParameter("apiid",apiid), new SqlParameter("apiid1", apiid), new SqlParameter("apiid2", apiid));
                        transaction.Commit();
                        return Json("", JsonRequestBehavior.AllowGet);
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();


                        return new HttpStatusCodeResult(400);
                    }
                }
            }
        }
        [Route("api/danh-sach-api/getinformationofapi")]
        [HttpPost]
        public ActionResult GetInformationOfApi(int apiid)
        {
            using (VNPOST_AppointmentEntities db = new VNPOST_AppointmentEntities())
            {
                APIEdit api = db.Database.SqlQuery<APIEdit>("select  APIMethodID,APIUri,APIDescription,SampleResponse,Username," +
                   "Password from API where APIID=@apiid", new SqlParameter("apiid", apiid)).FirstOrDefault();
                List<APIInputParam> listAPIInputParam = db.Database.SqlQuery<APIInputParam>("select APIID,APIInputParamName,APIInputParamType,APIInputParamDescription," +
                    "LastMofifiedTime from APIInputParam where APIID=@apiid", new SqlParameter("apiid", apiid)).ToList();
                List<APIOutputParam> listAPIOutputParam = db.Database.SqlQuery<APIOutputParam>("select APIID,APIOutputParamName,APIOutputParamType,APIOutputParamDescription,LastMofifiedTime" +
                    " from APIOutputParam where APIID=@apiid", new SqlParameter("apiid", apiid)).ToList();
                return Json(new
                {
                    api = api,
                    listAPIInputParam = listAPIInputParam,
                    listAPIOutputParam = listAPIOutputParam
                }, JsonRequestBehavior.AllowGet); 

            }
        }
    }
    public class APIDB 
    {

        public int APIID { get; set; }

        public string APIUri { get; set; }
        public string APIDescription { get; set; }
       

    }
}