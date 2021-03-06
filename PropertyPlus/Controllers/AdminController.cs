﻿using System;
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
                Type = p.type,
                Content = _service.ConvertProjectContentToModel(p.project_content.FirstOrDefault(q => q.language == 0))
            });
            ViewBag.KeySearch = search;
            return View(projectList.ToPagedList(curPage, 10));
        }

        [LoginActionFilter]
        public ActionResult UpcomingProject(int? page)
        {
            int curPage = page ?? 1;
            var projects = _service.GetUpcomingProjectList();
            var projectList = projects.Select(p => new ProjectModel()
            {
                Id = p.project_id,
                Img = p.img,
                Type = p.type,
                Content = _service.ConvertProjectContentToModel(p.project_content.FirstOrDefault(q => q.language == 0))
            });
            return View(projectList.ToPagedList(curPage, 10));
        }

        [LoginActionFilter]
        public ActionResult ProjectDetail(int? id)
        {
            ProjectModel model;
            var facilities = _service.GetAllFacilities();
            if (!Equals(id, null))
            {
                var project = _service.GetProjectById(id.Value);
                var projectContent = project.project_content.Select(p => new ProjectContentModel()
                {
                    Id = p.project_content_id,
                    Name = p.name,
                    Description = p.description
                }).ToList();

                var projectOverview = new List<ProjectOverviewModel>();
                if (project.project_overview.Count > 0)
                {
                    var overviewContent = project.project_overview.Where(p => p.language == 0).Select(p => new ProjectOverviewContentModel()
                    {
                        Id = p.project_overview_id,
                        Content = p.content
                    }).ToList();
                    projectOverview.Add(new ProjectOverviewModel()
                    {
                        ContentList = overviewContent
                    });
                    overviewContent = project.project_overview.Where(p => p.language == 1).Select(p => new ProjectOverviewContentModel()
                    {
                        Id = p.project_overview_id,
                        Content = p.content
                    }).ToList();
                    projectOverview.Add(new ProjectOverviewModel()
                    {
                        ContentList = overviewContent
                    });
                    overviewContent = project.project_overview.Where(p => p.language == 2).Select(p => new ProjectOverviewContentModel()
                    {
                        Id = p.project_overview_id,
                        Content = p.content
                    }).ToList();
                    projectOverview.Add(new ProjectOverviewModel()
                    {
                        ContentList = overviewContent
                    });
                }
                else
                {
                    for (int i = 0; i < 3; i++)
                    {
                        var contentList = new List<ProjectOverviewContentModel>();
                        for (int j = 0; j < 9; j++)
                        {
                            var c = new ProjectOverviewContentModel() { Id = 0, Language = i };
                            contentList.Add(c);
                        }
                        var content = new ProjectOverviewModel()
                        {
                            ContentList = contentList
                        };
                        projectOverview.Add(content);
                    }
                }

                var facilityList = new List<FacilityModel>();
                foreach (var item in facilities)
                {
                    var flag = false;
                    foreach (var fac in project.project_facility)
                    {
                        if (item.facility_id == fac.facility_id)
                        {
                            flag = true;
                            break;
                        }
                    }
                    var check = new FacilityModel()
                    {
                        Id = item.facility_id,
                        Content = _service.ConvertFacilityContentToModel(item.facility_content.FirstOrDefault(p => p.language == 0)),
                        Selected = flag
                    };
                    facilityList.Add(check);
                }

                //var facilityList = project.project_facility.Select(p => new FacilityModel()
                //{
                //    Id = p.facility_id,
                //    Content = _service.ConvertFacilityContentToModel(p.facility.facility_content.FirstOrDefault(q => q.language == 0))
                //}).ToList();
                model = new ProjectModel()
                {
                    Id = project.project_id,
                    Img = project.img,
                    Type = project.type,
                    Slide1 = project.slide_1,
                    Slide2 = project.slide_2,
                    Slide3 = project.slide_3,
                    ContentList = projectContent,
                    OverviewList = projectOverview,
                    FacilityList = facilityList,
                    Logo = project.logo,
                    Address = project.address,
                    Latitude = project.latitude,
                    Longitude = project.longitude
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

                var projectOverview = new List<ProjectOverviewModel>();
                for (int i = 0; i < 3; i++)
                {
                    var contentList = new List<ProjectOverviewContentModel>();
                    for (int j = 0; j < 9; j++)
                    {
                        var c = new ProjectOverviewContentModel() { Id = 0, Language = i };
                        contentList.Add(c);
                    }
                    var content = new ProjectOverviewModel()
                    {
                        ContentList = contentList
                    };
                    projectOverview.Add(content);
                }
                model = new ProjectModel()
                {
                    Id = 0,
                    ContentList = projectContent,
                    OverviewList = projectOverview,
                    Type = 0,
                    FacilityList = facilities.Select(p => new FacilityModel()
                    {
                        Id = p.facility_id,
                        Content = _service.ConvertFacilityContentToModel(p.facility_content.FirstOrDefault(q => q.language == 0)),
                        Selected = false

                    }).ToList()
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

                project.type = model.Type;
                project.address = model.Address;
                project.latitude = model.Latitude;
                project.longitude = model.Longitude;
                if (!Equals(model.ImageFile, null))
                {
                    string fileName = "Project_" + ConvertDatetime.GetCurrentUnixTimeStamp() + Path.GetExtension(model.ImageFile.FileName);
                    string path = Path.Combine(Server.MapPath("~/Upload"), fileName);
                    model.ImageFile.SaveAs(path);
                    project.img = fileName;
                }
                if (!Equals(model.Slide1File, null))
                {
                    string fileName = "Project_Slide1_" + ConvertDatetime.GetCurrentUnixTimeStamp() + Path.GetExtension(model.Slide1File.FileName);
                    string path = Path.Combine(Server.MapPath("~/Upload"), fileName);
                    model.Slide1File.SaveAs(path);
                    project.slide_1 = fileName;
                }
                if (!Equals(model.Slide2File, null))
                {
                    string fileName = "Project_Slide2_" + ConvertDatetime.GetCurrentUnixTimeStamp() + Path.GetExtension(model.Slide2File.FileName);
                    string path = Path.Combine(Server.MapPath("~/Upload"), fileName);
                    model.Slide2File.SaveAs(path);
                    project.slide_2 = fileName;
                }
                if (!Equals(model.Slide3File, null))
                {
                    string fileName = "Project_Slide3_" + ConvertDatetime.GetCurrentUnixTimeStamp() + Path.GetExtension(model.Slide3File.FileName);
                    string path = Path.Combine(Server.MapPath("~/Upload"), fileName);
                    model.Slide3File.SaveAs(path);
                    project.slide_3 = fileName;
                }
                if (!Equals(model.LogoFile, null))
                {
                    string fileName = "Project_Logo_" + ConvertDatetime.GetCurrentUnixTimeStamp() + Path.GetExtension(model.LogoFile.FileName);
                    string path = Path.Combine(Server.MapPath("~/Upload"), fileName);
                    model.LogoFile.SaveAs(path);
                    project.logo = fileName;
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
                    content.description = projectContent.Description;
                    _service.SaveProjectContent(content);
                    idx++;
                }

                foreach (var overview in model.OverviewList)
                {
                    foreach (var overviewContent in overview.ContentList)
                    {
                        var content = _service.GetProjectOverviewById(overviewContent.Id);
                        if (Equals(content, null))
                        {
                            content = new project_overview()
                            {
                                project_overview_id = 0,
                                project_id = project.project_id,
                                language = overviewContent.Language
                            };
                        }

                        content.content = overviewContent.Content;
                        _service.SaveProjectOverview(content);
                    }
                }

                _service.DeleteAllProjectFacilities(project.project_facility.ToList());
                foreach (var item in model.FacilityList)
                {
                    if (item.Selected)
                    {
                        var fac = new project_facility()
                        {
                            project_facility_id = 0,
                            project_id = project.project_id,
                            facility_id = item.Id
                        };
                        _service.SaveProjectFacility(fac);
                    }
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
        public ActionResult Career(int? page, string search)
        {
            int curPage = page ?? 1;
            var careers = _service.SearchCareerList(search);
            var careerList = careers.Select(p => new CareerModel()
            {
                Id = p.career_id,
                Type = p.type,
                CategoryId = p.category_id,
                Content = _service.ConvertCareerContentToModel(p.career_content.FirstOrDefault(q => q.language == 0))
            });
            ViewBag.KeySearch = search;
            return View(careerList.ToPagedList(curPage, 10));
        }

        [LoginActionFilter]
        public ActionResult CareerDetail(int? id)
        {
            CareerModel model;
            if (!Equals(id, null))
            {
                var career = _service.GetCareerById(id.Value);
                var careerContent = career.career_content.Select(p => new CareerContentModel()
                {
                    Id = p.career_content_id,
                    Content = p.content,
                    Title = p.title,
                }).ToList();
                model = new CareerModel()
                {
                    Id = career.career_id,
                    ContentList = careerContent,
                    City = career.city,
                    CategoryId = career.category_id,
                    Type = career.type,
                    SalaryMin = career.salary_min,
                    ExpiredDate = career.experied_date,
                    Location = career.location
                };
            }
            else
            {
                var careerContent = new List<CareerContentModel>();
                for (int i = 0; i < 3; i++)
                {
                    var content = new CareerContentModel()
                    {
                        Id = 0,
                        Language = i
                    };
                    careerContent.Add(content);
                }
                model = new CareerModel()
                {
                    Id = 0,
                    ContentList = careerContent
                };
            }
            return View(model);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult CareerDetail(CareerModel model)
        {
            using (var scope = new TransactionScope())
            {
                var career = _service.GetCareerById(model.Id);
                if (Equals(career, null))
                {
                    career = new career()
                    {
                        career_id = 0,
                        created_date = ConvertDatetime.GetCurrentUnixTimeStamp()
                    };
                }
                career.type = model.Type;
                career.city = model.City;
                career.category_id = model.CategoryId;
                career.salary_min = model.SalaryMin;
                career.location = model.Location;
                career.experied_date = model.ExpiredDate;
                _service.SaveCareer(career);

                int idx = 0;
                foreach (var careerContent in model.ContentList)
                {
                    var content = _service.GetCareerContentById(careerContent.Id);
                    if (Equals(content, null))
                    {
                        content = new career_content()
                        {
                            career_content_id = 0,
                            career_id = career.career_id,
                            language = idx
                        };
                    }
                    content.title = careerContent.Title;
                    content.content = careerContent.Content;
                    _service.SaveCareerContent(content);
                    idx++;
                }

                scope.Complete();
            }
            return RedirectToAction("Career");
        }

        public ActionResult DeleteCareer(int id)
        {
            _service.DeleteCareer(id);
            return RedirectToAction("Career");
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