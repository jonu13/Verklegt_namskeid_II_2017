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
            if(!checkAuthentication())
                return RedirectToAction("Login", "Account");
            //List<int> projectIdList = _service.getProjectIdsByUserId(1);    // Static parameter fyrir userId TODO: sækja úr db
            var currentUser = User.Identity.Name;
            List<int> projectIdList = _service.getProjectIdsByUserName(currentUser);
            var viewModel = _service.getProjectsFromIdList(projectIdList, currentUser);
            //viewModel.roles = _service.getRolesWithProjecList(currentUser, viewModel.projects);

            return View(viewModel);
        }

        public ActionResult EditFile(int id) {
            if (!checkAuthentication())
            { 
                return RedirectToAction("Login", "Account");
            }

            File fileToEdit = _service.getFileById(id);
            return View(fileToEdit);
        }

        [HttpPost]
        public ActionResult SaveCode(File model)
        {
            _service.updateFile(model);
            return View("EditFile", model); //Virkar ekki, þarf að senda model upplýsingarnar með í gegn
        }

		[HttpGet]
		public ActionResult CreateNewProject()
		{
            if (!checkAuthentication())
            {
                return RedirectToAction("Login", "Account");
            }
            Project model = new Project();
			model.projectFileType = "cpp";
			return View(model);
		}

		[HttpPost]
		public ActionResult CreateNewProject(Project model)
		{
			/*if(ModelState.IsValid)
			{
				_service.writeNewProjectToDataBase(model);
				return RedirectToAction("ProjectList");
			}*/
			_service.writeNewProjectToDataBase(model, User.Identity.Name);
			return RedirectToAction("ProjectList");
			//return View(model);
		}

		public bool projectIsEmpty(int projectID)
		{ 
			return _service.projectIsEmpty(projectID);
		}

		[HttpGet]
		public ActionResult CreateNewFile(int projectID)
		{
            if (!checkAuthentication())
            {
                return RedirectToAction("Login", "Account");
            }

            File model = new File();
            var userName = User.Identity.Name;

			List<int> listOfOneProject = new List<int>(1);
			listOfOneProject.Add(projectID);
			string projectFileType = _service.getProjectsFromIdList(listOfOneProject, userName).projects.First().projectFileType;

			model.content = "";
			model.projectID = projectID;
			model.fileType = projectFileType;
			
			if (projectIsEmpty(projectID))
			{   // First file in project shall be index.someFileType

				model.fileName = "index." + projectFileType;
				_service.WriteNewFileToDataBase(model);
				return RedirectToAction("ProjectList");
			}
			else
			{	// Any additional file has to be named by user.

				return View(model);
			}
		}

		[HttpPost]
		public ActionResult CreateNewFile(File model)
		{
			model.content = ""; // Because "" doesnt survive the view class.
			model.fileName = model.fileName + "." + model.fileType;
			if(!_service.projectAlreadyHasFileName(model.fileName, model.projectID))
			{
				_service.WriteNewFileToDataBase(model);
			}
			return RedirectToAction("ProjectList");
		}

        private bool checkAuthentication()
        {
            if (User.Identity.IsAuthenticated) 
                return true;
            return false;
        }
		
		[HttpGet]
		public ActionResult contactManager(int projectID)
		{
			contactManagerViewModel model = new contactManagerViewModel();
			model.connectors = _service.getAllConnections();
			model.projectID = projectID;
			model.userName = "";
			return View(model);
		}

		[HttpPost]
		public ActionResult contactManager(int projectID, string userName)
		{
			_service.addUserToProject(projectID, userName, false);
			return contactManager(projectID);
		}
	}

}