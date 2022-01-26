﻿using RSaitov.SoftwareDevelop.Data;
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
                var workerName = ReadString("Введите имя сотрудника: ");

                var workerFromEnteredName = _service.SelectWorker(workerName);
                if (ReferenceEquals(null, workerFromEnteredName))
                    Notify($"Невозможно найти сотрудника {workerName}");
                worker = workerFromEnteredName;
            }

            var dateStartString = ReadString("Введите дату начала в формате dd.MM.yyyy: ");
            var dateStart = UserEnteredValueParser.ParseDate(dateStartString, Notify);
            if (dateStart == DateTime.MinValue)
                return;

            var dateEndString = ReadString("Введите дату окончания в формате dd.MM.yyyy: ");
            var dateEnd = UserEnteredValueParser.ParseDate(dateEndString, Notify);
            if (dateEnd == DateTime.MinValue)
                return;

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
}