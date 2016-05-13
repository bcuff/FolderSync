using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FolderSync.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            var basePath = args[0];
            var baseSource = Path.Combine(basePath, @"WithBuddies.Web.Admin.Core");
            var baseDest = Path.Combine(basePath, @"Dice.Web.Admin");
            var watchers = new[]
            {
                new FolderSynchronizer($"{baseSource}\\Scripts\\",$"{baseDest}\\Scripts\\CoreScripts\\", "*.js"),
                new FolderSynchronizer($"{baseSource}\\Content\\",$"{baseDest}\\Content\\CoreContent\\", "*.js"),
                new FolderSynchronizer($"{baseSource}\\Content\\",$"{baseDest}\\Content\\CoreContent\\", "*.less"),
                new FolderSynchronizer($"{baseSource}\\Content\\",$"{baseDest}\\Content\\CoreContent\\", "*.css"),
                new FolderSynchronizer($"{baseSource}\\Views\\",$"{baseDest}\\Views\\CoreViews\\", "*.cshtml"),
                //new FolderSynchronizer($"{baseSource}\\fonts\\",$"{baseDest}\\fonts\\CoreFonts\\"),
            };
            foreach (var watcher in watchers)
            {
                watcher.Info += msg => System.Console.WriteLine(msg);
                watcher.Error += msg => System.Console.Error.WriteLine(msg);
                watcher.Start();
                System.Console.WriteLine($"Watcher Started{Environment.NewLine}\tSource: {watcher.Source}{Environment.NewLine}\tDest: {watcher.Dest}{Environment.NewLine}\tFilter: {watcher.Filter}");
            }
            System.Console.WriteLine("Press enter to stop.");
            System.Console.ReadLine();
        }
    }
}
