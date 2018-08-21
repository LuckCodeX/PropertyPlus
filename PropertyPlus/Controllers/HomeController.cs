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
