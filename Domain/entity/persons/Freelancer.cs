﻿using System.Collections.Generic;
using System.Linq;

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
            var hours = timeRecords.Sum(x => x.Hours);
            return hours * Settings.FreelancerSalaryPerHour;
        }
    }
}
