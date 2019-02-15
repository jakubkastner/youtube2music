using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace youtube_renamer
{
    public partial class FormUprava : Form
    {
        public List<VideoStare> upravovanaVidea { get; set; }
        string hudebniKnihovna = "";

        /// <summary>
        /// Index upravovaného videa.
        /// </summary>
        int index = 0;
        bool nacteno = false;


        // HOTOVO
        /*
        NAČTENÍ PROGRAMU
        */

        // HOTOVO
        /// <summary>
        /// Spusti formulář s úpravou videí.
        /// </summary>
        /// <param name="upravovanaVideaForm">Upravovaná videa.</param>
        /// <param name="hudebniKnihovnaForm">Složka hudební knihovny.</param>
        public FormUprava(List<VideoStare> upravovanaVideaForm, string hudebniKnihovnaForm/*, ListViewItem[] pol, ListViewGroupCollection listSkupiny*/)
        {
            InitializeComponent();
            upravovanaVidea = upravovanaVideaForm;
            hudebniKnihovna = hudebniKnihovnaForm;

            /*polozky = pol;
            skupinyList = listSkupiny;*/
        }
        // HOTOVO
        // spuštění programu
        private void FormUprava_Load(object sender, EventArgs e)
        {
            Nacist();
        }
        // HOTOVO
        /// <summary>
        /// Načte video dle aktuálního indexu.
        /// </summary>
        private void Nacist()
        {
            nacteno = false;

            // nastavení ovládacích prvků
            checkBoxUlozit.Checked = false;
            checkBoxStejnaSlozkaInterpret.Enabled = false;
            checkBoxStejnaSlozkaInterpret.Checked = false;
            checkBoxStejnaSlozkaVybrane.Enabled = false;
            checkBoxStejnaSlozkaVybrane.Checked = false;

            // tlačítka následující / předchozí
            if (index == 0)
            {
                buttonPredchozi.Enabled = false;
            }
            else
            {
                buttonPredchozi.Enabled = true;
            }
            if (index == upravovanaVidea.Count - 1)
            {
                buttonNasledujici.Enabled = false;
            }
            else
            {
                buttonNasledujici.Enabled = true;
            }
            
            // naplnění prvků hodnotami
            linkLabelID.Text = upravovanaVidea[index].id;
            textBoxID.Text = upravovanaVidea[index].kanal;
            textBoxDatum.Text = String.Format("{0:yyyy-MM-dd}", upravovanaVidea[index].publikovano);
            textBoxPuvodniNazev.Text = upravovanaVidea[index].nazevPuvodni;
            textBoxInterpret.Text = upravovanaVidea[index].interpret;
            labelInterpret.Text = upravovanaVidea[index].interpret;
            labelSkladba.Text = upravovanaVidea[index].skladbaFeaturing;
            textBoxSkladba.Text = upravovanaVidea[index].skladba;
            textBoxFeaturing.Text = upravovanaVidea[index].Featuring;
            textBoxNovyNazev.Text = upravovanaVidea[index].nazevNovy;
            textBoxSlozka.Text = upravovanaVidea[index].slozka;
            textBoxZanr.Text = upravovanaVidea[index].zanr;

            if (textBoxNovyNazev.Text == "")
            {
                this.Text = "Youtube Renamer ~ " + textBoxPuvodniNazev.Text;
            }
            else
            {
                this.Text = "Youtube Renamer ~ " + textBoxInterpret.Text + "-" + textBoxSkladba.Text;
                if (textBoxFeaturing.Text != "")
                {
                    this.Text += " (ft. " + textBoxFeaturing.Text + ")";
                }
            }
            /*int sirka = webBrowserVideo.Width - 45;
            int vyska = webBrowserVideo.Height - 45;*/
            //webBrowserVideo.AllowNavigation = true;
            //webBrowserVideo.Navigate("https://www.youtube.com/embed/" + upravovanaVidea[index].id);
            //webBrowserVideo.AllowNavigation = false;

            nacteno = true;
        }

        // HOTOVO
        /*
        DOLNÍ TLAČÍTKA 
        */

        // HOTOVO
        // zobrazí předchozí úpravu videa
        private void buttonPredchozi_Click(object sender, EventArgs e)
        {
            Ulozit();
            index--;
            Nacist();
        }
        // HOTOVO
        // zobrazí následující úpravu videa
        private void buttonNasledujici_Click(object sender, EventArgs e)
        {
            Ulozit();
            index++;
            Nacist();
        }
        // HOTOVO
        // obnoví znovu úpravu na výchozí hodnotu
        private void buttonObnovit_Click(object sender, EventArgs e)
        {
            Nacist();
        }


        // DODĚLAT + HOTOVO
        /*
        HORNÍ TLAČÍTKA 
        */
        // DODĚLAT
        private void buttonSlozkaNajit_Click(object sender, EventArgs e)
        {
            #region není dodělané
            // přidat -> najdi složku nového interpreta (nový featuring)
            //        -> najdi zdali soubor již neexistuje

            /*List<string> feat = textBoxFeaturing.Text.Split(new[] { " & ", ", " }, StringSplitOptions.None).ToList();
            string skladbaFeat = textBoxNovyNazev.Text.Split('-').Last();*/

            // 0 video id, 1 poznámka, 2 kanál, 3 kanál id, 4 žánr, 5 původní název, 6 interpret, 7 skladba, 
            // 8 featuring text, 9 skladba + featuring 10 nový název, 11 složka, 12 chyba, 13 skupina
            // datum publikování
            // feat (featuring) list
            // zaškrtnuto
            // DODĚLAT hledání složky u více interpretů (funguje pouze tvar: interpret & featuring, ale nefunguje: featuring & interpret) !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!



            //public Video(string[] polozky, DateTime vidPublikovano, List<string> vidFeat, bool vidZaskrtnuto)
            /*Video vid = new Video(
                new string[]{ linkLabelID.Text, videa[pocet].poznamka, linkLabelKanal.Text, videa[pocet].kanalID, textBoxZanr.Text, textBoxPuvodniNazev.Text, textBoxInterpret.Text, textBoxSkladba.Text,
                              textBoxFeaturing.Text, skladbaFeat, textBoxNovyNazev.Text, textBoxSlozka.Text, videa[pocet].chyba, videa[pocet].skupina },
                DateTime.ParseExact(textBoxDatum.Text,"yyyy-MM-dd",System.Globalization.CultureInfo.InvariantCulture),
                feat,
                videa[pocet].zaskrtnuto);
            videa[pocet] = vid;
            Nacti();*/
            #endregion
        }
        // HOTOVO
        // vybere ručně složku pomocí FolderBrowserDialogu
        private void buttonSlozkaJina_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog vyberSlozky = new FolderBrowserDialog();
            vyberSlozky.Description = "Vyberte složku do které se stáhne video:";
            // nastaví výchozí cestu
            if (textBoxSlozka.Text == "")
            {
                vyberSlozky.SelectedPath = hudebniKnihovna;
            }
            else
            {
                vyberSlozky.SelectedPath = textBoxSlozka.Text;
            }

            if (vyberSlozky.ShowDialog() == DialogResult.OK)
            {
                textBoxSlozka.Text = vyberSlozky.SelectedPath;
            }
        }
        // HOTOVO
        // otevře složku v průzkumníku Windows
        private void buttonSlozkaOtevrit_Click(object sender, EventArgs e)
        {
            if (!Directory.Exists(textBoxSlozka.Text))
            {
                Zobrazit.Chybu("Otevírání složky", "Složka neexistuje.");
                return;
            }
            Process.Start(textBoxSlozka.Text);
        }

        // HOTOVO VŠE
        /**
        OVEŘENÍ PROHLÍŽEČE S ODKAZEM
        **/
        // otevře v internetovém prohlížeči aktuální upravované video
        private void linkLabelID_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("https://youtu.be/" + linkLabelID.Text);
        }
        // ČASTEČNĚ HOTOVO
        /**
        ÚPRAVA HODNOT
        **/
        private void textBoxZanr_TextChanged(object sender, EventArgs e)
        {
            Zmena();
        }
        private void textBoxInterpret_TextChanged(object sender, EventArgs e)
        {
            ZmenaNazvu();
        }
        private void textBoxSkladba_TextChanged(object sender, EventArgs e)
        {
            ZmenaNazvu();
        }
        private void textBoxFeaturing_TextChanged(object sender, EventArgs e)
        {
            ZmenaNazvu();
        }
        private void textBoxNovyNazev_TextChanged(object sender, EventArgs e)
        {
            Zmena();
        }
        private void textBoxSlozka_TextChanged(object sender, EventArgs e)
        {
            ZmenaNazvu();

            if (String.IsNullOrEmpty(textBoxSlozka.Text))
            {
                buttonSlozkaOtevrit.Enabled = false;
                checkBoxStejnaSlozkaInterpret.Enabled = false;
                checkBoxStejnaSlozkaVybrane.Enabled = false;
            }
            else
            {
                buttonSlozkaOtevrit.Enabled = true;
                checkBoxStejnaSlozkaInterpret.Enabled = true;
                checkBoxStejnaSlozkaVybrane.Enabled = true;
            }
        }

        // DODĚLAT
        /// <summary>
        /// Pokud proběhla změna.
        /// Zkotroluje se, zdali je upravovaná hodnota shodná s výchozí hodnotou.
        /// Dle toho nastaví CheckBox uložení (shodná = false, rozdílná = true).
        /// </summary>
        private void Zmena()
        {
            // pokud nebylo načteno, nedochází ke změnám
            if (!nacteno) return;
            // kontroluje jestli je upravovaná hodnota shodná s výchozí hodnotou
            if (textBoxInterpret.Text != upravovanaVidea[index].interpret ||
                textBoxSkladba.Text != upravovanaVidea[index].skladba ||
                textBoxFeaturing.Text != upravovanaVidea[index].Featuring ||
                textBoxNovyNazev.Text != upravovanaVidea[index].nazevNovy ||
                textBoxSlozka.Text != upravovanaVidea[index].slozka ||
                textBoxZanr.Text != upravovanaVidea[index].zanr ||
                checkBoxStejnaSlozkaInterpret.Checked ||
                checkBoxStejnaSlozkaVybrane.Checked ||
                checkBoxStejnyZanrInterpret.Checked ||
                checkBoxStejnyZanrVybrane.Checked)
            {
                checkBoxUlozit.Checked = true;
            }
            else
            {
                checkBoxUlozit.Checked = false;
            }
        }
        // DODĚLAT
        /// <summary>
        /// Změní nový název.
        /// Získá featuring a převede jej na text.
        /// </summary>
        private void ZmenaNazvu()
        {
            if (!nacteno) return;

            // zobrazení interpreta na labelu
            labelInterpret.Text = textBoxInterpret.Text;

            string novyNazev = "";
            if (textBoxSlozka.Text.ToLower().Contains("_ostatní") || String.IsNullOrEmpty(textBoxSlozka.Text))
            {
                novyNazev = textBoxInterpret.Text + "-";
            }
            novyNazev += textBoxSkladba.Text;
            labelSkladba.Text = textBoxSkladba.Text;
            if (!String.IsNullOrEmpty(textBoxFeaturing.Text))
            {
                novyNazev += " (ft. " + textBoxFeaturing.Text + ")";
                labelSkladba.Text += " (ft. " + textBoxFeaturing.Text + ")";
            }
            textBoxNovyNazev.Text = novyNazev;
        }

        /**
        ZAVŘENÍ FORMULÁŘE A ULOŽENÍ ZMĚN
        **/
        // HOTOVO
        private void FormUprava_FormClosing(object sender, FormClosingEventArgs e)
        {
            Ulozit();
        }

        /// <summary>
        /// Uložení úprav.
        /// </summary>
        private void Ulozit()
        {
            if (!checkBoxUlozit.Checked)
            {
                // neukládám
                return;
            }
            // uloží hodnoty
            upravovanaVidea[index].interpret = textBoxInterpret.Text;
            upravovanaVidea[index].skladba = textBoxSkladba.Text;
            upravovanaVidea[index].Featuring = textBoxFeaturing.Text;
            if (textBoxFeaturing.Text != "")
            {
                upravovanaVidea[index].skladbaFeaturing = textBoxSkladba.Text + " (ft. " + textBoxFeaturing.Text + ")";
            }
            else
            {
                upravovanaVidea[index].skladbaFeaturing = textBoxSkladba.Text;
            }
            upravovanaVidea[index].nazevNovy = textBoxNovyNazev.Text;
            upravovanaVidea[index].zanr = textBoxZanr.Text;
            // uloží složku
            UlozSlozku(upravovanaVidea[index]);

            // pokud je zaškrtnuto použití stejné složky u všech upravovaných videí, uložím složku i u nich
            if (checkBoxStejnaSlozkaVybrane.Checked)
            {
                foreach (VideoStare vid in upravovanaVidea)
                {
                    UlozSlozku(vid);
                }
            }
            // pokud je zaškrtnuto použití stejné složky u všech upravovaných videí stejného interpreta, uložím složku i u nich
            if (checkBoxStejnaSlozkaInterpret.Checked)
            {
                foreach (VideoStare vid in upravovanaVidea)
                {
                    if (vid.interpret == textBoxInterpret.Text)
                    {
                        // interpret je stejný
                        UlozSlozku(vid);
                    }
                }
            }

            // pokud je zaškrtnuto použití stejného žánru u všech upravovaných videí, uložím žánr i u nich
            if (checkBoxStejnyZanrVybrane.Checked)
            {
                foreach (VideoStare vid in upravovanaVidea)
                {
                    vid.zanr = textBoxZanr.Text;
                }
            }
            // pokud je zaškrtnuto použití stejného žánru u všech upravovaných videí stejného interpreta, uložím žánr i u nich
            if (checkBoxStejnyZanrInterpret.Checked)
            {
                foreach (VideoStare vid in upravovanaVidea)
                {
                    if (vid.interpret == textBoxInterpret.Text)
                    {
                        vid.zanr = textBoxZanr.Text;
                    }
                }
            }
        }

        /// <summary>
        /// Zkontroluje ukládanou složku.
        /// Pokud není vybrána, zapíše se jako chyba.
        /// </summary>
        /// <param name="ukladaneVideo">Video k uložení složky.</param>
        private void UlozSlozku(VideoStare ukladaneVideo)
        {
            string ukladanaSlozka = textBoxSlozka.Text;
            if (String.IsNullOrEmpty(ukladanaSlozka))
            {
                // není vybrána žádná složka
                ukladaneVideo.chyba = "Složka nenalezena";
                ukladanaSlozka = "";
            }
            else
            {
                string novyNazev = "";
                if (ukladanaSlozka.ToLower().Contains("_ostatní"))
                {
                    novyNazev = ukladaneVideo.interpret + "-";
                }
                novyNazev += ukladaneVideo.skladba;

                if (!String.IsNullOrEmpty(ukladaneVideo.Featuring))
                {
                    novyNazev += " (ft. " + ukladaneVideo.Featuring + ")";
                }
                ukladaneVideo.nazevNovy = novyNazev;

                // ODSTRANIT !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
                // JEN DOČASNĚ
                ukladaneVideo.chyba = "";
            }
            ukladaneVideo.slozka = ukladanaSlozka;
        }

        // STARÉ UKLÁDÁNÍ
        #region UlozSlozku - staré
        /*private void UlozSlozku(Video ukladaneVideo)
        {
            string ukladanaSlozka = textBoxSlozka.Text;
            ukladaneVideo.slozka = textBoxSlozka.Text;
            if (String.IsNullOrEmpty(ukladanaSlozka))
            {
                // není vybrána žádná složka
                ukladaneVideo.chyba = "Složka nemohla být nalezena";
                return;
            }

            // UPRAVIT
            // toto řešit už při změně, nikoliv až teď
            /*if (ukladanaSlozka.ToLower().Contains("_ostatní"))
            {
                ukladaneVideo.nazevNovy = ukladaneVideo.interpret + "-" + ukladaneVideo.skladbaFeaturing;
            }
            else
            {
                ukladaneVideo.nazevNovy = ukladaneVideo.skladbaFeaturing;
            }*/

            /*foreach (ListViewItem polozka in polozky)
            {
                if (polozka.Text == ukladaneVideo.id)
                {
                    // 0 id, 1 kanál, 2 datum publikování, 3 původní název, 4 interpret, 5 skladba, 6 featuring, 7 nový název, 8 složka, 9 chyba, 10 žánr    
                    polozka.SubItems[4].Text = ukladaneVideo.interpret;
                    polozka.SubItems[5].Text = ukladaneVideo.skladba;
                    polozka.SubItems[6].Text = ukladaneVideo.featuring;
                    polozka.SubItems[7].Text = ukladaneVideo.nazevNovy;
                    polozka.SubItems[8].Text = ukladaneVideo.slozka;
                    polozka.SubItems[10].Text = ukladaneVideo.zanr;
                    foreach (ListViewGroup skupina in skupinyList)
                    {
                        if (skupina.Header == ukladaneVideo.skupina)
                        {
                            polozka.Group = skupina;
                            break;
                        }
                    }
                }
            }
        }*/
        #endregion
        
        
        // ???
        private void checkBoxUlozit_CheckedChanged(object sender, EventArgs e)
        {
            //buttonSlozkaNajit.Enabled = checkBoxUlozit.Checked;
        }

        // NAHRADIT
        // geckowebbrowser
        private void webBrowserVideo_NewWindow(object sender, CancelEventArgs e)
        {
            Process.Start("https://youtu.be/" + upravovanaVidea[index].id);
            e.Cancel = true;
        }

        private void checkBoxStejnyZanrInterpret_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxStejnyZanrInterpret.Checked)
            {
                checkBoxUlozit.Checked = true;
            }
            else
            {
                Zmena();
            }
        }

        private void checkBoxStejnyZanrVybrane_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxStejnyZanrVybrane.Checked)
            {
                checkBoxUlozit.Checked = true;
            }
            else
            {
                Zmena();
            }
        }

        private void checkBoxStejnaSlozkaInterpret_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxStejnaSlozkaInterpret.Checked)
            {
                checkBoxUlozit.Checked = true;
            }
            else
            {
                Zmena();
            }
        }

        private void checkBoxStejnaSlozkaVybrane_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxStejnaSlozkaVybrane.Checked)
            {
                checkBoxUlozit.Checked = true;
            }
            else
            {
                Zmena();
            }
        }
    }
}
