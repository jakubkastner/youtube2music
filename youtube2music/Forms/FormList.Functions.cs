﻿using Ookii.Dialogs.Wpf;
using System;
using System.Windows.Forms;
using youtube2music.App;

namespace youtube2music
{
    public partial class FormSeznam : Form
    {
        /*
         * SETTINGS
         * -> music library and program paths
         */

        /// <summary>
        /// Change or add new path to program.
        /// </summary>
        /// <param name="type">Program type</param>
        /// <param name="path">New program path</param>
        /// <return>true = succesfull, false = failed</return>
        private bool ProgramChange(Init.Program type, string path)
        {
            string typeName = Files.ProgramGetName(type);
            // change program path
            string pathNew = Files.ProgramChange(path, type);

            if (String.IsNullOrEmpty(pathNew))
            {
                // failed
                Actions.Show.Operation(type + " path could not be changed. File '" + path + "' does not exist.");
                Actions.Show.Error(typeName + " path change", "The path could not be changed.", "File '" + path + "' does not exist.", "Please try again");
                return false;
            }
            // succesfull
            Actions.Show.Operation(typeName + " path changed successfully: '" + pathNew + "'");
            return true;
        }

        /// <summary>
        /// Change or add new music directory to new folder.
        /// </summary>
        /// <param name="type">Type of music directory (opus/mp3)</param>
        /// <param name="path">New directory path</param>
        /// <return>true = succesfull, false = failed</return>
        private bool LibraryChange(Init.Library type, string path)
        {
            string typeName = Directories.LibraryGetType(type, true);
            // change music library
            string pathNew = Directories.LibraryChange(path, type);

            if (String.IsNullOrEmpty(pathNew))
            {
                // failed
                Actions.Show.Operation("Music library" + typeName + "could not be changed. Folder '" + path + "' does not exist.");
                Actions.Show.Error("Music library change" + typeName, "Music library" + typeName + "could not be changed.", "Folder '" + path + "' does not exist.", "Please try again");
                return false;
            }
            // succesfull
            Actions.Show.Operation("Music library" + typeName + "successfully changed: '" + pathNew + "'");
            return true;
        }

        /// <summary>
        /// The user selects a new program path using the FolderBrowserDialog. It appears in the menu.
        /// </summary>
        /// <param name="type">Program type</param>
        private void ProgramSelect(Init.Program type)
        {
            // init variables
            string typeName = Files.ProgramGetName(type, true);
            string path = Files.ProgramGetPath(type);

            // show dialog to select new program path
            OpenFileDialog selectFile = new OpenFileDialog();
            selectFile.Title = "Select the" + typeName + "executable file:";
            selectFile.Filter = "Executable files (*.exe)|*.exe|All files (*.*)|*.*";

            string directory = FD.Files.GetDirectory(path);
            if (!String.IsNullOrEmpty(directory))
            {
                selectFile.InitialDirectory = directory;
            }

            if (selectFile.ShowDialog() == DialogResult.OK)
            {
                if (selectFile.FilterIndex == 2)
                {
                    // not an exe file 
                    Actions.Show.Notice(typeName + " path change", "This is not an executable file (* .exe)! ", " The program may not work properly.");
                }
                // change the program path
                ProgramChange(type, selectFile.FileName);
            }
        }

        /// <summary>
        /// The user selects a new music library folder using the FolderBrowserDialog. It appears in the menu.
        /// </summary>
        /// <param name="type">Type of music library</param>
        private void LibrarySelect(Init.Library type)
        {
            // init variables
            string typeName = Directories.LibraryGetType(type, true);
            string path = Directories.LibraryGetPath(type);

            // show dialog to select new directory
            VistaFolderBrowserDialog selectDirectory = new VistaFolderBrowserDialog();
            selectDirectory.Description = "Select" + typeName + "music library folder";
            selectDirectory.UseDescriptionForTitle = true;
            if (!String.IsNullOrEmpty(path))
            {
                selectDirectory.SelectedPath = path;
            }

            if ((bool)selectDirectory.ShowDialog())
            {
                // change the directory
                LibraryChange(type, selectDirectory.SelectedPath);
            }
        }

