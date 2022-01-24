using RSaitov.SoftwareDevelop.Data;
using RSaitov.SoftwareDevelop.Domain;
using System;

namespace RSaitov.SoftwareDevelop.SoftwareDevelopConsole
{
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
}
