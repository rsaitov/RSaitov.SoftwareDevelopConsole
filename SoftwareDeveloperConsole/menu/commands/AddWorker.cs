using RSaitov.SoftwareDevelop.Data;

namespace RSaitov.SoftwareDevelop.SoftwareDevelopConsole
{
    internal class AddWorker : ICommand
    {
        public event SendMessage Notify;
        public event ReadString ReadString;

        public void Execute(IWorker sender)
        {
            Notify("Adding worker");
        }
        public bool Access(IWorker sender)
        {
            return sender.GetRole() == WorkerRole.Manager;
        }

        public string Title() => "Добавить сотрудника";
    }
}
