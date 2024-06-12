// Decompiled with JetBrains decompiler
// Type: BR.AN.PviServices.MemberCollection
// Assembly: BR.AN.PVIServices, Version=1.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DA853F0A-C070-4E30-B7A7-51D955FB85F6
// Assembly location: C:\Users\ss_six\Desktop\FXE\FXE software integration package\COMET_FXE-PVIServices_Example\Pluto-PVI-trial_01\packages\IToolS.BandR.3.5.58\lib\net20\BR.AN.PVIServices.dll

using System;
using System.Collections;

namespace BR.AN.PviServices
{
  public class MemberCollection : BaseCollection
  {
    private Hashtable mapNameToIndex;
    private int datavalidCount;

    internal MemberCollection(Variable parent, string address)
      : base(CollectionType.ArrayList, (object) parent, address)
    {
      this.mapNameToIndex = new Hashtable();
    }

    internal MemberCollection()
      : base(CollectionType.ArrayList, (object) null, (string) null)
    {
      this.mapNameToIndex = new Hashtable();
    }

    public override void Connect()
    {
      this.propCounter = 0;
      this.datavalidCount = 0;
      this.propErrorCount = 0;
      if (!((Base) this.propParent).IsConnected || 0 >= this.Count)
        return;
      foreach (Variable variable in (IEnumerable) this.Values)
      {
        variable.Connect();
        variable.Connected += new PviEventHandler(this.MemberConnected);
        variable.Error += new PviEventHandler(this.MemberError);
        variable.DataValidated += new PviEventHandler(this.MemberDataValid);
      }
    }

    internal override void Dispose(bool disposing, bool removeFromCollection)
    {
      if (this.propDisposed)
        return;
      object propParent = this.propParent;
      object propUserData = this.propUserData;
      string propName = this.propName;
      this.CleanUp(disposing);
      this.propParent = propParent;
      this.propUserData = propUserData;
      this.propName = propName;
      base.Dispose(disposing, removeFromCollection);
      this.propParent = (object) null;
      this.propUserData = (object) null;
      this.propName = (string) null;
    }

    internal void CleanUp(bool disposing)
    {
      ArrayList arrayList = new ArrayList();
      this.propCounter = 0;
      try
      {
        if (0 < this.Count)
        {
          foreach (Variable variable in (IEnumerable) this.Values)
          {
            if (!variable.ContainedInParentCollection())
            {
              arrayList.Add((object) variable);
              if (variable.LinkId != 0U)
                variable.Disconnect(0U);
            }
          }
        }
        for (int index = 0; index < arrayList.Count; ++index)
          ((Base) arrayList[index]).Dispose(disposing, true);
        this.Clear();
      }
      catch (OutOfMemoryException ex)
      {
        string message = ex.Message;
      }
    }

    public void Disconnect()
    {
      this.propCounter = 0;
      if (0 >= this.Count)
        return;
      foreach (Variable variable in (IEnumerable) this.Values)
      {
        variable.Disconnected += new PviEventHandler(this.MemberDisconnected);
        variable.Disconnect();
      }
    }

    private void MemberConnected(object sender, PviEventArgs e)
    {
      if (this.mapNameToIndex == null)
        this.mapNameToIndex = new Hashtable();
      ++this.propCounter;
      ((Base) sender).Connected -= new PviEventHandler(this.MemberConnected);
      int propCounter = this.propCounter;
      int count = this.propArrayList.Count;
    }

    private void MemberDisconnected(object sender, PviEventArgs e)
    {
      ++this.propCounter;
      ((Base) sender).Disconnected -= new PviEventHandler(this.MemberDisconnected);
      int propCounter = this.propCounter;
      int count = this.Count;
    }

    private void MemberError(object sender, PviEventArgs e)
    {
      ++this.propErrorCount;
      ((Base) sender).Error -= new PviEventHandler(this.MemberError);
    }

    private void MemberDataValid(object sender, PviEventArgs e)
    {
      ++this.datavalidCount;
      ((Variable) sender).DataValidated -= new PviEventHandler(this.MemberDataValid);
    }

    internal virtual void MoveToFirst() => this.propEnumer = this.GetEnumerator();

    internal virtual Variable GetNext() => this.propEnumer.MoveNext() ? (Variable) this.propEnumer.Current : (Variable) null;

    public override void Clear()
    {
      base.Clear();
      this.mapNameToIndex.Clear();
    }

    public override void Remove(string key)
    {
      if (!this.mapNameToIndex.ContainsKey((object) key))
        return;
      this.Remove(this.mapNameToIndex[(object) key]);
      this.mapNameToIndex.Remove((object) key);
    }

    internal virtual int Add(Variable member)
    {
      if (CollectionType.ArrayList == this.propCollectionType && !this.mapNameToIndex.ContainsKey((object) member.Name))
      {
        this.mapNameToIndex.Add((object) member.Name, (object) this.propArrayList.Count);
        this.propArrayList.Add((object) member);
      }
      return 0;
    }

    public override bool ContainsKey(object key) => this.mapNameToIndex.ContainsKey(key);

    internal virtual int CopyTo(MemberCollection collection)
    {
      if (collection != null)
        collection.propArrayList = new ArrayList((ICollection) this.propArrayList);
      return 0;
    }

    internal int CountExtended
    {
      get
      {
        int num1 = 0;
        int num2 = 0;
        if (0 < this.Count)
        {
          foreach (Variable variable in (IEnumerable) this.Values)
          {
            if (variable.Members != null && variable.Members.Count > 0)
              num1 += variable.Members.CountExtended * variable.propPviValue.ArrayLength;
            if (variable.propPviValue.ArrayLength > 1)
              num1 += variable.propPviValue.ArrayLength;
          }
          foreach (Variable variable in (IEnumerable) this.Values)
          {
            if (variable.propPviValue.DataType != DataType.Structure && variable.propPviValue.ArrayLength < 2)
              ++num2;
          }
        }
        return (num1 + num2) * ((Variable) this.propParent).propPviValue.ArrayLength;
      }
    }

    public Variable this[int index] => (Variable) this.propArrayList[index];

    internal Variable FirstSimpleTyped
    {
      get
      {
        Variable firstSimpleTyped = (Variable) null;
        if (0 < this.propArrayList.Count)
        {
          firstSimpleTyped = (Variable) this.propArrayList[0];
          if (1 < firstSimpleTyped.propPviValue.ArrayLength && DataType.Structure == firstSimpleTyped.propPviValue.DataType)
            firstSimpleTyped = ((Variable) this.propArrayList[0]).Members.FirstSimpleTyped;
        }
        return firstSimpleTyped;
      }
    }

    public Variable First => 0 < this.propArrayList.Count ? (Variable) this.propArrayList[0] : (Variable) null;

    public Variable this[string name] => this.mapNameToIndex.ContainsKey((object) name) ? (Variable) this.propArrayList[(int) this.mapNameToIndex[(object) name]] : (Variable) null;

    public override Service Service => this.propParent is Variable ? ((Base) this.propParent).Service : (Service) null;

    public bool DataValid => this.Count == this.datavalidCount;
  }
}
