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
        public ActionResult Account(int? page, string search)
        {
            var curPage = page ?? 1;
            var accounts = _service.SearchUserProfile(search);
            var accList = accounts.Select(p => new UserProfileModel()
            {
                FirstName = p.first_name,
                LastName = p.last_name,
                Email = p.email,
                Id = p.user_profile_id,
                CreatedString = ConvertDatetime.ConvertUnixTimeStampToDateTime(p.created_date)
            });
            return View(accList.ToPagedList(curPage, 10));
        }

        [LoginActionFilter]
        public ActionResult AccountDetail(int id)
        {
            var userProfile = _service.GetActiveUserProfileById(id);
            var model = new UserProfileModel()
            {
                Id = userProfile.user_profile_id,
                FirstName = userProfile.first_name,
                LastName = userProfile.last_name,
                Email = userProfile.email
            };
            return View(model);
        }

        [HttpPost]
        public ActionResult AccountDetail(UserProfileModel model)
        {
            if (!Equals(model.Password, model.ConfirmPassword))
            {
                ModelState.AddModelError("ConfirmPassword", "Xác nhận mật khẩu không chính xác");
                return View(model);
            }

            var userProfile = _service.GetActiveUserProfileById(model.Id);
            userProfile.first_name = model.FirstName;
            userProfile.last_name = model.LastName;
            _service.SaveUserProfile(userProfile);

            if (Equals(model.Password, null))
            {
                var userAccount = _service.GetUserAccountByUserProfileId(userProfile.user_profile_id);
                userAccount.password = Encrypt.EncodePassword(model.Password);
                _service.SaveUserAccount(userAccount);
            }

            return RedirectToAction("Account");
        }

        public ActionResult DeleteAccount(int id)
        {
            _service.DeleteAccount(id);
            return RedirectToAction("Account");
        }

        [LoginActionFilter]
        public ActionResult Blog(int? page, int? type, string search)
        {
            int curPage = page ?? 1;
            int curType = type ?? -1;
            var blogs = _service.SearchBlogList(curType, 0, search);
            var blogList = blogs.Select(p => new BlogModel()
            {
                Id = p.blog_id,
                CreatedString = ConvertDatetime.ConvertUnixTimeStampToDateTime(p.created_date),
                Img = p.img,
                Type = p.type,
                Content = _service.ConvertBlogContentToModel(p.blog_content.FirstOrDefault(q => q.language == 0))
            });
            ViewBag.TypeSearch = curType;
            ViewBag.KeySearch = search;
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

        [LoginActionFilter]
        public ActionResult SlideHome()
        {
            var slides = _service.GetListSlideByType(0).Select(p => new SlideModel()
            {
                Id = p.slide_id,
                Img = p.img,
                Url = p.url,
                Type = p.type
            }).ToList();

            ViewBag.SlideIntro = _service.GetListSlideByType(3).Select(p => new SlideModel()
            {
                Id = p.slide_id,
                Img = p.img,
                Url = p.url,
                Type = p.type
            }).FirstOrDefault();

            ViewBag.SlideService = _service.GetListSlideByType(4).Select(p => new SlideModel()
            {
                Id = p.slide_id,
                Img = p.img,
                Url = p.url,
                Type = p.type
            }).FirstOrDefault();

            return View(slides);
        }

        [LoginActionFilter]
        public ActionResult SlideProject()
        {
            var slides = _service.GetListSlideByType(1).Select(p => new SlideModel()
            {
                Id = p.slide_id,
                Img = p.img,
                Url = p.url,
                Type = p.type
            }).ToList();
            return View(slides);
        }

        [LoginActionFilter]
        public ActionResult SlideBlog()
        {
            var slides = _service.GetListSlideByType(2).Select(p => new SlideModel()
            {
                Id = p.slide_id,
                Img = p.img,
                Url = p.url,
                Type = p.type
            }).ToList();
            return View(slides);
        }

        [HttpPost]
        public ActionResult SaveSlide(List<SlideModel> list)
        {
            var type = 0;
            foreach (var model in list)
            {
                type = model.Type;
                var slide = _service.GetSlideById(model.Id);
                if (!Equals(model.ImageFile, null))
                {
                    var fileName = "Slide_" + slide.slide_id + "_" + ConvertDatetime.GetCurrentUnixTimeStamp() + Path.GetExtension(model.ImageFile.FileName);
                    string path = Path.Combine(Server.MapPath("~/Upload"), fileName);
                    model.ImageFile.SaveAs(path);
                    slide.img = fileName;
                }
                slide.url = model.Url;
                _service.SaveSlide(slide);
            }
            if (type == 1)
                return RedirectToAction("SlideProject");
            if (type == 2)
                return RedirectToAction("SlideBlog");
            return RedirectToAction("SlideHome");
        }

        [HttpPost]
        public ActionResult SaveSingleSlide(SlideModel model)
        {
            var slide = _service.GetSlideById(model.Id);
            if (!Equals(model.ImageFile, null))
            {
                var fileName = "Slide_" + slide.slide_id + "_" + ConvertDatetime.GetCurrentUnixTimeStamp() + Path.GetExtension(model.ImageFile.FileName);
                string path = Path.Combine(Server.MapPath("~/Upload"), fileName);
                model.ImageFile.SaveAs(path);
                slide.img = fileName;
            }
            //slide.url = model.Url;
            _service.SaveSlide(slide);
            return RedirectToAction("SlideHome");
        }

        protected override void Dispose(bool disposing)
        {
            _service.Dispose();
            base.Dispose(disposing);
        }


    }
}