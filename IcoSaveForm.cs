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
using PaintDotNet;

namespace IcoCur;

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

    public bool WantMerged => rbMerged.Checked;

    public IcoSaveForm(Document doc)
    {
        InitializeComponent();
        g_SupportedDims = new int[7]
        {
            256,
            128,
            64,
            48,
            32,
            24,
            16
        };
        g_Doc = doc;
        UpdateList();
    }

    private void btnSelectAll_Click(object sender, EventArgs e)
    {
        for (int index = 0; index < clbFormats.Items.Count; ++index)
        {
            clbFormats.SetItemChecked(index, true);
        }
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
        for (int index = 0; index < clbFormats.Items.Count; ++index)
        {
            if (clbFormats.GetItemChecked(index))
            {
                IcoCurSaveFormat icoCurSaveFormat = new IcoCurSaveFormat(g_Sizes[index].Width, g_Sizes[index].Height, clbFormats.Items[index].ToString().Contains("8-bit"));
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
            for (int index = 0; index < g_SupportedDims.Length; ++index)
            {
                if (w == g_SupportedDims[index])
                {
                    return true;
                }
            }
            flag = false;
        }
        return flag;
    }

    private void ModeCheckChanged(object sender, EventArgs e) => clbFormats.Enabled = rbMerged.Checked;

    private void SelNoneBtn_Click(object sender, EventArgs e)
    {
        for (int index = 0; index < clbFormats.Items.Count; ++index)
        {
            clbFormats.SetItemChecked(index, false);
        }
    }

    private void UpdateList()
    {
        clbFormats.Items.Clear();
        if (g_Doc == null)
        {
            return;
        }
        int width = g_Doc.Width;
        int height = g_Doc.Height;
        if (g_Doc.Width <= 256 && g_Doc.Height <= 256 && !IsSupportedDim(g_Doc.Width, g_Doc.Height))
        {
            g_Sizes = new List<Size>(14);
            string str1 = Convert.ToString(width) + "x" + Convert.ToString(height) + ", 32-bit";
            string str2 = Convert.ToString(width) + "x" + Convert.ToString(height) + ", 8-bit";
            clbFormats.Items.Add(str1, true);
            clbFormats.Items.Add(str2, true);
            g_Sizes.Add(new Size(width, height));
            g_Sizes.Add(new Size(width, height));
        }
        else
        {
            g_Sizes = new List<Size>(12);
        }
        clbFormats.Items.Add("256x256, PNG", false);
        clbFormats.Items.Add("128x128, 32-bit", false);
        clbFormats.Items.Add("128x128, 8-bit", false);
        clbFormats.Items.Add("64x64, 32-bit", false);
        clbFormats.Items.Add("64x64, 8-bit", false);
        clbFormats.Items.Add("48x48, 32-bit", false);
        clbFormats.Items.Add("48x48, 8-bit", false);
        clbFormats.Items.Add("32x32, 32-bit", true);
        clbFormats.Items.Add("32x32, 8-bit", true);
        clbFormats.Items.Add("24x24, 32-bit", false);
        clbFormats.Items.Add("24x24, 8-bit", false);
        clbFormats.Items.Add("16x16, 32-bit", true);
        clbFormats.Items.Add("16x16, 8-bit", true);
        g_Sizes.Add(new Size(256, 256));
        g_Sizes.Add(new Size(128, 128));
        g_Sizes.Add(new Size(128, 128));
        g_Sizes.Add(new Size(64, 64));
        g_Sizes.Add(new Size(64, 64));
        g_Sizes.Add(new Size(48, 48));
        g_Sizes.Add(new Size(48, 48));
        g_Sizes.Add(new Size(32, 32));
        g_Sizes.Add(new Size(32, 32));
        g_Sizes.Add(new Size(24, 24));
        g_Sizes.Add(new Size(24, 24));
        g_Sizes.Add(new Size(16, 16));
        g_Sizes.Add(new Size(16, 16));
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing && components != null)
            components.Dispose();
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
        CancelBtn = new Button();
        OKBtn = new Button();
        groupBox4 = new GroupBox();
        clbFormats = new CheckedListBox();
        btnSelectAll = new Button();
        SelNoneBtn = new Button();
        groupBox1 = new GroupBox();
        rbSeparate = new RadioButton();
        rbMerged = new RadioButton();
        groupBox4.SuspendLayout();
        groupBox1.SuspendLayout();
        SuspendLayout();
        CancelBtn.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
        CancelBtn.DialogResult = DialogResult.Cancel;
        CancelBtn.Location = new Point(207, 309);
        CancelBtn.Name = "CancelBtn";
        CancelBtn.Size = new Size(75, 23);
        CancelBtn.TabIndex = 4;
        CancelBtn.Text = "Cancel";
        CancelBtn.UseVisualStyleBackColor = true;
        OKBtn.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
        OKBtn.DialogResult = DialogResult.OK;
        OKBtn.Location = new Point(126, 309);
        OKBtn.Name = "OKBtn";
        OKBtn.Size = new Size(75, 23);
        OKBtn.TabIndex = 3;
        OKBtn.Text = "OK";
        OKBtn.UseVisualStyleBackColor = true;
        groupBox4.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        groupBox4.Controls.Add(clbFormats);
        groupBox4.Controls.Add(btnSelectAll);
        groupBox4.Controls.Add(SelNoneBtn);
        groupBox4.Location = new Point(12, 141);
        groupBox4.Name = "groupBox4";
        groupBox4.Size = new Size(270, 162);
        groupBox4.TabIndex = 1;
        groupBox4.TabStop = false;
        groupBox4.Text = "Copies to be saved";
        clbFormats.CheckOnClick = true;
        clbFormats.FormattingEnabled = true;
        clbFormats.Items.AddRange(new object[2]
        {
            "32x32, 32-bit",
            "32x32, 8-bit"
        });
        clbFormats.Location = new Point(6, 19);
        clbFormats.Name = "clbFormats";
        clbFormats.Size = new Size(258, 109);
        clbFormats.TabIndex = 3;
        btnSelectAll.Location = new Point(6, 134);
        btnSelectAll.Name = "btnSelectAll";
        btnSelectAll.Size = new Size(75, 23);
        btnSelectAll.TabIndex = 2;
        btnSelectAll.Text = "Select All";
        btnSelectAll.UseVisualStyleBackColor = true;
        btnSelectAll.Click += new EventHandler(btnSelectAll_Click);
        SelNoneBtn.Location = new Point(189, 134);
        SelNoneBtn.Name = "SelNoneBtn";
        SelNoneBtn.Size = new Size(75, 23);
        SelNoneBtn.TabIndex = 1;
        SelNoneBtn.Text = "Select None";
        SelNoneBtn.UseVisualStyleBackColor = true;
        SelNoneBtn.Click += new EventHandler(SelNoneBtn_Click);
        groupBox1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        groupBox1.Controls.Add(rbSeparate);
        groupBox1.Controls.Add(rbMerged);
        groupBox1.Location = new Point(12, 12);
        groupBox1.Name = "groupBox1";
        groupBox1.Size = new Size(270, 123);
        groupBox1.TabIndex = 5;
        groupBox1.TabStop = false;
        groupBox1.Text = "Save Mode";
        rbSeparate.AutoSize = true;
        rbSeparate.Location = new Point(16, 55);
        rbSeparate.Name = "rbSeparate";
        rbSeparate.Size = new Size(251, 56);
        rbSeparate.TabIndex = 1;
        rbSeparate.Text = "Each layer as an image within the icon file.\r\nLayer names will be used to determine cropping \r\nwidths and must be in the form #x#. Examples:\r\n32x32, 64x64, etc.";
        rbSeparate.UseVisualStyleBackColor = true;
        rbSeparate.CheckedChanged += new EventHandler(ModeCheckChanged);
        rbMerged.AutoSize = true;
        rbMerged.Checked = true;
        rbMerged.Location = new Point(16, 19);
        rbMerged.Name = "rbMerged";
        rbMerged.Size = new Size(225, 30);
        rbMerged.TabIndex = 0;
        rbMerged.TabStop = true;
        rbMerged.Text = "Merged image (multiple, different resolution\r\nimage copies within the icon file)";
        rbMerged.UseVisualStyleBackColor = true;
        rbMerged.CheckedChanged += new EventHandler(ModeCheckChanged);
        AcceptButton = OKBtn;
        AutoScaleDimensions = new SizeF(6f, 13f);
        AutoScaleMode = AutoScaleMode.Font;
        CancelButton = CancelBtn;
        ClientSize = new Size(292, 342);
        Controls.Add(groupBox1);
        Controls.Add(groupBox4);
        Controls.Add(OKBtn);
        Controls.Add(CancelBtn);
        FormBorderStyle = FormBorderStyle.FixedToolWindow;
        Name = nameof(IcoSaveForm);
        Text = "Icon Save Options";
        groupBox4.ResumeLayout(false);
        groupBox1.ResumeLayout(false);
        groupBox1.PerformLayout();
        Shown += new EventHandler(Form_Shown);
        ResumeLayout(false);
    }
}
