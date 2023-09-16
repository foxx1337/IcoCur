// Decompiled with JetBrains decompiler
// Type: EOStreamUtility
// Assembly: IcoCur, Version=4.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: EEB2FED8-625F-4555-8E27-7F881F458B17
// Assembly location: C:\src\cs\Paint.net\IcoCur.dll

using System;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;

namespace IcoCur.EvanOlds;

internal class EOStreamUtility
{
    public static unsafe void LoadStreamDataToAllocatedBlock(Stream s, void* mem)
    {
        byte[] buffer = new byte[s.Length];
        s.Read(buffer, 0, (int)s.Length);
        int length = (int)s.Length;
        for (int index = 0; index < length; ++index)
            *(sbyte*)((IntPtr)mem + index) = (sbyte)buffer[index];
    }

    public static unsafe void ReadRaw(Stream s, void* mem, int ReadSize)
    {
        byte[] numArray = new byte[ReadSize];
        s.Read(numArray, 0, ReadSize);
        Marshal.Copy(numArray, 0, new IntPtr(mem), ReadSize);
    }

    public static int ReadInt(Stream inS)
    {
        byte[] buffer = new byte[4];
        inS.Read(buffer, 0, 4);
        return (int)buffer[0] | (int)buffer[1] << 8 | (int)buffer[2] << 16 | (int)buffer[3] << 24;
    }

    public static uint Read_uint(Stream inS)
    {
        int num1 = inS.ReadByte();
        int num2 = inS.ReadByte();
        int num3 = inS.ReadByte();
        int num4 = inS.ReadByte();
        if (num1 == -1)
        {
            int num5;
            num4 = num5 = 0;
            num3 = num5;
            num1 = num5;
            num2 = num5;
        }
        else if (num2 == -1)
        {
            int num6;
            num4 = num6 = 0;
            num2 = num6;
            num3 = num6;
        }
        else if (num3 == -1)
            num4 = num3 = 0;
        else if (num4 == -1)
            num4 = 0;
        return (uint)(num1 | num2 << 8 | num3 << 16 | num4 << 24);
    }

    public static uint[] ReadUInts(Stream s, int Count)
    {
        byte[] buffer = new byte[Count * 4];
        uint[] numArray = new uint[Count];
        s.Read(buffer, 0, Count * 4);
        for (int index = 0; index < Count; ++index)
            numArray[index] = (uint)buffer[index * 4] | (uint)buffer[index * 4 + 1] | (uint)buffer[index * 4 + 2] | (uint)buffer[index * 4 + 3];
        return numArray;
    }

    public static ushort Read_ushort(Stream inS) => (ushort)(inS.ReadByte() | inS.ReadByte() << 8);

    public static void WriteBGRAColor(Stream outS, Color BGRA)
    {
        outS.WriteByte(BGRA.B);
        outS.WriteByte(BGRA.G);
        outS.WriteByte(BGRA.R);
        outS.WriteByte(BGRA.A);
    }

    public static unsafe bool WriteRaw(Stream outS, void* Data, int Size)
    {
        try
        {
            for (int index = 0; index < Size; ++index)
                outS.WriteByte(*(byte*)((IntPtr)Data + index));
        }
        catch (Exception ex)
        {
            return false;
        }
        return true;
    }

    public static unsafe bool WriteRaw(Stream outS, void* Data, uint Size)
    {
        try
        {
            for (uint index = 0; index < Size; ++index)
                outS.WriteByte(*(byte*)((IntPtr)Data + (int)index));
        }
        catch (Exception ex)
        {
            return false;
        }
        return true;
    }

    public static bool WriteString(Stream outS, string s, bool includeNullTerminator)
    {
        try
        {
            int length = s.Length;
            char[] charArray = s.ToCharArray();
            for (int index = 0; index < length; ++index)
                outS.WriteByte((byte)charArray[index]);
            if (includeNullTerminator)
                outS.WriteByte((byte)0);
        }
        catch (Exception ex)
        {
            return false;
        }
        return true;
    }

    public static void Write_ushort(Stream outS, ushort word)
    {
        outS.WriteByte((byte)((uint)word & (uint)byte.MaxValue));
        outS.WriteByte((byte)((uint)word >> 8));
    }

    public static void Write_uint(Stream outS, uint dword)
    {
        byte[] buffer = new byte[4]
        {
      (byte) dword,
      (byte) (dword >> 8),
      (byte) (dword >> 16),
      (byte) (dword >> 24)
        };
        outS.Write(buffer, 0, 4);
    }
}
