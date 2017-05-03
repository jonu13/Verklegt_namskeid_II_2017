using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebEditor.Models.Entities
{
    /// <summary>
    /// Project contains info about a specific project a user has created
    /// </summary>
    public class Project
    {
        public int id { get; set; }
        public string name { get; set; }
    }
}