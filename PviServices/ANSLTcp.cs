// Decompiled with JetBrains decompiler
// Type: BR.AN.PviServices.ANSLTcp
// Assembly: BR.AN.PVIServices, Version=1.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DA853F0A-C070-4E30-B7A7-51D955FB85F6
// Assembly location: C:\Users\ss_six\Desktop\FXE\FXE software integration package\COMET_FXE-PVIServices_Example\Pluto-PVI-trial_01\packages\IToolS.BandR.3.5.58\lib\net20\BR.AN.PVIServices.dll

using System;
using System.ComponentModel;
using System.Xml;

namespace BR.AN.PviServices
{
  [Serializable]
  public class ANSLTcp : DeviceBase
  {
    private uint remotePort;
    private uint communicationTimeout;
    private uint sendDelay;
    private bool propRedundancyCommMode;
    private uint communicationBufferSize;
    private string destinationIpAddress;

    internal override int ToXMLTextWriter(ref XmlTextWriter writer, ConfigurationFlags flags)
    {
      int xmlTextWriter = base.ToXMLTextWriter(ref writer, flags);
      if (this.DestinationIpAddress != null && this.DestinationIpAddress.Length > 0)
        writer.WriteAttributeString("DestinationIPAddress", this.DestinationIpAddress.ToString());
      if (0U < this.CommunicationTimeout)
        writer.WriteAttributeString("CommunicationTimeout", this.CommunicationTimeout.ToString());
      if (0U < this.CommunicationBufferSize)
        writer.WriteAttributeString("CommunicationBufferSize", this.CommunicationBufferSize.ToString());
      if (0U < this.SendDelay)
        writer.WriteAttributeString("SendDelay", this.SendDelay.ToString());
      if (0U < this.RemotePort)
        writer.WriteAttributeString("RemotePort", this.RemotePort.ToString());
      return xmlTextWriter;
    }

    public override int FromXmlTextReader(
      ref XmlTextReader reader,
      ConfigurationFlags flags,
      DeviceBase baseObj)
    {
      base.FromXmlTextReader(ref reader, flags, baseObj);
      ANSLTcp anslTcp = (ANSLTcp) baseObj;
      if (anslTcp == null)
        return -1;
      uint result = 0;
      string str = "";
      string attribute1 = reader.GetAttribute("remotePort");
      if (attribute1 != null && attribute1.Length > 0 && PviParse.TryParseUInt32(attribute1, out result))
        anslTcp.remotePort = result;
      str = "";
      string attribute2 = reader.GetAttribute("CommunicationTimeout");
      if (attribute2 != null && attribute2.Length > 0 && PviParse.TryParseUInt32(attribute2, out result))
        anslTcp.communicationTimeout = result;
      str = "";
      string attribute3 = reader.GetAttribute("CommunicationBufferSize");
      if (attribute3 != null && attribute3.Length > 0 && PviParse.TryParseUInt32(attribute3, out result))
        anslTcp.communicationBufferSize = result;
      str = "";
      string attribute4 = reader.GetAttribute("SendDelay");
      if (attribute4 != null && attribute4.Length > 0 && PviParse.TryParseUInt32(attribute4, out result))
        anslTcp.sendDelay = result;
      str = "";
      this.destinationIpAddress = "";
      this.destinationIpAddress = reader.GetAttribute("DestinationIPAddress");
      return 0;
    }

