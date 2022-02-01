using RSaitov.SoftwareDevelop.Data;
using RSaitov.SoftwareDevelop.Domain;
using System;

namespace RSaitov.SoftwareDevelop.SoftwareDevelopConsole
{
    /*
     * Отчет по любому сотруднику доступен только менеджеру
     * Остальным типам сотрудников доступны только собственные отчеты
     */
    internal class ReportWorker : ICommand
    {
        public event SendMessage Notify;
        public event ReadString ReadString;

        private readonly IService _service;
        private IWorker _sender;
        public ReportWorker(IWorker sender, IService service)
        {
            _sender = sender;
            _service = service;
        }
        public void Execute(IWorker sender)
        {
            var worker = sender;
            if (_sender.GetRole() == WorkerRole.Manager)
            {
                var workerName = ReadString?.Invoke("Введите имя сотрудника: ");

                var workerFromEnteredName = _service.GetWorker(workerName);
                if (ReferenceEquals(null, workerFromEnteredName))
                    Notify?.Invoke($"Невозможно найти сотрудника {workerName}");
                worker = workerFromEnteredName;
            }

            if (ReferenceEquals(null, sender) || ReferenceEquals(null, worker))
            {
                Notify?.Invoke($"Сотрудник не определён");
                return;
            }

            var defaultDateStart = $"01.01.{DateTime.Now.Year}";
            var dateStartString = ReadString?.Invoke($"Введите дату начала в формате dd.MM.yyyy (по умолчанию {defaultDateStart}): ");
            var dateStart = UserEnteredValueParser.ParseDate(string.IsNullOrEmpty(dateStartString) ? defaultDateStart
                : dateStartString, Notify);
            if (dateStart == DateTime.MinValue)
                return;

            var defaultDateEnd = $"31.12.{DateTime.Now.Year}";
            var dateEndString = ReadString?.Invoke($"Введите дату окончания в формате dd.MM.yyyy (по умолчанию {defaultDateEnd}): ");
            var dateEnd = UserEnteredValueParser.ParseDate(string.IsNullOrEmpty(dateEndString) ? defaultDateEnd
                : dateEndString, Notify);
            if (dateEnd == DateTime.MinValue)
                return;

            var report = _service.GetReportSingleWorker(sender, worker, dateStart, dateEnd);
            Notify?.Invoke($"Отчет по сотруднику: {report.Worker.GetName()} за период " +
                $"{report.Start.ToShortDateString()} по {report.End.ToShortDateString()}");
            foreach (var timeRecord in report.TimeRecords)
                Notify?.Invoke($"{timeRecord.Date.ToShortDateString()}: {timeRecord.Hours} часов, {timeRecord.Description}");
            Notify?.Invoke($"Итого: {report.Hours} часов, заработано: {report.Salary} руб");

        }
        public bool Access(IWorker sender) => true;

        public string Title() => _sender.GetRole() == WorkerRole.Manager ? 
            "Просмотреть отчет по конкретному сотруднику" :
            "Просмотреть собственный отчет";
    }
}
