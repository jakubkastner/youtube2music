using Gecko;
using Ookii.Dialogs.Wpf;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace youtube2music
{
    public partial class FormUprava : Form
    {
        public List<Video> upravovanaVidea { get; set; }
        string hudebniKnihovna = "";
        private Video upravovaneVideo;

        /// <summary>
        /// Index upravovaného videa.
        /// </summary>
        int indexVidea = 0;

        /// <summary>
        /// Požívá se pro kontrolu upravovaných vlastností. Pokud je hodnota false, nenačte kontrolu na textboxech.
        /// </summary>
        private bool nacteno = false;


        /*
        SPUŠTĚNÍ PROGRAMU
        */

        /// <summary>
        /// Spustí formulář s úpravou videí.
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
            checkBoxStejnaSlozkaInterpret.Checked = true;
            checkBoxStejnaSlozkaVybrane.Enabled = false;
            checkBoxStejnaSlozkaVybrane.Checked = false;
            checkBoxStejnaSlozkaPlaylist.Enabled = false;
            checkBoxStejnaSlozkaPlaylist.Checked = false;
            checkBoxStejnyZanrInterpret.Checked = false;
            checkBoxStejnyZanrVybrane.Checked = false;
            checkBoxStejnyZanrPlaylist.Checked = false;
            checkBoxNovyNazevAutomaticky.Checked = true;

            checkBoxPuvodniNazevInterpret.Enabled = false;
            checkBoxPuvodniNazevInterpret.Checked = false;
            checkBoxPuvodniNazevVybrane.Enabled = false;
            checkBoxPuvodniNazevVybrane.Checked = false;
            checkBoxPuvodniNazevPlaylist.Enabled = false;
            checkBoxPuvodniNazevPlaylist.Checked = false;

            // naplnění prvků hodnotami
            labelInterpret.Text = upravovaneVideo.Interpret;
            textBoxInterpret.Text = upravovaneVideo.Interpret;
            labelSkladba.Text = upravovaneVideo.NazevSkladbaFeat;
            textBoxSkladba.Text = upravovaneVideo.Skladba;
            textBoxFeaturing.Text = upravovaneVideo.InterpretiFeat;
            textBoxNovyNazev.Text = upravovaneVideo.NazevNovy;
            textBoxSlozka.Text = upravovaneVideo.Slozka;
            textBoxZanr.Text = upravovaneVideo.Zanr;
            if (upravovaneVideo.Album != null)
            {
                textBoxAlbum.Text = upravovaneVideo.Album.Interpret.Jmeno + " - " + upravovaneVideo.Album.Nazev;
                if (upravovaneVideo.Stopa > 0)
                {
                    textBoxStopa.Text = upravovaneVideo.Stopa.ToString("00");
                }
            }
            labelChyba.Text = upravovaneVideo.Chyba;

            // načtení nového videa
            if (!obnovit)
            {
                // naplnění prvků hodnotami
                linkLabelID.Text = upravovaneVideo.ID;
                linkLabelKanal.Text = upravovaneVideo.Kanal.Nazev;
                richTextBoxPopis.Text = upravovaneVideo.Popis;
                if (upravovaneVideo.PlaylistVidea.Nazev == upravovaneVideo.ID)
                {
                    // nejedná se o playlist
                    linkLabelPlaylist.Visible = false;
                    label12.Visible = false;
                }
                else
                {
                    // jedná se o playlist
                    linkLabelPlaylist.Visible = true;
                    label12.Visible = true;
                    linkLabelPlaylist.Text = upravovaneVideo.PlaylistVidea.Nazev;
                }
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
                if (String.IsNullOrEmpty(upravovaneVideo.Kanal.Nazev))
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
                    buttonNeexistujiciVyhledat.Visible = true;
                    richTextBoxPopis.Visible = false;
                    checkBoxUlozit.Enabled = false;
                    checkBoxNovyNazevAutomaticky.Enabled = false;
                    checkBoxStejnyInterpretPlaylist.Enabled = false;
                    checkBoxStejnyInterpretVybrane.Enabled = false;
                    buttonProhodit.Enabled = false;
                    buttonPuvodniNazev.Enabled = false;
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
                    buttonNeexistujiciVyhledat.Visible = false;
                    richTextBoxPopis.Visible = true;
                    checkBoxUlozit.Enabled = true;
                    checkBoxNovyNazevAutomaticky.Enabled = true;
                    checkBoxStejnyInterpretPlaylist.Enabled = true;
                    checkBoxStejnyInterpretVybrane.Enabled = true;
                    buttonProhodit.Enabled = true;
                    buttonPuvodniNazev.Enabled = true;
                    checkBoxPuvodniNazevInterpret.Enabled = true;
                    checkBoxPuvodniNazevVybrane.Enabled = true;
                    checkBoxPuvodniNazevPlaylist.Enabled = true;
                }
                geckoWebBrowserVideo.Navigate("https://www.youtube.com/embed/" + upravovaneVideo.ID);
            }

            // nadpis programu
            if (String.IsNullOrEmpty(textBoxNovyNazev.Text))
            {
                this.Text = textBoxPuvodniNazev.Text + " ~ youtube2music";
            }
            else
            {
                this.Text = textBoxInterpret.Text + "-" + textBoxSkladba.Text;
                if (textBoxFeaturing.Text != "")
                {
                    this.Text += " (ft. " + textBoxFeaturing.Text + ")";
                }
                this.Text += " ~ youtube2music";
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
        /// Obnoví upravené hodnoty videa na výchozí hodnoty.
        /// </summary>
        private void buttonObnovit_Click(object sender, EventArgs e)
        {
            string stav = upravovaneVideo.Stav;
            // odstraní interprety
            upravovaneVideo.Interpreti.RemoveRange(0, upravovaneVideo.Interpreti.Count());
            // získá název skladby a interprety
            upravovaneVideo.ZiskejInformace();
            // najde složku
            upravovaneVideo.NajdiSlozku();
            upravovaneVideo.Stav = stav;
            // načte změny
            Nacist(true);
        }


        /*
        HORNÍ TLAČÍTKA 
        */

        /// <summary>
        /// Nalezne automaticky složku a uspořádá některé informace.
        /// </summary>
        private void buttonSlozkaNajit_Click(object sender, EventArgs e)
        {
            string stav = upravovaneVideo.Stav;
            // uloží aktuální úpravu
            checkBoxUlozit.Checked = true;
            Ulozit();
            upravovaneVideo.Chyba = "";
            // najde automaticky složku a upraví některé informace
            upravovaneVideo.NajdiSlozku();
            if (upravovaneVideo.Album != null && upravovaneVideo.Stopa > 0)
            {
                upravovaneVideo.Slozka = upravovaneVideo.Album.Slozka;
                upravovaneVideo.NazevNovy = upravovaneVideo.NazevStopaSkladbaFeat;
            }
            upravovaneVideo.Stav = stav;
            // načte změny
            Nacist(true);
        }

        /// <summary>
        /// Vybere ručně složku pomocí FolderBrowserDialogu.
        /// </summary>
        private void buttonSlozkaJina_Click(object sender, EventArgs e)
        {
            VistaFolderBrowserDialog vyberSlozky = new VistaFolderBrowserDialog();
            vyberSlozky.Description = "Vyberte složku do které se stáhne video:";

            // nastaví výchozí cestu
            if (Directory.Exists(textBoxSlozka.Text))
            {
                // jedná se o složku
                vyberSlozky.SelectedPath = textBoxSlozka.Text;
            }
            else if (File.Exists(textBoxSlozka.Text))
            {
                // jedná se o soubor
                vyberSlozky.SelectedPath = Path.GetDirectoryName(textBoxSlozka.Text);
            }
            else
            {
                // nejedná se o existující složku ani soubor
                vyberSlozky.SelectedPath = hudebniKnihovna;
            }

            if ((bool)vyberSlozky.ShowDialog())
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
                // otevře složku
                try
                {
                    Process.Start(textBoxSlozka.Text);
                }
                catch (Exception)
                {
                    App.Show.Error("Otevírání složky", "Složku se nepodařilo otevřit.");
                }
                return;
            }
            if (File.Exists(textBoxSlozka.Text))
            {
                // otevře průzkumník a vybere soubor
                try
                {
                    Process.Start("explorer", "/select, \"" + textBoxSlozka.Text + "\"");
                }
                catch (Exception)
                {
                    App.Show.Error("Otevírání složky", "Složku se nepodařilo otevřit.");
                }
                return;
            }
            App.Show.Error("Otevírání složky", "Složka neexistuje.");
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

        /// <summary>
        /// Otevře v internetovém prohlížeči kanál daného videa.
        /// </summary>
        private void linkLabelKanal_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("https://youtube.com/channel/" + upravovaneVideo.Kanal.ID);
        }


        // DODĚLAT
        /*
        ÚPRAVA HODNOT
        */
        private void textBoxZanr_TextChanged(object sender, EventArgs e)
        {
            Zmena();
        }
        private void textBoxInterpret_TextChanged(object sender, EventArgs e)
        {
            Zmena();
            ZmenaNazvu();
        }
        private void textBoxSkladba_TextChanged(object sender, EventArgs e)
        {
            Zmena();
            ZmenaNazvu();
        }
        private void textBoxFeaturing_TextChanged(object sender, EventArgs e)
        {
            Zmena();
            ZmenaNazvu();
        }
        private void textBoxNovyNazev_TextChanged(object sender, EventArgs e)
        {
            Zmena();
        }
        private void textBoxSlozka_TextChanged(object sender, EventArgs e)
        {
            Zmena();
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
                checkBoxStejnyZanrPlaylist.Checked ||
                checkBoxPuvodniNazevVybrane.Checked ||
                checkBoxPuvodniNazevInterpret.Checked ||
                checkBoxPuvodniNazevPlaylist.Checked)
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
        /// Změní nový název dle složky.
        /// </summary>
        private void ZmenaNazvu()
        {
            if (!nacteno) return;

            // zobrazení interpreta na labelu
            labelInterpret.Text = textBoxInterpret.Text;

            if (checkBoxNovyNazevAutomaticky.Checked)
            {
                // automaticky se mění název
                string novyNazev = "";
                if (!String.IsNullOrEmpty(textBoxStopa.Text))
                {
                    novyNazev = textBoxStopa.Text + " ";
                }
                else if (textBoxSlozka.Text.ToLower().Contains("_ostatní") || String.IsNullOrEmpty(textBoxSlozka.Text))
                {
                    // v názvu bude interpret
                    novyNazev = textBoxInterpret.Text + "-";
                }
                novyNazev += textBoxSkladba.Text;
                labelSkladba.Text = textBoxSkladba.Text;

                if (!String.IsNullOrEmpty(textBoxFeaturing.Text))
                {
                    // v názvu bude featuring
                    novyNazev += " (ft. " + textBoxFeaturing.Text + ")";
                    labelSkladba.Text += " (ft. " + textBoxFeaturing.Text + ")";
                }
                textBoxNovyNazev.Text = novyNazev;
            }
        }


        /*
        ZAVŘENÍ FORMULÁŘE A ULOŽENÍ ZMĚN
        */

        /// <summary>
        /// Zavření úprav videa a jeho uložení.
        /// </summary>
        private void FormUprava_FormClosing(object sender, FormClosingEventArgs e)
        {
            Ulozit();
        }

        /// <summary>
        /// Uložení úprav videa.
        /// </summary>
        private void Ulozit()
        {
            if (!checkBoxUlozit.Checked)
            {
                // neukládám
                return;
            }
            // odstraní stávající interprety
            upravovaneVideo.Interpreti.RemoveRange(0, upravovaneVideo.Interpreti.Count());
            // přidá interpreta a interprety na featuringu
            upravovaneVideo.PridejInterpreta(textBoxInterpret.Text);
            upravovaneVideo.PridejInterpreta(textBoxFeaturing.Text.Split(new[] { " & ", ", ", " · " }, StringSplitOptions.None).ToList());
            // uloží skladbu a žánr
            upravovaneVideo.Skladba = textBoxSkladba.Text;
            upravovaneVideo.Zanr = textBoxZanr.Text;
            // uloží složku
            UlozSlozku(upravovaneVideo);
            upravovaneVideo.NazevNovy = textBoxNovyNazev.Text;

            // zašktnuto ukládání u jiných videí

            // ukládání interpreta
            if (checkBoxStejnyInterpretVybrane.Checked)
            {
                // je zaškrtnuto použití stejného interpreta u všech upravovaných videí, uložím interpreta i u nich
                foreach (Video vid in upravovanaVidea)
                {
                    // interpret je stejný
                    vid.OdstranVsechnyInterprety();
                    vid.PridejInterpreta(textBoxInterpret.Text);
                    UlozSlozku(vid);
                }
            }
            if (checkBoxStejnyInterpretPlaylist.Checked)
            {
                // je zaškrtnuto použití stejného interpreta u všech upravovaných videí ze stejného playlistu, uložím interpreta i u nich
                foreach (Video vid in upravovanaVidea)
                {
                    if (vid.PlaylistVidea.ID == upravovaneVideo.PlaylistVidea.ID)
                    {
                        // interpret je stejný
                        vid.OdstranVsechnyInterprety();
                        vid.PridejInterpreta(textBoxInterpret.Text);
                        UlozSlozku(vid);
                    }
                }
            }

            // ukládání složky
            if (checkBoxStejnaSlozkaVybrane.Checked)
            {
                // je zaškrtnuto použití stejné složky u všech upravovaných videí, uložím složku i u nich
                foreach (Video vid in upravovanaVidea)
                {
                    UlozSlozku(vid);
                }
            }
            if (checkBoxStejnaSlozkaInterpret.Checked)
            {
                // je zaškrtnuto použití stejné složky u všech upravovaných videí stejného interpreta, uložím složku i u nich
                foreach (Video vid in upravovanaVidea)
                {
                    if (vid.Interpret == textBoxInterpret.Text)
                    {
                        // interpret je stejný
                        UlozSlozku(vid);
                    }
                }
            }
            if (checkBoxStejnaSlozkaPlaylist.Checked)
            {
                // je zaškrtnuto použití stejné složky u všech upravovaných videí ze stejného playlistu, uložím složku i u nich
                foreach (Video vid in upravovanaVidea)
                {
                    if (vid.PlaylistVidea.ID == upravovaneVideo.PlaylistVidea.ID)
                    {
                        // žánr je stejný
                        UlozSlozku(vid);
                    }
                }
            }

            // ukládání žánru
            if (checkBoxStejnyZanrVybrane.Checked)
            {
                // je zaškrtnuto použití stejného žánru u všech upravovaných videí, uložím žánr i u nich
                foreach (Video vid in upravovanaVidea)
                {
                    vid.Zanr = textBoxZanr.Text;
                }
            }
            if (checkBoxStejnyZanrInterpret.Checked)
            {
                // je zaškrtnuto použití stejného žánru u všech upravovaných videí stejného interpreta, uložím žánr i u nich
                foreach (Video vid in upravovanaVidea)
                {
                    if (vid.Interpret == textBoxInterpret.Text)
                    {
                        // interpret je stejný
                        vid.Zanr = textBoxZanr.Text;
                    }
                }
            }
            if (checkBoxStejnyZanrPlaylist.Checked)
            {
                // je zaškrtnuto použití stejného žánru u všech upravovaných videí ze stejného playlistu, uložím žánr i u nich
                foreach (Video vid in upravovanaVidea)
                {
                    if (vid.PlaylistVidea.ID == upravovaneVideo.PlaylistVidea.ID)
                    {
                        // žánr je stejný
                        vid.Zanr = textBoxZanr.Text;
                    }
                }
            }


            // ukládání původního názvu jako názvu skladby
            if (checkBoxPuvodniNazevVybrane.Checked)
            {
                // je zaškrtnuto použití stejného žánru u všech upravovaných videí, uložím žánr i u nich
                foreach (Video vid in upravovanaVidea)
                {
                    vid.Skladba = vid.NazevPuvodni;
                    if (checkBoxNovyNazevAutomaticky.Checked)
                    {
                        // automaticky se mění název
                        string novyNazev = "";
                        if (vid.Album != null)
                        {
                            novyNazev = vid.Stopa.ToString("00") + " ";
                        }
                        else if (vid.Slozka.ToLower().Contains("_ostatní") || String.IsNullOrEmpty(vid.Slozka))
                        {
                            // v názvu bude interpret
                            novyNazev = vid.Interpret + "-";
                        }
                        novyNazev += vid.Skladba;

                        if (!String.IsNullOrEmpty(vid.InterpretiFeat))
                        {
                            // v názvu bude featuring
                            novyNazev += " (ft. " + vid.InterpretiFeat + ")";
                        }
                        vid.NazevNovy = novyNazev;
                    }
                }
            }
            if (checkBoxPuvodniNazevInterpret.Checked)
            {
                // je zaškrtnuto použití stejného žánru u všech upravovaných videí stejného interpreta, uložím žánr i u nich
                foreach (Video vid in upravovanaVidea)
                {
                    if (vid.Interpret == textBoxInterpret.Text)
                    {
                        // interpret je stejný
                        vid.Skladba = vid.NazevPuvodni;
                    }
                    if (checkBoxNovyNazevAutomaticky.Checked)
                    {
                        // automaticky se mění název
                        string novyNazev = "";
                        if (vid.Album != null)
                        {
                            novyNazev = vid.Stopa.ToString("00") + " ";
                        }
                        else if (vid.Slozka.ToLower().Contains("_ostatní") || String.IsNullOrEmpty(vid.Slozka))
                        {
                            // v názvu bude interpret
                            novyNazev = vid.Interpret + "-";
                        }
                        novyNazev += vid.Skladba;

                        if (!String.IsNullOrEmpty(vid.InterpretiFeat))
                        {
                            // v názvu bude featuring
                            novyNazev += " (ft. " + vid.InterpretiFeat + ")";
                        }
                        vid.NazevNovy = novyNazev;
                    }
                }
            }
            if (checkBoxPuvodniNazevPlaylist.Checked)
            {
                // je zaškrtnuto použití stejného žánru u všech upravovaných videí ze stejného playlistu, uložím žánr i u nich
                foreach (Video vid in upravovanaVidea)
                {
                    if (vid.PlaylistVidea.ID == upravovaneVideo.PlaylistVidea.ID)
                    {
                        // žánr je stejný
                        vid.Skladba = vid.NazevPuvodni;
                    }
                    if (checkBoxNovyNazevAutomaticky.Checked)
                    {
                        // automaticky se mění název
                        string novyNazev = "";
                        if (vid.Album != null)
                        {
                            novyNazev = vid.Stopa.ToString("00") + " ";
                        }
                        else if (vid.Slozka.ToLower().Contains("_ostatní") || String.IsNullOrEmpty(vid.Slozka))
                        {
                            // v názvu bude interpret
                            novyNazev = vid.Interpret + "-";
                        }
                        novyNazev += vid.Skladba;

                        if (!String.IsNullOrEmpty(vid.InterpretiFeat))
                        {
                            // v názvu bude featuring
                            novyNazev += " (ft. " + vid.InterpretiFeat + ")";
                        }
                        vid.NazevNovy = novyNazev;
                    }
                }
            }
        }

        // DODĚLAT
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
                if (checkBoxNovyNazevAutomaticky.Checked || String.IsNullOrEmpty(ukladaneVideo.Slozka))
                {
                    // automaticky se mění název
                    /*string novyNazev = "";*/
                    if (ukladaneVideo.Album != null && ukladaneVideo.Stopa > 0)
                    {
                        ukladaneVideo.NazevNovy = ukladaneVideo.NazevStopaSkladbaFeat;
                    }
                    else if (textBoxSlozka.Text.ToLower().Contains("_ostatní") || String.IsNullOrEmpty(textBoxSlozka.Text))
                    {
                        ukladaneVideo.NazevNovy = ukladaneVideo.NazevInterpretSkladbaFeat;
                    }
                    else
                    {
                        ukladaneVideo.NazevNovy = ukladaneVideo.NazevSkladbaFeat;
                    }
                }
                /*string novyNazev = "";
                if (ukladanaSlozka.ToLower().Contains("_ostatní"))
                {
                    novyNazev = ukladaneVideo.Interpret + "-";
                }
                novyNazev += ukladaneVideo.Skladba;

                if (!String.IsNullOrEmpty(ukladaneVideo.InterpretiFeat))
                {
                    novyNazev += " (ft. " + ukladaneVideo.InterpretiFeat + ")";
                }
                ukladaneVideo.NazevNovy = novyNazev;*/

                ukladaneVideo.Chyba = "";// ?
            }
            ukladaneVideo.Slozka = ukladanaSlozka;
        }

        /// <summary>
        /// Zaškrtnutí CheckBoxů se změnou i u dalších vybraných videí (všech, od interpreta, z playlistu)
        /// </summary>
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

        private void label7_Click(object sender, EventArgs e)
        {
            // otevření stránky videa klasicky
            geckoWebBrowserVideo.Navigate("https://www.youtube.com/watch?v=" + upravovaneVideo.ID);
        }

        private void linkLabelPlaylist_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("https://www.youtube.com/playlist?list=" + upravovaneVideo.PlaylistVidea.ID);
        }

        private void checkBoxNovyNazevAutomaticky_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxNovyNazevAutomaticky.Checked)
            {
                Zmena();
                ZmenaNazvu();
            }
        }

        private void buttonProhodit_Click(object sender, EventArgs e)
        {
            string skladba = textBoxSkladba.Text;
            textBoxSkladba.Text = textBoxInterpret.Text;
            textBoxInterpret.Text = skladba;
            string stav = upravovaneVideo.Stav;
            // uloží aktuální úpravu
            checkBoxUlozit.Checked = true;
            Ulozit();
            upravovaneVideo.Chyba = "";
            // najde automaticky složku a upraví některé informace
            upravovaneVideo.NajdiSlozku();
            upravovaneVideo.Stav = stav;
            // načte změny
            Nacist(true);
        }

        private void buttonNeexistujiciVyhledat_Click(object sender, EventArgs e)
        {
            Process.Start("https://www.google.com/search?q=" + upravovaneVideo.ID);
        }

        private void buttonPuvodniNazev_Click(object sender, EventArgs e)
        {
            textBoxSkladba.Text = textBoxPuvodniNazev.Text;
        }
    }
}