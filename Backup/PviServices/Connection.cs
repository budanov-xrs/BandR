// Decompiled with JetBrains decompiler
// Type: BR.AN.PviServices.Connection
// Assembly: BR.AN.PVIServices, Version=1.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DA853F0A-C070-4E30-B7A7-51D955FB85F6
// Assembly location: C:\Users\ss_six\Desktop\FXE\FXE software integration package\COMET_FXE-PVIServices_Example\Pluto-PVI-trial_01\packages\IToolS.BandR.3.5.58\lib\net20\BR.AN.PVIServices.dll

using System;
using System.ComponentModel;
using System.Xml;

namespace BR.AN.PviServices
{
  public class Connection : Base
  {
    private bool propLineNameHasBeenSet;
    private bool propDeviceNameHasBeenSet;
    private bool stationNameHasBeenSet;
    internal bool propLineIsDirty;
    internal bool propDeviceIsDirty;
    internal string propLineDesc;
    internal string propDeviceDesc;
    internal string propConnectionParameter;
    private Cpu propCpu;
    internal PviLINE pviLineObj;
    internal PviDEVICE pviDeviceObj;
    internal PviSTATION pviStationObj;
    private string propDeviceName;
    private string propModuleInfoPath;
    private DeviceType propDeviceType;
    private DeviceBase propDevice;
    internal Serial serial;
    internal Can can;
    internal TcpIp tcpip;
    internal ANSLTcp tcpANSL;
    internal TcpIpMODBUS tcpipModBus;
    internal SimpleNetworkManagementProtocol snmpLine;
    internal Shared shared;
    internal Modem propINAModem;
    internal AR000 arSim;
    private string propOldDeviceName;
    private string propOldStationName;
    private string propOldLineName;

    internal override int ToXMLTextWriter(ref XmlTextWriter writer, ConfigurationFlags flags)
    {
      writer.WriteStartElement(nameof (Connection));
      int xmlTextWriter = this.propDevice.ToXMLTextWriter(ref writer, flags);
      if (this.Routing != null && this.Routing.Length > 0)
        writer.WriteAttributeString("Routing", this.Routing);
      if (this.propDeviceName != null && this.propDeviceName.Length > 0)
        writer.WriteAttributeString("DeviceName", this.propDeviceName);
      if (this.propConnectionParameter != null && this.propConnectionParameter.Length > 0)
        writer.WriteAttributeString("ConnectionParameter", this.propConnectionParameter);
      writer.WriteEndElement();
      return xmlTextWriter;
    }

    internal override void Dispose(bool disposing, bool removeFromCollection)
    {
      if (this.propDisposed)
        return;
      Base propParent = this.propParent;
      if (disposing)
      {
        this.pviLineObj.Connected -= new PviEventHandler(this.pviLineObj_Connected);
        this.pviLineObj.Disconnected -= new PviEventHandler(this.pviLineObj_Disconnected);
        this.pviDeviceObj.Connected -= new PviEventHandler(this.pviDeviceObj_Connected);
        this.pviDeviceObj.Disconnected -= new PviEventHandler(this.pviDeviceObj_Disconnected);
        this.pviStationObj.Connected -= new PviEventHandler(this.pviStationObj_Connected);
        this.pviStationObj.Disconnected -= new PviEventHandler(this.pviStationObj_Disconnected);
        this.pviStationObj.Dispose(disposing, removeFromCollection);
        this.pviDeviceObj.Dispose(disposing, removeFromCollection);
        this.pviLineObj.Dispose(disposing, removeFromCollection);
        this.pviStationObj = (PviSTATION) null;
        this.pviDeviceObj = (PviDEVICE) null;
        this.pviLineObj = (PviLINE) null;
        this.serial = (Serial) null;
        this.can = (Can) null;
        this.tcpip = (TcpIp) null;
        this.tcpipModBus = (TcpIpMODBUS) null;
        this.tcpANSL = (ANSLTcp) null;
        this.shared = (Shared) null;
        this.arSim = (AR000) null;
        this.propINAModem = (Modem) null;
        this.propDeviceType = DeviceType.Serial;
      }
      base.Dispose(disposing, removeFromCollection);
    }

    public override int FromXmlTextReader(
      ref XmlTextReader reader,
      ConfigurationFlags flags,
      Base baseObj)
    {
      string str = "";
      string attribute1 = reader.GetAttribute("DeviceType");
      if (attribute1 != null && attribute1.Length > 0)
      {
        switch (attribute1.ToLower())
        {
          case "ansltcp":
            this.propDevice = (DeviceBase) this.tcpANSL;
            this.DeviceType = DeviceType.ANSLTcp;
            break;
          case "ar010":
          case "arwin":
          case "tcpip":
            this.propDevice = (DeviceBase) this.tcpip;
            this.DeviceType = DeviceType.TcpIp;
            break;
          case "tcpipmodbus":
            this.propDevice = (DeviceBase) this.tcpipModBus;
            this.DeviceType = DeviceType.TcpIpMODBUS;
            break;
          case "shared":
            this.propDevice = (DeviceBase) this.shared;
            this.DeviceType = DeviceType.Shared;
            break;
          case "serial":
            this.propDevice = (DeviceBase) this.serial;
            this.DeviceType = DeviceType.Serial;
            break;
          case "modem":
            this.propDevice = (DeviceBase) this.propINAModem;
            this.DeviceType = DeviceType.Modem;
            break;
          case "can":
            this.propDevice = (DeviceBase) this.can;
            this.DeviceType = DeviceType.Can;
            break;
          case "arsim":
          case "ar000":
            this.propDevice = (DeviceBase) this.arSim;
            this.DeviceType = DeviceType.AR000;
            break;
        }
      }
      str = "";
      string attribute2 = reader.GetAttribute("Routing");
      if (attribute2 != null && attribute2.Length > 0)
        this.Routing = attribute2;
      str = "";
      string attribute3 = reader.GetAttribute("DeviceName");
      if (attribute3 != null && attribute3.Length > 0)
        this.propDeviceName = attribute3;
      str = "";
      string attribute4 = reader.GetAttribute("ConnectionParameter");
      if (attribute4 != null && attribute4.Length > 0)
        this.propConnectionParameter = attribute4;
      this.propDevice.FromXmlTextReader(ref reader, flags, this.propDevice);
      reader.Read();
      return 0;
    }

