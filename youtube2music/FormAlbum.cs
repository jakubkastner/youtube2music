using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Windows.Forms;
using Ookii.Dialogs.Wpf;
using youtube2music.App;

namespace youtube2music
{
    public partial class FormAlbum : Form
    {
        string playlistID = "";
        string hudebniKnihovnaOpus = "";
        string hudebniKnihovnaMp3 = "";
        List<Video> videaVsechna = new List<Video>();
        List<Deezer> nalezenaAlba = new List<Deezer>();

        /// <summary>
        /// Získá proměnné
        /// </summary>
        /// <param name="playlist">Přidávaný playlist z youtube</param>
        /// <param name="videa">Seznam všech videí</param>
        /// <param name="hudebniKnihOpus">Cesta k hudební knihovně opus</param>
        /// <param name="hudebniKnihMp3">Cesta k hudební knihovně mp3</param>
        public FormAlbum(string playlist, List<Video> videa, string hudebniKnihOpus, string hudebniKnihMp3)
        {
            InitializeComponent();
            playlistID = playlist;
            videaVsechna = videa;
            hudebniKnihovnaOpus = hudebniKnihOpus;
            hudebniKnihovnaMp3 = hudebniKnihMp3;
        }

        /// <summary>
        /// Spustí formulář
        /// </summary>
        private void FormAlbum_Load(object sender, EventArgs e)
        {
            numericUpDownRok.Maximum = DateTime.Now.Year;
            numericUpDownRok.Value = numericUpDownRok.Maximum;
            // zobrazí získaná videa z youtube na seznamu
            treeListViewAlbaYoutube.Invoke(new Action(() =>
            {
                treeListViewAlbaYoutube.BeginUpdate();
                treeListViewAlbaYoutube.AddObjects(videaVsechna);
                treeListViewAlbaYoutube.EndUpdate();
            }));

            toolTipInformace.SetToolTip(pictureBoxCoverPredni, null);
            toolTipInformace.SetToolTip(pictureBoxCoverZadni, null);
            toolTipInformace.SetToolTip(buttonCoverPredniSmazat, "Smazat přední cover");
            toolTipInformace.SetToolTip(buttonCoverZadniSmazat, "Smazat zadní cover");
            buttonCoverPredniZmenit.Text = "Přidat přední cover";
            buttonCoverZadniZmenit.Text = "Přidat zadní cover";

            // rozbalení položek seznamu albumů deezer
            treeListViewAlbaDeezer.CanExpandGetter = delegate (Object x)
            {
                return (x is Deezer);
            };
            // přidání skladeb (podpoložek) k albumu
            treeListViewAlbaDeezer.ChildrenGetter = delegate (Object x)
            {
                if (x is Deezer)
                {
                    return ((Deezer)x).Skladby;
                }
                throw new ArgumentException("Musí být album nebo interpret");
            };

            // získání informací o playlistu youtube
            Playlist playlistAlba = new Playlist(playlistID);
            string interpret = "";
            string album = playlistAlba.Nazev.Replace("Album - ", "");
            linkLabelOdkaz.Text = playlistAlba.Url;
            this.Text = album + " - nové album ~ youtube2music";

            // nastaví hodnotu interpreta ro první video
            if (videaVsechna.Count > 0)
            {
                interpret = videaVsechna.First().Interpret;
            }
            textBoxInterpret.Text = interpret;
            textBoxAlbum.Text = album;
            DeezerVyhledej(interpret, album);
            if (treeListViewAlbaDeezer.Items.Count < 1)
            {
                // nenalezeno žádné album
                Interpret interpretAlba = new Interpret(textBoxInterpret.Text);
                interpretAlba.NajdiSlozky();

                Album novyAlbum = new Album(textBoxAlbum.Text, Convert.ToInt32(numericUpDownRok.Value), interpretAlba, hudebniKnihovnaOpus);
                textBoxSlozka.Text = novyAlbum.Slozka;
                textBoxZanr.Text = novyAlbum.Zanr;
                if (String.IsNullOrEmpty(textBoxSlozka.Text))
                {
                    // nejedná se o existující složku ani soubor
                    textBoxSlozka.Text = hudebniKnihovnaOpus;
                }
            }


            // získej žánry hudebních složek
            List<string> zanrySlozky = new List<string>();
            List<string> zanrySlozkyMp3 = ZiskejNazvySlozek(hudebniKnihovnaOpus);
            List<string> zanrySlozkyOpus = ZiskejNazvySlozek(hudebniKnihovnaMp3);
            if (zanrySlozkyMp3 != null)
            {
                zanrySlozky = zanrySlozkyMp3;
            }
            if (zanrySlozkyOpus != null)
            {
                foreach (string slozkaOpus in zanrySlozkyOpus)
                {
                    if (!zanrySlozkyMp3.Contains(slozkaOpus))
                    {
                        zanrySlozky.Add(slozkaOpus);
                    }
                }
            }
            if (zanrySlozky.Count > 0)
            {
                foreach (string slozka in zanrySlozky)
                {
                    comboBoxZanr.Items.Add(slozka);

                }
                if (comboBoxZanr.Items.Count > 0)
                {
                    comboBoxZanr.SelectedIndex = 0;
                }
            }
        }

