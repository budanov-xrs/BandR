// Decompiled with JetBrains decompiler
// Type: BR.AN.PviServices.LoggerEntry
// Assembly: BR.AN.PVIServices, Version=1.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DA853F0A-C070-4E30-B7A7-51D955FB85F6
// Assembly location: C:\Users\ss_six\Desktop\FXE\FXE software integration package\COMET_FXE-PVIServices_Example\Pluto-PVI-trial_01\packages\IToolS.BandR.3.5.58\lib\net20\BR.AN.PVIServices.dll

using System;
using System.Runtime.InteropServices;

namespace BR.AN.PviServices
{
  public class LoggerEntry : LoggerEntryBase
  {
    private int extendedInfo;
    private bool propARlessA2850;
    internal int _AdditionalDataFormat;
    internal uint _RecordId;
    internal int propOriginRecordId;
    internal object propEventData;
    internal EventDataTypes propEventDataType;

    [CLSCompliant(false)]
    public LoggerEntry(object parent, DateTime dateTime)
      : base(parent, dateTime)
    {
      this._RecordId = 0U;
      this._AdditionalDataFormat = 0;
      this.propOriginRecordId = 0;
      this.extendedInfo = 0;
      this.propEventId = 0;
      this.propEventData = (object) null;
      this.propEventDataType = EventDataTypes.EmptyEventData;
      this.propARlessA2850 = false;
    }

    [CLSCompliant(false)]
    public LoggerEntry(object parent, string strDT)
      : base(parent, strDT)
    {
      this._RecordId = 0U;
      this._AdditionalDataFormat = 0;
      this.propOriginRecordId = 0;
      this.extendedInfo = 0;
      this.propEventId = 0;
      this.propEventData = (object) null;
      this.propEventDataType = EventDataTypes.EmptyEventData;
      this.propARlessA2850 = false;
    }

    [CLSCompliant(false)]
    public LoggerEntry(object parent, string loggerName, DateTime dateTime, bool arLess2850)
      : base(parent, dateTime)
    {
      if (loggerName != null)
        this.propLoggerModuleName = loggerName;
      this._RecordId = 0U;
      this._AdditionalDataFormat = 0;
      this.propOriginRecordId = 0;
      this.extendedInfo = 0;
      this.propEventId = 0;
      this.propEventData = (object) null;
      this.propEventDataType = EventDataTypes.EmptyEventData;
      this.propARlessA2850 = arLess2850;
    }

    internal LoggerEntry()
    {
      this._RecordId = 0U;
      this._AdditionalDataFormat = 0;
      this.propOriginRecordId = 0;
      this.extendedInfo = 0;
      this.propEventId = 0;
      this.propEventData = (object) null;
      this.propEventDataType = EventDataTypes.EmptyEventData;
      this.propARlessA2850 = false;
    }

    internal LoggerEntry(string arV, LogBookEntry eLog)
      : base(eLog.propDateTime)
    {
      this._RecordId = 0U;
      this._AdditionalDataFormat = 0;
      this.propOriginRecordId = 0;
      this.extendedInfo = 0;
      this.propEventId = 0;
      this.propEventData = (object) null;
      this.propEventDataType = EventDataTypes.EmptyEventData;
      this.propARlessA2850 = true;
      this.propErrorNumber = eLog.propErrorNumber;
      this.propEventId = 0;
      this.propErrorText = eLog.propErrorText;
      this.propErrorInfo = eLog.propErrorInfo;
      this.propLevelType = eLog.propLevelType;
      this.propTask = eLog.propTask;
      this.propBinary = (byte[]) eLog.propBinary.Clone();
      this.propException = new Exception();
      this.propException.propBacktrace = new Backtrace();
      this.propException.propProcessorData = new ProcessorData();
      this.propException.propBacktrace.propPCInfo = new PCInfo();
      this.propException.propARVersion = arV;
      this.propRuntimeVersion = arV;
    }

