using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DiplomServer.Models
{
    public class ProblemChecklist
    {
        public int Id { get; set; }
        public int ProblemId { get; set; }
        public string ProblemName { get; set; }
        public bool IsChecked { get; set; }
    }
}