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
    
    public partial class aparment_image
    {
        public int apartment_image_id { get; set; }
        public int type { get; set; }
        public int apartment_id { get; set; }
        public string img { get; set; }
    
        public virtual apartment apartment { get; set; }
    }
}
