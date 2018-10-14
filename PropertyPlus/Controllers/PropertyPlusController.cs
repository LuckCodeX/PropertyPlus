﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Transactions;
using System.Web.Http;
using Newtonsoft.Json;
using PropertyPlus.Helper;
using PropertyPlus.Models;
using PropertyPlus.Services;

namespace PropertyPlus.Controllers
{
    [RoutePrefix("api/propertyplus")]
    public class PropertyPlusController : BaseController
    {
        private IService _service = new Service();

        [HttpPost]
        [Route("Register")]
        public UserProfileModel Register(UserProfileModel model)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                var userProfile = _service.GetUserProfileByEmail(model.Email);
                if (!Equals(userProfile, null))
                {
                    ExceptionContent(HttpStatusCode.Unauthorized, "err_email_already_existed");
                }

                userProfile = new user_profile()
                {
                    email = model.Email,
                    full_name = model.FullName,
                    user_profile_id = 0,
                    status = 1,
                    created_date = ConvertDatetime.GetCurrentUnixTimeStamp(),
                    avatar = "noavatar.jpg"
                };
                _service.SaveUserProfile(userProfile);

                var userAcc = new user_account()
                {
                    user_account_id = 0,
                    user_profile_id = userProfile.user_profile_id,
                    email = model.Email,
                    password = Encrypt.EncodePassword(model.Password),
                };
                _service.SaveUserAccount(userAcc);

                var token = new TokenModel()
                {
                    Id = userProfile.user_profile_id,
                    Username = userProfile.email,
                    Role = 0
                };

                scope.Complete();

