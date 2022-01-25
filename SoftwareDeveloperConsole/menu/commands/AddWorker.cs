using RSaitov.SoftwareDevelop.Data;
using RSaitov.SoftwareDevelop.Domain;
using System;

namespace RSaitov.SoftwareDevelop.SoftwareDevelopConsole
{
    internal class AddWorker : ICommand
    {
        public event SendMessage Notify;
        public event ReadString ReadString;
        private readonly IService _service;

        public AddWorker(IService service)
        {
            _service = service;
        }

        public void Execute(IWorker sender)
        {
            var workerName = ReadString("Введите имя сотрудника: ");
            var workerRoleString = ReadString("Выберите роль сотрудника (1 - Manager, 2 - Employee, 3 - Freelancer): ");
            int workerRoleNumber = 0;
            var result = Int32.TryParse(workerRoleString, out workerRoleNumber);
            if (!result)
            {
                Notify("Ошибка: роль не распознана");
                return;
            }

            var worker = WorkerFactory.GenerateWorker(workerName, (WorkerRole)workerRoleNumber);
            var createWorkerResult = _service.CreateWorker(sender, worker);
            if (!createWorkerResult)
            {
                Notify("Ошибка: сотрудник не добавлен");
                return;
            }

            Notify("Сотрудник успешно добавлен");
        }
        public bool Access(IWorker sender)
        {
            return sender.GetRole() == WorkerRole.Manager;
        }

        public string Title() => "Добавить сотрудника";
    }
}
