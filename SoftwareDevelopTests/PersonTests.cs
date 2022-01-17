using NUnit.Framework;
using RSaitov.SoftwareDevelop.Domain;
using RSaitov.SoftwareDevelop.Persistence;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace RSaitov.SoftwareDevelop.SoftwareDevelopTests
{
    public class PersonTests
    {
        IRepository repository;
        [SetUp]
        public void Setup()
        {
            repository = new TextFileDB();
        }        

        public void Manager_Salary_Success()
        {
            var timeRecords = new List<TimeRecord> {
                new TimeRecord(DateTime.Now.AddDays(-3), "Rinat", 8, "description 1"),
                new TimeRecord(DateTime.Now.AddDays(-2), "Rinat", 9, "description 1"),
                new TimeRecord(DateTime.Now.AddDays(-1), "Rinat", 7, "description 1"),
            };
        }
    }
}
