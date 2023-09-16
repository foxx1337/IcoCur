// Decompiled with JetBrains decompiler
// Type: PaintDotNet.Data.PDNCursorFormat
// Assembly: IcoCur, Version=4.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: EEB2FED8-625F-4555-8E27-7F881F458B17
// Assembly location: C:\src\cs\Paint.net\IcoCur.dll

using System.Drawing;
using System.IO;
using IcoCur.EvanOlds;
using PaintDotNet;
using PaintDotNet.Data;
using PaintDotNet.Rendering;

namespace IcoCur;

internal class PDNCursorFormat : FileType
{
    public PDNCursorFormat()
        : base("Cursors", new FileTypeOptions
        {
            SupportsLayers = true,
            SupportsCancellation = false,
            SaveExtensions = new[] { ".cur" },
            LoadExtensions = new[] { ".cur" }
        })
    {
    }

    protected override Document OnLoad(Stream input) => PDNIcoCurFormat.GeneralLoad(input);

    protected override SaveConfigToken OnCreateDefaultSaveConfigToken() => new CurSaveConfigToken();

    public override SaveConfigWidget CreateSaveConfigWidget() => new CurSaveConfigWidget();

    protected override void OnSave(
        Document input,
        Stream output,
        SaveConfigToken token,
        Surface scratchSurface,
        ProgressEventHandler callback)
    {
        CurSaveConfigToken curSaveConfigToken = (CurSaveConfigToken)token;
        EOIcoCurWriter eoIcoCurWriter = new EOIcoCurWriter(output, 1, EOIcoCurWriter.IcoCurType.Cursor);
        Surface surf = new Surface(input.Width, input.Height);
        surf.Fill(ColorBgra.FromBgra(byte.MaxValue, byte.MaxValue, byte.MaxValue, 0));
        EOPDNUtility.RenderClear(input, surf);
        if (surf.Width != 32 || surf.Height != 32)
        {
            Surface surface = EOPDNUtility.ResizeSurface(new Size(32, 32), surf);
            surf.Dispose();
            surf = surface;
        }
        Bitmap aliasedBitmap = surf.CreateAliasedBitmap();
        if (curSaveConfigToken.EightBit)
        {
            Bitmap img = Quantize(surf, 8, byte.MaxValue, false, null);
            eoIcoCurWriter.WriteBitmap(img, aliasedBitmap, curSaveConfigToken.HotSpot);
        }
        else
            eoIcoCurWriter.WriteBitmap(aliasedBitmap, aliasedBitmap, curSaveConfigToken.HotSpot);
        aliasedBitmap.Dispose();
        surf.Dispose();
    }
}
