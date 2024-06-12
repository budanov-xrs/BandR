// Decompiled with JetBrains decompiler
// Type: BR.AN.PviServices.TcpIpMODBUS
// Assembly: BR.AN.PVIServices, Version=1.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DA853F0A-C070-4E30-B7A7-51D955FB85F6
// Assembly location: C:\Users\ss_six\Desktop\FXE\FXE software integration package\COMET_FXE-PVIServices_Example\Pluto-PVI-trial_01\packages\IToolS.BandR.3.5.58\lib\net20\BR.AN.PVIServices.dll

using System;
using System.ComponentModel;
using System.Xml;

namespace BR.AN.PviServices
{
  [Serializable]
  public class TcpIpMODBUS : DeviceBase
  {
    private string propFBDConfiguration;
    private string propDestinationIPAddress;
    private int propPortNumber;
    private int propUnitID;
    private int propConnectionRetries;

    internal override int ToXMLTextWriter(ref XmlTextWriter writer, ConfigurationFlags flags)
    {
      base.ToXMLTextWriter(ref writer, flags);
      writer.WriteAttributeString("LINE", "LNMODBUS");
      if (this.FBDConfiguration != null && this.FBDConfiguration.Length > 0)
        writer.WriteAttributeString("FBDConfiguration", this.FBDConfiguration);
      if (this.DestinationIPAddress != null && this.DestinationIPAddress.Length > 0)
        writer.WriteAttributeString("DestinationIpAddress", this.DestinationIPAddress);
      if (this.PortNumber > 0)
        writer.WriteAttributeString("PortNumber", this.PortNumber.ToString());
      if (this.UnitID > 0)
        writer.WriteAttributeString("UnitID", this.UnitID.ToString());
      if (this.ConnectionRetries > 0)
        writer.WriteAttributeString("ConnectionRetries", this.ConnectionRetries.ToString());
      return 0;
    }

    public override int FromXmlTextReader(
      ref XmlTextReader reader,
      ConfigurationFlags flags,
      DeviceBase baseObj)
    {
      base.FromXmlTextReader(ref reader, flags, baseObj);
      TcpIpMODBUS tcpIpModbus = (TcpIpMODBUS) baseObj;
      if (tcpIpModbus == null)
        return -1;
      string str = "";
      string attribute1 = reader.GetAttribute("DestinationIpAddress");
      if (attribute1 != null && attribute1.Length > 0)
        tcpIpModbus.propDestinationIPAddress = attribute1;
      str = "";
      string attribute2 = reader.GetAttribute("FBDConfiguration");
      if (attribute2 != null && attribute2.Length > 0)
        tcpIpModbus.propFBDConfiguration = attribute2;
      str = "";
      string attribute3 = reader.GetAttribute("PortNumber");
      if (attribute3 != null && attribute3.Length > 0)
      {
        int result = 0;
        if (PviParse.TryParseInt32(attribute3, out result))
          tcpIpModbus.propPortNumber = result;
      }
      str = "";
      string attribute4 = reader.GetAttribute("UnitID");
      if (attribute4 != null && attribute4.Length > 0)
      {
        int result = 0;
        if (PviParse.TryParseInt32(attribute4, out result))
          tcpIpModbus.propUnitID = result;
      }
      str = "";
      string attribute5 = reader.GetAttribute("ConnectionRetries");
      if (attribute5 != null && attribute5.Length > 0)
      {
        int result = 0;
        if (PviParse.TryParseInt32(attribute5, out result))
          tcpIpModbus.propConnectionRetries = result;
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
        string str = (string) strArray.GetValue(index);
        if (DeviceBase.UpdateParameterFromString("/IF=", str, ref this.propInterfaceName))
          flag = true;
        else if (DeviceBase.UpdateParameterFromString("/DAIP=", str, ref this.propDestinationIPAddress))
          flag = true;
        else if (DeviceBase.UpdateParameterFromString("/CFG=", str, ref this.propFBDConfiguration))
          flag = true;
        else if (DeviceBase.UpdateParameterFromString("/PN=", str, ref this.propPortNumber))
          flag = true;
        else if (DeviceBase.UpdateParameterFromString("/DA=", str, ref this.propUnitID))
          flag = true;
        else if (DeviceBase.UpdateParameterFromString("/CR=", str, ref this.propConnectionRetries))
          flag = true;
        else
          base.UpdateDeviceParameters(str);
        if (flag)
        {
          if (this.propKnownDeviceParameters.Length == 0)
          {
            this.propKnownDeviceParameters = str.Trim();
          }
          else
          {
            TcpIpMODBUS tcpIpModbus = this;
            tcpIpModbus.propKnownDeviceParameters = tcpIpModbus.propKnownDeviceParameters + " " + str.Trim();
          }
        }
      }
    }

