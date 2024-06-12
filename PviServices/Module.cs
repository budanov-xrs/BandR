// Decompiled with JetBrains decompiler
// Type: BR.AN.PviServices.Module
// Assembly: BR.AN.PVIServices, Version=1.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DA853F0A-C070-4E30-B7A7-51D955FB85F6
// Assembly location: C:\Users\ss_six\Desktop\FXE\FXE software integration package\COMET_FXE-PVIServices_Example\Pluto-PVI-trial_01\packages\IToolS.BandR.3.5.58\lib\net20\BR.AN.PVIServices.dll

using System;
using System.Collections;
using System.ComponentModel;
using System.IO;
using System.Runtime.InteropServices;
using System.Xml;

namespace BR.AN.PviServices
{
  public class Module : Base
  {
    protected bool propModuleInfoRequested;
    private uint propModUID;
    private byte propVersion;
    private byte propRevision;
    private byte propPICount;
    private byte propInstallNumber;
    private byte propInstallationPriority;
    private bool propInstPriorityValid;
    private ushort propDMIndex;
    private ushort propPIIndex;
    private ushort propPVIndex;
    private ushort propPVCount;
    private uint propCreateTime;
    private uint propAndTime;
    private uint propStartAddress;
    private uint propLength;
    private uint propAnalogMemoryAddress;
    private uint propDigitalMemoryAddess;
    private string propCreateName;
    private string propAndName;
    private Actions propLastAction;
    private MemoryType propMemoryLocation;
    private ModuleType propType;
    private TaskClassType propTaskClass;
    private DomainState propDMState;
    protected ProgramState propPIState;
    internal Cpu propCpu;
    internal string propFileName;
    internal ConnectionType propMODULEState;
    internal Hashtable propUserCollections;
    internal bool propStartOrStopRequest;
    private PviObjectBrowser propObjectBrowser;

    internal static bool isTaskObject(ModuleType type) => type == ModuleType.PlcTask || type == ModuleType.TimerTask || type == ModuleType.ExceptionTask;

    internal static bool isLoggerObject(ModuleType type) => type == ModuleType.Logger;

    internal Module(Cpu cpu, string name, bool doNotAddToCollections)
      : base((Base) cpu, name, doNotAddToCollections)
    {
      this.propObjectBrowser = (PviObjectBrowser) null;
      this.Init(cpu, name);
    }

    public Module(Cpu cpu, string name)
      : base((Base) cpu, name)
    {
      if (cpu != null && !(this is Logger) && !(this is Task) && cpu.Modules[name] != null)
        throw new ArgumentException("There is already an object in \"" + cpu.Name + ".Modules\" which has the same name! Use a different name and the same address or use the object from \"" + cpu.Name + ".Modules\".", name);
      this.propObjectBrowser = (PviObjectBrowser) null;
      this.Init(cpu, name);
      cpu?.Modules.Add(this);
    }

    public Module(Cpu cpu, string name, ref XmlTextReader reader, ConfigurationFlags flags)
      : base((Base) cpu, name)
    {
      this.propObjectBrowser = (PviObjectBrowser) null;
      this.Init(cpu, name);
      this.FromXmlTextReader(ref reader, flags, (Base) this);
      cpu.Modules.Add(this);
    }

    internal Module(Cpu cpu, string name, ModuleCollection collection)
      : base((Base) cpu, name)
    {
      this.propObjectBrowser = (PviObjectBrowser) null;
      this.Init(cpu, name);
      collection.Add(this);
    }

    internal Module(object parent, string name)
      : base((Base) parent, name)
    {
      this.propObjectBrowser = (PviObjectBrowser) null;
      this.Init((Cpu) null, name);
      this.propModUID = 0U;
      if (parent == null)
        return;
      this.propModUID = ((Base) parent).Service.ModuleUID;
    }

    internal Module(Cpu cpu, PviObjectBrowser objBrowser, string name)
      : base((Base) cpu, name)
    {
      this.propObjectBrowser = objBrowser;
      this.Init(cpu, name);
    }

    private void Init(Cpu cpu, string name)
    {
      this.propModuleInfoRequested = false;
      this.propType = ModuleType.Unknown;
      switch (this)
      {
        case Task _:
          this.propType = ModuleType.PlcTask;
          break;
        case ErrorLogBook _:
          this.propType = ModuleType.Logger;
          break;
      }
      this.propCpu = cpu;
      this.propParent = (Base) cpu;
      this.propMODULEState = ConnectionType.None;
      if (this.Service != null)
        this.propModUID = this.Service.ModuleUID;
      this.propDMIndex = (ushort) 0;
      this.propPIIndex = (ushort) 0;
      this.propDMState = DomainState.NonExistent;
      this.propPIState = ProgramState.NonExistent;
      this.propPICount = (byte) 0;
      this.propStartAddress = 0U;
      this.propLength = 0U;
      this.propAddress = "";
      this.propName = name;
      this.propVersion = (byte) 0;
      this.propRevision = (byte) 0;
      this.propCreateTime = 0U;
      this.propAndTime = 0U;
      this.propCreateName = "";
      this.propAndName = "";
      this.propTaskClass = TaskClassType.NotValid;
      this.propInstallNumber = (byte) 0;
      this.propInstallationPriority = (byte) 128;
      this.propInstPriorityValid = false;
      this.propPVIndex = (ushort) 0;
      this.propPVCount = (ushort) 0;
      this.propAnalogMemoryAddress = 0U;
      this.propDigitalMemoryAddess = 0U;
      this.propMemoryLocation = MemoryType.NOTValid;
    }

    internal Module(Cpu cpu, APIFC_ModulInfoRes moduleInfo, ModuleCollection collection)
      : base((Base) cpu)
    {
      ModuleInfoDecription moduleInfo1 = new ModuleInfoDecription();
      moduleInfo1.Init(moduleInfo);
      this.Init(cpu, moduleInfo.name);
      this.updateProperties(moduleInfo1, false);
      collection?.Add(this);
    }

    internal Module(Cpu cpu, ModuleInfoDecription moduleInfo, ModuleCollection collection)
      : base((Base) cpu)
    {
      this.Init(cpu, moduleInfo.name);
      this.updateProperties(moduleInfo, cpu.BootMode == BootMode.Diagnostics);
      collection?.Add(this);
    }

    internal Module(Cpu cpu, APIFC_DiagModulInfoRes moduleInfo, ModuleCollection collection)
      : base((Base) cpu)
    {
      this.Init(cpu, moduleInfo.name);
      this.updateProperties(moduleInfo);
      collection.Add(this);
    }

    internal void updateProperties(object moduleInfo) => this.updateProperties(moduleInfo, false);

    internal void updateProperties(object moduleInfo, bool isDiagnosticData)
    {
      switch (moduleInfo)
      {
        case APIFC_ModulInfoRes apifcInfo:
          ModuleInfoDecription moduleInfo1 = new ModuleInfoDecription();
          moduleInfo1.Init(apifcInfo);
          this.updateProperties((object) moduleInfo1);
          break;
        case APIFC_DiagModulInfoRes moduleInfo2:
          this.updateProperties(moduleInfo2);
          break;
        case ModuleInfoDecription _:
          this.updateProperties((ModuleInfoDecription) moduleInfo, isDiagnosticData);
          break;
      }
    }

    internal void updateProperties(APIFC_ModulInfoRes moduleInfo)
    {
      ModuleInfoDecription moduleInfo1 = new ModuleInfoDecription();
      moduleInfo1.Init(moduleInfo);
      this.updateProperties((object) moduleInfo1);
    }

    internal void updateProperties(ModuleInfoDecription moduleInfo, bool isDiagnosticData)
    {
      this.propModuleInfoRequested = false;
      this.propDMIndex = moduleInfo.dm_index;
      this.propPIIndex = PviMarshal.Convert.BytesToUShort(moduleInfo.instP_valid, moduleInfo.instP_value);
      this.propInstallationPriority = (byte) 128;
      this.propInstPriorityValid = false;
      if (byte.MaxValue == moduleInfo.instP_valid)
      {
        this.propInstPriorityValid = true;
        this.propInstallationPriority = moduleInfo.instP_value;
      }
      this.propDMState = moduleInfo.dm_state;
      if (this.Cpu.Connection.DeviceType == DeviceType.ANSLTcp && (isDiagnosticData || this.Cpu.BootMode == BootMode.Diagnostics))
        this.propDMState = moduleInfo.dm_state != DomainState.NonExistent ? DomainState.Invalid : DomainState.Valid;
      if (this.Cpu != null)
        this.Cpu.UpdateModuleInfoList(moduleInfo);
      if (this.propStartOrStopRequest)
      {
        if (moduleInfo.pi_state == ProgramState.Stopped)
          this.OnStopped(new PviEventArgs(this.Name, this.Address, 0, this.Service.Language, Action.ModuleEvent, this.Service));
        else if (moduleInfo.pi_state == ProgramState.Running)
          this.OnStarted(new PviEventArgs(this.Name, this.Address, 0, this.Service.Language, Action.ModuleEvent, this.Service));
      }
      else if (this.propPIState == ProgramState.Running && moduleInfo.pi_state == ProgramState.Stopped)
        this.OnStopped(new PviEventArgs(this.Name, this.Address, 0, this.Service.Language, Action.ModuleEvent, this.Service));
      else if (this.propPIState == ProgramState.Stopped && moduleInfo.pi_state == ProgramState.Running)
        this.OnStarted(new PviEventArgs(this.Name, this.Address, 0, this.Service.Language, Action.ModuleEvent, this.Service));
      this.propPIState = moduleInfo.pi_state;
      this.propPICount = moduleInfo.pi_count;
      this.propStartAddress = moduleInfo.address;
      this.propLength = moduleInfo.length;
      this.propAddress = moduleInfo.name;
      if (this.propName == null || this.propName.Length == 0)
        this.propName = moduleInfo.name;
      this.propVersion = moduleInfo.version;
      this.propRevision = moduleInfo.revision;
      this.propCreateTime = moduleInfo.erz_time;
      this.propAndTime = moduleInfo.and_time;
      this.propCreateName = moduleInfo.erz_name;
      this.propAndName = moduleInfo.and_name;
      this.propTaskClass = moduleInfo.task_class;
      this.propInstallNumber = moduleInfo.install_no;
      this.propPVIndex = moduleInfo.pv_idx;
      this.propPVCount = moduleInfo.pv_cnt;
      this.propAnalogMemoryAddress = moduleInfo.mem_ana_adr;
      this.propDigitalMemoryAddess = moduleInfo.mem_dig_adr;
      this.propMemoryLocation = moduleInfo.mem_location;
      this.propType = moduleInfo.type;
    }

