using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Text.RegularExpressions;
using System.Globalization;
using System.IO;
using System.ComponentModel;

namespace youtube_renamer
{
    public class VideoStare
    {
        // 0 video id, 1 poznámka, 2 kanál, 3 kanál id, 4 datum publikování, 5 žánr, 6 původní název, 7 interpret, 8 skladba, 9 feat (featuring) list
        // 10 featuring text, 11 skladba + featuring 12 nový název, 13 složka, 14 chyba, 15 skupina, 16 zaškrtnuto
        public string id { get; set; }
        public string popis { get; set; }
        public string kanal { get; set; }
        public string kanalID { get; set; }
        public DateTime publikovano { get; set; }
        public string zanr { get; set; }
        public string nazevPuvodni { get; set; }
        public string interpret { get; set; }
        public string skladba { get; set; }
        /// <summary>
        /// Název hledané skladby.
        /// </summary>
        public string skladbaHledana {
            get
            {
                return skladba.Replace('/', ' ').Replace(':', ' ');
            }
        }
        /// <summary>
        /// Seznam interpretů na featuringu.
        /// </summary>
        public List<string> feat { get; set; }


        private string featuringUpraveny;
        /// <summary>
        /// Interpreti na featuringu v textové podobě.
        /// </summary>
        public string Featuring
        {
            get
            {
                if (!String.IsNullOrEmpty(this.featuringUpraveny))
                {
                    // featuring byl upraven
                    return this.featuringUpraveny;
                }
                if (feat == null)
                {
                    return "";
                }
                if (feat.Count == 0)
                {
                    return "";
                }
                if (feat.Count == 1)
                {
                    return feat.First();
                }

                // odstraní cokoliv v závorce ( ); 
                string vysledek = String.Join(", ", feat);
                vysledek = vysledek.Replace(", " + feat.Last(), " & " + feat.Last());
                return vysledek;
            }
            set
            {
                this.featuringUpraveny = value;
            }
        }
        public string skladbaFeaturing { get; set; }
        public string nazevNovy { get; set; }
        public string slozka { get; set; }
        public string chyba { get; set; }
        public string skupina { get; set; }
        public string stav { get; set; }
        public string playlist { get; set; }

        
        // HOTOVO
        /// <summary>
        /// Vytvoří nové Video.
        /// </summary>
        /// <param name="videoID">ID nového videa.</param>
        /// <param name="playlist">Playlist, ze kterého je video. Pokud se nejdená o playlist, použije se id vieda.</param>
        public VideoStare(string videoID, string playlist)
        {
            id = videoID;
            popis = "";
            kanal = "";
            kanalID = "";
            zanr = "";
            nazevPuvodni = "";
            interpret = "";
            skladba = "";
            skladbaFeaturing = "";
            nazevNovy = "";
            slozka = "";
            chyba = "";
            skupina = "";
            // získá informace z youtube api
            YouTubeApi.ZiskejInfoVideaStare(this, playlist);
            // přejmenuje 
            Prejmenuj();
        }

        // HOTOVO
        /// <summary>
        /// Odstraní přebytečný text z názvu (např. "music video", "officaial video", ...).
        /// </summary>
        /// <param name="vstup">Text k odstranění přebytečného textu.</param>
        /// <returns>Název bez přebytečného textu.</returns>
        private string OdstranZbytecnosti(string vstup)
        {
            string[] odstran = { "off.", "off vid", "off vd", "music video", "official", " vd", "audio", "prod", "wshh" };
            for (int i = 0; i < odstran.Length; i++)
            {
                // odstraní postupně všehchny přebytečn text z pole
                if (vstup.Contains(odstran[i]))
                {
                    vstup = vstup.Substring(0, vstup.IndexOf(odstran[i])).Trim();
                }
            }
            // nahradí lomítka
            vstup = vstup.Replace("//", "/")
                         .Replace(@"\\", "/")
                         .Trim();
            int posledniIndex = vstup.Length - 1;
            if (posledniIndex < 0)
            {
                // krátký název
                return vstup;
            }
            // poslední slovo obsahuje zbytečnost
            if ((vstup[posledniIndex] == '/') ||
                (vstup[posledniIndex] == '*') ||
                (vstup[posledniIndex] == '-') ||
                (vstup[posledniIndex] == '"') ||
                (vstup[posledniIndex] == '~') ||
                (vstup[posledniIndex] == ','))
            {
                // odtraním ho
                vstup = vstup.Substring(0, posledniIndex).Trim();
                posledniIndex = vstup.Length - 1;
            }
            // první slovo obsahuje zbytečnost
            if ((vstup[0] == '/') ||
                (vstup[0] == '*') ||
                (vstup[0] == '-') ||
                (vstup[0] == '"') ||
                (vstup[0] == '~') ||
                (vstup[0] == '.'))
            {
                // odtraním ho
                vstup = vstup.Substring(1, posledniIndex).Trim();
            }
            vstup = vstup.Trim();
            return vstup;
        }

