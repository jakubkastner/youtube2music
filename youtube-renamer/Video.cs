using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace youtube_renamer
{
    public class Video
    {
        public string ID { get; set; }
        public string Playlist { get; set; }
        public string Popis { get; set; }
        public VideoNazev Nazev { get; set; }
        public VideoKanal Kanal { get; set; }
        public DateTime Publikovano { get; set; }
        public string Zanr { get; set; }
        public VideoInterpret Interpret { get; set; }
        /// <summary>
        /// Seznam interpretů na featuringu.
        /// </summary>
        public List<VideoInterpret> Feat { get; set; }

        /// <summary>
        /// Interpreti na featuringu v textové podobě.
        /// </summary>
        /*public string FeatString
        {
            get
            {
                if (!String.IsNullOrEmpty(this.featuringUpraveny))
                {
                    // featuring byl upraven uživatelem ručně
                    return this.featuringUpraveny;
                }
                if (Feat == null)
                {
                    return "";
                }
                if (Feat.Count == 0)
                {
                    return "";
                }
                if (Feat.Count == 1)
                {
                    return Feat[0].Nazev;
                }
                // spojí pole interpretů na featu pomocí ", "
                string vysledek = String.Join(", ", Feat);
                // spojí poslední dva interprety na featuringu znakem " & "
                string posledni = Feat[Feat.Count - 1].Nazev;
                vysledek = vysledek.Replace(", " + posledni, " & " + posledni);
                return vysledek;
            }
            set
            {
                this.featuringUpraveny = value;
            }
        }*/
        public string Slozka { get; set; }
        public string Chyba { get; set; }
        public string Stav { get; set; }
        
        /// <summary>
        /// Pokud uživatel ručně upravil featuring v textové podobě, přepíše se ten automaticky vytvořený.
        /// </summary>
        //private string featuringUpraveny;

        public Video(string videoID, string videoPlaylist)
        {
            ID = videoID;
            Playlist = videoPlaylist;
            Popis = "";
            // název
            // kanál
            // publikováno
            Zanr = "";
            // interpret
            // feat
            Slozka = "";
            Chyba = "";
            Stav = "";
            Nazev = new VideoNazev(null/*this.Feat*/);
            // získá informace z youtube api
            try
            {
                YouTubeApi.ZiskejInfoVidea(this);
            }
            catch (Exception)
            {
                Chyba = "Chyba přidávání";
            }
            if (!String.IsNullOrEmpty(Chyba))
            {
                // video neexistuje, nebo nastala chyba přidávání videa
                return;
            }
            // Prejmenuj
        }
    }
}
