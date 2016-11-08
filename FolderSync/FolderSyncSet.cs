using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FolderSync
{
    public class FolderSyncSet
    {
        public string Base { get; set; }
        public string BaseSource { get; set; }
        public string BaseDest { get; set; }
        public FolderSyncConfig[] Policies { get; set; }

        public void Normalize()
        {
            if (!string.IsNullOrEmpty(Base))
            {
                if (!string.IsNullOrEmpty(BaseSource)) BaseSource = Path.Combine(Base, BaseSource);
                if (!string.IsNullOrEmpty(BaseDest)) BaseDest = Path.Combine(Base, BaseDest);
            }
            if (Policies == null) return;
            foreach (var policy in Policies)
            {
                if (!string.IsNullOrEmpty(BaseSource)) policy.Source = Path.Combine(BaseSource, policy.Source);
                if (!string.IsNullOrEmpty(BaseDest)) policy.Dest = Path.Combine(BaseDest, policy.Dest);
            }
        }
    }
}
