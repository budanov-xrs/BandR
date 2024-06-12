// Decompiled with JetBrains decompiler
// Type: BR.AN.PviServices.AR000
// Assembly: BR.AN.PVIServices, Version=1.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DA853F0A-C070-4E30-B7A7-51D955FB85F6
// Assembly location: C:\Users\ss_six\Desktop\FXE\FXE software integration package\COMET_FXE-PVIServices_Example\Pluto-PVI-trial_01\packages\IToolS.BandR.3.5.58\lib\net20\BR.AN.PVIServices.dll

using System;
using System.ComponentModel;
using System.Xml;

namespace BR.AN.PviServices
{
  [Serializable]
  public class AR000 : DeviceBase
  {
    private int propSourceAddress;
    private int propDestinationAddress;

    public AR000()
      : base(DeviceType.AR000)
    {
      this.propSourceAddress = 1;
      this.propDestinationAddress = 2;
    }

    [PviKeyWord("/SA")]
    public int SourceAddress
    {
      get => this.propSourceAddress;
      set => this.propSourceAddress = value;
    }

    [PviKeyWord("/DA")]
    public int DestinationAddress
    {
      get => this.propDestinationAddress;
      set => this.propDestinationAddress = value;
    }

    internal override int ToXMLTextWriter(ref XmlTextWriter writer, ConfigurationFlags flags)
    {
      int xmlTextWriter = base.ToXMLTextWriter(ref writer, flags);
      writer.WriteAttributeString("Channel", "SPWIN");
      if (this.propSourceAddress != 1)
        writer.WriteAttributeString("Source", this.propSourceAddress.ToString());
      if (this.propDestinationAddress != 2)
        writer.WriteAttributeString("Destination", this.propDestinationAddress.ToString());
      return xmlTextWriter;
    }

    public override int FromXmlTextReader(
      ref XmlTextReader reader,
      ConfigurationFlags flags,
      DeviceBase baseObj)
    {
      base.FromXmlTextReader(ref reader, flags, baseObj);
      AR000 ar000 = (AR000) baseObj;
      if (ar000 == null)
        return -1;
      string str = "";
      string attribute1 = reader.GetAttribute("Source");
      if (attribute1 != null && attribute1.Length > 0)
      {
        int result = 0;
        if (PviParse.TryParseInt32(attribute1, out result))
          ar000.propSourceAddress = result;
      }
      str = "";
      string attribute2 = reader.GetAttribute("Destination");
      if (attribute2 != null && attribute2.Length > 0)
      {
        int result = 0;
        if (PviParse.TryParseInt32(attribute2, out result))
          ar000.propDestinationAddress = result;
      }
      return 0;
    }

    [EditorBrowsable(EditorBrowsableState.Never)]
    [Browsable(false)]
    public override void UpdateDeviceParameters(string parameters)
    {
      string[] strArray = parameters.Split(" ".ToCharArray());
      for (int index = 0; index < strArray.Length; ++index)
      {
        string str = (string) strArray.GetValue(index);
        if (str.ToUpper().StartsWith("/IF=AR"))
          this.propInterfaceName = str.Substring(4);
        else if (!DeviceBase.UpdateParameterFromString("/SA=", str, ref this.propSourceAddress) && !DeviceBase.UpdateParameterFromString("/DA=", str, ref this.propDestinationAddress))
          base.UpdateDeviceParameters(str);
      }
    }

    public override string DeviceParameterString => base.DeviceParameterString + "/SA=" + this.propSourceAddress.ToString() + " /DA=" + this.propDestinationAddress.ToString() + " ";

    public override string ToString() => this.DeviceParameterString + this.CpuParameterString.Trim();
  }
}
