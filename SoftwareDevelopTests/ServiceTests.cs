using NUnit.Framework;
using RSaitov.SoftwareDevelop.Data;
using RSaitov.SoftwareDevelop.Domain;
using System;
using System.Linq;

namespace RSaitov.SoftwareDevelop.SoftwareDevelopTests
{
    public class ServiceTests
    {
        private IService service;
        private IRepository repository = new MockRepository();
        [SetUp]
        public void Setup()
        {            
            //repository = new TextFileDB();
            repository = new MockRepository();
            service = new Service(repository);
        }

        private IWorker GetFirstWorker(WorkerRole workerRole) => service.SelectWorkers().FirstOrDefault(x => x.GetRole() == workerRole);
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
            var result = service.CreateWorker(GetFirstWorker(WorkerRole.Manager), worker);
            Assert.IsTrue(result);
        }

        [Test]
        public void CreateExistedUser_Fail()
        {
            var workers = service.SelectWorkers();
            if (workers.Count() == 0)
                Assert.Pass("No workers in DB");

            var result = service.CreateWorker(GetFirstWorker(WorkerRole.Manager), workers.First());
            Assert.IsFalse(result);
        }

        [Test]
        public void CreateWorkerSenderNoAccess_Fail()
        {
            IWorker worker = GetRandomWorkerNotExisted();
            var result = service.CreateWorker(GetFirstWorker(WorkerRole.Employee), worker);
            Assert.IsFalse(result);
        }
        [Test]
        public void CreateOwnTimeRecord_Success()
        {
            var roles = new[] { WorkerRole.Manager, WorkerRole.Employee, WorkerRole.Freelancer };

            foreach (var role in roles)
            {
                IWorker worker = GetFirstWorker(role);
                var rand = new Random();
                var timeRecord = new TimeRecord(
                    DateTime.Now.AddDays(-rand.Next(1, 2)),
                    worker.GetName(),
                    (byte)rand.Next(1, 10),
                    "created from Service test"
                );
                var result = service.AddTimeRecord(worker, timeRecord);
                Assert.IsTrue(result);
            }
        }

        [Test]
        public void CreateTimeRecordToOtherRole()
        {
            var roles = new[] { WorkerRole.Manager, WorkerRole.Employee, WorkerRole.Freelancer };
            var results = new bool[] {
                true, true,
                false, false,
                false, false
            };

            var counter = 0;
            foreach (var senderRole in roles)
                foreach (var role in roles.Where(x => x != senderRole))
                {
                    IWorker senderWorker = GetFirstWorker(senderRole);
                    IWorker worker = GetFirstWorker(role);

                    var rand = new Random();
                    var timeRecord = new TimeRecord(
                        DateTime.Now.AddDays(-rand.Next(1, 2)),
                        worker.GetName(),
                        (byte)rand.Next(1, 10),
                        "created from Service test"
                    );
                    var result = service.AddTimeRecord(senderWorker, timeRecord);
                    Assert.AreEqual(result, results[counter++]);
                }
        }

        [Test]
        public void CreateTimeRecordOldDate()
        {
            var roles = new[] { WorkerRole.Manager, WorkerRole.Employee, WorkerRole.Freelancer };
            var results = new bool[] { true, true, false };

            var counter = 0;
            foreach (var role in roles)
            {
                IWorker worker = GetFirstWorker(role);

                var rand = new Random();
                var timeRecord = new TimeRecord(
                    DateTime.Now.AddDays(-rand.Next(5, 6)),
                    worker.GetName(),
                    (byte)rand.Next(1, 10),
                    "created from Service test"
                );
                var result = service.AddTimeRecord(worker, timeRecord);
                Assert.AreEqual(result, results[counter++]);
            }
        }

        [Test]
        public void GetReportSingleWorker_Success()
        {
            IWorker sender = GetFirstWorker(WorkerRole.Manager);
            IWorker worker = GetFirstWorker(WorkerRole.Manager);

            var report = service.GetReportSingleWorker(sender, worker, DateTime.Now.AddDays(-14).Date, DateTime.Now.Date);
            Assert.NotNull(report);
        }

        [Test]
        public void GetReportSingleWorkerSenderNoAccess_Fail()
        {
            IWorker sender = GetFirstWorker(WorkerRole.Employee);
            IWorker worker = GetFirstWorker(WorkerRole.Manager);

            var report = service.GetReportSingleWorker(sender, worker, DateTime.Now.AddDays(-14).Date, DateTime.Now.Date);
            Assert.Null(report);
        }

        [Test]
        public void GetReportAllWorkers_Success()
        {
            IWorker sender = GetFirstWorker(WorkerRole.Manager);

            var report = service.GetReportAllWorkers(sender, DateTime.Now.AddDays(-14).Date, DateTime.Now.Date);
            Assert.NotNull(report);
        }
    }
}
