// Decompiled with JetBrains decompiler
// Type: BR.AN.PviServices.ErrorLogBook
// Assembly: BR.AN.PVIServices, Version=1.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DA853F0A-C070-4E30-B7A7-51D955FB85F6
// Assembly location: C:\Users\ss_six\Desktop\FXE\FXE software integration package\COMET_FXE-PVIServices_Example\Pluto-PVI-trial_01\packages\IToolS.BandR.3.5.58\lib\net20\BR.AN.PVIServices.dll

using System;
using System.ComponentModel;
using System.IO;
using System.Runtime.InteropServices;

namespace BR.AN.PviServices
{
  public class ErrorLogBook : Logger
  {
    internal const int PT_ERROR_MASK = 127;
    internal const int PT_ERROR_INFO = 3;
    internal const string AR_V_ERROR_LOGBOOK = "A2850";
    public const string KW_LOGBOOK_NAME = "$LOG285$";
    private string errFileName;

    public ErrorLogBook(Cpu cpu)
      : base(cpu, "$LOG285$")
    {
      this.errFileName = (string) null;
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
      if (accessType == PVIReadAccessTypes.ReadErrorLogBook)
      {
        if (this.errFileName == null)
        {
          this.propReadRequestActive = false;
          if (this.ErrorCode == 0)
          {
            LoggerEntryCollection logBookEntries = this.GetLogBookEntries(pData, dataLen);
            this.OnEntriesRead(new LoggerEventArgs(this.Name, this.Address, errorCode, this.Service.Language, Action.ReadError, logBookEntries));
          }
          else
          {
            this.OnEntriesRead(new LoggerEventArgs(this.Name, this.Address, errorCode, this.Service.Language, Action.ReadError, new LoggerEntryCollection("EventEntries")));
            this.OnError(new PviEventArgs(this.Name, this.Address, errorCode, this.Service.Language, Action.ReadError, this.Service));
          }
        }
        else
        {
          FileStream fileStream = new FileStream(this.errFileName, FileMode.Create, FileAccess.Write);
          byte[] numArray = new byte[(IntPtr) (dataLen + 11U)];
          numArray[0] = (byte) 1;
          numArray[1] = (byte) 50;
          numArray[2] = (byte) 102;
          numArray[3] = (byte) 94;
          for (int index = 0; index < this.Cpu.RuntimeVersion.Length && index <= 6; ++index)
            numArray[4 + index] = System.Convert.ToByte(this.Cpu.RuntimeVersion[index]);
          if (dataLen < (uint) int.MaxValue)
          {
            Marshal.Copy(pData, numArray, 11, (int) dataLen);
            fileStream.Write(numArray, 0, (int) dataLen + 11);
            fileStream.Close();
          }
          this.OnEntriesRead(new LoggerEventArgs(this.Name, this.Address, errorCode, this.Service.Language, Action.ReadErrorToFile, new LoggerEntryCollection("EventEntries")));
        }
      }
      else
        base.OnPviRead(errorCode, accessType, dataState, pData, dataLen, option);
    }

    private void InsertSysLogBookEntries(
      APIFC_RLogbookRes_entry[] lbEntries,
      int itemCnt,
      LoggerEntryCollection eventEntries)
    {
      LoggerEntry entry = (LoggerEntry) null;
      this.LoggerEntries.propContentVersion = 0U;
      for (int index = itemCnt - 1; index > -1; --index)
      {
        LogBookEntry logBookEntry = new LogBookEntry(lbEntries[index]);
        if (logBookEntry.propErrorNumber != 0U || logBookEntry.propTask.Length != 0 || logBookEntry.propErrorInfo != 0U || logBookEntry.propErrorText.Length != 0)
        {
          if (entry != null && LevelType.Info == logBookEntry.propLevelType && (int) entry.propErrorNumber == (int) logBookEntry.propErrorNumber)
            entry.AppendSGxErrorInfo(logBookEntry, this.propCpu.IsSG4Target);
          else if (LevelType.Info != logBookEntry.propLevelType)
          {
            entry = new LoggerEntry(this, logBookEntry, itemCnt - index, true, false);
            entry.UpdateForSGx(logBookEntry, this.propCpu.IsSG4Target);
            this.LoggerEntries.Add((LoggerEntryBase) entry);
            eventEntries.Add((LoggerEntryBase) entry, true);
          }
          else
            entry = (LoggerEntry) null;
        }
      }
    }

