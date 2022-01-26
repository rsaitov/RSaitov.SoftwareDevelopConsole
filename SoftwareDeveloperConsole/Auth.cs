using RSaitov.SoftwareDevelop.Data;
using RSaitov.SoftwareDevelop.Domain;
using System;

namespace RSaitov.SoftwareDevelop.SoftwareDevelopConsole
{
    internal class AuthConsoleCommand
    {
        IService service = new Service(new MockRepository());
        public IWorker Execute()
        {
            Console.Write("Введите имя: ");
            var user = Console.ReadLine();
            var worker = service.GetWorker(user);

            return worker;
        }
    }
}
