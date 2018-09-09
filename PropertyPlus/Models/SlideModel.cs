using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PropertyPlus.Models
{
    public class SlideModel
    {
        public int Id { get; set; }
        public string Img { get; set; }
        public HttpPostedFileBase ImageFile { get; set; }
        public string Url { get; set; }
        public int Type { get; set; }
    }
}