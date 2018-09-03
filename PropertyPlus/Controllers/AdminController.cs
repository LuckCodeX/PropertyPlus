using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using PropertyPlus.Helper;
using PropertyPlus.Models;
using PropertyPlus.Services;

namespace PropertyPlus.Controllers
{
    public class AdminController : Controller
    {
        private IService _service = new Service();
        // GET: Admin

        public ActionResult Login()
        {
            //admin admin = new admin()
            //{
            //    username = "propertyplusadmin",
            //    password = Encrypt.EncodePassword("123456")
            //};
            //_service.AdminRepository.Insert(admin);
            Response.Cookies["PPAdmin"].Value = "";
            var model = new AdminModel();
            return View(model);
        }

        [HttpPost]
        public ActionResult Login(AdminModel model)
        {
            if (ModelState.IsValid)
            {
                
                var admin = _service.LoginAdmin(model);
                if (Equals(admin, null))
                {
                    ViewBag.ErrorText = "Tài khoản hoặc mật khẩu không chính xác";
                    return View(model);
                }
                var token = new TokenModel()
                {
                    Id = admin.admin_id,
                    Username = admin.username,
                    Role = admin.role
                };
                if (Equals(Request.Cookies["PPAdmin"], null) ||
                    string.IsNullOrEmpty(Request.Cookies["PPAdmin"].Value))
                {
                    HttpCookie cookie = new HttpCookie("PPAdmin");
                    cookie.Value = Encrypt.Base64Encode(JsonConvert.SerializeObject(token));
                    cookie.Expires = DateTime.Now.AddDays(30);
                    Response.SetCookie(cookie);
                }
                return RedirectToAction("Blog");

            }
            return View(model);
        }

        public ActionResult Dashboard()
        {
            return View();
        }

        [LoginActionFilter]
        public ActionResult Blog(int? page)
        {
            int curPage = page ?? 1;
            var blogs = _service.GetAllBlog();
            //var blogList
            return View();
        }

        protected override void Dispose(bool disposing)
        {
            _service.Dispose();
            base.Dispose(disposing);
        }
    }
}