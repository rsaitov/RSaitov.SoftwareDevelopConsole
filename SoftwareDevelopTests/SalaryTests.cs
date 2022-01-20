using NUnit.Framework;
using RSaitov.SoftwareDevelop.Data;
using RSaitov.SoftwareDevelop.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RSaitov.SoftwareDevelop.SoftwareDevelopTests
{
    public class SalaryTests
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

        [Test]
        public void Manager_Success()
        {
            IWorker worker = GetFirstWorker(WorkerRole.Manager);
            var timeRecords = service.GetTimeRecords(worker);
            var salary = worker.GetSalary(timeRecords);
            Assert.AreEqual(37250, salary);
        }
        
        [Test]
        public void Employee_Success()
        {
            IWorker worker = GetFirstWorker(WorkerRole.Employee);
            var timeRecords = service.GetTimeRecords(worker);
            var salary = worker.GetSalary(timeRecords);
            Assert.AreEqual(34500, salary);
        }
        [Test]
        public void Freelancer_Success()
        {
            IWorker worker = GetFirstWorker(WorkerRole.Freelancer);
            var timeRecords = service.GetTimeRecords(worker);
            var salary = worker.GetSalary(timeRecords);
            Assert.AreEqual(26000, salary);
        }
    }
}
