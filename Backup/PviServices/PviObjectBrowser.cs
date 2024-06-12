// Decompiled with JetBrains decompiler
// Type: BR.AN.PviServices.PviObjectBrowser
// Assembly: BR.AN.PVIServices, Version=1.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DA853F0A-C070-4E30-B7A7-51D955FB85F6
// Assembly location: C:\Users\ss_six\Desktop\FXE\FXE software integration package\COMET_FXE-PVIServices_Example\Pluto-PVI-trial_01\packages\IToolS.BandR.3.5.58\lib\net20\BR.AN.PVIServices.dll

using System;
using System.Collections;

namespace BR.AN.PviServices
{
  internal class PviObjectBrowser : Base
  {
    private bool propUseIDName;
    private Hashtable propLineNames;
    private Hashtable propDeviceNames;
    private Hashtable propCpuNames;
    private Hashtable propTaskNames;
    private Action pviActionType;
    private PviObjectBrowser propBrowserParent;
    private string pathName;

    public Action PviActionType
    {
      get => this.pviActionType;
      set => this.pviActionType = value;
    }

    public PviObjectBrowser BrowserParent
    {
      get => this.propBrowserParent;
      set => this.propBrowserParent = value;
    }

    public override string FullName
    {
      get
      {
        if (this.Name != null && 0 < this.Name.Length)
          return this.propBrowserParent.FullName + "." + this.Name;
        return this.propBrowserParent != null ? this.propBrowserParent.FullName : "";
      }
    }

    public override string PviPathName => this.Name != null && 0 < this.Name.Length ? this.propBrowserParent.PviPathName + "/\"" + this.propName + "\" OT=Task" : this.propBrowserParent.PviPathName;

    public string PathName
    {
      get
      {
        if (this.pathName != null)
          return this.pathName;
        return !this.propUseIDName && this.propBrowserParent != null ? this.propBrowserParent.PathName + "/" + this.propName : this.propName;
      }
      set => this.pathName = value;
    }

    public PviObjectBrowser(string strName, bool useIDName, Base baseObj, PviObjectBrowser parent)
      : base(baseObj)
    {
      this.propUseIDName = useIDName;
      this.propBrowserParent = parent;
      this.propName = strName;
      if (useIDName && 0 < strName.IndexOf(" OT="))
        this.propName = strName.Substring(0, strName.IndexOf(" OT="));
      this.Initialize(parent);
    }

    public PviObjectBrowser(string strName, bool useIDName, PviObjectBrowser parent)
      : base((Base) parent.Service)
    {
      this.propUseIDName = useIDName;
      this.propBrowserParent = parent;
      this.propName = strName;
      if (useIDName && 0 < strName.IndexOf(" OT="))
        this.propName = strName.Substring(0, strName.IndexOf(" OT="));
      this.Initialize(parent);
    }

    internal void Deinit()
    {
      if (Action.ServiceReadLinesList == this.pviActionType)
      {
        this.Service.Cpus.CleanUp(true);
        this.Service.LoggerCollections.Clear();
        this.Service.LoggerEntries.CleanUp(true);
        this.Service.LogicalObjects.Clear();
        this.Service.DisconnectEx(this.Service.hPvi);
      }
      this.propLineNames.Clear();
      this.propDeviceNames.Clear();
      this.propCpuNames.Clear();
      this.propTaskNames.Clear();
    }

    internal int Link()
    {
      int num = 2;
      string str1 = "";
      string str2 = "";
      if (Action.ServiceReadLinesList == this.pviActionType)
      {
        string str3 = "LM=" + this.Service.MessageLimitation.ToString();
        if (1 != this.Service.PVIAutoStart)
          str2 = " AS=" + this.Service.PVIAutoStart.ToString();
        if (0 < this.Service.ProcessTimeout)
          str1 = " PT=" + this.Service.ProcessTimeout.ToString();
        string pInitParam;
        if (this.Service.Server != null && this.Service.Server.Length > 0)
          pInitParam = string.Format("{0} IP={1} PN={2}{3}{4}", (object) str3, (object) this.Service.Server, (object) this.Service.Port, (object) str1, (object) str2);
        else
          pInitParam = string.Format("{0}{1}{2}", (object) str3, (object) str1, (object) str2);
        num = this.Service.Initialize(ref this.Service.hPvi, this.Service.Timeout, this.Service.RetryTime, pInitParam, new IntPtr(0));
        ++this.Service.propPendingObjectBrowserEvents;
        if (num == 0)
          num = this.XLinkRequest(this.Service.hPvi, "Pvi", 99U, "", 98U);
      }
      return num;
    }

