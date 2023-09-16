// Decompiled with JetBrains decompiler
// Type: PaintDotNet.Data.PDNCursorFormat
// Assembly: IcoCur, Version=4.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: EEB2FED8-625F-4555-8E27-7F881F458B17
// Assembly location: C:\src\cs\Paint.net\IcoCur.dll

using System.Drawing;
using System.IO;

namespace PaintDotNet.Data
{
  internal class PDNCursorFormat : FileType
  {
    public PDNCursorFormat()
      : base("Cursors", (FileTypeFlags) 13L, new string[1]
      {
        ".cur"
      })
    {
    }

    protected virtual Document OnLoad(Stream input) => PDNIcoCurFormat.GeneralLoad(input);

    protected virtual SaveConfigToken OnCreateDefaultSaveConfigToken() => (SaveConfigToken) new CurSaveConfigToken();

    public virtual SaveConfigWidget CreateSaveConfigWidget() => (SaveConfigWidget) new CurSaveConfigWidget();

    protected virtual void OnSave(
      Document input,
      Stream output,
      SaveConfigToken token,
      Surface scratchSurface,
      ProgressEventHandler callback)
    {
      CurSaveConfigToken curSaveConfigToken = (CurSaveConfigToken) token;
      EOIcoCurWriter eoIcoCurWriter = new EOIcoCurWriter(output, 1, EOIcoCurWriter.IcoCurType.Cursor);
      Surface surf = new Surface(input.Width, input.Height);
      surf.Clear(ColorBgra.FromBgra(byte.MaxValue, byte.MaxValue, byte.MaxValue, (byte) 0));
      using (RenderArgs renderArgs = new RenderArgs(surf))
        input.Render(renderArgs, true);
      if (surf.Width != 32 || surf.Height != 32)
      {
        Surface surface = EOPDNUtility.ResizeSurface(new Size(32, 32), surf);
        surf.Dispose();
        surf = surface;
      }
      Bitmap aliasedBitmap = surf.CreateAliasedBitmap();
      if (curSaveConfigToken.EightBit)
      {
        Bitmap img = this.Quantize(surf, 8, (int) byte.MaxValue, false, (ProgressEventHandler) null);
        eoIcoCurWriter.WriteBitmap(img, aliasedBitmap, curSaveConfigToken.HotSpot);
      }
      else
        eoIcoCurWriter.WriteBitmap(aliasedBitmap, aliasedBitmap, curSaveConfigToken.HotSpot);
      aliasedBitmap.Dispose();
      surf.Dispose();
    }
  }
}
