using System;
using System.Collections.Generic;
using System.Text;

namespace RSaitov.SoftwareDevelop.Domain
{
    public class ReportAllWorkers
    {
        public IEnumerable<ReportSingleWorker> SingleWorkerReports;
        public int HoursTotal { get; }
        public decimal SalaryTotal { get; }

        public ReportAllWorkers(IEnumerable<ReportSingleWorker> workerReports, int hoursTotal, decimal salaryTotal)
        {
            SingleWorkerReports = workerReports;
            HoursTotal = hoursTotal;
            SalaryTotal = salaryTotal;
        }
    }
}
