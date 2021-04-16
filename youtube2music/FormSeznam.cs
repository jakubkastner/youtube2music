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
using BrightIdeasSoftware;
using Microsoft.WindowsAPICodePack.Taskbar;
using Ookii.Dialogs.Wpf;

namespace youtube2music
{
    public partial class FormSeznam : Form
    {        
        /**** PROMĚNNÉ ****/

        string hudebniKnihovnaOpus = null;
        string hudebniKnihovnaMp3 = null;
        string cestaYoutubeDL = null;
        string cestaFFmpeg = null;
        string slozkaProgramuData = null;
        string slozkaProgramuCache = null;

        //bool album = false;
        //string youtubeID = null;
        List<Video> videaVsechna = new List<Video>();
        SeznamInterpretu vsichniInterpreti = new SeznamInterpretu();
        Aplikace.Cesty neco = new Aplikace.Cesty();

        public FormSeznam()
        {
            InitializeComponent();
            menuPridatSpotify.Visible = false;
        }

        // HOTOVO
        /**
        ZÍSKÁNÍ VYBRANÝCH VIDEÍ A SPUŠTĚNÍ ÚPRAVY VIDEÍ
        **/

        // HOTOVO
        /// <summary>
        /// Získá vybraná videa ze seznamu videí (ObjectListView).
        /// Pokud je vybráno bez chyb, získá jen ty, kde není chyba.
        /// </summary>
        /// <param name="bezChyb">Získání videí bez chyb (true) nebo s chybami (false).</param>
        /// <returns></returns>
        private List<Video> ZiskejVybranaVidea(bool bezChyb)
        {            
            List<Video> videaVybrana = new List<Video>();
            foreach (Video videoVybrane in objectListViewSeznamVidei.CheckedObjects)
            {
                if (bezChyb && !String.IsNullOrEmpty(videoVybrane.Chyba))
                {
                    // je vybráno získání bez chyb a existuje chyba, nepřidá se
                    continue;
                }
                videaVybrana.Add(videoVybrane);
            }
            return videaVybrana;
        }
        // HOTOVO
        // zobrazí Form s úpravou a předá vybraná videa ze seznamu
        private void menuUpravit_Click(object sender, EventArgs e)
        {
            List<Video> upravovanaVidea = ZiskejVybranaVidea(false);
            FormUprava uprava = new FormUprava(upravovanaVidea, hudebniKnihovnaOpus);
            uprava.ShowDialog();
            objectListViewSeznamVidei.UpdateObjects(upravovanaVidea);
        }

        // HOTOVO
        /**
        STAŽENÍ VIDEÍ
        **/

