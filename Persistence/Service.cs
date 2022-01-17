using RSaitov.SoftwareDevelop.Domain;
using System;
using System.Collections.Generic;

/*
 * Что можно улучшить:
 * - возвращать объект при создании CreateWorker
 * 
 */

namespace RSaitov.SoftwareDevelop.Persistence
{
    public interface IService
    {
        bool CreateWorker(IWorker worker);
        IWorker SelectWorker(string name);
        IEnumerable<IWorker> SelectWorkers();
    }
    public class Service : IService
    {
        private IRepository _repository;

        public Service()
        {
            _repository = new TextFileDB();
        }
        public bool CreateWorker(IWorker worker)
        {
            var workerInDb = _repository.SelectWorker(worker.GetName());
            if (ReferenceEquals(null, workerInDb))
                return _repository.InsertWorker(worker);
            
            return false;
        }

        public IWorker SelectWorker(string name) => _repository.SelectWorker(name);
        public IEnumerable<IWorker> SelectWorkers() => _repository.SelectWorkers();
    }
}
