// Decompiled with JetBrains decompiler
// Type: EOIcoCurLoader
// Assembly: IcoCur, Version=4.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: EEB2FED8-625F-4555-8E27-7F881F458B17
// Assembly location: C:\src\cs\Paint.net\IcoCur.dll

using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;

namespace IcoCur.EvanOlds;

internal class EOIcoCurLoader
{
    private Stream m_stream;
    private long m_initialStreamPos;
    public string ErrorMsg;
    private Point HotSpot;

    public EOIcoCurLoader(Stream icoCurStream)
    {
        this.m_stream = icoCurStream.CanRead ? icoCurStream : throw new ArgumentException("Cannot initialize EOIcoCurLoader with a stream that doesn't support reading");
        this.m_initialStreamPos = this.m_stream.Position;
        this.ErrorMsg = "An unspecified error has occured";
    }

    public int CountImages()
    {
        long position = this.m_stream.Position;
        this.m_stream.Position = this.m_initialStreamPos;
        byte[] buffer = new byte[6];
        try
        {
            this.m_stream.Read(buffer, 0, 6);
        }
        catch (Exception ex)
        {
            this.ErrorMsg = "Could not get 6 bytes from the beginning of the stream. The following exception was generated:\r\n" + ex.ToString();
            return -1;
        }
        int num1;
        if (buffer[0] != (byte)0 || buffer[1] != (byte)0 || buffer[2] != (byte)1 && buffer[2] != (byte)2 || buffer[3] != (byte)0)
        {
            num1 = -2;
        }
        else
        {
            int num2 = (int)buffer[4] + (int)buffer[5] * 256;
            this.m_stream.Seek(position, SeekOrigin.Begin);
            num1 = num2;
        }
        return num1;
    }

    public unsafe Bitmap GetImage(uint ImageIndex)
    {
        this.m_stream.Position = this.m_initialStreamPos;
        EOIcoCurLoader.IcoHeader icoHeader = new EOIcoCurLoader.IcoHeader();
        icoHeader.ReadFromStream(this.m_stream);
        Bitmap image;
        if (ImageIndex >= (uint)icoHeader.Count)
        {
            this.ErrorMsg = "Invalid image index of " + Convert.ToString(ImageIndex) + " was passed to GetImage";
            image = (Bitmap)null;
        }
        else
        {
            EOIcoCurLoader.DirectoryEntry[] directoryEntryArray = new EOIcoCurLoader.DirectoryEntry[(int)icoHeader.Count];
            for (int index = 0; index < (int)icoHeader.Count; ++index)
                directoryEntryArray[index].ReadFromStream(this.m_stream);
            this.HotSpot = new Point((int)directoryEntryArray[(int)(uint)(UIntPtr)ImageIndex].Planes_XHotspot, (int)directoryEntryArray[(int)(uint)(UIntPtr)ImageIndex].BitCount_YHotspot);
            if (this.m_initialStreamPos + (long)directoryEntryArray[(int)(uint)(UIntPtr)ImageIndex].dwImageOffset > this.m_stream.Length)
                throw new InvalidDataException("Directory entry is invalid. Image offset is outside of the bounds of the input stream.");
            this.m_stream.Position = this.m_initialStreamPos + (long)directoryEntryArray[(int)(uint)(UIntPtr)ImageIndex].dwImageOffset;
            if (1196314761U == EOStreamUtility.Read_uint(this.m_stream))
            {
                this.m_stream.Seek(-4L, SeekOrigin.Current);
                EOOffsetStream eoOffsetStream = new EOOffsetStream(this.m_stream);
                Bitmap bitmap;
                try
                {
                    bitmap = new Bitmap((Stream)eoOffsetStream);
                    bitmap.RotateFlip(RotateFlipType.Rotate180FlipX);
                }
                catch (ArgumentException ex)
                {
                    return (Bitmap)null;
                }
                image = bitmap;
            }
            else
            {
                this.m_stream.Seek(-4L, SeekOrigin.Current);
                uint out_Width = 0;
                uint out_Height = 0;
                uint out_bpp = 0;
                this.GetImageDimensions(ImageIndex, ref out_Width, ref out_Height, ref out_bpp);
                new EOIcoCurLoader.BITMAPINFOHEADER().ReadFromStream(this.m_stream);
                uint* Palette = stackalloc uint[256];
                if (out_bpp <= 8U)
                {
                    int num = 1 << (int)out_bpp;
                    for (int index = 0; index < num; ++index)
                        Palette[index] = EOStreamUtility.Read_uint(this.m_stream);
                }
                uint count = this.SizeComp(out_Width, out_Height, out_bpp);
                byte[] buffer = new byte[(IntPtr)count];
                this.m_stream.Read(buffer, 0, (int)count);
                Bitmap srcBM;
                fixed (byte* bits = buffer)
                    srcBM = EvanBitmap.FromRawBitsbpp((void*)bits, Palette, (int)out_Width, (int)out_Height, (int)out_bpp, true);
                if (srcBM != null && out_bpp != 32U)
                {
                    void* pointer = Marshal.AllocHGlobal((int)this.SizeComp(out_Width, out_Height, 1U)).ToPointer();
                    EOStreamUtility.ReadRaw(this.m_stream, pointer, (int)this.SizeComp(out_Width, out_Height, 1U));
                    Bitmap BWMask = EvanBitmap.FromRawBitsBinary(pointer, (int)out_Width, (int)out_Height, true);
                    Marshal.FreeHGlobal(new IntPtr(pointer));
                    EvanBitmap.MaskToAlpha(srcBM, BWMask);
                }
                image = srcBM;
            }
        }
        return image;
    }

