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
using System.Text.RegularExpressions;
using System.Reflection;
using System.Diagnostics;

namespace hudba
{
    public partial class FormUprava : Form
    {
        public List<Video> videa { get; set; }
        string hudebniKnihovna = "";

        ListViewItem[] polozky;
        
        ListViewGroupCollection skupinyList;
        int pocet = 0;
        bool nacteno = false;

        public FormUprava(List<Video> videaForm, string slozka, ListViewItem[] pol, ListViewGroupCollection listSkupiny)
        {
            InitializeComponent();
            videa = videaForm;
            hudebniKnihovna = slozka;

            polozky = pol;
            skupinyList = listSkupiny;
        }

        private void Nacti()
        {
            nacteno = false;
            checkBoxUlozit.Checked = false;
            checkBoxSlozkaInterpret.Enabled = false;
            checkBoxSlozkaVybrane.Enabled = false;
            checkBoxSlozkaInterpret.Checked = false;
            checkBoxSlozkaVybrane.Checked = false;
            if (pocet == 0)
            {
                buttonPredchozi.Enabled = false;
            }
            else
            {
                buttonPredchozi.Enabled = true;
            }
            if (pocet == videa.Count - 1)
            {
                buttonNasledujici.Enabled = false;
            }
            else
            {
                buttonNasledujici.Enabled = true;
            }
            
            linkLabelID.Text = videa[pocet].id;
            linkLabelKanal.Text = videa[pocet].kanal;
            textBoxDatum.Text = String.Format("{0:yyyy-MM-dd}", videa[pocet].publikovano);
            textBoxPuvodniNazev.Text = videa[pocet].nazevPuvodni;
            textBoxInterpret.Text = videa[pocet].interpret;
            labelInterpret.Text = videa[pocet].interpret.Replace("&", "&&");
            labelSkladba.Text = videa[pocet].skladbaFeaturing.Replace("&", "&&");
            textBoxSkladba.Text = videa[pocet].skladba;
            textBoxFeaturing.Text = videa[pocet].featuring;
            textBoxNovyNazev.Text = videa[pocet].nazevNovy;
            textBoxSlozka.Text = videa[pocet].slozka;
            textBoxZanr.Text = videa[pocet].zanr;

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
            webBrowserVideo.Navigate("https://www.youtube.com/embed/" + videa[pocet].id);
            //webBrowserVideo.AllowNavigation = false;

            nacteno = true;
        }

