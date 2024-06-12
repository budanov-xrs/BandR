// Decompiled with JetBrains decompiler
// Type: BR.AN.PviServices.Pvi
// Assembly: BR.AN.PVIServices, Version=1.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DA853F0A-C070-4E30-B7A7-51D955FB85F6
// Assembly location: C:\Users\ss_six\Desktop\FXE\FXE software integration package\COMET_FXE-PVIServices_Example\Pluto-PVI-trial_01\packages\IToolS.BandR.3.5.58\lib\net20\BR.AN.PVIServices.dll

using System;
using System.Globalization;

namespace BR.AN.PviServices
{
  internal class Pvi
  {
    public const int APIFC_STRLEN = 36;
    public const uint SET_PVICALLBACK_DATA = 4294967294;
    public const string KWPVTYPE_INT8 = "i8";
    public const string KWPVTYPE_Int16 = "i16";
    public const string KWPVTYPE_Int32 = "i32";
    public const string KWPVTYPE_Int64 = "i64";
    public const string KWPVTYPE_UINT8 = "u8";
    public const string KWPVTYPE_BYTE = "byte";
    public const string KWPVTYPE_UInt16 = "u16";
    public const string KWPVTYPE_WORD = "WORD";
    public const string KWPVTYPE_UInt32 = "u32";
    public const string KWPVTYPE_DWORD = "DWORD";
    public const string KWPVTYPE_UInt64 = "u64";
    public const string KWPVTYPE_FLOAT32 = "f32";
    public const string KWPVTYPE_FLOAT64 = "f64";
    public const string KWPVTYPE_Boolean = "boolean";
    public const string KWPVTYPE_String = "string";
    public const string KWPVTYPE_WideString = "wstring";
    public const string KWPVTYPE_Structure = "struct";
    public const string KWPVTYPE_DATA = "data";
    public const string KWPVTYPE_TIME = "time";
    public const string KWPVTYPE_DATI = "dt";
    public const string KWPVTYPE_DATE = "date";
    public const string KWPVTYPE_TOD = "tod";
    public const string KWPVI_LINKTYPE_PRCDATA = "prc";
    public const string KWPVI_LINKTYPE_RAWDATA = "raw";
    public const string KWPVI_EVENTMASK_ERROR = "e";
    public const string KWPVI_EVENTMASK_DATA = "d";
    public const string KWPVI_EVENTMASK_DATAFORM = "f";
    public const string KWPVI_EVENTMASK_CONNECT = "c";
    public const string KWPVI_EVENTMASK_STATUS = "s";
    public const string KWPVI_EVENTMASK_PROCEEDING = "p";
    public const string KWPVI_EVENTMASK_LINE = "l";
    public const string KWPVI_EVENTMASK_USERTAG = "u";
    public const string KW_ATTRIBUTE_EVENT = "e";
    public const string KW_ATTRIBUTE_READ = "r";
    public const string KW_ATTRIBUTE_WRITE = "w";
    public const string KW_ATTRIBUTE_SIMULATED = "s";
    public const string KW_ATTRIBUTE_DIRECT = "d";
    public const string KW_ATTRIBUTE_READWRITE = "rw";
    public const string KW_ATTRIBUTE_ECHO = "h";
    public const int SECSPERMIN = 60;
    public const int MINSPERHOUR = 60;
    public const int HOURSPERDAY = 24;
    public const int LEAPYEARDAYS = 366;
    public const int YEARDAYS = 365;
    public const uint SECSPERYEAR = 31536000;
    public const uint SECSPERLEAPYEAR = 31622400;
    public const int SECSPERHOUR = 3600;
    public const uint SECSPERDAY = 86400;
    public const int DAYSPERWEEK = 7;
    public const int MONSPERYEAR = 12;
    public const int YEAR_BASE = 1900;
    public const int EPOCH_YEAR = 1970;
    public const int EPOCH_WDAY = 4;
    public const int CALLBACK_DATA = -2;
    public const string KWPVI_CONNECT = "CD";
    public const string KWPVI_ALIGNMENT = "AL";
    public const string KWPVI_PVTYPE = "VT";
    public const string KWPVI_PVLEN = "VL";
    public const string KWPVI_PVCNT = "VN";
    public const string KWPVI_PVOFFS = "VO";
    public const string KWPVI_PVSPEC = "VS";
    public const string KWPVI_TYPE_NAME = "TN";
    public const string KWPVI_INIT_VALUE = "IV";
    public const string KWPVI_PVADDR = "VA";
    public const string KWPVI_ATTRIBUTE = "AT";
    public const string KWPVI_REFRESH = "RF";
    public const string KWPVI_HYSTERESE = "HY";
    public const string KWPVI_FUNCTION = "FS";
    public const string KWPVI_DEFAULT = "DV";
    public const string KWPVI_CASTMODE = "CM";
    public const string KWPVI_EVMASK = "EV";
    public const string KWPVI_LINKTYPE = "LT";
    public const string KWPVI_USERTAG = "UT";
    public const string KWPVI_OBJTYPE = "OT";
    public const string KWPVI_SCOPE = "SC";
    public const string KWPVI_SNAME = "SN";
    public const string KWPVI_LOADTYPE = "LD";
    public const string KWPVI_INSTMODE = "IM";
    public const string KWPVI_MODNAME = "MN";
    public const string KWPVI_STATUS = "ST";
    public const string KWPVI_FORCESTATE = "FC";
    public const string KWPVI_IOTYPE = "IO";
    public const string KWPVI_MODTYPE = "MT";
    public const string KWPVI_MODLEN = "ML";
    public const string KWPVI_MODOFFSET = "MO";
    public const string KWPVI_MEMLEN = "SL";
    public const string KWPVI_MEMFREELEN = "SF";
    public const string KWPVI_MEMBLOCKLEN = "SB";
    public const string KWPVI_DATALEN = "DL";
    public const string KWPVI_DATACNT = "DN";
    public const string KWPVI_CPUNAME = "CN";
    public const string KWPVI_CPUTYPE = "CT";
    public const string KWPVI_AWSTYPE = "AW";
    public const string KWPVI_UNRESLKN = "UL";
    public const string KWPVI_IDENTIFY = "ID";
    public const string KWPVI_VERSION = "VI";
    public const string KWPVI_MEM_SYS_RAM = "SysRam";
    public const string KWPVI_MEM_RAM = "Ram";
    public const string KWPVI_MEM_SYS_ROM = "SysRom";
    public const string KWPVI_MEM_ROM = "Rom";
    public const string KWPVI_MEM_MEMCARD = "MemCard";
    public const string KWPVI_MEM_FIX_RAM = "FixRam";
    public const string KWPVI_MEM_DRAM = "DRam";
    public const string KWPVI_MEM_PER_MEM = "PerMem";
    public const string KWPVI_MEM_DELETE = "Delete";
    public const string KWPVI_MEM_CLEAR = "Clear";
    public const string KWPVI_MEM_OVERLOAD = "Overload";
    public const string KWPVI_MEM_COPY = "Copy";
    public const string KWPVI_MEM_ONECYCLE = "OneCycle";
    public const string KWPVI_RUN_WARMSTART = "WarmStart";
    public const string KWPVI_RUN_COLDSTART = "ColdStart";
    public const string KWPVI_RUN_START = "Start";
    public const string KWPVI_RUN_STOP = "Stop";
    public const string KWPVI_RUN_RESUME = "Resume";
    public const string KWPVI_RUN_CYCLE = "Cycle";
    public const string KWPVI_RUN_CYCLE_ARG = "Cycle(%u)";
    public const string KWPVI_RUN_RESET = "Reset";
    public const string KWPVI_RUN_RECONF = "Reconfiguration";
    public const string KWPVI_RUN_NMI = "NMI";
    public const string KWPVI_RUN_DIAGNOSE = "Diagnose";
    public const string KWPVI_RUN_ERROR = "Error";
    public const string KWPVI_RUN_EXISTS = "Exists";
    public const string KWPVI_RUN_LOADING = "Loading";
    public const string KWPVI_RUN_INCOMPLETE = "Incomplete";
    public const string KWPVI_RUN_COMPLETE = "Complete";
    public const string KWPVI_RUN_READY = "Ready";
    public const string KWPVI_RUN_INUSE = "InUse";
    public const string KWPVI_RUN_NONEXISTING = "NonExisting";
    public const string KWPVI_RUN_UNRUNNABLE = "Unrunnable";
    public const string KWPVI_RUN_IDLE = "Idle";
    public const string KWPVI_RUN_RUNNING = "Running";
    public const string KWPVI_RUN_STOPPED = "Stopped";
    public const string KWPVI_RUN_STARTING = "Starting";
    public const string KWPVI_RUN_STOPPING = "Stopping";
    public const string KWPVI_RUN_RESUMING = "Resuming";
    public const string KWPVI_RUN_RESETING = "Reseting";
    public const string KWPVI_RUN_CONSTANT = "Const";
    public const string KWPVI_RUN_VARIABLE = "Var";
    public const string KW_VAR_TYPE = "VT=";
    public const string KW_VAR_LENGTH = "VL=";
    public const string KW_NUM_OF_ELEMENTS = "VN=";
    public const string KW_EXTENDED_TYPE = "VS=";
    public const string KW_EXTENDED_TYPE_ENUM = "VS=e";
    public const string KW_EXTENDED_TYPE_BITSTRING = "VS=b";
    public const string KW_EXTENDED_TYPE_MARRAY = "VS=a";
    public const string KW_EXTENDED_TYPE_SEMI_E = ";e";
    public const string KW_EXTENDED_TYPE_SEMI_B = ";b";
    public const string KW_EXTENDED_TYPE_SEMI_A = ";a";
    public const string KW_EXTENDED_TYPE_SEMI_V = ";v";
    public const string KW_EXTENDED_TYPE_DERIVED = "VS=v";
    public const string KW_EXTENDED_ENUM = "e";
    public const string KW_EXTENDED_BITSTRING = "b";
    public const string KW_EXTENDED_MARRAY = "a";
    public const string KW_EXTENDED_DERIVED = "v";
    public const string KW_DERIVED_FROM = "TN=";
    public const string KW_BIT_ADDRESS = "VA=";
    public const string KW_HYSTERESIS = "HY=";
    public const string KW_CAST_MODE = "CM=";
    public const string KW_ATTRIBUTE = "AT=";
    public const string KW_USER_TAG = "UT=";
    public const string KW_SCALING_FUNCTION = "FS=";
    public const string KW_ROCESS_ACCESS = "LT=";
    public const string KW_ROCESS_ACCESS_RAW = "raw";
    public const string KW_ROCESS_ACCESS_PRC = "prc";
    public const string KW_FORCE_STATE = "FC=";
    public const string KW_IO_STATE = "IO=";
    public const string KW_STATE = "ST=";
    public const string KW_UNRESOLVED_LINK = "UL=";
    public const string KW_SCOPE = "SC=";
    public const string KW_SCOPE_GLOBAL = "g";
    public const string KW_SCOPE_LOCAL = "l";
    public const string KW_SCOPE_DYNAMIC = "d";
    public const string KW_STRUCT_NAME = "SN=";
    public const string KW_IS_BIT_STRING = "b";
    public const string KW_IS_ENUM = "e";
    public const string KW_IS_DERIVED = "v";
    public const string KW_IS_ARRAY = "a";
    public const string KW_ALIGNMENT = "AL=";
    public const string KW_BIT_OFFSET = "VO=";
    public const string KW_EVENT_MASK = "EV=";
    public const string KW_REFRESH = "RF=";
    public const string KW_CONNECTION_DESC = "CD=";

