using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebEditor.Models.Entities
{
    public class File
    {
        public int fileId { get; set; }
        public string name { get; set; }
        public string content { get; set; }
    }
}