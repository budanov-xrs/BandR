// Decompiled with JetBrains decompiler
// Type: BR.AN.PviServices.Memory
// Assembly: BR.AN.PVIServices, Version=1.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DA853F0A-C070-4E30-B7A7-51D955FB85F6
// Assembly location: C:\Users\ss_six\Desktop\FXE\FXE software integration package\COMET_FXE-PVIServices_Example\Pluto-PVI-trial_01\packages\IToolS.BandR.3.5.58\lib\net20\BR.AN.PVIServices.dll

using System;
using System.Runtime.InteropServices;
using System.Xml;

namespace BR.AN.PviServices
{
  public class Memory : PviCBEvents, IDisposable
  {
    internal const int SET_PVICALLBACK_DATA = -2;
    private Service propService;
    internal string propErrorText = string.Empty;
    internal string propCurLanguage = string.Empty;
    private Cpu propCpu;
    private uint propFlags;
    private MemoryType propType;
    private uint propStartAddress;
    private uint propTotalLen;
    private uint propFreeLen;
    private uint propFreeBlockLen;
    private string propName;
    private string propAddress;
    internal Base propParent;
    internal uint propInternID;
    internal bool propDisposed;
    internal object propUserData;
    internal int propErrorCode;

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
      this.propAddress = (string) null;
      this.propCpu = (Cpu) null;
      this.propName = (string) null;
      this.propParent = (Base) null;
      this.propUserData = (object) null;
    }

    internal Memory(Cpu cpu, APIFC_CPmemInfoRes memory)
    {
      this.propService = cpu.Service;
      this.propDisposed = false;
      this.propCpu = cpu;
      this.propParent = (Base) cpu;
      this.propFlags = (uint) memory.flags;
      this.propType = memory.type;
      this.propStartAddress = memory.start_adr;
      this.propTotalLen = memory.total_len;
      this.propFreeLen = memory.free_len;
      this.propFreeBlockLen = memory.freeblk_len;
      this.propName = this.propType.ToString();
      this.propAddress = this.propName;
      this.AddToCBReceivers();
    }

    internal Memory(Cpu cpu, string name)
    {
      this.propService = cpu.Service;
      this.propDisposed = false;
      this.propCpu = cpu;
      this.propParent = (Base) cpu;
      this.propName = this.propType.ToString();
      this.propAddress = name;
      this.AddToCBReceivers();
    }

    private void RemoveFromCBReceivers()
    {
      if (this.Service == null)
        return;
      this.Service.RemoveID(this.propInternID);
    }

    private bool AddToCBReceivers() => this.Service != null && this.Service.AddID((object) this, ref this.propInternID);

    public bool IsStartAddressValid() => (this.propFlags & 1U) > 0U;

    public bool IsTotalLengthValid() => (this.propFlags & 2U) > 0U;

    public bool IsFreeLengthValid() => (this.propFlags & 4U) > 0U;

    public bool IsFreeBlockLengthValid() => (this.propFlags & 8U) > 0U;

    public void Clean()
    {
      if (this.propCpu.BootMode != BootMode.Diagnostics)
      {
        this.OnError(new PviEventArgs(this.propName, this.propAddress, 4025, this.propCpu.Service.Language, Action.ClearMemory, this.Service));
      }
      else
      {
        int dataLen = Marshal.SizeOf(typeof (int));
        Marshal.WriteInt32(this.Service.RequestBuffer, (int) this.propType);
        int errorCode = this.WriteRequest(this.propCpu.Service.hPvi, this.propCpu.LinkId, AccessTypes.ClearMemory, this.Service.RequestBuffer, dataLen, 618U);
        if (errorCode == 0)
          return;
        this.OnError(new PviEventArgs(this.propName, this.propAddress, errorCode, this.propCpu.Service.Language, Action.ClearMemory, this.Service));
      }
    }

    protected virtual void OnCleaned(PviEventArgs e)
    {
      if (this.Cleaned == null)
        return;
      this.Cleaned((object) this, e);
    }

