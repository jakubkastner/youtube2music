using System;
using System.Collections.Generic;
using System.IO;

namespace youtube2music
{
    /// <summary>
    /// Soubor k přečtení nebo zápisu.
    /// </summary>
    public class Soubor
    {
        /// <summary>
        /// Získá obsah (přečte a uloží) po řádcích zadaného souboru.
        /// </summary>
        /// <param name="cesta">Cesta souboru k získání obsahu.</param>
        /// <returns>Obsah souboru po jednotlivých řádcích.</returns>
        public static List<string> Precti(string cesta)
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
        public static void Zapis(string cesta, List<string> radkyKZapisu)
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

        // OK 2019
        /// <summary>
        /// Přesune vybraný soubor.
        /// </summary>
        /// <param name="soubor">cesta souboru k přesunutí</param>
        /// <param name="cilovaSlozka">cílová složka, kam se má soubor přesunout</param>
        /// <param name="novyNazev">nový název přesunutého souboru (pokud není vyplněno, použije se původní název)</param>
        /// <returns>
        ///     true = přesunutí proběhlo v pořádku
        ///     false = přesunutí se nezdařilo
        /// </returns>
        public static bool Presun(string soubor, string cilovaSlozka, string novyNazev = "")
        {
            // přesun souboru
            if (File.Exists(soubor))
            {
                try
                {
                    if (String.IsNullOrEmpty(novyNazev)) novyNazev = Path.GetFileName(soubor);
                    File.Copy(soubor, Path.Combine(cilovaSlozka, novyNazev), true);
                }
                catch (Exception)
                {
                    return false;
                }
                return true;
            }
            return false;
        }
    }
}
