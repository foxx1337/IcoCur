// Decompiled with JetBrains decompiler
// Type: EOIcoCurWriter
// Assembly: IcoCur, Version=4.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: EEB2FED8-625F-4555-8E27-7F881F458B17
// Assembly location: C:\src\cs\Paint.net\IcoCur.dll

using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace IcoCur.EvanOlds;

internal class EOIcoCurWriter
{
    private long m_StreamStart;
    private int m_NumWritten;
    private EOIcoCurWriter.DirectoryEntry[] m_Entries;
    private Stream m_outS;
    private EOIcoCurWriter.IcoCurType m_type;
    public string ErrorMsg;

    public EOIcoCurWriter(Stream outputStream, int imageCount, EOIcoCurWriter.IcoCurType type)
    {
        this.m_StreamStart = outputStream.CanSeek ? outputStream.Position : throw new ArgumentException("Icon/cursor output stream does not support seeking. A stream that supports seeking is required to write icon and cursor data.");
        this.m_outS = outputStream;
        this.m_type = type;
        new EOIcoCurWriter.IcoHeader(type)
        {
            Count = ((ushort)imageCount)
        }.WriteToStream(this.m_outS);
        this.m_NumWritten = 0;
        this.m_Entries = new EOIcoCurWriter.DirectoryEntry[imageCount];
    }

