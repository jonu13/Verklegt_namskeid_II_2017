using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebEditor.Models.Entities
{
    public class ProjectUserConnectors
    {
        public int Id { get; set; }
        public string userId { get; set; }
        public string userName { get; set; }
        public int projectId { get; set; }
        public string role { get; set; }
    }
}