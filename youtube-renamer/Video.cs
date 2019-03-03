using System;
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
                return novyNazevVidea;
            }
            set
            {
                string novyNazev = value;
                if (String.IsNullOrEmpty(novyNazev))
                {
                    this.novyNazevVidea = novyNazev;
                    return;
                }
                string nalezenySoubor = NajdiSoubor(novyNazev);
                if (!String.IsNullOrEmpty(nalezenySoubor))
                {
                    // soubor již existuje
                    novyNazev = "";
                    Slozka = nalezenySoubor;
                }
                this.novyNazevVidea = novyNazev;
            }
        }
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
            bool remix = false;

            // neexistuje název videa
            if (String.IsNullOrEmpty(NazevPuvodni))
            {
                Chyba = "Neexistující název videa";
                return;
            }

            Regex mezery = new Regex("\\s+"); // odstraní mezery
            Regex zavorka = new Regex(@"\(([^\}]+)\)"); // odstraní cokoliv v závorce ( )

            string interpret = "";
            string skladba = "";
            //string pridejNaKonec = "";
            string upravenyNazev = mezery.Replace(NazevPuvodni, " "); // odstraní více mezer

            // nahradí oddělovací znaky a feat
            upravenyNazev = upravenyNazev.ToLower()
                                         .Replace('—', '-')
                                         .Replace('–', '-')
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

            // úprava názvu dle kanálů
            switch (Kanal.Nazev.ToLower())
            {
                case "worldstarhiphop":
                    Regex uvozovky = new Regex(Regex.Escape("\""));
                    upravenyNazev = uvozovky.Replace(upravenyNazev, "-", 1);
                    upravenyNazev = upravenyNazev.Replace("\"", "")
                                         .Replace(" -", "-");
                    break;
                case "amida dragon":
                    // nahradí "_" na "-"
                    upravenyNazev = upravenyNazev.Replace("_", "-");
                    break;
                default:
                    break;
            }

            // odstraní pomlčky u interpretů s pomlčkou v názvu
            upravenyNazev = upravenyNazev.Replace("_", "")
                                         .Replace("gleb - zoo", "gleb")
                                         .Replace("mike will made-it", "mike will made it")
                                         .Replace("g-eazy", "g eazy")
                                         .Replace("flo-rida", "flo rida")
                                         .Replace("jay-z", "jay z")
                                         .Replace("t-pain", "t pain")
                                         .Replace("t-wayne", "t wayne");

            // v názvu není pomlčka - zkusí nahradit uvozovky za pomlčku
            if (!upravenyNazev.Contains("-"))
            {
                // za první "'" nahradí "-" a odstraní ostatní
                Regex uvozovky = new Regex(Regex.Escape("\""));
                upravenyNazev = uvozovky.Replace(upravenyNazev, "-", 1);
                upravenyNazev = upravenyNazev.Replace("\"", "")
                                     .Replace(" -", "-");
            }
            // v názvu není pomlčka - video se nepodařilo přejmenovat -> konec
            if (!upravenyNazev.Contains("-"))
            {
                Chyba = "Nenalezen oddělovač v názvu";
                return;
            }

            // před pomlčkou je interpret
            interpret = upravenyNazev.Substring(0, upravenyNazev.IndexOf('-'))
                                     .Trim()
                                     .Replace("(ft", ",")
                                     .Replace(" x ", ",")
                                     .Replace(" ft", ",")
                                     .Replace(" & ", ",");
            // za pomlčkou je název skladby
            skladba = upravenyNazev.Substring(upravenyNazev.IndexOf('-') + 1)
                                   .Trim()
                                   .Replace("'", "")
                                   .Replace("( ft", " ft")
                                   .Replace("(ft", " ft");

            // získá featuringy z interpreta
            PridejInterpreta(new List<string>(interpret.Split(',')));

            // úprava skladby
            // skladba je remix -> přidám to co je v závorce do názvu
            if (skladba.Contains("remix"))
            {
                remix = true;
                skladba = skladba.Replace("remix", "")
                                 .Replace("()", "");
                /*
                // nahradním otevírací závorku pomlčkou, zavírací odstraním
                skladba = skladba.Replace("(", "-")
                                 .Replace(")", "");
                string[] uprava = skladba.Split('-');
                if (uprava.Length > 1)
                {
                    // na konec přidám to co je mezi 1. a . pomlčkou
                    pridejNaKonec = " " + uprava[1].Trim();
                }
                skladba = uprava[0];*/
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

            skladba = OdstranZbytecnosti(skladba);
            skladba = skladba.Trim();
            if (remix)
            {
                skladba = skladba + " remix";
            }
            skladba = OdstranZbytecnosti(skladba);

            // skladba je prázdná - soubor se nepodařilo přejmenovat -> konec
            if (String.IsNullOrEmpty(skladba))
            {
                Chyba = "Nenalezen oddělovač";
                return;
            }

            // převedení na velká písmenka na začátku a uložení jako nový název
            Skladba = skladba[0].ToString().ToUpper() + skladba.Substring(1, skladba.Length - 1).ToLower().Replace(" i ", " I ");
        }


        private string OdstranZbytecnosti(string vstup)
        {
            if (String.IsNullOrEmpty(vstup))
            {
                return "";
            }
            string[] odstran = { "off.", "off vid", "off vd", "music video", "official", " vd", "audio", "prod", "wshh" };
            for (int i = 0; i < odstran.Length; i++)
            {
                // odstraní postupně všehchen přebytečný text z pole
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

        // přidá interpreta ze stringu
        private void PridejInterpreta(string jmenoInterpreta)
        {
            if (String.IsNullOrEmpty(jmenoInterpreta))
            {
                return;
            }
            // úprava interpreta - odstranění zbytečnosti a převedení na velká písmena
            jmenoInterpreta = OdstranZbytecnosti(jmenoInterpreta);
            Interpret novyInterpret = new Interpret(jmenoInterpreta);

            // zamezí duplicitním interpretům
            if (!JeInterpretNaFeatu(novyInterpret.Jmeno))
            {
                Interpret staryInterpret = vsichniInterpreti.VratInterpreta(novyInterpret.Jmeno);
                // interpret již je používán u jiného videa
                if (staryInterpret != null)
                {
                    Interpreti.Add(staryInterpret);
                }
                // interpret nebyl dosud použit u jiného videa, přidá se tedy ten nový
                else
                {
                    Interpreti.Add(novyInterpret);
                    vsichniInterpreti.Interpreti.Add(novyInterpret);
                }
            }
        }
        // přidá interprety z pole stringů
        private void PridejInterpreta(List<string> jmenaInterpretu)
        {
            foreach (string jmenoInterpreta in jmenaInterpretu)
            {
                if (!String.IsNullOrEmpty(jmenoInterpreta))
                {
                    PridejInterpreta(jmenoInterpreta);
                }
            }
        }


        // odstraní interpreta ze seznamu featuringů
        private void OdstranInterpreta(string jmenoInterpreta)
        {
            if (JeInterpretNaFeatu(jmenoInterpreta))
            {
                Interpret interpretKOdstraneni = vsichniInterpreti.VratInterpreta(jmenoInterpreta);
                if (interpretKOdstraneni != null)
                {
                    Interpreti.Remove(interpretKOdstraneni);
                }
            }
        }

        private bool JeInterpretNaFeatu(string hledanyInterpret)
        {
            // zjistí zdali je už interpret přidaný do seznamu dříve
            foreach (Interpret interpret in this.Interpreti)
            {
                if (interpret.Jmeno.ToLower().Equals(hledanyInterpret.ToLower()))
                {
                    return true;
                }
            }
            return false;
        }



        private void NajdiSlozku()
        {
            if (Interpreti.Count < 1)
            {
                return;
            }
            
            // získá hledaného interpreta
            Interpret hledanyInterpret = Interpreti.First();
            string hledanyInterpretNazev = hledanyInterpret.Jmeno.Trim().ToLower();

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


            // složka, která obsahuje nejvíce interpretů
            string nejlepsiSlozka = "";
            // projde názvy složek ze získaných složek interpreta
            foreach (string slozkaInterpretaCesta in hledanyInterpret.Slozky)
            {
                string slozkaInterpretaNazev = Path.GetFileName(slozkaInterpretaCesta).ToLower().Trim();
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
                        spravnaSlozka = JeInterpretNaFeatu(interpretSlozkaNazev);
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
                else if (slozkaInterpretaNazev == hledanyInterpretNazev)
                {
                    // není featuring ve videu nebo nebyla nalezena hromadná složka interpretů

                    // existuje složka s názvem interpreta
                    Slozka = slozkaInterpretaCesta;
                    NazevNovy = NazevSkladbaFeat;
                    return;
                }
                if (slozkaInterpretaNazev == "_ostatní")
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
                Slozka = nejlepsiSlozka;
                NazevNovy = NazevSkladbaFeat;
                return;
            }
            Chyba = "Složka nenalezena";
        }


        // najdi soubor ve složce (vrací soubor, který existuje NEBO null pokud soubor neexistuje)
        private string NajdiSoubor(string hledanaSkladba)
        {

            // DODĚLAT
            // kontrola souboru na začátku - pozor padá při nesprávných znacích v názvu souboru
            // 1. zkontroluje, zdali se ve složce nenachází stejný soubor
            /*if (File.Exists(Path.Combine(Slozka, novyNazev + ".opus")))
            {
                // soubor již existuje
                Slozka = Path.Combine(Slozka, novyNazev + ".opus");
                novyNazev = "";
                Chyba = "Video bylo staženo dříve";
                return;
            }*/

            // 2. zkontroluje i další složky
            Regex zavorka = new Regex(@"\(([^\}]+)\)"); // odstraní cokoliv v závorce ( ), včetně featuringu !

            // získá soubory ze složky
            List<string> soubory = new List<string>();
            try
            {
                soubory.AddRange(Directory.GetFiles(Slozka, "*.*", SearchOption.AllDirectories).Where(s => "*.opus".Contains(Path.GetExtension(s).ToLower())));
            }
            catch (Exception)
            {
                Chyba = "Chyba hledání souboru";
                return null;
            }

            hledanaSkladba = hledanaSkladba.ToLower();

            if (!hledanaSkladba.Contains("(ft."))
            {
                hledanaSkladba = zavorka.Replace(hledanaSkladba, "");
                hledanaSkladba = OdstranZacatek(hledanaSkladba);
                hledanaSkladba = OdstranKonec(hledanaSkladba);
                hledanaSkladba = OdstranZbytek(hledanaSkladba);
            }
            foreach (string souborCesta in soubory)
            {
                string souborNazev = Path.GetFileNameWithoutExtension(souborCesta).ToLower();
                if (!souborNazev.Contains("(ft."))
                {
                    souborNazev = OdstranZacatek(souborNazev);
                    souborNazev = OdstranKonec(souborNazev);
                    souborNazev = OdstranZbytek(souborNazev);
                }

                if (souborNazev == hledanaSkladba)
                {
                    Chyba = "Video bylo staženo dříve";
                    return souborCesta;
                }
                else if (!Slozka.Contains("_ostatní") && Regex.IsMatch(souborNazev.Split(' ').First(), @"^\d{2}$")) // album -> rok název (první 4 znaky jsou číslice)
                {
                    // pro složku ostatní toto neplatí
                    if (souborNazev.Substring(2, souborNazev.Length - 2).Trim() == hledanaSkladba)
                    {
                        Chyba = "Video bylo staženo dříve";
                        return souborCesta;
                    }
                }
            }

            hledanaSkladba = zavorka.Replace(hledanaSkladba, "");
            hledanaSkladba = OdstranZacatek(hledanaSkladba);
            hledanaSkladba = OdstranKonec(hledanaSkladba);
            hledanaSkladba = OdstranZbytek(hledanaSkladba);

            // soubor předtím nenalezen -> nyní zkusím odstranit i featuring a najít shodné názvy
            foreach (string souborCesta in soubory)
            {
                string souborNazev = Path.GetFileNameWithoutExtension(souborCesta).ToLower();
                souborNazev = zavorka.Replace(souborNazev, "");
                souborNazev = OdstranZacatek(souborNazev);
                souborNazev = OdstranKonec(souborNazev);
                souborNazev = OdstranZbytek(souborNazev);


                if (souborNazev == hledanaSkladba)
                {
                    Chyba = "Video bylo nejspíš staženo dříve";
                    return souborCesta;
                }
                else if (!Slozka.Contains("_ostatní") && Regex.IsMatch(souborNazev.Split(' ').First(), @"^\d{2}$")) // album -> rok název (první 4 znaky jsou číslice)
                {
                    // pro složku ostatní toto neplatí
                    if (souborNazev.Substring(2, souborNazev.Length - 2).Trim() == hledanaSkladba)
                    {
                        Chyba = "Video bylo nejspíš staženo dříve";
                        return souborCesta;
                    }
                }
            }
            return null;
        }


        private string OdstranZacatek(string vstup)
        {
            // 48 - 57 = 0 - 9
            // 65 - 90 = A - Z (ale tolower() takže nepořebujeme)
            // 97 - 122 = a - z
            //if (!((vstup.First() >= 48 && vstup.First() <= 57) || (vstup.First() >= 97 && vstup.First() <= 122)))
            if (string.IsNullOrEmpty(vstup))
            {
                return vstup;
            }
            if ((vstup.First() >= 32 && vstup.First() <= 47) || (vstup.First() >= 58 && vstup.First() <= 63) || (vstup.First() >= 91 && vstup.First() <= 96) || (vstup.First() >= 123 && vstup.First() <= 126))
            {
                vstup = vstup.Substring(1);
            }
            vstup = OdstranMezery(vstup);
            return vstup;
        }
        private string OdstranKonec(string vstup)
        {
            if (string.IsNullOrEmpty(vstup))
            {
                return vstup;
            }
            //if (!((vstup.Last() >= 48 && vstup.Last() <= 57) || (vstup.Last() >= 97 && vstup.Last() <= 122)))
            if ((vstup.Last() >= 32 && vstup.Last() <= 47) || (vstup.Last() >= 58 && vstup.Last() <= 63) || (vstup.Last() >= 91 && vstup.Last() <= 96) || (vstup.Last() >= 123 && vstup.Last() <= 126))
            {
                vstup = vstup.Substring(0, vstup.Length - 1);
            }
            vstup = OdstranMezery(vstup);
            return vstup;
        }
        private string OdstranMezery(string vstup)
        {
            if (string.IsNullOrEmpty(vstup))
            {
                return vstup;
            }
            while (vstup.Contains("  "))
            {
                vstup = vstup.Replace("  ", " ");
            }

            return vstup;
        }
        private string OdstranZbytek(string vstup)
        {
            if (string.IsNullOrEmpty(vstup))
            {
                return vstup;
            }
            foreach (char znak in vstup)
            {
                if ((znak >= 32 && znak <= 47) || (znak >= 58 && znak <= 63) || (znak >= 91 && znak <= 96) || (znak >= 123 && znak <= 126))
                {
                    vstup = vstup.Replace(znak, ' ');
                }
            }
            vstup = OdstranMezery(vstup);
            return vstup;
        }
    }
}
