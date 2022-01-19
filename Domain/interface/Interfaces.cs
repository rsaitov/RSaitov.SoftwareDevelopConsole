using RSaitov.SoftwareDevelop.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace RSaitov.SoftwareDevelop.Domain
{
    /// <summary>
    /// Интерфейс, описывающий работающего сотрудника
    /// </summary>
    public interface IWorker
    {
        string GetName();
        UserRole GetRole();
        decimal GetSalary(IEnumerable<TimeRecord> timeRecords);
    }
}
