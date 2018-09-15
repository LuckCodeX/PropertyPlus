using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PropertyPlus.Models
{
    public class UserAccountModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string New_Password { get; set; }
    }
}