// Decompiled with JetBrains decompiler
// Type: BR.AN.PviServices.IODataPointCollection
// Assembly: BR.AN.PVIServices, Version=1.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DA853F0A-C070-4E30-B7A7-51D955FB85F6
// Assembly location: C:\Users\ss_six\Desktop\FXE\FXE software integration package\COMET_FXE-PVIServices_Example\Pluto-PVI-trial_01\packages\IToolS.BandR.3.5.58\lib\net20\BR.AN.PVIServices.dll

using System;
using System.Collections;
using System.Xml;

namespace BR.AN.PviServices
{
  public class IODataPointCollection : BaseCollection
  {
    private bool cpuPVRequest;

    public IODataPointCollection(Base parent, string name)
      : base((object) parent, name)
    {
      this.cpuPVRequest = false;
      this.propParent = (object) parent;
    }

    public IODataPointCollection(object parent, string name)
      : base(parent, name)
    {
      this.cpuPVRequest = false;
      this.propParent = parent;
    }

    internal override void Dispose(bool disposing, bool removeFromCollection)
    {
      if (this.propDisposed)
        return;
      object propParent = this.propParent;
      object propUserData = this.propUserData;
      string propName = this.propName;
      this.CleanUp(disposing);
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
      this.propCounter = 0;
      if (this.Values != null)
      {
        foreach (IODataPoint ioDataPoint in (IEnumerable) this.Values)
          arrayList.Add((object) ioDataPoint);
        for (int index = 0; index < arrayList.Count; ++index)
        {
          IODataPoint ioDataPoint = (IODataPoint) arrayList[index];
          ioDataPoint.Disconnect(0);
          ioDataPoint.Dispose(disposing, true);
        }
      }
      this.Clear();
    }

    internal void Disconnect(bool noResponse)
    {
      if (!noResponse)
        return;
      foreach (Base @base in (IEnumerable) this.Values)
        @base.Disconnect(true);
      this.propConnectionState = ConnectionStates.Disconnected;
    }

    public void Upload() => this.Upload(Scope.UNDEFINED);

    internal void Upload(Scope variableScope)
    {
      int num = 0;
      this.cpuPVRequest = false;
      if (this.propParent is Cpu)
      {
        this.cpuPVRequest = true;
        num = this.ReadIOLinkNodes("", ((Base) this.propParent).Service.hPvi, ((Base) this.propParent).LinkId, this.InternId);
      }
      else
      {
        if (!(this.propParent is Variable))
          return;
        Variable propParent1 = (Variable) this.propParent;
        Cpu cpu = (Cpu) null;
        if (propParent1.propParent is Cpu)
          cpu = (Cpu) propParent1.propParent;
        else if (propParent1.propParent is Task)
        {
          Task propParent2 = (Task) propParent1.propParent;
          if (propParent2.propParent is Cpu)
            cpu = (Cpu) propParent2.propParent;
        }
        if (cpu != null)
        {
          string uploadFilter = this.GetUploadFilter(propParent1, variableScope == Scope.UNDEFINED ? propParent1.Scope : variableScope);
          IntPtr hglobal = PviMarshal.StringToHGlobal(uploadFilter);
          int errorCode = this.ReadIOLinkNodes(uploadFilter, this.Service.hPvi, cpu.LinkId, propParent1.InternId);
          PviMarshal.FreeHGlobal(ref hglobal);
          if (errorCode != 0)
            this.OnError(new PviEventArgs(propParent1.Name, propParent1.Address, errorCode, this.Service.Language, Action.LinkNodeList, this.Service));
        }
      }
    }

    private int ReadIOLinkNodes(string filter, uint hPvi, uint lnkID, uint internID)
    {
      IntPtr hglobal = PviMarshal.StringToHGlobal(filter);
      int num = this.ReadArgumentRequest(hPvi, lnkID, AccessTypes.LinkNodeList, hglobal, filter.Length, 810U, internID);
      PviMarshal.FreeHGlobal(ref hglobal);
      return num;
    }

    private string GetUploadFilter(Variable varRequ, Scope ioVarScope)
    {
      string str = "";
      if (varRequ.propParent is Cpu)
      {
        this.cpuPVRequest = true;
        str = "/QU=Pv /QP=" + varRequ.propAddress;
      }
      else if (varRequ.propParent is Task)
      {
        if (Scope.Global == ioVarScope)
        {
          this.cpuPVRequest = true;
          str = "/QU=Pv /QP=" + varRequ.propAddress;
        }
        else
          str = string.Format("/QU=Pv /QP={0}:{1}", (object) varRequ.propParent.Name, (object) varRequ.propAddress);
      }
      return str.Replace(",]", "]");
    }

