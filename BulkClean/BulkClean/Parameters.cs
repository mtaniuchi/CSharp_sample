using System.IO;

namespace BulkClean
{
    /// <summary>
    /// ParseResult
    /// </summary>
    internal enum ParseResult
    {
        Failure = -1,
        Succeeded = 0,
    }

    /// <summary>
    /// Args parameters
    /// </summary>
    internal class Parameters
    {
        /// <summary>
        /// Target path for deleting recursive
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// Delete files older than n days
        /// </summary>
        public int Days { get; set; }

        /// <summary>
        /// Output deleted file paths and directories
        /// </summary>
        public bool OutputDeleted { get; set; }

        /// <summary>
        /// command line parameters
        /// </summary>
        private readonly string[] _args;

        /// <summary>
        /// constructor
        /// </summary>
        public Parameters(string[] args)
        {
            _args = args;
        }

        /// <summary>
        /// Parse args
        /// </summary>
        public ParseResult ParseArgs()
        {
            if (!(_args.Length == 2 || _args.Length == 3))
            {
                return ParseResult.Failure;
            }

            // path
            Path = _args[0];
            if (!Directory.Exists(Path))
            {
                Logger.LogError("Path is not exists:{0}", Path);
                return ParseResult.Failure;
            }

            // days
            int days;
            if (!int.TryParse(_args[1], out days))
            {
                Logger.LogError("Invalid days:{0}", _args[1]);
                return ParseResult.Failure;
            }
            Days = days;

            // outputlog
            if (_args.Length >= 3 && @"/outputlog".Equals(_args[2].ToLower()))
            {
                OutputDeleted = true;
            }

            return ParseResult.Succeeded;
        }
    }
}
