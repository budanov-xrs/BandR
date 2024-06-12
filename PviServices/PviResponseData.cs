// Decompiled with JetBrains decompiler
// Type: BR.AN.PviServices.PviResponseData
// Assembly: BR.AN.PVIServices, Version=1.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DA853F0A-C070-4E30-B7A7-51D955FB85F6
// Assembly location: C:\Users\ss_six\Desktop\FXE\FXE software integration package\COMET_FXE-PVIServices_Example\Pluto-PVI-trial_01\packages\IToolS.BandR.3.5.58\lib\net20\BR.AN.PVIServices.dll

using System;

namespace BR.AN.PviServices
{
  internal class PviResponseData
  {
    private AccessModes propAccessMode;
    private AccessTypes propAccessType;
    private EventTypes propEventType;
    private int propErrorCode;
    private uint propLinkID;
    private int propState;
    private int propWParam;
    private int propLParam;
    private Action propAction;
    private IntPtr propPtrData;
    private int propDataLen;

    public PviResponseData(int wParam, int lParam, IntPtr pData, int dataLen, ResponseInfo info)
    {
      this.propDataLen = dataLen;
      this.propAction = (Action) lParam;
      this.propWParam = wParam;
      this.propLParam = lParam;
      this.propPtrData = pData;
      this.propAccessMode = (AccessModes) info.Mode;
      this.propAccessType = (AccessTypes) info.Type;
      this.propEventType = (EventTypes) info.Type;
      this.propState = info.Status;
      this.propLinkID = info.LinkId;
      this.propErrorCode = info.Error;
    }

    public AccessModes AccessMode => this.propAccessMode;

    public AccessTypes AccessType => this.propAccessType;

    internal EventTypes EventType => this.propEventType;

    public int ErrorCode => this.propErrorCode;

    public uint LinkID => this.propLinkID;

    public int State => this.propState;

    public int WParam => this.propWParam;

    public int LParam => this.propLParam;

    public Action Action => this.propAction;

    public IntPtr PtrData => this.propPtrData;

    public int DataLen => this.propDataLen;

    public bool IsError => this.propErrorCode != 0 && 12002 != this.propErrorCode;
  }
}
