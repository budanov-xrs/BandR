// Decompiled with JetBrains decompiler
// Type: BR.AN.PviServices.ModuleCollection
// Assembly: BR.AN.PVIServices, Version=1.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DA853F0A-C070-4E30-B7A7-51D955FB85F6
// Assembly location: C:\Users\ss_six\Desktop\FXE\FXE software integration package\COMET_FXE-PVIServices_Example\Pluto-PVI-trial_01\packages\IToolS.BandR.3.5.58\lib\net20\BR.AN.PVIServices.dll

using System;
using System.Collections;
using System.IO;
using System.Runtime.InteropServices;
using System.Xml;

namespace BR.AN.PviServices
{
  public class ModuleCollection : SynchronizableBaseCollection
  {
    private int percentage;
    private ModuleCollection errorModules;
    private ModuleCollection validModules;

    public ModuleCollection(object parent, string name)
      : base(parent, name)
    {
      this.synchronize = true;
      if (!(this.Parent is Cpu))
        return;
      if (((Cpu) this.Parent).propUserModuleCollections == null)
        ((Cpu) this.Parent).propUserModuleCollections = new Hashtable();
      ((Cpu) this.Parent).propUserModuleCollections.Add(this.Name, (object) this);
    }

    internal ModuleCollection(CollectionType colType, object parent, string name)
      : base(colType, parent, name)
    {
      this.synchronize = true;
    }

    protected void OnError(CollectionEventArgs e)
    {
      if (!(this.propParent is Cpu))
        return;
      ((Base) this.propParent).OnError((PviEventArgs) e);
    }

    public override void Connect(ConnectionType connectionType)
    {
      this.propSentCount = 0;
      this.propValidCount = 0;
      this.propErrorCount = 0;
      int num = 0;
      if (this.validModules == null)
      {
        this.validModules = new ModuleCollection(CollectionType.HashTable, this.Parent, "Valid modules");
        this.validModules.propInternalCollection = true;
      }
      else
        this.validModules.Clear();
      if (this.Count == 0)
      {
        this.OnCollectionConnected((CollectionEventArgs) new ModuleCollectionEventArgs(this.propName, "", 0, this.Service.Language, Action.ModulesConnect, this.validModules));
      }
      else
      {
        foreach (Module module in (IEnumerable) this.Values)
        {
          ++this.propSentCount;
          if (ConnectionStates.Connected == module.propConnectionState || ConnectionStates.ConnectedError == module.propConnectionState)
          {
            this.validModules.Add(module);
            ++this.propValidCount;
            ++num;
            if (num + this.propErrorCount == this.Count)
              this.OnCollectionConnected((CollectionEventArgs) new ModuleCollectionEventArgs(this.propName, "", 0, this.Service.Language, Action.ModulesConnect, this.validModules));
          }
          else
            module.Connect(connectionType);
        }
      }
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
        this.Disconnect();
    }

    public void Disconnect()
    {
      this.propSentCount = 0;
      this.propValidCount = 0;
      this.propErrorCount = 0;
      if (this.Values == null || this.Count == 0)
      {
        this.OnCollectionDisconnected((CollectionEventArgs) new ModuleCollectionEventArgs(this.propName, "", 0, this.Service.Language, Action.ModulesDisconnect, this));
      }
      else
      {
        if (this.validModules == null)
        {
          this.validModules = new ModuleCollection(CollectionType.HashTable, this.Parent, "Valid modules");
          this.validModules.propInternalCollection = true;
        }
        else
          this.validModules.Clear();
        if (this.Count == 0)
          this.OnCollectionDisconnected((CollectionEventArgs) new ModuleCollectionEventArgs(this.propName, "", 0, this.Service.Language, Action.ModulesDisconnect, this.validModules));
        else
          this.DisconnectObjects(false);
      }
    }

    private int DisconnectObjects(bool noResponse)
    {
      int num1 = 0;
      int num2 = 0;
      if (this.Values == null)
        return 0;
      foreach (Module module in (IEnumerable) this.Values)
      {
        if (module.LinkId == 0U)
        {
          if (!noResponse)
            this.OnDisconnected((Base) module, new PviEventArgs(module.Name, module.Address, 4808, "en", Action.ModuleDisconnect));
        }
        else
        {
          if (ConnectionStates.Connected != module.propConnectionState && ConnectionStates.ConnectedError != module.propConnectionState)
          {
            ++num2;
            if (num2 == this.Count && !noResponse)
              this.OnCollectionDisconnected((CollectionEventArgs) new ModuleCollectionEventArgs(this.propName, "", 0, this.Service.Language, Action.ModuleDisconnect, this.validModules));
            ++this.propValidCount;
            if (!noResponse)
              this.validModules.Add(module);
          }
          else
            module.Disconnect(noResponse);
          ++this.propSentCount;
        }
      }
      return num1;
    }

