// Decompiled with JetBrains decompiler
// Type: BR.AN.PviServices.Modem
// Assembly: BR.AN.PVIServices, Version=1.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DA853F0A-C070-4E30-B7A7-51D955FB85F6
// Assembly location: C:\Users\ss_six\Desktop\FXE\FXE software integration package\COMET_FXE-PVIServices_Example\Pluto-PVI-trial_01\packages\IToolS.BandR.3.5.58\lib\net20\BR.AN.PVIServices.dll

using System;
using System.Xml;

namespace BR.AN.PviServices
{
  [Serializable]
  public class Modem : ModemBase
  {
    internal override int ToXMLTextWriter(ref XmlTextWriter writer, ConfigurationFlags flags) => this.ToXMLTextWriter(ref writer, flags, "", "");

    public override int FromXmlTextReader(
      ref XmlTextReader reader,
      ConfigurationFlags flags,
      DeviceBase baseObj)
    {
      return base.FromXmlTextReader(ref reader, flags, baseObj);
    }

    public override string ToString() => base.ToString();
  }
}
