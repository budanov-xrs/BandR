// Decompiled with JetBrains decompiler
// Type: BR.AN.PviServices.BaseCollection
// Assembly: BR.AN.PVIServices, Version=1.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DA853F0A-C070-4E30-B7A7-51D955FB85F6
// Assembly location: C:\Users\ss_six\Desktop\FXE\FXE software integration package\COMET_FXE-PVIServices_Example\Pluto-PVI-trial_01\packages\IToolS.BandR.3.5.58\lib\net20\BR.AN.PVIServices.dll

using System;
using System.Collections;
using System.Xml;

namespace BR.AN.PviServices
{
  public abstract class BaseCollection : PviCBEvents, IDisposable, ICollection, IEnumerable
  {
    internal CollectionType propCollectionType;
    internal bool propDisposed;
    internal object propParent;
    internal string propName;
    internal SortedList propSortedList;
    internal Hashtable propHashTable;
    internal ArrayList propArrayList;
    internal Actions propResponses;
    internal Actions propRequests;
    internal IEnumerator propEnumer;
    internal ConnectionStates propConnectionState;
    internal bool propActive;
    internal int propRefreshTime;
    internal int propCounter;
    internal bool propInternalCollection;
    private Service propService;
    internal object propUserData;
    internal int propValidCount;
    internal int propSentCount;
    internal int propErrorCount;
    internal int propDisconnectedCount;
    internal uint propInternID;

    internal BaseCollection()
    {
      this.propConnectionState = ConnectionStates.Unininitialized;
      this.propInternalCollection = false;
      this.propUserData = (object) null;
      this.propService = (Service) null;
      this.propDisposed = false;
      this.propCollectionType = CollectionType.HashTable;
      this.propParent = (object) null;
      this.propName = "";
      this.propHashTable = new Hashtable();
    }

    internal BaseCollection(object parentObj, string name)
    {
      this.propConnectionState = ConnectionStates.Unininitialized;
      this.propUserData = (object) null;
      this.propDisposed = false;
      this.propCollectionType = CollectionType.HashTable;
      this.propParent = parentObj;
      this.propService = (Service) null;
      switch (parentObj)
      {
        case Cpu _:
          this.propService = ((Base) parentObj).Service;
          break;
        case Task _:
          this.propService = ((Base) parentObj).Service;
          break;
        case Variable _:
          this.propService = ((Base) parentObj).Service;
          break;
        case Service _:
          this.propService = ((Base) parentObj).Service;
          break;
      }
      this.propName = name;
      this.propHashTable = new Hashtable();
      this.AddToCBReceivers();
    }

    internal BaseCollection(CollectionType colType, object parentObj, string name)
    {
      this.propConnectionState = ConnectionStates.Unininitialized;
      this.propUserData = (object) null;
      this.propService = (Service) null;
      this.propParent = parentObj;
      switch (parentObj)
      {
        case Cpu _:
          this.propService = ((Base) parentObj).Service;
          break;
        case Task _:
          this.propService = ((Base) parentObj).Service;
          break;
        case Variable _:
          this.propService = ((Base) parentObj).Service;
          break;
        case Service _:
          this.propService = ((Base) parentObj).Service;
          break;
      }
      this.propDisposed = false;
      this.propCollectionType = colType;
      this.propName = name;
      switch (colType)
      {
        case CollectionType.SortedList:
          this.propSortedList = new SortedList((IComparer) new Comparer());
          break;
        case CollectionType.ArrayList:
          this.propArrayList = new ArrayList(1);
          break;
        default:
          this.propHashTable = new Hashtable();
          break;
      }
      this.AddToCBReceivers();
    }

    public event DisposeEventHandler Disposing;

    internal virtual void OnDisposing(bool disposing)
    {
      if (this.Disposing == null)
        return;
      this.Disposing((object) this, new DisposeEventArgs(disposing));
    }

    public virtual void Connect() => this.Connect(ConnectionType.CreateAndLink);

