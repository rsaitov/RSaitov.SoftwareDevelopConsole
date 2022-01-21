using System;
using System.Collections.Generic;
using RSaitov.SoftwareDevelop.Data;
using RSaitov.SoftwareDevelop.Domain;

namespace RSaitov.SoftwareDevelop.SoftwareDevelopConsole
{
    class Program
    {
        static void Main(string[] args)
        {

            var isUnauth = true;
            ICommand authCommand = new AuthConsoleCommand();
            MainMenu mainMenuCommand = null;

            ICommand nextCommand = authCommand;

            while (nextCommand != null)
            {
                if (!ReferenceEquals(null, mainMenuCommand))
                    Console.WriteLine(mainMenuCommand.Execute());

                var result = nextCommand.Execute();

                if (isUnauth)
                {
                    if (ReferenceEquals(null, result))
                        Console.WriteLine("Worker not found");
                    else
                    {
                        Console.WriteLine($"Welcome, {((IWorker)result).GetName()}!");
                        mainMenuCommand = new MainMenu((IWorker)result);
                        nextCommand = mainMenuCommand;
                        isUnauth = false;
                    }
                    continue;
                }

                Console.WriteLine(result);
                Console.Write("$ ");
                var command = Console.ReadLine();
                nextCommand = mainMenuCommand.GetCommand(command);
            }
        }
    }
}