    private bool IsNewEntry(LogBookEntry lbEntry, int eventEntryCnt)
    {
      if (eventEntryCnt == this.LoggerEntries.Count)
      {
        for (int index = 0; index < this.LoggerEntries.Count; ++index)
        {
          if (lbEntry.EqualsTo(this.LoggerEntries[index]))
            return false;
        }
      }
      return true;
    }

    private LoggerEntryCollection GetLogBookEntries(IntPtr pData, uint dataLen)
    {
      int num = Marshal.SizeOf(typeof (APIFC_RLogbookRes_entry));
      int itemCnt = (int) ((long) dataLen / (long) num);
      APIFC_RLogbookRes_entry[] lbEntries = new APIFC_RLogbookRes_entry[itemCnt];
      LoggerEntryCollection eventEntries = new LoggerEntryCollection("EventEntries");
      this.LoggerEntries.Clear();
      for (int index = 0; index < itemCnt; ++index)
      {
        int ptr = (int) pData + index * num;
        lbEntries[index] = (APIFC_RLogbookRes_entry) Marshal.PtrToStructure((IntPtr) ptr, typeof (APIFC_RLogbookRes_entry));
      }
      this.InsertSysLogBookEntries(lbEntries, itemCnt, eventEntries);
      return eventEntries;
    }

    public string Load(string fileName)
    {
      string str = (string) null;
      try
      {
        FileStream fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read);
        int length = (int) fileStream.Length;
        byte[] numArray = new byte[length];
        fileStream.Read(numArray, 0, length);
        IntPtr hMemory = PviMarshal.AllocHGlobal(length);
        if ((byte) 1 == numArray[0] && (byte) 50 == numArray[1] && (byte) 102 == numArray[2] && (byte) 94 == numArray[3])
        {
          PviMarshal.ToAnsiString(numArray, 4, 7);
          Marshal.Copy(numArray, 11, hMemory, length - 11);
        }
        else
          Marshal.Copy(numArray, 0, hMemory, length);
        fileStream.Close();
        this.OnEntriesRead(new LoggerEventArgs(this.Name, this.Address, 0, this.Service.Language, Action.ReadError, this.GetLogBookEntries(hMemory, (uint) length)));
        PviMarshal.FreeHGlobal(ref hMemory);
      }
      catch (System.Exception ex)
      {
        str = ex.Message;
      }
      return str;
    }

    public void Read(string fileName)
    {
      int dataLen = 4;
      int[] source = new int[1]{ 0 };
      IntPtr hMemory = PviMarshal.AllocCoTaskMem(4);
      Marshal.Copy(source, 0, hMemory, 1);
      this.errFileName = fileName;
      this.propReadRequestActive = true;
      int errorCode = this.ReadArgumentRequest(this.Service.hPvi, (uint) this.CpuLinkId, AccessTypes.ReadError, hMemory, dataLen, 270U);
      if (errorCode != 0)
        this.OnError(new PviEventArgs(this.Name, this.Address, errorCode, this.Service.Language, Action.ReadErrorToFile, this.Service));
      PviMarshal.FreeHGlobal(ref hMemory);
    }

    public override void Read()
    {
      this.propReturnValue = 0;
      int dataLen = 4;
      int[] source = new int[1]{ 0 };
      IntPtr hMemory = PviMarshal.AllocCoTaskMem(4);
      Marshal.Copy(source, 0, hMemory, 1);
      this.errFileName = (string) null;
      this.propReadRequestActive = true;
      this.propReturnValue = this.ReadArgumentRequest(this.Service.hPvi, (uint) this.CpuLinkId, AccessTypes.ReadError, hMemory, dataLen, 269U);
      if (this.propReturnValue != 0)
        this.OnError(new PviEventArgs(this.Name, this.Address, this.propReturnValue, this.Service.Language, Action.ReadError, this.Service));
      PviMarshal.FreeHGlobal(ref hMemory);
    }