    public virtual void Connect(ConnectionType connectionType)
    {
      this.propValidCount = 0;
      this.propErrorCount = 0;
      this.propSentCount = 0;
      this.Requests |= Actions.Connect;
      int num = 0;
      if (this.Count == 0)
      {
        this.Requests &= Actions.GetList | Actions.GetValue | Actions.SetValue | Actions.GetForce | Actions.SetForce | Actions.Start | Actions.Stop | Actions.RunCycle | Actions.GetType | Actions.SetType | Actions.GetLength | Actions.SetRefresh | Actions.CreateLink | Actions.Upload | Actions.Download | Actions.SetActive | Actions.SetHysteresis | Actions.Disconnect | Actions.SetInitValue | Actions.GetLBType | Actions.ModuleInfo | Actions.ListSNMPVariables | Actions.Connected | Actions.Uploading | Actions.GetCpuInfo | Actions.ReadPVFormat | Actions.FireActivated | Actions.ReadIndex | Actions.FireDataValidated | Actions.FireValueChanged | Actions.Link;
        this.OnCollectionConnected(new CollectionEventArgs(this.propName, "", 0, this.Service.Language, Action.ConnectedEvent, (BaseCollection) null));
      }
      else
      {
        if (this.propCollectionType != CollectionType.HashTable || 0 >= this.Count)
          return;
        foreach (Base @base in (IEnumerable) this.Values)
        {
          ++this.propSentCount;
          if (ConnectionStates.Connected == @base.propConnectionState)
          {
            ++num;
            ++this.propValidCount;
            if (num + this.propErrorCount == this.Count)
              this.OnCollectionConnected(new CollectionEventArgs(this.propName, "", 0, this.Service.Language, Action.ConnectedEvent, (BaseCollection) null));
          }
          else if (ConnectionStates.ConnectedError == @base.propConnectionState)
          {
            ++num;
            ++this.propErrorCount;
            if (num + this.propErrorCount == this.Count)
              this.OnCollectionConnected(new CollectionEventArgs(this.propName, "", 0, this.Service.Language, Action.ConnectedEvent, (BaseCollection) null));
          }
          else
            @base.Connect(connectionType);
        }
      }
    }

    public void Dispose()
    {
      this.RemoveFromCBReceivers();
      if (this.propDisposed)
        return;
      this.Dispose(true, false);
      GC.SuppressFinalize((object) this);
    }

    internal virtual void Dispose(bool disposing, bool removeFromCollection)
    {
      this.RemoveFromCBReceivers();
      if (this.propDisposed)
        return;
      this.OnDisposing(disposing);
      if (disposing)
        this.propDisposed = true;
      this.propName = (string) null;
      this.propParent = (object) null;
      if (this.propArrayList != null)
      {
        this.propArrayList.Clear();
        this.propArrayList = (ArrayList) null;
      }
      if (this.propEnumer != null)
        this.propEnumer = (IEnumerator) null;
      if (this.propHashTable != null)
      {
        this.propHashTable.Clear();
        this.propHashTable = (Hashtable) null;
      }
      if (this.propSortedList != null)
      {
        this.propSortedList.Clear();
        this.propSortedList = (SortedList) null;
      }
      this.propUserData = (object) null;
    }

    internal int ReadArgumentRequest(
      uint hPvi,
      uint linkID,
      AccessTypes nAccess,
      IntPtr pData,
      int DataLen,
      uint respParam,
      uint internalID)
    {
      StringMarshal stringMarshal = new StringMarshal();
      return this.Service.EventMessageType != EventMessageType.CallBack ? PInvokePvicom.PviComMsgReadArgumentRequest(hPvi, linkID, nAccess, pData, DataLen, this.Service.WindowHandle, respParam, internalID) : PInvokePvicom.PviComReadArgumentRequest(hPvi, linkID, nAccess, pData, DataLen, this.Service.cbRead, 4294967294U, internalID);
    }

    protected internal virtual void OnUploaded(PviEventArgs e)
    {
      if (this.Uploaded == null)
        return;
      this.Uploaded((object) this, e);
    }

    protected internal virtual void OnDisconnected(Base sender, PviEventArgs e)
    {
      ++this.propValidCount;
      this.Fire_Disconnected((object) sender, e);
    }

