using System.Collections.Generic;
using System.Linq;

namespace RSaitov.SoftwareDevelop.Domain
{
    public class Employee : Person, IWorker
    {
        public decimal MonthSalary => 120000;
        public Employee(string name) : base(name)
        {

        }

        public UserRole GetRole() => UserRole.Employee;
        public string GetName() => Name;

        public decimal GetSalary(IEnumerable<TimeRecord> timeRecords)
        {
            var payPerHour = MonthSalary / Settings.WorkingHoursPerMonth;
            var bonusPerHour = payPerHour * 2;
            var hours = timeRecords.Sum(x => x.Hours);

            var totalPay = 0m;

            foreach (var timeRecord in timeRecords)
            {
                if (timeRecord.Hours <= Settings.WorkingHoursPerDay)
                {
                    totalPay += timeRecord.Hours * payPerHour;
                }
                else
                {
                    totalPay += Settings.WorkingHoursPerDay * payPerHour 
                        + bonusPerHour * (timeRecord.Hours - Settings.WorkingHoursPerDay);
                }
            }

            return totalPay;
        }
    }
}
