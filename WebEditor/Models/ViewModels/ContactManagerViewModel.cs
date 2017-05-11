using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebEditor.Models.Entities;
using System.ComponentModel.DataAnnotations;

namespace WebEditor.Models.ViewModels
{
	public class contactManagerViewModel
	{
		public List<ProjectUserConnectors> connectors { get; set; }
		public Project project { get; set; }
		[Display(Name = "Add a registered user to your project:")]
		[Required(ErrorMessage = "Please put a registered username")]
		public string userName { get; set; }
	}
}