    protected internal virtual void OnError(Base sender, PviEventArgs e)
    {
      if (this.Error != null)
        this.Error((object) sender, e);
      if (this.propValidCount + this.propErrorCount != this.Count)
        return;
      this.OnCollectionError(new CollectionEventArgs(this.propName, "", 0, this.Service.Language, Action.NONE, (BaseCollection) null));
    }

    protected internal virtual void OnCollectionConnected(CollectionEventArgs e)
    {
      this.propValidCount = 0;
      this.propSentCount = 0;
      if (e.ErrorCode == 0 || 12002 == e.ErrorCode)
      {
        if (ConnectionStates.Connected == this.propConnectionState)
          return;
        this.propConnectionState = ConnectionStates.Connected;
        if (this.CollectionConnected == null)
          return;
        this.CollectionConnected((object) this, e);
      }
      else
      {
        this.propConnectionState = ConnectionStates.ConnectedError;
        if (this.CollectionConnected == null)
          return;
        this.CollectionConnected((object) this, e);
      }
    }

    protected internal virtual void OnConnected(Base sender, PviEventArgs e)
    {
      ++this.propValidCount;
      if (this.Connected != null)
        this.Connected((object) sender, e);
      if (this.propValidCount + this.propErrorCount != this.propSentCount)
        return;
      this.OnCollectionConnected(new CollectionEventArgs(this.propName, "", 0, this.Service.Language, Action.ConnectedEvent, (BaseCollection) null));
      if (this.propErrorCount <= 0)
        return;
      this.OnCollectionError(new CollectionEventArgs(this.propName, "", 0, this.Service.Language, Action.ConnectedEvent, (BaseCollection) null));
    }

    protected internal virtual void OnCollectionDisconnected(CollectionEventArgs e)
    {
      this.propValidCount = 0;
      this.propSentCount = 0;
      this.propConnectionState = ConnectionStates.Disconnected;
      if (this.CollectionDisconnected == null)
        return;
      this.CollectionDisconnected((object) this, e);
    }

    protected internal virtual void OnCollectionError(CollectionEventArgs e)
    {
      this.propErrorCount = 0;
      if (this.CollectionError == null)
        return;
      this.CollectionError((object) this, e);
    }

    public virtual int Count
    {
      get
      {
        switch (this.propCollectionType)
        {
          case CollectionType.SortedList:
            return this.propSortedList != null ? this.propSortedList.Count : 0;
          case CollectionType.ArrayList:
            return this.propArrayList != null ? this.propArrayList.Count : 0;
          default:
            return this.propHashTable != null ? this.propHashTable.Count : 0;
        }
      }
    }

    internal object Clone()
    {
      switch (this.propCollectionType)
      {
        case CollectionType.SortedList:
          return this.propSortedList.Clone();
        case CollectionType.HashTable:
          return this.propHashTable.Clone();
        case CollectionType.ArrayList:
          return this.propArrayList.Clone();
        default:
          return (object) null;
      }
    }

    internal void RemoveFromCollection(Base remObj, Action nAction)
    {
      remObj.DisconnectRet(2821U);
      remObj.RemoveReferences();
      remObj.RemoveFromBaseCollections();
      remObj.RemoveObject();
      this.RemoveFromBaseCollections(remObj.Name, 0);
      remObj.Fire_Deleted(new PviEventArgs(remObj.Name, remObj.Address, 0, remObj.Service.Language, nAction, this.Service));
      this.Remove(remObj.Name);
    }

    internal virtual void RemoveFromBaseCollections(string logicalName, int mode)
    {
      if (this.Service == null)
        return;
      this.Service.LogicalObjects.Remove(logicalName);
      if (this.Service.Services == null)
        return;
      this.Service.Services.LogicalObjects.Remove(logicalName);
    }

    private void RemoveFromCBReceivers()
    {
      if (this.Service != null)
        this.Service.RemoveID(this.propInternID);
      if (0 >= this.Count)
        return;
      foreach (Base @base in (IEnumerable) this.Values)
        @base.RemoveFromCBReceivers();
    }

