using System;
using System.Windows.Forms;
using youtube2music.App;

namespace youtube2music
{
    public partial class FormSeznam : Form
    {
        public FormSeznam()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Change or add new music directory to new folder.
        /// </summary>
        /// <param name="type">Typo of music directory (opus/mp3)</param>
        /// <param name="path">New directory path</param>
        private void LibraryChange(Init.Library type, string path)
        {
            string typeName = " " + ((type == Init.Library.Opus) ? "opus" : "mp3") + " ";
            // change music library
            path = Directories.LibraryChange(path, type);

            if (String.IsNullOrEmpty(path))
            {
                // failed
                Actions.Show.Operation("Music library" + typeName + "could not be changed. Folder '" + path + "' does not exist.");
                Actions.Show.Error("Music library change" + typeName, "Music library" + typeName + "could not be changed.", "Folder '" + path + "' does not exist.", "Please try again");
            }
            else
            {
                // succesfull
                Actions.Show.Operation("Music library" + typeName + "successfully changed: '" + path + "'");
            }
        }

        /// <summary>
        /// Delete music library from formList.
        /// </summary>
        /// <param name="type">Type of music library (opus/mp3)</param>
        public void LibraryDelete(Init.Library type)
        {
            ToolStripMenuItem menuSelected = (type == Init.Library.Opus) ? menuNastaveniKnihovnaOpusVybrana : menuNastaveniKnihovnaMp3Vybrana;

            // change selected menu
            menuSelected.Text = "No music library folder selected";
            menuSelected.Enabled = false;
            menuSelected.ToolTipText = "";

            // delete from history
            LibraryDeleteFromHistoryMenu(type);
        }

        // TODO delete also paths to programs
        /// <summary>
        /// Delete the selected music library path from history menu.
        /// </summary>
        /// <param name="type">Type of music library (opus/mp3)</param>
        private void LibraryDeleteFromHistoryMenu(Init.Library type)
        {
            ToolStripMenuItem menuHistory = (type == Init.Library.Opus) ? menuNastaveniKnihovnaOpusNaposledyVybrane : menuNastaveniKnihovnaMp3NaposledyVybrane;
            ToolStripMenuItem menuHistoryChecked = MenuLibraryHistoryGetChecked(type);

            if (menuHistoryChecked == null) return;

            menuHistory.DropDownItems.Remove(menuHistoryChecked);
            if (menuHistory.DropDownItems.Count >= 0)
            {
                menuHistory.Enabled = false;
                menuHistory.Text = "No recently selected folder was found";
            }
        }

        // TODO delete also paths to programs
        /// <summary>
        /// Get checked menu from history of music library.
        /// </summary>
        /// <param name="type">Type of music library (opus/mp3)</param>
        /// <returns>Checked menu or null if not found</returns>
        private ToolStripMenuItem MenuLibraryHistoryGetChecked(Init.Library type)
        {
            ToolStripMenuItem menuHistory = (type == Init.Library.Opus) ? menuNastaveniKnihovnaOpusNaposledyVybrane : menuNastaveniKnihovnaMp3NaposledyVybrane;

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
        /// Select path in menu for type. If the path is not in the menu, it will be added to menu.
        /// </summary>
        /// <param name="type">Type of path (music library or program)</param>
        public void MenuPathSelect(Init.LibraryOrProgram type)
        {
            // init variables
            string path;
            ToolStripMenuItem menuHistory;
            ToolStripMenuItem menuSelectedPath;

            if (type == Init.LibraryOrProgram.LibraryOpus)
            {
                path = Directories.LibraryOpus;
                menuHistory = menuNastaveniKnihovnaOpusNaposledyVybrane;
                menuSelectedPath = menuNastaveniKnihovnaOpusVybrana;
            }

            else if (type == Init.LibraryOrProgram.LibraryMp3)
            {
                path = Directories.LibraryMp3;
                menuHistory = menuNastaveniKnihovnaMp3NaposledyVybrane;
                menuSelectedPath = menuNastaveniKnihovnaMp3Vybrana;
            }

            else if (type == Init.LibraryOrProgram.ProgramYoutubeDl)
            {
                path = Files.ProgramYouTubeDl;
                menuHistory = menuNastaveniYoutubeDLCestaNaposledyVybrane;
                menuSelectedPath = menuNastaveniYoutubeDLCestaVybrana;
            }

            else if (type == Init.LibraryOrProgram.ProgramFfmpeg)
            {
                path = Files.ProgramFfmpeg;
                menuHistory = menuNastaveniFFmpegCestaNaposledyVybrane;
                menuSelectedPath = menuNastaveniFFmpegCestaVybrana;
            }

            else if (type == Init.LibraryOrProgram.ProgramMp3tag)
            {
                path = Files.ProgramMp3tag;
                menuHistory = menuNastaveniMp3tagCestaNaposledyVybrane;
                menuSelectedPath = menuNastaveniMp3tagCestaVybrana;
            }
            else
            {
                return;
            }

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
                MenuCestaPridat(path, type);
            }

            // menu settings with path 
            menuSelectedPath.Text = path;
            menuSelectedPath.Enabled = true;

            string text = (type == Init.LibraryOrProgram.LibraryOpus || type == Init.LibraryOrProgram.LibraryMp3) ? "Open folder" : "Find the file";
            text += " '" + path + "' in the file explorer";
            menuSelectedPath.ToolTipText = text;

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
    }
}
