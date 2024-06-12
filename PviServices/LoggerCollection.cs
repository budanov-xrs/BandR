// Decompiled with JetBrains decompiler
// Type: BR.AN.PviServices.LoggerCollection
// Assembly: BR.AN.PVIServices, Version=1.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DA853F0A-C070-4E30-B7A7-51D955FB85F6
// Assembly location: C:\Users\ss_six\Desktop\FXE\FXE software integration package\COMET_FXE-PVIServices_Example\Pluto-PVI-trial_01\packages\IToolS.BandR.3.5.58\lib\net20\BR.AN.PVIServices.dll

using System;
using System.Collections;
using System.IO;
using System.Text;
using System.Xml;

namespace BR.AN.PviServices
{
  public class LoggerCollection : SynchronizableBaseCollection
  {
    private LoggerCollection errorLoggers;
    private LoggerCollection validLoggers;

    public LoggerCollection(object parent, string name)
      : base(CollectionType.ArrayList, parent, name)
    {
      this.InitLoggerCollection(parent, name);
      if (!(this.Parent is Cpu))
        return;
      if (((Cpu) this.Parent).propUserLoggerCollections == null)
        ((Cpu) this.Parent).propUserLoggerCollections = new Hashtable();
      ((Cpu) this.Parent).propUserLoggerCollections.Add(this.Name, (object) this);
    }

    internal LoggerCollection(CollectionType colType, object parent, string name)
      : base(colType, parent, name)
    {
      this.InitLoggerCollection(parent, name);
    }

    private void InitLoggerCollection(object parent, string name)
    {
      this.synchronize = true;
      if (!(parent is Service))
        return;
      ArrayList loggerCollections = ((Service) parent).LoggerCollections;
      bool flag = false;
      int index;
      for (index = 0; index < loggerCollections.Count; ++index)
      {
        if (string.Compare(((BaseCollection) loggerCollections[index]).Name.ToString(), name) == 0)
        {
          flag = true;
          break;
        }
      }
      if (flag)
      {
        foreach (Logger logger in (BaseCollection) ((Service) parent).LoggerCollections[index])
          logger.GlobalMerge = false;
        loggerCollections.Remove(loggerCollections[index]);
        ((Service) parent).LoggerCollections.Insert(index, (object) this);
      }
      else
        ((Service) parent).LoggerCollections.Add((object) this);
    }

    internal override void OnPviRead(
      int errorCode,
      PVIReadAccessTypes accessType,
      PVIDataStates dataState,
      IntPtr pData,
      uint dataLen,
      int option)
    {
      if (PVIReadAccessTypes.ModuleList == accessType)
        this.LoggerListFromCB(errorCode, pData, dataLen, false);
      else if (PVIReadAccessTypes.ANSL_ModuleList == accessType)
        this.LoggerListFromCB(errorCode, pData, dataLen, true);
      else
        base.OnPviRead(errorCode, accessType, dataState, pData, dataLen, option);
    }

    private void LoggerListFromCB(int errorCode, IntPtr pData, uint dataLen, bool isANSL)
    {
      int updateFlags = 0;
      Hashtable newItems = new Hashtable();
      if (isANSL)
      {
        foreach (ModuleInfoDecription moduleInfoStruct in (IEnumerable) ((Cpu) this.Parent).ReadANSLMODList(pData, dataLen).Values)
        {
          if (moduleInfoStruct.name != null && ModuleType.Logger == moduleInfoStruct.type)
          {
            newItems.Add((object) moduleInfoStruct.name, (object) moduleInfoStruct.name);
            this.UpdateLoggerInfo(moduleInfoStruct, errorCode, ref updateFlags);
          }
        }
      }
      else
      {
        int num = (int) (dataLen / 164U);
        for (int index = 0; index < num; ++index)
        {
          APIFC_ModulInfoRes modulInfoStructure = PviMarshal.PtrToModulInfoStructure((IntPtr) ((int) pData + index * 164), typeof (APIFC_ModulInfoRes));
          if (modulInfoStructure.name != null && ModuleType.Logger == modulInfoStructure.type)
          {
            newItems.Add((object) modulInfoStructure.name, (object) modulInfoStructure.name);
            this.UpdateLoggerInfo(modulInfoStructure, errorCode, ref updateFlags);
          }
        }
      }
      if (this.synchronize || 1 == (updateFlags & 1))
        this.DoSynchronize((Cpu) this.propParent, newItems);
      if (this.Count == 0)
      {
        if (((Cpu) this.propParent).IsSG4Target)
        {
          Logger logger1 = new Logger((Cpu) this.propParent, "$arlogsys");
          Logger logger2 = new Logger((Cpu) this.propParent, "$arlogusr");
        }
        else
        {
          ErrorLogBook errorLogBook = new ErrorLogBook((Cpu) this.propParent);
        }
      }
      this.CheckFireUploadEvents(4224 == errorCode ? 0 : errorCode, Action.LoggersUpload, Action.LoggersConnect);
      this.OnCollectionUploaded((CollectionEventArgs) new LoggerCollectionEventArgs(this.propName, "", 0, this.Service.Language, Action.LoggersUpload, this.validLoggers));
      if (this.propErrorCount <= 0)
        return;
      this.OnCollectionError((CollectionEventArgs) new LoggerCollectionEventArgs(this.propName, "", 0, this.Service.Language, Action.LoggersUpload, this.errorLoggers));
    }

