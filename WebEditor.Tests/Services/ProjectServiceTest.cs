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
    class ProjectServiceTest
    {
        private ProjectService _service;

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

            var f1 = new File
            {
                fileID = 1,
                fileName = "file1",
                fileType = "cpp",
                content = "Kanski var Jesú ekki að segja Jerusalem heldur É'rú'Salem",
                projectID = 1,
            };
            mockDb.files.Add(f1);
            var f2 = new File
            {
                fileID = 2,
                fileName = "file2",
                fileType = "cpp",
                content = "Þetta eru dummy gögn",
                projectID = 1,
            };
            mockDb.files.Add(f2);
            var f3 = new File
            {
                fileID = 3,
                fileName = "file3",
                fileType = "js",
                content = "Þetta er dæmi content",
                projectID = 2,
            };
            mockDb.files.Add(f3);

            _service = new ProjectService(mockDb);
        }
        [TestMethod]
        public void getAllProjectsTest()
        {
            // Arrange:
            //ekkert = 0;

            // Act:
            var result = _service.getAllProjects();

            //Assert:
            Assert.AreEqual(3, result);
        }

    }
}
