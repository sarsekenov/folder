using Diplomapp.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Diplommapp.Models
{
    public class SalaryWithTaxes:Salary
    {
        public float tax1 { get; set; }
        public float tax2 { get; set; }
        public float tax3 { get; set; }
        public float tax4 { get; set; }
        public float saltotal { get; set; }
        public float total { get; set; }
    }
}
