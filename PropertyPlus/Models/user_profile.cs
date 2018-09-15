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
    
    public partial class user_profile
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public user_profile()
        {
            this.apartments = new HashSet<apartment>();
            this.user_account = new HashSet<user_account>();
            this.user_visit = new HashSet<user_visit>();
        }
    
        public int user_profile_id { get; set; }
        public string email { get; set; }
        public int status { get; set; }
        public int created_date { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public Nullable<int> gender { get; set; }
        public Nullable<int> birthday { get; set; }
        public string phone { get; set; }
        public string img_verification_1 { get; set; }
        public string img_verification_2 { get; set; }
        public string work { get; set; }
        public string contact { get; set; }
        public string description { get; set; }
        public string avatar { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<apartment> apartments { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<user_account> user_account { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<user_visit> user_visit { get; set; }
    }
}
