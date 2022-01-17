using NUnit.Framework;
using RSaitov.SoftwareDevelop.Domain;
using RSaitov.SoftwareDevelop.Persistence;
using System;
using System.Linq;

namespace RSaitov.SoftwareDevelop.SoftwareDevelopTests
{
    public class RepositoryTests
    {
        private IRepository repository;
        [SetUp]
        public void Setup()
        {
            repository = new TextFileDB();
        }

        [Test]
        public void SelectNotExistedPersons_Success()
        {
            var user = repository.SelectWorker("No user");
            Assert.Null(user);
        }
        [Test]
        public void SelectExistedPerson_Success()
        {
            var users = repository.SelectWorkers();
            if (users.Count() == 0)
                Assert.Pass("No users in DB");
            var firstUser = users.First();
            var user = repository.SelectWorker(firstUser.GetName());
            Assert.AreEqual(user.GetName(), firstUser.GetName());
            Assert.AreEqual(user.GetRole(), firstUser.GetRole());
        }
        [Test]
        public void SelectAllPersons_Success()
        {
            var users = repository.SelectWorkers();
            foreach (var user in users)
                Console.WriteLine($"{user.GetName()}, {user.GetRole()}");
            Assert.NotNull(users);
        }

        [Test]
        public void InsertRandomPerson_Success()
        {
            var worker = WorkerGenerator.CreateRandomWorker();
            var result = repository.InsertWorker(worker);
            Assert.IsTrue(result);
        }

        [Test]
        public void SelectTimeRecords_Success()
        {
            var timeRecordsManager = repository.SelectTimeRecords(UserRole.Manager);
            var timeRecordsEmployee = repository.SelectTimeRecords(UserRole.Employee);
            var timeRecordsFreelancer = repository.SelectTimeRecords(UserRole.Freelancer);

            Assert.NotNull(timeRecordsManager);
            Assert.NotNull(timeRecordsEmployee);
            Assert.NotNull(timeRecordsFreelancer);
        }
        [Test]
        public void InsertRandomTimeRecord_Success()
        {
            var rand = new Random();
            var workers = repository.SelectWorkers();
            var worker = repository.SelectWorkers().Skip(rand.Next(0, workers.Count() - 1)).First();
            var timeRecord = new TimeRecord(
                DateTime.Now.AddDays(-rand.Next(1,3)),
                worker.GetName(),
                (byte)rand.Next(1, 10),
                "some text"
            );
            var result = repository.InsertTimeRecord(timeRecord, worker.GetRole());
            Assert.IsTrue(result);
        }
    }
}