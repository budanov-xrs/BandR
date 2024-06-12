// Decompiled with JetBrains decompiler
// Type: BR.AN.PviServices.Shared
// Assembly: BR.AN.PVIServices, Version=1.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DA853F0A-C070-4E30-B7A7-51D955FB85F6
// Assembly location: C:\Users\ss_six\Desktop\FXE\FXE software integration package\COMET_FXE-PVIServices_Example\Pluto-PVI-trial_01\packages\IToolS.BandR.3.5.58\lib\net20\BR.AN.PVIServices.dll

using System;
using System.ComponentModel;
using System.Xml;

namespace BR.AN.PviServices
{
  [Serializable]
  public class Shared : DeviceBase
  {
    private byte propChannel;

    internal override int ToXMLTextWriter(ref XmlTextWriter writer, ConfigurationFlags flags)
    {
      int xmlTextWriter = this.SaveConnectionAttributes(ref writer);
      if (this.propChannel != (byte) 1)
        writer.WriteAttributeString("Channel", this.propChannel.ToString());
      return xmlTextWriter;
    }

    public override int FromXmlTextReader(
      ref XmlTextReader reader,
      ConfigurationFlags flags,
      DeviceBase baseObj)
    {
      base.FromXmlTextReader(ref reader, flags, baseObj);
      Shared shared = (Shared) baseObj;
      if (shared == null)
        return -1;
      string attribute = reader.GetAttribute("Channel");
      if (attribute != null && attribute.Length > 0)
      {
        byte result = 0;
        if (PviParse.TryParseByte(attribute, out result))
          shared.propChannel = result;
      }
      return 0;
    }

    [EditorBrowsable(EditorBrowsableState.Never)]
    [Browsable(false)]
    public override void UpdateDeviceParameters(string parameters)
    {
      string[] strArray = parameters.Split(" ".ToCharArray());
      this.propKnownDeviceParameters = "";
      for (int index = 0; index < strArray.Length; ++index)
      {
        bool flag = false;
        string parmItem = (string) strArray.GetValue(index);
        if (parmItem.ToUpper().StartsWith("/IF=LS251_"))
        {
          this.propInterfaceName = parmItem.Substring(4);
          this.propChannel = System.Convert.ToByte(parmItem.Substring(10));
          flag = true;
        }
        else
          base.UpdateDeviceParameters(parmItem);
        if (flag)
        {
          string str = parmItem;
          if (parmItem.IndexOf("/") != 0)
            str = "/" + parmItem;
          if (this.propKnownDeviceParameters.Length == 0)
          {
            this.propKnownDeviceParameters = str.Trim();
          }
          else
          {
            Shared shared = this;
            shared.propKnownDeviceParameters = shared.propKnownDeviceParameters + " " + str.Trim();
          }
        }
      }
    }

    public Shared()
      : base(DeviceType.Shared)
    {
      this.propChannel = (byte) 1;
    }

    [PviKeyWord("/IF")]
    public byte Channel
    {
      get => this.propChannel;
      set
      {
        this.propChannel = value;
        this.propInterfaceName = "LS251_" + value.ToString();
      }
    }

    public override string ToString() => this.DeviceParameterString + this.CpuParameterString.Trim();
  }
}
