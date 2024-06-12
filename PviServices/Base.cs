// Decompiled with JetBrains decompiler
// Type: BR.AN.PviServices.Base
// Assembly: BR.AN.PVIServices, Version=1.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DA853F0A-C070-4E30-B7A7-51D955FB85F6
// Assembly location: C:\Users\ss_six\Desktop\FXE\FXE software integration package\COMET_FXE-PVIServices_Example\Pluto-PVI-trial_01\packages\IToolS.BandR.3.5.58\lib\net20\BR.AN.PVIServices.dll

using System;
using System.Runtime.InteropServices;
using System.Xml;

namespace BR.AN.PviServices
{
  public abstract class Base : PviCBEvents, IDisposable
  {
    internal const int WM_USER = 10000;
    internal const uint SET_PVICALLBACK = 4294967295;
    internal const uint SET_PVICALLBACK_DATA = 4294967294;
    internal const uint SET_PVICALLBACK_ASYNC = 4294967293;
    internal const uint SET_PVIFUNCTION = 4294967292;
    internal bool propReadingFormat;
    protected bool propNoDisconnectedEvent;
    protected int propReturnValue;
    internal bool propDisposed;
    internal bool isObjectConnected;
    internal Service propService;
    protected bool propHasLinkObject;
    protected ConnectionType propConnectionType;
    internal bool reCreateActive;
    internal string propErrorText = string.Empty;
    internal string propCurLanguage = string.Empty;
    internal string propName;
    internal string propAddress;
    internal string propLinkName;
    internal SNMPBase propSNMPParent;
    internal Base propParent;
    internal string propLogicalName;
    internal object propUserData;
    internal int propErrorCode;
    internal uint propLinkId;
    internal string propObjectParam;
    internal string propLinkParam;
    internal bool propReCreateActive;
    internal ConnectionStates propConnectionState;
    internal Actions propResponses;
    internal Actions propRequests;
    internal bool propAddToLogicalObjects;
    internal MethodType propMethodType;
    internal uint propInternID;

    public int ReturnValue => this.propReturnValue;

    internal int CancelRequest() => this.CancelRequest(true);

    internal int CancelRequest(bool silent)
    {
      int[] source = new int[1]{ 0 };
      IntPtr hMemory = PviMarshal.AllocHGlobal(4);
      Marshal.Copy(source, 0, hMemory, 1);
      int num = silent ? this.WriteRequest(this.Service.hPvi, this.propLinkId, AccessTypes.Cancel, hMemory, 4, 0U) : this.WriteRequest(this.Service.hPvi, this.propLinkId, AccessTypes.Cancel, hMemory, 4, 1100U);
      PviMarshal.FreeHGlobal(ref hMemory);
      return num;
    }

    internal void UpdateServiceCreateState()
    {
      if (this.propConnectionState == ConnectionStates.Unininitialized || ConnectionStates.Disconnecting == this.propConnectionState || ConnectionStates.Disconnected == this.propConnectionState)
        return;
      this.propConnectionState = ConnectionStates.Unininitialized;
      this.reCreateActive = true;
    }

    private void Initialize(string name, bool addToVColls)
    {
      this.propReadingFormat = false;
      this.propReCreateActive = false;
      this.reCreateActive = false;
      this.propNoDisconnectedEvent = false;
      this.propSNMPParent = (SNMPBase) null;
      this.propService = (Service) null;
      this.propDisposed = false;
      this.isObjectConnected = false;
      this.propConnectionType = ConnectionType.CreateAndLink;
      this.propAddress = "";
      this.propName = name;
      this.propObjectParam = "";
      this.propConnectionState = ConnectionStates.Unininitialized;
      this.propErrorCode = 0;
      this.propLinkName = "";
      this.propAddToLogicalObjects = addToVColls;
    }

    public Base(Base parentObj)
    {
      this.Initialize("", true);
      this.propParent = parentObj;
      switch (parentObj)
      {
        case Cpu _:
          this.propService = parentObj.Service;
          break;
        case Task _:
          this.propService = parentObj.Service;
          break;
        case Variable _:
          this.propService = parentObj.Service;
          break;
        case Service _:
          this.propService = parentObj.Service;
          break;
      }
      this.AddToCBReceivers();
    }

    public Base() => this.Initialize("", false);

    public event DisposeEventHandler Disposing;

    internal virtual void OnDisposing(bool disposing)
    {
      if (this.Disposing == null)
        return;
      this.Disposing((object) this, new DisposeEventArgs(disposing));
    }

    public void Dispose()
    {
      this.RemoveFromCBReceivers();
      if (this.propDisposed)
        return;
      this.Dispose(true, true);
      GC.SuppressFinalize((object) this);
    }

    internal virtual void Dispose(bool disposing, bool removeFromCollection)
    {
      this.RemoveFromCBReceivers();
      if (this.propDisposed)
        return;
      this.OnDisposing(disposing);
      if (!disposing)
        return;
      if (this.Service != null)
        this.Service.RemoveLogicalObject(this.LogicalName);
      if (ConnectionStates.Disconnecting != this.propConnectionState)
        this.Unlink();
      this.propDisposed = true;
      this.propLinkName = (string) null;
      this.propLogicalName = (string) null;
      this.propUserData = (object) null;
      this.propObjectParam = (string) null;
      this.propLinkParam = (string) null;
      this.propName = (string) null;
      this.propAddress = (string) null;
      this.propParent = (Base) null;
      this.propSNMPParent = (SNMPBase) null;
      this.propService = (Service) null;
      this.propCurLanguage = (string) null;
    }

    internal Base(string name) => this.Initialize(name, true);

    public Base(Base parentObj, string name)
    {
      this.Initialize(name, true);
      this.propParent = parentObj;
      this.propSNMPParent = (SNMPBase) null;
      if (parentObj != null)
        this.propService = parentObj.Service;
      this.AddToCBReceivers();
    }