        // HOTOVO
        // přesunuto
        private string ZmenaNazvuInterpreta(string interpret)
        {
            #region switch
            switch (interpret)
            {
                // cz + sk
                case "jickson":
                    interpret = "jimmy dickson";
                    break;
                case "jckpt":
                    interpret = "jackpot";
                    break;
                case "radikal chef":
                    interpret = "radikal";
                    break;
                case "s.barracuda":
                    interpret = "sergei barracuda";
                    break;
                case "white russian":
                    interpret = "igor";
                    break;
                case "white rusian":
                    interpret = "igor";
                    break;
                case "mladej moris":
                    interpret = "yg moris";
                    break;
                case "gleb : zoo":
                    interpret = "gleb";
                    break;
                case "peatyf":
                    interpret = "peatyf.";
                    break;
                case "sheen":
                    interpret = "viktor sheen";
                    break;
                case "yzomandias":
                    interpret = "logic";
                    break;
                case "hráč roku":
                    interpret = "logic";
                    break;
                case "lvcas":
                    interpret = "lvcas dope";
                    break;
                case "kanabis":
                    interpret = "lvcas dope";
                    break;
                case "mike t":
                    interpret = "dj mike trafik";
                    break;
                case "mike trafik":
                    interpret = "dj mike trafik";
                    break;
                case "sensey syfu":
                    interpret = "senseysyfu";
                    break;
                case "karlo":
                    interpret = "gumbgu";
                    break;
                // zahraniční
                case "the black eyed peas":
                    interpret = "black eyed peas";
                    break;
                case "6ixty9ine":
                    interpret = "6ix9ine";
                    break;
                case "slim jxmmi of rae sremmurd":
                    interpret = "slim jxmmi";
                    break;
                case "jeezy":
                    interpret = "young jeezy";
                    break;
                case "waka flocka":
                    interpret = "waka flocka flame";
                    break;
                case "g herbo":
                    interpret = "lil herb";
                    break;
                case "v.cha$e":
                    interpret = "vinny cha$e";
                    break;
                case "joey bada$$":
                    interpret = "joey badass";
                    break;
                case "gab3":
                    interpret = "uzi";
                    break;
                case "travis scott":
                    interpret = "travi$ scott";
                    break;
                case "ty dolla sign":
                    interpret = "ty dolla $ign";
                    break;
                case "mgk":
                    interpret = "machine gun kelly";
                    break;
                case "tekashi69":
                    interpret = "6ix9ine";
                    break;
                default:
                    break;
            }
            #endregion
            interpret = interpret.Replace("asap", "a$ap");
            return interpret;
        }

        // HOTOVO
        /// <summary>
        /// Převede text na velká počáteční písmena po oddělovači.
        /// </summary>
        /// <param name="vstup">Text k převedení na velká písmena po oddělovači.</param>
        /// <param name="oddelovac">Znak, po kterém budou velká písmena ve vstupu.</param>
        /// <returns>Převedený text na velká písmena po oddělovači.</returns>
        private string VelkaPismena(string vstup, char oddelovac)
        {
            // vstup je krátký
            if (String.IsNullOrEmpty(vstup.Trim()))
            {
                return "";
            }

            // pokud je posledním indexu oddělovač, odstraním ho a na konci zase přidám
            bool oddelovacNaKonci = false;
            if (vstup.Last() == oddelovac)
            {
                interpret = interpret.TrimEnd(oddelovac);
                oddelovacNaKonci = true;
            }

            // rozdělí vstupní text pomocí oddělovače
            string[] oddelene = vstup.Split(oddelovac);
            vstup = "";
            for (int i = 0; i < oddelene.Length; i++)
            {
                if (!String.IsNullOrEmpty(oddelene[i]))
                {
                    // převede první znak na velké písmeno
                    //vstup += CultureInfo.CurrentCulture.TextInfo.ToTitleCase(oddelene[i][0].ToString()) + oddelene[i].Substring(1, oddelene[i].Length - 1);
                    vstup += oddelene[i][0].ToString().ToUpper() + oddelene[i].Substring(1, oddelene[i].Length - 1).ToLower();
                }
                if (oddelene.Length > 1)
                {
                    // nejedná se o poslední část nebo byl odstraněn oddělovač na začátku
                    if ((oddelene.Length - 1 != i) || oddelovacNaKonci)
                    {
                        // přidám oddělovač
                        vstup += oddelovac;
                    }
                }
            }
            // převedení velikosti názvů u interpretů
            vstup = vstup.Replace("..", ".")
                         .Replace("Dj", "DJ")
                         .Replace("A$ap", "A$AP")
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
            return vstup;
        }

