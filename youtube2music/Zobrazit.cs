using System;
using System.Windows.Forms;

namespace youtube_renamer
{
    /// <summary>
    /// Zobrazí upozornění nebo chybu.
    /// </summary>
    public class Zobrazit
    {
        /**
        ZOBRAZIT CHYBU
        **/

        /// <summary>
        /// Zobrazí MessageBox s chybou.
        /// </summary>
        /// <param name="nadpis">Nadpis chyby.</param>
        /// <param name="text">Text chyby.</param>
        public static void Chybu(string nadpis, string text)
        {
            nadpis = "Chyba: " + nadpis.ToLower();
            MessageBox.Show(text, nadpis, MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
        /// <summary>
        /// Zobrazí MessageBox s chybou.
        /// </summary>
        /// <param name="nadpis">Nadpis chyby.</param>
        /// <param name="text1">Text chyby na 1. řádku.</param>
        /// <param name="text2">Text chyby na 2. řádku.</param>
        public static void Chybu(string nadpis, string text1, string text2)
        {
            Chybu(nadpis, text1 + Environment.NewLine + text2);
        }
        /// <summary>
        /// Zobrazí MessageBox s chybou.
        /// </summary>
        /// <param name="nadpis">Nadpis chyby.</param>
        /// <param name="text1">Text chyby na 1. řádku.</param>
        /// <param name="text2">Text chyby na 2. řádku.</param>
        /// <param name="text3">Text chyby na 3. řádku.</param>
        public static void Chybu(string nadpis, string text1, string text2, string text3)
        {
            Chybu(nadpis, text1 + Environment.NewLine + text2 + Environment.NewLine + text3);
        }

        /**
        ZOBRAZIT UPOZORNĚNÍ
        **/

        /// <summary>
        /// Zobrazí MessageBox s upozorněním.
        /// </summary>
        /// <param name="nadpis">Nadpis upozornění.</param>
        /// <param name="text">Text upozornění.</param>
        public static void Upozorneni(string nadpis, string text)
        {
            MessageBox.Show(text, nadpis, MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
        /// <summary>
        /// Zobrazí MessageBox s upozorněním.
        /// </summary>
        /// <param name="nadpis">Nadpis upozornění.</param>
        /// <param name="text1">Text upozornění na 1. řádku.</param>
        /// <param name="text2">Text upozornění na 2. řádku.</param>
        public static void Upozorneni(string nadpis, string text1, string text2)
        {
            Upozorneni(nadpis, text1 + Environment.NewLine + text2);
        }
        /// <summary>
        /// Zobrazí MessageBox s upozorněním.
        /// </summary>
        /// <param name="nadpis">Nadpis upozornění.</param>
        /// <param name="text1">Text upozornění na 1. řádku.</param>
        /// <param name="text2">Text upozornění na 2. řádku.</param>
        /// <param name="text3">Text upozornění na 3. řádku.</param>
        public static void Upozorneni(string nadpis, string text1, string text2, string text3)
        {
            Upozorneni(nadpis, text1 + Environment.NewLine + text2 + Environment.NewLine + text3);
        }
    }
}