        /// <summary>
        /// Delete music library from formList.
        /// </summary>
        /// <param name="type">Type of music library (opus/mp3)</param>
        /// <param name="path">Delete specific path from history</param>
        public void MenuPathDelete(Init.LibraryOrProgram type, string path = null)
        {
            // init variables
            ToolStripMenuItem menuHistory;
            ToolStripMenuItem menuSelectedPath;
            string menuSelectedText = "No program path selected";
            string menuHistoryText = "files";

            switch (type)
            {
                case Init.LibraryOrProgram.LibraryOpus:
                    menuHistory = menuSettingsLibraryOpusHistory;
                    menuSelectedPath = menuSettingsLibraryOpusSelected;
                    menuSelectedText = "No music library folder selected";
                    menuHistoryText = "folders";
                    break;
                case Init.LibraryOrProgram.LibraryMp3:
                    menuHistory = menuSettingsLibraryMp3History;
                    menuSelectedPath = menuSettingsLibraryMp3Selected;
                    menuSelectedText = "No music library folder selected";
                    menuHistoryText = "folders";
                    break;
                case Init.LibraryOrProgram.ProgramYoutubedl:
                    menuHistory = menuSettingsYoutubedlHistory;
                    menuSelectedPath = menuSettingsYoutubedlSelected;
                    break;
                case Init.LibraryOrProgram.ProgramFfmpeg:
                    menuHistory = menuSettingsFfmpegHistory;
                    menuSelectedPath = menuSettingsFfmpegSelected;
                    break;
                case Init.LibraryOrProgram.ProgramMp3tag:
                    menuHistory = menuSettingsMp3tagHistory;
                    menuSelectedPath = menuSettingsMp3tagSelected;
                    break;
                case Init.LibraryOrProgram.Null:
                    return;
                default:
                    return;
            }

            // change selected menu
            if (menuSelectedPath.Text == path)
            {
                menuSelectedPath.Text = menuSelectedText;
                menuSelectedPath.Enabled = false;
                menuSelectedPath.ToolTipText = "";
            }

            // delete from history
            MenuPathHistoryDelete(menuHistory, menuHistoryText, path);
        }

        /// <summary>
        /// Delete the selected path from history menu.
        /// </summary>
        /// <param name="type">Type of music library (opus/mp3)</param>
        /// <param name="menuHistory">Menu with history paths</param>
        /// <param name="path">Delete specific path from history</param>
        private void MenuPathHistoryDelete(ToolStripMenuItem menuHistory, string typeText, string path = null)
        {
            if (menuHistory == null) return;
            ToolStripMenuItem menuHistoryDelete = (String.IsNullOrEmpty(path)) ? MenuPathHistoryGetChecked(menuHistory) : MenuPathHistoryGetByPath(menuHistory, path);

            if (menuHistoryDelete == null) return;

            menuHistory.DropDownItems.Remove(menuHistoryDelete);
            if (menuHistory.DropDownItems.Count <= 0)
            {
                menuHistory.Enabled = false;
                menuHistory.Text = "No recently selected " + typeText + " was found";
            }
        }

        /// <summary>
        /// Get checked menu from history.
        /// </summary>
        /// <param name="menuHistory">Menu with history paths</param>
        /// <returns>Checked menu or null if not found</returns>
        private ToolStripMenuItem MenuPathHistoryGetChecked(ToolStripMenuItem menuHistory)
        {
            if (menuHistory == null) return null;

            foreach (ToolStripMenuItem menuItem in menuHistory.DropDownItems)
            {
                if (menuItem.Checked)
                {
                    return menuItem;
                }
            }
            return null;
        }

        /// <summary>
        /// Get menu from history by path.
        /// </summary>
        /// <param name="menuHistory">Menu with history paths</param>
        /// <param name="path">Path from menu</param>
        /// <returns>Checked menu or null if not found</returns>
        private ToolStripMenuItem MenuPathHistoryGetByPath(ToolStripMenuItem menuHistory, string path)
        {
            if (menuHistory == null) return null;

            foreach (ToolStripMenuItem menuItem in menuHistory.DropDownItems)
            {
                if (menuItem.Text == path)
                {
                    return menuItem;
                }
            }
            return null;
        }

