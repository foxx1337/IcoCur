﻿// Decompiled with JetBrains decompiler
// Type: PaintDotNet.EOPDNUtility
// Assembly: IcoCur, Version=4.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: EEB2FED8-625F-4555-8E27-7F881F458B17
// Assembly location: C:\src\cs\Paint.net\IcoCur.dll

using System.Collections;
using System.Drawing;
using PaintDotNet;
using PaintDotNet.Rendering;

namespace IcoCur.EvanOlds;

internal static class EOPDNUtility
{
    public static void RenderClear(Document doc, Surface surface)
    {
        surface.Fill(ColorBgra.Zero);
        doc.CreateRenderer().Render(surface.AsRegionPtr(), surface.Bounds.Location);
    }

    public static Bitmap GetBitmapLayer(Document doc, int index)
    {
        Surface surface = new Surface(doc.Width, doc.Height);
        Rectangle rectangle = new Rectangle(0, 0, doc.Width, doc.Height);
        doc.Layers[index].CreateRenderer().Render(surface.AsRegionPtr(), rectangle.Location);
        using (Bitmap aliasedBitmap = surface.CreateAliasedBitmap())
            return new Bitmap(aliasedBitmap);
    }

    public static Bitmap GetBitmapLayerResized(Document Doc, int index, int Width, int Height)
    {
        Bitmap bitmapLayerResized = null;
        Surface surface1 = new Surface(Doc.Width, Doc.Height);
        Rectangle rectangle = new Rectangle(0, 0, Doc.Width, Doc.Height);
        Doc.Layers[index].CreateRenderer().Render(surface1.AsRegionPtr(), rectangle.Location);
        if (Doc.Width != Width || Doc.Height != Height)
        {
            Surface surface2 = new Surface(Width, Height);
            surface2.FitSurface((ResamplingAlgorithm)1, surface1);
            using (Bitmap aliasedBitmap = surface2.CreateAliasedBitmap())
                bitmapLayerResized = new Bitmap(aliasedBitmap);
            surface2.Dispose();
        }
        else
        {
            using (Bitmap aliasedBitmap = surface1.CreateAliasedBitmap())
                bitmapLayerResized = new Bitmap(aliasedBitmap);
        }
        surface1.Dispose();
        return bitmapLayerResized;
    }

    public static Bitmap ResizedBitmapFromSurface(Surface surf, Size desiredSize)
    {
        if (desiredSize.Width != surf.Width || desiredSize.Height != surf.Height)
        {
            Surface surface = new Surface(desiredSize);
            surface.FitSurface((ResamplingAlgorithm)1, surf);
            return new Bitmap(surface.CreateAliasedBitmap());
        }
        using (Bitmap aliasedBitmap = surf.CreateAliasedBitmap())
            return new Bitmap(aliasedBitmap);
    }

    public static Surface ResizeSurface(Size size, Surface surf)
    {
        Surface surface = new Surface(size);
        ResamplingAlgorithm resamplingAlgorithm = (ResamplingAlgorithm)3;
        if (size.Width > surf.Width || size.Height > surf.Height)
            resamplingAlgorithm = 0;
        surface.FitSurface(resamplingAlgorithm, surf);
        return surface;
    }
}
