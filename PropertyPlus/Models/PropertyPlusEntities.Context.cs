﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class PropertyPlusEntities : DbContext
    {
        public PropertyPlusEntities()
            : base("name=PropertyPlusEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<apartment_facility> apartment_facility { get; set; }
        public virtual DbSet<blog> blogs { get; set; }
        public virtual DbSet<facility> facilities { get; set; }
        public virtual DbSet<facility_content> facility_content { get; set; }
        public virtual DbSet<admin> admins { get; set; }
        public virtual DbSet<blog_content> blog_content { get; set; }
        public virtual DbSet<slide> slides { get; set; }
        public virtual DbSet<user_account> user_account { get; set; }
        public virtual DbSet<project> projects { get; set; }
        public virtual DbSet<project_content> project_content { get; set; }
        public virtual DbSet<aparment_image> aparment_image { get; set; }
        public virtual DbSet<apartment_content> apartment_content { get; set; }
        public virtual DbSet<user_social> user_social { get; set; }
        public virtual DbSet<apartment> apartments { get; set; }
        public virtual DbSet<user_profile> user_profile { get; set; }
        public virtual DbSet<user_visit> user_visit { get; set; }
        public virtual DbSet<user_visit_item> user_visit_item { get; set; }
    }
}
