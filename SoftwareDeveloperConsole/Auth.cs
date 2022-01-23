using RSaitov.SoftwareDevelop.Data;
using RSaitov.SoftwareDevelop.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RSaitov.SoftwareDevelop.SoftwareDevelopConsole
{    
    internal class AuthConsoleCommand
    {
        IService service = new Service(new MockRepository());
        public IWorker Execute()
        {
            Console.Write("Введите имя: ");
            var user = Console.ReadLine();
            var worker = service.SelectWorker(user);

            return worker;
        }
    }
}
