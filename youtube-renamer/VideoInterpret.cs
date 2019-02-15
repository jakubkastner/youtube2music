using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace youtube_renamer
{
    public class VideoInterpret
    {
        private string nazev;
        public string Nazev
        {
            get
            {
                return this.nazev;
            }
            set
            {
                Prejmenuj();
                this.nazev = value;
            }
        }

        // HOTOVO
        /// <summary>
        /// Změní název interpreta.
        /// </summary>
        /// <param name="interpret">Interpret ke změně názvu.</param>
        /// <returns>Změněný název interpreta.</returns>
        private string Prejmenuj()
        {
            #region switch
            switch (this.nazev)
            {
                // cz + sk
                case "jickson":
                    nazev = "jimmy dickson";
                    break;
                case "jckpt":
                    nazev = "jackpot";
                    break;
                case "radikal chef":
                    nazev = "radikal";
                    break;
                case "s.barracuda":
                    nazev = "sergei barracuda";
                    break;
                case "white russian":
                    nazev = "igor";
                    break;
                case "white rusian":
                    nazev = "igor";
                    break;
                case "mladej moris":
                    nazev = "yg moris";
                    break;
                case "gleb : zoo":
                    nazev = "gleb";
                    break;
                case "peatyf":
                    nazev = "peatyf.";
                    break;
                case "sheen":
                    nazev = "viktor sheen";
                    break;
                case "yzomandias":
                    nazev = "logic";
                    break;
                case "hráč roku":
                    nazev = "logic";
                    break;
                case "lvcas":
                    nazev = "lvcas dope";
                    break;
                case "kanabis":
                    nazev = "lvcas dope";
                    break;
                case "mike t":
                    nazev = "dj mike trafik";
                    break;
                case "mike trafik":
                    nazev = "dj mike trafik";
                    break;
                case "sensey syfu":
                    nazev = "senseysyfu";
                    break;
                case "karlo":
                    nazev = "gumbgu";
                    break;
                // zahraniční
                case "the black eyed peas":
                    nazev = "black eyed peas";
                    break;
                case "tekashi69":
                    nazev = "6ix9ine";
                    break;
                case "6ixty9ine":
                    nazev = "6ix9ine";
                    break;
                case "slim jxmmi of rae sremmurd":
                    nazev = "slim jxmmi";
                    break;
                case "jeezy":
                    nazev = "young jeezy";
                    break;
                case "waka flocka":
                    nazev = "waka flocka flame";
                    break;
                case "g herbo":
                    nazev = "lil herb";
                    break;
                case "v.cha$e":
                    nazev = "vinny cha$e";
                    break;
                case "joey bada$$":
                    nazev = "joey badass";
                    break;
                case "gab3":
                    nazev = "uzi";
                    break;
                case "travis scott":
                    nazev = "travi$ scott";
                    break;
                case "ty dolla sign":
                    nazev = "ty dolla $ign";
                    break;
                case "mgk":
                    nazev = "machine gun kelly";
                    break;
                default:
                    break;
            }
            #endregion
            nazev = nazev.Replace("asap", "a$ap");
            return nazev;
        }
    }
}