    public Connection(Service serviceObj)
      : base((Base) serviceObj)
    {
      this.propLineNameHasBeenSet = false;
      this.propDeviceNameHasBeenSet = false;
      this.stationNameHasBeenSet = false;
      this.propOldDeviceName = "";
      this.propOldStationName = "";
      this.propOldLineName = "";
      this.propLineIsDirty = false;
      this.propDeviceIsDirty = false;
      this.propLineDesc = "";
      this.propDeviceDesc = "";
    }

    public Connection(Cpu cpu)
      : base((Base) cpu)
    {
      this.propLineNameHasBeenSet = false;
      this.propDeviceNameHasBeenSet = false;
      this.stationNameHasBeenSet = false;
      this.propOldDeviceName = "";
      this.propOldStationName = "";
      this.propOldLineName = "";
      this.propDeviceIsDirty = false;
      this.propDeviceIsDirty = false;
      this.propLineDesc = "";
      this.propDeviceDesc = "";
      this.propCpu = cpu;
      this.serial = new Serial();
      this.can = new Can();
      this.tcpip = new TcpIp();
      this.tcpipModBus = new TcpIpMODBUS();
      this.shared = new Shared();
      this.arSim = new AR000();
      this.tcpANSL = new ANSLTcp();
      this.propINAModem = new Modem();
      this.propDeviceType = DeviceType.Serial;
      this.propDevice = (DeviceBase) this.serial;
      this.propDeviceName = "COM1";
      this.propConnectionParameter = "";
      this.propModuleInfoPath = "";
      this.pviLineObj = new PviLINE(cpu, "LNINA2");
      this.pviDeviceObj = new PviDEVICE(this.pviLineObj, (string) null);
      this.pviStationObj = new PviSTATION(this.pviDeviceObj, "");
      this.pviLineObj.Connected += new PviEventHandler(this.pviLineObj_Connected);
      this.pviLineObj.ConnectionChanged += new PviEventHandler(this.Line_ConnectionChanged);
      this.pviLineObj.Disconnected += new PviEventHandler(this.pviLineObj_Disconnected);
      this.pviDeviceObj.Connected += new PviEventHandler(this.pviDeviceObj_Connected);
      this.pviDeviceObj.ConnectionChanged += new PviEventHandler(this.Device_ConnectionChanged);
      this.pviDeviceObj.Disconnected += new PviEventHandler(this.pviDeviceObj_Disconnected);
      this.pviStationObj.Connected += new PviEventHandler(this.pviStationObj_Connected);
      this.pviStationObj.Disconnected += new PviEventHandler(this.pviStationObj_Disconnected);
    }

    internal void ResetLinkIds()
    {
      this.pviLineObj.propLinkId = 0U;
      this.pviLineObj.propConnectionState = ConnectionStates.Unininitialized;
      this.pviDeviceObj.propLinkId = 0U;
      this.pviDeviceObj.propConnectionState = ConnectionStates.Unininitialized;
      this.pviStationObj.propLinkId = 0U;
      this.pviStationObj.propConnectionState = ConnectionStates.Unininitialized;
    }

    private void pviStationObj_Disconnected(object sender, PviEventArgs e) => this.pviDeviceObj.PviDisconnect();

    private void pviStationObj_Connected(object sender, PviEventArgs e)
    {
      if (e.ErrorCode != 0)
        this.OnError(new PviEventArgs(this.pviStationObj.Name, this.pviStationObj.Address, e.ErrorCode, this.Service.Language, Action.CpuConnect, this.Service));
      this.OnConnected(new PviEventArgs(this.propName, this.propAddress, e.ErrorCode, this.Service.Language, Action.CpuConnect, this.Service));
    }

    private void pviDeviceObj_Disconnected(object sender, PviEventArgs e) => this.pviLineObj.PviDisconnect();

    private void pviDeviceObj_Connected(object sender, PviEventArgs e)
    {
      if (e.ErrorCode != 0)
      {
        this.OnError(new PviEventArgs(this.pviDeviceObj.Name, this.pviDeviceObj.Address, e.ErrorCode, this.Service.Language, Action.DeviceConnect, this.Service));
        this.OnConnected(new PviEventArgs(this.propName, this.propAddress, e.ErrorCode, this.Service.Language, Action.CpuConnect, this.Service));
      }
      else
      {
        if (!this.Service.WaitForParentConnection)
          return;
        if (this.DeviceType < DeviceType.TcpIpMODBUS)
        {
          if (!this.stationNameHasBeenSet)
            this.pviStationObj.SetName("PLC_" + this.pviDeviceObj.Name);
          int errorCode = this.pviStationObj.PviConnect();
          if (errorCode == 0)
            return;
          this.OnConnected(new PviEventArgs(this.propName, this.propAddress, errorCode, this.Service.Language, Action.StationConnect, this.Service));
        }
        else
          this.OnConnected(new PviEventArgs(this.propName, this.propAddress, e.ErrorCode, this.Service.Language, Action.CpuConnect, this.Service));
      }
    }

    private void pviLineObj_Disconnected(object sender, PviEventArgs e) => this.OnDisconnected(new PviEventArgs(this.propName, this.propAddress, e.ErrorCode, this.Service.Language, Action.CpuDisconnect, this.Service));

    private void pviLineObj_Connected(object sender, PviEventArgs e)
    {
      if (e.ErrorCode != 0)
      {
        this.OnError(new PviEventArgs(this.pviLineObj.Name, this.pviLineObj.Address, e.ErrorCode, this.Service.Language, Action.LineConnect, this.Service));
        this.OnConnected(new PviEventArgs(this.propName, this.propAddress, e.ErrorCode, this.Service.Language, Action.CpuConnect, this.Service));
      }
      else
      {
        if (!this.Service.WaitForParentConnection)
          return;
        int errorCode = this.pviDeviceObj.PviConnect();
        if (errorCode == 0)
          return;
        this.OnConnected(new PviEventArgs(this.propName, this.propAddress, errorCode, this.Service.Language, Action.DeviceConnect, this.Service));
      }
    }