    internal static int GetTimeSpanInt32(object value)
    {
      switch (value)
      {
        case int timeSpanInt32:
label_23:
          return timeSpanInt32;
        case uint num1:
          timeSpanInt32 = (int) num1;
          goto label_23;
        case short num2:
          timeSpanInt32 = (int) num2;
          goto label_23;
        case long num3:
          timeSpanInt32 = (int) num3;
          goto label_23;
        case ulong num4:
          timeSpanInt32 = (int) num4;
          goto label_23;
        case ushort num5:
          timeSpanInt32 = (int) num5;
          goto label_23;
        case sbyte num6:
          timeSpanInt32 = (int) num6;
          goto label_23;
        case byte num7:
          timeSpanInt32 = (int) num7;
          goto label_23;
        case float num8:
          timeSpanInt32 = (int) num8;
          goto label_23;
        case double num9:
          timeSpanInt32 = (int) num9;
          goto label_23;
        default:
          if ((object) (value as Value) != null)
          {
            timeSpanInt32 = PviMarshal.toInt32(((Value) value).propObjValue);
            goto label_23;
          }
          else
          {
            switch (value)
            {
              case string _:
              case Array _:
                object obj = value;
                if (value is Array)
                  obj = ((Array) value).GetValue(0);
                switch (obj)
                {
                  case DateTime dateTime1:
                    timeSpanInt32 = (int) (dateTime1.Ticks / 10000L);
                    goto label_23;
                  case TimeSpan timeSpan1:
                    timeSpanInt32 = (int) (timeSpan1.Ticks / 10000L);
                    goto label_23;
                  case string _:
                    timeSpanInt32 = -1 != ((string) obj).IndexOf('.') || -1 != ((string) obj).IndexOf(':') ? (int) (TimeSpan.Parse(obj.ToString()).Ticks / 10000L) : System.Convert.ToInt32(Pvi.GetIntVal((object) (System.Convert.ToInt64(obj) / 10000L)));
                    goto label_23;
                  default:
                    string str = obj.ToString();
                    timeSpanInt32 = -1 != str.IndexOf('.') || -1 != str.IndexOf(':') ? (int) (TimeSpan.Parse(obj.ToString()).Ticks / 10000L) : System.Convert.ToInt32(Pvi.GetIntVal((object) (System.Convert.ToInt64(str) / 10000L)));
                    goto label_23;
                }
              case DateTime dateTime2:
                timeSpanInt32 = (int) (dateTime2.Ticks / 10000L);
                goto label_23;
              case TimeSpan timeSpan2:
                timeSpanInt32 = (int) (timeSpan2.Ticks / 10000L);
                goto label_23;
              default:
                timeSpanInt32 = (int) value;
                goto label_23;
            }
          }
      }
    }

