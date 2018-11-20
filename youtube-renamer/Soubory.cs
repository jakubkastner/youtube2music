using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;
using System.Text.RegularExpressions;

namespace hudba
{
    class Soubory
    {
        // přečte řádky
        public List<string> Precti(string cesta)
        {
            if (!File.Exists(cesta))
            {
                return null;
            }

            List<string> radky = new List<string>();
            using (FileStream str = new FileStream(cesta, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            using (StreamReader ctecka = new StreamReader(str))
            {
                while (!ctecka.EndOfStream)
                {
                    radky.Add(ctecka.ReadLine());
                }
            }
            return radky;
        }

        // zapíše řádky
        public void Zapis(string cesta, List<string> radkyKZapisu)
        {
            using (FileStream str = new FileStream(cesta, FileMode.Create, FileAccess.Write))
            {
                using (StreamWriter zapisovacka = new StreamWriter(str))
                {
                    foreach (string radek in radkyKZapisu)
                    {
                        zapisovacka.WriteLine(radek);
                    }
                }
            }            
        }
    }
}
