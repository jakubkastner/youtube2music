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
        public static FormSeznam FormList { get; } = Application.OpenForms.OfType<FormSeznam>().FirstOrDefault();

        /// <summary>
        /// Name of the program.
        /// </summary>
        public static string Name { get; } = "youtube2music";

        /// <summary>
        /// Types of music library (mp3, opus) and programs (youtube-dl, ffmpeg, mp3tag).
        /// </summary>
        [Flags]
        public enum LibraryOrProgram
        {
            LibraryOpus,
            LibraryMp3,
            ProgramYoutubedl,
            ProgramFfmpeg,
            ProgramMp3tag,
            Null
        };

        /// <summary>
        /// Types of music library (mp3, opus).
        /// </summary>
        [Flags]
        public enum Library
        {
            Opus,
            Mp3
        };

        /// <summary>
        /// Types of programs(youtube-dl, ffmpeg, mp3tag).
        /// </summary>
        [Flags]
        public enum Program
        {
            Youtubedl,
            Ffmpeg,
            Mp3tag
        };

        /// <summary>
        /// Get type of LibraryOrProgram from type Program.
        /// </summary>
        /// <param name="type">Program type</param>
        /// <returns>LibraryOrProgram type, LibraryOrProgram.Null if doesn't exists</returns>
        public static LibraryOrProgram GetType(Program type)
        {
            switch (type)
            {
                case Program.Youtubedl:
                    return LibraryOrProgram.ProgramYoutubedl;
                case Program.Ffmpeg:
                    return LibraryOrProgram.ProgramFfmpeg;
                case Program.Mp3tag:
                    return LibraryOrProgram.ProgramMp3tag;
                default:
                    return LibraryOrProgram.Null;
            }
        }
        /// <summary>
        /// Get type of LibraryOrProgram from type Library.
        /// </summary>
        /// <param name="type">Library type</param>
        /// <returns>LibraryOrProgram type, LibraryOrProgram.Null if doesn't exists</returns>
        public static LibraryOrProgram GetType(Library type)
        {
            switch (type)
            {
                case Library.Opus:
                    return LibraryOrProgram.LibraryOpus;
                case Library.Mp3:
                    return LibraryOrProgram.LibraryMp3;
                default:
                    return LibraryOrProgram.Null;
            }
        }
    }
}