using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PropertyPlus.Controllers;

namespace PropertyPlus.Helper
{
    public class LoginActionFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var admin = Encrypt.GetAdminBySession(filterContext.HttpContext.Request.Cookies["PPAdmin"]);
            var control = (AdminController)filterContext.Controller;
            if (!Equals(control, null))
            {
                if (Equals(admin, null))
                {
                    control.HttpContext.Response.Redirect("./Login");
                }
            }
            base.OnActionExecuting(filterContext);
        }

    }
}