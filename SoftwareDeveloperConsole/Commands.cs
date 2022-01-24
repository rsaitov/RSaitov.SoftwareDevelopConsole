using RSaitov.SoftwareDevelop.Data;
using RSaitov.SoftwareDevelop.Domain;
using System;

namespace RSaitov.SoftwareDevelop.SoftwareDevelopConsole
{

    public interface ICommand
    {
        event SendMessage Notify;
        event ReadString ReadString;
        bool Access(IWorker sender);
        void Execute(IWorker sender);
        string Title();
    }

    internal class AddWorker : ICommand
    {
        public event SendMessage Notify;
        public event ReadString ReadString;

        public void Execute(IWorker sender)
        {
            Notify("Adding worker");
        }
        public bool Access(IWorker sender)
        {
            return sender.GetRole() == WorkerRole.Manager;
        }

        public string Title() => "Добавить сотрудника";
    }

    internal class ReportAllWorkers : ICommand
    {
        public event SendMessage Notify;
        public event ReadString ReadString;
        public void Execute(IWorker sender)
        {
            IRepository repository = new MockRepository();
            IService service = new Service(repository);
            var report = service.GetReportAllWorkers(sender, DateTime.Now.Date.AddDays(-7), DateTime.Now.Date);

            Notify($"Отчет за период с {report.Start.ToShortDateString()} по {report.End.ToShortDateString()}");
            foreach(var workerReport in report.SingleWorkerReports)
                Notify($"{workerReport.Worker.GetName()} отработал {workerReport.Hours} " +
                    $"часов и заработал за период {workerReport.Salary} рублей");
            Notify($"Всего часов отработано за период {report.HoursTotal}, " +
                $"сумма к выплате {report.SalaryTotal}");
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
        public event SendMessage Notify;
        public event ReadString ReadString;
        IRepository _repository;
        IService _service;
        private IWorker _sender;
        public ReportWorker(IWorker sender)
        {
            _repository = new MockRepository();
            _service = new Service(_repository);
            _sender = sender;
        }
        public void Execute(IWorker sender)
        {
            var worker = sender;
            if (_sender.GetRole() == WorkerRole.Manager)
            {
                var workerName = ReadString("Введите имя сотрудника: ");

                var workerFromEnteredName = _service.SelectWorker(workerName);
                if (ReferenceEquals(null, workerFromEnteredName))
                    Notify($"Невозможно найти сотрудника {workerName}");
                worker = workerFromEnteredName;
            }

            var dateStartString = ReadString("Введите дату начала в формате dd.MM.yyyy: ");
            DateTime dateStart;
            var resultDateStart = DateTime.TryParse(dateStartString, out dateStart);
            if (!resultDateStart)
                Notify("Ошибка даты");

            var dateEndString = ReadString("Введите дату начала в формате dd.MM.yyyy: ");
            DateTime dateEnd;
            var resultDateEnd = DateTime.TryParse(dateEndString, out dateEnd);
            if (!resultDateEnd)
                Notify("Ошибка даты");

            var report = _service.GetReportSingleWorker(sender, worker, dateStart, dateEnd);
            Notify($"Отчет по сотруднику: {report.Worker.GetName()} за период " +
                $"{report.Start.ToShortDateString()} по {report.End.ToShortDateString()}");
            foreach (var timeRecord in report.TimeRecords)
                Notify($"{timeRecord.Date.ToShortDateString()}: {timeRecord.Hours} часов, {timeRecord.Description}");
            Notify($"Итого: {report.Hours} часов, заработано: {report.Salary} руб");

        }
        public bool Access(IWorker sender) => true;

        public string Title() => _sender.GetRole() == WorkerRole.Manager ? 
            "Просмотреть отчет по конкретному сотруднику" :
            "Просмотреть собственный отчет";
    }

    internal class AddTimeRecord : ICommand
    {
        public event SendMessage Notify;
        public event ReadString ReadString;
        public void Execute(IWorker sender)
        {
            Notify("Adding time record");
        }
        public bool Access(IWorker sender) => true;

        public string Title() => "Добавить часы работы";
    }

    internal class Exit : ICommand
    {
        public event SendMessage Notify;
        public event ReadString ReadString;
        public void Execute(IWorker sender)
        {
            Environment.Exit(0);
        }
        public bool Access(IWorker sender) => true;

        public string Title() => "Выход из программы";
    }
}