    internal static uint GetTimeSpanUInt32(object value)
    {
      switch (value)
      {
        case uint timeSpanUint32:
label_23:
          return timeSpanUint32;
        case int num1:
          timeSpanUint32 = (uint) num1;
          goto label_23;
        case short num2:
          timeSpanUint32 = (uint) num2;
          goto label_23;
        case long num3:
          timeSpanUint32 = (uint) num3;
          goto label_23;
        case ulong num4:
          timeSpanUint32 = (uint) num4;
          goto label_23;
        case ushort num5:
          timeSpanUint32 = (uint) num5;
          goto label_23;
        case sbyte num6:
          timeSpanUint32 = (uint) num6;
          goto label_23;
        case byte num7:
          timeSpanUint32 = (uint) num7;
          goto label_23;
        case float num8:
          timeSpanUint32 = (uint) num8;
          goto label_23;
        case double num9:
          timeSpanUint32 = (uint) num9;
          goto label_23;
        default:
          if ((object) (value as Value) != null)
          {
            timeSpanUint32 = PviMarshal.toUInt32(((Value) value).propObjValue);
            goto label_23;
          }
          else
          {
            switch (value)
            {
              case string _:
              case Array _:
                object obj = value;
                if (value is Array)
                  obj = ((Array) value).GetValue(0);
                switch (obj)
                {
                  case DateTime dateTime1:
                    timeSpanUint32 = (uint) ((ulong) dateTime1.Ticks / 10000UL);
                    goto label_23;
                  case TimeSpan timeSpan1:
                    timeSpanUint32 = (uint) ((ulong) timeSpan1.Ticks / 10000UL);
                    goto label_23;
                  case string _:
                    timeSpanUint32 = -1 != ((string) obj).IndexOf('.') || -1 != ((string) obj).IndexOf(':') ? (uint) ((ulong) TimeSpan.Parse(obj.ToString()).Ticks / 10000UL) : System.Convert.ToUInt32(Pvi.GetIntVal((object) (System.Convert.ToUInt64(obj) / 10000UL)));
                    goto label_23;
                  default:
                    string str = obj.ToString();
                    timeSpanUint32 = -1 != str.IndexOf('.') || -1 != str.IndexOf(':') ? (uint) ((ulong) TimeSpan.Parse(obj.ToString()).Ticks / 10000UL) : System.Convert.ToUInt32(Pvi.GetIntVal((object) (System.Convert.ToUInt64(str) / 10000UL)));
                    goto label_23;
                }
              case DateTime dateTime2:
                timeSpanUint32 = (uint) ((ulong) dateTime2.Ticks / 10000UL);
                goto label_23;
              case TimeSpan timeSpan2:
                timeSpanUint32 = (uint) ((ulong) timeSpan2.Ticks / 10000UL);
                goto label_23;
              default:
                timeSpanUint32 = (uint) value;
                goto label_23;
            }
          }
      }
    }