    private void CheckForParamterChanges()
    {
      if (this.pviLineObj.ObjectParam.CompareTo(this.propLineDesc) != 0)
        this.propLineIsDirty = true;
      if (this.pviDeviceObj.ObjectParam.CompareTo(this.propDeviceDesc) != 0)
        this.propDeviceIsDirty = true;
      this.propLineDesc = this.pviLineObj.ObjectParam;
      this.propDeviceDesc = this.pviDeviceObj.ObjectParam;
    }

    public override void Connect()
    {
      int errorCode = 0;
      this.propReturnValue = 0;
      this.UpdateDeviceParameter();
      this.propOldDeviceName = "";
      this.propOldStationName = "";
      this.propOldLineName = "";
      if (!this.propLineNameHasBeenSet)
        this.propOldLineName = this.pviLineObj.Name;
      if (!this.propDeviceNameHasBeenSet)
        this.propOldDeviceName = this.pviDeviceObj.Name;
      if (!this.stationNameHasBeenSet)
        this.propOldStationName = this.pviStationObj.Name;
      if (DeviceType.TcpIpMODBUS == this.DeviceType)
      {
        this.pviStationObj.propName = this.TcpIpMODBUS.DestinationIPAddress;
        this.pviStationObj.propName = this.pviStationObj.propName.Replace('.', '_');
      }
      this.propReturnValue = this.pviLineObj.PviConnect();
      if (this.propReturnValue != 0)
        this.OnConnected(new PviEventArgs(this.propName, this.propAddress, errorCode, this.Service.Language, Action.LineConnect, this.Service));
      if (!this.Service.WaitForParentConnection)
      {
        this.propReturnValue = this.pviDeviceObj.PviConnect();
        if (this.propReturnValue != 0)
          this.OnConnected(new PviEventArgs(this.propName, this.propAddress, errorCode, this.Service.Language, Action.DeviceConnect, this.Service));
        if (this.DeviceType < DeviceType.TcpIpMODBUS)
        {
          if (!this.stationNameHasBeenSet)
            this.pviStationObj.SetName("PLC_" + this.pviDeviceObj.Name);
          this.propReturnValue = this.pviStationObj.PviConnect();
          if (this.propReturnValue != 0)
            this.OnConnected(new PviEventArgs(this.propName, this.propAddress, errorCode, this.Service.Language, Action.StationConnect, this.Service));
        }
      }
      if (this.propReturnValue == 0)
        return;
      this.OnError(new PviEventArgs(this.Name, this.Address, this.propReturnValue, this.Service.Language, Action.CpuConnect, this.Service));
    }

    internal int TurnOffEvents()
    {
      int num = 0;
      num = this.pviLineObj.TurnOffEvents();
      num = this.pviDeviceObj.TurnOffEvents();
      return this.pviStationObj.TurnOffEvents();
    }

    internal int TurnOnEvents()
    {
      int num = 0;
      num = this.pviLineObj.TurnOnEvents();
      num = this.pviDeviceObj.TurnOnEvents();
      return this.pviStationObj.TurnOnEvents();
    }

    public override int ChangeConnection()
    {
      int num1 = 0;
      this.UpdateDeviceParameter();
      num1 = this.ChangeToInvalidLineConnection();
      num1 = this.ChangeDeviceConnection();
      int num2 = this.ChangeLineConnection();
      this.propDeviceIsDirty = false;
      this.OnConnectionChanged(0, Action.DeviceConnect);
      return num2;
    }

    private int ChangeToInvalidLineConnection()
    {
      int invalidLineConnection = 0;
      string str = "\"/LN=\"";
      if (this.propLineIsDirty)
      {
        IntPtr zero = IntPtr.Zero;
        IntPtr hglobal = PviMarshal.StringToHGlobal(str);
        invalidLineConnection = this.Write(this.Service.hPvi, this.pviLineObj.LinkId, AccessTypes.Connect, hglobal, str.Length);
        PviMarshal.FreeHGlobal(ref hglobal);
      }
      return invalidLineConnection;
    }

    private int ChangeLineConnection()
    {
      int num1 = 0;
      IntPtr zero = IntPtr.Zero;
      if (this.propLineIsDirty)
      {
        if (this.Cpu.propConnectionState == ConnectionStates.Unininitialized)
        {
          this.propConnectionState = ConnectionStates.Unininitialized;
          this.Cpu.propConnectionState = ConnectionStates.Unininitialized;
          this.Cpu.propLinkId = 0U;
          this.Cpu.Requests = Actions.NONE;
          this.Cpu.Connect();
        }
        else
        {
          if (ConnectionStates.ConnectedError == this.Cpu.propConnectionState)
          {
            this.Cpu.Requests = Actions.NONE;
            this.Cpu.Requests |= Actions.Connect;
            this.Cpu.Requests |= Actions.GetCpuInfo;
            this.Cpu.Requests |= Actions.GetLBType;
          }
          string str = this.pviLineObj.ObjectParam;
          int num2 = this.pviLineObj.ObjectParam.IndexOf("\"/\"");
          if (-1 != num2)
          {
            str = str.Substring(num2 + 2);
          }
          else
          {
            int num3 = this.pviLineObj.ObjectParam.IndexOf("CD=\"");
            if (-1 != num3)
              str = str.Substring(num3 + 3);
          }
          IntPtr hglobal = PviMarshal.StringToHGlobal(str);
          num1 = this.Write(this.Service.hPvi, this.pviLineObj.LinkId, AccessTypes.Connect, hglobal, str.Length);
          PviMarshal.FreeHGlobal(ref hglobal);
        }
      }
      this.propLineIsDirty = false;
      return num1;
    }

    private int ChangeToInvalidDeviceConnection()
    {
      string str = "\"/IF=invalid \"";
      IntPtr zero = IntPtr.Zero;
      IntPtr hglobal = PviMarshal.StringToHGlobal(str);
      int deviceConnection = this.Write(this.Service.hPvi, this.pviDeviceObj.LinkId, AccessTypes.Connect, hglobal, str.Length);
      PviMarshal.FreeHGlobal(ref hglobal);
      return deviceConnection;
    }

