using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Diplomapp.Models
{
    public class ProjectFile
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
        public int ProjectId { get; set; }
        public string ProjectName { get; set; }
        public string ProjectFolder { get; set; }
    }
}