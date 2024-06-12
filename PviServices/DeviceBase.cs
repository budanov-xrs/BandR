// Decompiled with JetBrains decompiler
// Type: BR.AN.PviServices.DeviceBase
// Assembly: BR.AN.PVIServices, Version=1.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DA853F0A-C070-4E30-B7A7-51D955FB85F6
// Assembly location: C:\Users\ss_six\Desktop\FXE\FXE software integration package\COMET_FXE-PVIServices_Example\Pluto-PVI-trial_01\packages\IToolS.BandR.3.5.58\lib\net20\BR.AN.PVIServices.dll

using System;
using System.ComponentModel;
using System.Xml;

namespace BR.AN.PviServices
{
  [Serializable]
  public abstract class DeviceBase
  {
    protected string propKnownDeviceParameters;
    private string propUnknownDevParameters;
    protected string propKnownCpuParameters;
    private string propUnknownCpuParameters;
    private string propSavePath;
    private string propRoutingPath;
    protected int propIntervalTimeout;
    protected string propInterfaceName;
    private int propResponseTimeout;
    internal DeviceType propDeviceType;

    public DeviceBase(DeviceType type)
    {
      this.Init();
      this.propDeviceType = type;
      this.UpdateInterfaceName(type);
    }

    private void UpdateInterfaceName(DeviceType type)
    {
      switch (type)
      {
        case DeviceType.TcpIp:
          this.propInterfaceName = "tcpip";
          break;
        case DeviceType.Can:
          this.propInterfaceName = "inacan1";
          break;
        case DeviceType.Shared:
          this.propInterfaceName = "LS251_1";
          break;
        case DeviceType.Modem:
          this.propInterfaceName = "modem1";
          break;
        case DeviceType.AR000:
          this.propInterfaceName = "SPWIN";
          break;
        case DeviceType.ANSLTcp:
          this.propInterfaceName = "tcpip";
          break;
        case DeviceType.TcpIpMODBUS:
          this.propInterfaceName = "MBUSTCP";
          break;
        default:
          this.propInterfaceName = "COM1";
          break;
      }
    }

    internal DeviceBase(DeviceType type, ref XmlTextReader reader, ConfigurationFlags flags)
    {
      this.Init();
      this.propDeviceType = type;
      this.UpdateInterfaceName(type);
    }

    internal static bool UpdateParameterFromString(
      string strParam,
      string strConnection,
      ref byte paraValue)
    {
      string str = strConnection.Replace('"', char.MinValue);
      if (!str.ToUpper().StartsWith(strParam))
        return false;
      if (0 < str.Substring(strParam.Length).Length)
        paraValue = -1 == str.Substring(strParam.Length).ToUpper().IndexOf("0X") ? System.Convert.ToByte(str.Substring(strParam.Length)) : System.Convert.ToByte(str.Substring(strParam.Length + 2), 16);
      return true;
    }

    internal static bool UpdateParameterFromString(
      string strParam,
      string strConnection,
      ref uint paraValue)
    {
      string str = strConnection.Replace('"', char.MinValue);
      if (!str.ToUpper().StartsWith(strParam))
        return false;
      if (0 < str.Substring(strParam.Length).Length)
        paraValue = -1 == str.Substring(strParam.Length).ToUpper().IndexOf("0X") ? System.Convert.ToUInt32(str.Substring(strParam.Length)) : System.Convert.ToUInt32(str.Substring(strParam.Length + 2), 16);
      return true;
    }

    internal static bool UpdateParameterFromString(
      string strParam,
      string strConnection,
      ref int paraValue)
    {
      string str = strConnection.Replace('"', char.MinValue);
      if (!str.ToUpper().StartsWith(strParam))
        return false;
      if (0 < str.Substring(strParam.Length).Length)
        paraValue = -1 == str.Substring(strParam.Length).ToUpper().IndexOf("0X") ? System.Convert.ToInt32(str.Substring(strParam.Length)) : System.Convert.ToInt32(str.Substring(strParam.Length + 2), 16);
      return true;
    }

    internal static bool UpdateParameterFromString(
      string strParam,
      string strConnection,
      ref string paraValue)
    {
      if (!strConnection.ToUpper().StartsWith(strParam))
        return false;
      paraValue = "";
      if (0 < strConnection.Substring(strParam.Length).Length)
      {
        paraValue = strConnection.Substring(strParam.Length);
        paraValue.Trim();
      }
      return true;
    }