    private int ChangeDeviceConnection()
    {
      int num1 = 0;
      IntPtr zero = IntPtr.Zero;
      if (this.propDeviceIsDirty)
      {
        if (this.Cpu.propConnectionState == ConnectionStates.Unininitialized)
        {
          this.propConnectionState = ConnectionStates.Unininitialized;
          this.Cpu.propConnectionState = ConnectionStates.Unininitialized;
          this.Cpu.propLinkId = 0U;
          this.Cpu.Requests = Actions.NONE;
          this.Cpu.Connect();
        }
        else
        {
          if (ConnectionStates.ConnectedError == this.Cpu.propConnectionState)
          {
            this.Cpu.Requests = Actions.NONE;
            this.Cpu.Requests |= Actions.Connect;
            this.Cpu.Requests |= Actions.GetCpuInfo;
            this.Cpu.Requests |= Actions.GetLBType;
          }
          string str = this.pviDeviceObj.ObjectParam;
          int num2 = this.pviDeviceObj.ObjectParam.IndexOf("\"/\"");
          if (-1 != num2)
          {
            str = str.Substring(num2 + 2);
          }
          else
          {
            int num3 = this.pviDeviceObj.ObjectParam.IndexOf("CD=\"");
            if (-1 != num3)
              str = str.Substring(num3 + 3);
          }
          IntPtr hglobal = PviMarshal.StringToHGlobal(str);
          num1 = this.Write(this.Service.hPvi, this.pviDeviceObj.LinkId, AccessTypes.Connect, hglobal, str.Length);
          PviMarshal.FreeHGlobal(ref hglobal);
        }
      }
      this.propDeviceIsDirty = false;
      return num1;
    }

    private void Line_ConnectionChanged(object sender, PviEventArgs e)
    {
      if (e.ErrorCode != 0)
        this.OnError(e);
      if (ConnectionStates.ConnectionChanging == this.propConnectionState)
      {
        this.propConnectionState = ConnectionStates.Connecting;
        this.OnConnectionChanged(e.ErrorCode, e.Action);
      }
      else
      {
        this.propConnectionState = ConnectionStates.ConnectionChanging;
        int errorCode = this.ChangeDeviceConnection();
        if (errorCode == 0)
          return;
        e.SetErrorCode(errorCode);
        this.OnError(sender, e);
      }
    }

    private void Device_ConnectionChanged(object sender, PviEventArgs e) => this.OnConnectionChanged(e.ErrorCode, e.Action);

    internal void DisconnectNoResponses()
    {
      this.propOldDeviceName = "";
      this.propOldStationName = "";
      this.propOldLineName = "";
      this.pviStationObj.Disconnect(true);
      this.pviDeviceObj.Disconnect(true);
      this.pviLineObj.Disconnect(true);
      this.propConnectionState = ConnectionStates.Disconnected;
    }

    public override void Disconnect()
    {
      this.propOldDeviceName = "";
      this.propOldStationName = "";
      this.propOldLineName = "";
      if (ConnectionStates.Connected < this.propConnectionState)
        return;
      this.propConnectionState = ConnectionStates.Disconnecting;
      this.propReturnValue = this.pviStationObj.PviDisconnect();
    }

    private void UpdateDeviceObjectName(string newDevObjName) => this.pviDeviceObj.propName = newDevObjName;

    public void SynchronizeCommunicationParameters(Connection.CommSyncDirections syncDirection)
    {
      if (this.Cpu == null)
        return;
      if (syncDirection == Connection.CommSyncDirections.ConnectionToCpu)
      {
        this.Cpu.SavePath = this.Device.SavePath;
      }
      else
      {
        this.Device.ResponseTimeout = this.Cpu.ResponseTimeout;
        this.Device.SavePath = this.Cpu.SavePath;
      }
    }

    private string getLineName() => !this.propLineNameHasBeenSet && 0 < this.propOldLineName.Length ? this.propOldLineName : this.pviLineObj.Name;