        //ok přejmenuje interprety
        private void Prejmenuj()
        {
            string pridejNaKonec = "";
            string puvodniNazevUpraveny = "";

            Regex zavorka = new Regex(@"\(([^\}]+)\)"); // odstraní cokoliv v závorce ( );            
            Regex mezery = new Regex("\\s+"); // odstraní mezery

            // video neexistuje, bylo smazáno nebo zablokováno... -> konec
            if (String.IsNullOrWhiteSpace(kanal))
            {
                chyba = "Neexistující video";
                return;
            }
            // odstraní více mezer
            puvodniNazevUpraveny = mezery.Replace(nazevPuvodni, " ");
            // nahradí oddělovací znaky a feat
            puvodniNazevUpraveny = puvodniNazevUpraveny.ToLower()
                                                       .Replace('—', '-')
                                                       .Replace('–', '-')
                                                       .Replace("[", "(")
                                                       .Replace("]", ")")
                                                       .Replace("feat", "ft")
                                                       .Replace("featuring", "ft");
            // nahradí znak '|' za závorky
            if (puvodniNazevUpraveny.Contains("|"))
            {
                int pocet = puvodniNazevUpraveny.Split('|').Length - 1;
                // pokud jsou v názvu dva, nahradí první '(' a druhý ')'
                if (pocet == 2)
                {
                    Regex prvni = new Regex(Regex.Escape("|"));
                    puvodniNazevUpraveny = prvni.Replace(puvodniNazevUpraveny, "(", 1);
                    puvodniNazevUpraveny = prvni.Replace(puvodniNazevUpraveny, ")", 1);
                }
                // jinak se smaže
                else
                {
                    puvodniNazevUpraveny = puvodniNazevUpraveny.Replace("|", "");
                }
            }

            // úpravy dle kanálů
            if (kanal.ToLower() == "worldstarhiphop")
            {
                // za první "'" nahradí "-" a odstraní ostatní
                Regex uvozovky = new Regex(Regex.Escape("\""));
                puvodniNazevUpraveny = uvozovky.Replace(puvodniNazevUpraveny, "-", 1);
                puvodniNazevUpraveny = puvodniNazevUpraveny.Replace("\"", "")
                                                           .Replace(" -", "-");
            }
            else if (kanal.ToLower() == "amida dragon")
            {
                // nahradí "_" na "-"
                puvodniNazevUpraveny = puvodniNazevUpraveny.Replace("_", "-");
            }
            // odstraní pomlčky u interpretů s pomlčkou v názvu
            puvodniNazevUpraveny = puvodniNazevUpraveny.Replace("_", "")
                                       .Replace("gleb - zoo", "gleb")
                                       .Replace("mike will made-it", "mike will made it")
                                       .Replace("flo-rida", "flo rida")
                                       .Replace("jay-z", "jay z")
                                       .Replace("t-pain", "t pain")
                                       .Replace("t-wayne", "t wayne");

            // v názvu není pomlčka - video se nepodařilo přejmenovat -> konec
            if (!puvodniNazevUpraveny.Contains("-"))
            {
                chyba = "Nenalezen oddělovač";
                return;
            }
            // před pomlčkou je interpret
            interpret = puvodniNazevUpraveny.Substring(0, puvodniNazevUpraveny.IndexOf('-'))
                                            .Trim()
                                            .Replace("(ft", ",")
                                            .Replace(" x ", ",")
                                            .Replace(" ft", ",")
                                            .Replace(" & ", ",");
            // za pomlčkou je název skladby
            skladba = puvodniNazevUpraveny.Substring(puvodniNazevUpraveny.IndexOf('-') + 1)
                                          .Trim()
                                          .Replace("'", "")
                                          .Replace("( ft", " ft")
                                          .Replace("(ft", " ft");
            // získá featuringy z interpreta
            feat = interpret.Split(',').ToList();
            interpret = feat[0].Trim();
            feat.RemoveAt(0);

            // úprava interpreta - odstraní zbytečnosti a převede na velká písmena
            interpret = zavorka.Replace(interpret, "");
            interpret = OdstranZbytecnosti(interpret);
            interpret = ZmenaNazvuInterpreta(interpret);

            interpret = VelkaPismena(interpret, ' ');
            interpret = VelkaPismena(interpret, '.');

            // úprava skladby
            // skladba je remix -> přidám to co je v závorce do názvu
            if (skladba.Contains("remix"))
            {
                // nahradním otevírací závorku pomlčkou, zavírací odstraním
                skladba = skladba.Replace("(", "-")
                                 .Replace(")", "");
                string[] uprava = skladba.Split('-');
                if (uprava.Length > 1)
                {
                    // na konec přidám to co je mezi 1. a . pomlčkou
                    pridejNaKonec = " " + uprava[1].Trim();
                }
                skladba = uprava[0];
            }

            // odstranění textu v závorce
            skladba = zavorka.Replace(skladba, "");
            // existuje ukončovací závorka, odstraním vše co je za ní
            if (skladba.Contains(')'))
            {
                skladba = skladba.Substring(0, skladba.LastIndexOf(')'));
            }
            // odstranění zbytků závorek
            skladba = skladba.Replace("(", "")
                             .Replace(")", "");

            // v názvu skladby není " ft"
            if (!skladba.Contains(" ft"))
            {
                // nahradím " x " za " ft"
                skladba = skladba.Replace(" x ", " ft");
            }

            // v názvu skladby je " ft"
            if (skladba.Contains(" ft"))
            {
                // získám featuring ze skladby
                string[] uprava = skladba.Split(new[] { " ft" }, StringSplitOptions.None);
                skladba = uprava[0].Trim();
                uprava[1] = uprava[1].Trim()
                                     .Replace(" &", ",")
                                     .Replace(" x", ",");
                feat.AddRange(uprava[1].Split(','));
            }

            skladba = OdstranZbytecnosti(skladba);
            skladba = skladba.Trim() + pridejNaKonec;
            //skladba = OdstranZbytecnosti(skladba);
            skladba = OdstranZbytecnosti(skladba);

            // skladba je prázdná - soubor se nepodařilo přejmenovat -> konec
            if (String.IsNullOrEmpty(skladba))
            {
                chyba = "Nenalezen oddělovač";
                return;
            }

            skladba = skladba[0].ToString().ToUpper() + skladba.Substring(1, skladba.Length - 1).ToLower();
            
            skladbaFeaturing = skladba;

            /*if (feat.Count != 0)
            {
                if (feat[0] != "")
                {
                    skladbaFeaturing += " (ft. ";
                    if (feat.Count == 1)
                    {
                        featuring = feat[0].Replace(".", "").Trim();
                        featuring = zavorka.Replace(featuring, "");
                        featuring = featuring.Replace(")", "");
                        featuring = OdstranZbytecnosti(featuring);
                        featuring = ZmenaNazvuInterpreta(featuring);

                        featuring = VelkaPismena(featuring, ' ');
                        featuring = VelkaPismena(featuring, '.');

                        feat[0] = featuring;
                        skladbaFeaturing += featuring;
                    }
                    else if (feat.Count > 1)
                    {
                        featuring = "";
                        for (int i = 0; i < feat.Count; i++)
                        {
                            string ft = feat[i].Trim();
                            if (ft.Length == 0)
                            {
                                continue;
                            }
                            if ((ft[0] == '.') || (ft[0] == '/') || (ft[0] == '*') || (ft[0] == '-') || (ft[0] == '"'))
                            {
                                ft = ft.Substring(1, ft.Length - 1).Trim();
                            }
                            if (i == feat.Count - 1) // -1 = poslední, -2 přeposlední
                            {
                                ft = zavorka.Replace(ft, "").Replace(")", "");
                                ft = OdstranZbytecnosti(ft);
                            }

                            ft = ZmenaNazvuInterpreta(ft);


                            ft = VelkaPismena(ft, ' ');
                            ft = VelkaPismena(ft, '.');

                            skladbaFeaturing += ft; // null
                            featuring += ft;
                            feat[i] = ft;
                            if (i == feat.Count - 2) // -1 = poslední, -2 přeposlední
                            {
                                skladbaFeaturing += " & ";
                                featuring += " & ";
                            }
                            else if (i == feat.Count - 1) { }
                            else
                            {
                                skladbaFeaturing += ", ";
                                featuring += ", ";
                            }
                        }
                    }
                    skladbaFeaturing += ")";
                }
            }*/
            NajdiSlozku();
            /*OKOMENTOVÁNO JIŽ DŘÍVE:
            //NajdiSlozku(true);
            if (interpret == "PeatyF")
            {
                interpret = "PeatyF.";
                if (String.IsNullOrWhiteSpace(slozka))
                {
                    NajdiSlozku(false);                    
                }
            }
            else if (interpret == "M.I.A")
            {
                interpret = "M.I.A.";
                if (String.IsNullOrWhiteSpace(slozka))
                {
                    NajdiSlozku(false);
                }
            }
            else if (interpret == "B.o.b")
            {
                interpret = "B.o.B.";
                if (String.IsNullOrWhiteSpace(slozka))
                {
                    NajdiSlozku(false);
                }
            }*/
        }