    public bool GetImageDimensions(
      uint ImageIndex,
      ref uint out_Width,
      ref uint out_Height,
      ref uint out_bpp)
    {
        long position = this.m_stream.Position;
        this.m_stream.Position = this.m_initialStreamPos;
        EOIcoCurLoader.IcoHeader icoHeader = new EOIcoCurLoader.IcoHeader();
        icoHeader.ReadFromStream(this.m_stream);
        bool imageDimensions;
        if (ImageIndex >= (uint)icoHeader.Count)
        {
            this.ErrorMsg = "Invalid image index passed to GetImageDimensions.\r\nImage index: " + Convert.ToString(ImageIndex) + "\r\nAvailable image count: " + Convert.ToString(icoHeader.Count);
            imageDimensions = false;
        }
        else
        {
            EOIcoCurLoader.DirectoryEntry[] directoryEntryArray = new EOIcoCurLoader.DirectoryEntry[(int)icoHeader.Count];
            for (int index = 0; index < (int)icoHeader.Count; ++index)
                directoryEntryArray[index].ReadFromStream(this.m_stream);
            long offset = this.m_initialStreamPos + (long)directoryEntryArray[(int)(uint)(UIntPtr)ImageIndex].dwImageOffset;
            try
            {
                this.m_stream.Seek(offset, SeekOrigin.Begin);
            }
            catch (Exception ex)
            {
                this.ErrorMsg = "Could not seek to appropriate position in icon stream data.\r\nThe file data may be truncated, inaccessible or invalid.\r\nAttempted seek position: " + Convert.ToString(offset);
                this.m_stream.Seek(position, SeekOrigin.Begin);
                return false;
            }
            if (1196314761U == EOStreamUtility.Read_uint(this.m_stream))
            {
                this.m_stream.Seek(-4L, SeekOrigin.Current);
                EOOffsetStream eoOffsetStream = new EOOffsetStream(this.m_stream);
                try
                {
                    using (Bitmap bitmap = new Bitmap((Stream)eoOffsetStream))
                    {
                        out_Width = (uint)bitmap.Width;
                        out_Height = (uint)bitmap.Height;
                        out_bpp = this.PixelFormatTobpp(bitmap.PixelFormat);
                    }
                }
                catch (ArgumentException ex)
                {
                    return false;
                }
                imageDimensions = true;
            }
            else
            {
                this.m_stream.Seek(-4L, SeekOrigin.Current);
                EOIcoCurLoader.BITMAPINFOHEADER bitmapinfoheader = new EOIcoCurLoader.BITMAPINFOHEADER();
                bitmapinfoheader.ReadFromStream(this.m_stream);
                out_bpp = (uint)bitmapinfoheader.BitCount;
                out_Width = (uint)directoryEntryArray[(int)(uint)(UIntPtr)ImageIndex].bWidth;
                out_Height = (uint)directoryEntryArray[(int)(uint)(UIntPtr)ImageIndex].bHeight;
                uint num = this.SizeComp(out_Width, out_Height, out_bpp) + this.SizeComp(out_Width, out_Height, 1U);
                if (out_Width == 0U && out_Height == 0U)
                {
                    out_Width = (uint)bitmapinfoheader.Width;
                    out_Height = (uint)(bitmapinfoheader.Height / 2);
                }
                else if ((int)num != (int)directoryEntryArray[(int)(uint)(UIntPtr)ImageIndex].dwBytesInRes)
                {
                    if (out_Width == (uint)byte.MaxValue)
                        out_Width = 256U;
                    if (out_Height == (uint)byte.MaxValue)
                        out_Height = 256U;
                }
                if (out_Width == 0U)
                    out_Width = 256U;
                if (out_Height == 0U)
                    out_Height = 256U;
                this.m_stream.Seek(position, SeekOrigin.Begin);
                imageDimensions = true;
            }
        }
        return imageDimensions;
    }

