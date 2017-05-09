using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace WebEditor.Models.Entities
{
    /// <summary>
    /// Project contains info about a specific project a user has created
    /// </summary>
    public class Project
    {
        public int projectID { get; set; }
		[Display(Name = "Name of new project:")]
		//[Required(ErrorMessage ="The project must have a name!")]
        public string name { get; set; }
		[Display(Name = "Write the file endings:")]
		public string projectFileType { get; set; }
        public List<File> files { get; set; }
    }
}