    public override void Read(int count)
    {
      this.propReturnValue = 0;
      int dataLen = 4;
      int[] source = new int[1]{ count };
      IntPtr hMemory = PviMarshal.AllocCoTaskMem(4);
      Marshal.Copy(source, 0, hMemory, 1);
      this.propReadRequestActive = true;
      this.errFileName = (string) null;
      this.propReturnValue = this.ReadArgumentRequest(this.Service.hPvi, (uint) this.CpuLinkId, AccessTypes.ReadError, hMemory, dataLen, 269U);
      if (this.propReturnValue != 0)
        this.OnError(new PviEventArgs(this.Name, this.Address, this.propReturnValue, this.Service.Language, Action.ReadError, this.Service));
      PviMarshal.FreeHGlobal(ref hMemory);
    }

    internal override int Read(int count, int id, Action action)
    {
      this.propReturnValue = 0;
      this.errFileName = (string) null;
      return this.propReturnValue;
    }

    internal override void ReadEntry(int id)
    {
      this.propReturnValue = 0;
      this.errFileName = (string) null;
    }

    internal override void ReadIndex(Action action)
    {
      this.errFileName = (string) null;
      this.propReturnValue = 0;
    }

    internal override int ReadModuleInfo() => 0;

    public int CpuLinkId => this.propParent is Cpu ? (int) this.propParent.LinkId : 0;

    public override void Clear() => this.propReturnValue = 0;

    public override void Connect()
    {
      PviEventArgs e = new PviEventArgs(this.Name, this.Address, 0, this.Service.Language, Action.ConnectedEvent, this.Service);
      this.propReturnValue = 0;
      this.Fire_Connected((object) this, e);
    }

    public override void Connect(ConnectionType connectionType)
    {
      PviEventArgs e = new PviEventArgs(this.Name, this.Address, 0, this.Service.Language, Action.ConnectedEvent, this.Service);
      this.propReturnValue = 0;
      this.Fire_Connected((object) this, e);
    }

    public override void Delete()
    {
      this.propReturnValue = 0;
      this.OnDeleted(new PviEventArgs(this.Name, this.Address, 0, this.Service.Language, Action.LoggerDelete, this.Service));
    }

    public override void Disconnect()
    {
      this.propConnectionState = ConnectionStates.Disconnecting;
      this.propReturnValue = 0;
      this.OnDisconnected(new PviEventArgs(this.Name, this.Address, 4803, this.Service.Language, Action.LoggerDisconnect, this.Service));
    }

    [Obsolete("This method is no longer supported by ANSL!(Only valid for INA2000)")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Browsable(false)]
    public override void Start()
    {
      this.propReturnValue = 0;
      this.OnStarted(new PviEventArgs(this.Name, this.Address, 4803, this.Service.Language, Action.ModuleStart, this.Service));
    }

    public override void Download(MemoryType memoryType, InstallMode installMode)
    {
      this.propReturnValue = 0;
      this.OnDownloaded(new PviEventArgs(this.Name, this.Address, 4803, this.Service.Language, Action.ModuleDownload, this.Service));
    }

    public override void Download(MemoryType memoryType, InstallMode installMode, string fileName)
    {
      this.propReturnValue = 0;
      this.OnDownloaded(new PviEventArgs(this.Name, this.Address, 4803, this.Service.Language, Action.ModuleDownload, this.Service));
    }

    public override void Remove() => base.Remove();

    [Browsable(false)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete("This method is no longer supported by ANSL!(Only valid for INA2000)")]
    public override void Stop()
    {
      this.propReturnValue = 0;
      this.OnStopped(new PviEventArgs(this.Name, this.Address, 4803, this.Service.Language, Action.ModuleStop, this.Service));
    }

    public override void Upload(string fileName)
    {
      this.propReturnValue = 0;
      this.OnUploaded(new PviEventArgs(this.Name, this.Address, 4803, this.Service.Language, Action.ModuleUpload, this.Service));
    }
  }
}
