using RSaitov.SoftwareDevelop.Data;
using RSaitov.SoftwareDevelop.Domain;
using System;

namespace RSaitov.SoftwareDevelop.SoftwareDevelopConsole
{
    internal class AuthConsoleCommand
    {
        private IService _service;
        public AuthConsoleCommand(IService service)
        {
            _service = service;
        }
        public IWorker Execute()
        {
            Console.Write("Введите имя: ");
            var user = Console.ReadLine();
            var worker = _service.GetWorker(user);

            return worker;
        }
    }
}
