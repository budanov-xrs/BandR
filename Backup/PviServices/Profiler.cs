// Decompiled with JetBrains decompiler
// Type: BR.AN.PviServices.Profiler
// Assembly: BR.AN.PVIServices, Version=1.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DA853F0A-C070-4E30-B7A7-51D955FB85F6
// Assembly location: C:\Users\ss_six\Desktop\FXE\FXE software integration package\COMET_FXE-PVIServices_Example\Pluto-PVI-trial_01\packages\IToolS.BandR.3.5.58\lib\net20\BR.AN.PVIServices.dll

using System;
using System.Runtime.InteropServices;

namespace BR.AN.PviServices
{
  public class Profiler : PviCBEvents, IDisposable
  {
    private const uint TTServiceProfiler = 65025;
    private const uint TTServiceDataCode = 3;
    private const int TTServiceWrite = 1;
    private const int TTServiceRead = 2;
    private const string definitionModule = "prfmod$e";
    private string propName;
    private Cpu propParent;
    private ProfilerState propState;
    private uint propInternId;
    private int propErrorCode;
    private bool propCommandActive;
    internal bool propDisposed;

    public Profiler(Cpu cpu, string name)
    {
      this.propDisposed = false;
      this.propParent = cpu;
      cpu.propProfiler = cpu.propProfiler == null ? this : throw new InvalidOperationException();
      this.propName = name;
      this.propCommandActive = false;
      this.AddToCBReceivers();
    }

    private void RemoveFromCBReceivers()
    {
      if (this.Cpu == null || this.Cpu.Service == null)
        return;
      this.Cpu.Service.RemoveID(this.propInternId);
    }

    private bool AddToCBReceivers() => this.Cpu != null && this.Cpu.Service != null && this.Cpu.Service.AddID((object) this, ref this.propInternId);

    public void Install()
    {
      byte[] data = new byte[11];
      data[0] = (byte) 5;
      char[] charArray = "prfmod$e".ToCharArray();
      for (int index = 0; index < charArray.Length; ++index)
        data[1 + index] = (byte) charArray[index];
      this.TTS_Write(data, (byte) data.Length, 1003);
      this.propCommandActive = true;
    }

    public void Deinstall()
    {
      byte[] data = new byte[11];
      data[0] = (byte) 2;
      char[] charArray = "prfmod$e".ToCharArray();
      for (int index = 0; index < charArray.Length; ++index)
        data[1 + index] = (byte) charArray[index];
      this.TTS_Write(data, (byte) data.Length, 1004);
      this.propCommandActive = true;
    }

    public void Start()
    {
      byte[] data = new byte[11];
      data[0] = (byte) 6;
      char[] charArray = "prfmod$e".ToCharArray();
      for (int index = 0; index < charArray.Length; ++index)
        data[1 + index] = (byte) charArray[index];
      this.TTS_Write(data, (byte) data.Length, 1001);
      this.propCommandActive = true;
    }

    public void ExtendedStart()
    {
      byte[] data = new byte[11];
      data[0] = (byte) 0;
      char[] charArray = "prfmod$e".ToCharArray();
      for (int index = 0; index < charArray.Length; ++index)
        data[1 + index] = (byte) charArray[index];
      this.TTS_Write(data, (byte) data.Length, 1000);
      this.propCommandActive = true;
    }

    public void Stop()
    {
      byte[] data = new byte[11];
      data[0] = (byte) 1;
      char[] charArray = "prfmod$e".ToCharArray();
      for (int index = 0; index < charArray.Length; ++index)
        data[1 + index] = (byte) charArray[index];
      this.TTS_Write(data, (byte) data.Length, 1002);
      this.propCommandActive = true;
    }

    public void InstallDefault()
    {
      byte[] data = new byte[11];
      data[0] = (byte) 7;
      char[] charArray = "prfmod$e".ToCharArray();
      for (int index = 0; index < charArray.Length; ++index)
        data[1 + index] = (byte) charArray[index];
      this.TTS_Write(data, (byte) data.Length, 1003);
      this.propCommandActive = true;
    }

