using GamingZone.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace GamingZone.Infrastructure
{
    public class CustomAuthorizeAttribute : AuthorizeAttribute
    {
        private readonly string[] allowedroles;
        GamingZoneEntities db = new GamingZoneEntities();
        public CustomAuthorizeAttribute(params string[] roles)
        {
            this.allowedroles = roles;
        }
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            bool authorize = false;
            var userId = Convert.ToString(httpContext.Session["UserId"]);
            if (!string.IsNullOrEmpty(userId))
            {
                int id = Convert.ToInt32(userId);
                var userRole = db.Users.Where(m => m.Id == id).FirstOrDefault();
                foreach (var role in allowedroles)
                {
                    if (role == userRole.Role) return true;
                }
            }
           

            return authorize;
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            filterContext.Result = new RedirectToRouteResult(
               new RouteValueDictionary
               {
                    { "controller", "Account" },
                    { "action", "UnAuthorized" }
               });
        }
    }
}