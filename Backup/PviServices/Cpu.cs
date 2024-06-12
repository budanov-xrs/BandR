// Decompiled with JetBrains decompiler
// Type: BR.AN.PviServices.Cpu
// Assembly: BR.AN.PVIServices, Version=1.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DA853F0A-C070-4E30-B7A7-51D955FB85F6
// Assembly location: C:\Users\ss_six\Desktop\FXE\FXE software integration package\COMET_FXE-PVIServices_Example\Pluto-PVI-trial_01\packages\IToolS.BandR.3.5.58\lib\net20\BR.AN.PVIServices.dll

using System;
using System.Collections;
using System.ComponentModel;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Xml;

namespace BR.AN.PviServices
{
  [CLSCompliant(true)]
  public class Cpu : Base
  {
    public const string KW_DETECT_SYSLOGBOOK_NAME = "$Detect_SG4_SysLogger$";
    private bool propFireConChanged;
    private bool propHasARLogSysErrorEVH;
    internal string actDownLoadModuleName;
    internal string actUpLoadModuleName;
    internal ArrayList listOfDownLoadModules;
    internal ArrayList listOfUpLoadModules;
    private bool ignoreEvents;
    private string activeConnSettings;
    private Logger propARLogSys;
    private uint propErrorLogBookModUID;
    private ModuleListOptions m_ModuListULOption;
    private string propApplicationModuleFilter;
    internal bool propNewConnection;
    private BR.AN.PviServices.HardwareInfo propHardwareInfos;
    private BatteryStates propAccu;
    private BatteryStates propBattery;
    internal bool propIsSG4Target;
    private bool propRestarted;
    private TaskClassCollection propTaskClasses;
    private MemoryCollection propMemories;
    private VariableCollection propVariables;
    private TaskCollection propTasks;
    private ModuleCollection propModules;
    private LibraryCollection propLibraries;
    private IODataPointCollection propIODataPoints;
    private Connection propConnection;
    private string propSWVersion;
    private string propCPUName;
    private string propCPUType;
    private string propAWSType;
    private ushort propNodeNumber;
    private BootMode propInitDescription;
    private CpuState propState;
    private byte propVoltage;
    private byte propCpuUsage;
    private string propHost;
    private DateTime propDateTime;
    private string propSavePath;
    private TcpDestinationSettings propTCPDestinationSettings;
    private LoggerCollection propLoggers;
    internal ConnectionType propCPUState;
    internal Hashtable propUserCollections;
    internal Hashtable propUserModuleCollections;
    internal Hashtable propUserTaskCollections;
    internal Hashtable propUserLoggerCollections;
    internal Profiler propProfiler;
    private bool propModuleInfoRequested;
    internal Hashtable propModuleInfoList;
    private PviObjectBrowser propObjectBrowser;
    private EventWaitHandle hWaitOnDongles;
    private EventWaitHandle hWaitOnListOfExistingLicenses;
    private EventWaitHandle hWaitOnListOfRequiredLicenses;
    private EventWaitHandle hWaitOnReadContext;
    private EventWaitHandle hWaitOnUpdateLicense;
    private EventWaitHandle hWaitOnLicenseStatus;
    private bool licListDongles;
    private bool licListOfExistingLicenses;
    private bool licListOfRequiredLicenses;
    private bool licReadContext;
    private bool licUpdateLicense;
    private bool licBlinkDongle;
    private PviFunction cbLICReadFunc;
    private PviFunction cbLICWriteFunc;
    private string propLicStatus;
    private uint propLicStatusError;
    private MemoryType propClearmemType;
    private string propMemoryInfo;
    private MemoryInformation propMemoryInformationStruct;
    private string propHardwareInfo;
    private HardwareInformation propHardwareInformationStruct;
    private bool propRedundancyCommMode;
    private string propRedundancyInfo;
    private readonly object propRedundancyInfoLock = new object();
    private RedundancyInformation propRedundancyInformationStruct;
    private readonly object propRedundancyInformationStructLock = new object();

    public Cpu(Service service, string name)
      : base((Base) service, name)
    {
      this.propFireConChanged = false;
      this.propHasARLogSysErrorEVH = false;
      this.propARLogSys = (Logger) null;
      this.propIsSG4Target = true;
      this.propObjectBrowser = (PviObjectBrowser) null;
      this.Init(service, name);
    }

    internal Cpu(PviObjectBrowser objBrowser, string name)
      : base((Base) objBrowser.Service, name)
    {
      this.propFireConChanged = false;
      this.propHasARLogSysErrorEVH = false;
      this.propObjectBrowser = objBrowser;
      this.propARLogSys = (Logger) null;
      this.propIsSG4Target = true;
      this.Init(this.propObjectBrowser.Service, name);
    }

    internal void Init(Service service, string name)
    {
      this.m_ModuListULOption = ModuleListOptions.INA2000CompatibleMode;
      this.propLicStatusError = uint.MaxValue;
      this.propLicStatus = "";
      this.propRedundancyInfo = "";
      this.propRedundancyInformationStruct = (RedundancyInformation) null;
      this.propRedundancyCommMode = false;
      this.activeConnSettings = "";
      this.propTCPDestinationSettings = new TcpDestinationSettings();
      this.propHardwareInfos = new BR.AN.PviServices.HardwareInfo(this);
      this.propConnectionState = ConnectionStates.Unininitialized;
      this.propState = CpuState.Offline;
      this.propAccu = BatteryStates.UNDEFINED;
      this.propBattery = BatteryStates.UNDEFINED;
      this.propMemories = new MemoryCollection(this, name + ".Memories");
      this.propTaskClasses = new TaskClassCollection((Base) this, name);
      this.propVariables = new VariableCollection(service.CollectionType, (object) this, name + ".Variables");
      this.propVariables.propInternalCollection = true;
      this.propModules = new ModuleCollection(CollectionType.HashTable, (object) this, name + ".Modules");
      this.propModules.propInternalCollection = true;
      this.propTasks = new TaskCollection(service.CollectionType, (object) this, name + ".Tasks");
      this.propTasks.propInternalCollection = true;
      this.propIODataPoints = new IODataPointCollection((Base) this, name + ".IODatapoints");
      this.propLoggers = new LoggerCollection(CollectionType.ArrayList, (object) this, this.Name + ".Loggers");
      this.propLibraries = new LibraryCollection(this, name + ".Libraries");
      this.propConnection = new Connection(this);
      this.propConnection.Connected += new PviEventHandler(this.ConnectionEvent);
      this.propConnection.ConnectionChanged += new PviEventHandler(this.connection_ConnectionChanged);
      this.propConnection.Disconnected += new PviEventHandler(this.ConnectionDisconnected);
      this.propConnection.Error += new PviEventHandler(this.ConnectionEvent);
      this.propRestarted = false;
      this.propCPUState = ConnectionType.None;
      service.Cpus.Add(this);
      switch (this.Service.LogicalObjectsUsage)
      {
        case LogicalObjectsUsage.FullName:
          this.Service.AddLogicalObject(this.FullName, (object) this);
          break;
        case LogicalObjectsUsage.ObjectName:
          this.Service.AddLogicalObject(name, (object) this);
          break;
        case LogicalObjectsUsage.ObjectNameWithType:
          this.Service.AddLogicalObject(this.PviPathName, (object) this);
          break;
      }
      this.propErrorLogBookModUID = this.Service.ModuleUID;
      this.hWaitOnDongles = new EventWaitHandle(false, EventResetMode.AutoReset);
      this.hWaitOnListOfExistingLicenses = new EventWaitHandle(false, EventResetMode.AutoReset);
      this.hWaitOnListOfRequiredLicenses = new EventWaitHandle(false, EventResetMode.AutoReset);
      this.hWaitOnReadContext = new EventWaitHandle(false, EventResetMode.AutoReset);
      this.hWaitOnUpdateLicense = new EventWaitHandle(false, EventResetMode.AutoReset);
      this.hWaitOnLicenseStatus = new EventWaitHandle(false, EventResetMode.AutoReset);
      this.licListDongles = false;
      this.licListOfExistingLicenses = false;
      this.licListOfRequiredLicenses = false;
      this.licReadContext = false;
      this.licUpdateLicense = false;
      this.licBlinkDongle = false;
      this.cbLICReadFunc = new PviFunction(this.PVICB_LIC_ReadFunc);
      this.cbLICWriteFunc = new PviFunction(this.PVICB_LIC_WriteFunc);
    }

    ~Cpu() => this.Dispose(false, true);

    internal override void reCreateState()
    {
      this.propConnectionState = ConnectionStates.Unininitialized;
      this.propReCreateActive = true;
      this.propLinkId = 0U;
      this.Connection.ResetLinkIds();
      if (!this.reCreateActive)
        return;
      this.reCreateActive = false;
      this.Connect();
      if (this.Service.WaitForParentConnection)
        return;
      this.reCreateChildState();
    }

    internal void reCreateChildState()
    {
      if (this.propTasks.Values != null)
      {
        foreach (Task task in (IEnumerable) this.propTasks.Values)
        {
          if (task.isObjectConnected || task.reCreateActive)
          {
            task.reCreateState();
            if (!this.Service.WaitForParentConnection)
              task.reCreateChildState();
          }
        }
      }
      if (this.propLoggers.Values != null)
      {
        foreach (Logger logger in (IEnumerable) this.propLoggers.Values)
        {
          if (logger.isObjectConnected)
            logger.reCreateState();
        }
      }
      if (this.propModules.Values != null)
      {
        foreach (Module module in (IEnumerable) this.propModules.Values)
        {
          if (module.isObjectConnected)
            module.reCreateState();
        }
      }
      if (this.propLibraries.Values != null)
      {
        foreach (Library library in (IEnumerable) this.propLibraries.Values)
        {
          if (library.isObjectConnected)
            library.reCreateState();
        }
      }
      if (this.propVariables.Values != null)
      {
        foreach (Variable variable in (IEnumerable) this.propVariables.Values)
        {
          if (variable.isObjectConnected || variable.reCreateActive)
            variable.reCreateState();
        }
      }
      if (this.propIODataPoints.Values == null)
        return;
      foreach (IODataPoint ioDataPoint in (IEnumerable) this.propIODataPoints.Values)
      {
        if (ioDataPoint.isObjectConnected || ioDataPoint.reCreateActive)
          ioDataPoint.reCreateState();
      }
    }

    public int ReadCommunicationLibraryVersions(ref Hashtable versionInfos)
    {
      int num1 = 4096;
      versionInfos.Clear();
      IntPtr num2 = PviMarshal.AllocCoTaskMem(num1);
      int num3 = PInvokePvicom.PviComRead(this.Service.hPvi, this.propConnection.pviLineObj.LinkId, AccessTypes.Version, IntPtr.Zero, 0, num2, num1);
      if (num3 == 0)
      {
        PviMarshal.GetVersionInfos(num2, num1, ref versionInfos);
        num3 = PInvokePvicom.PviComRead(this.Service.hPvi, this.propConnection.pviDeviceObj.LinkId, AccessTypes.Version, IntPtr.Zero, 0, num2, num1);
      }
      if (num3 == 0)
        PviMarshal.GetVersionInfos(num2, num1, ref versionInfos);
      return num3;
    }

    public override bool IsConnected => ConnectionStates.Connected == this.propConnectionState;

    public override void Connect()
    {
      this.ignoreEvents = false;
      this.propNoDisconnectedEvent = false;
      this.propReturnValue = 0;
      this.Connect(this.ConnectionType);
    }

    internal string RemoveULorDLName(ref ArrayList modQueue, string modName)
    {
      if (0 < modQueue.Count)
      {
        if (modName == modQueue[0].ToString())
          modQueue.RemoveAt(0);
        if (0 < modQueue.Count)
          return modQueue[0].ToString();
      }
      return "";
    }

    public override void Connect(ConnectionType connectionType)
    {
      this.actDownLoadModuleName = "";
      this.actUpLoadModuleName = "";
      this.listOfDownLoadModules = new ArrayList();
      this.listOfUpLoadModules = new ArrayList();
      this.ignoreEvents = false;
      if (this.reCreateActive || ConnectionStates.Unininitialized < this.propConnectionState && ConnectionStates.Disconnecting > this.propConnectionState)
      {
        if (!this.HasPVIConnection || this.reCreateActive)
          return;
        this.Fire_ConnectedEvent((object) this, new PviEventArgs(this.propName, this.propAddress, this.propErrorCode, this.Service.Language, Action.CpuConnect, this.Service));
      }
      else
      {
        this.propConnectionState = ConnectionStates.Connecting;
        if (this.LinkId != 0U)
        {
          if (this.propErrorCode == 0)
            this.propErrorCode = this.propReturnValue = 12043;
          else
            this.propReturnValue = this.propErrorCode;
          this.OnError(new PviEventArgs(this.propName, this.propAddress, this.propErrorCode, this.Service.Language, Action.CpuConnect, this.Service));
          base.OnConnected(new PviEventArgs(this.propName, this.propAddress, this.propErrorCode, this.Service.Language, Action.CpuConnect, this.Service));
        }
        else
        {
          if (this.propModuleInfoList != null)
            this.propModuleInfoList.Clear();
          this.RedundancyInfo = "";
          this.RedundancyInformationStruct = (RedundancyInformation) null;
          this.propNoDisconnectedEvent = false;
          this.Requests |= Actions.Connect;
          this.Requests |= Actions.GetCpuInfo;
          this.propReturnValue = 0;
          this.ConnectionType = connectionType;
          if (this.propAddress == null || this.propAddress.Length == 0)
            this.propAddress = this.propName;
          if (this.ConnectionType != ConnectionType.Link)
          {
            this.Requests |= Actions.GetLBType;
            if (ConnectionStates.Connecting > this.propConnection.propConnectionState || ConnectionStates.Connecting > this.propConnectionState || ConnectionStates.Disconnected == this.propConnectionState || ConnectionStates.Disconnected == this.propConnection.propConnectionState)
            {
              this.propConnection.propConnectionState = ConnectionStates.Unininitialized;
              this.propConnection.Connect();
              this.propReturnValue = this.propConnection.ReturnValue;
            }
          }
          else
          {
            if (Actions.GetLBType == (this.Requests & Actions.GetLBType))
              this.Requests &= Actions.Connect | Actions.GetList | Actions.GetValue | Actions.SetValue | Actions.GetForce | Actions.SetForce | Actions.Start | Actions.Stop | Actions.RunCycle | Actions.GetType | Actions.SetType | Actions.GetLength | Actions.SetRefresh | Actions.CreateLink | Actions.Upload | Actions.Download | Actions.SetActive | Actions.SetHysteresis | Actions.Disconnect | Actions.SetInitValue | Actions.ModuleInfo | Actions.ListSNMPVariables | Actions.Connected | Actions.Uploading | Actions.GetCpuInfo | Actions.ReadPVFormat | Actions.FireActivated | Actions.ReadIndex | Actions.FireDataValidated | Actions.FireValueChanged | Actions.Link;
            this.ConnectionEvent((object) this, new PviEventArgs(this.Name, this.Address, 0, this.Service.Language, Action.CpuConnect, this.Service));
          }
          if (this.propCPUState == ConnectionType.Create)
            this.ConnectionEvent((object) this, new PviEventArgs(this.Name, this.Address, 0, this.Service.Language, Action.CpuConnect, this.Service));
          if (this.Service.WaitForParentConnection)
            return;
          this.propReturnValue = this.ConnectCpu(new PviEventArgs(this.Name, this.Address, 0, this.Service.Language, Action.CpuConnect, this.Service));
        }
      }
    }

    public override int ChangeConnection()
    {
      IntPtr zero = IntPtr.Zero;
      int num1 = 0;
      if (0 < this.activeConnSettings.Length && this.activeConnSettings.CompareTo(this.Connection.ToString()) == 0)
        return 4804;
      this.activeConnSettings = this.Connection.ToString();
      if (this.propModuleInfoList != null)
        this.propModuleInfoList.Clear();
      this.RedundancyInfo = "";
      this.RedundancyInformationStruct = (RedundancyInformation) null;
      num1 = this.CpuEventsOFF();
      this.Requests |= Actions.GetLBType;
      int num2 = this.Connection.ChangeConnection();
      this.propFireConChanged = true;
      return num2;
    }

    private int CpuEventsOFF()
    {
      int num = 0;
      string request = "";
      this.ignoreEvents = true;
      this.Service.BuildRequestBuffer(request);
      num = this.Write(this.Service.hPvi, this.LinkId, AccessTypes.EventMask, this.Service.RequestBuffer, request.Length);
      return this.Connection.TurnOffEvents();
    }

    private int CpuEventsON()
    {
      int num = 0;
      string request = "edlfp";
      this.ignoreEvents = false;
      this.Service.BuildRequestBuffer(request);
      num = this.WriteRequest(this.Service.hPvi, this.LinkId, AccessTypes.EventMask, this.Service.RequestBuffer, request.Length, this.propInternID);
      return this.Connection.TurnOnEvents();
    }

    private int ChangeCpuConnection()
    {
      string strValue = this.GetConnectionDescription();
      int num = strValue.IndexOf("\"/\"");
      if (-1 != num)
        strValue = strValue.Substring(num + 2);
      this.SysLoggerDisconnect();
      return this.WriteRequest(this.Service.hPvi, this.LinkId, AccessTypes.Connect, strValue, 217U, this.propInternID);
    }

    public override void Disconnect()
    {
      this.reCreateActive = false;
      int errorCode = this.Disconnect(202U);
      if (errorCode == 0 || this.propNoDisconnectedEvent)
        return;
      this.OnError(new PviEventArgs(this.propName, this.propAddress, errorCode, this.Service.Language, Action.CpuDisconnect));
      this.FireDisconnected(errorCode, Action.CpuDisconnect);
    }

    internal void DisconnectWithChilds()
    {
      if (this.IODataPoints != null)
        this.IODataPoints.Disconnect(true);
      if (this.Loggers != null)
        this.Loggers.Disconnect(true);
      if (this.Modules != null)
        this.Modules.Disconnect(true);
      if (this.Tasks != null)
        this.Tasks.Disconnect(true);
      if (this.Variables != null)
        this.Variables.Disconnect(true);
      this.SysLoggerDisconnect();
      base.Disconnect(true);
      if (this.Connection != null)
        this.Connection.DisconnectNoResponses();
      this.propConnectionState = ConnectionStates.Disconnected;
      this.Requests = Actions.NONE;
    }

    public override void Disconnect(bool noResponse)
    {
      this.SysLoggerDisconnect();
      this.propFireConChanged = false;
      if (noResponse)
      {
        if (ConnectionStates.Connected == this.propConnectionState || ConnectionStates.ConnectedError == this.propConnectionState || this.LinkId != 0U)
        {
          base.Disconnect(true);
          this.Connection.DisconnectNoResponses();
          this.propConnectionState = ConnectionStates.Disconnected;
          this.Requests = Actions.NONE;
        }
        else
        {
          if (ConnectionStates.Connecting != this.propConnectionState)
            return;
          this.propConnectionState = ConnectionStates.Disconnecting;
          this.Requests |= Actions.Disconnect;
        }
      }
      else
        this.Disconnect();
    }

    public void DisconnectChildObjects()
    {
      if (this.propIODataPoints != null && 0 < this.propIODataPoints.Count)
      {
        foreach (Base @base in (IEnumerable) this.propIODataPoints.Values)
          @base.Disconnect(true);
      }
      if (this.propVariables != null && 0 < this.propVariables.Count)
      {
        foreach (Base @base in (IEnumerable) this.propVariables.Values)
          @base.Disconnect(true);
      }
      if (this.propLoggers != null && 0 < this.propLoggers.Count)
      {
        foreach (Base @base in (IEnumerable) this.propLoggers.Values)
          @base.Disconnect(true);
      }
      if (this.propTasks != null && 0 < this.propTasks.Count)
      {
        foreach (Task task in (IEnumerable) this.propTasks.Values)
        {
          if (task.propVariables != null && 0 < task.propVariables.Count)
          {
            foreach (Base @base in (IEnumerable) task.propVariables.Values)
              @base.Disconnect(true);
          }
          task.Disconnect(true);
        }
      }
      if (this.propLibraries != null && 0 < this.propLibraries.Count)
      {
        foreach (Base @base in (IEnumerable) this.propLibraries.Values)
          @base.Disconnect(true);
      }
      if (this.propModules == null || 0 >= this.propModules.Count || this.propModules.Values == null)
        return;
      foreach (Base @base in (IEnumerable) this.propModules.Values)
        @base.Disconnect(true);
    }

    internal override int DisconnectRet(uint action) => this.Disconnect(action);

    private void SysLoggerDisconnect()
    {
      if (this.propARLogSys == null)
        return;
      this.propARLogSys.Disconnect(true);
      this.propARLogSys.Dispose(true, false);
      this.propARLogSys = (Logger) null;
    }

    internal int Disconnect(uint internalAction)
    {
      int num = 0;
      this.propReturnValue = 0;
      this.propFireConChanged = false;
      this.SysLoggerDisconnect();
      if (ConnectionStates.Disconnected == this.propConnectionState)
      {
        this.OnDisconnected(new PviEventArgs(this.propName, this.propAddress, 4808, this.Service.Language, Action.CpuDisconnect));
        return 0;
      }
      if (ConnectionStates.Connected < this.propConnectionState || this.propConnectionState == ConnectionStates.Unininitialized)
        return 4808;
      this.propConnectionState = ConnectionStates.Disconnecting;
      if (this.propDisposed)
        return num;
      int errorCode;
      if (this.Requests != Actions.NONE)
      {
        this.Requests = Actions.Disconnect;
        this.CancelRequest();
        errorCode = this.DisconnectCpuObjects(internalAction);
        if (errorCode != 0)
          this.OnDisconnected(new PviEventArgs(this.propName, this.propAddress, errorCode, this.Service.Language, Action.CpuDisconnect));
      }
      else
      {
        errorCode = this.DisconnectCpuObjects(internalAction);
        if (errorCode != 0)
          this.OnDisconnected(new PviEventArgs(this.propName, this.propAddress, errorCode, this.Service.Language, Action.CpuDisconnect));
      }
      this.Requests = Actions.NONE;
      return errorCode;
    }

    private int DisconnectCpuObjects(uint internalAction)
    {
      int num = 0;
      if (this.propLinkId != 0U)
      {
        this.propReturnValue = num = this.UnlinkRequest(internalAction);
        this.propLinkId = 0U;
      }
      else
        this.OnDisconnected(new PviEventArgs(this.propName, this.propAddress, 4808, this.Service.Language, Action.CpuDisconnect));
      return num;
    }

