using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Topshelf.Logging;

namespace FileConverterService
{
    class ConverterService
    {
        private FileSystemWatcher _watcher;
        private static readonly LogWriter _log = HostLogger.Get<ConverterService>();

        public bool Start()
        {
            _watcher = new FileSystemWatcher(@"c:\Temp\a","*_in.txt");
            _watcher.Created += FileCreated;

            _watcher.EnableRaisingEvents = true;

            return true;
        }

        private void FileCreated(object sender, FileSystemEventArgs e)
        {
            _log.InfoFormat("Starting conversion of '{0}'",e.FullPath);

            if (e.FullPath.Contains("bad_in"))
            {
                throw new NotSupportedException("Cannot convert");
            }

            string content = File.ReadAllText(e.FullPath);

            string uppperContent = content.ToUpperInvariant();

            var dir = Path.GetDirectoryName(e.FullPath);

            var convertedFileName = Path.GetFileName(e.FullPath) + ".converted";

            var convertedPath = Path.Combine(dir, convertedFileName);

            File.WriteAllText(convertedPath,uppperContent);
        }

        public bool Stop()
        {
            _watcher.Dispose();
            return true;
        }

        public bool Pause()
        {
            _watcher.EnableRaisingEvents = true;
            return true;
        }

        public bool Continue()
        {
            _watcher.EnableRaisingEvents = true;
            return true;
        }

        public void CustomCommand(int commandNumber)
        {
            _log.InfoFormat("Teste '{0}'",commandNumber);
        }
    }
}