        /**** NAJDI SLOŽKU ****/

        private void NajdiSlozku()
        {
            Soubory soubor = new Soubory();

            List<string> slozkyKnihovna = soubor.Precti(@"data/knihovna_slozky.txt");
            List<string> slozkyKnihovnaOstatni = new List<string>();
            //bool tecka = false;

            // slozkyKnihovna == null nebo slozkyKnihovna.Count == 0 -> chyba čtení souboru
            if (slozkyKnihovna == null)
            {
                chyba = "chyba čtení souboru";
                skupina = "Složka nemohla být nalezena";
                return;
            }
            if (slozkyKnihovna.Count == 0)
            {
                chyba = "chyba čtení souboru";
                skupina = "Složka nemohla být nalezena";
                return;
            }

            // není featuring v přidávaném videu
            if (feat.Count == 0)
            {
                string hledanyInterpret = OdstranZacatek(interpret);
                hledanyInterpret = OdstranKonec(hledanyInterpret);
                hledanyInterpret = hledanyInterpret.Trim().ToLower();
                /*if (interpret.Last() == '.')
                {
                    interpret = interpret.TrimEnd('.');
                    tecka = true;
                }*/
                foreach (string slozkaKnihovnaCesta in slozkyKnihovna)
                {
                    string slozkaKnihovnaNazev = Path.GetFileNameWithoutExtension(slozkaKnihovnaCesta);
                    // existuje složka s názvem interpreta
                    if (slozkaKnihovnaNazev.Trim().ToLower() == hledanyInterpret)
                    {
                        // najdi soubor, zdali už neexistuje
                        string nalezenySoubor = NajdiSoubor(slozkaKnihovnaCesta);
                        // video nebylo nalezeno -> v pořádku, můžeme přidat
                        if (String.IsNullOrWhiteSpace(nalezenySoubor))
                        {
                            slozka = slozkaKnihovnaCesta;
                            nazevNovy = skladbaHledana;
                            if (String.IsNullOrWhiteSpace(skupina))
                            {
                                skupina = "Složka nalezena";
                            }
                        }
                        // video již existuje -> nepřidám ho
                        else
                        {
                            slozka = nalezenySoubor;
                            if (String.IsNullOrWhiteSpace(skupina))
                            {
                                skupina = "Video bylo staženo dříve";
                            }
                        }
                        /*if (tecka)
                        {
                            interpret = interpret + '.';
                        }*/
                        return;
                    }
                    // najdi soubor ve složce _ostatní a zjisti zdali tam již není
                    else if (slozkaKnihovnaNazev.Trim().ToLower() == "_ostatní")
                    {
                        slozkyKnihovnaOstatni.Add(slozkaKnihovnaCesta);
                    }
                }

                /*if (tecka)
                {
                    interpret = interpret + '.';
                    tecka = false;
                }*/
                // najdi soubor ve složce ostatní_
                foreach (string slozkaKnihovnaOstatniCesta in slozkyKnihovnaOstatni)
                {
                    string nalezenySoubor = NajdiSouborOstatni(slozkaKnihovnaOstatniCesta);
                    // video nebylo nalezeno -> v pořádku, můžeme přidat
                    if (String.IsNullOrWhiteSpace(nalezenySoubor)) // nenalezen interpret
                    {
                        nazevNovy = interpret + "-" + skladbaHledana;
                        skupina = "Složka nemohla být nalezena";
                    }
                    else // nalezen interpret ve složce -> víme složku
                    {
                        slozka = nalezenySoubor;
                        return;
                    }
                }
            }
            // je featuring ve videu
            else
            {
                List<string> slozkyInterpreta = new List<string>();
                List<string> slozkyInterpretaOK = new List<string>();

                string hledanyInterpret = interpret.ToLower();
                hledanyInterpret = OdstranZacatek(hledanyInterpret);
                hledanyInterpret = OdstranKonec(hledanyInterpret);
                hledanyInterpret = OdstranZbytek(hledanyInterpret);

                /*if (interpret.Last() == '.')
                {
                    interpret = interpret.TrimEnd('.');
                    tecka = true;
                }*/

                // najde složky které obsahují interpreta
                foreach (string slozkaKnihovnaCesta in slozkyKnihovna)
                {
                    string slozkaKnihovnaNazev = Path.GetFileNameWithoutExtension(slozkaKnihovnaCesta).ToLower();
                    string slozkaKnihovnaPrvniInterpret = slozkaKnihovnaNazev.Split(new[] { " & ", ", " }, StringSplitOptions.None).First();

                    // existuje složka s názvem interpreta
                    if (hledanyInterpret == slozkaKnihovnaPrvniInterpret.Trim())
                    {
                        slozkyInterpreta.Add(slozkaKnihovnaCesta);
                    }
                }

                // nenalezena složka s prvním interpretem -> najdi ji v ostatní !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
                if (slozkyInterpreta == null)
                {
                    skupina = "Složka nemohla být nalezena";
                    slozka = "nenalezena, zkus _ostatní";
                    return;
                }
                if (slozkyInterpreta.Count == 0)
                {
                    skupina = "Složka nemohla být nalezena";
                    slozka = "nenalezena, zkus _ostatní";
                    return;
                }

                string slozkaAsi = "";

                // zkontroluje zdali již nalezené složky obsahují i další interprety
                foreach (string slozkaInterpretaCesta in slozkyInterpreta)
                {
                    string slozkaInterpretaNazev = Path.GetFileNameWithoutExtension(slozkaInterpretaCesta).ToLower();
                    string[] slozkaInterpretaInterpreti = slozkaInterpretaNazev.Split(new[] { " & ", ", " }, StringSplitOptions.None);
                    // slozkaInterpretaInterpreti -> může být víc interpretů než je na featuringu u videa, které chceme přidat -> pak tuhle složku nemůžeme zvolit
                    // feat -> může zde být více interpretů než máme v názvu složky -> v pořádku, odstraníme jen ty interprety, kteří mají složku
                    bool pridat = false;
                    for (int i = 1; i < slozkaInterpretaInterpreti.Length; i++)
                    {
                        foreach (string interpretFeaturing in feat)
                        {
                            if (slozkaInterpretaInterpreti[i] == interpretFeaturing.ToLower())
                            {
                                pridat = true;
                            }
                            else
                            {
                                // tato složka to není
                                pridat = false;
                                i = slozkaInterpretaInterpreti.Length;
                                slozkaAsi = slozkaInterpretaInterpreti[0];
                                break;
                            }
                        }
                    }
                    if (pridat)
                    {
                        slozkyInterpretaOK.Add(slozkaInterpretaCesta);
                    }
                }

                if (slozkyInterpretaOK == null)
                {
                    //skupina = "Složka nalezena";
                    slozka = slozkaAsi;
                    return;
                }
                if (slozkyInterpretaOK.Count == 0)
                {
                    //skupina = "Složka nalezena";
                    slozka = slozkaAsi;
                    return;
                }
                slozka = slozkyInterpretaOK[0];
            }
        }