    private void DisconnectChilds()
    {
      foreach (Base propVariable in (BaseCollection) this.propVariables)
        propVariable.Disconnect();
      foreach (Base propLogger in (BaseCollection) this.propLoggers)
        propLogger.Disconnect();
      foreach (Base propTask in (BaseCollection) this.propTasks)
        propTask.Disconnect();
      foreach (Base propLibrary in (BaseCollection) this.propLibraries)
        propLibrary.Disconnect();
      if (this.propModules.Values == null)
        return;
      foreach (Base @base in (IEnumerable) this.propModules.Values)
        @base.Disconnect();
    }

    protected override string GetConnectionDescription()
    {
      string str1 = "";
      string str2 = "";
      string str3 = "";
      if (this.propSavePath != null && 0 < this.propSavePath.Length)
      {
        str1 = "/SP=" + this.propSavePath;
        if (0 < this.Connection.ConnectionParameter.Length)
          str1 = " /SP=" + this.propSavePath;
      }
      else if (this.Connection.Device.SavePath != null && 0 < this.Connection.Device.SavePath.Length)
      {
        str1 = "/SP=" + this.Connection.Device.SavePath;
        if (0 < this.Connection.ConnectionParameter.Length)
          str1 = " /SP=" + this.Connection.Device.SavePath;
      }
      if (this.propApplicationModuleFilter != null)
      {
        str2 = "/AM=" + this.propApplicationModuleFilter;
        if (0 < str1.Length || 0 < this.Connection.ConnectionParameter.Length)
          str2 = " /AM=" + this.propApplicationModuleFilter;
      }
      if (DeviceType.ANSLTcp == this.propConnection.DeviceType && (this.propRedundancyCommMode || this.Connection.ANSLTcp.RedundancyCommMode))
      {
        this.propRedundancyCommMode = true;
        str3 = "/RED=1";
        if (0 < str2.Length || 0 < this.Connection.ConnectionParameter.Length)
          str2 = " /RED=1";
      }
      string connectionDescription;
      if (0 < this.Connection.ConnectionParameter.Length)
      {
        if (LogicalObjectsUsage.ObjectNameWithType == this.Service.LogicalObjectsUsage)
          connectionDescription = string.Format("\"{0}{1}{2}{3}\"", (object) this.Connection.ConnectionParameter.Trim(), (object) str1, (object) str2, (object) str3);
        else
          connectionDescription = string.Format("\"{0}\"/\"{1}{2}{3}{4}\"", (object) this.propConnection.pviStationObj.Name, (object) this.Connection.ConnectionParameter, (object) str1, (object) str2, (object) str3);
      }
      else if (LogicalObjectsUsage.ObjectNameWithType == this.Service.LogicalObjectsUsage)
      {
        connectionDescription = string.Format("\"{0}{1}{2}\"", (object) str1, (object) str2, (object) str3);
        if (str1.Length == 0 && str2.Length == 0 && str3.Length == 0)
          connectionDescription = string.Format("\"\"");
      }
      else
      {
        connectionDescription = string.Format("\"{0}\"/\"{1}{2}{3}\"", (object) this.propConnection.pviStationObj.Name, (object) str1, (object) str2, (object) str3);
        if (str1.Length == 0 && str2.Length == 0 && str3.Length == 0)
          connectionDescription = string.Format("\"{0}\"/", (object) this.propConnection.pviStationObj.Name);
      }
      return connectionDescription;
    }

    private int ConnectCpu(PviEventArgs e)
    {
      this.propLinkParam = "EV=edlfp";
      int error = 0;
      this.ignoreEvents = false;
      this.propConnectionState = ConnectionStates.Connecting;
      this.propObjectParam = "CD=" + this.GetConnectionDescription();
      this.activeConnSettings = this.Connection.ToString();
      if (!this.Service.IsStatic && this.ConnectionType == ConnectionType.CreateAndLink)
      {
        if (this.Connection.DeviceType < DeviceType.TcpIpMODBUS)
        {
          if (this.ConnectionType == ConnectionType.CreateAndLink)
            error = this.XCreateRequest(this.Service.hPvi, this.LinkName, ObjectType.POBJ_CPU, this.propObjectParam, 703U, this.propLinkParam, 201U);
          else if (this.ConnectionType != ConnectionType.Link && this.propCPUState != ConnectionType.Create)
            error = this.XCreateRequest(this.Service.hPvi, this.LinkName, ObjectType.POBJ_CPU, this.propObjectParam, 0U, "", 201U);
        }
        else if (!this.Service.IsStatic && this.ConnectionType == ConnectionType.CreateAndLink)
          error = this.XCreateRequest(this.Service.hPvi, this.Connection.pviStationObj.Name, ObjectType.POBJ_STATION, this.Connection.pviStationObj.ObjectParam, 201U, this.propLinkParam, 201U);
        else if (this.ConnectionType != ConnectionType.Link && this.propCPUState != ConnectionType.Create)
          error = this.XCreateRequest(this.Service.hPvi, this.Connection.pviStationObj.Name, ObjectType.POBJ_STATION, this.Connection.pviStationObj.ObjectParam, 0U, "", 0U);
      }
      else if (this.Service.IsStatic)
      {
        if (this.Connection.DeviceType < DeviceType.TcpIpMODBUS)
        {
          if (this.ConnectionType == ConnectionType.CreateAndLink)
            error = this.XCreateRequest(this.Service.hPvi, this.LinkName, ObjectType.POBJ_CPU, this.propObjectParam, 201U, this.propLinkParam, 201U);
          else if (this.ConnectionType != ConnectionType.Link && this.propCPUState != ConnectionType.Create)
            error = this.XCreateRequest(this.Service.hPvi, this.LinkName, ObjectType.POBJ_CPU, this.propObjectParam, 0U, "", 201U);
        }
        else
        {
          this.Connection.pviStationObj.Initialize(this.Connection.pviDeviceObj.Name, string.Format("\"{0}\"", (object) this.Connection.ConnectionParameter));
          if (!this.Service.IsStatic && this.ConnectionType == ConnectionType.CreateAndLink)
            error = this.XCreateRequest(this.Service.hPvi, this.Connection.pviStationObj.Name, ObjectType.POBJ_STATION, this.Connection.pviStationObj.ObjectParam, 201U, this.propLinkParam, 201U);
          else if (this.ConnectionType != ConnectionType.Link && this.propCPUState != ConnectionType.Create)
            error = this.XCreateRequest(this.Service.hPvi, this.Connection.pviStationObj.Name, ObjectType.POBJ_STATION, this.Connection.pviStationObj.ObjectParam, 0U, "", 201U);
        }
      }
      else
        error = !this.Service.IsStatic || this.ConnectionType == ConnectionType.Link ? this.XLinkRequest(this.Service.hPvi, this.LinkName, 703U, this.propLinkParam, 704U) : this.XLinkRequest(this.Service.hPvi, this.LinkName, 703U, this.propLinkParam, 4294967294U, 704U);
      if (error != 0)
      {
        this.propConnectionState = ConnectionStates.Unininitialized;
        if (this.Service.ErrorException)
          throw new PviException(string.Format("Connection Error: {0}", (object) error.ToString()), error, (object) this, e);
        if (this.Service.ErrorEvent)
        {
          e.propErrorCode = error;
          this.OnError(e);
        }
      }
      return error;
    }

    private void connection_ConnectionChanged(object sender, PviEventArgs e)
    {
      if (e.ErrorCode != 0)
        this.OnError(e);
      this.ChangeCpuConnection();
    }

    private void ConnectionEvent(object sender, PviEventArgs e)
    {
      if (e.ErrorCode != 0)
      {
        this.OnError(e);
        this.OnConnected(e);
      }
      else
      {
        if (!this.Service.WaitForParentConnection)
          return;
        this.ConnectCpu(e);
      }
    }

    internal override void OnPviCreated(int errorCode, uint linkID)
    {
      this.propLinkId = linkID;
      if (errorCode == 0 || 12002 == errorCode)
      {
        this.Requests &= Actions.GetList | Actions.GetValue | Actions.SetValue | Actions.GetForce | Actions.SetForce | Actions.Start | Actions.Stop | Actions.RunCycle | Actions.GetType | Actions.SetType | Actions.GetLength | Actions.SetRefresh | Actions.CreateLink | Actions.Upload | Actions.Download | Actions.SetActive | Actions.SetHysteresis | Actions.Disconnect | Actions.SetInitValue | Actions.GetLBType | Actions.ModuleInfo | Actions.ListSNMPVariables | Actions.Connected | Actions.Uploading | Actions.GetCpuInfo | Actions.ReadPVFormat | Actions.FireActivated | Actions.ReadIndex | Actions.FireDataValidated | Actions.FireValueChanged | Actions.Link;
        if (1U > linkID && this.Service.IsStatic && this.ConnectionType == ConnectionType.CreateAndLink)
        {
          this.propErrorCode = this.XLinkRequest(this.Service.hPvi, this.LinkName, 703U, this.propLinkParam, 704U);
        }
        else
        {
          this.propErrorCode = errorCode;
          if (this.ConnectionType == ConnectionType.Link)
            this.OnConnected(new PviEventArgs(this.Name, this.Address, errorCode, this.Service.Language, Action.CpuConnect, this.Service));
        }
        if (Actions.Disconnect != (this.Requests & Actions.Disconnect))
          return;
        this.Disconnect(true);
      }
      else
        this.OnError(new PviEventArgs(this.Name, this.Address, errorCode, this.Service.Language, Action.CpuConnect, this.Service));
    }

    internal override void OnPviUnLinked(int errorCode, int option)
    {
      this.propConnection.Disconnect();
      if (errorCode != 0)
        this.OnError(new PviEventArgs(this.Name, this.Address, errorCode, this.Service.Language, Action.CpuDisconnect, this.Service));
      if (this.propConnection.ReturnValue != 0)
        this.OnError(new PviEventArgs(this.Name, this.Address, this.propConnection.ReturnValue, this.Service.Language, Action.CpuDisconnect, this.Service));
      if (ConnectionStates.Connected != this.propConnectionState)
        this.propState = CpuState.Offline;
      if (ConnectionStates.Disconnected != this.propConnection.propConnectionState)
        return;
      this.OnDisconnected(new PviEventArgs(this.Name, this.Address, errorCode, this.Service.Language, Action.DisconnectedEvent, this.Service));
    }

    internal override void OnPviLinked(int errorCode, uint linkID, int option)
    {
      this.propErrorCode = errorCode;
      this.propLinkId = linkID;
      if (errorCode == 0)
      {
        if (this.Requests == Actions.NONE)
          this.propConnectionState = ConnectionStates.Connected;
        if (1 == option)
        {
          int errorCode1 = this.ReadRequest(this.Service.hPvi, this.propLinkId, AccessTypes.List, 120U);
          if (errorCode1 != 0)
            this.Service.OnPVIObjectsAttached(new PviEventArgs(this.propName, this.propAddress, errorCode1, this.Service.Language, Action.CpuReadTasksList));
        }
      }
      this.OnLinked(errorCode, Action.CpuLink);
      if (errorCode == 0)
        return;
      this.OnError(new PviEventArgs(this.Name, this.Address, errorCode, this.Service.Language, Action.LinkObject, this.Service));
    }

    private void InterpretAdvancedCPUInfo()
    {
      this.propAccu = BatteryStates.OK;
      this.propBattery = BatteryStates.OK;
      if (1 == ((int) this.propVoltage & 1))
        this.propAccu = BatteryStates.BAD;
      if (4 == ((int) this.propVoltage & 4))
        this.propAccu = BatteryStates.NOT_AVAILABLE;
      if (16 == ((int) this.propVoltage & 16))
        this.propAccu = BatteryStates.NOT_TESTED;
      if (2 == ((int) this.propVoltage & 2))
        this.propBattery = BatteryStates.BAD;
      if (8 == ((int) this.propVoltage & 8))
        this.propBattery = BatteryStates.NOT_AVAILABLE;
      if (32 != ((int) this.propVoltage & 32))
        return;
      this.propBattery = BatteryStates.NOT_TESTED;
    }

    private void GetANSLCPUInfo(IntPtr pData, uint dataLen)
    {
      try
      {
        byte[] numArray = new byte[(IntPtr) dataLen];
        Marshal.Copy(pData, numArray, 0, (int) dataLen);
        XmlTextReader xmlTextReader = new XmlTextReader((Stream) new MemoryStream(numArray));
        this.propAWSType = "";
        int content = (int) xmlTextReader.MoveToContent();
        while (xmlTextReader.NodeType != XmlNodeType.EndElement)
        {
          switch (xmlTextReader.Name)
          {
            case "SoftwareVers":
              this.propSWVersion = xmlTextReader.GetAttribute("AutomationRuntime");
              break;
            case "CpuConfiguration":
              this.propCPUType = xmlTextReader.GetAttribute("ShortName");
              this.propCPUName = xmlTextReader.GetAttribute("Type");
              uint num = uint.Parse(xmlTextReader.GetAttribute("Node"));
              this.propNodeNumber = (ushort) 0;
              if ((uint) ushort.MaxValue > num)
              {
                this.propNodeNumber = (ushort) num;
                break;
              }
              break;
            case "OperationalValues":
              string attribute1 = xmlTextReader.GetAttribute("CpuBootMode");
              if (attribute1 != null && 0 < attribute1.Length)
                this.propInitDescription = (BootMode) int.Parse(attribute1);
              string attribute2 = xmlTextReader.GetAttribute("CurrentCpuState");
              if (attribute2 != null && 0 < attribute2.Length)
              {
                if (1 < attribute2.Length)
                {
                  switch (attribute2)
                  {
                    case "RUN":
                      this.propState = CpuState.Run;
                      break;
                    case "SERVICE":
                      this.propState = CpuState.Service;
                      break;
                    case "STOP":
                      this.propState = CpuState.Stop;
                      break;
                    default:
                      this.propState = CpuState.Undefined;
                      break;
                  }
                }
                else
                  this.propState = (CpuState) int.Parse(attribute2);
              }
              else
              {
                string attribute3 = xmlTextReader.GetAttribute("CurrentCpuMode");
                if (attribute3 != null && 0 < attribute3.Length)
                {
                  if (1 < attribute3.Length)
                  {
                    switch (attribute3)
                    {
                      case "RUN":
                        this.propState = CpuState.Run;
                        break;
                      case "STOP":
                        this.propState = CpuState.Stop;
                        break;
                      case "SERVICE":
                        this.propState = CpuState.Service;
                        break;
                      case "DIAGNOSIS":
                        this.propState = CpuState.Service;
                        break;
                      default:
                        this.propState = CpuState.Undefined;
                        break;
                    }
                  }
                  else
                  {
                    switch (attribute3)
                    {
                      case "1":
                        this.propState = CpuState.Stop;
                        break;
                      case "2":
                        this.propState = CpuState.Service;
                        break;
                      case "3":
                        this.propState = CpuState.Service;
                        break;
                      case "4":
                        this.propState = CpuState.Run;
                        break;
                    }
                  }
                }
              }
              this.propCpuUsage = (byte) 0;
              string attribute4 = xmlTextReader.GetAttribute("CurrentCpuUsage");
              if (attribute4 != null && 0 < attribute4.Length)
                this.propCpuUsage = byte.Parse(attribute4);
              string attribute5 = xmlTextReader.GetAttribute("BatteryStatus");
              this.propVoltage = (byte) 0;
              if (attribute5.Length == 0)
              {
                this.propVoltage = byte.Parse(attribute5);
                break;
              }
              break;
            case "NetworkDevices":
              this.propHost = xmlTextReader.GetAttribute("Host");
              break;
          }
          xmlTextReader.Read();
        }
      }
      catch
      {
      }
    }

    private void GetINA2000CPUInfo(IntPtr pData, uint dataLen)
    {
      APIFC_CpuInfoRes structure = (APIFC_CpuInfoRes) Marshal.PtrToStructure(pData, typeof (APIFC_CpuInfoRes));
      this.propSWVersion = structure.sw_version;
      this.propCPUName = structure.cpu_name;
      this.propCPUType = structure.cpu_typ;
      this.propAWSType = structure.aws_typ;
      this.propNodeNumber = structure.node_nr;
      this.propInitDescription = (BootMode) structure.init_descr;
      this.propVoltage = structure.voltage;
      switch (structure.state)
      {
        case 0:
          this.propState = CpuState.Run;
          break;
        case 1:
          this.propState = CpuState.Service;
          break;
        case 2:
          this.propState = CpuState.Stop;
          break;
        case 3:
          this.propState = CpuState.Offline;
          break;
        default:
          this.propState = CpuState.Undefined;
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
        case PVIReadAccessTypes.ChildObjects:
          this.propErrorCode = 4815;
          break;
        case PVIReadAccessTypes.DateNTime:
          if (errorCode == 0)
          {
            APIFC_CpuDateTime structure = (APIFC_CpuDateTime) Marshal.PtrToStructure(pData, typeof (APIFC_CpuDateTime));
            this.propDateTime = Pvi.ToDateTime(structure.year + 1900, structure.mon + 1, structure.mday, structure.hour, structure.min, structure.sec);
            this.OnDateTimeRead(new CpuEventArgs(this.Name, this.Address, errorCode, this.Service.Language, Action.CpuReadTime, this.propDateTime));
            break;
          }
          this.OnError(new PviEventArgs(this.Name, this.Address, errorCode, this.Service.Language, Action.CpuReadTime, this.Service));
          break;
        case PVIReadAccessTypes.CPUInfo:
        case PVIReadAccessTypes.ANSL_CpuInfo:
          if (errorCode == 0)
          {
            if (PVIReadAccessTypes.ANSL_CpuInfo == accessType)
              this.GetANSLCPUInfo(pData, dataLen);
            else
              this.GetINA2000CPUInfo(pData, dataLen);
            this.InterpretAdvancedCPUInfo();
            if (Actions.GetCpuInfo == (this.Requests & Actions.GetCpuInfo))
              this.Requests &= Actions.Connect | Actions.GetList | Actions.GetValue | Actions.SetValue | Actions.GetForce | Actions.SetForce | Actions.Start | Actions.Stop | Actions.RunCycle | Actions.GetType | Actions.SetType | Actions.GetLength | Actions.SetRefresh | Actions.CreateLink | Actions.Upload | Actions.Download | Actions.SetActive | Actions.SetHysteresis | Actions.Disconnect | Actions.SetInitValue | Actions.GetLBType | Actions.ModuleInfo | Actions.ListSNMPVariables | Actions.Connected | Actions.Uploading | Actions.ReadPVFormat | Actions.FireActivated | Actions.ReadIndex | Actions.FireDataValidated | Actions.FireValueChanged | Actions.Link;
            if (this.Connection.DeviceType == DeviceType.ANSLTcp)
            {
              this.Requests &= Actions.Connect | Actions.GetList | Actions.GetValue | Actions.SetValue | Actions.GetForce | Actions.SetForce | Actions.Start | Actions.Stop | Actions.RunCycle | Actions.GetType | Actions.SetType | Actions.GetLength | Actions.SetRefresh | Actions.CreateLink | Actions.Upload | Actions.Download | Actions.SetActive | Actions.SetHysteresis | Actions.Disconnect | Actions.SetInitValue | Actions.ModuleInfo | Actions.ListSNMPVariables | Actions.Connected | Actions.Uploading | Actions.GetCpuInfo | Actions.ReadPVFormat | Actions.FireActivated | Actions.ReadIndex | Actions.FireDataValidated | Actions.FireValueChanged | Actions.Link;
              this.propIsSG4Target = true;
            }
            if (Actions.GetLBType == (this.Requests & Actions.GetLBType))
              this.DetectSGxLogger("$Detect_SG4_SysLogger$");
            else if (this.propConnectionState == ConnectionStates.ConnectionChanging)
              this.OnConnectionChanged(errorCode, Action.ChangeConnection);
            else
              this.OnConnected(new PviEventArgs(this.Name, this.Address, this.propErrorCode, this.Service.Language, Action.CpuConnect, this.Service));
            if (Actions.Upload == (this.Requests & Actions.Upload))
            {
              this.Requests &= Actions.Connect | Actions.GetList | Actions.GetValue | Actions.SetValue | Actions.GetForce | Actions.SetForce | Actions.Start | Actions.Stop | Actions.RunCycle | Actions.GetType | Actions.SetType | Actions.GetLength | Actions.SetRefresh | Actions.CreateLink | Actions.Download | Actions.SetActive | Actions.SetHysteresis | Actions.Disconnect | Actions.SetInitValue | Actions.GetLBType | Actions.ModuleInfo | Actions.ListSNMPVariables | Actions.Connected | Actions.Uploading | Actions.GetCpuInfo | Actions.ReadPVFormat | Actions.FireActivated | Actions.ReadIndex | Actions.FireDataValidated | Actions.FireValueChanged | Actions.Link;
              this.RequestModuleList(this.m_ModuListULOption);
            }
          }
          else
          {
            this.propErrorCode = errorCode;
            this.OnError(new PviEventArgs(this.Name, this.Address, errorCode, this.Service.Language, Action.CpuUploadInfo, this.Service));
            if (ConnectionStates.ConnectionChanging == this.propConnectionState)
              this.OnConnectionChanged(this.propErrorCode, Action.CpuChangeConnection);
            else
              this.OnConnected(new PviEventArgs(this.Name, this.Address, this.propErrorCode, this.Service.Language, Action.CpuUploadInfo, this.Service));
          }
          this.OnCpuInfoUpdated(new PviEventArgs(this.Name, this.Address, this.propErrorCode, this.Service.Language, Action.CpuUploadInfo, this.Service));
          break;
        case PVIReadAccessTypes.ModuleList:
          this.propErrorCode = errorCode;
          this.ModuleInfoListFromCB(true, errorCode, pData, dataLen, false);
          break;
        case PVIReadAccessTypes.ReadPhysicalMemory:
          this.OnPhysicalMemoryRead(errorCode, pData, dataLen);
          break;
        case PVIReadAccessTypes.TTService:
          if (20 == option)
          {
            this.OnTTService(errorCode, accessType, dataState, pData, dataLen);
            break;
          }
          this.propProfiler.OnPviRead(errorCode, accessType, dataState, pData, dataLen, option);
          break;
        case PVIReadAccessTypes.ReadErrorLogBook:
          if (errorCode == 0)
          {
            LoggerEntryCollection loggerEntryCollection = new LoggerEntryCollection("ErrorLogBook");
            this.DumpLoggerEntries(this.GetLogBookEntries(pData, dataLen));
            break;
          }
          this.OnError(new PviEventArgs(this.Name, this.Address, errorCode, this.Service.Language, Action.ReadError, this.Service));
          break;
        case PVIReadAccessTypes.DiagnoseModuleList:
          this.propErrorCode = errorCode;
          this.ModuleInfoListFromCB(false, errorCode, pData, dataLen, false);
          break;
        case PVIReadAccessTypes.SavePath:
          if (errorCode == 0)
          {
            this.propSavePath = PviMarshal.ToAnsiString(pData, dataLen);
            this.OnSavePathRead(new PviEventArgs(this.Name, this.Address, errorCode, this.Service.Language, Action.CpuReadSavePath, this.Service));
            break;
          }
          this.OnError(new PviEventArgs(this.Name, this.Address, errorCode, this.Service.Language, Action.CpuReadSavePath, this.Service));
          break;
        case PVIReadAccessTypes.ResolveNodeNumber:
          if (errorCode != 0)
            this.OnError(new PviEventArgs(this.Name, this.Address, errorCode, this.Service.Language, Action.ResolveNodeNumber, this.Service));
          this.OnTCPDestinationSettingsRead(PviMarshal.ToAnsiString(pData, dataLen), new PviEventArgs(this.Name, this.Address, errorCode, this.Service.Language, Action.ResolveNodeNumber, this.Service));
          break;
        case PVIReadAccessTypes.LIC_GetLicenseStatus:
          this.OnLicenseStatusRead(errorCode, pData, dataLen);
          break;
        case PVIReadAccessTypes.ANSL_ModuleList:
          this.propErrorCode = errorCode;
          this.ModuleInfoListFromCB(true, errorCode, pData, dataLen, true);
          break;
        case PVIReadAccessTypes.ANSL_MemoryInfo:
          this.OnMemoryInfoRead(errorCode, pData, dataLen);
          break;
        case PVIReadAccessTypes.ANSL_HardwareInfo:
          this.OnHardwareInfoRead(errorCode, pData, dataLen);
          break;
        case PVIReadAccessTypes.ANSL_RedundancyInfo:
          this.OnRedundancyInfoRead(errorCode, pData, dataLen);
          break;
        case PVIReadAccessTypes.ANSL_CpuExtendedInfo:
          if (19 != option)
            break;
          this.OnTOCRead(this.propName, this.propAddress, errorCode, pData, dataLen);
          break;
        case PVIReadAccessTypes.ANSL_ApplicationInfo:
          this.OnApplicationInfoRead(errorCode, pData, dataLen);
          break;
        default:
          base.OnPviRead(errorCode, accessType, dataState, pData, dataLen, option);
          break;
      }
    }

