using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Common;

namespace OnlineShop
{ 
    public class HasCredentialAttribute:AuthorizeAttribute
    {
        public string RoleID { get; set; }
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            var session = (UserLogin)HttpContext.Current.Session[Common.CommonConstant.USER_SESSION];
            if(session==null)
            {
                return false;
            }
            List<string> prive = this.GetCredentialByLoggedInUser(session.Username);
            if(prive.Contains(this.RoleID)|| session.GroupID==CommonConstant.ADMIN_GROUP)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            filterContext.Result = new ViewResult
            {
                ViewName = "~/Areas/Admin/Views/Shared/View401.cshtml"
            };
        }
        private List<string> GetCredentialByLoggedInUser(string username)
        {
            var credentials = (List<string>)HttpContext.Current.Session[Common.CommonConstant.SESSION_CREDENTIALS];
            return credentials;
        }
    }
}