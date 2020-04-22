using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using vnpost_ocr_system.Models;

namespace vnpost_ocr_system.Controllers.CustomController
{
    public class BaseUserController : Controller
    {
        private void InstantiateSession()
        {
            if (Session["userID"] == null)
            {
                HttpCookie AuthCookie = Request.Cookies["VNPostORCAuthToken"];
                if (AuthCookie != null)
                {
                    using (VNPOST_AppointmentEntities db = new VNPOST_AppointmentEntities())
                    {
                        DateTime dateTimeNow = DateTime.Now;
                        AuthenticationToken dbToken = db.AuthenticationTokens.Where(x => x.AuthToken.Equals(AuthCookie.Value) && x.Status.Equals(true) && x.ExpireDate >= dateTimeNow).FirstOrDefault();
                        if (dbToken != null)
                        {
                            Session["userID"] = dbToken.CustomerID;
                            Session["Role"] = "0";
                            Session["userName"] = db.Customers.Where(x => x.CustomerID.Equals(dbToken.CustomerID)).FirstOrDefault().FullName;

                            // Refesh ExpriesDate
                            dbToken.ExpireDate = DateTime.Now.AddHours(12);
                            db.Entry(dbToken).State = System.Data.Entity.EntityState.Modified;
                            db.SaveChanges();
                        }
                        else
                            AuthCookie.Expires = DateTime.Now.AddDays(-1d);
                    }
                }
            }
        }
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            InstantiateSession();
            base.OnActionExecuting(filterContext);
        }

        protected override void OnAuthorization(AuthorizationContext filterContext)
        {
            InstantiateSession();
            base.OnAuthorization(filterContext);
        }
               
    }
}