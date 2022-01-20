using RSaitov.SoftwareDevelop.Data;
using System;
using System.Collections.Generic;
using System.Linq;

/*
 * БИЗНЕС-ЛОГИКА
 * обеспечивает валидацию выполняемых действий пользователя
 * 
 * аргумент (IWorker sender) = отправитель действия для проверки доступа к действию
 * методы без логики пробрасывают запрос в репозиторий
 * 
 * Что можно улучшить:
 * возвращать созданный объект при вызове CreateWorker
 * при ошибках возвращать описание ошибок вместо возврата false
 * запретить внесение рабочих часов в предстоящих датах
 * 
 */

namespace RSaitov.SoftwareDevelop.Domain
{
    public class Service : IService
    {
        private IRepository _repository = new TextFileDB();
        private HashSet<WorkerRole> workerRolesAllowedToCreateWorkers =
            new HashSet<WorkerRole> { WorkerRole.Manager };
        private HashSet<WorkerRole> workerRolesAllowedToCreateTimeRecordsForAllWorkers =
            new HashSet<WorkerRole> { WorkerRole.Manager };
        private HashSet<WorkerRole> workerRolesAllowedToCreateTimeRecordsForAnyDateInPast =
            new HashSet<WorkerRole> { WorkerRole.Manager, WorkerRole.Employee };
        private HashSet<WorkerRole> workerRolesAllowedToViewReportAllWorkers =
            new HashSet<WorkerRole> { WorkerRole.Manager };

        public Service()
        {
        }

        public bool CreateWorker(IWorker sender, IWorker worker)
        {
            if (!workerRolesAllowedToCreateWorkers.Contains(sender.GetRole()))
                return false;

            var workerInDb = _repository.SelectWorker(worker.GetName());
            if (ReferenceEquals(null, workerInDb))
                return _repository.InsertWorker(new WorkerDTO(worker.GetName(), worker.GetRole()));

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
                !workerRolesAllowedToCreateTimeRecordsForAllWorkers.Contains(sender.GetRole()))
                return false;

            if (timeRecordDayDiff > 2 &&
                !workerRolesAllowedToCreateTimeRecordsForAnyDateInPast.Contains(sender.GetRole()))
                return false;

            return _repository.InsertTimeRecord(timeRecord, timeRecordWorker.GetRole());
        }

        public IWorker SelectWorker(string name) { 
            var worker = _repository.SelectWorker(name);
            return ReferenceEquals(null, worker) ? null :
                WorkerFactory.GenerateWorker(worker.Name, worker.Role);
        }
        public IEnumerable<IWorker> SelectWorkers() { 
            var workerDTOs = _repository.SelectWorkers();

            var list = new HashSet<IWorker>();
            foreach (var workerDTO in workerDTOs)
            {
                var worker = WorkerFactory.GenerateWorker(workerDTO.Name, workerDTO.Role);
                list.Add(worker);
            }
            return list;
        }

        public ReportSingleWorker GetReportSingleWorker(IWorker sender, IWorker worker, DateTime start, DateTime end)
        {
            var senderPersonMatchTimeRecord = string.Equals(sender.GetName(), worker.GetName());

            if (!senderPersonMatchTimeRecord &&
                !workerRolesAllowedToViewReportAllWorkers.Contains(sender.GetRole()))
                return null;

            var roleTimeRecords = _repository.SelectTimeRecords(worker.GetRole());
            var workerTimeRecords = roleTimeRecords
                .Where(x => string.Equals(x.Name, worker.GetName(), StringComparison.OrdinalIgnoreCase)
                && x.Date.Date >= start.Date && x.Date.Date <= end.Date);

            var salary = worker.GetSalary(workerTimeRecords);
            var hours = workerTimeRecords.Sum(x => x.Hours);
            return new ReportSingleWorker(workerTimeRecords, salary, hours);
        }

        public ReportAllWorkers GetReportAllWorkers(IWorker sender, DateTime start, DateTime end)
        {
            var workers = SelectWorkers();
            var workerReports = new List<ReportSingleWorker>();
            foreach (var worker in workers)
                workerReports.Add(GetReportSingleWorker(sender, worker, start, end));
            var hoursTotal = workerReports.Sum(x => x.Hours);
            var salaryTotal = workerReports.Sum(x => x.Salary);
            return new ReportAllWorkers(workerReports, hoursTotal, salaryTotal);
        }
    }
}