    private void DumpLoggerEntries(LoggerEntryCollection eventEntries)
    {
    }

    internal override void OnPviWritten(
      int errorCode,
      PVIWriteAccessTypes accessType,
      PVIDataStates dataState,
      int option,
      IntPtr pData,
      uint dataLen)
    {
      switch (accessType)
      {
        case PVIWriteAccessTypes.Connection:
          this.propConnectionState = ConnectionStates.ConnectionChanging;
          if (errorCode != 0)
          {
            this.propErrorCode = errorCode;
            this.OnConnectionChanged(this.propErrorCode, Action.ChangeConnection);
            break;
          }
          this.UploadCpuInfo(DeviceType.ANSLTcp == this.Connection.DeviceType ? AccessTypes.ANSL_CpuInfo : AccessTypes.Info);
          break;
        case PVIWriteAccessTypes.State:
          break;
        case PVIWriteAccessTypes.DateNTime:
          this.OnDateTimeWritten(new CpuEventArgs(this.Name, this.Address, errorCode, this.Service.Language, Action.CpuWriteTime, this.propDateTime));
          if (errorCode == 0)
            break;
          this.OnError(new PviEventArgs(this.Name, this.Address, errorCode, this.Service.Language, Action.CpuWriteTime, this.Service));
          break;
        case PVIWriteAccessTypes.CpuModuleDelete:
          this.OnModuleDeleted(new PviEventArgs(this.Name, this.Address, errorCode, this.Service.Language, Action.CpuModuleDelete, this.Service));
          if (errorCode == 0)
            break;
          this.OnError(new PviEventArgs(this.Name, this.Address, errorCode, this.Service.Language, Action.CpuModuleDelete, this.Service));
          break;
        case PVIWriteAccessTypes.WritePhysicalMemory:
          this.OnPhysicalMemoryWritten(errorCode);
          break;
        case PVIWriteAccessTypes.ClearMemory:
          this.OnMemoryCleared(errorCode);
          break;
        case PVIWriteAccessTypes.SavePath:
          this.OnSavePathWritten(new PviEventArgs(this.Name, this.Address, errorCode, this.Service.Language, Action.CpuWriteSavePath, this.Service));
          if (errorCode == 0)
            break;
          this.OnError(new PviEventArgs(this.Name, this.Address, errorCode, this.Service.Language, Action.CpuWriteSavePath, this.Service));
          break;
        case PVIWriteAccessTypes.GlobalForceOFF:
          this.OnGlobalForcedOFF(errorCode);
          break;
        case PVIWriteAccessTypes.ANSL_RedundancyControl:
          if (1 == option)
          {
            this.OnActiveCpuChanged(new PviEventArgs(this.Name, this.Address, errorCode, this.Service.Language, Action.CpuSwitchActiveCpu, this.Service));
            break;
          }
          this.OnApplicationSynchronizeStarted(new PviEventArgs(this.Name, this.Address, errorCode, this.Service.Language, Action.CpuSynchronizeApplication, this.Service));
          break;
        case PVIWriteAccessTypes.ANSL_COMMAND_Data:
          this.OnXMLCommand(errorCode, dataState, option, pData, dataLen);
          break;
        default:
          base.OnPviWritten(errorCode, accessType, dataState, option, pData, dataLen);
          break;
      }
    }

    private void CheckModuleChanged(APIFC_ModulInfoRes moduleInfoStruct, int errorCode)
    {
      ModuleInfoDecription moduleInfoDesc = new ModuleInfoDecription();
      moduleInfoDesc.Init(moduleInfoStruct);
      this.CheckModuleChanged(moduleInfoDesc, errorCode);
    }

    internal void UpdateModuleAPIFCInfoList(APIFC_ModulInfoRes moduleInfoStruct)
    {
      if (this.propModuleInfoList == null)
        return;
      if (this.propModuleInfoList.ContainsKey((object) moduleInfoStruct.name))
        this.propModuleInfoList[(object) moduleInfoStruct.name] = (object) moduleInfoStruct;
      else
        this.propModuleInfoList.Add((object) moduleInfoStruct.name, (object) moduleInfoStruct);
    }

    internal void UpdateModuleInfoList(ModuleInfoDecription moduleInfoDesc)
    {
      APIFC_ModulInfoRes apifcInfo = new APIFC_ModulInfoRes();
      moduleInfoDesc.UpdateAPIFCModulInfoRes(ref apifcInfo);
      this.UpdateModuleAPIFCInfoList(apifcInfo);
    }

    private void CheckModuleChanged(ModuleInfoDecription moduleInfoDesc, int errorCode)
    {
      if (errorCode == 0)
        this.UpdateModuleInfoList(moduleInfoDesc);
      if (Module.isTaskObject(moduleInfoDesc.type) && this.Tasks.Values != null)
      {
        Task task1 = this.Tasks[moduleInfoDesc.name];
        if (task1 == null)
        {
          if (this.Tasks.Synchronize && this.Tasks.isSyncable)
          {
            Task task2 = new Task(this, moduleInfoDesc.name);
            this.Tasks.OnModuleCreated(new ModuleEventArgs(this.propName, this.propAddress, errorCode, this.Service.Language, Action.ModuleCreatedEvent, (Module) task2, 0));
            task2.updateProperties((object) moduleInfoDesc);
            this.Tasks.OnModuleChanged(new ModuleEventArgs(task2.propName, task2.Address, errorCode, this.Service.Language, Action.ModuleChangedEvent, (Module) task2, 0));
            if (this.Modules.Values == null || !this.Modules.Synchronize)
              return;
            this.Modules.Add((Module) task2);
            this.Modules.OnModuleCreated(new ModuleEventArgs(task2.propName, task2.Address, errorCode, this.Service.Language, Action.ModuleCreatedEvent, (Module) task2, 0));
            this.Modules.OnModuleChanged(new ModuleEventArgs(task2.propName, task2.Address, errorCode, this.Service.Language, Action.ModuleChangedEvent, (Module) task2, 0));
          }
          else
          {
            if (this.Modules.Values == null || !this.Modules.Synchronize)
              return;
            Module module = this.Modules[moduleInfoDesc.name];
            if (module != null)
            {
              module.updateProperties((object) moduleInfoDesc);
              this.Modules.OnModuleChanged(new ModuleEventArgs(module.propName, module.Address, errorCode, this.Service.Language, Action.ModuleChangedEvent, module, 0));
            }
            else
            {
              Task task3 = new Task(this, moduleInfoDesc.name);
              this.Modules.OnModuleCreated(new ModuleEventArgs(task3.propName, task3.Address, errorCode, this.Service.Language, Action.ModuleCreatedEvent, (Module) task3, 0));
              task3.updateProperties((object) moduleInfoDesc);
              this.Modules.OnModuleChanged(new ModuleEventArgs(task3.propName, task3.Address, errorCode, this.Service.Language, Action.ModuleChangedEvent, (Module) task3, 0));
            }
          }
        }
        else
        {
          task1.updateProperties((object) moduleInfoDesc);
          this.Tasks.OnModuleChanged(new ModuleEventArgs(task1.propName, task1.Address, errorCode, this.Service.Language, Action.ModuleChangedEvent, (Module) task1, 0));
          if (this.Modules.Values == null || !this.Modules.Synchronize)
            return;
          this.Modules.OnModuleChanged(new ModuleEventArgs(task1.propName, task1.Address, errorCode, this.Service.Language, Action.ModuleChangedEvent, (Module) task1, 0));
        }
      }
      else if (moduleInfoDesc.type == ModuleType.Logger && this.Loggers.Values != null)
      {
        Logger logger1 = this.Loggers[moduleInfoDesc.name];
        if (logger1 == null)
        {
          if (this.Loggers.Synchronize && this.Loggers.isSyncable)
          {
            Logger logger2 = new Logger(this, moduleInfoDesc.name);
            this.Loggers.OnModuleCreated(new ModuleEventArgs(this.propName, this.propAddress, errorCode, this.Service.Language, Action.ModuleCreatedEvent, (Module) logger2, 0));
            logger2.updateProperties((object) moduleInfoDesc);
            this.Loggers.OnModuleChanged(new ModuleEventArgs(logger2.propName, logger2.Address, errorCode, this.Service.Language, Action.ModuleChangedEvent, (Module) logger2, 0));
            if (this.Modules.Values == null || !this.Modules.Synchronize)
              return;
            this.Modules.Add((Module) logger2);
            this.Modules.OnModuleCreated(new ModuleEventArgs(logger2.propName, logger2.Address, errorCode, this.Service.Language, Action.ModuleCreatedEvent, (Module) logger2, 0));
            this.Modules.OnModuleChanged(new ModuleEventArgs(logger2.propName, logger2.Address, errorCode, this.Service.Language, Action.ModuleChangedEvent, (Module) logger2, 0));
          }
          else
          {
            if (this.Modules.Values == null || !this.Modules.Synchronize)
              return;
            Module module = this.Modules[moduleInfoDesc.name];
            if (module != null)
            {
              module.updateProperties((object) moduleInfoDesc);
              this.Modules.OnModuleChanged(new ModuleEventArgs(module.propName, module.Address, errorCode, this.Service.Language, Action.ModuleChangedEvent, module, 0));
            }
            else
            {
              Logger logger3 = new Logger(this, moduleInfoDesc.name);
              this.Modules.OnModuleCreated(new ModuleEventArgs(logger3.propName, logger3.Address, errorCode, this.Service.Language, Action.ModuleCreatedEvent, (Module) logger3, 0));
              logger3.updateProperties((object) moduleInfoDesc);
              this.Modules.OnModuleChanged(new ModuleEventArgs(logger3.propName, logger3.Address, errorCode, this.Service.Language, Action.ModuleChangedEvent, (Module) logger3, 0));
            }
          }
        }
        else
        {
          logger1.updateProperties((object) moduleInfoDesc);
          this.Loggers.OnModuleChanged(new ModuleEventArgs(logger1.propName, logger1.Address, errorCode, this.Service.Language, Action.ModuleChangedEvent, (Module) logger1, 0));
          if (this.Modules.Values == null || !this.Modules.Synchronize)
            return;
          this.Modules.OnModuleChanged(new ModuleEventArgs(logger1.propName, logger1.Address, errorCode, this.Service.Language, Action.ModuleChangedEvent, (Module) logger1, 0));
        }
      }
      else
      {
        if (this.Modules.Values == null)
          return;
        Module module1 = this.Modules[moduleInfoDesc.name];
        if (module1 == null)
        {
          if (!this.Modules.Synchronize || !this.Modules.isSyncable)
            return;
          Module module2 = new Module(this, moduleInfoDesc, this.Modules);
          this.Modules.OnModuleCreated(new ModuleEventArgs(module2.propName, module2.propAddress, errorCode, this.Service.Language, Action.ModuleCreatedEvent, module2, 0));
          module2.updateProperties((object) moduleInfoDesc);
          this.Modules.OnModuleChanged(new ModuleEventArgs(module2.propName, module2.propAddress, errorCode, this.Service.Language, Action.ModuleChangedEvent, module2, 0));
        }
        else
        {
          module1.updateProperties((object) moduleInfoDesc);
          this.Modules.OnModuleChanged(new ModuleEventArgs(module1.propName, module1.propAddress, errorCode, this.Service.Language, Action.ModuleChangedEvent, module1, 0));
        }
      }
    }

    internal override void OnPviEvent(
      int errorCode,
      EventTypes eventType,
      PVIDataStates dataState,
      IntPtr pData,
      uint dataLen,
      int option)
    {
      ConnectionStates propConnectionState = this.propConnectionState;
      if (this.Name == null)
        return;
      switch (eventType)
      {
        case EventTypes.Error:
        case EventTypes.Data:
          if (errorCode == 0 || 12002 == errorCode)
          {
            if (this.Connection.DeviceType < DeviceType.TcpIpMODBUS)
            {
              if (!this.propFireConChanged)
                this.UploadCpuInfo(DeviceType.ANSLTcp == this.Connection.DeviceType ? AccessTypes.ANSL_CpuInfo : AccessTypes.Info);
            }
            else
              this.OnConnected(new PviEventArgs(this.Name, this.Address, errorCode, this.Service.Language, Action.CpuConnect, this.Service));
          }
          else
          {
            if (this.Variables != null && this.Variables.Values != null)
            {
              foreach (Variable variable in (IEnumerable) this.Variables.Values)
                variable.propErrorState = errorCode;
            }
            if (this.Tasks != null && this.Tasks.Values != null)
            {
              foreach (Task task in (IEnumerable) this.Tasks.Values)
              {
                if (task.Variables != null && task.Variables.Values != null)
                {
                  foreach (Variable variable in (IEnumerable) task.Variables.Values)
                    variable.propErrorState = errorCode;
                }
              }
            }
            if (this.IsConnected)
            {
              this.OnDisconnected(new PviEventArgs(this.Name, this.Address, errorCode, this.Service.Language, Action.DisconnectedEvent, this.Service));
              if (ConnectionStates.Connected == propConnectionState)
                this.propConnectionState = ConnectionStates.ConnectedError;
            }
            else if (ConnectionStates.Connecting == this.propConnectionState)
              this.OnConnected(new PviEventArgs(this.Name, this.Address, errorCode, this.Service.Language, Action.CpuConnect, this.Service));
            this.OnError(new PviEventArgs(this.Name, this.Address, errorCode, this.Service.Language, Action.CpuConnect, this.Service));
            if (ConnectionStates.ConnectionChanging == propConnectionState && errorCode != 13097 && errorCode != 11020)
            {
              this.propConnectionState = ConnectionStates.ConnectedError;
              this.OnConnectionChanged(errorCode, Action.CpuChangeConnection);
            }
          }
          if (ConnectionStates.Connected == this.propConnectionState)
            break;
          this.propState = CpuState.Offline;
          break;
        case EventTypes.Disconnect:
          this.OnDisconnected(new PviEventArgs(this.Name, this.Address, errorCode, this.Service.Language, Action.DisconnectedEvent, this.Service));
          break;
        case EventTypes.Connection:
          break;
        case EventTypes.Proceeding:
          (21 != PviMarshal.PtrToProgressInfoStructure(pData, typeof (ProgressInfo)).Access ? this.Modules[this.actUpLoadModuleName] : this.Modules[this.actDownLoadModuleName])?.OnProceeding(pData, (int) dataLen, errorCode);
          break;
        case EventTypes.ModuleChanged:
          this.CheckModuleChanged(PviMarshal.PtrToModulInfoStructure(pData, typeof (APIFC_ModulInfoRes)), errorCode);
          break;
        case EventTypes.ModuleDeleted:
          this.CheckModuleDeleted(PviMarshal.PtrToModulInfoStructure(pData, typeof (APIFC_ModulInfoRes)).name, errorCode, Action.ModuleDelete);
          break;
        case EventTypes.ModuleListChangedXML:
          if (0U >= dataLen)
            break;
          int updateFlags = 0;
          this.CheckANSLModuleListChanges(errorCode, pData, dataLen, ref updateFlags);
          break;
        case EventTypes.RedundancyCtrlEventXML:
          this.OnRedundancyCtrlEvent(errorCode, pData, dataLen);
          break;
        default:
          base.OnPviEvent(errorCode, eventType, dataState, pData, dataLen, option);
          break;
      }
    }

    private void ModuleInfoListFromCB(
      bool allModules,
      int errorCode,
      IntPtr ptrData,
      uint dataLen,
      bool isANSL)
    {
      int updateFlags = 0;
      Hashtable newItems = (Hashtable) null;
      if (this.propModuleInfoList == null)
        this.propModuleInfoList = new Hashtable();
      if (errorCode == 0)
        newItems = !isANSL ? this.GetINA2000ModuleList(allModules, errorCode, ptrData, dataLen, ref updateFlags) : this.GetANSLModuleList(errorCode, ptrData, dataLen, ref updateFlags);
      this.Requests &= Actions.Connect | Actions.GetList | Actions.GetValue | Actions.SetValue | Actions.GetForce | Actions.SetForce | Actions.Start | Actions.Stop | Actions.RunCycle | Actions.GetType | Actions.SetType | Actions.GetLength | Actions.SetRefresh | Actions.CreateLink | Actions.Download | Actions.SetActive | Actions.SetHysteresis | Actions.Disconnect | Actions.SetInitValue | Actions.GetLBType | Actions.ModuleInfo | Actions.ListSNMPVariables | Actions.Connected | Actions.Uploading | Actions.GetCpuInfo | Actions.ReadPVFormat | Actions.FireActivated | Actions.ReadIndex | Actions.FireDataValidated | Actions.FireValueChanged | Actions.Link;
      if (this.Requests == Actions.NONE)
      {
        this.propModuleInfoRequested = false;
        if (ConnectionStates.Connecting == this.propConnectionState)
          this.OnConnected(new PviEventArgs(this.Name, this.Address, this.propErrorCode, this.Service.Language, Action.CpuConnect, this.Service));
        else if (ConnectionStates.ConnectionChanging == this.propConnectionState)
          this.OnConnectionChanged(this.propErrorCode, Action.CpuChangeConnection);
      }
      this.Modules.CheckUploadedRequest();
      if (this.Tasks.Synchronize)
        this.Tasks.DoSynchronize(newItems);
      if (this.Modules.Synchronize || 2 == (updateFlags & 1))
        this.Modules.DoSynchronize(newItems);
      this.propModuleInfoRequested = false;
      this.Tasks.CheckFireUploadEvents(errorCode, Action.TasksUpload, Action.TasksConnect);
      this.Modules.CheckFireUploadEvents(errorCode, Action.ModulesUpload, Action.ModulesConnect);
    }

    internal void ReadANSLMODLists(
      IntPtr ptrData,
      uint dataLen,
      ref Hashtable delMods,
      ref Hashtable newMods,
      ref Hashtable chgMods)
    {
      try
      {
        if (0U >= dataLen || !(IntPtr.Zero != ptrData))
          return;
        delMods = new Hashtable();
        newMods = new Hashtable();
        chgMods = new Hashtable();
        byte[] numArray = new byte[(IntPtr) dataLen];
        Marshal.Copy(ptrData, numArray, 0, (int) dataLen);
        XmlTextReader xmlTReader = new XmlTextReader((Stream) new MemoryStream(numArray));
        int content = (int) xmlTReader.MoveToContent();
        if (xmlTReader.Name.CompareTo("ModList") != 0)
          return;
        Hashtable hashtable = newMods;
        while (!xmlTReader.EOF)
        {
          if (xmlTReader.NodeType != XmlNodeType.EndElement && xmlTReader.NodeType != XmlNodeType.Whitespace)
          {
            if (xmlTReader.Name.CompareTo("Deleted") == 0)
              hashtable = delMods;
            else if (xmlTReader.Name.CompareTo("New") == 0)
              hashtable = newMods;
            else if (xmlTReader.Name.CompareTo("Changed") == 0)
              hashtable = chgMods;
            if (xmlTReader.Name.CompareTo("ModInfo") == 0 || xmlTReader.Name.CompareTo("TaskInfo") == 0)
            {
              ModuleInfoDecription moduleInfoDecription = new ModuleInfoDecription();
              moduleInfoDecription.ReadFromXML(xmlTReader);
              if (moduleInfoDecription.name != null && !hashtable.ContainsKey((object) moduleInfoDecription.name))
                hashtable.Add((object) moduleInfoDecription.name, (object) moduleInfoDecription);
            }
          }
          xmlTReader.Read();
        }
      }
      catch
      {
      }
    }