    internal Base(string name, bool addToVColls) => this.Initialize(name, addToVColls);

    public Base(Base parentObj, string name, bool addToVColls)
    {
      this.Initialize(name, addToVColls);
      this.propParent = parentObj;
      switch (parentObj)
      {
        case Cpu _:
          this.propService = parentObj.Service;
          break;
        case Task _:
          this.propService = parentObj.Service;
          break;
        case Variable _:
          this.propService = parentObj.Service;
          break;
        case Service _:
          this.propService = parentObj.Service;
          break;
      }
      this.AddToCBReceivers();
    }

    ~Base()
    {
      if (this.Service != null && !this.Service.IsStatic && this.LinkId != 0U)
        PInvokePvicom.PviComUnlinkRequest(this.Service.hPvi, this.LinkId, (PviCallback) null, 0U, 0U);
      this.Dispose(false, true);
    }

    internal bool AddToCBReceivers() => this.Service != null && this.Service.AddID((object) this, ref this.propInternID);

    internal void RemoveFromCBReceivers()
    {
      if (this.Service == null)
        return;
      this.Service.RemoveID(this.propInternID);
    }

    internal int XCreateRequest(
      uint hPvi,
      string pObjName,
      ObjectType nPObjType,
      string pPObjDesc,
      uint eventParam,
      string pLinkDesc,
      uint respParam)
    {
      this.propHasLinkObject = true;
      if (eventParam == 0U && respParam == 0U)
        this.propHasLinkObject = false;
      int num;
      if (this.Service.EventMessageType == EventMessageType.CallBack)
      {
        if (this.Service.IsStatic)
        {
          switch (this)
          {
            case Cpu _:
            case Module _:
            case Variable _:
              num = PInvokePvicom.PviComCreateRequest(hPvi, pObjName, nPObjType, pPObjDesc, (PviCallback) null, 4294967294U, this.InternId, pLinkDesc, this.Service.cbCreate, 4294967294U, this.InternId);
              goto label_8;
          }
        }
        num = eventParam != 916U ? PInvokePvicom.PviComCreateRequest(hPvi, pObjName, nPObjType, pPObjDesc, this.Service.cbEvent, 4294967294U, this.InternId, pLinkDesc, this.Service.cbCreate, 4294967294U, this.InternId) : PInvokePvicom.PviComCreateRequest(hPvi, pObjName, nPObjType, pPObjDesc, this.Service.cbEventA, 4294967294U, this.InternId, pLinkDesc, this.Service.cbCreate, 4294967294U, this.InternId);
      }
      else
        num = !(this is Variable) || !this.Service.IsStatic ? PInvokePvicom.PviComMsgCreateRequest(hPvi, pObjName, nPObjType, pPObjDesc, this.Service.WindowHandle, eventParam, this.InternId, pLinkDesc, this.Service.WindowHandle, respParam, this.InternId) : PInvokePvicom.PviComMsgCreateRequest(hPvi, pObjName, nPObjType, pPObjDesc, IntPtr.Zero, eventParam, this.InternId, pLinkDesc, this.Service.WindowHandle, respParam, this.InternId);
label_8:
      return num;
    }

    internal int XLinkRequest(
      uint hPvi,
      string pObjName,
      uint eventParam,
      string pLinkDesc,
      uint respParam)
    {
      return this.XLinkRequest(hPvi, pObjName, eventParam, pLinkDesc, respParam, this.InternId);
    }

    internal int XLinkRequest(
      uint hPvi,
      string pObjName,
      uint eventParam,
      string pLinkDesc,
      uint respParam,
      uint internalID)
    {
      this.propHasLinkObject = true;
      StringMarshal stringMarshal = new StringMarshal();
      int num;
      if (this.Service.EventMessageType == EventMessageType.CallBack)
      {
        if (711U == eventParam)
        {
          num = 709U != respParam ? PInvokePvicom.PviComLinkRequest(hPvi, pObjName, this.Service.cbEvent, 4294967294U, this.InternId, pLinkDesc, this.Service.cbLink, 4294967294U, this.InternId) : PInvokePvicom.PviComLinkRequest(hPvi, pObjName, this.Service.cbEventA, 4294967294U, this.InternId, pLinkDesc, this.Service.cbLinkA, 4294967294U, this.InternId);
        }
        else
        {
          switch (respParam)
          {
            case 112:
            case 113:
            case 115:
            case 117:
            case 119:
            case 121:
            case 123:
            case 124:
            case 151:
              num = PInvokePvicom.PviComLinkRequest(hPvi, pObjName, this.Service.cbEvent, 4294967294U, this.InternId, pLinkDesc, this.Service.cbLinkA, 4294967294U, this.InternId);
              break;
            case 709:
              num = PInvokePvicom.PviComLinkRequest(hPvi, pObjName, this.Service.cbEvent, 4294967294U, this.InternId, pLinkDesc, this.Service.cbLinkB, 4294967294U, this.InternId);
              break;
            case 2712:
              num = PInvokePvicom.PviComLinkRequest(hPvi, pObjName, this.Service.cbEvent, 4294967294U, this.InternId, pLinkDesc, this.Service.cbLinkD, 4294967294U, this.InternId);
              break;
            case 2713:
              num = PInvokePvicom.PviComLinkRequest(hPvi, pObjName, this.Service.cbEvent, 4294967294U, this.InternId, pLinkDesc, this.Service.cbLinkC, 4294967294U, this.InternId);
              break;
            case 2718:
              num = PInvokePvicom.PviComLinkRequest(hPvi, pObjName, this.Service.cbEvent, 4294967294U, this.InternId, pLinkDesc, this.Service.cbLinkE, 4294967294U, this.InternId);
              break;
            default:
              num = PInvokePvicom.PviComLinkRequest(hPvi, pObjName, this.Service.cbEvent, 4294967294U, this.InternId, pLinkDesc, this.Service.cbLink, 4294967294U, this.InternId);
              break;
          }
        }
      }
      else
      {
        byte[] bytes1 = stringMarshal.GetBytes(pObjName);
        byte[] bytes2 = stringMarshal.GetBytes(pLinkDesc);
        num = PInvokePvicom.PviComMsgLinkRequest(hPvi, bytes1, this.Service.WindowHandle, eventParam, internalID, bytes2, this.Service.WindowHandle, respParam, internalID);
      }
      return num;
    }

