using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using RSaitov.SoftwareDevelop.Domain;
using RSaitov.SoftwareDevelop.Persistence;

namespace RSaitov.SoftwareDevelop.SoftwareDevelopTests
{
    public class ServiceTests
    {
        private IService service;
        [SetUp]
        public void Setup()
        {
            service = new Service();
        }

        [Test]
        public void CreateNotExistedWorker_Success()
        {
            IWorker worker;
            while (true)
            {
                worker = WorkerGenerator.CreateRandomWorker();
                var userInDb = service.SelectWorker(worker.GetName());
                if (ReferenceEquals(null, userInDb))
                    break;
            }

            var result = service.CreateWorker(worker);
            Assert.IsTrue(result);
        }

        [Test]
        public void CreateExistedUser_Fail()
        {
            var workers = service.SelectWorkers();
            if (workers.Count() == 0)
                Assert.Pass("No workers in DB");

            var result = service.CreateWorker(workers.First());
            Assert.IsFalse(result);
        }
    }
}
