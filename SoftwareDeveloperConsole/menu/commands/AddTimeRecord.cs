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
            var workerName = ReadString("Введите имя сотрудника: ");
            var workerInDb = _service.SelectWorker(workerName);
            if (ReferenceEquals(null, workerInDb))
            {
                Notify("Ошибка: сотрудник не найден");
                return;
            }

            var dateString = ReadString("Введите дату в формате dd.MM.yyyy: ");
            var date = ConsoleReadlineValueParse.ParseDate(dateString, Notify);
            if (date == DateTime.MinValue)
                return;

            var hoursString = ReadString("Введите количество часов: ");
            int hours = 0;
            var hoursParseResult = Int32.TryParse(hoursString, out hours);
            if (!hoursParseResult)
            {
                Notify("Ошибка: количество часов не распознано");
                return;
            }

            var description = ReadString("Введите описание: ");

            var createTimeRecordResult = _service.AddTimeRecord(sender, new TimeRecord(date, workerName, (byte)hours, description));
            if (!createTimeRecordResult)
            {
                Notify("Ошибка: рабочие часы не добавлены");
                return;
            }

            Notify("Рабочие часы успешно добавлены");
        }
        public bool Access(IWorker sender) => true;

        public string Title() => "Добавить часы работы";

    }
}
