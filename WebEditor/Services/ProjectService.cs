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

        public List<int> getProjectIdsByUserId(int userId)
        {
            var projectsIds = from p in _db.projectUserConnectors
                              where p.Id == userId
                              select p.projectId;
            return projectsIds.ToList();
        }

        public List<int> getProjectIdsByUserName(string userName)
        {
            var projectsIds = from p in _db.projectUserConnectors
                              where p.userName == userName
                              select p.projectId;
            return projectsIds.ToList();
        }

        public List<Project> getProjectsFromIdList(List<int> projIds)
        {
            var projects = _db.projects.Where(p => projIds.Contains(p.projectID));
            
            return projects.ToList();
        }

        public List<File> getFilesByProjectId(int id)
        {
            var filesById = from f in _db.files
                                   where f.projectID == id
                                   select f;
            return filesById.ToList();
        }
        
        public File getFileById(int id)
        {
            var file = _db.files.First(f => f.fileID == id);

            return file;
        }

		public void writeNewProjectToDataBase(Project newProject, string userName)
		{	// Adds newProject into table and also adds current user to that project.

			if(newProject.name != null)
			{
				_db.projects.Add(newProject);
				_db.SaveChanges();
				addUserToProject((_db.projects.SingleOrDefault(x => x.name == newProject.name)).projectID, userName, true);
			}
		}

		public void WriteNewFileToDataBase(File newFile)
		{
			_db.files.Add(newFile);
			_db.SaveChanges();
		}

		public void addUserToProject(int projectID, string userName, bool owner)
		{
			ProjectUserConnectors newUserProjectConnection = new ProjectUserConnectors();
			newUserProjectConnection.projectId = projectID;
			newUserProjectConnection.userName = userName;
			if(owner == true)
			{
				newUserProjectConnection.role = "Owner";
			}
			else
			{
				newUserProjectConnection.role = "";
			}
			newUserProjectConnection.userId = null;
			
			_db.projectUserConnectors.Add(newUserProjectConnection);
			_db.SaveChanges();
		}

        public void updateFile(File updateFile)
        {
            var orginalFile = _db.files.Find(updateFile.fileID);

            if(orginalFile != null)
            {
                _db.Entry(orginalFile).CurrentValues.SetValues(updateFile);
                _db.SaveChanges();
            }
        }

		public bool projectIsEmpty(int projectID)
		{   
			File tmpFile = _db.files.FirstOrDefault(x => x.projectID == projectID);
			if(tmpFile == null)
			{
				return true;
			}
			return false;
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