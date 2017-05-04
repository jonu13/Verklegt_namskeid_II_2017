using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebEditor.Models.Entities
{
    public class ProjectUserConnector
    {
        public int connectorId { get; set; }
        public int userId { get; set; }
        public int projectId { get; set; }
        public string role { get; set; }
    }
}