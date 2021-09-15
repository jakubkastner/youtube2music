using System;
using System.Diagnostics;

namespace youtube2music.Actions
{
    /// <summary>
    /// Runs the program.
    /// </summary>
    public static class Run
    {
        /// <summary>
        /// Runs the program or opens web browser, windows explorer.
        /// </summary>
        /// <param name="path">Path to the program, directory, file or web adress</param>
        /// <param name="check">true = check path for files or directories, false = doesn't check the path</param>
        /// <param name="fileOrDirectory">true = file, false = directory</param>
        /// <param name="arguments">Program startup arguments</param>
        /// <param name="wait">true = wait for the program exit, false = doesn't wait for exit</param>
        /// <returns>true = success, false = error</returns>
        public static bool Program(string path, bool check = false, bool fileOrDirectory = true, string arguments = null, bool wait = false)
        {
            if (String.IsNullOrEmpty(path)) return false;
            path = path.Trim();

            // check path
            if (check)
            {
                // file
                if (fileOrDirectory && !FD.Files.Exists(path)) return false;
                // directory
                if (!fileOrDirectory && !FD.Directories.Exists(path)) return false;
            }

            try
            {
                // setup arguments
                ProcessStartInfo info = new ProcessStartInfo(path);
                info.CreateNoWindow = false;
                if (!String.IsNullOrEmpty(arguments)) info.Arguments = arguments;

                // start new program
                Process program = new Process();
                program.StartInfo = info;
                program.Start();

                if (wait)
                {
                    // wait for exit the program
                    program.WaitForExit();
                }
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }
    }
}
