using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FolderSync
{
    public class FolderSyncConfig
    {
        public string Source { get; set; }
        public string Dest { get; set; }
        public string Filter { get; set; }
        public bool Recursive { get; set; }
    }
}
