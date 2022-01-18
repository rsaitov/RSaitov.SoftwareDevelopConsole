using System;

namespace RSaitov.SoftwareDevelop.Domain
{
    public static class WorkerGenerator
    {
        public static IWorker CreateRandomWorker()
        {
            string[] names = new string[] { "Jack", "Jane", "Ivan", "Mike", "Arnold", "Peter", "Kate" };
            string[] secondNames = new string[] { "Jackson", "Hammet", "Bowie", "Garett", "Brown", "Black", "Kane" };
            int[] userRoles = new int[] { 0, 1, 2 };
            var rnd = new Random();

            var name = $"{names[rnd.Next(names.Length)]} {secondNames[rnd.Next(secondNames.Length)]}";
            var userRole = userRoles[rnd.Next(userRoles.Length)];

            return WorkerFactory.GenerateWorker(name, (UserRole)userRole);
        }
    }
}
