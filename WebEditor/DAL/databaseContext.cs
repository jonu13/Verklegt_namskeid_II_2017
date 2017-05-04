using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebEditor.Models.Entities;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace WebEditor.DAL
{
    public class databaseContext : DbContext
    {
        public databaseContext() : base("databaseContext")
        {
        }

        public DbSet<Project> projects { get; set; }
        public DbSet<User> users { get; set; }
        public DbSet<ProjectUserConnector> projectUserConnector { get; set; }
        public DbSet<File> file { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }

    }
}