    [Browsable(false)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public override void UpdateCpuParameters(string parameters)
    {
      string[] strArray = parameters.Split(" ".ToCharArray());
      this.propKnownCpuParameters = " ";
      for (int index = 0; index < strArray.Length; ++index)
      {
        bool flag = false;
        string str1 = (string) strArray.GetValue(index);
        if (DeviceBase.UpdateParameterFromString("/DAIP=", str1, ref this.propDestinationIPAddress))
          flag = true;
        else if (DeviceBase.UpdateParameterFromString("/CFG=", str1, ref this.propFBDConfiguration))
          flag = true;
        else
          base.UpdateCpuParameters(str1);
        if (flag)
        {
          string str2 = str1;
          if (str1.IndexOf("/") != 0)
            str2 = "/" + str1;
          if (this.propKnownCpuParameters.Length == 0)
          {
            this.propKnownCpuParameters = str2.Trim();
          }
          else
          {
            TcpIpMODBUS tcpIpModbus = this;
            tcpIpModbus.propKnownCpuParameters = tcpIpModbus.propKnownCpuParameters + " " + str2.Trim();
          }
        }
      }
    }

    public TcpIpMODBUS()
      : base(DeviceType.TcpIpMODBUS)
    {
      this.propFBDConfiguration = "";
      this.propDestinationIPAddress = "";
      this.propPortNumber = 502;
      this.propUnitID = (int) byte.MaxValue;
      this.propConnectionRetries = 0;
    }

    [PviCpuParameter]
    [PviKeyWord("/CFG")]
    public string FBDConfiguration
    {
      get => this.propFBDConfiguration;
      set => this.propFBDConfiguration = value;
    }

    [PviCpuParameter]
    [PviKeyWord("/DAIP")]
    public string DestinationIPAddress
    {
      get => this.propDestinationIPAddress;
      set => this.propDestinationIPAddress = value;
    }

    [PviKeyWord("/PN")]
    [PviCpuParameter]
    public int PortNumber
    {
      get => this.propPortNumber;
      set => this.propPortNumber = value;
    }

    [PviCpuParameter]
    [PviKeyWord("/DA")]
    public int UnitID
    {
      get => this.propUnitID;
      set => this.propUnitID = value;
    }

    [PviCpuParameter]
    [PviKeyWord("/CR")]
    public int ConnectionRetries
    {
      get => this.propConnectionRetries;
      set => this.propConnectionRetries = value;
    }

    public override string DeviceParameterString => base.DeviceParameterString;

    public override string CpuParameterString
    {
      get
      {
        string cpuParameterString;
        if (this.FBDConfiguration != null && 0 < this.FBDConfiguration.Length)
        {
          if (this.UnitID != 0)
            cpuParameterString = base.CpuParameterString + "/CFG=" + this.FBDConfiguration.ToString() + " /DA=" + this.UnitID.ToString() + " /CR=" + this.ConnectionRetries.ToString() + " ";
          else
            cpuParameterString = base.CpuParameterString + "/CFG=" + this.FBDConfiguration.ToString() + " /CR=" + this.ConnectionRetries.ToString() + " ";
        }
        else if (this.DestinationIPAddress != null && 0 < this.DestinationIPAddress.Length)
        {
          if (this.UnitID != 0)
            cpuParameterString = base.CpuParameterString + "/DAIP=" + this.DestinationIPAddress + " /PN=" + this.PortNumber.ToString() + " /DA=" + this.UnitID.ToString() + " /CR=" + this.ConnectionRetries.ToString() + " ";
          else
            cpuParameterString = base.CpuParameterString + "/DAIP=" + this.DestinationIPAddress + " /PN=" + this.PortNumber.ToString() + " /CR=" + this.ConnectionRetries.ToString() + " ";
        }
        else if (this.UnitID != 0)
          cpuParameterString = base.CpuParameterString + "/PN=" + this.PortNumber.ToString() + " /DA=" + this.UnitID.ToString() + " /CR=" + this.ConnectionRetries.ToString() + " ";
        else
          cpuParameterString = base.CpuParameterString + "/PN=" + this.PortNumber.ToString() + " /CR=" + this.ConnectionRetries.ToString() + " ";
        return cpuParameterString;
      }
    }

    public override string ToString() => this.DeviceParameterString + this.CpuParameterString.Trim();
  }
}
