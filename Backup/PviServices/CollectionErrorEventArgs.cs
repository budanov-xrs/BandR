// Decompiled with JetBrains decompiler
// Type: BR.AN.PviServices.CollectionErrorEventArgs
// Assembly: BR.AN.PVIServices, Version=1.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DA853F0A-C070-4E30-B7A7-51D955FB85F6
// Assembly location: C:\Users\ss_six\Desktop\FXE\FXE software integration package\COMET_FXE-PVIServices_Example\Pluto-PVI-trial_01\packages\IToolS.BandR.3.5.58\lib\net20\BR.AN.PVIServices.dll

using System.Collections;

namespace BR.AN.PviServices
{
  public class CollectionErrorEventArgs : ErrorEventArgs
  {
    internal ArrayList propNewItems;
    internal ArrayList propChangedItems;

    internal CollectionErrorEventArgs(
      string name,
      int errorCode,
      string language,
      Action actEvent)
      : base(name, name, errorCode, language, actEvent)
    {
      this.propNewItems = new ArrayList();
      this.propChangedItems = new ArrayList();
    }

    public ArrayList NewItems => this.propNewItems;

    public ArrayList ChangedItems => this.propChangedItems;
  }
}
