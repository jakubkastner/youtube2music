using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace youtube2music
{
    public class Aplikace
    {
        public class Cesty
        {
            private string slozkaData = null;
            private string slozkaCache = null;
            private string slozkaOpus = null;
            private string slozkaMp3 = null;
            private string souborYoutubedl = null;
            private string souborFfmpeg = null;

            /// <summary>
            /// Získá složku s hudební knihovnou opus.
            /// </summary>
            public string HudebniKnihovnaOpus
            {
                get
                {
                    if (!ExistujeOpus()) return "";
                    return slozkaOpus;
                }
            }

            /// <summary>
            /// Změní složku hudební knihovny opus.
            /// </summary>
            /// <param name="novaSlozka"></param>
            /// <returns></returns>
            public bool ZmenOpus(string novaSlozka)
            {
                // kontrola existence složky
                if (!Directory.Exists(novaSlozka))
                {
                    return false;
                }
                slozkaOpus = novaSlozka;
                return true;
            }

            /// <summary>
            /// Zjistí, jestli existuje složka s hudební knihovnou opus.
            /// (true = existuje, false = neexistuje)
            /// </summary>
            /// <returns>true = složka existuje false = složka neexistuje</returns>
            public bool ExistujeOpus()
            {
                if (Directory.Exists(slozkaOpus)) return true;
                slozkaOpus = "";
                return false;
            }

        }
    }
}
