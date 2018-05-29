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

        static void Main(string[] args)
        {
            Console.WriteLine("-------------------------------");
            Console.WriteLine($"CommitMap execution started at: {DateTime.UtcNow.ToShortTimeString()}");

            var builder = new ContainerBuilder();

            builder.RegisterModule<CommitMapDIModule>();
            container = builder.Build();

            var watch = Stopwatch.StartNew();
            using (var scope = container.BeginLifetimeScope())
            {
                var app = scope.Resolve<ICommitMapEngine>();
                app.Run();
            }
            watch.Stop();

            Console.WriteLine($"Execution finished at: {DateTime.UtcNow.ToShortTimeString()}");
            Console.WriteLine($"Execution time: {watch.Elapsed.TotalSeconds} seconds");
            Console.WriteLine("-------------------------------");
            Console.ReadKey();
        }
    }
}
