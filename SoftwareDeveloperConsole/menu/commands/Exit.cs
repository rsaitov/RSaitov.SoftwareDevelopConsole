using RSaitov.SoftwareDevelop.Data;
using System;

namespace RSaitov.SoftwareDevelop.SoftwareDevelopConsole
{
    internal class Exit : ICommand
    {
        public event SendMessage Notify;
        public event ReadString ReadString;
        public void Execute(IWorker sender)
        {
            Environment.Exit(0);
        }
        public bool Access(IWorker sender) => true;

        public string Title() => "Выход из программы";
    }
}
