﻿using System;
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
using youtube2music.App;

namespace youtube2music
{
    public partial class FormSeznam : Form
    {
        /**** PROMĚNNÉ ****/
        List<Video> videaVsechna = new List<Video>();
        SeznamInterpretu vsichniInterpreti = new SeznamInterpretu();

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
            FormUprava uprava = new FormUprava(upravovanaVidea);
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
                Actions.Show.Operation("Stahování videí...");
                if (!backgroundWorkerStahniVidea.IsBusy)
                {
                    //List<Video> videaKeStazeni = ZiskejVybranaVidea(true);
                    List<Video> videaKeStazeni = ZiskejVybranaVidea(false);                    
                    Actions.Show.Progress(videaKeStazeni.Count * 5 + 1);
                    menuStahnout.Text = "ZASTAVIT STAHOVÁNÍ";
                    backgroundWorkerStahniVidea.RunWorkerAsync(videaKeStazeni);
                }
                else
                {
                    Actions.Show.Operation("Chyba stahování videí.", "Stahování videí");
                    Actions.Show.Error("Stahování videí", "Videa se nepodařilo stáhnout.", "Zkuzte stáhnout videa znovu.");
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

                Actions.Show.Operation("Stahuji " + stahovaneVideo.ID + " (" + stahovaneVideoIndex++ + " z " + videaKeStazeni.Count + ")", "Stahování videí");
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
                    string existujiciSouborMp3 = stahovaneVideo.Chyba.Replace(Directories.LibraryOpus, Directories.LibraryMp3).Replace("opus", "mp3"); // UDĚLAT JINAK
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
                psi.FileName = Files.ProgramYoutubedl;
                psi.RedirectStandardInput = true;
                psi.RedirectStandardOutput = true;
                psi.UseShellExecute = false;
                psi.WorkingDirectory = Directories.CurrentCache;


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
            if (!FD.Files.Write(Files.DownloadedVideosHistory, stazeno, false))
            {
                // TODO show error
            }

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
                Actions.Show.Operation("Zrušeno uživatelem.", "Stahování videí");
            }
            else if (e.Error != null)
            {
                Actions.Show.Operation("Chyba.", "Stahování videí");
                Actions.Show.Error("Stahování videí", "Došlo k chybě, videa nemohla být stažena.", "Zkuste stáhnout videa znovu.", e.Error.ToString());
            }
            else
            {
                Actions.Show.Operation("Úspěšně bylo staženo " + e.Result.ToString() + " videí z(e) " + objectListViewSeznamVidei.CheckedObjects.Count.ToString(), "Stahování videí");
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
                string cesta = Path.Combine(Directories.CurrentCache, stahovaneVideo.NazevNovySoubor + ".mp3");
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
            psi.FileName = Files.ProgramFfmpeg;
            psi.RedirectStandardInput = true;
            psi.RedirectStandardOutput = true;
            psi.UseShellExecute = false;
            psi.WorkingDirectory = Directories.CurrentCache;
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

            string cesta = Path.Combine(Directories.CurrentCache, stahovaneVideo.NazevNovySoubor + ".mp3");
            // nahradí opus knihovnu za mp3 knihovnu
            string slozka = stahovaneVideo.Slozka.Replace(Directories.LibraryOpus, Directories.LibraryMp3);
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

            cesta = Path.Combine(Directories.CurrentCache, stahovaneVideo.NazevNovySoubor + ".opus");
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

        /**
         * 
        SPOUŠTĚNÍ PROGRAMU
         *
        **/

        // HOTOVO 2019
        /// <summary>
        // start programu - spuštění BackgroundWorkeru s nastavením.
        /// </summary>
        private void FormSeznam_Load(object sender, EventArgs e)
        {
            Actions.Show.Status("Spouštění programu...");
            Actions.Show.Progress(13);

            this.FormBorderStyle = FormBorderStyle.FixedToolWindow;
            menuStripMenu.Visible = false;

            if (!backgroundWorkerNactiProgram.IsBusy)
            {
                backgroundWorkerNactiProgram.RunWorkerAsync();
            }
            else
            {
                Actions.Show.StartupError();
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

            // 1.) create and get app directories

            // date directory
            if (!FD.Directories.Create(Directories.Data))
            {
                e.Result = "slozka_programu";
                return;
            }
            backgroundWorkerNactiProgram.ReportProgress(2);

            // cache directory
            if (!FD.Directories.Create(Directories.CurrentCache, true))
            {
                e.Result = "slozka_programu";
                return;
            }
            backgroundWorkerNactiProgram.ReportProgress(3);

            // 2.) load settings
            menuStripMenu.Invoke(new Action(() =>
            {
                // login user from YouTube
                if (!YouTube.Init.Login())
                {
                    // TODO show error
                }
                backgroundWorkerNactiProgram.ReportProgress(6);

                // TODO delete toolStripMenuItem1.Visible = false;

                MenuCestaNactiZeSouboru(Init.LibraryOrProgram.LibraryOpus);
                backgroundWorkerNactiProgram.ReportProgress(7);

                MenuCestaNactiZeSouboru(Init.LibraryOrProgram.LibraryMp3);
                backgroundWorkerNactiProgram.ReportProgress(8);

                MenuCestaNactiZeSouboru(Init.LibraryOrProgram.ProgramYoutubedl);
                backgroundWorkerNactiProgram.ReportProgress(9);

                MenuCestaNactiZeSouboru(Init.LibraryOrProgram.ProgramFfmpeg);
                backgroundWorkerNactiProgram.ReportProgress(10);

                MenuCestaNactiZeSouboru(Init.LibraryOrProgram.ProgramMp3tag);
                backgroundWorkerNactiProgram.ReportProgress(11);
            }));
        }

        // HOTOVO 2019
        /// <summary>
        /// Změna procesu načítání programu.
        /// </summary>
        private void backgroundWorkerNactiProgram_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            // nastaví hodnotu ProgressBaru
            int hodnota = e.ProgressPercentage;

            // TODO zde program padal -> spouštěl se backgroundworker s prohledáváním složek, který nastavuje hodnutu max na 1
            if (hodnota <= progressBarStatus.Maximum) progressBarStatus.Value = e.ProgressPercentage;

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
                Actions.Show.StartupError();
                return;
            }
            if ((string)e.Result == "slozka_programu")
            {
                Actions.Show.StartupError("Problém se složkou programu");
                return;
            }
            Actions.Show.Operation("Program byl úspěšně spuštěn.");
            Actions.Show.Status("Ready");
            menuStripMenu.Visible = true;
            this.FormBorderStyle = FormBorderStyle.Sizable;

            // skryje se ProgressBar
            progressBarStatus.Visible = false;
            TaskbarManager.Instance.SetProgressValue(0, 0);
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
        /// <param name="type"></param>
        private void MenuCestaNactiZeSouboru(Init.LibraryOrProgram type)
        {
            // získá cestu souboru
            string cestaSouboru;
            if (type == Init.LibraryOrProgram.LibraryOpus) cestaSouboru = Files.LibraryOpusHistory;
            else if (type == Init.LibraryOrProgram.LibraryMp3) cestaSouboru = Files.LibraryMp3History;
            else if (type == Init.LibraryOrProgram.ProgramYoutubedl) cestaSouboru = Files.ProgramYouTubeDlHistory;
            else if (type == Init.LibraryOrProgram.ProgramFfmpeg) cestaSouboru = Files.ProgramFfmpegHistory;
            else if (type == Init.LibraryOrProgram.ProgramMp3tag) cestaSouboru = Files.ProgramMp3tagHistory;
            else { return; }

            // načte ze souboru cesty
            List<string> pridejCesty = FD.Files.Read(cestaSouboru) ?? new List<string>();
            if (pridejCesty.Count == 0) return;

            // projde cesty a přidá je do menu
            foreach (string cesta in pridejCesty)
            {
                MenuPathHistoryAdd(cesta.Trim(), type);
            }

            // uloží první řádek ze souboru jako výchozí cestu
            if ((type == Init.LibraryOrProgram.LibraryOpus || type == Init.LibraryOrProgram.LibraryMp3) && Directory.Exists(pridejCesty.First().Trim()))
            {
                Init.Library typeLibrary = (type == Init.LibraryOrProgram.LibraryOpus) ? Init.Library.Opus : Init.Library.Mp3;
                LibraryChange(typeLibrary, pridejCesty.First().Trim());
            }
            else if (File.Exists(pridejCesty.First().Trim()))
            {
                Init.Program programType;
                if (type == Init.LibraryOrProgram.ProgramYoutubedl) programType = Init.Program.Youtubedl;
                else if (type == Init.LibraryOrProgram.ProgramFfmpeg) programType = Init.Program.Ffmpeg;
                else if (type == Init.LibraryOrProgram.ProgramMp3tag) programType = Init.Program.Mp3tag;
                else return;
                ProgramChange(programType, pridejCesty.First().Trim());
            }
        }


        /**
         *
        NASTAVENÍ SLOŽKY S HUDEBNÍ KNIHOVNOU NEBO CESTY S YOUTUBE-DL NEBO FFMPEG - prohledání hudební knihovny
         *
        **/

        // TODO remove and automatize
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
            Actions.Show.Status("Prohledávání hudební knihovny...");
            if (!backgroundWorkerProhledejSlozky.IsBusy)
            {
                backgroundWorkerProhledejSlozky.RunWorkerAsync();
            }
            else
            {
                Actions.Show.Error("Prohledávání hudební knihovny", "Zkuste prohledat hudební knihovnu znovu.");
            }
        }

