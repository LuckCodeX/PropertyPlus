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
    
    public partial class career
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public career()
        {
            this.career_content = new HashSet<career_content>();
        }
    
        public int career_id { get; set; }
        public int created_date { get; set; }
        public int category_id { get; set; }
        public string city { get; set; }
        public int type { get; set; }
        public decimal salary_min { get; set; }
        public string experied_date { get; set; }
        public string location { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<career_content> career_content { get; set; }
    }
}
