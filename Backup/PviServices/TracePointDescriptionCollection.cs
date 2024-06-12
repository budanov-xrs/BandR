// Decompiled with JetBrains decompiler
// Type: BR.AN.PviServices.TracePointDescriptionCollection
// Assembly: BR.AN.PVIServices, Version=1.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DA853F0A-C070-4E30-B7A7-51D955FB85F6
// Assembly location: C:\Users\ss_six\Desktop\FXE\FXE software integration package\COMET_FXE-PVIServices_Example\Pluto-PVI-trial_01\packages\IToolS.BandR.3.5.58\lib\net20\BR.AN.PVIServices.dll

using System;
using System.Collections;

namespace BR.AN.PviServices
{
  public class TracePointDescriptionCollection : ICollection, IEnumerable, IDisposable
  {
    private bool disposed;
    private ArrayList propArrayList;
    private Hashtable propMapIDToObj;
    private int propPVIDataSize;

    public TracePointDescriptionCollection()
    {
      this.propArrayList = new ArrayList();
      this.propMapIDToObj = new Hashtable();
      this.propPVIDataSize = 0;
    }

    public virtual IEnumerator GetEnumerator() => this.propArrayList.GetEnumerator();

    public virtual object SyncRoot => this.propArrayList.SyncRoot;

    public virtual bool IsSynchronized => this.propArrayList.IsSynchronized;

    public virtual int Count => this.propArrayList.Count;

    public virtual void CopyTo(Array array, int count)
    {
    }

    public virtual ICollection Keys => this.propMapIDToObj.Keys;

    [CLSCompliant(false)]
    public int Add(uint id, ulong offset, ArrayList listOfVariables)
    {
      int num = 0;
      if (this.propMapIDToObj.ContainsKey((object) id))
        return -1;
      for (int index = 0; index < listOfVariables.Count; ++index)
      {
        num += listOfVariables[index].ToString().Length;
        if (index != 0)
          ++num;
      }
      uint recordLen = (uint) (25 + num);
      TracePointDescription pointDescription = new TracePointDescription(id, offset, listOfVariables, recordLen);
      this.propMapIDToObj.Add((object) id, (object) pointDescription);
      this.propArrayList.Add((object) pointDescription);
      this.propPVIDataSize += (int) recordLen;
      return this.propArrayList.Count;
    }

    [CLSCompliant(false)]
    public uint UpdateFormat(TracePointFormatCollection traceFormats)
    {
      for (int indexer = 0; indexer < traceFormats.Count; ++indexer)
      {
        TracePointFormat traceFormat = traceFormats[indexer];
        TracePointDescription pointDescription = this[traceFormat.ID];
        if (pointDescription == null)
          return traceFormat.ID;
        pointDescription.UpdateFormat(traceFormat.VariableFormats, traceFormat.VariableLengths);
      }
      return uint.MaxValue;
    }

    [CLSCompliant(false)]
    public uint UpdateTracePointsData(TracePointFormatCollection traceFormats)
    {
      for (int indexer = 0; indexer < traceFormats.Count; ++indexer)
      {
        TracePointFormat traceFormat = traceFormats[indexer];
        TracePointDescription pointDescription = this[traceFormat.ID];
        if (pointDescription == null)
          return traceFormat.ID;
        pointDescription.UpdateFormat(traceFormat.VariableFormats, traceFormat.VariableLengths);
      }
      return uint.MaxValue;
    }

    [CLSCompliant(false)]
    public uint UpdateTracePointsData(TracePointsDataCollection traceDataCol)
    {
      for (int indexer = 0; indexer < traceDataCol.Count; ++indexer)
      {
        TracePointsData tpDat = traceDataCol[indexer];
        TracePointDescription pointDescription = this[tpDat.ID];
        if (pointDescription == null)
          return tpDat.ID;
        pointDescription.UpdateFormatNData(tpDat);
      }
      return uint.MaxValue;
    }

    internal int PVIDataSize => this.propPVIDataSize;

    public TracePointDescription this[int indexer] => indexer < this.propArrayList.Count ? (TracePointDescription) this.propArrayList[indexer] : (TracePointDescription) null;

    [CLSCompliant(false)]
    public TracePointDescription this[uint key] => this.propMapIDToObj.ContainsKey((object) key) ? (TracePointDescription) this.propMapIDToObj[(object) key] : (TracePointDescription) null;

    public void Clear()
    {
      this.propArrayList.Clear();
      this.propMapIDToObj.Clear();
      this.propPVIDataSize = 0;
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
      {
        this.propArrayList.Clear();
        this.propMapIDToObj.Clear();
      }
      this.disposed = true;
    }

    ~TracePointDescriptionCollection() => this.Dispose(false);
  }
}
