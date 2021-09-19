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

        /*
         * SETTINGS
         * -> music library and program paths
         */

        #region open currently selected music library or program in file explorer
        /// <summary>
        /// Opens opus music library in file explorer.
        /// </summary>
        private void menuSettingsLibraryOpusSelected_Click(object sender, EventArgs e)
        {
            if (!Actions.Run.Program(Directories.LibraryOpus, true, false))
            {
                MenuPathDelete(Init.LibraryOrProgram.LibraryOpus);
                Actions.Show.Error("Opening opus music library", "The mp3 music library directory doesn't exist.");
            }
        }
        /// <summary>
        /// Opens mp3 music library in file explorer.
        /// </summary>
        private void menuSettingsLibraryMp3Selected_Click(object sender, EventArgs e)
        {
            if (!Actions.Run.Program(Directories.LibraryMp3, true, false))
            {
                MenuPathDelete(Init.LibraryOrProgram.LibraryMp3);
                Actions.Show.Error("Opening mp3 music library", "The mp3 music library directory doesn't exist.");
            }
        }
        /// <summary>
        /// Opens the folder with the path of the youtube-dl file in file explorer and selects the file.
        /// </summary>
        private void menuSettingsYoutubedlSelected_Click(object sender, EventArgs e)
        {
            // get path
            string path = Files.ProgramYoutubedl;
            if (String.IsNullOrEmpty(path))
            {
                MenuPathDelete(Init.LibraryOrProgram.ProgramYoutubedl);
                Actions.Show.Error("Opening youtube-dl file", "The youtube-dl path doesn't exist.");
                return;
            }
            // open file explorer
            if (!Actions.Run.Program("explorer", false, false, "/select, \"" + path + "\""))
            {
                Actions.Show.Error("Opening youtube-dl file", "Failed to found youtube-dl in file explorer.");
            }
        }
        /// <summary>
        /// Opens the folder with the path of the ffmpeg file in file explorer and selects the file.
        /// </summary>
        private void menuSettingsFfmpegSelected_Click(object sender, EventArgs e)
        {
            // get path
            string path = Files.ProgramFfmpeg;
            if (String.IsNullOrEmpty(path))
            {
                MenuPathDelete(Init.LibraryOrProgram.ProgramFfmpeg);
                Actions.Show.Error("Opening ffmpeg file", "The ffmpeg path doesn't exist.");
                return;
            }
            // open file explorer
            if (!Actions.Run.Program("explorer", false, false, "/select, \"" + path + "\""))
            {
                Actions.Show.Error("Opening ffmpeg file", "Failed to found ffmpeg in file explorer.");
            }
        }
        /// <summary>
        /// Opens the folder with the path of the mp3tag file in file explorer and selects the file.
        /// </summary>
        private void menuSettingsMp3tagSelected_Click(object sender, EventArgs e)
        {
            // get path
            string path = Files.ProgramMp3tag;
            if (String.IsNullOrEmpty(path))
            {
                MenuPathDelete(Init.LibraryOrProgram.ProgramMp3tag);
                Actions.Show.Error("Opening mp3tag file", "The mp3tag path doesn't exist.");
                return;
            }
            // open file explorer
            if (!Actions.Run.Program("explorer", false, false, "/select, \"" + path + "\""))
            {
                Actions.Show.Error("Opening mp3tag file", "Failed to found mp3tag in file explorer.");
            }
        }
        #endregion

        #region change music library or program path using the FolderBrowserDialog or OpenFileDialog
        /// <summary>
        /// User changes the path of the opus music library using the FolderBrowserDialog.
        /// </summary>
        private void menuSettingsLibraryOpusChange_Click(object sender, EventArgs e)
        {
            LibrarySelect(Init.Library.Opus);
        }
        /// <summary>
        /// User changes the path of the mp3 music library using the FolderBrowserDialog.
        /// </summary>
        private void menuSettingsLibraryMp3Change_Click(object sender, EventArgs e)
        {
            LibrarySelect(Init.Library.Mp3);
        }
        /// <summary>
        /// User changes the path of the youtube-dl using the OpenFileDialog.
        /// </summary>
        private void menuSettingsYoutubedlChange_Click(object sender, EventArgs e)
        {
            ProgramSelect(Init.Program.Youtubedl);
        }
        /// <summary>
        /// User changes the path of the ffmpeg using the OpenFileDialog.
        /// </summary>
        private void menuSettingsFfmpegChange_Click(object sender, EventArgs e)
        {
            ProgramSelect(Init.Program.Ffmpeg);
        }
        /// <summary>
        /// User changes the path of the mp3tag using the OpenFileDialog.
        /// </summary>
        private void menuSettingsMp3tagChange_Click(object sender, EventArgs e)
        {
            ProgramSelect(Init.Program.Mp3tag);
        }
        #endregion

        #region change music library or program path from a list of previously selected paths
        /// <summary>
        /// User selected a new opus music library from a list of previously selected paths.
        /// </summary>
        private void menuSettingsLibraryOpusHistoryPath_Click(object sender, EventArgs e)
        {
            var menu = sender as ToolStripMenuItem;
            if (!LibraryChange(Init.Library.Opus, menu.Text))
            {
                MenuPathDelete(Init.LibraryOrProgram.LibraryOpus, menu.Text);
            }
        }
        /// <summary>
        /// User selected a new mp3 music library from a list of previously selected paths.
        /// </summary>
        private void menuSettingsLibraryMp3HistoryPath_Click(object sender, EventArgs e)
        {
            var menu = sender as ToolStripMenuItem;
            if (!LibraryChange(Init.Library.Mp3, menu.Text))
            {
                MenuPathDelete(Init.LibraryOrProgram.LibraryMp3, menu.Text);
            }
        }
        /// <summary>
        /// User selected a new youtube-dl path from a list of previously selected paths.
        /// </summary>
        private void menuSettingsYoutubedlHistoryPath_Click(object sender, EventArgs e)
        {
            var menu = sender as ToolStripMenuItem;
            if (!ProgramChange(Init.Program.Youtubedl, menu.Text))
            {
                MenuPathDelete(Init.LibraryOrProgram.ProgramYoutubedl, menu.Text);
            }
        }
        /// <summary>
        /// User selected a new ffmpeg path from a list of previously selected paths.
        /// </summary>
        private void menuSettingsFfmpegHistoryPath_Click(object sender, EventArgs e)
        {
            var menu = sender as ToolStripMenuItem;
            if (!ProgramChange(Init.Program.Ffmpeg, menu.Text))
            {
                MenuPathDelete(Init.LibraryOrProgram.ProgramFfmpeg, menu.Text);
            }
        }
        /// <summary>
        /// User selected a new mp3tag path from a list of previously selected paths.
        /// </summary>
        private void menuSettingsMp3tagHistoryPath_Click(object sender, EventArgs e)
        {
            var menu = sender as ToolStripMenuItem;
            if (!ProgramChange(Init.Program.Mp3tag, menu.Text))
            {
                MenuPathDelete(Init.LibraryOrProgram.ProgramMp3tag, menu.Text);
            }
        }
        #endregion

        #region clear the history of selected music library or program paths
        /// <summary>
        /// Click to clear the history of selected opus music library paths.
        /// </summary>
        private void menuSettingsLibraryOpusHistoryClear_Click(object sender, EventArgs e)
        {
            MenuPathHistoryClearAll(Init.LibraryOrProgram.LibraryOpus);
        }
        /// <summary>
        /// Click to clear the history of selected mp3 music library paths.
        /// </summary>
        private void menuSettingsLibraryMp3HistoryClear_Click(object sender, EventArgs e)
        {
            MenuPathHistoryClearAll(Init.LibraryOrProgram.LibraryMp3);
        }
        /// <summary>
        /// Click to clear the history of selected youtube-dl paths.
        /// </summary>
        private void menuSettingsYoutubedlHistoryClear_Click(object sender, EventArgs e)
        {
            MenuPathHistoryClearAll(Init.LibraryOrProgram.ProgramYoutubedl);
        }
        /// <summary>
        /// Click to clear the history of selected ffmpeg paths.
        /// </summary>
        private void menuSettingsFfmpegHistoryClear_Click(object sender, EventArgs e)
        {
            MenuPathHistoryClearAll(Init.LibraryOrProgram.ProgramFfmpeg);
        }
        /// <summary>
        /// Click to clear the history of selected mp3tag paths.
        /// </summary>
        private void menuSettingsMp3tagHistoryClear_Click(object sender, EventArgs e)
        {
            MenuPathHistoryClearAll(Init.LibraryOrProgram.ProgramMp3tag);
        }
        #endregion
        
    }
}
