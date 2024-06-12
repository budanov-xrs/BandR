// Decompiled with JetBrains decompiler
// Type: BR.AN.PviServices.Task
// Assembly: BR.AN.PVIServices, Version=1.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DA853F0A-C070-4E30-B7A7-51D955FB85F6
// Assembly location: C:\Users\ss_six\Desktop\FXE\FXE software integration package\COMET_FXE-PVIServices_Example\Pluto-PVI-trial_01\packages\IToolS.BandR.3.5.58\lib\net20\BR.AN.PVIServices.dll

using System;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Xml;

namespace BR.AN.PviServices
{
  public class Task : Module
  {
    internal TracePointCollection propTracePoints;
    internal VariableCollection propVariables;
    internal VariableCollection propGlobals;
    private bool propTracePoints_Enabled;

    public Task(Cpu cpu, string name)
      : base(cpu, name)
    {
      if (cpu != null && cpu.Tasks[name] != null)
        Debugger.Launch();
      this.Init(cpu, name);
      this.propCpu.Tasks.Add(this);
    }

    internal Task(string name)
      : base((Cpu) null, name)
    {
      this.Init((Cpu) null, name);
    }

    public Task(Cpu cpu, string name, ref XmlTextReader reader, ConfigurationFlags flags)
      : base(cpu, name)
    {
      this.Init(cpu, name);
      this.FromXmlTextReader(ref reader, flags, (Base) this);
      this.propCpu.Tasks.Add(this);
    }

    internal Task(Cpu cpu, string name, TaskCollection collection)
      : base(cpu, name)
    {
      this.Init(cpu, name);
      collection.Add(this);
    }

    internal Task(Cpu cpu, PviObjectBrowser objBrowser, string name)
      : base(cpu, objBrowser, name)
    {
      this.Init(cpu, name);
      this.propCpu.Tasks.Add(this);
    }

    internal void Initialize(Task task) => this.propAddress = task.propAddress;

    internal void Init(Cpu cpu, string name)
    {
      this.propTracePoints_Enabled = false;
      this.reCreateActive = false;
      this.propCpu = cpu;
      this.propParent = (Base) cpu;
      this.propMODULEState = ConnectionType.None;
      this.propTracePoints = new TracePointCollection(this);
      this.propVariables = (VariableCollection) null;
      this.propGlobals = (VariableCollection) null;
      if (cpu != null)
      {
        this.propVariables = new VariableCollection(cpu.Service.CollectionType, (object) this, name + ".Variables");
        this.propGlobals = new VariableCollection(cpu.Service.CollectionType, (object) this, name + ".Variables");
      }
      else
      {
        this.propVariables = new VariableCollection(CollectionType.HashTable, (object) this, name + ".InternalVariables");
        this.propGlobals = new VariableCollection(CollectionType.HashTable, (object) this, name + ".InternalVariables");
      }
      if (this.propVariables != null)
      {
        this.propVariables.propInternalCollection = true;
        this.propGlobals.propInternalCollection = true;
      }
      if (this.Service == null)
        return;
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
    }

    internal void reCreateChildState()
    {
      if (0 >= this.propVariables.Count)
        return;
      foreach (Variable variable in (IEnumerable) this.propVariables.Values)
      {
        if (variable.isObjectConnected || variable.reCreateActive)
          variable.reCreateState();
      }
    }

    public override void Connect()
    {
      this.propReturnValue = 0;
      this.Connect(this.ConnectionType);
    }

    internal override void Connect(bool forceConnection)
    {
      this.propReturnValue = 0;
      this.Connect(forceConnection, this.ConnectionType);
    }

    public override void Connect(ConnectionType connectionType) => this.Connect(false, connectionType);

    protected override string getLinkDescription() => this.propTracePoints_Enabled ? "EV=edpsl" : "EV=edps";

