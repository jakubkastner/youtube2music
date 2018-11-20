using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.IO.Compression;
using System.Text.RegularExpressions;
using System.Globalization;
using System.Collections;
using System.Net;
using System.Diagnostics;
using System.Threading;

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

        /**** START ****/

        // start programu, načtení souborů, kontrola, youtube-dl
        private void FormSeznam_Load(object sender, EventArgs e)
        {
            this.FormBorderStyle = FormBorderStyle.FixedToolWindow;
            menuStripMenu.Visible = false;


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

        // složky interpretů - nalezne a uloží do souboru
        private void HudebniKnihovnaNajdiSlozky()
        {
            // 2018-11-13 NOVÉ

            if (!backgroundWorkerProhledejSlozky.IsBusy)
            {
                backgroundWorkerProhledejSlozky.RunWorkerAsync();
            }
            else
            {
                ZobrazNaLabelu("Prohledávám složky:", "Chyba - prohledávání složek", "Zkuste prohledat složky znovu.", "stazeno_chyba");
            }
        }

        /**** MENU - VLOŽENÍ VIDEA ****/

        // vybrat text
        private void textBoxOdkaz_Click(object sender, EventArgs e)
        {
            string labelTag = labelInfo.Tag.ToString();
            if (labelTag != "odkaz_vloz" || listViewSeznam.Items.Count <= 0)
            {
                textBoxOdkaz.SelectAll();
                textBoxOdkaz.Focus();
                ZobrazNaLabelu("", "", "VLOŽIT ODKAZ Z PAMĚTI", "odkaz_vloz");
            }
            else
            {
                ZobrazLabel(false);
            }
        }

        // kontrola odkazu
        private void textBoxOdkaz_TextChanged(object sender, EventArgs e)
        {
            if (textBoxOdkaz.Text != "VLOŽTE ODKAZ NA VIDEO NEBO PLAYLIST Z YOUTUBE")
            {
                ZiskejID(textBoxOdkaz.Text);
            }
            else
            {
                menuPridatVideoNeboPlaylist.Text = "PŘIDAT VIDEO NEBO PLAYLIST";
                menuPridatVideoNeboPlaylist.Enabled = false;
            }
        }

        // spustí funkci získej info z videa nebo playlistu
        private void menuPridatVideoNeboPlaylist_Click(object sender, EventArgs e)
        {
            ZobrazNaLabelu("Přidávám videa...", "");
            labelStatus.Text = "Přidávám videa...";

            progressBarStatus.Value = progressBarStatus.Minimum;
            progressBarStatus.Maximum = 1;
            progressBarStatus.Visible = true;

            if (!backgroundWorkerPridejVidea.IsBusy)
            {
                menuStripMenu.Visible = false;
                if (menuPridatVideoNeboPlaylist.Text == "PŘIDAT PLAYLIST")
                {
                    backgroundWorkerPridejVidea.RunWorkerAsync(true);
                }
                else if (menuPridatVideoNeboPlaylist.Text == "PŘIDAT VIDEO")
                {
                    backgroundWorkerPridejVidea.RunWorkerAsync(false);
                }
            }
            else
            {
                ZobrazNaLabelu("Přidávám videa:", "Chyba - přidávání videa", "Zkuste přidat videa znovu.", "pridavani_chyba");
            }
        }

        // přidávání videí nebo videí z playlistu
        private void backgroundWorkerPridejVidea_DoWork(object sender, DoWorkEventArgs e)
        {
            if (backgroundWorkerPridejVidea.CancellationPending)
            {
                e.Cancel = true;
                return;
            }
            bool playlist = (bool)e.Argument;

            // přidávání playlistu
            if (playlist)
            {
                // získá id z youtube api
                List<Video> videaNova = new List<Video>();
                List<string> videaID = YouTubeApi.ZiskejIDVidei(youtubeID);
                bool pridano = false;
                int pridaneDrive = 0;

                if (videaID == null)
                {
                    e.Result = "playlist_neexistuje";
                    return;
                }

                // nastvaví maximum na počet videí z playlistu
                statusStripStatus.Invoke(new Action(() =>
                {
                    progressBarStatus.Maximum = videaID.Count;
                }));

                // kontrola zdali video už nebylo přidáno
                foreach (string videoID in videaID)
                {
                    backgroundWorkerPridejVidea.ReportProgress(videaID.IndexOf(videoID) + 1);
                    ZobrazNaLabelu("Přidávám videa (" + (videaID.IndexOf(videoID) + 1) + " z " + videaID.Count + "):", videoID, "");

                    foreach (Video videoDrivePridane in videaVsechna)
                    {
                        if (videoID == videoDrivePridane.id) // id nové == id z listview
                        {
                            pridano = true;
                            break;
                        }
                    }
                    // video už bylo přidáno dříve - nyní se nepřidá
                    if (pridano)
                    {
                        pridaneDrive++;
                    }
                    else
                    {
                        videaNova.Add(new Video(videoID));
                    }
                }
                // přidá nová videa a zobrazí je
                videaVsechna.AddRange(videaNova);
                listViewSeznam.Invoke(new Action(() =>
                {
                    ZobrazVidea(videaNova);
                }));

                if (pridaneDrive > 0)
                {
                    ZobrazNaLabelu("Přidávám videa:", "Nebylo přidáno " + pridaneDrive + " videí z playlistu, protože už byly přidané dříve.", "OK", "stazeno_ok");
                    e.Result = "jiz_pridano";
                }
            }
            // přidávání jednoho videa
            else
            {
                // pokud už bylo toto video přidáno, nepřidá se
                bool pridano = false;

                ZobrazNaLabelu("Přidávám videa (1 z 1):", youtubeID, "");
                backgroundWorkerPridejVidea.ReportProgress(1);

                foreach (Video videoDrivePridane in videaVsechna)
                {
                    if (youtubeID == videoDrivePridane.id) // id nové == id z listview
                    {
                        pridano = true;
                        break;
                    }
                }
                if (pridano)
                {
                    ZobrazNaLabelu("Přidávám videa:", "Toto video nebylo přidáno, protože už bylo přidané dříve.", "OK", "stazeno_ok");
                    e.Result = "jiz_pridano";
                }
                else
                {
                    List<Video> videaNova = new List<Video>(1);
                    videaNova.Add(new Video(youtubeID));
                    videaVsechna.AddRange(videaNova);

                    listViewSeznam.Invoke(new Action(() =>
                    {
                        ZobrazVidea(videaNova);
                    }));
                }
            }
        }

        private void backgroundWorkerPridejVidea_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressBarStatus.Value = e.ProgressPercentage;
        }

        private void backgroundWorkerPridejVidea_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            progressBarStatus.Value = progressBarStatus.Maximum;
            if (e.Cancelled)
            {
                ZobrazNaLabelu("Přidávám videa:", "Zrušili jste práci", "stazeno_stormo");
            }
            else if (e.Error != null)
            {
                ZobrazNaLabelu("Přidávám videa:", "Chyba - přidávání videa", e.Error.ToString(), "Zkuste stáhnout videa znovu.", "stazeno_chyba");
            }
            else if ((string)e.Result == "playlist_neexistuje")
            {
                ZobrazNaLabelu("Tento playlist neexistuje.", "ZKUSTE TO PROSÍM ZNOVU", "odkaz");
            }
            else if ((string)e.Result == "jiz_pridano")
            {
            }
            else
            {
                ZobrazLabel(false);
                labelStatus.Text = "Připraven";
                progressBarStatus.Visible = false;
            }
            listBox1.Visible = false;
            menuStripMenu.Visible = true;
            textBoxOdkaz.Text = "VLOŽTE ODKAZ NA VIDEO NEBO PLAYLIST Z YOUTUBE";
        }

        // zobrazí videa na listView
        private void ZobrazVidea(List<Video> videa)
        {
            menuStripMenu.Invoke(new Action(() =>
            {
                menuVybrat.Enabled = true;
            }));
            listViewSeznam.BeginUpdate();

            foreach (Video videoList in videa)
            {
                foreach (ListViewItem videoListView in listViewSeznam.Items)
                {
                    if (videoList.id == videoListView.SubItems[0].Text)
                    {
                        videoListView.Remove();
                    }
                }
                ListView.ListViewItemCollection lvic = new ListView.ListViewItemCollection(listViewSeznam);
                // 0 id, 1 kanál, 2 datum publikování, 3 původní název, 4 interpret, 5 skladba, 6 featuring, 7 nový název, 8 složka, 9 chyba, 10 žánr  
                string slozka = "";
                if (videoList.skupina == "Video bylo staženo dříve")
                {
                    slozka = Path.GetFileNameWithoutExtension(videoList.slozka);
                }
                else
                {
                    slozka = videoList.slozka;
                    if (videoList.skupina == "Složka nalezena" && slozka.Contains(hudebniKnihovna))
                    {
                        slozka = slozka.Replace(hudebniKnihovna, "");
                    }
                }
                ListViewItem polozka = new ListViewItem(new string[] { videoList.id, videoList.kanal, String.Format("{0:yyyy-MM-dd}", videoList.publikovano), videoList.puvodniNazev, videoList.interpret, videoList.skladba, videoList.featuring, videoList.novyNazev, slozka, videoList.chyba, videoList.zanr });
                polozka.Checked = videoList.zaskrtnuto;
                foreach (ListViewGroup skupina in listViewSeznam.Groups)
                {
                    if (skupina.Header == videoList.skupina)
                    {
                        polozka.Group = skupina;
                        break;
                    }
                }
                lvic.Add(polozka);
            }
            listViewSeznam.EndUpdate();
        }

        /**** ID VIDEA A YOUTUBE API ****/

        // získá id z videa nebo playlistu
        private void ZiskejID(string odkaz)
        {
            Regex video = new Regex("(?:.+?)?(?:\\/v\\/|watch\\/|\\?v=|\\&v=|youtu\\.be\\/|\\/v=|^youtu\\.be\\/)([a-zA-Z0-9_-]{11})+", RegexOptions.Compiled);
            Regex playlist = new Regex(@"(?:http|https|)(?::\/\/|)(?:www.|)(?:youtu\.be\/|youtube\.com(?:\/embed\/|\/v\/|\/watch\?v=|\/ytscreeningroom\?v=|\/feeds\/api\/videos\/|\/user\S*[^\w\-\s]|\S*[^\w\-\s]))([\w\-]{12,})[a-z0-9;:@#?&%=+\/\$_.-]*");

            foreach (Match kontrola in playlist.Matches(odkaz))
            {
                foreach (var groupdata in kontrola.Groups.Cast<Group>().Where(groupdata => !groupdata.ToString().StartsWith("http://") && !groupdata.ToString().StartsWith("https://") && !groupdata.ToString().StartsWith("youtu") && !groupdata.ToString().StartsWith("www.")))
                {
                    menuPridatVideoNeboPlaylist.Text = "PŘIDAT PLAYLIST";
                    menuPridatVideoNeboPlaylist.Enabled = true;
                    youtubeID = groupdata.ToString();
                    ZobrazNaLabelu("Vložený odkaz je playlist.", "", "PŘIDAT PLAYLIST", "odkaz_ok");
                    return;
                }
            }
            foreach (Match kontrola in video.Matches(odkaz))
            {
                foreach (var groupdata in kontrola.Groups.Cast<Group>().Where(groupdata => !groupdata.ToString().StartsWith("http://") && !groupdata.ToString().StartsWith("https://") && !groupdata.ToString().StartsWith("youtu") && !groupdata.ToString().StartsWith("www.")))
                {
                    menuPridatVideoNeboPlaylist.Text = "PŘIDAT VIDEO";
                    menuPridatVideoNeboPlaylist.Enabled = true;
                    youtubeID = groupdata.ToString();
                    ZobrazNaLabelu("Vložený odkaz je video.", "", "PŘIDAT VIDEO", "odkaz_ok");
                    return;
                }
            }
            ZobrazNaLabelu("Vložený odkaz není z YouTube.", "ZKUSTE TO PROSÍM ZNOVU", "odkaz");

            menuPridatVideoNeboPlaylist.Enabled = false;
            menuPridatVideoNeboPlaylist.Text = "PŘIDAT VIDEO NEBO PLAYLIST";
        }


        /**** DODĚLAT **** MENU - FILTR SOUBORŮ ****/

        #region filtry
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
                string nazev = vid.novyNazev;
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

        // zobrazí na prograssBaru počet již stažených videí
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
                TagLib.File soubor = TagLib.File.Create(Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), "stazene", vid.novyNazev + ".mp3"));
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
                File.Move(Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), "stazene", vid.novyNazev + ".mp3"),
                            Path.Combine(vid.slozka, vid.novyNazev + ".mp3"));
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

        /**** MENU - NASTAVENÍ - prohledat hudební knihovnu ****/

        private void menuNastaveniKnihovnaProhledat_Click(object sender, EventArgs e)
        {
            HudebniKnihovnaNajdiSlozky();
        }

        private void backgroundWorkerProhledejSlozky_DoWork(object sender, DoWorkEventArgs e)
        {
            // prohledá složky a zapíše je do souboru
            if (backgroundWorkerStahniVideo.CancellationPending)
            {
                e.Cancel = true;
                return;
            }

            // hudební knhihovna neexistuje
            if (hudebniKnihovna == null)
            {
                ZobrazNaLabelu("Nebyla vybrána složka s hudební knihovnou.", "VYBRAT SLOŽKU", "hudebni_knihovna");
                //HudebniKnihovnaVyber();
                return;
            }
            if (!Directory.Exists(hudebniKnihovna))
            {
                ZobrazNaLabelu("Nebyla vybrána složka s hudební knihovnou.", "VYBRAT SLOŽKU", "hudebni_knihovna");
                //HudebniKnihovnaVyber();
                return;
            }

            List<string> slozkyinterpretu = new List<string>();
            try
            {
                foreach (string cestaSlozky in Directory.GetDirectories(hudebniKnihovna, "*.*", SearchOption.AllDirectories))
                {
                    // projde složky ve vybrané hudební knihovně
                    string nazevSlozky = Path.GetFileNameWithoutExtension(cestaSlozky);
                    if (!Regex.IsMatch(nazevSlozky.Split(' ').First(), @"^\d{4}$")) // album -> 
                    {
                        // pokud se nejedná o album (název je "rok název" = první 4 znaky jsou číslice), přidám do seznamu
                        slozkyinterpretu.Add(cestaSlozky);
                    }
                }
            }
            catch (Exception ex)
            {
                // chyba
                MessageBox.Show(ex.Message + Environment.NewLine + "Vyberte prosím jinou složku s hudební knihovnou.", "Chyba - výběr složky", MessageBoxButtons.OK, MessageBoxIcon.Error);
                ZobrazNaLabelu("", "Vyberte prosím jinou složku s hudební knihovnou.", "hudebni_knihovna");
                //HudebniKnihovnaVyber();
                return;
            }
            if (slozkyinterpretu == null)
            {
                MessageBox.Show("nenalezeny žádné složky");
                return;
            }
            if (slozkyinterpretu.Count() == 0)
            {
                MessageBox.Show("nenalezeny žádné složky");
                return;
            }
            // seřadí složky
            slozkyinterpretu.Sort();
            for (int i = 1; i < slozkyinterpretu.Count; i++)
            {
                if (slozkyinterpretu[i].Contains(slozkyinterpretu[i - 1]))
                {
                    if (!Path.GetFileNameWithoutExtension(slozkyinterpretu[i]).Contains(Path.GetFileNameWithoutExtension(slozkyinterpretu[i - 1])))
                    {
                        slozkyinterpretu.RemoveAt(i - 1);
                        i--;
                    }
                }
            }
            // na 1. pozici vloží aktuální hudební knihovnu
            slozkyinterpretu.Insert(0, hudebniKnihovna);
            // zapíše do souboru
            Soubory soubor = new Soubory();
            soubor.Zapis(Path.Combine(slozkaProgramuData, "knihovna_slozky.txt"), slozkyinterpretu);
            //HudebniKnihovnaNajdiSlozky(false);
        }

        private void backgroundWorkerProhledejSlozky_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            /*if (e.Cancelled)
            {
                ZobrazNaLabelu("Prohledávám složky:", "Zrušili jste práci", "stazeno_stormo");
            }
            else if (e.Error != null)
            {
                ZobrazNaLabelu("Prohledávám složky:", "Chyba - prohledávání složek", e.Error.ToString(), "Zkuste prohledat složky znovu.", "stazeno_chyba");
            }
            else
            {
                ZobrazNaLabelu("Prohledávám složky:", "Složky byly úspěšně prohledány", "OK", "slozky_ok");
            }*/
        }

        /**** NOVÉ ****/

        private void testToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //textBoxOdkaz.Text = "https://www.youtube.com/playlist?list=PLTFujRZGqO1bLKMsXFkozb0tJNg-t3g4m"; // ZKOUŠKA
            //textBoxOdkaz.Text = "https://www.youtube.com/playlist?list=PLTFujRZGqO1b6j6ZP8glz9s8iqHi1v-KD!"; // 2017.04 us & others
            //textBoxOdkaz.Text = "https://www.youtube.com/watch?v=6YJcaefdvao";
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


        // ok_new MENU - NASTAVENÍ - CESTY - vybrání již dříve přidané složky / cesty z menu

        /// <summary>
        /// Uživatel vybere pomocí FolderBrowserDialogu novou složku s hudební knihovnou.
        /// Ta se zobrazí v menu a automaticky se prohledají složky v nové hudební knihovně.
        /// </summary>
        private void HudebniKnihovnaVyber()
        {
            // uživatel vybere novou cestu složky s hudební knihovnou
            FolderBrowserDialog vyberSlozky = new FolderBrowserDialog();
            vyberSlozky.Description = "Vyberte složku s hudební knihovnou:";
            vyberSlozky.ShowNewFolderButton = false;
            if (vyberSlozky.ShowDialog() == DialogResult.OK)
            {
                hudebniKnihovna = vyberSlozky.SelectedPath;
                MenuCestaZobrazit(0);
                HudebniKnihovnaNajdiSlozky();
            }
        }
        private void menuNastaveniKnihovnaZmenit_Click(object sender, EventArgs e)
        {
            // uživatel vybere novou cestu složky s hudební knihovnou
            HudebniKnihovnaVyber();
        }
        private void menuNastaveniCestaYoutubeDLZmenit_Click(object sender, EventArgs e)
        {
            // uživatel vybere cestu youtube-dl pomocí OpenFileDialogu, ta se zobrazí v menu
            
            OpenFileDialog vyberSouboru = new OpenFileDialog();
            vyberSouboru.Title = "Vyberte spustitelný soubor YoutubeDL:";
            vyberSouboru.Filter = "Spustitelné soubory (*.exe)|*.exe|Všechny soubory (*.*)|*.*";
            if (vyberSouboru.ShowDialog() == DialogResult.OK)
            {
                if (vyberSouboru.FilterIndex == 2)
                {
                    // nejedná se o exe soubor
                    MessageBox.Show("Nejedná se o spustitelý soubor (*.exe)! Program nemusí fungovat správně.");
                }
                cestaYoutubeDL = vyberSouboru.FileName;
                MenuCestaZobrazit(1);
            }
        }
        private void menuNastaveniCestaFFmpegZmenit_Click(object sender, EventArgs e)
        {
            // uživatel vybere cestu ffmpeg pomocí OpenFileDialogu, ta se zobrazí v menu
            OpenFileDialog vyberSouboru = new OpenFileDialog();
            vyberSouboru.Title = "Vyberte spustitelný soubor FFmpeg:";
            vyberSouboru.Filter = "Spustitelné soubory (*.exe)|*.exe|Všechny soubory (*.*)|*.*";
            if (vyberSouboru.ShowDialog() == DialogResult.OK)
            {
                if (vyberSouboru.FilterIndex == 2)
                {
                    // nejedná se o exe soubor
                    MessageBox.Show("Nejedná se o spustitelý soubor (*.exe)! Program nemusí fungovat správně.");
                }
                cestaFFmpeg = vyberSouboru.FileName;
                MenuCestaZobrazit(2);
            }
        }
        

        // ok_new MENU - NASTAVENÍ - CESTY - vybrání již dříve přidané složky / cesty z menu

        private void menuNastaveniKnihovnaNaposledyVybrane_Click(object sender, EventArgs e)
        {
            // vybrere složku s hudební knihovnou z již dříve vybraných složek a zobrazí ji v menu
            var menu = sender as ToolStripMenuItem;
            if (Directory.Exists(menu.Text))
            {
                hudebniKnihovna = menu.Text;
                MenuCestaZobrazit(0);
                HudebniKnihovnaNajdiSlozky();
            }
            else
            {
                MessageBox.Show("cesta neexistuje");
            }
        }
        private void menuNastaveniCestaYoutubeDLNaposledyVybrane_Click(object sender, EventArgs e)
        {
            // vybrere cestu youtube-dl z již dříve vybraných složek a zobrazí ji v menu
            var menu = sender as ToolStripMenuItem;
            if (File.Exists(menu.Text))
            {
                cestaYoutubeDL = menu.Text;
                MenuCestaZobrazit(1);
            }
            else
            {
                MessageBox.Show("cesta neexistuje");
            }
        }
        private void menuNastaveniCestaFFmpegNaposledyVybrane_Click(object sender, EventArgs e)
        {
            // vybrere cestu ffmpeg z již dříve vybraných složek a zobrazí ji v menu
            var menu = sender as ToolStripMenuItem;
            if (File.Exists(menu.Text))
            {
                cestaFFmpeg = menu.Text;
                MenuCestaZobrazit(2);
            }
            else
            {
                MessageBox.Show("cesta neexistuje");
            }
        }


        // ok_new MENU - NASTAVENÍ - CESTY - načtení cest ze souboru

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
            if (typ == 0)       cestaSouboru = Path.Combine(slozkaProgramuData, "knihovna_historie.txt");
            else if (typ == 1)  cestaSouboru = Path.Combine(slozkaProgramuData, "youtubedl.txt");
            else if (typ == 2)  cestaSouboru = Path.Combine(slozkaProgramuData, "ffmpeg.txt");

            // načte ze souboru cesty
            Soubory soubor = new Soubory();
            List<string> pridejCesty = soubor.Precti(cestaSouboru);
            if (pridejCesty == null)    return;
            if (pridejCesty.Count == 0) return;

            // uloží první řádek ze souboru jako výchozí cestu
            if (typ == 0)
            {
                if (Directory.Exists(pridejCesty.First()))
                {
                    hudebniKnihovna = pridejCesty.First();
                    menuNastaveniKnihovnaProhledat.Text = "Prohledat hudební knihovnu";
                    menuNastaveniKnihovnaProhledat.Enabled = true;
                    menuNastaveniKnihovnaVybrana.Text = hudebniKnihovna;
                    menuNastaveniKnihovnaVybrana.Enabled = true;
                    menuNastaveniKnihovnaVybrana.ToolTipText = "Otevřít složku '" + hudebniKnihovna + "' v průzkumníku souborů";
                }
            }
            else if (typ == 1)
            {
                if (File.Exists(pridejCesty.First()))
                {
                    cestaYoutubeDL = pridejCesty.First();
                    menuNastaveniYoutubeDLCestaVybrana.Text = cestaYoutubeDL;
                    menuNastaveniYoutubeDLCestaVybrana.Enabled = true;
                    menuNastaveniYoutubeDLCestaVybrana.ToolTipText = "Najít soubor '" + cestaYoutubeDL + "' v průzkumníku souborů";
                }
            }
            else if (typ == 2)
            {
                if (File.Exists(pridejCesty.First()))
                {
                    cestaFFmpeg = pridejCesty.First();
                    menuNastaveniFFmpegCestaVybrana.Text = cestaFFmpeg;
                    menuNastaveniFFmpegCestaVybrana.Enabled = true;
                    menuNastaveniFFmpegCestaVybrana.ToolTipText = "Najít soubor '" + cestaFFmpeg + "' v průzkumníku souborů";
                }
            }

            // projde cesty a přidá je do menu
            foreach (string cesta in pridejCesty)
            {
                string aktualniCesta = cesta.Trim();
                if (typ == 0)
                {
                    if (Directory.Exists(aktualniCesta))
                    {
                        MenuCestaPridat(aktualniCesta, typ);
                    }
                }
                else
                {
                    if (File.Exists(aktualniCesta))
                    {
                        MenuCestaPridat(aktualniCesta, typ);
                    }
                }
            }
        }


        // ok_new MENU - NASTAVENÍ - CESTY - přidání nebo zobrazení cesty v menu

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
            // projde menu a zjistí, zdali cesta již byla v minulosti vybrána
            /*for (int i = 0; i < menuNaposledyVybraneCesty.DropDownItems.Count; i++)
            {
                ToolStripMenuItem menuCesta = (ToolStripMenuItem)menuNaposledyVybraneCesty.DropDownItems[i];
                if (menuCesta.Text == cestaVychozi)
                {
                    menuCesta.Checked = true;

                    if (menuNaposledyVybraneCesty.DropDownItems.Count > i + 1)
                    {
                        // pokud byla nalezena, přidám ji na konec a zaškrtnu ji v menu
                        menuNaposledyVybraneCesty.DropDownItems.RemoveAt(i);
                        i--;
                        menuNaposledyVybraneCesty.DropDownItems.Add(menuCesta);
                    }

                    nalezeno = true;
                }
                else
                {
                    menuCesta.Checked = false;
                }
            }*/

            foreach (ToolStripMenuItem menuCesta in menuNaposledyVybraneCesty.DropDownItems)
            {
                if (menuCesta.Text == cestaVychozi)
                {
                    // pokud byla nalezena, přidám ji na konec a zaškrtnu ji v menu
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
            }
            else
            {
                menuVybranaCesta.ToolTipText = "Najít soubor '" + cestaVychozi + "' v průzkumníku souborů";
            }
        }


        // ok_new MENU - NASTAVENÍ - CESTY - stažení youtube-dl / ffmpeg

        private void menuNastaveniYoutubeDLStahnout_Click(object sender, EventArgs e)
        {
            // stažení youtube-dl
            StahniProgram(true);
        }

        private void menuNastaveniFFmpegStahnout_Click(object sender, EventArgs e)
        {
            // stažení ffmpeg
            StahniProgram(false);
        }

        /// <summary>
        /// Otevře FolderBrowserDialog pro výběr cílové složky stahování.
        /// Následně spustí BackgroundWorker se stahováním programu. 
        /// </summary>
        /// <param name="youtubedl">Typ souboru ke stažení (true = youtube-dl, false = ffmpeg).</param>
        private void StahniProgram(bool youtubedl)
        {
            // true = youtube-dl
            // false = ffmpeg

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
                MessageBox.Show("nebyla vybrana slozka");
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
                MessageBox.Show("nejde spustit stahování");
            }
        }

        private void backgroundWorkerStahniProgram_DoWork(object sender, DoWorkEventArgs e)
        {
            // 1. cesta
            // 2. typ:
            //  a) true = youtube-dl
            //  b) false = ffmpeg

            if (backgroundWorkerStahniProgram.CancellationPending)
            {
                e.Cancel = true;
                return;
            }
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

            if (!Directory.Exists(cilovaSlozka))
            {
                // vytvoření cílové složky
                try
                {
                    Directory.CreateDirectory(cilovaSlozka);
                }
                catch (Exception)
                {
                    MessageBox.Show("problem vytvareni slozky");
                    return;
                }
            }

            // stáhnutí souboru
            WebClient stahovac = new WebClient();
            try
            {
                // kvůli problémům stahování
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };

                stahovac.DownloadFile(adresaStahovani, cilovySoubor);
            }
            catch (Exception ex)
            {
                MessageBox.Show("problem stahovani souboru" + ex.Message);
                return;
            }

            if (youtubedl)
            {
                if (!File.Exists(cilovySoubor))
                {
                    MessageBox.Show("nepodařilo se stáhnout");
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
                catch (Exception ex)
                {
                    MessageBox.Show("chyba rozbaleni souboru" + ex.Message);
                    return;
                }
                // přesun souboru ffmpeg.exe
                string souborFFmpeg = Path.Combine(slozkaFFmpeg, "bin", "ffmpeg.exe");
                if (File.Exists(souborFFmpeg))
                {
                    try
                    {
                        File.Copy(souborFFmpeg, Path.Combine(cilovaSlozka, Path.GetFileName(souborFFmpeg)), true);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("chyba presunu souboru" + ex.Message);
                        return;
                    }
                }
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
                catch (Exception ex)
                {
                    MessageBox.Show("chyba odstraneni souboru" + ex.Message);
                }
                // uložení aktuální cesty a zobrazení v menu
                cestaFFmpeg = Path.Combine(cilovaSlozka, Path.GetFileName(souborFFmpeg));
                menuStripMenu.Invoke(new Action(() =>
                {
                    MenuCestaZobrazit(2);
                }));
            }
        }

        private void backgroundWorkerStahniProgram_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {

        }

        private void backgroundWorkerStahniProgram_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            MessageBox.Show("hotovo");
        }


        // ok_new PROGRAM - UKONČENÍ - uložení nastavení a smazání cache složky

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
                catch (Exception)
                {
                    MessageBox.Show("chyba mazání cache složky");
                }
            }
        }

        // odstranění historie

        private void menuNastaveniKnihovnaNaposledyVymazat_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < menuNastaveniKnihovnaNaposledyVybrane.DropDownItems.Count; i++)
            {
                if (menuNastaveniKnihovnaNaposledyVybrane.DropDownItems[i].Text != hudebniKnihovna)
                {
                    menuNastaveniKnihovnaNaposledyVybrane.DropDownItems.RemoveAt(i);
                    i--;
                }
            }
        }
        private void menuNastaveniYoutubeDLCestaNaposledyVymazat_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < menuNastaveniYoutubeDLCestaNaposledyVybrane.DropDownItems.Count; i++)
            {
                if (menuNastaveniYoutubeDLCestaNaposledyVybrane.DropDownItems[i].Text != cestaYoutubeDL)
                {
                    menuNastaveniYoutubeDLCestaNaposledyVybrane.DropDownItems.RemoveAt(i);
                    i--;
                }
            }
        }
        private void menuNastaveniFFmpegCestaNaposledyVymazat_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < menuNastaveniFFmpegCestaNaposledyVybrane.DropDownItems.Count; i++)
            {
                if (menuNastaveniFFmpegCestaNaposledyVybrane.DropDownItems[i].Text != cestaFFmpeg)
                {
                    menuNastaveniFFmpegCestaNaposledyVybrane.DropDownItems.RemoveAt(i);
                    i--;
                }
            }
        }
    }
}