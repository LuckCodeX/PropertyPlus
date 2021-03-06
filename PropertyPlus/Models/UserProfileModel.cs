﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PropertyPlus.Models
{
    public class UserProfileModel
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string CreatedString { get; set; }
        public string Avatar { get; set; }
        public string Avatar_Base64 { get; set; }
        public int Status { get; set; }
        public int Gender { get; set; }
        public int BirthDay { get; set; }
        public string Phone { get; set; }
        public string ImgVerification1 { get; set; }
        public string ImgVerification1_Base64 { get; set; }
        public string ImgVerification2 { get; set; }
        public string ImgVerification2_Base64 { get; set; }
        public string Work { get; set; }
        public string Contact { get; set; }
        public string Description { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public string Token { get; set; }
    }
}