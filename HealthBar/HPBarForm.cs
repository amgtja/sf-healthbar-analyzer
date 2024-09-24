using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpenCvSharp;

namespace HealthBar {
    public partial class HPBarForm : Form {
        public VideoLoader videoL;
        public HPBarForm() {
            InitializeComponent();
            videoL = new VideoLoader();
        }

        public void HPBar_Load(object sender, EventArgs e) {

        }

        public void FileSelectB_Click(object sender, EventArgs e) {
            //FileSelectorクラスのインスタンス作成、動画ファイル選択
            FileSelector fileS = new FileSelector();
            string selectedF = fileS.SelectVideoFile();

            //動画を開く       
            int frame = 1;
            pictureBoxFrame.Image = OpenCvSharp.Extensions.BitmapConverter.ToBitmap(frame);
        }

        public void ConfigB_Click(object sender, EventArgs e) {
            //動画のロード確認
            if (videoL != null && videoL.TotalFrames > 0) {
                //HPBarConfigを新たなウィンドウで開く
                HPBarConfigForm hPBarConfig = new HPBarConfigForm();
                hPBarConfig.Show();
            } else { MessageBox.Show("動画が読み込まれていません", "error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }
    }
}
