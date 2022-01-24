using RSaitov.SoftwareDevelop.Data;
using RSaitov.SoftwareDevelop.Domain;
using System;

namespace RSaitov.SoftwareDevelop.SoftwareDevelopConsole
{
    public interface ICommand
    {
        object Execute(IWorker sender);
        string Title();
        bool Access(IWorker sender);
    }

    internal class AddWorker : ICommand
    {
        public object Execute(IWorker sender)
        {
            return "Adding worker";
        }
        public bool Access(IWorker sender)
        {
            return sender.GetRole() == WorkerRole.Manager;
        }

        public string Title() => "Добавить сотрудника";
    }

    internal class ReportAllWorkers : ICommand
    {
        public object Execute(IWorker sender)
        {
            IRepository repository = new MockRepository();
            IService service = new Service(repository);
            var report = service.GetReportAllWorkers(sender, DateTime.Now.Date.AddDays(-7), DateTime.Now.Date);

            Console.WriteLine($"Отчет за период с {report.Start.ToShortDateString()} по {report.End.ToShortDateString()}");
            foreach(var workerReport in report.SingleWorkerReports)
                Console.WriteLine($"{workerReport.Worker.GetName()} отработал {workerReport.Hours} " +
                    $"часов и заработал за период {workerReport.Salary} рублей");

            Console.WriteLine($"Всего часов отработано за период {report.HoursTotal}, " +
                $"сумма к выплате {report.SalaryTotal}");
            return "";
        }
        public bool Access(IWorker sender)
        {
            return sender.GetRole() == WorkerRole.Manager;
        }

        public string Title() => "Просмотреть отчет по всем сотрудникам";
    }

    /*
     * Отчет по любому сотруднику доступен только менеджеру
     * Остальным типам сотрудников доступны только собственные отчеты
     */
    internal class ReportWorker : ICommand
    {
        IRepository _repository;
        IService _service;
        private IWorker _sender;
        public ReportWorker(IWorker sender)
        {
            _repository = new MockRepository();
            _service = new Service(_repository);
            _sender = sender;
        }
        public object Execute(IWorker sender)
        {
            var worker = sender;
            if (_sender.GetRole() == WorkerRole.Manager)
            {
                Console.Write("Введите имя сотрудника: ");
                var workerName = Console.ReadLine();
                
                var workerFromEnteredName = _service.SelectWorker(workerName);
                if (ReferenceEquals(null, workerFromEnteredName))
                    return $"Невозможно найти сотрудника {workerName}";
                worker = workerFromEnteredName;
            }

            Console.Write("Введите дату начала в формате dd.MM.yyyy: ");
            var dateStartString = Console.ReadLine();
            DateTime dateStart;
            var resultDateStart = DateTime.TryParse(dateStartString, out dateStart);
            if (!resultDateStart)
                return "Ошибка даты";

            Console.Write("Введите дату окончания в формате dd.MM.yyyy: ");
            var dateEndString = Console.ReadLine();
            DateTime dateEnd;
            var resultDateEnd = DateTime.TryParse(dateEndString, out dateEnd);
            if (!resultDateEnd)
                return "Ошибка даты";

            var report = _service.GetReportSingleWorker(sender, worker, dateStart, dateEnd);
            Console.WriteLine($"Отчет по сотруднику: {report.Worker.GetName()} за период " +
                $"{report.Start.ToShortDateString()} по {report.End.ToShortDateString()}");
            foreach (var timeRecord in report.TimeRecords)
                Console.WriteLine($"{timeRecord.Date.ToShortDateString()}: {timeRecord.Hours} часов, {timeRecord.Description}");
            Console.WriteLine($"Итого: {report.Hours} часов, заработано: {report.Salary} руб");

            return "";
        }
        public bool Access(IWorker sender) => true;

        public string Title() => _sender.GetRole() == WorkerRole.Manager ? 
            "Просмотреть отчет по конкретному сотруднику" :
            "Просмотреть собственный отчет";
    }

    internal class AddTimeRecord : ICommand
    {
        public object Execute(IWorker sender)
        {
            return "Adding time record";
        }
        public bool Access(IWorker sender) => true;

        public string Title() => "Добавить часы работы";
    }

    internal class Exit : ICommand
    {
        public object Execute(IWorker sender)
        {
            Environment.Exit(0);
            return null;
        }
        public bool Access(IWorker sender) => true;

        public string Title() => "Выход из программы";
    }
}
