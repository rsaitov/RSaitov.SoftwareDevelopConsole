using RSaitov.SoftwareDevelop.Data;
using System;

/*
 * Консольное приложения сотрудника
 * 
 * Что можно улучшить:
 * меню крутится к бесконечном цикле, команда "выход" завершает приложение
 * лучше чтобы цикл корректно завершался, получив команду "выход" от объекта MainMenu
 */

namespace RSaitov.SoftwareDevelop.SoftwareDevelopConsole
{
    class Program
    {
        static void Main(string[] args)
        {

            var isUnauth = true;
            var auth = new AuthConsoleCommand();
            IWorker user = null;
            while (isUnauth)
            {
                var authResult = auth.Execute();
                if (ReferenceEquals(null, authResult))
                    Console.WriteLine("Worker not found");
                else
                {
                    isUnauth = false;
                    user = authResult;
                }
            }

            var mainMenuCommand = new MainMenu(user);
            var exit = false;
            while (!exit)
            {
                mainMenuCommand.ShowMenu();
                Console.Write("$ ");
                var commandNumber = Console.ReadLine();
                var command = mainMenuCommand.GetCommand(commandNumber);
                if (ReferenceEquals(null, command))
                {

                    Console.WriteLine("Невозможно распознать выбранную команду.");
                    continue;
                }
                var result = command.Execute();
                Console.WriteLine(result);
            }
        }
    }
}