    private void CheckANSLModuleListChanges(
      int errorCode,
      IntPtr ptrData,
      uint dataLen,
      ref int updateFlags)
    {
      Hashtable delMods = (Hashtable) null;
      Hashtable newMods = (Hashtable) null;
      Hashtable chgMods = (Hashtable) null;
      int errorCode1 = 12055;
      if (dataLen == 0U || errorCode != 0)
      {
        if (errorCode != 0)
          errorCode1 = errorCode;
        this.OnError(new PviEventArgs(this.Name, this.Address, errorCode1, this.Service.Language, Action.ModuleChangedEvent, this.Service));
      }
      else
      {
        this.ReadANSLMODLists(ptrData, dataLen, ref delMods, ref newMods, ref chgMods);
        if (delMods != null)
        {
          foreach (ModuleInfoDecription moduleInfoDecription in (IEnumerable) delMods.Values)
            this.CheckModuleDeleted(moduleInfoDecription.name, errorCode, Action.ModuleDelete);
        }
        if (newMods != null)
        {
          foreach (ModuleInfoDecription moduleInfoDesc in (IEnumerable) newMods.Values)
            this.CheckModuleChanged(moduleInfoDesc, errorCode);
        }
        if (chgMods == null)
          return;
        foreach (ModuleInfoDecription moduleInfoDesc in (IEnumerable) chgMods.Values)
          this.CheckModuleChanged(moduleInfoDesc, errorCode);
      }
    }

    internal Hashtable ReadANSLMODList(IntPtr ptrData, uint dataLen)
    {
      Hashtable hashtable = new Hashtable();
      try
      {
        if (0U < dataLen && IntPtr.Zero != ptrData)
        {
          byte[] numArray = new byte[(IntPtr) dataLen];
          Marshal.Copy(ptrData, numArray, 0, (int) dataLen);
          XmlTextReader xmlTReader = new XmlTextReader((Stream) new MemoryStream(numArray));
          int content = (int) xmlTReader.MoveToContent();
          if (xmlTReader.Name.CompareTo("ModList") == 0)
          {
            if (this.propModuleInfoList != null)
              this.propModuleInfoList.Clear();
            else
              this.propModuleInfoList = new Hashtable();
            while (!xmlTReader.EOF)
            {
              if (xmlTReader.NodeType != XmlNodeType.EndElement && xmlTReader.NodeType != XmlNodeType.Whitespace && (xmlTReader.Name.CompareTo("ModInfo") == 0 || xmlTReader.Name.CompareTo("TaskInfo") == 0))
              {
                ModuleInfoDecription moduleInfoDecription = new ModuleInfoDecription();
                moduleInfoDecription.ReadFromXML(xmlTReader);
                if (moduleInfoDecription.name != null && (this.m_ModuListULOption == ModuleListOptions.INA2000CompatibleMode && (this.BootMode == BootMode.Diagnostics && 2 == (moduleInfoDecription.modListed & 2) || this.BootMode != BootMode.Diagnostics && 1 == (moduleInfoDecription.modListed & 1)) || ModuleListOptions.INA2000List == this.m_ModuListULOption && 1 == (moduleInfoDecription.modListed & 1) || ModuleListOptions.INA2000DiagnosisList == this.m_ModuListULOption && 2 == (moduleInfoDecription.modListed & 2) || ModuleListOptions.All == this.m_ModuListULOption))
                {
                  if (!hashtable.ContainsKey((object) moduleInfoDecription.name))
                    hashtable.Add((object) moduleInfoDecription.name, (object) moduleInfoDecription);
                  if (this.propModuleInfoList.ContainsKey((object) moduleInfoDecription.name))
                    this.propModuleInfoList[(object) moduleInfoDecription.name] = (object) moduleInfoDecription;
                  else
                    this.propModuleInfoList.Add((object) moduleInfoDecription.name, (object) moduleInfoDecription);
                }
              }
              xmlTReader.Read();
            }
          }
        }
        return hashtable;
      }
      catch
      {
        return new Hashtable();
      }
    }

    private Hashtable GetANSLModuleList(
      int errorCode,
      IntPtr ptrData,
      uint dataLen,
      ref int updateFlags)
    {
      Hashtable anslModuleList = new Hashtable();
      int errorCode1 = 12055;
      if (dataLen == 0U || errorCode != 0)
      {
        if (errorCode != 0)
          errorCode1 = errorCode;
        this.OnError(new PviEventArgs(this.Name, this.Address, errorCode1, this.Service.Language, Action.ModuleInfo, this.Service));
      }
      else
      {
        foreach (ModuleInfoDecription moduleInfoStruct in (IEnumerable) this.ReadANSLMODList(ptrData, dataLen).Values)
        {
          anslModuleList.Add((object) moduleInfoStruct.name, (object) moduleInfoStruct.name);
          this.UpdateModuleInfoStruct(moduleInfoStruct, errorCode, ref updateFlags);
        }
      }
      return anslModuleList;
    }

    private Hashtable GetINA2000ModuleList(
      bool allModules,
      int errorCode,
      IntPtr ptrData,
      uint dataLen,
      ref int updateFlags)
    {
      Hashtable inA2000ModuleList = new Hashtable();
      if (allModules)
      {
        int num = (int) (dataLen / 164U);
        for (int index = 0; index < num; ++index)
        {
          APIFC_ModulInfoRes modulInfoStructure = PviMarshal.PtrToModulInfoStructure((IntPtr) ((int) ptrData + index * 164), typeof (APIFC_ModulInfoRes));
          if (modulInfoStructure.name != null)
          {
            inA2000ModuleList.Add((object) modulInfoStructure.name, (object) modulInfoStructure.name);
            if (this.propModuleInfoList.ContainsKey((object) modulInfoStructure.name))
              this.propModuleInfoList[(object) modulInfoStructure.name] = (object) modulInfoStructure;
            else
              this.propModuleInfoList.Add((object) modulInfoStructure.name, (object) modulInfoStructure);
            this.UpdateModuleInfoStruct(modulInfoStructure, errorCode, ref updateFlags);
          }
        }
      }
      else
      {
        int num = (int) (dataLen / 57U);
        for (int index = 0; index < num; ++index)
        {
          APIFC_DiagModulInfoRes modulInfoStructure = PviMarshal.PtrToDiagModulInfoStructure((IntPtr) ((int) ptrData + index * 57), typeof (APIFC_DiagModulInfoRes));
          if (modulInfoStructure.name != null)
          {
            if (!inA2000ModuleList.ContainsKey((object) modulInfoStructure.name))
            {
              inA2000ModuleList.Add((object) modulInfoStructure.name, (object) modulInfoStructure.name);
              if (this.propModuleInfoList.ContainsKey((object) modulInfoStructure.name))
                this.propModuleInfoList[(object) modulInfoStructure.name] = (object) modulInfoStructure;
              else
                this.propModuleInfoList.Add((object) modulInfoStructure.name, (object) modulInfoStructure);
            }
            this.UpdateModuleInfoStruct(modulInfoStructure, errorCode, ref updateFlags);
          }
        }
      }
      return inA2000ModuleList;
    }

    private void UpdateModuleInfoStruct(
      APIFC_ModulInfoRes moduleInfoStruct,
      int errorCode,
      ref int updateFlags)
    {
      ModuleInfoDecription moduleInfoStruct1 = new ModuleInfoDecription();
      moduleInfoStruct1.Init(moduleInfoStruct);
      this.UpdateModuleInfoStruct(moduleInfoStruct1, errorCode, ref updateFlags);
    }

    private void UpdateModuleInfoStruct(
      ModuleInfoDecription moduleInfoStruct,
      int errorCode,
      ref int updateFlags)
    {
      this.Tasks.UpdateModuleInfo(moduleInfoStruct, errorCode, ref updateFlags, this.BootMode == BootMode.Diagnostics);
      if (this.propUserTaskCollections != null)
      {
        foreach (TaskCollection taskCollection in (IEnumerable) this.propUserTaskCollections.Values)
        {
          if (taskCollection.ContainsKey((object) moduleInfoStruct.name))
          {
            Module module = (Module) taskCollection[moduleInfoStruct.name];
            module.updateProperties(moduleInfoStruct, this.BootMode == BootMode.Diagnostics);
            if (module.CheckModuleInfo(errorCode))
              module.Fire_OnConnected(new PviEventArgs(module.Name, module.Address, errorCode, this.Service.Language, Action.ConnectedEvent, this.Service));
          }
        }
      }
      this.Modules.UpdateModuleInfo(moduleInfoStruct, 0, ref updateFlags, this.BootMode == BootMode.Diagnostics);
      if (this.propUserModuleCollections == null)
        return;
      foreach (ModuleCollection moduleCollection in (IEnumerable) this.propUserModuleCollections.Values)
      {
        if (moduleCollection.ContainsKey((object) moduleInfoStruct.name))
        {
          Module module = moduleCollection[moduleInfoStruct.name];
          module.updateProperties(moduleInfoStruct, this.BootMode == BootMode.Diagnostics);
          if (module.CheckModuleInfo(errorCode))
            module.Fire_OnConnected(new PviEventArgs(module.Name, module.Address, errorCode, this.Service.Language, Action.ConnectedEvent, this.Service));
        }
      }
    }

    private void UpdateModuleInfoStruct(
      APIFC_DiagModulInfoRes diagModuleInfoStruct,
      int errorCode,
      ref int updateFlags)
    {
      this.Tasks.DiagnosticModeUpdateModuleInfo(diagModuleInfoStruct, errorCode, ref updateFlags);
      if (this.propUserTaskCollections != null)
      {
        foreach (TaskCollection taskCollection in (IEnumerable) this.propUserTaskCollections.Values)
        {
          if (taskCollection.ContainsKey((object) diagModuleInfoStruct.name))
          {
            Module module = (Module) taskCollection[diagModuleInfoStruct.name];
            module.updateProperties(diagModuleInfoStruct);
            if (module.CheckModuleInfo(errorCode))
              module.Fire_OnConnected(new PviEventArgs(module.Name, module.Address, errorCode, this.Service.Language, Action.ConnectedEvent, this.Service));
          }
        }
      }
      this.Modules.DiagnosticModeUpdateModuleInfo(diagModuleInfoStruct, errorCode, ref updateFlags);
      if (this.propUserModuleCollections == null)
        return;
      foreach (ModuleCollection moduleCollection in (IEnumerable) this.propUserModuleCollections.Values)
      {
        if (moduleCollection.ContainsKey((object) diagModuleInfoStruct.name))
        {
          Module module = moduleCollection[diagModuleInfoStruct.name];
          module.updateProperties(diagModuleInfoStruct);
          if (module.CheckModuleInfo(errorCode))
            module.Fire_OnConnected(new PviEventArgs(module.Name, module.Address, errorCode, this.Service.Language, Action.ConnectedEvent, this.Service));
        }
      }
    }

    internal void UpdateLoggerMode(bool isSG4)
    {
      this.propIsSG4Target = isSG4;
      if (!isSG4)
      {
        if (this.Loggers.ContainsKey((object) "$LOG285$"))
          return;
        ErrorLogBook errorLogBook = new ErrorLogBook(this);
      }
      else
      {
        if (!this.Loggers.ContainsKey((object) "$LOG285$"))
          return;
        this.Loggers.RemoveFromCollection((Base) this.Loggers["$LOG285$"], Action.LoggerDelete);
      }
    }

    private int DetectSGxLogger(string moduleName)
    {
      if (this.propARLogSys == null)
      {
        this.propARLogSys = new Logger(this, moduleName, true);
        this.propARLogSys.Address = "$arlogsys";
        this.propARLogSys.Error += new PviEventHandler(this.ARLogSys_Error);
        this.propHasARLogSysErrorEVH = true;
        if (LogicalObjectsUsage.ObjectNameWithType == this.Service.LogicalObjectsUsage)
          this.propARLogSys.LinkName = this.LinkName + "/$arlogsys OT=Module";
        else if (this.propLinkName != null && 0 < this.propLinkName.Length)
          this.propARLogSys.LinkName = this.LinkName + "/$arlogsys OT=Module";
      }
      else if (!this.propHasARLogSysErrorEVH)
      {
        this.propARLogSys.Error += new PviEventHandler(this.ARLogSys_Error);
        this.propHasARLogSysErrorEVH = true;
      }
      bool isStatic = this.Service.IsStatic;
      this.Service.IsStatic = false;
      int errorCode = this.propARLogSys.ConnectEx();
      this.Service.IsStatic = isStatic;
      if (errorCode != 0)
      {
        this.Requests &= Actions.Connect | Actions.GetList | Actions.GetValue | Actions.SetValue | Actions.GetForce | Actions.SetForce | Actions.Start | Actions.Stop | Actions.RunCycle | Actions.GetType | Actions.SetType | Actions.GetLength | Actions.SetRefresh | Actions.CreateLink | Actions.Upload | Actions.Download | Actions.SetActive | Actions.SetHysteresis | Actions.Disconnect | Actions.SetInitValue | Actions.ModuleInfo | Actions.ListSNMPVariables | Actions.Connected | Actions.Uploading | Actions.GetCpuInfo | Actions.ReadPVFormat | Actions.FireActivated | Actions.ReadIndex | Actions.FireDataValidated | Actions.FireValueChanged | Actions.Link;
        this.OnError(new PviEventArgs(moduleName, moduleName, errorCode, this.Service.Language, Action.ModuleConnect, this.Service));
        if (ConnectionStates.ConnectionChanging == this.propConnectionState)
          this.OnConnectionChanged(this.propErrorCode, Action.CpuChangeConnection);
        else
          this.OnConnected(new PviEventArgs(this.Name, this.Address, this.propErrorCode, this.Service.Language, Action.CpuConnect, this.Service));
      }
      return errorCode;
    }

    private void ARLogSys_Error(object sender, PviEventArgs e)
    {
      int errorCode = e.ErrorCode;
      if (Service.IsRemoteError(e.ErrorCode))
        return;
      this.propIsSG4Target = true;
      if (4813 == e.ErrorCode || 4820 == e.ErrorCode)
      {
        errorCode = 0;
        this.propIsSG4Target = false;
        if (this.Loggers.Values != null)
        {
          ArrayList arrayList = new ArrayList(this.Loggers.Values.Count);
          int num = 0;
          foreach (Logger logger in (IEnumerable) this.Loggers.Values)
          {
            logger.CleanLoggerEntries(0);
            logger.Disconnect(true);
            arrayList.Add((object) logger);
            ++num;
          }
          for (int index = 0; index < arrayList.Count; ++index)
            this.Loggers.RemoveFromCollection((Base) arrayList[index], Action.LoggerDelete);
          arrayList.Clear();
        }
        this.UpdateLoggerMode(false);
      }
      if (e.ErrorCode == 0 && Action.LoggerGetStatus != e.Action && this.propARLogSys != null)
      {
        this.propARLogSys.ReadRequest(this.Service.hPvi, ((Base) sender).LinkId, AccessTypes.Status, 917U);
      }
      else
      {
        this.propHasARLogSysErrorEVH = false;
        if (this.propARLogSys != null)
        {
          this.propARLogSys.Error -= new PviEventHandler(this.ARLogSys_Error);
          this.propHasARLogSysErrorEVH = false;
        }
        if (e.ErrorCode == 0 && this.Loggers.ContainsKey((object) "$LOG285$"))
        {
          Logger logger = this.Loggers["$LOG285$"];
          logger.CleanLoggerEntries(0);
          logger.Disconnect(true);
          this.Loggers.RemoveFromCollection((Base) logger, Action.LoggerDelete);
        }
        this.Requests &= Actions.Connect | Actions.GetList | Actions.GetValue | Actions.SetValue | Actions.GetForce | Actions.SetForce | Actions.Start | Actions.Stop | Actions.RunCycle | Actions.GetType | Actions.SetType | Actions.GetLength | Actions.SetRefresh | Actions.CreateLink | Actions.Upload | Actions.Download | Actions.SetActive | Actions.SetHysteresis | Actions.Disconnect | Actions.SetInitValue | Actions.ModuleInfo | Actions.ListSNMPVariables | Actions.Connected | Actions.Uploading | Actions.GetCpuInfo | Actions.ReadPVFormat | Actions.FireActivated | Actions.ReadIndex | Actions.FireDataValidated | Actions.FireValueChanged | Actions.Link;
        if (this.Requests == Actions.NONE)
          this.OnConnected(new PviEventArgs(this.Name, this.Address, this.propErrorCode, this.Service.Language, Action.CpuConnect, this.Service));
        if (Actions.GetCpuInfo == (this.Requests & Actions.GetCpuInfo))
          this.UploadCpuInfo(DeviceType.ANSLTcp == this.Connection.DeviceType ? AccessTypes.ANSL_CpuInfo : AccessTypes.Info);
        else if (Actions.Upload == (this.Requests & Actions.Upload) && !this.propModuleInfoRequested)
          this.RequestModuleList(this.m_ModuListULOption);
        if (Actions.Connect == (this.Requests & Actions.Connect))
        {
          this.Requests &= Actions.GetList | Actions.GetValue | Actions.SetValue | Actions.GetForce | Actions.SetForce | Actions.Start | Actions.Stop | Actions.RunCycle | Actions.GetType | Actions.SetType | Actions.GetLength | Actions.SetRefresh | Actions.CreateLink | Actions.Upload | Actions.Download | Actions.SetActive | Actions.SetHysteresis | Actions.Disconnect | Actions.SetInitValue | Actions.GetLBType | Actions.ModuleInfo | Actions.ListSNMPVariables | Actions.Connected | Actions.Uploading | Actions.GetCpuInfo | Actions.ReadPVFormat | Actions.FireActivated | Actions.ReadIndex | Actions.FireDataValidated | Actions.FireValueChanged | Actions.Link;
          if (this.Requests != Actions.NONE)
            return;
          if (ConnectionStates.ConnectionChanging == this.propConnectionState)
            this.OnConnectionChanged(errorCode, Action.CpuChangeConnection);
          else
            this.OnConnected(new PviEventArgs(this.Name, this.Address, this.propErrorCode, this.Service.Language, Action.CpuConnect, this.Service));
        }
        else
        {
          if (ConnectionStates.ConnectionChanging != this.propConnectionState)
            return;
          this.OnConnectionChanged(errorCode, Action.CpuChangeConnection);
        }
      }
    }

    private LoggerEntryCollection GetLogBookEntries(IntPtr pData, uint dataLen)
    {
      int num = Marshal.SizeOf(typeof (APIFC_RLogbookRes_entry));
      int itemCnt = (int) ((long) dataLen / (long) num);
      APIFC_RLogbookRes_entry[] lbEntries = new APIFC_RLogbookRes_entry[itemCnt];
      LoggerEntryCollection eventEntries = new LoggerEntryCollection("EventEntries");
      for (uint index = 0; (long) index < (long) itemCnt; ++index)
      {
        uint ptr = (uint) ((int) pData + (int) index * num);
        lbEntries[(IntPtr) index] = (APIFC_RLogbookRes_entry) Marshal.PtrToStructure((IntPtr) (long) ptr, typeof (APIFC_RLogbookRes_entry));
      }
      this.InsertSysLogBookEntries(lbEntries, itemCnt, eventEntries);
      return eventEntries;
    }

    private void InsertSysLogBookEntries(
      APIFC_RLogbookRes_entry[] lbEntries,
      int itemCnt,
      LoggerEntryCollection eventEntries)
    {
      LoggerEntry entry = (LoggerEntry) null;
      for (int index = itemCnt - 1; index > -1; --index)
      {
        LogBookEntry logBookEntry = new LogBookEntry(lbEntries[index]);
        if (logBookEntry.propErrorNumber != 0U || logBookEntry.propTask.Length != 0 || logBookEntry.propErrorInfo != 0U || logBookEntry.propErrorText.Length != 0)
        {
          if (entry != null && LevelType.Info == logBookEntry.propLevelType && (int) entry.propErrorNumber == (int) logBookEntry.propErrorNumber)
            entry.AppendSGxErrorInfo(logBookEntry, this.IsSG4Target);
          else if (LevelType.Info != logBookEntry.propLevelType)
          {
            entry = new LoggerEntry(this, logBookEntry, itemCnt - index, true, false);
            entry.UpdateForSGx(logBookEntry, this.IsSG4Target);
            eventEntries.Add((LoggerEntryBase) entry, true);
          }
          else
            entry = (LoggerEntry) null;
        }
      }
    }

    private void CheckModuleDeleted(string name, int error, Action action)
    {
      ArrayList arrayList1 = new ArrayList(1);
      if (this.Modules.Values != null)
      {
        Module module = this.Modules[name];
        if (module != null)
        {
          if (this.Modules.Synchronize)
            arrayList1.Add((object) module.Name);
          else if (this.Service != null)
            this.Modules.OnModuleDeleted(new ModuleEventArgs(name, this.propAddress, error, this.Service.Language, Action.ModuleDeletedEvent, module, 0));
        }
      }
      foreach (string str in arrayList1)
      {
        if (this.Tasks.ContainsKey((object) str))
        {
          Task task = this.Tasks[str];
          if (this.Service != null)
            task.Fire_Deleted(new PviEventArgs(task.Name, task.Address, error, this.Service.Language, action, this.Service));
          this.Tasks.Remove(str);
        }
        if (this.Modules.ContainsKey((object) str))
        {
          Module module = this.Modules[str];
          if (this.Service != null)
            module.Fire_Deleted(new PviEventArgs(module.Name, module.Address, error, this.Service.Language, action, this.Service));
          if (module.LinkId != 0U)
            module.Disconnect(true);
          this.Modules.Remove(str);
        }
        if (this.Loggers.ContainsKey((object) str))
        {
          Logger logger = this.Loggers[str];
          if (this.Service != null)
            logger.Fire_Deleted(new PviEventArgs(logger.Name, logger.Address, error, this.Service.Language, action, this.Service));
          this.Loggers.Remove(str);
        }
      }
      ArrayList arrayList2 = new ArrayList(1);
      if (this.Tasks.Values != null)
      {
        foreach (Task task in (IEnumerable) this.Tasks.Values)
        {
          if (!task.propDisposed && task.Address.CompareTo(name) == 0 && this.Modules.Synchronize)
            arrayList2.Add((object) task.Name);
        }
      }
      foreach (string key in arrayList2)
        this.Tasks.Remove(key);
    }

    public virtual void ReadDateTime()
    {
      int errorCode = this.ReadRequest(this.Service.hPvi, this.LinkId, AccessTypes.DateTime, 212U);
      if (errorCode == 0)
        return;
      this.OnError(new PviEventArgs(this.Name, this.Address, errorCode, this.Service.Language, Action.CpuReadTime, this.Service));
    }

