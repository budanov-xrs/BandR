// Decompiled with JetBrains decompiler
// Type: BR.AN.PviServices.TracePointsData
// Assembly: BR.AN.PVIServices, Version=1.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DA853F0A-C070-4E30-B7A7-51D955FB85F6
// Assembly location: C:\Users\ss_six\Desktop\FXE\FXE software integration package\COMET_FXE-PVIServices_Example\Pluto-PVI-trial_01\packages\IToolS.BandR.3.5.58\lib\net20\BR.AN.PVIServices.dll

using System;
using System.Collections;

namespace BR.AN.PviServices
{
  public class TracePointsData : IDisposable
  {
    private bool disposed;
    private uint propID;
    private ulong propOffset;
    private TracePointDataCollection propTraceData;

    private void InitMembers()
    {
      this.propID = 0U;
      this.propOffset = 0UL;
      this.propTraceData = (TracePointDataCollection) null;
    }

    public TracePointsData() => this.InitMembers();

    internal TracePointsData(IntPtr pData, uint dataLen, ref int dataOffset)
    {
      this.InitMembers();
      this.propTraceData = new TracePointDataCollection();
      uint lenOfRecord = PviMarshal.ReadUInt32(pData, ref dataOffset);
      this.propID = PviMarshal.ReadUInt32(pData, ref dataOffset);
      int num1 = (int) PviMarshal.ReadUInt32(pData, ref dataOffset);
      int num2 = (int) PviMarshal.ReadUInt32(pData, ref dataOffset);
      this.propOffset = PviMarshal.ReadUInt64(pData, ref dataOffset);
      this.propTraceData.ReadResponseData(pData, dataLen, lenOfRecord, ref dataOffset);
    }

    [CLSCompliant(false)]
    public uint ID => this.propID;

    [CLSCompliant(false)]
    public ulong Offset => this.propOffset;

    public TracePointDataCollection TraceData => this.propTraceData;

    internal void UpdateFormat(ArrayList formatTypes, ArrayList typeLenghts)
    {
      if (this.propTraceData == null)
        this.propTraceData = new TracePointDataCollection();
      for (int index = 0; index < formatTypes.Count; ++index)
      {
        TracePointData tpData;
        if (index < this.propTraceData.Count)
        {
          tpData = this.propTraceData[index];
        }
        else
        {
          tpData = new TracePointData();
          this.propTraceData.Add(tpData);
        }
        tpData.UpdateFormat((uint) formatTypes[index], (uint) typeLenghts[index]);
      }
    }

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
        this.propTraceData.Dispose();
      this.disposed = true;
    }

    ~TracePointsData() => this.Dispose(false);

    public override string ToString() => "ID=\"" + this.propID.ToString() + "\" Offset=\"" + this.propOffset.ToString() + "\"  Records=\"" + this.propTraceData.Count.ToString() + "\"";
  }
}
