// Decompiled with JetBrains decompiler
// Type: PaintDotNet.Data.IcoSaveForm
// Assembly: IcoCur, Version=4.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: EEB2FED8-625F-4555-8E27-7F881F458B17
// Assembly location: C:\src\cs\Paint.net\IcoCur.dll

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace PaintDotNet.Data
{
  public class IcoSaveForm : Form
  {
    private Document g_Doc;
    private List<Size> g_Sizes;
    private int[] g_SupportedDims;
    private IContainer components;
    private Button CancelBtn;
    private Button OKBtn;
    private GroupBox groupBox4;
    private Button btnSelectAll;
    private Button SelNoneBtn;
    private CheckedListBox clbFormats;
    private GroupBox groupBox1;
    private RadioButton rbSeparate;
    private RadioButton rbMerged;

    public bool WantMerged => this.rbMerged.Checked;

    public IcoSaveForm(Document doc)
    {
      this.InitializeComponent();
      this.g_SupportedDims = new int[7]
      {
        256,
        128,
        64,
        48,
        32,
        24,
        16
      };
      this.g_Doc = doc;
      this.UpdateList();
    }

    private void btnSelectAll_Click(object sender, EventArgs e)
    {
      for (int index = 0; index < this.clbFormats.Items.Count; ++index)
        this.clbFormats.SetItemChecked(index, true);
    }

    private int ClosestMultipleOf8(int num)
    {
      int num1 = num / 8 * 8;
      int num2 = num1 + 8;
      return num2 - num >= num - num1 ? num1 : num2;
    }

    internal List<IcoCurSaveFormat> GetSaveFormats()
    {
      List<IcoCurSaveFormat> saveFormats = new List<IcoCurSaveFormat>();
      for (int index = 0; index < this.clbFormats.Items.Count; ++index)
      {
        if (this.clbFormats.GetItemChecked(index))
        {
          IcoCurSaveFormat icoCurSaveFormat = new IcoCurSaveFormat(this.g_Sizes[index].Width, this.g_Sizes[index].Height, this.clbFormats.Items[index].ToString().Contains("8-bit"));
          saveFormats.Add(icoCurSaveFormat);
        }
      }
      return saveFormats;
    }

    private bool IsSupportedDim(int w, int h)
    {
      bool flag;
      if (w != h)
      {
        flag = false;
      }
      else
      {
        for (int index = 0; index < this.g_SupportedDims.Length; ++index)
        {
          if (w == this.g_SupportedDims[index])
            return true;
        }
        flag = false;
      }
      return flag;
    }

    private void ModeCheckChanged(object sender, EventArgs e) => this.clbFormats.Enabled = this.rbMerged.Checked;

    private void SelNoneBtn_Click(object sender, EventArgs e)
    {
      for (int index = 0; index < this.clbFormats.Items.Count; ++index)
        this.clbFormats.SetItemChecked(index, false);
    }

    private void UpdateList()
    {
      this.clbFormats.Items.Clear();
      if (this.g_Doc == null)
        return;
      int width = this.g_Doc.Width;
      int height = this.g_Doc.Height;
      if (this.g_Doc.Width <= 256 && this.g_Doc.Height <= 256 && !this.IsSupportedDim(this.g_Doc.Width, this.g_Doc.Height))
      {
        this.g_Sizes = new List<Size>(14);
        string str1 = Convert.ToString(width) + "x" + Convert.ToString(height) + ", 32-bit";
        string str2 = Convert.ToString(width) + "x" + Convert.ToString(height) + ", 8-bit";
        this.clbFormats.Items.Add((object) str1, true);
        this.clbFormats.Items.Add((object) str2, true);
        this.g_Sizes.Add(new Size(width, height));
        this.g_Sizes.Add(new Size(width, height));
      }
      else
        this.g_Sizes = new List<Size>(12);
      this.clbFormats.Items.Add((object) "256x256, PNG", false);
      this.clbFormats.Items.Add((object) "128x128, 32-bit", false);
      this.clbFormats.Items.Add((object) "128x128, 8-bit", false);
      this.clbFormats.Items.Add((object) "64x64, 32-bit", false);
      this.clbFormats.Items.Add((object) "64x64, 8-bit", false);
      this.clbFormats.Items.Add((object) "48x48, 32-bit", false);
      this.clbFormats.Items.Add((object) "48x48, 8-bit", false);
      this.clbFormats.Items.Add((object) "32x32, 32-bit", true);
      this.clbFormats.Items.Add((object) "32x32, 8-bit", true);
      this.clbFormats.Items.Add((object) "24x24, 32-bit", false);
      this.clbFormats.Items.Add((object) "24x24, 8-bit", false);
      this.clbFormats.Items.Add((object) "16x16, 32-bit", true);
      this.clbFormats.Items.Add((object) "16x16, 8-bit", true);
      this.g_Sizes.Add(new Size(256, 256));
      this.g_Sizes.Add(new Size(128, 128));
      this.g_Sizes.Add(new Size(128, 128));
      this.g_Sizes.Add(new Size(64, 64));
      this.g_Sizes.Add(new Size(64, 64));
      this.g_Sizes.Add(new Size(48, 48));
      this.g_Sizes.Add(new Size(48, 48));
      this.g_Sizes.Add(new Size(32, 32));
      this.g_Sizes.Add(new Size(32, 32));
      this.g_Sizes.Add(new Size(24, 24));
      this.g_Sizes.Add(new Size(24, 24));
      this.g_Sizes.Add(new Size(16, 16));
      this.g_Sizes.Add(new Size(16, 16));
    }

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
      this.CancelBtn = new Button();
      this.OKBtn = new Button();
      this.groupBox4 = new GroupBox();
      this.clbFormats = new CheckedListBox();
      this.btnSelectAll = new Button();
      this.SelNoneBtn = new Button();
      this.groupBox1 = new GroupBox();
      this.rbSeparate = new RadioButton();
      this.rbMerged = new RadioButton();
      this.groupBox4.SuspendLayout();
      this.groupBox1.SuspendLayout();
      this.SuspendLayout();
      this.CancelBtn.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.CancelBtn.DialogResult = DialogResult.Cancel;
      this.CancelBtn.Location = new Point(207, 309);
      this.CancelBtn.Name = "CancelBtn";
      this.CancelBtn.Size = new Size(75, 23);
      this.CancelBtn.TabIndex = 4;
      this.CancelBtn.Text = "Cancel";
      this.CancelBtn.UseVisualStyleBackColor = true;
      this.OKBtn.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.OKBtn.DialogResult = DialogResult.OK;
      this.OKBtn.Location = new Point(126, 309);
      this.OKBtn.Name = "OKBtn";
      this.OKBtn.Size = new Size(75, 23);
      this.OKBtn.TabIndex = 3;
      this.OKBtn.Text = "OK";
      this.OKBtn.UseVisualStyleBackColor = true;
      this.groupBox4.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.groupBox4.Controls.Add((Control) this.clbFormats);
      this.groupBox4.Controls.Add((Control) this.btnSelectAll);
      this.groupBox4.Controls.Add((Control) this.SelNoneBtn);
      this.groupBox4.Location = new Point(12, 141);
      this.groupBox4.Name = "groupBox4";
      this.groupBox4.Size = new Size(270, 162);
      this.groupBox4.TabIndex = 1;
      this.groupBox4.TabStop = false;
      this.groupBox4.Text = "Copies to be saved";
      this.clbFormats.CheckOnClick = true;
      this.clbFormats.FormattingEnabled = true;
      this.clbFormats.Items.AddRange(new object[2]
      {
        (object) "32x32, 32-bit",
        (object) "32x32, 8-bit"
      });
      this.clbFormats.Location = new Point(6, 19);
      this.clbFormats.Name = "clbFormats";
      this.clbFormats.Size = new Size(258, 109);
      this.clbFormats.TabIndex = 3;
      this.btnSelectAll.Location = new Point(6, 134);
      this.btnSelectAll.Name = "btnSelectAll";
      this.btnSelectAll.Size = new Size(75, 23);
      this.btnSelectAll.TabIndex = 2;
      this.btnSelectAll.Text = "Select All";
      this.btnSelectAll.UseVisualStyleBackColor = true;
      this.btnSelectAll.Click += new EventHandler(this.btnSelectAll_Click);
      this.SelNoneBtn.Location = new Point(189, 134);
      this.SelNoneBtn.Name = "SelNoneBtn";
      this.SelNoneBtn.Size = new Size(75, 23);
      this.SelNoneBtn.TabIndex = 1;
      this.SelNoneBtn.Text = "Select None";
      this.SelNoneBtn.UseVisualStyleBackColor = true;
      this.SelNoneBtn.Click += new EventHandler(this.SelNoneBtn_Click);
      this.groupBox1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.groupBox1.Controls.Add((Control) this.rbSeparate);
      this.groupBox1.Controls.Add((Control) this.rbMerged);
      this.groupBox1.Location = new Point(12, 12);
      this.groupBox1.Name = "groupBox1";
      this.groupBox1.Size = new Size(270, 123);
      this.groupBox1.TabIndex = 5;
      this.groupBox1.TabStop = false;
      this.groupBox1.Text = "Save Mode";
      this.rbSeparate.AutoSize = true;
      this.rbSeparate.Location = new Point(16, 55);
      this.rbSeparate.Name = "rbSeparate";
      this.rbSeparate.Size = new Size(251, 56);
      this.rbSeparate.TabIndex = 1;
      this.rbSeparate.Text = "Each layer as an image within the icon file.\r\nLayer names will be used to determine cropping \r\nwidths and must be in the form #x#. Examples:\r\n32x32, 64x64, etc.";
      this.rbSeparate.UseVisualStyleBackColor = true;
      this.rbSeparate.CheckedChanged += new EventHandler(this.ModeCheckChanged);
      this.rbMerged.AutoSize = true;
      this.rbMerged.Checked = true;
      this.rbMerged.Location = new Point(16, 19);
      this.rbMerged.Name = "rbMerged";
      this.rbMerged.Size = new Size(225, 30);
      this.rbMerged.TabIndex = 0;
      this.rbMerged.TabStop = true;
      this.rbMerged.Text = "Merged image (multiple, different resolution\r\nimage copies within the icon file)";
      this.rbMerged.UseVisualStyleBackColor = true;
      this.rbMerged.CheckedChanged += new EventHandler(this.ModeCheckChanged);
      this.AcceptButton = (IButtonControl) this.OKBtn;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.CancelButton = (IButtonControl) this.CancelBtn;
      this.ClientSize = new Size(292, 342);
      this.Controls.Add((Control) this.groupBox1);
      this.Controls.Add((Control) this.groupBox4);
      this.Controls.Add((Control) this.OKBtn);
      this.Controls.Add((Control) this.CancelBtn);
      this.FormBorderStyle = FormBorderStyle.FixedToolWindow;
      this.Name = nameof (IcoSaveForm);
      this.Text = "Icon Save Options";
      this.groupBox4.ResumeLayout(false);
      this.groupBox1.ResumeLayout(false);
      this.groupBox1.PerformLayout();
      this.Shown += new EventHandler(this.Form_Shown);
      this.ResumeLayout(false);
    }
  }
}
