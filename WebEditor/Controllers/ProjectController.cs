using System;
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
        private ProjectService _service = new ProjectService();
        List<File> fileList1 = new List<File>
        {
            new File { fileID=1, fileName="File1", content="ablabsleicanseilbf" },
            new File { fileID=2, fileName="File2", content="asefga asef ase fase fase f" },
            new File { fileID=3, fileName="File3", content="oesiaf joiase jfoioia sjefoiase f" }
        };

        List<Project> projectList1 = new List<Project>
        {
            new Project { projectID=1, name="project1"},
            new Project { projectID=2, name="project2"}
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
            var diff = User.Identity.Name;

            return Content(String.Format("pageIndex={0}&sortBy={1}", pageIndex, sortBy));
        }

        public ActionResult ProjectList() {
            var currentUser = User.Identity.Name;
            List<int> projectIdList = _service.getProjectIdsByUserName(currentUser);
            var viewModel = _service.getProjectsFromIdList(projectIdList);

            return View(viewModel);
        }

        public ActionResult EditFile(int id) {
            File fileToEdit = _service.getFileById(id);
            return View(fileToEdit);
        }

        [HttpPost]
        public ActionResult SaveCode(File model)
        {
            _service.updateFile(model);
            return View("EditFile", model); //Virkar ekki, þarf að senda model upplýsingarnar með í gegn
        }

        public ActionResult ContactManager()
        {
            var currentUser = User.Identity.Name;
            List<int> projectIdList = _service.getProjectIdsByUserName(currentUser);
            var viewModel = _service.getProjectsFromIdList2(projectIdList, currentUser);
            ModelState.Clear();

            return View(viewModel);
        }
    }
}