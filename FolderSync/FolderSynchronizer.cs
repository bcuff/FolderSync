using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FolderSync
{
    public class FolderSynchronizer : IDisposable
    {
        public string Source { get; }
        public string Dest { get; }
        public string Filter { get; }
        public bool Recursive { get; }
        FileSystemWatcher _watcher;

        public FolderSynchronizer(string source, string dest, string filter = null, bool recursive = true)
        {
            Source = Path.GetFullPath(source);
            Dest = Path.GetFullPath(dest);
            Filter = filter;
            Recursive = recursive;

            if (!Source.EndsWith("\\")) Source += "\\";
        }

        public void Start()
        {
            if (_watcher != null) throw new InvalidOperationException("Already started.");
            _watcher = new FileSystemWatcher(Source, Filter)
            {
                NotifyFilter = NotifyFilters.FileName | NotifyFilters.LastWrite | NotifyFilters.CreationTime,
                IncludeSubdirectories = Recursive,
            };
            _watcher.Changed += OnChange;
            _watcher.Created += OnChange;
            _watcher.Deleted += OnChange;
            _watcher.Renamed += OnRename;
            _watcher.Error += OnError;
            _watcher.EnableRaisingEvents = true;
        }

        private void OnError(object sender, ErrorEventArgs e)
        {
            Log(true, $"WATCHER ERROR - ${e.GetException()}");
        }

        public event Action<string> Info;

        public event Action<string> Error;

        private string GetDestPath(string sourcePath)
        {
            var subpath = sourcePath.Substring(Source.Length);
            return Path.Combine(Dest, subpath);
        }

        private void Log(bool error, string message)
        {
            var e = error ? Error : Info;
            e?.Invoke(message);
        }

        private void OnChange(object sender, FileSystemEventArgs e)
        {
            var destpath = GetDestPath(e.FullPath);
            if (e.ChangeType.HasFlag(WatcherChangeTypes.Deleted))
            {
                try
                {
                    File.Delete(destpath);
                }
                catch (Exception ex)
                {
                    Log(true, $"Failed to delete {destpath} - {ex}");
                    return;
                }
                Log(false, $"Deleted {destpath}");
                return;
            }
            if (e.ChangeType.HasFlag(WatcherChangeTypes.Changed) || e.ChangeType.HasFlag(WatcherChangeTypes.Created))
            {
                try
                {
                    File.Copy(e.FullPath, destpath, true);
                }
                catch (Exception ex)
                {
                    Log(true, $"Failed to copy {e.FullPath} to {destpath} - {ex}");
                    return;
                }
                Log(false, $"Copied {e.FullPath} to {destpath}");
            }
        }

        private void OnRename(object sender, RenamedEventArgs e)
        {
            var olddest = GetDestPath(e.OldFullPath);
            var newdest = GetDestPath(e.FullPath);
            if (File.Exists(olddest))
            {
                try
                {
                    File.Move(olddest, newdest);
                }
                catch (Exception ex)
                {
                    Log(true, $"Failed to rename {olddest} to {newdest} - {ex}");
                    return;
                }
                Log(false, $"Moved {olddest} to {newdest}.");
                return;
            }
            try
            {
                File.Copy(e.FullPath, newdest, true);
            }
            catch (Exception ex)
            {
                Log(true, $"Failed to copy {e.FullPath} to {newdest} - {ex}");
                return;
            }
            Log(false, $"Copied {e.FullPath} to {newdest}");
        }

        public void Dispose()
        {
            _watcher.Dispose();
        }
    }
}
