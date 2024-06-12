// Decompiled with JetBrains decompiler
// Type: BR.AN.PviServices.CpuCollection
// Assembly: BR.AN.PVIServices, Version=1.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DA853F0A-C070-4E30-B7A7-51D955FB85F6
// Assembly location: C:\Users\ss_six\Desktop\FXE\FXE software integration package\COMET_FXE-PVIServices_Example\Pluto-PVI-trial_01\packages\IToolS.BandR.3.5.58\lib\net20\BR.AN.PVIServices.dll

using System.Collections;

namespace BR.AN.PviServices
{
  public class CpuCollection : BaseCollection
  {
    private CpuCollection propValidCpus;
    private CpuCollection propErrorCpus;

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
      if (this.Values != null && 0 < this.Values.Count)
      {
        foreach (object obj in (IEnumerable) this.Values)
        {
          arrayList.Add(obj);
          if (((Base) obj).LinkId != 0U)
            ((Cpu) obj).Disconnect(0U);
        }
      }
      for (int index = 0; index < arrayList.Count; ++index)
        ((Base) arrayList[index]).Dispose(disposing, true);
      this.Clear();
    }

    public override void Remove(string key)
    {
      if (!this.ContainsKey((object) key))
        return;
      base.Remove(key);
      if (this.propValidCpus != null)
        this.propValidCpus.Remove(key);
      if (this.propErrorCpus == null)
        return;
      this.propErrorCpus.Remove(key);
    }

    public virtual void Remove(Cpu cpuObj)
    {
      if (!this.ContainsKey((object) cpuObj.Name))
        return;
      this.Remove(cpuObj.Name);
    }

    public CpuCollection(object parent, string name)
      : base(parent, name)
    {
    }

    internal CpuCollection(CollectionType colType, object parent, string name)
      : base(colType, parent, name)
    {
    }

    public override void Connect()
    {
      this.propValidCount = 0;
      this.propErrorCount = 0;
      this.propSentCount = 0;
      if (ConnectionStates.Connecting == this.propConnectionState)
        return;
      this.propConnectionState = ConnectionStates.Connecting;
      if (this.propValidCpus == null)
      {
        this.propValidCpus = new CpuCollection(this.propParent, "Valid cpus");
        this.propValidCpus.propInternalCollection = true;
      }
      else
        this.propValidCpus.Clear();
      if (this.propErrorCpus == null)
      {
        this.propErrorCpus = new CpuCollection(this.propParent, "Error cpus");
        this.propErrorCpus.propInternalCollection = true;
      }
      else
        this.propErrorCpus.Clear();
      if (this.propCollectionType != CollectionType.HashTable || this.Values == null)
        return;
      foreach (Cpu cpu in (IEnumerable) this.Values)
      {
        ++this.propSentCount;
        if (ConnectionStates.Connected == cpu.propConnectionState)
        {
          this.propValidCpus.Add(cpu);
          ++this.propValidCount;
        }
        else
        {
          cpu.Connected += new PviEventHandler(this.cpu_Connected);
          cpu.Connect();
        }
      }
    }

    private void cpu_Connected(object sender, PviEventArgs e)
    {
      ((Base) sender).Connected -= new PviEventHandler(this.cpu_Connected);
      this.OnConnected((Cpu) sender, e);
    }

    public void Disconnect()
    {
      if (ConnectionStates.Disconnecting == this.propConnectionState)
        return;
      this.propConnectionState = ConnectionStates.Disconnecting;
      if (this.Values == null || this.Values.Count == 0)
      {
        this.OnCollectionDisconnected((CollectionEventArgs) new CpuCollectionEventArgs(this.propName, "", 0, this.Service.Language, Action.CpusDisconnect, this));
      }
      else
      {
        this.propValidCount = 0;
        this.propErrorCount = 0;
        this.propSentCount = 0;
        if (this.propValidCpus != null)
          this.propValidCpus.Clear();
        else
          this.propValidCpus = new CpuCollection(this.Parent, "Valid cpus");
        if (this.propErrorCpus != null)
          this.propErrorCpus.Clear();
        else
          this.propErrorCpus = new CpuCollection(this.Parent, "Error cpus");
        if (this.propCollectionType != CollectionType.HashTable || this.Values == null || 0 >= this.Values.Count)
          return;
        ArrayList arrayList = new ArrayList();
        foreach (Cpu cpu in (IEnumerable) this.Values)
          arrayList.Add((object) cpu);
        for (int index = 0; index < arrayList.Count; ++index)
        {
          Cpu cpu = (Cpu) arrayList[index];
          cpu.Disconnected += new PviEventHandler(this.cpu_Disconnected);
          int errorCode = cpu.DisconnectRet(202U);
          ++this.propSentCount;
          if (errorCode != 0)
            cpu.FireDisconnected(errorCode, Action.CpuDisconnect);
        }
        if (this.propSentCount != 0 && (this.propValidCount + this.propErrorCount != this.propSentCount || this.propSentCount != this.Count))
          return;
        this.OnCollectionDisconnected((CollectionEventArgs) new CpuCollectionEventArgs(this.propName, "", 0, this.Service.Language, Action.CpusDisconnect, this));
      }
    }

