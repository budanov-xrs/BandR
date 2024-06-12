// Decompiled with JetBrains decompiler
// Type: BR.AN.PviServices.TcpIp
// Assembly: BR.AN.PVIServices, Version=1.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DA853F0A-C070-4E30-B7A7-51D955FB85F6
// Assembly location: C:\Users\ss_six\Desktop\FXE\FXE software integration package\COMET_FXE-PVIServices_Example\Pluto-PVI-trial_01\packages\IToolS.BandR.3.5.58\lib\net20\BR.AN.PVIServices.dll

using System;
using System.ComponentModel;
using System.Xml;

namespace BR.AN.PviServices
{
  [Serializable]
  public class TcpIp : DeviceBase
  {
    private TCPModes propTcpMode;
    private string propTarget;
    private bool propUniqueDeviceForSAandLOPO;
    private short sourcePort;
    private uint propLOPO;
    private byte sourceStation;
    private short destinationPort;
    private uint propREPO;
    private byte destinationStation;
    private byte checkDestinationStation;
    private int propQuickDownload;
    private string destinationIpAddress;

    internal override int ToXMLTextWriter(ref XmlTextWriter writer, ConfigurationFlags flags)
    {
      int xmlTextWriter = base.ToXMLTextWriter(ref writer, flags);
      if (this.checkDestinationStation > (byte) 0)
        writer.WriteAttributeString("CheckDestinationStation", this.checkDestinationStation.ToString());
      if (this.sourcePort > (short) 0 || 0U < this.propLOPO)
        writer.WriteAttributeString("SourcePort", 0U < this.propLOPO ? this.propLOPO.ToString() : this.sourcePort.ToString());
      if (this.sourceStation > (byte) 0)
        writer.WriteAttributeString("SourceStation", this.sourceStation.ToString());
      if (this.destinationPort > (short) 0 || 0U < this.propREPO)
        writer.WriteAttributeString("DestinationPort", 0U < this.propREPO ? this.propREPO.ToString() : this.destinationPort.ToString());
      if (this.destinationStation > (byte) 0)
        writer.WriteAttributeString("DestinationStation", this.destinationStation.ToString());
      if (this.destinationIpAddress != null && this.destinationIpAddress.Length > 0)
        writer.WriteAttributeString("DestinationIPAddress", this.destinationIpAddress.ToString());
      if (this.QuickDownload != 0)
        writer.WriteAttributeString("QuickDownload", this.QuickDownload.ToString());
      if (this.propTarget != "")
        writer.WriteAttributeString("Target", this.propTarget);
      if (this.propUniqueDeviceForSAandLOPO)
        writer.WriteAttributeString("UniqueDeviceForSAandLOPO", this.propUniqueDeviceForSAandLOPO.ToString());
      return xmlTextWriter;
    }

    public override int FromXmlTextReader(
      ref XmlTextReader reader,
      ConfigurationFlags flags,
      DeviceBase baseObj)
    {
      base.FromXmlTextReader(ref reader, flags, baseObj);
      TcpIp tcpIp = (TcpIp) baseObj;
      if (tcpIp == null)
        return -1;
      int result1 = 0;
      byte result2 = 0;
      uint result3 = 0;
      string str = "";
      string attribute1 = reader.GetAttribute("CheckDestinationStation");
      if (attribute1 != null && attribute1.Length > 0 && PviParse.TryParseByte(attribute1, out result2))
        tcpIp.checkDestinationStation = result2;
      str = "";
      string attribute2 = reader.GetAttribute("SourcePort");
      if (attribute2 != null && attribute2.Length > 0 && PviParse.TryParseUInt32(attribute2, out result3))
      {
        tcpIp.propLOPO = result3;
        if ((uint) short.MaxValue >= tcpIp.propLOPO)
          tcpIp.sourcePort = System.Convert.ToInt16(tcpIp.propLOPO);
      }
      str = "";
      string attribute3 = reader.GetAttribute("SourceStation");
      if (attribute3 != null && attribute3.Length > 0 && PviParse.TryParseByte(attribute3, out result2))
        tcpIp.sourceStation = result2;
      str = "";
      string attribute4 = reader.GetAttribute("DestinationPort");
      if (attribute4 != null && attribute4.Length > 0 && PviParse.TryParseUInt32(attribute4, out result3))
      {
        tcpIp.propREPO = result3;
        if ((uint) short.MaxValue >= tcpIp.propREPO)
          tcpIp.destinationPort = System.Convert.ToInt16(tcpIp.propREPO);
      }
      str = "";
      string attribute5 = reader.GetAttribute("DestinationStation");
      if (attribute5 != null && attribute5.Length > 0 && PviParse.TryParseByte(attribute5, out result2))
        tcpIp.destinationStation = result2;
      str = "";
      string attribute6 = reader.GetAttribute("DestinationIPAddress");
      if (attribute6 != null && attribute6.Length > 0)
        tcpIp.destinationIpAddress = attribute6;
      str = "";
      string attribute7 = reader.GetAttribute("QuickDownload");
      if (attribute7 != null && attribute7.Length > 0 && PviParse.TryParseInt32(attribute7, out result1))
        tcpIp.propQuickDownload = result1;
      str = "";
      string attribute8 = reader.GetAttribute("Target");
      if (attribute8 != null && attribute8.Length > 0)
        this.propTarget = attribute8;
      str = "";
      string attribute9 = reader.GetAttribute("UniqueDeviceForSAandLOPO");
      this.propUniqueDeviceForSAandLOPO = false;
      if (attribute9 != null && attribute9.ToLower().CompareTo("true") == 0)
        this.propUniqueDeviceForSAandLOPO = true;
      return 0;
    }

