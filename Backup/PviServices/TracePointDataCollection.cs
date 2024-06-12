// Decompiled with JetBrains decompiler
// Type: BR.AN.PviServices.TracePointDataCollection
// Assembly: BR.AN.PVIServices, Version=1.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DA853F0A-C070-4E30-B7A7-51D955FB85F6
// Assembly location: C:\Users\ss_six\Desktop\FXE\FXE software integration package\COMET_FXE-PVIServices_Example\Pluto-PVI-trial_01\packages\IToolS.BandR.3.5.58\lib\net20\BR.AN.PVIServices.dll

using System;
using System.Collections;

namespace BR.AN.PviServices
{
  public class TracePointDataCollection : ICollection, IEnumerable, IDisposable
  {
    private bool disposed;
    private ArrayList propArrayList;

    public TracePointDataCollection() => this.propArrayList = new ArrayList();

    public virtual IEnumerator GetEnumerator() => this.propArrayList.GetEnumerator();

    public virtual object SyncRoot => this.propArrayList.SyncRoot;

    public virtual bool IsSynchronized => this.propArrayList.IsSynchronized;

    public virtual int Count => this.propArrayList.Count;

    public virtual void CopyTo(Array array, int count)
    {
    }

    internal int Add(TracePointData tpData)
    {
      this.propArrayList.Add((object) tpData);
      return this.propArrayList.Count;
    }

    public TracePointData this[int indexer] => indexer < this.propArrayList.Count ? (TracePointData) this.propArrayList[indexer] : (TracePointData) null;

    internal void ReadResponseData(
      IntPtr pData,
      uint dataLen,
      uint lenOfRecord,
      ref int dataOffset)
    {
      uint typeLenght;
      uint num1;
      for (uint index = 24; index < lenOfRecord; index = num1 + typeLenght)
      {
        uint iecFormat = PviMarshal.ReadUInt32(pData, ref dataOffset);
        uint num2 = index + 4U;
        typeLenght = PviMarshal.ReadUInt32(pData, ref dataOffset);
        num1 = num2 + 4U;
        this.Add(new TracePointData(iecFormat, typeLenght, pData, dataLen, ref dataOffset));
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
        this.propArrayList.Clear();
      this.disposed = true;
    }

    ~TracePointDataCollection() => this.Dispose(false);
  }
}
