// Decompiled with JetBrains decompiler
// Type: EOOffsetStream
// Assembly: IcoCur, Version=4.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: EEB2FED8-625F-4555-8E27-7F881F458B17
// Assembly location: C:\src\cs\Paint.net\IcoCur.dll

using System.IO;

namespace IcoCur.EvanOlds;

internal sealed class EOOffsetStream : Stream
{
    private Stream m_base;
    private long m_pos;

    public override bool CanRead => this.m_base.CanRead;

    public override bool CanSeek => this.m_base.CanSeek;

    public override bool CanWrite => this.m_base.CanWrite;

    public override long Length => this.m_base.Length - this.m_pos;

    public override long Position
    {
        get => this.m_base.Position - this.m_pos;
        set => this.m_base.Position = this.m_pos + value;
    }

    public EOOffsetStream(Stream underlyingStream)
    {
        this.m_base = underlyingStream;
        this.m_pos = underlyingStream.Position;
    }

    public override void Flush() => this.m_base.Flush();

    public override int Read(byte[] buffer, int offset, int count) => this.m_base.Read(buffer, offset, count);

    public override long Seek(long offset, SeekOrigin origin) => this.m_base.Seek(offset + this.m_pos, origin);

    public override void SetLength(long value) => this.m_base.SetLength(value + this.m_pos);

    public override void Write(byte[] buffer, int offset, int count) => this.m_base.Write(buffer, offset, count);
}