        private string OdstranZacatek(string vstup)
        {
            // 48 - 57 = 0 - 9
            // 65 - 90 = A - Z (ale tolower() takže nepořebujeme)
            // 97 - 122 = a - z
            //if (!((vstup.First() >= 48 && vstup.First() <= 57) || (vstup.First() >= 97 && vstup.First() <= 122)))

            if ((vstup.First() >= 32 && vstup.First() <= 47) || (vstup.First() >= 58 && vstup.First() <= 63) || (vstup.First() >= 91 && vstup.First() <= 96) || (vstup.First() >= 123 && vstup.First() <= 126))
            {
                vstup = vstup.Substring(1);
                OdstranZacatek(vstup);
            }
            return vstup;
        }

        private string OdstranKonec(string vstup)
        {
            //if (!((vstup.Last() >= 48 && vstup.Last() <= 57) || (vstup.Last() >= 97 && vstup.Last() <= 122)))
            if ((vstup.Last() >= 32 && vstup.Last() <= 47) || (vstup.Last() >= 58 && vstup.Last() <= 63) || (vstup.Last() >= 91 && vstup.Last() <= 96) || (vstup.Last() >= 123 && vstup.Last() <= 126))
            {
                vstup = vstup.Substring(0, vstup.Length - 1);
                OdstranKonec(vstup);
            }
            return vstup;
        }

        private string OdstranMezery(string vstup)
        {
            if (vstup.Contains("  "))
            {
                vstup = vstup.Replace("  ", " ");
                OdstranMezery(vstup);
            }
            
            return vstup;
        }
        private string OdstranZbytek(string vstup)
        {
            foreach (char znak in vstup)
            {
                if ((znak >= 32 && znak <= 47) || (znak >= 58 && znak <= 63) || (znak >= 91 && znak <= 96) || (znak >= 123 && znak <= 126))
                {
                    vstup.Replace(znak, ' ');
                }
            }
            OdstranMezery(vstup);
            return vstup;
        }