    public virtual int DeleteModule(string name)
    {
      string request = "MN=" + name;
      this.Service.BuildRequestBuffer(request);
      int errorCode = this.WriteRequest(this.Service.hPvi, this.propLinkId, AccessTypes.CpuModuleDelete, this.Service.RequestBuffer, request.Length, 219U);
      if (this.propReturnValue != 0)
        this.OnError(new PviEventArgs(this.Name, this.Address, errorCode, this.Service.Language, Action.CpuModuleDelete, this.Service));
      return errorCode;
    }

    public virtual void WriteDateTime(DateTime datetime)
    {
      this.propDateTime = datetime;
      APIFC_CpuDateTime structure = new APIFC_CpuDateTime();
      structure.year = datetime.Year - 1900;
      structure.mon = datetime.Month - 1;
      structure.mday = datetime.Day;
      structure.hour = datetime.Hour;
      structure.min = datetime.Minute;
      structure.sec = datetime.Second;
      IntPtr hMemory = PviMarshal.AllocHGlobal((IntPtr) Marshal.SizeOf(typeof (APIFC_CpuDateTime)));
      Marshal.StructureToPtr((object) structure, hMemory, false);
      int errorCode = this.WriteRequest(this.Service.hPvi, this.propLinkId, AccessTypes.DateTime, hMemory, Marshal.SizeOf(typeof (APIFC_CpuDateTime)), 213U);
      PviMarshal.FreeHGlobal(ref hMemory);
      if (errorCode == 0)
        return;
      this.OnError(new PviEventArgs(this.Name, this.Address, errorCode, this.Service.Language, Action.CpuReadTime, this.Service));
    }

    public virtual void WriteSavePath(string savePath)
    {
      if (savePath.Length == 0)
        return;
      IntPtr hglobal = PviMarshal.StringToHGlobal(savePath);
      int errorCode = this.WriteRequest(this.Service.hPvi, this.propLinkId, AccessTypes.SavePath, hglobal, savePath.Length, 215U);
      PviMarshal.FreeHGlobal(ref hglobal);
      if (errorCode == 0)
        return;
      this.OnError(new PviEventArgs(this.Name, this.propAddress, errorCode, this.Service.Language, Action.CpuWriteSavePath, this.Service));
    }

    public virtual void ReadSavePath()
    {
      int errorCode = this.ReadRequest(this.Service.hPvi, this.LinkId, AccessTypes.SavePath, 216U);
      if (errorCode == 0)
        return;
      this.OnError(new PviEventArgs(this.Name, this.Address, errorCode, this.Service.Language, Action.CpuReadSavePath, this.Service));
    }

    internal override void Dispose(bool disposing, bool removeFromCollection)
    {
      if (this.propDisposed)
        return;
      if (this.Connection != null)
        this.DisconnectWithChilds();
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
      this.propService = propParent is Service ? (Service) propParent : propParent.Service;
      this.propLinkName = propLinkName;
      this.propLogicalName = propLogicalName;
      this.propUserData = propUserData;
      this.propName = propName;
      this.propAddress = propAddress;
      this.propAWSType = (string) null;
      this.propCPUName = (string) null;
      this.propCPUType = (string) null;
      if (removeFromCollection && this.Service != null)
        this.Service.Cpus.Remove(this.Name);
      if (this.propIODataPoints != null)
      {
        this.propIODataPoints.Dispose(disposing, removeFromCollection);
        this.propIODataPoints = (IODataPointCollection) null;
      }
      if (this.propLibraries != null)
      {
        this.propLibraries.Dispose(disposing, removeFromCollection);
        this.propLibraries = (LibraryCollection) null;
      }
      if (this.propMemories != null)
      {
        this.propMemories.Dispose(disposing, removeFromCollection);
        this.propMemories = (MemoryCollection) null;
      }
      if (this.propVariables != null)
      {
        this.propVariables.Dispose(disposing, removeFromCollection);
        this.propVariables = (VariableCollection) null;
      }
      if (this.propLoggers != null)
      {
        this.propLoggers.Dispose(disposing, removeFromCollection);
        this.propLoggers = (LoggerCollection) null;
      }
      if (this.propTasks != null)
      {
        this.propTasks.Dispose(disposing, removeFromCollection);
        this.propTasks = (TaskCollection) null;
      }
      if (this.propModules != null)
      {
        this.propModules.Dispose(disposing, removeFromCollection);
        this.propModules = (ModuleCollection) null;
      }
      if (this.propProfiler != null)
      {
        this.propProfiler.Dispose(disposing);
        this.propProfiler = (Profiler) null;
      }
      if (this.propTaskClasses != null)
      {
        this.propTaskClasses.Dispose(disposing, removeFromCollection);
        this.propTaskClasses = (TaskClassCollection) null;
      }
      if (this.propUserCollections != null)
      {
        this.propUserCollections.Clear();
        this.propUserCollections = (Hashtable) null;
      }
      if (this.propConnection != null)
      {
        this.propConnection.Connected -= new PviEventHandler(this.ConnectionEvent);
        this.propConnection.ConnectionChanged -= new PviEventHandler(this.connection_ConnectionChanged);
        this.propConnection.Disconnected -= new PviEventHandler(this.ConnectionDisconnected);
        this.propConnection.Error -= new PviEventHandler(this.ConnectionEvent);
        this.propConnection.Dispose(disposing, removeFromCollection);
        this.propConnection = (Connection) null;
      }
      if (this.propModuleInfoList != null)
      {
        this.propModuleInfoList.Clear();
        this.propModuleInfoList = (Hashtable) null;
      }
      if (this.propObjectBrowser != null)
        this.propObjectBrowser = (PviObjectBrowser) null;
      if (this.propARLogSys != null)
      {
        this.propARLogSys.Dispose(disposing, true);
        this.propARLogSys = (Logger) null;
      }
      if (this.propHardwareInfos != null)
      {
        this.propHardwareInfos.Dispose(disposing);
        this.propHardwareInfos = (BR.AN.PviServices.HardwareInfo) null;
      }
      this.propUserData = (object) null;
      this.propSavePath = (string) null;
      this.propSWVersion = (string) null;
      this.propLinkName = (string) null;
      this.propLogicalName = (string) null;
      this.propUserData = (object) null;
      this.propName = (string) null;
      this.propAddress = (string) null;
      this.propParent = (Base) null;
      this.propTCPDestinationSettings = (TcpDestinationSettings) null;
      this.propUserModuleCollections = (Hashtable) null;
      this.propUserTaskCollections = (Hashtable) null;
      this.propUserLoggerCollections = (Hashtable) null;
    }

    public override void Remove()
    {
      base.Remove();
      this.Service.Cpus.Remove(this.Name);
      if (this.propUserCollections == null || this.propUserCollections.Values == null)
        return;
      foreach (BaseCollection baseCollection in (IEnumerable) this.propUserCollections.Values)
        baseCollection.Remove(this.Name);
    }

    public virtual void Restart(BootMode bootMode)
    {
      if (!this.IsConnected)
      {
        this.OnError(new PviEventArgs(this.propName, this.propAddress, 4808, this.Service.Language, Action.CpuRestart, this.Service));
      }
      else
      {
        string request;
        switch (bootMode)
        {
          case BootMode.WarmRestart:
            request = "ST=WarmStart";
            break;
          case BootMode.ColdRestart:
            request = "ST=ColdStart";
            break;
          case BootMode.Reset:
            request = "ST=Reset";
            break;
          case BootMode.Diagnostics:
            request = "ST=Diagnose";
            break;
          default:
            this.OnError(new PviEventArgs(this.Name, this.Address, 12034, this.Service.Language, Action.CpuRestart, this.Service));
            return;
        }
        this.Service.BuildRequestBuffer(request);
        int errorCode = this.WriteRequest(this.Service.hPvi, this.propLinkId, AccessTypes.Status, this.Service.RequestBuffer, request.Length, 207U);
        if (errorCode != 0)
          this.OnError(new PviEventArgs(this.Name, this.Address, errorCode, this.Service.Language, Action.CpuRestart, this.Service));
        else
          this.propRestarted = true;
      }
    }

    public int UpdateCpuInfo() => this.UploadCpuInfo(DeviceType.ANSLTcp == this.Connection.DeviceType ? AccessTypes.ANSL_CpuInfo : AccessTypes.Info, false);

    internal int UploadCpuInfo(AccessTypes accType) => this.UploadCpuInfo(accType, true);

    internal int UploadCpuInfo(AccessTypes accType, bool doFireError)
    {
      int errorCode = this.ReadArgumentRequest(this.Service.hPvi, this.propLinkId, accType, IntPtr.Zero, 0, 702U);
      if (errorCode != 0 && doFireError)
        this.OnError(new PviEventArgs(this.Name, this.Address, errorCode, this.Service.Language, Action.NONE, this.Service));
      return errorCode;
    }

    internal uint ModUID => this.propErrorLogBookModUID;

    private int ReadErrorLogbook()
    {
      int dataLen = 4;
      int[] source = new int[1]{ 0 };
      IntPtr num = PviMarshal.AllocCoTaskMem(4);
      Marshal.Copy(source, 0, num, 1);
      return !this.IsConnected ? 4808 : this.ReadArgumentRequest(this.Service.hPvi, this.propLinkId, AccessTypes.ReadError, num, dataLen, 269U);
    }

    internal void Fire_OnError(PviEventArgs e) => this.OnError(e);

    protected internal override void OnError(PviEventArgs e)
    {
      this.propErrorCode = e.ErrorCode;
      if (this.ignoreEvents)
        return;
      base.OnError(e);
      if (this.propUserCollections != null && this.propUserCollections.Values != null)
      {
        foreach (CpuCollection cpuCollection in (IEnumerable) this.propUserCollections.Values)
          cpuCollection.OnError(this, e);
      }
      this.Service.Cpus.OnError(this, e);
    }

    protected virtual void OnCpuInfoUpdated(PviEventArgs e)
    {
      if (this.CpuInfoUpdated != null)
        this.CpuInfoUpdated((object) this, e);
      if (this.propUserCollections == null || this.propUserCollections.Values == null)
        return;
      foreach (BaseCollection baseCollection in (IEnumerable) this.propUserCollections.Values)
      {
        foreach (Cpu cpu in (IEnumerable) baseCollection.Values)
          cpu.OnCpuInfoUpdated(e);
      }
    }

    protected override void OnConnectionChanged(int errorCode, Action action)
    {
      this.SysLoggerDisconnect();
      this.propErrorCode = errorCode;
      this.ignoreEvents = false;
      this.CpuEventsON();
      if (this.propFireConChanged)
      {
        this.propFireConChanged = false;
        base.OnConnectionChanged(errorCode, action);
      }
      else if (errorCode != 0)
        this.propConnectionState = ConnectionStates.ConnectedError;
      else
        this.propConnectionState = ConnectionStates.Connected;
    }

    protected override void OnConnected(PviEventArgs e)
    {
      this.propErrorCode = e.ErrorCode;
      this.SysLoggerDisconnect();
      if (this.ignoreEvents)
        return;
      bool propFireConChanged = this.propFireConChanged;
      if (this.propRestarted)
        this.propConnection.propConnectionState = ConnectionStates.Connected;
      else
        this.propConnection.propConnectionState = this.propConnectionState;
      if (this.propModules.Synchronize)
      {
        int count = this.Modules.Count;
      }
      if (e.propErrorCode == 0)
      {
        if (Actions.Upload == (this.propModules.Requests & Actions.Upload))
          this.propModules.Upload();
        if (Actions.Upload == (this.propLoggers.Requests & Actions.Upload))
          this.propLoggers.Upload();
        else if (!this.propReCreateActive)
        {
          foreach (Logger logger in (IEnumerable) this.propLoggers.Values)
          {
            if (Actions.Connected == (logger.Requests & Actions.Connected))
            {
              logger.Requests &= Actions.Connect | Actions.GetList | Actions.GetValue | Actions.SetValue | Actions.GetForce | Actions.SetForce | Actions.Start | Actions.Stop | Actions.RunCycle | Actions.GetType | Actions.SetType | Actions.GetLength | Actions.SetRefresh | Actions.CreateLink | Actions.Upload | Actions.Download | Actions.SetActive | Actions.SetHysteresis | Actions.Disconnect | Actions.SetInitValue | Actions.GetLBType | Actions.ModuleInfo | Actions.ListSNMPVariables | Actions.Uploading | Actions.GetCpuInfo | Actions.ReadPVFormat | Actions.FireActivated | Actions.ReadIndex | Actions.FireDataValidated | Actions.FireValueChanged | Actions.Link;
              logger.ReadIndex(Action.LoggerIndexForConnect);
            }
            else if (Actions.Connect == (logger.Requests & Actions.Connect))
            {
              logger.Requests &= Actions.GetList | Actions.GetValue | Actions.SetValue | Actions.GetForce | Actions.SetForce | Actions.Start | Actions.Stop | Actions.RunCycle | Actions.GetType | Actions.SetType | Actions.GetLength | Actions.SetRefresh | Actions.CreateLink | Actions.Upload | Actions.Download | Actions.SetActive | Actions.SetHysteresis | Actions.Disconnect | Actions.SetInitValue | Actions.GetLBType | Actions.ModuleInfo | Actions.ListSNMPVariables | Actions.Connected | Actions.Uploading | Actions.GetCpuInfo | Actions.ReadPVFormat | Actions.FireActivated | Actions.ReadIndex | Actions.FireDataValidated | Actions.FireValueChanged | Actions.Link;
              logger.propConnectionState = ConnectionStates.Unininitialized;
              logger.Connect();
            }
          }
        }
        if ((this.propTasks.Requests & Actions.Upload) != Actions.NONE)
          this.propTasks.Upload();
        else if (!this.propReCreateActive)
        {
          foreach (Task task in (IEnumerable) this.propTasks.Values)
          {
            if (Actions.Connected == (task.Requests & Actions.Connected))
            {
              task.Requests &= Actions.Connect | Actions.GetList | Actions.GetValue | Actions.SetValue | Actions.GetForce | Actions.SetForce | Actions.Start | Actions.Stop | Actions.RunCycle | Actions.GetType | Actions.SetType | Actions.GetLength | Actions.SetRefresh | Actions.CreateLink | Actions.Upload | Actions.Download | Actions.SetActive | Actions.SetHysteresis | Actions.Disconnect | Actions.SetInitValue | Actions.GetLBType | Actions.ModuleInfo | Actions.ListSNMPVariables | Actions.Uploading | Actions.GetCpuInfo | Actions.ReadPVFormat | Actions.FireActivated | Actions.ReadIndex | Actions.FireDataValidated | Actions.FireValueChanged | Actions.Link;
              task.Fire_OnConnect();
            }
            else if (Actions.Connect == (task.Requests & Actions.Connect))
            {
              task.Requests &= Actions.GetList | Actions.GetValue | Actions.SetValue | Actions.GetForce | Actions.SetForce | Actions.Start | Actions.Stop | Actions.RunCycle | Actions.GetType | Actions.SetType | Actions.GetLength | Actions.SetRefresh | Actions.CreateLink | Actions.Upload | Actions.Download | Actions.SetActive | Actions.SetHysteresis | Actions.Disconnect | Actions.SetInitValue | Actions.GetLBType | Actions.ModuleInfo | Actions.ListSNMPVariables | Actions.Connected | Actions.Uploading | Actions.GetCpuInfo | Actions.ReadPVFormat | Actions.FireActivated | Actions.ReadIndex | Actions.FireDataValidated | Actions.FireValueChanged | Actions.Link;
              task.propConnectionState = ConnectionStates.Unininitialized;
              task.Connect(true);
            }
          }
        }
        if (Actions.Upload == (this.propTaskClasses.Requests & Actions.Upload))
        {
          this.propTaskClasses.Upload();
          this.propTaskClasses.Requests &= Actions.Connect | Actions.GetList | Actions.GetValue | Actions.SetValue | Actions.GetForce | Actions.SetForce | Actions.Start | Actions.Stop | Actions.RunCycle | Actions.GetType | Actions.SetType | Actions.GetLength | Actions.SetRefresh | Actions.CreateLink | Actions.Download | Actions.SetActive | Actions.SetHysteresis | Actions.Disconnect | Actions.SetInitValue | Actions.GetLBType | Actions.ModuleInfo | Actions.ListSNMPVariables | Actions.Connected | Actions.Uploading | Actions.GetCpuInfo | Actions.ReadPVFormat | Actions.FireActivated | Actions.ReadIndex | Actions.FireDataValidated | Actions.FireValueChanged | Actions.Link;
        }
        if (Actions.Upload == (this.propVariables.Requests & Actions.Upload))
        {
          this.propVariables.Upload();
          this.propVariables.Requests &= Actions.Connect | Actions.GetList | Actions.GetValue | Actions.SetValue | Actions.GetForce | Actions.SetForce | Actions.Start | Actions.Stop | Actions.RunCycle | Actions.GetType | Actions.SetType | Actions.GetLength | Actions.SetRefresh | Actions.CreateLink | Actions.Download | Actions.SetActive | Actions.SetHysteresis | Actions.Disconnect | Actions.SetInitValue | Actions.GetLBType | Actions.ModuleInfo | Actions.ListSNMPVariables | Actions.Connected | Actions.Uploading | Actions.GetCpuInfo | Actions.ReadPVFormat | Actions.FireActivated | Actions.ReadIndex | Actions.FireDataValidated | Actions.FireValueChanged | Actions.Link;
        }
        if (Actions.Upload == (this.propMemories.Requests & Actions.Upload))
        {
          this.propMemories.Upload();
          this.propMemories.propRequests &= Actions.Connect | Actions.GetList | Actions.GetValue | Actions.SetValue | Actions.GetForce | Actions.SetForce | Actions.Start | Actions.Stop | Actions.RunCycle | Actions.GetType | Actions.SetType | Actions.GetLength | Actions.SetRefresh | Actions.CreateLink | Actions.Download | Actions.SetActive | Actions.SetHysteresis | Actions.Disconnect | Actions.SetInitValue | Actions.GetLBType | Actions.ModuleInfo | Actions.ListSNMPVariables | Actions.Connected | Actions.Uploading | Actions.GetCpuInfo | Actions.ReadPVFormat | Actions.FireActivated | Actions.ReadIndex | Actions.FireDataValidated | Actions.FireValueChanged | Actions.Link;
        }
        if (Actions.Upload == (this.propLibraries.Requests & Actions.Upload))
        {
          this.propLibraries.Upload();
          this.propLibraries.Requests &= Actions.Connect | Actions.GetList | Actions.GetValue | Actions.SetValue | Actions.GetForce | Actions.SetForce | Actions.Start | Actions.Stop | Actions.RunCycle | Actions.GetType | Actions.SetType | Actions.GetLength | Actions.SetRefresh | Actions.CreateLink | Actions.Download | Actions.SetActive | Actions.SetHysteresis | Actions.Disconnect | Actions.SetInitValue | Actions.GetLBType | Actions.ModuleInfo | Actions.ListSNMPVariables | Actions.Connected | Actions.Uploading | Actions.GetCpuInfo | Actions.ReadPVFormat | Actions.FireActivated | Actions.ReadIndex | Actions.FireDataValidated | Actions.FireValueChanged | Actions.Link;
        }
      }
      if (!this.propReCreateActive)
      {
        if (this.propVariables != null && 0 < this.propVariables.Count)
        {
          foreach (Variable variable in (IEnumerable) this.Variables.Values)
          {
            if ((variable.Requests & Actions.Connect) != Actions.NONE)
            {
              variable.Connect(true);
              variable.Requests &= Actions.GetList | Actions.GetValue | Actions.SetValue | Actions.GetForce | Actions.SetForce | Actions.Start | Actions.Stop | Actions.RunCycle | Actions.GetType | Actions.SetType | Actions.GetLength | Actions.SetRefresh | Actions.CreateLink | Actions.Upload | Actions.Download | Actions.SetActive | Actions.SetHysteresis | Actions.Disconnect | Actions.SetInitValue | Actions.GetLBType | Actions.ModuleInfo | Actions.ListSNMPVariables | Actions.Connected | Actions.Uploading | Actions.GetCpuInfo | Actions.ReadPVFormat | Actions.FireActivated | Actions.ReadIndex | Actions.FireDataValidated | Actions.FireValueChanged | Actions.Link;
            }
            else if ((variable.Requests & Actions.SetActive) != Actions.NONE)
            {
              variable.Active = true;
              variable.Requests &= Actions.Connect | Actions.GetList | Actions.GetValue | Actions.SetValue | Actions.GetForce | Actions.SetForce | Actions.Start | Actions.Stop | Actions.RunCycle | Actions.GetType | Actions.SetType | Actions.GetLength | Actions.SetRefresh | Actions.CreateLink | Actions.Upload | Actions.Download | Actions.SetHysteresis | Actions.Disconnect | Actions.SetInitValue | Actions.GetLBType | Actions.ModuleInfo | Actions.ListSNMPVariables | Actions.Connected | Actions.Uploading | Actions.GetCpuInfo | Actions.ReadPVFormat | Actions.FireActivated | Actions.ReadIndex | Actions.FireDataValidated | Actions.FireValueChanged | Actions.Link;
            }
            else if (!this.IsSG4Target && variable.IsConnected)
              variable.Read_State(variable.LinkId, 2812U);
          }
        }
        if (this.propModules != null && 0 < this.propModules.Count)
        {
          foreach (Module module in (IEnumerable) this.Modules.Values)
          {
            if ((module.Requests & Actions.Connected) != Actions.NONE)
            {
              module.Fire_OnConnect();
              module.Requests &= Actions.Connect | Actions.GetList | Actions.GetValue | Actions.SetValue | Actions.GetForce | Actions.SetForce | Actions.Start | Actions.Stop | Actions.RunCycle | Actions.GetType | Actions.SetType | Actions.GetLength | Actions.SetRefresh | Actions.CreateLink | Actions.Upload | Actions.Download | Actions.SetActive | Actions.SetHysteresis | Actions.Disconnect | Actions.SetInitValue | Actions.GetLBType | Actions.ModuleInfo | Actions.ListSNMPVariables | Actions.Uploading | Actions.GetCpuInfo | Actions.ReadPVFormat | Actions.FireActivated | Actions.ReadIndex | Actions.FireDataValidated | Actions.FireValueChanged | Actions.Link;
            }
            else if (Actions.Connect == (module.Requests & Actions.Connect))
            {
              module.Requests &= Actions.GetList | Actions.GetValue | Actions.SetValue | Actions.GetForce | Actions.SetForce | Actions.Start | Actions.Stop | Actions.RunCycle | Actions.GetType | Actions.SetType | Actions.GetLength | Actions.SetRefresh | Actions.CreateLink | Actions.Upload | Actions.Download | Actions.SetActive | Actions.SetHysteresis | Actions.Disconnect | Actions.SetInitValue | Actions.GetLBType | Actions.ModuleInfo | Actions.ListSNMPVariables | Actions.Connected | Actions.Uploading | Actions.GetCpuInfo | Actions.ReadPVFormat | Actions.FireActivated | Actions.ReadIndex | Actions.FireDataValidated | Actions.FireValueChanged | Actions.Link;
              module.propConnectionState = ConnectionStates.Unininitialized;
              module.Connect(true, this.propConnectionType, 301U);
            }
          }
        }
      }
      if (!this.IsSG4Target && this.Tasks != null && 0 < this.Tasks.Count)
      {
        foreach (Task task in (IEnumerable) this.Tasks.Values)
        {
          if (task.propVariables != null && 0 < task.propVariables.Count)
          {
            foreach (Variable variable in (IEnumerable) task.Variables.Values)
            {
              if (variable.IsConnected)
                variable.Read_State(variable.LinkId, 2812U);
            }
          }
        }
      }
      if (!this.propReCreateActive)
      {
        if (this.IODataPoints != null && 0 < this.IODataPoints.Count)
        {
          foreach (IODataPoint ioDataPoint in (IEnumerable) this.IODataPoints.Values)
          {
            if ((ioDataPoint.Requests & Actions.Connect) != Actions.NONE)
            {
              ioDataPoint.Requests &= Actions.GetList | Actions.GetValue | Actions.SetValue | Actions.GetForce | Actions.SetForce | Actions.Start | Actions.Stop | Actions.RunCycle | Actions.GetType | Actions.SetType | Actions.GetLength | Actions.SetRefresh | Actions.CreateLink | Actions.Upload | Actions.Download | Actions.SetActive | Actions.SetHysteresis | Actions.Disconnect | Actions.SetInitValue | Actions.GetLBType | Actions.ModuleInfo | Actions.ListSNMPVariables | Actions.Connected | Actions.Uploading | Actions.GetCpuInfo | Actions.ReadPVFormat | Actions.FireActivated | Actions.ReadIndex | Actions.FireDataValidated | Actions.FireValueChanged | Actions.Link;
              ioDataPoint.Connect();
            }
          }
        }
        if (this.propLibraries != null && 0 < this.propLibraries.Count)
        {
          foreach (Library library in (IEnumerable) this.Libraries.Values)
          {
            if ((library.Requests & Actions.Connect) != Actions.NONE)
            {
              library.Connect();
              library.Requests &= Actions.GetList | Actions.GetValue | Actions.SetValue | Actions.GetForce | Actions.SetForce | Actions.Start | Actions.Stop | Actions.RunCycle | Actions.GetType | Actions.SetType | Actions.GetLength | Actions.SetRefresh | Actions.CreateLink | Actions.Upload | Actions.Download | Actions.SetActive | Actions.SetHysteresis | Actions.Disconnect | Actions.SetInitValue | Actions.GetLBType | Actions.ModuleInfo | Actions.ListSNMPVariables | Actions.Connected | Actions.Uploading | Actions.GetCpuInfo | Actions.ReadPVFormat | Actions.FireActivated | Actions.ReadIndex | Actions.FireDataValidated | Actions.FireValueChanged | Actions.Link;
            }
          }
        }
      }
      if (this.propUserCollections != null && this.propUserCollections.Count > 0)
      {
        foreach (CpuCollection cpuCollection in (IEnumerable) this.propUserCollections.Values)
          cpuCollection.OnConnected(this, e);
      }
      if (this.Service != null && this.Service.Cpus != null)
        this.Service.Cpus.Fire_Connected((object) this, e);
      if (this.propReCreateActive)
      {
        this.propReCreateActive = false;
        if (this.Service.WaitForParentConnection)
          this.reCreateChildState();
      }
      if (this.propRestarted)
      {
        this.OnRestarted(new PviEventArgs(this.Name, this.Address, e.ErrorCode, this.Service.Language, Action.CpuRestart, this.Service));
        base.OnConnected(new PviEventArgs(this.Name, this.Address, e.ErrorCode, this.Service.Language, Action.CpuRestart, this.Service));
        this.propRestarted = false;
      }
      else
        base.OnConnected(e);
      if (!propFireConChanged)
        return;
      this.OnConnectionChanged(e.ErrorCode, e.Action);
      this.propFireConChanged = false;
    }

