// Decompiled with JetBrains decompiler
// Type: PaintDotNet.Data.PDNANIFormat
// Assembly: IcoCur, Version=4.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: EEB2FED8-625F-4555-8E27-7F881F458B17
// Assembly location: C:\src\cs\Paint.net\IcoCur.dll

using System.Collections;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace PaintDotNet.Data
{
  internal class PDNANIFormat : FileType
  {
    public PDNANIFormat()
      : base("Animated Cursors", (FileTypeFlags) 29L, new string[1]
      {
        ".ani"
      })
    {
    }

    protected virtual Document OnLoad(Stream input)
    {
      EvanRIFFFormat evanRiffFormat = new EvanRIFFFormat();
      Document document1;
      if (evanRiffFormat.InitFromStream(input) != 0)
      {
        document1 = (Document) null;
      }
      else
      {
        if (evanRiffFormat.MasterChunk.GetStringHeaderID().ToLower() != "acon")
          throw new InvalidDataException("The specified RIFF file is not an animated cursor.");
        int ChunkID = 1852793705;
        Document document2 = (Document) null;
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
          ((ArrayList) document2.Layers).Add((object) bitmapLayer);
        }
        document1 = document2;
      }
      return document1;
    }

    protected virtual void OnSave(
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
      uint num = (uint) (((int) dword1 + 8) * ((ArrayList) input.Layers).Count);
      uint dword2 = num + 60U;
      byte[] buffer1 = new byte[4]
      {
        (byte) 82,
        (byte) 73,
        (byte) 70,
        (byte) 70
      };
      output.Write(buffer1, 0, 4);
      EOStreamUtility.Write_uint(output, dword2);
      byte[] buffer2 = new byte[4]
      {
        (byte) 65,
        (byte) 67,
        (byte) 79,
        (byte) 78
      };
      output.Write(buffer2, 0, 4);
      this.Writeanih(output, (uint) ((ArrayList) input.Layers).Count, animDelay);
      byte[] buffer3 = new byte[4]
      {
        (byte) 76,
        (byte) 73,
        (byte) 83,
        (byte) 84
      };
      output.Write(buffer3, 0, 4);
      EOStreamUtility.Write_uint(output, num + 4U);
      byte[] buffer4 = new byte[4]
      {
        (byte) 102,
        (byte) 114,
        (byte) 97,
        (byte) 109
      };
      output.Write(buffer4, 0, 4);
      for (int index = 0; index < ((ArrayList) input.Layers).Count; ++index)
      {
        byte[] buffer5 = new byte[4]
        {
          (byte) 105,
          (byte) 99,
          (byte) 111,
          (byte) 110
        };
        output.Write(buffer5, 0, 4);
        EOStreamUtility.Write_uint(output, dword1);
        Bitmap bitmapLayerResized = EOPDNUtility.GetBitmapLayerResized(input, index, 32, 32);
        new EOIcoCurWriter(output, 1, EOIcoCurWriter.IcoCurType.Cursor).WriteBitmap(bitmapLayerResized, (Bitmap) null, hotSpot);
      }
    }

    private void Writeanih(Stream outS, uint NumFrames, uint FrameRate)
    {
      byte[] buffer = new byte[4]
      {
        (byte) 97,
        (byte) 110,
        (byte) 105,
        (byte) 104
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
}
