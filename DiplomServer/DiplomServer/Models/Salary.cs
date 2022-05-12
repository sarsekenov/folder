using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DiplomServer.Models
{
    public class Salary
    {
        public int Id { get; set; }
        public string UserId{ get; set; }
        public float zp { get; set; }
        public float stavka { get; set; }
        public int ProjectId { get; set; }
    }
}