    private bool AddToCBReceivers() => this.Service != null && this.Service.AddID((object) this, ref this.propInternID);

    public virtual void Clear()
    {
      switch (this.propCollectionType)
      {
        case CollectionType.SortedList:
          if (this.propSortedList == null)
            break;
          this.propSortedList.Clear();
          break;
        case CollectionType.HashTable:
          if (this.propHashTable == null)
            break;
          this.propHashTable.Clear();
          break;
        case CollectionType.ArrayList:
          if (this.propArrayList == null)
            break;
          this.propArrayList.Clear();
          break;
      }
    }

    public virtual bool Contains(string key)
    {
      switch (this.propCollectionType)
      {
        case CollectionType.SortedList:
          return this.propSortedList.Contains((object) key);
        case CollectionType.ArrayList:
          return this.propArrayList.Contains((object) key);
        default:
          return this.propHashTable.Contains((object) key);
      }
    }

    public virtual bool Contains(object valObj)
    {
      switch (this.propCollectionType)
      {
        case CollectionType.SortedList:
          return this.propSortedList.Contains(valObj);
        case CollectionType.ArrayList:
          return this.propArrayList.Contains(valObj);
        default:
          return this.propHashTable.Contains(valObj);
      }
    }

    public virtual bool ContainsKey(object key)
    {
      if (key != null)
      {
        switch (this.propCollectionType)
        {
          case CollectionType.SortedList:
            return this.propSortedList != null && this.propSortedList.ContainsKey(key);
          case CollectionType.ArrayList:
            if (this.propArrayList == null)
              return false;
            if (!(key is string))
              return this.propArrayList.Contains(key);
            for (int index = 0; index < this.propArrayList.Count; ++index)
            {
              if (((string) key).CompareTo(((Base) this.propArrayList[index]).Name) == 0)
                return true;
            }
            break;
          default:
            return this.propHashTable != null && this.propHashTable.ContainsKey(key);
        }
      }
      return false;
    }

    public virtual void Remove(string key)
    {
      if (key == null)
        return;
      switch (this.propCollectionType)
      {
        case CollectionType.SortedList:
          this.propSortedList.Remove((object) key);
          break;
        case CollectionType.ArrayList:
          this.propArrayList.Remove((object) System.Convert.ToInt32(key));
          break;
        default:
          this.propHashTable.Remove((object) key);
          break;
      }
    }

    public virtual void Remove(object key)
    {
      switch (this.propCollectionType)
      {
        case CollectionType.SortedList:
          this.propSortedList.Remove(key);
          break;
        case CollectionType.ArrayList:
          if (key is int index)
          {
            this.propArrayList.RemoveAt(index);
            break;
          }
          this.propArrayList.Remove(key);
          break;
        default:
          this.propHashTable.Remove(key);
          break;
      }
    }

    public object ElementAt(int index)
    {
      switch (this.propCollectionType)
      {
        case CollectionType.SortedList:
          return this.propSortedList != null ? this.propSortedList.GetByIndex(index) : (object) null;
        case CollectionType.ArrayList:
          return this.propArrayList != null ? this.propArrayList[index] : (object) null;
        default:
          return this.propHashTable != null ? this.propHashTable[(object) index.ToString()] : (object) null;
      }
    }

    public object this[object indexer]
    {
      get
      {
        switch (this.propCollectionType)
        {
          case CollectionType.SortedList:
            return this.propSortedList != null ? this.propSortedList[indexer] : (object) null;
          case CollectionType.ArrayList:
            return this.propArrayList != null ? this.propArrayList[System.Convert.ToInt32(indexer)] : (object) null;
          default:
            return this.propHashTable != null ? this.propHashTable[indexer] : (object) null;
        }
      }
    }

    public virtual void Add(object key, object value)
    {
      switch (this.propCollectionType)
      {
        case CollectionType.SortedList:
          if (this.propSortedList == null)
            break;
          this.propSortedList.Add(key, value);
          break;
        case CollectionType.ArrayList:
          if (this.propArrayList == null)
            break;
          this.propArrayList.Add(value);
          break;
        default:
          if (this.propHashTable == null)
            break;
          this.propHashTable.Add(key, value);
          break;
      }
    }

