using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Diplomapp.Models
{
    public class ProjectMember
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Role { get; set; }
        public string Position { get; set; }
        public string UserID { get; set; }
        public int ProjectID { get; set; }
    }
}