    internal LoggerEntry(
      ErrorLogBook parent,
      LogBookEntry eLog,
      int internalid,
      bool addKeyOnly,
      bool reverseOrder)
      : base((object) parent, eLog.propDateTime, addKeyOnly, reverseOrder)
    {
      this._RecordId = 0U;
      this._AdditionalDataFormat = 0;
      this.propOriginRecordId = 0;
      this.extendedInfo = 0;
      this.propEventId = 0;
      this.propEventData = (object) null;
      this.propEventDataType = EventDataTypes.EmptyEventData;
      this.propARlessA2850 = true;
      this.propErrorNumber = eLog.propErrorNumber;
      this.propEventId = 0;
      this.propErrorText = eLog.propErrorText;
      this.propErrorInfo = eLog.propErrorInfo;
      this.propLevelType = eLog.propLevelType;
      this.propTask = eLog.propTask;
      this.propBinary = (byte[]) eLog.propBinary.Clone();
      this.propException = (Exception) null;
      this.propRuntimeVersion = parent.propCpu.RuntimeVersion;
      this.propInternID = (uint) internalid;
      this.UpdateUKey();
    }

    internal LoggerEntry(
      Cpu cpu,
      LogBookEntry eLog,
      int internalid,
      bool addKeyOnly,
      bool reverseOrder)
      : base((object) cpu, eLog.propDateTime, addKeyOnly, reverseOrder)
    {
      this._RecordId = 0U;
      this._AdditionalDataFormat = 0;
      this.propOriginRecordId = 0;
      this.extendedInfo = 0;
      this.propEventId = 0;
      this.propEventData = (object) null;
      this.propEventDataType = EventDataTypes.EmptyEventData;
      this.propARlessA2850 = true;
      this.propErrorNumber = eLog.propErrorNumber;
      this.propEventId = 0;
      this.propErrorText = eLog.propErrorText;
      this.propErrorInfo = eLog.propErrorInfo;
      this.propLevelType = eLog.propLevelType;
      this.propTask = eLog.propTask;
      this.propBinary = (byte[]) eLog.propBinary.Clone();
      this.propException = (Exception) null;
      this.propRuntimeVersion = cpu.RuntimeVersion;
      this.propInternID = (uint) internalid;
      this.UpdateUKey();
    }

    internal void UpdateForSGx(LogBookEntry entry, bool sg4)
    {
      if (LevelType.Fatal != entry.propLevelType || !sg4)
        return;
      this.ValidateExcepitonMember();
      if (entry.propTask.CompareTo("PC") == 0)
      {
        this.propException.propProcessorData.propProgramCounter = entry.propErrorInfo;
      }
      else
      {
        this.propException.propBacktrace.propPCInfo.propModuleName = entry.propErrorText;
        this.propException.propBacktrace.propPCInfo.propCodeOffset = entry.propErrorInfo;
      }
    }

    internal void AppendSGxErrorInfo(LogBookEntry entry, bool sg4)
    {
      ++this.extendedInfo;
      if (sg4)
      {
        this.ValidateExcepitonMember();
        this.propException.propBacktrace.propFunctionName = entry.propErrorText;
      }
      else
      {
        switch (this.extendedInfo)
        {
          case 1:
            this.ValidateExcepitonMember();
            if (this.propException.propProcessorData == null)
              this.propException.propProcessorData = new ProcessorData();
            this.propException.propProcessorData.propProgramCounter = entry.propErrorInfo;
            break;
          case 2:
            this.ValidateExcepitonMember();
            uint uint32 = System.Convert.ToUInt32(PviMarshal.LowWord(entry.propErrorInfo));
            if (uint32 == 0U)
            {
              this.propException.propBacktrace.propFunctionName = entry.propErrorText;
              this.propException.propBacktrace.propTaskIdx = System.Convert.ToUInt32(PviMarshal.HighWord(entry.propErrorInfo));
              break;
            }
            this.propException.propBacktrace.propPCInfo.propModuleName = entry.propErrorText;
            this.propException.propBacktrace.propInfo = System.Convert.ToUInt32(PviMarshal.HighWord(entry.propErrorInfo));
            this.propException.propBacktrace.propPCInfo.propCodeOffset = uint32;
            break;
          case 3:
            this.ValidateExcepitonMember();
            this.propException.propBacktrace.propFunctionName = entry.propErrorText;
            this.propException.propBacktrace.propTaskIdx = System.Convert.ToUInt32(PviMarshal.HighWord(entry.propErrorInfo));
            break;
        }
      }
    }

