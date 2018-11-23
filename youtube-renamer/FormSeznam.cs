using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace hudba
{
    public partial class FormSeznam : Form
    {
        /**** PROMĚNNÉ ****/

        string hudebniKnihovna = null;
        string cestaYoutubeDL = null;
        string cestaFFmpeg = null;
        string slozkaProgramuData = null;
        string slozkaProgramuCache = null;

        string youtubeID = null;
        List<Video> videaVsechna = new List<Video>();

        public FormSeznam()
        {
            InitializeComponent();
        }

        /**** OSTATNÍ *****/

        // získá vybraná videa
        private List<Video> ZiskejVybranaVidea(bool pouzeOK)
        {
            List<Video> videaVybrana = new List<Video>();
            foreach (Video videoSeznam in videaVsechna)
            {
                foreach (ListViewItem videoListView in listViewSeznam.Items)
                {
                    if (videoListView.Checked)
                    {
                        if (pouzeOK)
                        {
                            if (videoListView.Group.Header != "Složka nalezena")
                            {
                                continue;
                            }
                        }

                        if (videoSeznam.id == videoListView.SubItems[0].Text)
                        {
                            videaVybrana.Add(videoSeznam);
                        }
                    }
                }
            }
            return videaVybrana;
        }

        /**** DODĚLAT **** LABELY ****/
        #region labely
        // zobrazí zprávu na labelu
        private void ZobrazNaLabelu(string text1)
        {
            labelInfo.Invoke(new Action(() =>
            {
                listViewSeznam.Invoke(new Action(() =>
                {
                    ZobrazLabel(true);
                }
                ));
                if (text1 != null)
                {
                    labelInfo.Text = text1;
                }
                labelInfo.Tag = "";
            }
            ));
        }
        private void ZobrazNaLabelu(string text1, string tag)
        {
            labelInfo.Invoke(new Action(() =>
            {
                listViewSeznam.Invoke(new Action(() =>
                {
                    ZobrazLabel(true);
                }
                ));
                if (text1 != null)
                {
                    labelInfo.Text = text1;
                }
                if (tag != null)
                {
                    labelInfo.Tag = tag;
                }
            }
            ));
        }
        private void ZobrazNaLabelu(string text1, string text2, string tag)
        {
            ZobrazNaLabelu(text1, tag);
            labelInfo.Invoke(new Action(() =>
            {
                if (text2 != null)
                {
                    labelInfo.Text += Environment.NewLine + Environment.NewLine;
                    labelInfo.Text += text2;
                }
            }
            ));
        }
        private void ZobrazNaLabelu(string text1, string text2, string text3, string tag)
        {
            ZobrazNaLabelu(text1, text2, tag);
            labelInfo.Invoke(new Action(() =>
            {
                if (text3 != null)
                {
                    labelInfo.Text += Environment.NewLine + Environment.NewLine;
                    labelInfo.Text += text3;
                }
            }
            ));
        }
        private void ZobrazNaLabelu(string text1, string text2, string text3, string text4, string tag)
        {
            ZobrazNaLabelu(text1, text2, text3, tag);
            labelInfo.Invoke(new Action(() =>
            {
                if (text4 != null)
                {
                    labelInfo.Text += Environment.NewLine + Environment.NewLine;
                    labelInfo.Text += text4;
                }
            }
            ));
        }

        // zobrazí / skryje label
        private void ZobrazLabel(bool zobrazit)
        {
            if (zobrazit == true)
            {
                labelInfo.Visible = true;
                listViewSeznam.Visible = false;
            }
            else
            {
                ZobrazNaLabelu("");
                labelInfo.Visible = false;
                listViewSeznam.Visible = true;
            }
        }

        // DODĚLAT
        // kliknutí na label
        private void labelInfo_Click(object sender, EventArgs e)
        {
            string labelTag = labelInfo.Tag.ToString();
            switch (labelTag)
            {
                case "ukoncit_program":
                    this.Close();
                    break;

                case "hudebni_knihovna":
                    // vybrat složku s hudební knihovnou
                    HudebniKnihovnaVyber();
                    break;
                case "odkaz":
                    // vybrat všechen text v texBoxu
                    textBoxOdkaz_Click(null, null);
                    break;
                case "odkaz_vloz":
                    // vlož odkaz z paměti
                    textBoxOdkaz.Text = Clipboard.GetText();
                    textBoxOdkaz_TextChanged(null, null);
                    break;
                case "odkaz_ok":
                    // přidat video / videa z playlistu z youtube
                    menuPridatVideoNeboPlaylist_Click(null, null);
                    break;
                case "restart":
                    // chyba - restartuj program
                    System.Diagnostics.Process.Start(Application.ExecutablePath);
                    this.Close();
                    break;
                case "stahuji":
                    // zastav stahování
                    backgroundWorkerStahniVideo.CancelAsync();
                    break;
                case "stazeno_ok":
                    // stahování videa - úspěšně staženo
                    ZobrazLabel(false);
                    break;
                case "slozky_ok":
                    // ok
                    if (listViewSeznam.Items.Count == 0)
                    {
                        ZobrazNaLabelu("Změna složky:", "Vybraná složka s hudební knihovnou: " + Path.GetFileNameWithoutExtension(hudebniKnihovna), "VLOŽIT ODKAZ NA VIDEO NEBO PLAYLIST Z YOUTUBE", "odkaz");
                    }
                    else
                    {
                        ZobrazLabel(false);
                    }
                    break;
                case "stazeno_stormo":
                    // DODĚLAT !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
                    // stahování videa - zrušeno
                    break;
                case "stazeno_chyba":
                    // DODĚLAT !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
                    // stahování videa - chyba
                    break;
                default:
                    break;
            }
        }
#endregion

        // zobrazí počet vybraných videí ke stažení a povoluje nebo zakazuje úpravu
        private void listViewSeznam_ItemChecked(object sender, ItemCheckedEventArgs e)
        {
            labelStatus.Text = "Vybraných " + listViewSeznam.CheckedItems.Count.ToString() + " videí z(e) " + listViewSeznam.Items.Count.ToString();
            if (listViewSeznam.CheckedItems.Count > 0)
            {
                menuUpravit.Enabled = true;
                menuStahnout.Enabled = true;
                menuOdstranit.Enabled = true;
            }
            else
            {
                menuUpravit.Enabled = false;
                menuStahnout.Enabled = false;
                menuOdstranit.Enabled = false;
            }
            if (listViewSeznam.Items.Count > 0)
            {
                menuNastaveni.Enabled = false;
            }
            else
            {
                menuNastaveni.Enabled = true;
            }
        }

        /**** DODĚLAT **** MENU - FILTR SOUBORŮ ****/

        #region vyber souboru
        // všechny soubory
        private void menuVybratVse_Click(object sender, EventArgs e)
        {
            listViewSeznam.BeginUpdate();
            foreach (ListViewItem polozka in listViewSeznam.Items)
            {
                if (!polozka.Checked)
                {
                    polozka.Checked = true;
                }
            }
            listViewSeznam.EndUpdate();
        }

        //UPRAVIT pouze soubory, které se podařilo přejmenovat
        private void filtrSouboruOkMenu_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem polozka in listViewSeznam.Items)
            {
                polozka.Checked = false;
                if (polozka.Group.Header == "Složka nalezena")
                {
                    polozka.Checked = true;
                }
            }
        }

        //UPRAVIT pouze soubory, které se nepodařilo přejmenovat
        private void filtrSouboruNeMenu_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem polozka in listViewSeznam.Items)
            {
                polozka.Checked = false;
                if (polozka.Group.Header == "Následující videa se nepodařilo přejmenovat")
                {
                    polozka.Checked = true;
                }
            }
        }

        //ok obrátit výběr
        private void menuVybratObratit_Click(object sender, EventArgs e)
        {
            listViewSeznam.BeginUpdate();
            foreach (ListViewItem polozka in listViewSeznam.Items)
            {
                if (polozka.Checked)
                {
                    polozka.Checked = false;
                }
                else
                {
                    polozka.Checked = true;
                }
            }
            listViewSeznam.EndUpdate();
        }

        //ok zrušit výběr
        private void menuVybratZrusit_Click(object sender, EventArgs e)
        {
            listViewSeznam.BeginUpdate();
            foreach (ListViewItem polozka in listViewSeznam.Items)
            {
                polozka.Checked = false;
            }
            listViewSeznam.EndUpdate();
        }
        #endregion

        /**** MENU - UPRAVIT NOVÝ NÁZEV SOUBORŮ ****/

        // zobrazí form s úpravou a předá položky v listView1
        private void menuUpravit_Click(object sender, EventArgs e)
        {
            List<Video> videa = ZiskejVybranaVidea(false);
            ListViewItem[] polozky = new ListViewItem[listViewSeznam.Items.Count];
            listViewSeznam.Items.CopyTo(polozky, 0);
            FormUprava upravaForm = new FormUprava(videa, hudebniKnihovna, polozky, listViewSeznam.Groups);
            upravaForm.ShowDialog();
        }


        /**** MENU - STÁHNOUT ****/

        // menu - stáhnout video
        private void menuStahnout_Click(object sender, EventArgs e)
        {
            List<Video> videa = ZiskejVybranaVidea(true);

            ZobrazNaLabelu("Stahuji videa:", "stahuji");
            labelStatus.Text = "Stahuji videa...";

            progressBarStatus.Maximum = videa.Count + 1;
            progressBarStatus.Visible = true;

            if (!backgroundWorkerStahniVideo.IsBusy)
            {
                menuStripMenu.Visible = false;
                backgroundWorkerStahniVideo.RunWorkerAsync(videa);
            }
            else
            {
                ZobrazNaLabelu("Stahuji videa:", "Chyba - stahování videa", "Zkuste stáhnout videa znovu.", "stazeno_chyba");
            }
        }

        // stáhne video
        private void backgroundWorkerStahniVideo_DoWork(object sender, DoWorkEventArgs e)
        {
            if (backgroundWorkerStahniVideo.CancellationPending)
            {
                e.Cancel = true;
                return;
            }
            List<Video> videa = (List<Video>)e.Argument;
            if (videa.Count <= 0)
            {
                return;
            }
            // postupně stáhne vybraná videa
            foreach (Video vid in videa)
            {
                string adresaVidea = "https://youtu.be/" + vid.id;
                string nazev = vid.nazevNovy;
                Process cmd = new Process();
                ProcessStartInfo psi = new ProcessStartInfo();

                // zobrazí aktuální číslo stahovaného videa a aktuální stahované video
                backgroundWorkerStahniVideo.ReportProgress(videa.IndexOf(vid));
                int index = videa.IndexOf(vid) + 1;
                ZobrazNaLabelu("Stahuji video (" + index + " z(e) " + videa.Count + "):", vid.interpret + "-" + vid.skladbaFeaturing, "stahuji");

                // nastaví vlastnosti programu na stažení
                psi.Arguments = "-x -i -w  --audio-quality 0 --audio-format mp3 -o \"" + nazev + ".%(ext)s\" \"" + adresaVidea + "\""; // -U = update
                psi.CreateNoWindow = true;
                psi.ErrorDialog = true;
                psi.FileName = Path.Combine(cestaYoutubeDL, "youtube-dl");
                psi.RedirectStandardInput = true;
                psi.RedirectStandardOutput = true;
                psi.UseShellExecute = false;
                psi.WorkingDirectory = Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), "stazene");

                // spustí program na stažení
                cmd.OutputDataReceived += new DataReceivedEventHandler(OutputHandler);
                cmd.ErrorDataReceived += new DataReceivedEventHandler(ErrorHandler);
                cmd.StartInfo = psi;
                cmd.SynchronizingObject = labelInfo;
                cmd.Start();
                cmd.BeginOutputReadLine();
                cmd.WaitForExit();
                e.Result += Environment.NewLine + vid.interpret + "-" + vid.skladba;

                // uloží metadata do souboru a přesune soubor
                UlozMetadata(vid);
                PresunSoubor(vid);
            }
        }

        // zobrazí na progressBaru počet již stažených videí
        private void backgroundWorkerStahniVideo_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            listBox1.Items.Clear();
            progressBarStatus.Value = e.ProgressPercentage;
        }

        private void backgroundWorkerStahniVideo_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            progressBarStatus.Value = progressBarStatus.Maximum;
            if (e.Cancelled)
            {
                ZobrazNaLabelu("Stahuji videa:", "Zrušili jste práci", "stazeno_stormo");
            }
            else if (e.Error != null)
            {
                ZobrazNaLabelu("Stahuji videa:", "Chyba - stahování videa", e.Error.ToString(), "Zkuste stáhnout videa znovu.", "stazeno_chyba");
            }
            else
            {
                ZobrazNaLabelu("Následující videa byla úspěšně stažena:", e.Result.ToString(), "OK", "stazeno_ok");
                labelStatus.Text = "Připraven";
                progressBarStatus.Visible = false;
            }
            listBox1.Visible = false;
            menuStripMenu.Visible = true;
        }

        private void OutputHandler(Object source, DataReceivedEventArgs outLine)
        {
            if (!String.IsNullOrEmpty(outLine.Data))
            {
                listBox1.Visible = true;
                listBox1.Items.Add(outLine.Data);
            }
        }

        private void ErrorHandler(Object source, DataReceivedEventArgs outLine)
        {
            if (!String.IsNullOrEmpty(outLine.Data))
            {
                listBox1.Visible = true;
                listBox1.Items.Add(outLine.Data);
            }
        }

        // uloží metadata do souboru
        private void UlozMetadata(Video vid)
        {
            try
            {
                TagLib.File soubor = TagLib.File.Create(Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), "stazene", vid.nazevNovy + ".mp3"));
                soubor.Tag.Year = Convert.ToUInt32(String.Format("{0:yyyy}", vid.publikovano));
                soubor.Tag.Performers = new string[] { vid.interpret };
                soubor.Tag.Title = vid.skladbaFeaturing;
                soubor.Tag.Genres = new string[] { vid.zanr };
                soubor.Save();
            }
            catch (Exception ex)
            {
                ZobrazNaLabelu("Stahuji videa:", "Chyba - přidávání metadat do souboru", ex.ToString(), "Zkuste stáhnout videa znovu.", "stazeno_chyba");
                MessageBox.Show("Test");
            }
        }

        private void PresunSoubor(Video vid)
        {
            try
            {
                File.Move(Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), "stazene", vid.nazevNovy + ".mp3"),
                            Path.Combine(vid.slozka, vid.nazevNovy + ".mp3"));
                vid.skupina = "Staženo";
            }
            catch (Exception ex)
            {
                ZobrazNaLabelu("Stahuji videa:", "Chyba - přesunování souboru", ex.ToString(), "Zkuste stáhnout videa znovu.", "stazeno_chyba");
                MessageBox.Show("Test");
            }
        }

        /**** MENU - ODSTRANIT ****/

        // ostraní vybrané videa
        private void menuOdstranit_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem polozkaListView in listViewSeznam.CheckedItems)
            {
                for (int i = 0; i < videaVsechna.Count; i++)
                {
                    if (polozkaListView.SubItems[0].Text == videaVsechna[i].id)
                    {
                        videaVsechna.Remove(videaVsechna[i]);
                        i--;
                    }
                }
                polozkaListView.Checked = false;
                listViewSeznam.Items.Remove(polozkaListView);
            }
        }

        /**** TESTOVÁNÍ ****/

        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {
            textBoxOdkaz.Text = "https://www.youtube.com/playlist?list=PLTFujRZGqO1Zxe-AFgLWX4esfmBVOm1-2"; //2018.11 us & others
        }
        private void toolStripMenuItem4_Click(object sender, EventArgs e)
        {
            textBoxOdkaz.Text = "https://www.youtube.com/playlist?list=PLTFujRZGqO1b6j6ZP8glz9s8iqHi1v-KD!"; // 2017.04 us & others
        }
        private void toolStripMenuItem5_Click(object sender, EventArgs e)
        {
            textBoxOdkaz.Text = "https://www.youtube.com/watch?v=6YJcaefdvao"; // video
        }
        private void toolStripMenuItem6_Click(object sender, EventArgs e)
        {
            textBoxOdkaz.Text = "https://www.youtube.com/playlist?list=PLTFujRZGqO1aDbeKJy6MFAbihUXQPmzVZ"; // 2018.11 cz
        }
        private void toolStripMenuItem7_Click(object sender, EventArgs e)
        {
            textBoxOdkaz.Text = "https://www.youtube.com/playlist?list=PLTFujRZGqO1Z07yHjt9YtHRcK6M7tVSC6"; // 2018.10 us & others        
        }
        private void toolStripMenuItem8_Click(object sender, EventArgs e)
        {
            textBoxOdkaz.Text = "https://www.youtube.com/playlist?list=PLTFujRZGqO1bLKMsXFkozb0tJNg-t3g4m"; // ZKOUŠKA
        }

        /*******************************************************************************************************/
        /*******************************************************************************************************/
        /*******************************************************************************************************/
        /*******************************************************************************************************/
        /*******************************************************************************************************/
        /*******************************************************************************************************/
        /*******************************************************************************************************/

        private void ZobrazitChybuSpousteni(string chyba)
        {
            ZobrazNaLabelu("Spouštění programu:", "Chyba - " + chyba, "", "Spusťte prosím program znovu.", "restart");
            statusStripStatus.Invoke(new Action(() =>
            {
                labelStatus.Text = "Chyba - spouštění programu";
            }));

            /* POZOR - CHYBA !!!!!!!!!!!!!! */
            //// -> progressBarStatus.Visible = false;
        }


        // vytvoření složek, stažení youtube-dl + ffmpeg
        private void backgroundWorkerNactiProgram_DoWork(object sender, DoWorkEventArgs e)
        {
            slozkaProgramuData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            if (!Directory.Exists(slozkaProgramuData))
            {
                ZobrazitChybuSpousteni("získání složky programu");
            }
            slozkaProgramuData = Path.Combine(slozkaProgramuData, "youtube-renamer");
            slozkaProgramuCache = Path.Combine(slozkaProgramuData, "cache", Process.GetCurrentProcess().Id.ToString());
            slozkaProgramuData = Path.Combine(slozkaProgramuData, "data");

            //ZobrazNaLabelu("Spouštění programu:", "Vytvářím složky...", "UKONČIT PROGRAM", "ukoncit_program");
            // -> DOČASNĚ
            backgroundWorkerNactiProgram.ReportProgress(1);

            // 1. složka data = nastavení
            if (!Directory.Exists(slozkaProgramuData))
            {
                try
                {
                    Directory.CreateDirectory(slozkaProgramuData);
                }
                catch (Exception)
                {
                    ZobrazitChybuSpousteni("Vytváření data složky");
                }
            }

            // 2. složka cache = dočasné stažené soubory
            if (Directory.Exists(slozkaProgramuCache))
            {
                try
                {
                    Directory.Delete(slozkaProgramuCache, true);
                }
                catch (Exception)
                {
                    ZobrazitChybuSpousteni("Mazání cache složky");
                }
            }
            try
            {
                Directory.CreateDirectory(slozkaProgramuCache);
            }
            catch (Exception)
            {
                ZobrazitChybuSpousteni("Vytváření cache složky");
            }
            // 3. načtení nastavení
            menuStripMenu.Invoke(new Action(() =>
            {
                // a) cesta hudebních složek
                MenuCestaNactiZeSouboru(0);
                // b) cesta youtube-dl
                MenuCestaNactiZeSouboru(1);
                // c) cesta ffmpeg
                MenuCestaNactiZeSouboru(2);
            }));

            // 4. prohledá hudební knihovnu a uloží seznam do souboru
            //HudebniKnihovnaNajdiSlozky(true);
            // -> DOČASNĚ           
        }

        // zobrazuje proces na processBar
        private void backgroundWorkerNactiProgram_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressBarStatus.Value = e.ProgressPercentage;
        }


        // přidání složek a ukončení backgroundworkeru
        private void backgroundWorkerNactiProgram_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            progressBarStatus.Value = progressBarStatus.Maximum;
            if (e.Error != null)
            {
                ZobrazitChybuSpousteni("spouštění programu");
                MessageBox.Show("Chyba - spouštění programu" + Environment.NewLine + e.Error, "Spouštění programu - chyba", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                labelStatus.Text = "Připraven";
                menuStripMenu.Visible = true;
                menuNastaveni.Enabled = true;
                progressBarStatus.Visible = false;
                this.FormBorderStyle = FormBorderStyle.Sizable;
            }
        }
        /**** START ****/

        // start programu, načtení souborů, kontrola, youtube-dl
        private void FormSeznam_Load(object sender, EventArgs e)
        {
            this.FormBorderStyle = FormBorderStyle.FixedToolWindow;
            menuStripMenu.Visible = false;

            //
            textBoxOdkaz.Enabled = true;
            // -> DOČASNĚ

            /*ZobrazNaLabelu("Spouštění programu:", "Příprava spouštění programu", "UKONČIT PROGRAM", "ukoncit_program");
            labelStatus.Text = "Spouštím program...";

            progressBarStatus.Maximum = 6;
            progressBarStatus.Visible = true;*/
            // -> DOČASNĚ

            if (!backgroundWorkerNactiProgram.IsBusy)
            {
                backgroundWorkerNactiProgram.RunWorkerAsync();
            }
            else
            {
                ZobrazitChybuSpousteni("spouštění programu");
            }
        }

        // ok_new MENU - NASTAVENÍ - CESTY - otevření cest v průzkůmníku souborů

        private void menuNastaveniKnihovnaVybrana_Click(object sender, EventArgs e)
        {
            // otevře složku s hudební knihovnou v průzkumníku souborů
            if (hudebniKnihovna == null)
            {
                var menu = sender as ToolStripMenuItem;
                hudebniKnihovna = menu.Text;
            }
            if (!Directory.Exists(hudebniKnihovna))
            {
                menuNastaveniYoutubeDLCestaVybrana.Text = "Nebyla vybrána žádná složka";
                menuNastaveniYoutubeDLCestaVybrana.Enabled = false;
                hudebniKnihovna = null;
                MessageBox.Show("složka neexistuje");
                return;
            }
            Process.Start(hudebniKnihovna);
        }
        private void menuNastaveniCestaYoutubeDLVybrana_Click(object sender, EventArgs e)
        {
            // otevře složku s cestou souboru youtube-dl v průzkumníku souborů a vybere soubor
            if (cestaYoutubeDL == null)
            {
                var menu = sender as ToolStripMenuItem;
                cestaYoutubeDL = menu.Text;
            }
            if (!File.Exists(cestaYoutubeDL))
            {
                menuNastaveniYoutubeDLCestaVybrana.Text = "Není vybrána žádná cesta";
                menuNastaveniYoutubeDLCestaVybrana.Enabled = false;
                cestaYoutubeDL = null;
                MessageBox.Show("soubor neexistuje");
                return;
            }

            Process.Start("explorer", "/select, \"" + cestaFFmpeg + "\"");
        }
        private void menuNastaveniCestaFFmpegVybrana_Click(object sender, EventArgs e)
        {
            // otevře složku s cestou souboru ffmpeg v průzkumníku souborů a vybere soubor
            if (cestaFFmpeg == null)
            {
                var menu = sender as ToolStripMenuItem;
                cestaFFmpeg = menu.Text;
            }
            if (!File.Exists(cestaFFmpeg))
            {
                // DODĚLAT ????????????
                // -> i tak zůstane zaškrtnutá v seznamu naposledy vybraných, ale nelze vybrat, takže nemůže dojít k problému

                menuNastaveniFFmpegCestaVybrana.Text = "Není vybrána žádná cesta";
                menuNastaveniFFmpegCestaVybrana.Enabled = false;
                cestaFFmpeg = null;
                MessageBox.Show("soubor neexistuje");
                return;
            }

            Process.Start("explorer", "/select, \"" + cestaFFmpeg + "\"");
        }

        /*******************************************************************************************************/
        /*******************************************************************************************************/
        /*******************************************************************************************************/
        /*******************************************************************************************************/
        /*******************************************************************************************************/
        /*******************************************************************************************************/
        /*******************************************************************************************************/

        // HOTOVO
        /**
        NASTAVENÍ SLOŽKY S HUDEBNÍ KNIHOVNOU NEBO CESTY S YOUTUBE-DL NEBO FFMPEG - vybrání nové složky
        **/

        // HOTOVO
        private void menuNastaveniKnihovnaZmenit_Click(object sender, EventArgs e)
        {
            // hudební knihovna - uživatel vybere novou cestu složky
            HudebniKnihovnaVyber();
        }
        // HOTOVO
        /// <summary>
        /// Uživatel vybere pomocí FolderBrowserDialogu novou složku s hudební knihovnou.
        /// Ta se zobrazí v menu a automaticky se prohledají složky v nové hudební knihovně.
        /// </summary>
        private void HudebniKnihovnaVyber()
        {
            // hudební knihovna - uživatel vybere novou cestu složky
            FolderBrowserDialog vyberSlozky = new FolderBrowserDialog();
            vyberSlozky.Description = "Vyberte složku s hudební knihovnou:";
            vyberSlozky.ShowNewFolderButton = false;
            if (vyberSlozky.ShowDialog() == DialogResult.OK)
            {
                hudebniKnihovna = vyberSlozky.SelectedPath;
                MenuCestaZobrazit(0);
                ZobrazStatusLabel("Změna hudební knihovny", "Hudební knihovna úspěšně změněna: '" + hudebniKnihovna + "'");
            }
        }
        // HOTOVO
        private void menuNastaveniCestaYoutubeDLZmenit_Click(object sender, EventArgs e)
        {
            // youtube-dl - uživatel vybere cestu pomocí OpenFileDialogu, ta se zobrazí v menu

            OpenFileDialog vyberSouboru = new OpenFileDialog();
            vyberSouboru.Title = "Vyberte spustitelný soubor YoutubeDL:";
            vyberSouboru.Filter = "Spustitelné soubory (*.exe)|*.exe|Všechny soubory (*.*)|*.*";
            if (vyberSouboru.ShowDialog() == DialogResult.OK)
            {
                if (vyberSouboru.FilterIndex == 2)
                {
                    // nejedná se o exe soubor
                    ZobrazUpozorneni("Změna cesty YouTube-DL", "Nejedná se o spustitelý soubor (*.exe)!", "Program nemusí fungovat správně.");
                }
                cestaYoutubeDL = vyberSouboru.FileName;
                MenuCestaZobrazit(1);
                ZobrazStatusLabel("Změna cesty YouTube-DL", "Cesta úspěšně změněna: '" + cestaYoutubeDL + "'");
            }
        }
        // HOTOVO
        private void menuNastaveniCestaFFmpegZmenit_Click(object sender, EventArgs e)
        {
            // ffmpeg - uživatel vybere cestu pomocí OpenFileDialogu, ta se zobrazí v menu
            OpenFileDialog vyberSouboru = new OpenFileDialog();
            vyberSouboru.Title = "Vyberte spustitelný soubor FFmpeg:";
            vyberSouboru.Filter = "Spustitelné soubory (*.exe)|*.exe|Všechny soubory (*.*)|*.*";
            if (vyberSouboru.ShowDialog() == DialogResult.OK)
            {
                if (vyberSouboru.FilterIndex == 2)
                {
                    // nejedná se o exe soubor
                    ZobrazUpozorneni("Změna cesty FFmpeg", "Nejedná se o spustitelý soubor (*.exe)!", "Program nemusí fungovat správně.");
                }
                cestaFFmpeg = vyberSouboru.FileName;
                MenuCestaZobrazit(2);
                ZobrazStatusLabel("Změna cesty FFmpeg", "Cesta úspěšně změněna: '" + cestaFFmpeg + "'");
            }
        }



        // HOTOVO
        /**
        NASTAVENÍ SLOŽKY S HUDEBNÍ KNIHOVNOU NEBO CESTY S YOUTUBE-DL NEBO FFMPEG - vybrání již dříve vybraných cest z menu
        **/

        // HOTOVO
        private void menuNastaveniKnihovnaNaposledyVybrane_Click(object sender, EventArgs e)
        {
            // hudební knihovna - vybrere složku z již dříve vybraných složek a zobrazí ji v menu
            var menu = sender as ToolStripMenuItem;
            if (Directory.Exists(menu.Text))
            {
                hudebniKnihovna = menu.Text;
                MenuCestaZobrazit(0);
                ZobrazStatusLabel("Změna hudební knihovny", "Hudební knihovna úspěšně změněna: '" + hudebniKnihovna + "'");
            }
            else
            {
                ZobrazStatusLabel("Změna hudební knihovny", "Hudební knihovna nemohla být změněna. Složka '" + menu.Text + "' neexistuje.");
                ZobrazChybu("Změna hudební knihovny", "Hudební knihovna nemohla být změněna.", "Složka '" + menu.Text + "' neexistuje.", "Zkuste to prosím znovu");
            }
        }
        // HOTOVO
        private void menuNastaveniCestaYoutubeDLNaposledyVybrane_Click(object sender, EventArgs e)
        {
            // youtube-dl - vybrere cestu z již dříve vybraných složek a zobrazí ji v menu
            var menu = sender as ToolStripMenuItem;
            if (File.Exists(menu.Text))
            {
                cestaYoutubeDL = menu.Text;
                MenuCestaZobrazit(1);
                ZobrazStatusLabel("Změna cesty YouTube-DL", "Cesta úspěšně změněna: '" + cestaYoutubeDL + "'");
            }
            else
            {
                ZobrazStatusLabel("Změna cesty YouTube-DL", "Cesta nemohla být změněna. Soubor '" + menu.Text + "' neexistuje.");
                ZobrazChybu("Změna cesty YouTube-DL", "Cesta nemohla být změněna.", "Soubor '" + menu.Text + "' neexistuje.", "Zkuste to prosím znovu");
            }
        }
        // HOTOVO
        private void menuNastaveniCestaFFmpegNaposledyVybrane_Click(object sender, EventArgs e)
        {
            // ffmpeg - vybrere cestu z již dříve vybraných složek a zobrazí ji v menu
            var menu = sender as ToolStripMenuItem;
            if (File.Exists(menu.Text))
            {
                cestaFFmpeg = menu.Text;
                MenuCestaZobrazit(2);
                ZobrazStatusLabel("Změna cesty FFmpeg", "Cesta úspěšně změněna: '" + cestaFFmpeg + "'");
            }
            else
            {
                ZobrazStatusLabel("Změna cesty FFmpeg", "Cesta nemohla být změněna. Soubor '" + menu.Text + "' neexistuje.");
                ZobrazChybu("Změna cesty FFmpeg", "Cesta nemohla být změněna.", "Soubor '" + menu.Text + "' neexistuje.", "Zkuste to prosím znovu");
            }
        }


        // HOTOVO
        /**
        NASTAVENÍ SLOŽKY S HUDEBNÍ KNIHOVNOU NEBO CESTY S YOUTUBE-DL NEBO FFMPEG - ostatní funkce
        **/

        // HOTOVO
        /// <summary>
        /// Načte ze souboru (cesta dle typu) naposledy vybrané cesty (knihovny, youtube-dl, ffmpeg).
        /// Poslední cestu následně uloží jako výchozí.
        /// A přidá cesty do menu.
        /// </summary>
        /// <param name="typ">Typ cesty (0 = složky knihovny, 1 = cesty youtube-dl, 2 = cesty ffmpeg)</param>
        private void MenuCestaNactiZeSouboru(int typ)
        {
            // 0 = složky knihovny
            // 1 = cesty youtube-dl
            // 2 = cesty ffmpeg

            // získá cestu souboru
            string cestaSouboru = null;
            if      (typ == 0) cestaSouboru = Path.Combine(slozkaProgramuData, "knihovna_historie.txt");
            else if (typ == 1) cestaSouboru = Path.Combine(slozkaProgramuData, "youtubedl.txt");
            else if (typ == 2) cestaSouboru = Path.Combine(slozkaProgramuData, "ffmpeg.txt");

            // načte ze souboru cesty
            Soubory soubor = new Soubory();
            List<string> pridejCesty = soubor.Precti(cestaSouboru);
            if (pridejCesty == null)    return;
            if (pridejCesty.Count == 0) return;

            // projde cesty a přidá je do menu
            foreach (string cesta in pridejCesty)
            {
                MenuCestaPridat(cesta.Trim(), typ);
            }

            // uloží první řádek ze souboru jako výchozí cestu
            if (typ == 0 && Directory.Exists(pridejCesty.First().Trim()))
            {
                hudebniKnihovna = pridejCesty.First().Trim();
                MenuCestaZobrazit(typ);
            }
            else if (File.Exists(pridejCesty.First().Trim()))
            {
                if (typ == 1) cestaYoutubeDL = pridejCesty.First().Trim();
                else if (typ == 2) cestaFFmpeg = pridejCesty.First().Trim();
                MenuCestaZobrazit(typ);
            }
        }

        // HOTOVO
        /// <summary>
        /// Přidá danou cestu do menu.
        /// </summary>
        /// <param name="cesta">Cesta, kterou chceme přidat do menu.</param>
        /// <param name="typ">Typ cesty (0 = složky knihovny, 1 = cesty youtube-dl, 2 = cesty ffmpeg)</param>
        private void MenuCestaPridat(string cesta, int typ)
        {
            // 0 = složky knihovny
            // 1 = cesty youtube-dl
            // 2 = cesty ffmpeg

            // pokud se nejdená o existující složku / soubor, neuloží se do menu
            if (typ == 0 && !Directory.Exists(cesta)) return;
            else if ((typ == 1 || typ == 2) && !File.Exists(cesta)) return;
            // nastavení počátečních proměnných
            ToolStripMenuItem menuPridavane = new ToolStripMenuItem(cesta);
            menuPridavane.Text = cesta;

            // přidá dle typů cestu do menu
            if (typ == 0)
            {
                if (menuPridavane.Text == hudebniKnihovna)
                {
                    menuPridavane.Checked = true;
                }
                else
                {
                    menuPridavane.Checked = false;
                }
                menuPridavane.ToolTipText = "Vybrat složku '" + cesta + "'";
                menuPridavane.Click += new EventHandler(menuNastaveniKnihovnaNaposledyVybrane_Click);
                menuNastaveniKnihovnaNaposledyVybrane.DropDownItems.Add(menuPridavane);
                menuNastaveniKnihovnaNaposledyVybrane.Text = "Naposledy vybrané složky";
                menuNastaveniKnihovnaNaposledyVybrane.Enabled = true;
                return;
            }

            menuPridavane.ToolTipText = "Vybrat soubor '" + cesta + "'";

            if (typ == 1)
            {
                if (menuPridavane.Text == cestaYoutubeDL)
                {
                    menuPridavane.Checked = true;
                }
                else
                {
                    menuPridavane.Checked = false;
                }
                menuPridavane.Click += new EventHandler(menuNastaveniCestaYoutubeDLNaposledyVybrane_Click);
                menuNastaveniYoutubeDLCestaNaposledyVybrane.DropDownItems.Add(menuPridavane);
                menuNastaveniYoutubeDLCestaNaposledyVybrane.Text = "Naposledy vybrané soubory";
                menuNastaveniYoutubeDLCestaNaposledyVybrane.Enabled = true;
                return;
            }

            if (typ == 2)
            {
                if (menuPridavane.Text == cestaFFmpeg)
                {
                    menuPridavane.Checked = true;
                }
                else
                {
                    menuPridavane.Checked = false;
                }
                menuPridavane.Click += new EventHandler(menuNastaveniCestaFFmpegNaposledyVybrane_Click);
                menuNastaveniFFmpegCestaNaposledyVybrane.DropDownItems.Add(menuPridavane);
                menuNastaveniFFmpegCestaNaposledyVybrane.Text = "Naposledy vybrané soubory";
                menuNastaveniFFmpegCestaNaposledyVybrane.Enabled = true;
                return;
            }
        }
        // HOTOVO
        /// <summary>
        /// Zaškrtne nebo odškrtne cestu v menu. Pokud tam není, tak jí přidá a zobrazí cestu v menu.
        /// </summary>
        /// <param name="typ">Typ cesty (0 = složky knihovny, 1 = cesty youtube-dl, 2 = cesty ffmpeg)</param>
        private void MenuCestaZobrazit(int typ)
        {
            // 0 = složky knihovny
            // 1 = cesty youtube-dl
            // 2 = cesty ffmpeg

            // nastavení počátečních proměnných
            string cestaVychozi = null;
            ToolStripMenuItem menuNaposledyVybraneCesty = null;
            ToolStripMenuItem menuVybranaCesta = null;
            if (typ == 0)
            {
                cestaVychozi = hudebniKnihovna;
                menuNaposledyVybraneCesty = menuNastaveniKnihovnaNaposledyVybrane;
                menuVybranaCesta = menuNastaveniKnihovnaVybrana;
            }
            else if (typ == 1)
            {
                cestaVychozi = cestaYoutubeDL;
                menuNaposledyVybraneCesty = menuNastaveniYoutubeDLCestaNaposledyVybrane;
                menuVybranaCesta = menuNastaveniYoutubeDLCestaVybrana;
            }
            else if (typ == 2)
            {
                cestaVychozi = cestaFFmpeg;
                menuNaposledyVybraneCesty = menuNastaveniFFmpegCestaNaposledyVybrane;
                menuVybranaCesta = menuNastaveniFFmpegCestaVybrana;
            }

            bool nalezeno = false;

            // projde cesty v menu
            foreach (ToolStripMenuItem menuCesta in menuNaposledyVybraneCesty.DropDownItems)
            {
                if (menuCesta.Text == cestaVychozi)
                {
                    // cesta byla nalezena v menu, zaškrtnu ji
                    menuCesta.Checked = true;
                    nalezeno = true;
                }
                else
                {
                    menuCesta.Checked = false;
                }
            }
            if (!nalezeno)
            {
                // cesta v menu nenalezena, přidám ji do menu
                MenuCestaPridat(cestaVychozi, typ);
            }
            // nastavení menu s cestou
            menuVybranaCesta.Text = cestaVychozi;
            menuVybranaCesta.Enabled = true;
            if (typ == 0)
            {
                menuVybranaCesta.ToolTipText = "Otevřít složku '" + cestaVychozi + "' v průzkumníku souborů";
                menuNastaveniKnihovnaProhledat.Text = "Prohledat hudební knihovnu";
                menuNastaveniKnihovnaProhledat.Enabled = true;
                HudebniKnihovnaNajdiSlozky();
            }
            else
            {
                menuVybranaCesta.ToolTipText = "Najít soubor '" + cestaVychozi + "' v průzkumníku souborů";
            }
        }

        // HOTOVO
        /**
        NASTAVENÍ SLOŽKY S HUDEBNÍ KNIHOVNOU NEBO CESTY S YOUTUBE-DL NEBO FFMPEG - prohledání hudební knihovny
        **/

        // HOTOVO
        private void menuNastaveniKnihovnaProhledat_Click(object sender, EventArgs e)
        {
            // prohleldá složky s hudební knihovnou
            HudebniKnihovnaNajdiSlozky();
        }
        // HOTOVO
        /// <summary>
        /// Spustí BackgroundWorker s prohledáním všech složek v hudební knihovně.
        /// Ten odstraní složky alb a další přebytečné.
        /// Získá tedy složky interpretů a ty následně uloží do souboru.
        /// </summary>
        private void HudebniKnihovnaNajdiSlozky()
        {
            ZobrazStatusLabel("Prohledávání hudební knihovny...");
            if (!backgroundWorkerProhledejSlozky.IsBusy)
            {
                backgroundWorkerProhledejSlozky.RunWorkerAsync();
            }
            else
            {
                ZobrazChybu("Prohledávání hudební knihovny", "Zkuste prohledat hudební knihovnu znovu.");
            }
        }
        // HOTOVO
        private void backgroundWorkerProhledejSlozky_DoWork(object sender, DoWorkEventArgs e)
        {
            // prohledá složky a zapíše je do souboru
            ZobrazStatusLabel("Prohledávání hudební knihovny", "Probíhá prohledávání složek hudební knihovny.");
            ZobrazStatusProgressBar(1);

            if (hudebniKnihovna == null)
            {
                e.Result = "neexistuje";
                return;
            }
            if (!Directory.Exists(hudebniKnihovna))
            {
                e.Result = "neexistuje";
                return;
            }

            // získá složky z knihovny, krom složek alb
            List<string> slozkyinterpretu = new List<string>();
            try
            {
                foreach (string cestaSlozky in Directory.GetDirectories(hudebniKnihovna, "*.*", SearchOption.AllDirectories))
                {
                    // projde složky ve vybrané hudební knihovně
                    string nazevSlozky = Path.GetFileNameWithoutExtension(cestaSlozky);
                    if (!Regex.IsMatch(nazevSlozky.Split(' ').First(), @"^\d{4}$"))
                    {
                        // pokud se nejedná o album (název je "rok název" = první 4 znaky jsou číslice), přidám do seznamu
                        slozkyinterpretu.Add(cestaSlozky);
                    }
                }
            }
            catch (Exception)
            {
                e.Result = "chyba";
                return;
            }
            
            if (slozkyinterpretu == null)
            {
                e.Result = "zadne_slozky";
                return;
            }
            if (slozkyinterpretu.Count() == 0)
            {
                e.Result = "zadne_slozky";
                return;
            }

            // seřadí složky
            ZobrazStatusProgressBar(slozkyinterpretu.Count() + 1);
            slozkyinterpretu.Sort();

            // odstraní nepotřebné složky
            int pocitadlo = 1;
            for (int i = 1; i < slozkyinterpretu.Count; i++)
            {
                backgroundWorkerProhledejSlozky.ReportProgress(pocitadlo++);
                if (slozkyinterpretu[i].Contains(slozkyinterpretu[i - 1]))
                {
                    // složka má v názvu část předchozí složky
                    if (!Path.GetFileNameWithoutExtension(slozkyinterpretu[i]).Contains(Path.GetFileNameWithoutExtension(slozkyinterpretu[i - 1])))
                    {
                        // název složky a předchozí není stejný, odstraním předchozí složku
                        // tím zajistím neodstranění složek typu "interpret1 & interpret2" po složce "interpret1"
                        slozkyinterpretu.RemoveAt(i - 1);
                        i--;
                    }
                }
            }
            // na 1. pozici vloží aktuální hudební knihovnu
            slozkyinterpretu.Insert(0, hudebniKnihovna);
            // zapíše složky do souboru
            Soubory soubor = new Soubory();
            soubor.Zapis(Path.Combine(slozkaProgramuData, "knihovna_slozky.txt"), slozkyinterpretu);
        }
        // HOTOVO
        private void backgroundWorkerProhledejSlozky_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            // nastaví hodnotu ProgressBaru
            progressBarStatus.Value = e.ProgressPercentage;
        }
        // HOTOVO
        private void backgroundWorkerProhledejSlozky_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            // nastaví maximální hodnotu ProgressBaru
            progressBarStatus.Value = progressBarStatus.Maximum;

            // zobrazení výsledků
            if (e.Error != null)
            {
                ZobrazStatusLabel("Prohledávání hudební knihovny", "Chyba.");
                ZobrazChybu("Prohledávání hudební knihovny", "Došlo k chybě, složka s hudební knihovnou nebyla prohledána.", "Zkuste změnit hudební složku.", e.Error.ToString());
            }
            else if ((string)e.Result == "chyba")
            {
                ZobrazStatusLabel("Prohledávání hudební knihovny", "Chyba.");
                ZobrazChybu("Prohledávání hudební knihovny", "Došlo k chybě, složka s hudební knihovnou nebyla prohledána.", "Zkuste změnit hudební složku.");
            }
            else if ((string)e.Result == "neexistuje")
            {
                ZobrazStatusLabel("Prohledávání hudební knihovny", "Neexistující hudební knihovna.");
                ZobrazChybu("Prohledávání hudební knihovny", "Složka s hudební knihovnou neexistuje.", "Změňte prosím hudební složku.");
            }
            else if ((string)e.Result == "zadne_slozky")
            {
                ZobrazStatusLabel("Prohledávání hudební knihovny", "Nenalezeny žádné složky.");
                ZobrazChybu("Prohledávání hudební knihovny", "Ve složce s hudební knihovnou nebyly nalezeny žádné složky.", "Změňte prosím hudební složku.");
            }
            else
            {
                ZobrazStatusLabel("Prohledávání hudební knihovny", "Hudební knihovna '" + hudebniKnihovna + "' byla úspěšně prohledána.");
            }

            // skryje se ProgressBar
            progressBarStatus.Visible = false;
        }



        // HOTOVO
        /**
        STAŽENÍ YOUTUBE-DL NEBO FFMPEG
        **/

        // HOTOVO
        private void menuNastaveniYoutubeDLStahnout_Click(object sender, EventArgs e)
        {
            // stažení youtube-dl
            StahniProgram(true);
        }
        // HOTOVO
        private void menuNastaveniFFmpegStahnout_Click(object sender, EventArgs e)
        {
            // stažení ffmpeg
            StahniProgram(false);
        }

        // HOTOVO
        /// <summary>
        /// Otevře FolderBrowserDialog pro výběr cílové složky stahování.
        /// Následně spustí BackgroundWorker se stahováním programu. 
        /// </summary>
        /// <param name="youtubedl">Typ souboru ke stažení (true = youtube-dl, false = ffmpeg).</param>
        private void StahniProgram(bool youtubedl)
        {
            // true = youtube-dl
            // false = ffmpeg
            string          typStahovani = "Stahování YouTube-DL";
            if (!youtubedl) typStahovani = "Stahování FFmpeg";
            ZobrazStatusLabel(typStahovani + "...");

            // získání cílové složky
            string cilovaSlozka;
            FolderBrowserDialog vyberSlozky = new FolderBrowserDialog();
            vyberSlozky.Description = "Vyberte cílovou složku pro uložení souboru:";
            if (vyberSlozky.ShowDialog() == DialogResult.OK)
            {
                cilovaSlozka = vyberSlozky.SelectedPath;
            }
            else
            {
                ZobrazStatusLabel(typStahovani, "Program nebyl stažen.");
                ZobrazChybu(typStahovani, "Program nebyl stažen.", "Nebyla vybrána cílová složka stahování.");
                return;
            }
            // stažení programu
            if (!backgroundWorkerStahniProgram.IsBusy)
            {
                List<string> argumenty = new List<string>();
                argumenty.Add(cilovaSlozka);
                argumenty.Add(youtubedl.ToString().ToLower());
                backgroundWorkerStahniProgram.RunWorkerAsync(argumenty);
            }
            else
            {
                ZobrazStatusLabel(typStahovani, "Program nebyl stažen.");
                ZobrazChybu(typStahovani, "Program nebyl stažen.", "Nelze spustit stahování.", "Zkuste to prosím znovu.");
            }
        }
        // HOTOVO
        private void backgroundWorkerStahniProgram_DoWork(object sender, DoWorkEventArgs e)
        {
            // 1. cesta
            // 2. typ:
            //  a) true = youtube-dl
            //  b) false = ffmpeg
            ZobrazStatusProgressBar(7);

            // získání argumentů
            List<string> argument = (List<string>)e.Argument;
            bool youtubedl = false;
            if (argument.Last() == "true")
            {
                youtubedl = true;
            }
            string cilovySoubor;
            string adresaStahovani;
            string cilovaSlozka = argument.First();

            if (youtubedl)
            {
                adresaStahovani = "http://yt-dl.org/latest/youtube-dl.exe";
                cilovySoubor = Path.Combine(cilovaSlozka, "youtube-dl.exe");
            }
            else
            {
                adresaStahovani = "http://ffmpeg.zeranoe.com/builds/win32/static/ffmpeg-latest-win32-static.zip";
                cilovySoubor = Path.Combine(cilovaSlozka, "ffmpeg.zip");
            }

            backgroundWorkerStahniProgram.ReportProgress(1);
            if (!Directory.Exists(cilovaSlozka))
            {
                // vytvoření cílové složky
                try
                {
                    Directory.CreateDirectory(cilovaSlozka);
                }
                catch (Exception)
                {
                    e.Result = "cilova_slozka";
                    return;
                }
            }
            backgroundWorkerStahniProgram.ReportProgress(2);

            // stáhnutí souboru
            WebClient stahovac = new WebClient();
            try
            {
                // kvůli problémům stahování ohledně zabezpečeného připojení (ffmpeg)
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };

                stahovac.DownloadFile(adresaStahovani, cilovySoubor);
            }
            catch (Exception)
            {
                e.Result = "stahovani";
                return;
            }

            if (youtubedl)
            {
                backgroundWorkerStahniProgram.ReportProgress(4);
                if (!File.Exists(cilovySoubor))
                {
                    e.Result = "stahovani";
                    return;
                }
                // uložení aktuální cesty a zobrazení v menu
                cestaYoutubeDL = cilovySoubor;
                menuStripMenu.Invoke(new Action(() =>
                {
                    MenuCestaZobrazit(1);
                }));
            }
            else
            {
                backgroundWorkerStahniProgram.ReportProgress(3);
                string slozkaFFmpeg = Path.Combine(cilovaSlozka, "ffmpeg-latest-win32-static");
                try
                {
                    // smazání existující složky FFmpeg
                    if (Directory.Exists(slozkaFFmpeg))
                    {
                        Directory.Delete(slozkaFFmpeg, true);
                    }
                    // rozbalení staženého zip souboru
                    ZipFile.ExtractToDirectory(cilovySoubor, cilovaSlozka);
                }
                catch (Exception)
                {
                    e.Result = "rozbaleni";
                    return;
                }
                backgroundWorkerStahniProgram.ReportProgress(4);

                // přesun souboru ffmpeg.exe
                string souborFFmpeg = Path.Combine(slozkaFFmpeg, "bin", "ffmpeg.exe");
                if (File.Exists(souborFFmpeg))
                {
                    try
                    {
                        File.Copy(souborFFmpeg, Path.Combine(cilovaSlozka, Path.GetFileName(souborFFmpeg)), true);
                    }
                    catch (Exception)
                    {
                        e.Result = "presun";
                        return;
                    }
                }
                backgroundWorkerStahniProgram.ReportProgress(5);

                // odstranění složky ffpmeg a zip souboru
                try
                {
                    if (Directory.Exists(slozkaFFmpeg))
                    {
                        Directory.Delete(slozkaFFmpeg, true);
                    }
                    if (File.Exists(cilovySoubor))
                    {
                        File.Delete(cilovySoubor);
                    }
                }
                catch (Exception)
                {
                    e.Result = "odstraneni";
                }
                backgroundWorkerStahniProgram.ReportProgress(6);

                // uložení aktuální cesty a zobrazení v menu
                cestaFFmpeg = Path.Combine(cilovaSlozka, Path.GetFileName(souborFFmpeg));
                menuStripMenu.Invoke(new Action(() =>
                {
                    MenuCestaZobrazit(2);
                }));
            }
        }
        // HOTOVO
        private void backgroundWorkerStahniProgram_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            // nastaví hodnotu ProgressBaru
            progressBarStatus.Value = e.ProgressPercentage;
        }
        // HOTOVO
        private void backgroundWorkerStahniProgram_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            // nastaví maximální hodnotu ProgressBaru
            progressBarStatus.Value = progressBarStatus.Maximum;

            // zobrazení výsledků
            if (e.Error != null)
            {
                ZobrazStatusLabel("Stahování programu", "Chyba.");
                ZobrazChybu("Stahování programu", "Došlo k chybě, program nemohl být stažen.", "Zkuste stáhnout program znovu.", e.Error.ToString());
            }
            else if ((string)e.Result == "cilova_slozka")
            {
                ZobrazStatusLabel("Stahování programu", "Problém při vytváření cílové složky.");
                ZobrazChybu("Stahování programu", "Program nebyl stažen.", "Problém při vytváření cílové složky.", "Stáhněte program znovu.");
            }
            else if ((string)e.Result == "stahovani")
            {
                ZobrazStatusLabel("Stahování programu", "Problém při stahování souboru.");
                ZobrazChybu("Stahování programu", "Program nebyl stažen.", "Problém při stahování souboru.", "Stáhněte program znovu.");
            }
            else if ((string)e.Result == "rozbaleni")
            {
                ZobrazStatusLabel("Stahování programu", "Problém při rozbalování archivu.");
                ZobrazChybu("Stahování programu", "Program nebyl stažen.", "Problém při rozbalování archivu.", "Stáhněte program znovu.");
            }
            else if ((string)e.Result == "presun")
            {
                ZobrazStatusLabel("Stahování programu", "Problém při přesunování souborů.");
                ZobrazChybu("Stahování programu", "Program nebyl stažen.", "Problém při přesunování souborů.", "Stáhněte program znovu.");
            }
            else if ((string)e.Result == "presun")
            {
                ZobrazStatusLabel("Stahování programu", "Program byl úspěšně stažen, ale nedošlo k odstranění přebytečných souborů.");
                ZobrazChybu("Stahování programu", "Program úspěšně stažen.", "Chyba odstranění přebytečných souborů. Soubory nebyly odstraněny.");
            }
            else
            {
                ZobrazStatusLabel("Stahování programu", "Program byl úspěšně stažen.");
            }

            // skryje se ProgressBar
            progressBarStatus.Visible = false;
        }


        // HOTOVO
        /**
        UKONČENÍ PROGRAMU
        **/

        // HOTOVO
        // uložení nastavení a smazání cache složky
        private void FormSeznam_FormClosing(object sender, FormClosingEventArgs e)
        {
            // zeptá se na uzavření programu
            if (MessageBox.Show("Opravdu chcete ukončit program?", "Ukončení programu", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            {
                e.Cancel = true;
            }

            // uloží cesty z menu nastavení do souborů
            Soubory souboryUlozeni = new Soubory();
            List<string> zapisDoSouboru = new List<string>();

            // cesty hudební knihovny
            foreach (ToolStripMenuItem menuCesta in menuNastaveniKnihovnaNaposledyVybrane.DropDownItems)
            {
                if (menuCesta.Checked)
                {
                    // vybranou cestu uloží na první řádek
                    zapisDoSouboru.Insert(0, menuCesta.Text);
                }
                else
                {
                    zapisDoSouboru.Add(menuCesta.Text);
                }
            }
            souboryUlozeni.Zapis(Path.Combine(slozkaProgramuData, "knihovna_historie.txt"), zapisDoSouboru);
            zapisDoSouboru.Clear();

            // cesty youtube-dl
            foreach (ToolStripMenuItem menuCesta in menuNastaveniYoutubeDLCestaNaposledyVybrane.DropDownItems)
            {
                if (menuCesta.Checked)
                {
                    // vybranou cestu uloží na první řádek
                    zapisDoSouboru.Insert(0, menuCesta.Text);
                }
                else
                {
                    zapisDoSouboru.Add(menuCesta.Text);
                }
            }
            souboryUlozeni.Zapis(Path.Combine(slozkaProgramuData, "youtubedl.txt"), zapisDoSouboru);
            zapisDoSouboru.Clear();

            // cesty ffmpeg
            foreach (ToolStripMenuItem menuCesta in menuNastaveniFFmpegCestaNaposledyVybrane.DropDownItems)
            {
                if (menuCesta.Checked)
                {
                    // vybranou cestu uloží na první řádek
                    zapisDoSouboru.Insert(0, menuCesta.Text);
                }
                else
                {
                    zapisDoSouboru.Add(menuCesta.Text);
                }
            }
            souboryUlozeni.Zapis(Path.Combine(slozkaProgramuData, "ffmpeg.txt"), zapisDoSouboru);
            zapisDoSouboru.Clear();

            // smaže aktualní cache složku
            if (Directory.Exists(slozkaProgramuCache))
            {
                try
                {
                    Directory.Delete(slozkaProgramuCache, true);
                }
                catch (Exception ex)
                {
                    ZobrazChybu("Čištění souborů", "Nepodařilo se smazat cache programu.", ex.Message);
                }
            }
        }

        // HOTOVO
        /**
        ODSTRANĚNÍ HISTORIE CEST
        **/

        // HOTOVO
        private void menuNastaveniKnihovnaNaposledyVymazat_Click(object sender, EventArgs e)
        {
            // hudební knihovna
            for (int i = 0; i < menuNastaveniKnihovnaNaposledyVybrane.DropDownItems.Count; i++)
            {
                if (menuNastaveniKnihovnaNaposledyVybrane.DropDownItems[i].Text != hudebniKnihovna)
                {
                    // pokud se nejedná o aktuální cestu, odstraním ji z historie
                    menuNastaveniKnihovnaNaposledyVybrane.DropDownItems.RemoveAt(i);
                    i--;
                }
            }
        }
        // HOTOVO
        private void menuNastaveniYoutubeDLCestaNaposledyVymazat_Click(object sender, EventArgs e)
        {
            // youtube-dl
            for (int i = 0; i < menuNastaveniYoutubeDLCestaNaposledyVybrane.DropDownItems.Count; i++)
            {
                if (menuNastaveniYoutubeDLCestaNaposledyVybrane.DropDownItems[i].Text != cestaYoutubeDL)
                {
                    // pokud se nejedná o aktuální cestu, odstraním ji z historie
                    menuNastaveniYoutubeDLCestaNaposledyVybrane.DropDownItems.RemoveAt(i);
                    i--;
                }
            }
        }
        // HOTOVO
        private void menuNastaveniFFmpegCestaNaposledyVymazat_Click(object sender, EventArgs e)
        {
            // ffmpeg
            for (int i = 0; i < menuNastaveniFFmpegCestaNaposledyVybrane.DropDownItems.Count; i++)
            {
                if (menuNastaveniFFmpegCestaNaposledyVybrane.DropDownItems[i].Text != cestaFFmpeg)
                {
                    // pokud se nejedná o aktuální cestu, odstraním ji z historie
                    menuNastaveniFFmpegCestaNaposledyVybrane.DropDownItems.RemoveAt(i);
                    i--;
                }
            }
        }


        // HOTOVO
        /**
        KONTROLA ODKAZU A ZÍSKÁNÍ ID VIDEA NEBO PLAYLISTU
        **/

        // HOTOVO
        // výběr textu s odkazem
        private void textBoxOdkaz_Click(object sender, EventArgs e)
        {
            // kliknutí na TextBox s odkazem
            if (textBoxOdkaz.Text == "VLOŽTE ODKAZ NA VIDEO NEBO PLAYLIST Z YOUTUBE")
            {
                // pokud se jedná o úvodní hlášku, vyberu všehen text
                textBoxOdkaz.SelectAll();
                textBoxOdkaz.Focus();
            }
        }
        
        // HOTOVO
        // opuštění TextBoxu
        private void textBoxOdkaz_Leave(object sender, EventArgs e)
        {
            // pokud je TextBox prázdný, zobrazí se úvodní hláška
            if (String.IsNullOrEmpty(textBoxOdkaz.Text.Trim()))
            {
                textBoxOdkaz.Text = "VLOŽTE ODKAZ NA VIDEO NEBO PLAYLIST Z YOUTUBE";
            }
        }

        // HOTOVO
        // kontrola odkazu
        private void textBoxOdkaz_TextChanged(object sender, EventArgs e)
        {
            // změna textu u TextBoxu s odkazem
            if (textBoxOdkaz.Text == "VLOŽTE ODKAZ NA VIDEO NEBO PLAYLIST Z YOUTUBE")
            {
                // jedná se o úvodní hlášku, odkaz neexistuje
                menuPridatVideoNeboPlaylist.Text = "PŘIDAT VIDEO NEBO PLAYLIST";
                menuPridatVideoNeboPlaylist.Enabled = false;
            }
            else
            {
                // nejedná se o úvodní hlášku, zkontoluji, zdali se jedná o validní odkaz
                ZkontrolujOdkaz(textBoxOdkaz.Text);
            }
        }

        // HOTOVO
        /// <summary>
        /// Zkontroluje odkaz na YouTube.
        /// Rozhodne, zdali se jedná o video nebo playlist.
        /// Získá ID videa, nebo playlistu.
        /// </summary>
        /// <param name="odkaz">Odkaz ke kontrole a získání ID.</param>
        private void ZkontrolujOdkaz(string odkaz)
        {
            Regex video = new Regex("(?:.+?)?(?:\\/v\\/|watch\\/|\\?v=|\\&v=|youtu\\.be\\/|\\/v=|^youtu\\.be\\/)([a-zA-Z0-9_-]{11})+", RegexOptions.Compiled);
            Regex playlist = new Regex(@"(?:http|https|)(?::\/\/|)(?:www.|)(?:youtu\.be\/|youtube\.com(?:\/embed\/|\/v\/|\/watch\?v=|\/ytscreeningroom\?v=|\/feeds\/api\/videos\/|\/user\S*[^\w\-\s]|\S*[^\w\-\s]))([\w\-]{12,})[a-z0-9;:@#?&%=+\/\$_.-]*");

            foreach (Match kontrola in playlist.Matches(odkaz))
            {
                foreach (var groupdata in kontrola.Groups.Cast<Group>().Where(groupdata => !groupdata.ToString().StartsWith("http://") && !groupdata.ToString().StartsWith("https://") && !groupdata.ToString().StartsWith("youtu") && !groupdata.ToString().StartsWith("www.")))
                {
                    // jedná se o playlist
                    menuPridatVideoNeboPlaylist.Text = "PŘIDAT PLAYLIST";
                    menuPridatVideoNeboPlaylist.Enabled = true;
                    youtubeID = groupdata.ToString();
                    ZobrazStatusLabel("Vkládání odkazu", "Vložený odkaz je playlist.");
                    return;
                }
            }
            foreach (Match kontrola in video.Matches(odkaz))
            {
                foreach (var groupdata in kontrola.Groups.Cast<Group>().Where(groupdata => !groupdata.ToString().StartsWith("http://") && !groupdata.ToString().StartsWith("https://") && !groupdata.ToString().StartsWith("youtu") && !groupdata.ToString().StartsWith("www.")))
                {
                    // jedná se o video
                    menuPridatVideoNeboPlaylist.Text = "PŘIDAT VIDEO";
                    menuPridatVideoNeboPlaylist.Enabled = true;
                    youtubeID = groupdata.ToString();
                    ZobrazStatusLabel("Vkládání odkazu", "Vložený odkaz je video.");
                    return;
                }
            }
            // nejedná se o správný odkaz
            menuPridatVideoNeboPlaylist.Enabled = false;
            menuPridatVideoNeboPlaylist.Text = "ODKAZ NENÍ Z YOUTUBE";
            ZobrazStatusLabel("Vkládání odkazu", "Vložený odkaz není z YouTube.");
        }




        // HOTOVO
        /**
        ZÍSKÁNÍ INFORMACÍ Z VIDEÍ A ZOBRAZENÍ NA LISTVIEW
        **/

        // HOTOVO
        // spustí nebo zastaví přidávání videí
        private void menuPridatVideoNeboPlaylist_Click(object sender, EventArgs e)
        {
            if (menuPridatVideoNeboPlaylist.Text == "ZASTAVIT PŘIDÁVÁNÍ")
            {
                // zastaví přidávání videí
                backgroundWorkerPridejVidea.CancelAsync();
            }
            else
            {
                // spustí přidávání videí
                if (!backgroundWorkerPridejVidea.IsBusy)
                {
                    if (menuPridatVideoNeboPlaylist.Text == "PŘIDAT PLAYLIST")
                    {
                        menuPridatVideoNeboPlaylist.Text = "ZASTAVIT PŘIDÁVÁNÍ";
                        backgroundWorkerPridejVidea.RunWorkerAsync(true);
                    }
                    else if (menuPridatVideoNeboPlaylist.Text == "PŘIDAT VIDEO")
                    {
                        menuPridatVideoNeboPlaylist.Text = "ZASTAVIT PŘIDÁVÁNÍ";
                        backgroundWorkerPridejVidea.RunWorkerAsync(false);
                    }
                }
                else
                {
                    ZobrazChybu("přidávání videa", "Nepodařilo se přidat žádné videa.", "Zkuste přidat videa znovu.");
                    ZobrazStatusLabel("Přidávání videa", "Nepodařilo se přidat žádné videa.");
                }
            }
        }

        // HOTOVO
        // přidávání videí nebo videí z playlistu
        private void backgroundWorkerPridejVidea_DoWork(object sender, DoWorkEventArgs e)
        {
            // 1. získá video / videa z playlistu
            // 2. získá informace o jednotlivých videích
            // 3. zobrazí videa a ListView

            ZobrazStatusLabel("Přidávání videa...");
            ZobrazStatusProgressBar(1);
            if (backgroundWorkerPridejVidea.CancellationPending)
            {
                e.Cancel = true;
                return;
            }
            bool playlist = (bool)e.Argument;
            // true = playlist
            // false = video

            // přidávání playlistu
            if (playlist)
            {
                // získá id videí z youtube api
                List<string> videaNovaID = new List<string>();
                try
                {
                    videaNovaID = YouTubeApi.ZiskejIDVidei(youtubeID);
                }
                catch (Exception)
                {
                    e.Result = "chyba";
                    return;
                }

                // v playlistu nejsou žádné videa nebo neexistuje
                if (videaNovaID == null)
                {
                    e.Result = "neexistuje";
                    return;
                }
                if (videaNovaID.Count < 1)
                {
                    e.Result = "zadne_videa";
                    return;
                }

                // nastaví maximum ProgressBaru na počet videí z playlistu
                ZobrazStatusProgressBar(videaNovaID.Count);

                int pridaneDrive = 0;

                // kontrola zdali video už nebylo přidáno
                foreach (string videoNoveID in videaNovaID)
                {
                    if (backgroundWorkerPridejVidea.CancellationPending)
                    {
                        e.Cancel = true;
                        return;
                    }
                    bool pridano = false;

                    backgroundWorkerPridejVidea.ReportProgress(videaNovaID.IndexOf(videoNoveID) + 1);
                    ZobrazStatusLabel("Přidávání videí z playlistu", videoNoveID + " (" + (videaNovaID.IndexOf(videoNoveID) + 1) + " z " + videaNovaID.Count + ")");

                    foreach (Video videoDrivePridane in videaVsechna)
                    {
                        if (videoNoveID == videoDrivePridane.id)
                        {
                            // nové id videa je shodné s již přidaným
                            pridaneDrive++;
                            pridano = true;
                            break;
                        }
                    }
                    if (!pridano)
                    {
                        // video nebylo dříve přidáno - přidá se nové video a získají se informace o něm
                        videaVsechna.Add(new Video(videoNoveID));
                        objectListViewSeznamVidei.SetObjects(videaVsechna);
                    }
                }
                string pridaneDriveText = "";
                if (pridaneDrive > 0)
                {
                    pridaneDriveText = " Nepřidaná videa byla přidána již dříve.";
                }
                ZobrazStatusLabel("Přidávání videí z playlistu", "Bylo přidáno " + (videaNovaID.Count() - pridaneDrive) + " videí z " + videaNovaID.Count() + "." + pridaneDriveText);
            }
            // přidávání jednoho videa
            else
            {
                bool pridano = false;

                backgroundWorkerPridejVidea.ReportProgress(1);
                ZobrazStatusLabel("Přidávání videí", youtubeID + " (1 z 1)");

                foreach (Video videoDrivePridane in videaVsechna)
                {
                    if (backgroundWorkerPridejVidea.CancellationPending)
                    {
                        e.Cancel = true;
                        return;
                    }
                    // pokud už bylo toto video přidáno, nepřidá se
                    if (youtubeID == videoDrivePridane.id)
                    {
                        pridano = true;
                        break;
                    }
                }
                if (pridano)
                {
                    ZobrazStatusLabel("Přidávání videí", "Video nebylo přidáno, protože už bylo přidané dříve.");
                }
                else
                {
                    // pokud video nebylo přidáno, přidá se
                    videaVsechna.Add(new Video(youtubeID));
                    objectListViewSeznamVidei.SetObjects(videaVsechna);
                    ZobrazStatusLabel("Přidávání videí", "Video bylo úspěšně přidáno.");
                }
            }
        }
        // HOTOVO
        private void backgroundWorkerPridejVidea_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            // nastaví hodnotu ProgressBaru
            progressBarStatus.Value = e.ProgressPercentage;
        }
        // HOTOVO
        private void backgroundWorkerPridejVidea_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            // nastaví maximální hodnotu ProgressBaru
            progressBarStatus.Value = progressBarStatus.Maximum;

            if (e.Cancelled)
            {
                ZobrazStatusLabel("Přidávání videí", "Zrušeno uživatelem.");
            }
            else if (e.Error != null)
            {
                ZobrazStatusLabel("Přidávání videí", "Chyba.");
                ZobrazChybu("Přidávání videí", "Došlo k chybě, videa nebyla přidána.", "Zkuste přidat videa znovu.", e.Error.ToString());
            }
            else if ((string)e.Result == "chyba")
            {
                ZobrazStatusLabel("Přidávání videí", "Chyba.");
                ZobrazChybu("Přidávání videí", "Nepodařilo se získat videa z Youtube API. Videa nebyla přidána.", "Zkuste přidat videa znovu.");
            }
            else if ((string)e.Result == "neexistuje")
            {
                ZobrazStatusLabel("Přidávání videí z playlistu", "Tento playlist neexistuje.");
                ZobrazChybu("Přidávání videí z playlistu", "Tento playlist neexistuje.", "Zkuste přidat jiný playlist.");
            }
            else if ((string)e.Result == "zadne_videa")
            {
                ZobrazStatusLabel("Přidávání videí z playlistu", "V playlistu nejsou žádná videa.");
                ZobrazChybu("Přidávání videí z playlistu", "V playlistu nejsou žádná videa.", "Zkuste přidat jiný playlist.");
            }
            // skryje se ProgressBar
            progressBarStatus.Visible = false;
            // změní text v menu ("přidání videa/playlistu")
            textBoxOdkaz.Text = "VLOŽTE ODKAZ NA VIDEO NEBO PLAYLIST Z YOUTUBE";
        }


        // HOTOVO
        /**
        ZOBRAZENÍ INFORMACÍ - StatusProgressBar
        **/
        #region ZobrazStatusProgressBar
        /// <summary>
        /// Zobrazí ProgressBar a nastaví mu minimální a maximální hodnotu.
        /// </summary>
        /// <param name="maximum">Maximální hodnota ProgressBaru.</param>
        private void ZobrazStatusProgressBar(int maximum)
        {
            statusStripStatus.Invoke(new Action(() =>
            {
                progressBarStatus.Value = progressBarStatus.Minimum;
                progressBarStatus.Maximum = maximum;
                progressBarStatus.Visible = true;
            }));
        }
        #endregion


        // HOTOVO
        /**
        ZOBRAZENÍ INFORMACÍ - StatusLabel
        **/
        #region ZobrazStatusLabel
        /// <summary>
        /// Zobrazí vybraný text na StatuStripLabelu.
        /// </summary>
        /// <param name="text">Text k zobrazení.</param>
        private void ZobrazStatusLabel(string text)
        {
            statusStripStatus.Invoke(new Action(() =>
            {
                labelStatus.Text = text;
            }));
        }
        /// <summary>
        /// Zobrazí vybraný nadpis a text na StatuStripLabelu.
        /// </summary>
        /// <param name="nadpis">Nadpis k zobrazení před textem.</param>
        /// <param name="text">Text k zobrazení.</param>
        private void ZobrazStatusLabel(string nadpis, string text)
        {
            ZobrazStatusLabel(nadpis.ToUpper() + ": " + text);
        }
        #endregion


        // HOTOVO
        /**
        ZOBRAZENÍ INFORMACÍ - Chyby
        **/
        #region ZobrazChybu
        /// <summary>
        /// Zobrazí MessageBox s chybou.
        /// </summary>
        /// <param name="nadpis">Nadpis chyby.</param>
        /// <param name="text">Text chyby.</param>
        private void ZobrazChybu(string nadpis, string text)
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
        private void ZobrazChybu(string nadpis, string text1, string text2)
        {
            ZobrazChybu(nadpis, text1 + Environment.NewLine + text2);
        }
        /// <summary>
        /// Zobrazí MessageBox s chybou.
        /// </summary>
        /// <param name="nadpis">Nadpis chyby.</param>
        /// <param name="text1">Text chyby na 1. řádku.</param>
        /// <param name="text2">Text chyby na 2. řádku.</param>
        /// <param name="text3">Text chyby na 3. řádku.</param>
        private void ZobrazChybu(string nadpis, string text1, string text2, string text3)
        {
            ZobrazChybu(nadpis, text1 + Environment.NewLine + text2 + Environment.NewLine + text3);
        }
        #endregion


        // HOTOVO
        /**
        ZOBRAZENÍ INFORMACÍ - Upozornění
        **/
        #region ZobrazUpozorneni
        /// <summary>
        /// Zobrazí MessageBox s upozorněním.
        /// </summary>
        /// <param name="nadpis">Nadpis upozornění.</param>
        /// <param name="text">Text upozornění.</param>
        private void ZobrazUpozorneni(string nadpis, string text)
        {
            MessageBox.Show(text, nadpis, MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
        /// <summary>
        /// Zobrazí MessageBox s upozorněním.
        /// </summary>
        /// <param name="nadpis">Nadpis upozornění.</param>
        /// <param name="text1">Text upozornění na 1. řádku.</param>
        /// <param name="text2">Text upozornění na 2. řádku.</param>
        private void ZobrazUpozorneni(string nadpis, string text1, string text2)
        {
            ZobrazUpozorneni(nadpis, text1 + Environment.NewLine + text2);
        }
        /// <summary>
        /// Zobrazí MessageBox s upozorněním.
        /// </summary>
        /// <param name="nadpis">Nadpis upozornění.</param>
        /// <param name="text1">Text upozornění na 1. řádku.</param>
        /// <param name="text2">Text upozornění na 2. řádku.</param>
        /// <param name="text3">Text upozornění na 3. řádku.</param>
        private void ZobrazUpozorneni(string nadpis, string text1, string text2, string text3)
        {
            ZobrazUpozorneni(nadpis, text1 + Environment.NewLine + text2 + Environment.NewLine + text3);
        }
        #endregion
    }
}