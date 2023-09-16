// Decompiled with JetBrains decompiler
// Type: PaintDotNet.Data.CurSaveConfigToken
// Assembly: IcoCur, Version=4.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: EEB2FED8-625F-4555-8E27-7F881F458B17
// Assembly location: C:\src\cs\Paint.net\IcoCur.dll

using System;
using System.Drawing;
using PaintDotNet;

namespace IcoCur;

[Serializable]
public class CurSaveConfigToken : SaveConfigToken
{
    public Point HotSpot;
    public bool EightBit;

    public override object Clone() => new CurSaveConfigToken(this);

    public CurSaveConfigToken()
    {
        HotSpot = new Point(0, 0);
        EightBit = false;
    }

    protected CurSaveConfigToken(CurSaveConfigToken copyMe)
    {
        HotSpot = copyMe.HotSpot;
        EightBit = copyMe.EightBit;
    }
}
