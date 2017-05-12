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
        //Nær í service klasann okkar
        private ProjectService _service = new ProjectService();
 
#region GoToView
        /// <summary>
        /// Get all the project that the current user has and 
        /// all the contacts that are connected to those projects and 
        /// then returns those values to the view
        /// JDP
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult ContactManager()
        {
            if (!checkAuthentication())
            {
                return RedirectToAction("Login", "Account");
            }
            var currentUser = User.Identity.Name;
            List<int> projectIdList = _service.getProjectIdsByUserName(currentUser);
            var viewModel = _service.getProjectsFromIdListByUserName(projectIdList, currentUser);
            ModelState.Clear();

            return View(viewModel);
        }

        /// <summary>
        /// Creates a form view that creates a new file in a project with the inputed project Id
        /// JHU
        /// </summary>
        /// <param name="projectID"></param>
        /// <returns></returns>
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
				_service.writeNewFileToDatabase(model);
				return RedirectToAction("ProjectList");
			}
			else
			{	// Any additional file has to be named by user.
				return View(model);
			}
		}

        /// <summary>
        /// Creates a form view that creates a new project 
        /// JHU
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// get the file that is to be edited and returns it to the view
        /// TB
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult EditFile(int id)
        {
            if (!checkAuthentication())
            {
                return RedirectToAction("Login", "Account");
            }

            File fileToEdit = _service.getFileById(id);
            return View(fileToEdit);
        }

        /// <summary>
        /// Get all the projects that the current user has and put it in the viewmodel and returns it
        /// JÞ
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult ProjectList()
        {
            if(!checkAuthentication())
                return RedirectToAction("Login", "Account");
            var currentUser = User.Identity.Name;
            List<int> projectIdList = _service.getProjectIdsByUserName(currentUser);
            var viewModel = _service.getProjectsFromIdList(projectIdList, currentUser);

            return View(viewModel);
        }
        #endregion

#region ActionCallsFromView
        /// <summary>
        /// Adds a user to a project
        /// JDP
        /// </summary>
        /// <param name="projId">id af projecti sem á að bæta í</param>
        /// <param name="userName"> userinn sem á að bæta við</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult addUserToProject(int projId, string userName)
        {
            _service.addUserToProject(projId, userName, false);
            return RedirectToAction("ContactManager");
        }


        /// <summary>
        /// Creates a new File 
        /// JHU
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
		[HttpPost]
		public ActionResult CreateNewFile(File model)   // Fallið þarf að hafa stóran staf útaf því að hvernig er kallað í það í viewinu
        {
			model.content = ""; // Because "" doesnt survive the view class.
			model.fileName = model.fileName + "." + model.fileType;
			if(!_service.projectAlreadyHasFileName(model.fileName, model.projectID))
			{
				_service.writeNewFileToDatabase(model);
			}
			return RedirectToAction("ProjectList");
		}

        /// <summary>
        /// Creates a new project
        /// JHU
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
		[HttpPost]
		public ActionResult CreateNewProject(Project model) // Fallið þarf að hafa stóran staf útaf því að hvernig er kallað í það í viewinu
		{
			_service.writeNewProjectToDatabase(model, User.Identity.Name);
			return RedirectToAction("ProjectList");
		}

        /// <summary>
        /// Removes contact from a project
        /// JDP
        /// </summary>
        /// <param name="projId"> Id af projectinum sem á að losa úr</param>
        /// <param name="userName"> userinn sem á að losa sig við</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult dropUserFromProject(int projId, string userName)
        {
            _service.removeUserConnection(projId, userName);
            return RedirectToAction("ContactManager");
        }

        [HttpPost]
        public ActionResult SaveCodgetRolesWithProjecListe(File model)
        {
            _service.updateFile(model);
            return View("EditFile", model); //Virkar ekki, þarf að senda model upplýsingarnar með í gegn
        }
        #endregion

#region Helper functions
        /// <summary>
        /// Checks if a project has no files
        /// JHU
        /// </summary>
        /// <param name="projectID"></param>
        /// <returns></returns>
        public bool projectIsEmpty(int projectID)
		{
			return _service.projectIsEmpty(projectID);
		}

        /// <summary>
        /// Checks if a user is logged in
        /// JÞ
        /// </summary>
        /// <returns></returns>
        private bool checkAuthentication()
        {
            if (User.Identity.IsAuthenticated)
                return true;
            return false;
        }
#endregion
    }
}
