using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using vnpost_ocr_system.Models;
using vnpost_ocr_system.SupportClass;

namespace vnpost_ocr_system.Controllers.Application_program_interface
{
    public class EditAPIController : Controller
    {

        // GET: EditAPI
        [Auther(Roles = "1")]
        [Route("api/chinh-sua-api")]
        public ActionResult Index()
        {
            string api = Request.QueryString["apiid"];
            ViewBag.api = api;
            return View("/Views/API/EditAPI.cshtml");
        }
        [Route("api/chinh-sua-api/getinformation")]
        [HttpPost]
        public ActionResult GetInformation(int apiid)
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
                }, JsonRequestBehavior.AllowGet); ;

            }
        }
        [Auther(Roles = "1")]
        [Route("api/chinh-sua-api/edit")]
        [HttpPost]
        public ActionResult Edit(List<APIInputParam> listParameter, List<APIOutputParam> listRequest, API api)
        {
            using (VNPOST_AppointmentEntities db = new VNPOST_AppointmentEntities())
            {


                using (DbContextTransaction transaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        string bulk_update = String.Empty;
                        DateTime today = DateTime.Today;
                        string updateApi = $"update api set APIMethodID={api.APIMethodID},APIUri='{api.APIUri}',APIDescription=N'{api.APIDescription}',LastMofifiedTime=getdate()," +
                              $"SampleResponse = N'{api.SampleResponse}',Username = N'{api.Username}',Password = N'{api.Password}' where api.APIID={api.APIID} ";
                        bulk_update = string.Concat(bulk_update, updateApi);

                        foreach (APIInputParam input in listParameter)
                        {
                            string input_edit = $"if exists (select * from APIInputParam  where APIID={api.APIID} and APIInputParamName='{input.APIInputParamName}') " +
                                                "begin update APIInputParam set " +
                                              $"APIInputParamName = '{input.APIInputParamName}',APIInputParamType = '{input.APIInputParamType}',APIInputParamDescription = N'{input.APIInputParamDescription}',LastMofifiedTime = getdate() where APIID = {api.APIID} and APIInputParamName='{input.APIInputParamName}' " +
                                               "end else " +
                               " begin " +
                               "insert into APIInputParam(APIID, APIInputParamName, APIInputParamType, APIInputParamDescription, LastMofifiedTime) values(" +
        $" {api.APIID}, '{input.APIInputParamName}', '{input.APIInputParamType}', N'{input.APIInputParamDescription}', getdate()) " +
                                             " end ";
                            bulk_update = string.Concat(bulk_update, input_edit);
                        }
                        foreach (APIOutputParam output in listRequest)
                        {

                            string output_edit = $"if exists (select * from APIOutputParam  where APIID={api.APIID} and APIOutputParamName='{output.APIOutputParamName}') " +
                                                "begin update APIOutputParam set " +
                                              $"APIOutputParamName = '{output.APIOutputParamName}',APIOutputParamType = '{output.APIOutputParamType}',APIOutputParamDescription = N'{output.APIOutputParamDescription}',LastMofifiedTime = getdate() where APIID = {api.APIID} and APIOutputParamName='{output.APIOutputParamName}' " +
                                               "end else " +
                               " begin " +
                               "insert into APIOutputParam(APIID,APIOutputParamName,APIOutputParamType,APIOutputParamDescription,LastMofifiedTime) values(" +
                            $" {api.APIID}, '{output.APIOutputParamName}', '{output.APIOutputParamType}', N'{output.APIOutputParamDescription}', getdate()) " +
                             " end ";
                            bulk_update = string.Concat(bulk_update, output_edit);
                        }

                        db.Database.ExecuteSqlCommand(bulk_update);

                        transaction.Commit();
                        return Json("", JsonRequestBehavior.AllowGet);
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        return new HttpStatusCodeResult(400);
                    }
                }
            }
        }
        [Auther(Roles = "1")]
        [Route("api/chinh-sua-api/deleterequest")]
        [HttpPost]
        public ActionResult DeleteRequest(int apiid, string requestName)
        {
            JavaScriptSerializer js = new JavaScriptSerializer();
            string name = js.Deserialize<string>(requestName);
            using (VNPOST_AppointmentEntities db = new VNPOST_AppointmentEntities())
            {
                using (DbContextTransaction transaction = db.Database.BeginTransaction())
                {

                    try
                    {
                        db.Database.ExecuteSqlCommand("delete from APIOutputParam where APIID=@apiid and APIOutputParamName=@requestName"
                         , new SqlParameter("apiid", apiid), new SqlParameter("requestName", name));
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
        [Auther(Roles = "1")]
        [Route("api/chinh-sua-api/deleteparameter")]
        [HttpPost]
        public ActionResult DeleteParameter(int apiid, string paramterName)
        {
            JavaScriptSerializer js = new JavaScriptSerializer();
            string name = js.Deserialize<string>(paramterName);
            using (VNPOST_AppointmentEntities db = new VNPOST_AppointmentEntities())
            {
                using (DbContextTransaction transaction = db.Database.BeginTransaction())
                {

                    try
                    {
                        db.Database.ExecuteSqlCommand("delete from APIInputParam where APIID=@apiid and APIInputParamName=@paramterName"
                         , new SqlParameter("apiid", apiid), new SqlParameter("paramterName", name));
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
    }
    public class APIEdit
    {


        public int APIMethodID { get; set; }
        public string APIUri { get; set; }
        public string APIDescription { get; set; }
        public string SampleResponse { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }


    }

}