        /// <summary>
        /// Select path in menu for type. If the path is not in the menu, it will be added to menu.
        /// </summary>
        /// <param name="type">Type of path (music library or program)</param>
        public void MenuPathSelect(Init.LibraryOrProgram type)
        {
            // init variables
            string path;
            string menuSelectedText = "Find the file ";
            ToolStripMenuItem menuHistory;
            ToolStripMenuItem menuSelectedPath;

            switch (type)
            {
                case Init.LibraryOrProgram.LibraryOpus:
                    path = Directories.LibraryOpus;
                    menuHistory = menuSettingsLibraryOpusHistory;
                    menuSelectedPath = menuSettingsLibraryOpusSelected;
                    menuSelectedText = "Open folder ";
                    break;
                case Init.LibraryOrProgram.LibraryMp3:
                    path = Directories.LibraryMp3;
                    menuHistory = menuSettingsLibraryMp3History;
                    menuSelectedPath = menuSettingsLibraryMp3Selected;
                    menuSelectedText = "Open folder ";
                    break;
                case Init.LibraryOrProgram.ProgramYoutubedl:
                    path = Files.ProgramYoutubedl;
                    menuHistory = menuSettingsYoutubedlHistory;
                    menuSelectedPath = menuSettingsYoutubedlSelected;
                    break;
                case Init.LibraryOrProgram.ProgramFfmpeg:
                    path = Files.ProgramFfmpeg;
                    menuHistory = menuSettingsFfmpegHistory;
                    menuSelectedPath = menuSettingsFfmpegSelected;
                    break;
                case Init.LibraryOrProgram.ProgramMp3tag:
                    path = Files.ProgramMp3tag;
                    menuHistory = menuSettingsMp3tagHistory;
                    menuSelectedPath = menuSettingsMp3tagSelected;
                    break;
                default:
                    return;
            }

            if (String.IsNullOrEmpty(path)) return;

            bool found = false;

            // goes through the paths in the menu 
            foreach (ToolStripMenuItem menuItem in menuHistory.DropDownItems)
            {
                if (menuItem.Text == path)
                {
                    // the path was found in the menu, I will check it 
                    menuItem.Checked = true;
                    found = true;
                }
                else
                {
                    menuItem.Checked = false;
                }
            }

            if (!found)
            {
                // path not found in the menu, I will add it to the menu 
                MenuPathHistoryAdd(path, type);
            }

            // menu settings with path
            menuSelectedText += "'" + path + "' in the file explorer";
            menuSelectedPath.Text = path;
            menuSelectedPath.Enabled = true;
            menuSelectedPath.ToolTipText = menuSelectedText;

            // TODO delete this and automate HudebniKnihovnaNajdiSlozky
            if (type == Init.LibraryOrProgram.LibraryOpus || type == Init.LibraryOrProgram.LibraryMp3)
            {
                menuNastaveniKnihovnaOpusProhledat.Text = "Prohledat hudební knihovnu";
                menuNastaveniKnihovnaOpusProhledat.Enabled = true;
                if (type == Init.LibraryOrProgram.LibraryOpus)
                {
                    HudebniKnihovnaNajdiSlozky();
                }
            }
        }

