using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebEditor.Models.Entities
{
    public class ProjectUserConnector
    {
        public int connectorID { get; set; }
        public int userID { get; set; }
        public int projectID { get; set; }
        public string role { get; set; }
    }
}