    internal static long GetIntVal(object value)
    {
      switch (value)
      {
        case long intVal:
label_22:
          return intVal;
        case int num1:
          intVal = (long) num1;
          goto label_22;
        case short num2:
          intVal = (long) num2;
          goto label_22;
        case ulong num3:
          intVal = (long) num3;
          goto label_22;
        case uint num4:
          intVal = (long) num4;
          goto label_22;
        case ushort num5:
          intVal = (long) num5;
          goto label_22;
        case sbyte num6:
          intVal = (long) num6;
          goto label_22;
        case byte num7:
          intVal = (long) num7;
          goto label_22;
        case float num8:
          intVal = (long) num8;
          goto label_22;
        case double num9:
          intVal = (long) num9;
          goto label_22;
        default:
          if ((object) (value as Value) != null)
          {
            intVal = PviMarshal.toInt64(((Value) value).propObjValue);
            goto label_22;
          }
          else
          {
            string str = !(value is Array) ? value.ToString() : ((Array) value).GetValue(0).ToString();
            if (str.ToLower().CompareTo("true") == 0)
              return 1;
            if (str.ToLower().CompareTo("false") == 0)
              return 0;
            if (NumberFormatInfo.CurrentInfo.NumberDecimalSeparator.CompareTo(",") == 0)
            {
              if (-1 == str.IndexOf(','))
              {
                int num = str.IndexOf('.');
                if (-1 != num)
                {
                  int length = str.LastIndexOf('.');
                  str = num == length ? str.Replace('.', ',') : str.Substring(0, length) + (object) ',' + str.Substring(length + 1);
                }
              }
            }
            else
              str.Replace(',', '.');
            intVal = -1 != str.IndexOf("0x") || -1 != str.IndexOf("0X") ? System.Convert.ToInt64(str, 16) : System.Convert.ToInt64(System.Convert.ToDouble(str));
            goto label_22;
          }
      }
    }

