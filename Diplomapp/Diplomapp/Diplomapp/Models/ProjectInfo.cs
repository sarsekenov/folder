using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Diplomapp.Models
{
    public class ProjectInfo
    {
        public int Id { get; set; }
        public int ProjectId { get; set; }
        public string ProjectAims { get; set; }
        public string ProjectsBudjet { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string Zakazchik { get; set; }
        
    }
}