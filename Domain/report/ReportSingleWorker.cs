using RSaitov.SoftwareDevelop.Data;
using System.Collections.Generic;

namespace RSaitov.SoftwareDevelop.Domain
{
    public class ReportSingleWorker
    {
        public IEnumerable<TimeRecord> TimeRecords { get;  }
        public decimal Salary { get; }
        public int Hours { get; }

        public ReportSingleWorker(IEnumerable<TimeRecord> timeRecords, decimal salary, int hours)
        {
            TimeRecords = timeRecords;
            Salary = salary;
            Hours = hours;
        }
    }
}
