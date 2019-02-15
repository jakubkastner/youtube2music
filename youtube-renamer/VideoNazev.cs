using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace youtube_renamer
{
    public class VideoNazev
    {
        public string Puvodni { get; set; }
        public string Novy { get; set; }
        public string NovyFeaturing
        {
            get
            {
                this.featuringNovy = Novy;
                if (Feat.Count > 0)
                {
                    // existuje featuring
                    this.featuringNovy += " (" + FeatString + ")";
                }
                return this.featuringNovy;
            }
            set
            {
                this.featuringNovy = value;
            }
        }

        private List<VideoInterpret> Feat { get; set; }
        public VideoNazev(List<VideoInterpret> feat)
        {
            this.Feat = feat;
        }
        /// <summary>
        /// Interpreti na featuringu v textové podobě.
        /// </summary>
        public string FeatString
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
        }
        /// <summary>
        /// Pokud uživatel ručně upravil featuring v textové podobě, přepíše se ten automaticky vytvořený.
        /// </summary>
        private string featuringUpraveny;
        private string featuringNovy;

        /// <summary>
        /// Název skladby s featuringem.
        /// Například: "nazev_skladby (ft. interpret1 & interpret2)".
        /// </summary>
        /*public string Featuring
        {
            get
            {
                string vysledek = Novy;
                if (Feat.Count > 0)
                {
                    // existuje featuring
                    vysledek += " (" + FeatString + ")";
                }
                return vysledek;
            }
        }*/
        /// <summary>
        /// Název hledané skladby pro hledání v hudební knihovně.
        /// </summary>
        public string Hledany
        {
            get
            {
                return Novy.Replace('/', ' ').Replace(':', ' ');
            }
        }

    }
}
