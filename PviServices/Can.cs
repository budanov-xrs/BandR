// Decompiled with JetBrains decompiler
// Type: BR.AN.PviServices.Can
// Assembly: BR.AN.PVIServices, Version=1.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DA853F0A-C070-4E30-B7A7-51D955FB85F6
// Assembly location: C:\Users\ss_six\Desktop\FXE\FXE software integration package\COMET_FXE-PVIServices_Example\Pluto-PVI-trial_01\packages\IToolS.BandR.3.5.58\lib\net20\BR.AN.PVIServices.dll

using System;
using System.ComponentModel;
using System.Xml;

namespace BR.AN.PviServices
{
  [Serializable]
  public class Can : DeviceBase
  {
    private int propCANIdentifiers;
    private int propMAXStationNumber;
    private int propControllerNumber;
    private byte channel;
    private int interruptNumber;
    private int ioPort;
    private int messageCount;
    private int cycleTime;
    private int baseId;
    private int baudRate;
    private int sourceAddress;
    private int destinationAddress;

    internal override int ToXMLTextWriter(ref XmlTextWriter writer, ConfigurationFlags flags)
    {
      int xmlTextWriter = base.ToXMLTextWriter(ref writer, flags);
      if (this.channel != (byte) 1)
        writer.WriteAttributeString("Channel", this.channel.ToString());
      if (this.baseId != 1598)
        writer.WriteAttributeString("BaseID", this.baseId.ToString());
      if (this.baudRate != 500000)
        writer.WriteAttributeString("Baudrate", this.baudRate.ToString());
      if (this.cycleTime != 10)
        writer.WriteAttributeString("CycleTime", this.cycleTime.ToString());
      if (this.MessageCount != 10)
        writer.WriteAttributeString("Messages", this.messageCount.ToString());
      if (this.sourceAddress != 1)
        writer.WriteAttributeString("Source", this.sourceAddress.ToString());
      if (this.destinationAddress != 2)
        writer.WriteAttributeString("Destination", this.destinationAddress.ToString());
      if (this.interruptNumber != 10)
        writer.WriteAttributeString("IRQ", this.interruptNumber.ToString());
      if (this.ioPort != 900)
        writer.WriteAttributeString("Port", this.ioPort.ToString());
      writer.WriteAttributeString("Stations", this.propMAXStationNumber.ToString());
      writer.WriteAttributeString("Controller", this.propControllerNumber.ToString());
      writer.WriteAttributeString("Identifiers", this.propCANIdentifiers.ToString());
      return xmlTextWriter;
    }

    public override int FromXmlTextReader(
      ref XmlTextReader reader,
      ConfigurationFlags flags,
      DeviceBase baseObj)
    {
      base.FromXmlTextReader(ref reader, flags, baseObj);
      Can can = (Can) baseObj;
      if (can == null)
        return -1;
      int result1 = 0;
      string str = "";
      string attribute1 = reader.GetAttribute("Channel");
      if (attribute1 != null && attribute1.Length > 0)
      {
        byte result2 = 0;
        if (PviParse.TryParseByte(attribute1, out result2))
          can.channel = result2;
      }
      str = "";
      string attribute2 = reader.GetAttribute("BaseID");
      if (attribute2 != null && attribute2.Length > 0 && PviParse.TryParseInt32(attribute2, out result1))
        can.baseId = result1;
      str = "";
      string attribute3 = reader.GetAttribute("Baudrate");
      if (attribute3 != null && attribute3.Length > 0 && PviParse.TryParseInt32(attribute3, out result1))
        can.baudRate = result1;
      str = "";
      string attribute4 = reader.GetAttribute("CycleTime");
      if (attribute4 != null && attribute4.Length > 0 && PviParse.TryParseInt32(attribute4, out result1))
        can.cycleTime = result1;
      str = "";
      string attribute5 = reader.GetAttribute("Messages");
      if (attribute5 != null && attribute5.Length > 0 && PviParse.TryParseInt32(attribute5, out result1))
        can.messageCount = result1;
      str = "";
      string attribute6 = reader.GetAttribute("Source");
      if (attribute6 != null && attribute6.Length > 0 && PviParse.TryParseInt32(attribute6, out result1))
        can.sourceAddress = result1;
      str = "";
      string attribute7 = reader.GetAttribute("Destination");
      if (attribute7 != null && attribute7.Length > 0 && PviParse.TryParseInt32(attribute7, out result1))
        can.destinationAddress = result1;
      str = "";
      string attribute8 = reader.GetAttribute("IRQ");
      if (attribute8 != null && attribute8.Length > 0 && PviParse.TryParseInt32(attribute8, out result1))
        can.interruptNumber = result1;
      str = "";
      string attribute9 = reader.GetAttribute("Port");
      if (attribute9 != null && attribute9.Length > 0 && PviParse.TryParseInt32(attribute9, out result1))
        can.ioPort = result1;
      str = "";
      string attribute10 = reader.GetAttribute("Stations");
      if (attribute10 != null && attribute10.Length > 0 && PviParse.TryParseInt32(attribute10, out result1))
        can.propMAXStationNumber = result1;
      str = "";
      string attribute11 = reader.GetAttribute("Controller");
      if (attribute11 != null && attribute11.Length > 0 && PviParse.TryParseInt32(attribute11, out result1))
        can.propControllerNumber = result1;
      str = "";
      string attribute12 = reader.GetAttribute("Identifiers");
      if (attribute12 != null && attribute12.Length > 0 && PviParse.TryParseInt32(attribute12, out result1))
        can.propCANIdentifiers = result1;
      return 0;
    }

