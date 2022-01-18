using RSaitov.SoftwareDevelop.Domain;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace RSaitov.SoftwareDevelop.Persistence
{
    public interface IRepository
    {
        IWorker SelectWorker(string name);
        IEnumerable<IWorker> SelectWorkers();
        bool InsertWorker(IWorker person);

        IEnumerable<TimeRecord> SelectTimeRecords(UserRole userRole);
        bool InsertTimeRecord(TimeRecord timeRecord, UserRole userRole);
    }

    public class TextFileDB : IRepository
    {
        private static string _personsFile = @"C:\tmp\SoftwareDevelopFiles\person.csv";
        private static string _managerHoursFile = @"C:\tmp\SoftwareDevelopFiles\manager_hours.csv";
        private static string _employeeHoursFile = @"C:\tmp\SoftwareDevelopFiles\employee_hours.csv";
        private static string _freelanceHoursFile = @"C:\tmp\SoftwareDevelopFiles\freelance_hours.csv";

        private static string[] _filesDB = new string[] { _personsFile, _managerHoursFile, _employeeHoursFile, _freelanceHoursFile };

        public TextFileDB()
        {
            CreateFilesIfNotExist();
        }
        private static void CreateFilesIfNotExist()
        {
            foreach (var file in _filesDB)
                CreateFileIfNotExist(file);
        }
        private static void CreateFileIfNotExist(string fileNameWithPath)
        {
            if (!File.Exists(fileNameWithPath))
                File.Create(fileNameWithPath).Close();
        }
        private string ChooseWorkerFileByUserRole(UserRole userRole)
        {
            string fileName = "";
            switch (userRole)
            {
                case UserRole.Manager:
                    fileName = _managerHoursFile;
                    break;
                case UserRole.Employee:
                    fileName = _employeeHoursFile;
                    break;
                case UserRole.Freelancer:
                    fileName = _freelanceHoursFile;
                    break;
            }
            return fileName;
        }

        public IEnumerable<IWorker> SelectWorkers()
        {
            var list = new HashSet<IWorker>();
            using (var file = File.OpenText(_personsFile))
            {
                string s = "";
                while ((s = file.ReadLine()) != null)
                {
                    var stringMas = s.Split(',');
                    var worker = WorkerFactory.GenerateWorker(stringMas[0], (UserRole)Int32.Parse(stringMas[1]));
                    list.Add(worker);
                }
            }
            return list;
        }
        public bool InsertWorker(IWorker worker)
        {
            var row = string.Join(",", worker.GetName(), (int)worker.GetRole());
            using (var sw = File.AppendText(_personsFile))
                sw.WriteLine(row);
            return true;
        }

        public IWorker SelectWorker(string name)
        {
            var persons = SelectWorkers();
            return persons.FirstOrDefault(p => string.Equals(p.GetName(), name, StringComparison.OrdinalIgnoreCase));
        }

        public IEnumerable<TimeRecord> SelectTimeRecords(UserRole userRole)
        {
            var list = new List<TimeRecord>();
            string fileName = ChooseWorkerFileByUserRole(userRole);
            using (var file = File.OpenText(fileName))
            {
                string s = "";
                while ((s = file.ReadLine()) != null)
                {
                    var stringMas = s.Split(',');
                    var timeRecord = new TimeRecord(
                        Convert.ToDateTime(stringMas[0]),
                        stringMas[1].ToString(),
                        Convert.ToByte(stringMas[2]),
                        stringMas[3].ToString()
);
                    list.Add(timeRecord);
                }
            }
            return list;
        }

        public bool InsertTimeRecord(TimeRecord timeRecord, UserRole userRole)
        {
            var row = string.Join(",",
                timeRecord.Date.ToShortDateString(),
                timeRecord.Name,
                timeRecord.Hours,
                timeRecord.Description
            );
            var fileName = ChooseWorkerFileByUserRole(userRole);
            using (var sw = File.AppendText(fileName))
                sw.WriteLine(row);
            return true;
        }
    }
}