    private void ValidateExcepitonMember()
    {
      if (this.propException != null)
        return;
      this.propException = new Exception();
      this.propException.propBacktrace = new Backtrace();
      this.propException.propProcessorData = new ProcessorData();
      this.propException.propBacktrace.propPCInfo = new PCInfo();
      this.propException.propARVersion = this.propRuntimeVersion;
    }

    private DateTime T5TimeStampToDT(APIFC_RLogbookRes_entry eLog)
    {
      int num = (int) eLog.timestamp_0 >> 1;
      if (num < 90)
        num += 100;
      return Pvi.ToDateTime(num + 1900, (((int) eLog.timestamp_0 & 1) << 3) + ((int) eLog.timestamp_1 >> 5), (int) eLog.timestamp_1 & 31, (int) eLog.timestamp_2 >> 3, (((int) eLog.timestamp_2 & 7) << 3) + ((int) eLog.timestamp_3 >> 5), (((int) eLog.timestamp_3 & 31) << 1) + ((int) eLog.timestamp_4 >> 7), (int) eLog.timestamp_4 & (int) sbyte.MaxValue);
    }

    private string GetTaskName(APIFC_RLogbookRes_entry eInfo)
    {
      string taskName;
      if (eInfo.errtask_1 == char.MinValue)
        taskName = string.Format("{0}", (object) eInfo.errtask_0);
      else if (eInfo.errtask_2 == char.MinValue)
        taskName = string.Format("{0}{1}", (object) eInfo.errtask_0, (object) eInfo.errtask_1);
      else if (eInfo.errtask_3 == char.MinValue)
        taskName = string.Format("{0}{1}{2}", (object) eInfo.errtask_0, (object) eInfo.errtask_1, (object) eInfo.errtask_2);
      else
        taskName = string.Format("{0}{1}{2}{3}", (object) eInfo.errtask_0, (object) eInfo.errtask_1, (object) eInfo.errtask_2, (object) eInfo.errtask_3);
      return taskName;
    }

    public override string ToString() => base.ToString() + " ARlessA2850=\"" + this.propARlessA2850.ToString() + "\" ExtendedInfo=\"0x" + string.Format("{0:X8}", (object) this.extendedInfo) + "\" " + base.ToString();

    internal void Dump()
    {
      string str = "    " + string.Format("{0:X2} {1:X2} {2:X2} {3:X2}", this.propBinary.GetValue(0), this.propBinary.GetValue(1), this.propBinary.GetValue(2), this.propBinary.GetValue(3));
    }

    public bool ARlessA2850 => this.propARlessA2850;

    public int AdditionalDataFormat => this._AdditionalDataFormat;

    public int CustomerCode => this.propCustomerCode;

    public int FacilityCode => this.propFacilityCode;

    public int EventID => this.propEventId;

    public uint RecordId => this._RecordId;

    public int OriginRecordId => this.propOriginRecordId;

    public object EventData => this.propEventData;

    public EventDataTypes EventDataType => this.propEventDataType;