    protected internal virtual void OnError(PviEventArgs e)
    {
      this.propErrorCode = e.ErrorCode;
      if (this.Service.ErrorException)
        throw new PviException(e.ErrorText, e.ErrorCode, (object) this, e);
      if (this.Service.ErrorEvent && this.Error != null)
        this.Error((object) this, e);
      this.Service.OnError((object) this, e);
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
      if (PVIWriteAccessTypes.ClearMemory == accessType)
      {
        if (this.propErrorCode == 0)
        {
          this.OnCleaned(new PviEventArgs(this.Name, this.Address, this.propErrorCode, this.Service.Language, Action.ClearMemory, this.Service));
        }
        else
        {
          this.OnCleaned(new PviEventArgs(this.Name, this.Address, this.propErrorCode, this.Service.Language, Action.ClearMemory, this.Service));
          this.OnError(new PviEventArgs(this.Name, this.Address, this.propErrorCode, this.Service.Language, Action.ClearMemory, this.Service));
        }
      }
      else
        base.OnPviWritten(errorCode, accessType, dataState, option, pData, dataLen);
    }

    internal int WriteRequest(
      uint hPvi,
      uint linkID,
      AccessTypes nAccess,
      IntPtr pData,
      int dataLen,
      uint respParam)
    {
      return this.Service.EventMessageType != EventMessageType.CallBack ? PInvokePvicom.PviComMsgWriteRequest(hPvi, linkID, nAccess, pData, dataLen, this.Service.WindowHandle, respParam, this.propInternID) : PInvokePvicom.PviComWriteRequest(hPvi, linkID, nAccess, pData, dataLen, this.Service.cbWrite, 4294967294U, this.InternId);
    }

    public int FromXmlTextReader(ref XmlTextReader reader, ConfigurationFlags flags, Memory memory)
    {
      string attribute1 = reader.GetAttribute("Name");
      if (attribute1 != null && attribute1.Length > 0)
        memory.propName = attribute1;
      string attribute2 = reader.GetAttribute("Address");
      if (attribute2 != null && attribute2.Length > 0)
        memory.propAddress = attribute2;
      string attribute3 = reader.GetAttribute("UserData");
      if (attribute3 != null && attribute3.Length > 0)
        memory.propUserData = (object) attribute3;
      string attribute4 = reader.GetAttribute("ErrorCode");
      if (attribute4 != null && attribute4.Length > 0)
      {
        int result = 0;
        if (PviParse.TryParseInt32(attribute4, out result))
          memory.propErrorCode = result;
      }
      string attribute5 = reader.GetAttribute("FreeBlockLength");
      if (attribute5 != null && attribute5.Length > 0)
      {
        uint result = 0;
        if (PviParse.TryParseUInt32(attribute5, out result))
          memory.propFreeBlockLen = result;
      }
      string attribute6 = reader.GetAttribute("FreeLength");
      if (attribute6 != null && attribute6.Length > 0)
      {
        uint result = 0;
        if (PviParse.TryParseUInt32(attribute6, out result))
          memory.propFreeLen = result;
      }
      string attribute7 = reader.GetAttribute("StartAddress");
      if (attribute7 != null && attribute7.Length > 0)
      {
        uint result = 0;
        if (PviParse.TryParseUInt32(attribute7, out result))
          memory.propStartAddress = result;
      }
      string attribute8 = reader.GetAttribute("TotalLength");
      if (attribute8 != null && attribute8.Length > 0)
      {
        uint result = 0;
        if (PviParse.TryParseUInt32(attribute8, out result))
          memory.propTotalLen = result;
      }
      string attribute9 = reader.GetAttribute("Type");
      if (attribute9 != null && attribute9.Length > 0 && attribute9 != null && attribute9.Length > 0)
      {
        switch (attribute9.ToLower())
        {
          case "dram":
            memory.propType = MemoryType.Dram;
            break;
          case "fixram":
            memory.propType = MemoryType.FixRam;
            break;
          case "globalanalog":
            memory.propType = MemoryType.GlobalAnalog;
            break;
          case "globaldigital":
            memory.propType = MemoryType.GlobalDigital;
            break;
          case "oo":
            memory.propType = MemoryType.Io;
            break;
          case "memcard":
            memory.propType = MemoryType.MemCard;
            break;
          case "os":
            memory.propType = MemoryType.Os;
            break;
          case "permanen":
            memory.propType = MemoryType.Permanent;
            break;
          case "systemram":
            memory.propType = MemoryType.SystemRam;
            break;
          case "systemrom":
            memory.propType = MemoryType.SystemRom;
            break;
          case "tmp":
            memory.propType = MemoryType.Tmp;
            break;
          case "userram":
            memory.propType = MemoryType.UserRam;
            break;
          case "userrom":
            memory.propType = MemoryType.UserRom;
            break;
        }
      }
      reader.Read();
      return 0;
    }

