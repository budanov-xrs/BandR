// Decompiled with JetBrains decompiler
// Type: BR.AN.PviServices.TracePointFormatCollection
// Assembly: BR.AN.PVIServices, Version=1.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DA853F0A-C070-4E30-B7A7-51D955FB85F6
// Assembly location: C:\Users\ss_six\Desktop\FXE\FXE software integration package\COMET_FXE-PVIServices_Example\Pluto-PVI-trial_01\packages\IToolS.BandR.3.5.58\lib\net20\BR.AN.PVIServices.dll

using System;
using System.Collections;

namespace BR.AN.PviServices
{
  public class TracePointFormatCollection : ICollection, IEnumerable, IDisposable
  {
    private bool disposed;
    private ArrayList propArrayList;
    private Hashtable propMapIDToObj;

    public TracePointFormatCollection()
    {
      this.propArrayList = new ArrayList();
      this.propMapIDToObj = new Hashtable();
    }

    public virtual IEnumerator GetEnumerator() => this.propArrayList.GetEnumerator();

    public virtual object SyncRoot => this.propArrayList.SyncRoot;

    public virtual bool IsSynchronized => this.propArrayList.IsSynchronized;

    public virtual int Count => this.propArrayList.Count;

    public virtual void CopyTo(Array array, int count)
    {
    }

    public virtual ICollection Keys => this.propMapIDToObj.Keys;

    internal int ReadResponseData(IntPtr pData, uint dataLen)
    {
      int dataOffset = 0;
      while ((long) dataOffset < (long) dataLen)
      {
        TracePointFormat tracePointFormat = new TracePointFormat(pData, dataLen, ref dataOffset);
        this.propMapIDToObj.Add((object) tracePointFormat.ID, (object) tracePointFormat);
        this.propArrayList.Add((object) tracePointFormat);
      }
      return this.propArrayList.Count;
    }

    public TracePointFormat this[int indexer] => indexer < this.propArrayList.Count ? (TracePointFormat) this.propArrayList[indexer] : (TracePointFormat) null;

    [CLSCompliant(false)]
    public TracePointFormat this[uint key] => this.propMapIDToObj.ContainsKey((object) key) ? (TracePointFormat) this.propMapIDToObj[(object) key] : (TracePointFormat) null;

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

    ~TracePointFormatCollection() => this.Dispose(false);
  }
}
