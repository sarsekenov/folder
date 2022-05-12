using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DiplomServer.Models
{
    public class ProblemComment
    {
        public int Id { get; set; }
        public int  ProblemId { get; set; }
        public string Author { get; set; }
        public string Text { get; set; }
    }
}