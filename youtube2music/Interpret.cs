using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace youtube2music
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
            Slozky = new List<string>();
            Jmeno = jmenoInterpreta.Trim();
            Regex zavorka = new Regex(@"\(([^\}]+)\)"); // odstraní cokoliv v závorce ( )
            Jmeno = zavorka.Replace(Jmeno, "");
            Jmeno = Jmeno.Replace("(", "")
                         .Replace(")", "")
                         .Trim();

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
            PrejmenujInterpreta("PeatyF", "PeatyF.");
            PrejmenujInterpreta("M.I.A", "M.I.A.");
            PrejmenujInterpreta("B.o.B", "B.o.B.");
            PrejmenujInterpreta("P.A.T", "P.A.T.");
        }

        /// <summary>
        /// Najde složky přidávaného interpreta.
        /// </summary>
        public void NajdiSlozky()
        {
            // získá umístění složky programu
            string slozkaProgramuData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            slozkaProgramuData = Path.Combine(slozkaProgramuData, "youtube-renamer", "data");

            // složky hudební knihovny načtené ze souboru "knihovna_slozky.txt"
            List<string> slozkyKnihovna = Soubor.Precti(Path.Combine(slozkaProgramuData, "knihovna_slozky.txt"));

            // nenalezeny žádné složky knihovny
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
            if (String.IsNullOrEmpty(hledanyInterpret))
            {
                return;
            }
            while (hledanyInterpret.Last() == '.')
            {
                hledanyInterpret = hledanyInterpret.Remove(hledanyInterpret.Length - 1, 1);
            }
            bool zakladniSlozka = false; // true pokud existuje složka s názvem interpreta bez jiných hostujících interpretů

            // projde názvy složek ze souboru "knihovna_slozky.txt"
            foreach (string slozkaKnihovnaCesta in slozkyKnihovna)
            {
                // získá název složky z knihovny a seznam interpretů
                string slozkaKnihovnaNazev = Path.GetFileName(slozkaKnihovnaCesta).Trim().ToLower();                
                List<string> interpretiSlozky = new List<string>();
                if (slozkaKnihovnaNazev.Contains(" & ") && !hledanyInterpret.Contains(" & "))
                {
                    // v názvu složky je více intepretů, rozdělím je
                    interpretiSlozky.AddRange(slozkaKnihovnaNazev.Split(new[] { " & ", ", " }, StringSplitOptions.None));
                }
                else
                {
                    // v názvu složky není více interpretů
                    interpretiSlozky.Add(slozkaKnihovnaNazev);
                }

                // získá název prvního interpreta složky
                string slozkaKnihovnaPrvniInterpret = interpretiSlozky.First().ToLower().Trim();
                if (slozkaKnihovnaPrvniInterpret == hledanyInterpret)
                {
                    // existuje složka s názvem interpreta
                    Slozky.Add(slozkaKnihovnaCesta);
                    if (interpretiSlozky.Count == 1)
                    {
                        // existuje složka s názvem interpreta bez jiných hostujících interpretů
                        zakladniSlozka = true;
                    }
                }
                // najdi soubor ve složce _ostatní a zjisti zdali tam již není
                else if (slozkaKnihovnaNazev == "_ostatní")
                {
                    slozkyKnihovnaOstatni.Add(slozkaKnihovnaCesta);
                }
            }

            // nebyly nalezeny složky interpreta nebo nebyla nalezena základní složka interpreta
            if (Slozky.Count == 0 || !zakladniSlozka)
            {
                // najde zdali se interpret nenachází ve složce ostatní 
                foreach (string ostatniSlozka in slozkyKnihovnaOstatni)
                {
                    if (NajdiSlozkuOstatni(ostatniSlozka))
                    {
                        // interpret byl nalezen ve složce "ostatní"
                        Slozky.Add(ostatniSlozka);
                    }
                }
            }
        }

        /// <summary>
        /// Najde interperta ve složce "ostatní".
        /// </summary>
        /// <param name="slozka">Cesta prohledávané složky.</param>
        /// <returns>true = interpret nalezen ve složce, false = interpret ve složce nenalezen</returns>
        private bool NajdiSlozkuOstatni(string slozka)
        {
            try
            {
                // prohledá všechy soubory ve složce "ostatní"
                foreach (string cestaSoubor in Directory.GetFiles(slozka))
                {
                    // získá jméno hledaného interpreta a název souboru ve složce "ostatní"
                    string hledanyInterpret = Jmeno.Trim().ToLower();
                    string nazevSoubor = Path.GetFileNameWithoutExtension(cestaSoubor).ToLower().Trim();
                    // rozdělí název souboru ze složky "ostatní" podle pomlčky
                    string[] rozdelenyNazev = nazevSoubor.Split('-');
                    if (rozdelenyNazev == null)
                    {
                        return false;
                    }
                    if (rozdelenyNazev[0].Trim() == hledanyInterpret)
                    {
                        // interpet ze souboru ve složce "ostatní" se shoduje s názvem interpreta
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

        /// <summary>
        /// Změní jméno interpreta dle preferovaných úprav.
        /// </summary>
        private void Prejmenuj()
        {
            Jmeno = Jmeno.Replace("asap", "a$ap")
                         .Replace("a$ap jarda", "asap jarda");
            // cz + sk
            PrejmenujInterpreta("smack one", "smack");
            PrejmenujInterpreta("jickson", "jimmy dickson");
            PrejmenujInterpreta("jckpt", "jackpot");
            PrejmenujInterpreta("radikal chef", "radikal");
            PrejmenujInterpreta("white russian", "igor");
            PrejmenujInterpreta("white rusian", "igor");
            PrejmenujInterpreta("bílej rus", "igor");
            PrejmenujInterpreta("s.barracuda", "sergei barracuda");
            PrejmenujInterpreta("s. barracuda", "sergei barracuda");
            PrejmenujInterpreta("ak", "pastor & sergei barracuda"); // asi problém s velkýma písmenama
            PrejmenujInterpreta("yg moris", "mladej moris");
            PrejmenujInterpreta("gleb , zoo", "gleb");
            PrejmenujInterpreta("gleb:zoo", "zmrd");
            PrejmenujInterpreta("gleb : zoo", "zmrd");
            PrejmenujInterpreta("peatyf", "peatyf.");
            PrejmenujInterpreta("sheen", "viktor sheen");
            PrejmenujInterpreta("yzomandias", "logic");
            PrejmenujInterpreta("hráč roku", "logic");
            PrejmenujInterpreta("lvcas", "lvcas dope");
            PrejmenujInterpreta("kanabis", "lvcas dope");
            PrejmenujInterpreta("mike t", "dj mike trafik");
            PrejmenujInterpreta("mike trafik", "dj mike trafik");
            PrejmenujInterpreta("sensey syfu", "senseysyfu");
            PrejmenujInterpreta("karlo", "gumbgu");
            PrejmenujInterpreta("dokkey", "dokkeytino");
            PrejmenujInterpreta("otis", "otecko");
            PrejmenujInterpreta("samurai pítr", "samuraj pítr");
            PrejmenujInterpreta("samuray pítr", "samuraj pítr");
            PrejmenujInterpreta("sensey", "senseysyfu");
            PrejmenujInterpreta("†holyvandalboi†", "vandal");
            PrejmenujInterpreta("skinny barber", "barber");
            PrejmenujInterpreta("opak dissu", "opak dissu label");
            PrejmenujInterpreta("kamil hoffmann", "zmrd");
            PrejmenujInterpreta("samuraj pitr", "samuraj pítr");
            PrejmenujInterpreta("p$ycho rhyme", "psycho rhyme");
            PrejmenujInterpreta("luna 99", "luna");

            // zahraniční
            PrejmenujInterpreta("the black eyed peas", "black eyed peas");
            PrejmenujInterpreta("tekashi69", "6ix9ine");
            PrejmenujInterpreta("6ixty9ine", "6ix9ine");
            PrejmenujInterpreta("slim jxmmi of rae sremmurd", "slim jxmmi");
            PrejmenujInterpreta("jeezy", "young jeezy");
            PrejmenujInterpreta("waka flocka", "waka flocka flame");
            PrejmenujInterpreta("g herbo", "lil herb");
            PrejmenujInterpreta("v.cha$e", "vinny cha$e");
            PrejmenujInterpreta("joey bada$$", "joey badass");
            PrejmenujInterpreta("gab3", "uzi");
            PrejmenujInterpreta("travis scott", "travi$ scott");
            PrejmenujInterpreta("joey badass", "joey bada$$");
            PrejmenujInterpreta("ty dolla sign", "ty dolla $ign");
            PrejmenujInterpreta("mgk", "machine gun kelly");
            PrejmenujInterpreta("mustard", "dj mustard");
            PrejmenujInterpreta("pharell", "pharell williams");
        }

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
                         .Replace("Dj ", "DJ ")
                         .Replace("A$ap ", "A$AP ")
                         .Replace("Asap ", "ASAP ")
                         .Replace("Nba ", "NBA ");

            PrejmenujInterpreta("Mike Will Made It", "Mike WiLL Made It");
            PrejmenujInterpreta("Og Maco", "OG Maco");
            PrejmenujInterpreta("Schoolboy Q", "ScHoolboy Q");
            PrejmenujInterpreta("114kd", "114KD");
            PrejmenujInterpreta("B.O.B", "B.o.B");
            PrejmenujInterpreta("Yg", "YG");
            PrejmenujInterpreta("Bomfunk Mc", "Bomfunk MC");
            PrejmenujInterpreta("Travie Mccoy", "Travie McCoy");
            PrejmenujInterpreta("Ca$h Out", "CA$H OUT");
            PrejmenujInterpreta("Cl", "CL");
            PrejmenujInterpreta("DJ Xo", "DJ XO");
            PrejmenujInterpreta("Ishdarr", "IshDARR");
            PrejmenujInterpreta("Ost", "OST");
            PrejmenujInterpreta("Outkast", "OutKast");
            PrejmenujInterpreta("6lack", "6LACK");
            PrejmenujInterpreta("Charli Xcx", "Charli XCX");
            PrejmenujInterpreta("Xxxtentacion", "XXXTentacion");
            PrejmenujInterpreta("Omenxiii", "OmenXIII");
            PrejmenujInterpreta("Olaawave", "OlaaWave");
            PrejmenujInterpreta("Partynextdoor", "PARTYNEXTDOOR");
            PrejmenujInterpreta("Ilovemakonnen", "ILoveMakonnen");
            PrejmenujInterpreta("Rj", "RJ");
            PrejmenujInterpreta("Smoke Dza", "Smoke DZA");
            PrejmenujInterpreta("Tc Da Loc", "TC Da Loc");
            PrejmenujInterpreta("Jay Z", "Jay-Z");
            PrejmenujInterpreta("T Pain", "T-Pain");
            PrejmenujInterpreta("Flo Rida", "Flo-Rida");
            PrejmenujInterpreta("T Wayne", "T-Wayne");
            PrejmenujInterpreta("Ta Ra", "Ta-Ra");
            PrejmenujInterpreta("Alt J", "Alt-J");
            PrejmenujInterpreta("Yeezuz2020", "yeezuz2020");
            PrejmenujInterpreta("4d", "4D");
            PrejmenujInterpreta("DstfrsS", "DSTFRS");
            PrejmenujInterpreta("Inny Rap", "INNY rap");
            PrejmenujInterpreta("Jmy", "JMY");
            PrejmenujInterpreta("Maat", "MAAT");
            PrejmenujInterpreta("Nobodylisten", "NobodyListen");
            PrejmenujInterpreta("Peatyf", "PeatyF");
            PrejmenujInterpreta("Pnzkjs", "PNZKJS");
            PrejmenujInterpreta("Psh", "PSH");
            PrejmenujInterpreta("Senseysyfu", "SenseySyfu");
            PrejmenujInterpreta("Wip", "WIP");
            PrejmenujInterpreta("Www", "WWW");
            PrejmenujInterpreta("Dms", "DMS");
            PrejmenujInterpreta("Rnz", "RNZ");
            PrejmenujInterpreta("Mpp", "MPP");
            PrejmenujInterpreta("Ptk", "PTK");
            PrejmenujInterpreta("Tkx", "TKX");
            PrejmenujInterpreta("Tm88", "TM88");
            PrejmenujInterpreta("G Eazy", "G-Eazy");
            PrejmenujInterpreta("Lordk", "LordK");
            PrejmenujInterpreta("Idk", "IDK");
            PrejmenujInterpreta("Jpegmafia", "JPEGmafia");
            PrejmenujInterpreta("Zillakami", "ZillaKami");
            PrejmenujInterpreta("Pashapg", "PashaPG");
            PrejmenujInterpreta("Juice Wrld", "Juice WRLD");
            PrejmenujInterpreta("Tyler The Creator", "Tyler, The Creator");
            PrejmenujInterpreta("Iamddb", "IAMDDB");
            PrejmenujInterpreta("Vr", "VR");
            PrejmenujInterpreta("Vr/Nobody", "VR/Nobody");
            PrejmenujInterpreta("Blockboy Jb", "Blockboy JB");
            PrejmenujInterpreta("Jl Beats", "JL Beats");
            PrejmenujInterpreta("Madeintyo", "MadeinTYO");
            PrejmenujInterpreta("Cbch", "CBCH");
            PrejmenujInterpreta("Aj Tracey", "AJ Tracey");
            PrejmenujInterpreta("Specialbeatz", "SpecialBeatz");
            PrejmenujInterpreta("Megam", "MegaM");
            PrejmenujInterpreta("Warhol.Ss", "Warhol.SS");
            PrejmenujInterpreta("Saint Jhn", "SAINt JHN");
            PrejmenujInterpreta("Blueraykoranthug", "BlueRayKoranThug");
            PrejmenujInterpreta("A Boogie With Da Hoodie", "A Boogie with da Hoodie");
            PrejmenujInterpreta("Dababy", "DaBaby");
            PrejmenujInterpreta("Astralkid22", "AstralKid22");
        }

        private void PrejmenujInterpreta(string puvodni, string novy)
        {
            if (String.Equals(Jmeno, puvodni, StringComparison.OrdinalIgnoreCase))
            {
                Jmeno = novy.Trim();
            }
        }
    }
}
