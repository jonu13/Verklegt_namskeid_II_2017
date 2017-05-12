using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebEditor.Models.Entities
{
    public class File
    {
        public int fileID { get; set; }
        public string fileName { get; set; }
        public string content { get; set; }
        public string fileType { get; set; }
        [Display(Name = "Filetype:")]
        public int projectID { get; set; }
    }
}