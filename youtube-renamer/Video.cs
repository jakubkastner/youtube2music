﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace youtube_renamer
{
    /// <summary>
    /// Jedno video z YouTube ke stažení.
    /// </summary>
    public class Video
    {
        private SeznamInterpretu vsichniInterpreti;
        private string novyNazevVidea;

        public string ID { get; set; }
        public string Playlist { get; set; }
        public string Popis { get; set; }
        public Kanal Kanal { get; set; }
        public DateTime Publikovano { get; set; }
        public string Zanr { get; set; }
        public List<Interpret> Interpreti { get; set; }
        public string Slozka { get; set; }
        public string Chyba { get; set; }
        public string Stav { get; set; }


        public string InterpretiFeat
        {
            get
            {
                // neexistující interpreti
                if (Interpreti == null)
                {
                    return "";
                }
                // pouze 1 interpret = není featuring
                if (Interpreti.Count == 1)
                {
                    return "";
                }
                // více než 1 interpret = existuje featuring
                if (Interpreti.Count > 1)
                {
                    List<string> inteprerti = new List<string>();
                    for (int i = 1; i < Interpreti.Count; i++)
                    {
                        inteprerti.Add(Interpreti[i].Jmeno);
                    }

                    // spojí pole interpretů na featu pomocí ", "
                    string vysledek = String.Join(", ", inteprerti);
                    // spojí poslední dva interprety na featuringu znakem " & "
                    string posledni = inteprerti.Last();
                    vysledek = vysledek.Replace(", " + posledni, " & " + posledni);
                    return vysledek;
                }
                return "";
            }
        }
        public string NazevNovy
        {
            get
            {
                return this.novyNazevVidea;
            }
            set
            {
                string novyNazev = value;
                // odstraní mezery z názvu
                novyNazev = OdstranVicenasobneMezery(novyNazev);
                if (String.IsNullOrEmpty(novyNazev))
                {
                    this.novyNazevVidea = novyNazev;
                    return;
                }

                // odstraní znaky souboru, které se nemohou použít v názvu souboru
                string novyNazevSouboru = String.Join("", novyNazev.Split(Path.GetInvalidFileNameChars()));

                //zkontroluje, zda-li soubor ve složce už neexistuje
                string nalezenySoubor = NajdiSoubor(novyNazevSouboru, Slozka);
                if (!String.IsNullOrEmpty(nalezenySoubor))
                {
                    // soubor již existuje
                    NazevNovySoubor = "";
                    novyNazev = "";
                    Slozka = nalezenySoubor;
                }
                NazevNovySoubor = novyNazevSouboru;
                this.novyNazevVidea = novyNazev;
            }
        }

        // nový název ukládaného souboru (odstraněny špatné znaky)
        public string NazevNovySoubor { get; set; }

        public string NazevPuvodni { get; set; }
        public string Skladba { get; set; }
        public string NazevSkladbaFeat
        {
            get
            {
                // neexistující interpreti
                if (Interpreti == null)
                {
                    return "";
                }
                // méně než 1 interpret = není interpret
                if (Interpreti.Count < 1)
                {
                    return "";
                }
                // existují interpreti na featuringu
                string vysledek = Skladba;
                string vysledekFeat = InterpretiFeat;
                if (!String.IsNullOrEmpty(vysledekFeat))
                {
                    vysledek += " (ft. " + vysledekFeat + ")";
                }
                return vysledek;
            }
        }
        public string NazevInterpretSkladbaFeat
        {
            get
            {
                return Interpret + "-" + NazevSkladbaFeat;
            }
        }
        public string Interpret
        {
            get
            {
                // neexistující interpreti
                if (Interpreti == null)
                {
                    return "";
                }
                // méně než 1 interpret = není interpret
                if (Interpreti.Count < 1)
                {
                    return "";
                }
                return Interpreti.First().Jmeno;
            }
        }

        public Video(string videoID, string videoPlaylist, SeznamInterpretu interpreti)
        {
            this.vsichniInterpreti = interpreti;
            this.novyNazevVidea = "";
            ID = videoID;
            Playlist = videoPlaylist;
            Popis = ""; // -> viz YouTubeApi.ZiskejInfoVidea()
            // kanál - id + název -> viz YouTubeApi.ZiskejInfoVidea()
            Publikovano = new DateTime(); // -> viz YouTubeApi.ZiskejInfoVidea()
            Zanr = "";
            Interpreti = new List<Interpret>();
            Slozka = "";
            Chyba = "";
            Stav = "";
            NazevPuvodni = ""; // -> viz YouTubeApi.ZiskejInfoVidea()
            Skladba = ""; // -> viz ZiskejInformace()
            NazevNovySoubor = "";
            // získá informace z youtube api
            try
            {
                YouTubeApi.ZiskejInfoVidea(this);
                // -> získá původní název, kanál, popis, datum publikování
            }
            catch (Exception)
            {
                Chyba = "Chyba přidávání";
                return;
            }
            if (!String.IsNullOrEmpty(Chyba))
            {
                // video neexistuje, nebo nastala chyba přidávání videa
                return;
            }

            // získá název skladby a interprety
            ZiskejInformace();
            // najde složku
            NajdiSlozku();
        }

        /// <summary>
        /// Získá název skladby a interprety z názvu videa.
        /// </summary>
        private void ZiskejInformace()
        {

            // neexistuje název videa
            if (String.IsNullOrEmpty(NazevPuvodni))
            {
                Chyba = "Neexistující název videa";
                return;
            }

            string interpret = "";
            string skladba = "";
            bool remix = false; // pokud je v názvu remix, přidám ho nakonec
            Regex zavorka = new Regex(@"\(([^\}]+)\)"); // odstraní cokoliv v závorce ( )

            // odstraní více mezer
            string upravenyNazev = OdstranVicenasobneMezery(NazevPuvodni);

            // nahradí oddělovací znaky a feat
            upravenyNazev = upravenyNazev.ToLower()
                                         .Replace('—', '-')
                                         .Replace('–', '-')
                                         .Replace('~', '-')
                                         .Replace('_', '-')
                                         .Replace("[", "(")
                                         .Replace("]", ")")
                                         .Replace("featuring", "ft") // musí být před
                                         .Replace("feat", "ft");

            // nahradí znak '|' za závorky
            if (upravenyNazev.Contains("|"))
            {
                int pocet = upravenyNazev.Split('|').Length - 1;
                // pokud jsou v názvu dva, nahradí první '(' a druhý ')'
                if (pocet == 2)
                {
                    Regex lomitko = new Regex(Regex.Escape("|"));
                    upravenyNazev = lomitko.Replace(upravenyNazev, "(", 1);
                    upravenyNazev = lomitko.Replace(upravenyNazev, ")", 1);
                }
                // jinak se smaže
                else
                {
                    upravenyNazev = upravenyNazev.Replace("|", "");
                }
            }

            // úprava dle kanálu
            if (String.Compare(Kanal.Nazev, "worldstarhiphop", true) == 0)
            {
                Regex uvozovky = new Regex(Regex.Escape("\""));
                upravenyNazev = uvozovky.Replace(upravenyNazev, "-", 1);
                upravenyNazev = upravenyNazev.Replace("\"", "")
                                             .Replace(" -", "-");
            }

            // odstraní pomlčky u interpretů s pomlčkou v názvu
            upravenyNazev = upravenyNazev.Replace("gleb - zoo", "gleb")
                                         .Replace("mike will made-it", "mike will made it")
                                         .Replace("g-eazy", "g eazy")
                                         .Replace("flo-rida", "flo rida")
                                         .Replace("jay-z", "jay z")
                                         .Replace("t-pain", "t pain")
                                         .Replace("t-wayne", "t wayne")
                                         .Replace("tyler, the creator", "tyler the creator");

            // v názvu není oddělovací znak (pomlčka)
            if (!upravenyNazev.Contains("-"))
            {
                // za první "'" nahradí "-" a odstraní ostatní
                Regex uvozovky = new Regex(Regex.Escape("\""));
                upravenyNazev = uvozovky.Replace(upravenyNazev, "-", 1);
                upravenyNazev = upravenyNazev.Replace("\"", "")
                                     .Replace(" -", "-");
            }

            // v názvu stále není oddělovací znak (pomlčka)
            if (!upravenyNazev.Contains("-"))
            {
                // uložím původná název jako skladbu
                interpret = "";
                skladba = NazevPuvodni;
                Chyba = "Nenalezen oddělovač v názvu";
            }
            // v názvu je oddělovač
            else
            {
                int oddelovac = upravenyNazev.IndexOf('-');
                // před oddělovačem je interpret
                interpret = upravenyNazev.Substring(0, oddelovac)
                                         .Trim()
                                         .Replace("(ft", ",")
                                         .Replace(" x ", ",")
                                         .Replace(" ft", ",")
                                         .Replace(" & ", ",")
                                         .Replace(" + ", ",");
                // za oddělovačem je název skladby
                skladba = upravenyNazev.Substring(oddelovac + 1);
                // získá featuringy z interpreta
                PridejInterpreta(new List<string>(interpret.Split(',')));
            }

            // úprava skladby
            skladba = skladba.Trim()
                             .Replace("'", "")
                             .Replace("( ft", " ft")
                             .Replace("(ft", " ft");

            // v názvu skladby je remix
            if (skladba.Contains("remix"))
            {
                // odstraním ho z názvu a ložím do proměnné remix, že se jedná o remix
                remix = true;
                skladba = skladba.Replace("remix", "")
                                 .Replace("()", "");
            }

            // odstranění textu v závorce
            skladba = zavorka.Replace(skladba, "");

            // existuje ukončovací závorka, odstraním vše co je za ní
            if (skladba.Contains(")"))
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
                                     .Replace(" x ", ",");
                PridejInterpreta(new List<string>(uprava[1].Split(',')));
            }

            // odstraní zbytečné fráze ze skladby
            skladba = OdstranZbytecnosti(skladba);

            // jedná se o remix
            if (remix)
            {
                // přidám uto informaci na konec názvu
                skladba = skladba + " remix";
            }

            // skladba je prázdná
            if (String.IsNullOrEmpty(skladba))
            {
                // soubor se nepodařilo přejmenovat -> konec
                Chyba = "Nenalezen oddělovač";
                return;
            }

            // odstranění mezer
            skladba = OdstranVicenasobneMezery(skladba);

            // převedení na velká písmenka na začátku a při nalezení " i " a uložení do proměnné videa
            Skladba = skladba[0].ToString().ToUpper() + skladba.Substring(1, skladba.Length - 1).ToLower().Replace(" i ", " I ");
        }

        /// <summary>
        /// Odstraní zbytečné fráze a znaky ze vstupu.
        /// </summary>
        /// <param name="vstup">Text k odstraní zbytečných frází a znaků.</param>
        /// <returns>Text s odstraněnými frázemi a znaky.</returns>
        private string OdstranZbytecnosti(string vstup)
        {
            if (String.IsNullOrEmpty(vstup))
            {
                return "";
            }
            // odstraní postupně všechen přebytečný text z pole
            string[] odstran = { "off.", "off vid", "off vd", "music video", "official", " vd", "audio", "prod", "wshh" };
            for (int i = 0; i < odstran.Length; i++)
            {
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
            // příliš krátký název
            if (posledniIndex < 0)
            {
                return vstup;
            }
            // poslední slovo obsahuje zbytečnost
            if ((vstup[posledniIndex] == '/') ||
                (vstup[posledniIndex] == '*') ||
                (vstup[posledniIndex] == '-') ||
                (vstup[posledniIndex] == '"') ||
                (vstup[posledniIndex] == '~') ||
                (vstup[posledniIndex] == '.'))
            {
                // odstraním ho
                vstup = vstup.Substring(0, posledniIndex).Trim();
                posledniIndex = vstup.Length - 1;
            }
            if (String.IsNullOrEmpty(vstup))
            {
                return "";
            }
            // první slovo obsahuje zbytečnost
            if ((vstup[0] == '/') ||
                (vstup[0] == '*') ||
                (vstup[0] == '-') ||
                (vstup[0] == '"') ||
                (vstup[0] == '~') ||
                (vstup[0] == '.'))
            {
                // odstraním ho
                vstup = vstup.Substring(1, posledniIndex).Trim();
            }
            vstup = vstup.Trim();
            return vstup;
        }

        /// <summary>
        /// Přidá zadého jména interpreta do seznamu interpetů.
        /// </summary>
        /// <param name="jmenoInterpreta">Jméno interpreta k přidání do seznamu interpretů.</param>
        private void PridejInterpreta(string jmenoInterpreta)
        {
            if (String.IsNullOrEmpty(jmenoInterpreta))
            {
                return;
            }
            // odstranění zbytečnosti
            jmenoInterpreta = OdstranZbytecnosti(jmenoInterpreta);
            // převedení na velká písmena
            Interpret novyInterpret = new Interpret(jmenoInterpreta);

            // zamezení duplicitním interpretům - interpret je již na featu
            if (JeInterpretNaSeznamu(novyInterpret.Jmeno))
            {
                // nepřidám ho
                return;
            }

            // zjistí, zdali interpret nebyl již používán u jiného videa
            Interpret staryInterpret = vsichniInterpreti.VratInterpreta(novyInterpret.Jmeno);
            // interpret již je používán u jiného videa
            if (staryInterpret != null)
            {
                // přidám do seznamu jeho instanci
                Interpreti.Add(staryInterpret);
            }
            // interpret nebyl dosud použit u jiného videa
            else
            {
                // přidám do seznamu novou instanci interpreta
                Interpreti.Add(novyInterpret);
                vsichniInterpreti.Interpreti.Add(novyInterpret);
            }
        }

        /// <summary>
        /// Přidá z pole jmen interpretů interprety do seznamu interpetů.
        /// </summary>
        /// <param name="jmenaInterpretu">Seznam jmen interpetů k přidání do seznamu.</param>
        private void PridejInterpreta(List<string> jmenaInterpretu)
        {
            foreach (string jmenoInterpreta in jmenaInterpretu)
            {
                PridejInterpreta(jmenoInterpreta);
            }
        }

        /// <summary>
        /// Odstraní interpeta ze seznamu interpretů.
        /// </summary>
        /// <param name="jmenoInterpreta">Jméno interpreta k odstranění ze seznamu.</param>
        private void OdstranInterpreta(string jmenoInterpreta)
        {
            // interpret je na featu
            if (JeInterpretNaSeznamu(jmenoInterpreta))
            {
                // získám instanci interpreta
                Interpret interpretKOdstraneni = vsichniInterpreti.VratInterpreta(jmenoInterpreta);
                // instance byla nalezena
                if (interpretKOdstraneni != null)
                {
                    // odstraní interpreta ze seznamu
                    Interpreti.Remove(interpretKOdstraneni);
                }
            }
        }

        /// <summary>
        /// Zjistí jestli je interpret na seznamu interpretů.
        /// </summary>
        /// <param name="hledanyInterpret">Jméno interpreta ke zjištění.</param>
        /// <returns>
        /// true = interpret byl nalezen
        /// false = interpret nebyl nalezen
        /// </returns>
        private bool JeInterpretNaSeznamu(string hledanyInterpret)
        {
            if (String.IsNullOrEmpty(hledanyInterpret))
            {
                return false;
            }
            // projde seznam interpretů
            foreach (Interpret interpret in Interpreti)
            {
                // jména se shodují
                if (interpret.Jmeno.ToLower().Equals(hledanyInterpret.ToLower()))
                {
                    // interpret je na seznamu
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// DODĚLAT KONTROLU A POPIS !!!!!
        /// Nalezne správnou složku skladby.
        /// </summary>
        private void NajdiSlozku()
        {
            if (Interpreti.Count < 1)
            {
                return;
            }
            
            // získá hledaného interpreta a jeho jméno
            Interpret hledanyInterpret = Interpreti.First();
            string hledanyInterpretJmeno = hledanyInterpret.Jmeno.Trim().ToLower();

            // získá složky interpreta
            if (hledanyInterpret.Slozky == null)
            {
                hledanyInterpret.Slozky = new List<string>();
                hledanyInterpret.NajdiSlozky();
            }
            if (hledanyInterpret.Slozky.Count == 0)
            {
                hledanyInterpret.NajdiSlozky();
            }
            if (hledanyInterpret.Slozky == null)
            {
                Chyba = "Složka nenalezena";
                return;
            }
            if (hledanyInterpret.Slozky.Count == 0)
            {
                Chyba = "Složka nenalezena";
                return;
            }

            bool jizStazeno = false;            
            string nejlepsiSlozka = ""; // složka, která obsahuje nejvíce interpretů

            // projde názvy složek ze získaných složek interpreta
            foreach (string slozkaInterpretaCesta in hledanyInterpret.Slozky)
            {
                string slozkaInterpretaNazev = Path.GetFileName(slozkaInterpretaCesta).ToLower().Trim();

                // zjistí jestli už soubor není stažený
                string nalezenySoubor = "";
                if (slozkaInterpretaNazev == "_ostatní")
                {
                    nalezenySoubor = NajdiSoubor(NazevInterpretSkladbaFeat, slozkaInterpretaCesta);
                }
                else
                {
                    nalezenySoubor = NajdiSoubor(NazevSkladbaFeat, slozkaInterpretaCesta);
                }
                if (!String.IsNullOrEmpty(nalezenySoubor))
                {
                    // soubor již existuje
                    Slozka = nalezenySoubor;
                    NazevNovy = "";

                    if (slozkaInterpretaNazev.Contains("_ostatní"))
                    {
                        return;
                    }
                    try
                    {
                        nejlepsiSlozka = Path.GetDirectoryName(nalezenySoubor);
                    }
                    catch (Exception)
                    {
                        // Chyba="popis chyby" ????
                        return;
                    }

                    if (Regex.IsMatch(Path.GetFileName(nejlepsiSlozka).Split(' ').First(), @"^\d{4}$")) // album -> rok název (první 4 znaky jsou číslice)
                    {
                        // složka je album ("2012 jhfjsd"), musím získat rodičovskou složku (interpreta)

                        // získá ze souboru složku a z té získá rodičovskou složku
                        nejlepsiSlozka = Directory.GetParent(nejlepsiSlozka).FullName;
                    }
                    // musím ještě případně odstranit společné interprety, proto ne "return"
                    jizStazeno = true;
                    break;
                }

                // je featuring v přidávaném videu
                if (Interpreti.Count > 1)
                {
                    // získaní interpreti z názvu složky
                    string[] interpretiNazevSlozky = slozkaInterpretaNazev.Split(new[] { " & ", ", " }, StringSplitOptions.None);
                    bool spravnaSlozka = false;
                    // prochází interprety z názvu složky
                    foreach (string interpretSlozka in interpretiNazevSlozky)
                    {
                        string interpretSlozkaNazev = interpretSlozka.Trim().ToLower();
                        // daný interpret je na featu
                        spravnaSlozka = JeInterpretNaSeznamu(interpretSlozkaNazev);
                        if (!spravnaSlozka)
                        {
                            // není interpret na featu
                            break;
                        }
                    }
                    if (spravnaSlozka)
                    {
                        // jedná se o složku, kde se nachází interpreti, kteří jsou na featu
                        if (String.IsNullOrEmpty(nejlepsiSlozka))
                        {
                            // ještě nebyla nalezena žádná složka
                            nejlepsiSlozka = slozkaInterpretaCesta;
                        }
                        else
                        {
                            string nejlepsiSlozkaNazev = Path.GetFileName(nejlepsiSlozka).ToLower().Trim();
                            int pocetInterpretuNejlepsiSlozky = nejlepsiSlozkaNazev.Split(new[] { " & ", ", " }, StringSplitOptions.None).Count();
                            if (interpretiNazevSlozky.Count() > pocetInterpretuNejlepsiSlozky)
                            {
                                // nová složka má více správných interpetů než ta dosud nalezená
                                nejlepsiSlozka = slozkaInterpretaCesta;
                            }
                        }
                    }
                }
                else if (slozkaInterpretaNazev == hledanyInterpretJmeno)
                {
                    // není featuring ve videu nebo nebyla nalezena hromadná složka interpretů

                    // existuje složka s názvem interpreta
                    Slozka = slozkaInterpretaCesta;
                    NazevNovy = NazevSkladbaFeat;
                    return;
                }
                if (String.IsNullOrEmpty(nejlepsiSlozka) && slozkaInterpretaNazev == "_ostatní")
                {
                    // najdi soubor ve složce _ostatní a zjisti zdali tam již není
                    Slozka = slozkaInterpretaCesta;
                    NazevNovy = NazevInterpretSkladbaFeat;
                    return;
                }
            }
            if (!String.IsNullOrEmpty(nejlepsiSlozka))
            {
                string nejlepsiSlozkaNazev = Path.GetFileName(nejlepsiSlozka).ToLower().Trim();
                string[] interpretiSlozky = nejlepsiSlozkaNazev.Split(new[] { " & ", ", " }, StringSplitOptions.None);
                // byla nalezena složka s více interprety
                if (interpretiSlozky.Length > 1)
                {
                    // jedná se o složku s více interprety

                    // změní jméno interpreta skladby
                    Interpreti[0] = new Interpret(nejlepsiSlozkaNazev);
                    // odstraní přebytečné featuringy
                    foreach (string interpretSlozka in interpretiSlozky)
                    {
                        OdstranInterpreta(interpretSlozka);
                    }
                }
                if (!jizStazeno)
                {
                    // nejedená se o již stažený soubor
                    Slozka = nejlepsiSlozka;
                    NazevNovy = NazevSkladbaFeat;
                }
                return;
            }
            if (!jizStazeno)
            {
                Chyba = "Složka nenalezena";
            }
        }

        /// <summary>
        /// Nalezne soubor skladby ve složce a vrátí ho. Pokud neexistuje vrací null.
        /// </summary>
        /// <param name="hledanaSkladba">Název skladby (souboru) k prohledání.</param>
        /// <param name="hledanaSlozka">Cesta složky k prohledání.</param>
        /// <returns>Soubor skladby ve složce. Pokud neexistuje vrací null.</returns>
        private string NajdiSoubor(string hledanaSkladba, string hledanaSlozka)
        {
            // odstranění neplatných znaků v názvu souboru
            hledanaSkladba = String.Join("", hledanaSkladba.Split(Path.GetInvalidFileNameChars()));
            // celá cesta hledané skladby
            string hledanaSkladbaCesta = Path.Combine(hledanaSlozka, hledanaSkladba + ".opus");

            // zkontroluje, zdali se ve složce nenachází stejný soubor
            if (File.Exists(hledanaSkladbaCesta))
            {
                // soubor již existuje
                Slozka = hledanaSkladbaCesta;
                Chyba = "Video bylo staženo dříve";
                return hledanaSkladbaCesta;
            }

            // zkontroluje i další složky
            Regex zavorka = new Regex(@"\(([^\}]+)\)"); // odstraní cokoliv v závorce ( ), včetně featuringu

            // úpravy hledané skladby
            hledanaSkladba = hledanaSkladba.ToLower();

            // nejedná se o skladbu s featuringem
            if (!hledanaSkladba.Contains("(ft."))
            {
                // mohu odstranit vše v závorce
                hledanaSkladba = zavorka.Replace(hledanaSkladba, "");
                hledanaSkladba = OdstranZnaky(hledanaSkladba, false);
            }

            // získá všechny soubory ze složky
            List<string> soubory = new List<string>();
            try
            {
                soubory.AddRange(Directory.GetFiles(hledanaSlozka, "*.*", SearchOption.AllDirectories).Where(s => "*.opus".Contains(Path.GetExtension(s).ToLower())));
            }
            catch (Exception)
            {
                Chyba = "Chyba hledání souboru";
                return null;
            }

            // projde získané soubory ze složky
            foreach (string souborCesta in soubory)
            {
                // název souboru
                string souborNazev = Path.GetFileNameWithoutExtension(souborCesta).ToLower();
                // nejedná se o skladbu s featuringem
                if (!souborNazev.Contains("(ft."))
                {
                    // mohu odstranit vše v závorce
                    souborNazev = OdstranZnaky(souborNazev, false);
                }

                // názvy se shodují
                if (String.Compare(souborNazev, hledanaSkladba, true) == 0)
                {
                    // video bylo již staženo dříve
                    Chyba = "Video bylo staženo dříve";
                    return souborCesta;
                }
                // složka není ostatní a první 2 znaky jsou čísla a poté následuje mezera ("00 text") - nejspíše se jedná o skladby ve složce alba
                else if (!hledanaSlozka.Contains("_ostatní") && Regex.IsMatch(souborNazev.Split(' ').First(), @"^\d{2}$"))
                {
                    // názvy se shodují
                    if (souborNazev.Substring(2, souborNazev.Length - 2).Trim() == hledanaSkladba)
                    {
                        // video bylo již staženo dříve
                        Chyba = "Video bylo staženo dříve";
                        return souborCesta;
                    }
                }
            }

            // soubor nebyl doposud nalezen
            // odstraní z názvu ostatní znaky, featuring a mezery
            hledanaSkladba = zavorka.Replace(hledanaSkladba, "");
            hledanaSkladba = OdstranZnaky(hledanaSkladba, true);
            hledanaSkladba = OdstranVsechnyMezery(hledanaSkladba);

            // projde získané soubory ze složky
            foreach (string souborCesta in soubory)
            {
                // získá název souboru ze složky a odstraní ostatní znaky, featuring a mezery
                string souborNazev = Path.GetFileNameWithoutExtension(souborCesta).ToLower();
                souborNazev = zavorka.Replace(souborNazev, "");
                souborNazev = OdstranZnaky(souborNazev, true);
                // pro kontrolu souborů albumů s názvem "00 text" je nutné uložit název bez mezer
                string souborNazevSMezerami = souborNazev;
                // a až poté odstranit mezery
                souborNazev = OdstranVsechnyMezery(souborNazev);

                // názvy se shodují
                if (String.Compare(souborNazev, hledanaSkladba, true) == 0)
                {
                    // video bylo s největší pravděpodobností staženo dříve
                    Chyba = "Video bylo nejspíš staženo dříve";
                    return souborCesta;
                }
                // složka není ostatní a první 2 znaky jsou čísla a poté následuje mezera ("00 text") - nejspíše se jedná o skladby ve složce alba
                else if (!hledanaSlozka.Contains("_ostatní") && Regex.IsMatch(souborNazevSMezerami.Split(' ').First(), @"^\d{2}$"))
                {
                    // názvy se shodují
                    if (souborNazev.Substring(2, souborNazev.Length - 2).Trim() == hledanaSkladba)
                    {
                        // video bylo s největší pravděpodobností staženo dříve
                        Chyba = "Video bylo nejspíš staženo dříve";
                        return souborCesta;
                    }
                }
            }
            // soubor nebyl nalezen
            return null;
        }

        /// <summary>
        /// Odstraní všechny mezery z textu.
        /// </summary>
        /// <param name="vstup">Text k odstranění mezer.</param>
        /// <returns>Text s odstraněnými mezerami.</returns>
        private string OdstranVsechnyMezery(string vstup)
        {
            if (string.IsNullOrEmpty(vstup))
            {
                return vstup;
            }
            while (vstup.Contains(" "))
            {
                vstup = vstup.Replace(" ", "");
            }
            return vstup;
        }
        
        /// <summary>
        /// Nahradí z textu více mezer za sebou jednou mezerou.
        /// </summary>
        /// <param name="vstup">Text k nahrazení více mezer.</param>
        /// <returns>Text s maximálně jednou mezerou za sebou.</returns>
        private string OdstranVicenasobneMezery(string vstup)
        {
            if (string.IsNullOrEmpty(vstup))
            {
                return vstup;
            }
            Regex mezery = new Regex("\\s+"); // odstraní více mezer za sebou
            vstup = mezery.Replace(vstup, " ");
            return vstup;
        }

        /// <summary>
        /// Odstraní znaky z textu, ty nahradí mezerou.
        /// </summary>
        /// <param name="vstup">Text k odstranění znaků.</param>
        /// <param name="pomlcka">Zda-li se má odstranit i pomlčka (true = ano, false = ne)</param>
        /// <returns>Text s odstraněnými znaky.</returns>
        private string OdstranZnaky(string vstup, bool pomlcka)
        {
            if (string.IsNullOrEmpty(vstup))
            {
                return vstup;
            }
            foreach (char znak in vstup)
            {
                // odstraní znaky z textu
                if ((znak >= 32 && znak <= 44) || (znak >= 46 && znak <= 47) || (znak >= 58 && znak <= 63) || (znak >= 91 && znak <= 96) || (znak >= 123 && znak <= 126))
                {
                    vstup = vstup.Replace(znak, ' ');
                }
                // jedná se o pomlčku a je povoleno její odstranění
                if (pomlcka && znak == 45)
                {
                    vstup = vstup.Replace(znak, ' ');
                }
            }
            vstup = vstup.Trim();
            // zamezí více mezerám za sebou
            vstup = OdstranVicenasobneMezery(vstup);
            return vstup;
        }
    }
}
