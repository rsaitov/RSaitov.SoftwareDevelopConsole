using System;
using System.Collections.Generic;

namespace RSaitov.SoftwareDevelop.Domain
{
    public class Freelancer : Person, IWorker
    {
        public Freelancer(string name) : base(name)
        {

        }

        public UserRole GetRole() => UserRole.Freelancer;
        public string GetName() => Name;

        public decimal GetSalary(IEnumerable<TimeRecord> timeRecords)
        {
            throw new NotImplementedException();
        }
    }
}
