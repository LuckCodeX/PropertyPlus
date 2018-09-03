using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using PagedList;
using PropertyPlus.Helper;
using PropertyPlus.Models;
using PropertyPlus.Services;

namespace PropertyPlus.Controllers
{
    public class AdminController : Controller
    {
        private IService _service = new Service();

        [HttpPost]
        public ActionResult UploadImage(HttpPostedFileBase upload, string CKEditorFuncNum, string CKEditor, string langCode)
        {
            string vImagePath = String.Empty;
            string vMessage = String.Empty;
            string vFilePath = String.Empty;
            string vOutput = String.Empty;
            try
            {
                if (upload != null && upload.ContentLength > 0)
                {
                    var vFileName = DateTime.Now.ToString("yyyyMMdd-HHMMssff") + " - " + Path.GetFileName(upload.FileName);
                    var vFolderPath = Server.MapPath("/Upload/");
                    if (!Directory.Exists(vFolderPath))
                    {
                        Directory.CreateDirectory(vFolderPath);
                    }
                    vFilePath = Path.Combine(vFolderPath, vFileName);
                    upload.SaveAs(vFilePath);
                    vImagePath = string.Format("{0}://{1}{2}",
                        Request.Url.Scheme,
                        Request.Url.Authority,
                        Url.Content("/Upload/" + vFileName));
                    vMessage = "Tải ảnh thành công !!!";
                }
            }
            catch (Exception e)
            {
                vMessage = "There was an issue uploading:" + e.Message;
            }
            vOutput = string.Format("<script type='text/javascript'>window.parent.CKEDITOR.tools.callFunction({0}, \"{1}\", \"{2}\");</script>",
                CKEditorFuncNum,
                vImagePath,
                vMessage);
            return Content(vOutput, "text/html");
        }


        public ActionResult Login()
        {
            //admin admin = new admin()
            //{
            //    username = "propertyplusadmin",
            //    password = Encrypt.EncodePassword("123456")
            //};
            //_service.AdminRepository.Save(admin);
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

        public ActionResult ChangePassword()
        {
            return View();
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
            var blogList = blogs.Select(p => new BlogModel()
            {
                Id = p.blog_id,
                CreatedString = ConvertDatetime.ConvertUnixTimeStampToDateTime(p.created_date),
                Img = p.img,
                Type = p.type,
                Content = _service.ConvertBlogContentToModel(p.blog_content.FirstOrDefault(q => q.language == 0))
            });
            return View(blogList.ToPagedList(curPage, 10));
        }

        [LoginActionFilter]
        public ActionResult BlogDetail(int? id)
        {
            BlogModel model;
            if (!Equals(id, null))
            {
                var blog = _service.GetBlogById(id.Value);
                var blogContent = blog.blog_content.Select(p => new BlogContentModel()
                {
                    Id = p.blog_content_id,
                    Content = p.content,
                    Title = p.title,
                    Description = p.description
                }).ToList();
                model = new BlogModel()
                {
                    Id = blog.blog_id,
                    Img = blog.img,
                    ContentList = blogContent
                };
            }
            else
            {
                var blogContent = new List<BlogContentModel>();
                for (int i = 0; i < 3; i++)
                {
                    var content = new BlogContentModel()
                    {
                        Id = 0,
                        Language = i
                    };
                    blogContent.Add(content);
                }
                model = new BlogModel()
                {
                    Id = 0,
                    ContentList = blogContent
                };
            }
            return View(model);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult BlogDetail(BlogModel model)
        {
            var blog = _service.GetBlogById(model.Id);
            if (Equals(blog, null))
            {
                blog = new blog()
                {
                    blog_id = 0,
                    created_date = ConvertDatetime.GetCurrentUnixTimeStamp()
                };
            }
            string fileName = null;
            if (!Equals(model.ImageFile, null))
            {
                fileName = "Blog_" + ConvertDatetime.GetCurrentUnixTimeStamp() + Path.GetExtension(model.ImageFile.FileName);
                string path = Path.Combine(Server.MapPath("~/Upload"), fileName);
                model.ImageFile.SaveAs(path);
            }
            blog.img = fileName;
            blog.type = model.Type;
            _service.SaveBlog(blog);

            int idx = 0;
            foreach (var blogContent in model.ContentList)
            {
                var content = _service.GetBlogContentById(blogContent.Id);
                if (Equals(content, null))
                {
                    content = new blog_content()
                    {
                        blog_content_id = 0,
                        blog_id = blog.blog_id,
                        language = idx
                    };
                }
                content.title = blogContent.Title;
                content.description = blogContent.Description;
                content.content = blogContent.Content;
                _service.SaveBlogContent(content);
                idx++;
            }
            return RedirectToAction("Blog");
        }

        public ActionResult DeleteBlog(int id)
        {
            _service.DeleteBlog(id);
            return RedirectToAction("Blog");
        }

        protected override void Dispose(bool disposing)
        {
            _service.Dispose();
            base.Dispose(disposing);
        }
    }
}