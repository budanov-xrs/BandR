// Decompiled with JetBrains decompiler
// Type: BR.AN.PviServices.TaskCollection
// Assembly: BR.AN.PVIServices, Version=1.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DA853F0A-C070-4E30-B7A7-51D955FB85F6
// Assembly location: C:\Users\ss_six\Desktop\FXE\FXE software integration package\COMET_FXE-PVIServices_Example\Pluto-PVI-trial_01\packages\IToolS.BandR.3.5.58\lib\net20\BR.AN.PVIServices.dll

using System;
using System.Collections;
using System.Xml;

namespace BR.AN.PviServices
{
  public class TaskCollection : SynchronizableBaseCollection
  {
    private int percentage;
    private TaskCollection errorTasks;
    private TaskCollection validTasks;

    public TaskCollection(object parent, string name)
      : base(parent, name)
    {
      this.synchronize = true;
      this.propParent = parent;
      if (!(this.Parent is Cpu))
        return;
      if (((Cpu) this.Parent).propUserTaskCollections == null)
        ((Cpu) this.Parent).propUserTaskCollections = new Hashtable();
      if (((Cpu) this.Parent).propUserTaskCollections.ContainsKey(this.Name))
        return;
      ((Cpu) this.Parent).propUserTaskCollections.Add(this.Name, (object) this);
    }

    internal TaskCollection(CollectionType colType, object parent, string name)
      : base(colType, parent, name)
    {
      this.synchronize = true;
      this.propParent = parent;
    }