    public void ReadInfo()
    {
      byte[] data = new byte[11];
      data[0] = (byte) 4;
      char[] charArray = "prfmod$e".ToCharArray();
      for (int index = 0; index < charArray.Length; ++index)
        data[1 + index] = (byte) charArray[index];
      this.TTS_Write(data, (byte) data.Length, 1006);
      this.propCommandActive = true;
    }

    public void ReadStack()
    {
      byte[] data = new byte[11];
      data[0] = (byte) 4;
      char[] charArray = "prfmod$e".ToCharArray();
      for (int index = 0; index < charArray.Length; ++index)
        data[1 + index] = (byte) charArray[index];
      this.TTS_Write(data, (byte) data.Length, 1005);
      this.propCommandActive = true;
    }

    public void ReadState()
    {
      byte[] data = new byte[11];
      this.TTS_Read(data, (byte) data.Length, 1008);
      this.propCommandActive = true;
    }

    protected internal virtual int TTS_Read(byte[] data, byte dataLength, int respParam)
    {
      int num = 65025;
      byte[] source = new byte[(int) dataLength + 5];
      source[0] = (byte) (num & (int) byte.MaxValue);
      source[1] = (byte) ((num & 65280) >> 8);
      source[2] = (byte) 2;
      source[3] = (byte) 3;
      source[4] = dataLength;
      for (int index = 0; index < (int) dataLength; ++index)
        source[5 + index] = data[index];
      IntPtr hMemory = PviMarshal.AllocHGlobal((IntPtr) source.Length);
      Marshal.Copy(source, 0, hMemory, source.Length);
      int info = this.Cpu.Service.EventMessageType != EventMessageType.CallBack ? PInvokePvicom.PviComMsgReadArgumentRequest(this.Cpu.Service.hPvi, this.Cpu.LinkId, AccessTypes.TTService, hMemory, source.Length, this.Cpu.Service.WindowHandle, (uint) respParam, this.InternId) : PInvokePvicom.PviComReadArgumentRequest(this.Cpu.Service.hPvi, this.Cpu.LinkId, AccessTypes.TTService, hMemory, source.Length, this.Cpu.Service.cbRead, 4294967294U, this.InternId);
      PviMarshal.FreeHGlobal(ref hMemory);
      if (info != 0)
        this.OnError(new ProfilerEventArgs(this.Name, (Action) respParam, (object) info));
      return info;
    }

    protected internal virtual int TTS_Write(byte[] data, byte dataLength, int respParam)
    {
      int num = 65025;
      byte[] source = new byte[(int) dataLength + 5];
      source[0] = (byte) (num & (int) byte.MaxValue);
      source[1] = (byte) ((num & 65280) >> 8);
      source[2] = (byte) 1;
      source[3] = (byte) 3;
      source[4] = dataLength;
      for (int index = 0; index < (int) dataLength; ++index)
        source[5 + index] = data[index];
      IntPtr hMemory = PviMarshal.AllocHGlobal((IntPtr) source.Length);
      Marshal.Copy(source, 0, hMemory, source.Length);
      int info;
      if (this.Cpu.Service.EventMessageType == EventMessageType.CallBack)
      {
        switch (respParam)
        {
          case 1000:
            info = PInvokePvicom.PviComReadArgumentRequest(this.Cpu.Service.hPvi, this.Cpu.LinkId, AccessTypes.TTService, hMemory, source.Length, this.Cpu.Service.cbReadD, 4294967294U, this.InternId);
            break;
          case 1001:
            info = PInvokePvicom.PviComReadArgumentRequest(this.Cpu.Service.hPvi, this.Cpu.LinkId, AccessTypes.TTService, hMemory, source.Length, this.Cpu.Service.cbReadC, 4294967294U, this.InternId);
            break;
          case 1002:
            info = PInvokePvicom.PviComReadArgumentRequest(this.Cpu.Service.hPvi, this.Cpu.LinkId, AccessTypes.TTService, hMemory, source.Length, this.Cpu.Service.cbReadE, 4294967294U, this.InternId);
            break;
          case 1003:
            info = PInvokePvicom.PviComReadArgumentRequest(this.Cpu.Service.hPvi, this.Cpu.LinkId, AccessTypes.TTService, hMemory, source.Length, this.Cpu.Service.cbReadA, 4294967294U, this.InternId);
            break;
          case 1004:
            info = PInvokePvicom.PviComReadArgumentRequest(this.Cpu.Service.hPvi, this.Cpu.LinkId, AccessTypes.TTService, hMemory, source.Length, this.Cpu.Service.cbReadB, 4294967294U, this.InternId);
            break;
          case 1005:
            info = PInvokePvicom.PviComReadArgumentRequest(this.Cpu.Service.hPvi, this.Cpu.LinkId, AccessTypes.TTService, hMemory, source.Length, this.Cpu.Service.cbReadG, 4294967294U, this.InternId);
            break;
          case 1006:
            info = PInvokePvicom.PviComReadArgumentRequest(this.Cpu.Service.hPvi, this.Cpu.LinkId, AccessTypes.TTService, hMemory, source.Length, this.Cpu.Service.cbReadF, 4294967294U, this.InternId);
            break;
          default:
            info = PInvokePvicom.PviComReadArgumentRequest(this.Cpu.Service.hPvi, this.Cpu.LinkId, AccessTypes.TTService, hMemory, source.Length, this.Cpu.Service.cbRead, 4294967294U, this.InternId);
            break;
        }
      }
      else
        info = PInvokePvicom.PviComMsgReadArgumentRequest(this.Cpu.Service.hPvi, this.Cpu.LinkId, AccessTypes.TTService, hMemory, source.Length, this.Cpu.Service.WindowHandle, (uint) respParam, this.InternId);
      PviMarshal.FreeHGlobal(ref hMemory);
      if (info != 0)
        this.OnError(new ProfilerEventArgs(this.Name, (Action) respParam, (object) info));
      return info;
    }