    internal int XMLinkRequest(
      uint hPvi,
      out uint pLinkId,
      string objName,
      uint eventMsgNo,
      uint eventParam,
      string linkDesc)
    {
      return PInvokePvicom.PviComMsgLink(hPvi, out pLinkId, objName, this.Service.WindowHandle, 6U, 6U, linkDesc);
    }

    internal int XMLink(
      uint hPvi,
      out uint pLinkId,
      string objName,
      uint eventMsgNo,
      uint eventParam,
      string linkDesc)
    {
      return PInvokePvicom.PviComMsgLink(hPvi, out pLinkId, objName, this.Service.WindowHandle, 6U, 6U, linkDesc);
    }

    internal int Write(uint hPvi, uint linkID, AccessTypes nAccess, IntPtr pData, int dataLen)
    {
      IntPtr zero = IntPtr.Zero;
      int rstDataLen = 0;
      return PInvokePvicom.PviComWrite(hPvi, linkID, nAccess, pData, dataLen, zero, rstDataLen);
    }

    internal int WriteRequest(
      uint hPvi,
      uint linkID,
      AccessTypes nAccess,
      string strValue,
      uint respParam,
      uint objID)
    {
      uint respParam1 = respParam;
      IntPtr hMemory = IntPtr.Zero;
      if (uint.MaxValue != objID)
        respParam1 = objID;
      if (0 < strValue.Length)
        hMemory = PviMarshal.StringToHGlobal(strValue);
      int num = this.Service.EventMessageType != EventMessageType.CallBack ? PInvokePvicom.PviComMsgWriteRequest(hPvi, linkID, nAccess, hMemory, strValue.Length, this.Service.WindowHandle, 1300U, this.InternId) : (respParam != 723U ? PInvokePvicom.PviComWriteRequest(hPvi, linkID, nAccess, hMemory, strValue.Length, this.Service.cbWrite, 4294967294U, respParam1) : PInvokePvicom.PviComWriteRequest(hPvi, linkID, nAccess, hMemory, strValue.Length, this.Service.cbWriteA, 4294967294U, respParam1));
      if (IntPtr.Zero != hMemory)
        PviMarshal.FreeHGlobal(ref hMemory);
      return num;
    }

    internal int WriteRequest(
      uint hPvi,
      uint linkID,
      AccessTypes nAccess,
      string strValue,
      uint respParam)
    {
      return this.WriteRequest(hPvi, linkID, nAccess, strValue, respParam, uint.MaxValue);
    }

    internal int WriteRequestA(
      uint hPvi,
      uint linkID,
      AccessTypes nAccess,
      IntPtr pData,
      int dataLen,
      uint respParam)
    {
      return this.Service.EventMessageType != EventMessageType.CallBack ? PInvokePvicom.PviComMsgWriteRequest(hPvi, linkID, nAccess, pData, dataLen, this.Service.WindowHandle, respParam, this.InternId) : PInvokePvicom.PviComWriteRequest(hPvi, linkID, nAccess, pData, dataLen, this.Service.cbWriteA, 4294967294U, this.InternId);
    }

    internal int WriteRequest(
      uint hPvi,
      uint linkID,
      AccessTypes nAccess,
      IntPtr pData,
      int dataLen,
      uint respParam)
    {
      int num;
      if (this.Service.EventMessageType == EventMessageType.CallBack)
      {
        switch (respParam)
        {
          case 0:
            num = PInvokePvicom.PviComWriteRequest(hPvi, linkID, nAccess, pData, dataLen, (PviCallback) null, 0U, 0U);
            break;
          case 303:
          case 503:
          case 815:
            num = PInvokePvicom.PviComWriteRequest(hPvi, linkID, nAccess, pData, dataLen, this.Service.cbWriteA, 4294967294U, this.InternId);
            break;
          case 304:
            num = PInvokePvicom.PviComWriteRequest(hPvi, linkID, nAccess, pData, dataLen, this.Service.cbWriteC, 4294967294U, this.InternId);
            break;
          case 312:
          case 410:
            num = PInvokePvicom.PviComWriteRequest(hPvi, linkID, nAccess, pData, dataLen, this.Service.cbWriteF, 4294967294U, this.InternId);
            break;
          case 403:
          case 555:
          case 816:
            num = PInvokePvicom.PviComWriteRequest(hPvi, linkID, nAccess, pData, dataLen, this.Service.cbWriteB, 4294967294U, this.InternId);
            break;
          case 404:
            num = PInvokePvicom.PviComWriteRequest(hPvi, linkID, nAccess, pData, dataLen, this.Service.cbWriteD, 4294967294U, this.InternId);
            break;
          case 405:
            num = PInvokePvicom.PviComWriteRequest(hPvi, linkID, nAccess, pData, dataLen, this.Service.cbWriteE, 4294967294U, this.InternId);
            break;
          case 412:
            num = PInvokePvicom.PviComWriteRequest(hPvi, linkID, nAccess, pData, dataLen, this.Service.cbWriteF, 4294967294U, this.InternId);
            break;
          default:
            num = PInvokePvicom.PviComWriteRequest(hPvi, linkID, nAccess, pData, dataLen, this.Service.cbWrite, 4294967294U, this.InternId);
            break;
        }
      }
      else
        num = PInvokePvicom.PviComMsgWriteRequest(hPvi, linkID, nAccess, pData, dataLen, this.Service.WindowHandle, respParam, this.InternId);
      return num;
    }

