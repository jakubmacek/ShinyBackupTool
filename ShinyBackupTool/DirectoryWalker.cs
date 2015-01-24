using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShinyBackupTool
{
    internal abstract class DirectoryWalker
    {
        protected abstract void FileAction(FileInfo file);
        protected abstract void DirectoryAction(DirectoryInfo directory);
        protected abstract bool ShouldRecurse(DirectoryInfo directory);

        public void Walk(DirectoryInfo directory)
        {
            try
            {
                foreach (var file in directory.EnumerateFiles())
                    FileAction(file);

                foreach (var subdirectory in directory.EnumerateDirectories())
                    if (ShouldRecurse(subdirectory))
                        Walk(subdirectory);

                DirectoryAction(directory);
            }
            catch (UnauthorizedAccessException) // directory cannot be walked, just ignore
            {
            }
        }
    }
}
