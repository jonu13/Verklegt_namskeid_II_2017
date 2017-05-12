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
#region GoToView
        /// <summary>
        /// nær í öll project sem current user á og 
        /// nær í alla contacta sem að eru í þeim projectum og sendir það inn í viewið
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

        [HttpGet]
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

        [HttpGet]
        public ActionResult EditFile(int id) {
            if (!checkAuthentication())
            {
                return RedirectToAction("Login", "Account");
            }

            File fileToEdit = _service.getFileById(id);
            return View(fileToEdit);
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
        #endregion

#region ActionCallsFromView
        [HttpPost]
        public ActionResult SaveCode(File model)
        {
            _service.updateFile(model);
            return View("EditFile", model); //Virkar ekki, þarf að senda model upplýsingarnar með í gegn
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

		[HttpPost]
		public ActionResult CreateNewFile(File model)
		{
			model.content = ""; // Because "" doesnt survive the view class.

            model.fileName = model.fileName + "." + model.fileType;

            if (!_service.projectAlreadyHasFileName(model.fileName, model.projectID))
			{
				_service.WriteNewFileToDataBase(model);
			}
			return RedirectToAction("ProjectList");
		}

        /// <summary>
        /// eyðum contact úr projecti með því að ýta á drop takka inn í contact manager
        /// JDP
        /// </summary>
        /// <param name="projId"> Id af projectinum sem á að losa úr</param>
        /// <param name="userName"> userinn sem á að losa sig við</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult DropUserFromProject(int projId, string userName)
        {
            _service.removeUserConnection(projId, userName);
            return RedirectToAction("ContactManager");
        }

        /// <summary>
        /// bæti við user í project í contact managernum og endurhleð síðuna síðan
        /// JDP
        /// vantar leið til að tékka hvort userinn sem sendur inn er til yfir höfuð og senda viðeigandi error message
        /// </summary>
        /// <param name="projId">id af projecti sem á að bæta í</param>
        /// <param name="userName"> userinn sem á að bæta við</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AddUserToProject(int projId, string userName)
        {
            _service.addUserToProject(projId, userName, false);
            return RedirectToAction("ContactManager");
        }
#endregion

        public bool projectIsEmpty(int projectID)
		{
			return _service.projectIsEmpty(projectID);
		}

        private bool checkAuthentication()
        {
            if (User.Identity.IsAuthenticated)
                return true;
            return false;
        }
    }
}
