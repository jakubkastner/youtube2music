namespace youtube2music
{
    partial class FormAlbum
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormAlbum));
            this.textBoxInterpret = new System.Windows.Forms.TextBox();
            this.textBoxAlbum = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.numericUpDownRok = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.textBoxZanr = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.buttonPridatAlbum = new System.Windows.Forms.Button();
            this.buttonVyhledatDeezer = new System.Windows.Forms.Button();
            this.pictureBoxCoverPredni = new System.Windows.Forms.PictureBox();
            this.label5 = new System.Windows.Forms.Label();
            this.backgroundWorkerVyhledatDeezer = new System.ComponentModel.BackgroundWorker();
            this.treeListViewAlbaDeezer = new BrightIdeasSoftware.TreeListView();
            this.olvColumn1 = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvColumn2 = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvColumn3 = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvColumn4 = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.linkLabelOdkaz = new System.Windows.Forms.LinkLabel();
            this.label6 = new System.Windows.Forms.Label();
            this.textBoxSlozka = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.treeListViewAlbaYoutube = new BrightIdeasSoftware.TreeListView();
            this.olvColumn5 = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvColumn11 = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvColumn9 = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvColumn10 = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvColumn12 = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvColumn13 = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.buttonSlozkaZmenit = new System.Windows.Forms.Button();
            this.buttonSlozkaOtevit = new System.Windows.Forms.Button();
            this.buttonCoverZadniZmenit = new System.Windows.Forms.Button();
            this.label8 = new System.Windows.Forms.Label();
            this.pictureBoxCoverZadni = new System.Windows.Forms.PictureBox();
            this.buttonCoverPredniZmenit = new System.Windows.Forms.Button();
            this.buttonCoverPredniSmazat = new System.Windows.Forms.Button();
            this.buttonCoverZadniSmazat = new System.Windows.Forms.Button();
            this.toolTipInformace = new System.Windows.Forms.ToolTip(this.components);
            this.checkBoxCoverPredniDeezer = new System.Windows.Forms.CheckBox();
            this.buttonSlozkaNajit = new System.Windows.Forms.Button();
            this.checkBoxVyhledatDeezerSingly = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownRok)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxCoverPredni)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.treeListViewAlbaDeezer)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.treeListViewAlbaYoutube)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxCoverZadni)).BeginInit();
            this.SuspendLayout();
            // 
            // textBoxInterpret
            // 
            this.textBoxInterpret.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxInterpret.Location = new System.Drawing.Point(70, 30);
            this.textBoxInterpret.Name = "textBoxInterpret";
            this.textBoxInterpret.Size = new System.Drawing.Size(407, 20);
            this.textBoxInterpret.TabIndex = 0;
            // 
            // textBoxAlbum
            // 
            this.textBoxAlbum.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxAlbum.Location = new System.Drawing.Point(70, 56);
            this.textBoxAlbum.Name = "textBoxAlbum";
            this.textBoxAlbum.Size = new System.Drawing.Size(407, 20);
            this.textBoxAlbum.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 30);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(46, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Interpret";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 56);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(61, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Název alba";
            // 
            // numericUpDownRok
            // 
            this.numericUpDownRok.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.numericUpDownRok.Location = new System.Drawing.Point(70, 113);
            this.numericUpDownRok.Maximum = new decimal(new int[] {
            2100,
            0,
            0,
            0});
            this.numericUpDownRok.Minimum = new decimal(new int[] {
            1900,
            0,
            0,
            0});
            this.numericUpDownRok.Name = "numericUpDownRok";
            this.numericUpDownRok.Size = new System.Drawing.Size(407, 20);
            this.numericUpDownRok.TabIndex = 4;
            this.numericUpDownRok.Value = new decimal(new int[] {
            1900,
            0,
            0,
            0});
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(3, 113);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(27, 13);
            this.label3.TabIndex = 3;
            this.label3.Text = "Rok";
            // 
            // textBoxZanr
            // 
            this.textBoxZanr.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxZanr.Location = new System.Drawing.Point(70, 140);
            this.textBoxZanr.Name = "textBoxZanr";
            this.textBoxZanr.Size = new System.Drawing.Size(407, 20);
            this.textBoxZanr.TabIndex = 5;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(3, 140);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(29, 13);
            this.label4.TabIndex = 3;
            this.label4.Text = "Žánr";
            // 
            // buttonPridatAlbum
            // 
            this.buttonPridatAlbum.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonPridatAlbum.Location = new System.Drawing.Point(4, 722);
            this.buttonPridatAlbum.Name = "buttonPridatAlbum";
            this.buttonPridatAlbum.Size = new System.Drawing.Size(130, 23);
            this.buttonPridatAlbum.TabIndex = 6;
            this.buttonPridatAlbum.Text = "Přidat album";
            this.buttonPridatAlbum.UseVisualStyleBackColor = true;
            this.buttonPridatAlbum.Click += new System.EventHandler(this.ButtonPridatAlbum_Click);
            // 
            // buttonVyhledatDeezer
            // 
            this.buttonVyhledatDeezer.Location = new System.Drawing.Point(70, 82);
            this.buttonVyhledatDeezer.Name = "buttonVyhledatDeezer";
            this.buttonVyhledatDeezer.Size = new System.Drawing.Size(130, 23);
            this.buttonVyhledatDeezer.TabIndex = 7;
            this.buttonVyhledatDeezer.Text = "Vyhledat na Deezeru";
            this.buttonVyhledatDeezer.UseVisualStyleBackColor = true;
            this.buttonVyhledatDeezer.Click += new System.EventHandler(this.ButtonVyhledatDeezer_Click);
            // 
            // pictureBoxCoverPredni
            // 
            this.pictureBoxCoverPredni.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBoxCoverPredni.Location = new System.Drawing.Point(675, 8);
            this.pictureBoxCoverPredni.Name = "pictureBoxCoverPredni";
            this.pictureBoxCoverPredni.Size = new System.Drawing.Size(180, 180);
            this.pictureBoxCoverPredni.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBoxCoverPredni.TabIndex = 8;
            this.pictureBoxCoverPredni.TabStop = false;
            this.pictureBoxCoverPredni.LocationChanged += new System.EventHandler(this.pictureBoxCoverPredni_LocationChanged);
            this.pictureBoxCoverPredni.Click += new System.EventHandler(this.PictureBoxCover_Click);
            // 
            // label5
            // 
            this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(599, 9);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(70, 13);
            this.label5.TabIndex = 3;
            this.label5.Text = "Přední cover";
            // 
            // backgroundWorkerVyhledatDeezer
            // 
            this.backgroundWorkerVyhledatDeezer.WorkerReportsProgress = true;
            this.backgroundWorkerVyhledatDeezer.WorkerSupportsCancellation = true;
            this.backgroundWorkerVyhledatDeezer.DoWork += new System.ComponentModel.DoWorkEventHandler(this.BackgroundWorkerVyhledatDeezer_DoWork);
            this.backgroundWorkerVyhledatDeezer.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.BackgroundWorkerVyhledatDeezer_RunWorkerCompleted);
            // 
            // treeListViewAlbaDeezer
            // 
            this.treeListViewAlbaDeezer.AllColumns.Add(this.olvColumn1);
            this.treeListViewAlbaDeezer.AllColumns.Add(this.olvColumn2);
            this.treeListViewAlbaDeezer.AllColumns.Add(this.olvColumn3);
            this.treeListViewAlbaDeezer.AllColumns.Add(this.olvColumn4);
            this.treeListViewAlbaDeezer.AllowColumnReorder = true;
            this.treeListViewAlbaDeezer.CellEditUseWholeCell = false;
            this.treeListViewAlbaDeezer.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.olvColumn1,
            this.olvColumn2,
            this.olvColumn3,
            this.olvColumn4});
            this.treeListViewAlbaDeezer.Cursor = System.Windows.Forms.Cursors.Default;
            this.treeListViewAlbaDeezer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeListViewAlbaDeezer.FullRowSelect = true;
            this.treeListViewAlbaDeezer.HideSelection = false;
            this.treeListViewAlbaDeezer.Location = new System.Drawing.Point(0, 0);
            this.treeListViewAlbaDeezer.Name = "treeListViewAlbaDeezer";
            this.treeListViewAlbaDeezer.ShowGroups = false;
            this.treeListViewAlbaDeezer.Size = new System.Drawing.Size(493, 493);
            this.treeListViewAlbaDeezer.TabIndex = 9;
            this.treeListViewAlbaDeezer.UseCompatibleStateImageBehavior = false;
            this.treeListViewAlbaDeezer.UseFiltering = true;
            this.treeListViewAlbaDeezer.View = System.Windows.Forms.View.Details;
            this.treeListViewAlbaDeezer.VirtualMode = true;
            this.treeListViewAlbaDeezer.SelectedIndexChanged += new System.EventHandler(this.treeListViewAlbaDeezer_SelectedIndexChanged);
            // 
            // olvColumn1
            // 
            this.olvColumn1.AspectName = "Interpret";
            this.olvColumn1.Text = "Interpret";
            this.olvColumn1.Width = 92;
            // 
            // olvColumn2
            // 
            this.olvColumn2.AspectName = "Cislo";
            this.olvColumn2.Text = "Číslo";
            this.olvColumn2.Width = 66;
            // 
            // olvColumn3
            // 
            this.olvColumn3.AspectName = "Nazev";
            this.olvColumn3.Text = "Název";
            this.olvColumn3.Width = 89;
            // 
            // olvColumn4
            // 
            this.olvColumn4.AspectName = "Datum";
            this.olvColumn4.Text = "Datum";
            // 
            // linkLabelOdkaz
            // 
            this.linkLabelOdkaz.AutoSize = true;
            this.linkLabelOdkaz.Location = new System.Drawing.Point(67, 9);
            this.linkLabelOdkaz.Name = "linkLabelOdkaz";
            this.linkLabelOdkaz.Size = new System.Drawing.Size(36, 13);
            this.linkLabelOdkaz.TabIndex = 10;
            this.linkLabelOdkaz.TabStop = true;
            this.linkLabelOdkaz.Text = "odkaz";
            this.linkLabelOdkaz.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.LinkLabelOdkaz_LinkClicked);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(3, 9);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(39, 13);
            this.label6.TabIndex = 2;
            this.label6.Text = "Playlist";
            // 
            // textBoxSlozka
            // 
            this.textBoxSlozka.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxSlozka.Location = new System.Drawing.Point(70, 166);
            this.textBoxSlozka.Name = "textBoxSlozka";
            this.textBoxSlozka.Size = new System.Drawing.Size(599, 20);
            this.textBoxSlozka.TabIndex = 11;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(3, 166);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(39, 13);
            this.label7.TabIndex = 3;
            this.label7.Text = "Složka";
            // 
            // treeListViewAlbaYoutube
            // 
            this.treeListViewAlbaYoutube.AllColumns.Add(this.olvColumn5);
            this.treeListViewAlbaYoutube.AllColumns.Add(this.olvColumn11);
            this.treeListViewAlbaYoutube.AllColumns.Add(this.olvColumn9);
            this.treeListViewAlbaYoutube.AllColumns.Add(this.olvColumn10);
            this.treeListViewAlbaYoutube.AllColumns.Add(this.olvColumn12);
            this.treeListViewAlbaYoutube.AllColumns.Add(this.olvColumn13);
            this.treeListViewAlbaYoutube.AllowColumnReorder = true;
            this.treeListViewAlbaYoutube.CellEditUseWholeCell = false;
            this.treeListViewAlbaYoutube.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.olvColumn5,
            this.olvColumn11,
            this.olvColumn9,
            this.olvColumn10,
            this.olvColumn12,
            this.olvColumn13});
            this.treeListViewAlbaYoutube.Cursor = System.Windows.Forms.Cursors.Default;
            this.treeListViewAlbaYoutube.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeListViewAlbaYoutube.FullRowSelect = true;
            this.treeListViewAlbaYoutube.HideSelection = false;
            this.treeListViewAlbaYoutube.Location = new System.Drawing.Point(0, 0);
            this.treeListViewAlbaYoutube.Name = "treeListViewAlbaYoutube";
            this.treeListViewAlbaYoutube.ShowGroups = false;
            this.treeListViewAlbaYoutube.Size = new System.Drawing.Size(608, 493);
            this.treeListViewAlbaYoutube.TabIndex = 9;
            this.treeListViewAlbaYoutube.UseCompatibleStateImageBehavior = false;
            this.treeListViewAlbaYoutube.UseFiltering = true;
            this.treeListViewAlbaYoutube.View = System.Windows.Forms.View.Details;
            this.treeListViewAlbaYoutube.VirtualMode = true;
            // 
            // olvColumn5
            // 
            this.olvColumn5.AspectName = "Interpret";
            this.olvColumn5.Text = "Interpret";
            this.olvColumn5.Width = 104;
            // 
            // olvColumn11
            // 
            this.olvColumn11.AspectName = "Stopa";
            this.olvColumn11.Text = "Stopa";
            // 
            // olvColumn9
            // 
            this.olvColumn9.AspectName = "Skladba";
            this.olvColumn9.Text = "Skladba";
            this.olvColumn9.Width = 75;
            // 
            // olvColumn10
            // 
            this.olvColumn10.AspectName = "InterpretiFeat";
            this.olvColumn10.Text = "Hostující interpreti";
            this.olvColumn10.Width = 117;
            this.olvColumn10.WordWrap = true;
            // 
            // olvColumn12
            // 
            this.olvColumn12.AspectName = "Chyba";
            this.olvColumn12.Text = "Chyba";
            // 
            // olvColumn13
            // 
            this.olvColumn13.AspectName = "Slozka";
            this.olvColumn13.Text = "Složka";
            this.olvColumn13.Width = 135;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer1.Location = new System.Drawing.Point(4, 223);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.treeListViewAlbaDeezer);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.treeListViewAlbaYoutube);
            this.splitContainer1.Size = new System.Drawing.Size(1105, 493);
            this.splitContainer1.SplitterDistance = 493;
            this.splitContainer1.TabIndex = 12;
            // 
            // buttonSlozkaZmenit
            // 
            this.buttonSlozkaZmenit.Location = new System.Drawing.Point(348, 192);
            this.buttonSlozkaZmenit.Name = "buttonSlozkaZmenit";
            this.buttonSlozkaZmenit.Size = new System.Drawing.Size(130, 23);
            this.buttonSlozkaZmenit.TabIndex = 7;
            this.buttonSlozkaZmenit.Text = "Změnit složku";
            this.buttonSlozkaZmenit.UseVisualStyleBackColor = true;
            this.buttonSlozkaZmenit.Click += new System.EventHandler(this.buttonSlozkaZmenit_Click);
            // 
            // buttonSlozkaOtevit
            // 
            this.buttonSlozkaOtevit.Location = new System.Drawing.Point(212, 192);
            this.buttonSlozkaOtevit.Name = "buttonSlozkaOtevit";
            this.buttonSlozkaOtevit.Size = new System.Drawing.Size(130, 23);
            this.buttonSlozkaOtevit.TabIndex = 7;
            this.buttonSlozkaOtevit.Text = "Otevřít složku";
            this.buttonSlozkaOtevit.UseVisualStyleBackColor = true;
            this.buttonSlozkaOtevit.Click += new System.EventHandler(this.buttonSlozkaOtevit_Click);
            // 
            // buttonCoverZadniZmenit
            // 
            this.buttonCoverZadniZmenit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCoverZadniZmenit.Location = new System.Drawing.Point(929, 192);
            this.buttonCoverZadniZmenit.Name = "buttonCoverZadniZmenit";
            this.buttonCoverZadniZmenit.Size = new System.Drawing.Size(130, 23);
            this.buttonCoverZadniZmenit.TabIndex = 7;
            this.buttonCoverZadniZmenit.Text = "Změnit zadní cover";
            this.buttonCoverZadniZmenit.UseVisualStyleBackColor = true;
            this.buttonCoverZadniZmenit.Click += new System.EventHandler(this.ButtonCoverZadniZmenit_Click);
            // 
            // label8
            // 
            this.label8.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(857, 9);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(66, 13);
            this.label8.TabIndex = 3;
            this.label8.Text = "Zadní cover";
            // 
            // pictureBoxCoverZadni
            // 
            this.pictureBoxCoverZadni.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBoxCoverZadni.Location = new System.Drawing.Point(929, 8);
            this.pictureBoxCoverZadni.Name = "pictureBoxCoverZadni";
            this.pictureBoxCoverZadni.Size = new System.Drawing.Size(180, 180);
            this.pictureBoxCoverZadni.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBoxCoverZadni.TabIndex = 8;
            this.pictureBoxCoverZadni.TabStop = false;
            this.pictureBoxCoverZadni.LocationChanged += new System.EventHandler(this.pictureBoxCoverZadni_LocationChanged);
            this.pictureBoxCoverZadni.Click += new System.EventHandler(this.PictureBoxCover_Click);
            // 
            // buttonCoverPredniZmenit
            // 
            this.buttonCoverPredniZmenit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCoverPredniZmenit.Location = new System.Drawing.Point(675, 192);
            this.buttonCoverPredniZmenit.Name = "buttonCoverPredniZmenit";
            this.buttonCoverPredniZmenit.Size = new System.Drawing.Size(130, 23);
            this.buttonCoverPredniZmenit.TabIndex = 7;
            this.buttonCoverPredniZmenit.Text = "Změnit přední cover";
            this.buttonCoverPredniZmenit.UseVisualStyleBackColor = true;
            this.buttonCoverPredniZmenit.Click += new System.EventHandler(this.ButtonCoverPredniZmenit_Click);
            // 
            // buttonCoverPredniSmazat
            // 
            this.buttonCoverPredniSmazat.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCoverPredniSmazat.Location = new System.Drawing.Point(811, 192);
            this.buttonCoverPredniSmazat.Name = "buttonCoverPredniSmazat";
            this.buttonCoverPredniSmazat.Size = new System.Drawing.Size(44, 23);
            this.buttonCoverPredniSmazat.TabIndex = 7;
            this.buttonCoverPredniSmazat.Text = "X";
            this.buttonCoverPredniSmazat.UseVisualStyleBackColor = true;
            this.buttonCoverPredniSmazat.Click += new System.EventHandler(this.ButtonCoverPredniSmazat_Click);
            // 
            // buttonCoverZadniSmazat
            // 
            this.buttonCoverZadniSmazat.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCoverZadniSmazat.Location = new System.Drawing.Point(1065, 192);
            this.buttonCoverZadniSmazat.Name = "buttonCoverZadniSmazat";
            this.buttonCoverZadniSmazat.Size = new System.Drawing.Size(44, 23);
            this.buttonCoverZadniSmazat.TabIndex = 7;
            this.buttonCoverZadniSmazat.Text = "X";
            this.buttonCoverZadniSmazat.UseVisualStyleBackColor = true;
            this.buttonCoverZadniSmazat.Click += new System.EventHandler(this.ButtonCoverZadniSmazat_Click);
            // 
            // checkBoxCoverPredniDeezer
            // 
            this.checkBoxCoverPredniDeezer.AutoSize = true;
            this.checkBoxCoverPredniDeezer.Checked = true;
            this.checkBoxCoverPredniDeezer.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxCoverPredniDeezer.Location = new System.Drawing.Point(484, 196);
            this.checkBoxCoverPredniDeezer.Name = "checkBoxCoverPredniDeezer";
            this.checkBoxCoverPredniDeezer.Size = new System.Drawing.Size(185, 17);
            this.checkBoxCoverPredniDeezer.TabIndex = 13;
            this.checkBoxCoverPredniDeezer.Text = "Získávat přední cover z Deezeru";
            this.checkBoxCoverPredniDeezer.UseVisualStyleBackColor = true;
            // 
            // buttonSlozkaNajit
            // 
            this.buttonSlozkaNajit.Location = new System.Drawing.Point(70, 192);
            this.buttonSlozkaNajit.Name = "buttonSlozkaNajit";
            this.buttonSlozkaNajit.Size = new System.Drawing.Size(136, 23);
            this.buttonSlozkaNajit.TabIndex = 14;
            this.buttonSlozkaNajit.Text = "Najít složku automaticky";
            this.buttonSlozkaNajit.UseVisualStyleBackColor = true;
            this.buttonSlozkaNajit.Click += new System.EventHandler(this.buttonSlozkaNajit_Click);
            // 
            // checkBoxVyhledatDeezerSingly
            // 
            this.checkBoxVyhledatDeezerSingly.AutoSize = true;
            this.checkBoxVyhledatDeezerSingly.Location = new System.Drawing.Point(206, 86);
            this.checkBoxVyhledatDeezerSingly.Name = "checkBoxVyhledatDeezerSingly";
            this.checkBoxVyhledatDeezerSingly.Size = new System.Drawing.Size(101, 17);
            this.checkBoxVyhledatDeezerSingly.TabIndex = 15;
            this.checkBoxVyhledatDeezerSingly.Text = "Vyhledat i singly";
            this.checkBoxVyhledatDeezerSingly.UseVisualStyleBackColor = true;
            // 
            // FormAlbum
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1111, 751);
            this.Controls.Add(this.checkBoxVyhledatDeezerSingly);
            this.Controls.Add(this.buttonSlozkaNajit);
            this.Controls.Add(this.checkBoxCoverPredniDeezer);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.textBoxSlozka);
            this.Controls.Add(this.linkLabelOdkaz);
            this.Controls.Add(this.pictureBoxCoverZadni);
            this.Controls.Add(this.pictureBoxCoverPredni);
            this.Controls.Add(this.buttonCoverZadniSmazat);
            this.Controls.Add(this.buttonCoverPredniSmazat);
            this.Controls.Add(this.buttonCoverPredniZmenit);
            this.Controls.Add(this.buttonCoverZadniZmenit);
            this.Controls.Add(this.buttonSlozkaOtevit);
            this.Controls.Add(this.buttonSlozkaZmenit);
            this.Controls.Add(this.buttonVyhledatDeezer);
            this.Controls.Add(this.buttonPridatAlbum);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.textBoxZanr);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.numericUpDownRok);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBoxAlbum);
            this.Controls.Add(this.textBoxInterpret);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormAlbum";
            this.ShowIcon = false;
            this.Text = "FormAlbum";
            this.Load += new System.EventHandler(this.FormAlbum_Load);
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownRok)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxCoverPredni)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.treeListViewAlbaDeezer)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.treeListViewAlbaYoutube)).EndInit();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxCoverZadni)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBoxInterpret;
        private System.Windows.Forms.TextBox textBoxAlbum;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown numericUpDownRok;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBoxZanr;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button buttonPridatAlbum;
        private System.Windows.Forms.Button buttonVyhledatDeezer;
        private System.Windows.Forms.PictureBox pictureBoxCoverPredni;
        private System.Windows.Forms.Label label5;
        private System.ComponentModel.BackgroundWorker backgroundWorkerVyhledatDeezer;
        private BrightIdeasSoftware.TreeListView treeListViewAlbaDeezer;
        private BrightIdeasSoftware.OLVColumn olvColumn1;
        private BrightIdeasSoftware.OLVColumn olvColumn2;
        private BrightIdeasSoftware.OLVColumn olvColumn3;
        private BrightIdeasSoftware.OLVColumn olvColumn4;
        private System.Windows.Forms.LinkLabel linkLabelOdkaz;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox textBoxSlozka;
        private System.Windows.Forms.Label label7;
        private BrightIdeasSoftware.TreeListView treeListViewAlbaYoutube;
        private BrightIdeasSoftware.OLVColumn olvColumn5;
        private BrightIdeasSoftware.OLVColumn olvColumn9;
        private BrightIdeasSoftware.OLVColumn olvColumn10;
        private BrightIdeasSoftware.OLVColumn olvColumn11;
        private BrightIdeasSoftware.OLVColumn olvColumn12;
        private BrightIdeasSoftware.OLVColumn olvColumn13;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Button buttonSlozkaZmenit;
        private System.Windows.Forms.Button buttonSlozkaOtevit;
        private System.Windows.Forms.Button buttonCoverZadniZmenit;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.PictureBox pictureBoxCoverZadni;
        private System.Windows.Forms.Button buttonCoverPredniZmenit;
        private System.Windows.Forms.Button buttonCoverPredniSmazat;
        private System.Windows.Forms.Button buttonCoverZadniSmazat;
        private System.Windows.Forms.ToolTip toolTipInformace;
        private System.Windows.Forms.CheckBox checkBoxCoverPredniDeezer;
        private System.Windows.Forms.Button buttonSlozkaNajit;
        private System.Windows.Forms.CheckBox checkBoxVyhledatDeezerSingly;
    }
}