    internal void Connect(bool forceConnection, ConnectionType connectionType)
    {
      this.ConnectionType = connectionType;
      this.propReturnValue = 0;
      if (this.reCreateActive || this.LinkId != 0U)
        return;
      if (ConnectionStates.Connected == this.propConnectionState || ConnectionStates.ConnectedError == this.propConnectionState)
      {
        this.Fire_ConnectedEvent((object) this, new PviEventArgs(this.Name, this.Address, this.propErrorCode, this.Service.Language, Action.TaskConnect, this.Service));
      }
      else
      {
        if (ConnectionStates.Connecting == this.propConnectionState)
          return;
        if (this.propAddress == null || this.propAddress.Length == 0)
          this.propAddress = this.propName;
        this.propConnectionState = ConnectionStates.Connecting;
        if (!this.propCpu.HasPVIConnection && this.Service.WaitForParentConnection)
        {
          if (!forceConnection)
          {
            this.Requests |= Actions.Connect;
            return;
          }
          if (Actions.Connect == (this.Requests & Actions.Connect))
            this.Requests &= Actions.GetList | Actions.GetValue | Actions.SetValue | Actions.GetForce | Actions.SetForce | Actions.Start | Actions.Stop | Actions.RunCycle | Actions.GetType | Actions.SetType | Actions.GetLength | Actions.SetRefresh | Actions.CreateLink | Actions.Upload | Actions.Download | Actions.SetActive | Actions.SetHysteresis | Actions.Disconnect | Actions.SetInitValue | Actions.GetLBType | Actions.ModuleInfo | Actions.ListSNMPVariables | Actions.Connected | Actions.Uploading | Actions.GetCpuInfo | Actions.ReadPVFormat | Actions.FireActivated | Actions.ReadIndex | Actions.FireDataValidated | Actions.FireValueChanged | Actions.Link;
        }
        this.propObjectParam = "CD=" + this.GetConnectionDescription();
        string linkDescription = this.getLinkDescription();
        if (!this.Service.IsStatic && this.ConnectionType == ConnectionType.CreateAndLink)
          this.propReturnValue = this.XCreateRequest(this.Service.hPvi, this.LinkName, ObjectType.POBJ_TASK, this.propObjectParam, 705U, linkDescription, 401U);
        else if (this.ConnectionType != ConnectionType.Link && this.propMODULEState != ConnectionType.Create)
          this.propReturnValue = this.XCreateRequest(this.Service.hPvi, this.LinkName, ObjectType.POBJ_TASK, this.propObjectParam, 0U, "", 401U);
        else
          this.propReturnValue = this.PviLinkObject(706U);
        if (this.propReturnValue == 0)
          return;
        this.OnError(new PviEventArgs(this.Name, this.Address, this.propReturnValue, this.Service.Language, Action.TaskConnect, this.Service));
      }
    }

    protected override string GetConnectionDescription() => !this.propTracePoints_Enabled ? base.GetConnectionDescription() : (LogicalObjectsUsage.ObjectNameWithType != this.Service.LogicalObjectsUsage ? string.Format("\"{0}\"/\"TP+{1}\"", (object) this.propParent.LinkName, (object) this.propAddress) : string.Format("\"TP+{0}\"", (object) this.propAddress));

    internal override int PviLinkObject(uint action)
    {
      string linkDescription = this.getLinkDescription();
      return !this.Service.IsStatic || this.ConnectionType == ConnectionType.Link ? this.XLinkRequest(this.Service.hPvi, this.LinkName, 705U, linkDescription, action) : this.XLinkRequest(this.Service.hPvi, this.LinkName, 705U, linkDescription, action);
    }

    public override void Disconnect()
    {
      this.propReturnValue = 0;
      this.propConnectionState = ConnectionStates.Disconnecting;
      this.propReturnValue = this.UnlinkRequest(402U);
      if (this.propReturnValue == 0)
        return;
      this.OnError(new PviEventArgs(this.Name, this.Address, this.propReturnValue, this.Service.Language, Action.TaskDisconnect, this.Service));
    }

