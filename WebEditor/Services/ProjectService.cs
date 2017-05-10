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

        public ProjectViewModel getProjectsFromIdList(List<int> projIds)
        {
            var projects = _db.projects.Where(p => projIds.Contains(p.projectID));

            ProjectViewModel viewModel = new ProjectViewModel
            {
                projects = projects.ToList()
            };

            foreach (var project in viewModel.projects)
            {
                project.files = getFilesByProjectId(project.projectID);
            }

            return viewModel;
        }

        /// <summary>
        /// ATH!!! var að copy fall að ofan þarf mögulega að hreinsa til eftir að virkni hefur fengist
        /// ///breyta nafni á falli
        /// </summary>
        /// <param name="projIds"></param>
        /// <returns></returns>
        public ContactViewModel getProjectsFromIdList2(List<int> projIds, string userName)
        {
            var projects = _db.projects.Where(p => projIds.Contains(p.projectID));
            var contacts = _db.projectUserConnectors.Where(c => projIds.Contains(c.projectId) && c.userName != userName);

            ContactViewModel viewModel = new ContactViewModel
            {
                projects = projects.ToList(),
                contacts = contacts.ToList()
            };
            foreach (var project in viewModel.projects)
            {
                project.files = getFilesByProjectId(project.projectID);
            }

            return viewModel;
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

		public void writeNewProjectToDataBase(string projectName)
		{	
			Project newProject = new Project();
			newProject.name = projectName;
			_db.projects.Add(newProject);
			_db.SaveChanges();
		}

		public void WriteNewFileToDataBase(string fileName, int projectID)
		{
			File newFile = new File();
			newFile.fileName = fileName;
			newFile.projectID = projectID;
			newFile.content = "";
			_db.files.Add(newFile);
			_db.SaveChanges();
		}

		public void addUserToProject(int projectID, int userID)
		{
			ProjectUserConnectors newUserProjectConnection = new ProjectUserConnectors();
			newUserProjectConnection.projectId = projectID;
			newUserProjectConnection.Id = userID;
			newUserProjectConnection.role = "";
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
        /// <summary>
        /// Fallið er notað í Drop takka í ContactManager
        /// tek inn bæði username og projectid finn færsluna í ProjectUserConnector og eyði færslunni
        /// </summary>
        /// <param name="projId"></param>
        /// <param name="userName"></param>
        public void removeUserConnection(int projId, string userName)
        {       
            var removeUserConnection = _db.projectUserConnectors.First(c => (c.projectId == projId && c.userName == userName));
            _db.projectUserConnectors.Remove(removeUserConnection);
            _db.SaveChanges();
        }


	}
}