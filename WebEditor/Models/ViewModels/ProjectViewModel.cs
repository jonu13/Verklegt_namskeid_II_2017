using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebEditor.Models.Entities;

namespace WebEditor.Models.ViewModels
{
    public class ProjectViewModel
    {
        public Project project { get; set; }
        public List<User> users { get; set; }
    }
}