    internal static ulong GetUIntVal(object value)
    {
      switch (value)
      {
        case ulong uintVal:
label_22:
          return uintVal;
        case int num1:
          uintVal = (ulong) num1;
          goto label_22;
        case short num2:
          uintVal = (ulong) num2;
          goto label_22;
        case long num3:
          uintVal = (ulong) num3;
          goto label_22;
        case uint num4:
          uintVal = (ulong) num4;
          goto label_22;
        case ushort num5:
          uintVal = (ulong) num5;
          goto label_22;
        case sbyte num6:
          uintVal = (ulong) num6;
          goto label_22;
        case byte num7:
          uintVal = (ulong) num7;
          goto label_22;
        case float num8:
          uintVal = (ulong) num8;
          goto label_22;
        case double num9:
          uintVal = (ulong) num9;
          goto label_22;
        default:
          if ((object) (value as Value) != null)
          {
            uintVal = PviMarshal.toUInt64(((Value) value).propObjValue);
            goto label_22;
          }
          else
          {
            string str = !(value is Array) ? value.ToString() : ((Array) value).GetValue(0).ToString();
            if (str.ToLower().CompareTo("true") == 0)
              return 1;
            if (str.ToLower().CompareTo("false") == 0)
              return 0;
            if (NumberFormatInfo.CurrentInfo.NumberDecimalSeparator.CompareTo(",") == 0)
            {
              if (-1 == str.IndexOf(','))
              {
                int num = str.IndexOf('.');
                if (-1 != num)
                {
                  int length = str.LastIndexOf('.');
                  str = num == length ? str.Replace('.', ',') : str.Substring(0, length) + (object) ',' + str.Substring(length + 1);
                }
              }
            }
            else
              str.Replace(',', '.');
            uintVal = -1 != str.IndexOf("0x") || -1 != str.IndexOf("0X") ? System.Convert.ToUInt64(str, 16) : System.Convert.ToUInt64(System.Convert.ToDouble(str));
            goto label_22;
          }
      }
    }

    internal static bool IsLeapYear(int year) => year % 4 == 0 && (year < 1582 || year % 100 != 0 || year % 400 == 0);

