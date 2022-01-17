using System;
using System.Collections.Generic;
using System.Text;

namespace RSaitov.SoftwareDevelop.Domain
{
    public class Staff : Person
    {
        public decimal MonthSalary { get; }
        public Staff(string name, decimal monthSalary) : base(name)
        {
            MonthSalary = monthSalary;
        }
    }
}