    public virtual Module GetItem(int index)
    {
      switch (this.propCollectionType)
      {
        case CollectionType.SortedList:
        case CollectionType.ArrayList:
          return (Module) this.propArrayList[index];
        default:
          return (Module) null;
      }
    }

    internal virtual Module GetItem(string name) => this.propCollectionType == CollectionType.HashTable ? this[name] : (Module) null;

    public virtual int Add(Module module)
    {
      Module module1 = this[module.Name];
      if (module1 == null)
      {
        this.Add((object) module.Name, (object) module);
      }
      else
      {
        switch (module.Type)
        {
          case ModuleType.PlcTask:
          case ModuleType.TimerTask:
          case ModuleType.ExceptionTask:
          case ModuleType.Logger:
            return -1;
          case ModuleType.SystemTask:
            module1.propAddress = module.propAddress;
            break;
        }
      }
      if (!this.propInternalCollection)
      {
        if (module.propUserCollections == null)
          module.propUserCollections = new Hashtable();
        if (!module.propUserCollections.ContainsKey(this.Name))
          module.propUserCollections.Add(this.Name, (object) this);
      }
      return 0;
    }

    public override void Remove(string key)
    {
      if (this.validModules != null && this.validModules.ContainsKey((object) key))
        this.validModules.Remove(key);
      if (this.errorModules != null && this.errorModules.ContainsKey((object) key))
        this.errorModules.Remove(key);
      base.Remove(key);
    }

    public override void Remove(object key)
    {
      if (key is Module)
      {
        if (this.validModules != null && this.validModules.ContainsKey((object) ((Base) key).Name))
          this.validModules.Remove(((Base) key).Name);
        if (this.errorModules != null && this.errorModules.ContainsKey((object) ((Base) key).Name))
          this.errorModules.Remove(((Base) key).Name);
      }
      base.Remove(key);
    }

    internal override void Dispose(bool disposing, bool removeFromCollection)
    {
      if (this.propDisposed)
        return;
      object propParent = this.propParent;
      object propUserData = this.propUserData;
      string propName = this.propName;
      if (this.Parent is Cpu && ((Cpu) this.Parent).propUserModuleCollections != null && ((Cpu) this.Parent).propUserModuleCollections.ContainsKey(this.Name))
        ((Cpu) this.Parent).propUserModuleCollections.Remove(this.Name);
      this.CleanUp(disposing);
      if (this.validModules != null)
        this.validModules.Dispose(disposing, false);
      if (this.errorModules != null)
        this.errorModules.Dispose(disposing, false);
      this.propParent = propParent;
      this.propUserData = propUserData;
      this.propName = propName;
      base.Dispose(disposing, removeFromCollection);
      this.propParent = (object) null;
      this.propUserData = (object) null;
      this.propName = (string) null;
    }

    internal void CleanUp(bool disposing)
    {
      ArrayList arrayList = new ArrayList();
      if (this.validModules != null)
        this.validModules.Clear();
      if (this.errorModules != null)
        this.errorModules.Clear();
      if (this.Values != null)
      {
        foreach (Module module in (IEnumerable) this.Values)
          arrayList.Add((object) module);
        for (int index = 0; index < arrayList.Count; ++index)
        {
          Module module = (Module) arrayList[index];
          if (module.LinkId != 0U)
            module.DisconnectRet(0U);
          module.Dispose(disposing, true);
        }
      }
      this.Clear();
      if (!(this.Parent is Cpu) || ((Cpu) this.Parent).Tasks == null)
        return;
      ((Cpu) this.Parent).Tasks.CleanUp(disposing);
    }

    private void ModuleDisconnected(object sender, PviEventArgs e) => ((Base) sender).Disconnected -= new PviEventHandler(this.ModuleDisconnected);

    private void CheckForModInfoChanges(APIFC_ModulInfoRes moduleInfoStruct)
    {
      ModuleInfoDecription moduleInfoStruct1 = new ModuleInfoDecription();
      moduleInfoStruct1.Init(moduleInfoStruct);
      this.CheckForModInfoChanges(moduleInfoStruct1, 0);
    }

