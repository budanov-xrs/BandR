// Decompiled with JetBrains decompiler
// Type: BR.AN.PviServices.TracePointDescription
// Assembly: BR.AN.PVIServices, Version=1.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DA853F0A-C070-4E30-B7A7-51D955FB85F6
// Assembly location: C:\Users\ss_six\Desktop\FXE\FXE software integration package\COMET_FXE-PVIServices_Example\Pluto-PVI-trial_01\packages\IToolS.BandR.3.5.58\lib\net20\BR.AN.PVIServices.dll

using System;
using System.Collections;

namespace BR.AN.PviServices
{
  public class TracePointDescription : IDisposable
  {
    private bool disposed;
    private uint propRecordLen;
    private uint propID;
    private ulong propOffset;
    private ArrayList propListOfVars;
    private object propUserData;
    private TracePointDataCollection propTraceData;

    public TracePointDescription()
    {
      this.propID = 0U;
      this.propOffset = 0UL;
      this.propListOfVars = (ArrayList) null;
      this.propRecordLen = 0U;
      this.propUserData = (object) null;
      this.propTraceData = (TracePointDataCollection) null;
    }

    internal TracePointDescription(
      uint id,
      ulong offset,
      ArrayList listOfVariables,
      uint recordLen)
    {
      this.propID = id;
      this.propOffset = offset;
      this.propListOfVars = new ArrayList((ICollection) listOfVariables);
      this.propRecordLen = recordLen;
      this.propUserData = (object) null;
      this.propTraceData = (TracePointDataCollection) null;
    }

    internal uint RecordLen => this.propRecordLen;

    [CLSCompliant(false)]
    public uint ID => this.propID;

    [CLSCompliant(false)]
    public ulong Offset => this.propOffset;

    public ArrayList ListOfVariables => this.propListOfVars;

    public object UserData
    {
      get => this.propUserData;
      set => this.propUserData = value;
    }

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

    internal void UpdateFormatNData(TracePointsData tpDat)
    {
      if (this.propTraceData == null)
        this.propTraceData = new TracePointDataCollection();
      for (int indexer = 0; indexer < tpDat.TraceData.Count; ++indexer)
      {
        TracePointData tpData;
        if (indexer < this.propTraceData.Count)
        {
          tpData = this.propTraceData[indexer];
        }
        else
        {
          tpData = new TracePointData();
          this.propTraceData.Add(tpData);
        }
        tpData.UpdateData(tpDat.TraceData[indexer].IECType, tpDat.TraceData[indexer].TypeLength, tpDat.TraceData[indexer].DataBytes);
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
        this.propListOfVars.Clear();
      this.disposed = true;
    }

    ~TracePointDescription() => this.Dispose(false);

    public override string ToString() => "ID=\"" + this.propID.ToString() + "\" Offset=\"" + this.propOffset.ToString() + "\" RecordLen=\"" + this.propRecordLen.ToString() + "\" NumOfVars=\"" + this.propListOfVars.Count.ToString() + "\" NumOfData=\"" + this.propTraceData.Count.ToString() + "\"";
  }
}
