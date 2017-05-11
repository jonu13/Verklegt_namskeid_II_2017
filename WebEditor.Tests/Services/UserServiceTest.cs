using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using WebEditor.Services;
using WebEditor.Tests;
using WebEditor.Models.Entities;

namespace WebEditor.Tests
{
    [TestClass]
    class UserServiceTest
    {
        private UserService _service;

        [TestInitialize]
        public void Initialize()
        {
            var mockDb = new MockDatabase();
            var p1 = new Project
            {
                projectID = 1,
                name = "project1",
                projectFileType = "cpp"
            };
            mockDb.projects.Add(p1);
            var p2 = new Project
            {
                projectID = 2,
                name = "project2",
                projectFileType = "cpp"
            };
            mockDb.projects.Add(p2);
            var p3 = new Project
            {
                projectID = 3,
                name = "project3",
                projectFileType = "js"
            };
            mockDb.projects.Add(p3);

            _service = new UserService(mockDb);
        }

        [TestMethod]
        public void getContextTest()
        {
            // Arrange:

            // Act:

            //Assert:
        }
    }
}