    internal void updateProperties(APIFC_DiagModulInfoRes moduleInfo)
    {
      this.propModuleInfoRequested = false;
      if (this.propName == null || this.propName.Length == 0)
        this.propName = moduleInfo.name;
      this.propDMIndex = moduleInfo.dm_index;
      this.propAddress = moduleInfo.name;
      this.propVersion = moduleInfo.version;
      this.propRevision = moduleInfo.revision;
      this.propCreateTime = moduleInfo.erz_time;
      this.propAndTime = moduleInfo.and_time;
      this.propMemoryLocation = moduleInfo.mem_location;
      this.propDMState = moduleInfo.dm_state != (byte) 0 ? DomainState.Invalid : DomainState.Valid;
      this.propType = ModuleType.Unknown;
    }

    internal static int TicksToInt32(long ticks) => System.Convert.ToInt32(ticks / 10000000L);

    internal static uint TicksToUInt32(long ticks) => System.Convert.ToUInt32(ticks / 10000000L);

    public override void Connect() => this.Connect(this.ConnectionType);

    public override void Connect(ConnectionType connectionType)
    {
      this.ConnectionType = connectionType;
      this.propReturnValue = this.Connect(false, connectionType, 301U);
    }

    protected override string getLinkDescription() => "EV=edlfps";

    internal override void Connect(bool forceConnection) => this.Connect(forceConnection, this.propConnectionType, 301U);

    internal int Connect(bool forceConnection, ConnectionType connectionType, uint action)
    {
      this.ConnectionType = connectionType;
      int num = 0;
      if (this.reCreateActive || this.LinkId != 0U)
        return -2;
      if (ConnectionStates.Connected == this.propConnectionState || ConnectionStates.ConnectedError == this.propConnectionState)
      {
        this.Fire_ConnectedEvent((object) this, new PviEventArgs(this.Name, this.Address, this.propErrorCode, this.Service.Language, Action.ModuleConnect, this.Service));
        return this.propErrorCode;
      }
      if (this.IsConnected && action == 301U)
      {
        this.OnError(new PviEventArgs(this.propName, this.propAddress, 12002, this.Service.Language, (Action) action, this.Service));
        return 12002;
      }
      if (ConnectionStates.Connecting == this.propConnectionState)
        return 0;
      if (this.propAddress == null || this.propAddress.Length == 0)
        this.propAddress = this.propName;
      this.propObjectParam = "CD=" + this.GetConnectionDescription();
      this.propLinkParam = this.getLinkDescription();
      this.propConnectionState = ConnectionStates.Connecting;
      if (this is Logger && this.Address.ToLower().CompareTo("$Detect_SG4_SysLogger$") == 0)
        num = 0;
      else if (!this.propCpu.IsConnected && this.Service.WaitForParentConnection)
      {
        if (!forceConnection)
          this.Requests |= Actions.Connect;
        else if (Actions.Connect == (this.Requests & Actions.Connect))
          this.Requests &= Actions.GetList | Actions.GetValue | Actions.SetValue | Actions.GetForce | Actions.SetForce | Actions.Start | Actions.Stop | Actions.RunCycle | Actions.GetType | Actions.SetType | Actions.GetLength | Actions.SetRefresh | Actions.CreateLink | Actions.Upload | Actions.Download | Actions.SetActive | Actions.SetHysteresis | Actions.Disconnect | Actions.SetInitValue | Actions.GetLBType | Actions.ModuleInfo | Actions.ListSNMPVariables | Actions.Connected | Actions.Uploading | Actions.GetCpuInfo | Actions.ReadPVFormat | Actions.FireActivated | Actions.ReadIndex | Actions.FireDataValidated | Actions.FireValueChanged | Actions.Link;
        return 0;
      }
      int errorCode = this.Service.IsStatic || this.ConnectionType != ConnectionType.CreateAndLink ? (this.ConnectionType == ConnectionType.Link || this.propMODULEState == ConnectionType.Create ? this.PviLinkObject(action) : this.XCreateRequest(this.Service.hPvi, this.LinkName, ObjectType.POBJ_MODULE, this.propObjectParam, 0U, "", action)) : this.XCreateRequest(this.Service.hPvi, this.LinkName, ObjectType.POBJ_MODULE, this.propObjectParam, action, this.propLinkParam, action);
      if (errorCode != 0 && (action == 301U || action == 909U))
        this.OnError(new PviEventArgs(this.Name, this.Address, errorCode, this.Service.Language, (Action) action, this.Service));
      return errorCode;
    }

    public override void Disconnect()
    {
      this.propConnectionState = ConnectionStates.Disconnecting;
      this.propReturnValue = this.Disconnect(302U, this.propNoDisconnectedEvent);
      if (!this.propNoDisconnectedEvent)
        return;
      this.propConnectionState = ConnectionStates.Disconnected;
    }

    public override void Disconnect(bool noResponse)
    {
      this.propNoDisconnectedEvent = noResponse;
      this.propConnectionState = ConnectionStates.Disconnecting;
      this.propReturnValue = this.Disconnect(302U, this.propNoDisconnectedEvent);
      if (!this.propNoDisconnectedEvent)
        return;
      this.propConnectionState = ConnectionStates.Disconnected;
    }

    internal override int DisconnectRet(uint action) => this.Disconnect(action, false);

    internal int Disconnect(uint action, bool noResponse)
    {
      int errorCode = 0;
      this.Requests = Actions.NONE;
      if (action == 0U && this.LinkId == 0U)
        return 12004;
      if (this.Service != null && this.LinkId != 0U)
      {
        if (noResponse)
        {
          errorCode = this.Unlink();
        }
        else
        {
          errorCode = this.UnlinkRequest(action);
          if (errorCode != 0 && action == 302U)
            this.OnError(new PviEventArgs(this.Name, this.Address, errorCode, this.Service.Language, (Action) action, this.Service));
        }
      }
      return errorCode;
    }

    protected virtual int ModuleInfoRequest()
    {
      int num = 0;
      if (!this.propModuleInfoRequested)
      {
        this.propModuleInfoRequested = true;
        num = this.ReadArgumentRequest(this.Service.hPvi, this.propLinkId, AccessTypes.ANSL_ModuleInfo, IntPtr.Zero, 0, 308U);
      }
      return num;
    }

    internal virtual int ReadModuleInfo()
    {
      APIFC_ModulInfoRes modInfo = new APIFC_ModulInfoRes();
      APIFC_DiagModulInfoRes diagModInfo = new APIFC_DiagModulInfoRes();
      this.Requests |= Actions.ModuleInfo;
      int num = DeviceType.ANSLTcp != ((Cpu) this.propParent).Connection.DeviceType ? ((Cpu) this.propParent).ReadModuleList(this.propAddress, out modInfo, out diagModInfo) : this.ModuleInfoRequest();
      if (0 < num)
        return num;
      if (num != 0)
        return 0;
      if (modInfo.name != null && modInfo.name != "")
      {
        this.updateProperties(modInfo);
        this.Requests &= Actions.Connect | Actions.GetList | Actions.GetValue | Actions.SetValue | Actions.GetForce | Actions.SetForce | Actions.Start | Actions.Stop | Actions.RunCycle | Actions.GetType | Actions.SetType | Actions.GetLength | Actions.SetRefresh | Actions.CreateLink | Actions.Upload | Actions.Download | Actions.SetActive | Actions.SetHysteresis | Actions.Disconnect | Actions.SetInitValue | Actions.GetLBType | Actions.ListSNMPVariables | Actions.Connected | Actions.Uploading | Actions.GetCpuInfo | Actions.ReadPVFormat | Actions.FireActivated | Actions.ReadIndex | Actions.FireDataValidated | Actions.FireValueChanged | Actions.Link;
        this.OnConnected(new PviEventArgs(this.Name, this.Address, 0, this.Service.Language, Action.ConnectedEvent, this.Service));
      }
      else if (diagModInfo.name != null && diagModInfo.name != "")
        this.updateProperties(diagModInfo);
      return num;
    }

