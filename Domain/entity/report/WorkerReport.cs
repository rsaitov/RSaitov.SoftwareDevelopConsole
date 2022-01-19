using System.Collections.Generic;

namespace RSaitov.SoftwareDevelop.Domain
{
    public class WorkerReport
    {
        public IEnumerable<TimeRecord> TimeRecords { get;  }
        public decimal Salary { get; }
        public int Hours { get; }

        public WorkerReport(IEnumerable<TimeRecord> timeRecords, decimal salary, int hours)
        {
            TimeRecords = timeRecords;
            Salary = salary;
            Hours = hours;
        }
    }
}
