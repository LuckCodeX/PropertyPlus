using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace PropertyPlus
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Blog",
                url: "blog",
                defaults: new { controller = "Home", action = "Blog" }
            );

            routes.MapRoute(
                name: "Apartment",
                url: "apartment",
                defaults: new { controller = "Home", action = "Apartment" }
            );

            routes.MapRoute(
                name: "ApartmentDetail",
                url: "apartment-detail/{id}/{title}",
                defaults: new { controller = "Home", action = "ApartmentDetail", id = UrlParameter.Optional, title = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Project",
                url: "project",
                defaults: new { controller = "Home", action = "Project" }
            );

            routes.MapRoute(
                name: "UserProfileGeneral",
                url: "user-profile/general",
                defaults: new { controller = "Home", action = "UserProfileGeneral" }
            );

            routes.MapRoute(
                name: "UserProfileManage",
                url: "user-profile/manage",
                defaults: new { controller = "Home", action = "UserProfileManage" }
            );

            routes.MapRoute(
                name: "UserProfileEdit",
                url: "user-profile/edit",
                defaults: new { controller = "Home", action = "UserProfileEdit" }
            );

            routes.MapRoute(
                name: "UserProfileSetting",
                url: "user-profile/setting",
                defaults: new { controller = "Home", action = "UserProfileSetting" }
            );

            routes.MapRoute(
                name: "UserProfileRefer",
                url: "user-profile/refer",
                defaults: new { controller = "Home", action = "UserProfileRefer" }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
