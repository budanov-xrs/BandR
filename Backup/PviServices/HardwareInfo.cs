// Decompiled with JetBrains decompiler
// Type: BR.AN.PviServices.HardwareInfo
// Assembly: BR.AN.PVIServices, Version=1.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DA853F0A-C070-4E30-B7A7-51D955FB85F6
// Assembly location: C:\Users\ss_six\Desktop\FXE\FXE software integration package\COMET_FXE-PVIServices_Example\Pluto-PVI-trial_01\packages\IToolS.BandR.3.5.58\lib\net20\BR.AN.PVIServices.dll

using System;
using System.Collections;
using System.Runtime.InteropServices;

namespace BR.AN.PviServices
{
  public class HardwareInfo : PviCBEvents, IDisposable
  {
    private ArrayList propItems;
    private Cpu propCpu;
    private uint propInternID;
    private uint propEntriesCount;
    private uint propDataLength;
    private PlcFamily propHWPLCFamily;
    private byte[] propHWData;
    private bool propDisposed;

    private void PtrToHWInfoStruct(IntPtr pData, int dataLen)
    {
      int ofs1 = 0;
      this.propEntriesCount = (uint) Marshal.ReadInt32(pData, ofs1);
      int ofs2 = ofs1 + 4;
      this.propDataLength = (uint) Marshal.ReadInt32(pData, ofs2);
      int ofs3 = ofs2 + 4;
      this.propHWPLCFamily = (PlcFamily) Marshal.ReadInt32(pData, ofs3);
      int num = ofs3 + 4;
      this.propHWData = (byte[]) null;
      if (0 < dataLen - num && (long) this.propDataLength == (long) (dataLen - num))
      {
        this.propHWData = new byte[(IntPtr) this.propDataLength];
        for (int index = 0; (long) index < (long) this.propDataLength; ++index)
          this.propHWData[index] = Marshal.ReadByte(pData, num + index);
      }
      else
        this.propHWData = new byte[1];
    }

    internal HardwareInfo(Cpu cpuObj)
    {
      this.propItems = new ArrayList();
      this.propCpu = cpuObj;
      this.propDisposed = false;
      this.propInternID = 0U;
      this.propEntriesCount = 0U;
      this.propDataLength = 0U;
      this.propHWPLCFamily = PlcFamily.None;
      this.propHWData = (byte[]) null;
      this.AddToCBReceivers();
    }

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
      this.Dispose(true);
      GC.SuppressFinalize((object) this);
    }

    internal virtual void Dispose(bool disposing)
    {
      this.RemoveFromCBReceivers();
      if (this.propDisposed)
        return;
      this.OnDisposing(disposing);
      if (!disposing)
        return;
      this.propDisposed = true;
      this.propHWData = (byte[]) null;
      this.propCpu = (Cpu) null;
      this.propItems.Clear();
      this.propItems = (ArrayList) null;
    }

    private void RemoveFromCBReceivers()
    {
      if (this.Service == null)
        return;
      this.Service.RemoveID(this.propInternID);
    }

    private bool AddToCBReceivers() => this.Service != null && this.Service.AddID((object) this, ref this.propInternID);

    public virtual void CopyTo(Array array, int count)
    {
    }

    internal Service Service => this.propCpu != null ? this.propCpu.Service : (Service) null;

    public Base Parent => (Base) this.propCpu;

    internal override void OnPviRead(
      int errorCode,
      PVIReadAccessTypes accessType,
      PVIDataStates dataState,
      IntPtr pData,
      uint dataLen,
      int option)
    {
      if (PVIReadAccessTypes.UploadHardWareInfo == accessType)
      {
        if (errorCode == 0)
          this.PtrToHWInfoStruct(pData, (int) dataLen);
        this.OnUploaded(new PviEventArgs("HardwareUpload", this.propCpu.Address, errorCode, this.Service.Language, (Action) accessType, this.Service));
        if (errorCode == 0)
          return;
        this.OnError(new PviEventArgs("HardwareUpload", this.propCpu.Address, errorCode, this.Service.Language, (Action) accessType, this.Service));
      }
      else
        base.OnPviRead(errorCode, accessType, dataState, pData, dataLen, option);
    }

    [CLSCompliant(false)]
    public uint EntriesCount => this.propEntriesCount;

    [CLSCompliant(false)]
    public uint DataLength => this.propDataLength;

    public PlcFamily PlcFamily => this.propHWPLCFamily;

    public byte[] HardwareInfoData => this.propHWData;

    public event PviEventHandler Error;

    public event PviEventHandler Uploaded;

    protected internal virtual void OnError(PviEventArgs e)
    {
      if (this.Service.ErrorException)
        throw new PviException(e.ErrorText, e.ErrorCode, (object) this, e);
      if (this.Service.ErrorEvent && this.Error != null)
        this.Error((object) this, e);
      this.Service.OnError((object) this, e);
    }

    protected internal virtual void OnUploaded(PviEventArgs e)
    {
      if (this.Uploaded == null)
        return;
      this.Uploaded((object) this, e);
    }

    public int Upload() => this.Service.EventMessageType != EventMessageType.CallBack ? PInvokePvicom.PviComMsgReadArgumentRequest(this.Service.hPvi, this.propCpu.LinkId, AccessTypes.HardwareUpload, IntPtr.Zero, 0, this.Service.WindowHandle, 721U, this.propInternID) : PInvokePvicom.PviComReadArgumentRequest(this.Service.hPvi, this.propCpu.LinkId, AccessTypes.HardwareUpload, IntPtr.Zero, 0, this.Service.cbRead, 4294967294U, this.propInternID);
  }
}