        private void Uloz()
        {
            if (!checkBoxUlozit.Checked)
            {
                return;
            }
            videa[pocet].interpret = textBoxInterpret.Text;
            videa[pocet].skladba = textBoxSkladba.Text;
            videa[pocet].featuring = textBoxFeaturing.Text;
            if (textBoxFeaturing.Text != "")
            {
                videa[pocet].skladbaFeaturing = textBoxSkladba.Text + " (ft. " + textBoxFeaturing.Text + ")";
            }
            else
            {
                videa[pocet].skladbaFeaturing = textBoxSkladba.Text;
            }
            videa[pocet].nazevNovy = textBoxNovyNazev.Text;
            videa[pocet].zanr = textBoxZanr.Text;

            UlozSlozku(videa[pocet]);

            if (checkBoxSlozkaVybrane.Checked)
            {
                foreach (Video vid in videa)
                {
                    UlozSlozku(vid);
                }
            }
            if (checkBoxSlozkaInterpret.Checked)
            {
                foreach (Video vid in videa)
                {
                    if (vid.interpret == textBoxInterpret.Text)
                    {
                        UlozSlozku(vid);
                    }
                }
            }

            if (checkBoxZanrVybrane.Checked)
            {
                foreach (Video vid in videa)
                {
                    vid.zanr = textBoxZanr.Text;
                    foreach (ListViewItem polozka in polozky)
                    {
                        if (polozka.Text == vid.id)
                        {
                            // 0 id, 1 kanál, 2 datum publikování, 3 původní název, 4 interpret, 5 skladba, 6 featuring, 7 nový název, 8 složka, 9 chyba, 10 žánr
                            polozka.SubItems[10].Text = vid.zanr;
                        }
                    }
                }
            }
            if (checkBoxZanrInterpret.Checked)
            {
                foreach (Video vid in videa)
                {
                    if (vid.interpret == textBoxInterpret.Text)
                    {
                        vid.zanr = textBoxZanr.Text;
                        foreach (ListViewItem polozka in polozky)
                        {
                            if (polozka.Text == vid.id)
                            {
                                // 0 id, 1 kanál, 2 datum publikování, 3 původní název, 4 interpret, 5 skladba, 6 featuring, 7 nový název, 8 složka, 9 chyba, 10 žánr
                                polozka.SubItems[10].Text = vid.zanr;
                            }
                        }
                    }
                }
            }
            foreach (ListViewItem polozka in polozky)
            {
                if (polozka.Text == videa[pocet].id)
                {                    
                    // 0 id, 1 kanál, 2 datum publikování, 3 původní název, 4 interpret, 5 skladba, 6 featuring, 7 nový název, 8 složka, 9 chyba, 10 žánr    
                    polozka.SubItems[4].Text = videa[pocet].interpret;
                    polozka.SubItems[5].Text = videa[pocet].skladba;
                    polozka.SubItems[6].Text = videa[pocet].featuring;
                    polozka.SubItems[7].Text = videa[pocet].nazevNovy;
                    string slozka = "";
                    if (videa[pocet].skupina == "Video bylo staženo dříve")
                    {
                        slozka = Path.GetFileNameWithoutExtension(videa[pocet].slozka);
                    }
                    else
                    {
                        slozka = videa[pocet].slozka;
                        if (videa[pocet].skupina == "Složka nalezena" && slozka.Contains(hudebniKnihovna))
                        {
                            slozka = slozka.Replace(hudebniKnihovna, "");
                        }
                    }
                    polozka.SubItems[8].Text = slozka;
                    polozka.SubItems[10].Text = videa[pocet].zanr;
                    foreach (ListViewGroup skupina in skupinyList)
                    {
                        if (skupina.Header == videa[pocet].skupina)
                        {
                            polozka.Group = skupina;
                            break;
                        }
                    }
                }
            }
        }
        private void UlozSlozku(Video vid)
        {
            vid.slozka = textBoxSlozka.Text;
            if (vid.slozka == "")
            {
                vid.skupina = "Složka nemohla být nalezena";
            }
            else if (vid.slozka.Contains(".mp3"))
            {
                vid.skupina = "Video bylo staženo dříve";
            }
            else
            {
                vid.skupina = "Složka nalezena";
            }

            if (vid.slozka.ToLower().Contains("_ostatní"))
            {
                vid.nazevNovy = vid.interpret + "-" + vid.skladbaFeaturing;
            }
            else
            {
                vid.nazevNovy = vid.skladbaFeaturing;
            }

            foreach (ListViewItem polozka in polozky)
            {
                if (polozka.Text == vid.id)
                {
                    // 0 id, 1 kanál, 2 datum publikování, 3 původní název, 4 interpret, 5 skladba, 6 featuring, 7 nový název, 8 složka, 9 chyba, 10 žánr    
                    polozka.SubItems[4].Text = vid.interpret;
                    polozka.SubItems[5].Text = vid.skladba;
                    polozka.SubItems[6].Text = vid.featuring;
                    polozka.SubItems[7].Text = vid.nazevNovy;
                    polozka.SubItems[8].Text = vid.slozka;
                    polozka.SubItems[10].Text = vid.zanr;
                    foreach (ListViewGroup skupina in skupinyList)
                    {
                        if (skupina.Header == vid.skupina)
                        {
                            polozka.Group = skupina;
                            break;
                        }
                    }
                }
            }
        }

        private void Zaskrtni()
        {
            if (nacteno)
            {                
                if (!(textBoxInterpret.Text == videa[pocet].interpret &&
                    textBoxSkladba.Text == videa[pocet].skladba &&
                    textBoxFeaturing.Text == videa[pocet].featuring &&
                    textBoxNovyNazev.Text == videa[pocet].nazevNovy &&
                    textBoxSlozka.Text == videa[pocet].slozka &&
                    textBoxZanr.Text == videa[pocet].zanr))
                {
                    checkBoxUlozit.Checked = true;
                }
                else
                {
                    checkBoxUlozit.Checked = false;
                }
            }
        }

