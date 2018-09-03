using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PropertyPlus.Models
{
    public class BlogModel
    {
        public int Id { get; set; }
        public string Img { get; set; }
        public HttpPostedFileBase ImageFile { get; set; }
        public int CreatedDate { get; set; }
        public string CreatedString { get; set; }
        public List<BlogContentModel> ContentList { get; set; }
        public BlogContentModel Content { get; set; }
        public int Type { get; set; }
    }

    public class BlogContentModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Content { get; set; }
        public BlogModel Blog { get; set; }
        public int Language { get; set; }
    }
}