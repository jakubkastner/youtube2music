using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace youtube2music
{
    public class Album
    {
        private string slozkaAlbumu;

        private string zanrAlbumu;

        public string Nazev { get; set; }

        //public string Zanr { get; set; }

        public int Rok { get; set; }

        public Interpret Interpret { get; set; }


        public string Zanr
        {
            get
            {
                if (!String.IsNullOrEmpty(zanrAlbumu))
                {
                    return zanrAlbumu;
                }
                if (String.IsNullOrEmpty(this.Slozka))
                {
                    return "";
                }
                //string zanr = this.zanrAlbumu;
                string slozka = this.Slozka.ToLower();
                string zanr = DateTime.Now.ToString("yyyy.MM");
                if (slozka.Contains("rap"))
                {
                    zanr += " Rap";
                }
                else if (slozka.Contains("hip-hop"))
                {
                    zanr += " Hip-Hop";
                }
                else if (slozka.Contains("ostatní cz"))
                {
                    zanr += " Ostatní CZ";
                }
                else if (slozka.Contains("ostatní"))
                {
                    zanr += " Ostatní";
                }
                return zanr;
            }
            set
            {
                this.zanrAlbumu = value;
            }
        }

        public string Slozka
        {
            get
            {
                string slozka = this.slozkaAlbumu;
                string album = "";
                if (String.IsNullOrEmpty(slozka))
                {
                    if (Interpret != null)
                    {
                        if (Interpret.Slozky.Count > 0)
                        {
                            slozka = Interpret.Slozky.First();
                        }
                    }
                    if (Rok != 0)
                    {
                        album = Rok.ToString();
                    }
                    if (!String.IsNullOrEmpty(Nazev))
                    {
                        album += " " + Nazev;
                    }
                }
                if (!String.IsNullOrEmpty(album) && !String.IsNullOrEmpty(slozka))
                {
                    slozka = Path.Combine(slozka, album);
                }
                return slozka;
            }
            set
            {
                string novaSlozka = value;
                // odstraní mezery z názvu
                novaSlozka = OdstranVicenasobneMezery(novaSlozka);
                if (String.IsNullOrEmpty(novaSlozka))
                {
                    this.slozkaAlbumu = novaSlozka;
                    return;
                }

                // odstraní znaky složky, které se nemohou použít v názvu složky
                novaSlozka = String.Join("", novaSlozka.Split(Path.GetInvalidPathChars()));

                // odstraní ze složky dvojtečku
                string[] rozdelenaSlozka = novaSlozka.Split(':');
                if (rozdelenaSlozka.Length > 1)
                {
                    // nalezeno více dvojteček
                    novaSlozka = rozdelenaSlozka[0] + ':';
                    for (int i = 1; i < rozdelenaSlozka.Length; i++)
                    {
                        novaSlozka += rozdelenaSlozka[i];
                    }
                }
                // nalezena jedna dvojtečka = označení disku
                // nebo nenalezena žádná dvojtečka

                this.slozkaAlbumu = novaSlozka;
            }
        }
        
        public string Cover { get; set; }

        public Album(string nazev, int rok, Interpret inter, string zanr = "", string cover = "")
        {            
            nazev = Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(nazev.ToLower()).Replace(" i ", " I ");
            Nazev = nazev;
            Rok = rok;
            Interpret = inter;
            Cover = cover;
            if (!String.IsNullOrEmpty(zanr))
            {
                zanrAlbumu = zanr;
            }
        }

        // TODO PŘESUNOUT DO HROMADNÉ TŘÍDY (STEJNÁ FUNKCE SE POUŽÍVÁ PRO Video.cs)
        /// <summary>
        /// Nahradí z textu více mezer za sebou jednou mezerou.
        /// </summary>
        /// <param name="vstup">Text k nahrazení více mezer.</param>
        /// <returns>Text s maximálně jednou mezerou za sebou.</returns>
        private string OdstranVicenasobneMezery(string vstup)
        {
            if (string.IsNullOrEmpty(vstup))
            {
                return vstup;
            }
            Regex mezery = new Regex("\\s+"); // odstraní více mezer za sebou
            vstup = mezery.Replace(vstup, " ");
            return vstup;
        }
    }
}
