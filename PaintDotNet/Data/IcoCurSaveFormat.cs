// Decompiled with JetBrains decompiler
// Type: PaintDotNet.Data.IcoCurSaveFormat
// Assembly: IcoCur, Version=4.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: EEB2FED8-625F-4555-8E27-7F881F458B17
// Assembly location: C:\src\cs\Paint.net\IcoCur.dll

using System.Drawing;

namespace PaintDotNet.Data
{
  internal struct IcoCurSaveFormat
  {
    public Size Dimensions;
    public bool EightBit;

    public IcoCurSaveFormat(int width, int height, bool eightBit)
    {
      this.Dimensions = new Size(width, height);
      this.EightBit = eightBit;
    }
  }
}
