using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using PropertyPlus.Helper;
using PropertyPlus.Models;
using PropertyPlus.Services;

namespace PropertyPlus.Controllers
{
    [RoutePrefix("api/propertyplus")]
    public class PropertyPlusController : ApiController
    {
        private IService _service = new Service();

        [HttpGet]
        [Route("GetListBlog/{page}/{limit}/{type}/{search?}")]
        public PagingResult<BlogModel> GetListBlog(int page,int limit, int type, string search = null)
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
        protected override void Dispose(bool disposing)
        {
            _service.Dispose();
            base.Dispose(disposing);
        }
    }
}