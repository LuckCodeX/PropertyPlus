using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Transactions;
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

                    vOutput = "{\"uploaded\": 1,\"fileName\": \"" + vFileName + "\",\"url\": \"" + ("/Upload/" + vFileName) + "\"}";
                    return Content(vOutput, "text/html");
                }
            }
            catch (Exception e)
            {
                vMessage = "There was an issue uploading:" + e.Message;
            }
            //vOutput = string.Format("<script type='text/javascript'>window.parent.CKEDITOR.tools.callFunction({0}, \"{1}\", \"{2}\");</script>",
            //    CKEditorFuncNum,
            //    vImagePath,
            //    vMessage);
            //return Content(vOutput, "text/html");
            return null;
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
                FullName = p.full_name,
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
                FullName = userProfile.full_name,
                Email = userProfile.email
            };
            return View(model);
        }

        [HttpPost]
        public ActionResult AccountDetail(UserProfileModel model)
        {
            using (var scope = new TransactionScope())
            {
                if (!Equals(model.Password, model.ConfirmPassword))
                {
                    ModelState.AddModelError("ConfirmPassword", "Xác nhận mật khẩu không chính xác");
                    return View(model);
                }

                var userProfile = _service.GetActiveUserProfileById(model.Id);
                userProfile.full_name = model.FullName;
                _service.SaveUserProfile(userProfile);

                if (Equals(model.Password, null))
                {
                    var userAccount = _service.GetUserAccountByUserProfileId(userProfile.user_profile_id);
                    userAccount.password = Encrypt.EncodePassword(model.Password);
                    _service.SaveUserAccount(userAccount);
                }

                scope.Complete();
            }

            return RedirectToAction("Account");
        }

        public ActionResult DeleteAccount(int id)
        {
            _service.DeleteAccount(id);
            return RedirectToAction("Account");
        }

        [LoginActionFilter]
        public ActionResult Project(int? page, string search)
        {
            int curPage = page ?? 1;
            var projects = _service.SearchProjectList(search);
            var projectList = projects.Select(p => new ProjectModel()
            {
                Id = p.project_id,
                Img = p.img,
                Content = _service.ConvertProjectContentToModel(p.project_content.FirstOrDefault(q => q.language == 0))
            });
            ViewBag.KeySearch = search;
            return View(projectList.ToPagedList(curPage, 10));
        }

        [LoginActionFilter]
        public ActionResult ProjectDetail(int? id)
        {
            ProjectModel model;
            if (!Equals(id, null))
            {
                var project = _service.GetProjectById(id.Value);
                var projectContent = project.project_content.Select(p => new ProjectContentModel()
                {
                    Id = p.project_content_id,
                    Name = p.name
                }).ToList();
                model = new ProjectModel()
                {
                    Id = project.project_id,
                    Img = project.img,
                    ContentList = projectContent
                };
            }
            else
            {
                var projectContent = new List<ProjectContentModel>();
                for (int i = 0; i < 3; i++)
                {
                    var content = new ProjectContentModel()
                    {
                        Id = 0,
                        Language = i
                    };
                    projectContent.Add(content);
                }
                model = new ProjectModel()
                {
                    Id = 0,
                    ContentList = projectContent
                };
            }

            return View(model);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult ProjectDetail(ProjectModel model)
        {
            using (var scope = new TransactionScope())
            {
                var project = _service.GetProjectById(model.Id);
                if (Equals(project, null))
                {
                    project = new project()
                    {
                        project_id = 0,
                        status = 1
                    };
                }
                if (!Equals(model.ImageFile, null))
                {
                    string fileName = "Project_" + ConvertDatetime.GetCurrentUnixTimeStamp() + Path.GetExtension(model.ImageFile.FileName);
                    string path = Path.Combine(Server.MapPath("~/Upload"), fileName);
                    model.ImageFile.SaveAs(path);
                    project.img = fileName;
                }
                _service.SaveProject(project);

                int idx = 0;
                foreach (var projectContent in model.ContentList)
                {
                    var content = _service.GetProjectContentById(projectContent.Id);
                    if (Equals(content, null))
                    {
                        content = new project_content()
                        {
                            project_content_id = 0,
                            project_id = project.project_id,
                            language = idx
                        };
                    }
                    content.name = projectContent.Name;
                    _service.SaveProjectContent(content);
                    idx++;
                }

                scope.Complete();
            }

            return RedirectToAction("Project");
        }

        public ActionResult DeleteProject(int id)
        {
            var project = _service.GetProjectById(id);
            project.status = 2;
            _service.SaveProject(project);
            return RedirectToAction("Project");
        }

        [LoginActionFilter]
        public ActionResult Facility()
        {
            var facilities = _service.GetAllFacility().Select(p => new FacilityModel()
            {
                Id = p.facility_id,
                Img = p.img,
                Content = _service.ConvertFacilityContentToModel(p.facility_content.FirstOrDefault(q => q.language == 0))
            }).ToList();
            return View(facilities);
        }

        [LoginActionFilter]
        public ActionResult FacilityDetail(int? id)
        {
            FacilityModel model;
            if (!Equals(id, null))
            {
                var facility = _service.GetFacilityById(id.Value);
                var facilityContent = facility.facility_content.Select(p => new FacilityContentModel()
                {
                    Id = p.facility_content_id,
                    Name = p.name
                }).ToList();
                model = new FacilityModel()
                {
                    Id = facility.facility_id,
                    Img = facility.img,
                    ContentList = facilityContent
                };
            }
            else
            {
                var facilityContent = new List<FacilityContentModel>();
                for (int i = 0; i < 3; i++)
                {
                    var content = new FacilityContentModel()
                    {
                        Id = 0,
                        Language = i
                    };
                    facilityContent.Add(content);
                }
                model = new FacilityModel()
                {
                    Id = 0,
                    ContentList = facilityContent
                };
            }

            return View(model);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult FacilityDetail(FacilityModel model)
        {
            using (var scope = new TransactionScope())
            {
                var facility = _service.GetFacilityById(model.Id);
                if (Equals(facility, null))
                {
                    facility = new facility()
                    {
                        facility_id = 0,
                        status = 1
                    };
                }
                if (!Equals(model.ImageFile, null))
                {
                    string fileName = "Facility_" + ConvertDatetime.GetCurrentUnixTimeStamp() + Path.GetExtension(model.ImageFile.FileName);
                    string path = Path.Combine(Server.MapPath("~/Upload"), fileName);
                    model.ImageFile.SaveAs(path);
                    facility.img = fileName;
                }
                _service.SaveFacility(facility);

                int idx = 0;
                foreach (var facilityContent in model.ContentList)
                {
                    var content = _service.GetFacilityContentById(facilityContent.Id);
                    if (Equals(content, null))
                    {
                        content = new facility_content()
                        {
                            facility_content_id = 0,
                            facility_id = facility.facility_id,
                            language = idx
                        };
                    }
                    content.name = facilityContent.Name;
                    _service.SaveFacilityContent(content);
                    idx++;
                }

                scope.Complete();
            }
            return RedirectToAction("Facility");
        }

        public ActionResult DeleteFacility(int id)
        {
            var facility = _service.GetFacilityById(id);
            facility.status = 2;
            _service.SaveFacility(facility);
            return RedirectToAction("Facility");
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
            using (var scope = new TransactionScope())
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
                if (!Equals(model.ImageFile, null))
                {
                    string fileName = "Blog_" + ConvertDatetime.GetCurrentUnixTimeStamp() + Path.GetExtension(model.ImageFile.FileName);
                    string path = Path.Combine(Server.MapPath("~/Upload"), fileName);
                    model.ImageFile.SaveAs(path);
                    blog.img = fileName;
                }
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

                scope.Complete();
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