        // HOTOVO
        // menu - stáhnout video
        private void menuStahnout_Click(object sender, EventArgs e)
        {
            if (menuStahnout.Text == "STÁHNOUT A PŘESUNOUT")
            {
                // spustí backGroundWorker se stáhnutím videí
                ZobrazitOperaci("Stahování videí...");
                if (!backgroundWorkerStahniVidea.IsBusy)
                {
                    //List<Video> videaKeStazeni = ZiskejVybranaVidea(true);
                    List<Video> videaKeStazeni = ZiskejVybranaVidea(false);
                    ZobrazStatusProgressBar(videaKeStazeni.Count * 5 + 1);
                    menuStahnout.Text = "ZASTAVIT STAHOVÁNÍ";
                    backgroundWorkerStahniVidea.RunWorkerAsync(videaKeStazeni);
                }
                else
                {
                    ZobrazitOperaci("Stahování videí", "Chyba stahování videí.");
                    Zobrazit.Chybu("Stahování videí", "Videa se nepodařilo stáhnout.", "Zkuzte stáhnout videa znovu.");
                }
            }
            else if (menuStahnout.Text == "ZASTAVIT STAHOVÁNÍ")
            {
                // zastaví stahování videí
                menuStahnout.Text = "ZASTAVUJI STAHOVÁNÍ...";
                menuStahnout.Enabled = false;
                backgroundWorkerStahniVidea.CancelAsync();
            }
        }
        // HOTOVO
        private Video stahovaneVideoVerejne;
        // stáhne video
        private void backgroundWorkerStahniVidea_DoWork(object sender, DoWorkEventArgs e)
        {
            if (backgroundWorkerStahniVidea.CancellationPending)
            {
                e.Cancel = true;
                return;
            }
            List<Video> videaKeStazeni = (List<Video>)e.Argument;
            int stahovaneVideoIndex = 1;
            int stahovaniReport = 1;
            int prevedeno = 0;
            List<string> stazeno = new List<string>();
            // postupně stáhne vybraná videa
            foreach (Video stahovaneVideo in videaKeStazeni)
            {
                if (backgroundWorkerStahniVidea.CancellationPending)
                {
                    e.Cancel = true;
                    return;
                }

                string adresaVidea = "https://youtu.be/" + stahovaneVideo.ID;
                string nazev = stahovaneVideo.NazevNovySoubor;
                Process cmd = new Process();
                ProcessStartInfo psi = new ProcessStartInfo();

                ZobrazitOperaci("Stahování videí", "Stahuji " + stahovaneVideo.ID + " (" + stahovaneVideoIndex++ + " z " + videaKeStazeni.Count + ")");
                stahovaneVideo.Stav = "Stahování";
                objectListViewSeznamVidei.RefreshObject(stahovaneVideo);
                backgroundWorkerStahniVidea.ReportProgress(stahovaniReport++);

                stahovaneVideoVerejne = stahovaneVideo;

                // odstraní soubor
                if (File.Exists(stahovaneVideo.Chyba))
                {
                    // opus
                    string existujiciSouborOpus = stahovaneVideo.Chyba;
                    /*DialogResult odpoved = MessageBox.Show(existujiciSouborOpus, "smažu opus", MessageBoxButtons.YesNo);
                    if (odpoved == DialogResult.Yes)
                    {*/
                    File.Delete(existujiciSouborOpus);
                    //}
                    // mp3
                    string existujiciSouborMp3 = stahovaneVideo.Chyba.Replace(hudebniKnihovnaOpus, hudebniKnihovnaMp3).Replace("opus", "mp3"); // UDĚLAT JINAK
                    if (File.Exists(existujiciSouborMp3))
                    {
                        /*odpoved = MessageBox.Show(existujiciSouborMp3, "smažu mp3", MessageBoxButtons.YesNo);
                        if (odpoved == DialogResult.Yes)
                        {*/
                        File.Delete(existujiciSouborMp3);
                        //}
                    }
                    stahovaneVideo.Chyba = "";
                }

                // nastaví vlastnosti programu na stažení

                // nové s cookies
                psi.Arguments = "-x --cookies \"C:\\Programy\\youtube-cookies.txt\" -i -w  --audio-quality 0 --audio-format mp3 -o \"" + nazev + ".%(ext)s\" \"" + adresaVidea + "\""; // -U = update
                //psi.Arguments = "-x -i -w  --audio-quality 0 --audio-format mp3 -o \"" + nazev + ".%(ext)s\" \"" + adresaVidea + "\""; // -U = update

                /*
                -x = extrahuje audio (musí být pro --audio-format)
                -w = nepřepíše soubor
                -i = ignoruje chyby (např v playlistech)
                -o "vystup.%(ext)" = název vsýstupního soboru
                --cookies = soubor s cookies
                --audio-format mp3 = výstupní audio formát
                --audio-quality 0 = výstupní kvalita audia (0 nejlepší, 9 nejhorší)
                */

                psi.CreateNoWindow = true;
                //psi.CreateNoWindow = false;
                psi.ErrorDialog = true;
                psi.FileName = cestaYoutubeDL;
                psi.RedirectStandardInput = true;
                psi.RedirectStandardOutput = true;
                psi.UseShellExecute = false;
                psi.WorkingDirectory = slozkaProgramuCache;// Path.Combine(, "stazene");

                // spustí program na stažení
                cmd.OutputDataReceived += new DataReceivedEventHandler(CteckaVystupu);
                cmd.ErrorDataReceived += new DataReceivedEventHandler(CteckaVystupuChyby);
                cmd.StartInfo = psi;
                cmd.SynchronizingObject = objectListViewSeznamVidei;
                try
                {
                    cmd.Start();
                }
                catch (Exception)
                {
                    stahovaneVideo.Chyba = "Stahování se nezdařilo";
                    stahovaneVideo.Stav = "";
                    objectListViewSeznamVidei.RefreshObject(stahovaneVideo);
                    continue;
                }
                cmd.BeginOutputReadLine();
                cmd.WaitForExit();

                if (String.IsNullOrEmpty(stahovaneVideo.Chyba))
                {
                    // nedošlo k chybě stahování
                    stahovaneVideo.Stav = "Staženo";
                }
                else
                {
                    // došlo k chybě stahování
                    stahovaneVideo.Stav = "";
                    objectListViewSeznamVidei.RefreshObject(stahovaneVideo);
                    continue;
                }
                objectListViewSeznamVidei.RefreshObject(stahovaneVideo);
                backgroundWorkerStahniVidea.ReportProgress(stahovaniReport++);



                // uloží metadata do souboru a přesune soubor
                ZapisMetadata(stahovaneVideo);
                if (!String.IsNullOrEmpty(stahovaneVideo.Chyba))
                {
                    // došlo k chybě zápisu metadat
                    continue;
                }
                backgroundWorkerStahniVidea.ReportProgress(stahovaniReport++);

                PrevedNaOpus(stahovaneVideo);
                if (String.IsNullOrEmpty(stahovaneVideo.Chyba))
                {
                    // nedošlo k chybě stahování
                    stahovaneVideo.Stav = "Převedeno na opus";
                }
                else
                {
                    // došlo k chybě stahování
                    stahovaneVideo.Stav = "";
                    objectListViewSeznamVidei.RefreshObject(stahovaneVideo);
                    continue;
                }
                objectListViewSeznamVidei.RefreshObject(stahovaneVideo);
                backgroundWorkerStahniVidea.ReportProgress(stahovaniReport++);

                // přesune soubor
                PresunSoubor(stahovaneVideo);
                if (!String.IsNullOrEmpty(stahovaneVideo.Chyba))
                {
                    // došlo k chybě přesunu souboru
                    continue;
                }

                // uloží ID staženého videa do seznamu
                stazeno.Add(stahovaneVideo.ID);

                backgroundWorkerStahniVidea.ReportProgress(stahovaniReport++);
                prevedeno++;
            }

            // uloží id stažených videí do souboru
            Soubor.Zapis(Path.Combine(slozkaProgramuData, "historie.txt"), stazeno, false);

            e.Result = prevedeno;
        }
        // HOTOVO
        // zobrazí na progressBaru počet již stažených videí
        private void backgroundWorkerStahniVideo_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressBarStatus.Value = e.ProgressPercentage;
            TaskbarManager.Instance.SetProgressValue(e.ProgressPercentage, progressBarStatus.Maximum);
        }
        // HOTOVO
        private void backgroundWorkerStahniVideo_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            progressBarStatus.Value = progressBarStatus.Maximum;
            TaskbarManager.Instance.SetProgressValue(progressBarStatus.Maximum, progressBarStatus.Maximum);
            if (e.Cancelled)
            {
                ZobrazitOperaci("Stahování videí", "Zrušeno uživatelem.");
            }
            else if (e.Error != null)
            {
                ZobrazitOperaci("Stahování videí", "Chyba.");
                Zobrazit.Chybu("Stahování videí", "Došlo k chybě, videa nemohla být stažena.", "Zkuste stáhnout videa znovu.", e.Error.ToString());
            }
            else
            {
                ZobrazitOperaci("Stahování videí", "Úspěšně bylo staženo " + e.Result.ToString() + " videí z(e) " + objectListViewSeznamVidei.CheckedObjects.Count.ToString());
            }
            progressBarStatus.Visible = false;
            TaskbarManager.Instance.SetProgressValue(0, 0);
            menuStahnout.Text = "STÁHNOUT A PŘESUNOUT";
            menuStahnout.Enabled = true;
        }
        // HOTOVO
        /// <summary>
        /// Zobrazuje aktuální proces stahování pomocí youtube-dl.
        /// Aktualizuje stav jednotlivých videí.
        /// </summary>
        private void CteckaVystupu(Object source, DataReceivedEventArgs outLine)
        {
            if (!String.IsNullOrEmpty(outLine.Data))
            {
                string vystup = outLine.Data;
                if (vystup.Contains("[youtube-dl]"))
                {
                    vystup = vystup.Replace("[youtube-dl]", "").Trim();
                    vystup = "Stahování " + vystup;
                }
                else if (vystup.Contains("[download]"))
                {
                    vystup = vystup.Replace("[download]", "").Trim();
                    vystup = "Stahování " + vystup;
                }
                else if (vystup.Contains("[ffmpeg] Destination:"))
                {
                    vystup = vystup.Replace("[ffmpeg] Destination:", "").Trim();
                    vystup = "Převod souboru na hudební formát: '" + vystup + "'";
                }
                else if (vystup.Contains("[ffmpeg]"))
                {
                    vystup = vystup.Replace("[ffmpeg]", "").Trim();
                    vystup = "Převod souboru: " + vystup;
                }
                else
                {
                    return;
                }
                stahovaneVideoVerejne.Stav = vystup;
                objectListViewSeznamVidei.RefreshObject(stahovaneVideoVerejne);
            }
        }
        // DODĚLAT
        /// <summary>
        /// Zobrazuje chybu programu youtube-dl na MessageBox.
        /// </summary>
        private void CteckaVystupuChyby(Object source, DataReceivedEventArgs outLine)
        {
            if (!String.IsNullOrEmpty(outLine.Data))
            {
                MessageBox.Show(outLine.Data);
            }
        }

        // HOTOVO
        /// <summary>
        /// Zapíše metadata do souboru.
        /// </summary>
        /// <param name="stahovaneVideo"></param>
        private void ZapisMetadata(Video stahovaneVideo)
        {
            stahovaneVideo.Stav = "Zápis metadat";
            objectListViewSeznamVidei.RefreshObject(stahovaneVideo);
            try
            {
                string cesta = Path.Combine(slozkaProgramuCache, stahovaneVideo.NazevNovySoubor + ".mp3");
                if (!File.Exists(cesta))
                {
                    stahovaneVideo.Chyba = "Stažený soubor nenalezen";
                    stahovaneVideo.Stav = "";
                    objectListViewSeznamVidei.RefreshObject(stahovaneVideo);
                    return;
                }
                TagLib.File soubor = TagLib.File.Create(cesta);
                soubor.Tag.Year = Convert.ToUInt32(String.Format("{0:yyyy}", stahovaneVideo.Publikovano));
                soubor.Tag.Performers = new string[] { stahovaneVideo.Interpret };
                soubor.Tag.Title = stahovaneVideo.NazevSkladbaFeat;
                soubor.Tag.Genres = new string[] { stahovaneVideo.Zanr };
                Album albumSkladby = stahovaneVideo.Album;
                if (albumSkladby != null)
                {
                    soubor.Tag.Track = Convert.ToUInt32(stahovaneVideo.Stopa);
                    soubor.Tag.Album = albumSkladby.Nazev;
                    NastavCover(albumSkladby.Cover, soubor);
                }
                soubor.Save();
                stahovaneVideo.Stav = "Metadata zapsány";
                objectListViewSeznamVidei.RefreshObject(stahovaneVideo);
            }
            catch (Exception)
            {
                stahovaneVideo.Chyba = "Zapisování metadat se nezdařilo";
                stahovaneVideo.Stav = "";
                objectListViewSeznamVidei.RefreshObject(stahovaneVideo);
            }
        }

        public void NastavCover(string url, TagLib.File file)
        {
            //string path = string.Format(@"{0}temp\{1}.jpg", OutputPath, Guid.NewGuid().ToString());
            byte[] imageBytes;
            using (WebClient client = new WebClient())
            {
                imageBytes = client.DownloadData(url);
            }

            TagLib.Id3v2.AttachedPictureFrame cover = new TagLib.Id3v2.AttachedPictureFrame
            {
                Type = TagLib.PictureType.FrontCover,
                Description = "Cover",
                MimeType = System.Net.Mime.MediaTypeNames.Image.Jpeg,
                Data = imageBytes,
                TextEncoding = TagLib.StringType.UTF16
            };
            file.Tag.Pictures = new TagLib.IPicture[] { cover };
        }

        private void PrevedNaOpus(Video stahovaneVideo)
        {
            Process cmd = new Process();
            ProcessStartInfo psi = new ProcessStartInfo();

            stahovaneVideo.Stav = "Převádění na opus";
            objectListViewSeznamVidei.RefreshObject(stahovaneVideo);

            stahovaneVideoVerejne = stahovaneVideo;
            // nastaví vlastnosti programu na stažení
            string parametry = "-i \"" + stahovaneVideo.NazevNovySoubor + ".mp3\" -acodec libopus -b:a 128000 -vbr on -compression_level 10 -map a \"" + stahovaneVideo.NazevNovySoubor + ".opus\"";
            psi.Arguments = parametry;
            psi.CreateNoWindow = true;
            psi.ErrorDialog = true;
            psi.FileName = cestaFFmpeg;
            psi.RedirectStandardInput = true;
            psi.RedirectStandardOutput = true;
            psi.UseShellExecute = false;
            psi.WorkingDirectory = slozkaProgramuCache;// Path.Combine(, "stazene");
            // spustí program na převod
            /*cmd.OutputDataReceived += new DataReceivedEventHandler(CteckaVystupu);
            cmd.ErrorDataReceived += new DataReceivedEventHandler(CteckaVystupuChyby);*/
                cmd.StartInfo = psi;
            cmd.SynchronizingObject = objectListViewSeznamVidei;
            try
            {
                cmd.Start();
            }
            catch (Exception)
            {
                stahovaneVideo.Chyba = "Převod se nezdařil";
                stahovaneVideo.Stav = "";
                objectListViewSeznamVidei.RefreshObject(stahovaneVideo);
                //continue;
            }
            cmd.BeginOutputReadLine();
            cmd.WaitForExit();
        }

        // HOTOVO
        /// <summary>
        /// Přesune soubor do cílové složky
        /// </summary>
        /// <param name="stahovaneVideo"></param>
        private void PresunSoubor(Video stahovaneVideo)
        {
            // přesun souboru mp3
            stahovaneVideo.Stav = "Přesunování souboru mp3";
            objectListViewSeznamVidei.RefreshObject(stahovaneVideo);

            string cesta = Path.Combine(slozkaProgramuCache, stahovaneVideo.NazevNovySoubor + ".mp3");
            // nahradí opus knihovnu za mp3 knihovnu
            string slozka = stahovaneVideo.Slozka.Replace(hudebniKnihovnaOpus, hudebniKnihovnaMp3);
            if (!Directory.Exists(slozka))
            {
                try
                {
                    Directory.CreateDirectory(slozka);
                }
                catch (Exception ex)
                {
                    stahovaneVideo.Chyba = "Soubor mp3 se nezdařilo přesunout";
                    stahovaneVideo.Stav = "";
                    MessageBox.Show(ex.Message);
                    objectListViewSeznamVidei.RefreshObject(stahovaneVideo);
                    return;
                }
            }
            try
            {
                File.Move(cesta, Path.Combine(slozka, stahovaneVideo.NazevNovySoubor + ".mp3"));
                stahovaneVideo.Stav = "Soubor mp3 přesunut";
                objectListViewSeznamVidei.RefreshObject(stahovaneVideo);
            }
            catch (Exception ex)
            {
                stahovaneVideo.Chyba = "Soubor mp3 se nezdařilo přesunout";
                stahovaneVideo.Stav = "";
                MessageBox.Show(ex.Message);
                objectListViewSeznamVidei.RefreshObject(stahovaneVideo);
            }
            string cover = "cover.jpg";
            if (stahovaneVideo.Album != null && !File.Exists(Path.Combine(slozka, cover)))
            {
                // cover
                using (WebClient webClient = new WebClient())
                {
                    webClient.DownloadFile(stahovaneVideo.Album.Cover, Path.Combine(slozka, cover));
                }
            }

            // přesun souboru opus
            stahovaneVideo.Stav = "Přesunování souboru opus";
            objectListViewSeznamVidei.RefreshObject(stahovaneVideo);

            cesta = Path.Combine(slozkaProgramuCache, stahovaneVideo.NazevNovySoubor + ".opus");
            try
            {
                if (!Directory.Exists(stahovaneVideo.Slozka))
                {
                    Directory.CreateDirectory(stahovaneVideo.Slozka);
                }
            }
            catch (Exception)
            {
                stahovaneVideo.Chyba = "Soubor opus se nezdařilo přesunout";
                stahovaneVideo.Stav = "";
                objectListViewSeznamVidei.RefreshObject(stahovaneVideo);
                return;
            }
            try
            {
                // složka je ok
                File.Move(cesta, Path.Combine(stahovaneVideo.Slozka, stahovaneVideo.NazevNovySoubor + ".opus"));
                if (File.Exists(Path.Combine(slozka, cover)) && !File.Exists(Path.Combine(stahovaneVideo.Slozka, cover)))
                {
                    File.Copy(Path.Combine(slozka, cover), Path.Combine(stahovaneVideo.Slozka, cover));
                }
                stahovaneVideo.Stav = "Soubor opus přesunut";
                objectListViewSeznamVidei.RefreshObject(stahovaneVideo);
            }
            catch (Exception)
            {
                stahovaneVideo.Chyba = "Soubor opus se nezdařilo přesunout";
                stahovaneVideo.Stav = "";
                objectListViewSeznamVidei.RefreshObject(stahovaneVideo);
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
            textBoxOdkaz.Text = "https://www.youtube.com/watch?v=TwOpW6dP7FU"; // video
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
        private void testToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TaskbarManager.Instance.SetProgressValue(0, 0);
            //MessageBox.Show(neco.ToString());
        }

        /*******************************************************************************************************/
        /*******************************************************************************************************/
        /*******************************************************************************************************/
        /*******************************************************************************************************/
        /*******************************************************************************************************/
        /*******************************************************************************************************/
        /*******************************************************************************************************/


        /**
         * 
        SPOUŠTĚNÍ PROGRAMU
         *
        **/

        // HOTOVO 2019
        /// <summary>
        /// Zobrazí MessageBox s chybou.
        /// Následně restartuje nebo ukončí program.
        /// </summary>
        /// <param name="chyba">Zobrazovaný text chyby.</param>
        private void ZobrazChybuSpousteni(string chyba)
        {
            ZobrazitOperaci("Program se nepodařilo spustit.");

            DialogResult odpoved = Zobrazit.Otazku("Spuštění programu", "Program se nepodařilo spustit.", chyba, "Kliknutím na tlačítko 'Znovu' se program spustí znovu.", MessageBoxButtons.RetryCancel);
            if (odpoved == DialogResult.Retry)
            {
                // restatuje program
                Process.Start(Application.ExecutablePath);
            }
            this.Close();
        }

        // HOTOVO 2019
        /// <summary>
        // start programu - spuštění BackgroundWorkeru s nastavením.
        /// </summary>
        private void FormSeznam_Load(object sender, EventArgs e)
        {
            ZobrazitStav("Spouštění programu...");
            ZobrazStatusProgressBar(10);

            this.FormBorderStyle = FormBorderStyle.FixedToolWindow;
            menuStripMenu.Visible = false;

            if (!backgroundWorkerNactiProgram.IsBusy)
            {
                backgroundWorkerNactiProgram.RunWorkerAsync();
            }
            else
            {
                ZobrazChybuSpousteni("");
            }
        }

        // HOTOVO 2019
        // vytvoření složek, stažení youtube-dl + ffmpeg
        /// <summary>
        /// BackgroundWorker načítající nastavení programu.
        /// </summary>
        private void backgroundWorkerNactiProgram_DoWork(object sender, DoWorkEventArgs e)
        {
            backgroundWorkerNactiProgram.ReportProgress(1);
            // 1. složka programu v AppData

            // získání složky programu
            slozkaProgramuData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            if (!Directory.Exists(slozkaProgramuData))
            {
                // neexistuje získaná složka AppData
                e.Result = "slozka_programu";
                return;
            }
            slozkaProgramuData = Path.Combine(slozkaProgramuData, "youtube-renamer");
            slozkaProgramuCache = Path.Combine(slozkaProgramuData, "cache", Process.GetCurrentProcess().Id.ToString());
            slozkaProgramuData = Path.Combine(slozkaProgramuData, "data");
            backgroundWorkerNactiProgram.ReportProgress(2);

            // 2. složka data = nastavení
            if (!Directory.Exists(slozkaProgramuData))
            {
                // vytvoří složku programu (youtube-renamer/data)
                try
                {
                    Directory.CreateDirectory(slozkaProgramuData);
                }
                catch (Exception)
                {
                    e.Result = "slozka_programu";
                    return;
                }
            }
            backgroundWorkerNactiProgram.ReportProgress(3);

            // 3. složka cache = dočasné stažené soubory
            if (Directory.Exists(slozkaProgramuCache))
            {
                // smaže složku programu (youtube-renamer/aktuální id procesu)
                try
                {
                    Directory.Delete(slozkaProgramuCache, true);
                }
                catch (Exception)
                {
                    e.Result = "slozka_programu";
                    return;
                }
            }
            backgroundWorkerNactiProgram.ReportProgress(4);
            try
            {
                // vytvoří složku programu (youtube-renamer/aktuální id procesu)
                Directory.CreateDirectory(slozkaProgramuCache);
            }
            catch (Exception)
            {
                e.Result = "slozka_programu";
                return;
            }
            backgroundWorkerNactiProgram.ReportProgress(5);
            // 3. načtení nastavení
            menuStripMenu.Invoke(new Action(() =>
            {
                // a) cesta hudebních složek opus
                MenuCestaNactiZeSouboru(0);
                backgroundWorkerNactiProgram.ReportProgress(6);
                // a) cesta hudebních složek mp3
                MenuCestaNactiZeSouboru(1);
                backgroundWorkerNactiProgram.ReportProgress(7);
                // b) cesta youtube-dl
                MenuCestaNactiZeSouboru(2);
                backgroundWorkerNactiProgram.ReportProgress(8);
                // c) cesta ffmpeg
                MenuCestaNactiZeSouboru(3);
                backgroundWorkerNactiProgram.ReportProgress(9);
            }));
        }

        // HOTOVO 2019
        /// <summary>
        /// Změna procesu načítání programu.
        /// </summary>
        private void backgroundWorkerNactiProgram_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            // nastaví hodnotu ProgressBaru
            progressBarStatus.Value = e.ProgressPercentage;
            TaskbarManager.Instance.SetProgressValue(e.ProgressPercentage, progressBarStatus.Maximum);
        }

        // HOTOVO 2019
        /// <summary>
        /// Ukončení BackgroundWorkeru načítajícího program.
        /// </summary>
        private void backgroundWorkerNactiProgram_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            // nastaví maximální hodnotu ProgressBaru
            progressBarStatus.Value = progressBarStatus.Maximum;
            TaskbarManager.Instance.SetProgressValue(progressBarStatus.Maximum, progressBarStatus.Maximum);

            // zobrazení výsledků
            if (e.Error != null)
            {
                ZobrazChybuSpousteni("");
            }
            else if ((string)e.Result == "slozka_programu")
            {
                ZobrazChybuSpousteni("Problém se složkou programu");
            }
            else
            {
                ZobrazitOperaci("Program byl úspěšně spuštěn.");
                ZobrazitStav();
                menuStripMenu.Visible = true;
                this.FormBorderStyle = FormBorderStyle.Sizable;
            }

            // skryje se ProgressBar
            progressBarStatus.Visible = false;
            TaskbarManager.Instance.SetProgressValue(0, 0);
        }


        /**
         * 
        SPUŠTĚNÍ PRŮZKUMNÍKU WINDOWS S VYBRANOU CESTOU SLOŽKY (HUDEBNÍ SLOŽKA, YOUTUBE-DL, FFMPEG
         *
        **/

        // HOTOVO 2019
        /// <summary>
        /// Otevře hudební knihovnu opus ve správci souborů.
        /// </summary>
        private void menuNastaveniKnihovnaOpusVybrana_Click(object sender, EventArgs e)
        {
            var menu = sender as ToolStripMenuItem;
            OtevritKnihovnu(hudebniKnihovnaOpus, menu.Text);
        }

        // HOTOVO 2019
        /// <summary>
        /// Otevře hudební knihovnu mp3 ve správci souborů.
        /// </summary>
        private void menuNastaveniKnihovnaMp3Vybrana_Click(object sender, EventArgs e)
        {
            var menu = sender as ToolStripMenuItem;
            OtevritKnihovnu(hudebniKnihovnaMp3, menu.Text);
        }

        // HOTOVO 2019
        /// <summary>
        /// Otevře složku s hudební hnihovnou v průzkumníku souborů.
        /// </summary>
        /// <param name="hudebniKnihovna">Cesta ke složce s hudební knihovnou</param>
        /// <param name="textMenu">Text menu, na které se klikne</param>
        private void OtevritKnihovnu(string hudebniKnihovna, string textMenu)
        {
            if (String.IsNullOrEmpty(hudebniKnihovna))
            {
                hudebniKnihovna = textMenu;
            }
            if (!Directory.Exists(hudebniKnihovna))
            {
                menuNastaveniYoutubeDLCestaVybrana.Text = "Nebyla vybrána žádná složka";
                menuNastaveniYoutubeDLCestaVybrana.Enabled = false;
                hudebniKnihovna = null;
                Zobrazit.Chybu("Spuštění průzkumníku souborů", "Složka hudební knihovny neexistuje.");
                return;
            }
            Spustit.Program(hudebniKnihovna, false);
        }

        // HOTOVO 2019
        /// <summary>
        /// Otevře složku s cestou souboru youtube-dl v průzkumníku souborů a vybere soubor.
        /// </summary>
        private void menuNastaveniCestaYoutubeDLVybrana_Click(object sender, EventArgs e)
        {
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
                Zobrazit.Chybu("Spuštění průzkumníku souborů", "Složka programu YouTube-DL neexistuje.");
                return;
            }
            Spustit.Program("explorer", "/select, \"" + cestaYoutubeDL + "\"", false, false);
        }

        // HOTOVO 2019
        /// <summary>
        /// Otevře složku s cestou souboru ffmpeg v průzkumníku souborů a vybere soubor.
        /// </summary>
        private void menuNastaveniCestaFFmpegVybrana_Click(object sender, EventArgs e)
        {
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
                Zobrazit.Chybu("Spuštění průzkumníku souborů", "Složka programu FFMmpeg neexistuje.");
                return;
            }
            Spustit.Program("explorer", "/select, \"" + cestaFFmpeg + "\"", false, false);
        }


        /**
         * 
        NASTAVENÍ SLOŽKY S HUDEBNÍ KNIHOVNOU NEBO CESTY S YOUTUBE-DL NEBO FFMPEG - vybrání nové složky
         *
        **/

        // HOTOVO 2019
        /// <summary>
        /// Uživatel vybere novou cestu složky hudbní knihovny opus.
        /// </summary>
        private void menuNastaveniKnihovnaOpusZmenit_Click(object sender, EventArgs e)
        {
            HudebniKnihovnaVyber(true);
        }

        // HOTOVO 2019
        /// <summary>
        /// Uživatel vybere novou cestu složky hudbní knihovny mp3.
        /// </summary>
        private void menuNastaveniKnihovnaMp3Zmenit_Click(object sender, EventArgs e)
        {
            HudebniKnihovnaVyber(false);
        }

        // HOTOVO 2019
        /// <summary>
        /// Uživatel vybere pomocí FolderBrowserDialogu novou složku s hudební knihovnou.
        /// Ta se zobrazí v menu a automaticky se prohledají složky v nové hudební knihovně.
        /// </summary>
        /// <param name="opus">
        /// true = hudební knihovna opus,
        /// false = hudební knihovna mp3
        /// </param>
        private void HudebniKnihovnaVyber(bool opus)
        {
            string typ = opus ? "opus" : "mp3";
            string hudebniKnihovna = opus ? hudebniKnihovnaOpus : hudebniKnihovnaMp3;

            // hudební knihovna - uživatel vybere novou cestu složky
            VistaFolderBrowserDialog vyberSlozky = new VistaFolderBrowserDialog();
            vyberSlozky.Description = "Vyberte složku s hudební knihovnou " + typ;
            vyberSlozky.UseDescriptionForTitle = true;
            if (Directory.Exists(hudebniKnihovna))
            {
                vyberSlozky.SelectedPath = hudebniKnihovna;
            }

            if ((bool)vyberSlozky.ShowDialog())
            {
                if (opus)
                {
                    hudebniKnihovnaOpus = vyberSlozky.SelectedPath;                    
                    hudebniKnihovna = hudebniKnihovnaOpus;
                    MenuCestaZobrazit(0);
                }
                else
                {
                    hudebniKnihovnaMp3 = vyberSlozky.SelectedPath;
                    hudebniKnihovna = hudebniKnihovnaMp3;
                    MenuCestaZobrazit(1);
                }
                ZobrazitOperaci("Hudební knihovna " + typ + " úspěšně změněna: '" + hudebniKnihovna + "'");
            }
        }

        // HOTOVO 2019
        /// <summary>
        /// Uživatel změní cestu youtube-dl pomocí OpenFileDialogu, ta se zobrazí v menu.
        /// </summary>
        private void menuNastaveniCestaYoutubeDLZmenit_Click(object sender, EventArgs e)
        {
            OpenFileDialog vyberSouboru = new OpenFileDialog();
            vyberSouboru.Title = "Vyberte spustitelný soubor YoutubeDL:";
            vyberSouboru.Filter = "Spustitelné soubory (*.exe)|*.exe|Všechny soubory (*.*)|*.*";
            if (Directory.Exists(Path.GetDirectoryName(cestaYoutubeDL)))
            {
                vyberSouboru.InitialDirectory = Path.GetDirectoryName(cestaYoutubeDL);
            }
            if (vyberSouboru.ShowDialog() == DialogResult.OK)
            {
                if (vyberSouboru.FilterIndex == 2)
                {
                    // nejedná se o exe soubor
                    Zobrazit.Upozorneni("Změna cesty YouTube-DL", "Nejedná se o spustitelý soubor (*.exe)!", "Program nemusí fungovat správně.");
                }
                cestaYoutubeDL = vyberSouboru.FileName;
                MenuCestaZobrazit(2);
                ZobrazitOperaci("Cesta programu YouTube-DL úspěšně změněna: '" + cestaYoutubeDL + "'");
            }
        }

        // HOTOVO 2019
        /// <summary>
        /// Uživatel změní cestu ffmpeg pomocí OpenFileDialogu, ta se zobrazí v menu.
        /// </summary>
        private void menuNastaveniCestaFFmpegZmenit_Click(object sender, EventArgs e)
        {
            OpenFileDialog vyberSouboru = new OpenFileDialog();
            vyberSouboru.Title = "Vyberte spustitelný soubor FFmpeg:";
            vyberSouboru.Filter = "Spustitelné soubory (*.exe)|*.exe|Všechny soubory (*.*)|*.*";
            if (Directory.Exists(Path.GetDirectoryName(cestaFFmpeg)))
            {
                vyberSouboru.InitialDirectory = Path.GetDirectoryName(cestaFFmpeg);
            }
            if (vyberSouboru.ShowDialog() == DialogResult.OK)
            {
                if (vyberSouboru.FilterIndex == 2)
                {
                    // nejedná se o exe soubor
                    Zobrazit.Upozorneni("Změna cesty FFmpeg", "Nejedná se o spustitelý soubor (*.exe)!", "Program nemusí fungovat správně.");
                }
                cestaFFmpeg = vyberSouboru.FileName;
                MenuCestaZobrazit(3);
                ZobrazitOperaci("Cesta programu FFmpeg úspěšně změněna: '" + cestaFFmpeg + "'");
            }
        }


        /**
         * 
        NASTAVENÍ SLOŽKY S HUDEBNÍ KNIHOVNOU NEBO CESTY S YOUTUBE-DL NEBO FFMPEG - vybrání již dříve vybraných cest z menu
         *
        **/

        // HOTOVO 2019
        /// <summary>
        /// Uživatel vybral novou hudební knihovnu opus ze seznamu již dříve vybraných.
        /// </summary>
        private void menuNastaveniKnihovnaOpusNaposledyVybrane_Click(object sender, EventArgs e)
        {
            var menu = sender as ToolStripMenuItem;
            VyberSlozku(true, menu.Text);
        }

        // HOTOVO 2019
        /// <summary>
        /// Uživatel vybral novou hudební knihovnu mp3 ze seznamu již dříve vybraných.
        /// </summary>
        private void menuNastaveniKnihovnaMp3NaposledyVybrane_Click(object sender, EventArgs e)
        {
            var menu = sender as ToolStripMenuItem;
            VyberSlozku(false, menu.Text);
        }

        /// <summary>
        /// Změní hudební knihovnu na novou z již dříve vybraných a zobrazí ji v menu
        /// </summary>
        /// <param name="opus">
        ///     true = hudební knihovna opus
        ///     false = hudební knihovna mp3
        /// </param>
        /// <param name="hudebniKnihovna">cesta nové složky</param>
        private void VyberSlozku(bool opus, string hudebniKnihovna)
        {
            string typ = opus ? "opus" : "mp3";
            if (Directory.Exists(hudebniKnihovna))
            {
                if (opus)
                {
                    hudebniKnihovnaOpus = hudebniKnihovna;
                    MenuCestaZobrazit(0);
                }
                else
                {
                    hudebniKnihovnaMp3 = hudebniKnihovna;
                    MenuCestaZobrazit(1);
                }
                ZobrazitOperaci("Hudební knihovna " + typ + " úspěšně změněna: '" + hudebniKnihovna + "'");
            }
            else
            {
                ZobrazitOperaci("Hudební knihovna " + typ + " nemohla být změněna. Složka '" + hudebniKnihovna + "' neexistuje.");
                Zobrazit.Chybu("Změna hudební knihovny " + typ, "Hudební knihovna " + typ + " nemohla být změněna.", "Složka '" + hudebniKnihovna + "' neexistuje.", "Zkuste to prosím znovu");
            }
        }

        // HOTOVO 2019
        /// <summary>
        /// Uživatel vybral novou cestu youtube-dl ze seznamu již dříve vybraných.
        /// </summary>
        private void menuNastaveniCestaYoutubeDLNaposledyVybrane_Click(object sender, EventArgs e)
        {
            var menu = sender as ToolStripMenuItem;
            string novaCesta = menu.Text;
            if (File.Exists(novaCesta))
            {
                cestaYoutubeDL = novaCesta;
                MenuCestaZobrazit(2);
                ZobrazitOperaci("Cesta programu YouTube-DL úspěšně změněna: '" + novaCesta + "'");
            }
            else
            {
                ZobrazitOperaci("Cesta programu YouTube-DL nemohla být změněna. Soubor '" + novaCesta + "' neexistuje.");
                Zobrazit.Chybu("Změna cesty programu YouTube-DL", "Cesta nemohla být změněna.", "Soubor '" + novaCesta + "' neexistuje.", "Zkuste to prosím znovu");
            }
        }

        // HOTOVO 2019
        /// <summary>
        /// Uživatel vybral novou cestu ffmpeg ze seznamu již dříve vybraných.
        /// </summary>
        private void menuNastaveniCestaFFmpegNaposledyVybrane_Click(object sender, EventArgs e)
        {
            // ffmpeg - vybrere cestu z již dříve vybraných složek a zobrazí ji v menu
            var menu = sender as ToolStripMenuItem;
            string novaCesta = menu.Text;
            if (File.Exists(novaCesta))
            {
                cestaFFmpeg = novaCesta;
                MenuCestaZobrazit(3);
                ZobrazitOperaci("Cesta programu FFmpeg úspěšně změněna: '" + novaCesta + "'");
            }
            else
            {
                ZobrazitOperaci("Cesta programu FFmpeg nemohla být změněna. Soubor '" + novaCesta + "' neexistuje.");
                Zobrazit.Chybu("Změna cesty programu FFmpeg", "Cesta nemohla být změněna.", "Soubor '" + novaCesta + "' neexistuje.", "Zkuste to prosím znovu");
            }
        }


        /**
         *
        NASTAVENÍ SLOŽKY S HUDEBNÍ KNIHOVNOU NEBO CESTY S YOUTUBE-DL NEBO FFMPEG - ostatní funkce
         *
        **/

        // HOTOVO 2019
        /// <summary>
        /// Načte ze souboru (cesta dle typu) naposledy vybrané cesty (knihovny, youtube-dl, ffmpeg).
        /// Poslední cestu následně uloží jako výchozí.
        /// A přidá cesty do menu.
        /// </summary>
        /// <param name="typ">
        ///     Typ cesty:
        ///     0 = složky knihovny opus
        ///     1 = složky knihovny mp3
        ///     2 = cesty youtube-dl
        ///     3 = cesty ffmpeg
        /// </param>
        private void MenuCestaNactiZeSouboru(int typ)
        {
            // získá cestu souboru
            string cestaSouboru = null;
            if (typ == 0) cestaSouboru = Path.Combine(slozkaProgramuData, "knihovna_opus.txt");
            else if (typ == 1) cestaSouboru = Path.Combine(slozkaProgramuData, "knihovna_mp3.txt");
            else if (typ == 2) cestaSouboru = Path.Combine(slozkaProgramuData, "youtubedl.txt");
            else if (typ == 3) cestaSouboru = Path.Combine(slozkaProgramuData, "ffmpeg.txt");

            // načte ze souboru cesty
            List<string> pridejCesty = Soubor.Precti(cestaSouboru);
            if (pridejCesty == null) return;
            if (pridejCesty.Count == 0) return;

            // projde cesty a přidá je do menu
            foreach (string cesta in pridejCesty)
            {
                MenuCestaPridat(cesta.Trim(), typ);
            }

            // uloží první řádek ze souboru jako výchozí cestu
            if ((typ == 0 || typ == 1) && Directory.Exists(pridejCesty.First().Trim()))
            {
                if (typ == 0) hudebniKnihovnaOpus = pridejCesty.First().Trim();
                else if (typ == 1) hudebniKnihovnaMp3 = pridejCesty.First().Trim();
                MenuCestaZobrazit(typ);
            }
            else if (File.Exists(pridejCesty.First().Trim()))
            {
                if (typ == 2) cestaYoutubeDL = pridejCesty.First().Trim();
                else if (typ == 3) cestaFFmpeg = pridejCesty.First().Trim();
                MenuCestaZobrazit(typ);
            }
        }

        // HOTOVO 2019
        /// <summary>
        /// Přidá danou cestu do menu.
        /// </summary>
        /// <param name="cesta">Cesta, kterou chceme přidat do menu.</param>
        /// <param name="typ">
        ///     Typ cesty:
        ///     0 = složky knihovny opus
        ///     1 = složky knihovny mp3
        ///     2 = cesty youtube-dl
        ///     3 = cesty ffmpeg
        /// </param>
        private void MenuCestaPridat(string cesta, int typ)
        {
            // pokud se nejdená o existující složku / soubor, neuloží se do menu
            if ((typ == 0 || typ == 1) && !Directory.Exists(cesta)) return;
            else if ((typ == 2 || typ == 3) && !File.Exists(cesta)) return;
            // nastavení počátečních proměnných
            ToolStripMenuItem menuPridavane = new ToolStripMenuItem(cesta);
            menuPridavane.Text = cesta;

            // přidá dle typů cestu do menu
            // složky knihovny opus
            if (typ == 0)
            {
                if (menuPridavane.Text == hudebniKnihovnaOpus)
                {
                    menuPridavane.Checked = true;
                }
                else
                {
                    menuPridavane.Checked = false;
                }
                menuPridavane.ToolTipText = "Vybrat složku '" + cesta + "'";
                menuPridavane.Click += new EventHandler(menuNastaveniKnihovnaOpusNaposledyVybrane_Click);
                menuNastaveniKnihovnaOpusNaposledyVybrane.DropDownItems.Add(menuPridavane);
                menuNastaveniKnihovnaOpusNaposledyVybrane.Text = "Naposledy vybrané složky";
                menuNastaveniKnihovnaOpusNaposledyVybrane.Enabled = true;
                return;
            }
            // složky knihovny mp3
            if (typ == 1)
            {
                if (menuPridavane.Text == hudebniKnihovnaMp3)
                {
                    menuPridavane.Checked = true;
                }
                else
                {
                    menuPridavane.Checked = false;
                }
                menuPridavane.ToolTipText = "Vybrat složku '" + cesta + "'";
                menuPridavane.Click += new EventHandler(menuNastaveniKnihovnaMp3NaposledyVybrane_Click);
                menuNastaveniKnihovnaMp3NaposledyVybrane.DropDownItems.Add(menuPridavane);
                menuNastaveniKnihovnaMp3NaposledyVybrane.Text = "Naposledy vybrané složky";
                menuNastaveniKnihovnaMp3NaposledyVybrane.Enabled = true;
                return;
            }

            menuPridavane.ToolTipText = "Vybrat soubor '" + cesta + "'";
            // cesty youtube-dl
            if (typ == 2)
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
            // cesty ffmpeg
            if (typ == 3)
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

        // HOTOVO 2019
        /// <summary>
        /// Zaškrtne nebo odškrtne cestu v menu. Pokud tam není, tak jí přidá a zobrazí cestu v menu.
        /// </summary>
        /// <param name="typ">
        ///     Typ cesty:
        ///     0 = složky knihovny opus
        ///     1 = složky knihovny mp3
        ///     2 = cesty youtube-dl
        ///     3 = cesty ffmpeg
        /// </param>
        private void MenuCestaZobrazit(int typ)
        {
            // nastavení počátečních proměnných
            string cestaVychozi = null;
            ToolStripMenuItem menuNaposledyVybraneCesty = null;
            ToolStripMenuItem menuVybranaCesta = null;
            // složky knihovny opus
            if (typ == 0)
            {
                cestaVychozi = hudebniKnihovnaOpus;
                menuNaposledyVybraneCesty = menuNastaveniKnihovnaOpusNaposledyVybrane;
                menuVybranaCesta = menuNastaveniKnihovnaOpusVybrana;
            }
            // složky knihovny mp3
            else if (typ == 1)
            {
                cestaVychozi = hudebniKnihovnaMp3;
                menuNaposledyVybraneCesty = menuNastaveniKnihovnaMp3NaposledyVybrane;
                menuVybranaCesta = menuNastaveniKnihovnaMp3Vybrana;
            }
            // cesty youtube-dl
            else if (typ == 2)
            {
                cestaVychozi = cestaYoutubeDL;
                menuNaposledyVybraneCesty = menuNastaveniYoutubeDLCestaNaposledyVybrane;
                menuVybranaCesta = menuNastaveniYoutubeDLCestaVybrana;
            }
            // cesty ffmpeg
            else if (typ == 3)
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
            if (typ == 0 || typ == 1)
            {
                menuVybranaCesta.ToolTipText = "Otevřít složku '" + cestaVychozi + "' v průzkumníku souborů";
                menuNastaveniKnihovnaOpusProhledat.Text = "Prohledat hudební knihovnu";
                menuNastaveniKnihovnaOpusProhledat.Enabled = true;
                if (typ == 0)
                {
                    HudebniKnihovnaNajdiSlozky(); // !!!
                }
            }
            else
            {
                menuVybranaCesta.ToolTipText = "Najít soubor '" + cestaVychozi + "' v průzkumníku souborů";
            }
        }
        

        /**
         *
        NASTAVENÍ SLOŽKY S HUDEBNÍ KNIHOVNOU NEBO CESTY S YOUTUBE-DL NEBO FFMPEG - prohledání hudební knihovny
         *
        **/

        // HOTOVO 2019
        /// <summary>
        /// Kliknutí na prohledání hudební knihovny.
        /// </summary>
        private void menuNastaveniKnihovnaOpusProhledat_Click(object sender, EventArgs e)
        {
            HudebniKnihovnaNajdiSlozky();
        }

        // HOTOVO 2019
        /// <summary>
        /// Spustí BackgroundWorker s prohledáním všech složek v hudební knihovně.
        /// Ten odstraní složky alb a další přebytečné.
        /// Získá tedy složky interpretů a ty následně uloží do souboru.
        /// </summary>
        private void HudebniKnihovnaNajdiSlozky()
        {
            ZobrazitStav("Prohledávání hudební knihovny...");
            if (!backgroundWorkerProhledejSlozky.IsBusy)
            {
                backgroundWorkerProhledejSlozky.RunWorkerAsync();
            }
            else
            {
                Zobrazit.Chybu("Prohledávání hudební knihovny", "Zkuste prohledat hudební knihovnu znovu.");
            }
        }

        // HOTOVO 2019
        /// <summary>
        /// BackgroundWorker prohledávající složky hudební knihovny a zapísující je do souboru.
        /// </summary>
        private void backgroundWorkerProhledejSlozky_DoWork(object sender, DoWorkEventArgs e)
        {
            // prohledá složky a zapíše je do souboru
            ZobrazitOperaci("Probíhá prohledávání složek hudební knihovny.");
            ZobrazStatusProgressBar(1);

            if (hudebniKnihovnaOpus == null)
            {
                e.Result = "neexistuje";
                return;
            }
            if (!Directory.Exists(hudebniKnihovnaOpus))
            {
                e.Result = "neexistuje";
                return;
            }

            // získá složky z knihovny, krom složek alb
            List<string> slozkyinterpretu = new List<string>();
            try
            {
                foreach (string cestaSlozky in Directory.GetDirectories(hudebniKnihovnaOpus, "*.*", SearchOption.AllDirectories))
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
            slozkyinterpretu.Insert(0, hudebniKnihovnaOpus);
            // zapíše složky do souboru
            Soubor.Zapis(Path.Combine(slozkaProgramuData, "knihovna_slozky.txt"), slozkyinterpretu);
        }

        // HOTOVO 2019
        /// <summary>
        /// Změna procesu získávání složek.
        /// </summary>
        private void backgroundWorkerProhledejSlozky_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            // nastaví hodnotu ProgressBaru
            progressBarStatus.Value = e.ProgressPercentage;
            TaskbarManager.Instance.SetProgressValue(e.ProgressPercentage, progressBarStatus.Maximum);
        }

        // HOTOVO 2019
        /// <summary>
        /// Ukončení BackgroundWorkeru získávajícího hudební složky.
        /// </summary>
        private void backgroundWorkerProhledejSlozky_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            // nastaví maximální hodnotu ProgressBaru
            progressBarStatus.Value = progressBarStatus.Maximum;
            TaskbarManager.Instance.SetProgressValue(progressBarStatus.Maximum, progressBarStatus.Maximum);

            // zobrazení výsledků
            if (e.Error != null)
            {
                ZobrazitOperaci("Chyba prohledávání.");
                Zobrazit.Chybu("Prohledávání hudební knihovny", "Došlo k chybě, složka s hudební knihovnou nebyla prohledána.", "Zkuste změnit hudební složku.", e.Error.ToString());
            }
            else if ((string)e.Result == "chyba")
            {
                ZobrazitOperaci("Chyba prohledávání.");
                Zobrazit.Chybu("Prohledávání hudební knihovny", "Došlo k chybě, složka s hudební knihovnou nebyla prohledána.", "Zkuste změnit hudební složku.");
            }
            else if ((string)e.Result == "neexistuje")
            {
                ZobrazitOperaci("Neexistující hudební knihovna.");
                Zobrazit.Chybu("Prohledávání hudební knihovny", "Složka s hudební knihovnou neexistuje.", "Změňte prosím hudební složku.");
            }
            else if ((string)e.Result == "zadne_slozky")
            {
                ZobrazitOperaci("Nenalezeny žádné složky.");
                Zobrazit.Chybu("Prohledávání hudební knihovny", "Ve složce s hudební knihovnou nebyly nalezeny žádné složky.", "Změňte prosím hudební složku.");
            }
            else
            {
                ZobrazitOperaci("Hudební knihovna '" + hudebniKnihovnaOpus + "' byla úspěšně prohledána.");
                ZobrazitStav();
            }

            // skryje se ProgressBar
            progressBarStatus.Visible = false;
            TaskbarManager.Instance.SetProgressValue(0, 0);
        }



        /*
         *
        STAŽENÍ YOUTUBE-DL NEBO FFMPEG
         *
        */

        // HOTOVO 2019
        /// <summary>
        /// Kliknutí na stažení programu youtube-dl.
        /// </summary>
        private void menuNastaveniYoutubeDLStahnout_Click(object sender, EventArgs e)
        {
            // stažení youtube-dl
            StahniProgram(true);
        }

        // HOTOVO 2019
        /// <summary>
        /// Kliknutí na stažení programu ffmpeg.
        /// </summary>
        private void menuNastaveniFFmpegStahnout_Click(object sender, EventArgs e)
        {
            // stažení ffmpeg
            StahniProgram(false);
        }

        // HOTOVO 2019
        /// <summary>
        /// Otevře FolderBrowserDialog pro výběr cílové složky stahování.
        /// Následně spustí BackgroundWorker se stahováním programu. 
        /// </summary>
        /// <param name="youtubedl">
        /// Typ souboru ke stažení:
        ///     true = youtube-dl
        ///     false = ffmpeg
        /// </param>
        private void StahniProgram(bool youtubedl)
        {
            string typ = youtubedl ? "YouTube-DL" : "FFmpeg";
            ZobrazitStav("Probíhá stahování...");
            ZobrazitOperaci("Stahování programu " + typ + ".");

            // získání aktuální cesty
            string aktualniCesta = youtubedl ? cestaYoutubeDL : cestaFFmpeg;
            string nazevSouboru = Path.GetFileName(aktualniCesta);
            string aktualniSlozka = Path.GetDirectoryName(aktualniCesta);

            // zobrazení výběru pro uložení souboru
            SaveFileDialog vyberCesty = new SaveFileDialog();
            vyberCesty.Title = "Vyberte cílovou cestu pro uložení programu " + typ;
            if (Directory.Exists(aktualniSlozka))
            {
                vyberCesty.InitialDirectory = aktualniSlozka;
            }
            if (String.IsNullOrEmpty(nazevSouboru))
            {
                nazevSouboru = typ.ToLower() + ".exe";
            }
            vyberCesty.FileName = nazevSouboru;
            vyberCesty.Filter = "Spustitelný soubor (*.exe) | *.exe";

            // získání cílové cesty
            // název souboru bez přípony typu souboru
            // cesta složky
            string cilovaCesta;
            if (vyberCesty.ShowDialog() == DialogResult.OK)
            {
                cilovaCesta = vyberCesty.FileName;
            }
            else
            {
                ZobrazitStav();
                ZobrazitOperaci("Program " + typ + " nebyl stažen.");
                Zobrazit.Chybu("Stahování " + typ, "Program nebyl stažen.", "Nebyla vybrána cílová složka stahování.");
                return;
            }
            // stažení programu
            if (!backgroundWorkerStahniProgram.IsBusy)
            {
                List<string> argumenty = new List<string>();
                argumenty.Add(cilovaCesta);
                argumenty.Add(youtubedl.ToString().ToLower());
                backgroundWorkerStahniProgram.RunWorkerAsync(argumenty);
            }
            else
            {
                ZobrazitOperaci("Program " + typ + " nebyl stažen.");
                Zobrazit.Chybu("Stahování " + typ, "Program nebyl stažen.", "Nelze spustit stahování.", "Zkuste to prosím znovu.");
            }
        }

        // HOTOVO 2019
        /// <summary>
        /// BackgroundWorker stahující program youtube-dl nebo ffmpeg.
        /// </summary>
        private void backgroundWorkerStahniProgram_DoWork(object sender, DoWorkEventArgs e)
        {
            // argument:
            // 1. cílová cesta složky
            // 2. typ:
            //    a) true = youtube-dl
            //    b) false = ffmpeg
            ZobrazStatusProgressBar(7);

            // získání argumentů
            List<string> argumenty = (List<string>)e.Argument;
            string cilovaCesta = argumenty.First();
            string nazevSouboru = Path.GetFileNameWithoutExtension(cilovaCesta);
            string cilovaSlozka = Path.GetDirectoryName(cilovaCesta);
            bool youtubedl = argumenty.Last() == "true" ? true : false;
            string adresaStahovani;

            if (youtubedl)
            {
                adresaStahovani = "https://yt-dl.org/latest/youtube-dl.exe";
                cilovaCesta = Path.Combine(cilovaSlozka, nazevSouboru + ".exe");
            }
            else
            {
                adresaStahovani = "https://ffmpeg.zeranoe.com/builds/win32/static/ffmpeg-latest-win32-static.zip";
                cilovaCesta = Path.Combine(cilovaSlozka, nazevSouboru + ".zip");
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

                stahovac.DownloadFile(adresaStahovani, cilovaCesta);
            }
            catch (Exception)
            {
                e.Result = "stahovani";
                return;
            }

            if (youtubedl)
            {
                backgroundWorkerStahniProgram.ReportProgress(4);
                if (!File.Exists(cilovaCesta))
                {
                    e.Result = "stahovani";
                    return;
                }
                // uložení aktuální cesty a zobrazení v menu
                cestaYoutubeDL = cilovaCesta;
                menuStripMenu.Invoke(new Action(() =>
                {
                    MenuCestaZobrazit(2);
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
                    ZipFile.ExtractToDirectory(cilovaCesta, cilovaSlozka);
                }
                catch (Exception)
                {
                    e.Result = "rozbaleni";
                    return;
                }
                backgroundWorkerStahniProgram.ReportProgress(4);

                // přesun souboru ffmpeg.exe
                string souborCesta = Path.Combine(slozkaFFmpeg, "bin");
                if (!Soubor.Presun(Path.Combine(souborCesta, "ffmpeg.exe"), cilovaSlozka, nazevSouboru + ".exe")) e.Result = "presun";
                if (!Soubor.Presun(Path.Combine(souborCesta, "ffplay.exe"), cilovaSlozka)) e.Result = "presun";
                if (!Soubor.Presun(Path.Combine(souborCesta, "ffprobe.exe"), cilovaSlozka)) e.Result = "presun";
                backgroundWorkerStahniProgram.ReportProgress(5);

                // odstranění složky ffpmeg a zip souboru
                try
                {
                    if (Directory.Exists(slozkaFFmpeg))
                    {
                        Directory.Delete(slozkaFFmpeg, true);
                    }
                    if (File.Exists(cilovaCesta))
                    {
                        File.Delete(cilovaCesta);
                    }
                }
                catch (Exception)
                {
                    e.Result = "odstraneni";
                }
                backgroundWorkerStahniProgram.ReportProgress(6);

                // uložení aktuální cesty a zobrazení v menu
                cestaFFmpeg = Path.Combine(cilovaSlozka, nazevSouboru + ".exe");
                menuStripMenu.Invoke(new Action(() =>
                {
                    MenuCestaZobrazit(3);
                }));
            }
        }

        /// <summary>
        /// Změna procesu stahování programu.
        /// </summary>
        private void backgroundWorkerStahniProgram_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            // nastaví hodnotu ProgressBaru
            progressBarStatus.Value = e.ProgressPercentage;
            TaskbarManager.Instance.SetProgressValue(e.ProgressPercentage, progressBarStatus.Maximum);
        }

        // HOTOVO 2019
        /// <summary>
        /// Ukončení BackgroundWorkeru stahujícího program.
        /// </summary>
        private void backgroundWorkerStahniProgram_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            // nastaví maximální hodnotu ProgressBaru
            progressBarStatus.Value = progressBarStatus.Maximum;
            TaskbarManager.Instance.SetProgressValue(progressBarStatus.Maximum, progressBarStatus.Maximum);
            string vysledek = (string)e.Result;

            // zobrazení výsledků
            if (e.Error != null)
            {
                ZobrazitOperaci("Program se nepodařilo stáhnout.");
                Zobrazit.Chybu("Stahování programu", "Došlo k chybě, program nemohl být stažen.", "Zkuste stáhnout program znovu.", e.Error.ToString());
            }
            else if (vysledek == "cilova_slozka")
            {
                ZobrazitOperaci("Program nebyl stažen. Problém při vytváření cílové složky.");
                Zobrazit.Chybu("Stahování programu", "Program nebyl stažen.", "Problém při vytváření cílové složky.", "Stáhněte program znovu.");
            }
            else if (vysledek == "stahovani")
            {
                ZobrazitOperaci("Program nebyl stažen. Problém při stahování souboru.");
                Zobrazit.Chybu("Stahování programu", "Program nebyl stažen.", "Problém při stahování souboru.", "Stáhněte program znovu.");
            }
            else if (vysledek == "rozbaleni")
            {
                ZobrazitOperaci("Program nebyl stažen. Problém při rozbalování archivu.");
                Zobrazit.Chybu("Stahování programu", "Program nebyl stažen.", "Problém při rozbalování archivu.", "Stáhněte program znovu.");
            }
            else if (vysledek == "presun")
            {
                ZobrazitOperaci("Program nebyl stažen. Problém při přesunování souborů.");
                Zobrazit.Chybu("Stahování programu", "Program nebyl stažen.", "Problém při přesunování souborů.", "Stáhněte program znovu.");
            }
            else if (vysledek == "presun")
            {
                ZobrazitOperaci("Program nebyl stažen. Program byl úspěšně stažen, ale nedošlo k odstranění přebytečných souborů.");
                Zobrazit.Chybu("Stahování programu", "Program úspěšně stažen.", "Chyba odstranění přebytečných souborů. Soubory nebyly odstraněny.");
            }
            else
            {
                ZobrazitOperaci("Program byl úspěšně stažen.");
            }

            // skryje se ProgressBar
            progressBarStatus.Visible = false;
            TaskbarManager.Instance.SetProgressValue(0, 0);
            ZobrazitStav();
        }


        /*
         *
        UKONČENÍ PROGRAMU
         *
        */

        // HOTOVO 2019
        /// <summary>
        /// Ukončení programu.
        /// Uložení nastavení a smazání složky cache.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FormSeznam_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (objectListViewSeznamVidei.Items.Count > 0 || progressBarStatus.Visible)
            {
                // seznam videí není prázdný nebo běží nějaká operace
                if (Zobrazit.Otazku("Ukončení programu","Opravdu chcete ukončit program?", MessageBoxButtons.YesNo) == DialogResult.No)
                {
                    // zeptá se na uzavření programu
                    e.Cancel = true;
                }
            }

            // uloží cesty z menu nastavení do souborů
            List<string> zapisDoSouboru = new List<string>();

            // cesty hudební knihovny opus
            foreach (ToolStripMenuItem menuCesta in menuNastaveniKnihovnaOpusNaposledyVybrane.DropDownItems)
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
            Soubor.Zapis(Path.Combine(slozkaProgramuData, "knihovna_opus.txt"), zapisDoSouboru);
            zapisDoSouboru.Clear();

            // cesty hudební knihovny mp3
            foreach (ToolStripMenuItem menuCesta in menuNastaveniKnihovnaMp3NaposledyVybrane.DropDownItems)
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
            Soubor.Zapis(Path.Combine(slozkaProgramuData, "knihovna_mp3.txt"), zapisDoSouboru);
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
            Soubor.Zapis(Path.Combine(slozkaProgramuData, "youtubedl.txt"), zapisDoSouboru);
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
            Soubor.Zapis(Path.Combine(slozkaProgramuData, "ffmpeg.txt"), zapisDoSouboru);
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
                    Zobrazit.Chybu("Čištění souborů", "Nepodařilo se smazat cache programu.", ex.Message);
                }
            }
        }


        /*
         *
        ODSTRANĚNÍ HISTORIE CEST
         *
        */

        // HOTOVO 2019
        /// <summary>
        /// Kliknutí na smazání historie vybraných cest složky hudbní knihovny opus.
        /// </summary>
        private void menuNastaveniKnihovnaOpusNaposledyVymazat_Click(object sender, EventArgs e)
        {
            VymazatHistoriiMenu("0");
        }

        // HOTOVO 2019
        /// <summary>
        /// Kliknutí na smazání historie vybraných cest složky hudbní knihovny mp3.
        /// </summary>
        private void menuNastaveniKnihovnaMp3NaposledyVymazat_Click(object sender, EventArgs e)
        {
            VymazatHistoriiMenu("1");
        }
        // HOTOVO 2019
        /// <summary>
        /// Kliknutí na smazání historie vybraných cest programu youtube-dl.
        /// </summary>
        private void menuNastaveniYoutubeDLCestaNaposledyVymazat_Click(object sender, EventArgs e)
        {
            VymazatHistoriiMenu("2");
        }

        // HOTOVO 2019
        /// <summary>
        /// Kliknutí na smazání historie vybraných cest programu ffmpeg.
        /// </summary>
        private void menuNastaveniFFmpegCestaNaposledyVymazat_Click(object sender, EventArgs e)
        {
            VymazatHistoriiMenu("3");
        }

        // HOTOVO 2019
        /// <summary>
        /// Odstraní z historie vybrané cesty hudbní knihovny / programů.
        /// </summary>
        /// <param name="typ">
        ///     Typ cesty:
        ///     0 = složky knihovny opus
        ///     1 = složky knihovny mp3
        ///     2 = cesty youtube-dl
        ///     3 = cesty ffmpeg
        /// </param>
        private void VymazatHistoriiMenu(string typ)
        {
            // získání položek
            string aktualneVybrane;
            ToolStripMenuItem menuSlozek;
            // složky knihovny opus
            if (typ == "0")
            {
                menuSlozek = menuNastaveniKnihovnaOpusNaposledyVybrane;
                aktualneVybrane = hudebniKnihovnaOpus;
            }
            // složky knihovny mp3
            else if (typ == "1")
            {
                menuSlozek = menuNastaveniKnihovnaMp3NaposledyVybrane;
                aktualneVybrane = hudebniKnihovnaMp3;
            }
            // cesty youtube-dl
            else if (typ == "2")
            {

                menuSlozek = menuNastaveniYoutubeDLCestaNaposledyVybrane;
                aktualneVybrane = cestaYoutubeDL;
            }
            // cesty ffmpeg
            else if (typ == "3")
            {

                menuSlozek = menuNastaveniFFmpegCestaNaposledyVybrane;
                aktualneVybrane = cestaFFmpeg;
            }
            else
            {
                return;
            }

            // odstranění z menu
            for (int i = 0; i < menuSlozek.DropDownItems.Count; i++)
            {
                if (menuSlozek.DropDownItems[i].Text != aktualneVybrane)
                {
                    // pokud se nejedná o aktuální cestu, odstraním ji z historie
                    menuSlozek.DropDownItems.RemoveAt(i);
                    i--;
                }
            }
        }

        /*
         *
        KONTROLA ODKAZU A ZÍSKÁNÍ ID VIDEA NEBO PLAYLISTU
         *
        */

        // HOTOVO 2019
        /// <summary>
        /// Kliknutí na TextBox s odkazem.
        /// </summary>
        private void textBoxOdkaz_Click(object sender, EventArgs e)
        {
            if (textBoxOdkaz.Text == "VLOŽTE ODKAZ NA VIDEO NEBO PLAYLIST Z YOUTUBE")
            {
                // pokud se jedná o úvodní hlášku, vyberu všechen text
                textBoxOdkaz.SelectAll();
                textBoxOdkaz.Focus();
            }
        }

        // HOTOVO 2019
        /// <summary>
        /// Opuštění TextBoxu s odkazem.
        /// </summary>
        private void textBoxOdkaz_Leave(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(textBoxOdkaz.Text.Trim()))
            {
                // pokud je TextBox prázdný, zobrazí se úvodní hláška
                textBoxOdkaz.Text = "VLOŽTE ODKAZ NA VIDEO NEBO PLAYLIST Z YOUTUBE";
            }
        }

        // HOTOVO
        /// <summary>
        /// Změna textu v TextBoxu s odkazem.
        /// </summary>
        private void textBoxOdkaz_TextChanged(object sender, EventArgs e)
        {
            menuPridatVideo.Enabled = false;
            menuPridatPlaylist.Enabled = false;
            menuPridatAlbum.Enabled = false;
            if (textBoxOdkaz.Text == "VLOŽTE ODKAZ NA VIDEO NEBO PLAYLIST Z YOUTUBE")
            {
                // jedná se o úvodní hlášku, odkaz neexistuje
                menuPridatVideo.Text = "PŘIDAT VIDEO";
                menuPridatPlaylist.Text = "PŘIDAT PLAYLIST";
                menuPridatAlbum.Text = "PŘIDAT ALBUM";
            }
            else
            {
                // nejedná se o úvodní hlášku, zkontoluji, zdali se jedná o validní odkaz
                ZkontrolujOdkaz(textBoxOdkaz.Text);
            }
        }

        // HOTOVO 2019
        /// <summary>
        /// Zkontroluje odkaz na YouTube.
        /// Rozhodne, zdali se jedná o video nebo playlist.
        /// Získá ID videa, nebo playlistu.
        /// </summary>
        /// <param name="odkaz">Odkaz ke kontrole a získání ID.</param>
        private void ZkontrolujOdkaz(string odkaz)
        {
            // https://open.spotify.com/album/6grIHK7vZSucs3CUGqK4IO
            // regexy pro získání id videa nebo playlistu
            Regex video = new Regex("(?:.+?)?(?:\\/v\\/|watch\\/|\\?v=|\\&v=|youtu\\.be\\/|\\/v=|^youtu\\.be\\/)([a-zA-Z0-9_-]{11})+", RegexOptions.Compiled);
            Regex playlist = new Regex(@"(?:http|https|)(?::\/\/|)(?:www.|)(?:youtu\.be\/|youtube\.com(?:\/embed\/|\/v\/|\/watch\?v=|\/ytscreeningroom\?v=|\/feeds\/api\/videos\/|\/user\S*[^\w\-\s]|\S*[^\w\-\s]))([\w\-]{12,})[a-z0-9;:@#?&%=+\/\$_.-]*");
            
            // bohužel youtube music album má jiné id než výsledný youtube playlist, takže takto to nejde
            //Regex playlist = new Regex(@"(?:http|https|)(?::\/\/|)(?:www.|)(?:music.|)(?:youtu\.be\/|youtube\.com(?:\/embed\/|\/v\/|\/watch\?v=||\/browse\/|\/ytscreeningroom\?v=|\/feeds\/api\/videos\/|\/user\S*[^\w\-\s]|\S*[^\w\-\s]))([\w\-]{12,})[a-z0-9;:@#?&%=+\/\$_.-]*");

            string spotifyUrl = null;
            string videoID = null;
            string playlistID = null;
            bool konec = false;

            // kontrola, zdali se jedná spotify url
            if (odkaz.Contains("spotify"))
            {
                spotifyUrl = odkaz;
            }
            else
            {
                // kontrola, zdali se jedná o video
                foreach (Match kontrola in video.Matches(odkaz))
                {
                    foreach (var groupdata in kontrola.Groups.Cast<Group>().Where(groupdata => !groupdata.ToString().StartsWith("http://") && !groupdata.ToString().StartsWith("https://") && !groupdata.ToString().StartsWith("youtu") && !groupdata.ToString().StartsWith("www.")))
                    {
                        // jedná se o video
                        videoID = groupdata.ToString();
                        konec = true;
                        break;
                    }
                    if (konec) break;
                }
                konec = false;

                // kontrola, zdali se jedná o playlist
                foreach (Match kontrola in playlist.Matches(odkaz))
                {
                    foreach (var groupdata in kontrola.Groups.Cast<Group>().Where(groupdata => !groupdata.ToString().StartsWith("http://") && !groupdata.ToString().StartsWith("https://") && !groupdata.ToString().StartsWith("youtu") && !groupdata.ToString().StartsWith("www.")))
                    {
                        // jedná se o playlist
                        playlistID = groupdata.ToString();
                        konec = true;
                        break;
                    }
                    if (konec) break;
                }
                konec = false;
            }

            // konečná kontrola a zobrazení výsledků
            if (String.IsNullOrEmpty(playlistID) && String.IsNullOrEmpty(videoID) && String.IsNullOrEmpty(spotifyUrl))
            {
                // nejedná se o správný odkaz
                menuPridatVideo.Enabled = false;
                menuPridatPlaylist.Enabled = false;
                menuPridatAlbum.Enabled = false;
                menuPridatSpotify.Enabled = false;
                menuPridatVideo.Tag = "";
                menuPridatPlaylist.Tag = "";
                menuPridatSpotify.Tag = "";
                ZobrazitOperaci("Vkládání odkazu", "Vložený odkaz není z YouTube.");
            }
            else if (!String.IsNullOrEmpty(spotifyUrl))
            {
                // získá typ url
                string spotifyTyp = null;
                if (spotifyUrl.Contains("album"))
                {
                    spotifyTyp = "album";
                }
                else if (spotifyUrl.Contains("track"))
                {
                    spotifyTyp = "track";
                }
                else if (spotifyUrl.Contains("playlist"))
                {
                    spotifyTyp = "playlist";
                }
                else if (spotifyUrl.Contains("artist"))
                {
                    spotifyTyp = "artist";
                }
                else
                {
                    // nejedná se o správný odkaz
                    menuPridatVideo.Enabled = false;
                    menuPridatPlaylist.Enabled = false;
                    menuPridatAlbum.Enabled = false;
                    menuPridatSpotify.Enabled = false;
                    menuPridatVideo.Tag = "";
                    menuPridatPlaylist.Tag = "";
                    menuPridatSpotify.Tag = "";
                    ZobrazitOperaci("Vkládání odkazu", "Vložený odkaz je ze Spotify, ale je poškozený.");
                    return;
                }

                // získá id položky
                // spotify:album:6grIHK7vZSucs3CUGqK4IO
                // https://open.spotify.com/artist/4YQK33NdyLe6prjmIjHBcM?si=a036zbQvQW6RY59HVX3dOQ


                // odstraní https://
                if (spotifyUrl.Contains("open"))
                {
                    spotifyUrl = spotifyUrl.Replace("https://", "http://").Replace("http://", "").Replace("open.spotify.com/", "");
                    if (spotifyUrl.Contains("/"))
                    {
                        string spotifyId = spotifyUrl.Replace("/", ":");
                        if (spotifyId.Contains("?"))
                        {
                            spotifyId = spotifyId.Split('?').First();
                        }
                        if (!String.IsNullOrEmpty(spotifyId.Trim()))
                        {
                            spotifyTyp = "spotify:" + spotifyId;
                        }
                        else
                        {
                            spotifyTyp = null;
                        }
                    }
                    else
                    {
                        spotifyTyp = null;
                    }
                    if (String.IsNullOrEmpty(spotifyTyp))
                    {
                        // nejedná se o správný odkaz
                        menuPridatVideo.Enabled = false;
                        menuPridatPlaylist.Enabled = false;
                        menuPridatAlbum.Enabled = false;
                        menuPridatSpotify.Enabled = false;
                        menuPridatVideo.Tag = "";
                        menuPridatPlaylist.Tag = "";
                        menuPridatSpotify.Tag = "";
                        ZobrazitOperaci("Vkládání odkazu", "Vložený odkaz je ze Spotify, ale je poškozený.");
                        return;
                    }
                }
                else if (spotifyUrl.Contains("spotify:"))
                {
                    spotifyTyp = spotifyUrl;
                }
                else
                {
                    // nejedná se o správný odkaz
                    menuPridatVideo.Enabled = false;
                    menuPridatPlaylist.Enabled = false;
                    menuPridatAlbum.Enabled = false;
                    menuPridatSpotify.Enabled = false;
                    menuPridatVideo.Tag = "";
                    menuPridatPlaylist.Tag = "";
                    menuPridatSpotify.Tag = "";
                    ZobrazitOperaci("Vkládání odkazu", "Vložený odkaz je ze Spotify, ale je poškozený.");
                    return;
                }
                // jedná se o platnou spotify url
                menuPridatVideo.Enabled = false;
                menuPridatPlaylist.Enabled = false;
                menuPridatAlbum.Enabled = false;
                menuPridatSpotify.Enabled = true;
                menuPridatVideo.Tag = "";
                menuPridatPlaylist.Tag = "";
                menuPridatSpotify.Tag = spotifyTyp;
                ZobrazitOperaci("Vkládání odkazu", "Vložený odkaz je Spotify " + spotifyTyp + ".");
            }
            else if (String.IsNullOrEmpty(videoID))
            {
                // jedná se pouze o playlist (nejedná se o video)
                menuPridatVideo.Enabled = false;
                menuPridatPlaylist.Enabled = true;
                menuPridatAlbum.Enabled = true;
                menuPridatSpotify.Enabled = false;
                menuPridatVideo.Tag = "";
                menuPridatPlaylist.Tag = playlistID;
                menuPridatSpotify.Tag = "";
                ZobrazitOperaci("Vkládání odkazu", "Vložený odkaz je playlist.");
            }
            else if (String.IsNullOrEmpty(playlistID))
            {
                // jedná se o video (ne o playlist)
                menuPridatVideo.Enabled = true;
                menuPridatPlaylist.Enabled = false;
                menuPridatAlbum.Enabled = false;
                menuPridatSpotify.Enabled = false;
                menuPridatVideo.Tag = videoID;
                menuPridatPlaylist.Tag = "";
                menuPridatSpotify.Tag = "";
                ZobrazitOperaci("Vkládání odkazu", "Vložený odkaz je video.");
            }
            else
            {
                // jedná se o video nebo playlist
                menuPridatVideo.Enabled = true;
                menuPridatPlaylist.Enabled = true;
                menuPridatAlbum.Enabled = true;
                menuPridatSpotify.Enabled = false;
                menuPridatVideo.Tag = videoID;
                menuPridatPlaylist.Tag = playlistID;
                menuPridatSpotify.Tag = "";
                ZobrazitOperaci("Vkládání odkazu", "Vložený odkaz je video i playlist.");
            }
        }


        /*
         *
        ZÍSKÁNÍ INFORMACÍ Z VIDEÍ A ZOBRAZENÍ NA LISTVIEW
         *
        **/


        // HOTOVO 2019
        /// <summary>
        /// Kliknutí na tlačítko s přidáním videa.
        /// Spustí BackgroundWorker s přidáváním videa.
        /// </summary>
        private void menuPridatVideo_Click(object sender, EventArgs e)
        {
            if (menuPridatVideo.Text == "ZASTAVIT PŘIDÁVÁNÍ")
            {
                // zastaví přidávání videí
                menuPridatVideo.Text = "ZASTAVUJI PŘIDÁVÁNÍ";
                menuPridatVideo.Enabled = false;
                menuPridatPlaylist.Enabled = false;
                menuPridatAlbum.Enabled = false;
                backgroundWorkerPridejVidea.CancelAsync();
            }
            else if (menuPridatVideo.Text == "ZASTAVUJI PŘIDÁVÁNÍ") { }
            else
            {
                // spustí přidávání videí
                if (!backgroundWorkerPridejVidea.IsBusy)
                {
                    menuPridatVideo.Text = "ZASTAVIT PŘIDÁVÁNÍ";
                    textBoxOdkaz.ReadOnly = true;
                    menuPridatVideo.Enabled = true;
                    menuPridatPlaylist.Enabled = false;
                    menuPridatAlbum.Enabled = false;

                    List<string> argumenty = new List<string>();
                    argumenty.Add("0"); // typ 0 = video, 1 = playlist, 2 = album
                    argumenty.Add(menuPridatVideo.Tag.ToString()); // id videa / playlistu / albumu
                    backgroundWorkerPridejVidea.RunWorkerAsync(argumenty);
                }
                else
                {
                    Zobrazit.Chybu("Přidávání videa", "Video se nepodařilo přidat.", "Zkuste přidat video znovu.");
                    ZobrazitOperaci("Video se nepodařilo přidat.");
                }
            }
        }
        // hotovo 19
        private void menuPridatPlaylist_Click(object sender, EventArgs e)
        {
            if (menuPridatPlaylist.Text == "ZASTAVIT PŘIDÁVÁNÍ")
            {
                // zastaví přidávání videí
                menuPridatPlaylist.Text = "ZASTAVUJI PŘIDÁVÁNÍ";
                menuPridatVideo.Enabled = false;
                menuPridatPlaylist.Enabled = false;
                menuPridatAlbum.Enabled = false;
                backgroundWorkerPridejVidea.CancelAsync();
            }
            else if (menuPridatPlaylist.Text == "ZASTAVUJI PŘIDÁVÁNÍ") { }
            else
            {
                // spustí přidávání videí
                if (!backgroundWorkerPridejVidea.IsBusy)
                {
                    menuPridatPlaylist.Text = "ZASTAVIT PŘIDÁVÁNÍ";
                    textBoxOdkaz.ReadOnly = true;
                    menuPridatVideo.Enabled = false;
                    menuPridatPlaylist.Enabled = true;
                    menuPridatAlbum.Enabled = false;

                    List<string> argumenty = new List<string>();
                    argumenty.Add("1"); // typ 0 = video, 1 = playlist, 2 = album
                    argumenty.Add(menuPridatPlaylist.Tag.ToString()); // id videa / playlistu / albumu
                    backgroundWorkerPridejVidea.RunWorkerAsync(argumenty);
                }
                else
                {
                    Zobrazit.Chybu("Přidávání videí z playlistu", "Nepodařilo se přidat žádné videa z playlistu.", "Zkuste přidat playlist znovu.");
                    ZobrazitOperaci("Videa z playlistu se nepodařilo přidat.");
                }
            }
        }
        // hotovo 19
        private void menuPridatAlbum_Click(object sender, EventArgs e)
        {
            if (menuPridatAlbum.Text == "ZASTAVIT PŘIDÁVÁNÍ")
            {
                // zastaví přidávání videí
                menuPridatAlbum.Text = "ZASTAVUJI PŘIDÁVÁNÍ";
                menuPridatVideo.Enabled = false;
                menuPridatPlaylist.Enabled = false;
                menuPridatAlbum.Enabled = false;
                backgroundWorkerPridejVidea.CancelAsync();
            }
            else if (menuPridatAlbum.Text == "ZASTAVUJI PŘIDÁVÁNÍ") { }
            else
            {
                // spustí přidávání videí
                if (!backgroundWorkerPridejVidea.IsBusy)
                {
                    if (objectListViewSeznamVidei.Items.Count < 1 || DialogResult.Yes == Zobrazit.Otazku("Přidání albumu", "Přidání nového albumu odstraní všechna aktuální videa.", "Chcete je opravdu odstranit?", MessageBoxButtons.YesNo))
                    {
                        objectListViewSeznamVidei.ClearObjects();
                        videaVsechna.Clear();

                        menuPridatAlbum.Text = "ZASTAVIT PŘIDÁVÁNÍ";
                        textBoxOdkaz.ReadOnly = true;
                        menuPridatVideo.Enabled = false;
                        menuPridatPlaylist.Enabled = false;
                        menuPridatAlbum.Enabled = true;

                        List<string> argumenty = new List<string>();
                        argumenty.Add("2"); // typ 0 = video, 1 = playlist, 2 = album
                        argumenty.Add(menuPridatPlaylist.Tag.ToString()); // id videa / playlistu / albumu
                        backgroundWorkerPridejVidea.RunWorkerAsync(argumenty);
                    }
                }
                else
                {
                    Zobrazit.Chybu("Přidávání videí z albumu", "Nepodařilo se přidat žádné videa z albumu.", "Zkuste přidat album znovu.");
                    ZobrazitOperaci("Videa z albumu se nepodařilo přidat.");
                }
            }
        }

        bool albumVerejne = false;
        string youtubeIDverejne;
        // přidávání videí nebo videí z playlistu
        private void backgroundWorkerPridejVidea_DoWork(object sender, DoWorkEventArgs e)
        {
            // 1. získá video / videa z playlistu
            // 2. získá informace o jednotlivých videích
            // 3. zobrazí videa a ListView

            ZobrazitOperaci("Přidávání videa...");
            ZobrazStatusProgressBar(1);
            if (backgroundWorkerPridejVidea.CancellationPending)
            {
                e.Cancel = true;
                return;
            }
            // argumenty:
            // 1.) typ:
            //     a) 0 = video
            //     b) 1 = playlist
            //     c) 2 = album
            // 2.) id videa / playlistu / albumu
            List<string> argumenty = (List<string>)e.Argument;
            string typ = argumenty.First();
            bool playlist = typ != "0" ? true : false;
            bool album = typ == "2" ? true : false;
            string youtubeID = argumenty.Last();
            youtubeIDverejne = youtubeID;
            albumVerejne = album;

            // získá seznam dříve stažených videí ze souboru
            List<string> stazenaVidea = Soubor.Precti(Path.Combine(slozkaProgramuData, "historie.txt")) ?? new List<string>();

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
                Playlist playlistVidei = new Playlist(youtubeID);
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
                    ZobrazitOperaci("Přidávání videí z playlistu", videoNoveID + " (" + (videaNovaID.IndexOf(videoNoveID) + 1) + " z " + videaNovaID.Count + ")");

                    foreach (Video videoDrivePridane in videaVsechna)
                    {
                        if (videoNoveID == videoDrivePridane.ID)
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
                        //VideoStare noveVideo = new VideoStare(videoNoveID, youtubeID);
                        Video noveVideo = new Video(videoNoveID, playlistVidei, hudebniKnihovnaOpus, vsichniInterpreti);
                        noveVideo.Stopa = videaNovaID.IndexOf(videoNoveID) + 1;

                        // kontrola, zdali video už nebylo stažené dříve (z historie)
                        if (stazenaVidea.Contains(noveVideo.ID))
                        {
                            noveVideo.Chyba = "Video bylo staženo dříve";
                        }

                        videaVsechna.Add(noveVideo);
                        objectListViewSeznamVidei.Invoke(new Action(() =>
                        {
                            objectListViewSeznamVidei.BeginUpdate();
                            objectListViewSeznamVidei.AddObject(noveVideo);
                            objectListViewSeznamVidei.EndUpdate();
                        }));
                    }
                }
                string pridaneDriveText =  pridaneDrive > 0 ? " Nepřidaná videa (" + pridaneDrive + ") byla přidána již dříve." : "";
                ZobrazitOperaci("Přidávání videí z playlistu", "Bylo přidáno " + (videaNovaID.Count() - pridaneDrive) + " videí z " + videaNovaID.Count() + "." + pridaneDriveText);
                /*if (album)
                {
                    FormAlbum uprava = new FormAlbum(youtubeID, videaVsechna, hudebniKnihovnaOpus);
                    uprava.ShowDialog();
                    //uprava.Show();
                    objectListViewSeznamVidei.Invoke(new Action(() =>
                    {
                        objectListViewSeznamVidei.BeginUpdate();
                        objectListViewSeznamVidei.UpdateObjects(videaVsechna);
                        objectListViewSeznamVidei.CheckAll();
                        objectListViewSeznamVidei.EndUpdate();
                    }));
                }*/
            }
            // přidávání jednoho videa
            else
            {
                bool pridano = false;

                backgroundWorkerPridejVidea.ReportProgress(1);
                ZobrazitOperaci("Přidávání videí", youtubeID + " (1 z 1)");

                foreach (Video videoDrivePridane in videaVsechna)
                {
                    if (backgroundWorkerPridejVidea.CancellationPending)
                    {
                        e.Cancel = true;
                        return;
                    }
                    // pokud už bylo toto video přidáno, nepřidá se
                    if (youtubeID == videoDrivePridane.ID)
                    {
                        pridano = true;
                        break;
                    }
                }
                if (pridano)
                {
                    ZobrazitOperaci("Přidávání videí", "Video nebylo přidáno, protože už bylo přidané dříve.");
                }
                else
                {
                    // pokud video nebylo přidáno, přidá se
                    Video noveVideo = new Video(youtubeID, new Playlist(youtubeID, youtubeID), hudebniKnihovnaOpus, vsichniInterpreti);
                    // kontrola, zdali video už nebylo stažené dříve (z historie)
                    if (stazenaVidea.Contains(noveVideo.ID))
                    {
                        noveVideo.Chyba = "Video bylo staženo dříve";
                    }
                    videaVsechna.Add(noveVideo);
                    objectListViewSeznamVidei.Invoke(new Action(() =>
                    {
                        objectListViewSeznamVidei.BeginUpdate();
                        objectListViewSeznamVidei.AddObject(noveVideo);
                        objectListViewSeznamVidei.EndUpdate();
                    }));
                    ZobrazitOperaci("Přidávání videí", "Video bylo úspěšně přidáno.");
                }
            }
        }
        // HOTOVO 19
        private void backgroundWorkerPridejVidea_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            // nastaví hodnotu ProgressBaru
            progressBarStatus.Value = e.ProgressPercentage;
            TaskbarManager.Instance.SetProgressValue(e.ProgressPercentage, progressBarStatus.Maximum);
        }
        // HOTOVO 19
        private void backgroundWorkerPridejVidea_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            // nastaví maximální hodnotu ProgressBaru
            progressBarStatus.Value = progressBarStatus.Maximum;
            TaskbarManager.Instance.SetProgressValue(progressBarStatus.Maximum, progressBarStatus.Maximum);

            if (e.Cancelled)
            {
                ZobrazitOperaci("Přidávání videí zrušeno uživatelem.");
            }
            else if (e.Error != null)
            {
                ZobrazitOperaci("Chyba přidávání videí");
                Zobrazit.Chybu("Přidávání videí", "Došlo k chybě, videa nebyla přidána.", "Zkuste přidat videa znovu.", e.Error.ToString());
            }
            else if ((string)e.Result == "chyba")
            {
                ZobrazitOperaci("Chyba přidávání videí");
                Zobrazit.Chybu("Přidávání videí", "Nepodařilo se získat videa z Youtube API. Videa nebyla přidána.", "Zkuste přidat videa znovu.");
            }
            else if ((string)e.Result == "neexistuje")
            {
                ZobrazitOperaci("Přidávaný playlist neexistuje.");
                Zobrazit.Chybu("Přidávání videí z playlistu", "Tento playlist neexistuje.", "Zkuste přidat jiný playlist.");
            }
            else if ((string)e.Result == "zadne_videa")
            {
                ZobrazitOperaci("V přidávaném playlistu nejsou žádná videa.");
                Zobrazit.Chybu("Přidávání videí z playlistu", "V playlistu nejsou žádná videa.", "Zkuste přidat jiný playlist.");
            }
            // skryje se ProgressBar
            progressBarStatus.Visible = false;
            TaskbarManager.Instance.SetProgressValue(0, 0);
            // změní text v menu ("přidání videa/playlistu")
            ZobrazitStav();
            if (albumVerejne)
            {
                if (objectListViewSeznamVidei.Items.Count > 0)
                {
                    FormAlbum uprava = new FormAlbum(youtubeIDverejne, videaVsechna, hudebniKnihovnaOpus, hudebniKnihovnaMp3);
                    uprava.ShowDialog();
                    //uprava.Show();
                    objectListViewSeznamVidei.Invoke(new Action(() =>
                    {
                        objectListViewSeznamVidei.BeginUpdate();
                        objectListViewSeznamVidei.UpdateObjects(videaVsechna);
                        objectListViewSeznamVidei.CheckAll();
                        objectListViewSeznamVidei.EndUpdate();
                    }));
                }
            }
            textBoxOdkaz.Text = "VLOŽTE ODKAZ NA VIDEO NEBO PLAYLIST Z YOUTUBE";
            textBoxOdkaz.ReadOnly = false;
        }

        // HOTOVO
        /*
        VÝBĚR VIDEÍ ZE SEZNAMU VIDEÍ
        */

        // HOTOVO
        // vybere videa bez chyb
        private void menuVybratBezChyb_Click(object sender, EventArgs e)
        {
            objectListViewSeznamVidei.BeginUpdate();
            objectListViewSeznamVidei.UncheckAll();
            foreach (OLVListItem polozka in objectListViewSeznamVidei.Items)
            {
                // získá instanci videa
                Video prohledavaneVideo = ZiskejVideo(polozka.SubItems[1].Text);
                if (prohledavaneVideo == null)
                {
                    continue;
                }
                if (String.IsNullOrEmpty(prohledavaneVideo.Chyba))
                {
                    // video nemá žádnou chybu
                    polozka.Checked = true;
                }
            }
            objectListViewSeznamVidei.EndUpdate();
        }
        // HOTOVO
        // vybere videa u kterých se nepodařilo najít složku
        private void menuVybratSlozkaNenalezena_Click(object sender, EventArgs e)
        {
            objectListViewSeznamVidei.BeginUpdate();
            objectListViewSeznamVidei.UncheckAll();
            foreach (OLVListItem polozka in objectListViewSeznamVidei.Items)
            {
                // získá instanci videa
                Video prohledavaneVideo = ZiskejVideo(polozka.SubItems[1].Text);
                if (prohledavaneVideo == null)
                {
                    continue;
                }
                if (prohledavaneVideo.Chyba == "Složka nenalezena")
                {
                    // video nemá žádnou chybu
                    polozka.Checked = true;
                }
            }
            objectListViewSeznamVidei.EndUpdate();
        }
        // HOTOVO
        // vybere videa u kterých se nepodařilo přejmenování
        private void menuVybratNeprejmenovana_Click(object sender, EventArgs e)
        {
            objectListViewSeznamVidei.BeginUpdate();
            objectListViewSeznamVidei.UncheckAll();
            foreach (OLVListItem polozka in objectListViewSeznamVidei.Items)
            {
                // získá instanci videa
                Video prohledavaneVideo = ZiskejVideo(polozka.SubItems[1].Text);
                if (prohledavaneVideo == null)
                {
                    continue;
                }
                if (prohledavaneVideo.Chyba == "Nenalezen oddělovač" || prohledavaneVideo.Chyba == "Neexistující název videa")
                {
                    // video nemá žádnou chybu
                    polozka.Checked = true;
                }
            }
            objectListViewSeznamVidei.EndUpdate();
        }
        // HOTOVO
        // vybere videa, která už byla stažena dříve
        private void menuVybratDriveStazene_Click(object sender, EventArgs e)
        {
            objectListViewSeznamVidei.BeginUpdate();
            objectListViewSeznamVidei.UncheckAll();
            foreach (OLVListItem polozka in objectListViewSeznamVidei.Items)
            {
                // získá instanci videa
                Video prohledavaneVideo = ZiskejVideo(polozka.SubItems[1].Text);
                if (prohledavaneVideo == null)
                {
                    continue;
                }
                if (prohledavaneVideo.Chyba == "Video bylo staženo dříve" || prohledavaneVideo.Chyba == "Video bylo nejspíš staženo dříve")
                {
                    // video nemá žádnou chybu
                    polozka.Checked = true;
                }
            }
            objectListViewSeznamVidei.EndUpdate();
        }
        // HOTOVO
        // vybere všechna videa
        private void menuVybratVse_Click(object sender, EventArgs e)
        {
            objectListViewSeznamVidei.BeginUpdate();
            if (objectListViewSeznamVidei.Items.Count > 0)
            {
                try
                {
                    objectListViewSeznamVidei.CheckAll();
                }
                catch (Exception) { }
            }
            objectListViewSeznamVidei.EndUpdate();
        }
        // HOTOVO
        // obrátí výběr
        private void menuVybratObratit_Click(object sender, EventArgs e)
        {
            objectListViewSeznamVidei.BeginUpdate();
            foreach (OLVListItem polozka in objectListViewSeznamVidei.Items)
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
            objectListViewSeznamVidei.EndUpdate();
        }
        // HOTOVO
        //zruší výběr
        private void menuVybratZrusit_Click(object sender, EventArgs e)
        {
            objectListViewSeznamVidei.BeginUpdate();
            try
            {
                objectListViewSeznamVidei.UncheckAll();
            }
            catch (Exception) { }
            objectListViewSeznamVidei.EndUpdate();
        }


        // HOTOVO
        /**
        ODSTRANĚNÍ VYBRANÝCH VIDEÍ, ZMĚNA SEZNAMU VIDEÍ, ZMĚNA VYBRANÝCH VIDEÍ
        **/

        private void menuOdstranit_Click(object sender, EventArgs e)
        {
            // zeptá se na odstranění videí ze seznamu
            if (MessageBox.Show("Opravdu smazat vybraná videa ze seznamu?", "Smazání videí", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            {
                return;
            }

            int odstraneno = 0;
            objectListViewSeznamVidei.BeginUpdate();
            foreach (Video polozka in objectListViewSeznamVidei.CheckedObjects)
            {
                // odstraní vybraná videa ze seznamu a ObjectListView
                objectListViewSeznamVidei.RemoveObject(polozka);
                videaVsechna.Remove(polozka);
                odstraneno++;
            }
            objectListViewSeznamVidei.EndUpdate();

            ZobrazitOperaci("Odstranění videí", "Úspěšně odstraněno " + odstraneno.ToString() + " vide(o/a/í) ze seznamu.");
        }

        // HOTOVO
        /// <summary>
        /// Povolí nebo zakáže možnosti v Menu.
        /// Na základě vybraných videí v ObjectListView.
        /// </summary>
        private void ZmenaVybranychVidei()
        {
            if (objectListViewSeznamVidei.CheckedItems.Count > 0)
            {
                // jsou vybrané videa
                menuUpravit.Enabled = true;
                menuStahnout.Enabled = true;
                menuOdstranit.Enabled = true;
            }
            else
            {
                // nejsou žádné vybrané videa
                menuUpravit.Enabled = false;
                menuStahnout.Enabled = false;
                menuOdstranit.Enabled = false;
            }
            /*if (objectListViewSeznamVidei.Items.Count > 0)
            {
                menuNastaveni.Enabled = false;
            }
            else
            {
                menuNastaveni.Enabled = true;
            }*/
        }
        // HOTOVO
        private void objectListViewSeznamVidei_ItemChecked(object sender, ItemCheckedEventArgs e)
        {
            // zobrazí počet vybraných videí ke stažení 
            // povoluje nebo zakazuje odstranění, úpravu a stažení
            ZobrazitOperaci("Vybráno", objectListViewSeznamVidei.CheckedItems.Count.ToString() + " vide(o/a/í) z(e) " + objectListViewSeznamVidei.Items.Count.ToString());
            ZmenaVybranychVidei();
        }
        // HOTOVO
        private void objectListViewSeznamVidei_ItemsChanged(object sender, ItemsChangedEventArgs e)
        {
            // změna videí na seznamu (přidání / odstranění)
            if (objectListViewSeznamVidei.Items.Count > 0)
            {
                // jsou přidané videa
                menuVybrat.Enabled = true;
            }
            else
            {
                // nejsou žádné videa
                menuVybrat.Enabled = false;
            }
            ZmenaVybranychVidei();
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
            TaskbarManager.Instance.SetProgressValue(progressBarStatus.Minimum, maximum);
        }
        #endregion


        // HOTOVO
        /**
        ZOBRAZENÍ INFORMACÍ - StatusLabel
        - operace = to co se aktuálně provádí podrobněji
        - stav = stav programu (co dělá - pracuje, je připraven atd., více obecně)
        **/
        #region ZobrazStatusLabel
        /// <summary>
        /// Zobrazí vybraný text na StatusStripLabelu.
        /// </summary>
        /// <param name="text">Text k zobrazení.</param>
        private void ZobrazitOperaci(string text)
        {
            statusStripStatus.Invoke(new Action(() =>
            {
                labelOperace.Text = text;
            }));
        }
        /// <summary>
        /// Zobrazí vybraný nadpis a text na StatusStripLabelu.
        /// </summary>
        /// <param name="nadpis">Nadpis k zobrazení před textem.</param>
        /// <param name="text">Text k zobrazení.</param>
        private void ZobrazitOperaci(string nadpis, string text)
        {
            ZobrazitOperaci(nadpis.ToUpper() + ": " + text);
        }
        /// <summary>
        /// Zobrazí vybraný text na StatusStripLabelu.
        /// </summary>
        private void ZobrazitStav()
        {
            ZobrazitStav("Připraven");
        }
        /// <summary>
        /// Zobrazí vybraný text na StatusStripLabelu.
        /// </summary>
        /// <param name="text">Text k zobrazení.</param>
        private void ZobrazitStav(string text)
        {
            statusStripStatus.Invoke(new Action(() =>
            {
                labelStav.Text = text;
            }));
        }
        /// <summary>
        /// Zobrazí vybraný nadpis a text na StatusStripLabelu.
        /// </summary>
        /// <param name="nadpis">Nadpis k zobrazení před textem.</param>
        /// <param name="text">Text k zobrazení.</param>
        private void ZobrazitStav(string nadpis, string text)
        {
            ZobrazitStav(nadpis.ToUpper() + ": " + text);
        }
        #endregion

        /// <summary>
        /// Získá instanci videa pomocí zadaného ID videa.
        /// </summary>
        /// <param name="IDvidea">ID videa k nalezení.</param>
        /// <returns>Instance hledaného videa.</returns>
        private Video ZiskejVideo(string IDvidea)
        {
            foreach (Video prohledavaneVideo in videaVsechna)
            {
                if (String.Compare(prohledavaneVideo.ID, IDvidea, false) == 0)
                {
                    return prohledavaneVideo;
                }
            }
            return null;
        }

        // NOVÉ
        
        private void OpusToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string cesta = @"C:\Program Files (x86)\Mp3tag\Mp3tag.exe";
            List<Video> videaMp3tag = ZiskejVybranaVidea(false);
            foreach (var video in videaMp3tag)
            {
                string soubor = Path.Combine(video.Slozka, video.NazevNovySoubor + ".opus");
                string parametry = "/fn:\"" + soubor + "\"";
                ProcessStartInfo info = new ProcessStartInfo(cesta, parametry);
                Process spust = new Process();
                spust.StartInfo = info;
                spust.StartInfo.CreateNoWindow = false;
                spust.Start();
            }
        }

        private void Mp3ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string cesta = @"C:\Program Files (x86)\Mp3tag\Mp3tag.exe";
            List<Video> videaMp3tag = ZiskejVybranaVidea(false);
            foreach (var video in videaMp3tag)
            {
                string soubor = Path.Combine(video.Slozka.Replace(hudebniKnihovnaOpus, hudebniKnihovnaMp3), video.NazevNovySoubor + ".mp3");
                string parametry = "/fn:\"" + soubor + "\"";
                ProcessStartInfo info = new ProcessStartInfo(cesta, parametry);
                Process spust = new Process();
                spust.StartInfo = info;
                spust.StartInfo.CreateNoWindow = false;
                spust.Start();
            }
        }

        private void albumToolStripMenuItem_Click(object sender, EventArgs e)
        {
            textBoxOdkaz.Text = "https://music.youtube.com/playlist?list=OLAK5uy_nefv5WJefq7o6l9fMnjYmvHN-1HRWqKPQ"; //ALBUM KILLY
        }

        private void menuNastaveniSmazatCache_Click(object sender, EventArgs e)
        {
            if (DialogResult.OK == Zobrazit.Otazku("Smazání cache programu", "Kliknutím na OK vymažete složky cache programu youtube2music.", "Před potvrzením se ujistěte, zdali neběží jiná instance programu youtube2music.", MessageBoxButtons.OKCancel))
            {
                if (!backgroundWorkerSmazCache.IsBusy)
                {
                    menuNastaveniCacheSmazat.Enabled = false;
                    backgroundWorkerSmazCache.RunWorkerAsync();
                }
                else
                {
                    ZobrazitOperaci("Mazání cache programu", "Chyba.");
                    Zobrazit.Chybu("Mazání cache programu", "Došlo k chybě, cache programu nemohlo být smazáno.", "Zkuste smazat cache programu znovu.");
                }
            }
        }

        private void backgroundWorkerSmazCache_DoWork(object sender, DoWorkEventArgs e)
        {
            if (backgroundWorkerSmazCache.CancellationPending)
            {
                e.Cancel = true;
                return;
            }

            // získá složku do které se ukládají cache složky spuštěných instancí programu
            string slozkaCache = Directory.GetParent(slozkaProgramuCache).FullName;

            List<string> slozky = null;
            try
            {
                slozky = Directory.GetDirectories(slozkaCache).ToList();
            }
            catch (Exception)
            {
                e.Result = "ziskani_slozek";
                return;
            }
            foreach (string slozka in slozky)
            {
                if (backgroundWorkerSmazCache.CancellationPending)
                {
                    e.Cancel = true;
                    return;
                }
                // cesty nejsou stejné jako s aktuálně používanou složkou cache
                if (!Path.GetFullPath(slozka).Equals(Path.GetFullPath(slozkaProgramuCache)))
                {
                    try
                    {
                        Directory.Delete(slozka, true);
                    }
                    catch (Exception ex)
                    {
                        e.Result = "mazani_slozky";
                        Zobrazit.Chybu("Mazání cache programu", "Došlo k chybě při mazání složky cache: '" + slozka + "'", ex.Message);
                    }
                }
            }
        }

        private void backgroundWorkerSmazCache_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressBarStatus.Value = e.ProgressPercentage;
            TaskbarManager.Instance.SetProgressValue(e.ProgressPercentage, progressBarStatus.Maximum);
        }

        private void backgroundWorkerSmazCache_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            progressBarStatus.Value = progressBarStatus.Maximum;
            TaskbarManager.Instance.SetProgressValue(progressBarStatus.Maximum, progressBarStatus.Maximum);
            string chyba = (string)e.Result;
            if (e.Cancelled)
            {
                ZobrazitOperaci("Mazání cache programu", "Zrušeno uživatelem.");
            }
            else if (e.Error != null)
            {
                ZobrazitOperaci("Mazání cache programu", "Chyba.");
                Zobrazit.Chybu("Mazání cache programu", "Došlo k chybě, cache programu nemohlo být smazáno.", "Zkuste smazat cache programu znovu.", e.Error.ToString());
            }
            else if (chyba == "ziskani_slozek")
            {
                ZobrazitOperaci("Mazání cache programu", "Některé složky cache programu youtube2music nebyly smazány.");
                Zobrazit.Chybu("Mazání cache programu", "Došlo k chybě, cache programu nemohlo být smazáno.", "Cyhab získávání cache složek.", "Zkuste smazat cache programu znovu.");
            }
            else if (chyba == "mazani_slozky")
            {
                ZobrazitOperaci("Mazání cache programu", "Některé složky cache programu youtube2music nebyly smazány.");
            }
            else
            {
                ZobrazitOperaci("Mazání cache programu", "Úspěšně bylo smazáno cache programu youtube2music");
            }
            progressBarStatus.Visible = false;
            TaskbarManager.Instance.SetProgressValue(0, 0);
            menuNastaveniCacheSmazat.Enabled = true;
        }

        private void menuNastaveniCacheOtevrit_Click(object sender, EventArgs e)
        {
            Spustit.Program(slozkaProgramuCache, true);
        }

        private void menuNastaveniHistorieSmazat_Click(object sender, EventArgs e)
        {
            if (DialogResult.OK == Zobrazit.Otazku("Smazání historie stažených videí", "Kliknutím na OK vymažete veškerou historii stahovaných videí.", "Může pak docházet k duplikátnímu stažení souborů.", MessageBoxButtons.OKCancel))
            {
                if (!backgroundWorkerSmazHistorii.IsBusy)
                {
                    menuNastaveniHistorieSmazat.Enabled = false;
                    backgroundWorkerSmazHistorii.RunWorkerAsync();
                }
                else
                {
                    ZobrazitOperaci("Mazání historie stažených videí", "Chyba.");
                    Zobrazit.Chybu("Mazání historie stažených videí", "Došlo k chybě, historie stažených videí nemohla být smazána.", "Zkuste smazat historii znovu.");
                }
            }
        }

        private void backgroundWorkerSmazHistorii_DoWork(object sender, DoWorkEventArgs e)
        {
            if (backgroundWorkerSmazHistorii.CancellationPending)
            {
                e.Cancel = true;
                return;
            }
            if (!Soubor.Smaz(Path.Combine(slozkaProgramuData, "historie.txt")))
            {
                e.Result = "mazani_souboru";
            }

        }

        private void backgroundWorkerSmazHistorii_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressBarStatus.Value = e.ProgressPercentage;
            TaskbarManager.Instance.SetProgressValue(e.ProgressPercentage, progressBarStatus.Maximum);
        }

        private void backgroundWorkerSmazHistorii_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            progressBarStatus.Value = progressBarStatus.Maximum;
            TaskbarManager.Instance.SetProgressValue(progressBarStatus.Maximum, progressBarStatus.Maximum);
            string chyba = (string)e.Result;
            if (e.Cancelled)
            {
                ZobrazitOperaci("Mazání historie stažených videí", "Zrušeno uživatelem.");
            }
            else if (e.Error != null)
            {
                ZobrazitOperaci("Mazání historie stažených videí", "Chyba.");
                Zobrazit.Chybu("Mazání historie stažených videí", "Došlo k chybě, historie stažených videí nemohla být smazána.", "Zkuste smazat historii znovu.", e.Error.ToString());
            }
            else if (chyba == "mazani_souboru")
            {
                ZobrazitOperaci("Mazání historie stažených videí", "Soubor s historií stažených videí se nepodařilo odstranit.");
            }
            else
            {
                ZobrazitOperaci("Mazání historie stažených videí", "Úspěšně byla smazána historie stažených videí");
            }
            progressBarStatus.Visible = false;
            TaskbarManager.Instance.SetProgressValue(0, 0);
            menuNastaveniHistorieSmazat.Enabled = true;
        }

        private void menuPridatSpotify_Click(object sender, EventArgs e)
        {

        }
    }
}