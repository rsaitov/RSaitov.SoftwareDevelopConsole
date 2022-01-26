using NUnit.Framework;
using RSaitov.SoftwareDevelop.Data;
using RSaitov.SoftwareDevelop.Domain;
using RSaitov.SoftwareDevelop.SoftwareDevelopTests;
using System;
using System.Linq;

namespace Test_Command
{
    public class ReportSingleWorkerTests
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

        [Test]
        public void GetReportSingleWorker_Success()
        {
            IWorker sender = CommonActions.GetFirstWorker(WorkerRole.Manager);
            IWorker worker = CommonActions.GetFirstWorker(WorkerRole.Manager);

            var report = service.GetReportSingleWorker(sender, worker, DateTime.Now.AddDays(-14).Date, DateTime.Now.Date);
            Assert.NotNull(report);
        }

        [Test]
        public void GetReportSingleWorkerSenderNoAccess_Fail()
        {
            IWorker sender = CommonActions.GetFirstWorker(WorkerRole.Employee);
            IWorker worker = CommonActions.GetFirstWorker(WorkerRole.Manager);

            var report = service.GetReportSingleWorker(sender, worker, DateTime.Now.AddDays(-14).Date, DateTime.Now.Date);
            Assert.Null(report);
        }
    }
}
