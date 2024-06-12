// Decompiled with JetBrains decompiler
// Type: BR.AN.PviServices.HashtableArray
// Assembly: BR.AN.PVIServices, Version=1.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DA853F0A-C070-4E30-B7A7-51D955FB85F6
// Assembly location: C:\Users\ss_six\Desktop\FXE\FXE software integration package\COMET_FXE-PVIServices_Example\Pluto-PVI-trial_01\packages\IToolS.BandR.3.5.58\lib\net20\BR.AN.PVIServices.dll

using System.Collections;

namespace BR.AN.PviServices
{
  public class HashtableArray
  {
    internal ArrayList propArrayList;
    internal Hashtable propHashTable;

    public HashtableArray()
    {
      this.propArrayList = new ArrayList(1);
      this.propHashTable = new Hashtable();
    }

    public void Remove(object key)
    {
      if (!this.ContainsKey(key))
        return;
      this.propArrayList.RemoveAt((int) this.propHashTable[key]);
      this.propHashTable.Remove(key);
    }

    public int Add(object key, object value)
    {
      int num = -1;
      if (!this.propHashTable.ContainsKey(key))
      {
        num = this.propArrayList.Add(value);
        this.propHashTable.Add(key, (object) num);
      }
      return num;
    }

    public void Clear()
    {
      this.propArrayList.Clear();
      this.propHashTable.Clear();
    }

    public virtual int Count => this.propArrayList.Count;

    public virtual IEnumerator GetEnumerator() => this.propArrayList.GetEnumerator();

    public bool ContainsKey(object key) => this.propHashTable.ContainsKey(key);

    public object this[object key] => this.propArrayList[(int) this.propHashTable[key]];

    public object this[int index] => this.propArrayList[index];

    public virtual object Clone() => (object) new HashtableArray()
    {
      propHashTable = (Hashtable) this.propHashTable.Clone(),
      propArrayList = (ArrayList) this.propArrayList.Clone()
    };

    public ICollection Keys => this.propHashTable.Keys;
  }
}
