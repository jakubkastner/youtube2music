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
        /// <param name="fileOrDirectory">True, if it's file or directory</param>
        /// <param name="arguments">Program startup arguments</param>
        /// <param name="wait">True, if you want to wait for the program exit</param>
        /// <returns>true = success, false = error</returns>
        public static bool Program(string path, bool fileOrDirectory = true, string arguments = null, bool wait = false)
        {
            if (fileOrDirectory && (!File.Exists(path) || !Directory.Exists(path)))
            {
                // file or directory doesn't exist
                return false;
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
