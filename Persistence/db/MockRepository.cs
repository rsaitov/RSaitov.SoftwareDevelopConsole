using System;
using System.Collections.Generic;
using System.Linq;

namespace RSaitov.SoftwareDevelop.Data.db
{
    public class MockRepository : IRepository
    {
        private List<WorkerDTO> _workers;
        private List<TimeRecord> _timeRecords = new List<TimeRecord>();

        public bool InsertTimeRecord(TimeRecord timeRecord, WorkerRole workerRole)
        {
            _timeRecords.Add(timeRecord);
            return true;
        }

        public bool InsertWorker(WorkerDTO worker)
        {
            _workers.Add(worker);
            return true;
        }

        public IEnumerable<TimeRecord> SelectTimeRecords(WorkerRole workerRole)
        {
            return _timeRecords;            
        }

        public WorkerDTO SelectWorker(string name)
        {
            return _workers.FirstOrDefault(x => string.Equals(x.Name, name, 
                StringComparison.OrdinalIgnoreCase));
        }

        public IEnumerable<WorkerDTO> SelectWorkers()
        {
            return _workers;
        }
    }
}