    internal override void OnPviCreated(int errorCode, uint linkID) => base.OnPviCreated(errorCode, linkID);

    internal override void OnPviLinked(int errorCode, uint linkID, int option) => base.OnPviLinked(errorCode, linkID, option);

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

    internal override void OnPviWritten(
      int errorCode,
      PVIWriteAccessTypes accessType,
      PVIDataStates dataState,
      int option,
      IntPtr pData,
      uint dataLen)
    {
      this.propErrorCode = errorCode;
      if (accessType == PVIWriteAccessTypes.TTService)
        return;
      base.OnPviWritten(errorCode, accessType, dataState, option, pData, dataLen);
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
      TTResponse ttResponse;
      ttResponse.dataLen = (byte) 0;
      ttResponse.profilerError = (byte) 0;
      ttResponse.profilerState = (byte) 0;
      ttResponse.serviceGroup = (ushort) 0;
      ttResponse.serviceID = (byte) 0;
      if (accessType == PVIReadAccessTypes.TTService)
      {
        this.propCommandActive = false;
        if (pData == IntPtr.Zero)
          return;
        if (errorCode == 0)
        {
          TTResponse structure = (TTResponse) Marshal.PtrToStructure(pData, typeof (TTResponse));
          switch (option)
          {
            case 1:
              this.OnInstalled(new ProfilerEventArgs(this.propName, Action.ProfilerInstall, (object) (ushort) ((uint) structure.profilerState + (uint) (ushort) ((uint) structure.profilerError << 8))));
              break;
            case 2:
              this.OnDeinstalled(new ProfilerEventArgs(this.propName, Action.ProfilerDeinstall, (object) (ushort) ((uint) structure.profilerState + (uint) (ushort) ((uint) structure.profilerError << 8))));
              break;
            case 3:
              this.OnStarted(new ProfilerEventArgs(this.propName, Action.ProfilerStart, (object) (ushort) ((uint) structure.profilerState + (uint) (ushort) ((uint) structure.profilerError << 8))));
              break;
            case 4:
              this.OnStarted(new ProfilerEventArgs(this.propName, Action.ProfilerExtendedStart, (object) (ushort) ((uint) structure.profilerState + (uint) (ushort) ((uint) structure.profilerError << 8))));
              break;
            case 5:
              this.OnStopped(new ProfilerEventArgs(this.propName, Action.ProfilerStop, (object) (ushort) ((uint) structure.profilerState + (uint) (ushort) ((uint) structure.profilerError << 8))));
              break;
            case 6:
              ushort info = (ushort) ((uint) structure.profilerState + (uint) (ushort) ((uint) structure.profilerError << 8));
              if (info != (ushort) 0)
              {
                this.OnError(new ProfilerEventArgs(this.propName, Action.ProfilerGetInfo, (object) info));
                break;
              }
              byte[] destination = new byte[70];
              Marshal.Copy(pData, destination, 0, 70);
              string empty = string.Empty;
              for (int index = 6; index < 70 && destination[index] != (byte) 0; ++index)
                empty += ((char) destination[index]).ToString();
              this.OnInfoRead(new ProfilerEventArgs(this.propName, Action.ProfilerGetInfo, (object) empty));
              break;
            case 7:
              this.OnStackRead(new ProfilerEventArgs(this.propName, Action.ProfilerGetStack, (object) (ushort) ((uint) structure.profilerState + (uint) (ushort) ((uint) structure.profilerError << 8))));
              break;
            default:
              this.propState = (ProfilerState) structure.profilerState;
              this.OnStateRead(new ProfilerEventArgs(this.propName, Action.ProfilerReadState, (object) structure.profilerError));
              break;
          }
        }
        else
        {
          switch (option)
          {
            case 1:
              this.OnError(new ProfilerEventArgs(this.propName, Action.ProfilerInstall, (object) errorCode));
              break;
            case 2:
              this.OnError(new ProfilerEventArgs(this.propName, Action.ProfilerDeinstall, (object) errorCode));
              break;
            case 3:
              this.OnError(new ProfilerEventArgs(this.propName, Action.ProfilerStart, (object) errorCode));
              break;
            case 4:
              this.OnError(new ProfilerEventArgs(this.propName, Action.ProfilerExtendedStart, (object) errorCode));
              break;
            case 5:
              this.OnError(new ProfilerEventArgs(this.propName, Action.ProfilerStop, (object) errorCode));
              break;
            case 6:
              this.OnError(new ProfilerEventArgs(this.propName, Action.ProfilerGetInfo, (object) errorCode));
              break;
            case 7:
              this.OnError(new ProfilerEventArgs(this.propName, Action.ProfilerGetStack, (object) errorCode));
              break;
            default:
              this.OnError(new ProfilerEventArgs(this.propName, Action.ProfilerReadState, (object) errorCode));
              break;
          }
        }
      }
      else
        base.OnPviRead(errorCode, accessType, dataState, pData, dataLen, option);
    }

