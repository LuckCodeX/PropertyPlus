using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PropertyPlus.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";

            return View();
        }

        public ActionResult Apartment()
        {
            ViewBag.Title = "Apartment";

            return View();
        }

        public ActionResult ApartmentDetail()
        {
            ViewBag.Title = "Apartment Detail";

            return View();
        }

        public ActionResult UserProfileGeneral()
        {
            return View();
        }

        public ActionResult UserProfileManage()
        {
            return View();
        }

        public ActionResult UserProfileEdit()
        {
            return View();
        }

        public ActionResult UserProfileSetting()
        {
            return View();
        }

        public ActionResult UserProfileRefer()
        {
            return View();
        }

        public ActionResult Blog()
        {
            ViewBag.Title = "Blog";

            return View();
        }

        public ActionResult Project()
        {
            ViewBag.Title = "Project";

            return View();
        }
    }
}
