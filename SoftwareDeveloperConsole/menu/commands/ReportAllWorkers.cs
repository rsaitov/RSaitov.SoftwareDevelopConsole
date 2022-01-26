using RSaitov.SoftwareDevelop.Data;
using RSaitov.SoftwareDevelop.Domain;
using System;

namespace RSaitov.SoftwareDevelop.SoftwareDevelopConsole
{
    internal class ReportAllWorkers : ICommand
    {
        public event SendMessage Notify;
        public event ReadString ReadString;
        private readonly IService _service;
        public ReportAllWorkers(IService service)
        {
            _service = service;
        }
        public void Execute(IWorker sender)
        {
            var defaultDateStart = "01.01.2022";
            var dateStartString = ReadString($"Введите дату начала в формате dd.MM.yyyy (по умолчанию {defaultDateStart}): ");
            var dateStart = UserEnteredValueParser.ParseDate(string.IsNullOrEmpty(dateStartString) ? defaultDateStart
                : dateStartString, Notify);
            if (dateStart == DateTime.MinValue)
                return;

            var defaultDateEnd = "31.12.2022";
            var dateEndString = ReadString($"Введите дату окончания в формате dd.MM.yyyy (по умолчанию {defaultDateEnd}): ");
            var dateEnd = UserEnteredValueParser.ParseDate(string.IsNullOrEmpty(dateEndString) ? defaultDateEnd 
                : dateEndString, Notify);
            if (dateEnd == DateTime.MinValue)
                return;

            var report = _service.GetReportAllWorkers(sender, dateStart, dateEnd);
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
}