    internal virtual void Init()
    {
      this.propSavePath = "";
      this.propDeviceType = DeviceType.Serial;
      this.propResponseTimeout = 0;
      this.propRoutingPath = "";
      this.propInterfaceName = "COM1";
      this.propIntervalTimeout = 0;
      this.propUnknownCpuParameters = "";
      this.propKnownCpuParameters = "";
      this.propUnknownDevParameters = "";
      this.propKnownDeviceParameters = "";
    }

    internal virtual int ToXMLTextWriter(ref XmlTextWriter writer, ConfigurationFlags flags)
    {
      writer.WriteAttributeString("DeviceType", this.propDeviceType.ToString());
      int xmlTextWriter = this.SaveConnectionAttributes(ref writer);
      if (this.propSavePath != null && this.propSavePath.Length > 0)
        writer.WriteAttributeString("SavePath", this.propSavePath);
      if (this.InterfaceName != null && this.InterfaceName.Length > 0)
        writer.WriteAttributeString("InterfaceName", this.propInterfaceName);
      if (this.propKnownCpuParameters != null && this.propKnownCpuParameters.Length > 0)
        writer.WriteAttributeString("KnownCpuParameters", this.propKnownCpuParameters);
      if (this.propKnownDeviceParameters != null && this.propKnownDeviceParameters.Length > 0)
        writer.WriteAttributeString("KnownDeviceParameters", this.propKnownDeviceParameters);
      if (this.propUnknownCpuParameters != null && this.propUnknownCpuParameters.Length > 0)
        writer.WriteAttributeString("UnknownCpuParameters", this.propUnknownCpuParameters);
      if (this.propUnknownDevParameters != null && this.propUnknownDevParameters.Length > 0)
        writer.WriteAttributeString("UnknownDevParameters", this.propUnknownDevParameters);
      return xmlTextWriter;
    }

    public int SaveConnectionAttributes(ref XmlTextWriter writer)
    {
      if (this.propRoutingPath != null && this.propRoutingPath.Length > 0)
        writer.WriteAttributeString("RoutingPath", this.propRoutingPath.ToString());
      if (this.propResponseTimeout != 0)
        writer.WriteAttributeString("ResponseTimeout", this.propResponseTimeout.ToString());
      if (this.propIntervalTimeout != 0)
        writer.WriteAttributeString("IntervalTimeout", this.propIntervalTimeout.ToString());
      return 0;
    }

    public virtual int FromXmlTextReader(
      ref XmlTextReader reader,
      ConfigurationFlags flags,
      DeviceBase baseObj)
    {
      string str = "";
      string attribute1 = reader.GetAttribute("DeviceType");
      if (attribute1 != null && attribute1.Length > 0)
      {
        switch (attribute1.ToLower())
        {
          case "ansl":
            this.propDeviceType = DeviceType.ANSLTcp;
            break;
          case "ar010":
          case "arwin":
          case "tcpip":
            this.propDeviceType = DeviceType.TcpIp;
            break;
          case "tcpipmodbus":
            this.propDeviceType = DeviceType.TcpIpMODBUS;
            break;
          case "shared":
            this.propDeviceType = DeviceType.Shared;
            break;
          case "serial":
            this.propDeviceType = DeviceType.Serial;
            break;
          case "modem":
            this.propDeviceType = DeviceType.Modem;
            break;
          case "can":
            this.propDeviceType = DeviceType.Can;
            break;
          case "ar000":
          case "arsim":
            this.propDeviceType = DeviceType.AR000;
            break;
        }
      }
      str = "";
      string attribute2 = reader.GetAttribute("SavePath");
      if (attribute2 != null && attribute2.Length > 0)
        baseObj.propSavePath = attribute2;
      str = "";
      string attribute3 = reader.GetAttribute("InterfaceName");
      if (attribute3 != null && attribute3.Length > 0)
        baseObj.propInterfaceName = attribute3;
      str = "";
      string attribute4 = reader.GetAttribute("KnownCpuParameters");
      if (attribute4 != null && attribute4.Length > 0)
        baseObj.propKnownCpuParameters = attribute4;
      str = "";
      string attribute5 = reader.GetAttribute("KnownDeviceParameters");
      if (attribute5 != null && attribute5.Length > 0)
        baseObj.propKnownDeviceParameters = attribute5;
      str = "";
      string attribute6 = reader.GetAttribute("UnknownCpuParameters");
      if (attribute6 != null && attribute6.Length > 0)
        baseObj.propUnknownCpuParameters = attribute6;
      str = "";
      string attribute7 = reader.GetAttribute("UnknownDevParameters");
      if (attribute7 != null && attribute7.Length > 0)
        baseObj.propUnknownDevParameters = attribute7;
      str = "";
      string attribute8 = reader.GetAttribute("RoutingPath");
      if (attribute8 != null && attribute8.Length > 0)
        baseObj.propRoutingPath = attribute8;
      str = "";
      string attribute9 = reader.GetAttribute("ResponseTimeout");
      if (attribute9 != null && attribute9.Length > 0)
      {
        int result = 0;
        if (PviParse.TryParseInt32(attribute9, out result))
          baseObj.propResponseTimeout = result;
      }
      str = "";
      string attribute10 = reader.GetAttribute("IntervalTimeout");
      if (attribute10 != null && attribute10.Length > 0)
      {
        int result = 0;
        if (PviParse.TryParseInt32(attribute10, out result))
          baseObj.propIntervalTimeout = result;
      }
      return 0;
    }

