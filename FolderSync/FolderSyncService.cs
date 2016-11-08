using System;
using System.IO;
using System.Linq;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace FolderSync
{
    public class FolderSyncService : IDisposable
    {
        public static FolderSyncService FromConfigPath(string path)
        {
            var deserializer = new DeserializerBuilder()
                .WithNamingConvention(new UnderscoredNamingConvention())
                .Build();
            FolderServiceConfig config;
            using (var fs = File.OpenRead(path))
            using (var reader = new StreamReader(fs))
            {
                config = (FolderServiceConfig)deserializer.Deserialize(reader, typeof(FolderServiceConfig));
            }
            return new FolderSyncService(config);
        }

        public event Action<string> Info;
        private void OnInfo(string message) => Info?.Invoke(message);
        public event Action<string> Error;
        private void OnError(string message) => Error?.Invoke(message);

        readonly FolderSynchronizer[] _synchronizers;

        public FolderSyncService(FolderServiceConfig config)
        {
            config.Normalize();
            var q = from area in config.Areas ?? Enumerable.Empty<FolderSyncSet>()
                    from policy in area.Policies ?? Enumerable.Empty<FolderSyncConfig>()
                    select new FolderSynchronizer(policy.Source, policy.Dest, policy.Filter, policy.Recursive);
            _synchronizers = q.ToArray();
            var onInfo = new Action<string>(OnInfo);
            var onError = new Action<string>(OnError);
            foreach (var synchronizer in _synchronizers)
            {
                synchronizer.Info += onInfo;
                synchronizer.Error += onError;
            }
        }

        public void Start()
        {
            foreach (var synchronizer in _synchronizers)
            {
                synchronizer.Start();
            }
        }

        public void Dispose()
        {
            foreach (var synchronizer in _synchronizers)
            {
                synchronizer.Dispose();
            }
        }
    }
}