    internal void GetExceptionData()
    {
      int num1 = 0;
      num1 = 0;
      IntPtr hMemory1 = PviMarshal.AllocHGlobal((IntPtr) Marshal.SizeOf(typeof (ExceptionHeader)));
      int ofs1;
      for (ofs1 = 0; ofs1 < Marshal.SizeOf(typeof (ExceptionHeader)); ++ofs1)
        Marshal.WriteByte(hMemory1, ofs1, this.Binary[ofs1]);
      int num2 = ofs1;
      ExceptionHeader structure1 = (ExceptionHeader) Marshal.PtrToStructure(hMemory1, typeof (ExceptionHeader));
      this.propException = new Exception(structure1);
      PviMarshal.FreeHGlobal(ref hMemory1);
      this.propException.propType = ((int) this.propErrorInfo & 2) != 0 ? ExceptionType.System : ExceptionType.Processor;
      if (this.propErrorInfo == 0U)
      {
        if (((int) structure1.options & 2) == 2)
          this.propException.propType = ExceptionType.System;
        else
          this.propException.propType = ExceptionType.Processor;
      }
      IntPtr hMemory2 = PviMarshal.AllocHGlobal((IntPtr) Marshal.SizeOf(typeof (ExceptionProcessorInfo)));
      int ofs2;
      for (ofs2 = 0; ofs2 < Marshal.SizeOf(typeof (ExceptionProcessorInfo)); ++ofs2)
        Marshal.WriteByte(hMemory2, ofs2, this.Binary[ofs2 + num2]);
      int num3 = num2 + ofs2;
      this.Exception.propProcessorData = new ProcessorData((ExceptionProcessorInfo) Marshal.PtrToStructure(hMemory2, typeof (ExceptionProcessorInfo)));
      PviMarshal.FreeHGlobal(ref hMemory2);
      IntPtr hMemory3 = PviMarshal.AllocHGlobal((IntPtr) Marshal.SizeOf(typeof (ExceptionTaskInfo)));
      int ofs3;
      for (ofs3 = 0; ofs3 < Marshal.SizeOf(typeof (ExceptionTaskInfo)); ++ofs3)
        Marshal.WriteByte(hMemory3, ofs3, this.Binary[ofs3 + num3]);
      int num4 = num3 + ofs3;
      this.Exception.propTaskData = new TaskData((ExceptionTaskInfo) Marshal.PtrToStructure(hMemory3, typeof (ExceptionTaskInfo)));
      PviMarshal.FreeHGlobal(ref hMemory3);
      IntPtr hMemory4 = PviMarshal.AllocHGlobal((IntPtr) Marshal.SizeOf(typeof (ExceptionMemoryInfo)));
      int ofs4;
      for (ofs4 = 0; ofs4 < Marshal.SizeOf(typeof (ExceptionMemoryInfo)); ++ofs4)
        Marshal.WriteByte(hMemory4, ofs4, this.Binary[ofs4 + num4]);
      int num5 = num4 + ofs4;
      this.Exception.propMemoryData = new MemoryData((ExceptionMemoryInfo) Marshal.PtrToStructure(hMemory4, typeof (ExceptionMemoryInfo)));
      PviMarshal.FreeHGlobal(ref hMemory4);
      Backtrace backtrace = (Backtrace) null;
      for (int index1 = 0; (long) index1 < (long) this.Exception.BacktraceCount; ++index1)
      {
        IntPtr hMemory5 = PviMarshal.AllocHGlobal((IntPtr) Marshal.SizeOf(typeof (ExceptionTraceRecord)));
        int ofs5;
        for (ofs5 = 0; ofs5 < Marshal.SizeOf(typeof (ExceptionTraceRecord)); ++ofs5)
          Marshal.WriteByte(hMemory5, ofs5, this.Binary[ofs5 + num5]);
        num5 += ofs5;
        ExceptionTraceRecord structure2 = (ExceptionTraceRecord) Marshal.PtrToStructure(hMemory5, typeof (ExceptionTraceRecord));
        PviMarshal.FreeHGlobal(ref hMemory5);
        if (this.Exception.propBacktrace == null)
        {
          this.Exception.propBacktrace = new Backtrace(structure2);
          backtrace = this.Exception.propBacktrace;
        }
        else
        {
          backtrace.propNextBacktrace = new Backtrace(structure2);
          backtrace = backtrace.NextBacktrace;
        }
        if (((int) backtrace.propInfo & 1) != 0)
        {
          IntPtr hMemory6 = PviMarshal.AllocHGlobal((IntPtr) Marshal.SizeOf(typeof (ExceptionBRModuleFunction)));
          int ofs6;
          for (ofs6 = 0; ofs6 < Marshal.SizeOf(typeof (ExceptionBRModuleFunction)); ++ofs6)
            Marshal.WriteByte(hMemory6, ofs6, this.Binary[ofs6 + num5]);
          ExceptionBRModuleFunction structure3 = (ExceptionBRModuleFunction) Marshal.PtrToStructure(hMemory6, typeof (ExceptionBRModuleFunction));
          num5 += ofs6;
          backtrace.propFunctionInfo = new FunctionInfo(structure3);
          PviMarshal.FreeHGlobal(ref hMemory6);
        }
        if (((int) backtrace.propInfo & 4) != 0)
        {
          IntPtr hMemory7 = PviMarshal.AllocHGlobal((IntPtr) Marshal.SizeOf(typeof (ExceptionBRModulePC)));
          int ofs7;
          for (ofs7 = 0; ofs7 < Marshal.SizeOf(typeof (ExceptionBRModulePC)); ++ofs7)
            Marshal.WriteByte(hMemory7, ofs7, this.Binary[ofs7 + num5]);
          ExceptionBRModulePC structure4 = (ExceptionBRModulePC) Marshal.PtrToStructure(hMemory7, typeof (ExceptionBRModulePC));
          num5 += ofs7;
          backtrace.propPCInfo = new PCInfo(structure4);
          PviMarshal.FreeHGlobal(ref hMemory7);
        }
        for (int index2 = 0; (long) index2 < (long) structure2.paramCnt; ++index2)
        {
          IntPtr hMemory8 = PviMarshal.AllocHGlobal((IntPtr) 4);
          for (int ofs8 = 0; ofs8 < 4; ++ofs8)
            Marshal.WriteByte(hMemory8, ofs8, this.Binary[num5++]);
          backtrace.propParameter[index2] = (uint) Marshal.ReadInt32(hMemory8);
          PviMarshal.FreeHGlobal(ref hMemory8);
        }
        if (((int) backtrace.propInfo & 2) != 0)
        {
          IntPtr hMemory9 = PviMarshal.AllocHGlobal((IntPtr) Marshal.SizeOf(typeof (ExceptionBRModuleCallstack)));
          int ofs9;
          for (ofs9 = 0; ofs9 < Marshal.SizeOf(typeof (ExceptionBRModuleCallstack)); ++ofs9)
            Marshal.WriteByte(hMemory9, ofs9, this.Binary[ofs9 + num5]);
          ExceptionBRModuleCallstack structure5 = (ExceptionBRModuleCallstack) Marshal.PtrToStructure(hMemory9, typeof (ExceptionBRModuleCallstack));
          num5 += ofs9;
          backtrace.propCallstack = new Callstack(structure5);
          PviMarshal.FreeHGlobal(ref hMemory9);
        }
      }
    }