    private void CheckForModInfoChanges(ModuleInfoDecription moduleInfoStruct, int retCode)
    {
      Module module1 = (Module) null;
      if (!(this.propParent is Cpu))
        return;
      if (Module.isTaskObject(moduleInfoStruct.type))
      {
        Task task = ((Cpu) this.propParent).Tasks[moduleInfoStruct.name];
        if (task == null)
          task = new Task((Cpu) this.propParent, moduleInfoStruct.name);
        else if (((Cpu) this.propParent).Modules[moduleInfoStruct.name] == null)
          ((Cpu) this.propParent).Modules.Add((Module) task);
        task.updateProperties((object) moduleInfoStruct);
        if (retCode != 0)
          module1 = (Module) task;
      }
      else if (moduleInfoStruct.type == ModuleType.Logger)
      {
        Logger logger = ((Cpu) this.propParent).Loggers[moduleInfoStruct.name];
        if (logger == null)
          logger = new Logger((Cpu) this.propParent, moduleInfoStruct.name);
        else if (((Cpu) this.propParent).Modules[moduleInfoStruct.name] == null)
          ((Cpu) this.propParent).Modules.Add((Module) logger);
        logger.updateProperties((object) moduleInfoStruct);
        if (retCode != 0)
          module1 = (Module) logger;
      }
      else if (!((Cpu) this.Parent).Libraries.ContainsKey((object) moduleInfoStruct.name))
      {
        Module module2 = ((Cpu) this.propParent).Modules[moduleInfoStruct.name] ?? new Module((Cpu) this.propParent, moduleInfoStruct, this);
        module2.updateProperties((object) moduleInfoStruct);
        if (retCode != 0)
          module1 = module2;
      }
      if (retCode == 0 || module1 == null)
        return;
      this.OnModuleChanged(new ModuleEventArgs(module1.Name, module1.Address, 12054, this.Service.Language, Action.ModuleChangedEvent, module1, 0));
    }

    internal override void OnPviRead(
      int errorCode,
      PVIReadAccessTypes accessType,
      PVIDataStates dataState,
      IntPtr pData,
      uint dataLen,
      int option)
    {
      if (PVIReadAccessTypes.ANSL_ModuleList == accessType)
      {
        if (dataLen != 0U)
        {
          if (errorCode == 0)
          {
            try
            {
              ModuleInfoDecription moduleInfoStruct = new ModuleInfoDecription();
              byte[] numArray = new byte[(IntPtr) dataLen];
              Marshal.Copy(pData, numArray, 0, (int) dataLen);
              XmlTextReader xmlTReader = new XmlTextReader((Stream) new MemoryStream(numArray));
              int content = (int) xmlTReader.MoveToContent();
              if (xmlTReader.Name.CompareTo("ModList") == 0)
              {
                while (!xmlTReader.EOF && xmlTReader.NodeType != XmlNodeType.EndElement)
                {
                  if (xmlTReader.Name.CompareTo("ModInfo") == 0 || xmlTReader.Name.CompareTo("TaskInfo") == 0)
                  {
                    int retCode = moduleInfoStruct.ReadFromXML(xmlTReader);
                    if (moduleInfoStruct.name != null)
                      this.CheckForModInfoChanges(moduleInfoStruct, retCode);
                  }
                  else
                    xmlTReader.Read();
                }
              }
              if (!(this.propParent is Cpu) || ((Cpu) this.propParent).Loggers.Count != 0)
                return;
              ErrorLogBook errorLogBook = new ErrorLogBook((Cpu) this.propParent);
              ((Cpu) this.propParent).propIsSG4Target = false;
              return;
            }
            catch
            {
              this.OnError(new CollectionEventArgs(this.propName, "", 12054, this.Service.Language, Action.ModuleChangedEvent, (BaseCollection) null));
              return;
            }
          }
        }
        this.OnUploaded(new PviEventArgs(this.propName, "", errorCode, this.Service.Language, Action.ModulesUpload, this.Service));
        this.OnError(new CollectionEventArgs(this.propName, "", errorCode, this.Service.Language, Action.ModulesUpload, (BaseCollection) null));
      }
      else if (PVIReadAccessTypes.ModuleList == accessType)
      {
        if (errorCode == 0)
        {
          int num = (int) (dataLen / 164U);
          for (int index = 0; index < num; ++index)
            this.CheckForModInfoChanges(PviMarshal.PtrToModulInfoStructure((IntPtr) ((int) pData + index * 164), typeof (APIFC_ModulInfoRes)));
          if (!(this.propParent is Cpu) || ((Cpu) this.propParent).Loggers.Count != 0)
            return;
          ErrorLogBook errorLogBook = new ErrorLogBook((Cpu) this.propParent);
          ((Cpu) this.propParent).propIsSG4Target = false;
        }
        else
        {
          this.OnUploaded(new PviEventArgs(this.propName, "", errorCode, this.Service.Language, Action.ModulesUpload, this.Service));
          this.OnError(new CollectionEventArgs(this.propName, "", errorCode, this.Service.Language, Action.ModulesUpload, (BaseCollection) null));
        }
      }
      else if (PVIReadAccessTypes.DiagnoseModuleList == accessType)
      {
        if (errorCode == 0)
        {
          int num = (int) (dataLen / 57U);
          for (int index = 0; index < num; ++index)
          {
            APIFC_DiagModulInfoRes modulInfoStructure = PviMarshal.PtrToDiagModulInfoStructure((IntPtr) ((int) pData + index * 57), typeof (APIFC_DiagModulInfoRes));
            if (this.propParent is Cpu)
            {
              Module module;
              if ((module = ((Cpu) this.propParent).Modules[modulInfoStructure.name]) == null)
                module = new Module((Cpu) this.propParent, modulInfoStructure, this);
              module.updateProperties(modulInfoStructure);
            }
          }
        }
        else
        {
          this.OnUploaded(new PviEventArgs(this.propName, "", errorCode, this.Service.Language, Action.ModulesUpload, this.Service));
          this.OnError(new CollectionEventArgs(this.propName, "", errorCode, this.Service.Language, Action.ModulesUpload, (BaseCollection) null));
        }
      }
      else
        base.OnPviRead(errorCode, accessType, dataState, pData, dataLen, option);
    }

