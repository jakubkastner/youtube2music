using System.Collections.Generic;

namespace youtube_renamer
{
    /// <summary>
    /// Seznam interpretů.
    /// </summary>
    public class SeznamInterpretu
    {
        /// <summary>
        /// Seznam interpretů.
        /// </summary>
        public List<Interpret> Interpreti { get; set; }

        /// <summary>
        /// Inicializace seznamu interpretů.
        /// </summary>
        public SeznamInterpretu()
        {
            Interpreti = new List<Interpret>();
        }

        /// <summary>
        /// Vrátí referenci objektu "Interpret" dle zadaného jména interpreta.
        /// </summary>
        /// <param name="jmenoInterpreta">Jméno interpreta k získání.</param>
        /// <returns>Vrátí referenci objektu interpreta podle názvu. Pokud již nebyl přidán vrací null.</returns>
        public Interpret VratInterpreta(string jmenoInterpreta)
        {
            foreach (Interpret interpret in Interpreti)
            {
                if (jmenoInterpreta.Trim().ToLower() == interpret.Jmeno.Trim().ToLower())
                {
                    // interpret nalezen
                    return interpret;
                }
            }
            return null;
        }
    }
}
