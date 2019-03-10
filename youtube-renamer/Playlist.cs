namespace youtube_renamer
{
    /// <summary>
    /// YouTube kanál videa.
    /// </summary>
    public class Playlist
    {
        /// <summary>
        /// ID YouTube kanálu.
        /// </summary>
        public string ID { get; private set; }
        /// <summary>
        /// Název YouTube kanálu.
        /// </summary>
        public string Nazev { get; private set; }

        /// <summary>
        /// Vytvoří nový kanál YouTube.
        /// </summary>
        /// <param name="id">ID YouTube kanálu.</param>
        /// <param name="nazev">Název YouTube kanálu.</param>
        public Playlist(string id, string nazev)
        {
            ID = id;
            Nazev = nazev;
        }
    }
}
