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

        public string Nazev { get; set; }

        public int Rok { get; set; }

        public Interpret Interpret { get; set; }

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

        public Album(string nazev, int rok, Interpret inter, string cover)
        {
            Nazev = nazev;
            Rok = rok;
            Interpret = inter;
            Cover = cover;
        }
    }
}
