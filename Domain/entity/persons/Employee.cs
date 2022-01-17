using System;
using System.Collections.Generic;
using System.Text;

namespace RSaitov.SoftwareDevelop.Domain
{
    public class Employee : Staff, IWorker
    {
        public Employee(string name) : base(name, 120000)
        {

        }

        public UserRole GetRole() => UserRole.Employee;
        public string GetName() => Name;

        public decimal GetSalary(List<TimeRecord> timeRecords)
        {
            throw new NotImplementedException();
        }
    }
}
