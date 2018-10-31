using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PropertyPlus.Models
{
    public class ProjectModel
    {
        public int Id { get; set; }
        public string Img { get; set; }
        public string Slide1 { get; set; }
        public string Slide2 { get; set; }
        public string Slide3 { get; set; }
        public string Logo { get; set; }
        public int Type { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public string Url { get; set; }
        public HttpPostedFileBase ImageFile { get; set; }
        public HttpPostedFileBase Slide1File { get; set; }
        public HttpPostedFileBase Slide2File { get; set; }
        public HttpPostedFileBase Slide3File { get; set; }
        public HttpPostedFileBase LogoFile { get; set; }
        public List<ProjectContentModel> ContentList { get; set; }
        public List<ProjectOverviewModel> OverviewList { get; set; }
        public ProjectContentModel Content { get; set; }
        public ProjectOverviewModel Overview { get; set; }
        public List<FacilityModel> FacilityList { get; set; }
    }

    public class ProjectContentModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Language { get; set; }
    }

    public class ProjectOverviewModel
    {
        public List<ProjectOverviewContentModel> ContentList { get; set; }
        public string Content { get; set; }
    }

    public class ProjectOverviewContentModel
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public int Language { get; set; }
    }
}