        private List<String> ZiskejNazvySlozek(string slozka)
        {
            // získám soubory ze složky
            List<string> seznamSlozek = null;
            try
            {
                seznamSlozek = Directory.GetDirectories(slozka).ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Načítání složek hudební knihovny");
                return null;
            }
            // získám názvy složek
            List<string> seznamSlozekNazvy = new List<string>();
            foreach (string slozkaSeznam in seznamSlozek)
            {
                string nazevSlozky = new DirectoryInfo(slozkaSeznam).Name;
                seznamSlozekNazvy.Add(nazevSlozky);
            }
            return seznamSlozekNazvy;
        }

        /// <summary>
        /// Spustí vyhledávání albumu (interpreta) na deezeru.
        /// </summary>
        private void ButtonVyhledatDeezer_Click(object sender, EventArgs e)
        {
            DeezerVyhledej(textBoxInterpret.Text, textBoxAlbum.Text/*, checkBoxVyhledatDeezerSingly.Checked*/);
        }

        /// <summary>
        /// Spustí vyhledávání albumu (interpreta) na deezeru.
        /// </summary>
        /// <param name="interpret">Interpet k vyhledání</param>
        /// <param name="album">Album v vyhledání</param>
        /// <param name="vyhledatSingly">Vyhledat singly na deezeru true = singly, ep, albumy; false = ep, albumy</param>
        private void DeezerVyhledej(string interpret, string album/*, bool vyhledatSingly*/)
        {
            if (buttonVyhledatDeezer.Text == "Zrušit vyhledávání")
            {
                backgroundWorkerVyhledatDeezer.CancelAsync();
                buttonVyhledatDeezer.Text = "Vyhledat na Deezeru";
            }
            else
            {
                if (!backgroundWorkerVyhledatDeezer.IsBusy)
                {
                    buttonVyhledatDeezer.Text = "Zrušit vyhledávání";
                    backgroundWorkerVyhledatDeezer.RunWorkerAsync();
                }
                else
                {
                    buttonVyhledatDeezer.Text = "Ruším vyhledávání";
                    buttonVyhledatDeezer.Enabled = false;
                }
            }
        }

        private void BackgroundWorkerVyhledatDeezer_DoWork(object sender, DoWorkEventArgs e)
        {
            /// DODĚLAT
            /// nazastavuje se pokud dojde k chybě !!!!!

            if (backgroundWorkerVyhledatDeezer.CancellationPending)
            {
                e.Cancel = true;
                return;
            }

            string interpet = OdstranZnaky(textBoxInterpret.Text);
            string album = OdstranZnaky(textBoxAlbum.Text);
            bool vyhledatSingly = checkBoxVyhledatDeezerSingly.Checked;

            // získá alba umělce
            if (!ZiskejAlba("https://api.deezer.com/search/album?q=artist:\"" + interpet + "\" album:\"" + album + "\"?access_token=", true, vyhledatSingly))
            {
                e.Cancel = true;
                return;
            }
        }

