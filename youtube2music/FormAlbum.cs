using BrightIdeasSoftware;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace youtube2music
{
    public partial class FormAlbum : Form
    {
        string playlistID = "";
        string hudebniKnihovna = "";
        List<Video> videaVsechna = new List<Video>();

        public FormAlbum(string playlist, List<Video> videa, string hudebniKnihovnaOpus)
        {
            InitializeComponent();
            playlistID = playlist;
            videaVsechna = videa;
            hudebniKnihovna = hudebniKnihovnaOpus;
        }

        private void ButtonVyhledatDeezer_Click(object sender, EventArgs e)
        {
            // spuštění vyhledávání interpeta / alba
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

        private void ButtonPridatAlbum_Click(object sender, EventArgs e)
        {

            var vybrano = treeListViewAlbaDeezer.SelectedObject;
            if (vybrano == null)
            {
                return;
            }
            Deezer albumDeezer;
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
                return;
            }

            Interpret interpretAlba = new Interpret(textBoxInterpret.Text);
            interpretAlba.NajdiSlozky();
            Album novyAlbum = new Album(textBoxAlbum.Text, Convert.ToInt32(numericUpDownRok.Value), interpretAlba, pictureBoxCover.Tag);
            novyAlbum.Slozka = textBoxSlozka.Text;
            foreach (Video vid in videaVsechna)
            {
                // procházím videa z youtube
                vid.Album = novyAlbum;
                vid.PridejInterpreta(novyAlbum.Interpret.Jmeno);
                foreach (var interpretDeezer in albumDeezer.Skladby[vid.Stopa - 1].Interpreti)
                {
                    vid.PridejInterpreta(interpretDeezer);
                }
                vid.Chyba = ""; // SMAZAT
                vid.Stav = "YouTube Album";
                if (File.Exists(vid.Slozka))
                {
                    vid.Chyba = vid.Slozka;
                    vid.Stav = "Bude smazáno";
                }
                vid.Slozka = novyAlbum.Slozka;
                string stopa = "";
                if (vid.Stopa < 10)
                {
                    stopa += "0";
                }
                stopa += vid.Stopa;
                string nazev = vid.NazevSkladbaFeat;
                vid.NazevNovy = stopa + " " + nazev;
                vid.Zanr = textBoxZanr.Text;

            }

            this.Close();
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

            string umelec = OdstranZnaky(textBoxInterpret.Text);
            string album = OdstranZnaky(textBoxAlbum.Text);

            // získá alba umělce
            ZiskejAlba("https://api.deezer.com/search/album?q=artist:\"" + umelec + "\" album:\"" + album + "\"?access_token=", true);
        }

        private void BackgroundWorkerVyhledatDeezer_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            string umelec = OdstranZnaky(textBoxInterpret.Text);
            string album = OdstranZnaky(textBoxAlbum.Text);
            foreach (Deezer nalezeneAlbum in treeListViewAlbaDeezer.Objects)
            {
                if ((String.Compare(nalezeneAlbum.Nazev, album, true) == 0) && (String.Compare(nalezeneAlbum.Interpret, umelec, true) == 0))
                {
                    treeListViewAlbaDeezer.SelectObject(nalezeneAlbum);
                }
            }
            buttonVyhledatDeezer.Enabled = true;
            buttonVyhledatDeezer.Text = "Vyhledat na Deezeru";
        }


        List<Deezer> nalezenaAlba = new List<Deezer>();

        private void ZiskejAlba(string adresa, bool smaz)
        {
            // získá json soubor alba

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
                    ZiskejAlba(adresa, smaz);
                }
                return;
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
                    return;
                }
                string typAlbumu = nalezeneAlbum.record_type.ToLower();
                if (typAlbumu == "album" || typAlbumu == "ep")
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
                ZiskejAlba(seznamNalezenychAlb.next, false);
            }
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

        private void FormAlbum_Load(object sender, EventArgs e)
        {
            treeListViewAlbaYoutube.AddObjects(videaVsechna);
            // rozbalení položek
            treeListViewAlbaDeezer.CanExpandGetter = delegate (Object x)
            {
                return (x is Deezer);
            };

            // přidání podpoložek
            treeListViewAlbaDeezer.ChildrenGetter = delegate (Object x)
            {
                if (x is Deezer)
                {
                    return ((Deezer)x).Skladby;
                }
                throw new ArgumentException("Musí být album nebo interpret");
            };
            Playlist playlistAlba = new Playlist(playlistID);
            textBoxAlbum.Text = playlistAlba.Nazev;
            linkLabelOdkaz.Text = playlistAlba.Url;
            this.Text = playlistAlba.Nazev + " - nové album ~ youtube2music";
            if (videaVsechna.Count > 0)
            {
                textBoxInterpret.Text = videaVsechna.First().Interpret;
            }
            ButtonVyhledatDeezer_Click(null, null);
        }

        private void TreeListViewAlbaDeezer_SelectedIndexChanged(object sender, EventArgs e)
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
            textBoxAlbum.Text = album.Nazev;
            textBoxInterpret.Text = album.Interpret;
            numericUpDownRok.Value = Convert.ToInt32(album.Datum.Split('-').First());
            pictureBoxCover.ImageLocation = album.CoverStredni;
            pictureBoxCover.Tag = album.CoverNejvetsi;

            Interpret interpretAlba = new Interpret(textBoxInterpret.Text);
            interpretAlba.NajdiSlozky();
            Album novyAlbum = new Album(textBoxAlbum.Text, Convert.ToInt32(numericUpDownRok.Value), interpretAlba, pictureBoxCover.Tag.ToString());

            textBoxSlozka.Text = novyAlbum.Slozka;
        }

        private void LinkLabelOdkaz_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start(linkLabelOdkaz.Text);
        }

        private void ButtonAktualizovat_Click(object sender, EventArgs e)
        {
            /*treeListViewAlbaYoutube.RemoveObjects(videaVsechna);
            treeListViewAlbaYoutube.AddObjects(videaVsechna);*/
        }

        private void ButtonVyhledatAktualizovat_Click(object sender, EventArgs e)
        {
            /*ButtonAktualizovat_Click(sender, e);
            ButtonVyhledatDeezer_Click(sender, e);*/
        }

        private void buttonSlozkaOtevit_Click(object sender, EventArgs e)
        {
            string slozka = textBoxSlozka.Text;
            if (!Directory.Exists(slozka))
            {
                Zobrazit.Chybu("Otevírání složky", "Složka \"" + slozka + "\" neexistuje.");
                return;
            }

            // otevře složku
            try
            {
                Process.Start(slozka);
            }
            catch (Exception)
            {
                Zobrazit.Chybu("Otevírání složky", "Složku se nepodařilo otevřit.");
            }
        }

        private void buttonSlozkaZmenit_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog vyberSlozky = new FolderBrowserDialog();
            vyberSlozky.Description = "Vyberte složku do které se stáhne album:";

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

            if (vyberSlozky.ShowDialog() == DialogResult.OK)
            {
                textBoxSlozka.Text = vyberSlozky.SelectedPath;
            }
        }

        private void treeListViewAlbaYoutube_SelectedIndexChanged(object sender, EventArgs e)
        {
            // PROVÁDÍ SE ZACYKLENĚ !!!! ŠPATNĚ
            /*var vybrano = treeListViewAlbaYoutube.SelectedObject;
            if (vybrano == null)
            {
                return;
            }
            if (!(vybrano is Video))
            {
                return;
            }
            Video youtubeVideo = (Video)vybrano;
            try
            {
                var seznamDeezer = treeListViewAlbaDeezer.Objects; // získá jen alba (nikoliv SKLADBY !!!!!) -> chyba
                foreach (var deezer in seznamDeezer)
                {
                    if (deezer == null)
                    {
                        return;
                    }
                    if (deezer is Deezer)
                    {
                        treeListViewAlbaDeezer.SelectedItem = null;
                    }
                    else if (deezer is Deezer.SkladbyAlba)
                    {
                        Deezer.SkladbyAlba skladba = (Deezer.SkladbyAlba)deezer;
                        if (youtubeVideo.Stopa == skladba.Cislo)
                        {
                            treeListViewAlbaDeezer.SelectedItem = null;
                            treeListViewAlbaDeezer.SelectObject(skladba);
                            break;
                        }
                    }                        
                }
            }
            catch (Exception)
            {
            }*/
        }

        private void button4_Click(object sender, EventArgs e)
        {
            CoverNovy(pictureBoxCover);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            CoverNovy(pictureBox1);
        }


        private void CoverNovy(PictureBox picBox)
        {
            string cesta = "";
            using (var vyberSouboru = new OpenFileDialog())
            {
                /// DODĚLAT -> přidat další typy souborů ///
                vyberSouboru.Filter = "Obrázek | *.jpeg; *.jpg; *.gif; *.png; *.bmp"; //; *.JPEG; *.JPEG; *.GIF; *.PNG; *.BMP;
                DialogResult odpoved = vyberSouboru.ShowDialog();

                if (odpoved == DialogResult.OK && !String.IsNullOrWhiteSpace(vyberSouboru.FileName))
                {
                    cesta = vyberSouboru.FileName;
                }
                else
                {
                    return;
                }
            }
            // zobrazí nový cover a informace o něm
            if (!File.Exists(cesta))
            {
                return;
            }
            // zobrazí cover na pictureBoxu
            picBox.ImageLocation = cesta;
            picBox.Tag = cesta;

            // načte informace o coveru
            /*FileInfo soubor = new FileInfo(cesta);
            double coverVelikostB = soubor.Length;
            double coverVelikostMB = Math.Round(coverVelikostB / 1000000, 1);
            string coverNazev = Path.GetFileNameWithoutExtension(cesta);
            string coverTyp = Path.GetExtension(cesta).Replace(".", "");

            int coverSirka = 0;
            int coverVyska = 0;
            using (Bitmap img = new Bitmap(cesta))
            {
                coverSirka = img.Width;
                coverVyska = img.Height;
            }

            // zobrazí načtené informace na labelu
            labelPridaniUpravaArchivu_CoverInfo.Text = "Název:                " + coverNazev + Environment.NewLine
                                + "Typ souboru:     " + coverTyp + Environment.NewLine
                                + "Velikost:            " + coverVelikostMB + " MB   (" + coverVelikostB + " bajtů)" + Environment.NewLine
                                + "Rozměry:           " + coverVyska + " x " + coverSirka + " px   (výška x šířka)";
        }
        private void pictureBoxCover_Click(object sender, EventArgs e)
        {
            // zobrazení coveru v externím programu
            /*if (!String.IsNullOrEmpty((PictureBox)sender.ImageLocation))
            {
                ZobrazStavNovy("Zobrazuji cover", false);
                SpustitProgram(pictureBoxPridaniUpravaArchivu_Cover.ImageLocation, "", false);
            }*/
        }

        private void button5_Click(object sender, EventArgs e)
        {
            pictureBoxCover.Image = null;
            pictureBoxCover.Tag = "";
        }

        private void button6_Click(object sender, EventArgs e)
        {

            pictureBox1.Image = null;
            pictureBox1.Tag = "";
        }
    }
}
