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
        public Aplikace() { }
        public class Cesty
        {
            /// <summary>
            /// Seznam složek hudební knihovny.
            /// (0 = opus, 1 = mp3)
            /// </summary>
            private List<string> slozkyKnihovny = new List<string>();

            private string slozkaData = null;
            private string slozkaCache = null;
            private string souborYoutubedl = null;
            private string souborFfmpeg = null;

            /// <summary>
            /// Získá složku hudební knihovny na vybraném indexu ze seznamu.
            /// (0 = opus, 1 = mp3)
            /// </summary>
            /// <param name="index">Index ze seznamu</param>
            /// <returns>existuje = cesta složky, neexistuje = null</returns>
            public string ZískejHudebniKnihovnu(int index)
            {
                // index je mimo hranice seznamu
                if (slozkyKnihovny.Count() <= index) return null;
                // složka neexistuje
                if (!ExistujeHudebniKnihovna(index)) return null;
                return slozkyKnihovny.ElementAt(index);
            }

            /// <summary>
            /// Přidá novou hudební knihovnu do seznamu.
            /// (0 = opus, 1 = mp3)
            /// </summary>
            /// <param name="novaSlozka">Cesta nové hudební knihovny.</param>
            /// <returns>true = úspěšně přidáno, false = nepřidáno</returns>
            public bool PridejHudebniKnihovnu(string novaSlozka)
            {
                // složka neexistuje
                if (!Directory.Exists(novaSlozka)) return false;
                // přidání složky do seznamu
                slozkyKnihovny.Add(novaSlozka);
                return true;
            }

            /// <summary>
            /// Změní existující hudební knihovnu ze seznamu.
            /// (0 = opus, 1 = mp3)
            /// </summary>
            /// <param name="novaSlozka">Cesta hudební knihovny</param>
            /// <param name="index">Index ze seznamu</param>
            /// <returns>true = úspěšně změněno, false = nezměněno</returns>
            public bool ZmenHudebniKnihovnu(string novaSlozka, int index)
            {
                // složka neexistuje
                if (!Directory.Exists(novaSlozka)) return false;
                // index je mimo hranice seznamu
                if (slozkyKnihovny.Count() <= index) return false;
                // změna cesty složky ze seznamu
                slozkyKnihovny[index] = novaSlozka;
                return true;
            }

            /// <summary>
            /// Odstraní složku hudební knihovny na vybraném indexu ze seznamu.
            /// (0 = opus, 1 = mp3)
            /// </summary>
            /// <param name="index">Index ze seznamu</param>
            public void OdeberHudebniKnihovnu(int index)
            {
                // odstraní složku ze seznamu
                slozkyKnihovny.RemoveAt(index);
            }

            /// <summary>
            /// Zjistí, jestli existuje složka hudební knihovny ze seznamu.
            /// </summary>
            /// <returns>true = složka existuje, false = složka neexistuje</returns>
            public bool ExistujeHudebniKnihovna(int index)
            {
                // index je mimo hranice seznamu
                if (slozkyKnihovny.Count() <= index) return false;
                // složka existuje
                if (Directory.Exists(slozkyKnihovny.ElementAt(index))) return true;
                // složka neexistuje
                slozkyKnihovny[index] = null;
                return false;
            }
            public Cesty() { }
        }
    }
}
