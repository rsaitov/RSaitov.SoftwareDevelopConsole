using Autofac;
using RSaitov.SoftwareDevelop.Data;
using RSaitov.SoftwareDevelop.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace RSaitov.SoftwareDevelop.SoftwareDevelopConsole
{
    public static class ContainerConfig
    {
        public static IContainer Configure()
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<Application>().AsSelf();

            builder.RegisterType<TextFileDB>().As<IRepository>();
            builder.RegisterType<Service>().As<IService>();

            return builder.Build();
        }
    }
}
