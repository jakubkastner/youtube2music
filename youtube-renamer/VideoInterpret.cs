using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace youtube_renamer
{
    public class VideoInterpret
    {
        public string Nazev { get; set; }

        public VideoInterpret(string nazevInterpreta)
        {
            this.Nazev = nazevInterpreta.Trim();
            Regex zavorka = new Regex(@"\(([^\}]+)\)"); // odstraní cokoliv v závorce ( )
            this.Nazev = zavorka.Replace(this.Nazev, "");

            // nový interpret je prázdný
            if (String.IsNullOrEmpty(this.Nazev))
            {
                this.Nazev = "";
                return;
            }

            Prejmenuj();

            this.Nazev = this.Nazev.ToLower();
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

        // HOTOVO
        /// <summary>
        /// Změní název interpreta.
        /// </summary>
        /// <param name="interpret">Interpret ke změně názvu.</param>
        /// <returns>Změněný název interpreta.</returns>
        private void Prejmenuj()
        {
            Nazev = Nazev.Replace("asap", "a$ap");
            switch (this.Nazev)
            {
                // cz + sk
                case "jickson":
                    Nazev = "jimmy dickson";
                    break;
                case "jckpt":
                    Nazev = "jackpot";
                    break;
                case "radikal chef":
                    Nazev = "radikal";
                    break;
                case "s.barracuda":
                    Nazev = "sergei barracuda";
                    break;
                case "white russian":
                    Nazev = "igor";
                    break;
                case "white rusian":
                    Nazev = "igor";
                    break;
                case "mladej moris":
                    Nazev = "yg moris";
                    break;
                case "gleb : zoo":
                    Nazev = "gleb";
                    break;
                case "peatyf":
                    Nazev = "peatyf.";
                    break;
                case "sheen":
                    Nazev = "viktor sheen";
                    break;
                case "yzomandias":
                    Nazev = "logic";
                    break;
                case "hráč roku":
                    Nazev = "logic";
                    break;
                case "lvcas":
                    Nazev = "lvcas dope";
                    break;
                case "kanabis":
                    Nazev = "lvcas dope";
                    break;
                case "mike t":
                    Nazev = "dj mike trafik";
                    break;
                case "mike trafik":
                    Nazev = "dj mike trafik";
                    break;
                case "sensey syfu":
                    Nazev = "senseysyfu";
                    break;
                case "karlo":
                    Nazev = "gumbgu";
                    break;
                // zahraniční
                case "the black eyed peas":
                    Nazev = "black eyed peas";
                    break;
                case "tekashi69":
                    Nazev = "6ix9ine";
                    break;
                case "6ixty9ine":
                    Nazev = "6ix9ine";
                    break;
                case "slim jxmmi of rae sremmurd":
                    Nazev = "slim jxmmi";
                    break;
                case "jeezy":
                    Nazev = "young jeezy";
                    break;
                case "waka flocka":
                    Nazev = "waka flocka flame";
                    break;
                case "g herbo":
                    Nazev = "lil herb";
                    break;
                case "v.cha$e":
                    Nazev = "vinny cha$e";
                    break;
                case "joey bada$$":
                    Nazev = "joey badass";
                    break;
                case "gab3":
                    Nazev = "uzi";
                    break;
                case "travis scott":
                    Nazev = "travi$ scott";
                    break;
                case "ty dolla sign":
                    Nazev = "ty dolla $ign";
                    break;
                case "mgk":
                    Nazev = "machine gun kelly";
                    break;
                default:
                    break;
            }
        }

        // HOTOVO
        /// <summary>
        /// Převede text na velká počáteční písmena po oddělovači.
        /// </summary>
        /// <param name="vstup">Text k převedení na velká písmena po oddělovači.</param>
        /// <param name="oddelovac">Znak, po kterém budou velká písmena ve vstupu.</param>
        /// <returns>Převedený text na velká písmena po oddělovači.</returns>
        private void VelkaPismena(char oddelovac)
        {
            // pokud je posledním indexu oddělovač, odstraním ho a na konci zase přidám
            bool oddelovacNaKonci = false;
            if (Nazev.Last() == oddelovac)
            {
                Nazev = Nazev.TrimEnd(oddelovac);
                oddelovacNaKonci = true;
            }

            // rozdělí vstupní text pomocí oddělovače
            string[] oddelene = Nazev.Split(oddelovac);
            Nazev = "";
            for (int i = 0; i < oddelene.Length; i++)
            {
                if (!String.IsNullOrEmpty(oddelene[i]))
                {
                    // převede první znak na velké písmeno
                    Nazev += oddelene[i][0].ToString().ToUpper() + oddelene[i].Substring(1, oddelene[i].Length - 1);
                }
                if (oddelene.Length > 1)
                {
                    // nejedná se o poslední část nebo byl odstraněn oddělovač na začátku
                    if ((oddelene.Length - 1 != i) || oddelovacNaKonci)
                    {
                        // přidám oddělovač
                        Nazev += oddelovac;
                    }
                }
            }
            // převedení velikosti názvů u interpretů
            Nazev = Nazev.Replace("..", ".")
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
        }
    }
}
