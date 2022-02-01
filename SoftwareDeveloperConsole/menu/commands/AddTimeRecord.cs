using RSaitov.SoftwareDevelop.Data;
using RSaitov.SoftwareDevelop.Domain;
using System;

namespace RSaitov.SoftwareDevelop.SoftwareDevelopConsole
{
    internal class AddTimeRecord : ICommand
    {
        public event SendMessage Notify;
        public event ReadString ReadString;
        private readonly IService _service;
        public AddTimeRecord(IService service)
        {
            _service = service;
        }
        public void Execute(IWorker sender)
        {
            var worker = sender;
            if (sender.GetRole() == WorkerRole.Manager)
            {
                var workerName = ReadString?.Invoke("Введите имя сотрудника: ");
                worker = _service.GetWorker(workerName);
                if (ReferenceEquals(null, worker))
                {
                    Notify?.Invoke("Ошибка: сотрудник не найден");
                    return;
                }
            }

            var dateString = ReadString?.Invoke("Введите дату в формате dd.MM.yyyy: ");
            var date = UserEnteredValueParser.ParseDate(dateString, Notify);
            if (date == DateTime.MinValue)
                return;

            var hoursString = ReadString?.Invoke("Введите количество часов: ");
            int hours = UserEnteredValueParser.ParseInt(hoursString, Notify);
            if (hours == Int32.MinValue)
                return;

            var description = ReadString?.Invoke("Введите описание: ");

            var createTimeRecordResult = _service.AddTimeRecord(sender,
                new TimeRecord(date, worker.GetName(), (byte)hours, description));
            if (!createTimeRecordResult)
            {
                Notify?.Invoke("Ошибка: рабочие часы не добавлены");
                return;
            }

            Notify?.Invoke("Рабочие часы успешно добавлены");
        }
        public bool Access(IWorker sender) => true;

        public string Title() => "Добавить часы работы";

    }
}