        // najdi soubor ve složce (vrací soubor, který existuje NEBO null pokud soubor neexistuje)
        private string NajdiSoubor(string slozkaHledana)
        {
            Regex zavorka = new Regex(@"\(([^\}]+)\)"); // odstraní cokoliv v závorce ( );  
            // ÚPRAVA - přidat další koncovky !!!!!!!
            try
            {
                foreach (string cestaSoubor in Directory.GetFiles(slozkaHledana, "*.*", SearchOption.AllDirectories).Where(s => "*.mp3".Contains(Path.GetExtension(s).ToLower())))
                {
                    string kontolovanySoubor = Path.GetFileNameWithoutExtension(cestaSoubor);
                    kontolovanySoubor = kontolovanySoubor.ToLower();

                    if (!kontolovanySoubor.Contains("(ft."))
                    {
                        kontolovanySoubor = zavorka.Replace(kontolovanySoubor, ""); // odstraní i featuring
                        kontolovanySoubor = OdstranZacatek(kontolovanySoubor);
                        kontolovanySoubor = OdstranKonec(kontolovanySoubor);
                        kontolovanySoubor = OdstranZbytek(kontolovanySoubor);
                    }

                    string hledanaSkladba = skladbaHledana.ToLower();

                    if (!hledanaSkladba.Contains("(ft."))
                    {
                        hledanaSkladba = zavorka.Replace(hledanaSkladba, ""); // odstraní i featuring
                        hledanaSkladba = OdstranZacatek(hledanaSkladba);
                        hledanaSkladba = OdstranKonec(hledanaSkladba);
                        hledanaSkladba = OdstranZbytek(hledanaSkladba);
                    }

                    if (kontolovanySoubor == hledanaSkladba)
                    {
                        return cestaSoubor;
                    }
                    else if (Regex.IsMatch(kontolovanySoubor.Split(' ').First(), @"^\d{2}$")) // album -> rok název (první 4 znaky jsou číslice)
                    {
                        if (kontolovanySoubor.Substring(2, kontolovanySoubor.Length - 2).Trim() == hledanaSkladba)
                        {
                            return cestaSoubor;
                        }
                    }
                }
            }
            catch (Exception)
            {
                return "Chyba - výběr složky";
            }

            // soubor předtím nenalezen -> nyní zkusím odstranit i featuring a najít shodné názvy
            try
            {
                foreach (string cestaSoubor in Directory.GetFiles(slozkaHledana, "*.*", SearchOption.AllDirectories).Where(s => "*.mp3".Contains(Path.GetExtension(s).ToLower())))
                {
                    string kontolovanySoubor = Path.GetFileNameWithoutExtension(cestaSoubor);
                    kontolovanySoubor = kontolovanySoubor.ToLower();
                    kontolovanySoubor = zavorka.Replace(kontolovanySoubor, ""); // odstraní i featuring
                    kontolovanySoubor = OdstranZacatek(kontolovanySoubor);
                    kontolovanySoubor = OdstranKonec(kontolovanySoubor);
                    kontolovanySoubor = OdstranZbytek(kontolovanySoubor);

                    string hledanaSkladba = skladbaHledana.ToLower();
                    hledanaSkladba = zavorka.Replace(hledanaSkladba, ""); // odstraní i featuring
                    hledanaSkladba = OdstranZacatek(hledanaSkladba);
                    hledanaSkladba = OdstranKonec(hledanaSkladba);
                    hledanaSkladba = OdstranZbytek(hledanaSkladba);

                    if (kontolovanySoubor == hledanaSkladba)
                    {
                        skupina = "Následující videa jsou již možná stažena";
                        return cestaSoubor;
                    }
                    else if (Regex.IsMatch(kontolovanySoubor.Split(' ').First(), @"^\d{2}$")) // album -> rok název (první 4 znaky jsou číslice)
                    {
                        if (kontolovanySoubor.Substring(2, kontolovanySoubor.Length - 2).Trim() == hledanaSkladba)
                        {
                            skupina = "Následující videa jsou již možná stažena";
                            return cestaSoubor;
                        }
                    }
                }
            }
            catch (Exception)
            {
                return "Chyba - výběr složky";
            }
            return null;
        }

        // najdi soubor ve složce _ostatní
        private string NajdiSouborOstatni(string slozkaHledana)
        {
            string spravnaSlozka = null;
            // ÚPRAVA - přidat další koncovky !!!!!!!
            try
            {
                foreach (string cestaSoubor in Directory.GetFiles(slozkaHledana, "*.*", SearchOption.TopDirectoryOnly).Where(s => "*.mp3".Contains(Path.GetExtension(s).ToLower())))
                {
                    string nazevSoubor = Path.GetFileNameWithoutExtension(cestaSoubor);
                    nazevSoubor = nazevSoubor.ToLower();

                    string[] rozdelenyNazev = nazevSoubor.Split('-');
                    if (rozdelenyNazev == null)
                    {
                        return "chyba - není pomlčka";
                    }
                    if (rozdelenyNazev.Length < 2)
                    {
                        return "chyba - není pomlčka";
                    }
                    // video nalezeno
                    if (nazevSoubor == (interpret.ToLower() + "-" + skladbaHledana.ToLower()))
                    {
                        skupina = "Video bylo staženo dříve";
                        nazevNovy = "";
                        return cestaSoubor;
                    }
                    // video nenalezeno -> ale nalezen interpret ve složce -> správná složka
                    if (rozdelenyNazev[0] == interpret.ToLower())
                    {
                        spravnaSlozka = slozkaHledana;
                    }
                }
            }
            catch (Exception)
            {
                return "Chyba - výběr složky";
            }
            if (!String.IsNullOrWhiteSpace(spravnaSlozka))
            {
                nazevNovy = interpret + "-" + skladbaHledana;
                skupina = "Složka nalezena";
                return spravnaSlozka;
            }
            return null;
        }