    [EditorBrowsable(EditorBrowsableState.Never)]
    [Browsable(false)]
    private void UpdateParameters(string parameters, bool bCpu)
    {
      if (parameters == null)
        return;
      string[] strArray = parameters.Split(" ".ToCharArray());
      string paraValue = "";
      for (int index = 0; index < strArray.Length; ++index)
      {
        bool flag = false;
        string str1 = (string) strArray.GetValue(index);
        if (str1.ToUpper().StartsWith("/IF=TCP"))
        {
          this.propInterfaceName = str1.Substring(4);
          flag = true;
        }
        else if (DeviceBase.UpdateParameterFromString("/DAIP=", str1, ref this.destinationIpAddress))
          flag = true;
        else if (DeviceBase.UpdateParameterFromString("/LOPO=", str1, ref this.propLOPO))
        {
          if ((uint) short.MaxValue >= this.propLOPO)
            this.sourcePort = System.Convert.ToInt16(this.propLOPO);
          flag = true;
        }
        else if (DeviceBase.UpdateParameterFromString("/SA=", str1, ref this.sourceStation))
          flag = true;
        else if (DeviceBase.UpdateParameterFromString("/UDEV=", str1, ref paraValue))
        {
          this.propUniqueDeviceForSAandLOPO = false;
          if (paraValue.CompareTo("true") == 0)
            this.propUniqueDeviceForSAandLOPO = true;
          flag = true;
        }
        else if (DeviceBase.UpdateParameterFromString("/DA=", str1, ref this.destinationStation))
          flag = true;
        else if (DeviceBase.UpdateParameterFromString("/REPO=", str1, ref this.propREPO))
        {
          this.destinationPort = (short) 11159;
          if ((uint) short.MaxValue >= this.propREPO)
            this.destinationPort = System.Convert.ToInt16(this.propREPO);
          flag = true;
        }
        else if (DeviceBase.UpdateParameterFromString("/CKDA=", str1, ref this.checkDestinationStation))
          flag = true;
        else if (DeviceBase.UpdateParameterFromString("/ANSL=", str1, ref this.propQuickDownload))
          flag = true;
        else if (bCpu)
          base.UpdateCpuParameters(str1);
        else
          base.UpdateDeviceParameters(str1);
        if (flag)
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
              TcpIp tcpIp = this;
              tcpIp.propKnownCpuParameters = tcpIp.propKnownCpuParameters + " " + str2.Trim();
            }
          }
          else if (this.propKnownDeviceParameters.Length == 0)
          {
            this.propKnownDeviceParameters = str2.Trim();
          }
          else
          {
            TcpIp tcpIp = this;
            tcpIp.propKnownDeviceParameters = tcpIp.propKnownDeviceParameters + " " + str2.Trim();
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

    [EditorBrowsable(EditorBrowsableState.Never)]
    [Browsable(false)]
    public override void UpdateCpuParameters(string parameters)
    {
      this.propKnownCpuParameters = "";
      this.UpdateParameters(parameters, true);
    }

    internal override void Init()
    {
      base.Init();
      this.propDeviceType = DeviceType.TcpIp;
      this.InitTCpParams((byte) 0, (byte) 0, 0U, 0U, "");
      this.InterfaceName = "tcpip";
    }

    private void InitTCpParams(byte sa, byte da, uint sp, uint dp, string daip)
    {
      this.propTarget = (string) null;
      this.sourceStation = sa;
      this.destinationStation = da;
      this.propLOPO = sp;
      if ((uint) short.MaxValue >= this.propLOPO)
        this.sourcePort = System.Convert.ToInt16(this.propLOPO);
      this.propREPO = dp;
      if ((uint) short.MaxValue >= this.propREPO)
        this.destinationPort = System.Convert.ToInt16(this.propREPO);
      this.destinationIpAddress = daip;
      this.propQuickDownload = 1;
      this.propUniqueDeviceForSAandLOPO = false;
    }

    public TcpIp()
      : base(DeviceType.TcpIp)
    {
      this.Init();
      this.propInterfaceName = "tcpip";
    }

    public TcpIp(TCPModes mode)
      : base(DeviceType.TcpIp)
    {
      this.Init();
      this.propTcpMode = mode;
      this.InitMode(mode);
      this.propInterfaceName = "tcpip";
    }

    private void InitMode(TCPModes mode)
    {
      switch (mode)
      {
        case TCPModes.STANDARD:
          this.InitTCpParams((byte) 1, (byte) 2, 11159U, 11159U, "");
          break;
        case TCPModes.AR000:
          this.InitTCpParams((byte) 1, (byte) 2, 11159U, 11160U, "127.0.0.1");
          break;
        case TCPModes.AR010:
          this.InitTCpParams((byte) 1, (byte) 2, 0U, 0U, "192.168.0.2");
          break;
        default:
          this.InitTCpParams((byte) 0, (byte) 0, 0U, 0U, "");
          break;
      }
    }

    public TCPModes TcpMode
    {
      get => this.propTcpMode;
      set
      {
        this.propTcpMode = value;
        this.InitMode(this.propTcpMode);
      }
    }

    [PviKeyWord("/CKDA")]
    [PviCpuParameter]
    public byte CheckDestinationStation
    {
      get => this.checkDestinationStation;
      set => this.checkDestinationStation = value;
    }

    [CLSCompliant(false)]
    [PviKeyWord("/LOPO")]
    public uint LocalPort
    {
      get => this.propLOPO;
      set => this.propLOPO = value;
    }

    public short SourcePort
    {
      get => this.sourcePort;
      set => this.sourcePort = value;
    }

    [PviCpuParameter]
    [PviKeyWord("/TA")]
    public string Target
    {
      get => this.propTarget;
      set => this.propTarget = value;
    }

    [PviKeyWord("/SA")]
    public byte SourceStation
    {
      get => this.sourceStation;
      set => this.sourceStation = value;
    }

    [PviKeyWord("/REPO")]
    [PviCpuParameter]
    [CLSCompliant(false)]
    public uint RemotePort
    {
      get => this.propREPO;
      set => this.propREPO = value;
    }

    public short DestinationPort
    {
      get => this.destinationPort;
      set => this.destinationPort = value;
    }

    [PviKeyWord("/ANSL")]
    [PviCpuParameter]
    public int QuickDownload
    {
      get => this.propQuickDownload;
      set => this.propQuickDownload = value;
    }

    [PviKeyWord("/DA")]
    [PviCpuParameter]
    public byte DestinationStation
    {
      get => this.destinationStation;
      set => this.destinationStation = value;
    }

    [PviKeyWord("/UDEV")]
    public bool UniqueDeviceForSAandLOPO
    {
      get => this.propUniqueDeviceForSAandLOPO;
      set => this.propUniqueDeviceForSAandLOPO = value;
    }

    [PviKeyWord("/DAIP")]
    [PviCpuParameter]
    public string DestinationIpAddress
    {
      get => this.destinationIpAddress;
      set => this.destinationIpAddress = value;
    }

    public override string DeviceParameterString => base.DeviceParameterString + "/LOPO=" + (0U < this.propLOPO ? this.propLOPO.ToString() : this.sourcePort.ToString()) + " /SA=" + this.sourceStation.ToString() + (this.propUniqueDeviceForSAandLOPO ? " /UDEV=1 " : " ");

    public override string CpuParameterString
    {
      get
      {
        string cpuParameterString;
        if (this.DestinationIpAddress != null && 0 < this.DestinationIpAddress.Length)
        {
          if (this.DestinationStation != (byte) 0)
            cpuParameterString = base.CpuParameterString + " /DA=" + this.DestinationStation.ToString() + " /CKDA=" + this.checkDestinationStation.ToString() + " /REPO=" + (0U < this.propREPO ? this.propREPO.ToString() : this.destinationPort.ToString()) + (this.Target == null || 0 >= this.Target.Length ? "" : " /TA=" + this.Target) + " /ANSL=" + this.propQuickDownload.ToString() + " ";
          else
            cpuParameterString = base.CpuParameterString + "/DAIP=" + this.DestinationIpAddress + " /CKDA=" + this.checkDestinationStation.ToString() + " /REPO=" + (0U < this.propREPO ? this.propREPO.ToString() : this.destinationPort.ToString()) + (this.Target == null || 0 >= this.Target.Length ? "" : " /TA=" + this.Target) + " /ANSL=" + this.propQuickDownload.ToString() + " ";
        }
        else if (this.DestinationStation != (byte) 0)
          cpuParameterString = base.CpuParameterString + "/DA=" + this.DestinationStation.ToString() + " /CKDA=" + this.checkDestinationStation.ToString() + " /REPO=" + (0U < this.propREPO ? this.propREPO.ToString() : this.destinationPort.ToString()) + (this.Target == null || 0 >= this.Target.Length ? "" : " /TA=" + this.Target) + " /ANSL=" + this.propQuickDownload.ToString() + " ";
        else
          cpuParameterString = base.CpuParameterString + "/CKDA=" + this.checkDestinationStation.ToString() + " /REPO=" + (0U < this.propREPO ? this.propREPO.ToString() : this.destinationPort.ToString()) + (this.Target == null || 0 >= this.Target.Length ? "" : " /TA=" + this.Target) + " /ANSL=" + this.propQuickDownload.ToString() + " ";
        return cpuParameterString;
      }
    }

    public override string ToString() => this.DeviceParameterString + this.CpuParameterString.Trim();
  }
}
