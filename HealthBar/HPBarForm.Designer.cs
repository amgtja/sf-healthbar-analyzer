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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea19 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend19 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series19 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea20 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend20 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series20 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea21 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend21 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series21 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.FileSelectB = new System.Windows.Forms.Button();
            this.FileDisplay = new System.Windows.Forms.TextBox();
            this.AnalyzeB = new System.Windows.Forms.Button();
            this.pictureBoxFrame = new System.Windows.Forms.PictureBox();
            this.trackBarFrame = new System.Windows.Forms.TrackBar();
            this.ConfigB = new System.Windows.Forms.Button();
            this.BrightText = new System.Windows.Forms.TextBox();
            this.pictureBoxBW = new System.Windows.Forms.PictureBox();
            this.chartData = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.chartDataGray = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.pictureBoxHP = new System.Windows.Forms.PictureBox();
            this.chartG = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.SetBaseBoundaryB = new System.Windows.Forms.Button();
            this.CaliculateAllFramesB = new System.Windows.Forms.Button();
            this.SaveToCSVB = new System.Windows.Forms.Button();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.FrameBox = new System.Windows.Forms.TextBox();
            this.HealthText = new System.Windows.Forms.TextBox();
            this.timerFramePlay = new System.Windows.Forms.Timer(this.components);
            this.btnPlay = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxFrame)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarFrame)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxBW)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartData)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartDataGray)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxHP)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartG)).BeginInit();
            this.SuspendLayout();
            // 
            // FileSelectB
            // 
            this.FileSelectB.Location = new System.Drawing.Point(1108, 934);
            this.FileSelectB.Name = "FileSelectB";
            this.FileSelectB.Size = new System.Drawing.Size(154, 97);
            this.FileSelectB.TabIndex = 0;
            this.FileSelectB.Text = "ファイル選択ボタン";
            this.FileSelectB.UseVisualStyleBackColor = true;
            this.FileSelectB.Click += new System.EventHandler(this.FileSelectB_Click);
            // 
            // FileDisplay
            // 
            this.FileDisplay.Location = new System.Drawing.Point(1168, 1037);
            this.FileDisplay.Name = "FileDisplay";
            this.FileDisplay.Size = new System.Drawing.Size(792, 26);
            this.FileDisplay.TabIndex = 1;
            this.FileDisplay.TextChanged += new System.EventHandler(this.FileDisplay_TextChanged);
            // 
            // AnalyzeB
            // 
            this.AnalyzeB.Location = new System.Drawing.Point(1389, 806);
            this.AnalyzeB.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.AnalyzeB.Name = "AnalyzeB";
            this.AnalyzeB.Size = new System.Drawing.Size(528, 102);
            this.AnalyzeB.TabIndex = 3;
            this.AnalyzeB.Text = "解析開始ボタン";
            this.AnalyzeB.UseVisualStyleBackColor = true;
            this.AnalyzeB.Click += new System.EventHandler(this.AnalyzeB_Click);
            // 
            // pictureBoxFrame
            // 
            this.pictureBoxFrame.Location = new System.Drawing.Point(18, 18);
            this.pictureBoxFrame.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.pictureBoxFrame.Name = "pictureBoxFrame";
            this.pictureBoxFrame.Size = new System.Drawing.Size(960, 554);
            this.pictureBoxFrame.TabIndex = 4;
            this.pictureBoxFrame.TabStop = false;
            this.pictureBoxFrame.Click += new System.EventHandler(this.pictureBoxFrame_Click);
            // 
            // trackBarFrame
            // 
            this.trackBarFrame.Location = new System.Drawing.Point(966, 728);
            this.trackBarFrame.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.trackBarFrame.Name = "trackBarFrame";
            this.trackBarFrame.Size = new System.Drawing.Size(960, 69);
            this.trackBarFrame.TabIndex = 5;
            this.trackBarFrame.Scroll += new System.EventHandler(this.TrackBarFrame_Scroll);
            this.trackBarFrame.ValueChanged += new System.EventHandler(this.trackBarFrame_ValueChanged);
            // 
            // ConfigB
            // 
            this.ConfigB.Location = new System.Drawing.Point(966, 932);
            this.ConfigB.Name = "ConfigB";
            this.ConfigB.Size = new System.Drawing.Size(136, 97);
            this.ConfigB.TabIndex = 2;
            this.ConfigB.Text = "設定";
            this.ConfigB.UseVisualStyleBackColor = true;
            this.ConfigB.Click += new System.EventHandler(this.ConfigB_Click);
            // 
            // BrightText
            // 
            this.BrightText.Location = new System.Drawing.Point(966, 877);
            this.BrightText.Name = "BrightText";
            this.BrightText.Size = new System.Drawing.Size(134, 26);
            this.BrightText.TabIndex = 6;
            this.BrightText.TextChanged += new System.EventHandler(this.TextBox1_TextChanged);
            // 
            // pictureBoxBW
            // 
            this.pictureBoxBW.Location = new System.Drawing.Point(1854, 2);
            this.pictureBoxBW.Name = "pictureBoxBW";
            this.pictureBoxBW.Size = new System.Drawing.Size(72, 46);
            this.pictureBoxBW.TabIndex = 8;
            this.pictureBoxBW.TabStop = false;
            this.pictureBoxBW.Click += new System.EventHandler(this.pictureBoxBW_Click);
            // 
            // chartData
            // 
            chartArea19.Name = "ChartArea1";
            this.chartData.ChartAreas.Add(chartArea19);
            legend19.Name = "Legend1";
            this.chartData.Legends.Add(legend19);
            this.chartData.Location = new System.Drawing.Point(986, 320);
            this.chartData.Name = "chartData";
            series19.ChartArea = "ChartArea1";
            series19.Legend = "Legend1";
            series19.Name = "Series1";
            this.chartData.Series.Add(series19);
            this.chartData.Size = new System.Drawing.Size(940, 252);
            this.chartData.TabIndex = 9;
            this.chartData.Text = "chart1";
            // 
            // chartDataGray
            // 
            chartArea20.Name = "ChartArea1";
            this.chartDataGray.ChartAreas.Add(chartArea20);
            legend20.Name = "Legend1";
            this.chartDataGray.Legends.Add(legend20);
            this.chartDataGray.Location = new System.Drawing.Point(1731, 2);
            this.chartDataGray.Name = "chartDataGray";
            series20.ChartArea = "ChartArea1";
            series20.Legend = "Legend1";
            series20.Name = "Series1";
            this.chartDataGray.Series.Add(series20);
            this.chartDataGray.Size = new System.Drawing.Size(117, 57);
            this.chartDataGray.TabIndex = 10;
            this.chartDataGray.Text = "chart1";
            this.chartDataGray.Click += new System.EventHandler(this.chartDataGray_Click);
            // 
            // pictureBoxHP
            // 
            this.pictureBoxHP.Location = new System.Drawing.Point(-2, 663);
            this.pictureBoxHP.Name = "pictureBoxHP";
            this.pictureBoxHP.Size = new System.Drawing.Size(0, 0);
            this.pictureBoxHP.TabIndex = 7;
            this.pictureBoxHP.TabStop = false;
            // 
            // chartG
            // 
            chartArea21.Name = "ChartArea1";
            this.chartG.ChartAreas.Add(chartArea21);
            legend21.Name = "Legend1";
            this.chartG.Legends.Add(legend21);
            this.chartG.Location = new System.Drawing.Point(986, 65);
            this.chartG.Name = "chartG";
            series21.ChartArea = "ChartArea1";
            series21.Legend = "Legend1";
            series21.Name = "Series1";
            this.chartG.Series.Add(series21);
            this.chartG.Size = new System.Drawing.Size(940, 249);
            this.chartG.TabIndex = 11;
            this.chartG.Text = "chart1";
            // 
            // SetBaseBoundaryB
            // 
            this.SetBaseBoundaryB.Location = new System.Drawing.Point(1389, 918);
            this.SetBaseBoundaryB.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.SetBaseBoundaryB.Name = "SetBaseBoundaryB";
            this.SetBaseBoundaryB.Size = new System.Drawing.Size(150, 80);
            this.SetBaseBoundaryB.TabIndex = 12;
            this.SetBaseBoundaryB.Text = "基準設定";
            this.SetBaseBoundaryB.UseVisualStyleBackColor = true;
            this.SetBaseBoundaryB.Click += new System.EventHandler(this.SetBaseBoundaryB_Click);
            // 
            // CaliculateAllFramesB
            // 
            this.CaliculateAllFramesB.Location = new System.Drawing.Point(1580, 918);
            this.CaliculateAllFramesB.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.CaliculateAllFramesB.Name = "CaliculateAllFramesB";
            this.CaliculateAllFramesB.Size = new System.Drawing.Size(159, 80);
            this.CaliculateAllFramesB.TabIndex = 13;
            this.CaliculateAllFramesB.Text = "計算開始";
            this.CaliculateAllFramesB.UseVisualStyleBackColor = true;
            this.CaliculateAllFramesB.Click += new System.EventHandler(this.CaliculateAllFramesB_Click);
            // 
            // SaveToCSVB
            // 
            this.SaveToCSVB.Location = new System.Drawing.Point(1767, 918);
            this.SaveToCSVB.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.SaveToCSVB.Name = "SaveToCSVB";
            this.SaveToCSVB.Size = new System.Drawing.Size(150, 80);
            this.SaveToCSVB.TabIndex = 14;
            this.SaveToCSVB.Text = "結果保存";
            this.SaveToCSVB.UseVisualStyleBackColor = true;
            this.SaveToCSVB.Click += new System.EventHandler(this.SaveToCSVB_Click);
            // 
            // progressBar
            // 
            this.progressBar.Location = new System.Drawing.Point(966, 806);
            this.progressBar.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(414, 62);
            this.progressBar.TabIndex = 15;
            // 
            // FrameBox
            // 
            this.FrameBox.Location = new System.Drawing.Point(1108, 877);
            this.FrameBox.Name = "FrameBox";
            this.FrameBox.Size = new System.Drawing.Size(154, 26);
            this.FrameBox.TabIndex = 16;
            // 
            // HealthText
            // 
            this.HealthText.Location = new System.Drawing.Point(1280, 877);
            this.HealthText.Name = "HealthText";
            this.HealthText.Size = new System.Drawing.Size(100, 26);
            this.HealthText.TabIndex = 17;
            // 
            // timerFramePlay
            // 
            this.timerFramePlay.Tick += new System.EventHandler(this.timerFramePlay_Tick);
            // 
            // btnPlay
            // 
            this.btnPlay.Location = new System.Drawing.Point(390, 675);
            this.btnPlay.Name = "btnPlay";
            this.btnPlay.Size = new System.Drawing.Size(266, 57);
            this.btnPlay.TabIndex = 18;
            this.btnPlay.Text = "再生";
            this.btnPlay.UseVisualStyleBackColor = true;
            this.btnPlay.Click += new System.EventHandler(this.btnPlay_Click);
            // 
            // HPBarForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1924, 1062);
            this.Controls.Add(this.btnPlay);
            this.Controls.Add(this.HealthText);
            this.Controls.Add(this.FrameBox);
            this.Controls.Add(this.progressBar);
            this.Controls.Add(this.SaveToCSVB);
            this.Controls.Add(this.CaliculateAllFramesB);
            this.Controls.Add(this.SetBaseBoundaryB);
            this.Controls.Add(this.chartG);
            this.Controls.Add(this.chartDataGray);
            this.Controls.Add(this.chartData);
            this.Controls.Add(this.pictureBoxBW);
            this.Controls.Add(this.pictureBoxHP);
            this.Controls.Add(this.BrightText);
            this.Controls.Add(this.trackBarFrame);
            this.Controls.Add(this.pictureBoxFrame);
            this.Controls.Add(this.AnalyzeB);
            this.Controls.Add(this.ConfigB);
            this.Controls.Add(this.FileDisplay);
            this.Controls.Add(this.FileSelectB);
            this.Name = "HPBarForm";
            this.Text = "体力ゲージ記録";
            this.Load += new System.EventHandler(this.HPBar_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxFrame)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarFrame)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxBW)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartData)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartDataGray)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxHP)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartG)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.Button FileSelectB;
        public System.Windows.Forms.TextBox FileDisplay;
        public System.Windows.Forms.Button AnalyzeB;
        public System.Windows.Forms.PictureBox pictureBoxFrame;
        public System.Windows.Forms.TrackBar trackBarFrame;
        public System.Windows.Forms.Button ConfigB;
        public System.Windows.Forms.TextBox BrightText;
        public System.Windows.Forms.PictureBox pictureBoxBW;
        public System.Windows.Forms.DataVisualization.Charting.Chart chartData;
        public System.Windows.Forms.DataVisualization.Charting.Chart chartDataGray;
        public System.Windows.Forms.PictureBox pictureBoxHP;
        public System.Windows.Forms.DataVisualization.Charting.Chart chartG;
        private System.Windows.Forms.Button SetBaseBoundaryB;
        private System.Windows.Forms.Button CaliculateAllFramesB;
        private System.Windows.Forms.Button SaveToCSVB;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.TextBox FrameBox;
        private System.Windows.Forms.TextBox HealthText;
        private System.Windows.Forms.Timer timerFramePlay;
        private System.Windows.Forms.Button btnPlay;
    }
}

