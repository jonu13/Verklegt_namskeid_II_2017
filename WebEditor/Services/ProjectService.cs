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

        public List<Project> getAllProjects() {
            return _db.projects.ToList();
        }

        public List<File> getFilesByProjectId(int id)
        {
            var filesById = from f in _db.files
                                   where f.projectID == id
                                   select f;
            return filesById.ToList();
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