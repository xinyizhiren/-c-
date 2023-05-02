using NAudio;
using NAudio.Dsp;
using NAudio.Wave;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Net.Http;
using System.Runtime.InteropServices;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;
/*
炫酷蓝色：#00BFFF（0, 191, 255）
靛青色：#008B8B（0, 139, 139）
青绿色：#00CED1（0, 206, 209）
碧绿色：#7FFFD4（127, 255, 212）
酸橙色：#00FF7F（0, 255, 127）
银色：#C0C0C0（192, 192, 192）
暗灰色：#A9A9A9（169, 169, 169）
深青色：#008080（0, 128, 128）
紫罗兰色：#9400D3（148, 0, 211）
 */

namespace 音频示波器
{
    public partial class Form1 : Form
    {
        // 用于处理WM_NCHITTEST消息的常量
        private const int WM_NCHITTEST = 0x0084;
        private const int HTCLIENT = 1;
        private const int HTCAPTION = 2;

        // 用于移动窗口的常量和方法
        private const int WM_SYSCOMMAND = 0x0112;
        private const int SC_MOVE = 0xF010;
        [DllImport("user32.dll")]
        private static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [DllImport("user32.dll")]
        private static extern bool ReleaseCapture();

        WasapiLoopbackCapture cap;
        Rectangle screenBounds;
        double[] finalData;   //最终数据
        double[] oldData;  //上一次的数据
        bool removed = false;
        Color[] colors = { Color.FromArgb(0, 191, 255), Color.FromArgb(0, 139, 139) , Color.FromArgb(0, 206, 209) ,
            Color.FromArgb(127, 255, 212), Color.FromArgb(0, 255, 127), Color.FromArgb(192, 192, 192),
            Color.FromArgb(169, 169, 169), Color.FromArgb(0, 128, 128), Color.FromArgb(148, 0, 211)};
        //Color selected_Color;        
        ColorBlend colorBlend;
        //可以改一下根据当前位置改变大小
    public Form1()
        {
            InitializeComponent();
            init();
        }
        private void init()
        {
            //窗口显示位置初始化
            screenBounds = Screen.GetWorkingArea(this);
            this.ShowInTaskbar = false;
            this.Location = new Point(screenBounds.Width - 1000, screenBounds.Height - 1000);
            //控件初始化
            using(StreamReader reader = new StreamReader("C:\\data.txt"))
            {
                toolStripComboBox1.SelectedIndex = int.Parse(reader.ReadLine());
                toolStripComboBox2.SelectedIndex = int.Parse(reader.ReadLine());
                color.SelectedIndex = int.Parse(reader.ReadLine());
                this.Location = new Point(int.Parse(reader.ReadLine()), int.Parse(reader.ReadLine()));
            }
            //toolStripComboBox1.SelectedIndex = 0;
            //toolStripComboBox2.SelectedItem = 1000;
            is_fixed.SelectedIndex = 0;
            is_top.SelectedIndex = 0;
            //color.SelectedIndex = 0;
            //笔刷初始化
            //selected_Color = color1;
            //colorBlend = new ColorBlend();
            //colorBlend.Positions = new float[] { 0f, 1f };
            //colorBlend.Colors = colors;
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
            RectangleF[] rectangles = finalData
                .Select((v, i) => new RectangleF(i * (this.Width / finalData.Length), (float)0.9 * this.Height - (float)(50 * Math.Sqrt(v)), (float)0.6*(this.Width / finalData.Length), (float)(50 * Math.Sqrt(v))))
                .ToArray();
            //绘图
            Graphics g = e.Graphics;
            Brush brush = new SolidBrush(colors[color.SelectedIndex]);
            g.FillRectangles(brush, rectangles);
            oldData = new double[finalData.Length];
            Array.Copy(finalData, oldData, finalData.Length);
        }

        //窗口可移动方法实现
        protected override void WndProc(ref Message m)
        {
            // 处理WM_NCHITTEST消息，以实现窗口的移动
            if (m.Msg == WM_NCHITTEST && removed)
            {
                base.WndProc(ref m);
                if (m.Result == (IntPtr)HTCLIENT)
                {
                    m.Result = (IntPtr)HTCAPTION;
                }
                return;
            }

            base.WndProc(ref m);
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            // 在鼠标按下时，调用ReleaseCapture和SendMessage方法，移动窗口
            base.OnMouseDown(e);
            if (removed && e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(this.Handle, WM_SYSCOMMAND, SC_MOVE, 0);
            }
        }
        //SelectedIndexChanged事件
        private void is_fixed_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(is_fixed .SelectedIndex == 0)
            {
                removed = false;
            }
            else
            {
                removed = true;
            }
        }

        private void is_top_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (is_top.SelectedIndex == 0) this.TopMost = true;
            else this.TopMost = false;
        }

        private void toolStripComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.Opacity = 1 - toolStripComboBox1.SelectedIndex * 0.25;
        }
        private void toolStripComboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            //1500;1200;1000;800;600;400;200
            int width = 1000;
            if (toolStripComboBox2.SelectedItem != null)
            {
                width = Convert.ToInt32(toolStripComboBox2.SelectedItem);
            }
            int x = this.Location.X + this.Size.Width - width;
            int y = this.Location.Y + this.Size.Height - width;
            this.Location = new Point(x, y);
            this.Size = new Size(width, width);
        }
        //退出
        private void exit_Click(object sender, EventArgs e)
        {
            cap.StopRecording();
            using (StreamWriter writer = new StreamWriter("C:\\data.txt"))
            {
                writer.WriteLine(toolStripComboBox1.SelectedIndex);
                writer.WriteLine(toolStripComboBox2.SelectedIndex);
                writer.WriteLine(color.SelectedIndex);
                writer.WriteLine(this.Location.X);
                writer.WriteLine(this.Location.Y);
            }
            this.Close();
        }
    }
}















//一下为各种版本渐变笔刷尝试
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
             
            using (var brush = new LinearGradientBrush(new PointF(0, 0), new PointF(0, 110), Color.White, Color.White))
            {
                var colorBlend = new ColorBlend();
                colorBlend.Positions = new float[] { 0f, 1f };
                colorBlend.Colors = colors;

                brush.InterpolationColors = colorBlend;

                using (var g = Graphics.FromHwnd(this.Handle))
                {
                    foreach (var rect in rectangles)
                    {
                        g.FillRectangle(brush, rect);
                    }
                    //g.FillRectangles(brush, rectangles);
                }
            }
            */
/*
using (var g = Graphics.FromHwnd(this.Handle))
{

    foreach (var rect in rectangles)
    {
        if(rect.Height > 0)
        {
            //每当音频暂停时将会报错,只能是最后一组数据操作，因为不播放时不会运行该程序
            using (var brush = new LinearGradientBrush(rect, Color.White, Color.White, LinearGradientMode.Vertical))
            {
                brush.InterpolationColors = colorBlend;
                g.FillRectangle(brush, rect);
            }
        }
    }
}
*/