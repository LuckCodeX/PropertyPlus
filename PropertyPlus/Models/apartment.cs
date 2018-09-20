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
    
    public partial class apartment
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public apartment()
        {
            this.aparment_image = new HashSet<aparment_image>();
            this.apartment_facility = new HashSet<apartment_facility>();
            this.user_visit = new HashSet<user_visit>();
            this.apartment_content = new HashSet<apartment_content>();
        }
    
        public int apartment_id { get; set; }
        public int user_profile_owner_id { get; set; }
        public int created_date { get; set; }
        public int status { get; set; }
        public string code { get; set; }
        public int price { get; set; }
        public decimal area { get; set; }
        public int no_bedroom { get; set; }
        public int no_bathroom { get; set; }
        public string address { get; set; }
        public double latitude { get; set; }
        public double longitude { get; set; }
        public string city { get; set; }
        public Nullable<int> project_id { get; set; }
        public int type { get; set; }
        public int management_fee { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<aparment_image> aparment_image { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<apartment_facility> apartment_facility { get; set; }
        public virtual project project { get; set; }
        public virtual user_profile user_profile { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<user_visit> user_visit { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<apartment_content> apartment_content { get; set; }
    }
}
