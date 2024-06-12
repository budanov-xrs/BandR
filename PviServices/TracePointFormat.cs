// Decompiled with JetBrains decompiler
// Type: BR.AN.PviServices.TracePointFormat
// Assembly: BR.AN.PVIServices, Version=1.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DA853F0A-C070-4E30-B7A7-51D955FB85F6
// Assembly location: C:\Users\ss_six\Desktop\FXE\FXE software integration package\COMET_FXE-PVIServices_Example\Pluto-PVI-trial_01\packages\IToolS.BandR.3.5.58\lib\net20\BR.AN.PVIServices.dll

using System;
using System.Collections;

namespace BR.AN.PviServices
{
  public class TracePointFormat : IDisposable
  {
    private bool disposed;
    private uint propID;
    private ArrayList propPVFormats;
    private ArrayList propPVLengths;

    private void InitMembers()
    {
      this.propID = 0U;
      this.propPVFormats = new ArrayList();
      this.propPVLengths = new ArrayList();
    }

    public TracePointFormat() => this.InitMembers();

    internal TracePointFormat(IntPtr pData, uint dataLen, ref int dataOffset)
    {
      this.InitMembers();
      uint num1 = PviMarshal.ReadUInt32(pData, ref dataOffset);
      this.propID = PviMarshal.ReadUInt32(pData, ref dataOffset);
      for (uint index = 8; index < num1; index += 8U)
      {
        uint num2 = PviMarshal.ReadUInt32(pData, ref dataOffset);
        uint num3 = PviMarshal.ReadUInt32(pData, ref dataOffset);
        this.propPVFormats.Add((object) num2);
        this.propPVLengths.Add((object) num3);
      }
    }

    [CLSCompliant(false)]
    public uint ID => this.propID;

    public ArrayList VariableFormats => this.propPVFormats;

    public ArrayList VariableLengths => this.propPVLengths;

    public void Dispose()
    {
      this.Dispose(true);
      GC.SuppressFinalize((object) this);
    }

    protected virtual void Dispose(bool disposing)
    {
      if (this.disposed)
        return;
      if (disposing)
      {
        this.propPVFormats.Clear();
        this.propPVLengths.Clear();
      }
      this.disposed = true;
    }

    ~TracePointFormat() => this.Dispose(false);

    public override string ToString()
    {
      string str1 = "";
      string str2 = "";
      for (int index = 0; index < this.propPVFormats.Count; ++index)
      {
        IECDataTypes propPvFormat = (IECDataTypes) (uint) this.propPVFormats[index];
        if (0 < index)
        {
          str1 = str1 + "," + propPvFormat.ToString();
          str2 = str2 + "," + this.propPVLengths[index].ToString();
        }
        else
        {
          str1 += propPvFormat.ToString();
          str2 += this.propPVLengths[index].ToString();
        }
      }
      return "ID=\"" + this.propID.ToString() + "\" Formats=\"" + str1 + "\" Lengths=\"" + str2 + "\"";
    }
  }
}