    internal override string ToStringHTM(uint contentVersion)
    {
      string str = "<td align=\"right\">";
      string stringHtm = base.ToStringHTM(contentVersion);
      if (contentVersion >= 4112U)
        stringHtm = stringHtm + string.Format("{0}{1}</td>\r\n", (object) str, (object) this.EventID.ToString()) + string.Format("{0}{1}</td>\r\n", (object) str, 0U < this._RecordId ? (object) this._RecordId.ToString() : (object) this.propInternID.ToString()) + string.Format("{0}{1}</td>\r\n", (object) str, (object) this.OriginRecordId.ToString()) + string.Format("{0}{1}</td>\r\n", (object) str, (object) this.CustomerCode.ToString()) + string.Format("{0}{1}</td>\r\n", (object) str, (object) this.FacilityCode.ToString()) + string.Format("{0}{1}</td>\r\n", (object) str, (object) this.AdditionalDataFormat.ToString()) + string.Format("{0}{1}</td>\r\n", (object) str, (object) this.EventDataType.ToString());
      return stringHtm;
    }

    internal override string ToStringCSV(uint contentVersion)
    {
      string stringCsv = base.ToStringCSV(contentVersion);
      if (contentVersion >= 4112U)
      {
        string str1 = 0 >= this.EventID ? stringCsv + ";" : stringCsv + string.Format("\"{0}\";", (object) this.EventID.ToString());
        string str2 = 0U < this.propInternID || 0U < this._RecordId ? str1 + string.Format("\"{0}\";", 0U < this._RecordId ? (object) this._RecordId.ToString() : (object) this.propInternID.ToString()) : str1 + ";";
        string str3 = 0 >= this.OriginRecordId ? str2 + ";" : str2 + string.Format("\"{0}\";", (object) this.OriginRecordId.ToString());
        string str4 = 0 >= this.CustomerCode ? str3 + ";" : str3 + string.Format("\"{0}\";", (object) this.CustomerCode.ToString());
        string str5 = 0 >= this.FacilityCode ? str4 + ";" : str4 + string.Format("\"{0}\";", (object) this.FacilityCode.ToString());
        string str6 = 0 >= this.AdditionalDataFormat ? str5 + ";" : str5 + string.Format("\"{0}\";", (object) this.AdditionalDataFormat.ToString());
        stringCsv = this.EventDataType == EventDataTypes.Undefined || this.EventDataType == EventDataTypes.EmptyEventData ? str6 + ";" : str6 + string.Format("\"{0}\";", (object) this.EventDataType.ToString());
      }
      return stringCsv;
    }
  }
}