    [Browsable(false)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public virtual void UpdateDeviceParameters(string parmItem)
    {
      string[] strArray = parmItem.Split(" ".ToCharArray());
      for (int index = 0; index < strArray.Length; ++index)
      {
        bool flag = false;
        string strConnection = (string) strArray.GetValue(index);
        if (DeviceBase.UpdateParameterFromString("/IF=", strConnection, ref this.propInterfaceName))
          flag = true;
        else if (DeviceBase.UpdateParameterFromString("/RT=", strConnection, ref this.propResponseTimeout))
          flag = true;
        else if (DeviceBase.UpdateParameterFromString("RT=", strConnection, ref this.propResponseTimeout))
          flag = true;
        else if (DeviceBase.UpdateParameterFromString("/IT=", strConnection, ref this.propIntervalTimeout))
          flag = true;
        else if (DeviceBase.UpdateParameterFromString("IT=", strConnection, ref this.propIntervalTimeout))
          flag = true;
        else if (-1 == this.propUnknownDevParameters.IndexOf(strConnection) && -1 == this.propKnownDeviceParameters.IndexOf(strConnection))
        {
          string str = strConnection;
          if (strConnection.IndexOf("/") != 0)
            str = "/" + strConnection;
          if (this.propUnknownDevParameters.Length == 0)
          {
            this.propUnknownDevParameters = str.Trim();
          }
          else
          {
            DeviceBase deviceBase = this;
            deviceBase.propUnknownDevParameters = deviceBase.propUnknownDevParameters + " " + str.Trim();
          }
        }
        if (flag)
        {
          string str = strConnection;
          if (strConnection.IndexOf("/") != 0)
            str = "/" + strConnection;
          if (this.propKnownDeviceParameters.Length == 0)
          {
            this.propKnownDeviceParameters = str.Trim();
          }
          else
          {
            DeviceBase deviceBase = this;
            deviceBase.propKnownDeviceParameters = deviceBase.propKnownDeviceParameters + " " + str.Trim();
          }
        }
      }
    }

    [EditorBrowsable(EditorBrowsableState.Never)]
    [Browsable(false)]
    public virtual void UpdateCpuParameters(string parmItem)
    {
      string[] strArray = parmItem.Split(" ".ToCharArray());
      for (int index = 0; index < strArray.Length; ++index)
      {
        bool flag = false;
        string strConnection = (string) strArray.GetValue(index);
        if (DeviceBase.UpdateParameterFromString("/IF=", strConnection, ref this.propInterfaceName))
        {
          if (this.propKnownDeviceParameters.Length == 0)
          {
            this.propKnownDeviceParameters = strConnection.Trim();
          }
          else
          {
            DeviceBase deviceBase = this;
            deviceBase.propKnownDeviceParameters = deviceBase.propKnownDeviceParameters + " " + strConnection.Trim();
          }
        }
        else if (DeviceBase.UpdateParameterFromString("/RT=", strConnection, ref this.propResponseTimeout))
          flag = true;
        else if (DeviceBase.UpdateParameterFromString("RT=", strConnection, ref this.propResponseTimeout))
          flag = true;
        else if (DeviceBase.UpdateParameterFromString("/IT=", strConnection, ref this.propIntervalTimeout))
          flag = true;
        else if (DeviceBase.UpdateParameterFromString("IT=", strConnection, ref this.propIntervalTimeout))
          flag = true;
        else if (DeviceBase.UpdateParameterFromString("/SP=", strConnection, ref this.propSavePath))
          flag = true;
        else if (DeviceBase.UpdateParameterFromString("SP=", strConnection, ref this.propSavePath))
          flag = true;
        else if (DeviceBase.UpdateParameterFromString("/CN=", strConnection, ref this.propRoutingPath))
          flag = true;
        else if (DeviceBase.UpdateParameterFromString("CN=", strConnection, ref this.propRoutingPath))
          flag = true;
        else if (-1 == this.propUnknownCpuParameters.IndexOf(strConnection) && -1 == this.propKnownCpuParameters.IndexOf(strConnection))
        {
          string str = strConnection;
          if (strConnection.IndexOf("/") != 0)
            str = "/" + strConnection;
          if (this.propUnknownCpuParameters.Length == 0)
          {
            this.propUnknownCpuParameters = str.Trim();
          }
          else
          {
            DeviceBase deviceBase = this;
            deviceBase.propUnknownCpuParameters = deviceBase.propUnknownCpuParameters + " " + str.Trim();
          }
        }
        if (flag)
        {
          string str = strConnection;
          if (strConnection.IndexOf("/") != 0)
            str = "/" + strConnection;
          if (this.propKnownCpuParameters.Length == 0)
          {
            this.propKnownCpuParameters = str.Trim();
          }
          else
          {
            DeviceBase deviceBase = this;
            deviceBase.propKnownCpuParameters = deviceBase.propKnownCpuParameters + " " + str.Trim();
          }
        }
      }
    }

    [EditorBrowsable(EditorBrowsableState.Never)]
    [Browsable(false)]
    public string KnownDeviceParameters => this.propKnownDeviceParameters;

    public virtual string DeviceParameterString
    {
      get
      {
        string deviceParameterString = "/IF=" + this.InterfaceName + " ";
        if (this.propUnknownDevParameters != null && 0 < this.propUnknownDevParameters.Length)
          deviceParameterString = deviceParameterString + this.propUnknownDevParameters + " ";
        return deviceParameterString;
      }
    }

    public virtual string CpuParameterString
    {
      get
      {
        string str1 = "";
        string str2 = "";
        string str3 = "";
        if (this.RoutingPath != null && 0 < this.RoutingPath.Length)
          str1 = "/CN=" + this.RoutingPath + " ";
        if (this.ResponseTimeout != 0)
          str2 = "/RT=" + this.ResponseTimeout.ToString() + " ";
        if (this.SavePath != null && 0 < this.SavePath.Length)
          str3 = "/SP=" + this.SavePath + " ";
        string cpuParameterString = str1 + str2 + str3;
        if (this.propUnknownCpuParameters != null && 0 < this.propUnknownCpuParameters.Length)
          cpuParameterString = cpuParameterString + this.propUnknownCpuParameters + " ";
        return cpuParameterString;
      }
    }

    [EditorBrowsable(EditorBrowsableState.Never)]
    [Browsable(false)]
    public string UnknownDeviceParameters => this.propUnknownDevParameters;

    [Browsable(false)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public string KnownCpuParameters => this.propKnownCpuParameters;

    [EditorBrowsable(EditorBrowsableState.Never)]
    [Browsable(false)]
    public string UnknownCpuParameters => this.propUnknownCpuParameters;

    [PviKeyWord("/SP")]
    [PviCpuParameter]
    internal string SavePath
    {
      get => this.propSavePath;
      set => this.propSavePath = value;
    }

    [PviKeyWord("/CN")]
    [PviCpuParameter]
    public string RoutingPath
    {
      get => this.propRoutingPath;
      set => this.propRoutingPath = value;
    }

    [PviKeyWord("/IT")]
    public int IntervalTimeout
    {
      get => this.propIntervalTimeout;
      set => this.propIntervalTimeout = value;
    }

    [PviKeyWord("/IF")]
    public string InterfaceName
    {
      get => this.propInterfaceName;
      set => this.propInterfaceName = value;
    }

    [PviKeyWord("/RT")]
    [PviCpuParameter]
    public int ResponseTimeout
    {
      get => this.propResponseTimeout;
      set => this.propResponseTimeout = value;
    }

    public DeviceType DeviceType => this.propDeviceType;
  }
}
