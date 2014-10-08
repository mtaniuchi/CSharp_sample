using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace BulkClean
{
    /// <summary>
    /// Program
    /// </summary>
    internal class Program
    {
        /// <summary>
        /// entry point
        /// </summary>
        static int Main(string[] args)
        {
            // set parameters
            var param = new Parameters(args);
            switch (param.ParseArgs())
            {
                case ParseResult.Failure:
                    Console.WriteLine("Usage:BulkClean.exe targetPath days [/outputLog]");
                    Console.WriteLine("\ttargetPath:target path for deleting recursive");
                    Console.WriteLine("\tdays:delete files older than n days");
                    Console.WriteLine("\toutputLog:output deleted file paths and directories");
                    return ExitCode.FAILURE;
                case ParseResult.Succeeded:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            // delete files recursive
            DeleteFiles(param);

            // delete directories recursive
            var directories = Directory.GetDirectories(param.Path, "*", SearchOption.AllDirectories)
                .OrderByDescending(x => x.Length);
            DeleteDirectories(directories, param);

            // delete parent itself
            DeleteDirectories(new List<string>{ param.Path}, param);

            return ExitCode.SUCCESS;
        }

        /// <summary>
        /// delete files
        /// </summary>
        private static void DeleteFiles(Parameters param)
        {
            var d = DateTime.Now.AddDays(-param.Days);
            foreach (var file in Directory.GetFiles(param.Path, "*", SearchOption.AllDirectories))
            {
                if (File.GetLastWriteTime(file) >= d) continue;
                File.Delete(file);
                Logger.LogDeleted(file, param);
            }
        }

        /// <summary>
        /// delete directories
        /// </summary>
        private static void DeleteDirectories(IEnumerable<string> directories, Parameters param)
        {
            foreach (var directory in directories)
            {
                try
                {
                    // Delete empty directory
                    Directory.Delete(directory);
                    Logger.LogDeleted(directory, param);
                }
                catch
                {
                    // Pokémon Exception Handling :)
                    Logger.LogError("can't delete:{0}", directory);
                }
            }
        }
    }
}