    internal static DateTime ToDateTime(
      int tm_year,
      int tm_mon,
      int tm_mday,
      int tm_hour,
      int tm_min,
      int tm_sec)
    {
      return Pvi.ToDateTime(tm_year, tm_mon, tm_mday, tm_hour, tm_min, tm_sec, 0);
    }

    internal static DateTime ToDateTime(
      int tm_year,
      int tm_mon,
      int tm_mday,
      int tm_hour,
      int tm_min,
      int tm_sec,
      int tm_Msec)
    {
      DateTime dateTime;
      try
      {
        dateTime = new DateTime(tm_year, tm_mon, tm_mday, tm_hour, tm_min, tm_sec, tm_Msec);
      }
      catch
      {
        if (1 > tm_year)
          tm_year = 1;
        if (9999 < tm_year)
          tm_year = 9999;
        if (1 > tm_mon)
          tm_mon = 1;
        if (12 < tm_mon)
          tm_mon = 12;
        if (1 > tm_mday)
          tm_mday = 1;
        if (31 < tm_mday)
          tm_mday = 31;
        if (28 < tm_mday)
        {
          if (2 == tm_mon)
          {
            tm_mday = 29;
            if (Pvi.IsLeapYear(tm_year))
              tm_mday = 29;
          }
          else if (30 < tm_mday && (4 == tm_mon || 6 == tm_mon || 9 == tm_mon || 11 == tm_mon))
            tm_mday = 30;
        }
        if (0 > tm_hour || 23 < tm_hour)
          tm_hour = 0;
        if (0 > tm_min || 59 < tm_min)
          tm_min = 0;
        if (0 > tm_sec || 59 < tm_sec)
          tm_sec = 0;
        if (0 > tm_Msec || 999 < tm_Msec)
          tm_Msec = 0;
        dateTime = new DateTime(tm_year, tm_mon, tm_mday, tm_hour, tm_min, tm_sec, tm_Msec);
      }
      return dateTime;
    }

    internal static uint GetDateTimeUInt32(object value)
    {
      switch (value)
      {
        case uint dateTimeUint32_1:
          return dateTimeUint32_1;
        case int dateTimeUint32_2:
          return (uint) dateTimeUint32_2;
        case short dateTimeUint32_3:
          return (uint) dateTimeUint32_3;
        case long dateTimeUint32_4:
          return (uint) dateTimeUint32_4;
        case ulong dateTimeUint32_5:
          return (uint) dateTimeUint32_5;
        case ushort dateTimeUint32_6:
          return (uint) dateTimeUint32_6;
        case sbyte dateTimeUint32_7:
          return (uint) dateTimeUint32_7;
        case byte dateTimeUint32_8:
          return (uint) dateTimeUint32_8;
        case float dateTimeUint32_9:
          return (uint) dateTimeUint32_9;
        case double dateTimeUint32_10:
          return (uint) dateTimeUint32_10;
        default:
          if ((object) (value as Value) != null)
            return PviMarshal.toUInt32(((Value) value).propObjValue);
          switch (value)
          {
            case string _:
            case Array _:
              object obj = value;
              if (value is Array)
                obj = ((Array) value).GetValue(0);
              switch (obj)
              {
                case DateTime dt1:
                  return Pvi.DateTimeToUInt32(dt1);
                case TimeSpan timeSpan1:
                  return (uint) ((ulong) timeSpan1.Ticks / 10000000UL);
                case string _:
                  return Pvi.DateTimeToUInt32(DateTime.Parse(obj.ToString()));
                default:
                  return Pvi.DateTimeToUInt32(DateTime.Parse(obj.ToString()));
              }
            case DateTime dt2:
              return Pvi.DateTimeToUInt32(dt2);
            case TimeSpan timeSpan2:
              return (uint) ((ulong) timeSpan2.Ticks / 10000000UL);
            default:
              return (uint) value;
          }
      }
    }

