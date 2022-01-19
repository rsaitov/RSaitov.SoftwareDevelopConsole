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

        private IWorker GetFirstWorker(UserRole userRole) => service.SelectWorkers().FirstOrDefault(x => x.GetRole() == userRole);
        private IWorker GetRandomWorkerNotExisted()
        {
            while (true)
            {
                var worker = WorkerGenerator.CreateRandomWorker();
                var userInDb = service.SelectWorker(worker.GetName());
                if (ReferenceEquals(null, userInDb))
                    return worker;
            }
        }

        [Test]
        public void CreateNotExistedWorker_Success()
        {
            IWorker worker = GetRandomWorkerNotExisted();
            var result = service.CreateWorker(GetFirstWorker(UserRole.Manager), worker);
            Assert.IsTrue(result);
        }

        [Test]
        public void CreateExistedUser_Fail()
        {
            var workers = service.SelectWorkers();
            if (workers.Count() == 0)
                Assert.Pass("No workers in DB");

            var result = service.CreateWorker(GetFirstWorker(UserRole.Manager), workers.First());
            Assert.IsFalse(result);
        }

        [Test]
        public void CreateWorkerSenderNoAccess_Fail()
        {
            IWorker worker = GetRandomWorkerNotExisted();
            var result = service.CreateWorker(GetFirstWorker(UserRole.Employee), worker);
            Assert.IsFalse(result);
        }
        [Test]
        public void CreateOwnTimeRecord_Success()
        {
            var roles = new[] { UserRole.Manager, UserRole.Employee, UserRole.Freelancer };

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
            var roles = new[] { UserRole.Manager, UserRole.Employee, UserRole.Freelancer };
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
            var roles = new[] { UserRole.Manager, UserRole.Employee, UserRole.Freelancer };
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
        public void GetReportWorker()
        {
            IWorker sender = GetFirstWorker(UserRole.Manager);
            IWorker worker = GetFirstWorker(UserRole.Manager);

            var report = service.GetReport(sender, worker, DateTime.Now.AddDays(-14).Date, DateTime.Now.Date);
            Assert.NotNull(report);
        }
    }
}
