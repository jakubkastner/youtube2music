using System;

namespace youtube2music
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
        public string Url
        {
            get
            {
                return "https://www.youtube.com/playlist?list=" + ID;
            }
        }

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
        public Playlist(string id)
        {
            ID = id;
            ZiskejNazev();
        }

        private void ZiskejNazev()
        {
            try
            {
                Nazev = YouTubeApi.ZiskejNazevPlaylistu(ID);
            }
            catch (Exception ex)
            {

                Actions.Show.Error("chyba", ex.Message);
                return;
            }
        }
    }
}