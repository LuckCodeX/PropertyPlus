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
    
    public partial class problem_tracking
    {
        public int problem_tracking_id { get; set; }
        public int problem_id { get; set; }
        public int created_date { get; set; }
        public string content { get; set; }
        public Nullable<decimal> price { get; set; }
        public Nullable<int> employee_id { get; set; }
    
        public virtual employee employee { get; set; }
        public virtual problem problem { get; set; }
    }
}
