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
    
    public partial class contract
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public contract()
        {
            this.contract_employee = new HashSet<contract_employee>();
            this.contract_note = new HashSet<contract_note>();
            this.contract1 = new HashSet<contract>();
        }
    
        public int contract_id { get; set; }
        public string code { get; set; }
        public int type { get; set; }
        public Nullable<int> company_id { get; set; }
        public Nullable<int> user_profile_id { get; set; }
        public Nullable<int> owner_user_profile_id { get; set; }
        public Nullable<int> created_date { get; set; }
        public Nullable<int> apartment_id { get; set; }
        public string building { get; set; }
        public string no_apartment { get; set; }
        public string address { get; set; }
        public Nullable<decimal> area { get; set; }
        public Nullable<int> no_bedroom { get; set; }
        public string pass_wifi { get; set; }
        public string pass_door { get; set; }
        public string owner_name { get; set; }
        public string owner_phone { get; set; }
        public string owner_tax_code { get; set; }
        public string owner_address { get; set; }
        public string owner_bank_account { get; set; }
        public string owner_bank_name { get; set; }
        public string owner_bank_number { get; set; }
        public string owner_bank_branch { get; set; }
        public string tenant_name { get; set; }
        public string tenant_phone { get; set; }
        public string tenant_tax_code { get; set; }
        public string tenant_address { get; set; }
        public string tenant_bank_account { get; set; }
        public string tenant_bank_name { get; set; }
        public string tenant_bank_number { get; set; }
        public string tenant_bank_branch { get; set; }
        public Nullable<int> admin_id { get; set; }
        public int status { get; set; }
        public Nullable<int> parent_id { get; set; }
        public Nullable<int> start_date { get; set; }
        public Nullable<int> end_date { get; set; }
        public string resident_name { get; set; }
        public string resident_phone { get; set; }
        public string resident_identification { get; set; }
        public string owner_identification { get; set; }
    
        public virtual admin admin { get; set; }
        public virtual apartment apartment { get; set; }
        public virtual company company { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<contract_employee> contract_employee { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<contract_note> contract_note { get; set; }
        public virtual user_profile user_profile { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<contract> contract1 { get; set; }
        public virtual contract contract2 { get; set; }
        public virtual user_profile user_profile1 { get; set; }
    }
}
