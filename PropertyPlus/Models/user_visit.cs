//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace PropertyPlus.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class user_visit
    {
        public int user_visit_id { get; set; }
        public int user_profile_id { get; set; }
        public int apartment_id { get; set; }
        public bool is_management_fee { get; set; }
        public bool is_internet_wifi { get; set; }
        public int tv_type { get; set; }
        public int cleaning { get; set; }
        public int water { get; set; }
        public bool is_detergent { get; set; }
        public decimal bill { get; set; }
        public bool is_include_tax { get; set; }
        public decimal total_price { get; set; }
        public decimal service_price { get; set; }
    
        public virtual apartment apartment { get; set; }
        public virtual user_profile user_profile { get; set; }
    }
}
