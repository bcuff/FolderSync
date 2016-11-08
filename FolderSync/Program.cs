using System;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Text.RegularExpressions;

namespace FolderSync
{
    class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            if (args.Length >= 1 && args.Any(a => Regex.IsMatch(a, @"(--|-|/)Console", RegexOptions.IgnoreCase)))
            {
                var configPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "config.yaml");
                using (var service = FolderSyncService.FromConfigPath(configPath))
                {
                    service.Info += Console.WriteLine;
                    service.Error += Console.Error.WriteLine;
                    service.Start();
                    Console.WriteLine($"Watcher Started...");
                    Console.WriteLine("Press enter to stop.");
                    Console.ReadLine();
                }
            }
            else
            {
                ServiceBase[] ServicesToRun;
                ServicesToRun = new ServiceBase[]
                {
                    new FolderSyncRunnerService()
                };
                ServiceBase.Run(ServicesToRun);
            }
        }
    }
}
