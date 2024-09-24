using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpenCvSharp;

namespace HealthBar
{
    public class FileSelector
    {
        //ファイル選択を行うメソッド
        public string SelectVideoFile()
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                //ファイル種類のフィルタ
                ofd.Filter = "動画ファイル (*.mp4;*.avi;*.mov)|*.mp4;*.avi;*.mov|すべてのファイル (*.*)|*.*";
                ofd.Title = "動画ファイルを選択してください";
                //ダイアログを表示し、ファイルパスを返す
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    return ofd.FileName;
                }
                else
                {
                    return string.Empty;
                }
            }
        }
    }
}
