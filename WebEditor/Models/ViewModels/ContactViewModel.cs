using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebEditor.Models.Entities;


namespace WebEditor.Models.ViewModels
{
    public class ContactViewModel
    {
        public List<ProjectUserConnectors> contacts { get; set; }
        public List<Project> projects { get; set; }
    }
}