﻿namespace youtube2music
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormUprava));
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
            this.buttonSlozkaJina = new System.Windows.Forms.Button();
            this.textBoxSlozka = new System.Windows.Forms.TextBox();
            this.buttonSlozkaNajit = new System.Windows.Forms.Button();
            this.checkBoxStejnaSlozkaInterpret = new System.Windows.Forms.CheckBox();
            this.checkBoxStejnaSlozkaVybrane = new System.Windows.Forms.CheckBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.textBoxZanr = new System.Windows.Forms.TextBox();
            this.checkBoxStejnyZanrInterpret = new System.Windows.Forms.CheckBox();
            this.checkBoxStejnyZanrVybrane = new System.Windows.Forms.CheckBox();
            this.labelInterpret = new System.Windows.Forms.Label();
            this.labelSkladba = new System.Windows.Forms.Label();
            this.linkLabelID = new System.Windows.Forms.LinkLabel();
            this.linkLabelKanal = new System.Windows.Forms.LinkLabel();
            this.checkBoxStejnyZanrPlaylist = new System.Windows.Forms.CheckBox();
            this.checkBoxStejnaSlozkaPlaylist = new System.Windows.Forms.CheckBox();
            this.labelChyba = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.linkLabelPlaylist = new System.Windows.Forms.LinkLabel();
            this.checkBoxNovyNazevAutomaticky = new System.Windows.Forms.CheckBox();
            this.richTextBoxPopis = new System.Windows.Forms.RichTextBox();
            this.checkBoxStejnyInterpretVybrane = new System.Windows.Forms.CheckBox();
            this.checkBoxStejnyInterpretPlaylist = new System.Windows.Forms.CheckBox();
            this.buttonProhodit = new System.Windows.Forms.Button();
            this.label13 = new System.Windows.Forms.Label();
            this.textBoxStopa = new System.Windows.Forms.TextBox();
            this.textBoxAlbum = new System.Windows.Forms.TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.buttonNeexistujiciVyhledat = new System.Windows.Forms.Button();
            this.checkBoxPuvodniNazevInterpret = new System.Windows.Forms.CheckBox();
            this.buttonPuvodniNazev = new System.Windows.Forms.Button();
            this.checkBoxPuvodniNazevVybrane = new System.Windows.Forms.CheckBox();
            this.checkBoxPuvodniNazevPlaylist = new System.Windows.Forms.CheckBox();
            this.webViewVideo = new Microsoft.Web.WebView2.WinForms.WebView2();
            ((System.ComponentModel.ISupportInitialize)(this.webViewVideo)).BeginInit();
            this.SuspendLayout();
            // 
            // textBoxInterpret
            // 
            this.textBoxInterpret.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxInterpret.Location = new System.Drawing.Point(111, 477);
            this.textBoxInterpret.MaxLength = 160;
            this.textBoxInterpret.Name = "textBoxInterpret";
            this.textBoxInterpret.Size = new System.Drawing.Size(816, 20);
            this.textBoxInterpret.TabIndex = 0;
            this.textBoxInterpret.TextChanged += new System.EventHandler(this.textBoxInterpret_TextChanged);
            // 
            // textBoxSkladba
            // 
            this.textBoxSkladba.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxSkladba.Location = new System.Drawing.Point(111, 525);
            this.textBoxSkladba.MaxLength = 160;
            this.textBoxSkladba.Name = "textBoxSkladba";
            this.textBoxSkladba.Size = new System.Drawing.Size(816, 20);
            this.textBoxSkladba.TabIndex = 1;
            this.textBoxSkladba.TextChanged += new System.EventHandler(this.textBoxSkladba_TextChanged);
            // 
            // textBoxFeaturing
            // 
            this.textBoxFeaturing.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxFeaturing.Location = new System.Drawing.Point(111, 576);
            this.textBoxFeaturing.MaxLength = 160;
            this.textBoxFeaturing.Name = "textBoxFeaturing";
            this.textBoxFeaturing.Size = new System.Drawing.Size(816, 20);
            this.textBoxFeaturing.TabIndex = 2;
            this.textBoxFeaturing.TextChanged += new System.EventHandler(this.textBoxFeaturing_TextChanged);
            // 
            // textBoxNovyNazev
            // 
            this.textBoxNovyNazev.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxNovyNazev.Location = new System.Drawing.Point(111, 602);
            this.textBoxNovyNazev.MaxLength = 200;
            this.textBoxNovyNazev.Name = "textBoxNovyNazev";
            this.textBoxNovyNazev.Size = new System.Drawing.Size(816, 20);
            this.textBoxNovyNazev.TabIndex = 3;
            this.textBoxNovyNazev.TextChanged += new System.EventHandler(this.textBoxNovyNazev_TextChanged);
            // 
            // textBoxPuvodniNazev
            // 
            this.textBoxPuvodniNazev.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxPuvodniNazev.Location = new System.Drawing.Point(111, 451);
            this.textBoxPuvodniNazev.Name = "textBoxPuvodniNazev";
            this.textBoxPuvodniNazev.ReadOnly = true;
            this.textBoxPuvodniNazev.Size = new System.Drawing.Size(816, 20);
            this.textBoxPuvodniNazev.TabIndex = 5;
            // 
            // buttonPredchozi
            // 
            this.buttonPredchozi.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonPredchozi.Location = new System.Drawing.Point(109, 792);
            this.buttonPredchozi.Name = "buttonPredchozi";
            this.buttonPredchozi.Size = new System.Drawing.Size(220, 27);
            this.buttonPredchozi.TabIndex = 9;
            this.buttonPredchozi.Text = "Předchozí";
            this.buttonPredchozi.UseVisualStyleBackColor = true;
            this.buttonPredchozi.Click += new System.EventHandler(this.buttonPredchozi_Click);
            // 
            // buttonNasledujici
            // 
            this.buttonNasledujici.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonNasledujici.Cursor = System.Windows.Forms.Cursors.Default;
            this.buttonNasledujici.Location = new System.Drawing.Point(707, 792);
            this.buttonNasledujici.Name = "buttonNasledujici";
            this.buttonNasledujici.Size = new System.Drawing.Size(220, 27);
            this.buttonNasledujici.TabIndex = 10;
            this.buttonNasledujici.Text = "Následující";
            this.buttonNasledujici.UseVisualStyleBackColor = true;
            this.buttonNasledujici.Click += new System.EventHandler(this.buttonNasledujici_Click);
            // 
            // checkBoxUlozit
            // 
            this.checkBoxUlozit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.checkBoxUlozit.AutoSize = true;
            this.checkBoxUlozit.Location = new System.Drawing.Point(20, 798);
            this.checkBoxUlozit.Name = "checkBoxUlozit";
            this.checkBoxUlozit.Size = new System.Drawing.Size(52, 17);
            this.checkBoxUlozit.TabIndex = 11;
            this.checkBoxUlozit.Text = "Uložit";
            this.checkBoxUlozit.UseVisualStyleBackColor = true;
            // 
            // buttonObnovit
            // 
            this.buttonObnovit.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonObnovit.Location = new System.Drawing.Point(412, 792);
            this.buttonObnovit.MaximumSize = new System.Drawing.Size(220, 27);
            this.buttonObnovit.Name = "buttonObnovit";
            this.buttonObnovit.Size = new System.Drawing.Size(220, 27);
            this.buttonObnovit.TabIndex = 12;
            this.buttonObnovit.Text = "Obnovit";
            this.buttonObnovit.UseVisualStyleBackColor = true;
            this.buttonObnovit.Click += new System.EventHandler(this.buttonObnovit_Click);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(17, 250);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(34, 13);
            this.label1.TabIndex = 13;
            this.label1.Text = "Kanál";
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(17, 451);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(80, 13);
            this.label2.TabIndex = 14;
            this.label2.Text = "Původní název";
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(17, 477);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(46, 13);
            this.label3.TabIndex = 15;
            this.label3.Text = "Interpret";
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(17, 525);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(46, 13);
            this.label4.TabIndex = 16;
            this.label4.Text = "Skladba";
            // 
            // label5
            // 
            this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(17, 576);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(94, 13);
            this.label5.TabIndex = 17;
            this.label5.Text = "Hostující interpreti";
            // 
            // label6
            // 
            this.label6.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(17, 602);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(64, 13);
            this.label6.TabIndex = 18;
            this.label6.Text = "Nový název";
            // 
            // buttonSlozkaOtevrit
            // 
            this.buttonSlozkaOtevrit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonSlozkaOtevrit.Enabled = false;
            this.buttonSlozkaOtevrit.Location = new System.Drawing.Point(707, 698);
            this.buttonSlozkaOtevrit.Name = "buttonSlozkaOtevrit";
            this.buttonSlozkaOtevrit.Size = new System.Drawing.Size(220, 27);
            this.buttonSlozkaOtevrit.TabIndex = 21;
            this.buttonSlozkaOtevrit.Text = "Otevřít složku v průzkumníku souborů";
            this.buttonSlozkaOtevrit.UseVisualStyleBackColor = true;
            this.buttonSlozkaOtevrit.Click += new System.EventHandler(this.buttonSlozkaOtevrit_Click);
            // 
            // label9
            // 
            this.label9.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(17, 649);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(39, 13);
            this.label9.TabIndex = 22;
            this.label9.Text = "Složka";
            // 
            // textBoxDatum
            // 
            this.textBoxDatum.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.textBoxDatum.Location = new System.Drawing.Point(111, 277);
            this.textBoxDatum.Name = "textBoxDatum";
            this.textBoxDatum.ReadOnly = true;
            this.textBoxDatum.Size = new System.Drawing.Size(280, 20);
            this.textBoxDatum.TabIndex = 25;
            // 
            // label10
            // 
            this.label10.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(17, 277);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(66, 13);
            this.label10.TabIndex = 26;
            this.label10.Text = "Publikováno";
            // 
            // label11
            // 
            this.label11.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(17, 224);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(47, 13);
            this.label11.TabIndex = 27;
            this.label11.Text = "ID videa";
            // 
            // buttonSlozkaJina
            // 
            this.buttonSlozkaJina.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonSlozkaJina.Location = new System.Drawing.Point(412, 698);
            this.buttonSlozkaJina.MaximumSize = new System.Drawing.Size(220, 27);
            this.buttonSlozkaJina.Name = "buttonSlozkaJina";
            this.buttonSlozkaJina.Size = new System.Drawing.Size(220, 27);
            this.buttonSlozkaJina.TabIndex = 29;
            this.buttonSlozkaJina.Text = "Vybrat jinou složku";
            this.buttonSlozkaJina.UseVisualStyleBackColor = true;
            this.buttonSlozkaJina.Click += new System.EventHandler(this.buttonSlozkaJina_Click);
            // 
            // textBoxSlozka
            // 
            this.textBoxSlozka.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxSlozka.Location = new System.Drawing.Point(111, 649);
            this.textBoxSlozka.MaxLength = 400;
            this.textBoxSlozka.Name = "textBoxSlozka";
            this.textBoxSlozka.Size = new System.Drawing.Size(816, 20);
            this.textBoxSlozka.TabIndex = 30;
            this.textBoxSlozka.TextChanged += new System.EventHandler(this.textBoxSlozka_TextChanged);
            // 
            // buttonSlozkaNajit
            // 
            this.buttonSlozkaNajit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonSlozkaNajit.Enabled = false;
            this.buttonSlozkaNajit.Location = new System.Drawing.Point(111, 698);
            this.buttonSlozkaNajit.Name = "buttonSlozkaNajit";
            this.buttonSlozkaNajit.Size = new System.Drawing.Size(220, 27);
            this.buttonSlozkaNajit.TabIndex = 31;
            this.buttonSlozkaNajit.Text = "Najít složku automaticky";
            this.buttonSlozkaNajit.UseVisualStyleBackColor = true;
            this.buttonSlozkaNajit.Click += new System.EventHandler(this.buttonSlozkaNajit_Click);
            // 
            // checkBoxStejnaSlozkaInterpret
            // 
            this.checkBoxStejnaSlozkaInterpret.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.checkBoxStejnaSlozkaInterpret.AutoSize = true;
            this.checkBoxStejnaSlozkaInterpret.Location = new System.Drawing.Point(111, 675);
            this.checkBoxStejnaSlozkaInterpret.Name = "checkBoxStejnaSlozkaInterpret";
            this.checkBoxStejnaSlozkaInterpret.Size = new System.Drawing.Size(261, 17);
            this.checkBoxStejnaSlozkaInterpret.TabIndex = 32;
            this.checkBoxStejnaSlozkaInterpret.Text = "Použít složku pro všechna videa tohoto interpreta";
            this.checkBoxStejnaSlozkaInterpret.UseVisualStyleBackColor = true;
            this.checkBoxStejnaSlozkaInterpret.CheckedChanged += new System.EventHandler(this.checkBoxZmena_CheckedChanged);
            // 
            // checkBoxStejnaSlozkaVybrane
            // 
            this.checkBoxStejnaSlozkaVybrane.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.checkBoxStejnaSlozkaVybrane.AutoSize = true;
            this.checkBoxStejnaSlozkaVybrane.Location = new System.Drawing.Point(412, 675);
            this.checkBoxStejnaSlozkaVybrane.Name = "checkBoxStejnaSlozkaVybrane";
            this.checkBoxStejnaSlozkaVybrane.Size = new System.Drawing.Size(265, 17);
            this.checkBoxStejnaSlozkaVybrane.TabIndex = 33;
            this.checkBoxStejnaSlozkaVybrane.Text = "Použít složku pro všechny aktuálně vybraná videa";
            this.checkBoxStejnaSlozkaVybrane.UseVisualStyleBackColor = true;
            this.checkBoxStejnaSlozkaVybrane.CheckedChanged += new System.EventHandler(this.checkBoxZmena_CheckedChanged);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(409, 9);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(34, 13);
            this.label7.TabIndex = 35;
            this.label7.Text = "Video";
            this.label7.Click += new System.EventHandler(this.label7_Click);
            // 
            // label8
            // 
            this.label8.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(17, 356);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(29, 13);
            this.label8.TabIndex = 36;
            this.label8.Text = "Žánr";
            // 
            // textBoxZanr
            // 
            this.textBoxZanr.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.textBoxZanr.Location = new System.Drawing.Point(111, 356);
            this.textBoxZanr.MaxLength = 45;
            this.textBoxZanr.Name = "textBoxZanr";
            this.textBoxZanr.Size = new System.Drawing.Size(280, 20);
            this.textBoxZanr.TabIndex = 37;
            this.textBoxZanr.TextChanged += new System.EventHandler(this.textBoxZanr_TextChanged);
            // 
            // checkBoxStejnyZanrInterpret
            // 
            this.checkBoxStejnyZanrInterpret.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.checkBoxStejnyZanrInterpret.AutoSize = true;
            this.checkBoxStejnyZanrInterpret.Location = new System.Drawing.Point(111, 382);
            this.checkBoxStejnyZanrInterpret.Name = "checkBoxStejnyZanrInterpret";
            this.checkBoxStejnyZanrInterpret.Size = new System.Drawing.Size(251, 17);
            this.checkBoxStejnyZanrInterpret.TabIndex = 38;
            this.checkBoxStejnyZanrInterpret.Text = "Použít žánr pro všechna videa tohoto interpreta";
            this.checkBoxStejnyZanrInterpret.UseVisualStyleBackColor = true;
            this.checkBoxStejnyZanrInterpret.CheckedChanged += new System.EventHandler(this.checkBoxZmena_CheckedChanged);
            // 
            // checkBoxStejnyZanrVybrane
            // 
            this.checkBoxStejnyZanrVybrane.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.checkBoxStejnyZanrVybrane.AutoSize = true;
            this.checkBoxStejnyZanrVybrane.Location = new System.Drawing.Point(111, 405);
            this.checkBoxStejnyZanrVybrane.Name = "checkBoxStejnyZanrVybrane";
            this.checkBoxStejnyZanrVybrane.Size = new System.Drawing.Size(256, 17);
            this.checkBoxStejnyZanrVybrane.TabIndex = 39;
            this.checkBoxStejnyZanrVybrane.Text = "Použít žánr pro všechna aktuálně vybrané videa";
            this.checkBoxStejnyZanrVybrane.UseVisualStyleBackColor = true;
            this.checkBoxStejnyZanrVybrane.CheckedChanged += new System.EventHandler(this.checkBoxZmena_CheckedChanged);
            // 
            // labelInterpret
            // 
            this.labelInterpret.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F);
            this.labelInterpret.Location = new System.Drawing.Point(0, 9);
            this.labelInterpret.Name = "labelInterpret";
            this.labelInterpret.Size = new System.Drawing.Size(403, 82);
            this.labelInterpret.TabIndex = 40;
            this.labelInterpret.Text = "-";
            this.labelInterpret.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.labelInterpret.UseMnemonic = false;
            // 
            // labelSkladba
            // 
            this.labelSkladba.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.labelSkladba.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F);
            this.labelSkladba.Location = new System.Drawing.Point(0, 91);
            this.labelSkladba.Name = "labelSkladba";
            this.labelSkladba.Size = new System.Drawing.Size(403, 107);
            this.labelSkladba.TabIndex = 41;
            this.labelSkladba.Text = "-";
            this.labelSkladba.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.labelSkladba.UseMnemonic = false;
            // 
            // linkLabelID
            // 
            this.linkLabelID.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.linkLabelID.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.linkLabelID.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.linkLabelID.LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline;
            this.linkLabelID.LinkColor = System.Drawing.Color.Black;
            this.linkLabelID.Location = new System.Drawing.Point(111, 224);
            this.linkLabelID.Name = "linkLabelID";
            this.linkLabelID.Size = new System.Drawing.Size(280, 20);
            this.linkLabelID.TabIndex = 28;
            this.linkLabelID.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabelID_LinkClicked);
            // 
            // linkLabelKanal
            // 
            this.linkLabelKanal.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.linkLabelKanal.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.linkLabelKanal.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.linkLabelKanal.LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline;
            this.linkLabelKanal.LinkColor = System.Drawing.Color.Black;
            this.linkLabelKanal.Location = new System.Drawing.Point(111, 250);
            this.linkLabelKanal.Name = "linkLabelKanal";
            this.linkLabelKanal.Size = new System.Drawing.Size(280, 20);
            this.linkLabelKanal.TabIndex = 28;
            this.linkLabelKanal.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabelKanal_LinkClicked);
            // 
            // checkBoxStejnyZanrPlaylist
            // 
            this.checkBoxStejnyZanrPlaylist.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.checkBoxStejnyZanrPlaylist.AutoSize = true;
            this.checkBoxStejnyZanrPlaylist.Location = new System.Drawing.Point(111, 428);
            this.checkBoxStejnyZanrPlaylist.Name = "checkBoxStejnyZanrPlaylist";
            this.checkBoxStejnyZanrPlaylist.Size = new System.Drawing.Size(219, 17);
            this.checkBoxStejnyZanrPlaylist.TabIndex = 39;
            this.checkBoxStejnyZanrPlaylist.Text = "Použít žánr pro všechna videa z playlistu";
            this.checkBoxStejnyZanrPlaylist.UseVisualStyleBackColor = true;
            this.checkBoxStejnyZanrPlaylist.CheckedChanged += new System.EventHandler(this.checkBoxZmena_CheckedChanged);
            // 
            // checkBoxStejnaSlozkaPlaylist
            // 
            this.checkBoxStejnaSlozkaPlaylist.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.checkBoxStejnaSlozkaPlaylist.AutoSize = true;
            this.checkBoxStejnaSlozkaPlaylist.Location = new System.Drawing.Point(699, 675);
            this.checkBoxStejnaSlozkaPlaylist.Name = "checkBoxStejnaSlozkaPlaylist";
            this.checkBoxStejnaSlozkaPlaylist.Size = new System.Drawing.Size(229, 17);
            this.checkBoxStejnaSlozkaPlaylist.TabIndex = 33;
            this.checkBoxStejnaSlozkaPlaylist.Text = "Použít složku pro všechna videa z playlistu";
            this.checkBoxStejnaSlozkaPlaylist.UseVisualStyleBackColor = true;
            this.checkBoxStejnaSlozkaPlaylist.CheckedChanged += new System.EventHandler(this.checkBoxZmena_CheckedChanged);
            // 
            // labelChyba
            // 
            this.labelChyba.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.labelChyba.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F);
            this.labelChyba.Location = new System.Drawing.Point(20, 728);
            this.labelChyba.Name = "labelChyba";
            this.labelChyba.Size = new System.Drawing.Size(907, 61);
            this.labelChyba.TabIndex = 41;
            this.labelChyba.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.labelChyba.UseMnemonic = false;
            // 
            // label12
            // 
            this.label12.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(17, 201);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(39, 13);
            this.label12.TabIndex = 27;
            this.label12.Text = "Playlist";
            // 
            // linkLabelPlaylist
            // 
            this.linkLabelPlaylist.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.linkLabelPlaylist.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.linkLabelPlaylist.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.linkLabelPlaylist.LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline;
            this.linkLabelPlaylist.LinkColor = System.Drawing.Color.Black;
            this.linkLabelPlaylist.Location = new System.Drawing.Point(111, 200);
            this.linkLabelPlaylist.Name = "linkLabelPlaylist";
            this.linkLabelPlaylist.Size = new System.Drawing.Size(280, 20);
            this.linkLabelPlaylist.TabIndex = 28;
            this.linkLabelPlaylist.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabelPlaylist_LinkClicked);
            // 
            // checkBoxNovyNazevAutomaticky
            // 
            this.checkBoxNovyNazevAutomaticky.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.checkBoxNovyNazevAutomaticky.AutoSize = true;
            this.checkBoxNovyNazevAutomaticky.Location = new System.Drawing.Point(111, 628);
            this.checkBoxNovyNazevAutomaticky.Name = "checkBoxNovyNazevAutomaticky";
            this.checkBoxNovyNazevAutomaticky.Size = new System.Drawing.Size(170, 17);
            this.checkBoxNovyNazevAutomaticky.TabIndex = 11;
            this.checkBoxNovyNazevAutomaticky.Text = "Automaticky měnit nový název";
            this.checkBoxNovyNazevAutomaticky.UseVisualStyleBackColor = true;
            this.checkBoxNovyNazevAutomaticky.CheckedChanged += new System.EventHandler(this.checkBoxNovyNazevAutomaticky_CheckedChanged);
            // 
            // richTextBoxPopis
            // 
            this.richTextBoxPopis.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.richTextBoxPopis.Location = new System.Drawing.Point(409, 302);
            this.richTextBoxPopis.Name = "richTextBoxPopis";
            this.richTextBoxPopis.ReadOnly = true;
            this.richTextBoxPopis.Size = new System.Drawing.Size(518, 142);
            this.richTextBoxPopis.TabIndex = 45;
            this.richTextBoxPopis.Text = "";
            // 
            // checkBoxStejnyInterpretVybrane
            // 
            this.checkBoxStejnyInterpretVybrane.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.checkBoxStejnyInterpretVybrane.AutoSize = true;
            this.checkBoxStejnyInterpretVybrane.Location = new System.Drawing.Point(111, 503);
            this.checkBoxStejnyInterpretVybrane.Name = "checkBoxStejnyInterpretVybrane";
            this.checkBoxStejnyInterpretVybrane.Size = new System.Drawing.Size(280, 17);
            this.checkBoxStejnyInterpretVybrane.TabIndex = 32;
            this.checkBoxStejnyInterpretVybrane.Text = "Použít interpreta pro všechna aktuálně vybraná videa";
            this.checkBoxStejnyInterpretVybrane.UseVisualStyleBackColor = true;
            this.checkBoxStejnyInterpretVybrane.CheckedChanged += new System.EventHandler(this.checkBoxZmena_CheckedChanged);
            // 
            // checkBoxStejnyInterpretPlaylist
            // 
            this.checkBoxStejnyInterpretPlaylist.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.checkBoxStejnyInterpretPlaylist.AutoSize = true;
            this.checkBoxStejnyInterpretPlaylist.Location = new System.Drawing.Point(412, 502);
            this.checkBoxStejnyInterpretPlaylist.Name = "checkBoxStejnyInterpretPlaylist";
            this.checkBoxStejnyInterpretPlaylist.Size = new System.Drawing.Size(243, 17);
            this.checkBoxStejnyInterpretPlaylist.TabIndex = 33;
            this.checkBoxStejnyInterpretPlaylist.Text = "Použít interpreta pro všechna videa z playlistu";
            this.checkBoxStejnyInterpretPlaylist.UseVisualStyleBackColor = true;
            this.checkBoxStejnyInterpretPlaylist.CheckedChanged += new System.EventHandler(this.checkBoxZmena_CheckedChanged);
            // 
            // buttonProhodit
            // 
            this.buttonProhodit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonProhodit.Location = new System.Drawing.Point(765, 499);
            this.buttonProhodit.Name = "buttonProhodit";
            this.buttonProhodit.Size = new System.Drawing.Size(162, 22);
            this.buttonProhodit.TabIndex = 46;
            this.buttonProhodit.Text = "Prohodit skladbu a interpreta";
            this.buttonProhodit.UseVisualStyleBackColor = true;
            this.buttonProhodit.Click += new System.EventHandler(this.buttonProhodit_Click);
            // 
            // label13
            // 
            this.label13.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(17, 330);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(35, 13);
            this.label13.TabIndex = 48;
            this.label13.Text = "Stopa";
            // 
            // textBoxStopa
            // 
            this.textBoxStopa.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.textBoxStopa.Location = new System.Drawing.Point(111, 330);
            this.textBoxStopa.Name = "textBoxStopa";
            this.textBoxStopa.ReadOnly = true;
            this.textBoxStopa.Size = new System.Drawing.Size(280, 20);
            this.textBoxStopa.TabIndex = 47;
            // 
            // textBoxAlbum
            // 
            this.textBoxAlbum.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.textBoxAlbum.Location = new System.Drawing.Point(111, 304);
            this.textBoxAlbum.Name = "textBoxAlbum";
            this.textBoxAlbum.ReadOnly = true;
            this.textBoxAlbum.Size = new System.Drawing.Size(280, 20);
            this.textBoxAlbum.TabIndex = 25;
            // 
            // label14
            // 
            this.label14.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(17, 304);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(36, 13);
            this.label14.TabIndex = 26;
            this.label14.Text = "Album";
            // 
            // buttonNeexistujiciVyhledat
            // 
            this.buttonNeexistujiciVyhledat.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonNeexistujiciVyhledat.Location = new System.Drawing.Point(412, 318);
            this.buttonNeexistujiciVyhledat.MaximumSize = new System.Drawing.Size(220, 27);
            this.buttonNeexistujiciVyhledat.Name = "buttonNeexistujiciVyhledat";
            this.buttonNeexistujiciVyhledat.Size = new System.Drawing.Size(220, 27);
            this.buttonNeexistujiciVyhledat.TabIndex = 29;
            this.buttonNeexistujiciVyhledat.Text = "Vyhledat na Google";
            this.buttonNeexistujiciVyhledat.UseVisualStyleBackColor = true;
            this.buttonNeexistujiciVyhledat.Click += new System.EventHandler(this.buttonNeexistujiciVyhledat_Click);
            // 
            // checkBoxPuvodniNazevInterpret
            // 
            this.checkBoxPuvodniNazevInterpret.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.checkBoxPuvodniNazevInterpret.AutoSize = true;
            this.checkBoxPuvodniNazevInterpret.Location = new System.Drawing.Point(270, 555);
            this.checkBoxPuvodniNazevInterpret.Name = "checkBoxPuvodniNazevInterpret";
            this.checkBoxPuvodniNazevInterpret.Size = new System.Drawing.Size(195, 17);
            this.checkBoxPuvodniNazevInterpret.TabIndex = 32;
            this.checkBoxPuvodniNazevInterpret.Text = "Pro všechna videa tohoto interpreta";
            this.checkBoxPuvodniNazevInterpret.UseVisualStyleBackColor = true;
            this.checkBoxPuvodniNazevInterpret.CheckedChanged += new System.EventHandler(this.checkBoxZmena_CheckedChanged);
            // 
            // buttonPuvodniNazev
            // 
            this.buttonPuvodniNazev.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonPuvodniNazev.Location = new System.Drawing.Point(109, 551);
            this.buttonPuvodniNazev.Name = "buttonPuvodniNazev";
            this.buttonPuvodniNazev.Size = new System.Drawing.Size(128, 22);
            this.buttonPuvodniNazev.TabIndex = 46;
            this.buttonPuvodniNazev.Text = "Použít původní název";
            this.buttonPuvodniNazev.UseVisualStyleBackColor = true;
            this.buttonPuvodniNazev.Click += new System.EventHandler(this.buttonPuvodniNazev_Click);
            // 
            // checkBoxPuvodniNazevVybrane
            // 
            this.checkBoxPuvodniNazevVybrane.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.checkBoxPuvodniNazevVybrane.AutoSize = true;
            this.checkBoxPuvodniNazevVybrane.Location = new System.Drawing.Point(509, 555);
            this.checkBoxPuvodniNazevVybrane.Name = "checkBoxPuvodniNazevVybrane";
            this.checkBoxPuvodniNazevVybrane.Size = new System.Drawing.Size(200, 17);
            this.checkBoxPuvodniNazevVybrane.TabIndex = 32;
            this.checkBoxPuvodniNazevVybrane.Text = "Pro všechna aktuálně vybraná videa";
            this.checkBoxPuvodniNazevVybrane.UseVisualStyleBackColor = true;
            this.checkBoxPuvodniNazevVybrane.CheckedChanged += new System.EventHandler(this.checkBoxZmena_CheckedChanged);
            // 
            // checkBoxPuvodniNazevPlaylist
            // 
            this.checkBoxPuvodniNazevPlaylist.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.checkBoxPuvodniNazevPlaylist.AutoSize = true;
            this.checkBoxPuvodniNazevPlaylist.Location = new System.Drawing.Point(765, 555);
            this.checkBoxPuvodniNazevPlaylist.Name = "checkBoxPuvodniNazevPlaylist";
            this.checkBoxPuvodniNazevPlaylist.Size = new System.Drawing.Size(163, 17);
            this.checkBoxPuvodniNazevPlaylist.TabIndex = 32;
            this.checkBoxPuvodniNazevPlaylist.Text = "Pro všechna videa z playlistu";
            this.checkBoxPuvodniNazevPlaylist.UseVisualStyleBackColor = true;
            this.checkBoxPuvodniNazevPlaylist.CheckedChanged += new System.EventHandler(this.checkBoxZmena_CheckedChanged);
            // 
            // webViewVideo
            // 
            this.webViewVideo.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.webViewVideo.CreationProperties = null;
            this.webViewVideo.DefaultBackgroundColor = System.Drawing.Color.White;
            this.webViewVideo.Location = new System.Drawing.Point(412, 25);
            this.webViewVideo.Name = "webViewVideo";
            this.webViewVideo.Size = new System.Drawing.Size(515, 265);
            this.webViewVideo.TabIndex = 49;
            this.webViewVideo.ZoomFactor = 1D;
            // 
            // FormUprava
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(939, 831);
            this.Controls.Add(this.webViewVideo);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.textBoxStopa);
            this.Controls.Add(this.buttonPuvodniNazev);
            this.Controls.Add(this.buttonProhodit);
            this.Controls.Add(this.labelInterpret);
            this.Controls.Add(this.checkBoxStejnyZanrPlaylist);
            this.Controls.Add(this.checkBoxStejnyZanrVybrane);
            this.Controls.Add(this.checkBoxStejnyZanrInterpret);
            this.Controls.Add(this.textBoxZanr);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.checkBoxStejnyInterpretPlaylist);
            this.Controls.Add(this.checkBoxStejnaSlozkaPlaylist);
            this.Controls.Add(this.checkBoxPuvodniNazevPlaylist);
            this.Controls.Add(this.checkBoxPuvodniNazevVybrane);
            this.Controls.Add(this.checkBoxPuvodniNazevInterpret);
            this.Controls.Add(this.checkBoxStejnyInterpretVybrane);
            this.Controls.Add(this.checkBoxStejnaSlozkaVybrane);
            this.Controls.Add(this.checkBoxStejnaSlozkaInterpret);
            this.Controls.Add(this.buttonSlozkaNajit);
            this.Controls.Add(this.textBoxSlozka);
            this.Controls.Add(this.buttonNeexistujiciVyhledat);
            this.Controls.Add(this.buttonSlozkaJina);
            this.Controls.Add(this.linkLabelKanal);
            this.Controls.Add(this.linkLabelPlaylist);
            this.Controls.Add(this.linkLabelID);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.label14);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.textBoxAlbum);
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
            this.Controls.Add(this.checkBoxNovyNazevAutomaticky);
            this.Controls.Add(this.checkBoxUlozit);
            this.Controls.Add(this.buttonNasledujici);
            this.Controls.Add(this.buttonPredchozi);
            this.Controls.Add(this.textBoxPuvodniNazev);
            this.Controls.Add(this.textBoxNovyNazev);
            this.Controls.Add(this.textBoxFeaturing);
            this.Controls.Add(this.textBoxSkladba);
            this.Controls.Add(this.textBoxInterpret);
            this.Controls.Add(this.labelChyba);
            this.Controls.Add(this.labelSkladba);
            this.Controls.Add(this.richTextBoxPopis);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(955, 726);
            this.Name = "FormUprava";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Úprava videa";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormUprava_FormClosing);
            this.Load += new System.EventHandler(this.FormUprava_Load);
            ((System.ComponentModel.ISupportInitialize)(this.webViewVideo)).EndInit();
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
        private System.Windows.Forms.Button buttonSlozkaJina;
        private System.Windows.Forms.TextBox textBoxSlozka;
        private System.Windows.Forms.Button buttonSlozkaNajit;
        private System.Windows.Forms.CheckBox checkBoxStejnaSlozkaInterpret;
        private System.Windows.Forms.CheckBox checkBoxStejnaSlozkaVybrane;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox textBoxZanr;
        private System.Windows.Forms.CheckBox checkBoxStejnyZanrInterpret;
        private System.Windows.Forms.CheckBox checkBoxStejnyZanrVybrane;
        private System.Windows.Forms.Label labelInterpret;
        private System.Windows.Forms.Label labelSkladba;
        private System.Windows.Forms.LinkLabel linkLabelID;
        private System.Windows.Forms.LinkLabel linkLabelKanal;
        private System.Windows.Forms.CheckBox checkBoxStejnyZanrPlaylist;
        private System.Windows.Forms.CheckBox checkBoxStejnaSlozkaPlaylist;
        private System.Windows.Forms.Label labelChyba;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.LinkLabel linkLabelPlaylist;
        private System.Windows.Forms.CheckBox checkBoxNovyNazevAutomaticky;
        private System.Windows.Forms.RichTextBox richTextBoxPopis;
        private System.Windows.Forms.CheckBox checkBoxStejnyInterpretVybrane;
        private System.Windows.Forms.CheckBox checkBoxStejnyInterpretPlaylist;
        private System.Windows.Forms.Button buttonProhodit;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.TextBox textBoxStopa;
        private System.Windows.Forms.TextBox textBoxAlbum;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Button buttonNeexistujiciVyhledat;
        private System.Windows.Forms.CheckBox checkBoxPuvodniNazevInterpret;
        private System.Windows.Forms.Button buttonPuvodniNazev;
        private System.Windows.Forms.CheckBox checkBoxPuvodniNazevVybrane;
        private System.Windows.Forms.CheckBox checkBoxPuvodniNazevPlaylist;
        private Microsoft.Web.WebView2.WinForms.WebView2 webViewVideo;
    }
}