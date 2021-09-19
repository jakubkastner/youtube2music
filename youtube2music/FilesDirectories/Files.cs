using System;
using System.Collections.Generic;
using System.IO;

namespace youtube2music.FD
{
    public static class Files
    {
        /// <summary>
        /// Gets content (reads and saves to list) line by line for the specific file.
        /// </summary>
        /// <param name="path">File path</param>
        /// <returns>The contents of the file line by line as list</returns>
        public static List<string> Read(string path)
        {
            // file doesn't exists
            if (!Exists(path)) return null;

            List<string> lines = new List<string>();
            try
            {
                using (FileStream stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                using (StreamReader reader = new StreamReader(stream))
                {
                    // read lines from file
                    while (!reader.EndOfStream)
                    {
                        // save lines to list
                        lines.Add(reader.ReadLine());
                    }
                }
            }
            catch (Exception)
            {
                return null;
            }
            return lines;
        }

        /// <summary>
        /// Write content (line by line) from list to the file.
        /// </summary>
        /// <param name="path">File path</param>
        /// <param name="content">Content to writing to file.</param>
        /// <param name="rewrite">true = rewrite all content in the file, false = append the content o end of the file</param>
        /// <returns>true = success, false = error</returns>
        public static bool Write(string path, List<string> content, bool rewrite = true)
        {
            // file doesn't exists
            if (!Exists(path)) return false;

            FileMode writeMode = FileMode.Append;
            if (rewrite)
            {
                writeMode = FileMode.Create;
            }
            try
            {
                using (FileStream stream = new FileStream(path, writeMode, FileAccess.Write))
                {
                    using (StreamWriter writer = new StreamWriter(stream))
                    {
                        foreach (string line in content)
                        {
                            // writes lines to file
                            writer.WriteLine(line);
                        }
                    }
                }
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }


        /// <summary>
        /// Move file to new directory.
        /// </summary>
        /// <param name="filePath">File path</param>
        /// <param name="destinationDirectory">The destination directory where the file is to be moved</param>
        /// <param name="newFileName">New name of the moved file (if not filled in, the original name will be used)</param>
        /// <returns>true = success, false = error</returns>
        public static bool Move(string filePath, string destinationDirectory, string newFileName = null)
        {
            // file doesn't exists
            if (!Exists(filePath)) return false;

            try
            {
                if (String.IsNullOrEmpty(newFileName)) newFileName = Path.GetFileName(filePath);
                File.Copy(filePath, Path.Combine(destinationDirectory, newFileName), true);
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Delete file.
        /// </summary>
        /// <param name="path">File path</param>
        /// <returns>true = success, false = error</returns>
        public static bool Delete(string path)
        {
            // file doesn't exists
            if (!Exists(path)) return true;

            try
            {
                // file deleted
                File.Delete(path);
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Check if the file exists.
        /// </summary>
        /// <param name="path">File path</param>
        /// <returns>true = exist, false = doesn't exists</returns>
        public static bool Exists(string path)
        {
            // file doesn't exists
            if (String.IsNullOrEmpty(path)) return false;
            if (!File.Exists(path.Trim())) return false;

            // file exists
            return true;
        }

        /// <summary>
        /// Get directory of file.
        /// </summary>
        /// <param name="path">File path</param>
        /// <returns>Directory path, null = doesn't exist</returns>
        public static string GetDirectory(string path)
        {
            if (!Exists(path)) return null;
            return Path.GetDirectoryName(path);
        }
    }
}