    internal int Read(uint hPvi, uint linkID, AccessTypes nAccess, SyncReadData readData) => PInvokePvicom.PviComRead(hPvi, linkID, nAccess, readData.PtrArgData, readData.ArgDataLength, readData.PtrData, readData.DataLength);

    internal int ReadRequest(uint hPvi, uint linkID, AccessTypes nAccess, uint respParam)
    {
      int num;
      if (this.Service.EventMessageType == EventMessageType.CallBack)
      {
        switch (respParam)
        {
          case 150:
          case 700:
          case 907:
            num = PInvokePvicom.PviComReadRequest(hPvi, linkID, nAccess, this.Service.cbReadA, 4294967294U, this.InternId);
            break;
          case 908:
          case 2810:
            num = PInvokePvicom.PviComReadRequest(hPvi, linkID, nAccess, this.Service.cbReadB, 4294967294U, this.InternId);
            break;
          case 913:
            num = PInvokePvicom.PviComReadRequest(hPvi, linkID, nAccess, this.Service.cbReadD, 4294967294U, this.InternId);
            break;
          case 915:
            num = PInvokePvicom.PviComReadRequest(hPvi, linkID, nAccess, this.Service.cbReadC, 4294967294U, this.InternId);
            break;
          case 2809:
          case 2811:
            num = PInvokePvicom.PviComReadRequest(hPvi, linkID, nAccess, this.Service.cbReadE, 4294967294U, this.InternId);
            break;
          default:
            num = PInvokePvicom.PviComReadRequest(hPvi, linkID, nAccess, this.Service.cbRead, 4294967294U, this.InternId);
            break;
        }
      }
      else
        num = PInvokePvicom.PviComMsgReadRequest(hPvi, linkID, nAccess, this.Service.WindowHandle, respParam, this.InternId);
      return num;
    }

    internal int ReadArgumentRequest(
      uint hPvi,
      uint linkID,
      AccessTypes nAccess,
      string reqData,
      uint respParam,
      uint internalID)
    {
      IntPtr hglobal = PviMarshal.StringToHGlobal(reqData);
      int dataLen = reqData != null ? reqData.Length : 0;
      int num = this.ReadArgumentRequest(hPvi, linkID, nAccess, hglobal, dataLen, respParam, internalID);
      if (IntPtr.Zero != hglobal)
        PviMarshal.FreeHGlobal(ref hglobal);
      return num;
    }

    internal int ReadArgumentRequest(
      uint hPvi,
      uint linkID,
      AccessTypes nAccess,
      IntPtr pData,
      int dataLen,
      uint respParam)
    {
      return this.ReadArgumentRequest(hPvi, linkID, nAccess, pData, dataLen, respParam, this.InternId);
    }

    internal int ReadArgumentRequest(
      uint hPvi,
      uint linkID,
      AccessTypes nAccess,
      IntPtr pData,
      int dataLen,
      uint respParam,
      uint internalID)
    {
      int num;
      if (this.Service.EventMessageType == EventMessageType.CallBack)
      {
        switch (respParam)
        {
          case 214:
            num = PInvokePvicom.PviComReadArgumentRequest(hPvi, linkID, nAccess, pData, dataLen, this.Service.cbReadT, 4294967294U, this.InternId);
            break;
          case 725:
            num = PInvokePvicom.PviComReadArgumentRequest(hPvi, linkID, nAccess, pData, dataLen, this.Service.cbReadS, 4294967294U, this.InternId);
            break;
          case 901:
            num = PInvokePvicom.PviComReadArgumentRequest(hPvi, linkID, nAccess, pData, dataLen, this.Service.cbReadC, 4294967294U, this.InternId);
            break;
          case 904:
            num = PInvokePvicom.PviComReadArgumentRequest(hPvi, linkID, nAccess, pData, dataLen, this.Service.cbReadB, 4294967294U, this.InternId);
            break;
          case 912:
            num = PInvokePvicom.PviComReadArgumentRequest(hPvi, linkID, nAccess, pData, dataLen, this.Service.cbReadA, 4294967294U, this.InternId);
            break;
          case 914:
            num = PInvokePvicom.PviComReadArgumentRequest(hPvi, linkID, nAccess, pData, dataLen, this.Service.cbReadD, 4294967294U, this.InternId);
            break;
          default:
            num = PInvokePvicom.PviComReadArgumentRequest(hPvi, linkID, nAccess, pData, dataLen, this.Service.cbRead, 4294967294U, this.InternId);
            break;
        }
      }
      else
        num = PInvokePvicom.PviComMsgReadArgumentRequest(hPvi, linkID, nAccess, pData, dataLen, this.Service.WindowHandle, respParam, internalID);
      return num;
    }

    internal int PviDelete()
    {
      this.propHasLinkObject = false;
      this.propConnectionState = ConnectionStates.Unininitialized;
      return PInvokePvicom.PviComDelete(this.Service.hPvi, this.FullName);
    }

    internal int DeleteRequest(bool silent)
    {
      this.propHasLinkObject = false;
      int num;
      if (this.Service.EventMessageType == EventMessageType.CallBack)
      {
        if (silent)
        {
          this.propNoDisconnectedEvent = true;
          num = PInvokePvicom.PviComDeleteRequest(this.Service.hPvi, this.FullName, this.Service.cbDelete, 0U, 0U);
        }
        else
          num = PInvokePvicom.PviComDeleteRequest(this.Service.hPvi, this.FullName, this.Service.cbDelete, 4294967294U, this.InternId);
      }
      else if (silent)
      {
        this.propNoDisconnectedEvent = true;
        num = PInvokePvicom.PviComMsgDeleteRequest(this.Service.hPvi, this.FullName, this.Service.WindowHandle, 0U, this.InternId);
      }
      else
        num = PInvokePvicom.PviComMsgDeleteRequest(this.Service.hPvi, this.FullName, this.Service.WindowHandle, 6U, this.InternId);
      return num;
    }

