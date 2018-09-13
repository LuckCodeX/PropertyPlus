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
    public class PropertyPlusController : ApiController
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
                    var response = new HttpResponseMessage(HttpStatusCode.Unauthorized)
                    {
                        Content = new StringContent("err_email_already_existed")
                    };
                    throw new HttpResponseException(response);
                }

                userProfile = new user_profile()
                {
                    email = model.Email,
                    first_name = model.FirstName,
                    last_name = model.LastName,
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
                    FirstName = userProfile.first_name,
                    LastName = userProfile.last_name,
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
                var response = new HttpResponseMessage(HttpStatusCode.InternalServerError)
                {
                    Content = new StringContent("err_email_or_password_invalid")
                };
                throw new HttpResponseException(response);
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
                FirstName = userProfile.first_name,
                LastName = userProfile.last_name,
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

        [HttpPost]
        [Route("GetUserProfile")]
        public UserProfileModel GetUserProfile()
        {
            IEnumerable<string> values;
            if (this.Request.Headers.TryGetValues("Token", out values))
            {
                var token = values.First();
                var tokenModel = JsonConvert.DeserializeObject<TokenModel>(Encrypt.Base64Decode(token));
                var userProfile = _service.GetActiveUserProfileById(tokenModel.Id);
                return new UserProfileModel()
                {
                    FirstName = userProfile.first_name,
                    LastName = userProfile.last_name,
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
                        var response = new HttpResponseMessage(HttpStatusCode.NotFound)
                        {
                            Content = new StringContent("Account cannot be found !!!")
                        };
                        throw new HttpResponseException(response);
                    }

                    userProfile.first_name = model.FirstName;
                    userProfile.last_name = model.LastName;
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
                        FirstName = userProfile.first_name,
                        LastName = userProfile.last_name,
                        Avatar = userProfile.avatar,
                        Token = Encrypt.Base64Encode(JsonConvert.SerializeObject(tok))
                    };
                }
            }

            return null;
        }

        protected override void Dispose(bool disposing)
        {
            _service.Dispose();
            base.Dispose(disposing);
        }
    }
}