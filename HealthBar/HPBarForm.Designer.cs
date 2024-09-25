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
            this.FileSelectB = new System.Windows.Forms.Button();
            this.FileDisplay = new System.Windows.Forms.TextBox();
            this.ConfigB = new System.Windows.Forms.Button();
            this.AnalyzeB = new System.Windows.Forms.Button();
            this.pictureBoxFrame = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxFrame)).BeginInit();
            this.SuspendLayout();
            // 
            // FileSelectB
            // 
            this.FileSelectB.Location = new System.Drawing.Point(385, 422);
            this.FileSelectB.Margin = new System.Windows.Forms.Padding(2);
            this.FileSelectB.Name = "FileSelectB";
            this.FileSelectB.Size = new System.Drawing.Size(196, 108);
            this.FileSelectB.TabIndex = 0;
            this.FileSelectB.Text = "ファイル選択ボタン";
            this.FileSelectB.UseVisualStyleBackColor = true;
            this.FileSelectB.Click += new System.EventHandler(this.FileSelectB_Click);
            // 
            // FileDisplay
            // 
            this.FileDisplay.Location = new System.Drawing.Point(231, 10);
            this.FileDisplay.Margin = new System.Windows.Forms.Padding(2);
            this.FileDisplay.Name = "FileDisplay";
            this.FileDisplay.Size = new System.Drawing.Size(529, 19);
            this.FileDisplay.TabIndex = 1;
            // 
            // ConfigB
            // 
            this.ConfigB.Location = new System.Drawing.Point(84, 429);
            this.ConfigB.Margin = new System.Windows.Forms.Padding(2);
            this.ConfigB.Name = "ConfigB";
            this.ConfigB.Size = new System.Drawing.Size(143, 101);
            this.ConfigB.TabIndex = 2;
            this.ConfigB.Text = "設定";
            this.ConfigB.UseVisualStyleBackColor = true;
            this.ConfigB.Click += new System.EventHandler(this.ConfigB_Click);
            // 
            // AnalyzeB
            // 
            this.AnalyzeB.Location = new System.Drawing.Point(712, 414);
            this.AnalyzeB.Name = "AnalyzeB";
            this.AnalyzeB.Size = new System.Drawing.Size(182, 122);
            this.AnalyzeB.TabIndex = 3;
            this.AnalyzeB.Text = "解析ボタン";
            this.AnalyzeB.UseVisualStyleBackColor = true;
            // 
            // pictureBoxFrame
            // 
            this.pictureBoxFrame.Location = new System.Drawing.Point(171, 48);
            this.pictureBoxFrame.Name = "pictureBoxFrame";
            this.pictureBoxFrame.Size = new System.Drawing.Size(640, 360);
            this.pictureBoxFrame.TabIndex = 4;
            this.pictureBoxFrame.TabStop = false;
            // 
            // HPBarForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(971, 520);
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
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button FileSelectB;
        private System.Windows.Forms.TextBox FileDisplay;
        private System.Windows.Forms.Button ConfigB;
        private System.Windows.Forms.Button AnalyzeB;
        private System.Windows.Forms.PictureBox pictureBoxFrame;
    }
}

