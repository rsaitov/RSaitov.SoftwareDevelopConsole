using RSaitov.SoftwareDevelop.Data;
using RSaitov.SoftwareDevelop.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RSaitov.SoftwareDevelop.SoftwareDevelopTests
{
    static class CommonActions
    {
        private static IService service;
        private static IRepository repository;
        static CommonActions()
        {
            //repository = new TextFileDB();
            repository = new MockRepository();
            service = new Service(repository);
        }

        public static IWorker GetFirstWorker(WorkerRole workerRole) => service.SelectWorkers().FirstOrDefault(x => x.GetRole() == workerRole);
    }
}
