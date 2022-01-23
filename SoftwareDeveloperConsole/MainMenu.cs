using RSaitov.SoftwareDevelop.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RSaitov.SoftwareDevelop.SoftwareDevelopConsole
{
    internal class MainMenu
    {
        private List<ICommand> _commands { get; }
        public MainMenu(IWorker worker)
        {
            _commands = new List<ICommand> {
                new AddWorker(),
                new ReportAllWorkers(),
                new ReportWorker(),
                new AddTimeRecord(),
                new Exit(),
            };
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
