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
    
    public partial class apartment_content
    {
        public int apartment_content_id { get; set; }
        public int apartment_id { get; set; }
        public int language { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public string highlight { get; set; }
    
        public virtual apartment apartment { get; set; }
    }
}