    internal override void Dispose(bool disposing, bool removeFromCollection)
    {
      if (this.propDisposed)
        return;
      object propParent = this.propParent;
      object propUserData = this.propUserData;
      string propName = this.propName;
      if (this.Parent is Cpu && ((Cpu) this.Parent).propUserLoggerCollections != null && ((Cpu) this.Parent).propUserLoggerCollections.ContainsKey(this.Name))
        ((Cpu) this.Parent).propUserLoggerCollections.Remove(this.Name);
      this.CleanUpOnDispose(disposing);
      this.propParent = propParent;
      this.propUserData = propUserData;
      this.propName = propName;
      base.Dispose(disposing, removeFromCollection);
      this.propParent = (object) null;
      this.propUserData = (object) null;
      this.propName = (string) null;
    }

    internal void Disconnect(bool noResponse)
    {
      if (noResponse)
      {
        this.DisconnectObjects(true);
        this.propSentCount = 0;
        this.propValidCount = 0;
        this.propErrorCount = 0;
        this.propConnectionState = ConnectionStates.Disconnected;
      }
      else
        this.DisconnectObjects(false);
    }

    private int DisconnectObjects(bool noResponse)
    {
      int num1 = 0;
      int num2 = 0;
      if (this.Values == null)
        return 0;
      foreach (Logger sender in (IEnumerable) this.Values)
      {
        if (sender.LinkId == 0U)
        {
          if (!noResponse)
            this.OnDisconnected((Base) sender, new PviEventArgs(sender.Name, sender.Address, 4808, "en", Action.LoggerDisconnect));
        }
        else
        {
          if (ConnectionStates.Connected != sender.propConnectionState && ConnectionStates.ConnectedError != sender.propConnectionState)
          {
            ++num2;
            ++this.propValidCount;
          }
          else
            sender.Disconnect(noResponse);
          ++this.propSentCount;
        }
      }
      return num1;
    }

    private void CleanUpOnDispose(bool disposing)
    {
      ArrayList arrayList = new ArrayList();
      if (this.Values != null && 0 < this.Values.Count)
      {
        foreach (Logger logger in (IEnumerable) this.Values)
        {
          arrayList.Add((object) logger);
          logger.DisconnectRet(0U);
        }
      }
      for (int index = 0; index < arrayList.Count; ++index)
      {
        object obj = arrayList[index];
        if (((Base) obj).Name != null)
          this.RemoveFromBaseCollections(((Base) obj).LogicalName, 1);
        ((Base) obj).Dispose(disposing, true);
      }
      this.Clear();
    }

    public int Load(StreamReader stream) => 0;

    public int Load(string xmlFile)
    {
      int num = 0;
      if (!File.Exists(xmlFile))
        return 1;
      try
      {
        XmlTextReader reader = new XmlTextReader(xmlFile);
        do
        {
          if (string.Compare(reader.Name, "Logger") == 0)
          {
            string attribute1 = reader.GetAttribute("Name");
            string attribute2 = reader.GetAttribute("ContinuousActive");
            new Logger((Cpu) this.propParent, attribute1)
            {
              ContinuousActive = attribute2.ToLower().Equals("true")
            }.LoggerEntries.Load(reader);
          }
        }
        while (reader.Read());
        reader.Close();
      }
      catch
      {
        num = 12054;
      }
      return num;
    }

