// Decompiled with JetBrains decompiler
// Type: EOFC.BooleanBitArray
// Assembly: IcoCur, Version=4.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: EEB2FED8-625F-4555-8E27-7F881F458B17
// Assembly location: C:\src\cs\Paint.net\IcoCur.dll

using System;

namespace IcoCur.EvanOlds;

internal class BooleanBitArray
{
    private int m_bitSize;
    private byte[] m_data;

    public bool this[int index]
    {
        get => Get(index);
        set => Set(m_data, index, value);
    }

    public BooleanBitArray(int sizeInBits)
    {
        m_bitSize = sizeInBits;
        int length = m_bitSize / 8 + 1;
        if (m_bitSize % 8 == 0)
            length = m_bitSize / 8;
        m_data = new byte[length];
        if (m_data == null)
            throw new OutOfMemoryException("Could not allocate " + Convert.ToString(length) + " bytes of memory for boolean bit array");
        Array.Clear(m_data, 0, m_data.Length);
    }

    public bool Get(int bitIndex)
    {
        byte num = (byte)(1 << bitIndex % 8);
        return (m_data[bitIndex / 8] & num) != 0;
    }

    public static unsafe bool Get(byte* data, int bitIndex)
    {
        byte num = (byte)(1 << bitIndex % 8);
        return (data[bitIndex / 8] & num) != 0;
    }

    public static bool Get(byte[] data, int bitIndex)
    {
        byte num = (byte)(1 << bitIndex % 8);
        return (data[bitIndex / 8] & num) != 0;
    }

    public static unsafe void Set(byte* data, int BitIndex, bool value)
    {
        if (value)
        {
            byte num1 = (byte)(1 << BitIndex % 8);
            byte* numPtr = data + BitIndex / 8;
            int num2 = (byte)(*numPtr | (uint)num1);
            *numPtr = (byte)num2;
        }
        else
        {
            byte num3 = (byte)~(1 << BitIndex % 8);
            byte* numPtr = data + BitIndex / 8;
            int num4 = (byte)(*numPtr & (uint)num3);
            *numPtr = (byte)num4;
        }
    }

    public static void Set(byte[] data, int BitIndex, bool value)
    {
        if (value)
        {
            byte num = (byte)(1 << BitIndex % 8);
            int index = BitIndex / 8;
            data[index] |= num;
        }
        else
        {
            byte num = (byte)~(1 << BitIndex % 8);
            int index = BitIndex / 8;
            data[index] &= num;
        }
    }

    public static void SetMSbFirst(byte[] data, int BitIndex, bool value)
    {
        if (value)
        {
            byte num = (byte)(1 << 7 - BitIndex % 8);
            int index = BitIndex / 8;
            data[index] |= num;
        }
        else
        {
            byte num = (byte)~(1 << 7 - BitIndex % 8);
            int index = BitIndex / 8;
            data[index] &= num;
        }
    }
}