    public void Upload() => this.Upload(ModuleListOptions.INA2000CompatibleMode);

    public void Upload(ModuleListOptions lstOption)
    {
      if (this.propParent == null)
        return;
      this.Requests |= Actions.Upload;
      int error = ((Cpu) this.propParent).UpdateModuleList(lstOption);
      if (0 >= error)
        return;
      this.OnError(new CollectionEventArgs(((Base) this.propParent).Name, ((Base) this.propParent).Address, error, this.Service.Language, Action.ModulesUpload, (BaseCollection) null));
    }

    internal override int ToXMLTextWriter(ref XmlTextWriter writer, ConfigurationFlags flags)
    {
      int xmlTextWriter = 0;
      if (this.Values.Count > 0)
      {
        writer.WriteStartElement(this.GetType().Name);
        foreach (object obj in (IEnumerable) this.Values)
        {
          writer.WriteStartElement("Module");
          int num = ((Module) obj).SaveModuleConfiguration(ref writer, flags);
          if (num != 0)
            xmlTextWriter = num;
          writer.WriteEndElement();
        }
        writer.WriteEndElement();
      }
      return xmlTextWriter;
    }

    public void Download(MemoryType memoryType, InstallMode installMode)
    {
      this.propSentCount = 0;
      this.propValidCount = 0;
      this.propErrorCount = 0;
      if (this.validModules == null)
      {
        this.validModules = new ModuleCollection(CollectionType.HashTable, this.Parent, "Valid modules");
        this.validModules.propInternalCollection = true;
      }
      else
        this.validModules.Clear();
      if (this.Count == 0)
      {
        this.OnCollectionDownloaded((CollectionEventArgs) new ModuleCollectionEventArgs(this.propName, "", 0, this.Service.Language, Action.ModuleDownload, this.validModules));
      }
      else
      {
        foreach (Module module in (IEnumerable) this.Values)
        {
          module.Download(memoryType, installMode);
          ++this.propSentCount;
        }
      }
    }

    public void Upload(string path)
    {
      this.propSentCount = 0;
      this.propValidCount = 0;
      if (this.validModules == null)
      {
        this.validModules = new ModuleCollection(CollectionType.HashTable, this.Parent, "Valid modules");
        this.validModules.propInternalCollection = true;
      }
      else
        this.validModules.Clear();
      if (0 >= this.Count)
        return;
      foreach (Module module in (IEnumerable) this.Values)
      {
        module.Upload(string.Format("{0}\\{1}.br", (object) path, (object) module.Name));
        ++this.propSentCount;
      }
    }

    protected internal override void OnConnected(Base sender, PviEventArgs e)
    {
      ++this.propValidCount;
      if (this.validModules == null)
      {
        this.validModules = new ModuleCollection(CollectionType.HashTable, this.Parent, "Valid modules");
        this.validModules.propInternalCollection = true;
      }
      if (!this.validModules.ContainsKey((object) sender.Name))
        this.validModules.Add((Module) sender);
      this.Fire_Connected((object) sender, e);
      if (this.propValidCount + this.propErrorCount != this.propSentCount)
        return;
      this.OnCollectionConnected((CollectionEventArgs) new ModuleCollectionEventArgs(this.propName, "", 0, this.Service.Language, Action.ModulesConnect, this.validModules));
      if (this.propErrorCount <= 0)
        return;
      this.OnCollectionError((CollectionEventArgs) new ModuleCollectionEventArgs(this.propName, "", 0, this.Service.Language, Action.ModulesConnect, this.errorModules));
    }

