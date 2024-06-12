// Decompiled with JetBrains decompiler
// Type: BR.AN.PviServices.SNMPCollectionBase
// Assembly: BR.AN.PVIServices, Version=1.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DA853F0A-C070-4E30-B7A7-51D955FB85F6
// Assembly location: C:\Users\ss_six\Desktop\FXE\FXE software integration package\COMET_FXE-PVIServices_Example\Pluto-PVI-trial_01\packages\IToolS.BandR.3.5.58\lib\net20\BR.AN.PVIServices.dll

using System;
using System.Collections;

namespace BR.AN.PviServices
{
  public class SNMPCollectionBase : SNMPBase, ICollection, IEnumerable
  {
    internal Hashtable propItems;
    protected int propRequestCount;
    protected bool propRequesting;

    internal SNMPCollectionBase(string name, SNMPBase parentObj)
      : base(name, parentObj)
    {
      this.propItems = new Hashtable();
      this.propRequestCount = 0;
      this.propRequesting = false;
    }

    public int Count => this.propItems.Count;

    public ICollection Values => this.propItems.Values;

    public ICollection Keys => this.propItems.Keys;

    public override void Cleanup()
    {
      this.propItems.Clear();
      base.Cleanup();
    }

    public bool ContainsKey(string key) => this.propItems.ContainsKey((object) key);

    internal void Add(string key, object value) => this.propItems.Add((object) key, value);

    internal void Remove(string key) => this.propItems.Remove((object) key);

    public event CollectionErrorEventHandler Changed;

    protected virtual void OnChanged(CollectionErrorEventArgs e)
    {
      if (this.Changed == null)
        return;
      this.Changed((object) this, e);
    }

    public virtual IEnumerator GetEnumerator() => (IEnumerator) this.propItems.GetEnumerator();

    public virtual void CopyTo(Array array, int arrayIndex) => this.propItems.CopyTo(array, arrayIndex);

    public virtual bool IsSynchronized => this.propItems.IsSynchronized;

    public virtual object SyncRoot => this.propItems.SyncRoot;
  }
}
