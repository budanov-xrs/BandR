// Decompiled with JetBrains decompiler
// Type: BR.AN.PviServices.LogBookEntry
// Assembly: BR.AN.PVIServices, Version=1.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DA853F0A-C070-4E30-B7A7-51D955FB85F6
// Assembly location: C:\Users\ss_six\Desktop\FXE\FXE software integration package\COMET_FXE-PVIServices_Example\Pluto-PVI-trial_01\packages\IToolS.BandR.3.5.58\lib\net20\BR.AN.PVIServices.dll

using System;
using System.Runtime.InteropServices;

namespace BR.AN.PviServices
{
  internal class LogBookEntry
  {
    internal LevelType propLevelType;
    internal DateTime propDateTime;
    internal uint propErrorInfo;
    internal uint propErrorNumber;
    internal string propErrorText;
    internal byte[] propBinary;
    internal string propTask;

    internal LogBookEntry(APIFC_RLogbookRes_entry eLog)
    {
      this.propErrorNumber = (uint) eLog.errcode;
      this.propErrorText = eLog.errstring;
      this.propErrorInfo = eLog.errinfo;
      this.propLevelType = (LevelType) eLog.errtyp;
      switch (eLog.errtyp)
      {
        case APIFCerrtyp.APIFC_ET_EXCEPTION:
          this.propLevelType = LevelType.Fatal;
          break;
        case APIFCerrtyp.APIFC_ET_FATAL:
          this.propLevelType = LevelType.Fatal;
          break;
        case APIFCerrtyp.APIFC_ET_WARNING:
          this.propLevelType = LevelType.Warning;
          break;
        case APIFCerrtyp.APIFC_ET_INFO:
          this.propLevelType = LevelType.Info;
          break;
        case (APIFCerrtyp) 4:
          this.propLevelType = LevelType.Debug;
          break;
        default:
          this.propLevelType = LevelType.Warning;
          break;
      }
      this.propTask = this.GetTaskName(eLog);
      this.propBinary = new byte[4];
      if (eLog.errinfo != 0U)
      {
        byte[] destination = new byte[4];
        int[] source = new int[1]{ (int) eLog.errinfo };
        IntPtr hMemory = PviMarshal.AllocHGlobal(4);
        Marshal.Copy(source, 0, hMemory, 1);
        Marshal.Copy(hMemory, destination, 0, 4);
        this.propBinary[0] = destination[3];
        this.propBinary[1] = destination[2];
        this.propBinary[2] = destination[1];
        this.propBinary[3] = destination[0];
        PviMarshal.FreeHGlobal(ref hMemory);
      }
      this.propDateTime = this.T5TimeStampToDT(eLog);
    }

    internal bool EqualsTo(LoggerEntry lEntry)
    {
      if (!(this.propDateTime == lEntry.propDateTime) || (int) this.propErrorNumber != (int) lEntry.propErrorNumber || this.propLevelType != lEntry.propLevelType || (int) this.propErrorInfo != (int) lEntry.propErrorInfo || this.propErrorText.CompareTo(lEntry.ErrorText) != 0 || this.propTask.CompareTo(lEntry.propTask) != 0 || this.propBinary.Length != lEntry.propBinary.Length)
        return false;
      for (int index = 0; index < this.propBinary.Length; ++index)
      {
        if ((int) this.propBinary[index] != (int) lEntry.propBinary[index])
          return false;
      }
      return true;
    }

    private string GetHexRepresentation(uint eInfo) => string.Format((IFormatProvider) new AnyRadix(), "{0:Ra16}", (object) (long) eInfo);

    private DateTime T5TimeStampToDT(APIFC_RLogbookRes_entry eLog)
    {
      if (eLog.timestamp_0 == (byte) 0 && eLog.timestamp_2 == (byte) 0 && eLog.timestamp_3 == (byte) 0 && eLog.timestamp_4 == (byte) 0)
        return new DateTime(0L);
      int num = (int) eLog.timestamp_0 >> 1;
      if (num < 90)
        num += 100;
      int tm_year = num + 1900;
      int tm_mon = (((int) eLog.timestamp_0 & 1) << 3) + ((int) eLog.timestamp_1 >> 5);
      if (tm_mon == 0)
      {
        --tm_year;
        tm_mon = 12;
      }
      int tm_mday = (int) eLog.timestamp_1 & 31;
      int tm_hour = (int) eLog.timestamp_2 >> 3;
      int tm_min = (((int) eLog.timestamp_2 & 7) << 3) + ((int) eLog.timestamp_3 >> 5);
      int tm_sec = (((int) eLog.timestamp_3 & 31) << 1) + ((int) eLog.timestamp_4 >> 7);
      int tm_Msec = (int) eLog.timestamp_4 & (int) sbyte.MaxValue;
      return Pvi.ToDateTime(tm_year, tm_mon, tm_mday, tm_hour, tm_min, tm_sec, tm_Msec);
    }

    private string GetTaskName(APIFC_RLogbookRes_entry eInfo)
    {
      string taskName = "";
      if (eInfo.errtask_0 != char.MinValue)
      {
        if (eInfo.errtask_1 == char.MinValue)
          taskName = string.Format("{0}", (object) eInfo.errtask_0);
        else if (eInfo.errtask_2 == char.MinValue)
          taskName = string.Format("{0}{1}", (object) eInfo.errtask_0, (object) eInfo.errtask_1);
        else if (eInfo.errtask_3 == char.MinValue)
          taskName = string.Format("{0}{1}{2}", (object) eInfo.errtask_0, (object) eInfo.errtask_1, (object) eInfo.errtask_2);
        else
          taskName = string.Format("{0}{1}{2}{3}", (object) eInfo.errtask_0, (object) eInfo.errtask_1, (object) eInfo.errtask_2, (object) eInfo.errtask_3);
      }
      return taskName;
    }

    public override string ToString()
    {
      string str = "";
      for (int index = 0; index < this.propBinary.GetLength(0); ++index)
        str += string.Format("{0:X2} ", this.propBinary.GetValue(index));
      return "LevelType=\"" + this.propLevelType.ToString() + "\" DateTime=\"" + this.propDateTime.ToString("s") + "\" ErrorNumber=\"" + this.propErrorNumber.ToString() + "\" Errorinfo=\"0x" + string.Format("{0:X8}", (object) this.propErrorInfo) + "\" Binary=\"0x" + str + "\" ErrorText=\"" + this.propErrorText + "\" Task=\"" + this.propTask + "\"";
    }

    internal void Dump()
    {
      string str = "    " + string.Format("{0:X2} {1:X2} {2:X2} {3:X2}", this.propBinary.GetValue(0), this.propBinary.GetValue(1), this.propBinary.GetValue(2), this.propBinary.GetValue(3));
    }
  }
}