    public int Save(string file)
    {
      int num = 0;
      StringBuilder xmlTextBlock = new StringBuilder();
      xmlTextBlock.Append("<?xml version=\"1.0\" encoding=\"utf-8\" ?><?AutomationStudio Version=\"2.6\"?><?AutomationRuntimeIOSystem Version=\"1.0\"?><?LoggerControl Version=\"1.0\"?>");
      xmlTextBlock.Append(string.Format("<Loggers Count=\"{0}\">", (object) this.Count));
      foreach (Logger propArray in this.propArrayList)
      {
        xmlTextBlock.Append(string.Format("<Logger Name=\"{0}\" Entries=\"{1}\" ContinuousActive=\"{2}\">", (object) propArray.Name, (object) propArray.LoggerEntries.Count, propArray.ContinuousActive ? (object) "true" : (object) "false"));
        propArray.LoggerEntries.Save(ref xmlTextBlock);
        xmlTextBlock.Append("</Logger>");
      }
      xmlTextBlock.Append("</Loggers>");
      FileStream fileStream = new FileStream(file, FileMode.Create, FileAccess.Write);
      StreamWriter streamWriter = new StreamWriter((Stream) fileStream);
      streamWriter.Write(xmlTextBlock.ToString());
      streamWriter.Close();
      fileStream.Close();
      return num;
    }

    public override void Remove(string key)
    {
      int num = 0;
      while (num < this.propArrayList.Count && ((Base) this.propArrayList[num]).Name.CompareTo(key) != 0)
        ++num;
      if (num >= this.propArrayList.Count)
        return;
      this.Remove((object) num);
    }

    public int Add(Logger logger)
    {
      int num = 0;
      for (int index = 0; index < this.propArrayList.Count; ++index)
      {
        if (((Base) this.propArrayList[index]).Name.CompareTo(logger.Name) == 0)
          return -1;
      }
      this.propArrayList.Add((object) logger);
      logger.propParentCollection = this;
      return num;
    }

    public event PviEventHandler LoggerUploaded;

    public virtual void OnLoggerUploaded(Logger logMod, PviEventArgs e)
    {
      if (e.ErrorCode != 0)
      {
        ++this.propErrorCount;
        if (this.errorLoggers == null)
        {
          this.errorLoggers = new LoggerCollection(this.Parent, "Error loggers");
          this.errorLoggers.propInternalCollection = true;
        }
        if (!this.errorLoggers.ContainsKey((object) logMod.Name))
          this.errorLoggers.Add(logMod);
      }
      else
      {
        ++this.propValidCount;
        if (this.validLoggers == null)
        {
          this.validLoggers = new LoggerCollection(this.Parent, "Valid loggers");
          this.validLoggers.propInternalCollection = true;
        }
        if (!this.validLoggers.ContainsKey((object) logMod.Name))
          this.validLoggers.Add(logMod);
      }
      if (this.LoggerUploaded == null)
        return;
      this.LoggerUploaded((object) logMod, e);
    }

    public void Upload()
    {
      if (Actions.Uploading == (this.Requests & Actions.Uploading))
        return;
      this.Requests |= Actions.Uploading;
      int error = this.UpdateLoggerList();
      if (0 >= error)
        return;
      this.OnError(new CollectionEventArgs(((Base) this.propParent).Name, ((Base) this.propParent).Address, error, this.Service.Language, Action.ModulesUpload, (BaseCollection) null));
    }