        /*                 
            if (kontolovanySoubor.Contains("(ft."))
            {
                string feat1 = kontolovanySoubor.Split(new[] { "(ft." }, StringSplitOptions.None).Last();
                string[] feat2 = feat1.Split(new[] { " & ", ", " }, StringSplitOptions.None);
                for (int i = 0; i < feat2.Length; i++)
                {
                    feat2[i] = OdstranZacatek(feat2[i]);
                    feat2[i] = OdstranKonec(feat2[i]);
                }
            }
         */
        /********************************************************************************************************************************************************************************/
        private void NajdiSlozku(bool posledni)
        {
            Soubory soubor = new Soubory();
            if (posledni)
            {
                interpret = interpret.TrimEnd('.');
            }
            List<string> slozkyKnihovna = soubor.Precti(@"data/knihovna_slozky.txt");
            List<string> interpretSlozky = new List<string>();
            List<string[]> interpretSlozkyNejlepsi = new List<string[]>();
            List<string> odstranInterprety = new List<string>();

            // slozkyKnihovna == null nebo slozkyKnihovna.Count == 0 -> chyba čtení souboru
            if (slozkyKnihovna == null)
            {
                chyba = "chyba čtení souboru";
                skupina = "Složka nemohla být nalezena";
                return;
            }
            if (slozkyKnihovna.Count == 0)
            {
                chyba = "chyba čtení souboru";
                skupina = "Složka nemohla být nalezena";
                return;
            }

            // v souboru 'knihovna_slozky.txt' ( = slozkyKnihovna) projde řádky ( = slozkaKnihovnaCesta)
            foreach (string slozkaKnihovnaCesta in slozkyKnihovna)
            {
                // rozdělí název složky na interprety
                string[] interpretiSlozkaKnihovna = Path.GetFileNameWithoutExtension(slozkaKnihovnaCesta).Split(new[] { " & ", ", " }, StringSplitOptions.None);

                // projde interprety ze složky
                for (int i = 0; i < interpretiSlozkaKnihovna.Length; i++)
                {
                    // interpret hledaný (z youtube) je v názvu složky
                    if (interpretiSlozkaKnihovna[i].Trim().ToLower() == interpret.ToLower())
                    {
                        string nalezenySoubor = NajdiSoubor(skladbaFeaturing, slozkaKnihovnaCesta);
                        if (nalezenySoubor == "Chyba - výběr složky")
                        {
                            slozka = nalezenySoubor;
                            skupina = "Složka nemohla být nalezena";
                            return;
                        }
                        // soubor již existuje
                        if (nalezenySoubor != null)
                        {
                            slozka = nalezenySoubor;
                            skupina = "Video bylo staženo dříve";
                            return;
                        }

                        // není featuring ve videu, stejný soubor zatím nenalezen, složka nalezena
                        if (feat == null)
                        {
                            if (interpretiSlozkaKnihovna.Length == 1)
                            {
                                slozka = slozkaKnihovnaCesta;
                                interpret = Path.GetFileNameWithoutExtension(slozkaKnihovnaCesta);
                                nazevNovy = skladba.Replace('/', ' ').Replace(':', ' ');
                                skupina = "Složka nalezena";
                                return;
                            }
                            continue;
                        }
                        if (feat.Count == 0)
                        {
                            if (interpretiSlozkaKnihovna.Length == 1)
                            {
                                slozka = slozkaKnihovnaCesta;
                                interpret = Path.GetFileNameWithoutExtension(slozkaKnihovnaCesta);
                                nazevNovy = skladba.Replace('/', ' ').Replace(':', ' ');
                                skupina = "Složka nalezena";
                                return;
                            }
                            continue;
                        }

                        // je featuring ve videu
                        int pocet = 0;
                        for (int j = 0; j < feat.Count; j++)
                        {
                            for (int k = 1; k < interpretiSlozkaKnihovna.Length; k++)
                            {
                                if (feat[j].Trim().ToLower() == interpretiSlozkaKnihovna[k].Trim().ToLower())
                                {
                                    pocet++;
                                }
                            }
                        }
                        if (pocet >= interpretiSlozkaKnihovna.Length - 1)
                        {
                            interpretSlozkyNejlepsi.Add(new string[] { pocet.ToString(), slozkaKnihovnaCesta });
                        }
                        interpretSlozky.Add(slozkaKnihovnaCesta); //
                        i = interpretiSlozkaKnihovna.Length;
                    }
                }
            }
            interpretSlozkyNejlepsi.Sort((s, t) => String.Compare(s[0], t[0]));

            if (interpretSlozkyNejlepsi == null) { }
            else if (interpretSlozkyNejlepsi.Count == 0) { }
            else
            {
                odstranInterprety.AddRange(Path.GetFileNameWithoutExtension(interpretSlozkyNejlepsi.Last()[1]).Split(new[] { " & ", ", " }, StringSplitOptions.None));
                foreach (string interpretOdstran in odstranInterprety)
                {
                    for (int i = 0; i < feat.Count; i++)
                    {
                        if (interpretOdstran.Trim().ToLower() == feat[i].Trim().ToLower())
                        {
                            feat.RemoveAt(i);
                        }
                    }
                }
                string hledanaSkladba = skladba;

                if (feat == null)
                {
                    hledanaSkladba = skladba;
                }
                else if (feat.Count == 0)
                {
                    hledanaSkladba = skladba;
                }
                else
                {
                    hledanaSkladba += " (ft. ";
                    if (feat.Count == 1)
                    {
                        hledanaSkladba += feat.First();
                    }
                    else if (feat.Count > 1)
                    {
                        for (int i = 0; i < feat.Count; i++)
                        {
                            hledanaSkladba += feat[i];
                            if (i == feat.Count - 2) // -1 = poslední, -2 přeposlední
                            {
                                hledanaSkladba += " & ";
                            }
                            else if (i == feat.Count - 1) { }
                            else
                            {
                                hledanaSkladba += ", ";
                            }
                        }
                    }
                    hledanaSkladba += ")";
                }
                string nalezenySoubor = NajdiSoubor(hledanaSkladba, interpretSlozkyNejlepsi.Last()[1]);
                if (nalezenySoubor == "Chyba - výběr složky")
                {
                    slozka = nalezenySoubor;
                    skupina = "Složka nemohla být nalezena";
                    return;
                }
                if (nalezenySoubor != null)
                {
                    slozka = nalezenySoubor;
                    string[] rozdel = slozka.Split(new[] { "\\" }, StringSplitOptions.None);
                    interpret = rozdel[rozdel.Length - 3];
                    skupina = "Video bylo staženo dříve";
                    if (feat.Count == 0)
                    {
                        skladbaFeaturing = skladba;
                        //featuring = "";
                    }
                    return;
                }
                slozka = interpretSlozkyNejlepsi.Last()[1];
                interpret = Path.GetFileNameWithoutExtension(interpretSlozkyNejlepsi.Last()[1]);
                nazevNovy = hledanaSkladba.Replace('/', ' ').Replace(':', ' ');
                skupina = "Složka nalezena";
                /*featuring = "";
                for (int i = 0; i < feat.Count; i++)
                {
                    featuring += feat[i];
                    if (i == feat.Count - 2)
                    {
                        featuring += " & ";
                    }
                    else if (i == feat.Count - 1)
                    {

                    }
                    else
                    {
                        featuring += ", ";
                    }
                }*/
                return;
            }

            // složka nenalezena -> ostatní, nebo nevím ani ostatní
            slozka = NajdiSlozkuOstatni(slozkyKnihovna);
            nazevNovy = interpret + "-" + skladbaFeaturing;
            nazevNovy = nazevNovy.Replace('/', ' ').Replace(':', ' ');
            if (slozka == "" || slozka == null)
            {
                skupina = "Složka nemohla být nalezena";
            }
            else
            {
                skupina = "Složka nalezena";
            }
        }

