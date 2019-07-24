using System.Collections.Generic;
using System.IO;

namespace youtube2music
{
    /// <summary>
    /// Soubor k přečtení nebo zápisu.
    /// </summary>
    class Soubor
    {
        /// <summary>
        /// Získá obsah (přečte a uloží) po řádcích zadaného souboru.
        /// </summary>
        /// <param name="cesta">Cesta souboru k získání obsahu.</param>
        /// <returns>Obsah souboru po jednotlivých řádcích.</returns>
        public List<string> Precti(string cesta)
        {
            if (!File.Exists(cesta))
            {
                // soubor neexistuje
                return null;
            }

            List<string> radky = new List<string>();
            using (FileStream str = new FileStream(cesta, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            using (StreamReader ctecka = new StreamReader(str))
            {
                while (!ctecka.EndOfStream)
                {
                    // přidá jednotlivé řádky souboru do listu "radky"
                    radky.Add(ctecka.ReadLine());
                }
            }
            return radky;
        }

        /// <summary>
        /// Zapíše obsah (po řádcích ze seznamu) do souboru.
        /// </summary>
        /// <param name="cesta">Cesta souboru k uložení.</param>
        /// <param name="radkyKZapisu">Seznam řádků k zápisu do souboru.</param>
        public void Zapis(string cesta, List<string> radkyKZapisu)
        {
            using (FileStream str = new FileStream(cesta, FileMode.Create, FileAccess.Write))
            {
                using (StreamWriter zapisovacka = new StreamWriter(str))
                {
                    foreach (string radek in radkyKZapisu)
                    {
                        // zapíše jednotlivé řádky do souboru
                        zapisovacka.WriteLine(radek);
                    }
                }
            }            
        }
    }
}
