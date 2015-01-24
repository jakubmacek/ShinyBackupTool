using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShinyBackupTool
{
    class CheckDirectoryWalker : DirectoryWalker
    {
        public string[] IgnoreDirectories { get; private set; }

        public CheckDirectoryWalker(IEnumerable<string> ignoreDirectories)
        {
            if (ignoreDirectories == null)
                ignoreDirectories = new string[0];
            IgnoreDirectories = ignoreDirectories.Select(x => x.ToLower()).ToArray();
            Array.Sort(IgnoreDirectories);
        }

        protected override bool ShouldRecurse(DirectoryInfo directory)
        {
            if (Array.BinarySearch(IgnoreDirectories, directory.FullName.ToLower()) >= 0)
                return false;
            return true;
        }

        protected override void DirectoryAction(DirectoryInfo directory)
        {
            Console.WriteLine(directory.FullName);
        }

        protected override void FileAction(FileInfo file)
        {
            Console.WriteLine(file.FullName);

            var fileRecord = new FileRecord();
            fileRecord.Id = Guid.NewGuid();
            fileRecord.Path = file.FullName;
            fileRecord.LastWrite = file.LastWriteTime;
            fileRecord.Size = file.Length;

            var samples = fileRecord.CalculateNewSamples(3); // HACK, bude se nasledne stejne pocitat jinde
            using (var stream = file.OpenRead())
            {
                foreach (var sample in samples)
                    sample.ComputeChecksum(stream);
                stream.Close();
            }
        }
    }
}
