// Decompiled with JetBrains decompiler
// Type: BR.AN.PviServices.TraceDataCollection
// Assembly: BR.AN.PVIServices, Version=1.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DA853F0A-C070-4E30-B7A7-51D955FB85F6
// Assembly location: C:\Users\ss_six\Desktop\FXE\FXE software integration package\COMET_FXE-PVIServices_Example\Pluto-PVI-trial_01\packages\IToolS.BandR.3.5.58\lib\net20\BR.AN.PVIServices.dll

using System;
using System.Collections;

namespace BR.AN.PviServices
{
  public class TraceDataCollection : IDisposable
  {
    private ArrayList propItems;

    internal TraceDataCollection() => this.propItems = new ArrayList();

    public int Count => this.propItems == null ? 0 : this.propItems.Count;

    internal void Add(TraceData trcData) => this.propItems.Add((object) trcData);

    public TraceData this[int index] => index < this.propItems.Count ? (TraceData) this.propItems[index] : (TraceData) null;

    public void Dispose()
    {
      if (this.propItems != null)
      {
        this.propItems.Clear();
        this.propItems = (ArrayList) null;
      }
      GC.SuppressFinalize((object) this);
    }
  }
}
