namespace HealthBar
{
    partial class HPBarForm
    {
        /// <summary>
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージド リソースを破棄する場合は true を指定し、その他の場合は false を指定します。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows フォーム デザイナーで生成されたコード

        /// <summary>
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.FileSelectB = new System.Windows.Forms.Button();
            this.AnalyzeB = new System.Windows.Forms.Button();
            this.pictureBoxFrame = new System.Windows.Forms.PictureBox();
            this.trackBarFrame = new System.Windows.Forms.TrackBar();
            this.ConfigB = new System.Windows.Forms.Button();
            this.pictureBoxHP = new System.Windows.Forms.PictureBox();
            this.CaliculateAllFramesB = new System.Windows.Forms.Button();
            this.SaveToCSVB = new System.Windows.Forms.Button();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.FrameBox = new System.Windows.Forms.TextBox();
            this.timerFramePlay = new System.Windows.Forms.Timer(this.components);
            this.btnPlay = new System.Windows.Forms.Button();
            this.listBox = new System.Windows.Forms.ListBox();
            this.buttonSF5 = new System.Windows.Forms.Button();
            this.buttonSF6 = new System.Windows.Forms.Button();
            this.buttonClear = new System.Windows.Forms.Button();
            this.ConfigManualB = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxFrame)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarFrame)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxHP)).BeginInit();
            this.SuspendLayout();
            // 
            // FileSelectB
            // 
            this.FileSelectB.Location = new System.Drawing.Point(463, 302);
            this.FileSelectB.Margin = new System.Windows.Forms.Padding(2);
            this.FileSelectB.Name = "FileSelectB";
            this.FileSelectB.Size = new System.Drawing.Size(222, 35);
            this.FileSelectB.TabIndex = 0;
            this.FileSelectB.Text = "１．ファイル選択";
            this.FileSelectB.UseVisualStyleBackColor = true;
            this.FileSelectB.Click += new System.EventHandler(this.FileSelectB_Click);
            // 
            // AnalyzeB
            // 
            this.AnalyzeB.Location = new System.Drawing.Point(576, 455);
            this.AnalyzeB.Name = "AnalyzeB";
            this.AnalyzeB.Size = new System.Drawing.Size(134, 55);
            this.AnalyzeB.TabIndex = 3;
            this.AnalyzeB.Text = "４．解析設定読み込み";
            this.AnalyzeB.UseVisualStyleBackColor = true;
            this.AnalyzeB.Click += new System.EventHandler(this.AnalyzeB_Click);
            // 
            // pictureBoxFrame
            // 
            this.pictureBoxFrame.Location = new System.Drawing.Point(4, 1);
            this.pictureBoxFrame.Name = "pictureBoxFrame";
            this.pictureBoxFrame.Size = new System.Drawing.Size(1280, 295);
            this.pictureBoxFrame.TabIndex = 4;
            this.pictureBoxFrame.TabStop = false;
            //
            // trackBarFrame
            // 
            this.trackBarFrame.Location = new System.Drawing.Point(463, 341);
            this.trackBarFrame.Name = "trackBarFrame";
            this.trackBarFrame.Size = new System.Drawing.Size(821, 45);
            this.trackBarFrame.TabIndex = 5;
            this.trackBarFrame.Scroll += new System.EventHandler(this.TrackBarFrame_Scroll);
            this.trackBarFrame.ValueChanged += new System.EventHandler(this.trackBarFrame_ValueChanged);
            // 
            // ConfigB
            // 
            this.ConfigB.Location = new System.Drawing.Point(576, 386);
            this.ConfigB.Margin = new System.Windows.Forms.Padding(2);
            this.ConfigB.Name = "ConfigB";
            this.ConfigB.Size = new System.Drawing.Size(109, 55);
            this.ConfigB.TabIndex = 2;
            this.ConfigB.Text = "３．座標指定";
            this.ConfigB.UseVisualStyleBackColor = true;
            this.ConfigB.Click += new System.EventHandler(this.ConfigB_Click);
            // 
            // pictureBoxHP
            // 
            this.pictureBoxHP.Location = new System.Drawing.Point(-1, 398);
            this.pictureBoxHP.Margin = new System.Windows.Forms.Padding(2);
            this.pictureBoxHP.Name = "pictureBoxHP";
            this.pictureBoxHP.Size = new System.Drawing.Size(0, 0);
            this.pictureBoxHP.TabIndex = 7;
            this.pictureBoxHP.TabStop = false;
            // 
            // CaliculateAllFramesB
            // 
            this.CaliculateAllFramesB.Location = new System.Drawing.Point(901, 457);
            this.CaliculateAllFramesB.Name = "CaliculateAllFramesB";
            this.CaliculateAllFramesB.Size = new System.Drawing.Size(94, 55);
            this.CaliculateAllFramesB.TabIndex = 13;
            this.CaliculateAllFramesB.Text = "５．解析開始";
            this.CaliculateAllFramesB.UseVisualStyleBackColor = true;
            this.CaliculateAllFramesB.Click += new System.EventHandler(this.CaliculateAllFramesB_Click);
            // 
            // SaveToCSVB
            // 
            this.SaveToCSVB.Location = new System.Drawing.Point(1001, 458);
            this.SaveToCSVB.Name = "SaveToCSVB";
            this.SaveToCSVB.Size = new System.Drawing.Size(105, 55);
            this.SaveToCSVB.TabIndex = 14;
            this.SaveToCSVB.Text = "６．結果保存";
            this.SaveToCSVB.UseVisualStyleBackColor = true;
            this.SaveToCSVB.Click += new System.EventHandler(this.SaveToCSVB_Click);
            // 
            // progressBar
            // 
            this.progressBar.Location = new System.Drawing.Point(901, 404);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(373, 37);
            this.progressBar.TabIndex = 15;
            // 
            // FrameBox
            // 
            this.FrameBox.Location = new System.Drawing.Point(689, 320);
            this.FrameBox.Margin = new System.Windows.Forms.Padding(2);
            this.FrameBox.Name = "FrameBox";
            this.FrameBox.Size = new System.Drawing.Size(100, 19);
            this.FrameBox.TabIndex = 16;
            // 
            // timerFramePlay
            // 
            this.timerFramePlay.Interval = 16;
            this.timerFramePlay.Tick += new System.EventHandler(this.timerFramePlay_Tick);
            // 
            // btnPlay
            // 
            this.btnPlay.Location = new System.Drawing.Point(792, 302);
            this.btnPlay.Margin = new System.Windows.Forms.Padding(2);
            this.btnPlay.Name = "btnPlay";
            this.btnPlay.Size = new System.Drawing.Size(177, 34);
            this.btnPlay.TabIndex = 18;
            this.btnPlay.Text = "再生";
            this.btnPlay.UseVisualStyleBackColor = true;
            this.btnPlay.Click += new System.EventHandler(this.btnPlay_Click);
            // 
            // listBox
            // 
            this.listBox.FormattingEnabled = true;
            this.listBox.ItemHeight = 12;
            this.listBox.Location = new System.Drawing.Point(4, 305);
            this.listBox.Margin = new System.Windows.Forms.Padding(2);
            this.listBox.Name = "listBox";
            this.listBox.Size = new System.Drawing.Size(456, 208);
            this.listBox.TabIndex = 20;
            // 
            // buttonSF5
            // 
            this.buttonSF5.Location = new System.Drawing.Point(462, 386);
            this.buttonSF5.Margin = new System.Windows.Forms.Padding(2);
            this.buttonSF5.Name = "buttonSF5";
            this.buttonSF5.Size = new System.Drawing.Size(109, 55);
            this.buttonSF5.TabIndex = 21;
            this.buttonSF5.Text = "２．SF5用設定";
            this.buttonSF5.UseVisualStyleBackColor = true;
            this.buttonSF5.Click += new System.EventHandler(this.buttonSF5_Click);
            // 
            // buttonSF6
            // 
            this.buttonSF6.Location = new System.Drawing.Point(462, 455);
            this.buttonSF6.Margin = new System.Windows.Forms.Padding(2);
            this.buttonSF6.Name = "buttonSF6";
            this.buttonSF6.Size = new System.Drawing.Size(109, 55);
            this.buttonSF6.TabIndex = 22;
            this.buttonSF6.Text = "２．SF6用設定";
            this.buttonSF6.UseVisualStyleBackColor = true;
            this.buttonSF6.Click += new System.EventHandler(this.buttonSF6_Click);
            // 
            // buttonClear
            // 
            this.buttonClear.Location = new System.Drawing.Point(363, 478);
            this.buttonClear.Margin = new System.Windows.Forms.Padding(2);
            this.buttonClear.Name = "buttonClear";
            this.buttonClear.Size = new System.Drawing.Size(95, 33);
            this.buttonClear.TabIndex = 23;
            this.buttonClear.Text = "ClearMessage";
            this.buttonClear.UseVisualStyleBackColor = true;
            this.buttonClear.Click += new System.EventHandler(this.buttonClear_Click);
            // 
            // ConfigManualB
            // 
            this.ConfigManualB.Location = new System.Drawing.Point(690, 386);
            this.ConfigManualB.Name = "ConfigManualB";
            this.ConfigManualB.Size = new System.Drawing.Size(123, 55);
            this.ConfigManualB.TabIndex = 24;
            this.ConfigManualB.Text = "3. 座標指定(手動)";
            this.ConfigManualB.UseVisualStyleBackColor = true;
            this.ConfigManualB.Click += new System.EventHandler(this.ConfigManualB_Click);
            // 
            // HPBarForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1283, 522);
            this.Controls.Add(this.ConfigManualB);
            this.Controls.Add(this.buttonClear);
            this.Controls.Add(this.buttonSF6);
            this.Controls.Add(this.buttonSF5);
            this.Controls.Add(this.listBox);
            this.Controls.Add(this.btnPlay);
            this.Controls.Add(this.FrameBox);
            this.Controls.Add(this.progressBar);
            this.Controls.Add(this.SaveToCSVB);
            this.Controls.Add(this.CaliculateAllFramesB);
            this.Controls.Add(this.pictureBoxHP);
            this.Controls.Add(this.trackBarFrame);
            this.Controls.Add(this.pictureBoxFrame);
            this.Controls.Add(this.AnalyzeB);
            this.Controls.Add(this.ConfigB);
            this.Controls.Add(this.FileSelectB);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "HPBarForm";
            this.Text = "eスポーツ体力ゲージ解析プログラム";
            this.Load += new System.EventHandler(this.HPBar_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxFrame)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarFrame)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxHP)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.Button FileSelectB;
        public System.Windows.Forms.Button AnalyzeB;
        public System.Windows.Forms.PictureBox pictureBoxFrame;
        public System.Windows.Forms.TrackBar trackBarFrame;
        public System.Windows.Forms.Button ConfigB;
        public System.Windows.Forms.PictureBox pictureBoxHP;
        private System.Windows.Forms.Button CaliculateAllFramesB;
        private System.Windows.Forms.Button SaveToCSVB;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.TextBox FrameBox;
        private System.Windows.Forms.Timer timerFramePlay;
        private System.Windows.Forms.Button btnPlay;
        private System.Windows.Forms.ListBox listBox;
        private System.Windows.Forms.Button buttonSF5;
        private System.Windows.Forms.Button buttonSF6;
        private System.Windows.Forms.Button buttonClear;
        private System.Windows.Forms.Button ConfigManualB;
    }
}

