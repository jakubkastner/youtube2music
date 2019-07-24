using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace youtube2music
{
    public class Deezer
    {
        public int Id { get; set; }
        public string Nazev { get; set; }
        public string CoverNejvetsi { get; set; }
        public string CoverStredni { get; set; }
        public string Interpret
        {
            get
            {
                if (this.Interpreti == null)
                {
                    return "";
                }
                if (this.Interpreti.Count < 2)
                {
                    return this.Interpreti.First();
                }
                string vysledek = String.Join(", ", this.Interpreti);
                int posledniOddelovac = vysledek.LastIndexOf(',');
                vysledek = vysledek.Remove(posledniOddelovac, 1);
                vysledek = vysledek.Insert(posledniOddelovac, " &");
                return vysledek;
            }
        }
        public int Cislo
        {
            get
            {
                if (this.Skladby == null)
                {
                    return 0;
                }
                return this.Skladby.Count;
            }
        }

        public string Datum { get; set; }
        public List<string> Interpreti { get; set; }
        public List<SkladbyAlba> Skladby { get; set; }

        // vytvoří album
        public Deezer(int albumId)
        {
            InicializaceAlba(albumId);
        }

        public void InicializaceAlba(int albumId)
        {
            this.Id = albumId;
            // získá skladbu - id, název
            string adresa = "https://api.deezer.com/album/" + albumId;

            string ziskanyJson;
            using (WebClient klient = new WebClient())
            {
                ziskanyJson = klient.DownloadString(adresa);
            }
            Chyba chybaJson = new Chyba(ziskanyJson);
            if (chybaJson.JeChyba)
            {
                if (chybaJson.Kod == 4)
                {
                    InicializaceAlba(albumId);
                }
                return;
            }
            var seznamNalezenychSkladeb = JsonConvert.DeserializeObject<AlbumZiskejSkladby>(ziskanyJson);

            this.Nazev = seznamNalezenychSkladeb.title;
            this.CoverNejvetsi = seznamNalezenychSkladeb.cover_xl;
            this.CoverNejvetsi = this.CoverNejvetsi.Replace("1000", "2000");
            this.CoverStredni = seznamNalezenychSkladeb.cover_medium;
            this.Datum = seznamNalezenychSkladeb.release_date;

            this.Interpreti = new List<string>();
            foreach (var interpret in seznamNalezenychSkladeb.contributors)
            {
                this.Interpreti.Add(interpret.name);
            }

            this.Skladby = new List<SkladbyAlba>();
            int pocet = 0;
            foreach (var nalezenaSkladbaAlba in seznamNalezenychSkladeb.tracks.data)
            {
                this.Skladby.Add(new SkladbyAlba(nalezenaSkladbaAlba.id, ++pocet, nalezenaSkladbaAlba.title, this.Interpreti, this));
            }
        }

        public class SkladbyAlba
        {
            public int Id { get; set; }
            public int Cislo { get; set; }
            public string Nazev { get; set; }
            public Deezer Album { get; set; }
            public List<string> Interpreti { get; set; }

            public string Interpret
            {
                get
                {
                    if (this.Interpreti.Count < 1)
                    {
                        return "";
                    }
                    if (this.Interpreti.Count < 2)
                    {
                        return this.Interpreti.First();
                    }
                    string vysledek = String.Join(", ", this.Interpreti);
                    int a = vysledek.LastIndexOf(',');
                    vysledek = vysledek.Remove(a, 1);
                    vysledek = vysledek.Insert(a, " &");
                    return vysledek;
                }
            }

            private List<string> RozdelString(string kRozdeleni, string rozdelovac)
            {
                List<string> rozdeleny = new List<string>();
                if (kRozdeleni.Contains(rozdelovac))
                {
                    // je featuring v názvu skladby
                    rozdeleny = kRozdeleni.Split(new string[] { rozdelovac }, StringSplitOptions.None)
                                           .Select(s => s.Trim())
                                           .ToList();
                }
                return rozdeleny;
            }

            public SkladbyAlba(int idSkladby, int cisloSkladby, string nazevSkladby, List<string> interpetiAlba, Deezer album)
            {
                this.Id = idSkladby;
                this.Cislo = cisloSkladby;
                this.Album = album;
                List<string> interpretiZNazvu = new List<string>();
                List<string> featuringRozdeleny = new List<string>();
                featuringRozdeleny.AddRange(RozdelString(nazevSkladby, "(ft."));
                featuringRozdeleny.AddRange(RozdelString(nazevSkladby, "(feat."));

                if (featuringRozdeleny.Count >= 2)
                {
                    // je featuring v názvu
                    nazevSkladby = featuringRozdeleny.First()
                                                     .Trim();
                    string featuring = featuringRozdeleny.Last()
                                                         .Replace(")", "")
                                                         .Trim();
                    interpretiZNazvu = featuring.Split(',')
                                                .Select(s => s.Replace(" and ", " & ").Trim())
                                                .ToList();
                    List<string> odstranInterprety = new List<string>();
                    List<string> pridejInterprety = new List<string>();
                    foreach (var interpretZNazvu in interpretiZNazvu)
                    {
                        if (interpretZNazvu.Contains(" & "))
                        {
                            odstranInterprety.Add(interpretZNazvu);
                            string[] interpretZNazvuRozdeleny = interpretZNazvu.Split(new string[] { " & " }, StringSplitOptions.None);
                            // aktuálního interpreta přepíše první částí rozdělených stringů
                            for (int i = 0; i < interpretZNazvuRozdeleny.Length; i++)
                            {
                                // přidá další interprety
                                pridejInterprety.Add(interpretZNazvuRozdeleny[i].Trim());
                            }
                        }
                    }
                    // přidá interprety, kteří vznikly rozdělením " & "
                    interpretiZNazvu.AddRange(pridejInterprety);
                    // odstraní interprety kteří obsahují " & "
                    foreach (var odstranInterpreta in odstranInterprety)
                    {
                        interpretiZNazvu.Remove(odstranInterpreta);
                    }
                }

                this.Nazev = nazevSkladby;

                ZiskejInterpretySkladby(interpetiAlba);

                foreach (var interpretZNazvu in interpretiZNazvu)
                {
                    if (!interpetiAlba.Contains(interpretZNazvu) && !this.Interpreti.Contains(interpretZNazvu))
                    {
                        // interpreti alba ani interpeti skladby nejsou stejní jako ti získaní ze stringu
                        this.Interpreti.Add(interpretZNazvu);
                    }
                }
            }

            private void ZiskejInterpretySkladby(List<string> interpretiAlba)
            {
                // získá skladbu - interprety
                string adresa = "https://api.deezer.com/track/" + this.Id;

                string ziskanyJson;
                using (WebClient klient = new WebClient())
                {
                    ziskanyJson = klient.DownloadString(adresa);
                }
                Chyba chybaJson = new Chyba(ziskanyJson);
                if (chybaJson.JeChyba)
                {
                    if (chybaJson.Kod == 4)
                    {
                        ZiskejInterpretySkladby(interpretiAlba);
                    }
                    return;
                }

                this.Interpreti = new List<string>();

                var nalezenaSkladba = JsonConvert.DeserializeObject<AlbumZiskejInterpretySkladby>(ziskanyJson);
                foreach (var interpretSkladby in nalezenaSkladba.contributors)
                {
                    if (!interpretiAlba.Contains(interpretSkladby.name))
                    {
                        // interpret skladby není stejný jako u alba = můžu ho uložit
                        this.Interpreti.Add(interpretSkladby.name);
                    }
                }
            }
        }
    }
    //
    //  COVER 
    //
    public class AlbumZiskej
    {
        public List<AlbumInformace> data { get; set; }
        public string next { get; set; }
    }
    public class AlbumInformace
    {
        public int id { get; set; }
        public string record_type { get; set; }
        //public string title { get; set; }
        //public string cover_xl { get; set; }
        //public AlbumInterpret artist { get; set; }
    }

    /*public class AlbumInterpret
    {
        public string name { get; set; }
    }*/

    public class AlbumInterpreti
    {
        public string name { get; set; }
    }

    //
    //  INFO 
    //

    public class AlbumZiskejSkladby
    {
        public string title { get; set; }
        public string cover_xl { get; set; }
        public string cover_medium { get; set; }
        public string release_date { get; set; }
        public AlbumInterpreti[] contributors { get; set; }
        public AlbumSkladby tracks { get; set; }
    }

    public class AlbumSkladby
    {
        public AlbumSkladbyInformace[] data { get; set; }
    }

    public class AlbumSkladbyInformace
    {
        public int id { get; set; }
        public string title { get; set; }
    }
    //
    //  SLADBA 
    //
    public class AlbumZiskejInterpretySkladby
    {
        public AlbumSkladbyInterpreti[] contributors { get; set; }
    }

    public class AlbumSkladbyInterpreti
    {
        public string name { get; set; }
    }
}
