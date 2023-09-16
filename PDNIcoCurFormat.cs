// Decompiled with JetBrains decompiler
// Type: PaintDotNet.Data.PDNIcoCurFormat
// Assembly: IcoCur, Version=4.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: EEB2FED8-625F-4555-8E27-7F881F458B17
// Assembly location: C:\src\cs\Paint.net\IcoCur.dll

using IcoCur.EvanOlds;
using PaintDotNet;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace IcoCur;

[Guid("92F2EE29-8460-4abe-BE88-998F5F5A7681")]
public class PDNIcoCurFormat : FileType, IFileTypeFactory
{
    private List<IcoCurSaveFormat> m_saveFormats;

    public PDNIcoCurFormat()
        : base("Icons", new FileTypeOptions
        {
            SupportsLayers = true,
            SupportsCancellation = false,
            SaveExtensions = new[] { ".ico" },
            LoadExtensions = new[] { ".ico" }
        })
    {
        m_saveFormats = null;
    }

    public FileType[] GetFileTypeInstances() => new FileType[3]
    {
        new PDNIcoCurFormat(),
        new PDNCursorFormat(),
        new PDNANIFormat()
    };

    public static Document GeneralLoad(Stream input)
    {
        EOIcoCurLoader loader = new EOIcoCurLoader(input);
        int count = loader.CountImages();
        switch (count)
        {
            case -2:
                int num1 = (int)MessageBox.Show("Icon/Cursor data is invalid and cannot be loaded");
                return null;
            case -1:
                int num2 = (int)MessageBox.Show("An error occured while trying to load the icon/cursor data.");
                return null;
            case 0:
                int num3 = (int)MessageBox.Show("No valid icons or cursors could be found in the specified file.");
                return null;
            default:
                if (1 == count)
                {
                    Bitmap image = loader.GetImage(0U);
                    image.RotateFlip(RotateFlipType.Rotate180FlipX);
                    BitmapLayer bitmapLayer = new BitmapLayer(Surface.CopyFromBitmap(image));
                    Document document = new Document(image.Width, image.Height);
                    ((ArrayList)document.Layers).Add(bitmapLayer);
                    return document;
                }
                IcoCurLoadForm icoCurLoadForm = new IcoCurLoadForm(loader, (uint)count);
                if (icoCurLoadForm.ShowDialog() == DialogResult.OK)
                {
                    return icoCurLoadForm.BuildDocument();
                }
                int num4 = (int)MessageBox.Show("Load Cancelled");
                return Document.FromImage(new Bitmap(256, 256));
        }
    }

    protected override Document OnLoad(Stream input) => GeneralLoad(input);

    protected override void OnSave(
        Document input,
        Stream output,
        SaveConfigToken token,
        Surface scratchSurface,
        ProgressEventHandler callback)
    {
        IcoSaveForm icoSaveForm = new IcoSaveForm(input);
        if (icoSaveForm.ShowDialog() != DialogResult.OK)
        {
            return;
        }
        m_saveFormats = icoSaveForm.GetSaveFormats();
        bool wantMerged = icoSaveForm.WantMerged;
        icoSaveForm.Dispose();
        if (wantMerged)
        {
            Surface renderedSurface = new Surface(input.Width, input.Height);
            renderedSurface.Fill(ColorBgra.FromBgra(byte.MaxValue, byte.MaxValue, byte.MaxValue, 0));
            EOPDNUtility.RenderClear(input, renderedSurface);
            SaveImagesMerged(new EOIcoCurWriter(output, m_saveFormats.Count, EOIcoCurWriter.IcoCurType.Icon), renderedSurface);
            renderedSurface.Dispose();
        }
        else
        {
            EOIcoCurWriter eoIcoCurWriter = new EOIcoCurWriter(output, input.Layers.Count, EOIcoCurWriter.IcoCurType.Icon);
            for (int index = 0; index < input.Layers.Count; ++index)
            {
                if (((ArrayList)input.Layers)[index] is BitmapLayer)
                {
                    string name = ((Layer)((ArrayList)input.Layers)[index]).Name;
                    string[] strArray = name.Contains("x") || name.Contains("X") ? name.Split('x', 'X') : throw new InvalidOperationException("Layer name was not set by user to be of the format #x#. Examples: 32x32, 64x64, etc.");
                    int int32_1 = Convert.ToInt32(strArray[0]);
                    int int32_2 = Convert.ToInt32(strArray[1]);
                    if (int32_1 > 256 || int32_2 > 256 || int32_1 <= 0 || int32_2 <= 0)
                    {
                        throw new InvalidOperationException(
                            "Layer name indicated an invalid icon dimension. Icon dimensions must be in the range [1, 256].");
                    }

                    Bitmap bitmap = null;
                    using (Bitmap bitmapLayer = EOPDNUtility.GetBitmapLayer(input, index))
                    {
                        bitmap = EvanBitmap.ResizeCropPad(bitmapLayer, int32_1, int32_2);
                    }
                    eoIcoCurWriter.WriteBitmap(bitmap, bitmap, new Point());
                }
            }
        }
    }

    private void SaveImagesMerged(EOIcoCurWriter icoWriter, Surface renderedSurface)
    {
        foreach (IcoCurSaveFormat saveFormat in m_saveFormats)
        {
            if (saveFormat.EightBit)
            {
                Bitmap AlphaImgMask = EOPDNUtility.ResizedBitmapFromSurface(renderedSurface, saveFormat.Dimensions);
                Surface surface = EOPDNUtility.ResizeSurface(saveFormat.Dimensions, renderedSurface);
                Bitmap img = Quantize(surface, 8, byte.MaxValue, false, null);
                surface.Dispose();
                icoWriter.WriteBitmap(img, AlphaImgMask, new Point());
            }
            else
            {
                using Bitmap bitmap = EOPDNUtility.ResizedBitmapFromSurface(renderedSurface, saveFormat.Dimensions);
                icoWriter.WriteBitmap(bitmap, bitmap, new Point());
            }
        }
    }
}
