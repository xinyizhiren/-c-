

namespace 音频示波器
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.color = new System.Windows.Forms.ToolStripComboBox();
            this.is_top = new System.Windows.Forms.ToolStripComboBox();
            this.is_fixed = new System.Windows.Forms.ToolStripComboBox();
            this.size = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripComboBox2 = new System.Windows.Forms.ToolStripComboBox();
            this.opacity = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripComboBox1 = new System.Windows.Forms.ToolStripComboBox();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.exit = new System.Windows.Forms.ToolStripTextBox();
            //this.Cursor = Cursors.Hand;
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // notifyIcon1
            // 
            this.notifyIcon1.ContextMenuStrip = this.contextMenuStrip1;
            this.notifyIcon1.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon1.Icon")));
            this.notifyIcon1.Text = "音频可视化";
            this.notifyIcon1.Visible = true;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.ImageScalingSize = new System.Drawing.Size(32, 32);
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.is_top,
            this.is_fixed,
            this.size,
            this.color,
            this.opacity,
            this.toolStripSeparator1,
            this.exit});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(301, 313);
            // 
            // color
            // 
            this.color.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.color.Items.AddRange(new object[] {
            "炫酷蓝色",
            "靛青色",
            "青绿色",
            "碧绿色",
            "酸橙色",
            "银色",
            "暗灰色",
            "深青色",
            "紫罗兰色"});
            this.color.Name = "color";
            this.color.Size = new System.Drawing.Size(121, 39);
            // 
            // is_top
            // 
            this.is_top.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.is_top.Items.AddRange(new object[] {
            "Top",
            "Bottom"});
            this.is_top.Name = "is_top";
            this.is_top.Size = new System.Drawing.Size(121, 39);
            this.is_top.SelectedIndexChanged += new System.EventHandler(this.is_top_SelectedIndexChanged);
            // 
            // is_fixed
            // 
            this.is_fixed.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.is_fixed.Items.AddRange(new object[] {
            "固定",
            "不固定"});
            this.is_fixed.Name = "is_fixed";
            this.is_fixed.Size = new System.Drawing.Size(121, 39);
            this.is_fixed.SelectedIndexChanged += new System.EventHandler(this.is_fixed_SelectedIndexChanged);
            // 
            // size
            // 
            this.size.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.size.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripComboBox2});
            this.size.Name = "size";
            this.size.Size = new System.Drawing.Size(300, 38);
            this.size.Text = "大小（宽度）";
            // 
            // toolStripComboBox2
            // 
            this.toolStripComboBox2.Items.AddRange(new object[] {
            1920,
            1680,
            1500,
            1200,
            1000,
            800,
            400});
            this.toolStripComboBox2.Name = "toolStripComboBox2";
            this.toolStripComboBox2.Size = new System.Drawing.Size(121, 39);
            this.toolStripComboBox2.SelectedIndexChanged += new System.EventHandler(this.toolStripComboBox2_SelectedIndexChanged);
            // 
            // opacity
            // 
            this.opacity.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.opacity.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripComboBox1});
            this.opacity.Name = "opacity";
            this.opacity.Size = new System.Drawing.Size(300, 38);
            this.opacity.Text = "透明度";
            // 
            // toolStripComboBox1
            // 
            this.toolStripComboBox1.BackColor = System.Drawing.SystemColors.ScrollBar;
            this.toolStripComboBox1.Items.AddRange(new object[] {
            "0%",
            "25%",
            "50%",
            "75%",
            "100%"});
            this.toolStripComboBox1.Name = "toolStripComboBox1";
            this.toolStripComboBox1.Size = new System.Drawing.Size(121, 39);
            this.toolStripComboBox1.SelectedIndexChanged += new System.EventHandler(this.toolStripComboBox1_SelectedIndexChanged);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(297, 6);
            // 
            // exit
            // 
            this.exit.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.exit.Name = "exit";
            this.exit.ReadOnly = true;
            this.exit.Size = new System.Drawing.Size(100, 38);
            this.exit.Text = "退出";
            this.exit.Control.Cursor = Cursors.Default;
            this.exit.TextBoxTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.exit.Click += new System.EventHandler(this.exit_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(14F, 31F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1000, 1000);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "Form1";
            this.Text = "Form1";
            this.TopMost = true;
            this.TransparencyKey = System.Drawing.SystemColors.Control;
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.Form1_Paint);
            this.contextMenuStrip1.ResumeLayout(false);
            this.contextMenuStrip1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private NotifyIcon notifyIcon1;
        private ContextMenuStrip contextMenuStrip1;
        private ToolStripTextBox exit;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripMenuItem opacity;
        private ToolStripComboBox toolStripComboBox1;
        private ToolStripMenuItem size;
        private ToolStripComboBox toolStripComboBox2;
        private ToolStripComboBox is_fixed;
        private ToolStripComboBox is_top;
        private ToolStripComboBox color;
    }
}