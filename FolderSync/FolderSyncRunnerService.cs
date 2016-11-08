using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace FolderSync
{
    public class FolderSyncRunnerService : ServiceBase
    {
        FolderSyncService _service;

        public FolderSyncRunnerService()
        {
            ServiceName = "FolderSync";
        }

        protected override void OnStart(string[] args)
        {
            var installPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var configPath = Path.Combine(installPath, "config.yaml");
            _service = FolderSyncService.FromConfigPath(configPath);
            _service.Start();
        }

        protected override void OnStop()
        {
            _service.Dispose();
            _service = null;
        }
    }
}
