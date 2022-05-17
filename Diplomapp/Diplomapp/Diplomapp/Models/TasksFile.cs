using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Diplomapp.Models
{
    public class TasksFile
    {
        public int Id { get; set; }
        public string TaskName { get; set; }
        public int TaskId { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
        public string senderName { get; set; }
        public string ProjectFolder { get; set; }
    }
}