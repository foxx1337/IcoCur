// Decompiled with JetBrains decompiler
// Type: PaintDotNet.Data.CurSaveConfigWidget
// Assembly: IcoCur, Version=4.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: EEB2FED8-625F-4555-8E27-7F881F458B17
// Assembly location: C:\src\cs\Paint.net\IcoCur.dll

using System;
using System.Drawing;
using System.Windows.Forms;

namespace PaintDotNet.Data
{
  internal class CurSaveConfigWidget : SaveConfigWidget
  {
    private Label label1;
    private NumericUpDown XUpDown;
    private Label label2;
    private RadioButton Bit32RBtn;
    private RadioButton Bit8RBtn;
    private NumericUpDown YUpDown;

    public CurSaveConfigWidget() => this.InitializeComponent();

    protected virtual void InitFileType() => this.fileType = (FileType) new PDNIcoCurFormat();

    protected virtual void InitTokenFromWidget()
    {
      ((CurSaveConfigToken) this.Token).HotSpot.X = (int) this.XUpDown.Value;
      ((CurSaveConfigToken) this.Token).HotSpot.Y = (int) this.YUpDown.Value;
      ((CurSaveConfigToken) this.Token).EightBit = this.Bit8RBtn.Checked;
    }

    protected virtual void InitWidgetFromToken(SaveConfigToken token)
    {
      CurSaveConfigToken curSaveConfigToken = (CurSaveConfigToken) token;
      this.XUpDown.Value = (Decimal) curSaveConfigToken.HotSpot.X;
      this.YUpDown.Value = (Decimal) curSaveConfigToken.HotSpot.Y;
      this.Bit8RBtn.Checked = curSaveConfigToken.EightBit;
      this.Bit32RBtn.Checked = !curSaveConfigToken.EightBit;
    }

    private void InitializeComponent()
    {
      this.label1 = new Label();
      this.XUpDown = new NumericUpDown();
      this.label2 = new Label();
      this.YUpDown = new NumericUpDown();
      this.Bit32RBtn = new RadioButton();
      this.Bit8RBtn = new RadioButton();
      this.XUpDown.BeginInit();
      this.YUpDown.BeginInit();
      ((Control) this).SuspendLayout();
      this.label1.AutoSize = true;
      this.label1.Location = new Point(3, 11);
      this.label1.Name = "label1";
      this.label1.Size = new Size(57, 13);
      this.label1.TabIndex = 2;
      this.label1.Text = "Hotspot X:";
      this.XUpDown.Location = new Point(91, 11);
      this.XUpDown.Maximum = new Decimal(new int[4]
      {
        31,
        0,
        0,
        0
      });
      this.XUpDown.Name = "XUpDown";
      this.XUpDown.Size = new Size(56, 20);
      this.XUpDown.TabIndex = 3;
      this.XUpDown.ValueChanged += new EventHandler(this.HotSpotChange);
      this.XUpDown.KeyPress += new KeyPressEventHandler(this.HotSpotKeyPress);
      this.label2.AutoSize = true;
      this.label2.Location = new Point(3, 36);
      this.label2.Name = "label2";
      this.label2.Size = new Size(57, 13);
      this.label2.TabIndex = 4;
      this.label2.Text = "Hotspot Y:";
      this.YUpDown.Location = new Point(91, 37);
      this.YUpDown.Maximum = new Decimal(new int[4]
      {
        31,
        0,
        0,
        0
      });
      this.YUpDown.Name = "YUpDown";
      this.YUpDown.Size = new Size(56, 20);
      this.YUpDown.TabIndex = 5;
      this.YUpDown.ValueChanged += new EventHandler(this.HotSpotChange);
      this.YUpDown.KeyPress += new KeyPressEventHandler(this.HotSpotKeyPress);
      this.Bit32RBtn.AutoSize = true;
      this.Bit32RBtn.Checked = true;
      this.Bit32RBtn.Location = new Point(6, 76);
      this.Bit32RBtn.Name = "Bit32RBtn";
      this.Bit32RBtn.Size = new Size(51, 17);
      this.Bit32RBtn.TabIndex = 6;
      this.Bit32RBtn.TabStop = true;
      this.Bit32RBtn.Text = "32-bit";
      this.Bit32RBtn.UseVisualStyleBackColor = true;
      this.Bit32RBtn.CheckedChanged += new EventHandler(this.DepthCheckChanged);
      this.Bit8RBtn.AutoSize = true;
      this.Bit8RBtn.Location = new Point(91, 76);
      this.Bit8RBtn.Name = "Bit8RBtn";
      this.Bit8RBtn.Size = new Size(45, 17);
      this.Bit8RBtn.TabIndex = 7;
      this.Bit8RBtn.Text = "8-bit";
      this.Bit8RBtn.UseVisualStyleBackColor = true;
      this.Bit8RBtn.CheckedChanged += new EventHandler(this.DepthCheckChanged);
      ((ContainerControl) this).AutoScaleDimensions = new SizeF(96f, 96f);
      ((Control) this).Controls.Add((Control) this.Bit8RBtn);
      ((Control) this).Controls.Add((Control) this.Bit32RBtn);
      ((Control) this).Controls.Add((Control) this.YUpDown);
      ((Control) this).Controls.Add((Control) this.label2);
      ((Control) this).Controls.Add((Control) this.XUpDown);
      ((Control) this).Controls.Add((Control) this.label1);
      ((Control) this).Name = nameof (CurSaveConfigWidget);
      this.XUpDown.EndInit();
      this.YUpDown.EndInit();
      ((Control) this).ResumeLayout(false);
      ((Control) this).PerformLayout();
    }

    private void HotSpotChange(object sender, EventArgs e) => this.UpdateToken();

    private void HotSpotKeyPress(object sender, KeyPressEventArgs e) => this.UpdateToken();

    private void DepthCheckChanged(object sender, EventArgs e) => this.UpdateToken();
  }
}