    internal void CheckFireUploadEvents(int errorCode, Action actEvent, Action actCon)
    {
      bool flag = false;
      if ((this.Requests & Actions.Upload) != Actions.NONE)
      {
        this.Requests &= Actions.Connect | Actions.GetList | Actions.GetValue | Actions.SetValue | Actions.GetForce | Actions.SetForce | Actions.Start | Actions.Stop | Actions.RunCycle | Actions.GetType | Actions.SetType | Actions.GetLength | Actions.SetRefresh | Actions.CreateLink | Actions.Download | Actions.SetActive | Actions.SetHysteresis | Actions.Disconnect | Actions.SetInitValue | Actions.GetLBType | Actions.ModuleInfo | Actions.ListSNMPVariables | Actions.Connected | Actions.Uploading | Actions.GetCpuInfo | Actions.ReadPVFormat | Actions.FireActivated | Actions.ReadIndex | Actions.FireDataValidated | Actions.FireValueChanged | Actions.Link;
        flag = true;
      }
      if ((this.Requests & Actions.Uploading) != Actions.NONE)
      {
        this.Requests &= Actions.Connect | Actions.GetList | Actions.GetValue | Actions.SetValue | Actions.GetForce | Actions.SetForce | Actions.Start | Actions.Stop | Actions.RunCycle | Actions.GetType | Actions.SetType | Actions.GetLength | Actions.SetRefresh | Actions.CreateLink | Actions.Upload | Actions.Download | Actions.SetActive | Actions.SetHysteresis | Actions.Disconnect | Actions.SetInitValue | Actions.GetLBType | Actions.ModuleInfo | Actions.ListSNMPVariables | Actions.Connected | Actions.GetCpuInfo | Actions.ReadPVFormat | Actions.FireActivated | Actions.ReadIndex | Actions.FireDataValidated | Actions.FireValueChanged | Actions.Link;
        flag = true;
      }
      if (flag)
        this.OnUploaded(new PviEventArgs(this.propName, "", errorCode, this.Service.Language, actEvent, this.Service));
      if ((this.Requests & Actions.Connect) == Actions.NONE)
        return;
      this.OnCollectionConnected(new CollectionEventArgs(this.propName, "", errorCode, this.Service.Language, actCon, this));
    }

    public virtual void Add(object primKey, object secKey, object value)
    {
      switch (this.propCollectionType)
      {
        case CollectionType.SortedList:
          this.propSortedList.Add(secKey, value);
          break;
        case CollectionType.ArrayList:
          this.propArrayList.Add(value);
          break;
        default:
          this.propHashTable.Add(secKey, value);
          break;
      }
    }

    public virtual IEnumerator GetEnumerator()
    {
      switch (this.propCollectionType)
      {
        case CollectionType.SortedList:
          return (IEnumerator) this.propSortedList.GetEnumerator();
        case CollectionType.ArrayList:
          return this.propArrayList.GetEnumerator();
        default:
          return (IEnumerator) this.propHashTable.GetEnumerator();
      }
    }

    public virtual object SyncRoot
    {
      get
      {
        switch (this.propCollectionType)
        {
          case CollectionType.SortedList:
            return this.propSortedList.SyncRoot;
          case CollectionType.ArrayList:
            return this.propArrayList.SyncRoot;
          default:
            return this.propHashTable.SyncRoot;
        }
      }
    }

    public virtual object Parent => this.propParent;

    public virtual object Name => (object) this.propName;

    public virtual ICollection Values
    {
      get
      {
        switch (this.propCollectionType)
        {
          case CollectionType.SortedList:
            return this.propSortedList != null ? this.propSortedList.Values : (ICollection) this.propSortedList;
          case CollectionType.ArrayList:
            return (ICollection) this.propArrayList;
          default:
            return this.propHashTable != null ? this.propHashTable.Values : (ICollection) this.propHashTable;
        }
      }
    }