    internal override void Resume()
    {
      this.propReturnValue = 0;
      this.propStartOrStopRequest = true;
      string str = "ST=Resume";
      IntPtr hglobal = PviMarshal.StringToHGlobal(str);
      this.propReturnValue = this.WriteRequest(this.Cpu.Service.hPvi, this.propLinkId, AccessTypes.Status, hglobal, str.Length, 403U);
      PviMarshal.FreeHGlobal(ref hglobal);
      if (this.propReturnValue == 0)
        return;
      this.OnError(new PviEventArgs(this.Name, this.Address, this.propReturnValue, this.Service.Language, Action.TaskStart, this.Service));
      this.OnStarted(new PviEventArgs(this.Name, this.Address, this.propReturnValue, this.Service.Language, Action.TaskStart, this.Service));
    }

    public void Start(int numberOfCycles)
    {
      this.propStartOrStopRequest = true;
      if (this.ProgramState == ProgramState.Running)
      {
        this.propReturnValue = 4124;
        this.OnError(new PviEventArgs(this.Name, this.Address, this.propReturnValue, this.Service.Language, Action.TaskStart, this.Service));
        this.OnStarted(new PviEventArgs(this.Name, this.Address, this.propReturnValue, this.Service.Language, Action.TaskStop, this.Service));
      }
      else
      {
        this.propReturnValue = 0;
        string str = "ST=Cycle(" + numberOfCycles.ToString() + ")";
        IntPtr hglobal = PviMarshal.StringToHGlobal(str);
        this.propReturnValue = this.WriteRequest(this.Cpu.Service.hPvi, this.LinkId, AccessTypes.Status, hglobal, str.Length, 410U);
        PviMarshal.FreeHGlobal(ref hglobal);
        if (this.propReturnValue != 0)
          this.OnError(new PviEventArgs(this.Name, this.Address, this.propReturnValue, this.Service.Language, Action.TaskStart));
        if (this.propReturnValue == 0)
          return;
        this.OnError(new PviEventArgs(this.Name, this.Address, this.propReturnValue, this.Service.Language, Action.TaskStart, this.Service));
        this.OnStarted(new PviEventArgs(this.Name, this.Address, this.propReturnValue, this.Service.Language, Action.TaskStart, this.Service));
      }
    }

    [Browsable(false)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete("This method is deprecated use \"Start(0)\" instead")]
    public override void Start() => this.Start(0);

    public int RunCycles(int numberOfCycles)
    {
      this.propStartOrStopRequest = true;
      if (this.ProgramState == ProgramState.Running)
      {
        this.propReturnValue = 4124;
        this.OnError(new PviEventArgs(this.Name, this.Address, this.propReturnValue, this.Service.Language, Action.TaskStart, this.Service));
        this.OnStarted(new PviEventArgs(this.Name, this.Address, this.propReturnValue, this.Service.Language, Action.TaskStop, this.Service));
        return this.propReturnValue;
      }
      this.propReturnValue = 0;
      string str = "ST=Cycle(" + numberOfCycles.ToString() + ")";
      IntPtr hglobal = PviMarshal.StringToHGlobal(str);
      this.propReturnValue = this.WriteRequest(this.Cpu.Service.hPvi, this.LinkId, AccessTypes.Status, hglobal, str.Length, 405U);
      PviMarshal.FreeHGlobal(ref hglobal);
      if (this.propReturnValue != 0)
        this.OnError(new PviEventArgs(this.Name, this.Address, this.propReturnValue, this.Service.Language, Action.TaskStart));
      if (this.propReturnValue != 0)
      {
        this.OnError(new PviEventArgs(this.Name, this.Address, this.propReturnValue, this.Service.Language, Action.TaskStart, this.Service));
        this.OnStarted(new PviEventArgs(this.Name, this.Address, this.propReturnValue, this.Service.Language, Action.TaskStart, this.Service));
      }
      return this.propReturnValue;
    }

