using System;
using System.Diagnostics;

using Autofac;

using CommitMap.DI;
using CommitMap.Services.Facade;

namespace CommitMap.DesktopApp
{
    class Program
    {
        private static IContainer container { get; set; }

        private const string Decorator      = "---------------------------------------------------";
        private const string WelcomeMessage = "--------------Welcome to CommitMap-----------------";

        static void Main(string[] args)
        {
            Console.WriteLine(Decorator);
            Console.WriteLine(WelcomeMessage);
            Console.WriteLine(Decorator);
            Console.WriteLine();

            if (args.Length != 2 
                || args[0] == string.Empty 
                || args[0].Length != 40 
                || args[1] == string.Empty 
                || args[1].Length != 40)
            {
                Console.WriteLine("Specify from/to commits in order to run this app");
                return;
            }

            string fromCommit = args[0];
            string toCommit = args[1];

            Console.WriteLine(Decorator);
            Console.WriteLine($"CommitMap execution started at: {DateTime.UtcNow.ToShortTimeString()}");
            
            var builder = new ContainerBuilder();

            builder.RegisterModule<CommitMapDIModule>();
            container = builder.Build();
            
            var watch = Stopwatch.StartNew();

            CommitMapRunResult result;
            using (var scope = container.BeginLifetimeScope())
            {
                var app = scope.Resolve<ICommitMapEngine>();
                result = app.Run(fromCommit, toCommit).Result;
            }
            watch.Stop();

            foreach (var controller in result.Controllers)
            {
                Console.WriteLine(controller.ToString());
            }

            Console.WriteLine(Decorator);
            Console.WriteLine($"Execution finished at: {DateTime.UtcNow.ToShortTimeString()}");
            Console.WriteLine($"Execution time: {watch.Elapsed.TotalSeconds} seconds");
            Console.WriteLine(Decorator);
            Console.ReadKey();
        }
    }
}
