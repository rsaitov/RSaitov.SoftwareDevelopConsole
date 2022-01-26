using NUnit.Framework;
using RSaitov.SoftwareDevelop.Data;
using RSaitov.SoftwareDevelop.Domain;
using RSaitov.SoftwareDevelop.SoftwareDevelopTests;
using System.Linq;

namespace Test_Salary
{
    public class SalaryTests
    {
        private IService service = CommonActions.Service;

        [SetUp]
        public void Setup()
        {
        }
        
        [Test]
        public void Manager_Success()
        {
            IWorker worker = CommonActions.GetFirstWorker(WorkerRole.Manager);
            var timeRecords = service.GetTimeRecords(worker);
            var salary = worker.GetSalary(timeRecords);
            Assert.AreEqual(37250, salary);
        }
        
        [Test]
        public void Employee_Success()
        {
            IWorker worker = CommonActions.GetFirstWorker(WorkerRole.Employee);
            var timeRecords = service.GetTimeRecords(worker);
            var salary = worker.GetSalary(timeRecords);
            Assert.AreEqual(34500, salary);
        }
        [Test]
        public void Freelancer_Success()
        {
            IWorker worker = CommonActions.GetFirstWorker(WorkerRole.Freelancer);
            var timeRecords = service.GetTimeRecords(worker);
            var salary = worker.GetSalary(timeRecords);
            Assert.AreEqual(26000, salary);
        }
    }
}