    internal int Unlink()
    {
      int num = 0;
      this.propHasLinkObject = false;
      if (this.propLinkId != 0U)
      {
        num = PInvokePvicom.PviComUnlink(this.Service.hPvi, this.propLinkId);
        this.propLinkId = 0U;
      }
      else
        this.Requests |= Actions.Disconnect;
      return num;
    }

    internal int UnlinkRequest(uint respParam)
    {
      int num = 0;
      this.propHasLinkObject = false;
      if (this.propLinkId != 0U)
      {
        if (this.Service.EventMessageType == EventMessageType.CallBack)
        {
          if (respParam == 0U)
          {
            num = PInvokePvicom.PviComUnlinkRequest(this.Service.hPvi, this.propLinkId, (PviCallback) null, 0U, 0U);
          }
          else
          {
            switch (respParam)
            {
              case 502:
              case 2713:
                num = PInvokePvicom.PviComUnlinkRequest(this.Service.hPvi, this.propLinkId, this.Service.cbUnlinkA, 4294967294U, this.InternId);
                break;
              case 602:
              case 2712:
                num = PInvokePvicom.PviComUnlinkRequest(this.Service.hPvi, this.propLinkId, this.Service.cbUnlinkB, 4294967294U, this.InternId);
                break;
              case 2718:
                num = PInvokePvicom.PviComUnlinkRequest(this.Service.hPvi, this.propLinkId, this.Service.cbUnlinkC, 4294967294U, this.InternId);
                break;
              case 2813:
                num = PInvokePvicom.PviComUnlinkRequest(this.Service.hPvi, this.propLinkId, this.Service.cbUnlinkD, 4294967294U, this.InternId);
                break;
              default:
                num = PInvokePvicom.PviComUnlinkRequest(this.Service.hPvi, this.propLinkId, this.Service.cbUnlink, 4294967294U, this.InternId);
                break;
            }
          }
        }
        else
          num = respParam != 0U ? PInvokePvicom.PviComMsgUnlinkRequest(this.Service.hPvi, this.propLinkId, this.Service.WindowHandle, respParam, this.InternId) : PInvokePvicom.PviComMsgUnlinkRequest(this.Service.hPvi, this.propLinkId, IntPtr.Zero, 0U, 0U);
      }
      else
        this.Requests |= Actions.Disconnect;
      return num;
    }

    internal virtual void RemoveObject() => this.Remove();

    internal virtual void RemoveReferences()
    {
    }

    internal virtual void RemoveFromBaseCollections()
    {
      if (this.Service == null)
        return;
      this.Service.LogicalObjects.Remove(this.LogicalName);
      if (this.Service.Services == null)
        return;
      this.Service.Services.LogicalObjects.Remove(this.LogicalName);
    }

    public virtual void Remove()
    {
      if (this.Service != null)
      {
        this.Service.LogicalObjects.Remove(this.LogicalName);
        if (this.Service.Services != null)
          this.Service.Services.LogicalObjects.Remove(this.LogicalName);
      }
      if (this.Removed == null)
        return;
      if (this.Service == null)
        this.Removed((object) this, new PviEventArgs(this.Name, this.Address, 0, "en", Action.ObjectRemoved));
      else
        this.Removed((object) this, new PviEventArgs(this.Name, this.Address, 0, this.Service.Language, Action.ObjectRemoved, this.Service));
    }

    internal void Fire_Deleted(PviEventArgs e) => this.OnDeleted(e);

    protected virtual void OnDeleted(PviEventArgs e)
    {
      if (e.ErrorCode != 0)
        return;
      this.Remove();
    }

    protected bool Fire_Connected(PviEventArgs e)
    {
      if (e.ErrorCode == 0 || 12002 == e.ErrorCode)
      {
        if (ConnectionStates.Connected == this.propConnectionState)
          return false;
        this.isObjectConnected = true;
        this.propConnectionState = ConnectionStates.Connected;
        if (this.Connected != null)
          this.Connected((object) this, e);
      }
      else if (ConnectionStates.ConnectedError > this.propConnectionState && ConnectionStates.Unininitialized < this.propConnectionState && this.Connected != null)
        this.Connected((object) this, e);
      return true;
    }

    protected virtual void OnLinked(int errorCode, Action actionCode)
    {
      this.propErrorCode = errorCode;
      if (this.Linked == null)
        return;
      this.Linked((object) this, new PviEventArgs(this.Name, this.Address, errorCode, this.Service.Language, actionCode));
    }

    internal void Fire_OnConnected(PviEventArgs e) => this.OnConnected(e);

    internal void Fire_OnDisconnected(PviEventArgs e) => this.OnDisconnected(e);

    protected virtual void OnConnected(PviEventArgs e)
    {
      this.propErrorCode = e.ErrorCode;
      if (e.ErrorCode == 0 || 12002 == e.ErrorCode)
      {
        this.propErrorCode = 0;
        if (ConnectionStates.Connected == this.propConnectionState)
          return;
        this.isObjectConnected = true;
        if (ConnectionStates.ConnectionChanging == this.propConnectionState)
        {
          this.OnConnectionChanged(e.ErrorCode, Action.ChangeConnection);
        }
        else
        {
          this.propConnectionState = ConnectionStates.Connected;
          if (this.Connected == null)
            return;
          this.Connected((object) this, e);
        }
      }
      else
      {
        if ((ConnectionStates.ConnectedError <= this.propConnectionState || ConnectionStates.Unininitialized >= this.propConnectionState) && this.propConnectionState != ConnectionStates.Unininitialized)
          return;
        this.propConnectionState = ConnectionStates.ConnectedError;
        if (this.Connected == null)
          return;
        this.Connected((object) this, e);
      }
    }

