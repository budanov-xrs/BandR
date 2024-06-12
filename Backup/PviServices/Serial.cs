// Decompiled with JetBrains decompiler
// Type: BR.AN.PviServices.Serial
// Assembly: BR.AN.PVIServices, Version=1.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DA853F0A-C070-4E30-B7A7-51D955FB85F6
// Assembly location: C:\Users\ss_six\Desktop\FXE\FXE software integration package\COMET_FXE-PVIServices_Example\Pluto-PVI-trial_01\packages\IToolS.BandR.3.5.58\lib\net20\BR.AN.PVIServices.dll

using System;
using System.ComponentModel;
using System.Xml;

namespace BR.AN.PviServices
{
  [Serializable]
  public class Serial : DeviceBase
  {
    private FlowControls propFlowControl;
    private byte channel;
    private Parity parity;
    private int baudrate;

    internal override int ToXMLTextWriter(ref XmlTextWriter writer, ConfigurationFlags flags)
    {
      int xmlTextWriter = base.ToXMLTextWriter(ref writer, flags);
      if (this.channel != (byte) 1)
        writer.WriteAttributeString("Channel", this.channel.ToString());
      if (this.baudrate != 57600)
        writer.WriteAttributeString("BaudRate", this.baudrate.ToString());
      if (this.parity != Parity.Even)
        writer.WriteAttributeString("Parity", this.parity.ToString());
      if (this.propFlowControl != FlowControls.NOT_SET)
        writer.WriteAttributeString("FlowControl", this.propFlowControl.ToString());
      return xmlTextWriter;
    }

    public override int FromXmlTextReader(
      ref XmlTextReader reader,
      ConfigurationFlags flags,
      DeviceBase baseObj)
    {
      base.FromXmlTextReader(ref reader, flags, baseObj);
      Serial serial = (Serial) baseObj;
      if (serial == null)
        return -1;
      string str = "";
      string attribute1 = reader.GetAttribute("Channel");
      if (attribute1 != null && attribute1.Length > 0)
      {
        byte result = 0;
        if (PviParse.TryParseByte(attribute1, out result))
          serial.channel = result;
      }
      str = "";
      string attribute2 = reader.GetAttribute("BaudRate");
      if (attribute2 != null && attribute2.Length > 0)
      {
        int result = 0;
        if (PviParse.TryParseInt32(attribute2, out result))
          serial.baudrate = result;
      }
      str = "";
      string attribute3 = reader.GetAttribute("Parity");
      if (attribute3 != null && attribute3.Length > 0)
      {
        switch (attribute3.ToLower())
        {
          case "even":
            this.parity = Parity.Even;
            break;
          case "mark":
            this.parity = Parity.Mark;
            break;
          case "none":
            this.parity = Parity.None;
            break;
          case "odd":
            this.parity = Parity.Odd;
            break;
          case "space":
            this.parity = Parity.Space;
            break;
        }
      }
      str = "";
      string attribute4 = reader.GetAttribute("FlowControl");
      if (attribute4 != null && attribute4.Length > 0)
      {
        switch (attribute4.ToLower())
        {
          case "not_set":
            this.propFlowControl = FlowControls.NOT_SET;
            break;
          case "rs232":
            this.propFlowControl = FlowControls.RS232;
            break;
          case "rs422":
            this.propFlowControl = FlowControls.RS422;
            break;
          case "rts_off":
            this.propFlowControl = FlowControls.RTS_OFF;
            break;
          case "system":
            this.propFlowControl = FlowControls.SYSTEM;
            break;
        }
      }
      return 0;
    }

