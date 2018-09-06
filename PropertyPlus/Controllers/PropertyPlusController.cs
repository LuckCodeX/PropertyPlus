using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using PropertyPlus.Models;

namespace PropertyPlus.Controllers
{
    [RoutePrefix("api/propertyplus")]
    public class PropertyPlusController : ApiController
    {
        [HttpGet]
        [Route("GetListBlog")]
        public List<BlogModel> GetListBlog()
        {
            return new List<BlogModel>();
        }
    }
}