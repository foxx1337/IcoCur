// Decompiled with JetBrains decompiler
// Type: PaintDotNet.ANISaveOptForm
// Assembly: IcoCur, Version=4.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: EEB2FED8-625F-4555-8E27-7F881F458B17
// Assembly location: C:\src\cs\Paint.net\IcoCur.dll

using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace PaintDotNet
{
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

    public ANISaveOptForm() => this.InitializeComponent();

    public void InitFromDocument(Document doc)
    {
      int count = ((ArrayList) doc.Layers).Count;
      this.AnimBMs = new Bitmap[count];
      for (int index = 0; index < count; ++index)
        this.AnimBMs[index] = EOPDNUtility.GetBitmapLayerResized(doc, index, 32, 32);
      this.AnimIndex = 0;
      this.AnimTimer.Enabled = true;
      this.FPSCombo.SelectedIndex = 5;
    }

    private void AnimTimer_Tick(object sender, EventArgs e)
    {
      this.AnimPictureBox.Image = (Image) this.AnimBMs[this.AnimIndex];
      ++this.AnimIndex;
      if (this.AnimIndex < this.AnimBMs.Length)
        return;
      this.AnimIndex = 0;
    }

    private void FPSCombo_SelectedIndexChanged(object sender, EventArgs e) => this.AnimTimer.Interval = 1000 / Convert.ToInt32(this.FPSCombo.Text);

    public uint GetAnimDelay() => 60U / Convert.ToUInt32(this.FPSCombo.Text);

    public Point GetHotSpot() => new Point((int) this.XUpDown.Value, (int) this.YUpDown.Value);

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void Form_Shown(object sender, EventArgs e)
    {
      this.TopMost = true;
      this.Focus();
      this.BringToFront();
    }

    private void InitializeComponent()
    {
      this.components = (IContainer) new System.ComponentModel.Container();
      this.YUpDown = new NumericUpDown();
      this.label2 = new Label();
      this.XUpDown = new NumericUpDown();
      this.label1 = new Label();
      this.panel1 = new Panel();
      this.AnimPictureBox = new PictureBox();
      this.label3 = new Label();
      this.CancelBtn = new Button();
      this.OKBtn = new Button();
      this.FPSCombo = new ComboBox();
      this.AnimTimer = new Timer(this.components);
      this.label4 = new Label();
      this.YUpDown.BeginInit();
      this.XUpDown.BeginInit();
      this.panel1.SuspendLayout();
      ((ISupportInitialize) this.AnimPictureBox).BeginInit();
      this.SuspendLayout();
      this.YUpDown.Location = new Point(87, 35);
      this.YUpDown.Maximum = new Decimal(new int[4]
      {
        31,
        0,
        0,
        0
      });
      this.YUpDown.Name = "YUpDown";
      this.YUpDown.Size = new Size(56, 20);
      this.YUpDown.TabIndex = 9;
      this.label2.AutoSize = true;
      this.label2.Location = new Point(12, 35);
      this.label2.Name = "label2";
      this.label2.Size = new Size(57, 13);
      this.label2.TabIndex = 8;
      this.label2.Text = "Hotspot Y:";
      this.XUpDown.Location = new Point(87, 9);
      this.XUpDown.Maximum = new Decimal(new int[4]
      {
        31,
        0,
        0,
        0
      });
      this.XUpDown.Name = "XUpDown";
      this.XUpDown.Size = new Size(56, 20);
      this.XUpDown.TabIndex = 7;
      this.label1.AutoSize = true;
      this.label1.Location = new Point(12, 9);
      this.label1.Name = "label1";
      this.label1.Size = new Size(57, 13);
      this.label1.TabIndex = 6;
      this.label1.Text = "Hotspot X:";
      this.panel1.BorderStyle = BorderStyle.FixedSingle;
      this.panel1.Controls.Add((Control) this.AnimPictureBox);
      this.panel1.Location = new Point(172, 35);
      this.panel1.Name = "panel1";
      this.panel1.Size = new Size(36, 36);
      this.panel1.TabIndex = 10;
      this.AnimPictureBox.Location = new Point(1, 1);
      this.AnimPictureBox.Name = "AnimPictureBox";
      this.AnimPictureBox.Size = new Size(32, 32);
      this.AnimPictureBox.TabIndex = 0;
      this.AnimPictureBox.TabStop = false;
      this.label3.AutoSize = true;
      this.label3.Location = new Point(12, 61);
      this.label3.Name = "label3";
      this.label3.Size = new Size(64, 13);
      this.label3.TabIndex = 11;
      this.label3.Text = "Speed (fps):";
      this.CancelBtn.DialogResult = DialogResult.Cancel;
      this.CancelBtn.Location = new Point(141, 102);
      this.CancelBtn.Name = "CancelBtn";
      this.CancelBtn.Size = new Size(75, 23);
      this.CancelBtn.TabIndex = 13;
      this.CancelBtn.Text = "Cancel";
      this.CancelBtn.UseVisualStyleBackColor = true;
      this.OKBtn.DialogResult = DialogResult.OK;
      this.OKBtn.Location = new Point(60, 102);
      this.OKBtn.Name = "OKBtn";
      this.OKBtn.Size = new Size(75, 23);
      this.OKBtn.TabIndex = 14;
      this.OKBtn.Text = "OK";
      this.OKBtn.UseVisualStyleBackColor = true;
      this.FPSCombo.DropDownStyle = ComboBoxStyle.DropDownList;
      this.FPSCombo.FormattingEnabled = true;
      this.FPSCombo.Items.AddRange(new object[9]
      {
        (object) "1",
        (object) "2",
        (object) "3",
        (object) "4",
        (object) "5",
        (object) "10",
        (object) "20",
        (object) "30",
        (object) "60"
      });
      this.FPSCombo.Location = new Point(87, 63);
      this.FPSCombo.Name = "FPSCombo";
      this.FPSCombo.Size = new Size(56, 21);
      this.FPSCombo.TabIndex = 15;
      this.FPSCombo.SelectedIndexChanged += new EventHandler(this.FPSCombo_SelectedIndexChanged);
      this.AnimTimer.Tick += new EventHandler(this.AnimTimer_Tick);
      this.label4.AutoSize = true;
      this.label4.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Underline, GraphicsUnit.Point, (byte) 0);
      this.label4.Location = new Point(169, 11);
      this.label4.Name = "label4";
      this.label4.Size = new Size(45, 13);
      this.label4.TabIndex = 16;
      this.label4.Text = "Preview";
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(228, 132);
      this.Controls.Add((Control) this.label4);
      this.Controls.Add((Control) this.FPSCombo);
      this.Controls.Add((Control) this.OKBtn);
      this.Controls.Add((Control) this.CancelBtn);
      this.Controls.Add((Control) this.label3);
      this.Controls.Add((Control) this.panel1);
      this.Controls.Add((Control) this.YUpDown);
      this.Controls.Add((Control) this.label2);
      this.Controls.Add((Control) this.XUpDown);
      this.Controls.Add((Control) this.label1);
      this.FormBorderStyle = FormBorderStyle.FixedToolWindow;
      this.Name = nameof (ANISaveOptForm);
      this.Text = "Animated Cursor Options";
      this.YUpDown.EndInit();
      this.XUpDown.EndInit();
      this.panel1.ResumeLayout(false);
      ((ISupportInitialize) this.AnimPictureBox).EndInit();
      this.Shown += new EventHandler(this.Form_Shown);
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
