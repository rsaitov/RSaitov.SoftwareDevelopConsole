using RSaitov.SoftwareDevelop.Data;
using RSaitov.SoftwareDevelop.Domain;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RSaitov.SoftwareDevelop.SoftwareDevelopConsole
{
    public delegate void SendMessage(string message);

    public delegate string ReadString(string message);
    internal class MainMenu
    {
        public event SendMessage Notify;
        public event ReadString ReadString;
        private List<ICommand> _commands { get; }
        private IRepository _repository = new MockRepository();
        private IService _service;
        public MainMenu(IWorker worker)
        {
            _service = new Service(_repository);

            var addWorkerCommand = new AddWorker(_service);
            var reportAllWorkersCommand = new ReportAllWorkers(_service);
            var reportWorkerCommand = new ReportWorker(worker, _service);
            var addTimeRecordCommand = new AddTimeRecord();
            var exitCommand = new ExitCommand();

            addWorkerCommand.Notify += SendToNotify;
            reportAllWorkersCommand.Notify += SendToNotify;
            reportWorkerCommand.Notify += SendToNotify;
            addTimeRecordCommand.Notify += SendToNotify;
            exitCommand.Notify += SendToNotify;

            addWorkerCommand.ReadString += SendReadString;
            reportAllWorkersCommand.ReadString += SendReadString;
            reportWorkerCommand.ReadString += SendReadString;
            addTimeRecordCommand.ReadString += SendReadString;
            exitCommand.ReadString += SendReadString;

            _commands = new List<ICommand> {
                reportWorkerCommand,
                addTimeRecordCommand,
                exitCommand,
            };

            if (addWorkerCommand.Access(worker))
                _commands.Insert(0, addWorkerCommand);
            if (reportAllWorkersCommand.Access(worker))
                _commands.Insert(1, reportAllWorkersCommand);
        }
        private void SendToNotify(string message)
        {
            Notify(message);
        }
        private string SendReadString(string message)
        {
            return ReadString(message);
        }
        public ICommand GetCommand(string command)
        {
            int commandNumber;
            var result = Int32.TryParse(command, out commandNumber);
            if (!result)
                return null;

            return _commands[commandNumber - 1];
        }
        public void ShowMenu()
        {
            Console.WriteLine();
            Console.WriteLine("Выберите пункт меню:");

            var menu = string.Join("\r\n",
                _commands.Select((x, index) => $"   ({index + 1}). {x.Title()}"));
            Console.WriteLine(menu);
        }

        public string Title() => "";
    }
}