    public override void Connect(ConnectionType connectionType)
    {
      this.propSentCount = 0;
      this.propValidCount = 0;
      this.propErrorCount = 0;
      int num = 0;
      if (this.validTasks == null)
      {
        this.validTasks = new TaskCollection(CollectionType.HashTable, this.Parent, "Valid tasks");
        this.validTasks.propInternalCollection = true;
      }
      else
        this.validTasks.Clear();
      if (this.Count == 0)
      {
        this.OnCollectionConnected((CollectionEventArgs) new TaskCollectionEventArgs(this.propName, "", 0, this.Service.Language, Action.ModulesConnect, this.validTasks));
      }
      else
      {
        foreach (Task task in (IEnumerable) this.Values)
        {
          ++this.propSentCount;
          if (ConnectionStates.Connected == task.propConnectionState || ConnectionStates.ConnectedError == task.propConnectionState)
          {
            this.validTasks.Add(task);
            ++this.propValidCount;
            ++num;
            if (num + this.propErrorCount == this.Count)
              this.OnCollectionConnected((CollectionEventArgs) new TaskCollectionEventArgs(this.propName, "", 0, this.Service.Language, Action.ModulesConnect, this.validTasks));
          }
          else
            task.Connect(connectionType);
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
        this.OnCollectionDisconnected((CollectionEventArgs) new TaskCollectionEventArgs(this.propName, "", 0, this.Service.Language, Action.ModulesDisconnect, this));
      }
      else
      {
        if (this.validTasks == null)
        {
          this.validTasks = new TaskCollection(this.Parent, "Valid tasks");
          this.validTasks.propInternalCollection = true;
        }
        else
          this.validTasks.Clear();
        if (this.Count == 0)
          this.OnCollectionDisconnected((CollectionEventArgs) new TaskCollectionEventArgs(this.propName, "", 0, this.Service.Language, Action.ModulesDisconnect, this.validTasks));
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
      foreach (Task task in (IEnumerable) this.Values)
      {
        if (task.LinkId == 0U)
        {
          if (!noResponse)
            this.OnDisconnected(task, new PviEventArgs(task.Name, task.Address, 4808, "en", Action.TaskDisconnect));
        }
        else
        {
          if (ConnectionStates.Connected != task.propConnectionState && ConnectionStates.ConnectedError != task.propConnectionState)
          {
            ++num2;
            if (num2 == this.Count && !noResponse)
              this.OnCollectionDisconnected((CollectionEventArgs) new TaskCollectionEventArgs(this.propName, "", 0, this.Service.Language, Action.ModulesDisconnect, this.validTasks));
            ++this.propValidCount;
            if (!noResponse)
              this.validTasks.Add(task);
          }
          else
          {
            if (noResponse)
              task.Variables.Disconnect(true);
            task.Disconnect(noResponse);
          }
          ++this.propSentCount;
        }
      }
      return num1;
    }

    protected internal override void OnConnected(Base sender, PviEventArgs e)
    {
      ++this.propValidCount;
      if (this.validTasks == null)
      {
        this.validTasks = new TaskCollection(CollectionType.HashTable, this.Parent, "Valid tasks");
        this.validTasks.propInternalCollection = true;
      }
      if (!this.validTasks.ContainsKey((object) sender.Name))
        this.validTasks.Add((Task) sender);
      this.Fire_Connected((object) sender, e);
      if (this.propValidCount + this.propErrorCount != this.propSentCount)
        return;
      this.OnCollectionConnected((CollectionEventArgs) new TaskCollectionEventArgs(this.propName, "", 0, this.Service.Language, Action.TasksConnect, this.validTasks));
      if (this.propErrorCount <= 0)
        return;
      this.OnCollectionError((CollectionEventArgs) new TaskCollectionEventArgs(this.propName, "", 0, this.Service.Language, Action.TasksConnect, this.errorTasks));
    }

    protected internal virtual void OnDisconnected(Task task, PviEventArgs e)
    {
      this.propConnectionState = ConnectionStates.Disconnected;
      ++this.propValidCount;
      if (this.validTasks == null)
      {
        this.validTasks = new TaskCollection(this.Parent, "Valid tasks");
        this.validTasks.propInternalCollection = true;
      }
      if (!this.validTasks.ContainsKey((object) task.Name))
        this.validTasks.Add(task);
      this.Fire_Disconnected((object) task, e);
      if (this.propValidCount != this.propSentCount)
        return;
      this.OnCollectionDisconnected((CollectionEventArgs) new TaskCollectionEventArgs(this.propName, "", 0, this.Service.Language, Action.TasksDisconnect, this.validTasks));
      if (this.propErrorCount <= 0)
        return;
      this.OnCollectionError((CollectionEventArgs) new TaskCollectionEventArgs(this.propName, "", 0, this.Service.Language, Action.TasksDisconnect, this.errorTasks));
    }

    protected internal virtual void OnError(Task task, PviEventArgs e)
    {
      ++this.propErrorCount;
      if (this.errorTasks == null)
      {
        this.errorTasks = new TaskCollection(this.Parent, "Error tasks");
        this.errorTasks.propInternalCollection = true;
      }
      if (!this.errorTasks.ContainsKey((object) task.Name))
        this.errorTasks.Add(task);
      this.Fire_Error((object) task, e);
      if (this.propValidCount + this.propErrorCount != this.Count)
        return;
      this.OnCollectionError((CollectionEventArgs) new TaskCollectionEventArgs(this.propName, "", 0, this.Service.Language, Action.NONE, this.errorTasks));
      if (this.propValidCount <= 0)
        return;
      switch (e.Action)
      {
        case Action.TaskConnect:
          this.OnCollectionConnected((CollectionEventArgs) new TaskCollectionEventArgs(this.propName, "", 0, this.Service.Language, Action.TasksConnect, this.validTasks));
          break;
        case Action.TaskDisconnect:
          this.OnCollectionDisconnected((CollectionEventArgs) new TaskCollectionEventArgs(this.propName, "", 0, this.Service.Language, Action.TasksDisconnect, this.validTasks));
          break;
        case Action.TaskUpload:
          this.OnCollectionUploaded((CollectionEventArgs) new TaskCollectionEventArgs(this.propName, "", 0, this.Service.Language, Action.TasksUpload, this.validTasks));
          break;
        case Action.TasksDownload:
          this.OnCollectionDownloaded((CollectionEventArgs) new TaskCollectionEventArgs(this.propName, "", 0, this.Service.Language, Action.TasksDownload, this.validTasks));
          break;
      }
    }

    public virtual void OnTaskDownloaded(Task task, PviEventArgs e)
    {
      ++this.propValidCount;
      if (this.validTasks == null)
      {
        this.validTasks = new TaskCollection(this.Parent, "Valid tasks");
        this.validTasks.propInternalCollection = true;
      }
      if (!this.validTasks.ContainsKey((object) task.Name))
        this.validTasks.Add(task);
      if (this.Downloaded != null)
        this.Downloaded((object) task, e);
      if (this.propValidCount + this.propErrorCount == this.propSentCount)
      {
        this.OnCollectionDownloaded((CollectionEventArgs) new TaskCollectionEventArgs(this.propName, "", 0, this.Service.Language, Action.TasksDownload, this.validTasks));
        if (this.propErrorCount > 0)
          this.OnCollectionError((CollectionEventArgs) new TaskCollectionEventArgs(this.propName, "", 0, this.Service.Language, Action.TasksDownload, this.errorTasks));
      }
      if (this.propSentCount == 0)
        return;
      this.percentage = this.propErrorCount + this.propValidCount != this.propSentCount ? 100 / this.propSentCount * (this.propValidCount + this.propErrorCount) : 100;
      this.OnCollectionDownloadProgress(new ModuleCollectionProgressEventArgs(this.propName, "", 0, this.Service.Language, Action.TasksDownload, (Module) task, this.percentage));
    }

    internal virtual void OnTaskDeleted(ModuleEventArgs moduleEvent)
    {
      if (this.ModuleDeleted == null)
        return;
      this.ModuleDeleted((object) this, moduleEvent);
    }

    public virtual void OnTaskUploaded(Task task, PviEventArgs e)
    {
      ++this.propValidCount;
      if (this.validTasks == null)
      {
        this.validTasks = new TaskCollection(this.Parent, "Valid tasks");
        this.validTasks.propInternalCollection = true;
      }
      if (!this.validTasks.ContainsKey((object) task.Name))
        this.validTasks.Add(task);
      if (this.TaskUploaded != null)
        this.TaskUploaded((object) task, e);
      if (this.propValidCount + this.propErrorCount != this.propSentCount)
        return;
      this.OnCollectionUploaded((CollectionEventArgs) new TaskCollectionEventArgs(this.propName, "", 0, this.Service.Language, Action.TasksUpload, this.validTasks));
      if (this.propErrorCount <= 0)
        return;
      this.OnCollectionError((CollectionEventArgs) new TaskCollectionEventArgs(this.propName, "", 0, this.Service.Language, Action.TasksUpload, this.errorTasks));
    }

    protected void OnCollectionDownloaded(CollectionEventArgs e)
    {
      if (this.CollectionDownloaded == null)
        return;
      this.CollectionDownloaded((object) this, e);
    }

    protected void OnCollectionUploaded(CollectionEventArgs e)
    {
      this.hasBeenUploadedOnce = true;
      if (this.CollectionUploaded == null)
        return;
      this.CollectionUploaded((object) this, e);
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

    internal override void Dispose(bool disposing, bool removeFromCollection)
    {
      if (this.propDisposed)
        return;
      object propParent = this.propParent;
      object propUserData = this.propUserData;
      string propName = this.propName;
      if (this.Parent is Cpu && ((Cpu) this.Parent).propUserTaskCollections != null && ((Cpu) this.Parent).propUserTaskCollections.ContainsKey(this.Name))
        ((Cpu) this.Parent).propUserTaskCollections.Remove(this.Name);
      this.CleanUp(disposing);
      if (this.validTasks != null)
        this.validTasks.Dispose(disposing, false);
      if (this.errorTasks != null)
        this.errorTasks.Dispose(disposing, false);
      this.propParent = propParent;
      this.propUserData = propUserData;
      this.propName = propName;
      base.Dispose(disposing, removeFromCollection);
      this.propParent = (object) null;
      this.propUserData = (object) null;
      this.propName = (string) null;
    }

    public void CleanUp() => this.CleanUp(false);

    internal void CleanUp(bool disposing)
    {
      ArrayList arrayList = new ArrayList();
      if (this.validTasks != null)
        this.validTasks.Clear();
      if (this.errorTasks != null)
        this.errorTasks.Clear();
      if (this.Values != null)
      {
        foreach (Task task in (IEnumerable) this.Values)
          arrayList.Add((object) task);
        for (int index = 0; index < arrayList.Count; ++index)
        {
          Task task = (Task) arrayList[index];
          if (task.LinkId != 0U)
            task.DisconnectRet(0U);
          task.Dispose(disposing, true);
        }
      }
      this.Clear();
    }

    protected internal virtual void OnUploadProgress(Module module, ModuleEventArgs e)
    {
      if (this.UploadProgress == null)
        return;
      this.UploadProgress((object) module, e);
    }

    internal virtual Task GetItem(int index)
    {
      switch (this.propCollectionType)
      {
        case CollectionType.SortedList:
          return (Task) this.propSortedList.GetByIndex(index);
        case CollectionType.ArrayList:
          return (Task) this.propArrayList[index];
        default:
          return (Task) null;
      }
    }

    internal virtual Task GetItem(string address) => this.propCollectionType == CollectionType.HashTable ? this[address] : (Task) null;

    public virtual int Add(Task task)
    {
      if (this[task.Name] == null)
        this.Add((object) task.Name, (object) task);
      else
        this[task.Name].Initialize(task);
      if (!this.propInternalCollection)
      {
        if (task.propUserCollections == null)
          task.propUserCollections = new Hashtable();
        if (!task.propUserCollections.ContainsKey(this.Name))
          task.propUserCollections.Add(this.Name, (object) this);
      }
      return 0;
    }

    public override void Remove(string key)
    {
      if (this.validTasks != null && this.validTasks.ContainsKey((object) key))
        this.validTasks.Remove(key);
      if (this.errorTasks != null && this.errorTasks.ContainsKey((object) key))
        this.errorTasks.Remove(key);
      base.Remove(key);
    }

    public override void Remove(object key)
    {
      if (!(key is Task))
        return;
      this.Remove(((Base) key).Name);
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

    public void Upload()
    {
      this.Requests |= Actions.Upload;
      int error = this.UpdateTaskList();
      if (0 >= error)
        return;
      this.OnError(new CollectionEventArgs(((Base) this.propParent).Name, ((Base) this.propParent).Address, error, this.Service.Language, Action.ModulesUpload, (BaseCollection) null));
    }

    internal int UpdateTaskList()
    {
      if (!((Base) this.propParent).IsConnected)
      {
        this.Requests |= Actions.Upload;
        return -2;
      }
      this.propValidCount = 0;
      this.propErrorCount = 0;
      if (this.errorTasks != null)
        this.errorTasks.Clear();
      if (this.validTasks != null)
        this.validTasks.Clear();
      return ((Cpu) this.Parent).Connection.DeviceType == DeviceType.ANSLTcp ? this.ReadArgumentRequest(this.Service.hPvi, ((Base) this.propParent).LinkId, AccessTypes.ANSL_ModuleList, IntPtr.Zero, 0, 415U, this.InternId) : this.ReadArgumentRequest(this.Service.hPvi, ((Base) this.propParent).LinkId, AccessTypes.ModuleList, IntPtr.Zero, 0, 415U, this.InternId);
    }

    public void Upload(string path)
    {
      this.propSentCount = 0;
      this.propValidCount = 0;
      if (this.validTasks == null)
      {
        this.validTasks = new TaskCollection(this.Parent, "Valid tasks");
        this.validTasks.propInternalCollection = true;
      }
      else
        this.validTasks.Clear();
      if (0 >= this.Count)
        return;
      foreach (Task task in (IEnumerable) this.Values)
      {
        task.Upload(string.Format("{0}\\{1}.br", (object) path, (object) task.Name));
        ++this.propSentCount;
      }
    }

    private void TaskListFromCB(int errorCode, IntPtr pData, uint dataLen, bool isANSL)
    {
      int updateFlags = 0;
      Hashtable newItems = new Hashtable();
      if (isANSL)
      {
        Hashtable hashtable = new Hashtable();
        foreach (ModuleInfoDecription moduleInfoStruct in (IEnumerable) ((Cpu) this.Parent).ReadANSLMODList(pData, dataLen).Values)
        {
          if (moduleInfoStruct.name != null && Module.isTaskObject(moduleInfoStruct.type))
          {
            newItems.Add((object) moduleInfoStruct.name, (object) moduleInfoStruct.name);
            this.UpdateModuleInfo(moduleInfoStruct, errorCode, ref updateFlags, ((Cpu) this.propParent).BootMode == BootMode.Diagnostics);
          }
        }
      }
      else
      {
        int num = (int) (dataLen / 164U);
        for (int index = 0; index < num; ++index)
        {
          APIFC_ModulInfoRes modulInfoStructure = PviMarshal.PtrToModulInfoStructure((IntPtr) ((int) pData + index * 164), typeof (APIFC_ModulInfoRes));
          if (modulInfoStructure.name != null && Module.isTaskObject(modulInfoStructure.type))
          {
            newItems.Add((object) modulInfoStructure.name, (object) modulInfoStructure.name);
            this.UpdateModuleInfo(modulInfoStructure, errorCode, ref updateFlags);
          }
        }
      }
      if (this.synchronize || 1 == (updateFlags & 1))
        this.DoSynchronize(newItems);
      this.CheckFireUploadEvents(4224 == errorCode ? 0 : errorCode, Action.TasksUpload, Action.TasksConnect);
      this.OnCollectionUploaded((CollectionEventArgs) new TaskCollectionEventArgs(this.propName, "", 0, this.Service.Language, Action.TasksUpload, this.validTasks));
      if (this.propErrorCount <= 0)
        return;
      this.OnCollectionError((CollectionEventArgs) new TaskCollectionEventArgs(this.propName, "", 0, this.Service.Language, Action.TasksUpload, this.errorTasks));
    }

    private void OldTaskListFromCB(int errorCode, IntPtr pData, uint dataLen)
    {
      if (errorCode == 0 && 0U < dataLen)
      {
        string str = PviMarshal.PtrToStringAnsi(pData, dataLen);
        int length = str.IndexOf("\0");
        if (-1 != length)
          str = str.Substring(0, length);
        if (str != "")
        {
          foreach (string name in str.Split("\t".ToCharArray()))
          {
            if (this.propParent is Cpu)
            {
              Task task = new Task((Cpu) this.propParent, name, this);
            }
          }
        }
        this.OnUploaded(new PviEventArgs(this.propName, "", errorCode, this.Service.Language, Action.TasksUpload, this.Service));
      }
      else
      {
        this.OnUploaded(new PviEventArgs(this.propName, "", errorCode, this.Service.Language, Action.TasksUpload, this.Service));
        this.OnError(new CollectionEventArgs(this.propName, "", errorCode, this.Service.Language, Action.TasksUpload, (BaseCollection) null));
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
      if (PVIReadAccessTypes.ModuleList == accessType)
        this.TaskListFromCB(errorCode, pData, dataLen, false);
      else if (PVIReadAccessTypes.ANSL_ModuleList == accessType)
        this.TaskListFromCB(errorCode, pData, dataLen, true);
      else
        base.OnPviRead(errorCode, accessType, dataState, pData, dataLen, option);
    }

    internal override int ToXMLTextWriter(ref XmlTextWriter writer, ConfigurationFlags flags)
    {
      int xmlTextWriter1 = 0;
      if (this.Values.Count > 0)
      {
        writer.WriteStartElement(this.GetType().Name);
        foreach (Base @base in (IEnumerable) this.Values)
        {
          int xmlTextWriter2 = @base.ToXMLTextWriter(ref writer, flags);
          if (xmlTextWriter2 != 0)
            xmlTextWriter1 = xmlTextWriter2;
        }
        writer.WriteEndElement();
      }
      return xmlTextWriter1;
    }

    internal int DiagnosticModeUpdateModuleInfo(
      APIFC_DiagModulInfoRes diagModInfo,
      int errorCode,
      ref int updateFlags)
    {
      if (errorCode != 0)
        return errorCode;
      if ((this.Requests & Actions.Upload) != Actions.NONE)
        updateFlags |= 4;
      Task task = ((Cpu) this.propParent).Tasks[diagModInfo.name];
      if (task != null)
      {
        task.updateProperties(diagModInfo);
        if ((task.Requests & Actions.Upload) != Actions.NONE)
        {
          task.Requests &= Actions.Connect | Actions.GetList | Actions.GetValue | Actions.SetValue | Actions.GetForce | Actions.SetForce | Actions.Start | Actions.Stop | Actions.RunCycle | Actions.GetType | Actions.SetType | Actions.GetLength | Actions.SetRefresh | Actions.CreateLink | Actions.Download | Actions.SetActive | Actions.SetHysteresis | Actions.Disconnect | Actions.SetInitValue | Actions.GetLBType | Actions.ModuleInfo | Actions.ListSNMPVariables | Actions.Connected | Actions.Uploading | Actions.GetCpuInfo | Actions.ReadPVFormat | Actions.FireActivated | Actions.ReadIndex | Actions.FireDataValidated | Actions.FireValueChanged | Actions.Link;
          this.OnTaskUploaded(task, new PviEventArgs(this.propName, "", errorCode, this.Service.Language, Action.TaskUpload, this.Service));
        }
        if (task.CheckModuleInfo(errorCode))
          task.Fire_OnConnected(new PviEventArgs(task.Name, task.Address, errorCode, this.Service.Language, Action.ConnectedEvent, this.Service));
      }
      return 0;
    }

    internal int UpdateModuleInfo(
      APIFC_ModulInfoRes moduleInfoStruct,
      int errorCode,
      ref int updateFlags)
    {
      ModuleInfoDecription moduleInfoStruct1 = new ModuleInfoDecription();
      moduleInfoStruct1.Init(moduleInfoStruct);
      return this.UpdateModuleInfo(moduleInfoStruct1, errorCode, ref updateFlags, false);
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
        updateFlags |= 4;
      }
      ArrayList arrayList1 = new ArrayList();
      ArrayList arrayList2 = new ArrayList();
      Task task1 = (Task) null;
      if (Module.isTaskObject(moduleInfoStruct.type))
      {
        if (0 < this.Count)
        {
          foreach (Task task2 in (IEnumerable) ((Cpu) this.propParent).Tasks.Values)
          {
            if (task2.AddressEx.CompareTo(moduleInfoStruct.name) == 0)
            {
              task2.updateProperties(moduleInfoStruct, diagList);
              if ((task2.Requests & Actions.Upload) != Actions.NONE)
              {
                task2.Requests &= Actions.Connect | Actions.GetList | Actions.GetValue | Actions.SetValue | Actions.GetForce | Actions.SetForce | Actions.Start | Actions.Stop | Actions.RunCycle | Actions.GetType | Actions.SetType | Actions.GetLength | Actions.SetRefresh | Actions.CreateLink | Actions.Download | Actions.SetActive | Actions.SetHysteresis | Actions.Disconnect | Actions.SetInitValue | Actions.GetLBType | Actions.ModuleInfo | Actions.ListSNMPVariables | Actions.Connected | Actions.Uploading | Actions.GetCpuInfo | Actions.ReadPVFormat | Actions.FireActivated | Actions.ReadIndex | Actions.FireDataValidated | Actions.FireValueChanged | Actions.Link;
                arrayList1.Add((object) task2);
              }
              if (task2.CheckModuleInfo(errorCode))
                arrayList2.Add((object) task2);
            }
          }
        }
        task1 = ((Cpu) this.propParent).Tasks[moduleInfoStruct.name];
        if (flag && task1 == null)
          task1 = new Task((Cpu) this.propParent, moduleInfoStruct.name);
        if (task1 != null && (flag || (task1.Requests & Actions.Upload) != Actions.NONE || (task1.Requests & Actions.ModuleInfo) != Actions.NONE))
          task1.updateProperties(moduleInfoStruct, diagList);
      }
      if (task1 != null)
      {
        if ((task1.Requests & Actions.Upload) != Actions.NONE)
        {
          task1.Requests &= Actions.Connect | Actions.GetList | Actions.GetValue | Actions.SetValue | Actions.GetForce | Actions.SetForce | Actions.Start | Actions.Stop | Actions.RunCycle | Actions.GetType | Actions.SetType | Actions.GetLength | Actions.SetRefresh | Actions.CreateLink | Actions.Download | Actions.SetActive | Actions.SetHysteresis | Actions.Disconnect | Actions.SetInitValue | Actions.GetLBType | Actions.ModuleInfo | Actions.ListSNMPVariables | Actions.Connected | Actions.Uploading | Actions.GetCpuInfo | Actions.ReadPVFormat | Actions.FireActivated | Actions.ReadIndex | Actions.FireDataValidated | Actions.FireValueChanged | Actions.Link;
          this.OnTaskUploaded(task1, new PviEventArgs(this.propName, "", errorCode, this.Service.Language, Action.TaskUpload, this.Service));
        }
        if (task1.CheckModuleInfo(errorCode))
          task1.Fire_OnConnected(new PviEventArgs(task1.Name, task1.Address, errorCode, this.Service.Language, Action.ConnectedEvent, this.Service));
      }
      for (int index = 0; index < arrayList2.Count; ++index)
      {
        Task task3 = (Task) arrayList2[index];
        task3.Fire_OnConnected(new PviEventArgs(task3.Name, task3.Address, errorCode, this.Service.Language, Action.ConnectedEvent, this.Service));
      }
      for (int index = 0; index < arrayList1.Count; ++index)
      {
        Task task4 = (Task) arrayList1[index];
        this.OnTaskUploaded(task4, new PviEventArgs(task4.Name, task4.Address, errorCode, this.Service.Language, Action.TaskUpload, this.Service));
      }
      return 0;
    }

    internal void DoSynchronize(Hashtable newItems)
    {
      Hashtable hashtable = new Hashtable();
      if (this.Values != null)
      {
        foreach (Task task in (IEnumerable) this.Values)
        {
          if (task.Address == null && 0 < task.Address.Length)
          {
            if ((newItems != null || !newItems.ContainsKey((object) task.Address)) && !hashtable.ContainsKey((object) task.Name))
              hashtable.Add((object) task.Name, (object) task);
          }
          else if ((newItems == null || !newItems.ContainsKey((object) task.Name)) && !hashtable.ContainsKey((object) task.Name))
            hashtable.Add((object) task.Name, (object) task);
        }
      }
      if (0 < hashtable.Count)
      {
        foreach (Base remObj in (IEnumerable) hashtable.Values)
          this.RemoveFromCollection(remObj, Action.TaskDelete);
        hashtable.Clear();
      }
    }

    public Task this[string name] => (Task) this[(object) name];

    public override Service Service
    {
      get
      {
        if (this.propParent is Cpu)
          return ((Base) this.propParent).Service;
        return this.propParent is Service ? (Service) this.propParent : (Service) null;
      }
    }

    public event ModuleEventHandler ModuleCreated;

    public event ModuleEventHandler ModuleChanged;

    public event ModuleEventHandler ModuleDeleted;

    public event PviEventHandler TaskUploaded;

    public event PviEventHandler Downloaded;

    public event ModuleEventHandler UploadProgress;

    public event ModuleEventHandler DownloadProgress;

    public event CollectionEventHandler CollectionDownloaded;

    public event CollectionEventHandler CollectionUploaded;

    public event ModuleCollectionEventHandler CollectionUploadProgress;

    public event ModuleCollectionEventHandler CollectionDownloadProgress;
  }
}