    internal void UpdateDeviceParameter()
    {
      string str1 = "";
      string str2 = "";
      string str3 = "";
      string str4 = "";
      string str5 = "";
      string str6 = "";
      string str7 = "";
      string str8 = "";
      string str9 = "";
      string str10 = "";
      string str11 = "";
      string str12 = "";
      string str13 = "";
      string str14 = "";
      string str15 = "";
      switch (this.DeviceType)
      {
        case DeviceType.Serial:
          if (!this.propDeviceNameHasBeenSet)
            this.propDeviceName = string.Format("COM{0}", (object) this.Serial.Channel.ToString());
          Parity parity = this.Serial.Parity;
          if (this.ResponseTimeout > 0)
            str1 = string.Format(" /RT={0}", (object) this.ResponseTimeout);
          if (this.serial.IntervalTimeout > 0 && 20 != this.serial.IntervalTimeout)
            str2 = string.Format(" /IT={0}", (object) this.serial.IntervalTimeout);
          if (FlowControls.NOT_SET != this.Serial.FlowControl)
            str10 = string.Format(" /RS={0}", (object) (int) this.Serial.FlowControl);
          if (0 < this.Device.InterfaceName.Length)
            this.pviDeviceObj.Inititialize(this.getLineName(), string.Format("\"/IF={0} /BD={1} /PA={2}{3}{4} {5}\"", (object) this.Device.InterfaceName, (object) this.Serial.BaudRate.ToString(), (object) (int) parity, (object) str2, (object) str10, (object) this.Device.UnknownDeviceParameters));
          else
            this.pviDeviceObj.Inititialize(this.getLineName(), string.Format("\"/IF=COM{0} /BD={1} /PA={2}{3}{4} {5}\"", (object) this.Serial.Channel.ToString(), (object) this.Serial.BaudRate.ToString(), (object) (int) parity, (object) str2, (object) str10, (object) this.Device.UnknownDeviceParameters));
          if (this.Routing != null && this.Routing.Length > 0)
            str3 = string.Format(" /CN={0}", (object) this.Routing);
          if (this.ModuleInfoPath != null && this.ModuleInfoPath.Length > 0)
            str4 = string.Format(" /SP={0}", (object) this.ModuleInfoPath);
          this.propConnectionParameter = "";
          if (this.Routing != null && this.Routing.Length > 0 || this.ModuleInfoPath != null && this.ModuleInfoPath.Length > 0 || this.ResponseTimeout > 0)
          {
            this.propConnectionParameter = string.Format("{0}{1}{2}", (object) str3, (object) str4, (object) str1);
            break;
          }
          break;
        case DeviceType.TcpIp:
          if (!this.propDeviceNameHasBeenSet)
            this.propDeviceName = "TCPIP";
          if (this.tcpip.SourcePort > (short) 0)
            str5 = string.Format(" /LOPO={0}", (object) this.TcpIp.SourcePort);
          if (this.tcpip.LocalPort > 0U)
            str5 = string.Format(" /LOPO={0}", (object) this.TcpIp.LocalPort);
          if (this.tcpip.SourceStation > (byte) 0)
            str6 = string.Format(" /SA={0}", (object) this.tcpip.SourceStation);
          if (this.tcpip.UniqueDeviceForSAandLOPO)
            str15 = " /UDEV=1";
          if (this.Device.InterfaceName != null && 0 < this.Device.InterfaceName.Length)
            this.pviDeviceObj.Inititialize(this.getLineName(), string.Format("\"/IF={0}{1}{2}{3}{4}\"", (object) this.Device.InterfaceName, (object) str6, (object) str5, (object) str15, (object) this.Device.UnknownDeviceParameters));
          else
            this.pviDeviceObj.Inititialize(this.getLineName(), string.Format("\"/IF=tcpip{0}{1}{2}{3}\"", (object) str6, (object) str5, (object) str15, (object) this.Device.UnknownDeviceParameters));
          if (this.Routing != null && this.Routing.Length > 0)
            str3 = string.Format(" /CN={0}", (object) this.Routing);
          if (this.ModuleInfoPath != null && this.ModuleInfoPath.Length > 0)
            str4 = string.Format(" /SP={0}", (object) this.ModuleInfoPath);
          string str16 = this.tcpip.DestinationIpAddress == null || this.tcpip.DestinationIpAddress.Length <= 0 ? string.Format(" /CKDA={0}", (object) this.tcpip.CheckDestinationStation) : string.Format(" /DAIP={0}", (object) this.tcpip.DestinationIpAddress);
          if (this.tcpip.Target != null && this.tcpip.Target.Length > 0)
            str7 = string.Format(" /TA={0}", (object) this.tcpip.Target);
          if (this.tcpip.ResponseTimeout > 0)
            str1 = string.Format(" /RT={0}", (object) this.tcpip.ResponseTimeout);
          if (this.tcpip.DestinationPort > (short) 0)
            str8 = string.Format(" /REPO={0}", (object) this.tcpip.DestinationPort);
          if (this.tcpip.RemotePort > 0U)
            str8 = string.Format(" /REPO={0}", (object) this.tcpip.RemotePort);
          if (this.tcpip.DestinationStation > (byte) 0)
            str9 = string.Format("/DA={0}", (object) this.tcpip.DestinationStation);
          if (this.tcpip.QuickDownload != 1)
            str14 = string.Format(" /ANSL={0}", (object) this.tcpip.QuickDownload);
          this.propConnectionParameter = string.Format("{0}{1}{2}{3}{4}{5}{6}{7}", (object) str9, (object) str3, (object) str4, (object) str16, (object) str7, (object) str1, (object) str8, (object) str14);
          this.propConnectionParameter = this.propConnectionParameter.Trim();
          break;
        case DeviceType.Can:
          if (!this.propDeviceNameHasBeenSet)
            this.propDeviceName = "INACAN";
          if (this.can.IntervalTimeout > 0)
            str2 = string.Format(" /IT={0}", (object) this.can.IntervalTimeout);
          if (0 < this.Device.InterfaceName.Length)
            this.pviDeviceObj.Inititialize(this.getLineName(), string.Format("\"/IF={0} /CNO={1} /BD={2} /BI={3} /CT={4} /MC={5} /IO={6} /IR={7} /SA={8}{9} {10}\"", (object) this.Device.InterfaceName, (object) this.can.ControllerNumber, (object) this.can.BaudRate, (object) this.can.BaseId, (object) this.can.CycleTime, (object) this.can.MessageCount, (object) this.can.IoPort, (object) this.can.InterruptNumber, (object) this.can.SourceAddress, (object) str2, (object) this.Device.UnknownDeviceParameters));
          else
            this.pviDeviceObj.Inititialize(this.getLineName(), string.Format("\"/IF=INACAN{0} /CNO={1} /BD={2} /BI={3} /CT={4} /MC={5} /IO={6} /IR={7} /SA={8}{9} {10}\"", (object) this.can.Channel, (object) this.can.ControllerNumber, (object) this.can.BaudRate, (object) this.can.BaseId, (object) this.can.CycleTime, (object) this.can.MessageCount, (object) this.can.IoPort, (object) this.can.InterruptNumber, (object) this.can.SourceAddress, (object) str2, (object) this.Device.UnknownDeviceParameters));
          if (this.Routing != null && this.Routing.Length > 0)
            str3 = string.Format(" /CN={0}", (object) this.Routing);
          if (this.ModuleInfoPath != null && this.ModuleInfoPath.Length > 0)
            str4 = string.Format(" /SP={0}", (object) this.ModuleInfoPath);
          if (this.can.ResponseTimeout > 0)
            str1 = string.Format(" /RT={0}", (object) this.can.ResponseTimeout);
          this.propConnectionParameter = string.Format("/DA={0}{1}{2}{3}", (object) this.can.DestinationAddress, (object) str3, (object) str4, (object) str1);
          break;
        case DeviceType.Shared:
          if (!this.propDeviceNameHasBeenSet)
            this.propDeviceName = string.Format("LS251_{0}", (object) this.shared.Channel.ToString());
          if (0 < this.Device.InterfaceName.Length)
          {
            this.pviDeviceObj.Inititialize(this.getLineName(), string.Format("\"/IF={0} {1}\"", (object) this.Device.InterfaceName, (object) this.Device.UnknownDeviceParameters));
            break;
          }
          this.pviDeviceObj.Inititialize(this.getLineName(), string.Format("\"/IF=LS251_{0} {1}\"", (object) this.shared.Channel.ToString(), (object) this.Device.UnknownDeviceParameters));
          break;
        case DeviceType.Modem:
          if (!this.propDeviceNameHasBeenSet)
            this.propDeviceName = "modem" + this.propINAModem.CommunicationPort.ToString();
          if (this.ResponseTimeout > 0)
            str1 = string.Format(" /RT={0}", (object) this.ResponseTimeout);
          if (0 < this.Device.InterfaceName.Length)
          {
            if (-1 == this.Modem.Redial)
              this.pviDeviceObj.Inititialize(this.getLineName(), string.Format("\"/IF={0} /MO={1} /TN={2} /MR={3} /RI={4} /IT={5} {6}\"", (object) this.Device.InterfaceName, (object) this.Modem.Modem, (object) this.Modem.PhoneNumber, (object) "INFINITE", (object) this.Modem.RedialTimeout.ToString(), (object) this.Modem.IntervalTimeout.ToString(), (object) this.Device.UnknownDeviceParameters));
            else
              this.pviDeviceObj.Inititialize(this.getLineName(), string.Format("\"/IF={0} /MO={1} /TN={2} /MR={3} /RI={4} /IT={5} {6}\"", (object) this.Device.InterfaceName, (object) this.Modem.Modem, (object) this.Modem.PhoneNumber, (object) this.Modem.Redial.ToString(), (object) this.Modem.RedialTimeout.ToString(), (object) this.Modem.IntervalTimeout.ToString(), (object) this.Device.UnknownDeviceParameters));
          }
          else if (-1 == this.Modem.Redial)
            this.pviDeviceObj.Inititialize(this.getLineName(), string.Format("\"/IF=modem{0} /MO={1} /TN={2} /MR={3} /RI={4} /IT={5} {6}\"", (object) this.Modem.CommunicationPort, (object) this.Modem.Modem, (object) this.Modem.PhoneNumber, (object) "INFINITE", (object) this.Modem.RedialTimeout.ToString(), (object) this.Modem.IntervalTimeout.ToString(), (object) this.Device.UnknownDeviceParameters));
          else
            this.pviDeviceObj.Inititialize(this.getLineName(), string.Format("\"/IF=modem{0} /MO={1} /TN={2} /MR={3} /RI={4} /IT={5} {6}\"", (object) this.Modem.CommunicationPort, (object) this.Modem.Modem, (object) this.Modem.PhoneNumber, (object) this.Modem.Redial.ToString(), (object) this.Modem.RedialTimeout.ToString(), (object) this.Modem.IntervalTimeout.ToString(), (object) this.Device.UnknownDeviceParameters));
          if (this.Routing != null && this.Routing.Length > 0)
            str3 = string.Format(" /CN={0}", (object) this.Routing);
          if (this.ModuleInfoPath != null && this.ModuleInfoPath.Length > 0)
            str4 = string.Format(" /SP={0}", (object) this.ModuleInfoPath);
          if (this.Routing != null && this.Routing.Length > 0 || this.ModuleInfoPath != null && this.ModuleInfoPath.Length > 0 || this.ResponseTimeout > 0)
          {
            this.propConnectionParameter = string.Format("{0}{1}{2}", (object) str3, (object) str4, (object) str1);
            break;
          }
          break;
        case DeviceType.AR000:
          if (!this.propDeviceNameHasBeenSet)
            this.propDeviceName = string.Format("SPWIN");
          if (this.arSim.ResponseTimeout > 0)
            str1 = string.Format(" /RT={0}", (object) this.arSim.ResponseTimeout);
          if (0 < this.Device.InterfaceName.Length)
            this.pviDeviceObj.Inititialize(this.getLineName(), string.Format("\"/IF={0} /SA={1} {2}\"", (object) this.Device.InterfaceName, (object) this.arSim.SourceAddress, (object) this.Device.UnknownDeviceParameters));
          else
            this.pviDeviceObj.Inititialize(this.getLineName(), string.Format("\"/IF=SPWIN /SA={0} {1}\"", (object) this.arSim.SourceAddress, (object) this.Device.UnknownDeviceParameters));
          this.propConnectionParameter = string.Format("/DA={0}{1}", (object) this.arSim.DestinationAddress, (object) str1);
          break;
        case DeviceType.ANSLTcp:
          if (!this.propDeviceNameHasBeenSet)
            this.propDeviceName = string.Format("TCPANSL");
          if (this.ResponseTimeout > 0)
            str1 = string.Format(" /RT={0}", (object) this.ResponseTimeout);
          string str17;
          string str18;
          if (0U < this.tcpANSL.CommunicationBufferSize)
          {
            str17 = string.Format("/BSIZE={0}", (object) this.tcpANSL.CommunicationBufferSize);
            str18 = string.Format(" /COMT={0}", (object) this.tcpANSL.CommunicationTimeout);
          }
          else
          {
            str17 = "";
            str18 = string.Format("/COMT={0}", (object) this.tcpANSL.CommunicationTimeout);
          }
          string str19 = "";
          if (0U < this.tcpANSL.SendDelay)
            str19 = string.Format(" /SDT={0}", (object) this.tcpANSL.SendDelay);
          string str20 = string.Format(" /IP={0}", (object) this.tcpANSL.DestinationIpAddress);
          string str21 = string.Format(" /PT={0}", (object) this.tcpANSL.RemotePort);
          if (0 < this.Device.InterfaceName.Length)
            this.pviDeviceObj.Inititialize(this.getLineName(), string.Format("\"/IF={0} {1}\"", (object) this.Device.InterfaceName, (object) this.Device.UnknownDeviceParameters));
          else
            this.pviDeviceObj.Inititialize(this.getLineName(), string.Format("\"/IF=tcpip {0}\"", (object) this.Device.UnknownDeviceParameters));
          if (this.Routing != null && this.Routing.Length > 0)
            str3 = string.Format(" /CN={0}", (object) this.Routing);
          if (this.ModuleInfoPath != null && this.ModuleInfoPath.Length > 0)
            str4 = string.Format(" /SP={0}", (object) this.ModuleInfoPath);
          this.propConnectionParameter = "";
          if (this.Routing != null && this.Routing.Length > 0 || this.ModuleInfoPath != null && this.ModuleInfoPath.Length > 0 || this.ResponseTimeout > 0)
          {
            this.propConnectionParameter = string.Format("{0}{1}{2}{3}{4}{5}{6}{7}", (object) str17, (object) str18, (object) str19, (object) str20, (object) str21, (object) str3, (object) str4, (object) str1);
            break;
          }
          this.propConnectionParameter = string.Format("{0}{1}{2}{3}{4}", (object) str17, (object) str18, (object) str19, (object) str20, (object) str21);
          break;
        case DeviceType.TcpIpMODBUS:
          if (!this.propDeviceNameHasBeenSet)
            this.propDeviceName = "MBUSTCP";
          this.pviDeviceObj.Inititialize(this.getLineName(), string.Format("\"/IF=MBUSTCP {0}\"", (object) this.Device.UnknownDeviceParameters));
          if (this.tcpipModBus.DestinationIPAddress != null && this.tcpipModBus.DestinationIPAddress.Length > 0)
          {
            string str22 = string.Format("/DAIP={0}", (object) this.tcpipModBus.DestinationIPAddress);
            if (this.tcpipModBus.PortNumber != 502)
              str11 = string.Format(" /PN={0}", (object) this.tcpipModBus.PortNumber);
            if (this.tcpipModBus.UnitID != (int) byte.MaxValue)
              str12 = string.Format(" /DA={0}", (object) this.tcpipModBus.UnitID);
            if (this.tcpipModBus.ConnectionRetries > 0)
              str13 = string.Format(" /CR={0}", (object) this.tcpipModBus.ConnectionRetries);
            this.propConnectionParameter = string.Format("{0}{1}{2}", (object) str22, (object) str11, (object) str12, (object) str13);
            break;
          }
          this.propConnectionParameter = "";
          break;
      }
      if (0 < this.Device.SavePath.Length)
      {
        Connection connection = this;
        connection.propConnectionParameter = connection.propConnectionParameter + " /SP=" + this.Device.SavePath + " " + this.Device.UnknownCpuParameters;
      }
      else if (0 < this.Device.UnknownCpuParameters.Length)
      {
        Connection connection = this;
        connection.propConnectionParameter = connection.propConnectionParameter + " " + this.Device.UnknownCpuParameters;
      }
      this.UpdateDeviceObjectName(this.propDeviceName);
      if (this.DeviceType < DeviceType.TcpIpMODBUS)
        this.pviStationObj.Initialize(this.pviDeviceObj.Name, "");
      else
        this.pviStationObj.Initialize(this.pviDeviceObj.Name, string.Format("\"{0}\"", (object) this.ConnectionParameter));
      this.CheckForParamterChanges();
    }