    protected override void OnDisconnected(PviEventArgs e)
    {
      this.SysLoggerDisconnect();
      if (this.ignoreEvents)
        return;
      if (this.propNoDisconnectedEvent)
      {
        this.propConnectionState = ConnectionStates.Disconnected;
        if (this.Modules != null)
          this.Modules.propConnectionState = ConnectionStates.Disconnected;
        if (this.Tasks != null)
          this.Tasks.propConnectionState = ConnectionStates.Disconnected;
        if (this.Variables != null)
          this.Variables.propConnectionState = ConnectionStates.Disconnected;
        if (this.IODataPoints != null)
          this.IODataPoints.propConnectionState = ConnectionStates.Disconnected;
        this.propNoDisconnectedEvent = false;
      }
      else
      {
        this.Requests |= Actions.GetCpuInfo;
        if (this.ConnectionType != ConnectionType.Link)
          this.Requests |= Actions.GetLBType;
        this.propModuleInfoRequested = false;
        this.propState = CpuState.Offline;
        if (this.propModuleInfoList != null)
          this.propModuleInfoList.Clear();
        base.OnDisconnected(e);
        if (this.propConnection != null)
          this.propConnection.propConnectionState = this.propConnectionState;
        if (this.propUserCollections != null && this.propUserCollections.Count > 0)
        {
          foreach (CpuCollection cpuCollection in (IEnumerable) this.propUserCollections.Values)
            cpuCollection.OnDisconnected(this, e);
        }
        if (this.Service != null && this.Service.Cpus != null)
          this.Service.Cpus.Fire_Disconnected((object) this, e);
        if (this.Modules != null)
          this.Modules.propConnectionState = this.propConnectionState;
        if (this.Tasks != null)
          this.Tasks.propConnectionState = this.propConnectionState;
        if (this.Variables != null)
          this.Variables.propConnectionState = this.propConnectionState;
        if (this.IODataPoints == null)
          return;
        this.IODataPoints.propConnectionState = this.propConnectionState;
      }
    }

    protected virtual void OnRestarted(PviEventArgs e)
    {
      if (this.Restarted != null)
        this.Restarted((object) this, e);
      if (this.propUserCollections != null && this.propUserCollections.Count > 0)
      {
        foreach (CpuCollection cpuCollection in (IEnumerable) this.propUserCollections.Values)
          cpuCollection.OnRestarted(this, e);
      }
      this.Service.Cpus.OnRestarted(this, e);
    }

    protected virtual void OnDateTimeRead(CpuEventArgs e)
    {
      if (this.DateTimeRead == null)
        return;
      this.DateTimeRead((object) this, e);
    }

    protected virtual void OnDateTimeWritten(CpuEventArgs e)
    {
      if (this.DateTimeWritten == null)
        return;
      this.DateTimeWritten((object) this, e);
    }

    protected virtual void OnSavePathWritten(PviEventArgs e)
    {
      if (this.SavePathWritten == null)
        return;
      this.SavePathWritten((object) this, e);
    }

    protected virtual void OnModuleDeleted(PviEventArgs e)
    {
      if (this.ModuleDeleted == null)
        return;
      this.ModuleDeleted((object) this, e);
    }

    protected virtual void OnSavePathRead(PviEventArgs e)
    {
      if (this.SavePathRead == null)
        return;
      this.SavePathRead((object) this, e);
    }

    internal APIFC_CpuInfoRes PtrToCpuInfoStruct(IntPtr pData)
    {
      APIFC_CpuInfoRes cpuInfoStruct = new APIFC_CpuInfoRes();
      int num1 = 0;
      while (true)
      {
        IntPtr ptr = pData;
        int ofs = num1++;
        byte num2;
        if ((num2 = Marshal.ReadByte(ptr, ofs)) != (byte) 0)
        {
          // ISSUE: explicit reference operation
          (^ref cpuInfoStruct).sw_version += (string) (object) System.Convert.ToChar(num2);
        }
        else
          break;
      }
      int num3 = 36;
      while (true)
      {
        IntPtr ptr = pData;
        int ofs = num3++;
        byte num4;
        if ((num4 = Marshal.ReadByte(ptr, ofs)) != (byte) 0)
        {
          // ISSUE: explicit reference operation
          (^ref cpuInfoStruct).cpu_name += (string) (object) System.Convert.ToChar(num4);
        }
        else
          break;
      }
      int num5 = 72;
      while (true)
      {
        IntPtr ptr = pData;
        int ofs = num5++;
        byte num6;
        if ((num6 = Marshal.ReadByte(ptr, ofs)) != (byte) 0)
        {
          // ISSUE: explicit reference operation
          (^ref cpuInfoStruct).cpu_typ += (string) (object) System.Convert.ToChar(num6);
        }
        else
          break;
      }
      int num7 = 108;
      while (true)
      {
        IntPtr ptr = pData;
        int ofs = num7++;
        byte num8;
        if ((num8 = Marshal.ReadByte(ptr, ofs)) != (byte) 0)
        {
          // ISSUE: explicit reference operation
          (^ref cpuInfoStruct).aws_typ += (string) (object) System.Convert.ToChar(num8);
        }
        else
          break;
      }
      cpuInfoStruct.node_nr = (ushort) Marshal.ReadInt16(pData, 144);
      cpuInfoStruct.init_descr = Marshal.ReadInt32(pData, 146);
      cpuInfoStruct.state = Marshal.ReadInt32(pData, 150);
      cpuInfoStruct.voltage = Marshal.ReadByte(pData, 154);
      return cpuInfoStruct;
    }

    public override int FromXmlTextReader(
      ref XmlTextReader reader,
      ConfigurationFlags flags,
      Base baseObj)
    {
      Cpu cpu = (Cpu) baseObj;
      if (cpu == null)
        return -1;
      base.FromXmlTextReader(ref reader, flags, (Base) cpu);
      string str = "";
      string attribute1 = reader.GetAttribute("ApplicationMemory");
      if (attribute1 != null && attribute1.Length > 0)
        cpu.propAWSType = attribute1;
      str = "";
      string attribute2 = reader.GetAttribute("RuntimeVersion");
      if (attribute2 != null && attribute2.Length > 0)
        cpu.propSWVersion = attribute2;
      str = "";
      string attribute3 = reader.GetAttribute("SavePath");
      if (attribute3 != null && attribute3.Length > 0)
        cpu.SavePath = attribute3;
      str = "";
      string attribute4 = reader.GetAttribute("NodeNumber");
      if (attribute4 != null && attribute4.Length > 0)
      {
        ushort result = 0;
        if (PviParse.TryParseUInt16(attribute4, out result))
          cpu.propNodeNumber = result;
      }
      str = "";
      string attribute5 = reader.GetAttribute("State");
      if (attribute5 != null && attribute5.Length > 0)
      {
        switch (attribute5.ToLower())
        {
          case "offline":
            cpu.propState = CpuState.Offline;
            break;
          case "run":
            cpu.propState = CpuState.Run;
            break;
          case "service":
            cpu.propState = CpuState.Service;
            break;
          case "stop":
            cpu.propState = CpuState.Stop;
            break;
        }
      }
      str = "";
      string attribute6 = reader.GetAttribute("BootMode");
      if (attribute6 != null && attribute6.Length > 0)
      {
        switch (attribute6.ToLower())
        {
          case "boot":
            cpu.propInitDescription = BootMode.Boot;
            break;
          case "coldrestart":
            cpu.propInitDescription = BootMode.ColdRestart;
            break;
          case "diagnostics":
            cpu.propInitDescription = BootMode.Diagnostics;
            break;
          case "error":
            cpu.propInitDescription = BootMode.Error;
            break;
          case "reconfig":
            cpu.propInitDescription = BootMode.Reconfig;
            break;
          case "reset":
            cpu.propInitDescription = BootMode.Reset;
            break;
          case "warmrestart":
            cpu.propInitDescription = BootMode.WarmRestart;
            break;
          case "nmi":
            cpu.propInitDescription = BootMode.NMI;
            break;
        }
      }
      str = "";
      string attribute7 = reader.GetAttribute("IsSG4Target");
      if (attribute7 != null && attribute7.ToLower() == "false")
        cpu.propIsSG4Target = false;
      str = "";
      string attribute8 = reader.GetAttribute("ModUID");
      if (attribute8 != null && attribute8.Length > 0)
      {
        uint result = 0;
        if (PviParse.TryParseUInt32(attribute8, out result))
          cpu.propErrorLogBookModUID = result;
      }
      str = "";
      string attribute9 = reader.GetAttribute("Type");
      if (attribute9 != null && attribute9.Length > 0)
        cpu.propCPUType = attribute9;
      str = "";
      string attribute10 = reader.GetAttribute("Accu");
      if (attribute10 != null && attribute10.Length > 0)
      {
        switch (attribute10.ToLower())
        {
          case "bad":
            cpu.propAccu = BatteryStates.BAD;
            break;
          case "not_available":
            cpu.propAccu = BatteryStates.NOT_AVAILABLE;
            break;
          case "not_tested":
            cpu.propAccu = BatteryStates.NOT_TESTED;
            break;
          case "ok":
            cpu.propAccu = BatteryStates.OK;
            break;
          case "undefined":
            cpu.propAccu = BatteryStates.UNDEFINED;
            break;
        }
      }
      str = "";
      string attribute11 = reader.GetAttribute("Battery");
      if (attribute11 != null && attribute11.Length > 0)
      {
        switch (attribute11.ToLower())
        {
          case "bad":
            cpu.propBattery = BatteryStates.BAD;
            break;
          case "not_available":
            cpu.propBattery = BatteryStates.NOT_AVAILABLE;
            break;
          case "not_tested":
            cpu.propBattery = BatteryStates.NOT_TESTED;
            break;
          case "ok":
            cpu.propBattery = BatteryStates.OK;
            break;
          case "undefined":
            cpu.propBattery = BatteryStates.UNDEFINED;
            break;
        }
      }
      Variable var = (Variable) null;
      reader.Read();
      do
      {
        if (reader.NodeType == XmlNodeType.Element && string.Compare(reader.Name, "Members") == 0)
          var.ReadMemberVariables(ref reader, flags, var);
        else if (reader.NodeType == XmlNodeType.Element && string.Compare(reader.Name, "Connection") == 0)
          cpu.Connection.FromXmlTextReader(ref reader, flags, (Base) cpu);
        else if (reader.NodeType == XmlNodeType.Element && string.Compare(reader.Name, "ModuleCollection") == 0)
          cpu.Modules.FromXmlTextReader(ref reader, flags, (Base) cpu);
        else if (reader.NodeType == XmlNodeType.Element && string.Compare(reader.Name, "Module") == 0)
          cpu.Modules.FromXmlTextReader(ref reader, flags, (Base) cpu);
        else if (reader.NodeType == XmlNodeType.Element && string.Compare(reader.Name, "IODataPointCollection") == 0)
          cpu.IODataPoints.FromXmlTextReader(ref reader, flags, (Base) cpu);
        else if (reader.NodeType == XmlNodeType.Element && string.Compare(reader.Name, "IODataPoint") == 0)
          cpu.IODataPoints.FromXmlTextReader(ref reader, flags, (Base) cpu);
        else if (reader.NodeType == XmlNodeType.Element && string.Compare(reader.Name, "MemoryCollection") == 0)
          cpu.Memories.FromXmlTextReader(ref reader, flags, (Base) cpu);
        else if (reader.NodeType == XmlNodeType.Element && string.Compare(reader.Name, "Memory") == 0)
          cpu.Memories.FromXmlTextReader(ref reader, flags, (Base) cpu);
        else if (reader.NodeType == XmlNodeType.Element && string.Compare(reader.Name, "LibraryCollection") == 0)
          cpu.Libraries.FromXmlTextReader(ref reader, flags, (Base) cpu);
        else if (reader.NodeType == XmlNodeType.Element && string.Compare(reader.Name, "Library") == 0)
          cpu.Libraries.FromXmlTextReader(ref reader, flags, (Base) cpu);
        else if (reader.NodeType == XmlNodeType.Element && string.Compare(reader.Name, "LoggerCollection") == 0)
          cpu.Loggers.FromXmlTextReader(ref reader, flags, (Base) cpu);
        else if (reader.NodeType == XmlNodeType.Element && string.Compare(reader.Name, "Logger") == 0)
          cpu.Loggers.FromXmlTextReader(ref reader, flags, (Base) cpu);
        else if (reader.NodeType == XmlNodeType.Element && string.Compare(reader.Name, "TaskCollection") == 0)
          cpu.Tasks.FromXmlTextReader(ref reader, flags, (Base) cpu);
        else if (reader.NodeType == XmlNodeType.Element && string.Compare(reader.Name, "Task") == 0)
          cpu.Tasks.FromXmlTextReader(ref reader, flags, (Base) cpu);
        else if (reader.NodeType == XmlNodeType.Element && string.Compare(reader.Name, "VariableCollection") == 0)
          cpu.Variables.FromXmlTextReader(ref reader, flags, (Base) cpu);
        else if (reader.NodeType == XmlNodeType.Element && string.Compare(reader.Name, "Variable") == 0)
          cpu.Variables.FromXmlTextReader(ref reader, flags, (Base) cpu);
        else if (reader.NodeType == XmlNodeType.Element && string.Compare(reader.Name, "TaskClassCollection") == 0)
          cpu.TaskClasses.FromXmlTextReader(ref reader, flags, (Base) cpu);
        else if (reader.NodeType == XmlNodeType.Element && string.Compare(reader.Name, "TaskClass") == 0)
          cpu.TaskClasses.FromXmlTextReader(ref reader, flags, (Base) cpu);
        else
          reader.Read();
      }
      while (reader.NodeType != XmlNodeType.EndElement);
      reader.Read();
      return 0;
    }

    internal override int ToXMLTextWriter(ref XmlTextWriter writer, ConfigurationFlags flags)
    {
      writer.WriteStartElement(nameof (Cpu));
      int xmlTextWriter = base.ToXMLTextWriter(ref writer, flags);
      if (this.ApplicationMemory != null && this.ApplicationMemory.Length > 0)
        writer.WriteAttributeString("ApplicationMemory", this.ApplicationMemory);
      if (this.RuntimeVersion != null && this.RuntimeVersion.Length > 0)
        writer.WriteAttributeString("RuntimeVersion", this.RuntimeVersion);
      if (this.propSavePath != null && this.propSavePath.Length > 0)
        writer.WriteAttributeString("SavePath", this.SavePath);
      if (this.propAccu != BatteryStates.UNDEFINED)
        writer.WriteAttributeString("Accu", this.Accu.ToString());
      if (this.propBattery != BatteryStates.UNDEFINED)
        writer.WriteAttributeString("Battery", this.propBattery.ToString());
      if (!this.IsSG4Target)
        writer.WriteAttributeString("IsSG4Target", this.propIsSG4Target.ToString());
      if (this.ModUID != 0U)
        writer.WriteAttributeString("ModUID", this.ModUID.ToString());
      if (this.NodeNumber != (short) 0)
        writer.WriteAttributeString("NodeNumber", this.NodeNumber.ToString());
      if (this.Profiler != null)
        writer.WriteAttributeString("Profiler", this.Profiler.ToString());
      if (this.Type != null && this.Type.Length > 0)
        writer.WriteAttributeString("Type", this.Type);
      writer.WriteAttributeString("State", this.State.ToString());
      writer.WriteAttributeString("BootMode", this.BootMode.ToString());
      this.Connection.ToXMLTextWriter(ref writer, flags);
      this.IODataPoints.ToXMLTextWriter(ref writer, flags);
      this.Libraries.ToXMLTextWriter(ref writer, flags);
      this.Loggers.ToXMLTextWriter(ref writer, flags);
      this.Tasks.ToXMLTextWriter(ref writer, flags);
      this.Memories.ToXMLTextWriter(ref writer, flags);
      this.Modules.ToXMLTextWriter(ref writer, flags);
      this.Variables.ToXMLTextWriter(ref writer, flags);
      this.TaskClasses.ToXMLTextWriter(ref writer, flags);
      writer.WriteEndElement();
      return xmlTextWriter;
    }

    internal int ReadModuleList(
      string moduleName,
      out APIFC_ModulInfoRes modInfo,
      out APIFC_DiagModulInfoRes diagModInfo)
    {
      int num = 0;
      diagModInfo = new APIFC_DiagModulInfoRes();
      modInfo = new APIFC_ModulInfoRes();
      if (this.propModuleInfoList == null)
        this.propModuleInfoList = new Hashtable();
      if (this.propModuleInfoList.ContainsKey((object) moduleName))
      {
        object propModuleInfo = this.propModuleInfoList[(object) moduleName];
        if (propModuleInfo is APIFC_DiagModulInfoRes diagModulInfoRes)
          diagModInfo = diagModulInfoRes;
        else
          modInfo = (APIFC_ModulInfoRes) propModuleInfo;
      }
      else
      {
        num = this.UpdateModuleList(this.m_ModuListULOption);
        if (num == 0)
          return -3;
      }
      return num;
    }

    internal int RequestModuleList(ModuleListOptions lstOption)
    {
      AccessTypes nAccess = AccessTypes.ModuleList;
      if (this.propModuleInfoList != null)
        this.propModuleInfoList.Clear();
      this.propModuleInfoRequested = true;
      this.m_ModuListULOption = lstOption;
      int num;
      if (this.Connection.DeviceType == DeviceType.ANSLTcp)
      {
        num = this.ReadArgumentRequest(this.Service.hPvi, this.propLinkId, AccessTypes.ANSL_ModuleList, IntPtr.Zero, 0, 622U);
      }
      else
      {
        switch (this.m_ModuListULOption)
        {
          case ModuleListOptions.INA2000List:
            nAccess = AccessTypes.ModuleList;
            break;
          case ModuleListOptions.INA2000DiagnosisList:
            nAccess = AccessTypes.ReadDiagModList;
            break;
          default:
            if (this.BootMode == BootMode.Diagnostics)
            {
              nAccess = AccessTypes.ReadDiagModList;
              break;
            }
            break;
        }
        num = this.ReadArgumentRequest(this.Service.hPvi, this.propLinkId, nAccess, IntPtr.Zero, 0, 622U);
      }
      return num;
    }

