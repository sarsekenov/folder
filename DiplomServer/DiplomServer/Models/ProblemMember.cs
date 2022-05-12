using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DiplomServer.Models
{
    public class ProblemMember
    {
        public int Id { get; set; }
        public int ProblemId { get; set; }
        public string UserId { get; set; }
    }
}