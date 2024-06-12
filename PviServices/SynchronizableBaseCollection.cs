// Decompiled with JetBrains decompiler
// Type: BR.AN.PviServices.SynchronizableBaseCollection
// Assembly: BR.AN.PVIServices, Version=1.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DA853F0A-C070-4E30-B7A7-51D955FB85F6
// Assembly location: C:\Users\ss_six\Desktop\FXE\FXE software integration package\COMET_FXE-PVIServices_Example\Pluto-PVI-trial_01\packages\IToolS.BandR.3.5.58\lib\net20\BR.AN.PVIServices.dll

namespace BR.AN.PviServices
{
  public abstract class SynchronizableBaseCollection : BaseCollection
  {
    internal bool synchronize;
    internal bool hasBeenUploadedOnce;

    internal SynchronizableBaseCollection()
    {
      this.synchronize = false;
      this.hasBeenUploadedOnce = false;
    }

    internal SynchronizableBaseCollection(object parent, string name)
      : base(parent, name)
    {
      this.synchronize = false;
      this.hasBeenUploadedOnce = false;
    }

    internal SynchronizableBaseCollection(CollectionType colType, object parentObj, string name)
      : base(colType, parentObj, name)
    {
      this.synchronize = false;
      this.hasBeenUploadedOnce = false;
    }

    public bool Synchronize
    {
      get => this.synchronize;
      set => this.synchronize = value;
    }

    internal bool isSyncable => this.hasBeenUploadedOnce && this.Count != 0;
  }
}
