using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DiplomServer.Models
{
    public class Problem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int ProjectId { get; set; }
        public DateTime Creationtime { get; set; }
        public DateTime Deadline { get; set;}
    }
}