    public int CreateReadObjectRequest(int lnkId)
    {
      ++this.Service.propPendingObjectBrowserEvents;
      return this.propService.EventMessageType != EventMessageType.CallBack ? PInvokePvicom.PviComMsgReadRequest(this.propService.hPvi, (uint) lnkId, AccessTypes.List, this.Service.WindowHandle, (uint) this.pviActionType, this.propInternID) : PInvokePvicom.PviComReadRequest(this.propService.hPvi, (uint) lnkId, AccessTypes.List, this.Service.cbRead, 4294967294U, this.InternId);
    }

    public int CreateLinkObjectRequest()
    {
      Action respMsgNo = Action.ServiceLinkLine;
      switch (this.pviActionType)
      {
        case Action.ServiceReadDevicesList:
          respMsgNo = Action.ServiceLinkLine;
          break;
        case Action.ServiceReadStationsList:
          respMsgNo = Action.ServiceLinkDevice;
          break;
        case Action.ServiceReadCpuList:
          respMsgNo = Action.ServiceLinkStation;
          break;
        case Action.CpuReadTasksList:
          respMsgNo = Action.ServiceLinkCpu;
          break;
        case Action.CpuReadVariableList:
          respMsgNo = Action.CpuLinkVariable;
          break;
        case Action.TaskReadVariablesList:
          respMsgNo = Action.TaskLinkVariable;
          break;
      }
      ++this.Service.propPendingObjectBrowserEvents;
      return this.propService.EventMessageType != EventMessageType.CallBack ? PInvokePvicom.PviComMsgLinkRequest(this.propService.hPvi, new StringMarshal().GetBytes(this.PathName), this.Service.WindowHandle, 0U, 0U, (byte[]) null, this.Service.WindowHandle, (uint) respMsgNo, this.propInternID) : PInvokePvicom.PviComLinkRequest(this.propService.hPvi, this.PathName, this.Service.cbEvent, 0U, 0U, "", this.Service.cbLink, 4294967294U, this.InternId);
    }

    private void Initialize(PviObjectBrowser parent)
    {
      if (parent == null)
        this.pviActionType = Action.ServiceReadLinesList;
      else if (parent.pviActionType == Action.ServiceReadLinesList)
        this.pviActionType = Action.ServiceReadDevicesList;
      else if (parent.pviActionType == Action.ServiceReadDevicesList)
        this.pviActionType = Action.ServiceReadStationsList;
      else if (parent.pviActionType == Action.ServiceReadStationsList)
        this.pviActionType = Action.ServiceReadCpuList;
      else if (parent.pviActionType == Action.ServiceReadCpuList)
        this.pviActionType = Action.CpuReadTasksList;
      else if (parent.pviActionType == Action.CpuReadTasksList)
        this.pviActionType = Action.TaskReadVariablesList;
      else if (parent.pviActionType == Action.ServiceReadStationsList)
        this.pviActionType = Action.ServiceReadCpuList;
      this.propLineNames = new Hashtable();
      this.propDeviceNames = new Hashtable();
      this.propCpuNames = new Hashtable();
      this.propTaskNames = new Hashtable();
    }

    internal override void OnPviCreated(int errorCode, uint linkID) => base.OnPviCreated(errorCode, linkID);

