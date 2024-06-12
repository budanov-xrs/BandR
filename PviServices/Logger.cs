// Decompiled with JetBrains decompiler
// Type: BR.AN.PviServices.Logger
// Assembly: BR.AN.PVIServices, Version=1.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DA853F0A-C070-4E30-B7A7-51D955FB85F6
// Assembly location: C:\Users\ss_six\Desktop\FXE\FXE software integration package\COMET_FXE-PVIServices_Example\Pluto-PVI-trial_01\packages\IToolS.BandR.3.5.58\lib\net20\BR.AN.PVIServices.dll

using System;
using System.Collections;
using System.IO;
using System.Xml;

namespace BR.AN.PviServices
{
  public class Logger : Module
  {
    public const string KW_SYSLOGBOOK_NAME = "$arlogsys";
    public const string KW_USRLOGBOOK_NAME = "$arlogusr";
    private bool eIndexValid;
    private bool waitOnCancel;
    private LoggerEntry lastLoggerEntry;
    internal uint propContentVersion;
    private bool isClean;
    protected bool propReadRequestActive;
    private bool propIsArchive;
    internal static int DEFAULT_READ_BLOCKSIZE = 20;
    private uint propRequestContentVersion;
    private LoggerEntryCollection propLoggerEntries;
    private bool propContinuousActive;
    private bool propGlobalMerge;
    internal LoggerCollection propParentCollection;
    internal int readStartIndex;
    internal int entriesToRead;
    internal int entriesRead;
    private int logActID;
    private int oldLogID;
    private int readSize;
    private int readActID;
    private int readBlockSize;

    private bool isSGXDetectionLogger => this.propName.CompareTo("$Detect_SG4_SysLogger$") == 0;

    internal void ResetIndex() => this.eIndexValid = false;

    internal Logger(Cpu cpu, string name, bool doNotAddToCollections)
      : base(cpu, name, doNotAddToCollections)
    {
      this.lastLoggerEntry = (LoggerEntry) null;
      this.propLoggerEntries = (LoggerEntryCollection) null;
    }

    internal Logger(string name)
      : base((Cpu) null, name)
    {
      this.lastLoggerEntry = (LoggerEntry) null;
      this.propLoggerEntries = (LoggerEntryCollection) null;
      this.CleanInit();
    }

    public Logger(Cpu cpu, string name)
      : base(cpu, name)
    {
      if (cpu != null && cpu.Loggers[name] != null)
        throw new ArgumentException("There is already an object in \"" + cpu.Name + ".Loggers\" which has the same name! Use a different name and the same address or use the object from \"" + cpu.Name + ".Loggers\".", name);
      this.lastLoggerEntry = (LoggerEntry) null;
      this.propLoggerEntries = (LoggerEntryCollection) null;
      this.CleanInit();
      if (cpu == null)
        return;
      this.propCpu.Loggers.Add(this);
    }

    ~Logger() => this.Dispose(false, true);

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
      Service service = this.Service;
      base.Dispose(disposing, removeFromCollection);
      if (!disposing)
        return;
      this.propService = service;
      this.propParent = propParent;
      this.propLinkName = propLinkName;
      this.propLogicalName = propLogicalName;
      this.propUserData = propUserData;
      this.propName = propName;
      this.propAddress = propAddress;
      if (removeFromCollection)
        this.RemoveObject();
      if (this.propLoggerEntries != null)
      {
        this.propLoggerEntries.Dispose(disposing, removeFromCollection);
        this.propLoggerEntries = (LoggerEntryCollection) null;
      }
      if (this.propUserCollections != null)
      {
        this.propUserCollections.Clear();
        this.propUserCollections = (Hashtable) null;
      }
      this.propParentCollection = (LoggerCollection) null;
      this.propParent = (Base) null;
      this.propLinkName = (string) null;
      this.propLogicalName = (string) null;
      this.propUserData = (object) null;
      this.propService = (Service) null;
    }

    private void ResetReadIDs()
    {
      this.logActID = 0;
      this.readSize = 0;
      this.readActID = -1;
      this.readSize = 0;
      this.readStartIndex = 0;
      this.entriesToRead = 0;
      this.entriesRead = 0;
      this.oldLogID = -1;
    }

    internal Logger(Service service, string name)
      : base((object) service, name)
    {
      this.propConnectionState = ConnectionStates.Unininitialized;
      this.propIsArchive = false;
      this.lastLoggerEntry = new LoggerEntry();
      this.isClean = false;
      this.eIndexValid = false;
      this.readBlockSize = Logger.DEFAULT_READ_BLOCKSIZE;
      this.waitOnCancel = false;
      this.ResetReadIDs();
      this.propReadRequestActive = false;
      this.propLoggerEntries = new LoggerEntryCollection((Base) this, nameof (LoggerEntries));
      this.propGlobalMerge = false;
    }

    internal Logger(Service service, string name, bool isArchive)
      : base((object) service, name)
    {
      this.propConnectionState = ConnectionStates.Unininitialized;
      this.propIsArchive = isArchive;
      this.lastLoggerEntry = new LoggerEntry();
      this.isClean = false;
      this.eIndexValid = false;
      this.readBlockSize = Logger.DEFAULT_READ_BLOCKSIZE;
      this.waitOnCancel = false;
      this.ResetReadIDs();
      this.propReadRequestActive = false;
      this.propLoggerEntries = new LoggerEntryCollection((Base) this, nameof (LoggerEntries));
      this.propGlobalMerge = false;
    }

    internal override void RemoveObject()
    {
      this.LoggerEntries.Clear();
      this.Remove();
      if (this.Cpu == null)
        return;
      if (this.Cpu.Modules != null)
        this.Cpu.Modules.Remove(this.Name);
      if (this.propUserCollections == null || 0 >= this.propUserCollections.Count)
        return;
      foreach (BaseCollection baseCollection in (IEnumerable) this.propUserCollections.Values)
        baseCollection.Remove(this.Name);
    }