    internal int UpdateLoggerList()
    {
      if (!((Base) this.propParent).IsConnected)
      {
        if (Actions.Uploading == (this.Requests & Actions.Uploading))
          this.Requests &= Actions.Connect | Actions.GetList | Actions.GetValue | Actions.SetValue | Actions.GetForce | Actions.SetForce | Actions.Start | Actions.Stop | Actions.RunCycle | Actions.GetType | Actions.SetType | Actions.GetLength | Actions.SetRefresh | Actions.CreateLink | Actions.Upload | Actions.Download | Actions.SetActive | Actions.SetHysteresis | Actions.Disconnect | Actions.SetInitValue | Actions.GetLBType | Actions.ModuleInfo | Actions.ListSNMPVariables | Actions.Connected | Actions.GetCpuInfo | Actions.ReadPVFormat | Actions.FireActivated | Actions.ReadIndex | Actions.FireDataValidated | Actions.FireValueChanged | Actions.Link;
        this.Requests |= Actions.Upload;
        return -2;
      }
      this.propValidCount = 0;
      this.propErrorCount = 0;
      if (this.errorLoggers != null)
        this.errorLoggers.Clear();
      if (this.validLoggers != null)
        this.validLoggers.Clear();
      return ((Cpu) this.Parent).Connection.DeviceType == DeviceType.ANSLTcp ? this.ReadArgumentRequest(this.Service.hPvi, ((Base) this.propParent).LinkId, AccessTypes.ANSL_ModuleList, IntPtr.Zero, 0, 624U, this.InternId) : this.ReadArgumentRequest(this.Service.hPvi, ((Base) this.propParent).LinkId, AccessTypes.ModuleList, IntPtr.Zero, 0, 624U, this.InternId);
    }

    protected void OnError(CollectionEventArgs e)
    {
      if (!(this.propParent is Cpu))
        return;
      ((Base) this.propParent).OnError((PviEventArgs) e);
    }

    internal virtual void OnModuleChanged(ModuleEventArgs moduleEvent)
    {
      if (this.ModuleChanged == null)
        return;
      this.ModuleChanged((object) this, moduleEvent);
    }

    internal virtual void OnModuleDeleted(ModuleEventArgs moduleEvent)
    {
      if (this.ModuleDeleted == null)
        return;
      this.ModuleDeleted((object) this, moduleEvent);
    }

    internal virtual void OnModuleCreated(ModuleEventArgs moduleEvent)
    {
      if (this.ModuleCreated == null)
        return;
      this.ModuleCreated((object) this, moduleEvent);
    }

    public event CollectionEventHandler CollectionUploaded;

    protected void OnCollectionUploaded(CollectionEventArgs e)
    {
      this.hasBeenUploadedOnce = true;
      if (this.CollectionUploaded == null)
        return;
      this.CollectionUploaded((object) this, e);
    }

    internal override int ToXMLTextWriter(ref XmlTextWriter writer, ConfigurationFlags flags)
    {
      int xmlTextWriter = 0;
      if (this.Count > 0)
      {
        writer.WriteStartElement(this.GetType().Name);
        foreach (object obj in (IEnumerable) this.Values)
        {
          writer.WriteStartElement("Logger");
          xmlTextWriter = ((Module) obj).SaveModuleConfiguration(ref writer, flags);
          writer.WriteAttributeString("LoggerEntries", ((Logger) obj).LoggerEntries.Count.ToString());
          writer.WriteEndElement();
        }
        writer.WriteEndElement();
      }
      return xmlTextWriter;
    }

    internal int DiagnosticModeUpdateModuleInfo(
      APIFC_DiagModulInfoRes diagModInfo,
      int errorCode,
      ref int updateFlags)
    {
      int num = errorCode;
      bool flag = false;
      if ((this.Requests & Actions.Upload) != Actions.NONE)
      {
        flag = true;
        updateFlags |= 1;
      }
      Logger logger = ((Cpu) this.propParent).Loggers[diagModInfo.name];
      if (flag && logger == null && (diagModInfo.name.CompareTo("$arlogsys") == 0 || diagModInfo.name.CompareTo("$arlogusr") == 0))
        logger = new Logger((Cpu) this.propParent, diagModInfo.name);
      logger?.updateProperties(diagModInfo);
      return num;
    }

    internal void EmptyCollection()
    {
      Hashtable hashtable = new Hashtable();
      if (0 < this.Count)
      {
        foreach (Logger logger in (IEnumerable) this.Values)
          hashtable.Add((object) logger.Name, (object) logger);
        if (0 < hashtable.Count)
        {
          foreach (Logger remObj in (IEnumerable) hashtable.Values)
          {
            this.RemoveFromCollection((Base) remObj, Action.LoggerDelete);
            this.Remove(remObj.Name);
          }
          hashtable.Clear();
        }
      }
    }

