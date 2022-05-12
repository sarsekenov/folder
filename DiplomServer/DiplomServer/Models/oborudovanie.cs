using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DiplomServer.Models
{
    public class oborudovanie
    {
        public int Id { get; set; }
        public int ProjectId { get; set; }
        public string Name { get; set; }

        public string Measure { get; set; }

        public float Count { get; set; }

        public float Price { get; set; }
    }
}