    public Cpu Cpu => this.propCpu;

    public override Service Service => this.propCpu == null ? base.Service : this.propCpu.Service;

    protected internal virtual string DeviceParameter
    {
      get => this.pviDeviceObj.ObjectParam;
      set
      {
        this.ParseDeviceParameters(value);
        this.UpdateDeviceParameter();
      }
    }

    private void ParseDeviceParameters(string parameters)
    {
      int num = parameters.IndexOf("/IF=");
      if (-1 == num)
        return;
      string str = parameters.Substring(num + 4, 3);
      if (str.ToUpper().StartsWith("COM"))
        this.DeviceType = DeviceType.Serial;
      else if (str.ToUpper().Equals("TCP"))
      {
        if (this.DeviceType != DeviceType.TcpIp && this.DeviceType != DeviceType.ANSLTcp)
          this.DeviceType = DeviceType.TcpIp;
      }
      else
        this.DeviceType = !str.ToUpper().Equals("ANS") ? (!str.ToUpper().Equals("CAN") ? (!str.ToUpper().Equals("INA") ? (!str.ToUpper().Equals("MOD") ? (!str.ToUpper().Equals("LS2") ? DeviceType.Serial : DeviceType.Shared) : DeviceType.Modem) : DeviceType.Can) : DeviceType.Can) : DeviceType.ANSLTcp;
      this.propDevice.UpdateDeviceParameters(parameters);
    }

