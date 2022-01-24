using RSaitov.SoftwareDevelop.Data;
using System;

/*
 * Консольное приложение сотрудника
 * 
 * Что можно улучшить:
 * меню крутится в бесконечном цикле, команда "выход" завершает приложение
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
                    Console.WriteLine("Сотрудник не найден");
                else
                {
                    isUnauth = false;
                    user = authResult;

                    Console.WriteLine();
                    Console.WriteLine("Добро пожаловать!");
                    Console.WriteLine($"Ваша роль в системе: {user.GetRole()}.");
                }
            }

            var mainMenuCommand = new MainMenu(user);
            mainMenuCommand.Notify += ShowConsoleMessage;
            mainMenuCommand.ReadString += ReadLineString;
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
                Console.WriteLine();
                command.Execute(user);
            }
        }

        static void ShowConsoleMessage(string message)
        {
            Console.WriteLine(message);
        }
        static string ReadLineString(string message)
        {
            Console.Write(message);
            return Console.ReadLine();
        }
    }
}
