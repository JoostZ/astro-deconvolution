namespace AstroDeconvolution
{
    partial class MainForm
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
            this.menuMain = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.zoomToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.zoomInToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.zoomOutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.panelImage = new System.Windows.Forms.Panel();
            this.pictureMain = new System.Windows.Forms.PictureBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.pnlPhase1 = new System.Windows.Forms.Panel();
            this.picPreviewPsf = new System.Windows.Forms.PictureBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnPrev = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnLoad = new System.Windows.Forms.Button();
            this.btnNext = new System.Windows.Forms.Button();
            this.lblPhase = new System.Windows.Forms.Label();
            this.imageScan1 = new AstroDeconvolution.ImageScan();
            this.menuMain.SuspendLayout();
            this.panelImage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureMain)).BeginInit();
            this.panel1.SuspendLayout();
            this.pnlPhase1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picPreviewPsf)).BeginInit();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuMain
            // 
            this.menuMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.zoomToolStripMenuItem});
            this.menuMain.Location = new System.Drawing.Point(0, 0);
            this.menuMain.Name = "menuMain";
            this.menuMain.Size = new System.Drawing.Size(773, 24);
            this.menuMain.TabIndex = 1;
            this.menuMain.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(103, 22);
            this.openToolStripMenuItem.Text = "Open";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.menuFileOpen_Click);
            // 
            // zoomToolStripMenuItem
            // 
            this.zoomToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.zoomInToolStripMenuItem,
            this.zoomOutToolStripMenuItem});
            this.zoomToolStripMenuItem.Name = "zoomToolStripMenuItem";
            this.zoomToolStripMenuItem.Size = new System.Drawing.Size(51, 20);
            this.zoomToolStripMenuItem.Text = "Zoom";
            // 
            // zoomInToolStripMenuItem
            // 
            this.zoomInToolStripMenuItem.Name = "zoomInToolStripMenuItem";
            this.zoomInToolStripMenuItem.Size = new System.Drawing.Size(139, 22);
            this.zoomInToolStripMenuItem.Text = "Zoom in ...";
            // 
            // zoomOutToolStripMenuItem
            // 
            this.zoomOutToolStripMenuItem.Name = "zoomOutToolStripMenuItem";
            this.zoomOutToolStripMenuItem.Size = new System.Drawing.Size(139, 22);
            this.zoomOutToolStripMenuItem.Text = "Zoom out ...";
            // 
            // openFileDialog
            // 
            this.openFileDialog.FileName = "openFileDialog";
            this.openFileDialog.Filter = "All Image Files|*.jpg";
            this.openFileDialog.Title = "Open Astro Photo";
            // 
            // panelImage
            // 
            this.panelImage.AutoScroll = true;
            this.panelImage.Controls.Add(this.pictureMain);
            this.panelImage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelImage.Location = new System.Drawing.Point(0, 24);
            this.panelImage.Name = "panelImage";
            this.panelImage.Size = new System.Drawing.Size(589, 370);
            this.panelImage.TabIndex = 3;
            // 
            // pictureMain
            // 
            this.pictureMain.Location = new System.Drawing.Point(3, 3);
            this.pictureMain.Name = "pictureMain";
            this.pictureMain.Size = new System.Drawing.Size(100, 50);
            this.pictureMain.TabIndex = 0;
            this.pictureMain.TabStop = false;
            this.pictureMain.MouseLeave += new System.EventHandler(this.pictureMain_MouseLeave);
            this.pictureMain.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pictureMain_MouseUp);
            this.pictureMain.MouseEnter += new System.EventHandler(this.pictureMain_MouseEnter);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.pnlPhase1);
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Controls.Add(this.lblPhase);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel1.Location = new System.Drawing.Point(589, 24);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(184, 370);
            this.panel1.TabIndex = 4;
            // 
            // pnlPhase1
            // 
            this.pnlPhase1.Controls.Add(this.picPreviewPsf);
            this.pnlPhase1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlPhase1.Location = new System.Drawing.Point(0, 13);
            this.pnlPhase1.Name = "pnlPhase1";
            this.pnlPhase1.Size = new System.Drawing.Size(184, 299);
            this.pnlPhase1.TabIndex = 5;
            // 
            // picPreviewPsf
            // 
            this.picPreviewPsf.Location = new System.Drawing.Point(33, 13);
            this.picPreviewPsf.Name = "picPreviewPsf";
            this.picPreviewPsf.Size = new System.Drawing.Size(100, 87);
            this.picPreviewPsf.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picPreviewPsf.TabIndex = 0;
            this.picPreviewPsf.TabStop = false;
            // 
            // panel2
            // 
            this.panel2.AutoSize = true;
            this.panel2.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.panel2.Controls.Add(this.btnPrev);
            this.panel2.Controls.Add(this.btnSave);
            this.panel2.Controls.Add(this.btnLoad);
            this.panel2.Controls.Add(this.btnNext);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 312);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(184, 58);
            this.panel2.TabIndex = 4;
            // 
            // btnPrev
            // 
            this.btnPrev.AutoSize = true;
            this.btnPrev.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnPrev.Location = new System.Drawing.Point(3, 32);
            this.btnPrev.Name = "btnPrev";
            this.btnPrev.Size = new System.Drawing.Size(48, 23);
            this.btnPrev.TabIndex = 3;
            this.btnPrev.Text = "< Prev";
            this.btnPrev.UseVisualStyleBackColor = true;
            // 
            // btnSave
            // 
            this.btnSave.AutoSize = true;
            this.btnSave.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnSave.Location = new System.Drawing.Point(3, 3);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(42, 23);
            this.btnSave.TabIndex = 0;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnLoad
            // 
            this.btnLoad.AutoSize = true;
            this.btnLoad.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnLoad.Location = new System.Drawing.Point(58, 3);
            this.btnLoad.Name = "btnLoad";
            this.btnLoad.Size = new System.Drawing.Size(41, 23);
            this.btnLoad.TabIndex = 1;
            this.btnLoad.Text = "Load";
            this.btnLoad.UseVisualStyleBackColor = true;
            this.btnLoad.Click += new System.EventHandler(this.btnLoad_Click);
            // 
            // btnNext
            // 
            this.btnNext.Location = new System.Drawing.Point(58, 32);
            this.btnNext.Name = "btnNext";
            this.btnNext.Size = new System.Drawing.Size(47, 23);
            this.btnNext.TabIndex = 2;
            this.btnNext.Text = "Next >";
            this.btnNext.UseVisualStyleBackColor = true;
            this.btnNext.Click += new System.EventHandler(this.btnNext_Click);
            // 
            // lblPhase
            // 
            this.lblPhase.AutoSize = true;
            this.lblPhase.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblPhase.Location = new System.Drawing.Point(0, 0);
            this.lblPhase.Name = "lblPhase";
            this.lblPhase.Size = new System.Drawing.Size(35, 13);
            this.lblPhase.TabIndex = 3;
            this.lblPhase.Text = "label1";
            // 
            // imageScan1
            // 
            this.imageScan1.AutoScroll = true;
            this.imageScan1.BackColor = System.Drawing.SystemColors.ControlDark;
            this.imageScan1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.imageScan1.Location = new System.Drawing.Point(0, 394);
            this.imageScan1.Name = "imageScan1";
            this.imageScan1.Size = new System.Drawing.Size(773, 78);
            this.imageScan1.TabIndex = 2;
            this.imageScan1.WrapContents = false;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(773, 472);
            this.Controls.Add(this.panelImage);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.menuMain);
            this.Controls.Add(this.imageScan1);
            this.Name = "MainForm";
            this.Text = "AstroDeconvolution";
            this.menuMain.ResumeLayout(false);
            this.menuMain.PerformLayout();
            this.panelImage.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureMain)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.pnlPhase1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picPreviewPsf)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuMain;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private ImageScan imageScan1;
        private System.Windows.Forms.ToolStripMenuItem zoomToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem zoomInToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem zoomOutToolStripMenuItem;
        private System.Windows.Forms.Panel panelImage;
        private System.Windows.Forms.PictureBox pictureMain;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnLoad;
        private System.Windows.Forms.Button btnNext;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnPrev;
        private System.Windows.Forms.Label lblPhase;
        private System.Windows.Forms.Panel pnlPhase1;
        private System.Windows.Forms.PictureBox picPreviewPsf;
    }
}

