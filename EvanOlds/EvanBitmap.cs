// Decompiled with JetBrains decompiler
// Type: EvanBitmap
// Assembly: IcoCur, Version=4.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: EEB2FED8-625F-4555-8E27-7F881F458B17
// Assembly location: C:\src\cs\Paint.net\IcoCur.dll

using System;
using System.Drawing;
using System.Drawing.Imaging;

namespace IcoCur.EvanOlds;

internal static class EvanBitmap
{
    public static unsafe void AddToAlpha(Bitmap bm, int A_Add)
    {
        Rectangle rect = new Rectangle(0, 0, bm.Width, bm.Height);
        BitmapData bitmapdata = bm.LockBits(rect, ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
        byte* pointer = (byte*)bitmapdata.Scan0.ToPointer();
        for (byte* numPtr = pointer + ((IntPtr)(bm.Width * bm.Height) * 4).ToInt64(); pointer < numPtr; pointer += 4)
        {
            int num = (int)pointer[3] + A_Add;
            if (num < 0)
                num = 0;
            else if (num > (int)byte.MaxValue)
                num = (int)byte.MaxValue;
            pointer[3] = (byte)num;
        }
        bm.UnlockBits(bitmapdata);
    }

    private static uint ComputeSize(uint w, uint h, uint bpp, bool PaddedTo32)
    {
        uint num = w * bpp / 8U;
        if (PaddedTo32 && num % 4U != 0U)
            num += 4U - num % 4U;
        return h * num;
    }

    public static unsafe int CountTransparentColumnsFromLeft(Bitmap bm)
    {
        int width = bm.Width;
        int height = bm.Height;
        Rectangle rect = new Rectangle(0, 0, bm.Width, bm.Height);
        BitmapData bitmapdata = bm.LockBits(rect, ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
        uint* numPtr1 = (uint*)((IntPtr)bitmapdata.Scan0.ToPointer() + (IntPtr)(width * height) * 4);
        int num;
        for (num = 0; num < width; ++num)
        {
            for (uint* numPtr2 = (uint*)((IntPtr)bitmapdata.Scan0.ToPointer() + (IntPtr)num * 4); numPtr2 < numPtr1; numPtr2 += width)
            {
                if (((int)*numPtr2 & -16777216) != 0)
                {
                    bm.UnlockBits(bitmapdata);
                    return num;
                }
            }
        }
        bm.UnlockBits(bitmapdata);
        return num;
    }

    public static unsafe int CountTransparentColumnsFromRight(Bitmap bm)
    {
        int width = bm.Width;
        int height = bm.Height;
        Rectangle rect = new Rectangle(0, 0, bm.Width, bm.Height);
        BitmapData bitmapdata = bm.LockBits(rect, ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
        uint* numPtr1 = (uint*)((IntPtr)bitmapdata.Scan0.ToPointer() + (IntPtr)(width * height) * 4);
        for (int index = width - 1; index >= 0; --index)
        {
            for (uint* numPtr2 = (uint*)((IntPtr)bitmapdata.Scan0.ToPointer() + (IntPtr)index * 4); numPtr2 < numPtr1; numPtr2 += width)
            {
                if (((int)*numPtr2 & -16777216) != 0)
                {
                    bm.UnlockBits(bitmapdata);
                    return width - index - 1;
                }
            }
        }
        bm.UnlockBits(bitmapdata);
        return width;
    }

    public static unsafe int CountTransparentRowsFromBottom(Bitmap bm)
    {
        int width = bm.Width;
        int height = bm.Height;
        Rectangle rect = new Rectangle(0, 0, bm.Width, bm.Height);
        BitmapData bitmapdata = bm.LockBits(rect, ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
        for (int index = height - 1; index >= 0; --index)
        {
            uint* numPtr1 = (uint*)((IntPtr)bitmapdata.Scan0.ToPointer() + (IntPtr)(index * width) * 4);
            for (uint* numPtr2 = numPtr1 + width; numPtr1 < numPtr2; ++numPtr1)
            {
                if (((int)*numPtr1 & -16777216) != 0)
                {
                    bm.UnlockBits(bitmapdata);
                    return height - index - 1;
                }
            }
        }
        bm.UnlockBits(bitmapdata);
        return height;
    }

    public static unsafe void FillRawBits32(Bitmap bm, uint* bits, int w, int h)
    {
        int num = w * h;
        Rectangle rect = new Rectangle(0, 0, w, h);
        BitmapData bitmapdata = bm.LockBits(rect, ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
        uint* pointer = (uint*)bitmapdata.Scan0.ToPointer();
        for (int index = 0; index < num; ++index)
            bits[index] = pointer[index];
        bm.UnlockBits(bitmapdata);
    }

    public static PixelFormat FormatFrombpp(int bpp)
    {
        switch (bpp)
        {
            case 1:
                return PixelFormat.Format1bppIndexed;
            case 8:
                return PixelFormat.Format8bppIndexed;
            case 15:
                return PixelFormat.Format16bppRgb555;
            case 16:
                return PixelFormat.Format16bppRgb565;
            case 24:
                return PixelFormat.Format24bppRgb;
            case 32:
                return PixelFormat.Format32bppArgb;
            default:
                return PixelFormat.Undefined;
        }
    }

    public static unsafe Bitmap FromBitsNative(void* bits, int w, int h, int bpp)
    {
        uint size = EvanBitmap.ComputeSize((uint)w, (uint)h, (uint)bpp, true);
        PixelFormat format = EvanBitmap.FormatFrombpp(bpp);
        Bitmap bitmap = new Bitmap(w, h);
        BitmapData bitmapdata = bitmap.LockBits(new Rectangle(0, 0, w, h), ImageLockMode.ReadWrite, format);
        byte* pointer = (byte*)bitmapdata.Scan0.ToPointer();
        for (uint index = 0; index < size; ++index)
            pointer[(int)index] = *(byte*)((IntPtr)bits + (int)index);
        bitmap.UnlockBits(bitmapdata);
        return bitmap;
    }

    public static unsafe Bitmap FromRawBits24(void* bits, int w, int h, bool PaddedTo32Bit)
    {
        Bitmap bitmap = new Bitmap(w, h);
        Rectangle rect = new Rectangle(0, 0, w, h);
        BitmapData bitmapdata = bitmap.LockBits(rect, ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
        int num1 = w * 3;
        if (PaddedTo32Bit && num1 % 4 != 0)
            num1 += 4 - num1 % 4;
        byte* pointer = (byte*)bitmapdata.Scan0.ToPointer();
        for (int index1 = 0; index1 < h; ++index1)
        {
            uint* numPtr1 = (uint*)(pointer + ((IntPtr)index1 * bitmapdata.Stride).ToInt64());
            byte* numPtr2 = (byte*)((IntPtr)bits + (IntPtr)index1 * num1);
            if (index1 == h - 1)
            {
                for (int index2 = 0; index2 < w; ++index2)
                {
                    ushort num2 = *(ushort*)(numPtr2 + ((IntPtr)index2 * 3).ToInt64());
                    byte num3 = numPtr2[index2 * 3 + 2];
                    numPtr1[index2] = (uint)((int)num2 | (int)num3 << 16 | -16777216);
                }
            }
            else
            {
                for (int index3 = 0; index3 < w; ++index3)
                    numPtr1[index3] = *(uint*)(numPtr2 + ((IntPtr)index3 * 3).ToInt64()) | 4278190080U;
            }
        }
        bitmap.UnlockBits(bitmapdata);
        return bitmap;
    }

    public static unsafe Bitmap FromRawBits32(uint* bits, int w, int h)
    {
        int num = w * h;
        Bitmap bitmap = new Bitmap(w, h);
        Rectangle rect = new Rectangle(0, 0, w, h);
        BitmapData bitmapdata = bitmap.LockBits(rect, ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
        uint* pointer = (uint*)bitmapdata.Scan0.ToPointer();
        for (int index = 0; index < num; ++index)
            pointer[index] = bits[index];
        bitmap.UnlockBits(bitmapdata);
        return bitmap;
    }

    public static unsafe Bitmap FromRawBits4(
      void* bits,
      uint* Palette,
      int w,
      int h,
      bool PaddedTo32Bit)
    {
        Bitmap bitmap = new Bitmap(w, h);
        Rectangle rect = new Rectangle(0, 0, w, h);
        BitmapData bitmapdata = bitmap.LockBits(rect, ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
        int num = w / 2;
        if (PaddedTo32Bit && num % 4 != 0)
            num += 4 - num % 4;
        byte* pointer = (byte*)bitmapdata.Scan0.ToPointer();
        for (int index1 = 0; index1 < h; ++index1)
        {
            uint* numPtr1 = (uint*)(pointer + ((IntPtr)index1 * bitmapdata.Stride).ToInt64());
            byte* numPtr2 = (byte*)((IntPtr)bits + (IntPtr)index1 * num);
            for (int index2 = 0; index2 < w; ++index2)
            {
                bool flag = index2 % 2 == 0;
                uint index3 = (uint)numPtr2[index2 / 2] & (flag ? 240U : 15U);
                if (flag)
                    index3 >>= 4;
                numPtr1[index2] = Palette[index3] | 4278190080U;
            }
        }
        bitmap.UnlockBits(bitmapdata);
        return bitmap;
    }

    public static unsafe Bitmap FromRawBits8(
      void* bits,
      uint* Palette,
      int w,
      int h,
      bool PaddedTo32Bit)
    {
        Bitmap bitmap = new Bitmap(w, h);
        Rectangle rect = new Rectangle(0, 0, w, h);
        BitmapData bitmapdata = bitmap.LockBits(rect, ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
        int num = w;
        if (PaddedTo32Bit && w % 4 != 0)
            num += 4 - w % 4;
        byte* pointer = (byte*)bitmapdata.Scan0.ToPointer();
        for (int index1 = 0; index1 < h; ++index1)
        {
            uint* numPtr1 = (uint*)(pointer + ((IntPtr)index1 * bitmapdata.Stride).ToInt64());
            byte* numPtr2 = (byte*)((IntPtr)bits + (IntPtr)index1 * num);
            for (int index2 = 0; index2 < w; ++index2)
                numPtr1[index2] = Palette[numPtr2[index2]] | 4278190080U;
        }
        bitmap.UnlockBits(bitmapdata);
        return bitmap;
    }

    public static unsafe Bitmap FromRawBitsBinary(void* bits, int w, int h, bool PaddedTo32Bit)
    {
        Bitmap bitmap = new Bitmap(w, h);
        Rectangle rect = new Rectangle(0, 0, w, h);
        BitmapData bitmapdata = bitmap.LockBits(rect, ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
        int num1 = w / 8;
        if (PaddedTo32Bit && num1 % 4 != 0)
            num1 += 4 - num1 % 4;
        byte* pointer = (byte*)bitmapdata.Scan0.ToPointer();
        for (int index1 = 0; index1 < h; ++index1)
        {
            uint* numPtr1 = (uint*)(pointer + ((IntPtr)index1 * bitmapdata.Stride).ToInt64());
            byte* numPtr2 = (byte*)((IntPtr)bits + (IntPtr)index1 * num1);
            for (int index2 = 0; index2 < w; ++index2)
            {
                uint num2 = (uint)numPtr2[index2 / 8] >> 7 - index2 % 8 & 1U;
                numPtr1[index2] = num2 == 0U ? 4278190080U : uint.MaxValue;
            }
        }
        bitmap.UnlockBits(bitmapdata);
        return bitmap;
    }

    public static unsafe Bitmap FromRawBitsbpp(
      void* bits,
      uint* Palette,
      int w,
      int h,
      int bpp,
      bool PaddedTo32Bit)
    {
        Bitmap bitmap;
        switch (bpp)
        {
            case 1:
                bitmap = EvanBitmap.FromRawBitsBinary(bits, w, h, PaddedTo32Bit);
                break;
            case 4:
                bitmap = EvanBitmap.FromRawBits4(bits, Palette, w, h, PaddedTo32Bit);
                break;
            case 8:
                bitmap = EvanBitmap.FromRawBits8(bits, Palette, w, h, PaddedTo32Bit);
                break;
            case 16:
                bitmap = EvanBitmap.FromBitsNative(bits, w, h, bpp);
                break;
            case 24:
                bitmap = EvanBitmap.FromRawBits24(bits, w, h, PaddedTo32Bit);
                break;
            case 32:
                bitmap = EvanBitmap.FromRawBits32((uint*)bits, w, h);
                break;
            default:
                bitmap = (Bitmap)null;
                break;
        }
        return bitmap;
    }

    public static int GetBitsPerPixel(Bitmap bm)
    {
        switch (bm.PixelFormat)
        {
            case PixelFormat.Format24bppRgb:
                return 24;
            case PixelFormat.Format32bppRgb:
            case PixelFormat.Format32bppPArgb:
            case PixelFormat.Format32bppArgb:
                return 32;
            case PixelFormat.Format8bppIndexed:
                return 8;
            default:
                return 0;
        }
    }

    public static unsafe void MakeOpaque(Bitmap bm)
    {
        int width = bm.Width;
        int height = bm.Height;
        int num = width * height;
        Rectangle rect = new Rectangle(0, 0, width, height);
        BitmapData bitmapdata = bm.LockBits(rect, ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);
        uint* pointer = (uint*)bitmapdata.Scan0.ToPointer();
        for (int index = 0; index < num; ++index)
        {
            uint* numPtr = pointer + index;
            *numPtr = *numPtr | 4278190080U;
        }
        bm.UnlockBits(bitmapdata);
    }

    public static unsafe void MakeSolidColor(Bitmap bm, uint IntColor)
    {
        int width = bm.Width;
        int height = bm.Height;
        int num = width * height;
        Rectangle rect = new Rectangle(0, 0, width, height);
        BitmapData bitmapdata = bm.LockBits(rect, ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);
        uint* pointer = (uint*)bitmapdata.Scan0.ToPointer();
        for (int index = 0; index < num; ++index)
            pointer[index] = IntColor;
        bm.UnlockBits(bitmapdata);
    }

    public static unsafe bool MaskToAlpha(Bitmap srcBM, Bitmap BWMask)
    {
        bool alpha;
        if (srcBM.Width != BWMask.Width || srcBM.Height != BWMask.Height)
        {
            alpha = false;
        }
        else
        {
            int height = srcBM.Height;
            int width = srcBM.Width;
            Rectangle rect = new Rectangle(0, 0, width, height);
            BitmapData bitmapdata1;
            uint* pointer1;
            BitmapData bitmapdata2;
            uint* pointer2;
            try
            {
                bitmapdata1 = BWMask.LockBits(rect, ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
                pointer1 = (uint*)bitmapdata1.Scan0.ToPointer();
                bitmapdata2 = srcBM.LockBits(rect, ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
                pointer2 = (uint*)bitmapdata2.Scan0.ToPointer();
            }
            catch (Exception ex)
            {
                return false;
            }
            int num = width * height;
            for (int index = 0; index < num; ++index)
            {
                if (((int)pointer1[index] & 16777215) == 16777215)
                {
                    uint* numPtr = pointer2 + index;
                    *numPtr = *numPtr & 16777215U;
                }
            }
            BWMask.UnlockBits(bitmapdata1);
            srcBM.UnlockBits(bitmapdata2);
            alpha = true;
        }
        return alpha;
    }

    public static unsafe Bitmap ResizeCropPad(Bitmap bm, int NewWidth, int NewHeight)
    {
        Bitmap bitmap1;
        if (NewWidth == bm.Width && NewHeight == bm.Height)
        {
            bitmap1 = new Bitmap((Image)bm);
        }
        else
        {
            int height = bm.Height;
            int width = bm.Width;
            Rectangle rect1 = new Rectangle(0, 0, width, height);
            BitmapData bitmapdata1 = bm.LockBits(rect1, ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
            uint* pointer1 = (uint*)bitmapdata1.Scan0.ToPointer();
            Bitmap bitmap2 = new Bitmap(NewWidth, NewHeight);
            Rectangle rect2 = new Rectangle(0, 0, NewWidth, NewHeight);
            BitmapData bitmapdata2 = bitmap2.LockBits(rect2, ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
            uint* pointer2 = (uint*)bitmapdata2.Scan0.ToPointer();
            int num = NewWidth > bm.Width ? bm.Width : NewWidth;
            for (int index1 = 0; index1 < NewHeight; ++index1)
            {
                uint* numPtr1 = pointer2 + index1 * NewWidth;
                uint* numPtr2 = pointer1 + index1 * width;
                if (index1 >= height)
                {
                    for (int index2 = 0; index2 < NewWidth; ++index2)
                        numPtr1[index2] = 0U;
                }
                else
                {
                    for (int index3 = 0; index3 < NewWidth; ++index3)
                        numPtr1[index3] = index3 >= num ? 0U : numPtr2[index3];
                }
            }
            bitmap2.UnlockBits(bitmapdata2);
            bm.UnlockBits(bitmapdata1);
            bitmap1 = bitmap2;
        }
        return bitmap1;
    }
}