    protected virtual void OnConnectionChanged(int errorCode, Action action)
    {
      this.propConnectionState = errorCode == 0 ? ConnectionStates.Connected : ConnectionStates.ConnectedError;
      if (this.ConnectionChanged == null)
        return;
      this.ConnectionChanged((object) this, new PviEventArgs(this.Name, this.Address, errorCode, this.Service.Language, action));
    }

    internal virtual void reCreateState()
    {
      if (!this.reCreateActive)
        return;
      this.propConnectionState = ConnectionStates.Unininitialized;
      this.propReCreateActive = true;
      this.reCreateActive = false;
      this.propLinkId = 0U;
      this.Connect(true);
    }

    protected virtual string getLinkDescription() => "EV=ed";

    protected virtual string GetConnectionDescription() => LogicalObjectsUsage.ObjectNameWithType == this.Service.LogicalObjectsUsage ? string.Format("\"{0}\"", (object) this.propAddress) : string.Format("\"{0}\"/\"{1}\"", (object) this.propParent.LinkName, (object) this.propAddress);

    public virtual void Connect()
    {
      if (this.reCreateActive || this.LinkId != 0U)
        return;
      this.Connect(ConnectionType.CreateAndLink);
    }

    internal virtual void Connect(bool forceConnection)
    {
      if (this.reCreateActive || this.LinkId != 0U)
        return;
      this.Connect(ConnectionType.CreateAndLink);
    }

    public virtual void Connect(ConnectionType connectionType)
    {
      this.propNoDisconnectedEvent = false;
      this.propReturnValue = 0;
    }

    public virtual int ChangeConnection() => this.WriteRequest(this.Service.hPvi, this.LinkId, AccessTypes.Connect, this.GetConnectionDescription(), 3000U, this.propInternID);

    public virtual void Disconnect() => this.Disconnect(false);

    internal virtual int DisconnectRet(uint action)
    {
      this.Disconnect();
      return 0;
    }

    public virtual void Disconnect(bool noResponse)
    {
      this.propReturnValue = 0;
      this.propNoDisconnectedEvent = noResponse;
      this.propConnectionState = ConnectionStates.Disconnecting;
      if (noResponse)
      {
        this.propReturnValue = this.UnlinkRequest(0U);
        this.propLinkId = 0U;
        this.propConnectionState = ConnectionStates.Disconnected;
      }
      else
        this.propReturnValue = this.UnlinkRequest(this.InternId);
    }

    protected virtual void Call_Connected(PviEventArgs e)
    {
      if (this.Connected == null)
        return;
      this.Connected((object) this, e);
    }

    protected virtual void Fire_Connected(object sender, PviEventArgs e)
    {
      if (e.ErrorCode == 0 || 12002 == e.ErrorCode)
      {
        if (ConnectionStates.Connected == this.propConnectionState)
          return;
        this.isObjectConnected = true;
        this.propConnectionState = ConnectionStates.Connected;
        if (this.Connected == null)
          return;
        this.Connected(sender, e);
      }
      else
      {
        if (ConnectionStates.ConnectedError <= this.propConnectionState || ConnectionStates.Unininitialized >= this.propConnectionState)
          return;
        this.propConnectionState = ConnectionStates.ConnectedError;
        if (this.Connected == null)
          return;
        this.Connected(sender, e);
      }
    }

    internal void Fire_ConnectedEvent(object sender, PviEventArgs e)
    {
      if (this.Connected == null)
        return;
      this.Connected(sender, e);
    }

    internal void FireDisconnected(int errorCode, Action action) => this.OnDisconnected(new PviEventArgs(this.Name, this.Address, errorCode, this.Service.Language, action));

    protected virtual void OnDisconnected(PviEventArgs e)
    {
      if (Service.IsRemoteError(e.ErrorCode) && this.propConnectionState != ConnectionStates.Unininitialized && ConnectionStates.Disconnected != this.propConnectionState && ConnectionStates.Disconnecting != this.propConnectionState && this.Address.CompareTo("MacAddresses") != 0)
        this.reCreateActive = true;
      if (Action.ErrorEvent == e.Action && (12039 > e.ErrorCode || 12900 < e.ErrorCode))
      {
        this.propConnectionState = ConnectionStates.Disconnected;
        this.isObjectConnected = false;
      }
      else
      {
        if (ConnectionStates.Connected == this.propConnectionState)
          this.propConnectionState = ConnectionStates.ConnectedError;
        if (ConnectionStates.Disconnecting == this.propConnectionState)
          this.propConnectionState = ConnectionStates.Disconnected;
      }
      if (this.propNoDisconnectedEvent)
        return;
      this.propNoDisconnectedEvent = false;
      if (this.Disconnected == null)
        return;
      this.Disconnected((object) this, e);
    }

    protected internal virtual void OnError(PviEventArgs e)
    {
      this.propErrorCode = e.ErrorCode;
      if (this.Service != null)
      {
        if (this.Service.ErrorException)
          throw new PviException(e.ErrorText, e.ErrorCode, (object) this, e);
        if (this.Service.ErrorEvent && this.Error != null)
          this.Error((object) this, e);
        if (this.Service == null)
          return;
        this.Service.OnError((object) this, e);
      }
      else
      {
        if (this.Error == null)
          return;
        this.Error((object) this, e);
      }
    }

    protected internal void OnSingleError(PviEventArgs e)
    {
      if (this.Error == null)
        return;
      this.Error((object) this, e);
    }

    protected internal virtual void OnError(object sender, PviEventArgs e)
    {
      if (this.Error == null)
        return;
      this.Error(sender, e);
    }

    protected virtual void OnPropertyChanged(PviEventArgs e)
    {
      if (this.PropertyChanged == null)
        return;
      this.PropertyChanged((object) this, e);
    }

