using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FolderSync
{
    public class FolderServiceConfig
    {
        public FolderSyncSet[] Areas { get; set; }

        public void Normalize()
        {
            if (Areas == null) return;
            foreach (var area in Areas)
            {
                area.Normalize();
            }
        }
    }
}