    [Browsable(false)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public int RunCylcles(int numberOfCycles) => this.RunCycles(numberOfCycles);

    public override void Stop()
    {
      this.propStartOrStopRequest = true;
      if (this.ProgramState == ProgramState.Stopped)
      {
        this.propReturnValue = 4134;
        this.OnError(new PviEventArgs(this.Name, this.Address, this.propReturnValue, this.Service.Language, Action.TaskStop, this.Service));
        this.OnStopped(new PviEventArgs(this.Name, this.Address, this.propReturnValue, this.Service.Language, Action.TaskStop, this.Service));
      }
      else
      {
        this.propReturnValue = 0;
        string str = "ST=Stop";
        IntPtr hglobal = PviMarshal.StringToHGlobal(str);
        this.propReturnValue = this.WriteRequest(this.Cpu.Service.hPvi, this.LinkId, AccessTypes.Status, hglobal, str.Length, 404U);
        PviMarshal.FreeHGlobal(ref hglobal);
        if (this.propReturnValue != 0)
          this.OnError(new PviEventArgs(this.Name, this.Address, this.propReturnValue, this.Service.Language, Action.TaskStop));
        if (this.propReturnValue == 0)
          return;
        this.OnError(new PviEventArgs(this.Name, this.Address, this.propReturnValue, this.Service.Language, Action.TaskStop, this.Service));
        this.OnStopped(new PviEventArgs(this.Name, this.Address, this.propReturnValue, this.Service.Language, Action.TaskStop, this.Service));
      }
    }

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
      e.propAction = Action.TaskConnect;
      base.OnConnected(e);
      if (flag)
      {
        this.Cpu.Tasks.OnConnected((Base) this, e);
        if (0 < this.propVariables.Count)
        {
          foreach (Variable variable in (IEnumerable) this.propVariables.Values)
          {
            if ((variable.Requests & Actions.Connect) != Actions.NONE)
            {
              variable.Requests &= Actions.GetList | Actions.GetValue | Actions.SetValue | Actions.GetForce | Actions.SetForce | Actions.Start | Actions.Stop | Actions.RunCycle | Actions.GetType | Actions.SetType | Actions.GetLength | Actions.SetRefresh | Actions.CreateLink | Actions.Upload | Actions.Download | Actions.SetActive | Actions.SetHysteresis | Actions.Disconnect | Actions.SetInitValue | Actions.GetLBType | Actions.ModuleInfo | Actions.ListSNMPVariables | Actions.Connected | Actions.Uploading | Actions.GetCpuInfo | Actions.ReadPVFormat | Actions.FireActivated | Actions.ReadIndex | Actions.FireDataValidated | Actions.FireValueChanged | Actions.Link;
              variable.propConnectionState = ConnectionStates.Unininitialized;
              variable.Connect(true);
            }
          }
          if ((this.propVariables.Requests & Actions.Upload) != Actions.NONE)
          {
            this.propVariables.Upload();
            this.propVariables.Requests &= Actions.Connect | Actions.GetList | Actions.GetValue | Actions.SetValue | Actions.GetForce | Actions.SetForce | Actions.Start | Actions.Stop | Actions.RunCycle | Actions.GetType | Actions.SetType | Actions.GetLength | Actions.SetRefresh | Actions.CreateLink | Actions.Download | Actions.SetActive | Actions.SetHysteresis | Actions.Disconnect | Actions.SetInitValue | Actions.GetLBType | Actions.ModuleInfo | Actions.ListSNMPVariables | Actions.Connected | Actions.Uploading | Actions.GetCpuInfo | Actions.ReadPVFormat | Actions.FireActivated | Actions.ReadIndex | Actions.FireDataValidated | Actions.FireValueChanged | Actions.Link;
          }
        }
        if (this.propUserCollections != null && this.propUserCollections.Count > 0)
        {
          foreach (BaseCollection baseCollection in (IEnumerable) this.propUserCollections.Values)
          {
            if (baseCollection is TaskCollection)
              baseCollection.OnConnected((Base) this, e);
            if (baseCollection is ModuleCollection)
              baseCollection.OnConnected((Base) this, e);
          }
        }
      }
      if (!this.propReCreateActive)
        return;
      this.propReCreateActive = false;
      if (!this.Service.WaitForParentConnection)
        return;
      this.reCreateChildState();
    }

