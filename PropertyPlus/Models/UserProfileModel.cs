using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PropertyPlus.Models
{
    public class UserProfileModel
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public int Status { get; set; }
        public int Gender { get; set; }
        public int BirthDay { get; set; }
        public string Phone { get; set; }
        public string ImgVerification1 { get; set; }
        public string ImgVerification2 { get; set; }
        public string Work { get; set; }
        public string Contact { get; set; }
        public string Description { get; set; }
        public string Password { get; set; }
        public string Token { get; set; }
    }
}