        /// <summary>
        /// Delete all paths from history except the currently selected one.
        /// </summary>
        /// <param name="type">Library or program type</param>
        private void MenuPathHistoryClearAll(Init.LibraryOrProgram type)
        {
            // init variables
            string pathCurrent;
            ToolStripMenuItem menuHistory;

            switch (type)
            {
                case Init.LibraryOrProgram.LibraryOpus:
                    menuHistory = menuSettingsLibraryOpusHistory;
                    pathCurrent = Directories.LibraryOpus;
                    break;
                case Init.LibraryOrProgram.LibraryMp3:
                    menuHistory = menuSettingsLibraryMp3History;
                    pathCurrent = Directories.LibraryMp3;
                    break;
                case Init.LibraryOrProgram.ProgramYoutubedl:
                    menuHistory = menuSettingsYoutubedlHistory;
                    pathCurrent = Files.ProgramYoutubedl;
                    break;
                case Init.LibraryOrProgram.ProgramFfmpeg:
                    menuHistory = menuSettingsFfmpegHistory;
                    pathCurrent = Files.ProgramFfmpeg;
                    break;
                case Init.LibraryOrProgram.ProgramMp3tag:
                    menuHistory = menuSettingsMp3tagHistory;
                    pathCurrent = Files.ProgramMp3tag;
                    break;
                case Init.LibraryOrProgram.Null:
                    return;
                default:
                    return;
            }

            // delete from menu
            for (int i = 0; i < menuHistory.DropDownItems.Count; i++)
            {
                if (menuHistory.DropDownItems[i].Text != pathCurrent)
                {
                    // if it is not the current path, it removes it from the history 
                    menuHistory.DropDownItems.RemoveAt(i);
                    i--;
                }
            }
        }


        /// <summary>
        /// Add new path to history menu.
        /// </summary>
        /// <param name="pathNew">New path</param>
        /// <param name="type">Path type</param>
        private void MenuPathHistoryAdd(string pathNew, Init.LibraryOrProgram type)
        {
            string typeText;
            // check if directory or file exists
            if (type == Init.LibraryOrProgram.LibraryOpus || type == Init.LibraryOrProgram.LibraryMp3)
            {
                // folder
                if (!FD.Directories.Exists(pathNew)) return;
                typeText = "folder";
            }
            else if (type == Init.LibraryOrProgram.ProgramYoutubedl || type == Init.LibraryOrProgram.ProgramFfmpeg || type == Init.LibraryOrProgram.ProgramMp3tag)
            {
                // file
                if (!FD.Files.Exists(pathNew)) return;
                typeText = "file";
            }
            else
            {
                return;
            }

            // init variables
            ToolStripMenuItem menuNew = new ToolStripMenuItem(pathNew);
            menuNew.Text = pathNew;
            ToolStripMenuItem menuHistory;
            string pathSeleted;

            // and add click function
            switch (type)
            {
                case Init.LibraryOrProgram.LibraryOpus:
                    menuHistory = menuSettingsLibraryOpusHistory;
                    pathSeleted = Directories.LibraryOpus;
                    menuNew.Click += new EventHandler(menuSettingsLibraryOpusHistoryPath_Click);
                    break;
                case Init.LibraryOrProgram.LibraryMp3:
                    menuHistory = menuSettingsLibraryMp3History;
                    pathSeleted = Directories.LibraryMp3;
                    menuNew.Click += new EventHandler(menuSettingsLibraryMp3HistoryPath_Click);
                    break;
                case Init.LibraryOrProgram.ProgramYoutubedl:
                    menuHistory = menuSettingsYoutubedlHistory;
                    pathSeleted = Files.ProgramYoutubedl;
                    menuNew.Click += new EventHandler(menuSettingsYoutubedlHistoryPath_Click);
                    break;
                case Init.LibraryOrProgram.ProgramFfmpeg:
                    menuHistory = menuSettingsFfmpegHistory;
                    pathSeleted = Files.ProgramFfmpeg;
                    menuNew.Click += new EventHandler(menuSettingsFfmpegHistoryPath_Click);
                    break;
                case Init.LibraryOrProgram.ProgramMp3tag:
                    menuHistory = menuSettingsMp3tagHistory;
                    pathSeleted = Files.ProgramMp3tag;
                    menuNew.Click += new EventHandler(menuSettingsMp3tagHistoryPath_Click);
                    break;
                default:
                    return;
            }

            // set menu text and add new menu to history menu
            menuNew.Checked = (menuNew.Text == pathSeleted);
            menuNew.ToolTipText = "Select " + typeText + " '" + pathNew + "'";

            menuHistory.DropDownItems.Add(menuNew);
            menuHistory.Text = "Recently seleted " + typeText + "s";
            menuHistory.Enabled = true;
        }
    }
}