    internal override void OnPviLinked(int errorCode, uint linkID, int option)
    {
      Action action = this.pviActionType;
      int errorCode1 = errorCode;
      if (errorCode1 == 0)
      {
        switch (this.pviActionType)
        {
          case Action.ServiceReadLinesList:
            errorCode1 = this.CreateReadObjectRequest((int) linkID);
            action = Action.ServiceReadDevicesList;
            break;
          case Action.ServiceReadDevicesList:
            errorCode1 = this.CreateReadObjectRequest((int) linkID);
            action = Action.ServiceReadStationsList;
            break;
          case Action.ServiceReadStationsList:
            errorCode1 = this.CreateReadObjectRequest((int) linkID);
            action = Action.ServiceReadCpuList;
            break;
          case Action.ServiceReadCpuList:
            errorCode1 = this.CreateReadObjectRequest((int) linkID);
            action = Action.CpuReadTasksList;
            break;
          case Action.CpuReadTasksList:
            errorCode1 = this.CreateReadObjectRequest((int) linkID);
            action = Action.TaskReadVariablesList;
            break;
          case Action.CpuReadVariableList:
            errorCode1 = this.CreateReadObjectRequest((int) linkID);
            action = Action.CpuLinkVariable;
            break;
          case Action.TaskReadVariablesList:
            errorCode1 = this.CreateReadObjectRequest((int) linkID);
            action = Action.TaskLinkVariable;
            break;
        }
      }
      this.propService.OnPVIObjectsAttached(new PviEventArgs(this.propName, this.propService.propAddress, errorCode1, this.propService.Language, action));
    }

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
          break;
        case EventTypes.Data:
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
      int errorCode1 = 0;
      bool bCpusIn = false;
      if (accessType == PVIReadAccessTypes.ChildObjects)
      {
        if (errorCode != 0 || 0U >= dataLen)
          return;
        switch (this.pviActionType)
        {
          case Action.ServiceReadLinesList:
            errorCode1 = this.ReadAttachedLines(pData, dataLen);
            break;
          case Action.ServiceReadDevicesList:
            errorCode1 = this.ReadAttachedDevices(pData, dataLen);
            break;
          case Action.ServiceReadStationsList:
            errorCode1 = this.ReadAttachedStations(pData, dataLen, ref bCpusIn);
            if (errorCode1 == 0 && bCpusIn)
            {
              this.pviActionType = Action.ServiceReadCpuList;
              errorCode1 = this.ReadAttachedCpus(pData, dataLen);
              break;
            }
            break;
          case Action.ServiceReadCpuList:
            errorCode1 = this.ReadAttachedCpus(pData, dataLen);
            break;
          case Action.CpuReadTasksList:
            errorCode1 = this.ReadAttachedTasksOrGPVars(pData, dataLen);
            break;
          case Action.CpuReadVariableList:
            errorCode1 = this.ReadAttachedVariables(pData, dataLen);
            break;
          case Action.TaskReadVariablesList:
            errorCode1 = this.ReadAttachedVariables(pData, dataLen);
            break;
        }
        this.propService.OnPVIObjectsAttached(new PviEventArgs(this.propName, this.propService.propAddress, errorCode1, this.propService.Language, this.pviActionType));
      }
      else
        base.OnPviRead(errorCode, accessType, dataState, pData, dataLen, option);
    }

    private int ReadAttachedLines(IntPtr pData, uint dataLen)
    {
      int errorCode = 0;
      bool flag = false;
      string[] strArray = PviMarshal.ToAnsiString(pData, dataLen).Split('\t');
      for (int index = 0; index < strArray.Length; ++index)
      {
        flag = false;
        if (-1 != strArray[index].ToLower().IndexOf("ot=line"))
        {
          string str = strArray[index].Substring(0, strArray[index].ToLower().IndexOf("ot=line") - 1);
          bool useIDName = this.propLineNames.ContainsKey((object) str);
          if (!useIDName)
            this.propLineNames.Add((object) str, (object) str);
          errorCode = new PviObjectBrowser(str, useIDName, this).CreateLinkObjectRequest();
          if (errorCode != 0)
            this.propService.OnPVIObjectsAttached(new PviEventArgs(this.propName, this.propService.propAddress, errorCode, this.propService.Language, Action.ServiceLinkLine));
        }
        else if (-1 != strArray[index].ToLower().IndexOf("ot=pvar"))
        {
          string str = strArray[index].Substring(0, strArray[index].ToLower().IndexOf("ot=pvar") - 1);
          if (this.propLineNames.ContainsKey((object) str))
            str = "@Pvi/" + str;
          Variable variable = new Variable(this.propService, str);
          variable.ConnectionType = ConnectionType.Link;
          PviObjectBrowser pviObjectBrowser = new PviObjectBrowser(strArray[index], this.propUseIDName, this);
          variable.LinkName = pviObjectBrowser.PathName;
        }
      }
      return errorCode;
    }

