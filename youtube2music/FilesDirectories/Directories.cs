﻿using System;
using System.IO;

namespace youtube2music.FD
{
    /// <summary>
    /// Create or delete directories.
    /// </summary>
    public static class Directories
    {
        /// <summary>
        /// Create new directory, if is not exists.
        /// </summary>
        /// <param name="path">Path to directory</param>
        /// <param name="delete">Delete existing directory (true = delete, false = no)</param>
        /// <param name="deleteRecoursive">If you want delete directory, you can choose if you want to delete it recoursive.</param>
        /// <returns>true = success, false = error</returns>
        public static bool Create(string path, bool delete = false, bool deleteRecoursive = true)
        {
            if (Exists(path))
            {
                // directory exists
                if (!delete)
                {
                    return true;
                }
                // deleting directory
                if (!Delete(path, deleteRecoursive))
                {
                    // failed
                    return false;
                }
            }
            // create new directory
            try
            {
                Directory.CreateDirectory(path);
            }
            catch (Exception)
            {
                // failed
                return false;
            }
            return true;
        }

        /// <summary>
        /// Delete directory.
        /// </summary>
        /// <param name="path">Path to directory.</param>
        /// <param name="recursive">Recursive delete directories.</param>
        /// <returns>true = success, false = error</returns>
        public static bool Delete(string path, bool recursive = true)
        {
            if (!Exists(path))
            {
                // directory doesn't exists
                return true;
            }
            // delete directory
            try
            {
                Directory.Delete(path, recursive);
            }
            catch (Exception)
            {
                // failed
                return false;
            }
            return true;
        }
        /// <summary>
        /// Check if directory exists.
        /// </summary>
        /// <param name="path">Path to directory.</param>
        /// <returns>true = exists, false = doesn't exist</returns>
        public static bool Exists(string path)
        {
            // directory doesn't exists
            if (String.IsNullOrEmpty(path)) return false;
            if (!Directory.Exists(path.Trim())) return false;

            // exists
            return true;
        }
    }
}