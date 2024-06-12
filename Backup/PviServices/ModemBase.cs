// Decompiled with JetBrains decompiler
// Type: BR.AN.PviServices.ModemBase
// Assembly: BR.AN.PVIServices, Version=1.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DA853F0A-C070-4E30-B7A7-51D955FB85F6
// Assembly location: C:\Users\ss_six\Desktop\FXE\FXE software integration package\COMET_FXE-PVIServices_Example\Pluto-PVI-trial_01\packages\IToolS.BandR.3.5.58\lib\net20\BR.AN.PVIServices.dll

using System;
using System.ComponentModel;
using System.Xml;

namespace BR.AN.PviServices
{
  [Serializable]
  public abstract class ModemBase : DeviceBase
  {
    private string propModem;
    private int propCommPort;
    private string propPhoneNumber;
    private int propRedial;
    private int propRedialTimeout;

    public ModemBase()
      : base(DeviceType.Modem)
    {
      this.propModem = "MicroLink 56k";
      this.propCommPort = 1;
      this.propPhoneNumber = "";
      this.propRedialTimeout = 60;
      this.propIntervalTimeout = 40;
    }

    internal virtual int ToXMLTextWriter(
      ref XmlTextWriter writer,
      ConfigurationFlags flags,
      string attributeName,
      string attributeValue)
    {
      int xmlTextWriter = this.ToXMLTextWriter(ref writer, flags);
      if (this.propCommPort != 1)
        writer.WriteAttributeString("Channel", this.propCommPort.ToString());
      if (this.propModem != "MicroLink 56k")
        writer.WriteAttributeString("Modem", this.propModem);
      if (this.propPhoneNumber != null && this.propPhoneNumber.Length > 0)
        writer.WriteAttributeString("PhoneNumber", this.propPhoneNumber);
      if (this.propRedialTimeout != 60)
        writer.WriteAttributeString("RedialTimeout", this.propRedialTimeout.ToString());
      if (this.propRedial != 0)
        writer.WriteAttributeString("Redial", this.propRedial.ToString());
      if (attributeName != null && attributeName.Length > 0 && attributeValue != null && attributeValue.Length > 0)
        writer.WriteAttributeString(attributeName, attributeValue);
      return xmlTextWriter;
    }

    public override int FromXmlTextReader(
      ref XmlTextReader reader,
      ConfigurationFlags flags,
      DeviceBase baseObj)
    {
      base.FromXmlTextReader(ref reader, flags, baseObj);
      ModemBase modemBase = (ModemBase) baseObj;
      if (modemBase == null)
        return -1;
      int result = 0;
      string attribute1 = reader.GetAttribute("Channel");
      if (attribute1 != null && attribute1.Length > 0 && PviParse.TryParseInt32(attribute1, out result))
        modemBase.propCommPort = result;
      string attribute2 = reader.GetAttribute("Modem");
      if (attribute2 != null && attribute2.Length > 0)
        modemBase.propModem = attribute2;
      string attribute3 = reader.GetAttribute("PhoneNumber");
      if (attribute3 != null && attribute3.Length > 0)
        modemBase.propPhoneNumber = attribute3;
      string attribute4 = reader.GetAttribute("RedialTimeout");
      if (attribute4 != null && attribute4.Length > 0 && PviParse.TryParseInt32(attribute4, out result))
        modemBase.propRedialTimeout = result;
      string attribute5 = reader.GetAttribute("Redial");
      if (attribute5 != null && attribute5.Length > 0 && PviParse.TryParseInt32(attribute5, out result))
        modemBase.propRedial = result;
      return 0;
    }

    [Browsable(false)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public override void UpdateDeviceParameters(string parameters)
    {
      this.propKnownDeviceParameters = "";
      string[] strArray = parameters.Replace(" /", "\t").Split("\t".ToCharArray());
      for (int index = 0; index < strArray.Length; ++index)
      {
        bool flag = false;
        string str1 = (string) strArray.GetValue(index);
        if (DeviceBase.UpdateParameterFromString("/IF=", str1, ref this.propInterfaceName))
          flag = true;
        else if (DeviceBase.UpdateParameterFromString("IT=", str1, ref this.propIntervalTimeout))
          flag = true;
        else if (DeviceBase.UpdateParameterFromString("MO=", str1, ref this.propModem))
          flag = true;
        else if (DeviceBase.UpdateParameterFromString("TN=", str1, ref this.propPhoneNumber))
          flag = true;
        else if (str1.ToUpper().StartsWith("MR="))
        {
          this.propRedial = str1.Substring(3).IndexOf("INFINITE") != 0 ? System.Convert.ToInt32(str1.Substring(3)) : -1;
          flag = true;
        }
        else if (DeviceBase.UpdateParameterFromString("RI=", str1, ref this.propRedialTimeout))
          flag = true;
        else
          base.UpdateDeviceParameters(str1);
        if (flag)
        {
          string str2 = str1;
          if (str1.IndexOf("/") != 0)
            str2 = "/" + str1;
          if (this.propKnownDeviceParameters.Length == 0)
          {
            this.propKnownDeviceParameters = str2.Trim();
          }
          else
          {
            ModemBase modemBase = this;
            modemBase.propKnownDeviceParameters = modemBase.propKnownDeviceParameters + " " + str2.Trim();
          }
        }
      }
    }

    [PviKeyWord("/MO")]
    public string Modem
    {
      get => this.propModem;
      set => this.propModem = value;
    }

    [PviKeyWord("/IF")]
    public int CommunicationPort
    {
      get => this.propCommPort;
      set
      {
        this.propCommPort = value;
        this.propInterfaceName = "modem" + value.ToString();
      }
    }

    [PviKeyWord("/TN")]
    public string PhoneNumber
    {
      get => this.propPhoneNumber;
      set => this.propPhoneNumber = value;
    }

    [PviKeyWord("/MR")]
    public int Redial
    {
      get => this.propRedial;
      set => this.propRedial = value;
    }

    [PviKeyWord("/RI")]
    public int RedialTimeout
    {
      get => this.propRedialTimeout;
      set => this.propRedialTimeout = value;
    }

    public override string DeviceParameterString => base.DeviceParameterString + "/MO=" + this.Modem + " /TN=" + this.PhoneNumber + " /MR=" + this.Redial.ToString() + " /RI=" + this.RedialTimeout.ToString() + " /IT=" + this.IntervalTimeout.ToString() + " ";

    public override string ToString() => this.DeviceParameterString + this.CpuParameterString.Trim();
  }
}
