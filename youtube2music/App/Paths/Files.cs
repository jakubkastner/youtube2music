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
        private static readonly string nameHistory = "history.txt";
        private static readonly string nameYouTubeUser = "Google.Apis.Auth.OAuth2.Responses.TokenResponse-user";

        /// <summary>
        /// Get file path for program youtube-dl.
        /// </summary>
        public static string ProgramYouTubeDl
        {
            get
            {
                if (Directories.Data == null) return null;
                return Path.Combine(Directories.Data, nameProgramYouTubeDl);
            }
        }
        /// <summary>
        /// Get file path for program FFmpeg.
        /// </summary>
        public static string ProgramFfmpeg
        {
            get
            {
                if (Directories.Data == null) return null;
                return Path.Combine(Directories.Data, nameProgramFfmpeg);
            }
        }
        /// <summary>
        /// Get file path for program mp3tag.
        /// </summary>
        public static string ProgramMp3tag
        {
            get
            {
                if (Directories.Data == null) return null;
                return Path.Combine(Directories.Data, nameProgramMp3tag);
            }
        }
        /// <summary>
        /// Get file path for downloaded history.
        /// </summary>
        public static string History
        {
            get
            {
                if (Directories.Data == null) return null;
                return Path.Combine(Directories.Data, nameHistory);
            }
        }
        /// <summary>
        /// Get file path for logged in YouTube user.
        /// </summary>
        public static string YouTubeUser
        {
            get
            {
                if (Directories.Data == null) return null;
                return Path.Combine(Directories.Data, nameYouTubeUser);
            }
        }
    }
}