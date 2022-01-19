using RSaitov.SoftwareDevelop.Domain;
using System;
using System.Collections.Generic;
using System.Linq;

/*
 * Аргумент (IWorker sender) = отправитель действия для проверки доступа к действию
 * 
 * Что можно улучшить:
 * возвращать созданный объект при вызове CreateWorker
 * при ошибках возвращать описание ошибок вместо возврата false
 * запретить внесение рабочих часов в предстоящих датах
 * 
 */

namespace RSaitov.SoftwareDevelop.Persistence
{
    public interface IService
    {
        bool CreateWorker(IWorker sender, IWorker worker);
        IWorker SelectWorker(string name);
        IEnumerable<IWorker> SelectWorkers();

        bool AddTimeRecord(IWorker sender, TimeRecord timeRecord);

        WorkerReport GetReport(IWorker sender, IWorker worker, DateTime start, DateTime end);
    }
    public class Service : IService
    {
        private IRepository _repository = new TextFileDB();
        private HashSet<UserRole> userRolesAllowedToCreateWorkers = 
            new HashSet<UserRole> { UserRole.Manager };
        private HashSet<UserRole> userRolesAllowedToCreateTimeRecordsForAllWorkers =
            new HashSet<UserRole> { UserRole.Manager };
        private HashSet<UserRole> userRolesAllowedToCreateTimeRecordsForAnyDateInPast =
            new HashSet<UserRole> { UserRole.Manager, UserRole.Employee };
        private HashSet<UserRole> userRolesAllowedToViewReportAllWorkers =
            new HashSet<UserRole> { UserRole.Manager };

        public Service()
        {
        }

        public bool CreateWorker(IWorker sender, IWorker worker)
        {
            if (!userRolesAllowedToCreateWorkers.Contains(sender.GetRole()))
                return false;

            var workerInDb = _repository.SelectWorker(worker.GetName());
            if (ReferenceEquals(null, workerInDb))
                return _repository.InsertWorker(worker);
            
            return false;
        }

        public bool AddTimeRecord(IWorker sender, TimeRecord timeRecord)
        {
            var timeRecordWorker = SelectWorker(timeRecord.Name);
            if (ReferenceEquals(null, timeRecordWorker))
                return false;

            var senderPersonMatchTimeRecord = string.Equals(sender.GetName(), timeRecord.Name);
            var timeRecordDayDiff = (DateTime.Now - timeRecord.Date).TotalDays;
            
            if (!senderPersonMatchTimeRecord &&
                !userRolesAllowedToCreateTimeRecordsForAllWorkers.Contains(sender.GetRole()))
                return false;

            if (timeRecordDayDiff > 2 &&
                !userRolesAllowedToCreateTimeRecordsForAnyDateInPast.Contains(sender.GetRole()))
                return false;

            return _repository.InsertTimeRecord(timeRecord, timeRecordWorker.GetRole());
        }

        public IWorker SelectWorker(string name) => _repository.SelectWorker(name);
        public IEnumerable<IWorker> SelectWorkers() => _repository.SelectWorkers();

        public WorkerReport GetReport(IWorker sender, IWorker worker, DateTime start, DateTime end)
        {
            var senderPersonMatchTimeRecord = string.Equals(sender.GetName(), worker.GetName());

            if (!senderPersonMatchTimeRecord &&
                !userRolesAllowedToViewReportAllWorkers.Contains(sender.GetRole()))
                return null;

            var roleTimeRecords = _repository.SelectTimeRecords(worker.GetRole());
            var workerTimeRecords = roleTimeRecords
                .Where(x => string.Equals(x.Name, worker.GetName(), StringComparison.OrdinalIgnoreCase)
                && x.Date.Date >= start.Date && x.Date.Date <= end.Date);

            var salary = worker.GetSalary(workerTimeRecords);
            var hours = workerTimeRecords.Sum(x => x.Hours);
            return new WorkerReport(workerTimeRecords, salary, hours);
        }
    }
}
