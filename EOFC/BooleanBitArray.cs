// Decompiled with JetBrains decompiler
// Type: EOFC.BooleanBitArray
// Assembly: IcoCur, Version=4.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: EEB2FED8-625F-4555-8E27-7F881F458B17
// Assembly location: C:\src\cs\Paint.net\IcoCur.dll

using System;

namespace EOFC
{
  internal class BooleanBitArray
  {
    private int m_bitSize;
    private byte[] m_data;

    public bool this[int index]
    {
      get => this.Get(index);
      set => BooleanBitArray.Set(this.m_data, index, value);
    }

    public BooleanBitArray(int sizeInBits)
    {
      this.m_bitSize = sizeInBits;
      int length = this.m_bitSize / 8 + 1;
      if (this.m_bitSize % 8 == 0)
        length = this.m_bitSize / 8;
      this.m_data = new byte[length];
      if (this.m_data == null)
        throw new OutOfMemoryException("Could not allocate " + Convert.ToString(length) + " bytes of memory for boolean bit array");
      Array.Clear((Array) this.m_data, 0, this.m_data.Length);
    }

    public bool Get(int bitIndex)
    {
      byte num = (byte) (1 << bitIndex % 8);
      return ((int) this.m_data[bitIndex / 8] & (int) num) != 0;
    }

    public static unsafe bool Get(byte* data, int BitIndex)
    {
      byte num = (byte) (1 << BitIndex % 8);
      return ((int) data[BitIndex / 8] & (int) num) != 0;
    }

    public static bool Get(byte[] data, int bitIndex)
    {
      byte num = (byte) (1 << bitIndex % 8);
      return ((int) data[bitIndex / 8] & (int) num) != 0;
    }

    public static unsafe void Set(byte* data, int BitIndex, bool value)
    {
      if (value)
      {
        byte num1 = (byte) (1 << BitIndex % 8);
        byte* numPtr = data + BitIndex / 8;
        int num2 = (int) (byte) ((uint) *numPtr | (uint) num1);
        *numPtr = (byte) num2;
      }
      else
      {
        byte num3 = ~(byte) (1 << BitIndex % 8);
        byte* numPtr = data + BitIndex / 8;
        int num4 = (int) (byte) ((uint) *numPtr & (uint) num3);
        *numPtr = (byte) num4;
      }
    }

    public static void Set(byte[] data, int BitIndex, bool value)
    {
      if (value)
      {
        byte num = (byte) (1 << BitIndex % 8);
        int index = BitIndex / 8;
        data[index] |= num;
      }
      else
      {
        byte num = ~(byte) (1 << BitIndex % 8);
        int index = BitIndex / 8;
        data[index] &= num;
      }
    }

    public static void SetMSbFirst(byte[] data, int BitIndex, bool value)
    {
      if (value)
      {
        byte num = (byte) (1 << 7 - BitIndex % 8);
        int index = BitIndex / 8;
        data[index] |= num;
      }
      else
      {
        byte num = ~(byte) (1 << 7 - BitIndex % 8);
        int index = BitIndex / 8;
        data[index] &= num;
      }
    }
  }
}