    public virtual int FromXmlTextReader(
      ref XmlTextReader reader,
      ConfigurationFlags flags,
      Base baseObj)
    {
      string str = "";
      string attribute1 = reader.GetAttribute("Name");
      if (attribute1 != null && attribute1.Length > 0)
        baseObj.propName = attribute1;
      str = "";
      string attribute2 = reader.GetAttribute("Address");
      if (attribute2 != null && attribute2.Length > 0)
        baseObj.propAddress = attribute2;
      str = "";
      string attribute3 = reader.GetAttribute("UserData");
      if (attribute3 != null && attribute3.Length > 0)
        baseObj.propUserData = (object) attribute3;
      str = "";
      string attribute4 = reader.GetAttribute("LinkName");
      if (attribute4 != null && attribute4.Length > 0)
        baseObj.propLinkName = attribute4;
      str = "";
      string attribute5 = reader.GetAttribute("ConnectionType");
      if (attribute5 != null && attribute5.Length > 0 && (ConfigurationFlags.ConnectionState & flags) != ConfigurationFlags.None)
      {
        switch (attribute5.ToLower())
        {
          case "create":
            baseObj.propConnectionType = ConnectionType.Create;
            break;
          case "createandlink":
            baseObj.propConnectionType = ConnectionType.CreateAndLink;
            break;
          case "link":
            baseObj.propConnectionType = ConnectionType.Link;
            break;
          case "none":
            baseObj.propConnectionType = ConnectionType.None;
            break;
        }
      }
      str = "";
      string attribute6 = reader.GetAttribute("ErrorCode");
      if (attribute6 != null && attribute6.Length > 0)
      {
        int result = 0;
        if (PviParse.TryParseInt32(attribute6, out result))
          baseObj.propErrorCode = result;
      }
      str = "";
      string attribute7 = reader.GetAttribute("Connected");
      if (attribute7 != null && attribute7.ToLower() == "true" && (ConfigurationFlags.ConnectionState & flags) != ConfigurationFlags.None)
        baseObj.propRequests |= Actions.Connect;
      return 0;
    }

    internal virtual int ToXMLTextWriter(ref XmlTextWriter writer, ConfigurationFlags flags)
    {
      if (this.propName != null && this.propName.Length > 0)
        writer.WriteAttributeString("Name", this.propName);
      if (this.propName != this.propAddress && this.propAddress != null && this.propAddress.Length > 0)
        writer.WriteAttributeString("Address", this.propAddress);
      if (this.propUserData is string && this.propUserData != null && ((string) this.propUserData).Length > 0)
        writer.WriteAttributeString("UserData", this.propUserData.ToString());
      if (this.propLinkName != null && this.propLinkName.Length > 0)
        writer.WriteAttributeString("LinkName", this.propLinkName);
      writer.WriteAttributeString("Connected", this.propConnectionState.ToString());
      if (this.propErrorCode > 0)
        writer.WriteAttributeString("ErrorCode", this.propErrorCode.ToString());
      if ((ConfigurationFlags.ConnectionState & flags) != ConfigurationFlags.None && this.ConnectionType != ConnectionType.CreateAndLink)
        writer.WriteAttributeString("ConnectionType", this.ConnectionType.ToString());
      return 0;
    }

    public event PviEventHandler Connected;

    internal event PviEventHandler Linked;

    public event PviEventHandler ConnectionChanged;

    public event PviEventHandler Removed;

    public event PviEventHandler Disconnected;

    public event PviEventHandler Error;

    public event PviEventHandler PropertyChanged;

    public string Address
    {
      get => this.propAddress;
      set => this.propAddress = value;
    }

    internal string AddressEx => this.propAddress != null && 0 < this.propAddress.Length ? this.propAddress : this.propName;

    public string Name => this.propName;

    public virtual Base Parent => this.propParent;

    public abstract string FullName { get; }

    public abstract string PviPathName { get; }

    public virtual Service Service => this.propService;

    public object UserData
    {
      get => this.propUserData;
      set => this.propUserData = value;
    }

    internal MethodType MethodType
    {
      get => this.propMethodType;
      set => this.propMethodType = value;
    }

    public virtual bool HasPVIConnection => ConnectionStates.Connected == this.propConnectionState || ConnectionStates.ConnectedError == this.propConnectionState;

    public virtual bool IsConnected => ConnectionStates.Connected == this.propConnectionState;

    public virtual bool HasLinkObject => this.propHasLinkObject;

    public bool HasError => 0 != this.ErrorCode;

    internal string LogicalName
    {
      get
      {
        if (this.propLogicalName != null && this.propLogicalName.Length != 0)
          return this.propLogicalName;
        switch (this.Service.LogicalObjectsUsage)
        {
          case LogicalObjectsUsage.ObjectName:
            return this.Name;
          case LogicalObjectsUsage.ObjectNameWithType:
            return this.PviPathName;
          default:
            return this.FullName;
        }
      }
    }