    [Browsable(false)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public void SetLINEName(string name) => this.LineName = name;

    protected internal virtual string LineName
    {
      get => this.pviLineObj.Name;
      set
      {
        this.propLineNameHasBeenSet = true;
        this.pviLineObj.propName = value;
      }
    }

    protected internal virtual string DeviceName
    {
      get => this.pviDeviceObj.Name;
      set
      {
        this.propDeviceNameHasBeenSet = true;
        this.pviDeviceObj.propName = value;
        this.propDeviceName = value;
      }
    }

    [Browsable(false)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public void SetDEVICEName(string name) => this.DeviceName = name;

    protected internal virtual string StationName
    {
      get => this.pviStationObj.Name;
      set
      {
        this.stationNameHasBeenSet = true;
        this.pviStationObj.SetName(value);
      }
    }

    [Browsable(false)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public void SetSTATIONName(string name) => this.StationName = name;

    protected internal virtual string ConnectionParameter
    {
      get => this.propConnectionParameter;
      set
      {
        this.ParseConnectionParameters(value);
        this.UpdateDeviceParameter();
      }
    }

    private void ParseConnectionParameters(string parameters)
    {
      int num = parameters.IndexOf("/IF=");
      if (-1 != num)
      {
        string str = parameters.Substring(num + 4, 3);
        this.DeviceType = str.ToLower().CompareTo("tcp") != 0 ? (str.ToLower().CompareTo("ansl") != 0 ? (str.ToLower().CompareTo("can") != 0 ? (str.ToLower().CompareTo("mod") != 0 ? DeviceType.Serial : DeviceType.Modem) : DeviceType.Can) : DeviceType.ANSLTcp) : DeviceType.TcpIp;
      }
      this.propDevice.UpdateCpuParameters(parameters);
    }

    private void ResetDevices(DeviceType type)
    {
      this.propDeviceType = type;
      this.propINAModem = (Modem) null;
      this.can = (Can) null;
      this.serial = (Serial) null;
      this.tcpip = (TcpIp) null;
      this.tcpipModBus = (TcpIpMODBUS) null;
      this.snmpLine = (SimpleNetworkManagementProtocol) null;
      this.arSim = (AR000) null;
    }

    public DeviceBase Device => this.propDevice;

    public DeviceType DeviceType
    {
      get => this.propDeviceType;
      set
      {
        string name = this.pviLineObj.Name;
        switch (value)
        {
          case DeviceType.ANSLTcp:
            this.pviLineObj.Initialize(2);
            break;
          case DeviceType.TcpIpMODBUS:
            this.pviLineObj.Initialize(1);
            break;
          default:
            this.pviLineObj.Initialize(0);
            break;
        }
        this.propConnectionParameter = "";
        this.propDeviceType = value;
        switch (this.DeviceType)
        {
          case DeviceType.TcpIp:
            this.propDevice = (DeviceBase) this.tcpip;
            if (!this.propDeviceNameHasBeenSet)
            {
              this.propDeviceName = "TcpIp";
              break;
            }
            break;
          case DeviceType.Can:
            if (!this.propDeviceNameHasBeenSet)
              this.propDeviceName = "Can";
            this.propDevice = (DeviceBase) this.can;
            break;
          case DeviceType.Shared:
            if (!this.propDeviceNameHasBeenSet)
              this.propDeviceName = "Shared";
            this.propDevice = (DeviceBase) this.shared;
            break;
          case DeviceType.Modem:
            if (!this.propDeviceNameHasBeenSet)
              this.propDeviceName = "Modem";
            this.propDevice = (DeviceBase) this.propINAModem;
            break;
          case DeviceType.ANSLTcp:
            this.propDevice = (DeviceBase) this.tcpANSL;
            if (!this.propDeviceNameHasBeenSet)
            {
              this.propDeviceName = "ANSL";
              break;
            }
            break;
          case DeviceType.TcpIpMODBUS:
            if (!this.propDeviceNameHasBeenSet)
              this.propDeviceName = "TcpIpMODBUS";
            this.pviLineObj.Initialize(1);
            this.propDevice = (DeviceBase) this.tcpipModBus;
            break;
          default:
            if (!this.propDeviceNameHasBeenSet)
              this.propDeviceName = "Com1";
            this.propDevice = (DeviceBase) this.serial;
            break;
        }
        if (this.stationNameHasBeenSet)
          this.pviLineObj.propName = name;
        this.propDevice.Init();
      }
    }

    public Serial Serial
    {
      get => this.serial;
      set
      {
        this.ResetDevices(DeviceType.Serial);
        this.propDevice = (DeviceBase) (this.serial = value);
      }
    }

    public Modem Modem
    {
      get => this.propINAModem;
      set
      {
        this.ResetDevices(DeviceType.Modem);
        this.propDevice = (DeviceBase) (this.propINAModem = value);
      }
    }

    public Can Can
    {
      get => this.can;
      set
      {
        this.ResetDevices(DeviceType.Can);
        this.propDevice = (DeviceBase) (this.can = value);
      }
    }

    public AR000 AR000
    {
      get => this.arSim;
      set
      {
        this.ResetDevices(DeviceType.AR000);
        this.propDevice = (DeviceBase) (this.arSim = value);
      }
    }

    public TcpIp TcpIp
    {
      get => this.tcpip;
      set
      {
        this.ResetDevices(DeviceType.TcpIp);
        this.propDevice = (DeviceBase) (this.tcpip = value);
      }
    }

    public ANSLTcp ANSLTcp
    {
      get => this.tcpANSL;
      set
      {
        this.ResetDevices(DeviceType.ANSLTcp);
        this.propDevice = (DeviceBase) (this.tcpANSL = value);
      }
    }

    public Shared Shared
    {
      get => this.shared;
      set
      {
        this.ResetDevices(DeviceType.Shared);
        this.propDevice = (DeviceBase) (this.shared = value);
      }
    }

    public TcpIpMODBUS TcpIpMODBUS
    {
      get => this.tcpipModBus;
      set
      {
        this.ResetDevices(DeviceType.TcpIpMODBUS);
        this.propDevice = (DeviceBase) (this.tcpipModBus = value);
      }
    }

    public string ModuleInfoPath
    {
      get => this.propModuleInfoPath;
      set => this.propModuleInfoPath = value;
    }

    public int ResponseTimeout
    {
      get => this.propDevice.ResponseTimeout;
      set => this.propDevice.ResponseTimeout = value;
    }

    public string Routing
    {
      get => this.propDevice.RoutingPath;
      set => this.propDevice.RoutingPath = value;
    }

    public override string FullName => this.Name.Length > 0 ? (this.Parent != null ? this.Parent.FullName + "." + this.Name : this.Name) : (this.Parent != null ? this.Parent.FullName : "");

    public override string PviPathName
    {
      get
      {
        if (this.Name != null && 0 < this.Name.Length)
          return this.pviStationObj.PviPathName + this.Name;
        if (this.pviStationObj == null)
          return "";
        if (0 >= this.propOldLineName.Length && 0 >= this.propOldStationName.Length && 0 >= this.propOldDeviceName.Length)
          return this.pviStationObj.PviPathName;
        string str1 = this.pviLineObj.PviPathName;
        if (0 < this.propOldLineName.Length)
          str1 = this.pviLineObj.Parent.PviPathName + "/" + this.propOldLineName;
        string str2 = 0 >= this.propOldDeviceName.Length ? str1 + "/" + this.pviDeviceObj.Name : str1 + "/" + this.propOldDeviceName;
        return 0 >= this.propOldStationName.Length ? str2 + "/" + this.pviStationObj.Name : str2 + "/" + this.propOldStationName;
      }
    }

    public override string ToString()
    {
      string str;
      switch (this.DeviceType)
      {
        case DeviceType.TcpIp:
          str = this.tcpip.ToString();
          break;
        case DeviceType.Can:
          str = this.can.ToString();
          break;
        case DeviceType.Modem:
          str = this.Modem.ToString();
          break;
        case DeviceType.ANSLTcp:
          str = this.tcpANSL.ToString();
          break;
        case DeviceType.TcpIpMODBUS:
          str = this.tcpipModBus.ToString();
          break;
        default:
          str = this.serial.ToString();
          break;
      }
      return str;
    }

    public enum CommSyncDirections
    {
      ConnectionToCpu,
      FromCpuToConnection,
    }
  }
}