    internal static uint GetDateUInt32(object value)
    {
      switch (value)
      {
        case uint dateUint32_1:
          return dateUint32_1;
        case int dateUint32_2:
          return (uint) dateUint32_2;
        case short dateUint32_3:
          return (uint) dateUint32_3;
        case long dateUint32_4:
          return (uint) dateUint32_4;
        case ulong dateUint32_5:
          return (uint) dateUint32_5;
        case ushort dateUint32_6:
          return (uint) dateUint32_6;
        case sbyte dateUint32_7:
          return (uint) dateUint32_7;
        case byte dateUint32_8:
          return (uint) dateUint32_8;
        case float dateUint32_9:
          return (uint) dateUint32_9;
        case double dateUint32_10:
          return (uint) dateUint32_10;
        default:
          if ((object) (value as Value) != null)
            return PviMarshal.toUInt32(((Value) value).propObjValue);
          switch (value)
          {
            case string _:
            case Array _:
              object obj = value;
              if (value is Array)
                obj = ((Array) value).GetValue(0);
              switch (obj)
              {
                case DateTime dt1:
                  return Pvi.DateTimeToUInt32(dt1);
                case TimeSpan timeSpan1:
                  return (uint) ((ulong) timeSpan1.Ticks / 10000000UL);
                case string _:
                  return Pvi.DateTimeToUInt32(DateTime.Parse(obj.ToString()));
                default:
                  return Pvi.DateTimeToUInt32(DateTime.Parse(obj.ToString()));
              }
            case DateTime dt2:
              return Pvi.DateTimeToUInt32(dt2);
            case TimeSpan timeSpan2:
              return (uint) ((ulong) timeSpan2.Ticks / 10000000UL);
            default:
              return (uint) value;
          }
      }
    }

    internal static uint DateTimeToUInt32(DateTime dt)
    {
      uint num1 = (uint) ((ulong) (dt.DayOfYear - 1) * 86400UL) + (uint) (dt.Hour * 3600) + (uint) (dt.Minute * 60) + (uint) dt.Second + (uint) ((ulong) (dt.Year - 1970) * 31536000UL);
      int num2 = dt.Year - 1900;
      int num3 = num2 / 4 - 17 - num2 / 100 + (dt.Year / 400 - 4);
      uint uint32 = num1 + (uint) ((ulong) num3 * 86400UL);
      if (Pvi.IsLeapYear(dt.Year))
        uint32 -= 86400U;
      return uint32;
    }

    internal static uint ToUInt32(string value) => -1 == value.IndexOf("0x") ? (-1 == value.IndexOf("-") ? System.Convert.ToUInt32(value) : (uint) int.Parse(value)) : System.Convert.ToUInt32(value, 16);

    internal static bool IsLeap(uint year) => year % 4U == 0U && (year % 100U != 0U || year % 400U == 0U);

    internal static DateTime UInt32ToDateTime(uint timeValue)
    {
      uint[] numArray1 = new uint[2]{ 365U, 366U };
      uint[,] numArray2 = new uint[2, 12]
      {
        {
          31U,
          28U,
          31U,
          30U,
          31U,
          30U,
          31U,
          31U,
          30U,
          31U,
          30U,
          31U
        },
        {
          31U,
          29U,
          31U,
          30U,
          31U,
          30U,
          31U,
          31U,
          30U,
          31U,
          30U,
          31U
        }
      };
      uint num1 = timeValue / 86400U;
      uint num2 = timeValue % 86400U;
      while (num2 < 0U)
      {
        num2 += 86400U;
        --num1;
      }
      while (num2 >= 86400U)
      {
        num2 -= 86400U;
        ++num1;
      }
      uint tm_hour = num2 / 3600U;
      uint num3 = num2 % 3600U;
      uint tm_min = num3 / 60U;
      uint tm_sec = num3 % 60U;
      uint num4 = 1970;
      uint uint32;
      if (num1 >= 0U)
      {
        while (true)
        {
          uint32 = System.Convert.ToUInt32(Pvi.IsLeap(num4));
          if (num1 >= numArray1[(IntPtr) uint32])
          {
            ++num4;
            num1 -= numArray1[(IntPtr) uint32];
          }
          else
            break;
        }
      }
      else
      {
        do
        {
          --num4;
          uint32 = System.Convert.ToUInt32(Pvi.IsLeap(num4));
          num1 += numArray1[(IntPtr) uint32];
        }
        while (num1 < 0U);
      }
      uint index;
      for (index = 0U; num1 >= numArray2[(int) (IntPtr) uint32, (int) (IntPtr) index]; ++index)
        num1 -= numArray2[(int) (IntPtr) uint32, (int) (IntPtr) index];
      uint tm_mday = num1 + 1U;
      return Pvi.ToDateTime((int) num4, (int) index + 1, (int) tm_mday, (int) tm_hour, (int) tm_min, (int) tm_sec);
    }
  }
}