    private int ReadALL()
    {
      this.propReturnValue = 12011;
      this.CleanLoggerEntries(0);
      this.readActID = this.logActID;
      this.readStartIndex = this.logActID;
      this.entriesToRead = 1 + this.readStartIndex;
      this.entriesRead = 0;
      this.readSize = this.entriesToRead;
      this.oldLogID = -1;
      if (this.readSize > this.readBlockSize)
        this.readSize = this.readBlockSize;
      if (!this.propReadRequestActive)
        this.propReturnValue = this.Read(this.readSize, this.readStartIndex, Action.LoggerReadBlock);
      return this.propReturnValue;
    }

    public int ReadLoggerInfo() => this.ReadRequest(this.Service.hPvi, this.LinkId, AccessTypes.ANSL_LoggerModuleInfo, 4460U);

    public uint ContentVersion => this.propContentVersion;

    public event PviEventHandlerXmlData LoggerInfoRead;

    private int OnLoggerInfoRead(
      int errorCode,
      PVIReadAccessTypes accessType,
      IntPtr pData,
      uint dataLen)
    {
      int error = errorCode;
      string str = "";
      XmlReader xmlReader = (XmlReader) null;
      this.propContentVersion = 0U;
      if (dataLen != 0U)
      {
        if (IntPtr.Zero != pData)
        {
          try
          {
            str = PviMarshal.PtrToStringAnsi(pData, dataLen);
            xmlReader = XmlReader.Create((TextReader) new StringReader(str));
            int content = (int) xmlReader.MoveToContent();
            if (xmlReader.Name.CompareTo("LoggerInfo") == 0)
            {
              string attribute = xmlReader.GetAttribute("Version");
              if (!string.IsNullOrEmpty(attribute))
                this.propContentVersion = System.Convert.ToUInt32(attribute);
            }
          }
          catch
          {
            error = 12054;
          }
          finally
          {
            xmlReader?.Close();
          }
        }
      }
      if (this.LoggerInfoRead != null)
        this.LoggerInfoRead((object) this, new PviEventArgsXML(this.Name, this.Address, error, this.Service.Language, Action.LoggerINFORead, str));
      return error;
    }

    [CLSCompliant(false)]
    public virtual void Read(uint contentVersion)
    {
      if (contentVersion == 0U)
        this.Read();
      else
        this.ReadEntries(contentVersion);
    }

    public virtual void Read() => this.ReadEntries(0U);

    private void ReadEntries(uint contentVersion)
    {
      this.propRequestContentVersion = contentVersion;
      this.propReturnValue = 0;
      if (this.propReadRequestActive)
      {
        this.propReturnValue = this.CancelRequests();
        if (this.waitOnCancel)
          return;
      }
      if (!this.propContinuousActive)
        this.ReadIndex(Action.LoggerIndexForUpdate);
      else
        this.ReadIndex(Action.LoggerIndexForRead);
    }

    [CLSCompliant(false)]
    public virtual void Read(int count, uint contentVersion) => this.ReadEntriesOldStyle(count, contentVersion);

    public virtual void Read(int count) => this.ReadEntriesOldStyle(count, 0U);

    private void ReadEntriesOldStyle(int count, uint contentVersion)
    {
      this.propReturnValue = 0;
      string request = "DN=" + count.ToString();
      this.propReadRequestActive = true;
      this.propRequestContentVersion = contentVersion;
      if (0U < this.propRequestContentVersion)
        request = string.Format("DN={0} VI={1}", (object) count.ToString(), (object) this.propRequestContentVersion.ToString());
      this.Service.BuildRequestBuffer(request);
      if (0U < this.propRequestContentVersion)
        this.propReturnValue = this.ReadArgumentRequest(this.propParent.Service.hPvi, this.propLinkId, AccessTypes.ANSL_LoggerModuleData, this.Service.RequestBuffer, request.Length, 900U, this.propParent.propInternID);
      else
        this.propReturnValue = this.ReadArgumentRequest(this.propParent.Service.hPvi, this.propLinkId, AccessTypes.ModuleData, this.Service.RequestBuffer, request.Length, 900U, this.propParent.propInternID);
    }

    internal virtual void ReadEntry(int id)
    {
      this.propReturnValue = 0;
      string request = "DN=1 ID=" + id.ToString();
      this.propReadRequestActive = true;
      this.Service.BuildRequestBuffer(request);
      this.propReturnValue = this.ReadArgumentRequest(this.propParent.Service.hPvi, this.propLinkId, AccessTypes.ModuleData, this.Service.RequestBuffer, request.Length, 901U, this.propParent.propInternID);
    }

    internal virtual int Read(int count, int id, Action action)
    {
      string request = string.Format("DN={0} ID={1}", (object) count.ToString(), (object) id.ToString());
      this.propReadRequestActive = true;
      if (0U < this.propRequestContentVersion)
        request = string.Format("DN={0} ID={1} VI={2}", (object) count.ToString(), (object) id.ToString(), (object) this.propRequestContentVersion.ToString());
      this.Service.BuildRequestBuffer(request);
      return 0U >= this.propRequestContentVersion ? this.ReadArgumentRequest(this.propParent.Service.hPvi, this.propLinkId, AccessTypes.ModuleData, this.Service.RequestBuffer, request.Length, (uint) action, this.propParent.propInternID) : this.ReadArgumentRequest(this.propParent.Service.hPvi, this.propLinkId, AccessTypes.ANSL_LoggerModuleData, this.Service.RequestBuffer, request.Length, (uint) action, this.propParent.propInternID);
    }

    internal virtual void ReadIndex(Action action) => this.propReturnValue = this.ReadRequest(this.Service.hPvi, this.propLinkId, AccessTypes.Status, (uint) action);