                return new UserProfileModel()
                {
                    UserId = "UID_" + userProfile.user_profile_id.ToString().PadLeft(5, '0'),
                    FullName = userProfile.full_name,
                    Avatar = userProfile.avatar,
                    Token = Encrypt.Base64Encode(JsonConvert.SerializeObject(token))
                };
            }
        }

        [HttpPost]
        [Route("Login")]
        public UserProfileModel Login(UserAccountModel model)
        {
            var userAcc = _service.Login(model);
            if (Equals(userAcc, null))
            {
                ExceptionContent(HttpStatusCode.InternalServerError, "err_email_or_password_invalid");
            }

            var userProfile = _service.GetActiveUserProfileById(userAcc.user_profile_id);
            var token = new TokenModel()
            {
                Id = userProfile.user_profile_id,
                Username = userProfile.email,
                Role = 0
            };
            return new UserProfileModel()
            {
                UserId = "UID_" + userProfile.user_profile_id.ToString().PadLeft(5, '0'),
                FullName = userProfile.full_name,
                Avatar = userProfile.avatar,
                Token = Encrypt.Base64Encode(JsonConvert.SerializeObject(token))
            };
        }

        [HttpPost]
        [Route("LoginGoogle")]
        public UserProfileModel LoginGoogle(UserSocialModel model)
        {
            var userGG = _service.GetUserSocialByEmailAndType(model.Email, 0);
            var userProfile = _service.GetUserProfileByEmail(model.Email);
            if (Equals(userGG, null))
            {
                if (Equals(userProfile, null))
                {
                    userProfile = new user_profile()
                    {
                        user_profile_id = 0,
                        full_name = model.FullName,
                        avatar = model.Avatar,
                        email = model.Email,
                        created_date = ConvertDatetime.GetCurrentUnixTimeStamp(),
                        status = 1
                    };
                    _service.SaveUserProfile(userProfile);
                }
                userGG = new user_social()
                {
                    user_profile_id = userProfile.user_profile_id,
                    type = 0,
                    email = model.Email,
                    user_social_id = 0
                };
                _service.SaveUserSocial(userGG);
            }

            var token = new TokenModel()
            {
                Id = userProfile.user_profile_id,
                Username = userProfile.email,
                Role = 0
            };
            return new UserProfileModel()
            {
                UserId
 = "UID_" + userProfile.user_profile_id.ToString().PadLeft(5, '0'),
                FullName = userProfile.full_name,
                Avatar = userProfile.avatar,
                Token = Encrypt.Base64Encode(JsonConvert.SerializeObject(token))
            };
        }

        [HttpPost]
        [Route("LoginFacebook")]
        public UserProfileModel LoginFacebook(UserSocialModel model)
        {
            var userGG = _service.GetUserSocialByEmailAndType(model.Email, 1);
            var userProfile = _service.GetUserProfileByEmail(model.Email);
            if (Equals(userGG, null))
            {
                if (Equals(userProfile, null))
                {
                    userProfile = new user_profile()
                    {
                        user_profile_id = 0,
                        full_name = model.FullName,
                        avatar = model.Avatar,
                        email = model.Email,
                        created_date = ConvertDatetime.GetCurrentUnixTimeStamp(),
                        status = 1
                    };
                    _service.SaveUserProfile(userProfile);
                }
                userGG = new user_social()
                {
                    user_profile_id = userProfile.user_profile_id,
                    type = 1,
                    email = model.Email,
                    user_social_id = 0
                };
                _service.SaveUserSocial(userGG);
            }

            var token = new TokenModel()
            {
                Id = userProfile.user_profile_id,
                Username = userProfile.email,
                Role = 0
            };
            return new UserProfileModel()
            {
                UserId
                    = "UID_" + userProfile.user_profile_id.ToString().PadLeft(5, '0'),
                FullName = userProfile.full_name,
                Avatar = userProfile.avatar,
                Token = Encrypt.Base64Encode(JsonConvert.SerializeObject(token))
            };
        }

        [HttpGet]
        [Route("GetListBlog/{page}/{limit}/{type}/{search?}")]
        public PagingResult<BlogModel> GetListBlog(int page, int limit, int type, string search = null)
        {
            IEnumerable<string> values;
            if (this.Request.Headers.TryGetValues("Language", out values))
            {
                var language = Convert.ToInt32(values.First());
                var blogs = _service.SearchBlogList(type, language, search);
                var blogList = blogs.Select(p => new BlogModel()
                {
                    Id = p.blog_id,
                    CreatedString = ConvertDatetime.ConvertUnixTimeStampToDateTime(p.created_date),
                    Img = p.img,
                    Type = p.type,
                    Content = _service.ConvertBlogContentToModel(p.blog_content.FirstOrDefault(q => q.language == language))
                }).Skip((page - 1) * limit).Take(limit).ToList(); ;
                var result = new PagingResult<BlogModel>()
                {
                    total = blogs.Count,
                    data = blogList
                };
                return result;
            }
            return new PagingResult<BlogModel>();
        }

        [HttpGet]
        [Route("GetBlogDetail/{id}")]
        public BlogModel GetBlogDetail(int id)
        {
            IEnumerable<string> values;
            if (this.Request.Headers.TryGetValues("Language", out values))
            {
                var language = Convert.ToInt32(values.First());
                var blog = _service.GetBlogById(id);
                return new BlogModel()
                {
                    Id = blog.blog_id,
                    CreatedString = ConvertDatetime.ConvertUnixTimeStampToDateTime(blog.created_date),
                    Img = blog.img,
                    Type = blog.type,
                    Content = _service.ConvertBlogContentToModel(blog.blog_content.FirstOrDefault(q => q.language == language))
                };
            }
            return new BlogModel();
        }

        [HttpGet]
        [Route("GetSlide/{type}")]
        public SlideModel GetSlide(int type)
        {
            var slide = _service.GetRandomSlideByType(type);
            return new SlideModel()
            {
                Id = slide.slide_id,
                Type = slide.type,
                Url = slide.url,
                Img = slide.img
            };
        }

        [HttpGet]
        [Route("GetUserProfile")]
        public UserProfileModel GetUserProfile()
        {
            IEnumerable<string> values;
            if (this.Request.Headers.TryGetValues("Token", out values))
            {
                var token = values.First();
                var tokenModel = JsonConvert.DeserializeObject<TokenModel>(Encrypt.Base64Decode(token));
                var userProfile = _service.GetActiveUserProfileById(tokenModel.Id);
                if (Equals(userProfile, null))
                {
                    ExceptionContent(HttpStatusCode.NotFound, "err_account_not_found");
                }
                return new UserProfileModel()
                {
                    UserId = "UID_" + userProfile.user_profile_id.ToString().PadLeft(5, '0'),
                    FullName = userProfile.full_name,
                    Avatar = userProfile.avatar,
                    Gender = userProfile.gender ?? 0,
                    BirthDay = userProfile.birthday ?? 0,
                    Email = userProfile.email,
                    ImgVerification1 = userProfile.img_verification_1,
                    ImgVerification2 = userProfile.img_verification_2,
                    Work = userProfile.work,
                    Contact = userProfile.contact,
                    Description = userProfile.description,
                    Phone = userProfile.phone
                };
            }
            return null;
        }

        [HttpPost]
        [Route("EditUserProfile")]
        public UserProfileModel EditUserProfile(UserProfileModel model)
        {
            IEnumerable<string> values;
            if (this.Request.Headers.TryGetValues("Token", out values))
            {
                using (var scope = new TransactionScope())
                {
                    var token = values.First();
                    var tokenModel = JsonConvert.DeserializeObject<TokenModel>(Encrypt.Base64Decode(token));
                    var userProfile = _service.GetActiveUserProfileById(tokenModel.Id);
                    if (Equals(userProfile, null))
                    {
                        ExceptionContent(HttpStatusCode.NotFound, "err_account_not_found");
                    }

                    userProfile.full_name = model.FullName;
                    userProfile.gender = model.Gender;
                    userProfile.birthday = model.BirthDay;
                    userProfile.phone = model.Phone;
                    userProfile.work = model.Work;
                    userProfile.contact = model.Contact;
                    userProfile.description = model.Description;

                    if (!Equals(model.Avatar_Base64, null))
                    {
                        userProfile.avatar = _service.SaveImage("~/Upload/avatar/",
                            "Avatar_" + userProfile.user_profile_id + ".png",
                            model.Avatar_Base64);
                    }

                    if (!Equals(model.ImgVerification1_Base64, null))
                    {
                        userProfile.img_verification_1 = _service.SaveImage("~/Upload/avatar/",
                            "verification_" + userProfile.user_profile_id + "_1.png",
                            model.ImgVerification1_Base64);
                    }

                    if (!Equals(model.ImgVerification2_Base64, null))
                    {
                        userProfile.img_verification_2 = _service.SaveImage("~/Upload/avatar/",
                            "verification_" + userProfile.user_profile_id + "_2.png",
                            model.ImgVerification2_Base64);
                    }

                    _service.SaveUserProfile(userProfile);

                    scope.Complete();

                    var tok = new TokenModel()
                    {
                        Id = userProfile.user_profile_id,
                        Username = userProfile.email,
                        Role = 0
                    };

                    return new UserProfileModel()
                    {
                        FullName = userProfile.full_name,
                        Avatar = userProfile.avatar,
                        Token = Encrypt.Base64Encode(JsonConvert.SerializeObject(tok))
                    };
                }
            }

            return null;
        }

        [HttpPost]
        [Route("ChangePassword")]
        public void ChangePassword(UserAccountModel model)
        {
            IEnumerable<string> values;
            if (this.Request.Headers.TryGetValues("Token", out values))
            {
                var token = values.First();
                var tokenModel = JsonConvert.DeserializeObject<TokenModel>(Encrypt.Base64Decode(token));
                var userProfile = _service.GetActiveUserProfileById(tokenModel.Id);
                if (Equals(userProfile, null))
                {
                    ExceptionContent(HttpStatusCode.NotFound, "err_account_not_found");
                }

                var userAccount = _service.GetUserAccountByUserProfileId(userProfile.user_profile_id);
                if (Encrypt.EncodePassword(model.Password) != userAccount.password)
                {
                    ExceptionContent(HttpStatusCode.NotFound, "err_old_password_invalid");
                }

                userAccount.password = Encrypt.EncodePassword(model.NewPassword);
                _service.SaveUserAccount(userAccount);
            }
        }

        [HttpGet]
        [Route("GetVisitList")]
        public List<UserVisitModel> GetVisitList()
        {
            IEnumerable<string> values;
            if (this.Request.Headers.TryGetValues("Token", out values))
            {
                var token = values.First();
                var tokenModel = JsonConvert.DeserializeObject<TokenModel>(Encrypt.Base64Decode(token));
                var userProfile = _service.GetActiveUserProfileById(tokenModel.Id);
                if (Equals(userProfile, null))
                {
                    ExceptionContent(HttpStatusCode.NotFound, "err_account_not_found");
                }
                IEnumerable<string> languages;
                this.Request.Headers.TryGetValues("Language", out languages);
                var language = Convert.ToInt32(languages.First());
                var visits = _service.GetListUserVisitByUserProfileId(userProfile.user_profile_id);
                return visits.Select(p => new UserVisitModel()
                {
                    TotalPrice = p.total_price,
                    Id = p.user_visit_id,
                    Apartment = new ApartmentModel()
                    {
                        Id = p.apartment_id,
                        Code = p.apartment.code,
                        Name = p.apartment.apartment_content.FirstOrDefault(q => q.language == language).name,
                        NoBedRoom = p.apartment.no_bedroom,
                        City = p.apartment.city
                    }
                }).ToList();
            }
            return new List<UserVisitModel>();
        }

        [HttpPost]
        [Route("AddVisitList")]
        public void AddVisitList(UserVisitModel model)
        {
            IEnumerable<string> values;
            if (this.Request.Headers.TryGetValues("Token", out values))
            {
                var token = values.First();
                var tokenModel = JsonConvert.DeserializeObject<TokenModel>(Encrypt.Base64Decode(token));
                var userProfile = _service.GetActiveUserProfileById(tokenModel.Id);
                if (Equals(userProfile, null))
                {
                    ExceptionContent(HttpStatusCode.NotFound, "err_account_not_found");
                }

                var userVisit =
                    _service.GetUserVisitByUserProfileIdAndApartmentId(userProfile.user_profile_id, model.ApartmentId);
                if (!Equals(userVisit, null))
                    return;
                userVisit = new user_visit()
                {
                    user_profile_id = userProfile.user_profile_id,
                    apartment_id = model.ApartmentId,
                    user_visit_id = 0,
                    bill = model.Bill,
                    cleaning = model.Cleaning,
                    is_detergent = model.IsDetergent,
                    is_include_tax = model.IsIncludeTax,
                    is_internet_wifi = model.IsInternetWifi,
                    is_management_fee = model.IsApartmentFee,
                    service_price = model.ServicePrice,
                    total_price = model.TotalPrice,
                    tv_type = model.TvType,
                    water = model.Water
                };
                _service.SaveUserVisit(userVisit);
            }
        }

        [HttpDelete]
        [Route("DeleteVisitList/{userVisitId}")]
        public void DeleteVisitList(int userVisitId)
        {
            IEnumerable<string> values;
            if (this.Request.Headers.TryGetValues("Token", out values))
            {
                var token = values.First();
                var tokenModel = JsonConvert.DeserializeObject<TokenModel>(Encrypt.Base64Decode(token));
                var userProfile = _service.GetActiveUserProfileById(tokenModel.Id);
                if (Equals(userProfile, null))
                {
                    ExceptionContent(HttpStatusCode.NotFound, "err_account_not_found");
                }
                var userVisit =
                    _service.GetUserVisitById(userVisitId);
                if (!Equals(userVisit, null) && userVisit.user_profile_id == userProfile.user_profile_id)
                    _service.DeleteUserVisit(userVisit);
            }
        }

        [HttpPost]
        [Route("CreateApartment")]
        public void CreateApartment(ApartmentModel model)
        {
            IEnumerable<string> values;
            if (this.Request.Headers.TryGetValues("Token", out values))
            {
                try
                {
                    var token = values.First();
                    var tokenModel = JsonConvert.DeserializeObject<TokenModel>(Encrypt.Base64Decode(token));
                    var userProfile = _service.GetActiveUserProfileById(tokenModel.Id);
                    if (Equals(userProfile, null))
                    {
                        ExceptionContent(HttpStatusCode.NotFound, "err_account_not_found");
                    }
                    IEnumerable<string> languages;
                    this.Request.Headers.TryGetValues("Language", out languages);
                    var language = Convert.ToInt32(languages.First());
                    using (var scope = new TransactionScope())
                    {
                        var count = _service.GetListApartmentByUserProfileId(userProfile.user_profile_id).Count + 1;
                        var apartment = new apartment()
                        {
                            apartment_id = 0,
                            user_profile_owner_id = userProfile.user_profile_id,
                            code = "AID_" + userProfile.user_profile_id.ToString().PadLeft(5, '0') + "_" +
                                   count.ToString().PadLeft(3, '0'),
                            created_date = ConvertDatetime.GetCurrentUnixTimeStamp(),
                            type = model.Type,
                            project_id = model.ProjectId,
                            no_bathroom = model.NoBathRoom,
                            no_bedroom = model.NoBedRoom,
                            area = model.Area,
                            address = model.Address,
                            city = model.City,
                            latitude = model.Latitude,
                            longitude = model.Longitude,
                            price = model.Price,
                            management_fee = model.ManagementFee,
                            status = 0
                        };
                        _service.SaveApartment(apartment);

                        var apartmentContent = new apartment_content()
                        {
                            apartment_id = apartment.apartment_id,
                            name = model.Name,
                            description = model.Description,
                            language = language,
                            apartment_content_id = 0,
                        };
                        _service.SaveApartmentContent(apartmentContent);

                        int idx = 0;
                        foreach (var img in model.ImgList)
                        {
                            var apartmentImage = new aparment_image()
                            {
                                apartment_id = apartment.apartment_id,
                                type = img.Type,
                                img = "http://propertyplus.com.vn/Upload/apartment/" + _service.SaveImage("~/Upload/apartment/",
                                    "apt_" + ConvertDatetime.GetCurrentUnixTimeStamp() + "_" + idx + ".png",
                                    img.Img_Base64)
                            };
                            _service.SaveApartmentImage(apartmentImage);
                            idx++;
                        }

                        foreach (var fac in model.FacilityList)
                        {
                            var apartmentFacility = new apartment_facility()
                            {
                                apartment_id = apartment.apartment_id,
                                facility_id = fac.Id,
                                apartment_facility_id = 0
                            };
                            _service.SaveApartmentFacility(apartmentFacility);
                        }

                        scope.Complete();
                    }
                }
                catch (Exception ex)
                {
                    ExceptionContent(HttpStatusCode.InternalServerError, ex.Message);
                }
            }
        }

        [HttpGet]
        [Route("GetAllProject")]
        public List<ProjectModel> GetAllProject()
        {
            IEnumerable<string> languages;
            if (this.Request.Headers.TryGetValues("Language", out languages))
            {
                var language = Convert.ToInt32(languages.First());
                return _service.GetAllProject().Select(p => new ProjectModel()
                {
                    Id = p.project_id,
                    Name = p.project_content.FirstOrDefault(q => q.language == language).name
                }).ToList();
            }

            return null;
        }

        [HttpPost]
        [Route("GetListApartment")]
        public PagingResult<ApartmentModel> GetListApartment(FilterModel filter)
        {
            IEnumerable<string> languages;
            if (this.Request.Headers.TryGetValues("Language", out languages))
            {
                var language = Convert.ToInt32(languages.First());
                var apartments = _service.SearchListApartment(filter);
                var apartmentList = apartments.Select(p => new ApartmentModel()
                {
                    Id = p.apartment_id,
                    Name = p.apartment_content.FirstOrDefault(q => q.language == language) == null ? "" : p.apartment_content.FirstOrDefault(q => q.language == language).name,
                    Description = p.apartment_content.FirstOrDefault(q => q.language == language) == null ? "" : p.apartment_content.FirstOrDefault(q => q.language == language).description,
                    Code = p.code,
                    Address = p.address,
                    City = p.city,
                    Area = p.area,
                    Latitude = p.latitude,
                    Longitude = p.longitude,
                    NoBathRoom = p.no_bathroom,
                    NoBedRoom = p.no_bedroom,
                    Price = p.price + p.management_fee,
                    UserProfileOwner = new UserProfileModel()
                    {
                        Id = p.user_profile.user_profile_id,
                        FullName = p.user_profile.full_name,
                        Avatar = p.user_profile.avatar
                    },
                    Project = Equals(p.project_id, null) ? new ProjectModel() : new ProjectModel()
                    {
                        Id = p.project.project_id,
                        Name = p.project.project_content.FirstOrDefault(q => q.language == language).name
                    },
                    ImgList = p.aparment_image.Where(q => q.type == 0).OrderBy(q => q.type).Select(q => new ApartmentImageModel()
                    {
                        Id = q.apartment_image_id,
                        Type = q.type,
                        Img = q.img
                    }).ToList()
                }).Skip((filter.Page - 1) * filter.Limit).Take(filter.Limit).ToList();
                return new PagingResult<ApartmentModel>()
                {
                    total = apartments.Count,
                    data = apartmentList
                };
            }
            return new PagingResult<ApartmentModel>()
            {
                total = 0
            };
        }

        [HttpGet]
        [Route("GetApartmentDetail/{id}")]
        public ApartmentModel GetApartmentDetail(int id)
        {
            IEnumerable<string> languages;
            if (this.Request.Headers.TryGetValues("Language", out languages))
            {
                var language = Convert.ToInt32(languages.First());
                var apartment = _service.GetActiveApartmentById(id);
                if (Equals(apartment, null))
                {
                    ExceptionContent(HttpStatusCode.NotFound, "err_apartment_not_found");
                }
                return new ApartmentModel()
                {
                    Id = apartment.apartment_id,
                    Name = apartment.apartment_content.FirstOrDefault(q => q.language == language) == null ? "" : apartment.apartment_content.FirstOrDefault(q => q.language == language).name,
                    Description = apartment.apartment_content.FirstOrDefault(q => q.language == language) == null ? "" : apartment.apartment_content.FirstOrDefault(q => q.language == language).description,
                    Code = apartment.code,
                    Address = apartment.address,
                    City = apartment.city,
                    Area = apartment.area,
                    Latitude = apartment.latitude,
                    Longitude = apartment.longitude,
                    NoBathRoom = apartment.no_bathroom,
                    NoBedRoom = apartment.no_bedroom,
                    Price = apartment.price + apartment.management_fee,
                    UserProfileOwner = new UserProfileModel()
                    {
                        Id = apartment.user_profile.user_profile_id,
                        FullName = apartment.user_profile.full_name,
                        Avatar = apartment.user_profile.avatar
                    },
                    Project = Equals(apartment.project_id, null) ? new ProjectModel() : new ProjectModel()
                    {
                        Id = apartment.project.project_id,
                        Name = apartment.project.project_content.FirstOrDefault(q => q.language == language).name
                    },
                    ImgList = apartment.aparment_image.Where(q => q.type != -1 && q.type != 1).OrderBy(q => q.type).Select(q => new ApartmentImageModel()
                    {
                        Id = q.apartment_image_id,
                        Type = q.type,
                        Img = q.img
                    }).ToList()
                };
            }
            return new ApartmentModel();
        }

        [HttpGet]
        [Route("GetYourListApartment/{status}")]
        public List<ApartmentModel> GetYourListApartment(int status)
        {
            IEnumerable<string> values;
            if (this.Request.Headers.TryGetValues("Token", out values))
            {
                var token = values.First();
                var tokenModel = JsonConvert.DeserializeObject<TokenModel>(Encrypt.Base64Decode(token));
                var userProfile = _service.GetActiveUserProfileById(tokenModel.Id);
                if (Equals(userProfile, null))
                {
                    ExceptionContent(HttpStatusCode.NotFound, "err_account_not_found");
                }

                IEnumerable<string> languages;
                this.Request.Headers.TryGetValues("Language", out languages);
                var language = Convert.ToInt32(languages.First());
                var apartments = _service.SearchListApartmentByUserProfileId(status, userProfile.user_profile_id);
                return apartments.Select(p => new ApartmentModel()
                {
                    Id = p.apartment_id,
                    Name = p.apartment_content.FirstOrDefault(q => q.language == language) == null
                        ? ""
                        : p.apartment_content.FirstOrDefault(q => q.language == language).name,
                    Description = p.apartment_content.FirstOrDefault(q => q.language == language) == null
                        ? ""
                        : p.apartment_content.FirstOrDefault(q => q.language == language).description,
                    Code = p.code,
                    Address = p.address,
                    City = p.city,
                    Area = p.area,
                    Latitude = p.latitude,
                    Longitude = p.longitude,
                    NoBathRoom = p.no_bathroom,
                    NoBedRoom = p.no_bedroom,
                    Price = p.price + p.management_fee,
                    UserProfileOwner = new UserProfileModel()
                    {
                        Id = p.user_profile.user_profile_id,
                        FullName = p.user_profile.full_name,
                        Avatar = p.user_profile.avatar
                    },
                    Project = Equals(p.project_id, null)
                        ? new ProjectModel()
                        : new ProjectModel()
                        {
                            Id = p.project.project_id,
                            Name = p.project.project_content.FirstOrDefault(q => q.language == language).name
                        },
                    ImgList = p.aparment_image.Where(q => q.type == 0).OrderBy(q => q.type).Select(q =>
                        new ApartmentImageModel()
                        {
                            Id = q.apartment_image_id,
                            Type = q.type,
                            Img = q.img
                        }).ToList()
                }).ToList();
            }
            return new List<ApartmentModel>();
        }

        [HttpGet]
        [Route("GetYourApartmentDetail/{id}")]
        public ApartmentModel GetYourApartmentDetail(int id)
        {
            IEnumerable<string> values;
            if (this.Request.Headers.TryGetValues("Token", out values))
            {
                var token = values.First();
                var tokenModel = JsonConvert.DeserializeObject<TokenModel>(Encrypt.Base64Decode(token));
                var userProfile = _service.GetActiveUserProfileById(tokenModel.Id);
                if (Equals(userProfile, null))
                {
                    ExceptionContent(HttpStatusCode.NotFound, "err_account_not_found");
                }

                IEnumerable<string> languages;
                this.Request.Headers.TryGetValues("Language", out languages);
                var language = Convert.ToInt32(languages.First());
                var apartment = _service.GetApartmentById(id);
                if (Equals(apartment, null) && apartment.user_profile_owner_id != userProfile.user_profile_id)
                {
                    ExceptionContent(HttpStatusCode.NotFound, "err_apartment_not_found");
                }
                return new ApartmentModel()
                {
                    Id = apartment.apartment_id,
                    Name = apartment.apartment_content.FirstOrDefault(q => q.language == language) == null
                        ? ""
                        : apartment.apartment_content.FirstOrDefault(q => q.language == language).name,
                    Description = apartment.apartment_content.FirstOrDefault(q => q.language == language) == null
                        ? ""
                        : apartment.apartment_content.FirstOrDefault(q => q.language == language).description,
                    Code = apartment.code,
                    Address = apartment.address,
                    City = apartment.city,
                    Area = apartment.area,
                    Latitude = apartment.latitude,
                    Longitude = apartment.longitude,
                    NoBathRoom = apartment.no_bathroom,
                    NoBedRoom = apartment.no_bedroom,
                    Price = apartment.price + apartment.management_fee,
                    UserProfileOwner = new UserProfileModel()
                    {
                        Id = apartment.user_profile.user_profile_id,
                        FullName = apartment.user_profile.full_name,
                        Avatar = apartment.user_profile.avatar
                    },
                    Project = Equals(apartment.project_id, null)
                        ? new ProjectModel()
                        : new ProjectModel()
                        {
                            Id = apartment.project.project_id,
                            Name = apartment.project.project_content.FirstOrDefault(q => q.language == language).name
                        },
                    ImgList = apartment.aparment_image.Where(q => q.type == 0).OrderBy(q => q.type).Select(q =>
                        new ApartmentImageModel()
                        {
                            Id = q.apartment_image_id,
                            Type = q.type,
                            Img = q.img
                        }).ToList()
                };
            }
            return new ApartmentModel();
        }

        protected override void Dispose(bool disposing)
        {
            _service.Dispose();
            base.Dispose(disposing);
        }
    }
}