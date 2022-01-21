using RSaitov.SoftwareDevelop.Data;
using RSaitov.SoftwareDevelop.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RSaitov.SoftwareDevelop.SoftwareDevelopConsole
{
    public interface ICommand
    {
        object Execute();
        string Title();
    }
    internal class AuthConsoleCommand : ICommand
    {
        IService service = new Service(new MockRepository());
        public object Execute()
        {
            Console.Write("Enter your name: ");
            var user = Console.ReadLine();
            var worker = service.SelectWorker(user);

            return worker;
        }

        public string Title() => "Введите имя: ";
    }

    internal class MainMenu : ICommand
    {
        private List<ICommand> _commands { get;  }
        public MainMenu(IWorker worker)
        {
            _commands = new List<ICommand> {
                new AddWorker(),
                new ReportAllWorkers(),
                new Exit(),
            };
        }
        public ICommand GetCommand(string command)
        {
            int commandNumber;
            var result = Int32.TryParse(command, out commandNumber);
            if (!result)
                return this;
            
            return _commands[commandNumber - 1];
        }
        public object Execute()
        {
            var menu = string.Join("\r\n", 
                _commands.Select((x, index) => $"   ({index+1}). {x.Title()}"));
            return menu;
        }

        public string Title() => "";
    }

    internal class AddWorker : ICommand
    {
        public object Execute()
        {
            return "Adding worker";
        }

        public string Title() => "Добавить сотрудника";
    }

    internal class ReportAllWorkers : ICommand
    {
        public object Execute()
        {
            return "Showing report all workers";
        }

        public string Title() => "Просмотреть отчет по всем сотрудникам";
    }

    internal class Exit : ICommand
    {
        public object Execute()
        {
            Environment.Exit(0);
            return null;
        }

        public string Title() => "Выход из программы";
    }
}