    [Browsable(false)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    internal virtual void Resume()
    {
      this.propReturnValue = 0;
      this.propStartOrStopRequest = true;
      this.propReturnValue = this.WriteRequest(this.Cpu.Service.hPvi, this.propLinkId, AccessTypes.ResumeModule, IntPtr.Zero, 0, 303U);
      if (this.propReturnValue == 0)
        return;
      this.OnError(new PviEventArgs(this.Name, this.Address, this.propReturnValue, this.Service.Language, Action.ModuleStart, this.Service));
      this.OnStarted(new PviEventArgs(this.Name, this.Address, this.propReturnValue, this.Service.Language, Action.ModuleStart, this.Service));
    }

    [Browsable(false)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public virtual void Start()
    {
      this.propReturnValue = 0;
      this.propStartOrStopRequest = true;
      this.propReturnValue = this.WriteRequest(this.Cpu.Service.hPvi, this.propLinkId, AccessTypes.ResumeModule, IntPtr.Zero, 0, 303U);
      if (this.propReturnValue == 0)
        return;
      this.OnError(new PviEventArgs(this.Name, this.Address, this.propReturnValue, this.Service.Language, Action.ModuleStart, this.Service));
      this.OnStarted(new PviEventArgs(this.Name, this.Address, this.propReturnValue, this.Service.Language, Action.ModuleStart, this.Service));
    }

    [Browsable(false)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public virtual void Stop()
    {
      this.propReturnValue = 0;
      this.propStartOrStopRequest = true;
      this.propReturnValue = this.WriteRequest(this.Cpu.Service.hPvi, this.propLinkId, AccessTypes.StopModule, IntPtr.Zero, 0, 304U);
      if (this.propReturnValue == 0)
        return;
      this.OnError(new PviEventArgs(this.Name, this.Address, this.propReturnValue, this.Service.Language, Action.ModuleStop, this.Service));
      this.OnStopped(new PviEventArgs(this.Name, this.Address, this.propReturnValue, this.Service.Language, Action.ModuleStop, this.Service));
    }

    public virtual void Delete() => this.Delete(false);

    public virtual void Delete(bool useParentCpu)
    {
      IntPtr zero = IntPtr.Zero;
      this.propReturnValue = 0;
      if (useParentCpu)
      {
        string str = "MN=";
        string request = this.Address == null || 0 >= this.Address.Length ? str + this.Name : str + this.Address;
        this.Service.BuildRequestBuffer(request);
        int length = request.Length;
        if (this.Cpu.BootMode == BootMode.Diagnostics && DeviceType.ANSLTcp != ((Cpu) this.propParent).Connection.DeviceType)
        {
          PviMarshal.WriteUInt16(this.Service.RequestBuffer, this.propDMIndex);
          this.propReturnValue = this.WriteRequest(this.Service.hPvi, this.Cpu.LinkId, AccessTypes.DeleteDiagModule, this.Service.RequestBuffer, Marshal.SizeOf(typeof (ushort)), 219U);
        }
        else
          this.propReturnValue = this.WriteRequest(this.Service.hPvi, this.Cpu.LinkId, AccessTypes.CpuModuleDelete, this.Service.RequestBuffer, length, 219U);
        if (this.propReturnValue == 0)
          return;
        this.OnError(new PviEventArgs(this.Name, this.Address, this.propReturnValue, this.Service.Language, Action.ModuleDelete, this.Service));
      }
      else
      {
        if (this.Cpu.BootMode == BootMode.Diagnostics)
        {
          PviMarshal.WriteUInt16(this.Service.RequestBuffer, this.propDMIndex);
          this.propReturnValue = this.WriteRequest(this.Service.hPvi, this.Cpu.LinkId, AccessTypes.DeleteDiagModule, this.Service.RequestBuffer, Marshal.SizeOf(typeof (ushort)), 305U);
        }
        else
          this.propReturnValue = this.WriteRequest(this.Service.hPvi, this.propLinkId, AccessTypes.DeleteModule, zero, 0, 305U);
        if (this.propReturnValue == 0)
          return;
        this.OnError(new PviEventArgs(this.Name, this.Address, this.propReturnValue, this.Service.Language, Action.ModuleDelete, this.Service));
      }
    }

    public virtual void Upload(string fName) => this.Upload(fName, ConversionModes.BR, CommentLanguages.DEFAULT);

    public virtual void Upload(string fName, ConversionModes uploadConversion) => this.Upload(fName, uploadConversion, CommentLanguages.DEFAULT);

    public virtual void Upload(
      string fName,
      ConversionModes uploadConversion,
      CommentLanguages commentLanguage)
    {
      this.propReturnValue = 0;
      if (!this.IsConnected && this.Service.WaitForParentConnection)
      {
        this.Requests |= Actions.Upload;
        this.propFileName = fName;
      }
      else
      {
        string str;
        switch (uploadConversion)
        {
          case ConversionModes.TXT:
            str = " \"MT=BRT\" ";
            break;
          case ConversionModes.NC_UPLOAD:
          case ConversionModes.CNC:
          case ConversionModes.ZPO:
          case ConversionModes.TDT:
          case ConversionModes.RPT:
          case ConversionModes.CAM:
          case ConversionModes.CAP:
            str = commentLanguage != CommentLanguages.DEFAULT ? " \"MT=NC_ RL=" + commentLanguage.ToString() + "\" " : " \"MT=NC_\" ";
            break;
          default:
            str = "\"" + fName + "\"";
            break;
        }
        IntPtr hglobal = PviMarshal.StringToHGlobal(str);
        this.Cpu.actUpLoadModuleName = this.Name;
        this.Cpu.listOfUpLoadModules.Add((object) this.Name);
        this.propLastAction = Actions.Upload;
        this.propReturnValue = this.ReadArgumentRequest(this.Service.hPvi, this.LinkId, AccessTypes.UploadStream, hglobal, str.Length, 306U);
        PviMarshal.FreeHGlobal(ref hglobal);
        if (this.propReturnValue == 0)
          this.propFileName = fName;
        if (this.propReturnValue == 0)
          return;
        this.propLastAction = Actions.NONE;
        this.OnError(new PviEventArgs(this.Name, this.Address, this.propReturnValue, this.Service.Language, Action.ModuleUpload, this.Service));
        this.OnUploaded(new PviEventArgs(this.Name, this.Address, this.propReturnValue, this.Service.Language, Action.ModuleUpload, this.Service));
      }
    }

    public virtual void Download(MemoryType memoryType, InstallMode installMode, string fileName)
    {
      int num = fileName.IndexOf(".");
      ConversionModes conversionMode = ConversionModes.BR;
      if (-1 != num && 4 < fileName.Length)
      {
        string str = fileName.Substring(fileName.Length - 4, 4);
        if (str.ToLower().CompareTo(".txt") == 0)
          conversionMode = ConversionModes.TXT;
        else if (str.ToLower().CompareTo(".cnc") == 0)
          conversionMode = ConversionModes.CNC;
        else if (str.ToLower().CompareTo(".zp0") == 0)
          conversionMode = ConversionModes.ZPO;
        else if (str.ToLower().CompareTo(".zpo") == 0)
          conversionMode = ConversionModes.ZPO;
        else if (str.ToLower().CompareTo(".tdt") == 0)
          conversionMode = ConversionModes.TDT;
        else if (str.ToLower().CompareTo(".rpt") == 0)
          conversionMode = ConversionModes.RPT;
        else if (str.ToLower().CompareTo(".cam") == 0)
          conversionMode = ConversionModes.CAM;
        else if (str.ToLower().CompareTo(".cap") == 0)
          conversionMode = ConversionModes.CAP;
        this.Download(memoryType, installMode, fileName, conversionMode, (string) null, (string) null);
      }
      else
        this.OnDownloaded(new PviEventArgs(this.Name, this.Address, 4067, this.Service.Language, Action.ModuleDownload, this.Service));
    }

    public virtual void Download() => this.Download(MemoryType.UserRam, InstallMode.Overload, this.propFileName);

    public virtual void Download(MemoryType memoryType) => this.Download(memoryType, InstallMode.Overload, this.propFileName);

    public virtual void Download(MemoryType memoryType, InstallMode installMode) => this.Download(memoryType, installMode, this.propFileName);

    public void Download(
      MemoryType memoryType,
      InstallMode installMode,
      string srcFileName,
      string moduleVersion,
      string moduleName)
    {
      this.Download(memoryType, installMode, srcFileName, ConversionModes.TXT, moduleVersion, moduleName);
    }

    public void Download(
      MemoryType memoryType,
      InstallMode installMode,
      string srcFileName,
      ConversionModes conversionMode,
      string moduleName)
    {
      this.Download(memoryType, installMode, srcFileName, conversionMode, (string) null, moduleName);
    }

    private void Download(
      MemoryType memoryType,
      InstallMode installMode,
      string srcFileName,
      ConversionModes conversionMode,
      string moduleVersion,
      string moduleName)
    {
      FileStream input = new FileStream(srcFileName, FileMode.Open, FileAccess.Read);
      BinaryReader binaryReader = new BinaryReader((Stream) input);
      byte[] moduleData = binaryReader.ReadBytes((int) input.Length);
      binaryReader.Close();
      input.Close();
      this.Download(memoryType, installMode, conversionMode, moduleVersion, moduleName, moduleData);
      if (this.propReturnValue != 0)
        return;
      this.propFileName = srcFileName;
    }

    public void Download(
      MemoryType memoryType,
      InstallMode installMode,
      ConversionModes conversionMode,
      string moduleVersion,
      string moduleName,
      Stream streamData)
    {
      BinaryReader binaryReader = new BinaryReader(streamData);
      byte[] moduleData = binaryReader.ReadBytes((int) streamData.Length);
      binaryReader.Close();
      streamData.Close();
      this.Download(memoryType, installMode, conversionMode, moduleVersion, moduleName, moduleData);
    }

    public unsafe void Download(
      MemoryType memoryType,
      InstallMode installMode,
      ConversionModes conversionMode,
      string moduleVersion,
      string moduleName,
      byte[] moduleData)
    {
      this.propReturnValue = 0;
      string str1 = "";
      switch (conversionMode)
      {
        case ConversionModes.TXT:
          str1 = " \"MT=BRT";
          if (moduleVersion != null && 0 < moduleVersion.Length)
          {
            str1 = str1 + " MV=" + moduleVersion;
            break;
          }
          break;
        case ConversionModes.CNC:
          str1 = " \"MT=NC_CNC";
          break;
        case ConversionModes.ZPO:
          str1 = " \"MT=NC_ZPO";
          break;
        case ConversionModes.TDT:
          str1 = " \"MT=NC_TDT";
          break;
        case ConversionModes.RPT:
          str1 = " \"MT=NC_RPT";
          break;
        case ConversionModes.CAM:
          str1 = " \"MT=NC_CAM";
          break;
        case ConversionModes.CAP:
          str1 = " \"MT=NC_CAP";
          break;
      }
      if (ConversionModes.BR < conversionMode)
        str1 = moduleName == null || 0 >= moduleName.Length ? (this.propAddress == null || 0 >= this.propAddress.Length ? str1 + " MN=" + this.propName + "\" " : str1 + " MN=" + this.Address + "\" ") : str1 + " MN=" + moduleName + "\" ";
      else if (moduleName != null && 0 < moduleName.Length)
        str1 = 0 >= str1.Length ? " MN=" + moduleName + "\" " : str1 + " MN=" + moduleName + "\" ";
      string str2;
      switch (memoryType)
      {
        case MemoryType.SystemRom:
          str2 = str1 + "LD=SysRom";
          break;
        case MemoryType.UserRom:
          str2 = str1 + "LD=Rom";
          break;
        case MemoryType.UserRam:
          str2 = str1 + "LD=Ram";
          break;
        case MemoryType.MemCard:
          str2 = str1 + "LD=MemCard";
          break;
        case MemoryType.FixRam:
          str2 = str1 + "LD=FixRam";
          break;
        case MemoryType.Dram:
          str2 = str1 + "LD=DRam";
          break;
        case MemoryType.Permanent:
          str2 = str1 + "LD=PerMem";
          break;
        case MemoryType.TransferModule:
          str2 = str1 + "LD=Trsf";
          break;
        default:
          throw new InvalidOperationException();
      }
      switch (installMode)
      {
        case InstallMode.Overload:
          str2 += " IM=Overload";
          break;
        case InstallMode.OneCycle:
          str2 += " IM=OneCycle";
          break;
        case InstallMode.Copy:
          str2 += " IM=Copy";
          break;
      }
      str2.ToCharArray();
      int num = str2.Length + moduleData.Length + 1;
      IntPtr zero = IntPtr.Zero;
      IntPtr hMemory = PviMarshal.AllocHGlobal((IntPtr) num);
      for (int index = 0; index < str2.Length; ++index)
        *(sbyte*) ((IntPtr) hMemory.ToPointer() + index) = (sbyte) (byte) str2[index];
      *(sbyte*) ((IntPtr) hMemory.ToPointer() + str2.Length) = (sbyte) 0;
      for (int index = str2.Length + 1; index < num; ++index)
        *(sbyte*) ((IntPtr) hMemory.ToPointer() + index) = (sbyte) moduleData[index - str2.Length - 1];
      this.Cpu.actDownLoadModuleName = this.Name;
      this.Cpu.listOfDownLoadModules.Add((object) this.Name);
      this.propLastAction = Actions.Download;
      this.propReturnValue = this.WriteRequest(this.Service.hPvi, this.Cpu.LinkId, AccessTypes.DownloadStream, hMemory, num, 307U);
      PviMarshal.FreeHGlobal(ref hMemory);
      if (this.propReturnValue != 0)
      {
        this.propLastAction = Actions.NONE;
        this.OnError(new PviEventArgs(this.Name, this.Address, this.propReturnValue, this.Service.Language, Action.ModuleDownload, this.Service));
        this.OnDownloaded(new PviEventArgs(this.Name, this.Address, this.propReturnValue, this.Service.Language, Action.ModuleDownload, this.Service));
      }
      else
      {
        if (this.propCpu.Modules.ContainsKey((object) this.propName))
          return;
        this.propCpu.Modules.Add(this);
      }
    }

    public virtual void Cancel()
    {
      this.propReturnValue = 0;
      int val = 21;
      IntPtr zero = IntPtr.Zero;
      IntPtr hMemory = PviMarshal.AllocHGlobal((IntPtr) 4);
      Marshal.WriteInt32(hMemory, val);
      this.Cpu.actDownLoadModuleName = this.Cpu.RemoveULorDLName(ref this.Cpu.listOfDownLoadModules, this.Name);
      this.Cpu.actUpLoadModuleName = this.Cpu.RemoveULorDLName(ref this.Cpu.listOfUpLoadModules, this.Name);
      if (Actions.Download == (this.propLastAction & Actions.Download))
        this.propReturnValue = this.WriteRequest(this.Service.hPvi, this.Cpu.LinkId, AccessTypes.Cancel, hMemory, 4, 1100U);
      else
        this.propReturnValue = this.WriteRequest(this.Service.hPvi, this.propLinkId, AccessTypes.Cancel, IntPtr.Zero, 0, 1100U);
      if (this.propReturnValue != 0)
        this.OnError(new PviEventArgs(this.Name, this.Address, this.propReturnValue, this.Service.Language, Action.Cancel, this.Service));
      PviMarshal.FreeHGlobal(ref hMemory);
    }

    internal override void OnPviCreated(int errorCode, uint linkID)
    {
      this.propErrorCode = errorCode;
      if (errorCode == 0 || 12002 == errorCode)
      {
        this.propLinkId = linkID;
        if (this.Service == null || !this.Service.IsStatic || this.ConnectionType != ConnectionType.CreateAndLink)
          return;
        this.propMODULEState = ConnectionType.Create;
        this.PviLinkObject(301U);
      }
      else
      {
        if (!Service.IsRemoteError(errorCode))
          return;
        this.Requests |= Actions.Connect;
      }
    }

    internal override int PviLinkObject(uint action) => !this.Service.IsStatic || this.ConnectionType == ConnectionType.Link ? (916U != action ? this.XLinkRequest(this.Service.hPvi, this.LinkName, 707U, this.propLinkParam, 707U) : this.XLinkRequest(this.Service.hPvi, this.LinkName, action, this.propLinkParam, action)) : (916U != action ? this.XLinkRequest(this.Service.hPvi, this.LinkName, 707U, this.propLinkParam, 707U) : this.XLinkRequest(this.Service.hPvi, this.LinkName, action, this.propLinkParam, action));

    internal override void OnPviLinked(int errorCode, uint linkID, int option)
    {
      this.propErrorCode = errorCode;
      this.propLinkId = linkID;
      if (errorCode == 0 && 1 == option)
      {
        int errorCode1 = this.ReadRequest(this.Service.hPvi, linkID, AccessTypes.List, 150U);
        if (errorCode1 != 0)
          this.Service.OnPVIObjectsAttached(new PviEventArgs(this.propName, this.propAddress, errorCode1, this.Service.Language, Action.TaskReadVariablesList));
      }
      base.OnPviLinked(errorCode, linkID, 1);
    }

    internal override void OnPviEvent(
      int errorCode,
      EventTypes eventType,
      PVIDataStates dataState,
      IntPtr pData,
      uint dataLen,
      int option)
    {
      int propErrorCode = this.propErrorCode;
      this.propErrorCode = errorCode;
      switch (eventType)
      {
        case EventTypes.Error:
          if (1 == option)
          {
            this.OnError((object) this, new PviEventArgs(this.Name, this.Address, errorCode, this.Service.Language, Action.LoggerDetectSGType, this.Service));
            break;
          }
          if (errorCode != 0)
          {
            if (ConnectionStates.Connected == this.propConnectionState || ConnectionStates.ConnectedError == this.propConnectionState)
            {
              if (this.Service != null)
                this.OnDisconnected(new PviEventArgs(this.Name, this.Address, errorCode, this.Service.Language, Action.ErrorEvent, this.Service));
            }
            else if (ConnectionStates.Connecting == this.propConnectionState)
              this.OnConnected(new PviEventArgs(this.Name, this.Address, errorCode, this.Service.Language, Action.ConnectedEvent, this.Service));
            if (this.Service == null)
              break;
            this.OnError(new PviEventArgs(this.Name, this.Address, errorCode, this.Service.Language, Action.ErrorEvent, this.Service));
            break;
          }
          this.ReadModuleInfo();
          break;
        case EventTypes.Data:
          int errorCode1 = this.ReadModuleInfo();
          if (errorCode1 == 0)
            break;
          this.OnConnected(new PviEventArgs(this.Name, this.Address, errorCode1, this.Service.Language, Action.ModuleInfo));
          break;
        case EventTypes.Status:
          if (errorCode != 0)
            break;
          break;
        case EventTypes.Proceeding:
          ModuleEventArgs e = new ModuleEventArgs(this.propName, this.propAddress, this.ErrorCode, this.Service.Language, Action.ModuleProgressEvent, this, PviMarshal.PtrToProgressInfoStructure(pData, typeof (ProgressInfo)).Percent);
          if (Actions.Upload == (this.propLastAction & Actions.Upload))
          {
            this.OnUploadProgress(e);
            break;
          }
          this.OnDownloadProgress(e);
          break;
        case EventTypes.ModuleChanged:
          if (errorCode != 0)
            break;
          this.updateProperties(PviMarshal.PtrToModulInfoStructure(pData, typeof (APIFC_ModulInfoRes)));
          if (!(this.Parent is Cpu))
            break;
          ((Cpu) this.Parent).Modules.OnModuleChanged(new ModuleEventArgs(this.propName, this.propAddress, errorCode, this.Service.Language, Action.ModuleChangedEvent, this, 0));
          break;
        case EventTypes.ModuleDeleted:
          if (errorCode != 0)
            break;
          break;
        default:
          base.OnPviEvent(errorCode, eventType, dataState, pData, dataLen, option);
          break;
      }
    }

    internal override void OnPviRead(
      int errorCode,
      PVIReadAccessTypes accessType,
      PVIDataStates dataState,
      IntPtr pData,
      uint dataLen,
      int option)
    {
      this.propErrorCode = errorCode;
      switch (accessType)
      {
        case PVIReadAccessTypes.State:
          this.OnError(new PviEventArgs(this.Name, this.Address, errorCode, this.Service.Language, Action.ModuleState, this.Service));
          break;
        case PVIReadAccessTypes.StreamUpload:
          if (errorCode == 0)
          {
            FileStream output = new FileStream(this.propFileName, FileMode.Create);
            BinaryWriter binaryWriter = new BinaryWriter((Stream) output);
            byte[] numArray = new byte[(IntPtr) dataLen];
            Marshal.Copy(pData, numArray, 0, (int) dataLen);
            binaryWriter.Write(numArray, 0, (int) dataLen);
            binaryWriter.Close();
            output.Close();
            this.OnUploaded(new PviEventArgs(this.propName, this.propAddress, errorCode, this.Service.Language, Action.ModuleUpload, this.Service));
            break;
          }
          if (12043 == errorCode)
            this.OnCancelled(new PviEventArgs(this.propName, this.propAddress, errorCode, this.Service.Language, Action.ModuleUpload, this.Service));
          this.OnUploaded(new PviEventArgs(this.propName, this.propAddress, errorCode, this.Service.Language, Action.ModuleUpload, this.Service));
          break;
        case PVIReadAccessTypes.ModuleList:
          if (errorCode != 0 || this.UpdateModuleInfo(pData, dataLen) != 0)
            break;
          if (1 == option)
          {
            this.propConnectionState = ConnectionStates.Connected;
            this.Upload(this.propFileName);
            break;
          }
          this.OnConnected(new PviEventArgs(this.Name, this.Address, errorCode, this.Service.Language, Action.ConnectedEvent, this.Service));
          break;
        case PVIReadAccessTypes.ANSL_ModuleInfo:
          this.ANSLModuleDescriptionRead(pData, dataLen, errorCode);
          this.Fire_OnConnected(new PviEventArgs(this.Name, this.Address, errorCode, this.Service.Language, Action.ConnectedEvent, this.Service));
          break;
        default:
          base.OnPviRead(errorCode, accessType, dataState, pData, dataLen, option);
          break;
      }
    }

    internal override void OnPviWritten(
      int errorCode,
      PVIWriteAccessTypes accessType,
      PVIDataStates dataState,
      int option,
      IntPtr pData,
      uint dataLen)
    {
      this.propErrorCode = errorCode;
      switch (accessType)
      {
        case PVIWriteAccessTypes.State:
          if (1 == option)
          {
            this.OnStarted(new PviEventArgs(this.Name, this.Address, errorCode, this.Service.Language, Action.ModuleStart, this.Service));
            break;
          }
          if (2 == option)
          {
            this.OnStarted(new PviEventArgs(this.Name, this.Address, errorCode, this.Service.Language, Action.TaskStart, this.Service));
            break;
          }
          if (3 == option)
          {
            this.OnStopped(new PviEventArgs(this.Name, this.Address, errorCode, this.Service.Language, Action.ModuleStop, this.Service));
            break;
          }
          if (4 == option)
          {
            this.OnStopped(new PviEventArgs(this.Name, this.Address, errorCode, this.Service.Language, Action.TaskStop, this.Service));
            break;
          }
          if (5 == option)
          {
            this.OnRunCycleCountSet(new PviEventArgs(this.Name, this.Address, errorCode, this.Service.Language, Action.TaskRunCycle, this.Service));
            break;
          }
          if (6 == option)
          {
            this.Resume();
            break;
          }
          base.OnPviWritten(errorCode, accessType, dataState, option, pData, dataLen);
          break;
        case PVIWriteAccessTypes.StreamDownLoad:
          if (12043 == errorCode)
            this.OnCancelled(new PviEventArgs(this.Name, this.Address, errorCode, this.Service.Language, Action.ModuleDownload, this.Service));
          this.OnDownloaded(new PviEventArgs(this.Name, this.Address, errorCode, this.Service.Language, Action.ModuleDownload, this.Service));
          break;
        case PVIWriteAccessTypes.CpuModuleDelete:
        case PVIWriteAccessTypes.DeleteDiagModule:
        case PVIWriteAccessTypes.DeleteModule:
          this.OnDeleted(new PviEventArgs(this.Name, this.Address, errorCode, this.Service.Language, Action.ModuleDelete, this.Service));
          if (errorCode != 0)
            break;
          this.Remove();
          break;
        case PVIWriteAccessTypes.StartModule:
          this.OnStarted(new PviEventArgs(this.Name, this.Address, errorCode, this.Service.Language, Action.ModuleStart, this.Service));
          break;
        case PVIWriteAccessTypes.StopModule:
          this.OnStopped(new PviEventArgs(this.Name, this.Address, errorCode, this.Service.Language, Action.ModuleStop, this.Service));
          break;
        case PVIWriteAccessTypes.ResumeModule:
          this.OnStarted(new PviEventArgs(this.Name, this.Address, errorCode, this.Service.Language, Action.ModuleResume, this.Service));
          break;
        default:
          base.OnPviWritten(errorCode, accessType, dataState, option, pData, dataLen);
          break;
      }
    }

    internal override void OnPviUnLinked(int errorCode, int option)
    {
      this.propErrorCode = errorCode;
      if (1 == option)
      {
        this.propLinkId = 0U;
        if (this.Service != null)
          this.OnDisconnected(new PviEventArgs(this.Name, this.Address, errorCode, this.Service.Language, Action.UnLinkObject, this.Service));
        if (errorCode == 0)
          return;
        this.OnError(new PviEventArgs(this.Name, this.Address, errorCode, this.Service.Language, Action.UnLinkObject, this.Service));
      }
      else
        base.OnPviUnLinked(errorCode, option);
    }

    internal int UpdateModuleInfo(IntPtr ptrData, uint dataLen)
    {
      int num1 = 164;
      if (this.Cpu.BootMode == BootMode.Diagnostics)
        num1 = 57;
      int num2 = (int) ((long) dataLen / (long) num1);
      for (int index = 0; index < num2; ++index)
      {
        int ptr = (int) ptrData + index * num1;
        if (this.Cpu.BootMode == BootMode.Diagnostics)
        {
          APIFC_DiagModulInfoRes modulInfoStructure = PviMarshal.PtrToDiagModulInfoStructure((IntPtr) ptr, typeof (APIFC_DiagModulInfoRes));
          if (this.propParent is Cpu && this.Address == modulInfoStructure.name)
            this.updateProperties(modulInfoStructure);
        }
        else
        {
          APIFC_ModulInfoRes modulInfoStructure = PviMarshal.PtrToModulInfoStructure((IntPtr) ptr, typeof (APIFC_ModulInfoRes));
          if (this.propParent is Cpu && this.Address.CompareTo(modulInfoStructure.name) == 0)
          {
            this.updateProperties(modulInfoStructure);
            return 0;
          }
        }
      }
      return -1;
    }

    internal virtual bool CheckModuleInfo(int errorCode)
    {
      if ((this.Requests & Actions.ModuleInfo) == Actions.NONE)
        return false;
      this.Requests &= Actions.Connect | Actions.GetList | Actions.GetValue | Actions.SetValue | Actions.GetForce | Actions.SetForce | Actions.Start | Actions.Stop | Actions.RunCycle | Actions.GetType | Actions.SetType | Actions.GetLength | Actions.SetRefresh | Actions.CreateLink | Actions.Upload | Actions.Download | Actions.SetActive | Actions.SetHysteresis | Actions.Disconnect | Actions.SetInitValue | Actions.GetLBType | Actions.ListSNMPVariables | Actions.Connected | Actions.Uploading | Actions.GetCpuInfo | Actions.ReadPVFormat | Actions.FireActivated | Actions.ReadIndex | Actions.FireDataValidated | Actions.FireValueChanged | Actions.Link;
      return true;
    }

    internal override void Dispose(bool disposing, bool removeFromCollection)
    {
      if (this.propDisposed)
        return;
      Base propParent = this.propParent;
      string propLinkName = this.propLinkName;
      string propLogicalName = this.propLogicalName;
      object propUserData = this.propUserData;
      string propName = this.propName;
      string propAddress = this.propAddress;
      base.Dispose(disposing, removeFromCollection);
      if (!disposing)
        return;
      this.propParent = propParent;
      this.propLinkName = propLinkName;
      this.propLogicalName = propLogicalName;
      this.propUserData = propUserData;
      this.propName = propName;
      this.propAddress = propAddress;
      if (removeFromCollection)
      {
        this.RemoveFromBaseCollections();
        this.RemoveObject();
      }
      this.propAndName = (string) null;
      this.propCpu = (Cpu) null;
      this.propCreateName = (string) null;
      this.propFileName = (string) null;
      if (this.propUserCollections != null)
      {
        this.propUserCollections.Clear();
        this.propUserCollections = (Hashtable) null;
      }
      this.propParent = (Base) null;
      this.propLinkName = (string) null;
      this.propLogicalName = (string) null;
      this.propUserData = (object) null;
    }

    internal override void RemoveReferences()
    {
      if (this.propUserCollections == null || 0 >= this.propUserCollections.Count)
        return;
      foreach (BaseCollection baseCollection in (IEnumerable) this.propUserCollections.Values)
        baseCollection.Remove(this.Name);
    }

    internal override void RemoveObject()
    {
      base.RemoveObject();
      if (this.Cpu == null)
        return;
      if (this.Cpu.Tasks != null && this is Task)
        this.Cpu.Tasks.Remove(this.Name);
      if (this.Cpu.Loggers != null && this is Logger)
        this.Cpu.Loggers.Remove(this.Name);
      if (this.propUserCollections == null || 0 >= this.propUserCollections.Count)
        return;
      foreach (BaseCollection baseCollection in (IEnumerable) this.propUserCollections.Values)
        baseCollection.Remove(this.Name);
    }

    public override void Remove()
    {
      base.Remove();
      if (this.Cpu != null)
      {
        this.Cpu.Modules.Remove(this.Name);
        if (this is Task && this.Cpu.Tasks != null)
          this.Cpu.Tasks.Remove(this.Name);
        if (this is Logger && this.Cpu.Loggers != null)
          this.propCpu.Loggers.Remove(this.Name);
      }
      if (this.propUserCollections == null || 0 >= this.propUserCollections.Count)
        return;
      foreach (BaseCollection baseCollection in (IEnumerable) this.propUserCollections.Values)
        baseCollection.Remove(this.Name);
    }

    internal override void RemoveFromBaseCollections()
    {
      base.RemoveFromBaseCollections();
      if (this is Task && this.propCpu != null)
      {
        if (CollectionType.ArrayList == this.propCpu.Tasks.propCollectionType)
          this.propCpu.Tasks.Remove((object) this);
        else
          this.propCpu.Tasks.Remove(this.Name);
      }
      if (this is Logger && this.propCpu != null)
      {
        if (CollectionType.ArrayList == this.propCpu.Loggers.propCollectionType)
          this.propCpu.Loggers.Remove((object) this);
        else
          this.propCpu.Loggers.Remove(this.Name);
      }
      if (this.propUserCollections == null || 0 >= this.propUserCollections.Count)
        return;
      foreach (BaseCollection baseCollection in (IEnumerable) this.propUserCollections.Values)
        baseCollection.Remove(this.Name);
    }

    internal void Fire_OnConnect() => this.OnConnected(new PviEventArgs(this.Name, this.Address, this.propErrorCode, this.Service.Language, Action.ConnectedEvent));

    protected override void OnConnected(PviEventArgs e)
    {
      bool flag = false;
      if (e.ErrorCode == 0 || 12002 == e.ErrorCode)
      {
        if (ConnectionStates.Connected != this.propConnectionState)
          flag = true;
      }
      else if (ConnectionStates.ConnectedError > this.propConnectionState && ConnectionStates.Unininitialized < this.propConnectionState)
        flag = true;
      if ((this.Requests & Actions.Upload) != Actions.NONE)
      {
        this.Requests &= Actions.Connect | Actions.GetList | Actions.GetValue | Actions.SetValue | Actions.GetForce | Actions.SetForce | Actions.Start | Actions.Stop | Actions.RunCycle | Actions.GetType | Actions.SetType | Actions.GetLength | Actions.SetRefresh | Actions.CreateLink | Actions.Download | Actions.SetActive | Actions.SetHysteresis | Actions.Disconnect | Actions.SetInitValue | Actions.GetLBType | Actions.ModuleInfo | Actions.ListSNMPVariables | Actions.Connected | Actions.Uploading | Actions.GetCpuInfo | Actions.ReadPVFormat | Actions.FireActivated | Actions.ReadIndex | Actions.FireDataValidated | Actions.FireValueChanged | Actions.Link;
        this.Upload(this.propFileName);
      }
      base.OnConnected(e);
      if (!flag)
        return;
      if (this.Cpu != null)
        this.Cpu.Modules.OnConnected((Base) this, e);
      if (this.propUserCollections == null || this.propUserCollections.Count <= 0 || this is Task)
        return;
      foreach (BaseCollection baseCollection in (IEnumerable) this.propUserCollections.Values)
        baseCollection.OnConnected((Base) this, e);
    }

    protected override void OnDisconnected(PviEventArgs e)
    {
      bool flag = ConnectionStates.Connected == this.propConnectionState || ConnectionStates.ConnectedError == this.propConnectionState || ConnectionStates.Disconnecting == this.propConnectionState;
      base.OnDisconnected(e);
      this.propMODULEState = ConnectionType.None;
      if (!flag)
        return;
      if (this.Cpu != null && this.Cpu.Modules != null)
        this.Cpu.Modules.OnDisconnected((Base) this, e);
      if (this.propUserCollections == null || 0 >= this.propUserCollections.Count)
        return;
      foreach (BaseCollection baseCollection in (IEnumerable) this.propUserCollections.Values)
        baseCollection.OnDisconnected((Base) this, e);
    }

    protected virtual void OnRunCycleCountSet(PviEventArgs e)
    {
      if (this.RunCycleCountSet == null)
        return;
      this.RunCycleCountSet((object) this, e);
    }

    protected virtual void OnStarted(PviEventArgs e)
    {
      if (e.ErrorCode == 0)
      {
        this.propPIState = ProgramState.Running;
        this.propStartOrStopRequest = false;
      }
      if (this.Started == null)
        return;
      this.Started((object) this, e);
    }

    protected virtual void OnStopped(PviEventArgs e)
    {
      if (e.ErrorCode == 0)
      {
        this.propPIState = ProgramState.Stopped;
        this.propStartOrStopRequest = false;
      }
      if (this.Stopped == null)
        return;
      this.Stopped((object) this, e);
    }

    protected override void OnDeleted(PviEventArgs e)
    {
      if (this.Cpu != null && this.Service != null)
      {
        this.Cpu.Modules.OnModuleDeleted(new ModuleEventArgs(this.Name, this.propAddress, e.ErrorCode, this.Service.Language, Action.ModuleDeletedEvent, this, 0));
        if (this is Task)
          this.Cpu.Tasks.OnTaskDeleted(new ModuleEventArgs(this.Name, this.propAddress, e.ErrorCode, this.Service.Language, Action.ModuleDeletedEvent, this, 0));
        Logger logger = this as Logger;
      }
      if (this.LinkId == 0U)
        base.OnDeleted(e);
      if (this.Deleted == null)
        return;
      this.Deleted((object) this, e);
    }

    protected virtual void OnUploaded(PviEventArgs e)
    {
      this.propLastAction = Actions.NONE;
      this.Cpu.actUpLoadModuleName = this.Cpu.RemoveULorDLName(ref this.Cpu.listOfUpLoadModules, this.Name);
      if (this.Uploaded != null)
        this.Uploaded((object) this, e);
      if (this.Cpu != null)
        this.Cpu.Modules.OnModuleUploaded(this, e);
      if (this is Task && this.Cpu != null)
        this.Cpu.Tasks.OnTaskUploaded((Task) this, e);
      if (this is Logger && this.Cpu != null)
        this.Cpu.Loggers.OnLoggerUploaded((Logger) this, e);
      if (this.propUserCollections == null || this.propUserCollections.Count <= 0)
        return;
      foreach (BaseCollection baseCollection in (IEnumerable) this.propUserCollections.Values)
      {
        if (baseCollection is LoggerCollection)
          ((LoggerCollection) baseCollection).OnLoggerUploaded((Logger) this, e);
        if (baseCollection is TaskCollection)
          ((TaskCollection) baseCollection).OnTaskUploaded((Task) this, e);
        if (baseCollection is ModuleCollection)
          ((ModuleCollection) baseCollection).OnModuleUploaded(this, e);
      }
    }

    protected virtual void OnDownloaded(PviEventArgs e)
    {
      this.propLastAction = Actions.NONE;
      this.Cpu.actDownLoadModuleName = this.Cpu.RemoveULorDLName(ref this.Cpu.listOfDownLoadModules, this.Name);
      if (this.Downloaded != null)
        this.Downloaded((object) this, e);
      if (this.Cpu != null)
        this.Cpu.Modules.OnModuleDownloaded(this, e);
      if (this.propUserCollections == null || this.propUserCollections.Count <= 0)
        return;
      foreach (BaseCollection baseCollection in (IEnumerable) this.propUserCollections.Values)
      {
        if (baseCollection is TaskCollection)
          ((TaskCollection) baseCollection).OnTaskDownloaded((Task) this, e);
        if (baseCollection is ModuleCollection)
          ((ModuleCollection) baseCollection).OnModuleDownloaded(this, e);
      }
    }

    protected virtual void OnUploadProgress(ModuleEventArgs e)
    {
      if (this.UploadProgress != null)
        this.UploadProgress((object) this, e);
      if (this.Cpu != null)
        this.Cpu.Modules.OnUploadProgress(this, e);
      if (this is Task && this.Cpu != null)
        this.Cpu.Tasks.OnUploadProgress(this, e);
      if (this.propUserCollections == null || this.propUserCollections.Count <= 0)
        return;
      if (this is Task)
      {
        foreach (TaskCollection taskCollection in (IEnumerable) this.propUserCollections.Values)
          taskCollection.OnUploadProgress(this, e);
      }
      else
      {
        foreach (BaseCollection baseCollection in (IEnumerable) this.propUserCollections.Values)
        {
          if (baseCollection is TaskCollection)
            ((TaskCollection) baseCollection).OnUploadProgress(this, e);
          else
            ((ModuleCollection) baseCollection).OnUploadProgress(this, e);
        }
      }
    }

    protected virtual void OnDownloadProgress(ModuleEventArgs e)
    {
      if (this.DownloadProgress != null)
        this.DownloadProgress((object) this, e);
      if (this.Cpu != null)
        this.Cpu.Modules.OnDownloadProgress(this, e);
      if (this.propUserCollections == null || this.propUserCollections.Count <= 0)
        return;
      foreach (BaseCollection baseCollection in (IEnumerable) this.propUserCollections.Values)
      {
        if (baseCollection is TaskCollection)
          ((TaskCollection) baseCollection).OnDownloadProgress(this, e);
        else
          ((ModuleCollection) baseCollection).OnDownloadProgress(this, e);
      }
    }

    protected internal override void OnError(PviEventArgs e)
    {
      base.OnError(e);
      if (this.Cpu != null && this.Cpu.Modules != null)
        this.Cpu.Modules.OnError((Base) this, e);
      if (this.propUserCollections == null || this.propUserCollections.Count <= 0 || this is Task)
        return;
      foreach (BaseCollection baseCollection in (IEnumerable) this.propUserCollections.Values)
        baseCollection.OnError((Base) this, e);
    }

    protected internal void OnCancelled(PviEventArgs e)
    {
      if (this.Cancelled == null)
        return;
      this.Cancelled((object) this, e);
    }

    public override int FromXmlTextReader(
      ref XmlTextReader reader,
      ConfigurationFlags flags,
      Base baseObj)
    {
      Module baseObj1 = (Module) baseObj;
      if (baseObj1 == null)
        return -1;
      base.FromXmlTextReader(ref reader, flags, (Base) baseObj1);
      DateTime result1 = DateTime.MinValue;
      string str = "";
      string attribute1 = reader.GetAttribute("CreationTime");
      if (attribute1 != null && attribute1.Length > 0 && PviParse.TryParseDateTime(attribute1, out result1))
        baseObj1.propCreateTime = Pvi.GetDateTimeUInt32((object) result1);
      str = "";
      string attribute2 = reader.GetAttribute("CreationName");
      if (attribute2 != null && attribute2.Length > 0)
        baseObj1.propCreateName = attribute2;
      str = "";
      string attribute3 = reader.GetAttribute("LastWriteTime");
      if (attribute3 != null && attribute3.Length > 0 && PviParse.TryParseDateTime(attribute3, out result1))
        baseObj1.propAndTime = Pvi.GetDateTimeUInt32((object) result1);
      str = "";
      string attribute4 = reader.GetAttribute("LastWriteName");
      if (attribute4 != null && attribute4.Length > 0)
        baseObj1.propAndName = attribute4;
      str = "";
      string attribute5 = reader.GetAttribute("Length");
      if (attribute5 != null && attribute5.Length > 0)
      {
        uint result2 = 0;
        if (PviParse.TryParseUInt32(attribute5, out result2))
          baseObj1.propLength = result2;
      }
      str = "";
      string attribute6 = reader.GetAttribute("DigitalMemoryAddress");
      if (attribute6 != null && attribute6.Length > 0)
      {
        uint result3 = 0;
        if (PviParse.TryParseUInt32(attribute6, out result3))
          baseObj1.propDigitalMemoryAddess = result3;
      }
      str = "";
      string attribute7 = reader.GetAttribute("AnalogMemoryAddress");
      if (attribute7 != null && attribute7.Length > 0)
      {
        uint result4 = 0;
        if (PviParse.TryParseUInt32(attribute7, out result4))
          baseObj1.propAnalogMemoryAddress = result4;
      }
      str = "";
      string attribute8 = reader.GetAttribute("MemoryType");
      if (attribute8 != null && attribute8.Length > 0)
      {
        switch (attribute8.ToLower())
        {
          case "dram":
            baseObj1.propMemoryLocation = MemoryType.Dram;
            break;
          case "fixram":
            baseObj1.propMemoryLocation = MemoryType.FixRam;
            break;
          case "globalanalog":
            baseObj1.propMemoryLocation = MemoryType.GlobalAnalog;
            break;
          case "globaldigital":
            baseObj1.propMemoryLocation = MemoryType.GlobalDigital;
            break;
          case "oo":
            baseObj1.propMemoryLocation = MemoryType.Io;
            break;
          case "memcard":
            baseObj1.propMemoryLocation = MemoryType.MemCard;
            break;
          case "os":
            baseObj1.propMemoryLocation = MemoryType.Os;
            break;
          case "permanent":
            baseObj1.propMemoryLocation = MemoryType.Permanent;
            break;
          case "systemram":
            baseObj1.propMemoryLocation = MemoryType.SystemRam;
            break;
          case "systemrom":
            baseObj1.propMemoryLocation = MemoryType.SystemRom;
            break;
          case "tmp":
            baseObj1.propMemoryLocation = MemoryType.Tmp;
            break;
          case "userram":
            baseObj1.propMemoryLocation = MemoryType.UserRam;
            break;
          case "userrom":
            baseObj1.propMemoryLocation = MemoryType.UserRom;
            break;
          case "sysinternal":
            baseObj1.propMemoryLocation = MemoryType.SysInternal;
            break;
          case "remanent":
            baseObj1.propMemoryLocation = MemoryType.Remanent;
            break;
          case "systemsettings":
            baseObj1.propMemoryLocation = MemoryType.SystemSettings;
            break;
          case "transfermodule":
            baseObj1.propMemoryLocation = MemoryType.TransferModule;
            break;
        }
      }
      str = "";
      string attribute9 = reader.GetAttribute("DomainIndex");
      if (attribute9 != null && attribute9.Length > 0)
      {
        ushort result5 = 0;
        if (PviParse.TryParseUInt16(attribute9, out result5))
          baseObj1.propDMIndex = result5;
      }
      str = "";
      string attribute10 = reader.GetAttribute("DomainState");
      if (attribute10 != null && attribute10.Length > 0)
      {
        switch (attribute10.ToLower())
        {
          case "complete":
            baseObj1.propDMState = DomainState.Complete;
            break;
          case "existent":
            baseObj1.propDMState = DomainState.Existent;
            break;
          case "incomplete":
            baseObj1.propDMState = DomainState.Incomplete;
            break;
          case "invalid":
            baseObj1.propDMState = DomainState.Invalid;
            break;
          case "loading":
            baseObj1.propDMState = DomainState.Loading;
            break;
          case "nonexistent":
            baseObj1.propDMState = DomainState.NonExistent;
            break;
          case "ready":
            baseObj1.propDMState = DomainState.Ready;
            break;
          case "use":
            baseObj1.propDMState = DomainState.Use;
            break;
          case "valid":
            baseObj1.propDMState = DomainState.Valid;
            break;
        }
      }
      str = "";
      string attribute11 = reader.GetAttribute("InstallNumber");
      if (attribute11 != null && attribute11.Length > 0)
      {
        byte result6 = 0;
        if (PviParse.TryParseByte(attribute11, out result6))
          baseObj1.propInstallNumber = result6;
      }
      str = "";
      string attribute12 = reader.GetAttribute("ModUID");
      if (attribute12 != null && attribute12.Length > 0)
      {
        uint result7 = 0;
        if (PviParse.TryParseUInt32(attribute12, out result7))
          baseObj1.propModUID = result7;
      }
      str = "";
      string attribute13 = reader.GetAttribute("PiCount");
      if (attribute13 != null && attribute13.Length > 0)
      {
        byte result8 = 0;
        if (PviParse.TryParseByte(attribute13, out result8))
          baseObj1.propPICount = result8;
      }
      str = "";
      string attribute14 = reader.GetAttribute("PiIndex");
      if (attribute14 != null && attribute14.Length > 0)
      {
        ushort result9 = 0;
        if (PviParse.TryParseUInt16(attribute14, out result9))
          baseObj1.propPIIndex = result9;
      }
      str = "";
      string attribute15 = reader.GetAttribute("StartAddress");
      if (attribute15 != null && attribute15.Length > 0)
      {
        uint result10 = 0;
        if (PviParse.TryParseUInt32(attribute15, out result10))
          baseObj1.propStartAddress = result10;
      }
      str = "";
      string attribute16 = reader.GetAttribute("VariableCount");
      if (attribute16 != null && attribute16.Length > 0)
      {
        ushort result11 = 0;
        if (PviParse.TryParseUInt16(attribute16, out result11))
          baseObj1.propPVCount = result11;
      }
      str = "";
      string attribute17 = reader.GetAttribute("VariableIndex");
      if (attribute17 != null && attribute17.Length > 0)
      {
        ushort result12 = 0;
        if (PviParse.TryParseUInt16(attribute17, out result12))
          baseObj1.propPVIndex = result12;
      }
      str = "";
      string attribute18 = reader.GetAttribute("Version");
      if (attribute18 != null && attribute18.Length > 0)
      {
        byte result13 = 0;
        if (PviParse.TryParseByte(attribute18, out result13))
          baseObj1.propVersion = result13;
      }
      str = "";
      string attribute19 = reader.GetAttribute("TaskClassType");
      if (attribute19 != null && attribute19.Length > 0)
      {
        switch (attribute19.ToLower())
        {
          case "cyclic1":
            baseObj1.propTaskClass = TaskClassType.Cyclic1;
            break;
          case "cyclic2":
            baseObj1.propTaskClass = TaskClassType.Cyclic2;
            break;
          case "cyclic3":
            baseObj1.propTaskClass = TaskClassType.Cyclic3;
            break;
          case "cyclic4":
            baseObj1.propTaskClass = TaskClassType.Cyclic4;
            break;
          case "cyclic5":
            baseObj1.propTaskClass = TaskClassType.Cyclic5;
            break;
          case "cyclic6":
            baseObj1.propTaskClass = TaskClassType.Cyclic6;
            break;
          case "cyclic7":
            baseObj1.propTaskClass = TaskClassType.Cyclic7;
            break;
          case "cyclic8":
            baseObj1.propTaskClass = TaskClassType.Cyclic8;
            break;
          case "timer1":
            baseObj1.propTaskClass = TaskClassType.Timer1;
            break;
          case "timer2":
            baseObj1.propTaskClass = TaskClassType.Timer2;
            break;
          case "timer3":
            baseObj1.propTaskClass = TaskClassType.Timer3;
            break;
          case "timer4":
            baseObj1.propTaskClass = TaskClassType.Timer4;
            break;
          case "exception":
            baseObj1.propTaskClass = TaskClassType.Exception;
            break;
          case "interrupt":
            baseObj1.propTaskClass = TaskClassType.Interrupt;
            break;
          case "notvalid":
            baseObj1.propTaskClass = TaskClassType.NotValid;
            break;
        }
      }
      str = "";
      string attribute20 = reader.GetAttribute("Type");
      if (attribute20 != null && attribute20.Length > 0)
      {
        switch (attribute20.ToLower())
        {
          case "acp10":
            baseObj1.propType = ModuleType.ACP10;
            break;
          case "addlib":
            baseObj1.propType = ModuleType.AddLib;
            break;
          case "avtlib":
            baseObj1.propType = ModuleType.AvtLib;
            break;
          case "bootmodule":
            baseObj1.propType = ModuleType.BootModule;
            break;
          case "comconfig":
            baseObj1.propType = ModuleType.ComConfig;
            break;
          case "comlib":
            baseObj1.propType = ModuleType.ComLib;
            break;
          case "config":
            baseObj1.propType = ModuleType.Config;
            break;
          case "contents":
            baseObj1.propType = ModuleType.Contents;
            break;
          case "contents2":
            baseObj1.propType = ModuleType.Contents2;
            break;
          case "data":
            baseObj1.propType = ModuleType.Data;
            break;
          case "datamodule":
            baseObj1.propType = ModuleType.DataModule;
            break;
          case "device":
            baseObj1.propType = ModuleType.Device;
            break;
          case "error":
            baseObj1.propType = ModuleType.Error;
            break;
          case "exception":
            baseObj1.propType = ModuleType.Exception;
            break;
          case "exceptiontask":
            baseObj1.propType = ModuleType.ExceptionTask;
            break;
          case "exe":
            baseObj1.propType = ModuleType.Exe;
            break;
          case "history":
            baseObj1.propType = ModuleType.History;
            break;
          case "hwlib":
            baseObj1.propType = ModuleType.HwLib;
            break;
          case "instlib":
            baseObj1.propType = ModuleType.InstLib;
            break;
          case "interrupt":
            baseObj1.propType = ModuleType.Interrupt;
            break;
          case "interrupttask":
            baseObj1.propType = ModuleType.InterruptTask;
            break;
          case "io":
            baseObj1.propType = ModuleType.Io;
            break;
          case "ioconfig":
            baseObj1.propType = ModuleType.IOConfig;
            break;
          case "iomap":
            baseObj1.propType = ModuleType.IoMap;
            break;
          case "lib":
            baseObj1.propType = ModuleType.Lib;
            break;
          case "logger":
            baseObj1.propType = ModuleType.Logger;
            break;
          case "mathlib":
            baseObj1.propType = ModuleType.MathLib;
            break;
          case "memoryextension":
            baseObj1.propType = ModuleType.MemoryExtension;
            break;
          case "merker":
            baseObj1.propType = ModuleType.Merker;
            break;
          case "ncdriver":
            baseObj1.propType = ModuleType.NcDriver;
            break;
          case "ncupdate":
            baseObj1.propType = ModuleType.NcUpdate;
            break;
          case "noc":
            baseObj1.propType = ModuleType.Noc;
            break;
          case "osexe":
            baseObj1.propType = ModuleType.OsExe;
            break;
          case "plcconfig":
            baseObj1.propType = ModuleType.PlcConfig;
            break;
          case "plctask":
            baseObj1.propType = ModuleType.PlcTask;
            break;
          case "ppconfig":
            baseObj1.propType = ModuleType.PpConfig;
            break;
          case "opcconfig":
            baseObj1.propType = ModuleType.OpcConfig;
            break;
          case "opcuaconfig":
            baseObj1.propType = ModuleType.OpcUaConfig;
            break;
          case "probe":
            baseObj1.propType = ModuleType.Probe;
            break;
          case "probeio":
            baseObj1.propType = ModuleType.ProbeIo;
            break;
          case "startup":
            baseObj1.propType = ModuleType.Startup;
            break;
          case "syslib":
            baseObj1.propType = ModuleType.SysLib;
            break;
          case "systemtask":
            baseObj1.propType = ModuleType.SystemTask;
            break;
          case "table":
            baseObj1.propType = ModuleType.Table;
            break;
          case "timertask":
            baseObj1.propType = ModuleType.TimerTask;
            break;
          case "tkloc":
            baseObj1.propType = ModuleType.TkLoc;
            break;
          case "tracerdata":
            baseObj1.propType = ModuleType.TracerData;
            break;
          case "tracerdefinition":
            baseObj1.propType = ModuleType.TracerDefinition;
            break;
          case "unknown":
            baseObj1.propType = ModuleType.Unknown;
            break;
          case "update":
            baseObj1.propType = ModuleType.Update;
            break;
          case "usertask":
            baseObj1.propType = ModuleType.UserTask;
            break;
        }
      }
      str = "";
      string attribute21 = reader.GetAttribute("ProgramState");
      if (attribute21 != null && attribute21.Length > 0)
      {
        switch (attribute21.ToLower())
        {
          case "idle":
            baseObj1.propPIState = ProgramState.Idle;
            break;
          case "nonexistent":
            baseObj1.propPIState = ProgramState.NonExistent;
            break;
          case "resetting":
            baseObj1.propPIState = ProgramState.Resetting;
            break;
          case "resuming":
            baseObj1.propPIState = ProgramState.Resuming;
            break;
          case "running":
            baseObj1.propPIState = ProgramState.Running;
            break;
          case "starting":
            baseObj1.propPIState = ProgramState.Starting;
            break;
          case "stopped":
            baseObj1.propPIState = ProgramState.Stopped;
            break;
          case "stopping":
            baseObj1.propPIState = ProgramState.Stopping;
            break;
          case "unrunnable":
            baseObj1.propPIState = ProgramState.Unrunnable;
            break;
        }
      }
      if (ConnectionStates.Connected == this.propConnectionState || ConnectionStates.ConnectedError == this.propConnectionState)
        baseObj1.Requests |= Actions.Connect;
      str = "";
      string attribute22 = reader.GetAttribute("InstallationPriority");
      if (attribute22 != null && attribute22.Length > 0)
      {
        byte result14 = 0;
        if (PviParse.TryParseByte(attribute22, out result14))
          baseObj1.propInstallationPriority = result14;
      }
      reader.Read();
      return 0;
    }

    internal override int ToXMLTextWriter(ref XmlTextWriter writer, ConfigurationFlags flags)
    {
      writer.WriteStartElement(nameof (Module));
      int xmlTextWriter = this.SaveModuleConfiguration(ref writer, flags);
      writer.WriteEndElement();
      return xmlTextWriter;
    }

    public int SaveModuleConfiguration(ref XmlTextWriter writer, ConfigurationFlags flags)
    {
      base.ToXMLTextWriter(ref writer, flags);
      if (this.propCreateName != null && this.propCreateName.Length > 0)
        writer.WriteAttributeString("CreationName", this.propCreateName.ToString());
      writer.WriteAttributeString("CreationTime", Pvi.UInt32ToDateTime(this.propCreateTime).ToString());
      if (this.propAndName != null && this.propAndName.Length > 0)
        writer.WriteAttributeString("LastWriteName", this.propAndName);
      writer.WriteAttributeString("LastWriteTime", Pvi.UInt32ToDateTime(this.propAndTime).ToString());
      if (this.propLength > 0U)
        writer.WriteAttributeString("Length", this.propLength.ToString());
      if (this.propDigitalMemoryAddess != 0U)
        writer.WriteAttributeString("DigitalMemoryAddress", this.propDigitalMemoryAddess.ToString());
      if (this.propAnalogMemoryAddress != 0U)
        writer.WriteAttributeString("AnalogMemoryAddress", this.propAnalogMemoryAddress.ToString());
      writer.WriteAttributeString("MemoryType", this.propMemoryLocation.ToString());
      if (this.propDMIndex != (ushort) 0)
        writer.WriteAttributeString("DomainIndex", this.propDMIndex.ToString());
      writer.WriteAttributeString("DomainState", this.propDMState.ToString());
      if (this.propInstallNumber != (byte) 0)
        writer.WriteAttributeString("InstallNumber", this.propInstallNumber.ToString());
      if (this.propModUID != 0U)
        writer.WriteAttributeString("ModUID", this.propModUID.ToString());
      if (this.propPICount != (byte) 0)
        writer.WriteAttributeString("PiCount", this.propPICount.ToString());
      if (this.propPIIndex != (ushort) 0)
        writer.WriteAttributeString("PiIndex", this.propPIIndex.ToString());
      if (this.propStartAddress != 0U)
        writer.WriteAttributeString("StartAddress", this.propStartAddress.ToString());
      if (this.propPVCount != (ushort) 0)
        writer.WriteAttributeString("VariableCount", this.propPVCount.ToString());
      if (this.propPVIndex != (ushort) 0)
        writer.WriteAttributeString("VariableIndex", this.propPVIndex.ToString());
      writer.WriteAttributeString("Version", this.propVersion.ToString());
      writer.WriteAttributeString("TaskClassType", this.propTaskClass.ToString());
      writer.WriteAttributeString("Type", this.propType.ToString());
      if (this.ProgramState != ProgramState.Running)
        writer.WriteAttributeString("ProgramState", this.ProgramState.ToString());
      if (this.propInstallationPriority != (byte) 0)
        writer.WriteAttributeString("InstallationPriority", this.propInstallNumber.ToString());
      return 0;
    }

    public short DomainIndex => (short) this.propDMIndex < (short) 0 ? (short) 0 : (short) this.propDMIndex;

    [Obsolete("This property is no longer supported by ANSL!(Only valid for INA2000)")]
    internal short PiIndex => (short) this.propPIIndex < (short) 0 ? (short) 0 : (short) this.propPIIndex;

    public DomainState DomainState => this.propDMState;

    public ProgramState ProgramState => this.propPIState;

    internal byte PiCount => this.propPICount;

    [Obsolete("This property is no longer supported by ANSL!(Only valid for INA2000)")]
    [CLSCompliant(false)]
    public uint StartAddress => this.propStartAddress < 0U ? 0U : this.propStartAddress;

    [CLSCompliant(false)]
    public uint Length => this.propLength < 0U ? 0U : this.propLength;

    public short Version => System.Convert.ToInt16(((int) this.propVersion >> 4 & 15) * 1000 + ((int) this.propVersion & 15) * 100 + ((int) this.propRevision >> 4 & 15) * 10 + ((int) this.propRevision & 15));

    public string VersionText => string.Format("{0}.{1}{2}.{3}", (object) System.Convert.ToString((int) this.propVersion >> 4 & 15), (object) System.Convert.ToString((int) this.propVersion & 15), (object) System.Convert.ToString((int) this.propRevision >> 4 & 15), (object) System.Convert.ToString((int) this.propRevision & 15));

    public DateTime CreationTime => Pvi.UInt32ToDateTime(this.propCreateTime);

    public DateTime LastWriteTime => Pvi.UInt32ToDateTime(this.propAndTime);

    [Obsolete("This property is no longer supported by ANSL!(Only valid for INA2000)")]
    public string CreationName => this.propCreateName;

    [Obsolete("This property is no longer supported by ANSL!(Only valid for INA2000)")]
    public string LastWriteName => this.propAndName;

    public TaskClassType TaskClassType => this.propTaskClass;

    public byte InstallNumber => this.propInstallNumber;

    public byte InstallationPriority => this.propInstallationPriority;

    public bool InstallationPriorityValid => this.propInstPriorityValid;

    [Obsolete("This property is no longer supported by ANSL!(Only valid for INA2000)")]
    internal short VariableIndex => (short) this.propPVIndex < (short) 0 ? (short) 0 : (short) this.propPVIndex;

    [Obsolete("This property is no longer supported by ANSL!(Only valid for INA2000)")]
    public short VariableCount => (short) this.propPVCount < (short) 0 ? (short) 0 : (short) this.propPVCount;

    [Obsolete("This property is no longer supported by ANSL!(Only valid for INA2000)")]
    [CLSCompliant(false)]
    public uint AnalogMemoryAddress => this.propAnalogMemoryAddress < 0U ? 0U : this.propAnalogMemoryAddress;

    [CLSCompliant(false)]
    [Obsolete("This property is no longer supported by ANSL!(Only valid for INA2000)")]
    public uint DigitalMemoryAddress => this.propDigitalMemoryAddess < 0U ? 0U : this.propDigitalMemoryAddess;

    public MemoryType MemoryType => this.propMemoryLocation;

    internal uint ModUID => this.propModUID;

    public ModuleType Type => this.propType;

    public Cpu Cpu => this.propCpu;

    public override string FullName => this.Name.Length > 0 ? (this.Parent != null ? this.Parent.FullName + "." + this.Name : this.Name) : (this.Parent != null ? this.Parent.FullName : "");

    public override string PviPathName => this.Parent != null ? (this.Name != null && 0 < this.Name.Length ? this.Parent.PviPathName + "/\"" + this.propName + "\" OT=Module" : this.Parent.PviPathName) : (this.Name != null && 0 < this.Name.Length ? "/\"" + this.propName + "\" OT=Module" : this.propName);

    public string FileName
    {
      get => this.propFileName;
      set => this.propFileName = value;
    }

    internal PviObjectBrowser ObjectBrowser
    {
      get => this.propObjectBrowser;
      set => this.propObjectBrowser = value;
    }

    public event PviEventHandler Stopped;

    public event PviEventHandler Started;

    public event PviEventHandler Deleted;

    public event PviEventHandler Uploaded;

    public event PviEventHandler Downloaded;

    public event PviEventHandler Cancelled;

    public event ModuleEventHandler UploadProgress;

    public event ModuleEventHandler DownloadProgress;

    public event PviEventHandler RunCycleCountSet;

    internal void OnProceeding(IntPtr ptrData, int dataLen, int errorCode)
    {
      ProgressInfo progressInfoStructure = PviMarshal.PtrToProgressInfoStructure(ptrData, typeof (ProgressInfo));
      ModuleEventArgs e = new ModuleEventArgs(this.propName, this.propAddress, errorCode, this.Service.Language, Action.ModuleProgressEvent, this, progressInfoStructure.Percent);
      if (this.propLastAction == Actions.Upload)
        this.OnUploadProgress(e);
      else
        this.OnDownloadProgress(e);
    }

    [CLSCompliant(false)]
    protected void ANSLModuleDescriptionRead(IntPtr pData, uint dataLen, int errorCode)
    {
      ModuleInfoDecription moduleInfo = new ModuleInfoDecription();
      int errorCode1 = 12055;
      try
      {
        if (dataLen == 0U || errorCode != 0)
        {
          if (errorCode != 0)
            errorCode1 = errorCode;
          this.OnError(new PviEventArgs(this.Name, this.Address, errorCode1, this.Service.Language, Action.ModuleInfo, this.Service));
        }
        else
        {
          byte[] numArray = new byte[(IntPtr) dataLen];
          Marshal.Copy(pData, numArray, 0, (int) dataLen);
          XmlTextReader xmlTReader = new XmlTextReader((Stream) new MemoryStream(numArray));
          int content = (int) xmlTReader.MoveToContent();
          while (!xmlTReader.EOF && xmlTReader.NodeType != XmlNodeType.EndElement)
          {
            if (xmlTReader.Name.CompareTo("TaskInfo") == 0 || xmlTReader.Name.CompareTo("ModulInfo") == 0 || xmlTReader.Name.CompareTo("ModInfo") == 0)
            {
              this.propModuleInfoRequested = false;
              int errorCode2 = moduleInfo.ReadFromXML(xmlTReader);
              if (moduleInfo.name != null && (moduleInfo.name.CompareTo(this.Address) == 0 || moduleInfo.name.CompareTo(this.Name) == 0))
              {
                this.updateProperties((object) moduleInfo);
                if (errorCode2 != 0)
                  this.OnPropertyChanged(new PviEventArgs(this.Name, this.Address, errorCode2, this.Service.Language, Action.ModuleChangedEvent));
              }
              this.propModuleInfoRequested = false;
            }
            xmlTReader.Read();
          }
        }
      }
      catch
      {
      }
    }
  }
}