    public override void Connect(ConnectionType connectionType)
    {
      if (this.reCreateActive || this.LinkId != 0U)
        return;
      if (ConnectionStates.Connected == this.propConnectionState || ConnectionStates.ConnectedError == this.propConnectionState)
      {
        this.Fire_ConnectedEvent((object) this, new PviEventArgs(this.Name, this.Address, this.propErrorCode, this.Service.Language, Action.LoggerConnect, this.Service));
      }
      else
      {
        if (ConnectionStates.Unininitialized < this.propConnectionState && ConnectionStates.Disconnecting > this.propConnectionState)
          return;
        this.propReturnValue = 0;
        this.ConnectionType = connectionType;
        this.Connect(false, connectionType, 909U);
      }
    }

    internal int ConnectEx()
    {
      int num = 0;
      if (this.HasPVIConnection)
        return this.ReadRequest(this.Service.hPvi, this.LinkId, AccessTypes.Status, 917U);
      if (ConnectionStates.Connecting != this.propConnectionState)
      {
        this.propConnectionState = ConnectionStates.Connecting;
        if (this.propAddress == null || this.propAddress.Length == 0)
          this.propAddress = this.propName;
        if (LogicalObjectsUsage.ObjectNameWithType == this.Service.LogicalObjectsUsage)
          this.propObjectParam = "CD=" + string.Format("\"{0}\"", (object) this.propAddress);
        else
          this.propObjectParam = "CD=" + string.Format("\"{0}\"/\"{1}\"", (object) this.propParent.LinkName, (object) this.propAddress);
        this.propLinkParam = this.getLinkDescription();
        num = this.XCreateRequest(this.Service.hPvi, this.LinkName, ObjectType.POBJ_MODULE, this.propObjectParam, 916U, this.propLinkParam, 916U);
      }
      return num;
    }

    internal void CleanInit()
    {
      this.propConnectionState = ConnectionStates.Unininitialized;
      this.propIsArchive = false;
      this.lastLoggerEntry = (LoggerEntry) null;
      this.lastLoggerEntry = new LoggerEntry();
      this.isClean = false;
      this.eIndexValid = false;
      this.readBlockSize = Logger.DEFAULT_READ_BLOCKSIZE;
      this.waitOnCancel = false;
      this.ResetReadIDs();
      this.propReadRequestActive = false;
      if (this.propLoggerEntries != null && this.Service != null)
      {
        if (this.propGlobalMerge)
        {
          if (this.Service.LoggerEntries != null && 0 < this.Service.LoggerEntries.Count && 0 < this.Service.LoggerEntries.Count)
          {
            foreach (LoggerEntry key in (IEnumerable) this.LoggerEntries.Values)
              this.Service.LoggerEntries.Remove((object) key);
          }
          this.OnGlobalRemoved(new LoggerEventArgs(this.Name, this.Address, 0, this.Service.Language, Action.LoggerGlobalRemoved, this.LoggerEntries));
        }
        this.propLoggerEntries.Clear();
        this.propLoggerEntries = (LoggerEntryCollection) null;
      }
      this.propLoggerEntries = new LoggerEntryCollection((Base) this, "LoggerEntries");
    }

    public virtual void Clear()
    {
      this.propReturnValue = 0;
      string request = "LD=Clear";
      this.Service.BuildRequestBuffer(request);
      this.propReturnValue = this.WriteRequest(this.Cpu.Service.hPvi, this.propLinkId, AccessTypes.Status, this.Service.RequestBuffer, request.Length, 905U);
      if (this.propReturnValue == 0)
        return;
      this.OnError(new PviEventArgs(this.Name, this.Address, this.propReturnValue, this.Service.Language, Action.LoggerClear, this.Service));
    }

    internal override void OnPviCreated(int errorCode, uint linkID)
    {
      this.propLinkId = linkID;
      this.propErrorCode = errorCode;
      if (errorCode == 0 || 12002 == errorCode)
      {
        if (this.Service.IsStatic && this.ConnectionType == ConnectionType.CreateAndLink)
        {
          this.propErrorCode = this.XLinkRequest(this.Service.hPvi, this.LinkName, 909U, this.propLinkParam, 909U);
        }
        else
        {
          this.logActID = -2;
          if (this.isSGXDetectionLogger)
            return;
          this.ResetReadIDs();
          this.ReadIndex(Action.LoggerIndexForConnect);
        }
      }
      else
      {
        this.OnConnected(new PviEventArgs(this.Name, this.Address, errorCode, this.Service.Language, Action.LoggerConnect, this.Service));
        this.OnError(new PviEventArgs(this.Name, this.Address, errorCode, this.Service.Language, Action.LoggerConnect, this.Service));
      }
    }

    internal override void OnPviLinked(int errorCode, uint linkID, int option) => base.OnPviLinked(errorCode, linkID, option);

