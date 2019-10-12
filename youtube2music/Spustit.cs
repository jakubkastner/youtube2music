using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace youtube2music
{
    public class Spustit
    {
        public static void Program(string cesta, bool soubor)
        {
            Program(cesta, "", false, soubor);
        }

        public static void Program(string cesta, string parametry = "", bool pockej = false, bool soubor = true)
        {
            // spustí program
            if (!soubor || File.Exists(cesta) || Directory.Exists(cesta))
            {
                try
                {
                    ProcessStartInfo info = new ProcessStartInfo(cesta, parametry);
                    Process spust = new Process();
                    spust.StartInfo = info;
                    spust.StartInfo.CreateNoWindow = false;
                    spust.Start();

                    if (pockej)
                    {
                        spust.WaitForExit();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Spouštění programu");
                    return;
                }
            }
            else
            {
                MessageBox.Show("Neexistující soubor (" + cesta + ") nelze spustit!", "Spouštění programu");
                return;
            }
        }
    }
}
