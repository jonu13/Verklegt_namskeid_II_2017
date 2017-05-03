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
            var project = new Project() { projectID = 1, name = "newProject" };
            var users = new List<User>
            {
                new User { userID=1, userName="Nafn1" },
                new User { userID=2, userName="Nafn2" }
            };

            var viewModel = new ProjectViewModel
            {
                project = project,
                users = users
            };

            return View(viewModel);
        }

        public ActionResult EditFile(int? id) {
            return View(id);
        }
    }
}