    private void cpu_Disconnected(object sender, PviEventArgs e)
    {
      ((Base) sender).Disconnected -= new PviEventHandler(this.cpu_Disconnected);
      this.OnDisconnected((Cpu) sender, e);
    }

    public void Restart(BootMode bootMode)
    {
      this.propValidCount = 0;
      this.propErrorCount = 0;
      this.propSentCount = 0;
      if (this.propValidCpus != null)
        this.propValidCpus.Clear();
      else
        this.propValidCpus = new CpuCollection(this.Parent, "Valid cpus");
      if (this.propErrorCpus != null)
        this.propErrorCpus.Clear();
      else
        this.propErrorCpus = new CpuCollection(this.Parent, "Error cpus");
      if (this.propCollectionType != CollectionType.HashTable || this.Values == null)
        return;
      foreach (Cpu cpu in (IEnumerable) this.Values)
      {
        if (cpu.ErrorCode == 0 || cpu.ErrorCode == 12002)
        {
          if (!cpu.IsConnected)
            ++this.propValidCount;
          else
            cpu.Restart(bootMode);
          ++this.propSentCount;
        }
      }
    }

    internal virtual Cpu GetItem(int index)
    {
      switch (this.propCollectionType)
      {
        case CollectionType.SortedList:
          return (Cpu) this.propSortedList.GetByIndex(index);
        case CollectionType.ArrayList:
          return (Cpu) this.propArrayList[index];
        default:
          return (Cpu) null;
      }
    }

    internal virtual Cpu GetItem(string address) => this.propCollectionType == CollectionType.HashTable ? this[address] : (Cpu) null;

    public virtual void Add(Cpu cpu)
    {
      if (cpu == null || cpu.Name == null)
        return;
      if (!this.ContainsKey((object) cpu.Name))
        this.Add((object) cpu.Name, (object) cpu);
      if (this.propInternalCollection)
        return;
      if (cpu.propUserCollections == null)
        cpu.propUserCollections = new Hashtable();
      if (cpu.propUserCollections.ContainsKey(this.Name))
        return;
      cpu.propUserCollections.Add(this.Name, (object) this);
    }

    protected internal virtual void OnConnected(Cpu cpu, PviEventArgs e)
    {
      ++this.propValidCount;
      if (this.propValidCpus == null)
      {
        this.propValidCpus = new CpuCollection(this.Parent, "Valid cpus");
        this.propValidCpus.propInternalCollection = true;
      }
      if (!this.propValidCpus.ContainsKey((object) cpu.Name))
        this.propValidCpus.Add(cpu);
      if (this.propValidCount + this.propErrorCount != this.propSentCount)
        return;
      this.OnCollectionConnected((CollectionEventArgs) new CpuCollectionEventArgs(this.propName, "", 0, this.Service.Language, Action.CpusConnect, this.propValidCpus));
      if (this.propErrorCount <= 0)
        return;
      this.OnCollectionError((CollectionEventArgs) new CpuCollectionEventArgs(this.propName, "", 0, this.Service.Language, Action.CpusConnect, this.propErrorCpus));
    }