    internal override void OnPviEvent(
      int errorCode,
      EventTypes eventType,
      PVIDataStates dataState,
      IntPtr pData,
      uint dataLen,
      int option)
    {
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
            if (!this.IsConnected)
              break;
            if (4813 == errorCode || 4812 == errorCode)
            {
              this.Disconnect();
              this.OnDeleted(new PviEventArgs(this.Name, this.Address, errorCode, this.Service.Language, Action.LoggerDelete, this.Service));
              break;
            }
            this.OnDisconnected(new PviEventArgs(this.Name, this.Address, errorCode, this.Service.Language, Action.LoggerConnect, this.Service));
            break;
          }
          if (ConnectionStates.Connecting == this.propConnectionState)
          {
            this.OnConnected(new PviEventArgs(this.Name, this.Address, errorCode, this.Service.Language, Action.ConnectedEvent, this.Service));
            break;
          }
          this.ReadModuleInfo();
          break;
        case EventTypes.Status:
          this.logActID = this.GetID(pData, dataLen);
          if (this.logActID < this.oldLogID)
            this.ReadALL();
          this.eIndexValid = true;
          if (errorCode != 0 || this.propReadRequestActive)
            break;
          if (this.readActID != this.logActID)
          {
            this.readStartIndex = this.logActID;
            this.entriesRead = 0;
            this.entriesToRead = this.readStartIndex - this.readActID;
            this.readActID = this.readStartIndex;
            this.readSize = this.entriesToRead;
            if (this.readSize > this.readBlockSize)
              this.readSize = this.readBlockSize;
            this.Read(this.readSize, this.readStartIndex, Action.LoggerReadBlock);
            break;
          }
          this.Read(1, this.logActID, Action.LoggerReadEntryForCompare);
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
          int logActId = this.logActID;
          this.logActID = this.GetID(pData, dataLen);
          if (this.logActID < this.oldLogID)
          {
            if (this.isSGXDetectionLogger)
              break;
            this.ReadALL();
            break;
          }
          switch (option)
          {
            case 1:
              if (0 > this.logActID)
              {
                this.OnError(new PviEventArgs(this.Name, this.Address, errorCode, this.Service.Language, Action.LoggerClear, this.Service));
                this.propReadRequestActive = false;
                return;
              }
              if (this.propReadRequestActive)
                return;
              this.ReadALL();
              return;
            case 2:
              if (0 > this.logActID)
              {
                base.OnError(new PviEventArgs("Incompatible PVI version for connecting logger", "", 0, "", Action.NONE, this.Service));
                return;
              }
              this.eIndexValid = true;
              this.OnConnected(new PviEventArgs(this.Name, this.Address, errorCode, this.Service.Language, Action.ConnectedEvent, this.Service));
              return;
            case 3:
              if (0 > this.logActID)
              {
                this.propReadRequestActive = false;
                return;
              }
              this.OnUpdateLoggerEntriesRequest(logActId, errorCode);
              return;
            case 4:
              if (0 > this.logActID)
              {
                this.OnError(new PviEventArgs(this.Name, this.Address, errorCode, this.Service.Language, Action.LoggerIndexForUpdate, this.Service));
                this.propReadRequestActive = false;
                return;
              }
              this.OnUpdateLoggerEntriesRequest(logActId, errorCode);
              return;
            default:
              this.OnError(new PviEventArgs(this.Name, this.Address, errorCode, this.Service.Language, Action.LoggerGetStatus, this.Service));
              this.propReadRequestActive = false;
              return;
          }
        case PVIReadAccessTypes.ModuleData:
        case PVIReadAccessTypes.ANSL_LoggerModuleData:
          switch (option)
          {
            case 1:
              this.accModuleData(errorCode, accessType, pData, dataLen, Action.LoggerReadBlockForAdded);
              return;
            case 2:
              this.accModuleData(errorCode, accessType, pData, dataLen, Action.LoggerReadBlock);
              return;
            case 3:
              this.accModuleData(errorCode, accessType, pData, dataLen, Action.LoggerReadEntry);
              return;
            case 4:
              this.accModuleData(errorCode, accessType, pData, dataLen, Action.LoggerReadEntryForCompare);
              return;
            default:
              this.accModuleData(errorCode, accessType, pData, dataLen, Action.LoggerRead);
              return;
          }
        case PVIReadAccessTypes.ModuleList:
          if (errorCode != 0)
            break;
          this.UpdateModuleInfo(pData, dataLen);
          break;
        case PVIReadAccessTypes.ANSL_LoggerModuleInfo:
          this.OnLoggerInfoRead(errorCode, accessType, pData, dataLen);
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
        case PVIWriteAccessTypes.EventMask:
          if (errorCode != 0)
            break;
          if (option == 0)
          {
            this.OnContinuousActivated(new PviEventArgs(this.Name, this.Address, errorCode, this.Service.Language, Action.LoggerContinuousActivate, this.Service));
            break;
          }
          this.OnContinuousDeactivated(new PviEventArgs(this.Name, this.Address, errorCode, this.Service.Language, Action.LoggerContinuousDeactivate, this.Service));
          break;
        case PVIWriteAccessTypes.State:
          this.ResetReadIDs();
          if (!this.CleanLoggerEntries(errorCode))
            break;
          this.OnCleared(new PviEventArgs(this.Name, this.Address, errorCode, this.Service.Language, Action.LoggerClear, this.Service));
          break;
        case PVIWriteAccessTypes.Cancel:
          if (!this.waitOnCancel)
            break;
          this.waitOnCancel = false;
          this.ReadIndex(Action.LoggerIndexForRead);
          break;
        default:
          base.OnPviWritten(errorCode, accessType, dataState, option, pData, dataLen);
          break;
      }
    }

    private int GetID(IntPtr ptrData, uint dataLen)
    {
      this.oldLogID = this.logActID;
      if (dataLen == 0U || IntPtr.Zero == ptrData)
        return -2;
      string stringAnsi = PviMarshal.PtrToStringAnsi(ptrData, dataLen);
      int num1 = stringAnsi.IndexOf("ID=");
      int num2 = stringAnsi.IndexOf("\0");
      if (num1 == -1)
        return -1;
      if (-1 == num2)
        num2 = stringAnsi.Length;
      return System.Convert.ToInt32(stringAnsi.Substring(num1 + 3, num2 - num1 - 3));
    }

    private string GetIDData(PviResponseData cb)
    {
      string idData = "";
      if (cb.DataLen != 0 && IntPtr.Zero != cb.PtrData)
        idData = PviMarshal.PtrToStringAnsi(cb.PtrData, cb.DataLen);
      return idData;
    }

    private int CancelRequests()
    {
      int num = 0;
      if (this.propReadRequestActive)
      {
        this.waitOnCancel = true;
        num = this.CancelRequest();
        this.propReadRequestActive = false;
      }
      return num;
    }

    private int OnUpdateLoggerEntriesRequest(int previousActID, int eCode)
    {
      if (!this.propReadRequestActive)
      {
        if (previousActID < this.logActID)
        {
          this.readActID = this.logActID;
          this.readStartIndex = this.logActID;
          this.entriesToRead = previousActID != 0 ? this.readStartIndex - previousActID : 1 + (this.readStartIndex - previousActID);
          this.entriesRead = 0;
          this.readSize = this.entriesToRead;
          if (this.readSize > this.readBlockSize)
            this.readSize = this.readBlockSize;
          return this.Read(this.readSize, this.readStartIndex, Action.LoggerReadBlock);
        }
        if (previousActID > this.logActID)
        {
          this.ReadALL();
        }
        else
        {
          if (this.propLoggerEntries.Count >= 1 + this.logActID)
            return this.Read(1, this.logActID, Action.LoggerReadEntryForCompare);
          this.readActID = this.logActID;
          this.readStartIndex = this.logActID;
          this.entriesToRead = 1 + (previousActID - this.propLoggerEntries.Count);
          this.entriesRead = 0;
          this.readSize = this.entriesToRead;
          if (this.readSize > this.readBlockSize)
            this.readSize = this.readBlockSize;
          return this.Read(this.readSize, this.readStartIndex, Action.LoggerReadBlock);
        }
      }
      return 0;
    }

    internal bool CleanLoggerEntries(int eCode)
    {
      if (!this.isClean)
      {
        this.isClean = true;
        if (this.LoggerEntries != null && 0 < this.LoggerEntries.Count)
          this.LoggerEntries.Clear();
        this.OnCleared(new PviEventArgs(this.Name, this.Address, eCode, this.Service.Language, Action.LoggerClear, this.Service));
      }
      return this.isClean;
    }

    internal override int ReadModuleInfo()
    {
      int num = 0;
      if (((Cpu) this.propParent).propModuleInfoList != null && ((Cpu) this.propParent).propModuleInfoList.ContainsKey((object) this.Name))
      {
        this.updateProperties(((Cpu) this.propParent).propModuleInfoList[(object) this.Name], ((Cpu) this.propParent).BootMode == BootMode.Diagnostics);
        this.OnConnected(new PviEventArgs(this.Name, this.Address, 0, this.Service.Language, Action.ConnectedEvent, this.Service));
      }
      else
      {
        num = ((Cpu) this.propParent).Loggers.UpdateLoggerList();
        if (num == 0)
          num = -3;
      }
      return num;
    }

    private bool ReadOutstandingData(int readCount, ref int retVal, Action action)
    {
      retVal = 0;
      if (readCount == 0)
      {
        this.entriesRead = this.entriesToRead;
        this.readStartIndex = this.readActID;
      }
      else
      {
        this.entriesRead += readCount;
        this.readStartIndex -= readCount;
      }
      if (this.entriesToRead <= this.entriesRead)
        return this.FinaleReadData(ref retVal, action);
      this.readSize = this.entriesToRead - this.entriesRead;
      if (this.readSize > this.readBlockSize)
        this.readSize = this.readBlockSize;
      retVal = this.Read(this.readSize, this.readStartIndex, action);
      return true;
    }

    private bool FinaleReadData(ref int retVal, Action action)
    {
      retVal = 0;
      if (this.readActID >= this.logActID)
        return false;
      this.readStartIndex = this.logActID;
      this.entriesToRead = this.logActID - this.readActID;
      this.readActID = this.logActID;
      this.readSize = this.entriesToRead;
      if (this.readSize > this.readBlockSize)
        this.readSize = this.readBlockSize;
      retVal = this.Read(this.readSize, this.readStartIndex, action);
      return true;
    }

    private int LoggerEntriesDifffer(LoggerEntry lEntry1, LoggerEntry entryTo)
    {
      if (!this.LoggerEntries.ContainsKey((object) lEntry1.UniqueKey))
        return -1;
      LoggerEntry loggerEntry = this.LoggerEntries[lEntry1.UniqueKey];
      if (lEntry1.DateTime != loggerEntry.DateTime)
        return 1;
      if ((int) lEntry1.ErrorNumber != (int) loggerEntry.ErrorNumber)
        return 2;
      if (lEntry1.ErrorInfo != loggerEntry.ErrorInfo)
        return 3;
      if ((int) lEntry1.CodeOffset != (int) loggerEntry.CodeOffset)
        return 4;
      return lEntry1.LevelType != loggerEntry.LevelType ? 5 : 0;
    }

    private int accModuleData(
      int errorCode,
      PVIReadAccessTypes accessType,
      IntPtr pData,
      uint dataLen,
      Action action)
    {
      int retVal = errorCode;
      int error = errorCode;
      int readCount = 0;
      LoggerEntryCollection eventEntries = (LoggerEntryCollection) null;
      if (errorCode == 0 || 11191 == errorCode || 11004 == errorCode)
      {
        if (11191 == errorCode || 11004 == errorCode)
        {
          retVal = 0;
          eventEntries = new LoggerEntryCollection("EventEventEntries");
          error = 0;
        }
        else
          retVal = PVIReadAccessTypes.ANSL_LoggerModuleData != accessType ? this.DoInterpretLoggerData(errorCode, pData, dataLen, action, ref readCount, ref eventEntries) : this.DoInterpretANSLLoggerData(errorCode, pData, dataLen, action, ref readCount, ref eventEntries);
        if (retVal != 0)
        {
          this.OnEntriesRead(new LoggerEventArgs(this.Name, this.Address, retVal, this.Service.Language, Action.LoggerRead, new LoggerEntryCollection("EventEntries")));
          return retVal;
        }
        switch (action)
        {
          case Action.LoggerRead:
            this.OnEntriesRead(new LoggerEventArgs(this.Name, this.Address, error, this.Service.Language, Action.LoggerRead, eventEntries));
            break;
          case Action.LoggerReadEntry:
            this.OnEntryAdded(new LoggerEventArgs(this.Name, this.Address, error, this.Service.Language, Action.LoggerReadEntry, eventEntries));
            break;
          case Action.LoggerReadBlock:
            if (!this.ReadOutstandingData(readCount, ref retVal, Action.LoggerReadBlock))
            {
              this.OnEntriesRead(new LoggerEventArgs(this.Name, this.Address, error, this.Service.Language, Action.LoggerReadBlock, eventEntries));
              break;
            }
            this.OnEntryBlockRead(new LoggerEventArgs(this.Name, this.Address, error, this.Service.Language, Action.LoggerReadBlock, eventEntries));
            break;
          case Action.LoggerReadBlockForAdded:
            if (!this.ReadOutstandingData(readCount, ref retVal, Action.LoggerReadBlockForAdded))
            {
              this.OnEntriesRead(new LoggerEventArgs(this.Name, this.Address, error, this.Service.Language, Action.LoggerReadBlockForAdded, new LoggerEntryCollection("EventEntries")));
              break;
            }
            break;
          case Action.LoggerReadEntryForCompare:
            if (1 == eventEntries.Count)
            {
              LoggerEntry entryTo = eventEntries[0];
              if (0 < this.propLoggerEntries.Count)
              {
                if (this.LoggerEntriesDifffer(this.lastLoggerEntry, entryTo) == 0)
                {
                  this.OnEntriesRead(new LoggerEventArgs(this.Name, this.Address, error, this.Service.Language, Action.LoggerIndexForUpdate, new LoggerEntryCollection("EventEntries")));
                  break;
                }
                this.propReadRequestActive = false;
                this.ReadALL();
                break;
              }
              this.propReadRequestActive = false;
              this.ReadALL();
              break;
            }
            break;
        }
      }
      else
      {
        this.logActID -= readCount;
        Action action1 = action;
        if (Action.LoggerReadEntryForCompare == action1 || Action.LoggerReadBlock == action1)
        {
          this.logActID -= this.entriesToRead;
          if (0 > this.logActID)
            this.logActID = 0;
          this.OnEntriesRead(new LoggerEventArgs(this.Name, this.Address, error, this.Service.Language, action1, new LoggerEntryCollection("EventEntries")));
        }
      }
      return retVal;
    }

    private int DoInterpretANSLLoggerData(
      int errorCode,
      IntPtr pData,
      uint dataLen,
      Action action,
      ref int readCount,
      ref LoggerEntryCollection eventEntries)
    {
      uint readVersion = 0;
      readCount = 0;
      if (dataLen != 0U)
      {
        if (!(IntPtr.Zero == pData))
        {
          int xmlContent;
          try
          {
            string stringAnsi = PviMarshal.PtrToStringAnsi(pData, dataLen);
            eventEntries = new LoggerEntryCollection("EventEventEntries");
            eventEntries.Initialize(stringAnsi);
            xmlContent = new LoggerXMLInterpreter().ParseXMLContent(this, stringAnsi, ref eventEntries, ref readVersion);
            if (xmlContent == 0)
            {
              this.LoggerEntries.propContentVersion = this.propContentVersion = eventEntries.propContentVersion = readVersion;
              for (int index = 0; index < eventEntries.Count; ++index)
              {
                LoggerEntry entry = eventEntries[index];
                this.LoggerEntries.Add((LoggerEntryBase) entry);
                ++readCount;
                if (Action.LoggerReadBlockForAdded == action)
                {
                  LoggerEntryCollection entries = new LoggerEntryCollection("EventEntries")
                  {
                    (LoggerEntryBase) entry
                  };
                  entries.propContentVersion = this.propContentVersion;
                  this.OnEntryAdded(new LoggerEventArgs(this.Name, this.Address, errorCode, this.Service.Language, Action.LoggerReadEntry, entries));
                }
              }
            }
          }
          catch
          {
            return 11;
          }
          return xmlContent;
        }
      }
      return -1;
    }

    private int DoInterpretLoggerData(
      int errorCode,
      IntPtr pData,
      uint dataLen,
      Action action,
      ref int readCount,
      ref LoggerEntryCollection eventEntries)
    {
      int num1 = 0;
      readCount = 0;
      if (dataLen != 0U)
      {
        if (!(IntPtr.Zero == pData))
        {
          try
          {
            this.LoggerEntries.propContentVersion = this.propContentVersion = 0U;
            string stringAnsi = PviMarshal.PtrToStringAnsi(pData, dataLen);
            int num2 = stringAnsi.IndexOf("DN=");
            int num3 = stringAnsi.IndexOf("\0");
            if (-1 == num3)
              num3 = stringAnsi.Length;
            string str1 = stringAnsi.Substring(num2 + 3, num3 - num2 - 3);
            readCount = System.Convert.ToInt32(str1);
            string[] strArray1 = stringAnsi.Split(new char[1]);
            uint num4 = 0;
            eventEntries = new LoggerEntryCollection("EventEventEntries");
            eventEntries.propContentVersion = this.propContentVersion;
            for (int index1 = 1; index1 < strArray1.Length - 1; ++index1)
            {
              if (index1 % 3 == 1)
              {
                LoggerEntry entry = (LoggerEntry) null;
                uint num5 = 0;
                LevelType levelType = LevelType.Success;
                string str2 = "";
                uint num6 = 0;
                uint num7 = 0;
                string str3 = strArray1[index1];
                ++num4;
                string[] strArray2 = str3.Split(' ');
                for (int index2 = 0; index2 < strArray2.Length; ++index2)
                {
                  if (-1 != strArray2[index2].IndexOf('"'))
                  {
                    for (int index3 = index2 + 1; index3 < strArray2.Length; ++index3)
                    {
                      string str4 = strArray2[index3];
                      string[] strArray3;
                      IntPtr index4;
                      (strArray3 = strArray2)[(int) (index4 = (IntPtr) index2)] = strArray3[index4] + " " + str4;
                      strArray2[index3] = "";
                      if (-1 != str4.IndexOf('"'))
                        break;
                    }
                  }
                }
                for (int index5 = 0; index5 < strArray2.Length; ++index5)
                {
                  if (strArray2[index5].StartsWith("TIME="))
                  {
                    int num8 = strArray2[index5].IndexOf("=");
                    entry = new LoggerEntry((object) this, strArray2[index5].Substring(num8 + 1, strArray2[index5].Length - num8 - 1));
                  }
                  if (strArray2[index5].StartsWith("E="))
                  {
                    int num9 = strArray2[index5].IndexOf("=");
                    num5 = Pvi.ToUInt32(strArray2[index5].Substring(num9 + 1, strArray2[index5].Length - num9 - 1));
                  }
                  if (strArray2[index5].StartsWith("LEV="))
                  {
                    int num10 = strArray2[index5].IndexOf("=");
                    levelType = (LevelType) System.Convert.ToInt32(strArray2[index5].Substring(num10 + 1, strArray2[index5].Length - num10 - 1));
                  }
                  if (strArray2[index5].StartsWith("TN="))
                  {
                    int num11 = strArray2[index5].IndexOf("=");
                    str2 = strArray2[index5].Substring(num11 + 2, strArray2[index5].Length - num11 - 3);
                  }
                  if (strArray2[index5].StartsWith("ID="))
                  {
                    int num12 = strArray2[index5].IndexOf("=");
                    num6 = System.Convert.ToUInt32(strArray2[index5].Substring(num12 + 1, strArray2[index5].Length - num12 - 1));
                  }
                  if (strArray2[index5].StartsWith("INFO="))
                  {
                    int num13 = strArray2[index5].IndexOf("=");
                    num7 = System.Convert.ToUInt32(strArray2[index5].Substring(num13 + 1, strArray2[index5].Length - num13 - 1));
                  }
                }
                if (entry == null && str3.IndexOf("TIME=") == -1)
                  entry = new LoggerEntry((object) this, DateTime.Now);
                if (entry != null)
                {
                  entry.propErrorNumber = num5;
                  entry.propLevelType = levelType;
                  entry.propTask = str2;
                  entry.propInternID = num6;
                  entry.UpdateUKey();
                  entry.propErrorInfo = num7;
                  int num14;
                  entry.propErrorText = strArray1[num14 = index1 + 1];
                  index1 = num14 + 1;
                  entry.propBinary = HexConvert.ToBytes(strArray1[index1]);
                  if (((int) entry.propErrorInfo & 2) != 0 || ((int) entry.propErrorInfo & 1) != 0)
                    entry.GetExceptionData();
                  this.LoggerEntries.Add((LoggerEntryBase) entry);
                  eventEntries.Add((LoggerEntryBase) entry);
                  if (Action.LoggerReadBlockForAdded == action)
                    this.OnEntryAdded(new LoggerEventArgs(this.Name, this.Address, errorCode, this.Service.Language, Action.LoggerReadEntry, new LoggerEntryCollection("EventEntries")
                    {
                      (LoggerEntryBase) entry
                    }));
                }
              }
            }
          }
          catch
          {
            return 11;
          }
          return num1;
        }
      }
      return -1;
    }

    protected override void OnDisconnected(PviEventArgs e)
    {
      this.eIndexValid = false;
      base.OnDisconnected(e);
      if (!this.propReadRequestActive)
        return;
      this.OnEntriesRead(new LoggerEventArgs(this.Name, this.Address, e.ErrorCode, this.Service.Language, Action.LoggerRead, new LoggerEntryCollection("EventEntries")));
      this.propReadRequestActive = false;
    }

    protected override string getLinkDescription() => this.propContinuousActive ? "EV=es" : "EV=e";

    internal override bool CheckModuleInfo(int errorCode)
    {
      if (Actions.Connected != (this.Requests & Actions.Connected))
        return base.CheckModuleInfo(errorCode);
      this.Requests &= Actions.Connect | Actions.GetList | Actions.GetValue | Actions.SetValue | Actions.GetForce | Actions.SetForce | Actions.Start | Actions.Stop | Actions.RunCycle | Actions.GetType | Actions.SetType | Actions.GetLength | Actions.SetRefresh | Actions.CreateLink | Actions.Upload | Actions.Download | Actions.SetActive | Actions.SetHysteresis | Actions.Disconnect | Actions.SetInitValue | Actions.GetLBType | Actions.ModuleInfo | Actions.ListSNMPVariables | Actions.Uploading | Actions.GetCpuInfo | Actions.ReadPVFormat | Actions.FireActivated | Actions.ReadIndex | Actions.FireDataValidated | Actions.FireValueChanged | Actions.Link;
      return true;
    }

    protected override void OnDeleted(PviEventArgs e) => base.OnDeleted(e);

    protected override void OnConnected(PviEventArgs e)
    {
      if (e.ErrorCode == 0 && !this.propParent.IsConnected)
        this.Requests |= Actions.Connected;
      else if (4819 == e.ErrorCode)
      {
        this.ResetReadIDs();
        this.Connect();
      }
      else
      {
        if (!this.eIndexValid)
          return;
        base.OnConnected(e);
        if (Actions.SetActive != (this.Requests & Actions.SetActive))
          return;
        this.Requests &= Actions.Connect | Actions.GetList | Actions.GetValue | Actions.SetValue | Actions.GetForce | Actions.SetForce | Actions.Start | Actions.Stop | Actions.RunCycle | Actions.GetType | Actions.SetType | Actions.GetLength | Actions.SetRefresh | Actions.CreateLink | Actions.Upload | Actions.Download | Actions.SetHysteresis | Actions.Disconnect | Actions.SetInitValue | Actions.GetLBType | Actions.ModuleInfo | Actions.ListSNMPVariables | Actions.Connected | Actions.Uploading | Actions.GetCpuInfo | Actions.ReadPVFormat | Actions.FireActivated | Actions.ReadIndex | Actions.FireDataValidated | Actions.FireValueChanged | Actions.Link;
        this.SetContinuousActive(this.propContinuousActive);
      }
    }

    private void UpdateContinuouseActive()
    {
      if (!this.ContinuousActive)
        return;
      this.propContinuousActive = false;
      this.ContinuousActive = true;
    }

    protected virtual void OnContinuousActivated(PviEventArgs e)
    {
      this.propContinuousActive = true;
      this.ReadIndex(Action.LoggerIndexForContinuousActivated);
      if (this.ContinuousActivated == null)
        return;
      this.ContinuousActivated((object) this, e);
    }

    protected virtual void OnContinuousDeactivated(PviEventArgs e)
    {
      this.propContinuousActive = false;
      if (this.ContinuousDeactivated == null)
        return;
      this.ContinuousDeactivated((object) this, e);
    }

    protected virtual void OnEntriesRead(LoggerEventArgs e)
    {
      if (0 < e.Entries.Count)
        this.lastLoggerEntry = e.Entries[0];
      else if (this.propLoggerEntries != null)
        this.lastLoggerEntry = this.propLoggerEntries[0];
      this.propReadRequestActive = false;
      this.entriesToRead = 0;
      if (4808 == e.ErrorCode)
        this.ResetReadIDs();
      if (this.EntriesRead != null)
        this.EntriesRead((object) this, e);
      this.OnGlobalAdded(e);
    }

    protected virtual void OnEntryBlockRead(LoggerEventArgs e)
    {
      if (this.EntryBlockRead != null)
        this.EntryBlockRead((object) this, e);
      this.OnGlobalAdded(e);
    }

    protected virtual void OnCleared(PviEventArgs e)
    {
      if (this.Cleared == null)
        return;
      this.Cleared((object) this, e);
    }

    protected internal override void OnError(PviEventArgs e)
    {
      if (Action.LoggerDetectSGType == e.Action || Action.LoggerGetStatus == e.Action)
        this.OnSingleError(e);
      else
        base.OnError(e);
    }

    protected virtual void OnEntryAdded(LoggerEventArgs e)
    {
      if (this.EntryAdded != null)
        this.EntryAdded((object) this, e);
      this.OnGlobalAdded(e);
    }

    internal void CallOnEntriesRemoved(LoggerEventArgs e)
    {
      this.OnGlobalRemoved(e);
      this.OnEntriesRemoved(e);
    }

    protected virtual void OnEntriesRemoved(LoggerEventArgs e)
    {
      if (this.EntriesRemoved == null)
        return;
      this.EntriesRemoved((object) this, e);
    }

    protected virtual void OnGlobalAdded(LoggerEventArgs e)
    {
      if (!this.GlobalMerge || this.GlobalAdded == null)
        return;
      this.GlobalAdded((object) this, new LoggerEventArgs(e, Action.LoggerGlobalAdded));
    }

    protected virtual void OnGlobalRemoved(LoggerEventArgs e)
    {
      if (!this.GlobalMerge || this.GlobalRemoved == null)
        return;
      this.GlobalRemoved((object) this, e);
    }

    public LoggerEntryCollection LoggerEntries => this.propLoggerEntries;

    public bool ReadRequestActive => this.propReadRequestActive;

    public bool IsArchive => this.propIsArchive;

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

    private void SetContinuousActive(bool activate)
    {
      Action action = Action.LoggerContinuousActivate;
      string request = "es";
      if (!activate)
      {
        request = "e";
        action = Action.LoggerContinuousDeactivate;
      }
      this.Service.BuildRequestBuffer(request);
      int errorCode = activate ? this.WriteRequest(this.Service.hPvi, this.propLinkId, AccessTypes.EventMask, this.Service.RequestBuffer, request.Length, (uint) action) : this.WriteRequestA(this.Service.hPvi, this.propLinkId, AccessTypes.EventMask, this.Service.RequestBuffer, request.Length, (uint) action);
      if (errorCode == 0)
        return;
      this.OnError(new PviEventArgs(this.Name, this.Address, errorCode, this.Service.Language, action, this.Service));
    }

    public bool ContinuousActive
    {
      get => this.propContinuousActive;
      set
      {
        if (this.propContinuousActive == value)
          return;
        this.propContinuousActive = value;
        if (this.IsConnected)
        {
          this.SetContinuousActive(this.propContinuousActive);
        }
        else
        {
          if (Actions.SetActive == (this.Requests & Actions.SetActive))
            return;
          this.Requests |= Actions.SetActive;
        }
      }
    }

    public bool GlobalMerge
    {
      set
      {
        if (this.propGlobalMerge == value)
          return;
        if (value)
        {
          if (this.LoggerEntries != null && 0 < this.LoggerEntries.Count)
          {
            if (this is ErrorLogBook)
            {
              foreach (LoggerEntryBase entry in (IEnumerable) this.LoggerEntries.Values)
                this.Service.LoggerEntries.Add(entry, true);
            }
            else
            {
              foreach (LoggerEntryBase entry in (IEnumerable) this.LoggerEntries.Values)
                this.Service.LoggerEntries.Add(entry);
            }
          }
          this.propGlobalMerge = value;
          this.OnGlobalAdded(new LoggerEventArgs(this.Name, this.Address, 0, this.Service.Language, Action.LoggerGlobalAdded, this.LoggerEntries));
        }
        else
        {
          if (this.Service.LoggerEntries == null)
            return;
          if (this.LoggerEntries != null && 0 < this.LoggerEntries.Count)
          {
            foreach (LoggerEntry key in (IEnumerable) this.LoggerEntries.Values)
              this.Service.LoggerEntries.Remove((object) key);
          }
          this.OnGlobalRemoved(new LoggerEventArgs(this.Name, this.Address, 0, this.Service.Language, Action.LoggerGlobalRemoved, this.LoggerEntries));
          this.propGlobalMerge = value;
        }
      }
      get => this.propGlobalMerge;
    }

    public LoggerCollection ParentCollection => this.propParentCollection;

    public int ReadBlockSize
    {
      get => this.readBlockSize;
      set => this.readBlockSize = value;
    }

    public event PviEventHandler ContinuousActivated;

    public event PviEventHandler ContinuousDeactivated;

    public event LoggerEventHandler EntriesRead;

    public event LoggerEventHandler EntryBlockRead;

    public event PviEventHandler Cleared;

    public event LoggerEventHandler EntryAdded;

    public event LoggerEventHandler EntriesRemoved;

    public event LoggerEventHandler GlobalAdded;

    public event LoggerEventHandler GlobalRemoved;
  }
}
