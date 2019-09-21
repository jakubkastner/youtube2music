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
            this.pictureBoxCover = new System.Windows.Forms.PictureBox();
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
            this.olvColumn9 = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvColumn10 = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvColumn11 = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvColumn12 = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvColumn13 = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.buttonAktualizovat = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownRok)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxCover)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.treeListViewAlbaDeezer)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.treeListViewAlbaYoutube)).BeginInit();
            this.SuspendLayout();
            // 
            // textBoxInterpret
            // 
            this.textBoxInterpret.Location = new System.Drawing.Point(98, 30);
            this.textBoxInterpret.Name = "textBoxInterpret";
            this.textBoxInterpret.Size = new System.Drawing.Size(407, 20);
            this.textBoxInterpret.TabIndex = 0;
            // 
            // textBoxAlbum
            // 
            this.textBoxAlbum.Location = new System.Drawing.Point(98, 56);
            this.textBoxAlbum.Name = "textBoxAlbum";
            this.textBoxAlbum.Size = new System.Drawing.Size(407, 20);
            this.textBoxAlbum.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 33);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(46, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Interpret";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 56);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(61, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Název alba";
            // 
            // numericUpDownRok
            // 
            this.numericUpDownRok.Location = new System.Drawing.Point(98, 463);
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
            this.label3.Location = new System.Drawing.Point(12, 462);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(27, 13);
            this.label3.TabIndex = 3;
            this.label3.Text = "Rok";
            // 
            // textBoxZanr
            // 
            this.textBoxZanr.Location = new System.Drawing.Point(98, 490);
            this.textBoxZanr.Name = "textBoxZanr";
            this.textBoxZanr.Size = new System.Drawing.Size(407, 20);
            this.textBoxZanr.TabIndex = 5;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(13, 490);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(29, 13);
            this.label4.TabIndex = 3;
            this.label4.Text = "Žánr";
            // 
            // buttonPridatAlbum
            // 
            this.buttonPridatAlbum.Location = new System.Drawing.Point(98, 722);
            this.buttonPridatAlbum.Name = "buttonPridatAlbum";
            this.buttonPridatAlbum.Size = new System.Drawing.Size(99, 23);
            this.buttonPridatAlbum.TabIndex = 6;
            this.buttonPridatAlbum.Text = "Přidat album";
            this.buttonPridatAlbum.UseVisualStyleBackColor = true;
            this.buttonPridatAlbum.Click += new System.EventHandler(this.ButtonPridatAlbum_Click);
            // 
            // buttonVyhledatDeezer
            // 
            this.buttonVyhledatDeezer.Location = new System.Drawing.Point(98, 82);
            this.buttonVyhledatDeezer.Name = "buttonVyhledatDeezer";
            this.buttonVyhledatDeezer.Size = new System.Drawing.Size(198, 23);
            this.buttonVyhledatDeezer.TabIndex = 7;
            this.buttonVyhledatDeezer.Text = "Vyhledat na Deezeru";
            this.buttonVyhledatDeezer.UseVisualStyleBackColor = true;
            this.buttonVyhledatDeezer.Click += new System.EventHandler(this.ButtonVyhledatDeezer_Click);
            // 
            // pictureBoxCover
            // 
            this.pictureBoxCover.Location = new System.Drawing.Point(555, 466);
            this.pictureBoxCover.Name = "pictureBoxCover";
            this.pictureBoxCover.Size = new System.Drawing.Size(200, 200);
            this.pictureBoxCover.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBoxCover.TabIndex = 8;
            this.pictureBoxCover.TabStop = false;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(514, 466);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(35, 13);
            this.label5.TabIndex = 3;
            this.label5.Text = "Cover";
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
            this.treeListViewAlbaDeezer.FullRowSelect = true;
            this.treeListViewAlbaDeezer.HideSelection = false;
            this.treeListViewAlbaDeezer.Location = new System.Drawing.Point(4, 111);
            this.treeListViewAlbaDeezer.Name = "treeListViewAlbaDeezer";
            this.treeListViewAlbaDeezer.ShowGroups = false;
            this.treeListViewAlbaDeezer.Size = new System.Drawing.Size(565, 346);
            this.treeListViewAlbaDeezer.TabIndex = 9;
            this.treeListViewAlbaDeezer.UseCompatibleStateImageBehavior = false;
            this.treeListViewAlbaDeezer.UseFiltering = true;
            this.treeListViewAlbaDeezer.View = System.Windows.Forms.View.Details;
            this.treeListViewAlbaDeezer.VirtualMode = true;
            this.treeListViewAlbaDeezer.SelectedIndexChanged += new System.EventHandler(this.TreeListViewAlbaDeezer_SelectedIndexChanged);
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
            this.linkLabelOdkaz.Location = new System.Drawing.Point(95, 9);
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
            this.label6.Location = new System.Drawing.Point(13, 9);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(39, 13);
            this.label6.TabIndex = 2;
            this.label6.Text = "Playlist";
            // 
            // textBoxSlozka
            // 
            this.textBoxSlozka.Location = new System.Drawing.Point(98, 516);
            this.textBoxSlozka.Name = "textBoxSlozka";
            this.textBoxSlozka.Size = new System.Drawing.Size(407, 20);
            this.textBoxSlozka.TabIndex = 11;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(10, 516);
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
            this.treeListViewAlbaYoutube.FullRowSelect = true;
            this.treeListViewAlbaYoutube.HideSelection = false;
            this.treeListViewAlbaYoutube.Location = new System.Drawing.Point(575, 111);
            this.treeListViewAlbaYoutube.Name = "treeListViewAlbaYoutube";
            this.treeListViewAlbaYoutube.ShowGroups = false;
            this.treeListViewAlbaYoutube.Size = new System.Drawing.Size(518, 346);
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
            // olvColumn11
            // 
            this.olvColumn11.AspectName = "Stopa";
            this.olvColumn11.Text = "Stopa";
            // 
            // olvColumn12
            // 
            this.olvColumn12.AspectName = "Chyba";
            this.olvColumn12.Text = "Chyba";
            // 
            // olvColumn13
            // 
            this.olvColumn13.AspectName = "Slozka";
            this.olvColumn13.Text = "Slozka";
            this.olvColumn13.Width = 135;
            // 
            // buttonAktualizovat
            // 
            this.buttonAktualizovat.Location = new System.Drawing.Point(575, 82);
            this.buttonAktualizovat.Name = "buttonAktualizovat";
            this.buttonAktualizovat.Size = new System.Drawing.Size(198, 23);
            this.buttonAktualizovat.TabIndex = 7;
            this.buttonAktualizovat.Text = "Aktualizovat seznam";
            this.buttonAktualizovat.UseVisualStyleBackColor = true;
            this.buttonAktualizovat.Click += new System.EventHandler(this.ButtonAktualizovat_Click);
            // 
            // FormAlbum
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1111, 751);
            this.Controls.Add(this.textBoxSlozka);
            this.Controls.Add(this.linkLabelOdkaz);
            this.Controls.Add(this.treeListViewAlbaYoutube);
            this.Controls.Add(this.treeListViewAlbaDeezer);
            this.Controls.Add(this.pictureBoxCover);
            this.Controls.Add(this.buttonAktualizovat);
            this.Controls.Add(this.buttonVyhledatDeezer);
            this.Controls.Add(this.buttonPridatAlbum);
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
            this.Name = "FormAlbum";
            this.Text = "FormAlbum";
            this.Load += new System.EventHandler(this.FormAlbum_Load);
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownRok)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxCover)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.treeListViewAlbaDeezer)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.treeListViewAlbaYoutube)).EndInit();
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
        private System.Windows.Forms.PictureBox pictureBoxCover;
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
        private System.Windows.Forms.Button buttonAktualizovat;
    }
}