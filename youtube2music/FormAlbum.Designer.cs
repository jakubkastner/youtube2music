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
            this.olvColumn3 = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvColumn4 = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvColumn2 = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownRok)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxCover)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.treeListViewAlbaDeezer)).BeginInit();
            this.SuspendLayout();
            // 
            // textBoxInterpret
            // 
            this.textBoxInterpret.Location = new System.Drawing.Point(98, 10);
            this.textBoxInterpret.Name = "textBoxInterpret";
            this.textBoxInterpret.Size = new System.Drawing.Size(407, 20);
            this.textBoxInterpret.TabIndex = 0;
            // 
            // textBoxAlbum
            // 
            this.textBoxAlbum.Location = new System.Drawing.Point(98, 36);
            this.textBoxAlbum.Name = "textBoxAlbum";
            this.textBoxAlbum.Size = new System.Drawing.Size(407, 20);
            this.textBoxAlbum.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(46, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Interpret";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 36);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(61, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Název alba";
            // 
            // numericUpDownRok
            // 
            this.numericUpDownRok.Location = new System.Drawing.Point(98, 443);
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
            this.label3.Location = new System.Drawing.Point(12, 442);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(27, 13);
            this.label3.TabIndex = 3;
            this.label3.Text = "Rok";
            // 
            // textBoxZanr
            // 
            this.textBoxZanr.Location = new System.Drawing.Point(98, 470);
            this.textBoxZanr.Name = "textBoxZanr";
            this.textBoxZanr.Size = new System.Drawing.Size(407, 20);
            this.textBoxZanr.TabIndex = 5;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(13, 470);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(29, 13);
            this.label4.TabIndex = 3;
            this.label4.Text = "Žánr";
            // 
            // buttonPridatAlbum
            // 
            this.buttonPridatAlbum.Location = new System.Drawing.Point(98, 717);
            this.buttonPridatAlbum.Name = "buttonPridatAlbum";
            this.buttonPridatAlbum.Size = new System.Drawing.Size(99, 23);
            this.buttonPridatAlbum.TabIndex = 6;
            this.buttonPridatAlbum.Text = "Přidat album";
            this.buttonPridatAlbum.UseVisualStyleBackColor = true;
            this.buttonPridatAlbum.Click += new System.EventHandler(this.ButtonPridatAlbum_Click);
            // 
            // buttonVyhledatDeezer
            // 
            this.buttonVyhledatDeezer.Location = new System.Drawing.Point(98, 62);
            this.buttonVyhledatDeezer.Name = "buttonVyhledatDeezer";
            this.buttonVyhledatDeezer.Size = new System.Drawing.Size(198, 23);
            this.buttonVyhledatDeezer.TabIndex = 7;
            this.buttonVyhledatDeezer.Text = "Vyhledat na Deezeru";
            this.buttonVyhledatDeezer.UseVisualStyleBackColor = true;
            this.buttonVyhledatDeezer.Click += new System.EventHandler(this.ButtonVyhledatDeezer_Click);
            // 
            // pictureBoxCover
            // 
            this.pictureBoxCover.Location = new System.Drawing.Point(98, 496);
            this.pictureBoxCover.Name = "pictureBoxCover";
            this.pictureBoxCover.Size = new System.Drawing.Size(200, 200);
            this.pictureBoxCover.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBoxCover.TabIndex = 8;
            this.pictureBoxCover.TabStop = false;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(13, 496);
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
            this.treeListViewAlbaDeezer.Location = new System.Drawing.Point(98, 91);
            this.treeListViewAlbaDeezer.Name = "treeListViewAlbaDeezer";
            this.treeListViewAlbaDeezer.ShowGroups = false;
            this.treeListViewAlbaDeezer.Size = new System.Drawing.Size(657, 346);
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
            // olvColumn2
            // 
            this.olvColumn2.AspectName = "Cislo";
            this.olvColumn2.Text = "Číslo";
            this.olvColumn2.Width = 66;
            // 
            // FormAlbum
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 751);
            this.Controls.Add(this.treeListViewAlbaDeezer);
            this.Controls.Add(this.pictureBoxCover);
            this.Controls.Add(this.buttonVyhledatDeezer);
            this.Controls.Add(this.buttonPridatAlbum);
            this.Controls.Add(this.textBoxZanr);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.numericUpDownRok);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBoxAlbum);
            this.Controls.Add(this.textBoxInterpret);
            this.Name = "FormAlbum";
            this.Text = "FormAlbum";
            this.Load += new System.EventHandler(this.FormAlbum_Load);
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownRok)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxCover)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.treeListViewAlbaDeezer)).EndInit();
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
    }
}