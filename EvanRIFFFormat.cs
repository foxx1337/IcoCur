// Decompiled with JetBrains decompiler
// Type: EvanRIFFFormat
// Assembly: IcoCur, Version=4.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: EEB2FED8-625F-4555-8E27-7F881F458B17
// Assembly location: C:\src\cs\Paint.net\IcoCur.dll

using System.Collections.Generic;
using System.IO;

internal class EvanRIFFFormat
{
  public EvanRIFFFormat.Chunk MasterChunk;

  public EvanRIFFFormat() => this.MasterChunk = (EvanRIFFFormat.Chunk) null;

  private void AddAllChunks(
    List<EvanRIFFFormat.Chunk> list,
    EvanRIFFFormat.Chunk chunk,
    int ChunkID)
  {
    if (chunk.Children == null)
      return;
    for (int index = 0; index < chunk.Children.Count; ++index)
    {
      if (chunk.Children[index].ID == ChunkID)
        list.Add(chunk.Children[index]);
      this.AddAllChunks(list, chunk.Children[index], ChunkID);
    }
  }

  public List<EvanRIFFFormat.Chunk> FindAllChunks(int ChunkID)
  {
    List<EvanRIFFFormat.Chunk> list = new List<EvanRIFFFormat.Chunk>();
    this.AddAllChunks(list, this.MasterChunk, ChunkID);
    return list;
  }

  public EvanRIFFFormat.Chunk FindFirstChunk(int ChunkID) => this.MasterChunk != null ? this.FindInChunk(this.MasterChunk, ChunkID) : (EvanRIFFFormat.Chunk) null;

  private EvanRIFFFormat.Chunk FindInChunk(EvanRIFFFormat.Chunk chunk, int ChunkID)
  {
    EvanRIFFFormat.Chunk inChunk1;
    if (chunk.Children == null)
    {
      inChunk1 = (EvanRIFFFormat.Chunk) null;
    }
    else
    {
      for (int index = 0; index < chunk.Children.Count; ++index)
      {
        if (chunk.Children[index].ID == ChunkID)
          return chunk.Children[index];
        EvanRIFFFormat.Chunk inChunk2 = this.FindInChunk(chunk.Children[index], ChunkID);
        if (inChunk2 != null)
          return inChunk2;
      }
      inChunk1 = (EvanRIFFFormat.Chunk) null;
    }
    return inChunk1;
  }

  private void GetLISTChildChunks(EvanRIFFFormat.Chunk chnk, Stream s)
  {
    long position = s.Position;
    s.Seek(chnk.StreamOffset + 12L, SeekOrigin.Begin);
    chnk.Children = new List<EvanRIFFFormat.Chunk>();
    while (s.Position < chnk.StreamOffset + (long) chnk.Size + 8L)
    {
      EvanRIFFFormat.Chunk chunk = new EvanRIFFFormat.Chunk();
      chunk.StreamOffset = s.Position;
      chunk.ID = EOStreamUtility.ReadInt(s);
      chunk.Size = EOStreamUtility.ReadInt(s);
      chnk.Children.Add(chunk);
      s.Seek((long) chunk.Size, SeekOrigin.Current);
    }
    s.Seek(position, SeekOrigin.Begin);
  }

  public int InitFromStream(Stream s)
  {
    long position = s.Position;
    int num1 = 1179011410;
    int num2 = EOStreamUtility.ReadInt(s);
    int num3;
    if (num2 != num1)
    {
      num3 = -1;
    }
    else
    {
      this.MasterChunk = new EvanRIFFFormat.Chunk();
      this.MasterChunk.ID = num2;
      this.MasterChunk.StreamOffset = position;
      this.MasterChunk.Size = EOStreamUtility.ReadInt(s);
      this.MasterChunk.HeaderID = EOStreamUtility.ReadInt(s);
      this.MasterChunk.Children = new List<EvanRIFFFormat.Chunk>();
      int num4 = 1414744396;
      while (s.Position < position + (long) this.MasterChunk.Size + 8L && s.Position < s.Length)
      {
        EvanRIFFFormat.Chunk chnk = new EvanRIFFFormat.Chunk();
        chnk.StreamOffset = s.Position;
        chnk.ID = EOStreamUtility.ReadInt(s);
        chnk.Size = EOStreamUtility.ReadInt(s);
        if (chnk.ID == num4)
        {
          chnk.HeaderID = EOStreamUtility.ReadInt(s);
          this.GetLISTChildChunks(chnk, s);
        }
        this.MasterChunk.Children.Add(chnk);
        if (chnk.ID == num4)
          s.Seek((long) (chnk.Size - 4), SeekOrigin.Current);
        else
          s.Seek((long) chnk.Size, SeekOrigin.Current);
      }
      num3 = 0;
    }
    return num3;
  }

  public class Chunk
  {
    public int ID;
    public int Size;
    public int HeaderID;
    public long StreamOffset;
    public List<EvanRIFFFormat.Chunk> Children;

    public Chunk()
    {
      this.ID = 0;
      this.Size = 0;
      this.HeaderID = 0;
      this.StreamOffset = 0L;
    }

    public long DataOffset() => this.StreamOffset + 8L;

    public string GetStringID() => this.IntToString(this.ID);

    public string GetStringHeaderID() => this.HeaderID != 0 ? this.IntToString(this.HeaderID) : (string) null;

    private string IntToString(int i) => new string(new char[4]
    {
      (char) (i & (int) byte.MaxValue),
      (char) (i >> 8 & (int) byte.MaxValue),
      (char) (i >> 16 & (int) byte.MaxValue),
      (char) (i >> 24 & (int) byte.MaxValue)
    });
  }
}
