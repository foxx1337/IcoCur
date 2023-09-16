// Decompiled with JetBrains decompiler
// Type: PaintDotNet.Data.PDNANIFormat
// Assembly: IcoCur, Version=4.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: EEB2FED8-625F-4555-8E27-7F881F458B17
// Assembly location: C:\src\cs\Paint.net\IcoCur.dll

using System.Collections;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using IcoCur.EvanOlds;
using PaintDotNet;

namespace IcoCur;

internal class PDNANIFormat : FileType
{
    public PDNANIFormat()
        : base("Animated Cursors", new FileTypeOptions
        {
            SupportsLayers = true,
            SupportsCancellation = false,
            LoadExtensions = new[] { ".ani" },
            SaveExtensions = new[] { ".ani" }
        })
    {
    }

    protected override Document OnLoad(Stream input)
    {
        EvanRIFFFormat evanRiffFormat = new EvanRIFFFormat();
        Document document1;
        if (evanRiffFormat.InitFromStream(input) != 0)
        {
            document1 = null;
        }
        else
        {
            if (evanRiffFormat.MasterChunk.GetStringHeaderID().ToLower() != "acon")
                throw new InvalidDataException("The specified RIFF file is not an animated cursor.");
            int ChunkID = 1852793705;
            Document document2 = null;
            foreach (EvanRIFFFormat.Chunk allChunk in evanRiffFormat.FindAllChunks(ChunkID))
            {
                input.Seek(allChunk.DataOffset(), SeekOrigin.Begin);
                Bitmap image = new EOIcoCurLoader(input).GetImage(0U);
                if (image == null)
                    throw new InvalidDataException("Data within the animated cursor file is corrupt or invalid");
                if (document2 == null)
                    document2 = new Document(image.Width, image.Height);
                image.RotateFlip(RotateFlipType.Rotate180FlipX);
                Surface surface = Surface.CopyFromBitmap(image);
                image.Dispose();
                BitmapLayer bitmapLayer = new BitmapLayer(surface);
                ((ArrayList)document2.Layers).Add(bitmapLayer);
            }
            document1 = document2;
        }
        return document1;
    }

    protected override void OnSave(
        Document input,
        Stream output,
        SaveConfigToken token,
        Surface scratchSurface,
        ProgressEventHandler callback)
    {
        ANISaveOptForm aniSaveOptForm = new ANISaveOptForm();
        aniSaveOptForm.InitFromDocument(input);
        if (aniSaveOptForm.ShowDialog() != DialogResult.OK)
            return;
        uint animDelay = aniSaveOptForm.GetAnimDelay();
        Point hotSpot = aniSaveOptForm.GetHotSpot();
        uint dword1 = 4286;
        uint num = (uint)(((int)dword1 + 8) * input.Layers.Count);
        uint dword2 = num + 60U;
        byte[] buffer1 = new byte[4]
        {
            82,
            73,
            70,
            70
        };
        output.Write(buffer1, 0, 4);
        EOStreamUtility.Write_uint(output, dword2);
        byte[] buffer2 = new byte[4]
        {
            65,
            67,
            79,
            78
        };
        output.Write(buffer2, 0, 4);
        Writeanih(output, (uint)input.Layers.Count, animDelay);
        byte[] buffer3 = new byte[4]
        {
            76,
            73,
            83,
            84
        };
        output.Write(buffer3, 0, 4);
        EOStreamUtility.Write_uint(output, num + 4U);
        byte[] buffer4 = new byte[4]
        {
            102,
            114,
            97,
            109
        };
        output.Write(buffer4, 0, 4);
        for (int index = 0; index < input.Layers.Count; ++index)
        {
            byte[] buffer5 = new byte[4]
            {
                105,
                99,
                111,
                110
            };
            output.Write(buffer5, 0, 4);
            EOStreamUtility.Write_uint(output, dword1);
            Bitmap bitmapLayerResized = EOPDNUtility.GetBitmapLayerResized(input, index, 32, 32);
            new EOIcoCurWriter(output, 1, EOIcoCurWriter.IcoCurType.Cursor).WriteBitmap(bitmapLayerResized, null,
                hotSpot);
        }
    }

    private void Writeanih(Stream outS, uint NumFrames, uint FrameRate)
    {
        byte[] buffer = new byte[4]
        {
            97,
            110,
            105,
            104
        };
        outS.Write(buffer, 0, 4);
        uint dword = 36;
        EOStreamUtility.Write_uint(outS, dword);
        EOStreamUtility.Write_uint(outS, dword);
        EOStreamUtility.Write_uint(outS, NumFrames);
        EOStreamUtility.Write_uint(outS, NumFrames);
        EOStreamUtility.Write_uint(outS, 0U);
        EOStreamUtility.Write_uint(outS, 0U);
        EOStreamUtility.Write_uint(outS, 0U);
        EOStreamUtility.Write_uint(outS, 0U);
        EOStreamUtility.Write_uint(outS, FrameRate);
        EOStreamUtility.Write_uint(outS, 1U);
    }
}