    private int ReadAttachedDevices(IntPtr pData, uint dataLen)
    {
      int errorCode = 0;
      string ansiString = PviMarshal.ToAnsiString(pData, dataLen);
      if (ansiString == "")
        return -1;
      string str = ansiString;
      char[] chArray = new char[1]{ '\t' };
      foreach (string strName in str.Split(chArray))
      {
        errorCode = new PviObjectBrowser(strName, this.propUseIDName, this).CreateLinkObjectRequest();
        if (errorCode != 0)
          this.propService.OnPVIObjectsAttached(new PviEventArgs(this.propName, this.propService.propAddress, errorCode, this.propService.Language, Action.ServiceLinkDevice));
      }
      return errorCode;
    }

    private int ReadAttachedStations(IntPtr pData, uint dataLen, ref bool bCpusIn)
    {
      int errorCode = 0;
      string ansiString = PviMarshal.ToAnsiString(pData, dataLen);
      if (ansiString == "")
        return -1;
      string[] strArray = ansiString.Split('\t');
      for (int index = 0; index < strArray.Length; ++index)
      {
        if (-1 != strArray[index].ToLower().IndexOf("ot=station"))
        {
          errorCode = new PviObjectBrowser(strArray[index], this.propUseIDName, this).CreateLinkObjectRequest();
          if (errorCode != 0)
            this.propService.OnPVIObjectsAttached(new PviEventArgs(this.propName, this.propService.propAddress, errorCode, this.propService.Language, Action.ServiceLinkStation));
        }
        else if (-1 != strArray[index].ToLower().IndexOf("ot=cpu"))
          bCpusIn = true;
      }
      return errorCode;
    }

    private string GetBrowserObjectName(
      PviObjectBrowser browseParent,
      LogicalObjectsUsage nameOption,
      string pviText,
      string otString)
    {
      string browserObjectName = pviText.Substring(0, pviText.ToLower().IndexOf(otString) - 1);
      if (nameOption == LogicalObjectsUsage.ObjectName)
      {
        if (browseParent == null)
        {
          int length = pviText.IndexOf('.');
          if (-1 == length)
          {
            browserObjectName = pviText.Substring(0, pviText.ToLower().IndexOf(otString) - 1);
          }
          else
          {
            browserObjectName = pviText.Substring(length + 1, pviText.ToLower().IndexOf(otString) - 2 - length);
            this.Service.propAddress = pviText.Substring(0, length);
          }
        }
        else
        {
          string str = browseParent.Name.Substring(0, browseParent.Name.ToLower().IndexOf("ot=") - 1);
          int num = pviText.IndexOf(str);
          browserObjectName = -1 != num ? pviText.Substring(num + str.Length + 1, pviText.ToLower().IndexOf(otString) - 1 - num - str.Length) : pviText.Substring(0, pviText.ToLower().IndexOf(otString) - 1);
        }
      }
      return browserObjectName;
    }