    public virtual ICollection Keys
    {
      get
      {
        switch (this.propCollectionType)
        {
          case CollectionType.SortedList:
            return this.propSortedList.Keys;
          case CollectionType.ArrayList:
            Hashtable hashtable = new Hashtable();
            for (int key = 0; key < this.propArrayList.Count; ++key)
              hashtable.Add((object) key, (object) key);
            return hashtable.Keys;
          default:
            return this.propHashTable.Keys;
        }
      }
    }

    public virtual bool IsSynchronized
    {
      get
      {
        switch (this.propCollectionType)
        {
          case CollectionType.SortedList:
            return this.propSortedList.IsSynchronized;
          case CollectionType.ArrayList:
            return this.propArrayList.IsSynchronized;
          default:
            return this.propHashTable.IsSynchronized;
        }
      }
    }

    internal Actions Requests
    {
      get => this.propRequests;
      set => this.propRequests = value;
    }

    internal Actions Responses
    {
      get => this.propResponses;
      set => this.propResponses = value;
    }

    internal uint InternId => this.propInternID;

    public bool HasError => this.propErrorCount > 0;

    public virtual Service Service => this.propService;

    public object UserData
    {
      get => this.propUserData;
      set => this.propUserData = value;
    }

    internal bool IsConnected => ConnectionStates.Connected == this.propConnectionState || ConnectionStates.ConnectedError == this.propConnectionState;

    public virtual void CopyTo(Array array, int count)
    {
    }

    public int FromXmlTextReader(
      ref XmlTextReader reader,
      ConfigurationFlags flags,
      Base parentObj)
    {
      bool flag = false;
      Variable variable = (Variable) null;
      Base @base = parentObj;
      if (reader.Name.ToLower().CompareTo("module") != 0 && reader.Name.ToLower().CompareTo("task") != 0 && reader.Name.ToLower().CompareTo("variable") != 0 && reader.Name.ToLower().CompareTo("logger") != 0 && reader.Name.ToLower().CompareTo("taskclass") != 0 && reader.Name.ToLower().CompareTo("iodatapoint") != 0 && reader.Name.ToLower().CompareTo("memory") != 0 && reader.Name.ToLower().CompareTo("library") != 0 && reader.Name.ToLower().CompareTo("members") != 0)
        reader.Read();
      int depth = reader.Depth;
label_37:
      while (reader.NodeType != XmlNodeType.EndElement)
      {
        if (reader.NodeType == XmlNodeType.Comment)
        {
          reader.Read();
        }
        else
        {
          string attribute = reader.GetAttribute("Name");
          switch (reader.Name)
          {
            case "Module":
              Module module = ((Cpu) parentObj).Modules[attribute];
              if (module != null)
              {
                module.FromXmlTextReader(ref reader, flags, (Base) module);
                break;
              }
              Module baseObj1 = new Module((Cpu) parentObj, attribute);
              baseObj1.FromXmlTextReader(ref reader, flags, (Base) baseObj1);
              break;
            case "Task":
              Task baseObj2 = new Task((Cpu) parentObj, attribute);
              baseObj2.FromXmlTextReader(ref reader, flags, (Base) baseObj2);
              break;
            case "TaskClass":
              APIFC_TkInfoRes taskClassInfo = new APIFC_TkInfoRes();
              TaskClass.FromXmlTextReader(ref reader, flags, ref taskClassInfo);
              TaskClass taskClass = new TaskClass(taskClassInfo);
              break;
            case "Variable":
              if (depth < reader.Depth)
                @base = (Base) variable;
              else if (depth > reader.Depth)
              {
                for (int index = depth; index > reader.Depth; --index)
                  @base = @base.propParent;
              }
              depth = reader.Depth;
              if (!flag)
              {
                switch (@base)
                {
                  case Cpu _:
                    variable = new Variable((Cpu) @base, attribute);
                    break;
                  case Service _:
                    variable = new Variable((Service) @base, attribute);
                    break;
                  case Variable _:
                    variable = new Variable((Variable) @base, attribute);
                    break;
                  default:
                    variable = new Variable((Task) @base, attribute);
                    break;
                }
                variable.FromXmlTextReader(ref reader, flags, (Base) variable);
                break;
              }
              Variable baseObj3 = new Variable(variable, attribute);
              baseObj3.FromXmlTextReader(ref reader, flags, (Base) baseObj3);
              break;
            case "Logger":
              Logger baseObj4 = new Logger((Cpu) parentObj, attribute);
              baseObj4.FromXmlTextReader(ref reader, flags, (Base) baseObj4);
              break;
            case "IODataPoint":
              IODataPoint baseObj5 = new IODataPoint((Cpu) parentObj, attribute);
              baseObj5.FromXmlTextReader(ref reader, flags, (Base) baseObj5);
              break;
            case "Memory":
              Memory memory = new Memory((Cpu) parentObj, attribute);
              memory.FromXmlTextReader(ref reader, flags, memory);
              break;
            case "Library":
              Library baseObj6 = new Library((Cpu) parentObj, attribute);
              baseObj6.FromXmlTextReader(ref reader, flags, (Base) baseObj6);
              break;
            case "Members":
              if (reader.NodeType == XmlNodeType.Element)
                flag = true;
              reader.Read();
              break;
            default:
              return 0;
          }
          if (reader.Name == "Members" && reader.NodeType == XmlNodeType.EndElement)
          {
            flag = false;
            reader.Read();
          }
          while (true)
          {
            if (reader.Name == "Variable" && reader.NodeType == XmlNodeType.EndElement)
              reader.Read();
            else
              goto label_37;
          }
        }
      }
      reader.Read();
      return 0;
    }

