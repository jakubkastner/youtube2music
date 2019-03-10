using Gecko;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace youtube_renamer
{
    public partial class FormUprava : Form
    {
        public List<Video> upravovanaVidea { get; set; }
        string hudebniKnihovna = "";

        private Video upravovaneVideo;
        private string zanr = "";
        private List<Interpret> interpreti = new List<Interpret>();
        private string slozka = "";
        private string chyba = "";
        private string skladba = "";
        private string nazevNovy = "";

        /// <summary>
        /// Index upravovaného videa.
        /// </summary>
        int indexVidea = 0;
        bool nacteno = false;


        /*
        SPUŠTĚNÍ PROGRAMU
        */

        /// <summary>
        /// Spusti formulář s úpravou videí.
        /// </summary>
        /// <param name="upravovanaVideaForm">Upravovaná videa.</param>
        /// <param name="hudebniKnihovnaForm">Složka hudební knihovny.</param>
        public FormUprava(List<Video> upravovanaVideaForm, string hudebniKnihovnaForm)
        {
            InitializeComponent();
            // prohlížeč
            Xpcom.Initialize("Firefox");
            Gecko.GeckoPreferences.User["general.useragent.override"] = "User-Agent:Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:60.0) Gecko/20100101 Firefox/60.0";

            upravovanaVidea = upravovanaVideaForm;
            hudebniKnihovna = hudebniKnihovnaForm;
        }

        /// <summary>
        /// Spuštění programu.
        /// </summary>
        private void FormUprava_Load(object sender, EventArgs e)
        {
            Nacist(false);
        }

        /// <summary>
        /// Načte video dle aktuálního indexu.
        /// </summary>
        /// <param name="obnovit">True, pokud uživatel kliknul na tlačítko "Obnovit". Nemusím tedy načítat všechny části znovu.</param>
        private void Nacist(bool obnovit)
        {
            nacteno = false;

            // neexistují upravovaná videa
            if (upravovanaVidea == null)
            {
                return;
            }
            if (upravovanaVidea.Count < 1)
            {
                return;
            }
            // nové video k úpravě
            if (!obnovit)
            {
                upravovaneVideo = upravovanaVidea[indexVidea];
            }

            // nastavení ovládacích prvků
            checkBoxUlozit.Checked = false;
            checkBoxStejnaSlozkaInterpret.Enabled = false;
            checkBoxStejnaSlozkaInterpret.Checked = false;
            checkBoxStejnaSlozkaVybrane.Enabled = false;
            checkBoxStejnaSlozkaVybrane.Checked = false;
            checkBoxStejnaSlozkaPlaylist.Enabled = false;
            checkBoxStejnaSlozkaPlaylist.Checked = false;
            checkBoxStejnyZanrInterpret.Checked = false;
            checkBoxStejnyZanrVybrane.Checked = false;
            checkBoxStejnyZanrPlaylist.Checked = false;


            // naplnění prvků hodnotami
            textBoxInterpret.Text = upravovaneVideo.Interpret;
            labelInterpret.Text = upravovaneVideo.Interpret;
            labelSkladba.Text = upravovaneVideo.NazevSkladbaFeat;
            textBoxSkladba.Text = upravovaneVideo.Skladba;
            textBoxFeaturing.Text = upravovaneVideo.InterpretiFeat;
            textBoxNovyNazev.Text = upravovaneVideo.NazevNovy;
            textBoxSlozka.Text = upravovaneVideo.Slozka;
            textBoxZanr.Text = upravovaneVideo.Zanr;
            labelChyba.Text = upravovaneVideo.Chyba;

            // nadpis programu
            if (String.IsNullOrEmpty(textBoxNovyNazev.Text))
            {
                this.Text = textBoxPuvodniNazev.Text + " ~ Youtube Renamer";
            }
            else
            {
                this.Text = textBoxInterpret.Text + "-" + textBoxSkladba.Text;
                if (textBoxFeaturing.Text != "")
                {
                    this.Text += " (ft. " + textBoxFeaturing.Text + ")";
                }
                this.Text += " ~ Youtube Renamer";
            }

            // načtení nového videa
            if (!obnovit)
            {
                // uložení do proměnných
                zanr = upravovaneVideo.Zanr;
                interpreti = upravovaneVideo.Interpreti;
                slozka = upravovaneVideo.Slozka;
                chyba = upravovaneVideo.Chyba;
                skladba = upravovaneVideo.Skladba;
                nazevNovy = upravovaneVideo.NazevNovy;
                // naplnění prvků hodnotami
                linkLabelID.Text = upravovaneVideo.ID;
                linkLabelKanal.Text = upravovaneVideo.Kanal.Nazev;
                textBoxDatum.Text = String.Format("{0:yyyy-MM-dd}", upravovaneVideo.Publikovano);
                textBoxPuvodniNazev.Text = upravovaneVideo.NazevPuvodni;

                // tlačítka následující / předchozí
                if (indexVidea == 0)
                {
                    // první upravované video
                    buttonPredchozi.Enabled = false;
                }
                else
                {
                    buttonPredchozi.Enabled = true;
                }
                if (indexVidea == upravovanaVidea.Count - 1)
                {
                    // poslední upravované video
                    buttonNasledujici.Enabled = false;
                }
                else
                {
                    buttonNasledujici.Enabled = true;
                }

                // nastavení ovládacích prvků
                if (labelChyba.Text == "Video neexistuje")
                {
                    // neexistující video
                    textBoxInterpret.Enabled = false;
                    textBoxSkladba.Enabled = false;
                    textBoxFeaturing.Enabled = false;
                    textBoxNovyNazev.Enabled = false;
                    textBoxSlozka.Enabled = false;
                    textBoxZanr.Enabled = false;
                    buttonSlozkaJina.Enabled = false;
                    buttonSlozkaNajit.Enabled = false;
                    buttonSlozkaOtevrit.Enabled = false;
                    checkBoxStejnyZanrInterpret.Enabled = false;
                    checkBoxStejnyZanrVybrane.Enabled = false;
                    checkBoxStejnyZanrPlaylist.Enabled = false;
                }
                else
                {
                    // existující video
                    textBoxInterpret.Enabled = true;
                    textBoxSkladba.Enabled = true;
                    textBoxFeaturing.Enabled = true;
                    textBoxNovyNazev.Enabled = true;
                    textBoxSlozka.Enabled = true;
                    textBoxZanr.Enabled = true;
                    buttonSlozkaJina.Enabled = true;
                    buttonSlozkaNajit.Enabled = true;
                    buttonSlozkaOtevrit.Enabled = true;
                    checkBoxStejnyZanrInterpret.Enabled = true;
                    checkBoxStejnyZanrVybrane.Enabled = true;
                    checkBoxStejnyZanrPlaylist.Enabled = true;
                }
                geckoWebBrowserVideo.Navigate("https://www.youtube.com/embed/" + upravovaneVideo.ID);
            }

            nacteno = true;
        }

        /*
        DOLNÍ TLAČÍTKA 
        */

        /// <summary>
        /// Zobrazí předchozí úpravu videa.
        /// </summary>
        private void buttonPredchozi_Click(object sender, EventArgs e)
        {
            Ulozit();
            indexVidea--;
            Nacist(false);
        }

        /// <summary>
        /// Zobrazí následující úpravu videa.
        /// </summary>
        private void buttonNasledujici_Click(object sender, EventArgs e)
        {
            Ulozit();
            indexVidea++;
            Nacist(false);
        }

        /// <summary>
        /// Obnoví znovu úpravu na výchozí hodnotu.
        /// </summary>
        private void buttonObnovit_Click(object sender, EventArgs e)
        {
            upravovaneVideo.Interpreti.RemoveRange(0, upravovaneVideo.Interpreti.Count());
            // získá název skladby a interprety
            upravovaneVideo.ZiskejInformace();
            // najde složku
            upravovaneVideo.NajdiSlozku();
            // načte změny
            Nacist(true);
        }


        /*
        HORNÍ TLAČÍTKA 
        */

            private void buttonSlozkaNajit_Click(object sender, EventArgs e)
        {
            checkBoxUlozit.Checked = true;
            Ulozit();
            upravovaneVideo.NajdiSlozku();
            Nacist(true);
        }

        /// <summary>
        /// Vybere ručně složku pomocí FolderBrowserDialogu.
        /// </summary>
        private void buttonSlozkaJina_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog vyberSlozky = new FolderBrowserDialog();
            vyberSlozky.Description = "Vyberte složku do které se stáhne video:";
            // nastaví výchozí cestu
            if (Directory.Exists(textBoxSlozka.Text))
            {
                vyberSlozky.SelectedPath = textBoxSlozka.Text;
            }
            else if (File.Exists(textBoxSlozka.Text))
            {
                vyberSlozky.SelectedPath = Path.GetDirectoryName(textBoxSlozka.Text);
            }
            else
            {
                vyberSlozky.SelectedPath = hudebniKnihovna;
            }

            if (vyberSlozky.ShowDialog() == DialogResult.OK)
            {
                textBoxSlozka.Text = vyberSlozky.SelectedPath;
            }
        }

        /// <summary>
        /// Otevře složku nebo soubor v průzkumníku Windows.
        /// </summary>
        private void buttonSlozkaOtevrit_Click(object sender, EventArgs e)
        {
            if (Directory.Exists(textBoxSlozka.Text))
            {
                try
                {
                    Process.Start(textBoxSlozka.Text);
                }
                catch (Exception)
                {
                    Zobrazit.Chybu("Otevírání složky", "Složku se nepodařilo otevřit.");
                }
                return;
            }
            if (File.Exists(textBoxSlozka.Text))
            {
                try
                {
                    Process.Start("explorer", "/select, \"" + textBoxSlozka.Text + "\"");
                }
                catch (Exception)
                {
                    Zobrazit.Chybu("Otevírání složky", "Složku se nepodařilo otevřit.");
                }
                return;
            }
            Zobrazit.Chybu("Otevírání složky", "Složka neexistuje.");
        }

        /*
        OTEŘENÍ PROHLÍŽEČE S ODKAZEM
        */

        /// <summary>
        /// Otevře v internetovém prohlížeči aktuální upravované video.
        /// </summary>
        private void linkLabelID_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("https://youtu.be/" + upravovaneVideo.ID);
        }

        private void linkLabelKanal_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("https://youtube.com/channel/" + upravovaneVideo.Kanal.ID);
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
                checkBoxStejnaSlozkaPlaylist.Enabled = false;
            }
            else
            {
                buttonSlozkaOtevrit.Enabled = true;
                checkBoxStejnaSlozkaInterpret.Enabled = true;
                checkBoxStejnaSlozkaVybrane.Enabled = true;
                checkBoxStejnaSlozkaPlaylist.Enabled = true;
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
            if (textBoxInterpret.Text != upravovaneVideo.Interpret ||
                textBoxSkladba.Text != upravovaneVideo.Skladba ||
                textBoxFeaturing.Text != upravovaneVideo.InterpretiFeat ||
                textBoxNovyNazev.Text != upravovaneVideo.NazevInterpretSkladbaFeat || // NazevNovy
                textBoxSlozka.Text != upravovaneVideo.Slozka ||
                textBoxZanr.Text != upravovaneVideo.Zanr ||
                checkBoxStejnaSlozkaInterpret.Checked ||
                checkBoxStejnaSlozkaVybrane.Checked ||
                checkBoxStejnaSlozkaPlaylist.Checked ||
                checkBoxStejnyZanrInterpret.Checked ||
                checkBoxStejnyZanrVybrane.Checked ||
                checkBoxStejnyZanrPlaylist.Checked)
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
            //upravovanaVidea[index].Interpret = textBoxInterpret.Text; //UPRAVIT INTERPRET READONLY
            //upravovaneVideo.OdstranInterpreta(interpreti.First().Jmeno);
            /*foreach (Interpret interpretKOdstraneni in upravovaneVideo.Interpreti)
            {
                upravovaneVideo.Interpreti.Remove(interpretKOdstraneni);
            }*/
            upravovaneVideo.Interpreti.RemoveRange(0, upravovaneVideo.Interpreti.Count());
            upravovaneVideo.PridejInterpreta(textBoxInterpret.Text);
            upravovaneVideo.PridejInterpreta(textBoxFeaturing.Text.Split(new[] { " & ", ", " }, StringSplitOptions.None).ToList());
            upravovaneVideo.Skladba = textBoxSkladba.Text;
            //upravovanaVidea[index].InterpretiFeat = textBoxFeaturing.Text; //UPRAVIT INTERPRETI FEAT READONLY
            /*if (textBoxFeaturing.Text != "")
            {
                upravovanaVidea[index].skladbaFeaturing = textBoxSkladba.Text + " (ft. " + textBoxFeaturing.Text + ")";
            }
            else
            {
                upravovanaVidea[index].skladbaFeaturing = textBoxSkladba.Text;
            }*/
            //upravovanaVidea[index].NazevCely = textBoxNovyNazev.Text; // NazevNovy = readonly
            upravovaneVideo.Zanr = textBoxZanr.Text;
            // uloží složku
            UlozSlozku(upravovaneVideo);

            // pokud je zaškrtnuto použití stejné složky u všech upravovaných videí, uložím složku i u nich
            if (checkBoxStejnaSlozkaVybrane.Checked)
            {
                foreach (Video vid in upravovanaVidea)
                {
                    UlozSlozku(vid);
                }
            }
            // pokud je zaškrtnuto použití stejné složky u všech upravovaných videí stejného interpreta, uložím složku i u nich
            if (checkBoxStejnaSlozkaInterpret.Checked)
            {
                foreach (Video vid in upravovanaVidea)
                {
                    if (vid.Interpret == textBoxInterpret.Text)
                    {
                        // interpret je stejný
                        UlozSlozku(vid);
                    }
                }
            }
            // pokud je zaškrtnuto použití stejné složky u všech upravovaných videí stejného interpreta, uložím složku i u nich
            if (checkBoxStejnaSlozkaPlaylist.Checked)
            {
                foreach (Video vid in upravovanaVidea)
                {
                    if (vid.Playlist == upravovaneVideo.Playlist)
                    {
                        // interpret je stejný
                        UlozSlozku(vid);
                    }
                }
            }

            // pokud je zaškrtnuto použití stejného žánru u všech upravovaných videí, uložím žánr i u nich
            if (checkBoxStejnyZanrVybrane.Checked)
            {
                foreach (Video vid in upravovanaVidea)
                {
                    vid.Zanr = textBoxZanr.Text;
                }
            }
            // pokud je zaškrtnuto použití stejného žánru u všech upravovaných videí stejného interpreta, uložím žánr i u nich
            if (checkBoxStejnyZanrInterpret.Checked)
            {
                foreach (Video vid in upravovanaVidea)
                {
                    if (vid.Interpret == textBoxInterpret.Text)
                    {
                        vid.Zanr = textBoxZanr.Text;
                    }
                }
            }
            if (checkBoxStejnyZanrPlaylist.Checked)
            {
                foreach (Video vid in upravovanaVidea)
                {
                    if (vid.Playlist == upravovaneVideo.Playlist)
                    {
                        vid.Zanr = textBoxZanr.Text;
                    }
                }
            }
        }

        /// <summary>
        /// Zkontroluje ukládanou složku.
        /// Pokud není vybrána, zapíše se jako chyba.
        /// </summary>
        /// <param name="ukladaneVideo">Video k uložení složky.</param>
        private void UlozSlozku(Video ukladaneVideo)
        {
            string ukladanaSlozka = textBoxSlozka.Text;
            if (String.IsNullOrEmpty(ukladanaSlozka))
            {
                // není vybrána žádná složka
                ukladaneVideo.Chyba = "Složka nenalezena";
                ukladanaSlozka = "";
            }
            else
            {
                string novyNazev = "";
                if (ukladanaSlozka.ToLower().Contains("_ostatní"))
                {
                    novyNazev = ukladaneVideo.Interpret + "-";
                }
                novyNazev += ukladaneVideo.Skladba;

                if (!String.IsNullOrEmpty(ukladaneVideo.InterpretiFeat))
                {
                    novyNazev += " (ft. " + ukladaneVideo.InterpretiFeat + ")";
                }
                //ukladaneVideo.NazevCely = novyNazev; // NazevNovy - readonly

                // ODSTRANIT !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
                // JEN DOČASNĚ
                ukladaneVideo.Chyba = "";
            }
            ukladaneVideo.Slozka = ukladanaSlozka;
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
            Process.Start("https://youtu.be/" + upravovaneVideo.ID);
            e.Cancel = true;
        }

        private void checkBoxZmena_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox zaskrtavac = (CheckBox)sender;
            if (zaskrtavac.Checked)
            {
                checkBoxUlozit.Checked = true;
            }
            else
            {
                Zmena();
            }
        }

        private void webBrowserVideo_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {
            geckoWebBrowserVideo.Navigate("https://www.youtube.com/watch?v=" + upravovaneVideo.ID);
        }

    }
}