    [Browsable(false)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    private void UpdateParameters(string parameters, bool bCpu)
    {
      uint paraValue = 0;
      if (parameters == null)
        return;
      string[] strArray = parameters.Split(" ".ToCharArray());
      for (int index = 0; index < strArray.Length; ++index)
      {
        bool flag = false;
        string str1 = (string) strArray.GetValue(index);
        if (DeviceBase.UpdateParameterFromString("/IF=", str1, ref this.propInterfaceName))
        {
          if (this.propInterfaceName.ToLower().CompareTo("tcpip") != 0)
          {
            this.propInterfaceName = "tcpip";
            str1 = (string) null;
          }
          flag = true;
        }
        else if (DeviceBase.UpdateParameterFromString("/IP=", str1, ref this.destinationIpAddress))
          flag = true;
        else if (DeviceBase.UpdateParameterFromString("/DAIP=", str1, ref this.destinationIpAddress))
          flag = true;
        else if (DeviceBase.UpdateParameterFromString("/PT=", str1, ref this.remotePort))
          flag = true;
        else if (DeviceBase.UpdateParameterFromString("/COMT=", str1, ref this.communicationTimeout))
          flag = true;
        else if (DeviceBase.UpdateParameterFromString("/SDT=", str1, ref this.sendDelay))
          flag = true;
        else if (DeviceBase.UpdateParameterFromString("/BSIZE=", str1, ref this.communicationBufferSize))
          flag = true;
        else if (DeviceBase.UpdateParameterFromString("/RED=", str1, ref paraValue))
        {
          this.propRedundancyCommMode = false;
          if (paraValue != 0U)
            this.propRedundancyCommMode = true;
          flag = true;
        }
        else if (bCpu)
          base.UpdateCpuParameters(str1);
        else
          base.UpdateDeviceParameters(str1);
        if (flag && str1 != null)
        {
          string str2 = str1;
          if (str1.IndexOf("/") != 0)
            str2 = "/" + str1;
          if (bCpu)
          {
            if (this.propKnownCpuParameters.Length == 0)
            {
              this.propKnownCpuParameters = str2.Trim();
            }
            else
            {
              ANSLTcp anslTcp = this;
              anslTcp.propKnownCpuParameters = anslTcp.propKnownCpuParameters + " " + str2.Trim();
            }
          }
          else if (this.propKnownDeviceParameters.Length == 0)
          {
            this.propKnownDeviceParameters = str2.Trim();
          }
          else
          {
            ANSLTcp anslTcp = this;
            anslTcp.propKnownDeviceParameters = anslTcp.propKnownDeviceParameters + " " + str2.Trim();
          }
        }
      }
    }

    [EditorBrowsable(EditorBrowsableState.Never)]
    [Browsable(false)]
    public override void UpdateDeviceParameters(string parameters)
    {
      this.propKnownDeviceParameters = "";
      this.UpdateParameters(parameters, false);
    }

    [Browsable(false)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public override void UpdateCpuParameters(string parameters)
    {
      this.propKnownCpuParameters = "";
      this.UpdateParameters(parameters, true);
    }

    public ANSLTcp()
      : base(DeviceType.ANSLTcp)
    {
      this.Init();
    }

    internal override void Init()
    {
      base.Init();
      this.propDeviceType = DeviceType.ANSLTcp;
      this.communicationBufferSize = 0U;
      this.communicationTimeout = 1500U;
      this.sendDelay = 0U;
      this.destinationIpAddress = "127.0.0.1";
      this.remotePort = 11169U;
      this.propInterfaceName = "tcpip";
    }

    [CLSCompliant(false)]
    [PviCpuParameter]
    [PviKeyWord("/PT")]
    public uint RemotePort
    {
      get => this.remotePort;
      set => this.remotePort = value;
    }

    [CLSCompliant(false)]
    [PviKeyWord("/COMT")]
    [PviCpuParameter]
    public uint CommunicationTimeout
    {
      get => this.communicationTimeout;
      set => this.communicationTimeout = value;
    }

    [PviKeyWord("/SDT")]
    [CLSCompliant(false)]
    [PviCpuParameter]
    public uint SendDelay
    {
      get => this.sendDelay;
      set => this.sendDelay = value;
    }

    public bool RedundancyCommMode
    {
      get => this.propRedundancyCommMode;
      set => this.propRedundancyCommMode = value;
    }

    [PviKeyWord("/BSIZE")]
    [PviCpuParameter]
    [CLSCompliant(false)]
    public uint CommunicationBufferSize
    {
      get => this.communicationBufferSize;
      set => this.communicationBufferSize = value;
    }

    [PviCpuParameter]
    [PviKeyWord("/IP")]
    public string DestinationIpAddress
    {
      get => this.destinationIpAddress;
      set => this.destinationIpAddress = value;
    }

    public override string DeviceParameterString => base.DeviceParameterString;

    public override string CpuParameterString
    {
      get
      {
        string cpuParameterString = base.CpuParameterString + "/IP=" + this.DestinationIpAddress + " /PT=" + this.remotePort.ToString() + " /COMT=" + this.CommunicationTimeout.ToString();
        if (0U < this.SendDelay)
          cpuParameterString = cpuParameterString + " /SDT=" + this.SendDelay.ToString();
        if (this.CommunicationBufferSize != 0U && 65536U != this.CommunicationBufferSize)
          cpuParameterString = cpuParameterString + " /BSIZE=" + this.CommunicationBufferSize.ToString();
        return cpuParameterString;
      }
    }

    public override string ToString() => this.DeviceParameterString + this.CpuParameterString.Trim();
  }
}