        // HOTOVO 2019
        /// <summary>
        /// BackgroundWorker prohledávající složky hudební knihovny a zapísující je do souboru.
        /// </summary>
        private void backgroundWorkerProhledejSlozky_DoWork(object sender, DoWorkEventArgs e)
        {
            // prohledá složky a zapíše je do souboru
            Actions.Show.Operation("Probíhá prohledávání složek hudební knihovny.");
            //Actions.Show.Progress(1);

            if (Directories.LibraryOpus == null)
            {
                e.Result = "neexistuje";
                return;
            }
            if (!Directory.Exists(Directories.LibraryOpus))
            {
                e.Result = "neexistuje";
                return;
            }

            // získá složky z knihovny, krom složek alb
            List<string> slozkyinterpretu = new List<string>();
            try
            {
                foreach (string cestaSlozky in Directory.GetDirectories(Directories.LibraryOpus, "*.*", SearchOption.AllDirectories))
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

            /*if (slozkyinterpretu == null)
            {
                e.Result = "zadne_slozky";
                return;
            }
            if (slozkyinterpretu.Count() == 0)
            {
                e.Result = "zadne_slozky";
                return;
            }*/

            // seřadí složky
            Actions.Show.Progress(slozkyinterpretu.Count() + 1);
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
            slozkyinterpretu.Insert(0, Directories.LibraryOpus);
            // zapíše složky do souboru
            if (!FD.Files.Write(Path.Combine(Directories.Data, "knihovna_slozky.txt"), slozkyinterpretu))
            {
                // TODO show error
            }
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
                Actions.Show.Operation("Chyba prohledávání.");
                Actions.Show.Error("Prohledávání hudební knihovny", "Došlo k chybě, složka s hudební knihovnou nebyla prohledána.", "Zkuste změnit hudební složku.", e.Error.ToString());
            }
            else if ((string)e.Result == "chyba")
            {
                Actions.Show.Operation("Chyba prohledávání.");
                Actions.Show.Error("Prohledávání hudební knihovny", "Došlo k chybě, složka s hudební knihovnou nebyla prohledána.", "Zkuste změnit hudební složku.");
            }
            else if ((string)e.Result == "neexistuje")
            {
                Actions.Show.Operation("Neexistující hudební knihovna.");
                Actions.Show.Error("Prohledávání hudební knihovny", "Složka s hudební knihovnou neexistuje.", "Změňte prosím hudební složku.");
            }
            else if ((string)e.Result == "zadne_slozky")
            {
                Actions.Show.Operation("Nenalezeny žádné složky.");
                Actions.Show.Error("Prohledávání hudební knihovny", "Ve složce s hudební knihovnou nebyly nalezeny žádné složky.", "Změňte prosím hudební složku.");
            }
            else
            {
                Actions.Show.Operation("Hudební knihovna '" + Directories.LibraryOpus + "' byla úspěšně prohledána.");
                Actions.Show.Status();
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
            //StahniProgram(false);
            // původní link nefunguje
            if (!Actions.Run.Program("https://ffmpeg.org/download.html"))
            {
                // TODO show error
            }
        }

        // HOTOVO 2019
        /// <summary>
        /// Kliknutí na stažení programu mp3tag.
        /// </summary>
        private void menuNastaveniMp3tagStahnout_Click(object sender, EventArgs e)
        {
            // otevření stránky se stažením
            if (!Actions.Run.Program("https://www.mp3tag.de/en/dodownload.html"))
            {
                // TODO show error
            }
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
            Actions.Show.Status("Probíhá stahování...");
            Actions.Show.Operation("Stahování programu " + typ + ".");

            // získání aktuální cesty
            string aktualniCesta = youtubedl ? Files.ProgramYoutubedl : Files.ProgramFfmpeg;
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
                Actions.Show.Status();
                Actions.Show.Operation("Program " + typ + " nebyl stažen.");
                Actions.Show.Error("Stahování " + typ, "Program nebyl stažen.", "Nebyla vybrána cílová složka stahování.");
                return;
            }
            // stažení programu
            if (!backgroundWorkerStahniProgram.IsBusy)
            {
                List<string> argumenty = new List<string>
                {
                    cilovaCesta,
                    youtubedl.ToString().ToLower()
                };
                backgroundWorkerStahniProgram.RunWorkerAsync(argumenty);
            }
            else
            {
                Actions.Show.Operation("Program " + typ + " nebyl stažen.");
                Actions.Show.Error("Stahování " + typ, "Program nebyl stažen.", "Nelze spustit stahování.", "Zkuste to prosím znovu.");
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
            Actions.Show.Progress(7);

            // získání argumentů
            List<string> argumenty = (List<string>)e.Argument;
            string cilovaCesta = argumenty.First();
            string nazevSouboru = Path.GetFileNameWithoutExtension(cilovaCesta);
            string cilovaSlozka = Path.GetDirectoryName(cilovaCesta);
            bool youtubedl = argumenty.Last() == "true";
            string adresaStahovani;

            if (youtubedl)
            {
                adresaStahovani = "https://yt-dl.org/latest/youtube-dl.exe";
                cilovaCesta = Path.Combine(cilovaSlozka, nazevSouboru + ".exe");
            }
            else
            {
                adresaStahovani = "https://ffmpeg.zeranoe.com/builds/win32/static/ffmpeg-latest-win32-static.zip"; // LINK NEFUNGUJE !!
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
                menuStripMenu.Invoke(new Action(() =>
                {
                    ProgramChange(Init.Program.Youtubedl, cilovaCesta);
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
                if (!FD.Files.Move(Path.Combine(souborCesta, "ffmpeg.exe"), cilovaSlozka, nazevSouboru + ".exe")) e.Result = "presun";
                if (!FD.Files.Move(Path.Combine(souborCesta, "ffplay.exe"), cilovaSlozka)) e.Result = "presun";
                if (!FD.Files.Move(Path.Combine(souborCesta, "ffprobe.exe"), cilovaSlozka)) e.Result = "presun";
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
                menuStripMenu.Invoke(new Action(() =>
                {
                    ProgramChange(Init.Program.Ffmpeg, Path.Combine(cilovaSlozka, nazevSouboru + ".exe"));
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
                Actions.Show.Operation("Program se nepodařilo stáhnout.");
                Actions.Show.Error("Stahování programu", "Došlo k chybě, program nemohl být stažen.", "Zkuste stáhnout program znovu.", e.Error.ToString());
            }
            else if (vysledek == "cilova_slozka")
            {
                Actions.Show.Operation("Program nebyl stažen. Problém při vytváření cílové složky.");
                Actions.Show.Error("Stahování programu", "Program nebyl stažen.", "Problém při vytváření cílové složky.", "Stáhněte program znovu.");
            }
            else if (vysledek == "stahovani")
            {
                Actions.Show.Operation("Program nebyl stažen. Problém při stahování souboru.");
                Actions.Show.Error("Stahování programu", "Program nebyl stažen.", "Problém při stahování souboru.", "Stáhněte program znovu.");
            }
            else if (vysledek == "rozbaleni")
            {
                Actions.Show.Operation("Program nebyl stažen. Problém při rozbalování archivu.");
                Actions.Show.Error("Stahování programu", "Program nebyl stažen.", "Problém při rozbalování archivu.", "Stáhněte program znovu.");
            }
            else if (vysledek == "presun")
            {
                Actions.Show.Operation("Program nebyl stažen. Problém při přesunování souborů.");
                Actions.Show.Error("Stahování programu", "Program nebyl stažen.", "Problém při přesunování souborů.", "Stáhněte program znovu.");
            }
            else if (vysledek == "presun")
            {
                Actions.Show.Operation("Program nebyl stažen. Program byl úspěšně stažen, ale nedošlo k odstranění přebytečných souborů.");
                Actions.Show.Error("Stahování programu", "Program úspěšně stažen.", "Chyba odstranění přebytečných souborů. Soubory nebyly odstraněny.");
            }
            else
            {
                Actions.Show.Operation("Program byl úspěšně stažen.");
            }

            // skryje se ProgressBar
            progressBarStatus.Visible = false;
            TaskbarManager.Instance.SetProgressValue(0, 0);
            Actions.Show.Status();
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
                if (Actions.Show.Question("Ukončení programu", MessageBoxButtons.YesNo, "Opravdu chcete ukončit program?") == DialogResult.No)
                {
                    // zeptá se na uzavření programu
                    e.Cancel = true;
                }
            }

            // uloží cesty z menu nastavení do souborů
            List<string> zapisDoSouboru = new List<string>();

            // cesty hudební knihovny opus
            foreach (ToolStripMenuItem menuCesta in menuSettingsLibraryOpusHistory.DropDownItems)
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
            if (!FD.Files.Write(Files.LibraryOpusHistory, zapisDoSouboru))
            {
                // TODO show error
            }
            zapisDoSouboru.Clear();

            // cesty hudební knihovny mp3
            foreach (ToolStripMenuItem menuCesta in menuSettingsLibraryMp3History.DropDownItems)
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
            if (!FD.Files.Write(Files.LibraryMp3History, zapisDoSouboru))
            {
                // TODO show error
            }
            zapisDoSouboru.Clear();

            // cesty youtube-dl
            foreach (ToolStripMenuItem menuCesta in menuSettingsYoutubedlHistory.DropDownItems)
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
            if (!FD.Files.Write(Files.ProgramYouTubeDlHistory, zapisDoSouboru))
            {
                // TODO show error
            }
            zapisDoSouboru.Clear();

            // cesty ffmpeg
            foreach (ToolStripMenuItem menuCesta in menuSettingsFfmpegHistory.DropDownItems)
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
            if (!FD.Files.Write(Files.ProgramFfmpegHistory, zapisDoSouboru))
            {
                // TODO show error 
            }
            zapisDoSouboru.Clear();

            // cesty mp3tag
            foreach (ToolStripMenuItem menuCesta in menuSettingsMp3tagHistory.DropDownItems)
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
            if (!FD.Files.Write(Files.ProgramMp3tagHistory, zapisDoSouboru))
            {
                // TODO show error
            }
            zapisDoSouboru.Clear();

            // delete cache
            if (!FD.Directories.Delete(Directories.CurrentCache))
            {
                Actions.Show.Error("Deleting cache", "Couldn't delete cache directory '" + Directories.CurrentCache + "'.");
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
                menuPridatVideo.Tag = "";
                menuPridatPlaylist.Tag = "";
                Actions.Show.Operation("Vkládání odkazu", "Vložený odkaz není z YouTube.");
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
                    menuPridatVideo.Tag = "";
                    menuPridatPlaylist.Tag = "";
                    Actions.Show.Operation("Vložený odkaz je ze Spotify, ale je poškozený.", "Vkládání odkazu");
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
                        menuPridatVideo.Tag = "";
                        menuPridatPlaylist.Tag = "";
                        Actions.Show.Operation("Vložený odkaz je ze Spotify, ale je poškozený.", "Vkládání odkazu");
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
                    menuPridatVideo.Tag = "";
                    menuPridatPlaylist.Tag = "";
                    Actions.Show.Operation("Vložený odkaz je ze Spotify, ale je poškozený.", "Vkládání odkazu");
                    return;
                }
                // jedná se o platnou spotify url
                menuPridatVideo.Enabled = false;
                menuPridatPlaylist.Enabled = false;
                menuPridatAlbum.Enabled = false;
                menuPridatVideo.Tag = "";
                menuPridatPlaylist.Tag = "";
                Actions.Show.Operation("Vložený odkaz je Spotify " + spotifyTyp + ".", "Vkládání odkazu");
            }
            else if (String.IsNullOrEmpty(videoID))
            {
                // jedná se pouze o playlist (nejedná se o video)
                menuPridatVideo.Enabled = false;
                menuPridatPlaylist.Enabled = true;
                menuPridatAlbum.Enabled = true;
                menuPridatVideo.Tag = "";
                menuPridatPlaylist.Tag = playlistID;
                Actions.Show.Operation("Vložený odkaz je playlist.", "Vkládání odkazu");
            }
            else if (String.IsNullOrEmpty(playlistID))
            {
                // jedná se o video (ne o playlist)
                menuPridatVideo.Enabled = true;
                menuPridatPlaylist.Enabled = false;
                menuPridatAlbum.Enabled = false;
                menuPridatVideo.Tag = videoID;
                menuPridatPlaylist.Tag = "";
                Actions.Show.Operation("Vložený odkaz je video.", "Vkládání odkazu");
            }
            else
            {
                // jedná se o video nebo playlist
                menuPridatVideo.Enabled = true;
                menuPridatPlaylist.Enabled = true;
                menuPridatAlbum.Enabled = true;
                menuPridatVideo.Tag = videoID;
                menuPridatPlaylist.Tag = playlistID;
                Actions.Show.Operation("Vložený odkaz je video i playlist.", "Vkládání odkazu");
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
                    Actions.Show.Error("Přidávání videa", "Video se nepodařilo přidat.", "Zkuste přidat video znovu.");
                    Actions.Show.Operation("Video se nepodařilo přidat.");
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
                    Actions.Show.Error("Přidávání videí z playlistu", "Nepodařilo se přidat žádné videa z playlistu.", "Zkuste přidat playlist znovu.");
                    Actions.Show.Operation("Videa z playlistu se nepodařilo přidat.");
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
                if (String.IsNullOrEmpty(App.Directories.LibraryOpus))
                {
                    Actions.Show.Error("Adding album", "Cannot add album, because you dont have selected your opus music library.");
                    Actions.Show.Operation("Videa z albumu se nepodařilo přidat.");
                    return;
                }
                if (String.IsNullOrEmpty(App.Directories.LibraryMp3))
                {
                    Actions.Show.Error("Adding album", "Cannot add album, because you dont have selected your mp3 music library.");
                    Actions.Show.Operation("Videa z albumu se nepodařilo přidat.");
                    return;
                }
                // spustí přidávání videí
                if (!backgroundWorkerPridejVidea.IsBusy)
                {
                    if (objectListViewSeznamVidei.Items.Count < 1 || DialogResult.Yes == Actions.Show.Question("Přidání albumu", MessageBoxButtons.YesNo, "Přidání nového albumu odstraní všechna aktuální videa.", "Chcete je opravdu odstranit?"))
                    {
                        objectListViewSeznamVidei.ClearObjects();
                        videaVsechna.Clear();

                        menuPridatAlbum.Text = "ZASTAVIT PŘIDÁVÁNÍ";
                        textBoxOdkaz.ReadOnly = true;
                        menuPridatVideo.Enabled = false;
                        menuPridatPlaylist.Enabled = false;
                        menuPridatAlbum.Enabled = true;

                        List<string> argumenty = new List<string>
                        {
                            "2", // typ 0 = video, 1 = playlist, 2 = album
                            menuPridatPlaylist.Tag.ToString() // id videa / playlistu / albumu
                        };
                        backgroundWorkerPridejVidea.RunWorkerAsync(argumenty);
                    }
                }
                else
                {
                    Actions.Show.Error("Přidávání videí z albumu", "Nepodařilo se přidat žádné videa z albumu.", "Zkuste přidat album znovu.");
                    Actions.Show.Operation("Videa z albumu se nepodařilo přidat.");
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

            Actions.Show.Operation("Přidávání videa...");
            Actions.Show.Progress(1);
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
            bool playlist = typ != "0";
            bool album = typ == "2";
            string youtubeID = argumenty.Last();
            youtubeIDverejne = youtubeID;
            albumVerejne = album;

            // získá seznam dříve stažených videí ze souboru
            List<string> stazenaVidea = FD.Files.Read(Files.DownloadedVideosHistory) ?? new List<string>();

            // přidávání playlistu
            if (playlist)
            {
                // získá id videí z youtube api
                List<string> videaNovaID = new List<string>();
                try
                {
                    videaNovaID = YouTube.Api.ZiskejIDVidei(youtubeID);
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
                Actions.Show.Progress(videaNovaID.Count);

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
                    Actions.Show.Operation(videoNoveID + " (" + (videaNovaID.IndexOf(videoNoveID) + 1) + " z " + videaNovaID.Count + ")", "Přidávání videí z playlistu");

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
                        Video noveVideo = new Video(videoNoveID, playlistVidei, vsichniInterpreti);
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
                string pridaneDriveText = pridaneDrive > 0 ? " Nepřidaná videa (" + pridaneDrive + ") byla přidána již dříve." : "";
                Actions.Show.Operation("Bylo přidáno " + (videaNovaID.Count() - pridaneDrive) + " videí z " + videaNovaID.Count() + "." + pridaneDriveText, "Přidávání videí z playlistu");
                /*if (album)
                {
                    FormAlbum uprava = new FormAlbum(youtubeID, videaVsechna, Directories.LibraryOpus);
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
                Actions.Show.Operation(youtubeID + " (1 z 1)", "Přidávání videí");

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
                    Actions.Show.Operation("Video nebylo přidáno, protože už bylo přidané dříve.", "Přidávání videí");
                }
                else
                {
                    // pokud video nebylo přidáno, přidá se
                    Video noveVideo = new Video(youtubeID, new Playlist(youtubeID, youtubeID), vsichniInterpreti);
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
                    Actions.Show.Operation("Video bylo úspěšně přidáno.", "Přidávání videí");
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
                Actions.Show.Operation("Přidávání videí zrušeno uživatelem.");
            }
            else if (e.Error != null)
            {
                Actions.Show.Operation("Chyba přidávání videí");
                Actions.Show.Error("Přidávání videí", "Došlo k chybě, videa nebyla přidána.", "Zkuste přidat videa znovu.", e.Error.ToString());
            }
            else if ((string)e.Result == "chyba")
            {
                Actions.Show.Operation("Chyba přidávání videí");
                Actions.Show.Error("Přidávání videí", "Nepodařilo se získat videa z Youtube API. Videa nebyla přidána.", "Zkuste přidat videa znovu.");
            }
            else if ((string)e.Result == "neexistuje")
            {
                Actions.Show.Operation("Přidávaný playlist neexistuje.");
                Actions.Show.Error("Přidávání videí z playlistu", "Tento playlist neexistuje.", "Zkuste přidat jiný playlist.");
            }
            else if ((string)e.Result == "zadne_videa")
            {
                Actions.Show.Operation("V přidávaném playlistu nejsou žádná videa.");
                Actions.Show.Error("Přidávání videí z playlistu", "V playlistu nejsou žádná videa.", "Zkuste přidat jiný playlist.");
            }
            // skryje se ProgressBar
            progressBarStatus.Visible = false;
            TaskbarManager.Instance.SetProgressValue(0, 0);
            // změní text v menu ("přidání videa/playlistu")
            Actions.Show.Status();
            if (albumVerejne)
            {
                if (objectListViewSeznamVidei.Items.Count > 0)
                {
                    FormAlbum uprava = new FormAlbum(youtubeIDverejne, videaVsechna);
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

            Actions.Show.Operation("Úspěšně odstraněno " + odstraneno.ToString() + " vide(o/a/í) ze seznamu.", "Odstranění videí");
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
                menuMp3tag.Enabled = true;
            }
            else
            {
                // nejsou žádné vybrané videa
                menuUpravit.Enabled = false;
                menuStahnout.Enabled = false;
                menuOdstranit.Enabled = false;
                menuMp3tag.Enabled = false;
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
            Actions.Show.Operation(objectListViewSeznamVidei.CheckedItems.Count.ToString() + " vide(o/a/í) z(e) " + objectListViewSeznamVidei.Items.Count.ToString(), "Vybráno");
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



        private void menuNastaveniSmazatCache_Click(object sender, EventArgs e)
        {
            if (DialogResult.OK == Actions.Show.Question("Smazání cache programu", MessageBoxButtons.OKCancel, "Kliknutím na OK vymažete složky cache programu youtube2music.", "Před potvrzením se ujistěte, zdali neběží jiná instance programu youtube2music."))
            {
                if (!backgroundWorkerSmazCache.IsBusy)
                {
                    menuNastaveniCacheSmazat.Enabled = false;
                    backgroundWorkerSmazCache.RunWorkerAsync();
                }
                else
                {
                    Actions.Show.Operation("Chyba.", "Mazání cache programu");
                    Actions.Show.Error("Mazání cache programu", "Došlo k chybě, cache programu nemohlo být smazáno.", "Zkuste smazat cache programu znovu.");
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
            List<string> slozky;
            try
            {
                slozky = Directory.GetDirectories(Directories.Cache).ToList();
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
                if (!Path.GetFullPath(slozka).Equals(Path.GetFullPath(Directories.CurrentCache)))
                {
                    try
                    {
                        Directory.Delete(slozka, true);
                    }
                    catch (Exception ex)
                    {
                        e.Result = "mazani_slozky";
                        Actions.Show.Error("Mazání cache programu", "Došlo k chybě při mazání složky cache: '" + slozka + "'", ex.Message);
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
                Actions.Show.Operation("Zrušeno uživatelem.", "Mazání cache programu");
            }
            else if (e.Error != null)
            {
                Actions.Show.Operation("Chyba.", "Mazání cache programu");
                Actions.Show.Error("Mazání cache programu", "Došlo k chybě, cache programu nemohlo být smazáno.", "Zkuste smazat cache programu znovu.", e.Error.ToString());
            }
            else if (chyba == "ziskani_slozek")
            {
                Actions.Show.Operation("Některé složky cache programu youtube2music nebyly smazány.", "Mazání cache programu");
                Actions.Show.Error("Mazání cache programu", "Došlo k chybě, cache programu nemohlo být smazáno.", "Cyhab získávání cache složek.", "Zkuste smazat cache programu znovu.");
            }
            else if (chyba == "mazani_slozky")
            {
                Actions.Show.Operation("Některé složky cache programu youtube2music nebyly smazány.", "Mazání cache programu");
            }
            else
            {
                Actions.Show.Operation("Úspěšně bylo smazáno cache programu youtube2music", "Mazání cache programu");
            }
            progressBarStatus.Visible = false;
            TaskbarManager.Instance.SetProgressValue(0, 0);
            menuNastaveniCacheSmazat.Enabled = true;
        }

        private void menuNastaveniCacheOtevrit_Click(object sender, EventArgs e)
        {
            if (!Actions.Run.Program(Directories.Cache, true, false))
            {
                // TODO show error
            }
        }

        private void menuNastaveniHistorieSmazat_Click(object sender, EventArgs e)
        {
            if (DialogResult.OK == Actions.Show.Question("Smazání historie stažených videí", MessageBoxButtons.OKCancel, "Kliknutím na OK vymažete veškerou historii stahovaných videí.", "Může pak docházet k duplikátnímu stažení souborů."))
            {
                if (!backgroundWorkerSmazHistorii.IsBusy)
                {
                    menuNastaveniHistorieSmazat.Enabled = false;
                    backgroundWorkerSmazHistorii.RunWorkerAsync();
                }
                else
                {
                    Actions.Show.Operation("Chyba.", "Mazání historie stažených videí");
                    Actions.Show.Error("Mazání historie stažených videí", "Došlo k chybě, historie stažených videí nemohla být smazána.", "Zkuste smazat historii znovu.");
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
            if (!FD.Files.Delete(Files.DownloadedVideosHistory))
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
                Actions.Show.Operation("Zrušeno uživatelem.", "Mazání historie stažených videí");
            }
            else if (e.Error != null)
            {
                Actions.Show.Operation("Chyba.", "Mazání historie stažených videí");
                Actions.Show.Error("Mazání historie stažených videí", "Došlo k chybě, historie stažených videí nemohla být smazána.", "Zkuste smazat historii znovu.", e.Error.ToString());
            }
            else if (chyba == "mazani_souboru")
            {
                Actions.Show.Operation("Soubor s historií stažených videí se nepodařilo odstranit.", "Mazání historie stažených videí");
            }
            else
            {
                Actions.Show.Operation("Úspěšně byla smazána historie stažených videí", "Mazání historie stažených videí");
            }
            progressBarStatus.Visible = false;
            TaskbarManager.Instance.SetProgressValue(0, 0);
            menuNastaveniHistorieSmazat.Enabled = true;
        }


        private void menuMp3tagOpus_Click(object sender, EventArgs e)
        {
            List<Video> videaMp3tag = ZiskejVybranaVidea(false);
            foreach (var video in videaMp3tag)
            {
                string soubor = Path.Combine(video.Slozka, video.NazevNovySoubor + ".opus");
                if (video.Stav == "Bude smazáno")
                {
                    if (File.Exists(video.Chyba))
                    {
                        soubor = video.Chyba.Replace(".opus", ".mp3");
                    }
                    else
                    {
                        soubor = video.Slozka.Replace(Directories.LibraryOpus, Directories.LibraryMp3);
                        if (!soubor.Contains(".opus"))
                        {
                            soubor += ".opus";
                        }
                    }
                }
                else if (video.Chyba == "Video bylo staženo dříve" || video.Chyba == "Video bylo nejspíš staženo dříve")
                {
                    soubor = video.Slozka;
                }
                else if (video.Stav != "Soubor opus přesunut")
                {
                    continue;
                }
                if (!Actions.Run.Program(Files.ProgramMp3tag, true, true, "/fn:\"" + soubor + "\""))
                {
                    // TODO show error
                }            
            }
        }

        private void menuMp3tagMp3_Click(object sender, EventArgs e)
        {
            List<Video> videaMp3tag = ZiskejVybranaVidea(false);
            foreach (var video in videaMp3tag)
            {
                string soubor = Path.Combine(video.Slozka.Replace(Directories.LibraryOpus, Directories.LibraryMp3), video.NazevNovySoubor);
                if (video.Stav == "Bude smazáno")
                {
                    if (File.Exists(video.Chyba))
                    {
                        soubor = video.Chyba.Replace(".opus", ".mp3");
                    }
                    else
                    {
                        soubor = video.Slozka.Replace(Directories.LibraryOpus, Directories.LibraryMp3);
                        if (!soubor.Contains(".opus"))
                        {
                            soubor += ".opus";
                        }
                        soubor = soubor.Replace(".opus", ".mp3");
                    }
                }
                else if ((video.Chyba == "Video bylo staženo dříve" || video.Chyba == "Video bylo nejspíš staženo dříve") && soubor.Contains(".opus"))
                {
                    soubor = soubor.Replace(".opus", ".mp3");
                }
                else if (video.Chyba == "Video bylo staženo dříve" || video.Chyba == "Video bylo nejspíš staženo dříve")
                {
                    soubor += ".mp3";
                }
                else if (video.Stav != "Soubor opus přesunut")
                {
                    continue;
                }
                else
                {
                    soubor += ".mp3";
                }
                if (!Actions.Run.Program(Files.ProgramMp3tag, true, true, "/fn:\"" + soubor + "\""))
                {
                    // TODO show error
                }
            }
        }

        private void menuNastaveniHistorieOtevrit_Click(object sender, EventArgs e)
        {
            if (!Actions.Run.Program(Files.DownloadedVideosHistory, true))
            {
                // TODO show error
            }
        }

        private void menuNastaveniUzivatel_Click(object sender, EventArgs e)
        {
            if (!Actions.Run.Program(YouTube.Init.LoggedInUser.ChannelUrl))
            {
                // TODO show error
            }
        }

        private void menuNastaveniUzivatelOdhlasit_Click(object sender, EventArgs e)
        {
            try
            {
                File.Delete(Files.YouTubeUser);
            }
            catch (Exception ex)
            {
                Actions.Show.Error("Odhlášení uživatele", "Odhlášení uživatele se nezdařilo", ex.Message);
                Actions.Show.Operation("Odhlášení z YouTube se nezdařilo.");
                return;
            }
            menuNastaveniUzivatel.Enabled = false;
            menuNastaveniUzivatelOdhlasit.Enabled = false;
            menuNastaveniUzivatel.Text = "Přihlášený uživatel z YouTube:";
            menuNastaveniUzivatel.ToolTipText = "";
            menuNastaveniUzivatelOdhlasit.Text = "Není přihlášen žádný uživatel";
            Actions.Show.Operation("Odhlášení z YouTube proběhlo úspěšně.");
        }
    }
}