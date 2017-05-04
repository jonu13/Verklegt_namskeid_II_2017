﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebEditor.Services;
using WebEditor.Models.Entities;
using WebEditor.Models.ViewModels;
using WebEditor.Models;

namespace WebEditor.Controllers
{
    public class ProjectController : Controller
    {
        private ApplicationDbContext _db = new ApplicationDbContext();
        private ProjectService _service = new ProjectService();
        List<File> fileList1 = new List<File>
        {
            new File { fileID=1, fileName="File1", content="ablabsleicanseilbf" },
            new File { fileID=2, fileName="File2", content="asefga asef ase fase fase f" },
            new File { fileID=3, fileName="File3", content="oesiaf joiase jfoioia sjefoiase f" }
        };
        // GET: Project
        public ActionResult Index(int? pageIndex, string sortBy) {
            if(!pageIndex.HasValue)
            {
                pageIndex = 1;
            }

            if(String.IsNullOrWhiteSpace(sortBy))
            {
                sortBy = "Name";
            }

            return Content(String.Format("pageIndex={0}&sortBy={1}", pageIndex, sortBy));
        }

        public ActionResult ProjectList() {
            List<Project> allProjects = _service.getAllProjects();
            foreach(var project in allProjects)
            {
                project.files = _service.getFilesByProjectId(project.projectID);
            }
            return View(allProjects);

            //var viewModel = _service.getProjectById(id);
            /*
            var projects = new List<Project>
            {
                new Project { projectID=1, name="Project 1", files=fileList1 },
                new Project { projectID=2, name="Project 2", files=fileList1 }
            };

            var users = new List<User>
            {
                new User { userID=1, userName="Nafn1" },
                new User { userID=2, userName="Nafn2" }
            };

            var viewModel = new ProjectViewModel
            {
                projects = projects
            };

            return View(viewModel);
            */
        }

        public ActionResult EditFile(int? id) {
            File fileToEdit = new File { };
            for(int i = 0; i < fileList1.Count; i++)
            {
                if(fileList1[i].fileID == id)
                {
                    fileToEdit = fileList1[i];
                }
            }
            return View(fileToEdit);
        }
    }
}