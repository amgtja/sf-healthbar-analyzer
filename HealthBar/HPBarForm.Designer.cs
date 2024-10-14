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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea2 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend2 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea3 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend3 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series3 = new System.Windows.Forms.DataVisualization.Charting.Series();
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
            this.FileSelectB.Location = new System.Drawing.Point(739, 560);
            this.FileSelectB.Margin = new System.Windows.Forms.Padding(2);
            this.FileSelectB.Name = "FileSelectB";
            this.FileSelectB.Size = new System.Drawing.Size(103, 58);
            this.FileSelectB.TabIndex = 0;
            this.FileSelectB.Text = "ファイル選択ボタン";
            this.FileSelectB.UseVisualStyleBackColor = true;
            this.FileSelectB.Click += new System.EventHandler(this.FileSelectB_Click);
            // 
            // FileDisplay
            // 
            this.FileDisplay.Location = new System.Drawing.Point(753, 622);
            this.FileDisplay.Margin = new System.Windows.Forms.Padding(2);
            this.FileDisplay.Name = "FileDisplay";
            this.FileDisplay.Size = new System.Drawing.Size(529, 19);
            this.FileDisplay.TabIndex = 1;
            this.FileDisplay.TextChanged += new System.EventHandler(this.FileDisplay_TextChanged);
            // 
            // AnalyzeB
            // 
            this.AnalyzeB.Location = new System.Drawing.Point(926, 484);
            this.AnalyzeB.Name = "AnalyzeB";
            this.AnalyzeB.Size = new System.Drawing.Size(352, 61);
            this.AnalyzeB.TabIndex = 3;
            this.AnalyzeB.Text = "解析開始ボタン";
            this.AnalyzeB.UseVisualStyleBackColor = true;
            this.AnalyzeB.Click += new System.EventHandler(this.AnalyzeB_Click);
            // 
            // pictureBoxFrame
            // 
            this.pictureBoxFrame.Location = new System.Drawing.Point(-1, 1);
            this.pictureBoxFrame.Name = "pictureBoxFrame";
            this.pictureBoxFrame.Size = new System.Drawing.Size(640, 360);
            this.pictureBoxFrame.TabIndex = 4;
            this.pictureBoxFrame.TabStop = false;
            // 
            // trackBarFrame
            // 
            this.trackBarFrame.Location = new System.Drawing.Point(644, 433);
            this.trackBarFrame.Name = "trackBarFrame";
            this.trackBarFrame.Size = new System.Drawing.Size(640, 45);
            this.trackBarFrame.TabIndex = 5;
            this.trackBarFrame.Scroll += new System.EventHandler(this.TrackBarFrame_Scroll);
            // 
            // ConfigB
            // 
            this.ConfigB.Location = new System.Drawing.Point(644, 559);
            this.ConfigB.Margin = new System.Windows.Forms.Padding(2);
            this.ConfigB.Name = "ConfigB";
            this.ConfigB.Size = new System.Drawing.Size(91, 58);
            this.ConfigB.TabIndex = 2;
            this.ConfigB.Text = "設定";
            this.ConfigB.UseVisualStyleBackColor = true;
            this.ConfigB.Click += new System.EventHandler(this.ConfigB_Click);
            // 
            // BrightText
            // 
            this.BrightText.Location = new System.Drawing.Point(644, 526);
            this.BrightText.Margin = new System.Windows.Forms.Padding(2);
            this.BrightText.Name = "BrightText";
            this.BrightText.Size = new System.Drawing.Size(91, 19);
            this.BrightText.TabIndex = 6;
            this.BrightText.TextChanged += new System.EventHandler(this.TextBox1_TextChanged);
            // 
            // pictureBoxBW
            // 
            this.pictureBoxBW.Location = new System.Drawing.Point(644, 1);
            this.pictureBoxBW.Margin = new System.Windows.Forms.Padding(2);
            this.pictureBoxBW.Name = "pictureBoxBW";
            this.pictureBoxBW.Size = new System.Drawing.Size(640, 360);
            this.pictureBoxBW.TabIndex = 8;
            this.pictureBoxBW.TabStop = false;
            this.pictureBoxBW.Click += new System.EventHandler(this.pictureBoxBW_Click);
            // 
            // chartData
            // 
            chartArea1.Name = "ChartArea1";
            this.chartData.ChartAreas.Add(chartArea1);
            legend1.Name = "Legend1";
            this.chartData.Legends.Add(legend1);
            this.chartData.Location = new System.Drawing.Point(0, 147);
            this.chartData.Margin = new System.Windows.Forms.Padding(2);
            this.chartData.Name = "chartData";
            series1.ChartArea = "ChartArea1";
            series1.Legend = "Legend1";
            series1.Name = "Series1";
            this.chartData.Series.Add(series1);
            this.chartData.Size = new System.Drawing.Size(640, 214);
            this.chartData.TabIndex = 9;
            this.chartData.Text = "chart1";
            // 
            // chartDataGray
            // 
            chartArea2.Name = "ChartArea1";
            this.chartDataGray.ChartAreas.Add(chartArea2);
            legend2.Name = "Legend1";
            this.chartDataGray.Legends.Add(legend2);
            this.chartDataGray.Location = new System.Drawing.Point(644, 240);
            this.chartDataGray.Margin = new System.Windows.Forms.Padding(2);
            this.chartDataGray.Name = "chartDataGray";
            series2.ChartArea = "ChartArea1";
            series2.Legend = "Legend1";
            series2.Name = "Series1";
            this.chartDataGray.Series.Add(series2);
            this.chartDataGray.Size = new System.Drawing.Size(640, 180);
            this.chartDataGray.TabIndex = 10;
            this.chartDataGray.Text = "chart1";
            this.chartDataGray.Click += new System.EventHandler(this.chartDataGray_Click);
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
            // chartG
            // 
            chartArea3.Name = "ChartArea1";
            this.chartG.ChartAreas.Add(chartArea3);
            legend3.Name = "Legend1";
            this.chartG.Legends.Add(legend3);
            this.chartG.Location = new System.Drawing.Point(-1, 365);
            this.chartG.Margin = new System.Windows.Forms.Padding(2);
            this.chartG.Name = "chartG";
            series3.ChartArea = "ChartArea1";
            series3.Legend = "Legend1";
            series3.Name = "Series1";
            this.chartG.Series.Add(series3);
            this.chartG.Size = new System.Drawing.Size(640, 272);
            this.chartG.TabIndex = 11;
            this.chartG.Text = "chart1";
            // 
            // SetBaseBoundaryB
            // 
            this.SetBaseBoundaryB.Location = new System.Drawing.Point(926, 551);
            this.SetBaseBoundaryB.Name = "SetBaseBoundaryB";
            this.SetBaseBoundaryB.Size = new System.Drawing.Size(100, 48);
            this.SetBaseBoundaryB.TabIndex = 12;
            this.SetBaseBoundaryB.Text = "基準設定";
            this.SetBaseBoundaryB.UseVisualStyleBackColor = true;
            this.SetBaseBoundaryB.Click += new System.EventHandler(this.SetBaseBoundaryB_Click);
            // 
            // CaliculateAllFramesB
            // 
            this.CaliculateAllFramesB.Location = new System.Drawing.Point(1053, 551);
            this.CaliculateAllFramesB.Name = "CaliculateAllFramesB";
            this.CaliculateAllFramesB.Size = new System.Drawing.Size(106, 48);
            this.CaliculateAllFramesB.TabIndex = 13;
            this.CaliculateAllFramesB.Text = "計算開始";
            this.CaliculateAllFramesB.UseVisualStyleBackColor = true;
            this.CaliculateAllFramesB.Click += new System.EventHandler(this.CaliculateAllFramesB_Click);
            // 
            // SaveToCSVB
            // 
            this.SaveToCSVB.Location = new System.Drawing.Point(1178, 551);
            this.SaveToCSVB.Name = "SaveToCSVB";
            this.SaveToCSVB.Size = new System.Drawing.Size(100, 48);
            this.SaveToCSVB.TabIndex = 14;
            this.SaveToCSVB.Text = "結果保存";
            this.SaveToCSVB.UseVisualStyleBackColor = true;
            this.SaveToCSVB.Click += new System.EventHandler(this.SaveToCSVB_Click);
            // 
            // progressBar
            // 
            this.progressBar.Location = new System.Drawing.Point(644, 484);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(276, 37);
            this.progressBar.TabIndex = 15;
            // 
            // HPBarForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1290, 637);
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
            this.Margin = new System.Windows.Forms.Padding(2);
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
    }
}

