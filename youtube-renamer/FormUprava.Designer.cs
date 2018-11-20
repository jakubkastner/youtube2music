namespace hudba
{
    partial class FormUprava
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.textBoxInterpret = new System.Windows.Forms.TextBox();
            this.textBoxSkladba = new System.Windows.Forms.TextBox();
            this.textBoxFeaturing = new System.Windows.Forms.TextBox();
            this.textBoxNovyNazev = new System.Windows.Forms.TextBox();
            this.textBoxPuvodniNazev = new System.Windows.Forms.TextBox();
            this.buttonPredchozi = new System.Windows.Forms.Button();
            this.buttonNasledujici = new System.Windows.Forms.Button();
            this.checkBoxUlozit = new System.Windows.Forms.CheckBox();
            this.buttonObnovit = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.buttonSlozkaOtevrit = new System.Windows.Forms.Button();
            this.label9 = new System.Windows.Forms.Label();
            this.textBoxDatum = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.linkLabelID = new System.Windows.Forms.LinkLabel();
            this.buttonSlozkaJina = new System.Windows.Forms.Button();
            this.textBoxSlozka = new System.Windows.Forms.TextBox();
            this.buttonSlozkaNajit = new System.Windows.Forms.Button();
            this.checkBoxSlozkaInterpret = new System.Windows.Forms.CheckBox();
            this.checkBoxSlozkaVybrane = new System.Windows.Forms.CheckBox();
            this.webBrowserVideo = new System.Windows.Forms.WebBrowser();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.textBoxZanr = new System.Windows.Forms.TextBox();
            this.checkBoxZanrInterpret = new System.Windows.Forms.CheckBox();
            this.checkBoxZanrVybrane = new System.Windows.Forms.CheckBox();
            this.labelInterpret = new System.Windows.Forms.Label();
            this.labelSkladba = new System.Windows.Forms.Label();
            this.linkLabelKanal = new System.Windows.Forms.LinkLabel();
            this.SuspendLayout();
            // 
            // textBoxInterpret
            // 
            this.textBoxInterpret.Location = new System.Drawing.Point(111, 372);
            this.textBoxInterpret.Name = "textBoxInterpret";
            this.textBoxInterpret.Size = new System.Drawing.Size(816, 20);
            this.textBoxInterpret.TabIndex = 0;
            this.textBoxInterpret.TextChanged += new System.EventHandler(this.textBoxInterpret_TextChanged);
            // 
            // textBoxSkladba
            // 
            this.textBoxSkladba.Location = new System.Drawing.Point(111, 398);
            this.textBoxSkladba.Name = "textBoxSkladba";
            this.textBoxSkladba.Size = new System.Drawing.Size(816, 20);
            this.textBoxSkladba.TabIndex = 1;
            this.textBoxSkladba.TextChanged += new System.EventHandler(this.textBoxSkladba_TextChanged);
            // 
            // textBoxFeaturing
            // 
            this.textBoxFeaturing.Location = new System.Drawing.Point(111, 424);
            this.textBoxFeaturing.Name = "textBoxFeaturing";
            this.textBoxFeaturing.Size = new System.Drawing.Size(816, 20);
            this.textBoxFeaturing.TabIndex = 2;
            this.textBoxFeaturing.TextChanged += new System.EventHandler(this.textBoxFeaturing_TextChanged);
            // 
            // textBoxNovyNazev
            // 
            this.textBoxNovyNazev.Location = new System.Drawing.Point(111, 450);
            this.textBoxNovyNazev.Name = "textBoxNovyNazev";
            this.textBoxNovyNazev.ReadOnly = true;
            this.textBoxNovyNazev.Size = new System.Drawing.Size(816, 20);
            this.textBoxNovyNazev.TabIndex = 3;
            this.textBoxNovyNazev.Click += new System.EventHandler(this.textBoxNovyNazev_Click);
            this.textBoxNovyNazev.TextChanged += new System.EventHandler(this.textBoxNovyNazev_TextChanged);
            // 
            // textBoxPuvodniNazev
            // 
            this.textBoxPuvodniNazev.Location = new System.Drawing.Point(111, 346);
            this.textBoxPuvodniNazev.Name = "textBoxPuvodniNazev";
            this.textBoxPuvodniNazev.ReadOnly = true;
            this.textBoxPuvodniNazev.Size = new System.Drawing.Size(816, 20);
            this.textBoxPuvodniNazev.TabIndex = 5;
            // 
            // buttonPredchozi
            // 
            this.buttonPredchozi.Location = new System.Drawing.Point(111, 601);
            this.buttonPredchozi.Name = "buttonPredchozi";
            this.buttonPredchozi.Size = new System.Drawing.Size(220, 27);
            this.buttonPredchozi.TabIndex = 9;
            this.buttonPredchozi.Text = "Předchozí";
            this.buttonPredchozi.UseVisualStyleBackColor = true;
            this.buttonPredchozi.Click += new System.EventHandler(this.buttonPredchozi_Click);
            // 
            // buttonNasledujici
            // 
            this.buttonNasledujici.Cursor = System.Windows.Forms.Cursors.Default;
            this.buttonNasledujici.Location = new System.Drawing.Point(707, 601);
            this.buttonNasledujici.Name = "buttonNasledujici";
            this.buttonNasledujici.Size = new System.Drawing.Size(220, 27);
            this.buttonNasledujici.TabIndex = 10;
            this.buttonNasledujici.Text = "Následující";
            this.buttonNasledujici.UseVisualStyleBackColor = true;
            this.buttonNasledujici.Click += new System.EventHandler(this.buttonNasledujici_Click);
            // 
            // checkBoxUlozit
            // 
            this.checkBoxUlozit.AutoSize = true;
            this.checkBoxUlozit.Location = new System.Drawing.Point(111, 568);
            this.checkBoxUlozit.Name = "checkBoxUlozit";
            this.checkBoxUlozit.Size = new System.Drawing.Size(52, 17);
            this.checkBoxUlozit.TabIndex = 11;
            this.checkBoxUlozit.Text = "Uložit";
            this.checkBoxUlozit.UseVisualStyleBackColor = true;
            this.checkBoxUlozit.CheckedChanged += new System.EventHandler(this.checkBoxUlozit_CheckedChanged);
            // 
            // buttonObnovit
            // 
            this.buttonObnovit.Location = new System.Drawing.Point(412, 601);
            this.buttonObnovit.Name = "buttonObnovit";
            this.buttonObnovit.Size = new System.Drawing.Size(220, 27);
            this.buttonObnovit.TabIndex = 12;
            this.buttonObnovit.Text = "Obnovit";
            this.buttonObnovit.UseVisualStyleBackColor = true;
            this.buttonObnovit.Click += new System.EventHandler(this.buttonObnovit_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(17, 222);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(34, 13);
            this.label1.TabIndex = 13;
            this.label1.Text = "Kanál";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(17, 346);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(80, 13);
            this.label2.TabIndex = 14;
            this.label2.Text = "Původní název";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(17, 372);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(46, 13);
            this.label3.TabIndex = 15;
            this.label3.Text = "Interpret";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(17, 398);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(46, 13);
            this.label4.TabIndex = 16;
            this.label4.Text = "Skladba";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(17, 424);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(51, 13);
            this.label5.TabIndex = 17;
            this.label5.Text = "Featuring";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(17, 450);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(64, 13);
            this.label6.TabIndex = 18;
            this.label6.Text = "Nový název";
            // 
            // buttonSlozkaOtevrit
            // 
            this.buttonSlozkaOtevrit.Enabled = false;
            this.buttonSlozkaOtevrit.Location = new System.Drawing.Point(707, 525);
            this.buttonSlozkaOtevrit.Name = "buttonSlozkaOtevrit";
            this.buttonSlozkaOtevrit.Size = new System.Drawing.Size(220, 27);
            this.buttonSlozkaOtevrit.TabIndex = 21;
            this.buttonSlozkaOtevrit.Text = "Otevřít složku v průzkumníku souborů";
            this.buttonSlozkaOtevrit.UseVisualStyleBackColor = true;
            this.buttonSlozkaOtevrit.Click += new System.EventHandler(this.buttonSlozkaOtevrit_Click);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(17, 476);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(39, 13);
            this.label9.TabIndex = 22;
            this.label9.Text = "Složka";
            // 
            // textBoxDatum
            // 
            this.textBoxDatum.Location = new System.Drawing.Point(111, 248);
            this.textBoxDatum.Name = "textBoxDatum";
            this.textBoxDatum.ReadOnly = true;
            this.textBoxDatum.Size = new System.Drawing.Size(280, 20);
            this.textBoxDatum.TabIndex = 25;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(17, 248);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(66, 13);
            this.label10.TabIndex = 26;
            this.label10.Text = "Publikováno";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(17, 195);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(47, 13);
            this.label11.TabIndex = 27;
            this.label11.Text = "ID videa";
            // 
            // linkLabelID
            // 
            this.linkLabelID.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.linkLabelID.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.linkLabelID.LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline;
            this.linkLabelID.LinkColor = System.Drawing.Color.Black;
            this.linkLabelID.Location = new System.Drawing.Point(111, 195);
            this.linkLabelID.Name = "linkLabelID";
            this.linkLabelID.Size = new System.Drawing.Size(280, 20);
            this.linkLabelID.TabIndex = 28;
            this.linkLabelID.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabelID_LinkClicked);
            // 
            // buttonSlozkaJina
            // 
            this.buttonSlozkaJina.Location = new System.Drawing.Point(412, 525);
            this.buttonSlozkaJina.Name = "buttonSlozkaJina";
            this.buttonSlozkaJina.Size = new System.Drawing.Size(220, 27);
            this.buttonSlozkaJina.TabIndex = 29;
            this.buttonSlozkaJina.Text = "Vybrat jinou složku";
            this.buttonSlozkaJina.UseVisualStyleBackColor = true;
            this.buttonSlozkaJina.Click += new System.EventHandler(this.buttonSlozkaJina_Click);
            // 
            // textBoxSlozka
            // 
            this.textBoxSlozka.Location = new System.Drawing.Point(111, 476);
            this.textBoxSlozka.Name = "textBoxSlozka";
            this.textBoxSlozka.ReadOnly = true;
            this.textBoxSlozka.Size = new System.Drawing.Size(816, 20);
            this.textBoxSlozka.TabIndex = 30;
            this.textBoxSlozka.TextChanged += new System.EventHandler(this.textBoxSlozka_TextChanged);
            // 
            // buttonSlozkaNajit
            // 
            this.buttonSlozkaNajit.Location = new System.Drawing.Point(111, 525);
            this.buttonSlozkaNajit.Name = "buttonSlozkaNajit";
            this.buttonSlozkaNajit.Size = new System.Drawing.Size(220, 27);
            this.buttonSlozkaNajit.TabIndex = 31;
            this.buttonSlozkaNajit.Text = "Najít složku automaticky";
            this.buttonSlozkaNajit.UseVisualStyleBackColor = true;
            this.buttonSlozkaNajit.Click += new System.EventHandler(this.buttonSlozkaNajit_Click);
            // 
            // checkBoxSlozkaInterpret
            // 
            this.checkBoxSlozkaInterpret.AutoSize = true;
            this.checkBoxSlozkaInterpret.Location = new System.Drawing.Point(111, 502);
            this.checkBoxSlozkaInterpret.Name = "checkBoxSlozkaInterpret";
            this.checkBoxSlozkaInterpret.Size = new System.Drawing.Size(260, 17);
            this.checkBoxSlozkaInterpret.TabIndex = 32;
            this.checkBoxSlozkaInterpret.Text = "Použít složku pro všechny videa tohoto interpreta";
            this.checkBoxSlozkaInterpret.UseVisualStyleBackColor = true;
            // 
            // checkBoxSlozkaVybrane
            // 
            this.checkBoxSlozkaVybrane.AutoSize = true;
            this.checkBoxSlozkaVybrane.Location = new System.Drawing.Point(662, 502);
            this.checkBoxSlozkaVybrane.Name = "checkBoxSlozkaVybrane";
            this.checkBoxSlozkaVybrane.Size = new System.Drawing.Size(265, 17);
            this.checkBoxSlozkaVybrane.TabIndex = 33;
            this.checkBoxSlozkaVybrane.Text = "Použít složku pro všechny aktuálně vybrané videa";
            this.checkBoxSlozkaVybrane.UseVisualStyleBackColor = true;
            // 
            // webBrowserVideo
            // 
            this.webBrowserVideo.IsWebBrowserContextMenuEnabled = false;
            this.webBrowserVideo.Location = new System.Drawing.Point(406, 9);
            this.webBrowserVideo.Margin = new System.Windows.Forms.Padding(0);
            this.webBrowserVideo.MinimumSize = new System.Drawing.Size(20, 20);
            this.webBrowserVideo.Name = "webBrowserVideo";
            this.webBrowserVideo.ScriptErrorsSuppressed = true;
            this.webBrowserVideo.ScrollBarsEnabled = false;
            this.webBrowserVideo.Size = new System.Drawing.Size(521, 325);
            this.webBrowserVideo.TabIndex = 34;
            this.webBrowserVideo.Visible = false;
            this.webBrowserVideo.NewWindow += new System.ComponentModel.CancelEventHandler(this.webBrowserVideo_NewWindow);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(369, 330);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(34, 13);
            this.label7.TabIndex = 35;
            this.label7.Text = "Video";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(17, 274);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(29, 13);
            this.label8.TabIndex = 36;
            this.label8.Text = "Žánr";
            // 
            // textBoxZanr
            // 
            this.textBoxZanr.Location = new System.Drawing.Point(111, 274);
            this.textBoxZanr.Name = "textBoxZanr";
            this.textBoxZanr.Size = new System.Drawing.Size(280, 20);
            this.textBoxZanr.TabIndex = 37;
            this.textBoxZanr.TextChanged += new System.EventHandler(this.textBoxZanr_TextChanged);
            // 
            // checkBoxZanrInterpret
            // 
            this.checkBoxZanrInterpret.AutoSize = true;
            this.checkBoxZanrInterpret.Location = new System.Drawing.Point(111, 300);
            this.checkBoxZanrInterpret.Name = "checkBoxZanrInterpret";
            this.checkBoxZanrInterpret.Size = new System.Drawing.Size(250, 17);
            this.checkBoxZanrInterpret.TabIndex = 38;
            this.checkBoxZanrInterpret.Text = "Použít žánr pro všechny videa tohoto interpreta";
            this.checkBoxZanrInterpret.UseVisualStyleBackColor = true;
            // 
            // checkBoxZanrVybrane
            // 
            this.checkBoxZanrVybrane.AutoSize = true;
            this.checkBoxZanrVybrane.Location = new System.Drawing.Point(111, 323);
            this.checkBoxZanrVybrane.Name = "checkBoxZanrVybrane";
            this.checkBoxZanrVybrane.Size = new System.Drawing.Size(255, 17);
            this.checkBoxZanrVybrane.TabIndex = 39;
            this.checkBoxZanrVybrane.Text = "Použít žánr pro všechny aktuálně vybrané videa";
            this.checkBoxZanrVybrane.UseVisualStyleBackColor = true;
            // 
            // labelInterpret
            // 
            this.labelInterpret.Font = new System.Drawing.Font("Microsoft Sans Serif", 27.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.labelInterpret.Location = new System.Drawing.Point(20, 9);
            this.labelInterpret.Name = "labelInterpret";
            this.labelInterpret.Size = new System.Drawing.Size(371, 87);
            this.labelInterpret.TabIndex = 40;
            this.labelInterpret.Text = "-";
            this.labelInterpret.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelSkladba
            // 
            this.labelSkladba.Font = new System.Drawing.Font("Microsoft Sans Serif", 27.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.labelSkladba.Location = new System.Drawing.Point(20, 96);
            this.labelSkladba.Name = "labelSkladba";
            this.labelSkladba.Size = new System.Drawing.Size(371, 87);
            this.labelSkladba.TabIndex = 41;
            this.labelSkladba.Text = "-";
            this.labelSkladba.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // linkLabelKanal
            // 
            this.linkLabelKanal.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.linkLabelKanal.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.linkLabelKanal.LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline;
            this.linkLabelKanal.LinkColor = System.Drawing.Color.Black;
            this.linkLabelKanal.Location = new System.Drawing.Point(111, 225);
            this.linkLabelKanal.Name = "linkLabelKanal";
            this.linkLabelKanal.Size = new System.Drawing.Size(280, 20);
            this.linkLabelKanal.TabIndex = 42;
            this.linkLabelKanal.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabelKanal_LinkClicked);
            // 
            // FormUprava
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(939, 642);
            this.Controls.Add(this.linkLabelKanal);
            this.Controls.Add(this.labelInterpret);
            this.Controls.Add(this.checkBoxZanrVybrane);
            this.Controls.Add(this.checkBoxZanrInterpret);
            this.Controls.Add(this.textBoxZanr);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.webBrowserVideo);
            this.Controls.Add(this.checkBoxSlozkaVybrane);
            this.Controls.Add(this.checkBoxSlozkaInterpret);
            this.Controls.Add(this.buttonSlozkaNajit);
            this.Controls.Add(this.textBoxSlozka);
            this.Controls.Add(this.buttonSlozkaJina);
            this.Controls.Add(this.linkLabelID);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.textBoxDatum);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.buttonSlozkaOtevrit);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.buttonObnovit);
            this.Controls.Add(this.checkBoxUlozit);
            this.Controls.Add(this.buttonNasledujici);
            this.Controls.Add(this.buttonPredchozi);
            this.Controls.Add(this.textBoxPuvodniNazev);
            this.Controls.Add(this.textBoxNovyNazev);
            this.Controls.Add(this.textBoxFeaturing);
            this.Controls.Add(this.textBoxSkladba);
            this.Controls.Add(this.textBoxInterpret);
            this.Controls.Add(this.labelSkladba);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "FormUprava";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "too";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormUprava_FormClosing);
            this.Load += new System.EventHandler(this.FormUprava_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBoxInterpret;
        private System.Windows.Forms.TextBox textBoxSkladba;
        private System.Windows.Forms.TextBox textBoxFeaturing;
        private System.Windows.Forms.TextBox textBoxNovyNazev;
        private System.Windows.Forms.TextBox textBoxPuvodniNazev;
        private System.Windows.Forms.Button buttonPredchozi;
        private System.Windows.Forms.Button buttonNasledujici;
        private System.Windows.Forms.CheckBox checkBoxUlozit;
        private System.Windows.Forms.Button buttonObnovit;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button buttonSlozkaOtevrit;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox textBoxDatum;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.LinkLabel linkLabelID;
        private System.Windows.Forms.Button buttonSlozkaJina;
        private System.Windows.Forms.TextBox textBoxSlozka;
        private System.Windows.Forms.Button buttonSlozkaNajit;
        private System.Windows.Forms.CheckBox checkBoxSlozkaInterpret;
        private System.Windows.Forms.CheckBox checkBoxSlozkaVybrane;
        private System.Windows.Forms.WebBrowser webBrowserVideo;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox textBoxZanr;
        private System.Windows.Forms.CheckBox checkBoxZanrInterpret;
        private System.Windows.Forms.CheckBox checkBoxZanrVybrane;
        private System.Windows.Forms.Label labelInterpret;
        private System.Windows.Forms.Label labelSkladba;
        private System.Windows.Forms.LinkLabel linkLabelKanal;
    }
}