    protected override void OnDisconnected(PviEventArgs e)
    {
      if (Service.IsRemoteError(e.ErrorCode) && ConnectionStates.Unininitialized < this.propConnectionState && ConnectionStates.Disconnecting > this.propConnectionState)
        this.reCreateActive = true;
      e.propAction = Action.TaskDisconnect;
      if (this.Cpu != null && this.Cpu.Tasks != null)
        this.Cpu.Tasks.OnDisconnected(this, e);
      base.OnDisconnected(e);
    }

    protected internal override void OnError(PviEventArgs e)
    {
      base.OnError(e);
      if (this.Cpu != null)
        this.Cpu.Tasks.OnError(this, e);
      if (this.propUserCollections == null || this.propUserCollections.Count <= 0)
        return;
      foreach (BaseCollection baseCollection in (IEnumerable) this.propUserCollections.Values)
        baseCollection.OnError((Base) this, e);
    }

    internal override void Dispose(bool disposing, bool removeFromCollection)
    {
      if (this.propDisposed || !disposing)
        return;
      if (this.propTracePoints != null)
      {
        this.propTracePoints.Dispose();
        this.propTracePoints = (TracePointCollection) null;
      }
      if (this.propVariables != null)
      {
        this.propVariables.Dispose(disposing, removeFromCollection);
        this.propVariables = (VariableCollection) null;
      }
      if (this.propGlobals != null)
      {
        this.propGlobals.Dispose(disposing, removeFromCollection);
        this.propGlobals = (VariableCollection) null;
      }
      if (removeFromCollection)
        this.RemoveObject();
      this.propErrorText = (string) null;
      base.Dispose(disposing, removeFromCollection);
    }

    internal override void RemoveObject()
    {
      this.Remove();
      if (this.Cpu == null || this.propUserCollections == null || 0 >= this.propUserCollections.Count)
        return;
      foreach (object obj in (IEnumerable) this.propUserCollections.Values)
      {
        switch (obj)
        {
          case ModuleCollection _:
            ((BaseCollection) obj).Remove(this.Name);
            continue;
          case TaskCollection _:
            ((BaseCollection) obj).Remove(this.Name);
            continue;
          case LoggerCollection _:
            ((BaseCollection) obj).Remove(this.Name);
            continue;
          case LibraryCollection _:
            ((BaseCollection) obj).Remove(this.Name);
            continue;
          default:
            continue;
        }
      }
    }

    internal override void RemoveFromBaseCollections()
    {
      if (this.Service != null)
      {
        this.Service.LogicalObjects.Remove(this.LogicalName);
        if (this.Service.Services != null)
          this.Service.Services.LogicalObjects.Remove(this.LogicalName);
      }
      if (this.propUserCollections == null || 0 >= this.propUserCollections.Count)
        return;
      foreach (BaseCollection baseCollection in (IEnumerable) this.propUserCollections.Values)
        baseCollection.Remove(this.Name);
    }

