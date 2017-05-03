using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebEditor.Models.Entities
{
    public class File
    {
        public int fileID { get; set; }
        public string fileName { get; set; }
        public string content { get; set; }
        public int projectID { get; set; }
    }
}