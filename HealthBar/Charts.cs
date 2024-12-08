using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.DataVisualization.Charting;
using OpenCvSharp;

namespace HealthBar {
    public class Charts {
        public bool legend = false;
        public HPBarForm form;

        public Charts(HPBarForm form) {
            this.form = form;
        }
        public void DrawChart(List<byte> data) {
            form.chartDataGray.Series.Clear();
            form.chartDataGray.ChartAreas.Clear();

            ChartArea chartArea = new ChartArea("BrightnessArea");
            form.chartDataGray.ChartAreas.Add(chartArea);

            Series series = new Series("Brightness") {
                ChartType = SeriesChartType.Column,
                IsVisibleInLegend = legend
            };
            form.chartDataGray.Series.Add(series);

            // DataBindXY を使ってデータを一度に追加
            series.Points.DataBindXY(Enumerable.Range(0, data.Count), data);

            form.chartDataGray.ChartAreas["BrightnessArea"].AxisX.Title = "X座標";
            form.chartDataGray.ChartAreas["BrightnessArea"].RecalculateAxesScale();
        }

        public void DrawChartGradient(List<int> data) {
            //初期化
            form.chartG.Series.Clear();
            form.chartG.ChartAreas.Clear();

            //新しいエリアとシリーズの確保
            ChartArea chartArea = new ChartArea("GradientsArea");
            form.chartG.ChartAreas.Add(chartArea);
            Series series = new Series("Gradients") {
                ChartType = SeriesChartType.Column
            };
            form.chartG.Series.Add(series);
            //凡例なし
            series.IsVisibleInLegend = legend;
            //輝度データ追加
            for (int x = 0; x < data.Count; x++) {
                series.Points.AddXY(x, data[x]);
            }
            form.chartG.ChartAreas["GradientsArea"].AxisX.Title = "X座標";
            form.chartG.ChartAreas["GradientsArea"].RecalculateAxesScale();
        }
        public void DrawChartRGB((List<byte> R, List<byte> G, List<byte> B) rgbValues) {
            form.chartData.Series.Clear();
            form.chartData.ChartAreas.Clear(); 

            ChartArea chartArea = new ChartArea("RGBArea");
            form.chartData.ChartAreas.Add(chartArea);
            Series seriesR = new Series("Red") {
                ChartType = SeriesChartType.Line,
                Color = Color.Red
            };
            Series seriesG = new Series("Green") {
                ChartType = SeriesChartType.Line,
                Color = Color.Green
            };
            Series seriesB = new Series("Blue") {
                ChartType = SeriesChartType.Line,
                Color = Color.Blue
            };
            form.chartData.Series.Add(seriesR);
            form.chartData.Series.Add(seriesG);
            form.chartData.Series.Add(seriesB);
            //凡例なし
            seriesR.IsVisibleInLegend = legend;
            seriesG.IsVisibleInLegend = legend;
            seriesB.IsVisibleInLegend = legend;

            for (int x = 0; x < rgbValues.R.Count; x++) {
                seriesR.Points.AddXY(x, rgbValues.R[x]);
                seriesG.Points.AddXY(x, rgbValues.G[x]);
                seriesB.Points.AddXY(x, rgbValues.B[x]);
            }
            form.chartData.ChartAreas["RGBArea"].AxisX.Title = "X座標";
            form.chartData.ChartAreas["RGBArea"].RecalculateAxesScale();
        }
    }
}
