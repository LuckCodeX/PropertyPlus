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
    
    public partial class apartment_employee_issue
    {
        public int apartment_employee_issue_id { get; set; }
        public int apartment_employee_id { get; set; }
        public int issue_id { get; set; }
        public bool is_complete { get; set; }
    
        public virtual apartment_employee apartment_employee { get; set; }
        public virtual issue issue { get; set; }
    }
}