    public override int FromXmlTextReader(
      ref XmlTextReader reader,
      ConfigurationFlags flags,
      Base baseObj)
    {
      Task task = (Task) baseObj;
      if (task == null)
        return -1;
      base.FromXmlTextReader(ref reader, flags, (Base) task);
      do
      {
        if (reader.NodeType == XmlNodeType.Comment)
          reader.Read();
        else if (reader.Name.ToLower().CompareTo("variablecollection") == 0)
        {
          task.Variables.FromXmlTextReader(ref reader, flags, (Base) task);
          reader.Read();
        }
        else if (reader.Name.ToLower().CompareTo("variable") == 0)
          task.Variables.FromXmlTextReader(ref reader, flags, (Base) task);
        else
          break;
      }
      while (reader.NodeType != XmlNodeType.EndElement);
      if (reader.NodeType == XmlNodeType.EndElement && reader.Name.ToLower().CompareTo("task") == 0)
        reader.Read();
      return 0;
    }

    internal override int ToXMLTextWriter(ref XmlTextWriter writer, ConfigurationFlags flags)
    {
      writer.WriteStartElement(nameof (Task));
      int xmlTextWriter = this.SaveModuleConfiguration(ref writer, flags);
      this.Variables.ToXMLTextWriter(ref writer, flags);
      writer.WriteEndElement();
      return xmlTextWriter;
    }

