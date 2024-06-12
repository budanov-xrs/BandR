// Decompiled with JetBrains decompiler
// Type: BR.AN.PviServices.LogicalCollection
// Assembly: BR.AN.PVIServices, Version=1.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DA853F0A-C070-4E30-B7A7-51D955FB85F6
// Assembly location: C:\Users\ss_six\Desktop\FXE\FXE software integration package\COMET_FXE-PVIServices_Example\Pluto-PVI-trial_01\packages\IToolS.BandR.3.5.58\lib\net20\BR.AN.PVIServices.dll

using System.Collections;

namespace BR.AN.PviServices
{
  public class LogicalCollection : BaseCollection
  {
    public LogicalCollection(object parent, string name)
      : base(parent, name)
    {
    }

    public override void Connect()
    {
      this.propCounter = 0;
      this.propErrorCount = 0;
      if (this.propCollectionType != CollectionType.HashTable || 0 >= this.Count)
        return;
      foreach (object obj in (IEnumerable) this.Values)
      {
        switch (obj)
        {
          case Variable _:
            ((Base) obj).Connected += new PviEventHandler(this.LogicalObjectConnected);
            ((Base) obj).Connect();
            continue;
          case Task _:
            ((Base) obj).Connected += new PviEventHandler(this.LogicalObjectConnected);
            ((Base) obj).Connect();
            continue;
          case Cpu _:
            ((Base) obj).Connected += new PviEventHandler(this.LogicalObjectConnected);
            ((Base) obj).Connect();
            continue;
          default:
            continue;
        }
      }
    }

    public void Disconnect()
    {
      if (this.propCollectionType != CollectionType.HashTable || 0 >= this.Count)
        return;
      foreach (object obj in (IEnumerable) this.Values)
      {
        switch (obj)
        {
          case Variable _:
            ((Base) obj).Disconnected += new PviEventHandler(this.LogicalObjectDisconnected);
            ((Base) obj).Disconnect();
            continue;
          case Task _:
            ((Base) obj).Disconnected += new PviEventHandler(this.LogicalObjectDisconnected);
            ((Base) obj).Disconnect();
            continue;
          case Cpu _:
            ((Base) obj).Disconnected += new PviEventHandler(this.LogicalObjectDisconnected);
            ((Base) obj).Disconnect();
            continue;
          default:
            continue;
        }
      }
    }

    private void LogicalObjectConnected(object sender, PviEventArgs e)
    {
      ++this.propCounter;
      switch (sender)
      {
        case Variable _:
          ((Base) sender).Connected -= new PviEventHandler(this.LogicalObjectConnected);
          break;
        case Task _:
          ((Base) sender).Connected -= new PviEventHandler(this.LogicalObjectConnected);
          break;
      }
      int propCounter = this.propCounter;
      int count = this.Count;
    }

    private void LogicalObjectDisconnected(object sender, PviEventArgs e)
    {
      ++this.propCounter;
      switch (sender)
      {
        case Variable _:
          ((Base) sender).Disconnected -= new PviEventHandler(this.LogicalObjectDisconnected);
          break;
        case Task _:
          ((Base) sender).Disconnected -= new PviEventHandler(this.LogicalObjectDisconnected);
          break;
      }
      int propCounter = this.propCounter;
      int count = this.Count;
    }

    public object this[string name] => this.propCollectionType == CollectionType.HashTable ? this[(object) name] : (object) null;

    public override Service Service => this.propParent is Service ? (Service) this.propParent : (Service) null;
  }
}
