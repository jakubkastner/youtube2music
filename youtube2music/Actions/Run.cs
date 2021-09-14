using System;
using System.Diagnostics;
using System.IO;
using System.Windows;

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
                if (fileOrDirectory && !FilesDirectories.Files.Exists(path)) return false;
                // directory
                if (!fileOrDirectory && !FilesDirectories.Directories.Exists(path)) return false;
            }

            try
            {
                // start new program
                ProcessStartInfo info = new ProcessStartInfo(path);
                if (!String.IsNullOrEmpty(arguments)) info.Arguments = arguments;

                Process program = new Process();
                program.StartInfo = info;
                program.StartInfo.CreateNoWindow = false;
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