        private void ZmenaNazvu()
        {
            if (nacteno)
            {
                labelInterpret.Text = textBoxInterpret.Text.Replace("&", "&&");

                textBoxNovyNazev.Text = "";
                if (textBoxSlozka.Text.ToLower().Contains("_ostatní") || textBoxSlozka.Text == "")
                {
                    textBoxNovyNazev.Text = textBoxInterpret.Text + "-";
                }
                textBoxNovyNazev.Text += textBoxSkladba.Text;
                labelSkladba.Text = textBoxSkladba.Text.Replace("&", "&&");
                if (textBoxFeaturing.Text != "")
                {
                    textBoxNovyNazev.Text += " (ft. " + textBoxFeaturing.Text + ")";
                    labelSkladba.Text += " (ft. " + textBoxFeaturing.Text + ")".Replace("&", "&&");
                }
            }
        }

        private void FormUprava_Load(object sender, EventArgs e)
        {
            Nacti();
        }

        private void FormUprava_FormClosing(object sender, FormClosingEventArgs e)
        {
            //FormSeznam szn = new FormSeznam();
            Uloz();
            //szn.ZobrazVidea(videa);
        }

        private void buttonPredchozi_Click(object sender, EventArgs e)
        {
            Uloz();
            pocet--;
            Nacti();
        }

        private void buttonNasledujici_Click(object sender, EventArgs e)
        {
            Uloz();
            pocet++;
            Nacti();
        }
        
        private void buttonObnovit_Click(object sender, EventArgs e)
        {
            Video vid = new Video(videa[pocet].id);
            videa[pocet] = vid;
            Nacti();
        }

        private void textBoxZanr_TextChanged(object sender, EventArgs e)
        {
            Zaskrtni();
        }

        private void textBoxInterpret_TextChanged(object sender, EventArgs e)
        {
            ZmenaNazvu();
            Zaskrtni();
        }

        private void textBoxSkladba_TextChanged(object sender, EventArgs e)
        {
            
            ZmenaNazvu();
            Zaskrtni();
        }

        private void textBoxFeaturing_TextChanged(object sender, EventArgs e)
        {
            ZmenaNazvu();
            Zaskrtni();
        }

        private void textBoxNovyNazev_TextChanged(object sender, EventArgs e)
        {
            Zaskrtni();
        }

        private void textBoxSlozka_TextChanged(object sender, EventArgs e)
        {
            ZmenaNazvu();

            if (textBoxSlozka.Text == "")
            {
                buttonSlozkaOtevrit.Enabled = false;
                checkBoxSlozkaInterpret.Enabled = false;
                checkBoxSlozkaVybrane.Enabled = false;
            }
            else
            {
                buttonSlozkaOtevrit.Enabled = true;
                checkBoxSlozkaInterpret.Enabled = true;
                checkBoxSlozkaVybrane.Enabled = true;
            }

            Zaskrtni();
        }

        private void linkLabelID_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://youtu.be/" + videa[pocet].id);
        }

        private void buttonSlozkaOtevrit_Click(object sender, EventArgs e)
        {
            if (File.Exists(textBoxSlozka.Text))
            {
                Process.Start("explorer.exe", "/select, \"" + textBoxSlozka.Text + "\"");
                return;
            }
            if (!Directory.Exists(textBoxSlozka.Text))
            {
                MessageBox.Show("Složka neexistuje");
                return;
            }
            Process.Start(textBoxSlozka.Text);
        }


        private void buttonSlozkaJina_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog vyberSlozky = new FolderBrowserDialog();
            vyberSlozky.Description = "Vyberte složku do které se stáhne video:";
            vyberSlozky.SelectedPath = textBoxSlozka.Text;

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

        private void buttonSlozkaNajit_Click(object sender, EventArgs e)
        {
            MessageBox.Show("není dodělané");
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
        }

        private void textBoxNovyNazev_Click(object sender, EventArgs e)
        {
            if (textBoxSlozka.Text != "")
            {
                if (textBoxNovyNazev.ReadOnly)
                {                    
                    textBoxNovyNazev.ReadOnly = false;
                }
                else
                {
                    textBoxNovyNazev.ReadOnly = true;
                }
            }
        }

        private void checkBoxUlozit_CheckedChanged(object sender, EventArgs e)
        {
            buttonSlozkaNajit.Enabled = checkBoxUlozit.Checked;
        }

        private void webBrowserVideo_NewWindow(object sender, CancelEventArgs e)
        {
            System.Diagnostics.Process.Start("https://youtu.be/" + videa[pocet].id);
            e.Cancel = true;
        }

        private void linkLabelKanal_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://youtube.com/channel/" + videa[pocet].kanalID);
        }

    }
}