    private void UpdateParameters(string parameters, bool bCpu)
    {
      string[] strArray = parameters.Split(" ".ToCharArray());
      for (int index = 0; index < strArray.Length; ++index)
      {
        bool flag = false;
        string str1 = (string) strArray.GetValue(index);
        if (str1.ToUpper().StartsWith("/IF=INACAN"))
        {
          this.propInterfaceName = str1.Substring(4);
          this.channel = System.Convert.ToByte(str1.Substring(10));
          flag = true;
        }
        else if (DeviceBase.UpdateParameterFromString("/CNO=", str1, ref this.propControllerNumber))
          flag = true;
        else if (DeviceBase.UpdateParameterFromString("/BI=", str1, ref this.baseId))
          flag = true;
        else if (DeviceBase.UpdateParameterFromString("/IT=", str1, ref this.propIntervalTimeout))
          flag = true;
        else if (DeviceBase.UpdateParameterFromString("/MDA=", str1, ref this.propMAXStationNumber))
          flag = true;
        else if (DeviceBase.UpdateParameterFromString("/BD=", str1, ref this.baudRate))
          flag = true;
        else if (DeviceBase.UpdateParameterFromString("/CMODE=", str1, ref this.propCANIdentifiers))
          flag = true;
        else if (DeviceBase.UpdateParameterFromString("/CT=", str1, ref this.cycleTime))
          flag = true;
        else if (DeviceBase.UpdateParameterFromString("/MC=", str1, ref this.messageCount))
          flag = true;
        else if (DeviceBase.UpdateParameterFromString("/SA=", str1, ref this.sourceAddress))
          flag = true;
        else if (DeviceBase.UpdateParameterFromString("/DA=", str1, ref this.destinationAddress))
          flag = true;
        else if (DeviceBase.UpdateParameterFromString("/IR=", str1, ref this.interruptNumber))
          flag = true;
        else if (DeviceBase.UpdateParameterFromString("/IO=", str1, ref this.ioPort))
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
              Can can = this;
              can.propKnownCpuParameters = can.propKnownCpuParameters + " " + str2.Trim();
            }
          }
          else if (this.propKnownDeviceParameters.Length == 0)
          {
            this.propKnownDeviceParameters = str2.Trim();
          }
          else
          {
            Can can = this;
            can.propKnownDeviceParameters = can.propKnownDeviceParameters + " " + str2.Trim();
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

    public Can()
      : base(DeviceType.Can)
    {
      this.propCANIdentifiers = 29;
      this.propMAXStationNumber = 32;
      this.propIntervalTimeout = 0;
      this.propControllerNumber = 0;
      this.channel = (byte) 1;
      this.baudRate = 500000;
      this.baseId = 1598;
      this.cycleTime = 10;
      this.messageCount = 10;
      this.ioPort = 900;
      this.interruptNumber = 10;
      this.sourceAddress = 1;
      this.destinationAddress = 2;
    }

    [EditorBrowsable(EditorBrowsableState.Always)]
    [Category("Communication")]
    [Description("Gets or sets the number of CAN identifiers. CAN communication with 29-bit identifiers (extended frames) or 11-bit identifiers (standard frames). If 29-bit CAN identifiers (extended frames) are used, then 11-bit identifiers cannot be sent or received. Every station in the INA2000 network must have the same setting.")]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    [PviKeyWord("/CMODE")]
    [Browsable(true)]
    [DefaultValue(29)]
    public int CANIdentifiers
    {
      get => this.propCANIdentifiers;
      set => this.propCANIdentifiers = value;
    }

    [Browsable(true)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    [DefaultValue(32)]
    [PviKeyWord("/MDA")]
    [Category("Communication")]
    [Description("Gets or sets the highest station number. Number of maximum possible INA2000 stations (= highest station number). Every station in the INA2000 network must have the same setting.")]
    [EditorBrowsable(EditorBrowsableState.Always)]
    public int MAXStationNumber
    {
      get => this.propMAXStationNumber;
      set => this.propMAXStationNumber = value;
    }

    [Browsable(true)]
    [Description("Gets or sets the number of the controller. 2 CAN controllers are available on the LS172 card. The desired controller is selected with the CNO parameter. No value other than 0 (zero) may be specified for the default CAN controller.")]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    [Category("Communication")]
    [PviKeyWord("/CNO")]
    [EditorBrowsable(EditorBrowsableState.Always)]
    [DefaultValue(0)]
    public int ControllerNumber
    {
      get => this.propControllerNumber;
      set => this.propControllerNumber = value;
    }

    [PviKeyWord("/IF")]
    public byte Channel
    {
      get => this.channel;
      set
      {
        this.channel = value;
        this.propInterfaceName = "inacan" + value.ToString();
      }
    }

    [PviKeyWord("/BD")]
    public int BaudRate
    {
      get => this.baudRate;
      set => this.baudRate = value;
    }

    [PviKeyWord("/BI")]
    public int BaseId
    {
      get => this.baseId;
      set => this.baseId = value;
    }

    [PviKeyWord("/CT")]
    public int CycleTime
    {
      get => this.cycleTime;
      set => this.cycleTime = value;
    }

    [PviKeyWord("/MC")]
    public int MessageCount
    {
      get => this.messageCount;
      set => this.messageCount = value;
    }

    [PviKeyWord("/IO")]
    public int IoPort
    {
      get => this.ioPort;
      set => this.ioPort = value;
    }

    [PviKeyWord("/IR")]
    public int InterruptNumber
    {
      get => this.interruptNumber;
      set => this.interruptNumber = value;
    }

    [PviKeyWord("/SA")]
    public int SourceAddress
    {
      get => this.sourceAddress;
      set => this.sourceAddress = value;
    }

    [PviCpuParameter]
    [PviKeyWord("/DA")]
    public int DestinationAddress
    {
      get => this.destinationAddress;
      set => this.destinationAddress = value;
    }

    public override string DeviceParameterString => base.DeviceParameterString + "/CNO=" + this.ControllerNumber.ToString() + " /BI=" + this.BaseId.ToString() + " /IT=" + this.IntervalTimeout.ToString() + " /MDA=" + this.MAXStationNumber.ToString() + " /BD=" + this.BaudRate.ToString() + " /CMODE=" + this.CANIdentifiers.ToString() + " /CT=" + this.CycleTime.ToString() + " /MC=" + this.MessageCount.ToString() + " /SA=" + this.SourceAddress.ToString() + " /IR=" + this.InterruptNumber.ToString() + " /IO=" + this.IoPort.ToString() + " ";

    public override string CpuParameterString => base.CpuParameterString + "/DA=" + this.destinationAddress.ToString() + " ";

    public override string ToString() => this.DeviceParameterString + this.CpuParameterString.Trim();
  }
}