    protected internal override void OnDisconnected(Base sender, PviEventArgs e)
    {
      this.propConnectionState = ConnectionStates.Disconnected;
      ++this.propValidCount;
      if (this.validModules == null)
      {
        this.validModules = new ModuleCollection(CollectionType.HashTable, this.Parent, "Valid modules");
        this.validModules.propInternalCollection = true;
      }
      if (!this.validModules.ContainsKey((object) sender.Name))
        this.validModules.Add((Module) sender);
      this.Fire_Disconnected((object) sender, e);
      if (this.propValidCount != this.propSentCount || this.Service == null)
        return;
      this.OnCollectionDisconnected((CollectionEventArgs) new ModuleCollectionEventArgs(this.propName, "", 0, this.Service.Language, Action.ModulesDisconnect, this.validModules));
      if (this.propErrorCount <= 0)
        return;
      this.OnCollectionError((CollectionEventArgs) new ModuleCollectionEventArgs(this.propName, "", 0, this.Service.Language, Action.ModulesDisconnect, this.errorModules));
    }

    protected internal override void OnError(Base sender, PviEventArgs e)
    {
      if (this.errorModules == null)
      {
        this.errorModules = new ModuleCollection(CollectionType.HashTable, this.Parent, "Error modules");
        this.errorModules.propInternalCollection = true;
      }
      if (!this.errorModules.ContainsKey((object) sender.Name))
      {
        this.errorModules.Add((Module) sender);
        ++this.propErrorCount;
      }
      this.Fire_Error((object) sender, e);
      if (this.propSentCount != 0)
      {
        if (e.Action == Action.ModuleDownload)
        {
          this.percentage = this.propErrorCount + this.propValidCount != this.propSentCount ? 100 / this.propSentCount * (this.propValidCount + this.propErrorCount) : 100;
          this.OnCollectionDownloadProgress(new ModuleCollectionProgressEventArgs(this.propName, "", e.ErrorCode, this.Service.Language, Action.ModulesDownload, (Module) sender, this.percentage));
        }
        if (e.Action == Action.ModuleUpload)
        {
          this.percentage = this.propErrorCount + this.propValidCount != this.propSentCount ? 100 / this.propSentCount * (this.propValidCount + this.propErrorCount) : 100;
          this.OnCollectionUploadProgress(new ModuleCollectionProgressEventArgs(this.propName, "", e.ErrorCode, this.Service.Language, Action.ModulesUpload, (Module) sender, this.percentage));
        }
      }
      if (this.propValidCount + this.propErrorCount != this.Count)
        return;
      this.OnCollectionError((CollectionEventArgs) new ModuleCollectionEventArgs(this.propName, "", e.ErrorCode, this.Service.Language, e.Action, this.errorModules));
      if (this.propValidCount <= 0)
        return;
      switch (e.Action)
      {
        case Action.ModuleConnect:
        case Action.LoggerConnect:
          this.OnCollectionConnected((CollectionEventArgs) new ModuleCollectionEventArgs(this.propName, "", 0, this.Service.Language, Action.ModulesConnect, this.validModules));
          break;
        case Action.ModuleDisconnect:
          this.OnCollectionDisconnected((CollectionEventArgs) new ModuleCollectionEventArgs(this.propName, "", 0, this.Service.Language, Action.ModulesDisconnect, this.validModules));
          break;
        case Action.ModuleUpload:
          this.OnCollectionUploaded((CollectionEventArgs) new ModuleCollectionEventArgs(this.propName, "", 0, this.Service.Language, Action.ModulesUpload, this.validModules));
          break;
        case Action.ModuleDownload:
          this.OnCollectionDownloaded((CollectionEventArgs) new ModuleCollectionEventArgs(this.propName, "", 0, this.Service.Language, Action.ModulesDownload, this.validModules));
          break;
      }
    }

