using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FakeDbSet;
using System.Data.Entity;
using WebEditor.Models;
using WebEditor.Models.Entities;

namespace WebEditor.Tests
{
    /// <summary>
    /// This is an example of how we'd create a fake database by implementing the 
    /// same interface that the BookeStoreEntities class implements.
    /// </summary>
    public class MockDatabase : IAppDataContext
    {
        /// <summary>
        /// Sets up the fake database.
        /// </summary>
        public MockDatabase()
        {
            // We're setting our DbSets to be InMemoryDbSets rather than using SQL Server.
            this.projects = new InMemoryDbSet<Project>();;
            this.files = new InMemoryDbSet<File>();
            this.projectUserConnectors = new InMemoryDbSet<ProjectUserConnectors>();
        }

        public IDbSet<Project> projects { get; set; }
        public IDbSet<File> files { get; set; }
        public IDbSet<ProjectUserConnectors> projectUserConnectors { get; set; }
        //Bæta við hinum töflunum

        public int SaveChanges()
        {
            // Pretend that each entity gets a database id when we hit save.
            int changes = 0;
//            changes += DbSetHelper.IncrementPrimaryKey<Author>(x => x.AuthorId, this.Authors);
//            changes += DbSetHelper.IncrementPrimaryKey<Book>(x => x.BookId, this.Books);

            return changes;
        }

        public void Dispose()
        {
            // Do nothing!
        }
    }
}