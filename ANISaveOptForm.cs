// Decompiled with JetBrains decompiler
// Type: PaintDotNet.ANISaveOptForm
// Assembly: IcoCur, Version=4.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: EEB2FED8-625F-4555-8E27-7F881F458B17
// Assembly location: C:\src\cs\Paint.net\IcoCur.dll

using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using IcoCur.EvanOlds;
using PaintDotNet;

namespace IcoCur;

public class ANISaveOptForm : Form
{
    private Bitmap[] AnimBMs;
    private int AnimIndex;
    private IContainer components;
    private NumericUpDown YUpDown;
    private Label label2;
    private NumericUpDown XUpDown;
    private Label label1;
    private Panel panel1;
    private Label label3;
    private Button CancelBtn;
    private Button OKBtn;
    private ComboBox FPSCombo;
    private Timer AnimTimer;
    private PictureBox AnimPictureBox;
    private Label label4;

    public ANISaveOptForm() => InitializeComponent();

    public void InitFromDocument(Document doc)
    {
        int count = doc.Layers.Count;
        AnimBMs = new Bitmap[count];
        for (int index = 0; index < count; ++index)
        {
            AnimBMs[index] = EOPDNUtility.GetBitmapLayerResized(doc, index, 32, 32);
        }
        AnimIndex = 0;
        AnimTimer.Enabled = true;
        FPSCombo.SelectedIndex = 5;
    }

    private void AnimTimer_Tick(object sender, EventArgs e)
    {
        AnimPictureBox.Image = AnimBMs[AnimIndex];
        ++AnimIndex;
        if (AnimIndex < AnimBMs.Length)
        {
            return;
        }
        AnimIndex = 0;
    }

    private void FPSCombo_SelectedIndexChanged(object sender, EventArgs e) => AnimTimer.Interval = 1000 / Convert.ToInt32(FPSCombo.Text);

    public uint GetAnimDelay() => 60U / Convert.ToUInt32(FPSCombo.Text);

    public Point GetHotSpot() => new Point((int)XUpDown.Value, (int)YUpDown.Value);

    protected override void Dispose(bool disposing)
    {
        if (disposing && components != null)
        {
            components.Dispose();
        }
        base.Dispose(disposing);
    }

    private void Form_Shown(object sender, EventArgs e)
    {
        TopMost = true;
        Focus();
        BringToFront();
    }

    private void InitializeComponent()
    {
        components = new Container();
        YUpDown = new NumericUpDown();
        label2 = new Label();
        XUpDown = new NumericUpDown();
        label1 = new Label();
        panel1 = new Panel();
        AnimPictureBox = new PictureBox();
        label3 = new Label();
        CancelBtn = new Button();
        OKBtn = new Button();
        FPSCombo = new ComboBox();
        AnimTimer = new Timer(components);
        label4 = new Label();
        YUpDown.BeginInit();
        XUpDown.BeginInit();
        panel1.SuspendLayout();
        ((ISupportInitialize)AnimPictureBox).BeginInit();
        SuspendLayout();
        YUpDown.Location = new Point(87, 35);
        YUpDown.Maximum = new decimal(new int[4]
        {
            31,
            0,
            0,
            0
        });
        YUpDown.Name = "YUpDown";
        YUpDown.Size = new Size(56, 20);
        YUpDown.TabIndex = 9;
        label2.AutoSize = true;
        label2.Location = new Point(12, 35);
        label2.Name = "label2";
        label2.Size = new Size(57, 13);
        label2.TabIndex = 8;
        label2.Text = "Hotspot Y:";
        XUpDown.Location = new Point(87, 9);
        XUpDown.Maximum = new decimal(new int[4]
        {
            31,
            0,
            0,
            0
        });
        XUpDown.Name = "XUpDown";
        XUpDown.Size = new Size(56, 20);
        XUpDown.TabIndex = 7;
        label1.AutoSize = true;
        label1.Location = new Point(12, 9);
        label1.Name = "label1";
        label1.Size = new Size(57, 13);
        label1.TabIndex = 6;
        label1.Text = "Hotspot X:";
        panel1.BorderStyle = BorderStyle.FixedSingle;
        panel1.Controls.Add(AnimPictureBox);
        panel1.Location = new Point(172, 35);
        panel1.Name = "panel1";
        panel1.Size = new Size(36, 36);
        panel1.TabIndex = 10;
        AnimPictureBox.Location = new Point(1, 1);
        AnimPictureBox.Name = "AnimPictureBox";
        AnimPictureBox.Size = new Size(32, 32);
        AnimPictureBox.TabIndex = 0;
        AnimPictureBox.TabStop = false;
        label3.AutoSize = true;
        label3.Location = new Point(12, 61);
        label3.Name = "label3";
        label3.Size = new Size(64, 13);
        label3.TabIndex = 11;
        label3.Text = "Speed (fps):";
        CancelBtn.DialogResult = DialogResult.Cancel;
        CancelBtn.Location = new Point(141, 102);
        CancelBtn.Name = "CancelBtn";
        CancelBtn.Size = new Size(75, 23);
        CancelBtn.TabIndex = 13;
        CancelBtn.Text = "Cancel";
        CancelBtn.UseVisualStyleBackColor = true;
        OKBtn.DialogResult = DialogResult.OK;
        OKBtn.Location = new Point(60, 102);
        OKBtn.Name = "OKBtn";
        OKBtn.Size = new Size(75, 23);
        OKBtn.TabIndex = 14;
        OKBtn.Text = "OK";
        OKBtn.UseVisualStyleBackColor = true;
        FPSCombo.DropDownStyle = ComboBoxStyle.DropDownList;
        FPSCombo.FormattingEnabled = true;
        FPSCombo.Items.AddRange(new object[9]
        {
            "1",
            "2",
            "3",
            "4",
            "5",
            "10",
            "20",
            "30",
            "60"
        });
        FPSCombo.Location = new Point(87, 63);
        FPSCombo.Name = "FPSCombo";
        FPSCombo.Size = new Size(56, 21);
        FPSCombo.TabIndex = 15;
        FPSCombo.SelectedIndexChanged += new EventHandler(FPSCombo_SelectedIndexChanged);
        AnimTimer.Tick += new EventHandler(AnimTimer_Tick);
        label4.AutoSize = true;
        label4.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Underline, GraphicsUnit.Point, 0);
        label4.Location = new Point(169, 11);
        label4.Name = "label4";
        label4.Size = new Size(45, 13);
        label4.TabIndex = 16;
        label4.Text = "Preview";
        AutoScaleDimensions = new SizeF(6f, 13f);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(228, 132);
        Controls.Add(label4);
        Controls.Add(FPSCombo);
        Controls.Add(OKBtn);
        Controls.Add(CancelBtn);
        Controls.Add(label3);
        Controls.Add(panel1);
        Controls.Add(YUpDown);
        Controls.Add(label2);
        Controls.Add(XUpDown);
        Controls.Add(label1);
        FormBorderStyle = FormBorderStyle.FixedToolWindow;
        Name = nameof(ANISaveOptForm);
        Text = "Animated Cursor Options";
        YUpDown.EndInit();
        XUpDown.EndInit();
        panel1.ResumeLayout(false);
        ((ISupportInitialize)AnimPictureBox).EndInit();
        Shown += new EventHandler(Form_Shown);
        ResumeLayout(false);
        PerformLayout();
    }
}
