using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Web.Mvc;

namespace vnpost_ocr_system.SupportClass
{
    public class Auther : ActionFilterAttribute, IAuthorizationFilter
    {
        public string Roles { get; set; }

        public void OnAuthorization(AuthorizationContext filterContext)
        {
            if (string.IsNullOrEmpty(Convert.ToString(filterContext.HttpContext.Session["Role"])))
            {
                var Url = new UrlHelper(filterContext.RequestContext);
                var url = Url.Action("Index", "Home");
                filterContext.Result = new RedirectResult(url);
            }
            else
            {
                string role = (string)filterContext.HttpContext.Session["Role"];
                bool Check = false;
                foreach (var r in Roles.Split(','))
                {
                    if (role.Contains(r))
                    {
                        Check = true;
                        break;
                    }
                }
                if (!Check)
                {
                    string url = (string)filterContext.HttpContext.Session["url"];
                    filterContext.Result = new RedirectResult(url);
                }
            }
        }
    }
}