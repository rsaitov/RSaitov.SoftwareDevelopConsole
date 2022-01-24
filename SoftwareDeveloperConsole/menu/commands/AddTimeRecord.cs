using RSaitov.SoftwareDevelop.Data;

namespace RSaitov.SoftwareDevelop.SoftwareDevelopConsole
{
    internal class AddTimeRecord : ICommand
    {
        public event SendMessage Notify;
        public event ReadString ReadString;
        public void Execute(IWorker sender)
        {
            Notify("Adding time record");
        }
        public bool Access(IWorker sender) => true;

        public string Title() => "Добавить часы работы";
    }
}
