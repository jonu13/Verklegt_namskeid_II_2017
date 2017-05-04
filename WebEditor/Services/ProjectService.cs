using WebEditor.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebEditor.Models;
using WebEditor.Models.Entities;

namespace WebEditor.Services
{
    public class ProjectService
    {
        private ApplicationDbContext _db;

        public ProjectService() {
            _db = new ApplicationDbContext();
        }

        public List<ProjectViewModel> getAllProjects() {
            //List<ProjectViewModel> prList;
            //prList = _db.projects.ToList();
            return null;
        }
        /*
        public ProjectViewModel getProjectById(int projectId) {
            var project = _db.projects.SingleOrDefault(x => x.id == projectId);

            if(project == null) {
                // Err handling
                return null;
            }

            var viewModel = new ProjectViewModel
            {
                name = project.name
            };
           

            return viewModel;
        }*/
    }
}