    public virtual void OnModuleDownloaded(Module module, PviEventArgs e)
    {
      if (e.ErrorCode == 0)
      {
        ++this.propValidCount;
        if (this.validModules == null)
        {
          this.validModules = new ModuleCollection(CollectionType.HashTable, this.Parent, "Valid modules");
          this.validModules.propInternalCollection = true;
        }
        if (!this.validModules.ContainsKey((object) module.Name))
          this.validModules.Add(module);
      }
      else
      {
        ++this.propErrorCount;
        if (this.errorModules == null)
        {
          this.errorModules = new ModuleCollection(CollectionType.HashTable, this.Parent, "Error modules");
          this.errorModules.propInternalCollection = true;
        }
        if (!this.errorModules.ContainsKey((object) module.Name))
          this.errorModules.Add(module);
        this.Fire_Error((object) module, e);
      }
      if (this.ModuleDownloaded != null)
        this.ModuleDownloaded((object) module, e);
      if (this.propSentCount != 0)
      {
        this.percentage = this.propErrorCount + this.propValidCount != this.propSentCount ? 100 / this.propSentCount * (this.propValidCount + this.propErrorCount) : 100;
        this.OnCollectionDownloadProgress(new ModuleCollectionProgressEventArgs(this.propName, "", e.ErrorCode, this.Service.Language, Action.ModulesDownload, module, this.percentage));
      }
      if (this.propValidCount + this.propErrorCount != this.propSentCount)
        return;
      this.OnCollectionDownloaded((CollectionEventArgs) new ModuleCollectionEventArgs(this.propName, "", 0, this.Service.Language, Action.ModulesDownload, this.validModules));
      if (this.propErrorCount <= 0)
        return;
      this.OnCollectionError((CollectionEventArgs) new ModuleCollectionEventArgs(this.propName, "", 0, this.Service.Language, Action.ModulesDownload, this.errorModules));
    }

    protected internal virtual void OnModuleUploaded(Module module, PviEventArgs e)
    {
      if (e.ErrorCode == 0)
      {
        ++this.propValidCount;
        if (this.validModules == null)
        {
          this.validModules = new ModuleCollection(CollectionType.HashTable, this.Parent, "Valid modules");
          this.validModules.propInternalCollection = true;
        }
        if (!this.validModules.ContainsKey((object) module.Name))
          this.validModules.Add(module);
      }
      else
      {
        ++this.propErrorCount;
        if (this.errorModules == null)
        {
          this.errorModules = new ModuleCollection(CollectionType.HashTable, this.Parent, "Error modules");
          this.errorModules.propInternalCollection = true;
        }
        if (!this.errorModules.ContainsKey((object) module.Name))
          this.errorModules.Add(module);
        this.Fire_Error((object) module, e);
      }
      if (this.ModuleUploaded != null)
        this.ModuleUploaded((object) module, e);
      if (this.propSentCount != 0)
      {
        this.percentage = this.propErrorCount + this.propValidCount != this.propSentCount ? 100 / this.propSentCount * (this.propValidCount + this.propErrorCount) : 100;
        this.OnCollectionUploadProgress(new ModuleCollectionProgressEventArgs(this.propName, "", e.ErrorCode, this.Service.Language, Action.ModulesUpload, module, this.percentage));
      }
      if (this.propValidCount + this.propErrorCount != this.propSentCount)
        return;
      this.OnCollectionUploaded((CollectionEventArgs) new ModuleCollectionEventArgs(this.propName, "", 0, this.Service.Language, Action.ModulesUpload, this.validModules));
      if (this.propErrorCount <= 0)
        return;
      this.OnCollectionError((CollectionEventArgs) new ModuleCollectionEventArgs(this.propName, "", 0, this.Service.Language, Action.ModulesUpload, this.errorModules));
    }

    internal void CheckUploadedRequest()
    {
      if (Actions.Upload != (this.Requests & Actions.Upload))
        return;
      this.OnCollectionUploaded((CollectionEventArgs) new ModuleCollectionEventArgs(this.propName, "", 0, this.Service.Language, Action.ModulesUpload, this));
    }

    protected void OnCollectionDownloaded(CollectionEventArgs e)
    {
      if (this.CollectionDownloaded != null)
        this.CollectionDownloaded((object) this, e);
      this.propErrorCount = 0;
      this.propValidCount = 0;
      this.propSentCount = 0;
      if (this.validModules != null)
        this.validModules.Clear();
      if (this.errorModules == null)
        return;
      this.errorModules.Clear();
    }

    protected void OnCollectionUploaded(CollectionEventArgs e)
    {
      this.hasBeenUploadedOnce = true;
      if (this.CollectionUploaded != null)
        this.CollectionUploaded((object) this, e);
      this.propErrorCount = 0;
      this.propValidCount = 0;
      this.propSentCount = 0;
      if (this.validModules != null)
        this.validModules.Clear();
      if (this.errorModules == null)
        return;
      this.errorModules.Clear();
    }

    protected void OnCollectionDownloadProgress(ModuleCollectionProgressEventArgs e)
    {
      if (this.CollectionDownloadProgress == null)
        return;
      this.CollectionDownloadProgress((object) this, e);
    }

    protected void OnCollectionUploadProgress(ModuleCollectionProgressEventArgs e)
    {
      if (this.CollectionUploadProgress == null)
        return;
      this.CollectionUploadProgress((object) this, e);
    }

