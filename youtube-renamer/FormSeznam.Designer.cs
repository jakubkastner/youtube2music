namespace hudba
{
    partial class FormSeznam
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
            System.Windows.Forms.ListViewGroup listViewGroup1 = new System.Windows.Forms.ListViewGroup("Složka nalezena", System.Windows.Forms.HorizontalAlignment.Left);
            System.Windows.Forms.ListViewGroup listViewGroup2 = new System.Windows.Forms.ListViewGroup("Následující videa se nepodařilo přejmenovat", System.Windows.Forms.HorizontalAlignment.Left);
            System.Windows.Forms.ListViewGroup listViewGroup3 = new System.Windows.Forms.ListViewGroup("Následující videa byly smazány z Youtube a nebo neexistují", System.Windows.Forms.HorizontalAlignment.Left);
            System.Windows.Forms.ListViewGroup listViewGroup4 = new System.Windows.Forms.ListViewGroup("Složka nemohla být nalezena", System.Windows.Forms.HorizontalAlignment.Left);
            System.Windows.Forms.ListViewGroup listViewGroup5 = new System.Windows.Forms.ListViewGroup("Video bylo staženo dříve", System.Windows.Forms.HorizontalAlignment.Left);
            System.Windows.Forms.ListViewGroup listViewGroup6 = new System.Windows.Forms.ListViewGroup("Staženo", System.Windows.Forms.HorizontalAlignment.Left);
            System.Windows.Forms.ListViewGroup listViewGroup7 = new System.Windows.Forms.ListViewGroup("Následující videa jsou již možná stažena", System.Windows.Forms.HorizontalAlignment.Left);
            this.backgroundWorkerStahniVidea = new System.ComponentModel.BackgroundWorker();
            this.backgroundWorkerNactiProgram = new System.ComponentModel.BackgroundWorker();
            this.labelInfo = new System.Windows.Forms.Label();
            this.statusStripStatus = new System.Windows.Forms.StatusStrip();
            this.progressBarStatus = new System.Windows.Forms.ToolStripProgressBar();
            this.labelStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.textBoxOdkaz = new System.Windows.Forms.ToolStripTextBox();
            this.menuPridatVideoNeboPlaylist = new System.Windows.Forms.ToolStripMenuItem();
            this.menuVybrat = new System.Windows.Forms.ToolStripMenuItem();
            this.menuVybratVse = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator7 = new System.Windows.Forms.ToolStripSeparator();
            this.filtrSouboruOkMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.filtrSouboruNeMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.menuVybratObratit = new System.Windows.Forms.ToolStripMenuItem();
            this.menuVybratZrusit = new System.Windows.Forms.ToolStripMenuItem();
            this.menuUpravit = new System.Windows.Forms.ToolStripMenuItem();
            this.menuNastaveni = new System.Windows.Forms.ToolStripMenuItem();
            this.menuNastaveniKnihovnaSlozka = new System.Windows.Forms.ToolStripMenuItem();
            this.menuNastaveniKnihovnaVybrana = new System.Windows.Forms.ToolStripMenuItem();
            this.menuNastaveniKnihovnaZmenit = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.menuNastaveniKnihovnaNaposledyVybrane = new System.Windows.Forms.ToolStripMenuItem();
            this.menuNastaveniKnihovnaNaposledyVymazat = new System.Windows.Forms.ToolStripMenuItem();
            this.menuNastaveniKnihovnaProhledat = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.menuNastaveniYoutubeDL = new System.Windows.Forms.ToolStripMenuItem();
            this.menuNastaveniYoutubeDLCestaVybrana = new System.Windows.Forms.ToolStripMenuItem();
            this.menuNastaveniYoutubeDLCestaZmenit = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.menuNastaveniYoutubeDLCestaNaposledyVybrane = new System.Windows.Forms.ToolStripMenuItem();
            this.menuNastaveniYoutubeDLCestaNaposledyVymazat = new System.Windows.Forms.ToolStripMenuItem();
            this.menuNastaveniYoutubeDLStahnout = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
            this.menuNastaveniFFmpeg = new System.Windows.Forms.ToolStripMenuItem();
            this.menuNastaveniFFmpegCestaVybrana = new System.Windows.Forms.ToolStripMenuItem();
            this.menuNastaveniFFmpegCestaZmenit = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.menuNastaveniFFmpegCestaNaposledyVybrane = new System.Windows.Forms.ToolStripMenuItem();
            this.menuNastaveniFFmpegCestaNaposledyVymazat = new System.Windows.Forms.ToolStripMenuItem();
            this.menuNastaveniFFmpegStahnout = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStahnout = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStripMenu = new System.Windows.Forms.MenuStrip();
            this.menuOdstranit = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem4 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem5 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem6 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem7 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem8 = new System.Windows.Forms.ToolStripMenuItem();
            this.testToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.backgroundWorkerProhledejSlozky = new System.ComponentModel.BackgroundWorker();
            this.backgroundWorkerPridejVidea = new System.ComponentModel.BackgroundWorker();
            this.listViewSeznam = new System.Windows.Forms.ListView();
            this.columnHeaderID = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderKanal = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderDatum = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderNazevPuvodni = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderInterpret = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderSkladba = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderFeat = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderNazevNovy = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderSlozka = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderChyba = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderZanr = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.backgroundWorkerStahniProgram = new System.ComponentModel.BackgroundWorker();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.panelSeznamVidei = new System.Windows.Forms.Panel();
            this.objectListViewSeznamVidei = new BrightIdeasSoftware.ObjectListView();
            this.olvColumn1 = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvColumn2 = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvColumn3 = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvColumn4 = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvColumn5 = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvColumn6 = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvColumn7 = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvColumn8 = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvColumn9 = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvColumn10 = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvColumn11 = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.statusStripStatus.SuspendLayout();
            this.menuStripMenu.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.panelSeznamVidei.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.objectListViewSeznamVidei)).BeginInit();
            this.SuspendLayout();
            // 
            // backgroundWorkerStahniVidea
            // 
            this.backgroundWorkerStahniVidea.WorkerReportsProgress = true;
            this.backgroundWorkerStahniVidea.WorkerSupportsCancellation = true;
            this.backgroundWorkerStahniVidea.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorkerStahniVidea_DoWork);
            this.backgroundWorkerStahniVidea.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.backgroundWorkerStahniVideo_ProgressChanged);
            this.backgroundWorkerStahniVidea.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorkerStahniVideo_RunWorkerCompleted);
            // 
            // backgroundWorkerNactiProgram
            // 
            this.backgroundWorkerNactiProgram.WorkerReportsProgress = true;
            this.backgroundWorkerNactiProgram.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorkerNactiProgram_DoWork);
            this.backgroundWorkerNactiProgram.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.backgroundWorkerNactiProgram_ProgressChanged);
            this.backgroundWorkerNactiProgram.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorkerNactiProgram_RunWorkerCompleted);
            // 
            // labelInfo
            // 
            this.labelInfo.Font = new System.Drawing.Font("Microsoft Sans Serif", 26.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.labelInfo.Location = new System.Drawing.Point(447, 205);
            this.labelInfo.Name = "labelInfo";
            this.labelInfo.Padding = new System.Windows.Forms.Padding(0, 50, 0, 0);
            this.labelInfo.Size = new System.Drawing.Size(434, 98);
            this.labelInfo.TabIndex = 7;
            this.labelInfo.Tag = "";
            this.labelInfo.Text = "--------------";
            this.labelInfo.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // statusStripStatus
            // 
            this.statusStripStatus.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.progressBarStatus,
            this.labelStatus});
            this.statusStripStatus.Location = new System.Drawing.Point(0, 665);
            this.statusStripStatus.Name = "statusStripStatus";
            this.statusStripStatus.ShowItemToolTips = true;
            this.statusStripStatus.Size = new System.Drawing.Size(1396, 25);
            this.statusStripStatus.TabIndex = 13;
            // 
            // progressBarStatus
            // 
            this.progressBarStatus.AutoSize = false;
            this.progressBarStatus.MarqueeAnimationSpeed = 500;
            this.progressBarStatus.Maximum = 10;
            this.progressBarStatus.Name = "progressBarStatus";
            this.progressBarStatus.Size = new System.Drawing.Size(200, 19);
            this.progressBarStatus.Step = 1;
            this.progressBarStatus.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.progressBarStatus.Visible = false;
            // 
            // labelStatus
            // 
            this.labelStatus.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.labelStatus.Name = "labelStatus";
            this.labelStatus.Size = new System.Drawing.Size(263, 20);
            this.labelStatus.Text = "Připraven                                                ";
            this.labelStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // listBox1
            // 
            this.listBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listBox1.FormattingEnabled = true;
            this.listBox1.Location = new System.Drawing.Point(3, 3);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(1362, 199);
            this.listBox1.TabIndex = 19;
            this.listBox1.Visible = false;
            // 
            // textBoxOdkaz
            // 
            this.textBoxOdkaz.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBoxOdkaz.MaxLength = 1000;
            this.textBoxOdkaz.Name = "textBoxOdkaz";
            this.textBoxOdkaz.Size = new System.Drawing.Size(400, 23);
            this.textBoxOdkaz.Text = "VLOŽTE ODKAZ NA VIDEO NEBO PLAYLIST Z YOUTUBE";
            this.textBoxOdkaz.Leave += new System.EventHandler(this.textBoxOdkaz_Leave);
            this.textBoxOdkaz.Click += new System.EventHandler(this.textBoxOdkaz_Click);
            this.textBoxOdkaz.TextChanged += new System.EventHandler(this.textBoxOdkaz_TextChanged);
            // 
            // menuPridatVideoNeboPlaylist
            // 
            this.menuPridatVideoNeboPlaylist.AutoSize = false;
            this.menuPridatVideoNeboPlaylist.Enabled = false;
            this.menuPridatVideoNeboPlaylist.Name = "menuPridatVideoNeboPlaylist";
            this.menuPridatVideoNeboPlaylist.Size = new System.Drawing.Size(200, 23);
            this.menuPridatVideoNeboPlaylist.Text = "PŘIDAT VIDEO NEBO PLAYLIST";
            this.menuPridatVideoNeboPlaylist.Click += new System.EventHandler(this.menuPridatVideoNeboPlaylist_Click);
            // 
            // menuVybrat
            // 
            this.menuVybrat.AutoSize = false;
            this.menuVybrat.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuVybratVse,
            this.toolStripSeparator7,
            this.filtrSouboruOkMenu,
            this.filtrSouboruNeMenu,
            this.toolStripSeparator5,
            this.menuVybratObratit,
            this.menuVybratZrusit});
            this.menuVybrat.Enabled = false;
            this.menuVybrat.Name = "menuVybrat";
            this.menuVybrat.Size = new System.Drawing.Size(122, 23);
            this.menuVybrat.Text = "VYBRAT";
            // 
            // menuVybratVse
            // 
            this.menuVybratVse.Name = "menuVybratVse";
            this.menuVybratVse.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.A)));
            this.menuVybratVse.Size = new System.Drawing.Size(371, 22);
            this.menuVybratVse.Text = "Vybrat vše";
            this.menuVybratVse.Click += new System.EventHandler(this.menuVybratVse_Click);
            // 
            // toolStripSeparator7
            // 
            this.toolStripSeparator7.Name = "toolStripSeparator7";
            this.toolStripSeparator7.Size = new System.Drawing.Size(368, 6);
            // 
            // filtrSouboruOkMenu
            // 
            this.filtrSouboruOkMenu.Name = "filtrSouboruOkMenu";
            this.filtrSouboruOkMenu.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.K)));
            this.filtrSouboruOkMenu.Size = new System.Drawing.Size(371, 22);
            this.filtrSouboruOkMenu.Text = "Pouze soubory, které se podařilo přejmenovat";
            this.filtrSouboruOkMenu.Visible = false;
            this.filtrSouboruOkMenu.Click += new System.EventHandler(this.filtrSouboruOkMenu_Click);
            // 
            // filtrSouboruNeMenu
            // 
            this.filtrSouboruNeMenu.Name = "filtrSouboruNeMenu";
            this.filtrSouboruNeMenu.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.N)));
            this.filtrSouboruNeMenu.Size = new System.Drawing.Size(371, 22);
            this.filtrSouboruNeMenu.Text = "Pouze soubory, které se nepodařilo přejmenovat";
            this.filtrSouboruNeMenu.Visible = false;
            this.filtrSouboruNeMenu.Click += new System.EventHandler(this.filtrSouboruNeMenu_Click);
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(368, 6);
            this.toolStripSeparator5.Visible = false;
            // 
            // menuVybratObratit
            // 
            this.menuVybratObratit.Name = "menuVybratObratit";
            this.menuVybratObratit.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.R)));
            this.menuVybratObratit.Size = new System.Drawing.Size(371, 22);
            this.menuVybratObratit.Text = "Obrátit výběr";
            this.menuVybratObratit.Click += new System.EventHandler(this.menuVybratObratit_Click);
            // 
            // menuVybratZrusit
            // 
            this.menuVybratZrusit.Name = "menuVybratZrusit";
            this.menuVybratZrusit.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.C)));
            this.menuVybratZrusit.Size = new System.Drawing.Size(371, 22);
            this.menuVybratZrusit.Text = "Zrušit výběř";
            this.menuVybratZrusit.Click += new System.EventHandler(this.menuVybratZrusit_Click);
            // 
            // menuUpravit
            // 
            this.menuUpravit.AutoSize = false;
            this.menuUpravit.Enabled = false;
            this.menuUpravit.Name = "menuUpravit";
            this.menuUpravit.Size = new System.Drawing.Size(100, 23);
            this.menuUpravit.Text = "UPRAVIT";
            this.menuUpravit.Click += new System.EventHandler(this.menuUpravit_Click);
            // 
            // menuNastaveni
            // 
            this.menuNastaveni.AutoSize = false;
            this.menuNastaveni.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuNastaveniKnihovnaSlozka,
            this.menuNastaveniKnihovnaProhledat,
            this.toolStripSeparator2,
            this.menuNastaveniYoutubeDL,
            this.menuNastaveniYoutubeDLStahnout,
            this.toolStripSeparator6,
            this.menuNastaveniFFmpeg,
            this.menuNastaveniFFmpegStahnout});
            this.menuNastaveni.Name = "menuNastaveni";
            this.menuNastaveni.Size = new System.Drawing.Size(100, 23);
            this.menuNastaveni.Text = "NASTAVENÍ";
            // 
            // menuNastaveniKnihovnaSlozka
            // 
            this.menuNastaveniKnihovnaSlozka.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuNastaveniKnihovnaVybrana,
            this.menuNastaveniKnihovnaZmenit,
            this.toolStripSeparator1,
            this.menuNastaveniKnihovnaNaposledyVybrane,
            this.menuNastaveniKnihovnaNaposledyVymazat});
            this.menuNastaveniKnihovnaSlozka.Name = "menuNastaveniKnihovnaSlozka";
            this.menuNastaveniKnihovnaSlozka.Size = new System.Drawing.Size(255, 22);
            this.menuNastaveniKnihovnaSlozka.Text = "Složka s hudební knihovnou";
            // 
            // menuNastaveniKnihovnaVybrana
            // 
            this.menuNastaveniKnihovnaVybrana.Enabled = false;
            this.menuNastaveniKnihovnaVybrana.Name = "menuNastaveniKnihovnaVybrana";
            this.menuNastaveniKnihovnaVybrana.Size = new System.Drawing.Size(331, 22);
            this.menuNastaveniKnihovnaVybrana.Text = "Nebyla vybrána žádná složka";
            this.menuNastaveniKnihovnaVybrana.Click += new System.EventHandler(this.menuNastaveniKnihovnaVybrana_Click);
            // 
            // menuNastaveniKnihovnaZmenit
            // 
            this.menuNastaveniKnihovnaZmenit.Name = "menuNastaveniKnihovnaZmenit";
            this.menuNastaveniKnihovnaZmenit.Size = new System.Drawing.Size(331, 22);
            this.menuNastaveniKnihovnaZmenit.Text = "Změnit složku";
            this.menuNastaveniKnihovnaZmenit.Click += new System.EventHandler(this.menuNastaveniKnihovnaZmenit_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(328, 6);
            // 
            // menuNastaveniKnihovnaNaposledyVybrane
            // 
            this.menuNastaveniKnihovnaNaposledyVybrane.Enabled = false;
            this.menuNastaveniKnihovnaNaposledyVybrane.Name = "menuNastaveniKnihovnaNaposledyVybrane";
            this.menuNastaveniKnihovnaNaposledyVybrane.Size = new System.Drawing.Size(331, 22);
            this.menuNastaveniKnihovnaNaposledyVybrane.Text = "Nebyla nalezena žádná naposledy vybraná šložka";
            // 
            // menuNastaveniKnihovnaNaposledyVymazat
            // 
            this.menuNastaveniKnihovnaNaposledyVymazat.Name = "menuNastaveniKnihovnaNaposledyVymazat";
            this.menuNastaveniKnihovnaNaposledyVymazat.Size = new System.Drawing.Size(331, 22);
            this.menuNastaveniKnihovnaNaposledyVymazat.Text = "Vymazat historii";
            this.menuNastaveniKnihovnaNaposledyVymazat.Click += new System.EventHandler(this.menuNastaveniKnihovnaNaposledyVymazat_Click);
            // 
            // menuNastaveniKnihovnaProhledat
            // 
            this.menuNastaveniKnihovnaProhledat.Enabled = false;
            this.menuNastaveniKnihovnaProhledat.Name = "menuNastaveniKnihovnaProhledat";
            this.menuNastaveniKnihovnaProhledat.Size = new System.Drawing.Size(255, 22);
            this.menuNastaveniKnihovnaProhledat.Text = "Nebyla vybrána hudební knihovna";
            this.menuNastaveniKnihovnaProhledat.Click += new System.EventHandler(this.menuNastaveniKnihovnaProhledat_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(252, 6);
            // 
            // menuNastaveniYoutubeDL
            // 
            this.menuNastaveniYoutubeDL.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuNastaveniYoutubeDLCestaVybrana,
            this.menuNastaveniYoutubeDLCestaZmenit,
            this.toolStripSeparator3,
            this.menuNastaveniYoutubeDLCestaNaposledyVybrane,
            this.menuNastaveniYoutubeDLCestaNaposledyVymazat});
            this.menuNastaveniYoutubeDL.Name = "menuNastaveniYoutubeDL";
            this.menuNastaveniYoutubeDL.Size = new System.Drawing.Size(255, 22);
            this.menuNastaveniYoutubeDL.Text = "Cesta YoutubeDL";
            // 
            // menuNastaveniYoutubeDLCestaVybrana
            // 
            this.menuNastaveniYoutubeDLCestaVybrana.Enabled = false;
            this.menuNastaveniYoutubeDLCestaVybrana.Name = "menuNastaveniYoutubeDLCestaVybrana";
            this.menuNastaveniYoutubeDLCestaVybrana.Size = new System.Drawing.Size(326, 22);
            this.menuNastaveniYoutubeDLCestaVybrana.Text = "Není vybraná žádná cesta";
            this.menuNastaveniYoutubeDLCestaVybrana.Click += new System.EventHandler(this.menuNastaveniCestaYoutubeDLVybrana_Click);
            // 
            // menuNastaveniYoutubeDLCestaZmenit
            // 
            this.menuNastaveniYoutubeDLCestaZmenit.Name = "menuNastaveniYoutubeDLCestaZmenit";
            this.menuNastaveniYoutubeDLCestaZmenit.Size = new System.Drawing.Size(326, 22);
            this.menuNastaveniYoutubeDLCestaZmenit.Text = "Změnit cestu programu";
            this.menuNastaveniYoutubeDLCestaZmenit.Click += new System.EventHandler(this.menuNastaveniCestaYoutubeDLZmenit_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(323, 6);
            // 
            // menuNastaveniYoutubeDLCestaNaposledyVybrane
            // 
            this.menuNastaveniYoutubeDLCestaNaposledyVybrane.Enabled = false;
            this.menuNastaveniYoutubeDLCestaNaposledyVybrane.Name = "menuNastaveniYoutubeDLCestaNaposledyVybrane";
            this.menuNastaveniYoutubeDLCestaNaposledyVybrane.Size = new System.Drawing.Size(326, 22);
            this.menuNastaveniYoutubeDLCestaNaposledyVybrane.Text = "Nebyly nalezeny žádné naposledy vybrané cesty";
            // 
            // menuNastaveniYoutubeDLCestaNaposledyVymazat
            // 
            this.menuNastaveniYoutubeDLCestaNaposledyVymazat.Name = "menuNastaveniYoutubeDLCestaNaposledyVymazat";
            this.menuNastaveniYoutubeDLCestaNaposledyVymazat.Size = new System.Drawing.Size(326, 22);
            this.menuNastaveniYoutubeDLCestaNaposledyVymazat.Text = "Vymazat historii";
            this.menuNastaveniYoutubeDLCestaNaposledyVymazat.Click += new System.EventHandler(this.menuNastaveniYoutubeDLCestaNaposledyVymazat_Click);
            // 
            // menuNastaveniYoutubeDLStahnout
            // 
            this.menuNastaveniYoutubeDLStahnout.Name = "menuNastaveniYoutubeDLStahnout";
            this.menuNastaveniYoutubeDLStahnout.Size = new System.Drawing.Size(255, 22);
            this.menuNastaveniYoutubeDLStahnout.Text = "Stáhnout YoutubeDL";
            this.menuNastaveniYoutubeDLStahnout.Click += new System.EventHandler(this.menuNastaveniYoutubeDLStahnout_Click);
            // 
            // toolStripSeparator6
            // 
            this.toolStripSeparator6.Name = "toolStripSeparator6";
            this.toolStripSeparator6.Size = new System.Drawing.Size(252, 6);
            // 
            // menuNastaveniFFmpeg
            // 
            this.menuNastaveniFFmpeg.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuNastaveniFFmpegCestaVybrana,
            this.menuNastaveniFFmpegCestaZmenit,
            this.toolStripSeparator4,
            this.menuNastaveniFFmpegCestaNaposledyVybrane,
            this.menuNastaveniFFmpegCestaNaposledyVymazat});
            this.menuNastaveniFFmpeg.Name = "menuNastaveniFFmpeg";
            this.menuNastaveniFFmpeg.Size = new System.Drawing.Size(255, 22);
            this.menuNastaveniFFmpeg.Text = "Cesta FFmpeg";
            // 
            // menuNastaveniFFmpegCestaVybrana
            // 
            this.menuNastaveniFFmpegCestaVybrana.Enabled = false;
            this.menuNastaveniFFmpegCestaVybrana.Name = "menuNastaveniFFmpegCestaVybrana";
            this.menuNastaveniFFmpegCestaVybrana.Size = new System.Drawing.Size(326, 22);
            this.menuNastaveniFFmpegCestaVybrana.Text = "Není vybrána žádná cesta";
            this.menuNastaveniFFmpegCestaVybrana.Click += new System.EventHandler(this.menuNastaveniCestaFFmpegVybrana_Click);
            // 
            // menuNastaveniFFmpegCestaZmenit
            // 
            this.menuNastaveniFFmpegCestaZmenit.Name = "menuNastaveniFFmpegCestaZmenit";
            this.menuNastaveniFFmpegCestaZmenit.Size = new System.Drawing.Size(326, 22);
            this.menuNastaveniFFmpegCestaZmenit.Text = "Změnit cestu programu";
            this.menuNastaveniFFmpegCestaZmenit.Click += new System.EventHandler(this.menuNastaveniCestaFFmpegZmenit_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(323, 6);
            // 
            // menuNastaveniFFmpegCestaNaposledyVybrane
            // 
            this.menuNastaveniFFmpegCestaNaposledyVybrane.Enabled = false;
            this.menuNastaveniFFmpegCestaNaposledyVybrane.Name = "menuNastaveniFFmpegCestaNaposledyVybrane";
            this.menuNastaveniFFmpegCestaNaposledyVybrane.Size = new System.Drawing.Size(326, 22);
            this.menuNastaveniFFmpegCestaNaposledyVybrane.Text = "Nebyly nalezeny žádné naposledy vybrané cesty";
            // 
            // menuNastaveniFFmpegCestaNaposledyVymazat
            // 
            this.menuNastaveniFFmpegCestaNaposledyVymazat.Name = "menuNastaveniFFmpegCestaNaposledyVymazat";
            this.menuNastaveniFFmpegCestaNaposledyVymazat.Size = new System.Drawing.Size(326, 22);
            this.menuNastaveniFFmpegCestaNaposledyVymazat.Text = "Vymazat historii";
            this.menuNastaveniFFmpegCestaNaposledyVymazat.Click += new System.EventHandler(this.menuNastaveniFFmpegCestaNaposledyVymazat_Click);
            // 
            // menuNastaveniFFmpegStahnout
            // 
            this.menuNastaveniFFmpegStahnout.Name = "menuNastaveniFFmpegStahnout";
            this.menuNastaveniFFmpegStahnout.Size = new System.Drawing.Size(255, 22);
            this.menuNastaveniFFmpegStahnout.Text = "Stáhnout FFmpeg";
            this.menuNastaveniFFmpegStahnout.Click += new System.EventHandler(this.menuNastaveniFFmpegStahnout_Click);
            // 
            // menuStahnout
            // 
            this.menuStahnout.AutoSize = false;
            this.menuStahnout.Enabled = false;
            this.menuStahnout.Name = "menuStahnout";
            this.menuStahnout.Size = new System.Drawing.Size(200, 23);
            this.menuStahnout.Text = "STÁHNOUT A PŘESUNOUT";
            this.menuStahnout.Click += new System.EventHandler(this.menuStahnout_Click);
            // 
            // menuStripMenu
            // 
            this.menuStripMenu.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.menuStripMenu.Dock = System.Windows.Forms.DockStyle.None;
            this.menuStripMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.textBoxOdkaz,
            this.menuPridatVideoNeboPlaylist,
            this.menuVybrat,
            this.menuUpravit,
            this.menuStahnout,
            this.menuOdstranit,
            this.menuNastaveni,
            this.toolStripMenuItem1});
            this.menuStripMenu.Location = new System.Drawing.Point(0, 0);
            this.menuStripMenu.Name = "menuStripMenu";
            this.menuStripMenu.Size = new System.Drawing.Size(1270, 27);
            this.menuStripMenu.TabIndex = 2;
            this.menuStripMenu.Text = "Menu";
            // 
            // menuOdstranit
            // 
            this.menuOdstranit.AutoSize = false;
            this.menuOdstranit.Enabled = false;
            this.menuOdstranit.Name = "menuOdstranit";
            this.menuOdstranit.Size = new System.Drawing.Size(100, 23);
            this.menuOdstranit.Text = "ODSTRANIT";
            this.menuOdstranit.Click += new System.EventHandler(this.menuOdstranit_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem3,
            this.toolStripMenuItem4,
            this.toolStripMenuItem5,
            this.toolStripMenuItem6,
            this.toolStripMenuItem7,
            this.toolStripMenuItem8,
            this.testToolStripMenuItem});
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(38, 23);
            this.toolStripMenuItem1.Text = "test";
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(93, 22);
            this.toolStripMenuItem3.Text = "1";
            this.toolStripMenuItem3.Click += new System.EventHandler(this.toolStripMenuItem3_Click);
            // 
            // toolStripMenuItem4
            // 
            this.toolStripMenuItem4.Name = "toolStripMenuItem4";
            this.toolStripMenuItem4.Size = new System.Drawing.Size(93, 22);
            this.toolStripMenuItem4.Text = "2";
            this.toolStripMenuItem4.Click += new System.EventHandler(this.toolStripMenuItem4_Click);
            // 
            // toolStripMenuItem5
            // 
            this.toolStripMenuItem5.Name = "toolStripMenuItem5";
            this.toolStripMenuItem5.Size = new System.Drawing.Size(93, 22);
            this.toolStripMenuItem5.Text = "3";
            this.toolStripMenuItem5.Click += new System.EventHandler(this.toolStripMenuItem5_Click);
            // 
            // toolStripMenuItem6
            // 
            this.toolStripMenuItem6.Name = "toolStripMenuItem6";
            this.toolStripMenuItem6.Size = new System.Drawing.Size(93, 22);
            this.toolStripMenuItem6.Text = "4";
            this.toolStripMenuItem6.Click += new System.EventHandler(this.toolStripMenuItem6_Click);
            // 
            // toolStripMenuItem7
            // 
            this.toolStripMenuItem7.Name = "toolStripMenuItem7";
            this.toolStripMenuItem7.Size = new System.Drawing.Size(93, 22);
            this.toolStripMenuItem7.Text = "5";
            this.toolStripMenuItem7.Click += new System.EventHandler(this.toolStripMenuItem7_Click);
            // 
            // toolStripMenuItem8
            // 
            this.toolStripMenuItem8.Name = "toolStripMenuItem8";
            this.toolStripMenuItem8.Size = new System.Drawing.Size(93, 22);
            this.toolStripMenuItem8.Text = "6";
            this.toolStripMenuItem8.Click += new System.EventHandler(this.toolStripMenuItem8_Click);
            // 
            // testToolStripMenuItem
            // 
            this.testToolStripMenuItem.Name = "testToolStripMenuItem";
            this.testToolStripMenuItem.Size = new System.Drawing.Size(93, 22);
            this.testToolStripMenuItem.Text = "test";
            this.testToolStripMenuItem.Click += new System.EventHandler(this.testToolStripMenuItem_Click);
            // 
            // backgroundWorkerProhledejSlozky
            // 
            this.backgroundWorkerProhledejSlozky.WorkerReportsProgress = true;
            this.backgroundWorkerProhledejSlozky.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorkerProhledejSlozky_DoWork);
            this.backgroundWorkerProhledejSlozky.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.backgroundWorkerProhledejSlozky_ProgressChanged);
            this.backgroundWorkerProhledejSlozky.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorkerProhledejSlozky_RunWorkerCompleted);
            // 
            // backgroundWorkerPridejVidea
            // 
            this.backgroundWorkerPridejVidea.WorkerReportsProgress = true;
            this.backgroundWorkerPridejVidea.WorkerSupportsCancellation = true;
            this.backgroundWorkerPridejVidea.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorkerPridejVidea_DoWork);
            this.backgroundWorkerPridejVidea.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.backgroundWorkerPridejVidea_ProgressChanged);
            this.backgroundWorkerPridejVidea.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorkerPridejVidea_RunWorkerCompleted);
            // 
            // listViewSeznam
            // 
            this.listViewSeznam.BackColor = System.Drawing.SystemColors.Control;
            this.listViewSeznam.CheckBoxes = true;
            this.listViewSeznam.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeaderID,
            this.columnHeaderKanal,
            this.columnHeaderDatum,
            this.columnHeaderNazevPuvodni,
            this.columnHeaderInterpret,
            this.columnHeaderSkladba,
            this.columnHeaderFeat,
            this.columnHeaderNazevNovy,
            this.columnHeaderSlozka,
            this.columnHeaderChyba,
            this.columnHeaderZanr});
            this.listViewSeznam.FullRowSelect = true;
            listViewGroup1.Header = "Složka nalezena";
            listViewGroup1.Name = "listViewGroupOK";
            listViewGroup2.Header = "Následující videa se nepodařilo přejmenovat";
            listViewGroup2.Name = "listViewGroupChybaPrejmenovani";
            listViewGroup3.Header = "Následující videa byly smazány z Youtube a nebo neexistují";
            listViewGroup3.Name = "listViewGroupChybaVideoNeexistuje";
            listViewGroup4.Header = "Složka nemohla být nalezena";
            listViewGroup4.Name = "listViewGroupChybaSlozka";
            listViewGroup5.Header = "Video bylo staženo dříve";
            listViewGroup5.Name = "listViewGroupExistuje";
            listViewGroup6.Header = "Staženo";
            listViewGroup6.Name = "listViewGroupStazeno";
            listViewGroup6.Tag = "stazeno";
            listViewGroup7.Header = "Následující videa jsou již možná stažena";
            listViewGroup7.Name = "listViewGroupKontrola";
            this.listViewSeznam.Groups.AddRange(new System.Windows.Forms.ListViewGroup[] {
            listViewGroup1,
            listViewGroup2,
            listViewGroup3,
            listViewGroup4,
            listViewGroup5,
            listViewGroup6,
            listViewGroup7});
            this.listViewSeznam.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.listViewSeznam.Location = new System.Drawing.Point(3, 208);
            this.listViewSeznam.Name = "listViewSeznam";
            this.listViewSeznam.Size = new System.Drawing.Size(438, 341);
            this.listViewSeznam.Sorting = System.Windows.Forms.SortOrder.Ascending;
            this.listViewSeznam.TabIndex = 24;
            this.listViewSeznam.UseCompatibleStateImageBehavior = false;
            this.listViewSeznam.View = System.Windows.Forms.View.Details;
            this.listViewSeznam.Visible = false;
            // 
            // columnHeaderID
            // 
            this.columnHeaderID.Text = "ID videa";
            this.columnHeaderID.Width = 94;
            // 
            // columnHeaderKanal
            // 
            this.columnHeaderKanal.Text = "Kanál";
            this.columnHeaderKanal.Width = 130;
            // 
            // columnHeaderDatum
            // 
            this.columnHeaderDatum.Text = "Publikováno";
            this.columnHeaderDatum.Width = 83;
            // 
            // columnHeaderNazevPuvodni
            // 
            this.columnHeaderNazevPuvodni.Tag = "";
            this.columnHeaderNazevPuvodni.Text = "Původní název";
            this.columnHeaderNazevPuvodni.Width = 232;
            // 
            // columnHeaderInterpret
            // 
            this.columnHeaderInterpret.Text = "Interpret";
            this.columnHeaderInterpret.Width = 127;
            // 
            // columnHeaderSkladba
            // 
            this.columnHeaderSkladba.Text = "Skladba";
            this.columnHeaderSkladba.Width = 160;
            // 
            // columnHeaderFeat
            // 
            this.columnHeaderFeat.Text = "Featuring";
            this.columnHeaderFeat.Width = 147;
            // 
            // columnHeaderNazevNovy
            // 
            this.columnHeaderNazevNovy.Text = "Nový název";
            this.columnHeaderNazevNovy.Width = 209;
            // 
            // columnHeaderSlozka
            // 
            this.columnHeaderSlozka.Text = "Složka";
            this.columnHeaderSlozka.Width = 123;
            // 
            // columnHeaderChyba
            // 
            this.columnHeaderChyba.Text = "Chyba";
            this.columnHeaderChyba.Width = 115;
            // 
            // columnHeaderZanr
            // 
            this.columnHeaderZanr.Text = "Žánr";
            this.columnHeaderZanr.Width = 100;
            // 
            // backgroundWorkerStahniProgram
            // 
            this.backgroundWorkerStahniProgram.WorkerReportsProgress = true;
            this.backgroundWorkerStahniProgram.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorkerStahniProgram_DoWork);
            this.backgroundWorkerStahniProgram.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.backgroundWorkerStahniProgram_ProgressChanged);
            this.backgroundWorkerStahniProgram.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorkerStahniProgram_RunWorkerCompleted);
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.flowLayoutPanel1.AutoScroll = true;
            this.flowLayoutPanel1.Controls.Add(this.listBox1);
            this.flowLayoutPanel1.Controls.Add(this.listViewSeznam);
            this.flowLayoutPanel1.Controls.Add(this.labelInfo);
            this.flowLayoutPanel1.Location = new System.Drawing.Point(621, 346);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(775, 283);
            this.flowLayoutPanel1.TabIndex = 25;
            // 
            // panelSeznamVidei
            // 
            this.panelSeznamVidei.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelSeznamVidei.Controls.Add(this.flowLayoutPanel1);
            this.panelSeznamVidei.Controls.Add(this.objectListViewSeznamVidei);
            this.panelSeznamVidei.Location = new System.Drawing.Point(0, 30);
            this.panelSeznamVidei.Name = "panelSeznamVidei";
            this.panelSeznamVidei.Size = new System.Drawing.Size(1396, 632);
            this.panelSeznamVidei.TabIndex = 26;
            // 
            // objectListViewSeznamVidei
            // 
            this.objectListViewSeznamVidei.AllColumns.Add(this.olvColumn1);
            this.objectListViewSeznamVidei.AllColumns.Add(this.olvColumn2);
            this.objectListViewSeznamVidei.AllColumns.Add(this.olvColumn3);
            this.objectListViewSeznamVidei.AllColumns.Add(this.olvColumn4);
            this.objectListViewSeznamVidei.AllColumns.Add(this.olvColumn5);
            this.objectListViewSeznamVidei.AllColumns.Add(this.olvColumn6);
            this.objectListViewSeznamVidei.AllColumns.Add(this.olvColumn7);
            this.objectListViewSeznamVidei.AllColumns.Add(this.olvColumn8);
            this.objectListViewSeznamVidei.AllColumns.Add(this.olvColumn9);
            this.objectListViewSeznamVidei.AllColumns.Add(this.olvColumn10);
            this.objectListViewSeznamVidei.AllColumns.Add(this.olvColumn11);
            this.objectListViewSeznamVidei.CellEditUseWholeCell = false;
            this.objectListViewSeznamVidei.CheckBoxes = true;
            this.objectListViewSeznamVidei.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.olvColumn1,
            this.olvColumn2,
            this.olvColumn3,
            this.olvColumn4,
            this.olvColumn5,
            this.olvColumn6,
            this.olvColumn7,
            this.olvColumn8,
            this.olvColumn9,
            this.olvColumn10,
            this.olvColumn11});
            this.objectListViewSeznamVidei.Cursor = System.Windows.Forms.Cursors.Default;
            this.objectListViewSeznamVidei.Dock = System.Windows.Forms.DockStyle.Fill;
            this.objectListViewSeznamVidei.FullRowSelect = true;
            this.objectListViewSeznamVidei.GridLines = true;
            this.objectListViewSeznamVidei.Location = new System.Drawing.Point(0, 0);
            this.objectListViewSeznamVidei.Name = "objectListViewSeznamVidei";
            this.objectListViewSeznamVidei.Size = new System.Drawing.Size(1396, 632);
            this.objectListViewSeznamVidei.Sorting = System.Windows.Forms.SortOrder.Ascending;
            this.objectListViewSeznamVidei.TabIndex = 0;
            this.objectListViewSeznamVidei.UseCompatibleStateImageBehavior = false;
            this.objectListViewSeznamVidei.View = System.Windows.Forms.View.Details;
            this.objectListViewSeznamVidei.ItemsChanged += new System.EventHandler<BrightIdeasSoftware.ItemsChangedEventArgs>(this.objectListViewSeznamVidei_ItemsChanged);
            this.objectListViewSeznamVidei.ItemChecked += new System.Windows.Forms.ItemCheckedEventHandler(this.objectListViewSeznamVidei_ItemChecked);
            // 
            // olvColumn1
            // 
            this.olvColumn1.AspectName = "id";
            this.olvColumn1.Text = "ID videa";
            this.olvColumn1.Width = 97;
            // 
            // olvColumn2
            // 
            this.olvColumn2.AspectName = "kanal";
            this.olvColumn2.Text = "Kanál";
            this.olvColumn2.Width = 96;
            // 
            // olvColumn3
            // 
            this.olvColumn3.AspectName = "nazevPuvodni";
            this.olvColumn3.Text = "Původní název";
            this.olvColumn3.Width = 159;
            // 
            // olvColumn4
            // 
            this.olvColumn4.AspectName = "interpret";
            this.olvColumn4.Text = "Interpret";
            this.olvColumn4.Width = 135;
            // 
            // olvColumn5
            // 
            this.olvColumn5.AspectName = "skladba";
            this.olvColumn5.Text = "Skladba";
            this.olvColumn5.Width = 125;
            // 
            // olvColumn6
            // 
            this.olvColumn6.AspectName = "featuring";
            this.olvColumn6.Text = "Featuring";
            this.olvColumn6.Width = 102;
            // 
            // olvColumn7
            // 
            this.olvColumn7.AspectName = "novyNazev";
            this.olvColumn7.Text = "Nový název";
            this.olvColumn7.Width = 135;
            // 
            // olvColumn8
            // 
            this.olvColumn8.AspectName = "slozka";
            this.olvColumn8.Text = "Složka";
            this.olvColumn8.Width = 143;
            // 
            // olvColumn9
            // 
            this.olvColumn9.AspectName = "zanr";
            this.olvColumn9.Text = "Žánr";
            this.olvColumn9.Width = 78;
            // 
            // olvColumn10
            // 
            this.olvColumn10.AspectName = "chyba";
            this.olvColumn10.Text = "Chyba";
            this.olvColumn10.Width = 184;
            // 
            // olvColumn11
            // 
            this.olvColumn11.AspectName = "stav";
            this.olvColumn11.Text = "Stav";
            // 
            // FormSeznam
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(1396, 690);
            this.Controls.Add(this.statusStripStatus);
            this.Controls.Add(this.menuStripMenu);
            this.Controls.Add(this.panelSeznamVidei);
            this.MainMenuStrip = this.menuStripMenu;
            this.MinimumSize = new System.Drawing.Size(1125, 300);
            this.Name = "FormSeznam";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "YouTube Renamer";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormSeznam_FormClosing);
            this.Load += new System.EventHandler(this.FormSeznam_Load);
            this.statusStripStatus.ResumeLayout(false);
            this.statusStripStatus.PerformLayout();
            this.menuStripMenu.ResumeLayout(false);
            this.menuStripMenu.PerformLayout();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.panelSeznamVidei.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.objectListViewSeznamVidei)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.ComponentModel.BackgroundWorker backgroundWorkerStahniVidea;
        private System.ComponentModel.BackgroundWorker backgroundWorkerNactiProgram;
        private System.Windows.Forms.StatusStrip statusStripStatus;
        private System.Windows.Forms.ToolStripProgressBar progressBarStatus;
        private System.Windows.Forms.ToolStripStatusLabel labelStatus;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.ToolStripTextBox textBoxOdkaz;
        private System.Windows.Forms.ToolStripMenuItem menuPridatVideoNeboPlaylist;
        private System.Windows.Forms.ToolStripMenuItem menuVybrat;
        private System.Windows.Forms.ToolStripMenuItem menuVybratVse;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator7;
        private System.Windows.Forms.ToolStripMenuItem filtrSouboruOkMenu;
        private System.Windows.Forms.ToolStripMenuItem filtrSouboruNeMenu;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripMenuItem menuVybratObratit;
        private System.Windows.Forms.ToolStripMenuItem menuVybratZrusit;
        private System.Windows.Forms.ToolStripMenuItem menuUpravit;
        private System.Windows.Forms.ToolStripMenuItem menuNastaveni;
        private System.Windows.Forms.ToolStripMenuItem menuNastaveniKnihovnaSlozka;
        private System.Windows.Forms.ToolStripMenuItem menuNastaveniKnihovnaVybrana;
        private System.Windows.Forms.ToolStripMenuItem menuNastaveniKnihovnaZmenit;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem menuNastaveniKnihovnaNaposledyVybrane;
        private System.Windows.Forms.ToolStripMenuItem menuNastaveniKnihovnaProhledat;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem menuStahnout;
        private System.Windows.Forms.MenuStrip menuStripMenu;
        private System.ComponentModel.BackgroundWorker backgroundWorkerProhledejSlozky;
        private System.Windows.Forms.ToolStripMenuItem menuOdstranit;
        private System.Windows.Forms.Label labelInfo;
        private System.ComponentModel.BackgroundWorker backgroundWorkerPridejVidea;
        private System.Windows.Forms.ListView listViewSeznam;
        private System.Windows.Forms.ColumnHeader columnHeaderID;
        private System.Windows.Forms.ColumnHeader columnHeaderKanal;
        private System.Windows.Forms.ColumnHeader columnHeaderDatum;
        private System.Windows.Forms.ColumnHeader columnHeaderNazevPuvodni;
        private System.Windows.Forms.ColumnHeader columnHeaderInterpret;
        private System.Windows.Forms.ColumnHeader columnHeaderSkladba;
        private System.Windows.Forms.ColumnHeader columnHeaderFeat;
        private System.Windows.Forms.ColumnHeader columnHeaderNazevNovy;
        private System.Windows.Forms.ColumnHeader columnHeaderSlozka;
        private System.Windows.Forms.ColumnHeader columnHeaderChyba;
        private System.Windows.Forms.ColumnHeader columnHeaderZanr;
        private System.Windows.Forms.ToolStripMenuItem menuNastaveniYoutubeDL;
        private System.Windows.Forms.ToolStripMenuItem menuNastaveniYoutubeDLCestaVybrana;
        private System.Windows.Forms.ToolStripMenuItem menuNastaveniYoutubeDLCestaZmenit;
        private System.Windows.Forms.ToolStripMenuItem menuNastaveniFFmpeg;
        private System.Windows.Forms.ToolStripMenuItem menuNastaveniFFmpegCestaVybrana;
        private System.Windows.Forms.ToolStripMenuItem menuNastaveniFFmpegCestaZmenit;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem menuNastaveniYoutubeDLCestaNaposledyVybrane;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripMenuItem menuNastaveniFFmpegCestaNaposledyVybrane;
        private System.ComponentModel.BackgroundWorker backgroundWorkerStahniProgram;
        private System.Windows.Forms.ToolStripMenuItem menuNastaveniFFmpegStahnout;
        private System.Windows.Forms.ToolStripMenuItem menuNastaveniYoutubeDLStahnout;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator6;
        private System.Windows.Forms.ToolStripMenuItem menuNastaveniKnihovnaNaposledyVymazat;
        private System.Windows.Forms.ToolStripMenuItem menuNastaveniYoutubeDLCestaNaposledyVymazat;
        private System.Windows.Forms.ToolStripMenuItem menuNastaveniFFmpegCestaNaposledyVymazat;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Panel panelSeznamVidei;
        private BrightIdeasSoftware.ObjectListView objectListViewSeznamVidei;
        private BrightIdeasSoftware.OLVColumn olvColumn1;
        private BrightIdeasSoftware.OLVColumn olvColumn2;
        private BrightIdeasSoftware.OLVColumn olvColumn3;
        private BrightIdeasSoftware.OLVColumn olvColumn4;
        private BrightIdeasSoftware.OLVColumn olvColumn5;
        private BrightIdeasSoftware.OLVColumn olvColumn6;
        private BrightIdeasSoftware.OLVColumn olvColumn7;
        private BrightIdeasSoftware.OLVColumn olvColumn8;
        private BrightIdeasSoftware.OLVColumn olvColumn9;
        private BrightIdeasSoftware.OLVColumn olvColumn10;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem3;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem4;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem5;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem6;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem7;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem8;
        private System.Windows.Forms.ToolStripMenuItem testToolStripMenuItem;
        private BrightIdeasSoftware.OLVColumn olvColumn11;
    }
}