    internal override void OnPviRead(
      int errorCode,
      PVIReadAccessTypes accessType,
      PVIDataStates dataState,
      IntPtr pData,
      uint dataLen,
      int option)
    {
      if (PVIReadAccessTypes.LinkNodeList == accessType)
      {
        if (errorCode == 0 && dataLen > 0U)
        {
          string stringAnsi = PviMarshal.PtrToStringAnsi(pData, dataLen);
          bool ioData = stringAnsi != null && 1 < stringAnsi.Length;
          if (ioData)
          {
            Cpu cpu = (Cpu) null;
            if (this.propParent is Cpu)
              cpu = (Cpu) this.propParent;
            else if (this.propParent is Variable)
            {
              Variable propParent = (Variable) this.propParent;
              cpu = !(propParent.propParent is Cpu) ? (Cpu) propParent.propParent.propParent : (Cpu) propParent.propParent;
            }
            string[] strArray = stringAnsi.Split("\t".ToCharArray());
            for (int index = 0; index < strArray.Length; ++index)
            {
              string name = strArray.GetValue(index).ToString();
              int length = name.IndexOf("\0");
              if (-1 != length)
                name = name.Substring(0, length);
              IODataPoint ioDataPoint;
              if ((ioDataPoint = cpu.IODataPoints[name]) == null)
                ioDataPoint = new IODataPoint(cpu, name);
              if (this.propParent is Variable && !((Variable) this.propParent).IODataPoints.ContainsKey((object) ioDataPoint.Name))
                ((Variable) this.propParent).IODataPoints.Add(ioDataPoint);
            }
          }
          this.FireUploadedEvent(ioData, errorCode);
        }
        else if (4599 != errorCode)
        {
          this.OnUploaded(new PviEventArgs(this.propName, "", errorCode, this.Service.Language, Action.LinkNodesUpload, this.Service));
          this.OnError((PviEventArgs) new CollectionEventArgs(this.propName, "", errorCode, this.Service.Language, Action.LinkNodesUpload, (BaseCollection) null));
        }
        else
          this.OnError((PviEventArgs) new CollectionEventArgs(this.propName, "", errorCode, this.Service.Language, Action.LinkNodesUpload, (BaseCollection) null));
      }
      else
        base.OnPviRead(errorCode, accessType, dataState, pData, dataLen, option);
    }

    private void FireUploadedEvent(bool ioData, int errorCode)
    {
      if (this.cpuPVRequest)
        this.OnUploaded(new PviEventArgs(this.propName, "", errorCode, this.Service.Language, Action.LinkNodesUpload, this.Service));
      else
        this.Upload(Scope.Global);
    }

    protected virtual void OnError(PviEventArgs e) => this.Fire_Error((object) this, e);

    public virtual void Add(IODataPoint ioDataPoint)
    {
      if (this.ContainsKey((object) ioDataPoint.Name))
        return;
      this.Add((object) ioDataPoint.Name, (object) ioDataPoint);
    }

    public IODataPoint this[string name] => this.propCollectionType == CollectionType.HashTable ? (IODataPoint) this[(object) name] : (IODataPoint) null;

    public override Service Service
    {
      get
      {
        if (this.propParent is Cpu)
          return ((Base) this.propParent).Service;
        if (this.propParent is Task)
          return ((Base) this.propParent).Service;
        if (this.propParent is Variable)
          return ((Base) this.propParent).Service;
        return this.propParent is Service ? (Service) this.propParent : (Service) null;
      }
    }

    internal override int ToXMLTextWriter(ref XmlTextWriter writer, ConfigurationFlags flags)
    {
      int xmlTextWriter1 = 0;
      if (this.Count > 0)
      {
        writer.WriteStartElement(this.GetType().Name);
        foreach (object obj in (IEnumerable) this.Values)
        {
          writer.WriteStartElement("IODataPoint");
          int xmlTextWriter2 = ((Base) obj).ToXMLTextWriter(ref writer, flags);
          if (xmlTextWriter2 != 0)
            xmlTextWriter1 = xmlTextWriter2;
          writer.WriteEndElement();
        }
        writer.WriteEndElement();
      }
      return xmlTextWriter1;
    }
  }
}
