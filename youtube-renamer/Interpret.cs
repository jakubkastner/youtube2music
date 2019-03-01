using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace youtube_renamer
{
    /// <summary>
    /// Interpret skladby.
    /// </summary>
    public class Interpret
    {
        /// <summary>
        /// Upravené jméno interpreta.
        /// </summary>
        public string Jmeno { get; set; }
        /// <summary>
        /// Seznam nalezených složek interpreta.
        /// </summary>
        public List<string> Slozky { get; set; }

        /// <summary>
        /// Vytvoří instanci nového interpreta a automaticky přejmenuje a upraví jeho jméno.
        /// </summary>
        /// <param name="jmenoInterpreta">Jméno přidávaného interpreta.</param>
        public Interpret(string jmenoInterpreta)
        {
            this.Slozky = new List<string>();
            Jmeno = jmenoInterpreta.Trim();
            Regex zavorka = new Regex(@"\(([^\}]+)\)"); // odstraní cokoliv v závorce ( )
            Jmeno = zavorka.Replace(Jmeno, "");

            // nový interpret je prázdný
            if (String.IsNullOrEmpty(Jmeno))
            {
                Jmeno = "";
                return;
            }

            Prejmenuj();

            Jmeno = Jmeno.ToLower();
            VelkaPismena(' ');
            VelkaPismena('.');
            VelkaPismena('-');

            // interpreti s tečkou
            /*
             PeatyF.
             M.I.A.
             B.o.B.
            */
        }

        public void NajdiSlozky()
        {
            /*string hledanyInterpret = OdstranZacatek(Interpret);
            hledanyInterpret = OdstranKonec(hledanyInterpret);
            hledanyInterpret = hledanyInterpret.Trim().ToLower();*/


            string slozkaProgramuData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            slozkaProgramuData = Path.Combine(slozkaProgramuData, "youtube-renamer", "data");

            Soubor soubor = new Soubor();
            // složky hudební knihovny načtené ze souboru "knihovna_slozky.txt"
            List<string> slozkyKnihovna = soubor.Precti(Path.Combine(slozkaProgramuData, "knihovna_slozky.txt"));

            // slozkyKnihovna == null nebo slozkyKnihovna.Count == 0 -> chyba čtení souboru
            if (slozkyKnihovna == null)
            {
                return;
            }
            if (slozkyKnihovna.Count == 0)
            {
                return;
            }

            List<string> slozkyKnihovnaOstatni = new List<string>();
            string hledanyInterpret = Jmeno.Trim().ToLower();
            // projde názvy složek ze souboru "knihovna_slozky.txt"
            foreach (string slozkaKnihovnaCesta in slozkyKnihovna)
            {
                // název složky z knihovny
                string slozkaKnihovnaNazev = Path.GetFileName(slozkaKnihovnaCesta).Trim().ToLower();
                // první interpret z názvu složky v knihovně
                string slozkaKnihovnaPrvniInterpret = slozkaKnihovnaNazev.Split(new[] { " & ", ", " }, StringSplitOptions.None).First().ToLower().Trim();

                // existuje složka s názvem interpreta
                if (slozkaKnihovnaPrvniInterpret == hledanyInterpret)
                {
                    Slozky.Add(slozkaKnihovnaCesta);
                }
                // najdi soubor ve složce _ostatní a zjisti zdali tam již není
                else if (slozkaKnihovnaNazev == "_ostatní")
                {
                    slozkyKnihovnaOstatni.Add(slozkaKnihovnaCesta);
                }
            }
            // nebyly nalezeny složky interpreta
            if (Slozky.Count == 0)
            {
                // najde zdali se interpret nenachází ve složce ostatní 
                foreach (string ostatniSlozka in slozkyKnihovnaOstatni)
                {
                    if (NajdiSouborOstatni(ostatniSlozka))
                    {
                        Slozky.Add(ostatniSlozka);
                    }
                }
            }
        }
        private bool NajdiSouborOstatni(string slozka)
        {
            // úprava - přidat další koncovky !!!!!!
            // vrací true, pokud nalezne soubor, který má před pomlčkou název interpreta
            try
            {
                foreach (string cestaSoubor in Directory.GetFiles(slozka))
                {
                    string hledanyInterpret = Jmeno.Trim().ToLower();
                    string nazevSoubor = Path.GetFileNameWithoutExtension(cestaSoubor).ToLower().Trim();
                    string[] rozdelenyNazev = nazevSoubor.Split('-');
                    if (rozdelenyNazev == null)
                    {
                        return false;
                    }
                    if (rozdelenyNazev[0].Trim() == hledanyInterpret)
                    {
                        return true;
                    }
                }
            }
            catch (Exception)
            {
                return false;
            }
            return false;
        }

        // HOTOVO
        /// <summary>
        /// Změní jméno interpreta dle preferovaných úprav.
        /// </summary>
        private void Prejmenuj()
        {
            Jmeno = Jmeno.Replace("asap", "a$ap")
                         .Replace("a$ap jarda", "asap jarda");
            switch (Jmeno)
            {
                // cz + sk
                case "jickson":
                    Jmeno = "jimmy dickson";
                    break;
                case "jckpt":
                    Jmeno = "jackpot";
                    break;
                case "radikal chef":
                    Jmeno = "radikal";
                    break;
                case "s.barracuda":
                    Jmeno = "sergei barracuda";
                    break;
                case "white russian":
                    Jmeno = "igor";
                    break;
                case "white rusian":
                    Jmeno = "igor";
                    break;
                case "mladej moris":
                    Jmeno = "yg moris";
                    break;
                case "gleb : zoo":
                    Jmeno = "gleb";
                    break;
                case "peatyf":
                    Jmeno = "peatyf.";
                    break;
                case "sheen":
                    Jmeno = "viktor sheen";
                    break;
                case "yzomandias":
                    Jmeno = "logic";
                    break;
                case "hráč roku":
                    Jmeno = "logic";
                    break;
                case "lvcas":
                    Jmeno = "lvcas dope";
                    break;
                case "kanabis":
                    Jmeno = "lvcas dope";
                    break;
                case "mike t":
                    Jmeno = "dj mike trafik";
                    break;
                case "mike trafik":
                    Jmeno = "dj mike trafik";
                    break;
                case "sensey syfu":
                    Jmeno = "senseysyfu";
                    break;
                case "karlo":
                    Jmeno = "gumbgu";
                    break;
                // zahraniční
                case "the black eyed peas":
                    Jmeno = "black eyed peas";
                    break;
                case "tekashi69":
                    Jmeno = "6ix9ine";
                    break;
                case "6ixty9ine":
                    Jmeno = "6ix9ine";
                    break;
                case "slim jxmmi of rae sremmurd":
                    Jmeno = "slim jxmmi";
                    break;
                case "jeezy":
                    Jmeno = "young jeezy";
                    break;
                case "waka flocka":
                    Jmeno = "waka flocka flame";
                    break;
                case "g herbo":
                    Jmeno = "lil herb";
                    break;
                case "v.cha$e":
                    Jmeno = "vinny cha$e";
                    break;
                case "joey bada$$":
                    Jmeno = "joey badass";
                    break;
                case "gab3":
                    Jmeno = "uzi";
                    break;
                case "travis scott":
                    Jmeno = "travi$ scott";
                    break;
                case "ty dolla sign":
                    Jmeno = "ty dolla $ign";
                    break;
                case "mgk":
                    Jmeno = "machine gun kelly";
                    break;
                default:
                    break;
            }
        }

        // HOTOVO
        /// <summary>
        /// Změní jméno interpreta na velká počáteční písmena po zadaném oddělovači.
        /// </summary>
        /// <param name="oddelovac">Znak, po kterém budou velká písmena ve vstupu.</param>
        private void VelkaPismena(char oddelovac)
        {
            // pokud je posledním indexu oddělovač, odstraním ho a na konci zase přidám
            bool oddelovacNaKonci = false;
            if (Jmeno.Last() == oddelovac)
            {
                Jmeno = Jmeno.TrimEnd(oddelovac);
                oddelovacNaKonci = true;
            }

            // rozdělí vstupní text pomocí oddělovače
            string[] oddelene = Jmeno.Split(oddelovac);
            Jmeno = "";
            for (int i = 0; i < oddelene.Length; i++)
            {
                if (!String.IsNullOrEmpty(oddelene[i]))
                {
                    // převede první znak na velké písmeno
                    Jmeno += oddelene[i][0].ToString().ToUpper() + oddelene[i].Substring(1, oddelene[i].Length - 1);
                }
                if (oddelene.Length > 1)
                {
                    // nejedná se o poslední část nebo byl odstraněn oddělovač na začátku
                    if ((oddelene.Length - 1 != i) || oddelovacNaKonci)
                    {
                        // přidám oddělovač
                        Jmeno += oddelovac;
                    }
                }
            }
            // převedení velikosti názvů u interpretů
            Jmeno = Jmeno.Replace("..", ".")
                         .Replace("Dj", "DJ")
                         .Replace("A$ap", "A$AP")
                         .Replace("Asap", "ASAP")
                         .Replace("Mike Will Made It", "Mike WiLL Made It")
                         .Replace("Og Maco", "OG Maco")
                         .Replace("Schoolboy Q", "ScHoolboy Q")
                         .Replace("114kd", "114KD")
                         .Replace("B.O.B", "B.o.B")
                         .Replace("Yg", "YG")
                         .Replace("Bomfunk Mc", "Bomfunk MC")
                         .Replace("Travie Mccoy", "Travie McCoy")
                         .Replace("Ca$h Out", "CA$H OUT")
                         .Replace("Cl", "CL")
                         .Replace("DJ Xo", "DJ XO")
                         .Replace("Ishdarr", "IshDARR")
                         .Replace("Ost", "OST")
                         .Replace("Outkast", "OutKast")
                         .Replace("6lack", "6LACK")
                         .Replace("Charli Xcx", "Charli XCX")
                         .Replace("Xxxtentacion", "XXXTentacion")
                         .Replace("Omenxiii", "OmenXIII")
                         .Replace("Olaawave", "OlaaWave")
                         .Replace("Partynextdoor", "PARTYNEXTDOOR")
                         .Replace("Ilovemakonnen", "ILoveMakonnen")
                         .Replace("Rj", "RJ")
                         .Replace("Smoke Dza", "Smoke DZA")
                         .Replace("Tc Da Loc", "TC Da Loc")
                         .Replace("Jay Z", "Jay-Z")
                         .Replace("T Pain", "T-Pain")
                         .Replace("Flo Rida", "Flo-Rida")
                         .Replace("T Wayne", "T-Wayne")
                         .Replace("Yeezuz2020", "yeezuz2020")
                         .Replace("4d", "4D")
                         .Replace("DstfrsS", "DSTFRS")
                         .Replace("Inny Rap", "INNY rap")
                         .Replace("Jmy", "JMY")
                         .Replace("Maat", "MAAT")
                         .Replace("Nobodylisten", "NobodyListen")
                         .Replace("Peatyf", "PeatyF")
                         .Replace("Pnzkjs", "PNZKJS")
                         .Replace("Psh", "PSH")
                         .Replace("Senseysyfu", "SenseySyfu")
                         .Replace("Wip", "WIP")
                         .Replace("Www", "WWW")
                         .Replace("Dms", "DMS")
                         .Replace("Mpp", "MPP")
                         .Trim();
        }
    }
}