        private void BackgroundWorkerVyhledatDeezer_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled)
            {
            }
            else if (e.Error != null)
            {
                Actions.Show.Error("Hledání albumu na Deezeru", "Došlo k chybě, album nemohlo být nalezeno na Deezeru.", e.Error.ToString());
            }
            else
            {
                string umelec = OdstranZnaky(textBoxInterpret.Text);
                string album = OdstranZnaky(textBoxAlbum.Text);
                foreach (Deezer nalezeneAlbum in treeListViewAlbaDeezer.Objects)
                {
                    // PADÁ POKUD VYBERU ALBUM MOC RYCHLE
                    // -> PAK BĚŽÍ BACKGROUNDWORKER STÁLE A VYBERE NĚCO CO UŽ NEEXISTUJE
                    if ((String.Compare(nalezeneAlbum.Nazev, album, true) == 0) && (String.Compare(nalezeneAlbum.Interpret, umelec, true) == 0))
                    {
                        treeListViewAlbaDeezer.SelectObject(nalezeneAlbum);
                    }
                }
                buttonVyhledatDeezer.Enabled = true;
                buttonVyhledatDeezer.Text = "Vyhledat na Deezeru";
            }
        }


        private bool ZiskejAlba(string adresa, bool smaz, bool vyhledatSingly)
        {
            // získá json soubor alba
            if (backgroundWorkerVyhledatDeezer.CancellationPending)
            {
                return false;
            }

            string ziskanyJson;
            using (WebClient klient = new WebClient())
            {
                ziskanyJson = klient.DownloadString(adresa);
            }
            Chyba chybaJson = new Chyba(ziskanyJson);
            if (chybaJson.JeChyba)
            {
                if (chybaJson.Kod == 4)
                {
                    ZiskejAlba(adresa, smaz, vyhledatSingly);
                }
                return true;
            }
            // získá seznam nalezených alb
            var seznamNalezenychAlb = JsonConvert.DeserializeObject<AlbumZiskej>(ziskanyJson);
            if (smaz)
            {
                treeListViewAlbaDeezer.ClearObjects();
                nalezenaAlba.Clear();
            }
            foreach (AlbumInformace nalezeneAlbum in seznamNalezenychAlb.data)
            {
                if (backgroundWorkerVyhledatDeezer.CancellationPending)
                {
                    return false;
                }
                string typAlbumu = nalezeneAlbum.record_type.ToLower();

                // pokud nenajde album nebo ep, může vrátit i singl
                if (vyhledatSingly || typAlbumu == "album" || typAlbumu == "ep" || seznamNalezenychAlb.data.Count == 1)
                {
                    // jedná se o album (nikoliv o singl)
                    // přidám nalezené album do seznamu
                    nalezenaAlba.Add(new Deezer(nalezeneAlbum.id));
                    treeListViewAlbaDeezer.SetObjects(nalezenaAlba);
                }
            }
            if (!String.IsNullOrEmpty(seznamNalezenychAlb.next))
            {
                // pokud existuje další stránka vyhledávání
                ZiskejAlba(seznamNalezenychAlb.next, false, vyhledatSingly);
            }
            return true;
        }

        private string OdstranZnaky(string text)
        {
            text.Replace(@"&", "")
                .Replace(".", "")
                .Replace(@"'", "")
                .Replace(@"\", "")
                .Replace("\"", "")
                .Replace(";", "")
                .Replace(":", "")
                .Replace("?", "")
                .Replace("!", "")
                .Trim();
            return text;
        }


        private void treeListViewAlbaDeezer_SelectedIndexChanged(object sender, EventArgs e)
        {
            var vybrano = treeListViewAlbaDeezer.SelectedObject;
            if (vybrano == null)
            {
                return;
            }
            Deezer album;
            if (vybrano is Deezer)
            {
                album = (Deezer)vybrano;
            }
            else if (vybrano is Deezer.SkladbyAlba)
            {
                Deezer.SkladbyAlba skladba = (Deezer.SkladbyAlba)vybrano;
                album = skladba.Album;
                try
                {
                    // vybere stejné video ze seznamu youtube
                    var seznamYoutube = treeListViewAlbaYoutube.Objects;
                    foreach (Video youtubeVideo in seznamYoutube)
                    {
                        if (youtubeVideo.Stopa == skladba.Cislo)
                        {
                            //treeListViewAlbaYoutube.SelectedItem = null;
                            treeListViewAlbaYoutube.SelectObject(youtubeVideo);
                            break;
                        }
                    }
                }
                catch (Exception)
                {
                }
            }
            else
            {
                return;
            }
            if (checkBoxCoverPredniDeezer.Checked)
            {
                CoverNovy(album.CoverNejvetsi, pictureBoxCoverPredni);
            }
            Interpret interpretAlba = new Interpret(album.Interpret);
            interpretAlba.NajdiSlozky();
            Album novyAlbum = new Album(album.Nazev, Convert.ToInt32(album.DatumRok), interpretAlba, hudebniKnihovnaOpus, "", album.CoverNejvetsi);

            textBoxAlbum.Text = novyAlbum.Nazev;
            numericUpDownRok.Value = Convert.ToDecimal(album.DatumRok);
            textBoxInterpret.Text = interpretAlba.Jmeno;
            comboBoxZanr.Text = novyAlbum.ZanrNazev;

            if (checkBoxSlozkaMenitAuto.Checked)
            {
                textBoxSlozka.Text = novyAlbum.Slozka;
            }
            textBoxZanr.Text = novyAlbum.Zanr;
            if (String.IsNullOrEmpty(textBoxSlozka.Text))
            {
                // nejedná se o existující složku ani soubor
                textBoxSlozka.Text = hudebniKnihovnaOpus;
                if (checkBoxSlozkaMenitAuto.Checked)
                {
                    NajitSlozku();
                }
            }
        }

        private void LinkLabelOdkaz_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start(linkLabelOdkaz.Text);
        }


        private void buttonSlozkaNajit_Click(object sender, EventArgs e)
        {
            NajitSlozku();
        }

        private void NajitSlozku()
        {
            Interpret interpretAlba = new Interpret(textBoxInterpret.Text);
            interpretAlba.NajdiSlozky();
            Album novyAlbum;
            string zanr = comboBoxZanr.Text;
            novyAlbum = new Album(textBoxAlbum.Text, Convert.ToInt32(numericUpDownRok.Value), interpretAlba, hudebniKnihovnaOpus, zanr);
            if (String.IsNullOrEmpty(novyAlbum.Slozka))
            {
                // nejedná se o existující složku ani soubor
                textBoxSlozka.Text = Path.Combine(hudebniKnihovnaOpus, zanr, interpretAlba.Jmeno, novyAlbum.Rok + " " + novyAlbum.Nazev);
            }
            else
            {
                textBoxSlozka.Text = novyAlbum.Slozka;
            }
            novyAlbum = new Album(textBoxAlbum.Text, Convert.ToInt32(numericUpDownRok.Value), interpretAlba, hudebniKnihovnaOpus, "", "", textBoxSlozka.Text);
            textBoxZanr.Text = novyAlbum.Zanr;
        }

        private void buttonSlozkaOtevit_Click(object sender, EventArgs e)
        {
            string slozka = textBoxSlozka.Text;
            if (!Directory.Exists(slozka))
            {
                Actions.Show.Error("Otevírání složky", "Složka \"" + slozka + "\" neexistuje.");
                return;
            }

            // otevře složku
            try
            {
                Process.Start(slozka);
            }
            catch (Exception)
            {
                Actions.Show.Error("Otevírání složky", "Složku se nepodařilo otevřit.");
            }
        }

        private void buttonSlozkaZmenit_Click(object sender, EventArgs e)
        {
            VistaFolderBrowserDialog vyberSlozky = new VistaFolderBrowserDialog();
            vyberSlozky.Description = "Vyberte složku do které se stáhne album:";

            // nastaví výchozí cestu
            if (Directory.Exists(textBoxSlozka.Text))
            {
                // jedná se o složku
                vyberSlozky.SelectedPath = textBoxSlozka.Text + "\\";
            }
            else if (File.Exists(textBoxSlozka.Text))
            {
                // jedná se o soubor
                vyberSlozky.SelectedPath = Path.GetDirectoryName(textBoxSlozka.Text) + "\\";
            }
            else
            {
                // nejedná se o existující složku ani soubor
                vyberSlozky.SelectedPath = hudebniKnihovnaOpus + "\\";
            }
            if ((bool)vyberSlozky.ShowDialog())
            {
                textBoxSlozka.Text = vyberSlozky.SelectedPath;
            }
            if (checkBoxSlozkaMenitAuto.Checked)
            {
                NajitSlozku();
            }
        }

        private void ButtonCoverPredniZmenit_Click(object sender, EventArgs e)
        {
            checkBoxCoverPredniDeezer.Checked = false;
            VyberCover(pictureBoxCoverPredni);
        }

        private void ButtonCoverZadniZmenit_Click(object sender, EventArgs e)
        {
            VyberCover(pictureBoxCoverZadni);
        }

        private void VyberCover(PictureBox pictureBoxCover)
        {
            using (var vyberSouboru = new OpenFileDialog())
            {
                /// DODĚLAT -> přidat další typy souborů ///
                vyberSouboru.Filter = "Obrázek | *.jpeg; *.jpg; *.gif; *.png; *.bmp"; //; *.JPEG; *.JPEG; *.GIF; *.PNG; *.BMP;
                DialogResult odpoved = vyberSouboru.ShowDialog();

                if (odpoved == DialogResult.OK && !String.IsNullOrWhiteSpace(vyberSouboru.FileName))
                {
                    string cesta = vyberSouboru.FileName;
                    CoverNovy(cesta, pictureBoxCover);
                }
                else
                {
                    return;
                }
            }
        }

        private void CoverNovy(string cesta, PictureBox pictureBoxCover)
        {
            // zobrazí nový cover a informace o něm
            if (String.IsNullOrEmpty(cesta))
            {
                return;
            }
            // zobrazí cover na pictureBoxu
            pictureBoxCover.ImageLocation = cesta;
            pictureBoxCover.Tag = cesta;

            // načte informace o coveru
            double coverVelikostB = 0;
            double coverVelikostMB = 0;
            string coverNazev = Path.GetFileNameWithoutExtension(cesta);
            string coverTyp = Path.GetExtension(cesta).Replace(".", "");

            int coverSirka = 0;
            int coverVyska = 0;

            if (File.Exists(cesta))
            {
                FileInfo soubor = new FileInfo(cesta);
                coverVelikostB = soubor.Length;
                using (Bitmap img = new Bitmap(cesta))
                {
                    coverSirka = img.Width;
                    coverVyska = img.Height;
                }
            }
            else
            {
                HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(cesta);
                HttpWebResponse response = (HttpWebResponse)req.GetResponse();
                coverVelikostB = Convert.ToDouble(response.Headers.Get("Content-Length"));
                Stream stream = response.GetResponseStream();
                Image img = Image.FromStream(stream);
                stream.Close();
                coverSirka = img.Width;
                coverVyska = img.Height;
            }
            coverVelikostMB = Math.Round(coverVelikostB / 1000000, 1);

            // zobrazí načtené informace na labelu
            toolTipInformace.SetToolTip
            (
                pictureBoxCover,
                "\"" + cesta + "\"" + Environment.NewLine
                + "Název:                " + coverNazev + Environment.NewLine
                + "Typ souboru:     " + coverTyp + Environment.NewLine
                + "Velikost:             " + coverVelikostMB + " MB   (" + coverVelikostB + " bajtů)" + Environment.NewLine
                + "Rozměry:           " + coverVyska + " x " + coverSirka + " px   (výška x šířka)"
            );
            ZmenaCoveru();
        }

        private void ZmenaCoveru()
        {

            if (String.IsNullOrEmpty(pictureBoxCoverPredni.ImageLocation))
            {
                buttonCoverPredniZmenit.Text = "Přidat přední cover";
            }
            else
            {
                buttonCoverPredniZmenit.Text = "Změnit přední cover";
            }
            if (String.IsNullOrEmpty(pictureBoxCoverZadni.ImageLocation))
            {
                buttonCoverZadniZmenit.Text = "Přidat zadní cover";
            }
            else
            {
                buttonCoverZadniZmenit.Text = "Změnit zadní cover";
            }
        }

        private void ButtonCoverPredniSmazat_Click(object sender, EventArgs e)
        {
            checkBoxCoverPredniDeezer.Checked = false;
            pictureBoxCoverPredni.ImageLocation = null;
            toolTipInformace.SetToolTip(pictureBoxCoverPredni, "");
            ZmenaCoveru();
        }

        private void ButtonCoverZadniSmazat_Click(object sender, EventArgs e)
        {
            pictureBoxCoverZadni.ImageLocation = null;
            toolTipInformace.SetToolTip(pictureBoxCoverZadni, "");
            ZmenaCoveru();
        }

        private void ButtonPridatAlbum_Click(object sender, EventArgs e)
        {
            var vybrano = treeListViewAlbaDeezer.SelectedObject;
            if (vybrano == null)
            {
                //return;
            }
            Deezer albumDeezer = null;
            if (vybrano is Deezer)
            {
                albumDeezer = (Deezer)vybrano;
            }
            else if (vybrano is Deezer.SkladbyAlba)
            {
                Deezer.SkladbyAlba skladba = (Deezer.SkladbyAlba)vybrano;
                albumDeezer = skladba.Album;
            }
            else
            {
                //return;
            }

            Interpret interpretAlba = new Interpret(textBoxInterpret.Text);
            interpretAlba.NajdiSlozky();
            Album novyAlbum;
            if (pictureBoxCoverPredni.Tag == null)
            {
                novyAlbum = new Album(textBoxAlbum.Text, Convert.ToInt32(numericUpDownRok.Value), interpretAlba, hudebniKnihovnaOpus, textBoxZanr.Text);
            }
            else
            {
                novyAlbum = new Album(textBoxAlbum.Text, Convert.ToInt32(numericUpDownRok.Value), interpretAlba, hudebniKnihovnaOpus, textBoxZanr.Text, pictureBoxCoverPredni.Tag.ToString());
            }
            novyAlbum.Slozka = textBoxSlozka.Text;
            textBoxZanr.Text = novyAlbum.Zanr;

            // je více interpretů albumu
            List<string> interpretiAlba = new List<string>();
            interpretiAlba.AddRange(interpretAlba.Jmeno.Split(new[] { " & ", ", " }, StringSplitOptions.None));

            foreach (Video videoYoutube in videaVsechna)
            {
                // procházím videa z youtube
                videoYoutube.Album = novyAlbum;

                // zamezí duplicitám interpretů
                List<Interpret> interpretiYoutube = new List<Interpret>();
                interpretiYoutube.AddRange(videoYoutube.Interpreti);

                videoYoutube.Interpreti.Clear();

                videoYoutube.PridejInterpreta(novyAlbum.Interpret.Jmeno);
                foreach (var interpretYoutube in interpretiYoutube)
                {
                    videoYoutube.PridejInterpreta(interpretYoutube.Jmeno);
                }

                if (albumDeezer != null)
                {
                    foreach (var interpretDeezer in albumDeezer.Skladby[videoYoutube.Stopa - 1].Interpreti)
                    {
                        // procházím interprety deezeru
                        if (!interpretiAlba.Contains(interpretDeezer))
                        {
                            // nepřidám jednotlivé interprety albumu k jednotlivým songům (již jsou přidáni)
                            videoYoutube.PridejInterpreta(interpretDeezer);
                        }
                    }
                }

                videoYoutube.Chyba = ""; // SMAZAT
                videoYoutube.Stav = "YouTube Album";
                string jizStazenySoubor = "";
                if (File.Exists(videoYoutube.Slozka))
                {
                    jizStazenySoubor = videoYoutube.Slozka;
                    videoYoutube.Stav = "Bude smazáno";
                }
                videoYoutube.Slozka = novyAlbum.Slozka;
                videoYoutube.NazevNovy = videoYoutube.NazevStopaSkladbaFeat;

                if (videoYoutube.Slozka.ToLower().Contains("various artists"))
                {
                    // najdi soubor ve složce _ostatní a zjisti zdali tam již není
                    videoYoutube.NazevNovy = videoYoutube.NazevStopaInterpretSkladbaFeat;
                }

                if (!String.IsNullOrEmpty(jizStazenySoubor))
                {
                    videoYoutube.Chyba = jizStazenySoubor;
                }
                videoYoutube.Zanr = textBoxZanr.Text;
                if (videoYoutube.Publikovano.Year > novyAlbum.Rok)
                {
                    // video publikovano pozdeji nez rok vydani alba
                    videoYoutube.Publikovano = new DateTime(novyAlbum.Rok, videoYoutube.Publikovano.Month, videoYoutube.Publikovano.Day);
                }

            }
            backgroundWorkerVyhledatDeezer.CancelAsync();
            this.Close();
        }

        private void PictureBoxCover_Click(object sender, EventArgs e)
        {
            PictureBox pictureBoxCover = (PictureBox)sender;
            string cesta = pictureBoxCover.Tag.ToString();
            // zobrazení coveru v externím programu
            if (!Actions.Run.Program(cesta))
            {
                // TODO show error
            }
        }

        private void pictureBoxCoverPredni_LocationChanged(object sender, EventArgs e)
        {
        }

        private void pictureBoxCoverZadni_LocationChanged(object sender, EventArgs e)
        {

        }

        private void comboBoxZanr_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (checkBoxSlozkaMenitAuto.Checked)
            {
                NajitSlozku();
            }
        }
    }
}