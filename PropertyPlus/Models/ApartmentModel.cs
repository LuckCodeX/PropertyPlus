﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PropertyPlus.Models
{
    public class ApartmentModel
    {
        public int Id { get; set; }
        public int UserProfileOwnerId { get; set; }
        public UserProfileModel UserProfileOwner { get; set; }
        public int CreatedDate { get; set; }
        public string Code { get; set; }
        public decimal? Price { get; set; }
        public decimal? ManagementFee { get; set; }
        public decimal? Area { get; set; }
        public int? NoBedRoom { get; set; }
        public int? NoBathRoom { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public int? ProjectId { get; set; }
        public ProjectModel Project { get; set; }
        public int Type { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<ApartmentImageModel> ImgList { get; set; }
        public List<FacilityModel> FacilityList { get; set; }
        public List<ApartmentContentModel> ContentList { get; set; }
    }

    public class ApartmentContentModel
    {
        public int Id { get; set; }
        public int Language { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }

    public class ApartmentImageModel
    {
        public int Id { get; set; }
        public int Type { get; set; }
        public string Img { get; set; }
        public string Img_Base64 { get; set; }
    }

    public class ApartmentFacility
    {
        public int Id { get; set; }
        public int ApartmentId { get; set; }
        public int FacilityId { get; set; }
    }
}