    protected virtual void OnStateRead(ProfilerEventArgs e)
    {
      if (this.StateRead == null)
        return;
      this.StateRead((object) this, e);
    }

    protected virtual void OnInfoRead(ProfilerEventArgs e)
    {
      if (this.InfoRead == null)
        return;
      this.InfoRead((object) this, e);
    }

    protected virtual void OnInstalled(ProfilerEventArgs e)
    {
      if (this.Installed == null)
        return;
      this.Installed((object) this, e);
    }

    protected virtual void OnDeinstalled(ProfilerEventArgs e)
    {
      if (this.Deinstalled == null)
        return;
      this.Deinstalled((object) this, e);
    }

    protected virtual void OnStarted(ProfilerEventArgs e)
    {
      if (this.Started == null)
        return;
      this.Started((object) this, e);
    }

    protected virtual void OnStopped(ProfilerEventArgs e)
    {
      if (this.Stopped == null)
        return;
      this.Stopped((object) this, e);
    }

    protected virtual void OnStackRead(ProfilerEventArgs e)
    {
      if (this.StackRead == null)
        return;
      this.StackRead((object) this, e);
    }

    protected virtual void OnError(ProfilerEventArgs e)
    {
      if (this.Error == null)
        return;
      this.Error((object) this, e);
    }

    public string Name => this.propName;

    public Cpu Cpu => this.propParent;

    public int ErrorNumber => this.propErrorCode;

    internal uint InternId => this.propInternId;

    public ProfilerState State => this.propState;

    public bool CommandActive => this.propCommandActive;

    public event ProfilerEventHandler StateRead;

    public event ProfilerEventHandler InfoRead;

    public event ProfilerEventHandler Installed;

    public event ProfilerEventHandler Deinstalled;

    public event ProfilerEventHandler Started;

    public event ProfilerEventHandler Stopped;

    public event ProfilerEventHandler StackRead;

    public event ProfilerEventHandler Error;

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
      this.propName = (string) null;
      this.propParent = (Cpu) null;
    }
  }
}
