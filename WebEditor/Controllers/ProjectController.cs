using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebEditor.Services;
using WebEditor.Models.Entities;
using WebEditor.Models.ViewModels;

namespace WebEditor.Controllers
{
    public class ProjectController : Controller
    {
        //private ProjectService _service = new ProjectService();
        List<File> fileList1 = new List<File>
        {
            new File { fileId=1, name="File1", content="ablabsleicanseilbf" },
            new File { fileId=2, name="File2", content="asefga asef ase fase fase f" },
            new File { fileId=3, name="File3", content="oesiaf joiase jfoioia sjefoiase f" }
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
            //var viewModel = _service.getProjectById(id);

            var projects = new List<Project>
            {
                new Project { id=1, name="Project 1", files=fileList1 },
                new Project { id=2, name="Project 2", files=fileList1 }
            };

            var viewModel = new ProjectViewModel
            {
                projects = projects
            };

            return View(viewModel);
        }

        public ActionResult EditFile(int? id) {
            File fileToEdit = new File { };
            for(int i = 0; i < fileList1.Count; i++)
            {
                if(fileList1[i].fileId == id)
                {
                    fileToEdit = fileList1[i];
                }
            }
            return View(fileToEdit);
        }
    }
}