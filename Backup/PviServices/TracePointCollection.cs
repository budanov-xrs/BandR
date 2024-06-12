// Decompiled with JetBrains decompiler
// Type: BR.AN.PviServices.TracePointCollection
// Assembly: BR.AN.PVIServices, Version=1.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DA853F0A-C070-4E30-B7A7-51D955FB85F6
// Assembly location: C:\Users\ss_six\Desktop\FXE\FXE software integration package\COMET_FXE-PVIServices_Example\Pluto-PVI-trial_01\packages\IToolS.BandR.3.5.58\lib\net20\BR.AN.PVIServices.dll

using System;
using System.Collections;

namespace BR.AN.PviServices
{
  public class TracePointCollection : IDisposable
  {
    private Hashtable propTPKeys;
    private ArrayList propItems;

    internal TracePointCollection(Task task)
    {
      this.propItems = new ArrayList();
      this.propTPKeys = new Hashtable();
    }

    internal int Add(TracePoint trcPoint)
    {
      if (this.propTPKeys.ContainsKey((object) trcPoint.Name))
        return -2;
      this.propTPKeys.Add((object) trcPoint.Name, (object) this.propItems.Count);
      return this.propItems.Add((object) trcPoint);
    }

    public int Count => this.propItems == null ? 0 : this.propItems.Count;

    public int Contains(string nameOfVariable) => this.propTPKeys.ContainsKey((object) nameOfVariable) ? (int) this.propTPKeys[(object) nameOfVariable] : -1;

    public TracePoint this[int index] => index < this.propItems.Count ? (TracePoint) this.propItems[index] : (TracePoint) null;

    public ArrayList Keys
    {
      get
      {
        ArrayList keys = new ArrayList();
        if (this.propItems != null)
        {
          for (int index = 0; index < this.propItems.Count; ++index)
            keys.Add((object) ((Base) this.propItems[index]).Name);
        }
        return keys;
      }
    }

    public void Clear()
    {
      for (int index = 0; index < this.propItems.Count; ++index)
        ((Base) this.propItems[index]).Dispose();
      this.propItems.Clear();
      this.propTPKeys.Clear();
    }

    public void Dispose()
    {
      for (int index = 0; index < this.propItems.Count; ++index)
        ((Base) this.propItems[index]).Dispose();
      this.propItems = (ArrayList) null;
      if (this.propTPKeys != null)
      {
        this.propTPKeys.Clear();
        this.propTPKeys = (Hashtable) null;
      }
      GC.SuppressFinalize((object) this);
    }
  }
}