    internal virtual int ToXMLTextWriter(ref XmlTextWriter writer, ConfigurationFlags flags)
    {
      int xmlTextWriter = 0;
      if (0 < this.Count && this.Values != null)
      {
        writer.WriteStartElement(this.GetType().Name);
        foreach (object obj in (IEnumerable) this.Values)
        {
          int num = !(obj is TaskClass) ? ((Base) obj).ToXMLTextWriter(ref writer, flags) : ((TaskClass) obj).ToXMLTextWriter(ref writer, flags);
          if (num != 0)
            xmlTextWriter = num;
        }
        writer.WriteEndElement();
      }
      return xmlTextWriter;
    }

    public event PviEventHandler Connected;

    internal void Fire_Connected(object sender, PviEventArgs e)
    {
      if (this.Connected == null)
        return;
      this.Connected(sender, e);
    }

    public event PviEventHandler Disconnected;

    internal void Fire_Disconnected(object sender, PviEventArgs e)
    {
      if (this.Disconnected == null)
        return;
      this.Disconnected(sender, e);
    }

    internal void Fire_CollectionDisconnected(CollectionEventArgs e) => this.OnCollectionDisconnected(e);

    public event PviEventHandler Error;

    internal void Fire_Error(object sender, PviEventArgs e)
    {
      if (this.Error == null)
        return;
      this.Error(sender, e);
    }

    public event CollectionEventHandler CollectionConnected;

    public event CollectionEventHandler CollectionDisconnected;

    public event CollectionEventHandler CollectionError;

    public event PviEventHandler Uploaded;

    internal override void OnPviCreated(int errorCode, uint linkID)
    {
    }

    internal override void OnPviLinked(int errorCode, uint linkID, int option)
    {
    }

    internal override void OnPviEvent(
      int errorCode,
      EventTypes eventType,
      PVIDataStates dataState,
      IntPtr pData,
      uint dataLen,
      int option)
    {
    }

    internal override void OnPviRead(
      int errorCode,
      PVIReadAccessTypes accessType,
      PVIDataStates dataState,
      IntPtr pData,
      uint dataLen,
      int option)
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
    }

    internal override void OnPviUnLinked(int errorCode, int option)
    {
    }

    internal override void OnPviDeleted(int errorCode)
    {
    }

    internal override void OnPviChangedLink(int errorCode)
    {
    }
  }
}
