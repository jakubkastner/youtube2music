using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace youtube2music
{
    public class Chyba
    {
        public bool JeChyba { get; set; }
        public int Kod { get; set; }

        public class ChybaInformace
        {
            public string type { get; set; }
            public string message { get; set; }
            public string code { get; set; }
        }

        public Chyba(string ziskanyJson)
        {
            NajdiChybu(ziskanyJson, true);
        }
        public Chyba(string ziskanyJson, bool upozorneni)
        {
            NajdiChybu(ziskanyJson, upozorneni);
        }

        private void NajdiChybu(string ziskanyJson, bool upozorneni)
        {
            this.JeChyba = false;
            // kontrola, zdali je nalezena chyba
            if (ziskanyJson.Contains("error") && ziskanyJson.Contains("type") && ziskanyJson.Contains("message") && ziskanyJson.Contains("code"))
            {
                this.JeChyba = true;
                ziskanyJson = ziskanyJson.Replace("{\"error\":", "");
                ziskanyJson = ziskanyJson.Remove(ziskanyJson.Count() - 1, 1);

                var nalezenaChyba = JsonConvert.DeserializeObject<ChybaInformace>(ziskanyJson);
                this.Kod = Convert.ToInt32(nalezenaChyba.code);

                if (upozorneni && this.Kod != 4) // kód 4 je překročení limitu
                {
                    MessageBox.Show(nalezenaChyba.message, this.Kod + ": " + nalezenaChyba.type);
                }
            }
        }
    }
}
