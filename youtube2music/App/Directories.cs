using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace youtube2music.App
{
    /// <summary>
    /// Get program path for directories.
    /// </summary>
    public class Directories
    {
        private static string libraryOpus;
        private static string libraryMp3;
        private static readonly string nameData = "data";
        private static readonly string nameCache = "cache";
        private static string appData;
        private static string data;
        private static string cache;
        private static string currentCache;

        /// <summary>
        /// Directory path for opus music library.
        /// </summary>
        public static string LibraryOpus
        {
            get
            {
                LibraryCheck(Init.Library.Opus);
                return libraryOpus;
            }
        }
        /// <summary>
        /// Directory path for mp3 music library.
        /// </summary>
        public static string LibraryMp3
        {
            get
            {
                LibraryCheck(Init.Library.Mp3);
                return libraryMp3;
            }
        }


        /// <summary>
        /// Application data directory.
        /// </summary>
        public static string AppData
        {
            get
            {
                if (String.IsNullOrEmpty(appData))
                {
                    // %appdata%/youtube2music
                    appData = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), Init.Name);
                }
                return appData;
            }
        }
        /// <summary>
        /// Data directory.
        /// </summary>
        public static string Data
        {
            get
            {
                if (String.IsNullOrEmpty(data))
                {
                    data = Path.Combine(AppData, nameData);
                }
                return data;
            }
        }
        /// <summary>
        /// Current running instance cache directory.
        /// </summary>
        public static string Cache
        {
            get
            {
                if (String.IsNullOrEmpty(cache))
                {
                    cache = Path.Combine(AppData, nameCache);
                }
                return cache;
            }
        }
        /// <summary>
        /// Current running instance cache directory.
        /// </summary>
        public static string CurrentCache
        {
            get
            {
                if (String.IsNullOrEmpty(currentCache))
                {
                    currentCache = Path.Combine(Cache, Process.GetCurrentProcess().Id.ToString());
                }
                return currentCache;
            }
        }

        /// <summary>
        /// Change music library (opus/mp3).
        /// </summary>
        /// <param name="path">Path to new directory</param>
        /// <param name="type">Type of directory</param>
        /// <returns>Path to the new library or null</returns>
        public static string LibraryChange(string path, Init.Library type)
        {
            // bad path
            if (String.IsNullOrEmpty(path)) return null;
            if (!FD.Directories.Exists(path)) return null;

            path = path.Trim();
            Init.LibraryOrProgram typeLibrary;

            // set path
            switch (type)
            {
                case Init.Library.Opus:
                    libraryOpus = path;
                    break;
                case Init.Library.Mp3:
                    libraryMp3 = path;
                    break;
                default:
                    return null;
            }

            // get type
            typeLibrary = Init.GetType(type);
            if (typeLibrary == Init.LibraryOrProgram.Null) return null;

            // change library in formList
            Init.FormList.MenuPathSelect(typeLibrary);

            return path;
        }

        /// <summary>
        /// Check if the music library directory exists. If doesn't exists - clear it from formList and set variable to null.
        /// </summary>
        /// <param name="type">Type of the direcotry (opus/mp3)</param>
        public static void LibraryCheck(Init.Library type)
        {
            // opus
            if (type == Init.Library.Opus)
            {
                if (FD.Directories.Exists(libraryOpus)) return;
                // directory doesn't exist
                libraryOpus = null;
            }

            // mp3
            else if (type == Init.Library.Mp3)
            {
                if (FD.Directories.Exists(libraryMp3)) return;
                // directory doesn't exist
                libraryMp3 = null;
            }
            else
            {
                return;
            }
            // remove library from formList
            Init.LibraryOrProgram libraryType = Init.GetType(type);
            Init.FormList.MenuPathDelete(libraryType);
        }

        /// <summary>
        /// Get the path of the music library based on the type.
        /// </summary>
        /// <param name="type">Music library type</param>
        /// <param name="spaces">true = return spaces before and after path, false = return path without spaces</param>
        /// <returns>Music library path, null = doesn't found</returns>
        public static string LibraryGetPath(Init.Library type, bool spaces = false)
        {
            string path;
            switch (type)
            {
                case Init.Library.Opus:
                    path = LibraryOpus;
                    break;
                case Init.Library.Mp3:
                    path = LibraryMp3;
                    break;
                default:
                    return null;
            }
            if (String.IsNullOrEmpty(path)) return null;
            if (spaces) path = " " + path + " ";
            return path;
        }

        /// <summary>
        /// Get the name as string of the music library based on the type.
        /// </summary>
        /// <param name="type">Music library type</param>
        /// <param name="spaces">true = return spaces before and after path, false = return path without spaces</param>
        /// <returns>Music library name, empty = doesn't found</returns>
        public static string LibraryGetType(Init.Library type, bool spaces = false)
        {
            string name = spaces ? " " : "";
            switch (type)
            {
                case Init.Library.Opus:
                    name += "opus";
                    break;
                case Init.Library.Mp3:
                    name += "mp3";
                    break;
                default:
                    name = "";
                    break;
            }
            if (spaces) name += " ";
            return name;
        }


        // TODO !!!!!!! music library - change
        /// <summary>
        /// Seznam složek hudební knihovny.
        /// (0 = opus, 1 = mp3)
        /// </summary>
        private List<string> slozkyKnihovny = new List<string>();

        /// <summary>
        /// Získá složku hudební knihovny na vybraném indexu ze seznamu.
        /// (0 = opus, 1 = mp3)
        /// </summary>
        /// <param name="index">Index ze seznamu</param>
        /// <returns>existuje = cesta složky, neexistuje = null</returns>
        public string ZiskejHudebniKnihovnu(int index)
        {
            // index je mimo hranice seznamu
            if (slozkyKnihovny.Count() <= index) return null;
            // složka neexistuje
            if (!ExistujeHudebniKnihovna(index)) return null;
            return slozkyKnihovny.ElementAt(index);
        }

        /// <summary>
        /// Přidá novou hudební knihovnu do seznamu.
        /// (0 = opus, 1 = mp3)
        /// </summary>
        /// <param name="novaSlozka">Cesta nové hudební knihovny.</param>
        /// <returns>true = úspěšně přidáno, false = nepřidáno</returns>
        public bool PridejHudebniKnihovnu(string novaSlozka)
        {
            // složka neexistuje
            if (!Directory.Exists(novaSlozka)) return false;
            // přidání složky do seznamu
            slozkyKnihovny.Add(novaSlozka);
            return true;
        }

        /// <summary>
        /// Změní existující hudební knihovnu ze seznamu.
        /// (0 = opus, 1 = mp3)
        /// </summary>
        /// <param name="novaSlozka">Cesta hudební knihovny</param>
        /// <param name="index">Index ze seznamu</param>
        /// <returns>true = úspěšně změněno, false = nezměněno</returns>
        public bool ZmenHudebniKnihovnu(string novaSlozka, int index)
        {
            // složka neexistuje
            if (!Directory.Exists(novaSlozka)) return false;
            // index je mimo hranice seznamu
            if (slozkyKnihovny.Count() <= index) return false;
            // změna cesty složky ze seznamu
            slozkyKnihovny[index] = novaSlozka;
            return true;
        }

        /// <summary>
        /// Odstraní složku hudební knihovny na vybraném indexu ze seznamu.
        /// (0 = opus, 1 = mp3)
        /// </summary>
        /// <param name="index">Index ze seznamu</param>
        public void OdeberHudebniKnihovnu(int index)
        {
            // odstraní složku ze seznamu
            slozkyKnihovny.RemoveAt(index);
        }

        /// <summary>
        /// Zjistí, jestli existuje složka hudební knihovny ze seznamu.
        /// </summary>
        /// <returns>true = složka existuje, false = složka neexistuje</returns>
        public bool ExistujeHudebniKnihovna(int index)
        {
            // index je mimo hranice seznamu
            if (slozkyKnihovny.Count() <= index) return false;
            // složka existuje
            if (Directory.Exists(slozkyKnihovny.ElementAt(index))) return true;
            // složka neexistuje
            slozkyKnihovny[index] = null;
            return false;
        }
    }
}