using System;
using System.Collections.Generic;
using System.Text;

namespace RSaitov.SoftwareDevelop.SoftwareDevelopConsole
{
    public interface ICommand
    {
        object Execute();
        string Title();
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

    internal class ReportWorker : ICommand
    {
        public object Execute()
        {
            return "Showing report worker";
        }

        public string Title() => "Просмотреть отчет по конкретному сотруднику";
    }

    internal class AddTimeRecord : ICommand
    {
        public object Execute()
        {
            return "Adding time record";
        }

        public string Title() => "Добавить часы работы";
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
