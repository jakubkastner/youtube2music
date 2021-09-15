using System;

namespace youtube2music.App
{
    /// <summary>
    /// Init Program youtube2music.
    /// </summary>
    public class Init
    {
        /// <summary>
        /// Name of the program.
        /// </summary>
        public static string Name { get; } = "youtube2music";

        /// <summary>
        /// Types of direcories (mp3/opus) and programs (youtube-dl, ffmpeg, mp3tag)
        /// </summary>
        [Flags]
        public enum FileOrDirectory
        {
            LibraryOpus,
            LibraryMp3,
            ProgramYoutubeDl,
            ProgramFfmpeg,
            ProgramMp3tag
        };
    }
}