    internal virtual int ToXMLTextWriter(ref XmlTextWriter writer, ConfigurationFlags flags)
    {
      writer.WriteStartElement(nameof (Memory));
      if (this.propName != null && this.propName.Length > 0)
        writer.WriteAttributeString("Name", this.propName);
      if (this.propAddress != null && this.propAddress.Length > 0)
        writer.WriteAttributeString("Address", this.propAddress);
      if (this.propUserData is string && this.propUserData != null && ((string) this.propUserData).Length > 0)
        writer.WriteAttributeString("UserData", this.propUserData.ToString());
      if (this.propErrorCode > 0)
        writer.WriteAttributeString("ErrorCode", this.propErrorCode.ToString());
      writer.WriteAttributeString("FreeBlockLength", this.FreeBlockLength.ToString());
      writer.WriteAttributeString("FreeLength", this.FreeLength.ToString());
      writer.WriteAttributeString("StartAddress", this.StartAddress.ToString());
      writer.WriteAttributeString("TotalLength", this.TotalLength.ToString());
      writer.WriteAttributeString("Type", this.Type.ToString());
      writer.WriteEndElement();
      return 0;
    }

    public string Address
    {
      get => this.propAddress;
      set => this.propAddress = value;
    }

    public string Name => this.propName;

    public MemoryType Type => this.propType;

    [Obsolete("This property is no longer supported by ANSL!(Only valid for INA2000)")]
    [CLSCompliant(false)]
    public uint StartAddress => this.propStartAddress < 0U ? 0U : this.propStartAddress;

    [CLSCompliant(false)]
    public uint TotalLength => this.propTotalLen < 0U ? 0U : this.propTotalLen;

    [CLSCompliant(false)]
    public uint FreeLength => this.propFreeLen < 0U ? 0U : this.propFreeLen;

    [CLSCompliant(false)]
    public uint FreeBlockLength => this.propFreeBlockLen < 0U ? 0U : this.propFreeBlockLen;

    public Service Service => this.propService;

    public string FullName => this.Name.Length > 0 ? this.Parent.FullName + "." + this.Name : this.Parent.FullName;

    internal uint LinkId => this.propCpu != null ? this.propCpu.LinkId : 0U;

    internal uint InternId => this.propInternID;

    public object UserData
    {
      get => this.propUserData;
      set => this.propUserData = value;
    }

    public int ErrorCode => this.propErrorCode;

    public string ErrorText
    {
      get
      {
        string str = "en";
        if (this.Service != null)
          str = this.Service.Language;
        if (this.propErrorText == string.Empty)
        {
          this.propCurLanguage = str;
          this.propErrorText = this.Service != null ? this.Service.Utilities.GetErrorText(this.propErrorCode) : Service.GetErrorText(this.propErrorCode, str);
          return this.propErrorText;
        }
        if (this.propCurLanguage.CompareTo(str) == 0)
          return this.propErrorText;
        this.propCurLanguage = str;
        this.propErrorText = this.Service != null ? this.Service.Utilities.GetErrorText(this.propErrorCode) : Service.GetErrorText(this.propErrorCode, str);
        return this.propErrorText;
      }
    }

    public virtual Base Parent => this.propParent;

    public event PviEventHandler Cleaned;

    public event PviEventHandler Error;
  }
}
