// Decompiled with JetBrains decompiler
// Type: BR.AN.PviServices.SNMPBase
// Assembly: BR.AN.PVIServices, Version=1.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DA853F0A-C070-4E30-B7A7-51D955FB85F6
// Assembly location: C:\Users\ss_six\Desktop\FXE\FXE software integration package\COMET_FXE-PVIServices_Example\Pluto-PVI-trial_01\packages\IToolS.BandR.3.5.58\lib\net20\BR.AN.PVIServices.dll

using System;
using System.Runtime.InteropServices;
using System.Xml;

namespace BR.AN.PviServices
{
  public class SNMPBase : PviCBEvents
  {
    internal uint propLinkID;
    protected Service propService;
    protected SNMPBase propParent;
    internal bool propIsConnected;
    internal uint propServiceArrayIndex;
    internal Actions propRequestQueue;
    private string propName;
    private object propData;

    internal SNMPBase(string name, Service serviceOBJ)
    {
      this.propData = (object) null;
      this.propName = name;
      this.propLinkID = 0U;
      this.propRequestQueue = Actions.NONE;
      this.propIsConnected = false;
      this.propParent = (SNMPBase) null;
      this.propService = serviceOBJ;
      this.AddToCBReceivers();
    }

    internal SNMPBase(string name, SNMPBase parentOBJ)
    {
      this.propData = (object) null;
      this.propName = name;
      this.propLinkID = 0U;
      this.propRequestQueue = Actions.NONE;
      this.propIsConnected = false;
      this.propParent = parentOBJ;
      this.propService = this.propParent.Service;
      this.AddToCBReceivers();
    }

    internal void InitConnect(Service serviceOBJ)
    {
      this.propService = serviceOBJ;
      if (this.Service == null || !this.Service.ContainsIDKey(this.propServiceArrayIndex))
        return;
      this.AddToCBReceivers();
    }

    internal void InitConnect(SNMPBase parentOBJ)
    {
      this.propParent = parentOBJ;
      this.propService = this.propParent.Service;
      if (this.Service == null || !this.Service.ContainsIDKey(this.propServiceArrayIndex))
        return;
      this.AddToCBReceivers();
    }

    internal virtual int ToXMLTextWriter(ref XmlTextWriter writer, ConfigurationFlags flags) => 0;

    public virtual int FromXmlTextReader(ref XmlTextReader reader, ConfigurationFlags flags) => 0;

    internal override void OnPviCreated(int errorCode, uint linkID)
    {
      this.propLinkID = linkID;
      if (errorCode != 0 && 12002 != errorCode)
        return;
      this.propIsConnected = true;
    }

    public SNMPBase Parent => this.propParent;

    public Service Service => this.propService;

    public string Name => this.propName;

    public string FullName
    {
      get
      {
        if (this.propParent != null)
          return this.propParent.FullName + "." + this.propName;
        return this.Service == null ? this.propName : this.Service.Name + "." + this.propName;
      }
    }

    public object UserData
    {
      get => this.propData;
      set => this.propData = value;
    }

    public virtual void Cleanup()
    {
      this.RemoveFromCBReceivers();
      this.CancelRequest();
      this.propIsConnected = false;
      this.propLinkID = 0U;
      this.propRequestQueue = Actions.NONE;
      this.propIsConnected = false;
      this.propParent = (SNMPBase) null;
      this.propService = (Service) null;
    }

    internal int CancelRequest()
    {
      int num = 0;
      if (this.propLinkID != 0U && this.Service != null)
      {
        int[] source = new int[1]{ 0 };
        IntPtr hMemory = PviMarshal.AllocHGlobal(4);
        Marshal.Copy(source, 0, hMemory, 1);
        num = this.Service.EventMessageType != EventMessageType.CallBack ? PInvokePvicom.PviComMsgWriteRequest(this.Service.hPvi, this.propLinkID, AccessTypes.Cancel, hMemory, 4, IntPtr.Zero, 0U, 0U) : PInvokePvicom.PviComWriteRequest(this.Service.hPvi, this.propLinkID, AccessTypes.Cancel, hMemory, 4, (PviCallback) null, 0U, 0U);
        PviMarshal.FreeHGlobal(ref hMemory);
      }
      return num;
    }

    internal int DeleteRequest(string objName) => this.Service.EventMessageType != EventMessageType.CallBack ? PInvokePvicom.PviComUnlinkRequest(this.Service.hPvi, this.propLinkID, (PviCallback) null, 0U, 0U) : PInvokePvicom.PviComUnlinkRequest(this.Service.hPvi, this.propLinkID, (PviCallback) null, 0U, 0U);

    internal int DeleteRequest() => this.DeleteRequest(this.FullName);

    internal int ConnectPviObject(
      bool withEvents,
      string objName,
      string objDesc,
      string lnkDesc,
      ObjectType objType,
      int action,
      out uint linkID)
    {
      return withEvents ? PInvokePvicom.PviComCreate(this.Service.hPvi, out linkID, objName, objType, objDesc, this.Service.cbSNMP, (uint) action, this.propServiceArrayIndex, lnkDesc) : PInvokePvicom.PviComCreate(this.Service.hPvi, out linkID, objName, objType, objDesc, this.Service.cbSNMP, 0U, 0U, lnkDesc);
    }

    internal int UnlinkPviObject(uint linkID) => this.Service != null && 0U < linkID ? PInvokePvicom.PviComUnlink(this.Service.hPvi, linkID) : 0;

    internal virtual int Connect() => -1;

    public virtual int Disconnect() => 12063;

    public virtual int Disconnect(bool synchronous) => 12063;

    protected int GetSNMPVariables(int linkID, int action)
    {
      AccessTypes nAccess = AccessTypes.ListVariable;
      return this.Service.EventMessageType != EventMessageType.CallBack ? PInvokePvicom.PviComMsgReadRequest(this.propService.hPvi, (uint) linkID, nAccess, this.propService.WindowHandle, (uint) action, this.propServiceArrayIndex) : PInvokePvicom.PviComReadRequest(this.propService.hPvi, (uint) linkID, nAccess, this.Service.cbRead, 4294967294U, this.propServiceArrayIndex);
    }

    public event ErrorEventHandler SearchCompleted;

    protected virtual void OnSearchCompleted(ErrorEventArgs e)
    {
      if (this.SearchCompleted == null)
        return;
      this.SearchCompleted((object) this, e);
    }

    public event ErrorEventHandler Error;

    protected virtual void OnError(ErrorEventArgs e)
    {
      if (this.Error == null)
        return;
      this.Error((object) this, e);
    }

    private void RemoveFromCBReceivers()
    {
      if (this.Service == null)
        return;
      this.Service.RemoveID(this.propServiceArrayIndex);
    }

    private bool AddToCBReceivers() => this.Service != null && this.Service.AddID((object) this, ref this.propServiceArrayIndex);
  }
}
