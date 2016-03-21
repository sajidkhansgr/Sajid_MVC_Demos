using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using UniversityProject.Models;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions; 



namespace UniversityProject.Dal
{
    public class SchoolContext:DbContext 
    {
        public DbSet<Student> Students { get; set; }
        public DbSet<Enrollment> Enrollments { get; set; }
        public DbSet<Course> Courses { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        } 

    }
}