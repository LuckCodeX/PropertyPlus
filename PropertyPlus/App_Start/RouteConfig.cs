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
                name: "BlogDetail",
                url: "blog-detail/{id}/{title}",
                defaults: new { controller = "Home", action = "BlogDetail", id = UrlParameter.Optional, title = UrlParameter.Optional }
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
                name: "HostDashboard",
                url: "host/dashboard",
                defaults: new { controller = "Home", action = "HostDashboard" }
            );

            routes.MapRoute(
                name: "HostListing",
                url: "host/listing",
                defaults: new { controller = "Home", action = "HostDashboard" }
            );

            routes.MapRoute(
                name: "HostCreateStep11",
                url: "host/create/step-1-1",
                defaults: new { controller = "Home", action = "HostCreateStep11" }
            );

            routes.MapRoute(
                name: "HostCreateStep12",
                url: "host/create/step-1-2",
                defaults: new { controller = "Home", action = "HostCreateStep12" }
            );

            routes.MapRoute(
                name: "HostCreateStep13",
                url: "host/create/step-1-3",
                defaults: new { controller = "Home", action = "HostCreateStep13" }
            );

            routes.MapRoute(
                name: "HostCreateStep21",
                url: "host/create/step-2",
                defaults: new { controller = "Home", action = "HostCreateStep21" }
            );

            //routes.MapRoute(
            //    name: "HostCreateStep22",
            //    url: "host/create/step-2-2",
            //    defaults: new { controller = "Home", action = "HostCreateStep22" }
            //);

            routes.MapRoute(
                name: "HostCreateStep31",
                url: "host/create/step-3",
                defaults: new { controller = "Home", action = "HostCreateStep31" }
            );

            //routes.MapRoute(
            //    name: "HostCreateStep32",
            //    url: "host/create/step-3-2",
            //    defaults: new { controller = "Home", action = "HostCreateStep32" }
            //);

            routes.MapRoute(
                name: "HostCreateStep4",
                url: "host/create/step-4",
                defaults: new { controller = "Home", action = "HostCreateStep4" }
            );

            routes.MapRoute(
                name: "HostCreateFinish",
                url: "host/create/finish",
                defaults: new { controller = "Home", action = "HostCreateFinish" }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
