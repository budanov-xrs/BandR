// Decompiled with JetBrains decompiler
// Type: BR.AN.PviServices.StructMemberCollection
// Assembly: BR.AN.PVIServices, Version=1.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DA853F0A-C070-4E30-B7A7-51D955FB85F6
// Assembly location: C:\Users\ss_six\Desktop\FXE\FXE software integration package\COMET_FXE-PVIServices_Example\Pluto-PVI-trial_01\packages\IToolS.BandR.3.5.58\lib\net20\BR.AN.PVIServices.dll

using System.Collections;

namespace BR.AN.PviServices
{
  public class StructMemberCollection : HashtableArray
  {
    protected int propCount;

    public StructMemberCollection() => this.propCount = -1;

    public StructMemberCollection(int count) => this.propCount = count;

    internal void CleanUp(bool disposing)
    {
      for (int index = 0; index < this.Count; ++index)
      {
        object obj = this[index];
        if (obj is Variable)
        {
          if (((Base) obj).LinkId != 0U)
            ((Variable) obj).Disconnect(0U);
          if (((Variable) obj).StructureMembers != null)
            ((Variable) obj).StructureMembers.CleanUp(disposing);
          ((Base) obj).Dispose(disposing, true);
        }
      }
      this.Clear();
    }

    public Variable this[string name] => -1 != this.propCount ? (Variable) null : (Variable) this[(object) name];

    public new object this[int index]
    {
      get
      {
        if (-1 == this.propCount)
          return (object) (Variable) base[index];
        return this.propCount > index ? (object) ("[" + index.ToString() + "]") : (object) -1;
      }
    }

    public bool IsVirtual => -1 != this.propCount;

    public override object Clone()
    {
      StructMemberCollection memberCollection = new StructMemberCollection();
      memberCollection.propHashTable = (Hashtable) this.propHashTable.Clone();
      memberCollection.propArrayList = (ArrayList) this.propArrayList.Clone();
      memberCollection.propCount = this.propCount;
      return (object) memberCollection;
    }

    public override int Count => -1 != this.propCount ? this.propCount : base.Count;
  }
}