    internal int UpdateModuleList(ModuleListOptions lstOption)
    {
      if (this.propModuleInfoRequested)
        return -1;
      if (!this.IsConnected && 12058 != this.propErrorCode)
      {
        this.Requests |= Actions.Upload;
        return -2;
      }
      if (12058 != this.propErrorCode)
        return this.RequestModuleList(lstOption);
      this.Requests |= Actions.Upload;
      return this.propErrorCode;
    }

    public event PviEventHandler Restarted;

    public event CpuEventHandler DateTimeRead;

    public event CpuEventHandler DateTimeWritten;

    public event PviEventHandler SavePathWritten;

    public event PviEventHandler SavePathRead;

    public event PviEventHandler ModuleDeleted;

    public event PviEventHandler TCPDestinationSettingsRead;

    public event PviEventHandler CpuInfoUpdated;

    public string ApplicationModuleFilter
    {
      get => this.propApplicationModuleFilter;
      set => this.propApplicationModuleFilter = value;
    }

    public int ResponseTimeout
    {
      get => this.propConnection.Device.ResponseTimeout;
      set => this.propConnection.Device.ResponseTimeout = value;
    }

    public Connection Connection
    {
      get => this.propConnection;
      set
      {
        this.propNewConnection = true;
        if (this.propConnection != null)
        {
          this.propConnection.Connected -= new PviEventHandler(this.ConnectionEvent);
          this.propConnection.ConnectionChanged -= new PviEventHandler(this.connection_ConnectionChanged);
          this.propConnection.Error -= new PviEventHandler(this.ConnectionEvent);
          this.propConnection.Disconnected -= new PviEventHandler(this.ConnectionDisconnected);
        }
        value.pviLineObj.propLinkId = this.propConnection.pviLineObj.propLinkId;
        value.pviDeviceObj.propLinkId = this.propConnection.pviDeviceObj.propLinkId;
        value.pviStationObj.propLinkId = this.propConnection.pviStationObj.propLinkId;
        if (!this.propConnection.propDeviceIsDirty && (this.propConnection.DeviceType == value.DeviceType || this.propConnection.DeviceType == DeviceType.ANSLTcp && value.DeviceType == DeviceType.TcpIp || value.DeviceType == DeviceType.ANSLTcp && this.propConnection.DeviceType == DeviceType.TcpIp))
          value.propDeviceIsDirty = false;
        value.propLineDesc = this.propConnection.propLineDesc;
        value.propDeviceDesc = this.propConnection.propDeviceDesc;
        this.propConnection = value;
        this.propConnection.Connected += new PviEventHandler(this.ConnectionEvent);
        this.propConnection.ConnectionChanged += new PviEventHandler(this.connection_ConnectionChanged);
        this.propConnection.Disconnected += new PviEventHandler(this.ConnectionDisconnected);
        this.propConnection.Error += new PviEventHandler(this.ConnectionEvent);
      }
    }

    public ModuleCollection Modules => this.propModules;

    public TaskCollection Tasks => this.propTasks;

    public VariableCollection Variables => this.propVariables;

    public MemoryCollection Memories => this.propMemories;

    public TaskClassCollection TaskClasses => this.propTaskClasses;

    public BR.AN.PviServices.HardwareInfo HardwareInfos => this.propHardwareInfos;

    public LibraryCollection Libraries => this.propLibraries;

    public int CompareRuntimeVersionTo(string vCompare)
    {
      if (this.propSWVersion == null)
        return -2;
      if (4 == this.propSWVersion.Length)
      {
        if ((int) vCompare[1] > (int) this.propSWVersion[1])
          return -1;
        if ((int) vCompare[1] < (int) this.propSWVersion[1])
          return 1;
        if ((int) vCompare[2] > (int) this.propSWVersion[2])
          return -1;
        if ((int) vCompare[2] < (int) this.propSWVersion[2])
          return 1;
        if ((int) vCompare[3] > (int) this.propSWVersion[3])
          return -1;
        if ((int) vCompare[3] < (int) this.propSWVersion[3])
          return 1;
      }
      if (5 == this.propSWVersion.Length)
      {
        if ((int) vCompare[4] > (int) this.propSWVersion[4])
          return -1;
        if ((int) vCompare[4] < (int) this.propSWVersion[4])
          return 1;
      }
      return 0;
    }

    public string RuntimeVersion
    {
      get
      {
        if (this.propSWVersion == null)
          return string.Empty;
        string runtimeVersion = this.propSWVersion;
        if (-1 == this.propSWVersion.IndexOf('.'))
        {
          if (this.propSWVersion.Length == 4)
            runtimeVersion = string.Format("{0}{1}.{2}{3}", (object) this.propSWVersion[0], (object) this.propSWVersion[1], (object) this.propSWVersion[2], (object) this.propSWVersion[3]);
          else if (this.propSWVersion.Length == 5)
            runtimeVersion = string.Format("{0}{1}.{2}{3}.{4}", (object) this.propSWVersion[0], (object) this.propSWVersion[1], (object) this.propSWVersion[2], (object) this.propSWVersion[3], (object) this.propSWVersion[4]);
        }
        if ('.' == runtimeVersion[3] && '0' == runtimeVersion[1])
          runtimeVersion = runtimeVersion[0].ToString() + runtimeVersion.Substring(2);
        return runtimeVersion;
      }
    }

    public string Type => this.propCPUType;

    [Obsolete("This property is no longer supported by ANSL!(Only valid for INA2000)")]
    public string ApplicationMemory => this.propAWSType;

    public short NodeNumber => (short) this.propNodeNumber;

    public BatteryStates Accu => this.propAccu;

    public byte CpuUsage => this.propCpuUsage;

    public BatteryStates Battery => this.propBattery;

    public BootMode BootMode => this.propInitDescription;

    public CpuState State => this.propState;

    public override string FullName
    {
      get
      {
        if (this.Name != null && 0 < this.Name.Length)
          return this.Parent.FullName + "." + this.Name;
        return this.Parent != null ? this.Parent.FullName : "";
      }
    }

    public override string PviPathName => this.Name != null && 0 < this.Name.Length ? (this.Connection != null ? this.Connection.PviPathName + "/\"" + this.Service.Name + "." + this.Name + "\" OT=Cpu" : this.Parent.PviPathName + "/\"" + this.Service.Name + "." + this.Name + "\" OT=Cpu") : (this.Connection != null ? this.Connection.PviPathName : this.Parent.PviPathName);

    public bool HasErrorLogBook => !this.propIsSG4Target;

    public bool IsSG4Target => this.propIsSG4Target;

    public IODataPointCollection IODataPoints => this.propIODataPoints;

    public string SavePath
    {
      get => this.propSavePath;
      set => this.propSavePath = value;
    }

    public LoggerCollection Loggers => this.propLoggers;

    public Profiler Profiler => this.propProfiler;

    internal PviObjectBrowser ObjectBrowser => this.propObjectBrowser;

    private void ConnectionDisconnected(object sender, PviEventArgs e) => this.OnDisconnected(new PviEventArgs(this.Name, this.Address, e.ErrorCode, this.Service.Language, Action.CpuDisconnect, this.Service));

    public int ReadTCPDestinationSettings()
    {
      int errorCode = this.ReadRequest(this.Service.hPvi, this.LinkId, AccessTypes.ResolveNodeNumber, 291U);
      if (errorCode != 0)
        this.OnError(new PviEventArgs(this.Name, this.Address, errorCode, this.Service.Language, Action.ResolveNodeNumber, this.Service));
      return 0;
    }

    protected virtual void OnTCPDestinationSettingsRead(string strData, PviEventArgs e)
    {
      this.propTCPDestinationSettings.Parse(strData);
      if (this.TCPDestinationSettingsRead == null)
        return;
      this.TCPDestinationSettingsRead((object) this, e);
    }

    public TcpDestinationSettings TcpDestinationSettings => this.propTCPDestinationSettings;

    [EditorBrowsable(EditorBrowsableState.Never)]
    [CLSCompliant(false)]
    [Browsable(false)]
    public int CallTTService(
      ushort ttGroup,
      byte ttServ,
      byte ttFormat,
      byte[] dataBytes,
      byte dataLen)
    {
      byte[] source = new byte[5 + (int) dataLen];
      source[0] = (byte) ((uint) ttGroup & (uint) byte.MaxValue);
      source[1] = (byte) (((int) ttGroup & 65280) >> 8);
      source[2] = ttServ;
      source[3] = ttFormat;
      source[4] = dataLen;
      for (int index = 0; index < (int) dataLen; ++index)
        source[5 + index] = dataBytes[index];
      IntPtr hMemory = PviMarshal.AllocHGlobal((IntPtr) source.Length);
      Marshal.Copy(source, 0, hMemory, source.Length);
      int num = this.ReadArgumentRequest(this.Service.hPvi, this.LinkId, AccessTypes.TTService, hMemory, source.Length, 214U, this.InternId);
      if (num != 0)
        this.OnError(new PviEventArgs(this.propName, this.propAddress, this.propErrorCode, this.Service.Language, Action.CpuTTService, this.Service));
      PviMarshal.FreeHGlobal(ref hMemory);
      return num;
    }

    [EditorBrowsable(EditorBrowsableState.Never)]
    [Browsable(false)]
    public int WriteDataToPhysicalAddress(
      int physicalAddress,
      byte[] dataBytes,
      int numOfBytesToWrite)
    {
      int length = numOfBytesToWrite;
      int[] source = new int[2]{ physicalAddress, 0 };
      if (numOfBytesToWrite == 0)
        length = dataBytes.Length;
      source[1] = length;
      IntPtr hMemory = PviMarshal.AllocHGlobal(8 + length);
      Marshal.Copy(source, 0, hMemory, 2);
      IntPtr destination = new IntPtr(hMemory.ToInt64() + 8L);
      Marshal.Copy(dataBytes, 0, destination, length);
      int physicalAddress1 = this.WriteRequest(this.Service.hPvi, this.LinkId, AccessTypes.WritePhysicalMemory, hMemory, 8 + length, this.InternId);
      PviMarshal.FreeHGlobal(ref hMemory);
      return physicalAddress1;
    }

    [EditorBrowsable(EditorBrowsableState.Never)]
    [Browsable(false)]
    public int ReadDataFromPhysicalAddress(int physicalAddress, int numOfBytesToRead)
    {
      int[] source = new int[2]
      {
        physicalAddress,
        numOfBytesToRead
      };
      IntPtr hMemory = PviMarshal.AllocHGlobal(8);
      Marshal.Copy(source, 0, hMemory, 2);
      int num = this.ReadArgumentRequest(this.Service.hPvi, this.LinkId, AccessTypes.ReadPhysicalMemory, hMemory, 8, 220U, this.InternId);
      PviMarshal.FreeHGlobal(ref hMemory);
      return num;
    }

    [EditorBrowsable(EditorBrowsableState.Never)]
    [Browsable(false)]
    public event CpuTTServiceEventHandler TTServiceResponse;

    private void OnTTService(
      int errorCode,
      PVIReadAccessTypes accessType,
      PVIDataStates dataState,
      IntPtr pData,
      uint dataLen)
    {
      int[] destination1 = new int[2];
      byte[] destination2 = new byte[2];
      if (5U < dataLen)
      {
        Marshal.Copy(pData, destination1, 0, 1);
        Marshal.Copy(new IntPtr(pData.ToInt64() + 2L), destination2, 0, 2);
      }
      if (this.TTServiceResponse == null)
        return;
      this.TTServiceResponse((object) this, new CpuTTServiceEventArgs(this.Name, this.Address, errorCode, this.Service.Language, Action.CpuTTService, (ushort) destination1[0], destination2[0], pData, destination2[1]));
    }

