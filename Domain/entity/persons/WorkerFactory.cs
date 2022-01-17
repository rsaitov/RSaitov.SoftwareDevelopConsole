namespace RSaitov.SoftwareDevelop.Domain
{
    public static class WorkerFactory
    {
        public static IWorker GenerateWorker(string name, UserRole role)
        {
            switch (role)
            {
                case UserRole.Manager:
                    return new Manager(name);
                case UserRole.Employee:
                    return new Employee(name);
                case UserRole.Freelancer:
                    return new Freelancer(name);
                default:
                    return null;
            }
            
        }
    }
}
