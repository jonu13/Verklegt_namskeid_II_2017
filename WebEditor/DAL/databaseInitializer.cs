using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebEditor.Models.Entities;

namespace WebEditor.DAL
{
    public class databaseInitializer : System.Data.Entity.DropCreateDatabaseIfModelChanges<databaseContext>
    {
        protected override void Seed(databaseContext context)
        {
            var projects = new List<Project>
            {
                new Project{ name="Project1"},
                new Project{ name="Project2"},
                new Project{ name="Project3"},
                new Project{ name="Project4"},
            };
            
            var files = new List<File>
            {
                new File{ }
            }
        }
    }
}