using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace youtube2music
{
    public class Album
    {
        private string slozkaAlbumu;

        //private string zanrAlbumu;

        public string Nazev { get; set; }

        //public string Zanr { get; set; }

        public int Rok { get; set; }

        public Interpret Interpret { get; set; }


        public string Zanr
        {
            get
            {
                //string zanr = this.zanrAlbumu;
                string slozka = this.Slozka.ToLower();
                string zanr = DateTime.Now.ToString("yyyy.MM");
                if (slozka.Contains("rap & hip-hop"))
                {
                    if (slozka.Contains("cz & sk"))
                    {
                        zanr += " Rap";
                    }
                    else if (slozka.Contains("ostatní země"))
                    {
                        zanr += " Hip-Hop";
                    }
                }
                else if (slozka.Contains("ostatní žánry"))
                {
                    if (slozka.Contains("cz & sk"))
                    {
                        zanr += " Ostatní CZ";
                    }
                    else if (slozka.Contains("ostatní země"))
                    {
                        zanr += " Ostatní";
                    }
                }
                return zanr;
            }
            /*set
            {
                this.zanrAlbumu = value;
            }*/
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
                    return Path.Combine(slozka, album);
                }
                return slozka;
            }
            set
            {
                this.slozkaAlbumu = value;
            }
        }
        
        public string Cover { get; set; }

        public Album(string nazev, int rok, Interpret inter, string cover = "")
        {
            Nazev = nazev;
            Rok = rok;
            Interpret = inter;
            Cover = cover;
        }
    }
}
