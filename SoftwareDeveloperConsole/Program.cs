using Autofac;
using log4net;
using log4net.Config;
using RSaitov.SoftwareDevelop.Data;
using RSaitov.SoftwareDevelop.Domain;
using System;
using System.Diagnostics.CodeAnalysis;

/*
 * Консольное приложение сотрудника
 * 
 * Что можно улучшить:
 * меню крутится в бесконечном цикле, команда "выход" завершает приложение
 * лучше чтобы цикл корректно завершался, получив команду "выход" от объекта MainMenu
 */

namespace RSaitov.SoftwareDevelop.SoftwareDevelopConsole
{
    [ExcludeFromCodeCoverage]
    static class Program
    {
        static void Main(string[] args)
        {
            IContainer container = ContainerConfig.Configure();

            using (var scope = container.BeginLifetimeScope())
            {
                var app = scope.Resolve<Application>();
                app.Run();
            }
        }
    }
}