    internal int UpdateLoggerInfo(
      APIFC_ModulInfoRes moduleInfoStruct,
      int errorCode,
      ref int updateFlags)
    {
      ModuleInfoDecription moduleInfoStruct1 = new ModuleInfoDecription();
      moduleInfoStruct1.Init(moduleInfoStruct);
      return this.UpdateLoggerInfo(moduleInfoStruct1, errorCode, ref updateFlags);
    }

    internal int UpdateLoggerInfo(
      ModuleInfoDecription moduleInfoStruct,
      int errorCode,
      ref int updateFlags)
    {
      if (errorCode != 0)
        return errorCode;
      bool flag = false;
      if (Actions.Upload == (this.Requests & Actions.Upload) || Actions.Uploading == (this.Requests & Actions.Uploading))
      {
        flag = true;
        updateFlags |= 1;
      }
      Logger logMod = (Logger) null;
      if (moduleInfoStruct.type == ModuleType.Logger)
      {
        logMod = ((Cpu) this.propParent).Loggers[moduleInfoStruct.name];
        if (flag && logMod == null)
          logMod = new Logger((Cpu) this.propParent, moduleInfoStruct.name);
        if (logMod != null && (flag || Actions.Upload == (logMod.Requests & Actions.Upload) || Actions.ModuleInfo == (logMod.Requests & Actions.ModuleInfo) || Actions.Uploading == (logMod.Requests & Actions.Uploading)))
          logMod.updateProperties(moduleInfoStruct, ((Cpu) this.propParent).BootMode == BootMode.Diagnostics);
      }
      if (logMod != null)
      {
        this.OnLoggerUploaded(logMod, new PviEventArgs(this.propName, "", errorCode, this.Service.Language, Action.LoggerUpload, this.Service));
        if (logMod.CheckModuleInfo(errorCode))
          logMod.Fire_OnConnected(new PviEventArgs(logMod.Name, logMod.Address, errorCode, this.Service.Language, Action.ConnectedEvent, this.Service));
      }
      if (!((Cpu) this.propParent).IsSG4Target && !((Cpu) this.propParent).Loggers.ContainsKey((object) "$LOG285$"))
      {
        Logger logger = (Logger) new ErrorLogBook((Cpu) this.propParent);
      }
      return 0;
    }

    internal void ResetIDs()
    {
      if (this.Values == null)
        return;
      foreach (Logger logger in (IEnumerable) this.Values)
        logger.ResetIndex();
    }

    internal void DoSynchronize(Cpu callingCpu, Hashtable newItems)
    {
      Hashtable hashtable = new Hashtable();
      if (this.Values != null)
      {
        foreach (Logger logger in (IEnumerable) this.Values)
        {
          if (!newItems.ContainsKey((object) logger.Name) && !hashtable.ContainsKey((object) logger.propName))
            hashtable.Add((object) logger.Name, (object) logger);
        }
        if (0 < hashtable.Count)
        {
          foreach (Logger remObj in (IEnumerable) hashtable.Values)
          {
            if (callingCpu.IsSG4Target)
            {
              if (remObj.Name.CompareTo("$arlogsys") != 0 && remObj.Name.CompareTo("$arlogusr") != 0 && !remObj.IsArchive)
                this.RemoveFromCollection((Base) remObj, Action.LoggerDelete);
            }
            else if (remObj.Name.CompareTo("$LOG285$") != 0 && !remObj.IsArchive)
              this.RemoveFromCollection((Base) remObj, Action.LoggerDelete);
          }
          hashtable.Clear();
        }
      }
    }

    public override Service Service
    {
      get
      {
        if (this.propParent is Cpu)
          return ((Base) this.propParent).Service;
        return this.propParent is Service ? (Service) this.propParent : (Service) null;
      }
    }

    public Logger this[string name]
    {
      get
      {
        for (int index = 0; index < this.propArrayList.Count; ++index)
        {
          Logger propArray = (Logger) this.propArrayList[index];
          if (name.Equals(propArray.Name))
            return propArray;
        }
        return (Logger) null;
      }
    }

    public event ModuleEventHandler ModuleCreated;

    public event ModuleEventHandler ModuleChanged;

    public event ModuleEventHandler ModuleDeleted;
  }
}
