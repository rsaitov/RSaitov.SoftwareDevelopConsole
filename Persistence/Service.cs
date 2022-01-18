using RSaitov.SoftwareDevelop.Domain;
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

namespace RSaitov.SoftwareDevelop.Persistence
{
    public interface IService
    {
        bool CreateWorker(IWorker sender, IWorker worker);
        IWorker SelectWorker(string name);
        IEnumerable<IWorker> SelectWorkers();

        bool AddTimeRecord(IWorker sender, TimeRecord timeRecord);
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
    }
}
