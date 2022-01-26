using NUnit.Framework;
using RSaitov.SoftwareDevelop.Data;
using RSaitov.SoftwareDevelop.Domain;
using RSaitov.SoftwareDevelop.SoftwareDevelopTests;
using System;
using System.Linq;

namespace Test_Service
{
    public class ServiceTests
    {
        private IService service;
        private IRepository repository;
        [SetUp]
        public void Setup()
        {                        
            //repository = new TextFileDB();
            repository = new MockRepository();
            service = new Service(repository);
        }

        private IWorker GetRandomWorkerNotExisted()
        {
            while (true)
            {
                var worker = WorkerGenerator.CreateRandomIWorker();
                var userInDb = service.SelectWorker(worker.GetName());
                if (ReferenceEquals(null, userInDb))
                    return worker;
            }
        }

        [Test]
        public void CreateNotExistedWorker_Success()
        {
            IWorker worker = GetRandomWorkerNotExisted();
            var result = service.CreateWorker(CommonActions.GetFirstWorker(WorkerRole.Manager), worker);
            Assert.IsTrue(result);
        }

        [Test]
        public void CreateExistedUser_Fail()
        {
            var workers = service.SelectWorkers();
            if (workers.Count() == 0)
                Assert.Pass("No workers in DB");

            var result = service.CreateWorker(CommonActions.GetFirstWorker(WorkerRole.Manager), workers.First());
            Assert.IsFalse(result);
        }

        [Test]
        public void CreateWorkerSenderNoAccess_Fail()
        {
            IWorker worker = GetRandomWorkerNotExisted();
            var result = service.CreateWorker(CommonActions.GetFirstWorker(WorkerRole.Employee), worker);
            Assert.IsFalse(result);
        } 
    }
}
