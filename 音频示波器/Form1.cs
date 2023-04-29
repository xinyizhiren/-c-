using NAudio;
using NAudio.Dsp;
using NAudio.Wave;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;

namespace 音频示波器
{
    public partial class Form1 : Form
    {
        WasapiLoopbackCapture cap;
        double[] finalData;   //最终数据
        double[] oldData;  //上一次的数据

    public Form1()
        {
            InitializeComponent();
            Rectangle screenBounds = Screen.GetWorkingArea(this);
            this.ShowInTaskbar = false;
            // 计算窗口的左上角坐标，使其位于屏幕中心
            int x = screenBounds.Width - this.Width ;
            int y = screenBounds.Height - this.Height;
            this.Location = new Point(x, y);
            toolStripComboBox1.SelectedIndex = 0;
            Audio_Capture();
        }
        private void Audio_Capture()
        {
            cap = new WasapiLoopbackCapture();
            cap.DataAvailable += (sender, e) =>      // 录制数据可用时触发此事件, 参数中包含音频数据
            {
                float[] allSamples = Enumerable      // 提取数据中的采样
                    .Range(0, e.BytesRecorded / 4)   // 除以四是因为, 缓冲区内每 4 个字节构成一个浮点数, 一个浮点数是一个采样
                    .Select(i => BitConverter.ToSingle(e.Buffer, i * 4))  // 转换为 float
                    .ToArray();    // 转换为数组
                                   // 获取采样后, 在这里进行详细处理
                
                int channelCount = cap.WaveFormat.Channels;   // WasapiLoopbackCapture 的 WaveFormat 指定了当前声音的波形格式, 其中包含就通道数
                float[][] channelSamples = Enumerable
                    .Range(0, channelCount)
                    .Select(channel => Enumerable
                        .Range(0, allSamples.Length / channelCount)
                        .Select(i => allSamples[channel + i * channelCount])
                        .ToArray())
                    .ToArray();
                float[] averageSamples = Enumerable
                    .Range(0, allSamples.Length / channelCount)
                    .Select(index => Enumerable
                        .Range(0, channelCount)
                        .Select(channel => channelSamples[channel][index])
                        .Average())
                        .ToArray();
                // 因为对于快速傅里叶变换算法, 需要数据长度为 2 的 n 次方, 这里进行
                double log = Math.Ceiling(Math.Log(averageSamples.Length, 2));   // 取对数并向上取整
                int newLen = (int)Math.Pow(2, log);                             // 计算新长度
                float[] filledSamples = new float[newLen];
                Array.Copy(averageSamples, filledSamples, averageSamples.Length);   // 拷贝到新数组
                Complex[] complexSrc = filledSamples
                    .Select(v => new Complex() { X = v })        // 将采样转换为复数
                    .ToArray();
                FastFourierTransform.FFT(false, (int)log, complexSrc);   // 进行傅里叶变换
                Complex[] halfData = complexSrc
                    .Take(complexSrc.Length / 2)
                    .ToArray();    // 一半的数据
                double[] dftData = halfData
                    .Select(v => Math.Sqrt(v.X * v.X + v.Y * v.Y))  // 取复数的模
                    .ToArray();    // 将复数结果转换为我们所需要的频率幅度
                int count = 2500 / (cap.WaveFormat.SampleRate / filledSamples.Length);
                finalData = Enumerable
                .Range(0, count / 2)
                .Select(i => (dftData[2 * i] + dftData[2 * i + 1]))
                .ToArray();
                this.Invalidate();
            };
            cap.StartRecording();   // 开始录制
        }
        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            if (finalData == null || finalData.Length < 100)
            {
                return;
            }
            if (oldData != null)
            {
                finalData = Enumerable
                .Range(0, Math.Min(finalData.Length, oldData.Length))
                .Select(i => oldData[i] + 0.8*(finalData[i] - oldData[i]))
                .ToArray();
            }
            RectangleF[] rectangle = finalData
                .Select((v, i) => new RectangleF(i * (this.Width / finalData.Length), (float)0.9 * this.Height - (float)(50 * Math.Sqrt(v)), (float)0.6*(this.Width / finalData.Length), (float)(50 * Math.Sqrt(v))))
                .ToArray();
            //绘图
            Graphics g = e.Graphics;
            Brush brush = new SolidBrush(Color.FromArgb(0, 191,255));
            /*
             * foreach (RectangleF rec in rectangle)
            {
                //下面这条经常报错
                if(rec.Height > 0)
                {
                Brush b = new LinearGradientBrush(rec, Color.FromArgb(0, 191,255), Color.FromArgb(0,139,139), LinearGradientMode.Vertical);
                g.FillRectangle(b, rec);
                }
                
            }
             */
            
            g.FillRectangles(brush, rectangle);
            oldData = new double[finalData.Length];
            Array.Copy(finalData, oldData, finalData.Length);
        }

        private void exit_Click(object sender, EventArgs e)
        {
            System.Environment.Exit(0);
        }

        private void toolStripComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.Opacity = 1 - toolStripComboBox1.SelectedIndex * 0.25;
        }
    }
}