    private int ReadAttachedCpus(IntPtr pData, uint dataLen)
    {
      int errorCode = 0;
      string ansiString = PviMarshal.ToAnsiString(pData, dataLen);
      if ("" != ansiString)
      {
        string[] strArray = ansiString.Split('\t');
        for (int index = 0; index < strArray.Length; ++index)
        {
          if (-1 != strArray[index].ToLower().IndexOf("ot=cpu"))
          {
            if (!this.Service.Cpus.ContainsKey((object) strArray[index]))
            {
              Cpu baseObj = new Cpu(this.Service, this.GetBrowserObjectName((PviObjectBrowser) null, this.Service.LogicalObjectsUsage, strArray[index], "ot=cpu"));
              baseObj.ConnectionType = ConnectionType.Link;
              PviObjectBrowser pviObjectBrowser = new PviObjectBrowser(strArray[index], this.propUseIDName, (Base) baseObj, this);
              baseObj.LinkName = pviObjectBrowser.PathName;
              errorCode = pviObjectBrowser.CreateLinkObjectRequest();
              if (errorCode != 0)
                this.propService.OnPVIObjectsAttached(new PviEventArgs(this.propName, this.propService.propAddress, errorCode, this.propService.Language, Action.ServiceLinkCpu));
            }
            else
              this.propService.OnPVIObjectsAttached(new PviEventArgs(this.propName, this.propService.propAddress, 0, this.propService.Language, Action.ServiceLinkCpu));
          }
        }
      }
      return errorCode;
    }

    private int ReadAttachedTasksOrGPVars(IntPtr pData, uint dataLen)
    {
      int errorCode = 0;
      string ansiString = PviMarshal.ToAnsiString(pData, dataLen);
      if ("" != ansiString)
      {
        string[] strArray = ansiString.Split('\t');
        for (int index = 0; index < strArray.Length; ++index)
        {
          if (-1 != strArray[index].ToLower().IndexOf("ot=task"))
          {
            Task baseObj = new Task((Cpu) this.Parent, this.GetBrowserObjectName(this, this.Service.LogicalObjectsUsage, strArray[index], "ot=task"));
            baseObj.ConnectionType = ConnectionType.Link;
            PviObjectBrowser pviObjectBrowser = new PviObjectBrowser(strArray[index], this.propUseIDName, (Base) baseObj, this);
            baseObj.LinkName = pviObjectBrowser.PathName;
            errorCode = pviObjectBrowser.CreateLinkObjectRequest();
            if (errorCode != 0)
              this.propService.OnPVIObjectsAttached(new PviEventArgs(this.propName, this.propService.propAddress, errorCode, this.propService.Language, Action.CpuLinkTask));
          }
          else if (-1 != strArray[index].ToLower().IndexOf("ot=module"))
          {
            Module baseObj = new Module((Cpu) this.Parent, this.GetBrowserObjectName(this, this.Service.LogicalObjectsUsage, strArray[index], "ot=module"));
            baseObj.ConnectionType = ConnectionType.Link;
            PviObjectBrowser pviObjectBrowser = new PviObjectBrowser(strArray[index], this.propUseIDName, (Base) baseObj, this);
            baseObj.LinkName = pviObjectBrowser.PathName;
          }
          else if (-1 != strArray[index].ToLower().IndexOf("ot=pvar"))
          {
            Variable baseObj = new Variable((Cpu) this.Parent, this.GetBrowserObjectName(this, this.Service.LogicalObjectsUsage, strArray[index], "ot=pvar"));
            baseObj.ConnectionType = ConnectionType.Link;
            PviObjectBrowser pviObjectBrowser = new PviObjectBrowser(strArray[index], this.propUseIDName, (Base) baseObj, this);
            baseObj.LinkName = pviObjectBrowser.PathName;
          }
        }
      }
      return errorCode;
    }

    private int ReadAttachedVariables(IntPtr pData, uint dataLen)
    {
      int num = 0;
      string ansiString = PviMarshal.ToAnsiString(pData, dataLen);
      if ("" != ansiString)
      {
        string[] strArray = ansiString.Split('\t');
        for (int index = 0; index < strArray.Length; ++index)
        {
          if (-1 != strArray[index].ToLower().IndexOf("ot=pvar"))
          {
            Variable baseObj = new Variable((Task) this.Parent, strArray[index].Substring(0, strArray[index].ToLower().IndexOf("ot=pvar") - 1));
            baseObj.ConnectionType = ConnectionType.Link;
            PviObjectBrowser pviObjectBrowser = new PviObjectBrowser(strArray[index], this.propUseIDName, (Base) baseObj, this);
            baseObj.LinkName = pviObjectBrowser.PathName;
          }
        }
      }
      return num;
    }
  }
}
