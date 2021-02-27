using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.Web.Mvc;
using Model.EF;

namespace OnlineShop.Middleware
{
    public class VerifyAdminAttribute : AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            var session = HttpContext.Current.Session["USER_SESSION"];
            //var session = (UserLogin)HttpContext.Current.Session[Common.CommonConstant.USER_SESSION];
            if (session == null)
            {
                return false;
            }
            return true;
        }
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            filterContext.Result = new RedirectResult("/Admin/AdminLogin/Index");
        }
    }
}