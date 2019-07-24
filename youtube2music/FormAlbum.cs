﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace youtube2music
{
    public partial class FormAlbum : Form
    {
        public FormAlbum()
        {
            InitializeComponent();
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

            // získá ba umělce
            ZiskejAlba("https://api.deezer.com/search/album?q=artist:\"" + umelec + "\" album:\"" + album + "\"?access_token=", true);
        }

        private void BackgroundWorkerVyhledatDeezer_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
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
                if (nalezeneAlbum.record_type.ToLower() == "album")
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
            }
            else
            {
                return;
            }
            textBoxAlbum.Text = album.Nazev;
            textBoxInterpret.Text = album.Interpret;
            numericUpDownRok.Value = Convert.ToInt32(album.Datum.Split('-').First());
            pictureBoxCover.ImageLocation = album.CoverStredni;
        }
    }
}