    [Browsable(false)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public TracePointCollection TracePoints => this.propTracePoints;

    public VariableCollection Variables => this.propVariables;

    public VariableCollection Globals => this.propGlobals;

    public override string FullName => this.propName != null && 0 < this.Name.Length ? (this.Parent != null ? this.Parent.FullName + "." + this.Name : this.Name) : (this.Parent != null ? this.Parent.FullName : this.Name);

    public override string PviPathName
    {
      get
      {
        if (this.Name == null || 0 >= this.Name.Length)
          return this.Parent.PviPathName;
        return this.Parent != null ? this.Parent.PviPathName + "/\"" + this.propName + "\" OT=Task" : "\"" + this.propName + "\" OT=Task";
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
      int propErrorCode = this.propErrorCode;
      this.propErrorCode = errorCode;
      if (eventType == EventTypes.TracePointsDataChanged)
        this.OnTracePointsDataChanged(errorCode, option);
      else
        base.OnPviEvent(errorCode, eventType, dataState, pData, dataLen, option);
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
          this.propErrorCode = 2;
          break;
        case PVIReadAccessTypes.ANSL_TaskInfo:
          this.ANSLModuleDescriptionRead(pData, dataLen, errorCode);
          this.Fire_OnConnected(new PviEventArgs(this.Name, this.Address, errorCode, this.Service.Language, Action.ConnectedEvent, this.Service));
          break;
        case PVIReadAccessTypes.ANSL_TracePointsReadData:
          this.OnTracePointsRead(pData, dataLen, errorCode, option);
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
        case PVIWriteAccessTypes.ANSL_TracePointsRegister:
          this.OnTracePointsRegistered(pData, dataLen, errorCode, option);
          break;
        case PVIWriteAccessTypes.ANSL_TracePointsUnregister:
          this.OnTracePointsUnRegistered(errorCode, option);
          break;
        default:
          base.OnPviWritten(errorCode, accessType, dataState, option, pData, dataLen);
          break;
      }
    }

    protected override int ModuleInfoRequest()
    {
      int num = 0;
      if (!this.propModuleInfoRequested)
      {
        this.propModuleInfoRequested = true;
        num = this.ReadArgumentRequest(this.Service.hPvi, this.propLinkId, AccessTypes.ANSL_TaskInfo, IntPtr.Zero, 0, 411U);
      }
      return num;
    }

    [Browsable(false)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public bool TracePoints_Enabled
    {
      get => this.propTracePoints_Enabled;
      set
      {
        if (this.propConnectionState != ConnectionStates.Unininitialized && this.propConnectionState != ConnectionStates.Disconnected)
          return;
        this.propTracePoints_Enabled = value;
      }
    }

    [Browsable(false)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public int TracePoints_Register(TracePointDescriptionCollection tracePoints)
    {
      int offset1 = 0;
      IntPtr num1 = IntPtr.Zero;
      int dataLen = 0;
      char val1 = ' ';
      if (tracePoints != null && 0 < tracePoints.Count)
      {
        dataLen = tracePoints.PVIDataSize;
        num1 = Marshal.AllocHGlobal(tracePoints.PVIDataSize);
        for (int indexer = 0; indexer < tracePoints.Count; ++indexer)
        {
          int val2 = 0;
          TracePointDescription tracePoint = tracePoints[indexer];
          PviMarshal.WriteUInt32(num1, offset1, tracePoint.RecordLen);
          int offset2 = offset1 + 4;
          PviMarshal.WriteUInt32(num1, offset2, tracePoint.ID);
          int ofs1 = offset2 + 4;
          Marshal.WriteInt32(num1, ofs1, val2);
          int ofs2 = ofs1 + 4;
          Marshal.WriteInt32(num1, ofs2, val2);
          int offset3 = ofs2 + 4;
          PviMarshal.WriteUInt64(num1, offset3, tracePoint.Offset);
          int num2 = offset3 + 8;
          for (int index = 0; index < tracePoint.ListOfVariables.Count; ++index)
          {
            if (0 < index)
            {
              Marshal.WriteByte(num1, num2, (byte) val1);
              ++num2;
            }
            string val3 = tracePoint.ListOfVariables[index].ToString();
            PviMarshal.WriteString(num1, num2, val3);
            num2 += val3.Length;
          }
          Marshal.WriteByte(num1, num2, (byte) 0);
          offset1 = num2 + 1;
        }
      }
      int num3 = this.WriteRequest(this.Cpu.Service.hPvi, this.propLinkId, AccessTypes.ANSL_TracePointsRegister, num1, dataLen, 412U);
      if (IntPtr.Zero != num1)
        Marshal.FreeHGlobal(num1);
      return num3;
    }

    [Browsable(false)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public event TPRegisterEventHandler TracePoints_Registered;

    private void OnTracePointsRegistered(IntPtr pData, uint dataLen, int errorCode, int option)
    {
      if (this.TracePoints_Registered == null)
        return;
      this.TracePoints_Registered((object) this, new TPFormatEventArgs(this.propName, this.propAddress, errorCode, this.Service.Language, Action.TaskRegisterTPs, pData, dataLen));
    }

    [Browsable(false)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public int TracePoints_Unregister() => this.WriteRequest(this.Cpu.Service.hPvi, this.propLinkId, AccessTypes.ANSL_TracePointsUnregister, IntPtr.Zero, 0, 413U);

    [Browsable(false)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public event PviEventHandler TracePoints_Unregistered;

    private void OnTracePointsUnRegistered(int errorCode, int option)
    {
      if (this.TracePoints_Unregistered == null)
        return;
      this.TracePoints_Unregistered((object) this, new PviEventArgs(this.propName, this.propAddress, errorCode, this.Service.Language, Action.TaskUnregisterTPs));
    }

    [Browsable(false)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public int TracePoints_ReadData() => this.ReadRequest(this.Cpu.Service.hPvi, this.propLinkId, AccessTypes.ANSL_TracePointsReadData, 414U);

    [Browsable(false)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public event TPDataEventHandler TracePoints_DataRead;

    private void OnTracePointsRead(IntPtr pData, uint dataLen, int errorCode, int option)
    {
      if (this.TracePoints_DataRead == null)
        return;
      this.TracePoints_DataRead((object) this, new TPDataEventArgs(this.propName, this.propAddress, errorCode, this.Service.Language, Action.TaskReadTPsData, pData, dataLen));
    }

    [Browsable(false)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public event PviEventHandler TracePoints_DataChanged;

    private void OnTracePointsDataChanged(int errorCode, int option)
    {
      if (this.TracePoints_DataChanged == null)
        return;
      this.TracePoints_DataChanged((object) this, new PviEventArgs(this.propName, this.propAddress, errorCode, this.Service.Language, Action.TracePointDataChanged));
    }
  }
}
