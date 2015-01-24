using clipr;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using clipr.Usage;

namespace ShinyBackupTool
{
    static class Program
    {
        [ApplicationInfo(Description = "ShinyBackupTool is designed to maintain a simple checksum database of files on your hard drive and backup drive and then produce recommendation for synchronization.")]
        private class Options
        {
            [Verb("check", "Checks the files and updates the database of checksums.")]
            public CheckOptions Check { get; set; }

            [Verb("update", "Compares two paths in the database and synchronizes target with the source.")]
            public UpdateOptions Update { get; set; }
        }

        private class CheckOptions
        {
            [PositionalArgument(0, MetaVar = "database")]
            public string Database { get; set; }

            [PositionalArgument(1, MetaVar = "path")]
            public string Path { get; set; }

            [NamedArgument('i', "ignore", Action = ParseAction.Append, Description = "Directory that shouldn't be checked, may be used more than once.")]
            public List<string> IgnoreDirectories { get; set; }
        }

        private class UpdateOptions
        {
            [PositionalArgument(0, MetaVar = "database")]
            public string Database { get; set; }

            [PositionalArgument(1, MetaVar = "source-path")]
            public string SourcePath { get; set; }

            [PositionalArgument(2, MetaVar = "target-path")]
            public string TargetPath { get; set; }

            [NamedArgument('r', "really-do-it", Description = "The files should be really synchronized. May delete files in target-path.")]
            public bool Really { get; set; }
        }

        static void Main(string[] args)
        {
            var options = CliParser.StrictParse<Options>(args);
            if (options.Check != null)
                Check(options.Check);
            else if (options.Update != null)
                Update(options.Update);

            Console.WriteLine("\n\n\nDone");
            Console.ReadKey();
        }

        private static void Update(UpdateOptions update)
        {
            throw new NotImplementedException();
        }

        private static void Check(CheckOptions check)
        {
            var rootDirectory = new DirectoryInfo(check.Path);
            var database = new Database();
            if (File.Exists(check.Database))
                database.Load(check.Database);
            var walker = new CheckDirectoryWalker(check.IgnoreDirectories);
            walker.Walk(rootDirectory);
            database.Save(check.Database);
        }
    }
}