        private string NajdiSlozkuOstatni(List<string> slozkySoubor)
        {
            List<string> slozkyOstatni = new List<string>();
            foreach (string slozkaCesta in slozkySoubor)
            {
                string slozkaNazev = Path.GetFileNameWithoutExtension(slozkaCesta);
                if (slozkaNazev.ToLower() == "_ostatní")
                {
                    slozkyOstatni.Add(slozkaCesta);
                    string nalezenySoubor = NajdiSouborOstatni(interpret, skladbaFeaturing, slozkaCesta);
                    if (nalezenySoubor != null)
                    {
                        return nalezenySoubor;
                        // soubor již existuje -> vrátí složku
                        // soubor zde neexistuje -> najdi zdali je zde interpret -> pokud ano, vrátí složku
                    }
                }
            }
            return null;
        }

        private string NajdiSoubor(string skladba, string slozka)
        {
            // úprava - přidat další koncovky !!!!!!!
            try
            {
                foreach (string cestaSoubor in Directory.GetFiles(slozka, "*.*", SearchOption.AllDirectories).Where(s => "*.mp3".Contains(Path.GetExtension(s).ToLower())))
                {
                    string nazevSoubor = Path.GetFileNameWithoutExtension(cestaSoubor);

                    if (nazevSoubor.ToLower() == skladba.ToLower())
                    {
                        return cestaSoubor;
                    }
                    else if (Regex.IsMatch(nazevSoubor.Split(' ').First(), @"^\d{2}$")) // album -> rok název (první 4 znaky jsou číslice)
                    {
                        if (nazevSoubor.Substring(2, nazevSoubor.Length - 2).Trim().ToLower() == skladba.ToLower())
                        {
                            return cestaSoubor;
                        }
                    }
                }
            }
            catch (Exception)
            {
                return "Chyba - výběr složky";
            }
            return null;
        }

        private string NajdiSouborOstatni(string interpret, string skladba, string slozka)
        {
            // úprava - přidat další koncovky !!!!!!!
            try
            {
                foreach (string cestaSoubor in Directory.GetFiles(slozka, "*.*", SearchOption.TopDirectoryOnly).Where(s => "*.mp3".Contains(Path.GetExtension(s).ToLower())))
                {
                    string nazevSoubor = Path.GetFileNameWithoutExtension(cestaSoubor);
                    string[] rozdelenyNazev = nazevSoubor.Split('-');
                    if (rozdelenyNazev == null)
                    {
                        return "chyba není pomlčka";
                    }
                    if (rozdelenyNazev.Length < 2) //???
                    {
                        return "chyba není pomlčka";
                    }
                    if (nazevSoubor.ToLower() == (interpret.ToLower() + "-" + skladba.ToLower()))
                    {
                        return cestaSoubor;
                    }
                    else if (rozdelenyNazev[0].ToLower() == interpret.ToLower())
                    {
                        return slozka;
                    }
                }
            }
            catch (Exception)
            {
                return "Chyba - výběr složky";
            }
            return null;
        }

    }
}