    [Browsable(false)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public event PviEventHandler PhysicalMemoryWritten;

    private void OnPhysicalMemoryWritten(int errorCode)
    {
      if (this.PhysicalMemoryWritten == null)
        return;
      this.PhysicalMemoryWritten((object) this, new PviEventArgs(this.Name, this.Address, errorCode, this.Service.Language, Action.CpuWritePhysicalMemory));
    }

    [EditorBrowsable(EditorBrowsableState.Never)]
    [Browsable(false)]
    public event CpuPhysicalMemReadEventHandler PhysicalMemoryRead;

    private void OnPhysicalMemoryRead(int errorCode, IntPtr pData, uint dataLen)
    {
      if (this.PhysicalMemoryRead == null)
        return;
      this.PhysicalMemoryRead((object) this, new CpuPhysicalMemReadEventArgs(this.Name, this.Address, errorCode, this.Service.Language, Action.CpuReadPhysicalMemory, pData, (int) dataLen));
    }

    public int GetListOfDongles(ref string dongleData) => this.ListDongles(ref dongleData, true);

    public int GetExistingLicense(string boxMask, string serialNumber, ref string existingLicData) => this.ListOfExistingLicenses(boxMask, serialNumber, ref existingLicData, true);

    public int GetRequiredLicenses(ref string requiredLicData) => this.ListOfRequiredLicenses(ref requiredLicData, true);

    public int GetLicenseContext(
      string boxMask,
      string serialNumber,
      string firmCode,
      ref string contextLicData)
    {
      string[] firmCodes = new string[1]{ firmCode };
      return this.ReadContext(boxMask, serialNumber, firmCodes, ref contextLicData, true);
    }

    public int GetLicenseContext(
      string boxMask,
      string serialNumber,
      string[] firmCodes,
      ref string contextLicData)
    {
      return this.ReadContext(boxMask, serialNumber, firmCodes, ref contextLicData, true);
    }

    public int UpdateLicenseData(string licenseData) => this.WriteLicenseData(licenseData, true);

    [CLSCompliant(false)]
    public int BlinkDongle(
      string boxMask,
      string serialNumber,
      uint ledColor,
      uint blinkCount,
      uint blinkTime)
    {
      return this.WriteLicenseBlinkDongle(boxMask, serialNumber, ledColor, blinkCount, blinkTime, true);
    }

    [CLSCompliant(false)]
    public int GetLicenseStatus(uint whichState, ref uint licStatus, bool aSync)
    {
      licStatus = 0U;
      return !aSync ? this.GetLicenseStatusSync(whichState, ref licStatus) : this.GetLicenseStatusASync(whichState);
    }

    [CLSCompliant(false)]
    public int GetLicenseStatus(uint whichState, ref uint licStatus) => this.GetLicenseStatusSync(whichState, ref licStatus);

    internal void PVICB_LIC_ReadFunc(int wParam, IntPtr lParam)
    {
      IntPtr zero = IntPtr.Zero;
      PInvokePvicom.PviComFnReadResponse(this.Service.hPvi, wParam, zero, 0);
    }

    internal void PVICB_LIC_WriteFunc(int wParam, IntPtr lParam) => PInvokePvicom.PviComFnWriteResponse(this.Service.hPvi, wParam);

    internal int LicenseDataWrtitten(int wParam, IntPtr lParam, ResponseInfo info)
    {
      int num = 0;
      if (350 == info.Type)
      {
        num = PInvokePvicom.PviComFnWriteResponse(this.Service.hPvi, wParam);
        this.licUpdateLicense = true;
      }
      else if (352 == info.Type)
      {
        num = PInvokePvicom.PviComFnWriteResponse(this.Service.hPvi, wParam);
        this.licBlinkDongle = true;
      }
      else
        this.PVICB_LIC_WriteFunc(wParam, lParam);
      return num;
    }

    internal int ReadLicenseData(
      int wParam,
      IntPtr lParam,
      ResponseInfo info,
      int dataLen,
      ref uint resultData)
    {
      IntPtr num1 = IntPtr.Zero;
      if (info.Error == 0)
        num1 = PviMarshal.AllocHGlobal(dataLen);
      int num2 = PInvokePvicom.PviComFnReadResponse(this.Service.hPvi, wParam, num1, dataLen);
      if (num2 == 0)
      {
        if (351 == info.Type)
          PviMarshal.Copy(num1, ref resultData);
        else
          this.PVICB_LIC_ReadFunc(wParam, lParam);
      }
      else if (351 != info.Type)
        this.PVICB_LIC_ReadFunc(wParam, lParam);
      return num2;
    }

    internal int ReadLicenseData(
      int wParam,
      IntPtr lParam,
      ResponseInfo info,
      int dataLen,
      ref string resultData)
    {
      IntPtr num1 = IntPtr.Zero;
      if (info.Error == 0)
        num1 = PviMarshal.AllocHGlobal(dataLen);
      int num2 = PInvokePvicom.PviComFnReadResponse(this.Service.hPvi, wParam, num1, dataLen);
      if (num2 == 0)
      {
        switch (info.Type)
        {
          case 346:
            resultData = PviMarshal.ToAnsiString(num1, dataLen);
            this.licListDongles = true;
            break;
          case 347:
            resultData = PviMarshal.ToAnsiString(num1, dataLen);
            this.licListOfExistingLicenses = true;
            break;
          case 348:
            resultData = PviMarshal.ToAnsiString(num1, dataLen);
            this.licListOfRequiredLicenses = true;
            break;
          case 349:
            resultData = PviMarshal.ToAnsiString(num1, dataLen);
            this.licReadContext = true;
            break;
          case 351:
            resultData = PviMarshal.ToAnsiString(num1, dataLen);
            break;
          default:
            this.PVICB_LIC_ReadFunc(wParam, lParam);
            break;
        }
      }
      else
      {
        switch (info.Type)
        {
          case 346:
            this.licListDongles = true;
            break;
          case 347:
            this.licListOfExistingLicenses = true;
            break;
          case 348:
            this.licListOfRequiredLicenses = true;
            break;
          case 349:
            this.licReadContext = true;
            break;
          default:
            this.PVICB_LIC_ReadFunc(wParam, lParam);
            break;
        }
      }
      return num2;
    }

    private int GetLicenseStatusASync(uint whichState)
    {
      IntPtr zero = IntPtr.Zero;
      PviMarshal.Copy(whichState, zero);
      int licenseStatusAsync = this.ReadArgumentRequest(this.Service.hPvi, this.LinkId, AccessTypes.LIC_GetLicenseStatus, zero, 4, 750U);
      PviMarshal.FreeHGlobal(ref zero);
      return licenseStatusAsync;
    }

    private uint ExtractLicStatus(string strXML)
    {
      uint maxValue = uint.MaxValue;
      XmlReader xmlReader = (XmlReader) null;
      try
      {
        xmlReader = XmlReader.Create((TextReader) new StringReader(strXML));
        int content = (int) xmlReader.MoveToContent();
        while (xmlReader.NodeType != XmlNodeType.Element || xmlReader.Name.CompareTo("Status") != 0)
        {
          if (!xmlReader.Read())
            goto label_9;
        }
        maxValue = uint.Parse(xmlReader.GetAttribute("Error"));
      }
      catch
      {
      }
      finally
      {
        xmlReader?.Close();
      }
label_9:
      return maxValue;
    }

    private int GetLicenseStatusSync(uint whichState, ref uint licStatus)
    {
      licStatus = 0U;
      IntPtr zero1 = IntPtr.Zero;
      IntPtr zero2 = IntPtr.Zero;
      PviMarshal.Copy(whichState, zero1);
      IntPtr hMemory = PviMarshal.AllocCoTaskMem(512);
      this.propLicStatusError = uint.MaxValue;
      this.propLicStatus = "";
      int licenseStatusSync = PInvokePvicom.PviComRead(this.Service.hPvi, this.LinkId, AccessTypes.LIC_GetLicenseStatus, zero1, 4, hMemory, 512);
      this.propLicStatus = PviMarshal.ToAnsiString(hMemory, 512);
      this.propLicStatusError = licStatus = this.ExtractLicStatus(this.propLicStatus);
      PviMarshal.FreeHGlobal(ref hMemory);
      PviMarshal.FreeHGlobal(ref zero1);
      return licenseStatusSync;
    }

    private int ListDongles(ref string dongleData, bool syncPVIAccess)
    {
      dongleData = "";
      int num;
      if (syncPVIAccess)
      {
        num = this.PviReadLicense(AccessTypes.LIC_ListDongles, "", ref dongleData);
        this.licListDongles = true;
      }
      else
        num = this.PviLicenseRead(AccessTypes.LIC_ListDongles, ref this.licListDongles, ref this.hWaitOnDongles, ref dongleData);
      return num;
    }

    private int ListDongles(ref string dongleData) => this.ListDongles(ref dongleData, false);

    private int ListOfExistingLicenses(
      string boxMask,
      string serialNumber,
      ref string existingLicData,
      bool syncPVIAccess)
    {
      existingLicData = "";
      string requestData = string.Format("BOXMASK={0} SERNUM={1}", (object) boxMask, (object) serialNumber);
      int num;
      if (syncPVIAccess)
      {
        num = this.PviReadLicense(AccessTypes.LIC_ListOfExistingLicenses, requestData, ref existingLicData);
        this.licListOfExistingLicenses = true;
      }
      else
        num = this.PviLicenseRead(AccessTypes.LIC_ListOfExistingLicenses, requestData, ref this.licListOfExistingLicenses, ref this.hWaitOnListOfExistingLicenses, ref existingLicData);
      return num;
    }

    private int ListOfExistingLicenses(
      string boxMask,
      string serialNumber,
      ref string existingLicData)
    {
      return this.ListOfExistingLicenses(boxMask, serialNumber, ref existingLicData, false);
    }

    private int ListOfRequiredLicenses(ref string requiredLicData, bool syncPVIAccess)
    {
      requiredLicData = "";
      int num;
      if (syncPVIAccess)
      {
        num = this.PviReadLicense(AccessTypes.LIC_ListOfRequiredLicenses, requiredLicData, ref requiredLicData);
        this.licListOfRequiredLicenses = true;
      }
      else
        num = this.PviLicenseRead(AccessTypes.LIC_ListOfRequiredLicenses, ref this.licListOfRequiredLicenses, ref this.hWaitOnListOfRequiredLicenses, ref requiredLicData);
      return num;
    }

    private int ListOfRequiredLicenses(ref string requiredLicData) => this.ListOfRequiredLicenses(ref requiredLicData, false);

    private int ReadContext(
      string boxMask,
      string serialNumber,
      string[] firmCodes,
      ref string contextLicData,
      bool syncPVIAccess)
    {
      string str = "";
      for (int index = 0; index < firmCodes.GetLength(0); ++index)
        str = index != 0 ? str + "," + firmCodes.GetValue(index).ToString() : firmCodes.GetValue(index).ToString();
      string requestData = string.Format("BOXMASK={0} SERNUM={1} FIRMCODE={2}", (object) boxMask, (object) serialNumber, (object) str);
      int num;
      if (syncPVIAccess)
      {
        num = this.PviReadLicense(AccessTypes.LIC_ReadContext, requestData, ref contextLicData);
        this.licReadContext = true;
      }
      else
        num = this.PviLicenseRead(AccessTypes.LIC_ReadContext, requestData, ref this.licReadContext, ref this.hWaitOnReadContext, ref contextLicData);
      return num;
    }

    private int ReadContext(
      string boxMask,
      string serialNumber,
      string[] firmCodes,
      ref string contextLicData)
    {
      return this.ReadContext(boxMask, serialNumber, firmCodes, ref contextLicData, false);
    }

    private int WriteLicenseData(string licenseData, bool syncPVIAccess)
    {
      int num;
      if (syncPVIAccess)
      {
        num = this.PviWriteLicense(AccessTypes.LIC_UpdateLicense, licenseData);
        this.licUpdateLicense = true;
      }
      else
        num = this.PviLicenseWrite(AccessTypes.LIC_UpdateLicense, licenseData, ref this.licUpdateLicense, ref this.hWaitOnUpdateLicense);
      return num;
    }

    private int WriteLicenseData(string licenseData) => this.WriteLicenseData(licenseData, false);

    private int PviReadLicense(AccessTypes accType, string requestData, ref string resultData)
    {
      int argDataLen = 0;
      IntPtr hMemory1 = IntPtr.Zero;
      IntPtr zero = IntPtr.Zero;
      IntPtr hMemory2 = PviMarshal.AllocCoTaskMem(1048576);
      if (!string.IsNullOrEmpty(requestData))
      {
        argDataLen = requestData.Length;
        hMemory1 = PviMarshal.StringToHGlobal(requestData);
      }
      int num = PInvokePvicom.PviComRead(this.Service.hPvi, this.LinkId, accType, hMemory1, argDataLen, hMemory2, 1048576);
      resultData = PviMarshal.ToAnsiString(hMemory2, 1048576);
      PviMarshal.FreeHGlobal(ref hMemory2);
      if (IntPtr.Zero != hMemory1)
        PviMarshal.FreeHGlobal(ref hMemory1);
      return num;
    }

    private int PviLicenseRead(
      AccessTypes accType,
      ref bool exitLicLoop,
      ref EventWaitHandle hWaitOObject,
      ref string resultData)
    {
      return this.PviLicenseRead(accType, (string) null, ref exitLicLoop, ref hWaitOObject, ref resultData);
    }

    private int PviLicenseRead(
      AccessTypes accType,
      string requestData,
      ref bool exitLicLoop,
      ref EventWaitHandle hWaitOObject,
      ref string licData)
    {
      uint resultUIData = 0;
      return this.ProcessPviLicenseRead(accType, requestData, ref exitLicLoop, ref hWaitOObject, 0, ref licData, ref resultUIData);
    }

    private int ProcessPviLicenseRead(
      AccessTypes accType,
      string requestData,
      ref bool exitLicLoop,
      ref EventWaitHandle hWaitOObject,
      int mode,
      ref string resultData,
      ref uint resultUIData)
    {
      int pParam = 0;
      uint pDataLen = 0;
      int wParam = 0;
      IntPtr zero = IntPtr.Zero;
      PviFunction respFnPtr = (PviFunction) null;
      int dataLen = 0;
      IntPtr hMemory = IntPtr.Zero;
      if (!string.IsNullOrEmpty(requestData))
      {
        dataLen = requestData.Length;
        hMemory = PviMarshal.StringToHGlobal(requestData);
      }
      exitLicLoop = false;
      int num1 = 0 >= dataLen ? PInvokePvicom.PviComFnReadRequest(this.Service.hPvi, this.LinkId, accType, this.cbLICReadFunc, 4294967292U, this.InternId) : PInvokePvicom.PviComFnReadArgumentRequest(this.Service.hPvi, this.LinkId, accType, hMemory, dataLen, this.cbLICReadFunc, 4294967292U, this.InternId);
      if (num1 == 0)
      {
        while (!exitLicLoop)
        {
          num1 = PInvokePvicom.PviComGetNextResponse(this.Service.hPvi, out wParam, zero, out respFnPtr, hWaitOObject.SafeWaitHandle);
          if (num1 == 0)
          {
            if (wParam == 0)
            {
              int num2 = (int) User32.WaitForSingleObject(hWaitOObject.SafeWaitHandle, 10U);
            }
            else
            {
              ResponseInfo pInfo = new ResponseInfo(0, 0, 0, 0, 0);
              num1 = PInvokePvicom.PviComFnGetResponseInfo(this.Service.hPvi, wParam, out pParam, out pDataLen, ref pInfo, (uint) Marshal.SizeOf(typeof (ResponseInfo)));
              switch (num1)
              {
                case 0:
                  if ((AccessTypes) pInfo.Type == accType && respFnPtr == this.cbLICReadFunc)
                  {
                    num1 = mode != 0 ? this.ReadLicenseData(wParam, zero, pInfo, (int) pDataLen, ref resultUIData) : this.ReadLicenseData(wParam, zero, pInfo, (int) pDataLen, ref resultData);
                    continue;
                  }
                  continue;
                case 12055:
                  continue;
                default:
                  goto label_10;
              }
            }
          }
          else
            break;
        }
      }
label_10:
      if (IntPtr.Zero != hMemory)
        PviMarshal.FreeHGlobal(ref hMemory);
      return num1;
    }

    private int PviWriteLicense(AccessTypes accType, string requestData)
    {
      IntPtr hMemory = IntPtr.Zero;
      int dataLen = 0;
      if (!string.IsNullOrEmpty(requestData))
      {
        dataLen = requestData.Length;
        hMemory = PviMarshal.StringToHGlobal(requestData);
      }
      int num = PInvokePvicom.PviComWrite(this.Service.hPvi, this.LinkId, accType, hMemory, dataLen, IntPtr.Zero, 0);
      if (IntPtr.Zero != hMemory)
        PviMarshal.FreeHGlobal(ref hMemory);
      return num;
    }

    private int PviLicenseWrite(
      AccessTypes accType,
      string requestData,
      ref bool exitLicLoop,
      ref EventWaitHandle hWaitOObject)
    {
      int pParam = 0;
      int wParam = 0;
      IntPtr zero = IntPtr.Zero;
      uint pDataLen = 0;
      PviFunction respFnPtr = (PviFunction) null;
      IntPtr pData = IntPtr.Zero;
      int dataLen = 0;
      if (!string.IsNullOrEmpty(requestData))
        pData = PviMarshal.StringToHGlobal(requestData);
      exitLicLoop = false;
      int num1 = PInvokePvicom.PviComFnWriteRequest(this.Service.hPvi, this.LinkId, accType, pData, dataLen, this.cbLICWriteFunc, 4294967292U, this.InternId);
      if (num1 == 0)
      {
        while (!exitLicLoop)
        {
          num1 = PInvokePvicom.PviComGetNextResponse(this.Service.hPvi, out wParam, zero, out respFnPtr, hWaitOObject.SafeWaitHandle);
          if (num1 == 0)
          {
            if (wParam == 0)
            {
              int num2 = (int) User32.WaitForSingleObject(hWaitOObject.SafeWaitHandle, uint.MaxValue);
            }
            else
            {
              ResponseInfo pInfo = new ResponseInfo(0, 0, 0, 0, 0);
              num1 = PInvokePvicom.PviComFnGetResponseInfo(this.Service.hPvi, wParam, out pParam, out pDataLen, ref pInfo, (uint) Marshal.SizeOf(typeof (ResponseInfo)));
              switch (num1)
              {
                case 0:
                  if ((AccessTypes) pInfo.Type == accType && respFnPtr == this.cbLICWriteFunc)
                  {
                    num1 = this.LicenseDataWrtitten(wParam, zero, pInfo);
                    continue;
                  }
                  continue;
                case 12055:
                  continue;
                default:
                  goto label_10;
              }
            }
          }
          else
            break;
        }
      }
label_10:
      return num1;
    }

    private int WriteLicenseBlinkDongle(
      string boxMask,
      string serialNumber,
      uint ledColor,
      uint blinkCount,
      uint blinkTime,
      bool syncPVIAccess)
    {
      IntPtr zero = IntPtr.Zero;
      string requestData = "BOXMASK=" + boxMask + " SERNUM=" + serialNumber + " LEDVIEW=" + ledColor.ToString() + "," + blinkCount.ToString() + "," + blinkTime.ToString();
      int num;
      if (syncPVIAccess)
      {
        num = this.PviWriteLicense(AccessTypes.LIC_BlinkDongle, requestData);
        this.licBlinkDongle = true;
      }
      else
        num = this.PviLicenseWrite(AccessTypes.LIC_BlinkDongle, requestData, ref this.licBlinkDongle, ref this.hWaitOnUpdateLicense);
      return num;
    }

    private int WriteLicenseBlinkDongle(
      string boxMask,
      string serialNumber,
      uint ledColor,
      uint blinkCount,
      uint blinkTime)
    {
      return this.WriteLicenseBlinkDongle(boxMask, serialNumber, ledColor, blinkCount, blinkTime, false);
    }

    [EditorBrowsable(EditorBrowsableState.Never)]
    [Browsable(false)]
    public event PviEventHandler GlobalForcedOFF;

    private void OnGlobalForcedOFF(int errorCode)
    {
      if (this.GlobalForcedOFF == null)
        return;
      this.GlobalForcedOFF((object) this, new PviEventArgs(this.Name, this.Address, errorCode, this.Service.Language, Action.CpuGlobalForceOFF));
    }

    public int GlobalForceOFF() => this.IsSG4Target ? this.WriteRequest(this.Service.hPvi, this.propLinkId, AccessTypes.LinkNodeForceOff, IntPtr.Zero, 0, 222U) : this.WriteRequest(this.Service.hPvi, this.propLinkId, AccessTypes.ForceOff, IntPtr.Zero, 0, 222U);

    public event PviEventHandler LicenseStatusGot;

    public string LicenseStatusInfo => this.propLicStatus;

    [CLSCompliant(false)]
    public uint LicenseStatusError => this.propLicStatusError;

    private void OnLicenseStatusRead(int errorCode, IntPtr pData, uint dataLen)
    {
      this.propLicStatusError = uint.MaxValue;
      this.propLicStatus = "";
      if (0U < dataLen && errorCode == 0)
      {
        this.propLicStatus = PviMarshal.ToAnsiString(pData, dataLen);
        this.propLicStatusError = this.ExtractLicStatus(this.propLicStatus);
      }
      if (this.LicenseStatusGot == null)
        return;
      this.LicenseStatusGot((object) this, new PviEventArgs(this.Address, this.Name, errorCode, this.Service.Language, Action.LIC_GetStatus, this.Service));
    }

    public event PviEventHandler MemoryCleared;

    private void OnMemoryCleared(int errorCode)
    {
      if (this.MemoryCleared == null)
        return;
      this.MemoryCleared((object) this, new PviEventArgs(this.propClearmemType.ToString(), this.Name, errorCode, this.Service.Language, Action.ClearMemory, this.Service));
    }

    public int ClearMemory(MemoryType memType)
    {
      int num;
      if (this.BootMode != BootMode.Diagnostics)
      {
        num = 4025;
      }
      else
      {
        this.propClearmemType = memType;
        int dataLen = Marshal.SizeOf(typeof (int));
        Marshal.WriteInt32(this.Service.RequestBuffer, (int) memType);
        num = this.WriteRequest(this.Service.hPvi, this.LinkId, AccessTypes.ClearMemory, this.Service.RequestBuffer, dataLen, 618U);
      }
      return num;
    }

    public string MemoryInfo => this.propMemoryInfo;

    public MemoryInformation MemoryInformationStruct => this.propMemoryInformationStruct;

    public int ReadMemoryInfo() => DeviceType.ANSLTcp != this.propConnection.DeviceType ? 12058 : this.ReadRequest(this.Service.hPvi, this.propLinkId, AccessTypes.ANSL_MemoryInfo, 726U);

    public event PviEventHandler MemoryInfoRead;

    private void OnMemoryInfoRead(int errorCode, IntPtr pData, uint dataLen)
    {
      if (this.MemoryInfoRead == null)
        return;
      this.propMemoryInfo = "";
      this.propMemoryInformationStruct = (MemoryInformation) null;
      if (0U < dataLen)
      {
        this.propMemoryInfo = PviMarshal.PtrToStringAnsi(pData);
        this.propMemoryInformationStruct = new MemoryInformation(this.propMemoryInfo);
      }
      this.MemoryInfoRead((object) this, new PviEventArgs(this.Name, this.Address, errorCode, this.Service.Language, Action.CpuReadMemoryInfo));
    }

    public string HardwareInfo => this.propHardwareInfo;

    public HardwareInformation HardwareInformationStruct => this.propHardwareInformationStruct;

    public int ReadHardwareInfo() => DeviceType.ANSLTcp != this.propConnection.DeviceType ? 12058 : this.ReadRequest(this.Service.hPvi, this.propLinkId, AccessTypes.ANSL_HardwareInfo, 727U);

    public event PviEventHandler HardwareInfoRead;

    private void OnHardwareInfoRead(int errorCode, IntPtr pData, uint dataLen)
    {
      if (this.HardwareInfoRead == null)
        return;
      this.propHardwareInfo = "";
      this.propHardwareInformationStruct = (HardwareInformation) null;
      if (0U < dataLen)
      {
        this.propHardwareInfo = PviMarshal.PtrToStringAnsi(pData);
        this.propHardwareInformationStruct = new HardwareInformation(this.propHardwareInfo);
      }
      this.HardwareInfoRead((object) this, new PviEventArgs(this.Name, this.Address, errorCode, this.Service.Language, Action.CpuReadHardwareInfo));
    }

    public int ReadApplicationInfo() => DeviceType.ANSLTcp != this.propConnection.DeviceType ? 12058 : this.ReadRequest(this.Service.hPvi, this.propLinkId, AccessTypes.ANSL_ApplicationInfo, 729U);

    public event PviEventHandlerXmlApplicationInfo ApplicationInfoRead;

    private void OnApplicationInfoRead(int errorCode, IntPtr pData, uint dataLen)
    {
      if (this.ApplicationInfoRead == null)
        return;
      this.ApplicationInfoRead((object) this, new PviEventArgsXMLApplicationInfo(this.Name, this.Address, errorCode, this.Service.Language, Action.CpuReadApplicationInfo, pData, dataLen));
    }

    public event PviEventHandler RedundancyInfoRead = delegate { };

    private void OnRedundancyInfoRead(int errorCode, IntPtr pData, uint dataLen)
    {
      if (dataLen <= 0U)
      {
        this.RedundancyInfo = "";
        this.RedundancyInformationStruct = (RedundancyInformation) null;
      }
      else
      {
        this.RedundancyInfo = PviMarshal.PtrToStringAnsi(pData);
        this.RedundancyInformationStruct = new RedundancyInformation(this.RedundancyInfo);
      }
      this.RedundancyInfoRead((object) this, new PviEventArgs(this.Name, this.Address, errorCode, this.Service.Language, Action.CpuReadRedundancyInfo));
    }

    public event PviEventHandler RedundancyInfoChanged;

    private void OnRedundancyInfoChanged(int errorCode, IntPtr pData, uint dataLen)
    {
      if (dataLen <= 0U)
      {
        this.RedundancyInfo = "";
        this.RedundancyInformationStruct = (RedundancyInformation) null;
      }
      else
      {
        this.RedundancyInfo = PviMarshal.PtrToStringAnsi(pData);
        this.RedundancyInformationStruct = new RedundancyInformation(this.RedundancyInfo);
      }
      if (this.RedundancyInfoChanged == null)
        return;
      this.RedundancyInfoChanged((object) this, new PviEventArgs(this.Name, this.Address, errorCode, this.Service.Language, Action.CpuEventRedundancyInfo));
    }

    public event PviEventHandler ActiveCpuChanged;

    private void OnActiveCpuChanged(PviEventArgs e)
    {
      if (this.ActiveCpuChanged == null)
        return;
      this.ActiveCpuChanged((object) this, e);
    }

    public event PviEventHandler ApplicationSynchronizeStarted;

    private void OnApplicationSynchronizeStarted(PviEventArgs e)
    {
      if (this.ApplicationSynchronizeStarted == null)
        return;
      this.ApplicationSynchronizeStarted((object) this, e);
    }

    public event PviProgressHandler ApplicationSyncing;

    private void OnRedundancyCtrlEvent(int errorCode, IntPtr pData, uint dataLen)
    {
      int num1 = errorCode;
      string str = 0U >= dataLen ? "<?xml version=\"1.0\" encoding=\"utf-8\"?>\n<CpuRedCtrl Cmd=\"2\" Percent=\"23\" Error=\"12055\" />\n" : PviMarshal.PtrToStringAnsi(pData);
      if (-1 != str.IndexOf("Cmd=\"1\""))
      {
        int num2 = str.IndexOf("Error=");
        if (0 < num2)
        {
          int num3 = str.IndexOf("\"", num2 + 8);
          num1 = int.Parse(str.Substring(num2 + 7, num3 - num2 - 7));
        }
        this.OnActiveCpuChanged(new PviEventArgs(this.Name, this.Address, num1, this.Service.Language, Action.CpuSwitchActiveCpu, this.Service));
      }
      else if (-1 != str.IndexOf("Cmd=\"16\""))
      {
        this.OnRedundancyInfoChanged(errorCode, pData, dataLen);
      }
      else
      {
        if (-1 == str.IndexOf("Cmd=\"2\"") || this.ApplicationSyncing == null)
          return;
        int num4 = str.IndexOf("Error=");
        if (0 < num4)
        {
          int num5 = str.IndexOf("\"", num4 + 8);
          num1 = int.Parse(str.Substring(num4 + 7, num5 - num4 - 7));
        }
        int percentage = 0;
        int num6 = str.IndexOf("Percent=");
        if (0 < num6)
        {
          int num7 = str.IndexOf("\"", num6 + 10);
          percentage = int.Parse(str.Substring(num6 + 9, num7 - num6 - 9));
        }
        this.ApplicationSyncing((object) this, new PviProgessEventArgs(this.Name, this.Address, num1, this.Service.Language, Action.CpuSynchronizeApplication, percentage));
      }
    }

    public int ReadRedundancyInfo() => DeviceType.ANSLTcp != this.propConnection.DeviceType ? 12058 : this.ReadRequest(this.Service.hPvi, this.propLinkId, AccessTypes.ANSL_RedundancyInfo, 722U);

    public bool RedundancyCommMode
    {
      get => this.propRedundancyCommMode;
      set => this.propRedundancyCommMode = value;
    }

    public string RedundancyInfo
    {
      get
      {
        lock (this.propRedundancyInfoLock)
          return this.propRedundancyInfo;
      }
      internal set
      {
        lock (this.propRedundancyInfoLock)
          this.propRedundancyInfo = value;
      }
    }

    public RedundancyInformation RedundancyInformationStruct
    {
      get
      {
        lock (this.propRedundancyInformationStructLock)
          return this.propRedundancyInformationStruct;
      }
      internal set
      {
        lock (this.propRedundancyInformationStructLock)
          this.propRedundancyInformationStruct = value;
      }
    }

    public int SwitchActiveCpu(bool force) => DeviceType.ANSLTcp != this.propConnection.DeviceType ? 12058 : this.WriteRequest(this.Service.hPvi, this.LinkId, AccessTypes.ANSL_RedundancyControl, !force ? "CMD=\"1\"" : "CMD=\"1\" ARG=\"1\"", 723U, this.propInternID);

    public int SwitchActiveCpu() => this.SwitchActiveCpu(false);

    public int SynchronizeRApplication(bool automaticMode) => DeviceType.ANSLTcp != this.propConnection.DeviceType ? 12058 : this.WriteRequest(this.Service.hPvi, this.LinkId, AccessTypes.ANSL_RedundancyControl, !automaticMode ? "CMD=\"2\" ARG=\"1\"" : "CMD=\"2\" ARG=\"0\"", 724U, this.propInternID);

    public event PviEventHandlerXmlData TOCRead;

    private void OnTOCRead(string name, string address, int error, IntPtr pData, uint dataLen)
    {
      string xmlData = (string) null;
      if (this.TOCRead == null)
        return;
      if (dataLen != 0U)
        xmlData = PviMarshal.PtrToStringAnsi(pData);
      this.TOCRead((object) this, new PviEventArgsXML(name, address, error, this.Service.Language, Action.CpuExtendedInfoTOC, xmlData));
    }

    public int ReadTOC() => DeviceType.ANSLTcp != this.propConnection.DeviceType ? 12058 : this.ReadArgumentRequest(this.Service.hPvi, this.LinkId, AccessTypes.ANSL_CpuExtendedInfo, "TYPE=1 ARG=0", 725U, this.propInternID);

    [EditorBrowsable(EditorBrowsableState.Never)]
    [Browsable(false)]
    public event PviEventHandlerXmlData XMLCommandSent;

    [Browsable(false)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public int SendXMLCommand(string commandData) => DeviceType.ANSLTcp != this.propConnection.DeviceType ? 12058 : this.WriteRequest(this.Service.hPvi, this.LinkId, AccessTypes.ANSL_COMMAND_Data, commandData, 730U, this.propInternID);

    private void OnXMLCommand(
      int errorCode,
      PVIDataStates dataState,
      int option,
      IntPtr pData,
      uint dataLen)
    {
      int error = errorCode;
      string xmlData = "";
      if (dataLen != 0U)
      {
        if (IntPtr.Zero != pData)
        {
          try
          {
            xmlData = PviMarshal.PtrToStringAnsi(pData);
          }
          catch
          {
            error = 12054;
          }
        }
      }
      if (this.XMLCommandSent == null)
        return;
      this.XMLCommandSent((object) this, new PviEventArgsXML(this.Name, this.Address, error, this.Service.Language, Action.CpuXMLCommand, xmlData));
    }
  }
}
