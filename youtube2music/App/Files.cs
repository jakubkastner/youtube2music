using System;
using System.IO;

namespace youtube2music.App
{
    /// <summary>
    /// Get program path for files.
    /// </summary>
    public class Files
    {
        private static readonly string nameProgramYoutubeDl = "youtubedl.txt";
        private static readonly string nameProgramFfmpeg = "ffmpeg.txt";
        private static readonly string nameProgramMp3tag = "mp3tag.txt";
        private static readonly string nameLibraryOpus = "library_opus.txt";
        private static readonly string nameLibraryMp3 = "library_mp3.txt";
        private static readonly string nameDownloadedVideos = "videos.txt";
        private static readonly string nameYouTubeUser = "Google.Apis.Auth.OAuth2.Responses.TokenResponse-user";
        private static string libraryOpusHistory;
        private static string libraryMp3History;
        private static string programYoutubedlHistory;
        private static string programFfmpegHistory;
        private static string programMp3tagHistory;
        private static string downloadedVideosHistory;
        private static string youTubeUser;

        private static string programYoutubedl;
        private static string programFfmpeg;
        private static string programMp3tag;

        /// <summary>
        /// File path for program youtube-dl.
        /// </summary>
        public static string ProgramYoutubedl
        {
            get
            {
                ProgramCheck(Init.Program.Youtubedl);
                return programYoutubedl;
            }
        }
        /// <summary>
        /// File path for program FFmpeg.
        /// </summary>
        public static string ProgramFfmpeg
        {
            get
            {
                ProgramCheck(Init.Program.Ffmpeg);
                return programFfmpeg;
            }
        }
        /// <summary>
        /// File path for program mp3tag.
        /// </summary>
        public static string ProgramMp3tag
        {
            get
            {
                ProgramCheck(Init.Program.Mp3tag);
                return programMp3tag;
            }
        }

        /// <summary>
        /// Get file path for history of opus music library.
        /// </summary>
        public static string LibraryOpusHistory
        {
            get
            {
                if (String.IsNullOrEmpty(libraryOpusHistory))
                {
                    if (Directories.Data == null) return null;
                    libraryOpusHistory = Path.Combine(Directories.Data, nameLibraryOpus);
                }
                return libraryOpusHistory;
            }
        }
        /// <summary>
        /// Get file path for history of mp3 music library.
        /// </summary>
        public static string LibraryMp3History
        {
            get
            {
                if (String.IsNullOrEmpty(libraryMp3History))
                {
                    if (Directories.Data == null) return null;
                    libraryMp3History = Path.Combine(Directories.Data, nameLibraryMp3);
                }
                return libraryMp3History;
            }
        }
        /// <summary>
        /// Get file path for history of program youtube-dl.
        /// </summary>
        public static string ProgramYouTubeDlHistory
        {
            get
            {
                if (String.IsNullOrEmpty(programYoutubedlHistory))
                {
                    if (Directories.Data == null) return null;
                    programYoutubedlHistory = Path.Combine(Directories.Data, nameProgramYoutubeDl);
                }
                return programYoutubedlHistory;
            }
        }
        /// <summary>
        /// Get file path for history of program FFmpeg.
        /// </summary>
        public static string ProgramFfmpegHistory
        {
            get
            {
                if (String.IsNullOrEmpty(programFfmpegHistory))
                {
                    if (Directories.Data == null) return null;
                    programFfmpegHistory = Path.Combine(Directories.Data, nameProgramFfmpeg);
                }
                return programFfmpegHistory;
            }
        }
        /// <summary>
        /// Get file path for history of program mp3tag.
        /// </summary>
        public static string ProgramMp3tagHistory
        {
            get
            {
                if (String.IsNullOrEmpty(programMp3tagHistory))
                {
                    if (Directories.Data == null) return null;
                    programMp3tagHistory = Path.Combine(Directories.Data, nameProgramMp3tag);
                }
                return programMp3tagHistory;
            }
        }
        /// <summary>
        /// Get file path for downloaded videos history.
        /// </summary>
        public static string DownloadedVideosHistory
        {
            get
            {
                if (String.IsNullOrEmpty(downloadedVideosHistory))
                {
                    if (Directories.Data == null) return null;
                    downloadedVideosHistory = Path.Combine(Directories.Data, nameDownloadedVideos);
                }
                return downloadedVideosHistory;
            }
        }
        /// <summary>
        /// Get file path for logged in YouTube user.
        /// </summary>
        public static string YouTubeUser
        {
            get
            {
                if (String.IsNullOrEmpty(youTubeUser))
                {
                    if (Directories.Data == null) return null;
                    youTubeUser = Path.Combine(Directories.Data, nameYouTubeUser);
                }
                return youTubeUser;
            }
        }