    protected internal virtual void OnDisconnected(Cpu cpu, PviEventArgs e)
    {
      ++this.propValidCount;
      if (this.propValidCpus == null)
      {
        this.propValidCpus = new CpuCollection(this.Parent, "Valid cpus");
        this.propValidCpus.propInternalCollection = true;
      }
      if (!this.propValidCpus.ContainsKey((object) cpu.Name))
        this.propValidCpus.Add(cpu);
      if (this.propValidCount + this.propErrorCount != this.propSentCount || this.propSentCount != this.Count)
        return;
      this.OnCollectionDisconnected((CollectionEventArgs) new CpuCollectionEventArgs(this.propName, "", 0, this.Service.Language, Action.CpusDisconnect, this.propValidCpus));
      if (this.propErrorCount <= 0)
        return;
      this.OnCollectionError((CollectionEventArgs) new CpuCollectionEventArgs(this.propName, "", 0, this.Service.Language, Action.CpusConnect, this.propErrorCpus));
    }

    protected internal virtual void OnRestarted(Cpu cpu, PviEventArgs e)
    {
      ++this.propValidCount;
      if (this.propValidCpus == null)
      {
        this.propValidCpus = new CpuCollection(this.Parent, "Valid cpus");
        this.propValidCpus.propInternalCollection = true;
      }
      if (!this.propValidCpus.ContainsKey((object) cpu.Name))
        this.propValidCpus.Add(cpu);
      if (this.Restarted != null)
        this.Restarted((object) cpu, e);
      if (this.propValidCount + this.propErrorCount != this.propSentCount)
        return;
      this.OnCollectionRestarted((CollectionEventArgs) new CpuCollectionEventArgs(this.propName, "", 0, this.Service.Language, Action.CpusRestart, this.propValidCpus));
      if (this.propErrorCount <= 0)
        return;
      this.OnCollectionError((CollectionEventArgs) new CpuCollectionEventArgs(this.propName, "", 0, this.Service.Language, Action.CpusRestart, this.propErrorCpus));
    }

    protected internal virtual void OnError(Cpu cpu, PviEventArgs e)
    {
      if (cpu == null || cpu.Name == null)
        return;
      if (this.propErrorCpus == null)
      {
        this.propErrorCpus = new CpuCollection(this.Parent, "Error cpus");
        this.propErrorCpus.propInternalCollection = true;
      }
      if (!this.propErrorCpus.ContainsKey((object) cpu.Name))
      {
        this.propErrorCpus.Add(cpu);
        ++this.propErrorCount;
      }
      this.Fire_Error((object) cpu, e);
      if (this.propValidCount + this.propErrorCount != this.Count)
        return;
      this.OnCollectionError((CollectionEventArgs) new CpuCollectionEventArgs(this.propName, "", 0, this.Service.Language, Action.CpusConnect, this.propErrorCpus));
      if (this.propValidCount > 0 && e.Action == Action.CpuConnect)
      {
        this.OnCollectionConnected((CollectionEventArgs) new CpuCollectionEventArgs(this.propName, "", 0, this.Service.Language, Action.CpusConnect, this.propValidCpus));
      }
      else
      {
        if (this.propValidCount <= 0 || e.Action != Action.CpusDisconnect)
          return;
        this.OnCollectionDisconnected((CollectionEventArgs) new CpuCollectionEventArgs(this.propName, "", 0, this.Service.Language, Action.CpusDisconnect, this.propValidCpus));
      }
    }

    protected internal virtual void OnCollectionRestarted(CollectionEventArgs e)
    {
      this.propValidCount = 0;
      this.propSentCount = 0;
      if (e.ErrorCode == 0 || 12002 == e.ErrorCode)
      {
        if (ConnectionStates.Connected == this.propConnectionState)
          return;
        this.propConnectionState = ConnectionStates.Connected;
        if (this.CollectionRestarted == null)
          return;
        this.CollectionRestarted((object) this, e);
      }
      else
      {
        if (ConnectionStates.ConnectedError <= this.propConnectionState || ConnectionStates.Unininitialized >= this.propConnectionState)
          return;
        this.propConnectionState = ConnectionStates.ConnectedError;
        if (this.CollectionRestarted == null)
          return;
        this.CollectionRestarted((object) this, e);
      }
    }

    public event PviEventHandler Restarted;

    public event CollectionEventHandler CollectionRestarted;

    public Cpu this[string address] => (Cpu) this[(object) address];

    public override Service Service => this.propParent is Service ? (Service) this.propParent : (Service) null;
  }
}
