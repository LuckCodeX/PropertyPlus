using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
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
        public UserProfileModel Register(UserProfileModel model)
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
                created_date = ConvertDatetime.GetCurrentUnixTimeStamp()
            };
            _service.SaveUserProfile(userProfile);

            var userAcc = new user_account()
            {
                user_account_id = 0,
                user_profile_id = userProfile.user_profile_id,
                email = model.Email,
                password = model.Password
            };
            _service.SaveUserAccount(userAcc);

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
                Id = userProfile.user_profile_id,
                Token = Encrypt.Base64Encode(JsonConvert.SerializeObject(token))
            };
        }

        [HttpPost]
        public UserProfileModel Login(UserAccountModel model)
        {
            var userAcc = _service.Login(model);
            if (Equals(userAcc, null))
            {
                var response = new HttpResponseMessage(HttpStatusCode.Unauthorized)
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
                Id = userProfile.user_profile_id,
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

        protected override void Dispose(bool disposing)
        {
            _service.Dispose();
            base.Dispose(disposing);
        }
    }
}