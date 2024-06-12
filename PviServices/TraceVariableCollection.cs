// Decompiled with JetBrains decompiler
// Type: BR.AN.PviServices.TraceVariableCollection
// Assembly: BR.AN.PVIServices, Version=1.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DA853F0A-C070-4E30-B7A7-51D955FB85F6
// Assembly location: C:\Users\ss_six\Desktop\FXE\FXE software integration package\COMET_FXE-PVIServices_Example\Pluto-PVI-trial_01\packages\IToolS.BandR.3.5.58\lib\net20\BR.AN.PVIServices.dll

using System;
using System.Collections;

namespace BR.AN.PviServices
{
  public class TraceVariableCollection : IDisposable
  {
    private TracePoint propParentTracePoint;
    private Hashtable propTPKeys;
    private ArrayList propItems;

    internal TraceVariableCollection(TracePoint tracePoint)
    {
      this.propParentTracePoint = tracePoint;
      this.propItems = new ArrayList();
      this.propTPKeys = new Hashtable();
    }

    internal int Add(string name)
    {
      if (this.propTPKeys.ContainsKey((object) name))
        return -2;
      this.propTPKeys.Add((object) name, (object) this.propItems.Count);
      return this.propItems.Add((object) new TracePointVariable(this.propParentTracePoint, name));
    }

    public int Count => this.propItems == null ? 0 : this.propItems.Count;

    public int Contains(string nameOfVariable) => this.propTPKeys.ContainsKey((object) nameOfVariable) ? (int) this.propTPKeys[(object) nameOfVariable] : -1;

    public TracePointVariable this[int index] => index < this.propItems.Count ? (TracePointVariable) this.propItems[index] : (TracePointVariable) null;

    public ArrayList Keys
    {
      get
      {
        ArrayList keys = new ArrayList();
        if (this.propItems != null)
        {
          for (int index = 0; index < this.propItems.Count; ++index)
            keys.Add((object) ((TracePointVariable) this.propItems[index]).Name);
        }
        return keys;
      }
    }

    public void Clear()
    {
      for (int index = 0; index < this.propItems.Count; ++index)
        ((TracePointVariable) this.propItems[index]).Dispose();
      this.propItems.Clear();
    }

    public void Dispose()
    {
      this.propParentTracePoint = (TracePoint) null;
      if (this.propItems != null)
      {
        for (int index = 0; index < this.propItems.Count; ++index)
          ((TracePointVariable) this.propItems[index]).Dispose();
      }
      this.propItems = (ArrayList) null;
      GC.SuppressFinalize((object) this);
    }
  }
}