    private unsafe void MakeMask(Bitmap AlphaImg, ref byte[] maskdata, int MaskRowSize)
    {
        int num = MaskRowSize * 8;
        int width = AlphaImg.Width;
        int height = AlphaImg.Height;
        Rectangle rect = new Rectangle(0, 0, width, height);
        BitmapData bitmapdata = AlphaImg.LockBits(rect, ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
        for (int index1 = 0; index1 < height; ++index1)
        {
            byte* numPtr = (byte*)((IntPtr)bitmapdata.Scan0.ToPointer() + (IntPtr)bitmapdata.Stride * index1 + new IntPtr(3));
            int BitIndex = num * (height - 1 - index1);
            for (int index2 = 0; index2 < width; ++index2)
            {
                if (*numPtr > (byte)127)
                    BooleanBitArray.SetMSbFirst(maskdata, BitIndex, false);
                else
                    BooleanBitArray.SetMSbFirst(maskdata, BitIndex, true);
                ++BitIndex;
                numPtr += 4;
            }
        }
        AlphaImg.UnlockBits(bitmapdata);
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

    public unsafe bool WriteBitmap(Bitmap img, Bitmap AlphaImgMask, Point hotSpot)
    {
        long num1 = (long)(EOIcoCurWriter.IcoHeader.GetStructSize() + EOIcoCurWriter.DirectoryEntry.GetStructSize() * this.m_Entries.Length);
        bool flag;
        if (img == null || img.Width <= 0 || img.Height <= 0)
        {
            this.ErrorMsg = "Invalid image passed to \"WriteBitmap\".";
            flag = false;
        }
        else
        {
            MemoryStream memoryStream = (MemoryStream)null;
            if (img.Width >= 256 || img.Height >= 256)
            {
                memoryStream = new MemoryStream();
                img.Save((Stream)memoryStream, ImageFormat.Png);
            }
            int width = img.Width;
            int height = img.Height;
            int bitsPerPixel = EvanBitmap.GetBitsPerPixel(img);
            int Size = this.SizeComp(width, height, bitsPerPixel);
            int MaskRowSize = width / 8;
            int count = width * height / 8;
            if (MaskRowSize % 4 != 0)
            {
                int num2 = 4 - MaskRowSize % 4;
                MaskRowSize += num2;
                count += num2 * height;
            }
            if (this.m_NumWritten != 0)
                num1 = (long)(this.m_Entries[this.m_NumWritten - 1].dwImageOffset + this.m_Entries[this.m_NumWritten - 1].dwBytesInRes);
            this.m_Entries[this.m_NumWritten].bWidth = width >= 256 ? (byte)0 : (byte)width;
            this.m_Entries[this.m_NumWritten].bHeight = height >= 256 ? (byte)0 : (byte)height;
            this.m_Entries[this.m_NumWritten].bColorCount = (byte)0;
            this.m_Entries[this.m_NumWritten].bReserved = (byte)0;
            this.m_Entries[this.m_NumWritten].Planes_XHotspot = (ushort)1;
            this.m_Entries[this.m_NumWritten].BitCount_YHotspot = (ushort)bitsPerPixel;
            if (EOIcoCurWriter.IcoCurType.Cursor == this.m_type)
            {
                this.m_Entries[this.m_NumWritten].Planes_XHotspot = (ushort)hotSpot.X;
                this.m_Entries[this.m_NumWritten].BitCount_YHotspot = (ushort)hotSpot.Y;
            }
            this.m_Entries[this.m_NumWritten].dwBytesInRes = (uint)(EOIcoCurWriter.BITMAPINFOHEADER.GetStructSize() + (bitsPerPixel == 8 ? 1024 : 0) + Size + count);
            if (memoryStream != null)
                this.m_Entries[this.m_NumWritten].dwBytesInRes = (uint)memoryStream.Length;
            this.m_Entries[this.m_NumWritten].dwImageOffset = (uint)num1;
            this.m_outS.Seek(this.m_StreamStart + (long)EOIcoCurWriter.IcoHeader.GetStructSize() + (long)(EOIcoCurWriter.DirectoryEntry.GetStructSize() * this.m_NumWritten), SeekOrigin.Begin);
            this.m_Entries[this.m_NumWritten].WriteToStream(this.m_outS);
            this.m_outS.Seek(this.m_StreamStart + num1, SeekOrigin.Begin);
            if (memoryStream != null)
            {
                this.m_outS.Write(memoryStream.ToArray(), 0, (int)memoryStream.Length);
            }
            else
            {
                new EOIcoCurWriter.BITMAPINFOHEADER(width, height * 2, bitsPerPixel).WriteToStream(this.m_outS);
                if (8 == bitsPerPixel)
                {
                    ColorPalette palette = img.Palette;
                    int length = palette.Entries.Length;
                    for (int index = 0; index < length; ++index)
                        EOStreamUtility.WriteBGRAColor(this.m_outS, palette.Entries[index]);
                }
                img.RotateFlip(RotateFlipType.Rotate180FlipX);
                Rectangle rect = new Rectangle(0, 0, width, height);
                BitmapData bitmapdata = img.LockBits(rect, ImageLockMode.ReadOnly, img.PixelFormat);
                EOStreamUtility.WriteRaw(this.m_outS, bitmapdata.Scan0.ToPointer(), Size);
                img.UnlockBits(bitmapdata);
                img.RotateFlip(RotateFlipType.Rotate180FlipX);
                Bitmap AlphaImg = AlphaImgMask == null ? img : AlphaImgMask;
                byte[] maskdata = new byte[count];
                this.MakeMask(AlphaImg, ref maskdata, MaskRowSize);
                this.m_outS.Write(maskdata, 0, count);
            }
            ++this.m_NumWritten;
            flag = true;
        }
        return flag;
    }

    public enum IcoCurType
    {
        Icon = 1,
        Cursor = 2,
    }

    private struct IcoHeader
    {
        public ushort Reserved;
        public ushort Type;
        public ushort Count;

        public IcoHeader(EOIcoCurWriter.IcoCurType type)
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

        public static unsafe int GetStructSize() => sizeof(EOIcoCurWriter.IcoHeader);

        public void ReadFromStream(Stream inStream)
        {
            this.Reserved = EOStreamUtility.Read_ushort(inStream);
            this.Type = EOStreamUtility.Read_ushort(inStream);
            this.Count = EOStreamUtility.Read_ushort(inStream);
        }

        public void WriteToStream(Stream m_outS)
        {
            EOStreamUtility.Write_ushort(m_outS, this.Reserved);
            EOStreamUtility.Write_ushort(m_outS, this.Type);
            EOStreamUtility.Write_ushort(m_outS, this.Count);
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

        public static unsafe int GetStructSize() => sizeof(EOIcoCurWriter.DirectoryEntry);

        public void ReadFromStream(Stream inStream)
        {
            this.bWidth = (byte)inStream.ReadByte();
            this.bHeight = (byte)inStream.ReadByte();
            this.bColorCount = (byte)inStream.ReadByte();
            this.bReserved = (byte)inStream.ReadByte();
            this.Planes_XHotspot = EOStreamUtility.Read_ushort(inStream);
            this.BitCount_YHotspot = EOStreamUtility.Read_ushort(inStream);
            this.dwBytesInRes = EOStreamUtility.Read_uint(inStream);
            this.dwImageOffset = EOStreamUtility.Read_uint(inStream);
        }

        public void WriteToStream(Stream mOutS)
        {
            mOutS.WriteByte(this.bWidth);
            mOutS.WriteByte(this.bHeight);
            mOutS.WriteByte(this.bColorCount);
            mOutS.WriteByte(this.bReserved);
            EOStreamUtility.Write_ushort(mOutS, this.Planes_XHotspot);
            EOStreamUtility.Write_ushort(mOutS, this.BitCount_YHotspot);
            EOStreamUtility.Write_uint(mOutS, this.dwBytesInRes);
            EOStreamUtility.Write_uint(mOutS, this.dwImageOffset);
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
            this.StructSize = (uint)sizeof(EOIcoCurWriter.BITMAPINFOHEADER);
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

        public static unsafe int GetStructSize() => sizeof(EOIcoCurWriter.BITMAPINFOHEADER);

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

        public void WriteToStream(Stream m_outS)
        {
            EOStreamUtility.Write_uint(m_outS, this.StructSize);
            EOStreamUtility.Write_uint(m_outS, (uint)this.Width);
            EOStreamUtility.Write_uint(m_outS, (uint)this.Height);
            EOStreamUtility.Write_ushort(m_outS, this.Planes);
            EOStreamUtility.Write_ushort(m_outS, this.BitCount);
            EOStreamUtility.Write_uint(m_outS, this.biCompression);
            EOStreamUtility.Write_uint(m_outS, this.biSizeImage);
            EOStreamUtility.Write_uint(m_outS, (uint)this.biXPelsPerMeter);
            EOStreamUtility.Write_uint(m_outS, (uint)this.biYPelsPerMeter);
            EOStreamUtility.Write_uint(m_outS, this.biClrUsed);
            EOStreamUtility.Write_uint(m_outS, this.biClrImportant);
        }
    }
}
