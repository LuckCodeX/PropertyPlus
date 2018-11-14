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

        public ActionResult BlogDetail()
        {
            return View();
        }

        public ActionResult Project()
        {
            ViewBag.Title = "Project";

            return View();
        }

        public ActionResult ProjectDetail()
        {

            return View();
        }

        public ActionResult HostDashboard()
        {
            return View();
        }
        public ActionResult HostListing()
        {
            return View();
        }

        public ActionResult HostCreateStep11()
        {
            return View();
        }

        public ActionResult HostCreateStep12()
        {
            return View();
        }

        public ActionResult HostCreateStep13()
        {
            return View();
        }

        public ActionResult HostCreateStep21()
        {
            return View();
        }

        public ActionResult HostCreateStep22()
        {
            return View();
        }

        public ActionResult HostCreateStep31()
        {
            return View();
        }

        public ActionResult HostCreateStep32()
        {
            return View();
        }

        public ActionResult HostCreateStep4()
        {
            return View();
        }

        public ActionResult HostCreateFinish()
        {
            return View();
        }

        public ActionResult HostManageOverview()
        {
            return View();
        }

        public ActionResult HostManageProblem()
        {
            return View();
        }

        public ActionResult HostManageDescribe()
        {
            return View();
        }

        public ActionResult HostManageInformation()
        {
            return View();
        }

        public ActionResult HostManagePhoto()
        {
            return View();
        }

        public ActionResult HostManageLocation()
        {
            return View();
        }

        public ActionResult BookMark()
        {
            return View();
        }

        public ActionResult OurService()
        {
            return View();
        }

        public ActionResult OurTechnology()
        {
            return View();
        }

        public ActionResult AboutUs()
        {
            return View();
        }
    }
}