    internal string ObjectParam
    {
      get => this.propObjectParam;
      set => this.propObjectParam = value;
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

    internal uint LinkId => this.propLinkId;

    internal uint InternId => this.propInternID;

    public ConnectionType ConnectionType
    {
      get => this.propConnectionType;
      set => this.propConnectionType = value;
    }

    public int ErrorCode => this.propErrorCode;

    public string ErrorText
    {
      get
      {
        string language = "en";
        if (this.Service != null)
          language = this.Service.Language;
        if (this.propErrorText == string.Empty)
        {
          this.propCurLanguage = language;
          this.propErrorText = this.Service != null ? this.Service.Utilities.GetErrorText(this.ErrorCode) : Service.GetErrorText(this.ErrorCode, language);
          return this.propErrorText;
        }
        if (language.CompareTo(this.propCurLanguage) == 0)
          return this.propErrorText;
        this.propCurLanguage = language;
        this.propErrorText = this.Service != null ? this.Service.Utilities.GetErrorText(this.ErrorCode) : Service.GetErrorText(this.ErrorCode, language);
        return this.propErrorText;
      }
    }

    public string LinkName
    {
      get
      {
        if (this.propLinkName != null && this.propLinkName.Length != 0)
          return this.propLinkName;
        if (this.Service == null)
          return this.Name;
        switch (this.Service.LogicalObjectsUsage)
        {
          case LogicalObjectsUsage.FullName:
            return this.FullName;
          case LogicalObjectsUsage.ObjectName:
            return this.Name;
          case LogicalObjectsUsage.ObjectNameWithType:
            return this.PviPathName;
          default:
            return this.LogicalName;
        }
      }
      set => this.propLinkName = value;
    }

    internal static uint MakeWindowMessage(uint pvisAction) => 0U < pvisAction ? pvisAction + 10000U : 0U;

    internal static uint MakeWindowMessage(Action pvisAction) => Base.MakeWindowMessage((uint) pvisAction);

    protected virtual string GetObjectName() => this.propSNMPParent == null ? this.LinkName : this.propSNMPParent.FullName + "." + this.Name;

    internal virtual int PviLinkObject(uint action)
    {
      if (this.propLinkName == null || this.propLinkName.Length == 0)
        this.propLinkName = this.GetObjectName();
      return !this.Service.IsStatic || this.ConnectionType == ConnectionType.Link ? this.XLinkRequest(this.Service.hPvi, this.LinkName, 550U, this.propLinkParam, action) : this.XLinkRequest(this.Service.hPvi, this.GetObjectName(), 550U, this.propLinkParam, action);
    }

    internal int Read_FormatEX(uint lnkID)
    {
      int num = 0;
      if (!this.propReadingFormat && this.propHasLinkObject)
      {
        this.propReadingFormat = true;
        num = !(this.propParent is Service) ? this.ReadRequest(this.Service.hPvi, lnkID, AccessTypes.TypeExtern, 2810U) : this.ReadRequest(this.Service.hPvi, lnkID, AccessTypes.TypeIntern, 2810U);
      }
      return num;
    }

    internal override void OnPviCreated(int errorCode, uint linkID)
    {
      this.propErrorCode = errorCode;
      this.propLinkId = linkID;
      if (errorCode == 0 || 12002 == errorCode)
      {
        if (1U > linkID && this.Service.IsStatic && this.ConnectionType == ConnectionType.CreateAndLink)
        {
          this.propErrorCode = this.XLinkRequest(this.Service.hPvi, this.LinkName, 703U, this.propLinkParam, 704U);
        }
        else
        {
          this.propErrorCode = errorCode;
          if (this.ConnectionType != ConnectionType.Link)
            return;
          this.OnConnected(new PviEventArgs(this.Name, this.Address, errorCode, this.Service.Language, Action.ConnectedEvent, this.Service));
        }
      }
      else
        this.OnError(new PviEventArgs(this.Name, this.Address, errorCode, this.Service.Language, Action.ConnectedEvent, this.Service));
    }

    internal override void OnPviCancelled(int errorCode, int type) => this.propErrorCode = errorCode;

    internal override void OnPviLinked(int errorCode, uint linkID, int option)
    {
      this.propErrorCode = errorCode;
      PviEventArgs e = new PviEventArgs(this.Name, this.Address, errorCode, this.Service.Language, Action.LinkObject, this.Service);
      this.OnConnected(e);
      if (errorCode == 0)
      {
        this.propLinkId = linkID;
        this.propConnectionState = ConnectionStates.Connected;
      }
      if (this.Linked != null)
        this.Linked((object) this, e);
      if (errorCode == 0)
        return;
      this.OnError(e);
    }

    internal override void OnPviEvent(
      int errorCode,
      EventTypes eventType,
      PVIDataStates dataState,
      IntPtr pData,
      uint dataLen,
      int option)
    {
      this.propErrorCode = errorCode;
      switch (eventType)
      {
        case EventTypes.Error:
        case EventTypes.Data:
          if (ConnectionStates.Connected > this.propConnectionState || errorCode == 0)
            this.OnConnected(new PviEventArgs(this.Name, this.Address, errorCode, this.Service.Language, Action.ConnectedEvent, this.Service));
          if (errorCode == 0 || 12002 == errorCode)
          {
            this.OnError(new PviEventArgs(this.Name, this.Address, 0, this.Service.Language, Action.ConnectedEvent, this.Service));
            break;
          }
          this.OnError(new PviEventArgs(this.Name, this.Address, errorCode, this.Service.Language, Action.ConnectedEvent, this.Service));
          break;
        case EventTypes.Disconnect:
          this.OnDisconnected(new PviEventArgs(this.Name, this.Address, errorCode, this.Service.Language, Action.DisconnectedEvent, this.Service));
          break;
        case EventTypes.Connection:
          this.OnConnected(new PviEventArgs(this.Name, this.Address, errorCode, this.Service.Language, Action.ConnectedEvent, this.Service));
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
      if (PVIWriteAccessTypes.Connection != accessType)
        return;
      this.OnConnectionChanged(errorCode, (Action) accessType);
    }

    internal override void OnPviUnLinked(int errorCode, int option)
    {
      this.propErrorCode = errorCode;
      if (ConnectionStates.Disconnecting != this.propConnectionState)
        return;
      this.propLinkId = 0U;
      this.OnDisconnected(new PviEventArgs(this.Name, this.Address, errorCode, this.Service.Language, Action.DisconnectedEvent));
    }

    internal override void OnPviDeleted(int errorCode) => this.propErrorCode = errorCode;

    internal override void OnPviChangedLink(int errorCode) => this.propErrorCode = errorCode;
  }
}