    private PixelFormat PixelFormatFrombpp(uint bpp)
    {
        switch (bpp)
        {
            case 1:
                return PixelFormat.Format1bppIndexed;
            case 4:
                return PixelFormat.Format4bppIndexed;
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

    private uint PixelFormatTobpp(PixelFormat pf)
    {
        switch (pf)
        {
            case PixelFormat.Format16bppRgb555:
                return 15;
            case PixelFormat.Format16bppRgb565:
                return 16;
            case PixelFormat.Format24bppRgb:
                return 24;
            case PixelFormat.Format1bppIndexed:
                return 1;
            case PixelFormat.Format4bppIndexed:
                return 4;
            case PixelFormat.Format8bppIndexed:
                return 8;
            case PixelFormat.Format32bppArgb:
                return 32;
            default:
                return 32;
        }
    }

    private int SizeComp(int w, int h, int bpp)
    {
        int num = w * bpp / 8;
        if (num % 4 != 0)
            num += 4 - num % 4;
        return h * num;
    }

    private uint SizeComp(uint w, uint h, uint bpp)
    {
        uint num = w * bpp / 8U;
        if (num % 4U != 0U)
            num += 4U - num % 4U;
        return h * num;
    }

    public enum Type
    {
        Type_Icon = 1,
        Type_Cursor = 2,
    }

    private struct IcoHeader
    {
        public ushort Reserved;
        public ushort Type;
        public ushort Count;

        public IcoHeader(EOIcoCurLoader.Type type)
        {
            this.Reserved = (ushort)0;
            this.Type = (ushort)type;
            this.Count = (ushort)0;
        }

        public bool IsValid()
        {
            if (this.Reserved != (ushort)0)
                return false;
            return this.Type == (ushort)1 || this.Type == (ushort)2;
        }

        public static unsafe int GetStructSize() => sizeof(EOIcoCurLoader.IcoHeader);

        public void ReadFromStream(Stream InStream)
        {
            this.Reserved = EOStreamUtility.Read_ushort(InStream);
            this.Type = EOStreamUtility.Read_ushort(InStream);
            this.Count = EOStreamUtility.Read_ushort(InStream);
        }

        public void WriteToStream(Stream outS)
        {
            EOStreamUtility.Write_ushort(outS, this.Reserved);
            EOStreamUtility.Write_ushort(outS, this.Type);
            EOStreamUtility.Write_ushort(outS, this.Count);
        }
    }

    private struct DirectoryEntry
    {
        public byte bWidth;
        public byte bHeight;
        public byte bColorCount;
        public byte bReserved;
        public ushort Planes_XHotspot;
        public ushort BitCount_YHotspot;
        public uint dwBytesInRes;
        public uint dwImageOffset;

        public static unsafe int GetStructSize() => sizeof(EOIcoCurLoader.DirectoryEntry);

        public void ReadFromStream(Stream InStream)
        {
            this.bWidth = (byte)InStream.ReadByte();
            this.bHeight = (byte)InStream.ReadByte();
            this.bColorCount = (byte)InStream.ReadByte();
            this.bReserved = (byte)InStream.ReadByte();
            this.Planes_XHotspot = EOStreamUtility.Read_ushort(InStream);
            this.BitCount_YHotspot = EOStreamUtility.Read_ushort(InStream);
            this.dwBytesInRes = EOStreamUtility.Read_uint(InStream);
            this.dwImageOffset = EOStreamUtility.Read_uint(InStream);
        }

        public void WriteToStream(Stream outS)
        {
            outS.WriteByte(this.bWidth);
            outS.WriteByte(this.bHeight);
            outS.WriteByte(this.bColorCount);
            outS.WriteByte(this.bReserved);
            EOStreamUtility.Write_ushort(outS, this.Planes_XHotspot);
            EOStreamUtility.Write_ushort(outS, this.BitCount_YHotspot);
            EOStreamUtility.Write_uint(outS, this.dwBytesInRes);
            EOStreamUtility.Write_uint(outS, this.dwImageOffset);
        }
    }

    private struct BITMAPINFOHEADER
    {
        public uint StructSize;
        public int Width;
        public int Height;
        public ushort Planes;
        public ushort BitCount;
        public uint biCompression;
        public uint biSizeImage;
        public int biXPelsPerMeter;
        public int biYPelsPerMeter;
        public uint biClrUsed;
        public uint biClrImportant;

        public unsafe BITMAPINFOHEADER(int width, int height, int bpp)
        {
            this.StructSize = (uint)sizeof(EOIcoCurLoader.BITMAPINFOHEADER);
            this.Width = width;
            this.Height = height;
            this.Planes = (ushort)1;
            this.BitCount = (ushort)bpp;
            this.biCompression = 0U;
            this.biSizeImage = (uint)(width * height * bpp / 8);
            this.biXPelsPerMeter = 0;
            this.biYPelsPerMeter = 0;
            this.biClrUsed = 0U;
            this.biClrImportant = 0U;
        }

        public static unsafe int GetStructSize() => sizeof(EOIcoCurLoader.BITMAPINFOHEADER);

        public void ReadFromStream(Stream inS)
        {
            this.StructSize = EOStreamUtility.Read_uint(inS);
            this.Width = EOStreamUtility.ReadInt(inS);
            this.Height = EOStreamUtility.ReadInt(inS);
            this.Planes = EOStreamUtility.Read_ushort(inS);
            this.BitCount = EOStreamUtility.Read_ushort(inS);
            this.biCompression = EOStreamUtility.Read_uint(inS);
            this.biSizeImage = EOStreamUtility.Read_uint(inS);
            this.biXPelsPerMeter = EOStreamUtility.ReadInt(inS);
            this.biYPelsPerMeter = EOStreamUtility.ReadInt(inS);
            this.biClrUsed = EOStreamUtility.Read_uint(inS);
            this.biClrImportant = EOStreamUtility.Read_uint(inS);
        }

        public void WriteToStream(Stream outS)
        {
            EOStreamUtility.Write_uint(outS, this.StructSize);
            EOStreamUtility.Write_uint(outS, (uint)this.Width);
            EOStreamUtility.Write_uint(outS, (uint)this.Height);
            EOStreamUtility.Write_ushort(outS, this.Planes);
            EOStreamUtility.Write_ushort(outS, this.BitCount);
            EOStreamUtility.Write_uint(outS, this.biCompression);
            EOStreamUtility.Write_uint(outS, this.biSizeImage);
            EOStreamUtility.Write_uint(outS, (uint)this.biXPelsPerMeter);
            EOStreamUtility.Write_uint(outS, (uint)this.biYPelsPerMeter);
            EOStreamUtility.Write_uint(outS, this.biClrUsed);
            EOStreamUtility.Write_uint(outS, this.biClrImportant);
        }
    }
}
