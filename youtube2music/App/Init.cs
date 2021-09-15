using System;
using System.Linq;
using System.Windows.Forms;

namespace youtube2music.App
{
    /// <summary>
    /// Init Program youtube2music.
    /// </summary>
    public class Init
    {
        /// <summary>
        /// Main FormList with list of videos.
        /// </summary>
        public static readonly FormSeznam formList = Application.OpenForms.OfType<FormSeznam>().FirstOrDefault();

        /// <summary>
        /// Name of the program.
        /// </summary>
        public static string Name { get; } = "youtube2music";

        /// <summary>
        /// Types of music library (mp3/opus) and programs (youtube-dl, ffmpeg, mp3tag).
        /// </summary>
        [Flags]
        public enum LibraryOrProgram
        {
            LibraryOpus,
            LibraryMp3,
            ProgramYoutubeDl,
            ProgramFfmpeg,
            ProgramMp3tag
        };

        /// <summary>
        /// Types of music library (mp3/opus).
        /// </summary>
        [Flags]
        public enum Library
        {
            Opus,
            Mp3
        };
    }
}