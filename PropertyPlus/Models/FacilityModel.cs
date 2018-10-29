﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PropertyPlus.Models
{
    public class FacilityModel
    {
        public int Id { get; set; }
        public string Img { get; set; }
        public HttpPostedFileBase ImageFile { get; set; }
        public List<FacilityContentModel> ContentList { get; set; }
        public FacilityContentModel Content { get; set; }
        public int ApartmentFacilityId { get; set; }
        public bool Selected { get; set; }
    }

    public class FacilityContentModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Language { get; set; }
    }
}