        /// <summary>
        /// Change path to program.
        /// </summary>
        /// <param name="path">New path to program</param>
        /// <param name="type">Type of program</param>
        /// <returns>New path to the program or null</returns>
        public static string ProgramChange(string path, Init.Program type)
        {
            // bad path
            if (String.IsNullOrEmpty(path)) return null;
            if (!FD.Files.Exists(path)) return null;

            path = path.Trim();
            Init.LibraryOrProgram typeProgram;

            // change program             
            // set path
            switch (type)
            {
                case Init.Program.Youtubedl:
                    programYoutubedl = path;
                    break;
                case Init.Program.Ffmpeg:
                    programFfmpeg = path;
                    break;
                case Init.Program.Mp3tag:
                    programMp3tag = path;
                    break;
                default:
                    return null;
            }

            // get type
            typeProgram = Init.GetType(type);
            if (typeProgram == Init.LibraryOrProgram.Null) return null;

            // change library in formList
            Init.FormList.MenuPathSelect(typeProgram);

            return path;
        }

        /// <summary>
        /// Check if the path to the program exists. If doesn't exists - clear it from formList and set variable to null.
        /// </summary>
        /// <param name="type">Program type</param>
        public static void ProgramCheck(Init.Program type)
        {
            // youtube-dl
            if (type == Init.Program.Youtubedl)
            {
                if (FD.Files.Exists(programYoutubedl)) return;
                // program file doesn't exist
                programYoutubedl = null;              
            }
            // ffmpeg
            else if (type == Init.Program.Ffmpeg)
            {
                if (FD.Files.Exists(programFfmpeg)) return;                
                // program file doesn't exist
                programFfmpeg = null;             
            }
            // mp3tag
            else if (type == Init.Program.Mp3tag)
            {
                if (FD.Files.Exists(programMp3tag)) return;
                // program file doesn't exist
                programMp3tag = null;
            }
            else
            {
                return;
            }

            // remove program path from formList
            Init.LibraryOrProgram programType = Init.GetType(type);
            Init.FormList.MenuPathDelete(programType);
        }

        /// <summary>
        /// Get the path of the program based on the type.
        /// </summary>
        /// <param name="type">Program type</param>
        /// <param name="spaces">true = return spaces before and after path, false = return path without spaces</param>
        /// <returns>Program path, null = doesn't found</returns>
        public static string ProgramGetPath(Init.Program type, bool spaces = false)
        {
            string path;
            switch (type)
            {
                case Init.Program.Youtubedl:
                    path = ProgramYoutubedl;
                    break;
                case Init.Program.Ffmpeg:
                    path = ProgramFfmpeg;
                    break;
                case Init.Program.Mp3tag:
                    path = ProgramMp3tag;
                    break;
                default:
                    return null;
            }
            if (String.IsNullOrEmpty(path)) return null;
            if (spaces) path = " " + path + " ";
            return path;
        }

        /// <summary>
        /// Get the name as string of the program based on the type.
        /// </summary>
        /// <param name="type">Program type</param>
        /// <param name="spaces">true = return spaces before and after path, false = return path without spaces</param>
        /// <returns>Program name, empty = doesn't found</returns>
        public static string ProgramGetName(Init.Program type, bool spaces = false)
        {
            string name = spaces ? " " : "";
            switch (type)
            {
                case Init.Program.Youtubedl:
                    name += "youtube-dl";
                    break;
                case Init.Program.Ffmpeg:
                    name += "ffmpeg";
                    break;
                case Init.Program.Mp3tag:
                    name += "mp3tag";
                    break;
                default:
                    name = "";
                    break;
            }
            if (spaces) name += " ";
            return name;
        }
    }
}