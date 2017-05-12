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
        #region Constructors
        private readonly IAppDataContext _dbcontext;

        public ProjectService(IAppDataContext context)
        {
            _dbcontext = context ?? new ApplicationDbContext();
        }

        private ApplicationDbContext _db;
        public ProjectService() {
            _db = new ApplicationDbContext();
        }
#endregion

        #region Get functions
        /// <summary>
        /// Gets all project in the database
        /// JÞ
        /// </summary>
        /// <returns></returns>
        public List<Project> getAllProjects() {
            return _db.projects.ToList();
        }

        /// <summary>
        /// Get a file that has the inputed id
        /// JÞ
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public File getFileById(int id)
        {
            var file = _db.files.First(f => f.fileID == id);

            return file;
        }

        /// <summary>
        /// Gets all files that have the inputed project Id
        /// JÞ
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private List<File> getFilesByProjectId(int id)
        {
            var filesById = from f in _db.files
                                   where f.projectID == id
                                   select f;
            return filesById.ToList();
        }

        /// <summary>
        /// Gets all projects that a specific ueser is a part of
        /// JÞ
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public List<int> getProjectIdsByUserName(string userName)
        {
            var projectsIds = from p in _db.projectUserConnectors
                              where p.userName == userName
                              select p.projectId;
            return projectsIds.ToList();
        }

        /// <summary>
        /// Gets all projects with the project Ids that are inputed and 
        /// all the roles that the specific user has in those projects
        /// JÞ
        /// </summary>
        /// <param name="projIds"></param>
        /// <param name="userName"></param>
        /// <returns></returns>
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

            viewModel.roles = getRolesWithProjectList(userName, viewModel.projects);

            return viewModel;
        }

        /// <summary>
        /// Fills the ContactViewModel of all the contacts that are conected 
        /// to the project Ids that the current user owns
        /// JDP
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

        /// <summary>
        /// Get the roles a specific user has in his project
        /// JÞ
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="projects"></param>
        /// <returns></returns>
        private List<string> getRolesWithProjectList(string userName, List<Project> projects)
        {
            var roleList = new List<string>();
            foreach (var pro in projects)
            {
                var curRole = _db.projectUserConnectors.First(r => r.userName == userName && r.projectId == pro.projectID);
                roleList.Add(curRole.role);
            }
            return roleList;
        }
        #endregion
        #region Add to database
        /// <summary>
        /// Adds a inputed user to the inputed project and set his role in the project
        /// JDP
        /// </summary>
        /// <param name="projectID"></param>
        /// <param name="userName"></param>
        /// <param name="owner"></param>
        public void addUserToProject(int projectID, string userName, bool owner)
		{
            //checkes if the user is part of the database
            if (!isRegisteredUser(userName))
            {
                return;
            }
            //checks if the user is already part of the project
            if (isAlreadyConnectedToProject(projectID, userName))
            {
                return;
            }
            ProjectUserConnectors newUserProjectConnection = new ProjectUserConnectors();
			newUserProjectConnection.projectId = projectID;
			newUserProjectConnection.userName = userName;

            //sets the role of newly added user depending on the value of the owner variable
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

        /// <summary>
        /// Add a file to the database 
        /// JHU
        /// </summary>
        /// <param name="newFile"></param>
		public void writeNewFileToDatabase(File newFile)
		{
			_db.files.Add(newFile);
			_db.SaveChanges();
		}

        /// <summary>
        /// Adds newProject into table and also adds current user to that project.
        /// JHU
        /// </summary>
        /// <param name="newProject"></param>
        /// <param name="userName"></param>
        public void writeNewProjectToDatabase(Project newProject, string userName)
		{
			if(newProject.name != null)
			{
				_db.projects.Add(newProject);
				_db.SaveChanges();
				addUserToProject((_db.projects.SingleOrDefault(x => x.name == newProject.name)).projectID, userName, true);
			}
		}
        #endregion

        #region Edit database
        /// <summary>
        /// Updates the content of the inputed file 
        /// TB
        /// </summary>
        /// <param name="updateFile"></param>
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
        /// JDP
        /// </summary>
        /// <param name="projId"></param>
        /// <param name="userName"></param>
        public void removeUserConnection(int projId, string userName)
        {
            var removeUserConnection = _db.projectUserConnectors.First(c => (c.projectId == projId && c.userName == userName));
            _db.projectUserConnectors.Remove(removeUserConnection);
            _db.SaveChanges();
        }
        #endregion

        #region Helper functions
        /// <summary>
        /// checks if the project already has a file that has the same name as was inputed
        /// JHU
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="projectID"></param>
        /// <returns></returns>
		public bool projectAlreadyHasFileName(string fileName, int projectID)
		{
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

        /// <summary>
        /// checks if inputed project is empty
        /// JHU
        /// </summary>
        /// <param name="projectID"></param>
        /// <returns></returns>
        public bool projectIsEmpty(int projectID)
		{
			File tmpFile = _db.files.FirstOrDefault(x => x.projectID == projectID);
			if(tmpFile == null)
			{
				return true;
			}
			return false;
		}

        /// <summary>
        /// checks if the user that is sent in is already part of a 
        /// project that is in the project that is sent in
        /// JHU
        /// </summary>
        /// <param name="projectID"></param>
        /// <param name="userName"></param>
        /// <returns></returns>
        private bool isAlreadyConnectedToProject(int projectID, string userName)
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

        /// <summary>
        /// checks if the user that is sent in is part of the database
        /// JHU
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        private bool isRegisteredUser(string userName)
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
#endregion
    }
}
