using System;
using System.IO;

namespace youtube2music.App.Paths
{
    /// <summary>
    /// Get program path for files.
    /// </summary>
    public class Files
    {
        private static readonly string nameProgramYouTubeDl = "youtubedl.txt";
        private static readonly string nameProgramFfmpeg = "ffmpeg.txt";
        private static readonly string nameProgramMp3tag = "mp3tag.txt";
        private static readonly string nameLibraryOpus = "library_opus.txt";
        private static readonly string nameLibraryMp3 = "library_mp3.txt";
        private static readonly string nameDownloadedVideos = "videos.txt";
        private static readonly string nameYouTubeUser = "Google.Apis.Auth.OAuth2.Responses.TokenResponse-user";
        private static string libraryOpusHistory;
        private static string libraryMp3History;
        private static string programYouTubeDlHistory;
        private static string programFfmpegHistory;
        private static string programMp3tagHistory;
        private static string downloadedVideosHistory;
        private static string youTubeUser;

        /// <summary>
        /// File path for program youtube-dl.
        /// </summary>
        public static string ProgramYouTubeDl { get; set; }
        /// <summary>
        /// File path for program FFmpeg.
        /// </summary>
        public static string ProgramFfmpeg { get; set; }
        /// <summary>
        /// File path for program mp3tag.
        /// </summary>
        public static string ProgramMp3tag { get; set; }

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
                if (String.IsNullOrEmpty(programYouTubeDlHistory))
                {
                    if (Directories.Data == null) return null;
                    programYouTubeDlHistory = Path.Combine(Directories.Data, nameProgramYouTubeDl);
                }
                return programYouTubeDlHistory;
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
    }
}