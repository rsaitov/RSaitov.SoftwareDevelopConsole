using RSaitov.SoftwareDevelop.Data;
using System;
using System.Collections.Generic;

/*
 * Аргумент (IWorker sender) = отправитель действия для проверки доступа к действию
 * 
 * Что можно улучшить:
 * возвращать созданный объект при вызове CreateWorker
 * при ошибках возвращать описание ошибок вместо возврата false
 * запретить внесение рабочих часов в предстоящих датах
 * 
 */

namespace RSaitov.SoftwareDevelop.Domain
{
    public interface IService
    {
        bool CreateWorker(IWorker sender, IWorker worker);
        IWorker SelectWorker(string name);
        IEnumerable<IWorker> SelectWorkers();

        bool AddTimeRecord(IWorker sender, TimeRecord timeRecord);

        ReportSingleWorker GetReportSingleWorker(IWorker sender, IWorker worker, DateTime start, DateTime end);
        ReportAllWorkers GetReportAllWorkers(IWorker sender, DateTime start, DateTime end);
    }
}
