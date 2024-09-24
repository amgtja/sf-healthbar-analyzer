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

namespace HealthBar
{
    public partial class HPBarConfigForm : Form
    {
        public VideoLoader videoL;
        public HPBarConfigForm()
        {
            InitializeComponent();
        }

        public void HPBarConfig_Load(object sender, EventArgs e)
        {
            //シークバーの設定
            TrackBarFrame.Minimum = 0;
            TrackBarFrame.Maximum = videoL.TotalFrames - 1;
            TrackBarFrame.Scroll += TrackBarFrame_Scroll;
        }

        public void TrackBarFrame_Scroll(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