    [EditorBrowsable(EditorBrowsableState.Never)]
    [Browsable(false)]
    public override void UpdateDeviceParameters(string parameters)
    {
      string[] strArray = parameters.Split(" ".ToCharArray());
      string paraValue = "";
      this.propKnownDeviceParameters = "";
      for (int index = 0; index < strArray.Length; ++index)
      {
        bool flag = false;
        string str1 = (string) strArray.GetValue(index);
        if (str1.ToUpper().StartsWith("/IF=COM"))
        {
          this.propInterfaceName = str1.Substring(4);
          this.channel = System.Convert.ToByte(str1.Substring(7));
          flag = true;
        }
        else if (DeviceBase.UpdateParameterFromString("/BD=", str1, ref this.baudrate))
          flag = true;
        else if (DeviceBase.UpdateParameterFromString("/RS=", str1, ref paraValue))
        {
          this.propFlowControl = FlowControls.NOT_SET;
          if (paraValue.CompareTo("SYSTEM") == 0 || paraValue.CompareTo("-1") == 0)
            this.propFlowControl = FlowControls.SYSTEM;
          else if (paraValue.CompareTo("RTS_OFF") == 0 || paraValue.CompareTo("0") == 0)
            this.propFlowControl = FlowControls.RTS_OFF;
          else if (paraValue.CompareTo("RS232") == 0 || paraValue.CompareTo("232") == 0)
            this.propFlowControl = FlowControls.RS232;
          else if (paraValue.CompareTo("RS422") == 0 || paraValue.CompareTo("422") == 0)
            this.propFlowControl = FlowControls.RS422;
          flag = true;
        }
        else if (DeviceBase.UpdateParameterFromString("/PA=", str1, ref paraValue))
        {
          this.parity = Parity.None;
          if (paraValue.CompareTo("even") == 0 || paraValue.CompareTo("2") == 0)
            this.parity = Parity.Even;
          else if (paraValue.CompareTo("mark") == 0 || paraValue.CompareTo("3") == 0)
            this.parity = Parity.Mark;
          else if (paraValue.CompareTo("odd") == 0 || paraValue.CompareTo("1") == 0)
            this.parity = Parity.Odd;
          else if (paraValue.CompareTo("space") == 0 || paraValue.CompareTo("4") == 0)
            this.parity = Parity.Space;
          flag = true;
        }
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
            Serial serial = this;
            serial.propKnownDeviceParameters = serial.propKnownDeviceParameters + " " + str2.Trim();
          }
        }
      }
    }

    public Serial()
      : base(DeviceType.Serial)
    {
      this.channel = (byte) 1;
      this.baudrate = 57600;
      this.parity = Parity.Even;
      this.propFlowControl = FlowControls.NOT_SET;
      this.propIntervalTimeout = 20;
      this.propInterfaceName = "COM" + this.channel.ToString();
    }

    [PviKeyWord("/IF")]
    public byte Channel
    {
      get => this.channel;
      set
      {
        this.channel = value;
        this.propInterfaceName = "COM" + value.ToString();
      }
    }

    [PviKeyWord("/BD")]
    public int BaudRate
    {
      get => this.baudrate;
      set => this.baudrate = value;
    }

    [PviKeyWord("/PA")]
    public Parity Parity
    {
      get => this.parity;
      set => this.parity = value;
    }

    [PviKeyWord("/RS")]
    public FlowControls FlowControl
    {
      get => this.propFlowControl;
      set => this.propFlowControl = value;
    }

    public override string DeviceParameterString
    {
      get
      {
        string deviceParameterString;
        if (FlowControls.NOT_SET != this.FlowControl)
          deviceParameterString = base.DeviceParameterString + "/BD=" + this.baudrate.ToString() + " /PA=" + ((int) this.parity).ToString() + " /IT=" + this.IntervalTimeout.ToString() + " /RS=" + ((int) this.FlowControl).ToString() + " ";
        else
          deviceParameterString = base.DeviceParameterString + "/BD=" + this.baudrate.ToString() + " /PA=" + ((int) this.parity).ToString() + " /IT=" + this.IntervalTimeout.ToString() + " ";
        return deviceParameterString;
      }
    }

    public override string ToString() => this.DeviceParameterString + this.CpuParameterString.Trim();
  }
}
