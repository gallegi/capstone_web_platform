using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Web.Mvc;
using vnpost_ocr_system.Models;
using vnpost_ocr_system.SupportClass;

namespace vnpost_ocr_system.Controllers.Application_program_interface
{
    public class AddAPIController : Controller
    {
        // GET: AddAPI
        [Auther(Roles = "1")]
        [Route("api/them-moi-api")]
        public ActionResult Index()
        {
            return View("/Views/API/AddAPI.cshtml");
        }
        [Auther(Roles = "1")]
        [Route("api/them-moi-api/add")]
        [HttpPost]
        public ActionResult Add(List<APIInputParam> listParameter, List<APIOutputParam> listRequest, string materialize, string uri, string description, string username, string password, string response)
        {
            using (VNPOST_AppointmentEntities db = new VNPOST_AppointmentEntities())
            {


                using (DbContextTransaction transaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        string bulk_insert = string.Empty;
                        DateTime today = DateTime.Today;
                        string insertApi = $"insert  into API(APIMethodID,APIUri,APIDescription,LastMofifiedTime,SampleResponse,Username,Password) values({int.Parse(materialize)},'{uri}',N'{description}',getdate(),N'{response}',N'{username}',N'{password}');";
                        bulk_insert = string.Concat(bulk_insert, insertApi);

                        foreach (APIInputParam input in listParameter)
                        {
                            string input_insert = "insert into APIInputParam(APIID, APIInputParamName, APIInputParamType, APIInputParamDescription, LastMofifiedTime) values(" +
                        $"(select top 1 APIID from API order by APIID desc),'{input.APIInputParamName}','{input.APIInputParamType}',N'{input.APIInputParamDescription}',getdate());";
                            bulk_insert = string.Concat(bulk_insert, input_insert);
                        }
                        foreach (APIOutputParam output in listRequest)
                        {
                            string output_insert = "insert into APIOutputParam(APIID,APIOutputParamName,APIOutputParamType,APIOutputParamDescription,LastMofifiedTime) values(" +
                        $"(select top 1 APIID from API order by APIID desc),'{output.APIOutputParamName}','{output.APIOutputParamType}',N'{output.APIOutputParamDescription}',getdate());";
                            bulk_insert = string.Concat(bulk_insert, output_insert);
                        }
                        db.Database.ExecuteSqlCommand(bulk_insert);
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
    }
}