    protected internal virtual void OnDownloadProgress(Module module, ModuleEventArgs e)
    {
      if (this.DownloadProgress == null)
        return;
      this.DownloadProgress((object) module, e);
    }

    protected internal virtual void OnUploadProgress(Module module, ModuleEventArgs e)
    {
      if (this.UploadProgress == null)
        return;
      this.UploadProgress((object) module, e);
    }

    internal virtual void OnModuleCreated(ModuleEventArgs moduleEvent)
    {
      if (this.ModuleCreated == null)
        return;
      this.ModuleCreated((object) this, moduleEvent);
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

    internal int DiagnosticModeUpdateModuleInfo(
      APIFC_DiagModulInfoRes diagModInfo,
      int errorCode,
      ref int updateFlags)
    {
      if (errorCode != 0)
        return errorCode;
      bool flag = false;
      if ((this.Requests & Actions.Upload) != Actions.NONE)
      {
        flag = true;
        updateFlags |= 2;
      }
      Module module = ((Cpu) this.propParent).Modules[diagModInfo.name];
      if (flag && module == null)
        module = new Module((Cpu) this.propParent, diagModInfo.name);
      if (module != null && (flag || (module.Requests & Actions.Upload) != Actions.NONE || (module.Requests & Actions.ModuleInfo) != Actions.NONE))
        module.updateProperties(diagModInfo);
      if (module != null)
      {
        if ((module.Requests & Actions.Upload) != Actions.NONE)
        {
          module.Requests &= Actions.Connect | Actions.GetList | Actions.GetValue | Actions.SetValue | Actions.GetForce | Actions.SetForce | Actions.Start | Actions.Stop | Actions.RunCycle | Actions.GetType | Actions.SetType | Actions.GetLength | Actions.SetRefresh | Actions.CreateLink | Actions.Download | Actions.SetActive | Actions.SetHysteresis | Actions.Disconnect | Actions.SetInitValue | Actions.GetLBType | Actions.ModuleInfo | Actions.ListSNMPVariables | Actions.Connected | Actions.Uploading | Actions.GetCpuInfo | Actions.ReadPVFormat | Actions.FireActivated | Actions.ReadIndex | Actions.FireDataValidated | Actions.FireValueChanged | Actions.Link;
          this.OnModuleUploaded(module, new PviEventArgs(this.propName, "", errorCode, this.Service.Language, Action.ModuleUpload, this.Service));
        }
        if (module.CheckModuleInfo(errorCode))
          module.Fire_OnConnected(new PviEventArgs(module.Name, module.Address, errorCode, this.Service.Language, Action.ConnectedEvent, this.Service));
      }
      return 0;
    }

    internal int UpdateModuleInfo(
      ModuleInfoDecription moduleInfoStruct,
      int errorCode,
      ref int updateFlags,
      bool diagList)
    {
      if (errorCode != 0)
        return errorCode;
      bool flag = false;
      if ((this.Requests & Actions.Upload) != Actions.NONE)
      {
        flag = true;
        updateFlags |= 2;
      }
      ArrayList arrayList = new ArrayList();
      if (0 < ((Cpu) this.propParent).Modules.Count)
      {
        foreach (Module module in (IEnumerable) ((Cpu) this.propParent).Modules.Values)
        {
          if (module.AddressEx.CompareTo(moduleInfoStruct.name) == 0)
          {
            module.updateProperties(moduleInfoStruct, diagList);
            if ((module.Requests & Actions.Upload) != Actions.NONE)
            {
              module.Requests &= Actions.Connect | Actions.GetList | Actions.GetValue | Actions.SetValue | Actions.GetForce | Actions.SetForce | Actions.Start | Actions.Stop | Actions.RunCycle | Actions.GetType | Actions.SetType | Actions.GetLength | Actions.SetRefresh | Actions.CreateLink | Actions.Download | Actions.SetActive | Actions.SetHysteresis | Actions.Disconnect | Actions.SetInitValue | Actions.GetLBType | Actions.ModuleInfo | Actions.ListSNMPVariables | Actions.Connected | Actions.Uploading | Actions.GetCpuInfo | Actions.ReadPVFormat | Actions.FireActivated | Actions.ReadIndex | Actions.FireDataValidated | Actions.FireValueChanged | Actions.Link;
              this.OnModuleUploaded(module, new PviEventArgs(module.Name, module.Address, errorCode, this.Service.Language, Action.ModuleUpload, this.Service));
            }
            if (module.CheckModuleInfo(errorCode))
              arrayList.Add((object) module);
          }
        }
      }
      Module module1 = ((Cpu) this.propParent).Modules[moduleInfoStruct.name];
      if (flag && module1 == null)
      {
        if (Module.isTaskObject(moduleInfoStruct.type) || ((Cpu) this.propParent).Tasks.ContainsKey((object) moduleInfoStruct.name))
        {
          module1 = (Module) ((Cpu) this.propParent).Tasks[moduleInfoStruct.name];
          if (module1 == null)
            module1 = (Module) new Task((Cpu) this.propParent, moduleInfoStruct.name);
          else
            ((Cpu) this.propParent).Modules.Add(module1);
        }
        else if (Module.isLoggerObject(moduleInfoStruct.type) || ((Cpu) this.propParent).Loggers.ContainsKey((object) moduleInfoStruct.name))
        {
          module1 = (Module) ((Cpu) this.propParent).Loggers[moduleInfoStruct.name];
          if (module1 == null)
            module1 = (Module) new Logger((Cpu) this.propParent, moduleInfoStruct.name);
          else
            ((Cpu) this.propParent).Modules.Add(module1);
        }
        else
          module1 = new Module((Cpu) this.propParent, moduleInfoStruct.name);
      }
      if (module1 != null && (flag || (module1.Requests & Actions.Upload) != Actions.NONE || (module1.Requests & Actions.ModuleInfo) != Actions.NONE))
        module1.updateProperties(moduleInfoStruct, diagList);
      if (module1 != null)
      {
        if ((module1.Requests & Actions.Upload) != Actions.NONE)
        {
          module1.Requests &= Actions.Connect | Actions.GetList | Actions.GetValue | Actions.SetValue | Actions.GetForce | Actions.SetForce | Actions.Start | Actions.Stop | Actions.RunCycle | Actions.GetType | Actions.SetType | Actions.GetLength | Actions.SetRefresh | Actions.CreateLink | Actions.Download | Actions.SetActive | Actions.SetHysteresis | Actions.Disconnect | Actions.SetInitValue | Actions.GetLBType | Actions.ModuleInfo | Actions.ListSNMPVariables | Actions.Connected | Actions.Uploading | Actions.GetCpuInfo | Actions.ReadPVFormat | Actions.FireActivated | Actions.ReadIndex | Actions.FireDataValidated | Actions.FireValueChanged | Actions.Link;
          this.OnModuleUploaded(module1, new PviEventArgs(this.propName, "", errorCode, this.Service.Language, Action.ModuleUpload, this.Service));
        }
        if (module1.CheckModuleInfo(errorCode))
          module1.Fire_OnConnected(new PviEventArgs(module1.Name, module1.Address, errorCode, this.Service.Language, Action.ConnectedEvent, this.Service));
      }
      for (int index = 0; index < arrayList.Count; ++index)
      {
        Module module2 = (Module) arrayList[index];
        module2.Fire_OnConnected(new PviEventArgs(module2.Name, module2.Address, errorCode, this.Service.Language, Action.ConnectedEvent, this.Service));
      }
      return 0;
    }

    internal void DoSynchronize(Hashtable newItems)
    {
      Hashtable hashtable = new Hashtable();
      if (this.Values != null)
      {
        foreach (Module module in (IEnumerable) this.Values)
        {
          if (module.Address != null && 0 < module.Address.Length)
          {
            if ((newItems == null || !newItems.ContainsKey((object) module.Address)) && !hashtable.ContainsKey((object) module.Name))
              hashtable.Add((object) module.Name, (object) module);
          }
          else if ((newItems == null || !newItems.ContainsKey((object) module.Name)) && !hashtable.ContainsKey((object) module.Name))
            hashtable.Add((object) module.Name, (object) module);
        }
      }
      if (0 < hashtable.Count)
      {
        foreach (Module remObj in (IEnumerable) hashtable.Values)
        {
          if (((Cpu) this.propParent).IsSG4Target || remObj.Name.CompareTo("$LOG285$") != 0)
            this.RemoveFromCollection((Base) remObj, Action.ModuleDelete);
        }
        hashtable.Clear();
      }
    }

    public event ModuleEventHandler ModuleCreated;

    public event ModuleEventHandler ModuleChanged;

    public event ModuleEventHandler ModuleDeleted;

    public event PviEventHandler ModuleUploaded;

    public event PviEventHandler ModuleDownloaded;

    public event ModuleEventHandler UploadProgress;

    public event ModuleEventHandler DownloadProgress;

    public event CollectionEventHandler CollectionDownloaded;

    public event CollectionEventHandler CollectionUploaded;

    public event ModuleCollectionEventHandler CollectionUploadProgress;

    public event ModuleCollectionEventHandler CollectionDownloadProgress;

    public Module this[string name] => this.propCollectionType == CollectionType.HashTable ? (Module) this[(object) name] : (Module) null;
  }
}
