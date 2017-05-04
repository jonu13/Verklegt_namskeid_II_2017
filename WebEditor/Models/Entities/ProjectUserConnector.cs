using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebEditor.Models.Entities
{
    public class ProjectUserConnectors
    {
        [Key]
        public int connectorID { get; set; }
        public int userId { get; set; }
        public int projectID { get; set; }
        public string role { get; set; }
    }
}