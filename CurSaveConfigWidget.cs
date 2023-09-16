// Decompiled with JetBrains decompiler
// Type: PaintDotNet.Data.CurSaveConfigWidget
// Assembly: IcoCur, Version=4.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: EEB2FED8-625F-4555-8E27-7F881F458B17
// Assembly location: C:\src\cs\Paint.net\IcoCur.dll

using System;
using System.Drawing;
using System.Windows.Forms;
using PaintDotNet;

namespace IcoCur;

internal class CurSaveConfigWidget : SaveConfigWidget
{
    private Label label1;
    private NumericUpDown XUpDown;
    private Label label2;
    private RadioButton Bit32RBtn;
    private RadioButton Bit8RBtn;
    private NumericUpDown YUpDown;

    public CurSaveConfigWidget() => InitializeComponent();

    protected override void InitFileType() => fileType = new PDNIcoCurFormat();

    protected override void InitTokenFromWidget()
    {
        ((CurSaveConfigToken)Token).HotSpot.X = (int)XUpDown.Value;
        ((CurSaveConfigToken)Token).HotSpot.Y = (int)YUpDown.Value;
        ((CurSaveConfigToken)Token).EightBit = Bit8RBtn.Checked;
    }

    protected override void InitWidgetFromToken(SaveConfigToken token)
    {
        CurSaveConfigToken curSaveConfigToken = (CurSaveConfigToken)token;
        XUpDown.Value = curSaveConfigToken.HotSpot.X;
        YUpDown.Value = curSaveConfigToken.HotSpot.Y;
        Bit8RBtn.Checked = curSaveConfigToken.EightBit;
        Bit32RBtn.Checked = !curSaveConfigToken.EightBit;
    }

    private void InitializeComponent()
    {
        label1 = new Label();
        XUpDown = new NumericUpDown();
        label2 = new Label();
        YUpDown = new NumericUpDown();
        Bit32RBtn = new RadioButton();
        Bit8RBtn = new RadioButton();
        XUpDown.BeginInit();
        YUpDown.BeginInit();
        this.SuspendLayout();
        label1.AutoSize = true;
        label1.Location = new Point(3, 11);
        label1.Name = "label1";
        label1.Size = new Size(57, 13);
        label1.TabIndex = 2;
        label1.Text = "Hotspot X:";
        XUpDown.Location = new Point(91, 11);
        XUpDown.Maximum = new decimal(new int[4]
        {
            31,
            0,
            0,
            0
        });
        XUpDown.Name = "XUpDown";
        XUpDown.Size = new Size(56, 20);
        XUpDown.TabIndex = 3;
        XUpDown.ValueChanged += new EventHandler(HotSpotChange);
        XUpDown.KeyPress += new KeyPressEventHandler(HotSpotKeyPress);
        label2.AutoSize = true;
        label2.Location = new Point(3, 36);
        label2.Name = "label2";
        label2.Size = new Size(57, 13);
        label2.TabIndex = 4;
        label2.Text = "Hotspot Y:";
        YUpDown.Location = new Point(91, 37);
        YUpDown.Maximum = new decimal(new int[4]
        {
            31,
            0,
            0,
            0
        });
        YUpDown.Name = "YUpDown";
        YUpDown.Size = new Size(56, 20);
        YUpDown.TabIndex = 5;
        YUpDown.ValueChanged += new EventHandler(HotSpotChange);
        YUpDown.KeyPress += new KeyPressEventHandler(HotSpotKeyPress);
        Bit32RBtn.AutoSize = true;
        Bit32RBtn.Checked = true;
        Bit32RBtn.Location = new Point(6, 76);
        Bit32RBtn.Name = "Bit32RBtn";
        Bit32RBtn.Size = new Size(51, 17);
        Bit32RBtn.TabIndex = 6;
        Bit32RBtn.TabStop = true;
        Bit32RBtn.Text = "32-bit";
        Bit32RBtn.UseVisualStyleBackColor = true;
        Bit32RBtn.CheckedChanged += new EventHandler(DepthCheckChanged);
        Bit8RBtn.AutoSize = true;
        Bit8RBtn.Location = new Point(91, 76);
        Bit8RBtn.Name = "Bit8RBtn";
        Bit8RBtn.Size = new Size(45, 17);
        Bit8RBtn.TabIndex = 7;
        Bit8RBtn.Text = "8-bit";
        Bit8RBtn.UseVisualStyleBackColor = true;
        Bit8RBtn.CheckedChanged += new EventHandler(DepthCheckChanged);
        this.AutoScaleDimensions = new SizeF(96f, 96f);
        this.Controls.Add(Bit8RBtn);
        this.Controls.Add(Bit32RBtn);
        this.Controls.Add(YUpDown);
        this.Controls.Add(label2);
        this.Controls.Add(XUpDown);
        this.Controls.Add(label1);
        this.Name = nameof(CurSaveConfigWidget);
        XUpDown.EndInit();
        YUpDown.EndInit();
        this.ResumeLayout(false);
        this.PerformLayout();
    }

    private void HotSpotChange(object sender, EventArgs e) => UpdateToken();

    private void HotSpotKeyPress(object sender, KeyPressEventArgs e) => UpdateToken();

    private void DepthCheckChanged(object sender, EventArgs e) => UpdateToken();
}
