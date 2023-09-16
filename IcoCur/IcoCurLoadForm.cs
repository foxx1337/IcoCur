// Decompiled with JetBrains decompiler
// Type: IcoCur.IcoCurLoadForm
// Assembly: IcoCur, Version=4.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: EEB2FED8-625F-4555-8E27-7F881F458B17
// Assembly location: C:\src\cs\Paint.net\IcoCur.dll

using PaintDotNet;
using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace IcoCur
{
  internal class IcoCurLoadForm : Form
  {
    private IContainer components;
    private GroupBox RadioItemsGBox;
    private RadioButton LoadAllRBtn;
    private RadioButton LoadOneRBtn;
    private PictureBox pictureBox1;
    private Button CancelBtn;
    private ListBox listBox1;
    private Button OKBtn;
    private uint LargestHeight;
    private uint LargestWidth;
    private EOIcoCurLoader ldr;

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.RadioItemsGBox = new GroupBox();
      this.LoadAllRBtn = new RadioButton();
      this.LoadOneRBtn = new RadioButton();
      this.pictureBox1 = new PictureBox();
      this.CancelBtn = new Button();
      this.listBox1 = new ListBox();
      this.OKBtn = new Button();
      this.RadioItemsGBox.SuspendLayout();
      ((ISupportInitialize) this.pictureBox1).BeginInit();
      this.SuspendLayout();
      this.RadioItemsGBox.Controls.Add((Control) this.LoadAllRBtn);
      this.RadioItemsGBox.Controls.Add((Control) this.LoadOneRBtn);
      this.RadioItemsGBox.Location = new Point(12, 12);
      this.RadioItemsGBox.Name = "RadioItemsGBox";
      this.RadioItemsGBox.Size = new Size(200, 70);
      this.RadioItemsGBox.TabIndex = 0;
      this.RadioItemsGBox.TabStop = false;
      this.RadioItemsGBox.Text = "There are (x) images available";
      this.LoadAllRBtn.AutoSize = true;
      this.LoadAllRBtn.Location = new Point(16, 42);
      this.LoadAllRBtn.Name = "LoadAllRBtn";
      this.LoadAllRBtn.Size = new Size(143, 17);
      this.LoadAllRBtn.TabIndex = 1;
      this.LoadAllRBtn.TabStop = true;
      this.LoadAllRBtn.Text = "Load all available images";
      this.LoadAllRBtn.UseVisualStyleBackColor = true;
      this.LoadAllRBtn.CheckedChanged += new EventHandler(this.LoadAllRBtn_CheckedChanged);
      this.LoadOneRBtn.AutoSize = true;
      this.LoadOneRBtn.Checked = true;
      this.LoadOneRBtn.Location = new Point(16, 19);
      this.LoadOneRBtn.Name = "LoadOneRBtn";
      this.LoadOneRBtn.Size = new Size(145, 17);
      this.LoadOneRBtn.TabIndex = 0;
      this.LoadOneRBtn.TabStop = true;
      this.LoadOneRBtn.Text = "Load only selected image";
      this.LoadOneRBtn.UseVisualStyleBackColor = true;
      this.LoadOneRBtn.CheckedChanged += new EventHandler(this.LoadOneRBtn_CheckedChanged);
      this.pictureBox1.BorderStyle = BorderStyle.FixedSingle;
      this.pictureBox1.Location = new Point(218, 12);
      this.pictureBox1.Name = "pictureBox1";
      this.pictureBox1.Size = new Size(256, 256);
      this.pictureBox1.TabIndex = 1;
      this.pictureBox1.TabStop = false;
      this.CancelBtn.DialogResult = DialogResult.Cancel;
      this.CancelBtn.Location = new Point(399, 274);
      this.CancelBtn.Name = "CancelBtn";
      this.CancelBtn.Size = new Size(75, 23);
      this.CancelBtn.TabIndex = 2;
      this.CancelBtn.Text = "Cancel";
      this.CancelBtn.UseVisualStyleBackColor = true;
      this.listBox1.FormattingEnabled = true;
      this.listBox1.Location = new Point(12, 95);
      this.listBox1.Name = "listBox1";
      this.listBox1.Size = new Size(200, 173);
      this.listBox1.TabIndex = 3;
      this.listBox1.SelectedIndexChanged += new EventHandler(this.listBox1_SelectedIndexChanged);
      this.OKBtn.DialogResult = DialogResult.OK;
      this.OKBtn.Location = new Point(318, 274);
      this.OKBtn.Name = "OKBtn";
      this.OKBtn.Size = new Size(75, 23);
      this.OKBtn.TabIndex = 4;
      this.OKBtn.Text = "OK";
      this.OKBtn.UseVisualStyleBackColor = true;
      this.AcceptButton = (IButtonControl) this.OKBtn;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.CancelButton = (IButtonControl) this.CancelBtn;
      this.ClientSize = new Size(485, 307);
      this.Controls.Add((Control) this.OKBtn);
      this.Controls.Add((Control) this.listBox1);
      this.Controls.Add((Control) this.CancelBtn);
      this.Controls.Add((Control) this.pictureBox1);
      this.Controls.Add((Control) this.RadioItemsGBox);
      this.FormBorderStyle = FormBorderStyle.FixedToolWindow;
      this.Name = nameof (IcoCurLoadForm);
      this.Text = "Icon/Cursor Load Options";
      this.Shown += new EventHandler(this.IcoCurLoadForm_Shown);
      this.RadioItemsGBox.ResumeLayout(false);
      this.RadioItemsGBox.PerformLayout();
      ((ISupportInitialize) this.pictureBox1).EndInit();
      this.ResumeLayout(false);
    }

    public IcoCurLoadForm(EOIcoCurLoader loader, uint count)
    {
      this.InitializeComponent();
      this.ldr = loader;
      this.SetImageCount(count);
      for (uint ImageIndex = 0; ImageIndex < count; ++ImageIndex)
      {
        uint out_Width = 0;
        uint out_Height = 0;
        uint out_bpp = 0;
        if (!loader.GetImageDimensions(ImageIndex, ref out_Width, ref out_Height, ref out_bpp))
        {
          int num = (int) MessageBox.Show(loader.ErrorMsg);
        }
        if (out_Width > this.LargestWidth)
          this.LargestWidth = out_Width;
        if (out_Height > this.LargestHeight)
          this.LargestHeight = out_Height;
        this.AddImageToList(out_Width, out_Height, out_bpp, 0U);
      }
    }

    public void AddImageToList(uint width, uint height, uint bpp, uint compression)
    {
      if (compression != 0U)
        this.listBox1.Items.Add((object) (Convert.ToString(width) + "x" + Convert.ToString(height) + ", (compressed)"));
      else
        this.listBox1.Items.Add((object) (Convert.ToString(width) + "x" + Convert.ToString(height) + ", " + Convert.ToString(bpp) + "-bit"));
    }

    public Document BuildDocument()
    {
      Document document1;
      if (this.LoadOneRBtn.Checked && this.listBox1.SelectedIndex == -1)
      {
        document1 = (Document) null;
      }
      else
      {
        uint selectedIndex = (uint) this.listBox1.SelectedIndex;
        uint out_Width = 0;
        uint out_Height = 0;
        uint out_bpp = 0;
        this.ldr.GetImageDimensions(selectedIndex, ref out_Width, ref out_Height, ref out_bpp);
        Document document2 = !this.LoadOneRBtn.Checked ? new Document((int) this.LargestWidth, (int) this.LargestHeight) : new Document((int) out_Width, (int) out_Height);
        if (document2 == null)
        {
          int num = (int) MessageBox.Show("Could not build a PDN document from the image data.", "Icon/Cursor Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
          document1 = (Document) null;
        }
        else if (this.LoadOneRBtn.Checked)
        {
          Bitmap image = this.ldr.GetImage(selectedIndex);
          image.RotateFlip(RotateFlipType.Rotate180FlipX);
          Surface surface = Surface.CopyFromBitmap(image);
          image.Dispose();
          BitmapLayer bitmapLayer = new BitmapLayer(surface);
          ((Layer) bitmapLayer).Name = "Background";
          ((ArrayList) document2.Layers).Add((object) bitmapLayer);
          document1 = document2;
        }
        else
        {
          for (int ImageIndex = 0; ImageIndex < this.listBox1.Items.Count; ++ImageIndex)
          {
            Bitmap image = this.ldr.GetImage((uint) ImageIndex);
            uint width = (uint) image.Width;
            uint height = (uint) image.Height;
            image.RotateFlip(RotateFlipType.Rotate180FlipX);
            Bitmap bitmap = EvanBitmap.ResizeCropPad(image, (int) this.LargestWidth, (int) this.LargestHeight);
            image.Dispose();
            Surface surface = Surface.CopyFromBitmap(bitmap);
            bitmap.Dispose();
            BitmapLayer bitmapLayer = new BitmapLayer(surface);
            ((Layer) bitmapLayer).Name = string.Format("{0}x{1}", (object) width, (object) height);
            ((ArrayList) document2.Layers).Add((object) bitmapLayer);
          }
          document1 = document2;
        }
      }
      return document1;
    }

    private void IcoCurLoadForm_Shown(object sender, EventArgs e)
    {
      this.listBox1.SelectedIndex = 0;
      this.TopMost = true;
      this.Focus();
      this.BringToFront();
    }

    private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
    {
      Bitmap image = this.ldr.GetImage((uint) this.listBox1.SelectedIndex);
      if (image == null)
        return;
      image.RotateFlip(RotateFlipType.Rotate180FlipX);
      this.pictureBox1.Image = (Image) image;
    }

    public void SetImageCount(uint count) => this.RadioItemsGBox.Text = "There are " + Convert.ToString(count) + " images available";

    private void LoadAllRBtn_CheckedChanged(object sender, EventArgs e)
    {
      this.listBox1.SelectedIndex = -1;
      this.listBox1.Enabled = false;
    }

    private void LoadOneRBtn_CheckedChanged(object sender, EventArgs e)
    {
      this.listBox1.SelectedIndex = 0;
      this.listBox1.Enabled = true;
    }
  }
}
