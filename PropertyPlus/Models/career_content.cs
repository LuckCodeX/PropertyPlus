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
    
    public partial class career_content
    {
        public int career_content_id { get; set; }
        public int career_id { get; set; }
        public string title { get; set; }
        public string content { get; set; }
        public int language { get; set; }
    
        public virtual career career { get; set; }
    }
}
