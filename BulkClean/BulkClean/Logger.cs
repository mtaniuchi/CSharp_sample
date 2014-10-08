using System;

namespace BulkClean
{
    /// <summary>
    /// Simple Logger
    /// </summary>
    internal static class Logger
    {
        /// <summary>
        /// Log for delete file
        /// </summary>
        public static void LogDeleted(string path, Parameters parameters)
        {
            if (!parameters.OutputDeleted) return;
            Console.WriteLine("Deleted:{0}", path);
        }

        /// <summary>
        /// Log for Error
        /// </summary>
        public static void LogError(string format, params object[] args)
        {
            Console.Error.WriteLine(@"Error:" + format, args);
        }
    }
}
