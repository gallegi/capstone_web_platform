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
                        var dbToken = db.AuthenticationTokens.Where(x => x.Token.Equals(AuthCookie.Value) && x.Status.Equals(true)).FirstOrDefault();
                        if (dbToken != null)
                        {
                            Session["userID"] = dbToken.CustomerID;
                            Session["Role"] = "0";
                            Session["userName"] = db.Customers.Where(x => x.CustomerID.Equals(dbToken.CustomerID)).FirstOrDefault().FullName;
                        }
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