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
        private readonly IAppDataContext _dbcontext;

        public ProjectService(IAppDataContext context)
        {
            _dbcontext = context ?? new ApplicationDbContext();
        }

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

        public ProjectViewModel getProjectsFromIdList(List<int> projIds, string userName)
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

            viewModel.roles = getRolesWithProjecList(userName, viewModel.projects);

            return viewModel;
        }

        public List<string> getRolesWithProjecList(string userName, List<Project> projects)
        {
            var roleList = new List<string>();
            foreach (var pro in projects)
            {
                var curRole = _db.projectUserConnectors.First(r => r.userName == userName && r.projectId == pro.projectID);
                roleList.Add(curRole.role);
            }
            return roleList;
        }

        /// <summary>
        /// fyllir contactviewmodel af öllum contactum sem eru tengdir projectin sem að current user á 
        /// </summary>
        /// <param name="projIds"></param>
        /// <returns></returns>
        public ContactViewModel getProjectsFromIdListByUserName(List<int> projIds, string userName)
        {
            var projects = _db.projects.Where(p => projIds.Contains(p.projectID));
            var contacts = _db.projectUserConnectors.Where(c => projIds.Contains(c.projectId) );

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

        public List<string> getProjectConnections(string userName, List<int> projectIds)
        {
            var connections = from c in _db.projectUserConnectors
                              where c.userName == userName && projectIds.Contains(c.projectId)
                              select c.role;
            return connections.ToList();
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
            if (!isRegisteredUser(userName))
            {
                return;
            }
            if (isAlreadyConnectedToProject(projectID, userName))
            {
                return;
            }
            ProjectUserConnectors newUserProjectConnection = new ProjectUserConnectors();
			newUserProjectConnection.projectId = projectID;
			newUserProjectConnection.userName = userName;
			if(owner == true)
			{
				newUserProjectConnection.role = "owner";
			}
			else
			{
				newUserProjectConnection.role = "guest";
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

        public bool projectIsEmpty(int projectID)
		{
			File tmpFile = _db.files.FirstOrDefault(x => x.projectID == projectID);
			if(tmpFile == null)
			{
				return true;
			}
			return false;
		}

		public bool projectAlreadyHasFileName(string fileName, int projectID)
		{   // TODO! implement!
			File fileInDB = _db.files.FirstOrDefault(x => (x.fileName == fileName && x.projectID == projectID));
			if(fileInDB == null)
			{
				return false;
			}
			else
			{
				return true;
			}
		}

        public bool isRegisteredUser(string userName)
        {
            var userEntry = _db.Users.FirstOrDefault(f => f.UserName == userName);
            if (userEntry == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public bool isAlreadyConnectedToProject(int projectID, string userName)
        {
            ProjectUserConnectors connection = _db.projectUserConnectors.FirstOrDefault(x => (x.projectId == projectID && x.userName == userName));
            if (connection == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
	}
}
