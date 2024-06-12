// Decompiled with JetBrains decompiler
// Type: BR.AN.PviServices.LoggerEntryCollection
// Assembly: BR.AN.PVIServices, Version=1.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DA853F0A-C070-4E30-B7A7-51D955FB85F6
// Assembly location: C:\Users\ss_six\Desktop\FXE\FXE software integration package\COMET_FXE-PVIServices_Example\Pluto-PVI-trial_01\packages\IToolS.BandR.3.5.58\lib\net20\BR.AN.PVIServices.dll

using System;
using System.Collections;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Xml;

namespace BR.AN.PviServices
{
  public class LoggerEntryCollection : BaseCollection
  {
    private const uint ARLOG_MODULEKENN = 11159;
    private const uint ARLOG_MODULETYPE = 83;
    private const uint ARLOG_RECTAGBEGIN = 3735901816;
    protected ArrayList arrayOfLoggerEntries;
    internal uint propContentVersion;
    private string propXMLData;

    public LoggerEntryCollection(Base parent, string name)
      : base(CollectionType.SortedList, (object) parent, name)
    {
      this.propContentVersion = 0U;
      this.arrayOfLoggerEntries = new ArrayList(1);
    }

    public LoggerEntryCollection(string name, LoggerEntryCollection eventEntries)
      : base(CollectionType.SortedList, (object) null, name)
    {
      this.propContentVersion = eventEntries.ContentVersion;
      this.arrayOfLoggerEntries = new ArrayList(1);
      for (int index = 0; index < eventEntries.Count; ++index)
        this.Add((LoggerEntryBase) eventEntries[index]);
    }

    public LoggerEntryCollection(string name)
      : base(CollectionType.SortedList, (object) null, name)
    {
      this.propContentVersion = 0U;
      this.arrayOfLoggerEntries = new ArrayList(1);
    }

    ~LoggerEntryCollection() => this.Dispose(false, true);

    public uint ContentVersion => this.propContentVersion;

    internal override void Dispose(bool disposing, bool removeFromCollection)
    {
      if (this.propDisposed)
        return;
      this.CleanUp(disposing);
      base.Dispose(disposing, removeFromCollection);
      this.propParent = (object) null;
      this.propUserData = (object) null;
      this.propName = (string) null;
    }

    internal void CleanUp(bool disposing)
    {
      ArrayList arrayList = new ArrayList();
      this.propCounter = 0;
      if (this.Values != null)
      {
        foreach (LoggerEntryBase loggerEntryBase in (IEnumerable) this.Values)
          arrayList.Add((object) loggerEntryBase);
      }
      for (int index = 0; index < arrayList.Count; ++index)
        ((LoggerEntryBase) arrayList[index]).Dispose(disposing);
      this.Clear();
      if (!disposing)
        return;
      this.arrayOfLoggerEntries = (ArrayList) null;
    }

    private static uint ConvertToUInt32(byte[] bytearray)
    {
      uint uint32 = 0;
      for (int index = 0; bytearray.Length == 4 && index < bytearray.Length; ++index)
        uint32 += (uint) bytearray[3 - index] * (uint) Math.Pow(2.0, (double) (index * 8));
      return uint32;
    }

    private static ushort ConvertToUInt16(byte[] bytearray)
    {
      ushort uint16 = 0;
      for (int index = 0; bytearray.Length == 2 && index < bytearray.Length; ++index)
        uint16 += (ushort) ((uint) bytearray[1 - index] * (uint) (ushort) Math.Pow(2.0, (double) (index * 8)));
      return uint16;
    }

    private unsafe void ModuleNameBytesToAsciiString(
      byte[] brString,
      ref char[] asciiString,
      ref int asciiLength)
    {
      byte[] numArray1 = new byte[10]
      {
        (byte) 6,
        (byte) 0,
        (byte) 2,
        (byte) 4,
        (byte) 6,
        (byte) 0,
        (byte) 2,
        (byte) 4,
        (byte) 6,
        (byte) 0
      };
      byte[] numArray2 = new byte[10]
      {
        (byte) 0,
        (byte) 0,
        (byte) 1,
        (byte) 2,
        (byte) 3,
        (byte) 3,
        (byte) 4,
        (byte) 5,
        (byte) 6,
        (byte) 6
      };
      byte[] bytearray = new byte[2];
      asciiLength = (int) brString[0] >> 4;
      if (asciiLength > 10)
        return;
      for (int index = 0; index < asciiLength; ++index)
      {
        fixed (byte* numPtr = &brString[0])
        {
          bytearray[0] = numPtr[(int) numArray2[index]];
          bytearray[1] = (numPtr + (int) numArray2[index])[1];
        }
        byte num = (byte) ((uint) (byte) ((uint) LoggerEntryCollection.ConvertToUInt16(bytearray) >> (int) numArray1[index]) & 63U);
        if (num >= (byte) 0 && num <= (byte) 9)
          asciiString[index] = (char) (48U + (uint) num);
        else if (num >= (byte) 10 && num <= (byte) 35)
          asciiString[index] = (char) (65 + (int) num - 10);
        else if (num >= (byte) 36 && num <= (byte) 61)
        {
          asciiString[index] = (char) (97 + (int) num - 36);
        }
        else
        {
          switch (num)
          {
            case 62:
              asciiString[index] = '_';
              continue;
            case 63:
              asciiString[index] = '$';
              continue;
            default:
              continue;
          }
        }
      }
      asciiString[asciiLength] = char.MinValue;
    }

    public int Load(XmlTextReader reader)
    {
      int num = 0;
      try
      {
        this.LoadARLEntries((LoggerCollection) null, reader, false, true);
      }
      catch
      {
        num = 12054;
      }
      return num;
    }

    public int Load(string file)
    {
      int num1 = 0;
      int severityCode = 0;
      int customerCode = 0;
      int facilityCode = 0;
      LoggerXMLInterpreter loggerXmlInterpreter = new LoggerXMLInterpreter();
      if (!File.Exists(file))
        return 2;
      try
      {
        if (Path.GetExtension(file).ToLower().Equals(".br"))
        {
          if (!(this.propParent is Service) && !(this.propParent is Logger))
            return 5;
          FileStream input = new FileStream(file, FileMode.Open, FileAccess.Read);
          int attributes = (int) File.GetAttributes(file);
          BinaryReader binaryReader = new BinaryReader((Stream) input);
          byte[] bytearray1 = binaryReader.ReadBytes(2);
          byte num2 = binaryReader.ReadByte();
          if (LoggerEntryCollection.ConvertToUInt16(bytearray1) != (ushort) 11159 || num2 != (byte) 83)
          {
            binaryReader.Close();
            input.Close();
            return 11;
          }
          binaryReader.ReadBytes(1);
          byte[] brString = binaryReader.ReadBytes(8);
          binaryReader.ReadBytes(2);
          uint uint32_1 = LoggerEntryCollection.ConvertToUInt32(binaryReader.ReadBytes(4));
          binaryReader.ReadBytes(14);
          byte[] bytearray2 = binaryReader.ReadBytes(4);
          byte[] bytearray3 = binaryReader.ReadBytes(4);
          byte[] bytearray4 = binaryReader.ReadBytes(4);
          int uint32_2 = (int) LoggerEntryCollection.ConvertToUInt32(bytearray2);
          uint uint32_3 = LoggerEntryCollection.ConvertToUInt32(bytearray3);
          uint uint32_4 = LoggerEntryCollection.ConvertToUInt32(bytearray4);
          binaryReader.ReadBytes((int) uint32_3 - 32 - 12);
          if (LoggerEntryCollection.ConvertToUInt16(bytearray1) == (ushort) 11159 && num2 == (byte) 83)
          {
            LoggerCollection loggerCollection = new LoggerCollection(CollectionType.ArrayList, this.propParent, file);
            Logger logger;
            if ((logger = loggerCollection[file]) == null)
            {
              char[] asciiString = new char[11];
              int asciiLength = 0;
              this.ModuleNameBytesToAsciiString(brString, ref asciiString, ref asciiLength);
              string name = new string(asciiString, 0, asciiLength);
              if (this.propParent is Service)
              {
                logger = new Logger((Service) this.propParent, name, true);
                logger.GlobalMerge = true;
              }
              else
                logger = (Logger) this.propParent;
            }
            byte[] numArray = binaryReader.ReadBytes((int) uint32_1 - (int) uint32_3);
            if (numArray != null)
            {
              IntPtr hMemory1 = PviMarshal.AllocHGlobal((IntPtr) Marshal.SizeOf(typeof (VWSection)));
              for (int ofs = 0; ofs < Marshal.SizeOf(typeof (VWSection)); ++ofs)
                Marshal.WriteByte(hMemory1, ofs, numArray[ofs]);
              VWSection structure1 = (VWSection) Marshal.PtrToStructure(hMemory1, typeof (VWSection));
              PviMarshal.FreeHGlobal(ref hMemory1);
              this.propContentVersion = structure1.version;
              uint num3 = uint32_4 + structure1.lenOfLogData;
              uint num4 = structure1.actOff;
              IntPtr hMemory2 = PviMarshal.AllocHGlobal((IntPtr) Marshal.SizeOf(typeof (ARLoggerEntry)));
              for (int ofs = 0; ofs < Marshal.SizeOf(typeof (ARLoggerEntry)); ++ofs)
                Marshal.WriteByte(hMemory2, ofs, numArray[(long) ofs + (long) uint32_4 - (long) uint32_3 + (long) num4]);
              ARLoggerEntry structure2 = (ARLoggerEntry) Marshal.PtrToStructure(hMemory2, typeof (ARLoggerEntry));
              PviMarshal.FreeHGlobal(ref hMemory2);
              int num5 = 0;
              bool flag = false;
              if (uint32_4 + num4 + structure2.entLen > num3)
              {
                num5 = (int) num3 - (int) num4 - (int) structure1.invLen;
                flag = true;
              }
              while (structure2.recTagBegin == 3735901816U)
              {
                DateTime dateTime = Pvi.UInt32ToDateTime(System.Convert.ToUInt32(structure2.logTime.sec));
                long num6 = System.Convert.ToInt64(structure2.logTime.nanoSec) / 1000L;
                dateTime = dateTime.AddTicks(num6);
                LoggerEntry logEntry = new LoggerEntry((object) logger, dateTime);
                logEntry.propLevelType = (LevelType) structure2.logLevel;
                logEntry.propErrorNumber = structure2.errorNumber;
                logEntry.propTask = structure2.taskName;
                logEntry.propErrorInfo = structure2.infoFlag;
                logEntry._AdditionalDataFormat = 0;
                if (this.propContentVersion >= 4112U)
                {
                  logEntry._RecordId = structure2.index;
                  logEntry._AdditionalDataFormat = (int) ((structure2.infoFlag & 16711680U) >> 16);
                  logEntry.propEventId = (int) structure2.ulEventId;
                  logEntry.propOriginRecordId = (int) structure2.ulOriginRecordId;
                  logEntry.propErrorNumber = loggerXmlInterpreter.DecodeV1010EventID(logEntry.propEventId, ref severityCode, ref customerCode, ref facilityCode);
                  logEntry.propLevelType = (LevelType) severityCode;
                  logEntry.propCustomerCode = customerCode;
                  logEntry.propFacilityCode = facilityCode;
                }
                logEntry.propInternID = structure2.index;
                logEntry.UpdateUKey();
                logger.LoggerEntries.propContentVersion = logger.propContentVersion = this.propContentVersion;
                logger.LoggerEntries.Add((object) logEntry.UniqueKey, (object) logEntry);
                if (flag)
                {
                  num5 -= Marshal.SizeOf(typeof (ARLoggerEntry));
                  int num7 = (int) structure2.lenOfBinData;
                  if (num5 < num7)
                    num7 = num5;
                  if (structure2.lenOfBinData > 0U)
                  {
                    logEntry.propBinary = new byte[(IntPtr) structure2.lenOfBinData];
                    for (int index = 0; index < num7; ++index)
                      logEntry.propBinary[index] = numArray[(long) index + (long) uint32_4 - (long) uint32_3 + (long) num4 + (long) Marshal.SizeOf(typeof (ARLoggerEntry))];
                    for (int index = num7; (long) index < (long) structure2.lenOfBinData; ++index)
                      logEntry.propBinary[index] = numArray[(long) (index - num7) + (long) uint32_4 - (long) uint32_3];
                  }
                  int lenOfAsciiString = (int) structure2.lenOfAsciiString;
                  int num8 = (long) num5 <= (long) structure2.lenOfBinData ? 0 : num5 - (int) structure2.lenOfBinData;
                  if (structure2.lenOfAsciiString > 0U)
                  {
                    IntPtr hMemory3 = PviMarshal.AllocHGlobal((IntPtr) (long) structure2.lenOfAsciiString);
                    for (int ofs = 0; ofs < num8; ++ofs)
                      Marshal.WriteByte(hMemory3, ofs, numArray[(long) ofs + (long) uint32_4 - (long) uint32_3 + (long) num4 + (long) Marshal.SizeOf(typeof (ARLoggerEntry)) + (long) structure2.lenOfBinData]);
                    for (int ofs = num8; (long) ofs < (long) structure2.lenOfAsciiString; ++ofs)
                      Marshal.WriteByte(hMemory3, ofs, numArray[(long) (ofs - num8) + (long) uint32_4 - (long) uint32_3]);
                    logEntry.propErrorText = PviMarshal.PtrToStringAnsi(hMemory3);
                    PviMarshal.FreeHGlobal(ref hMemory3);
                  }
                }
                else
                {
                  if (structure2.lenOfBinData > 0U)
                  {
                    logEntry.propBinary = new byte[(IntPtr) structure2.lenOfBinData];
                    for (int index = 0; (long) index < (long) structure2.lenOfBinData; ++index)
                      logEntry.propBinary[index] = numArray[(long) index + (long) uint32_4 - (long) uint32_3 + (long) num4 + (long) Marshal.SizeOf(typeof (ARLoggerEntry))];
                    if (((int) structure2.infoFlag & 2) != 0 || ((int) structure2.infoFlag & 1) != 0)
                      logEntry.GetExceptionData();
                    if (this.propContentVersion >= 4112U && 0 < logEntry.AdditionalDataFormat)
                      loggerXmlInterpreter.DecodeV1010AdditionalData(logEntry, logEntry.AdditionalDataFormat, logEntry.Binary.GetLength(0), logEntry.Binary);
                  }
                  if (structure2.lenOfAsciiString > 0U)
                  {
                    IntPtr hMemory4 = PviMarshal.AllocHGlobal((IntPtr) (long) structure2.lenOfAsciiString);
                    for (int ofs = 0; (long) ofs < (long) structure2.lenOfAsciiString; ++ofs)
                      Marshal.WriteByte(hMemory4, ofs, numArray[(long) ofs + (long) uint32_4 - (long) uint32_3 + (long) num4 + (long) Marshal.SizeOf(typeof (ARLoggerEntry)) + (long) structure2.lenOfBinData]);
                    logEntry.propErrorText = PviMarshal.PtrToStringAnsi(hMemory4);
                    PviMarshal.FreeHGlobal(ref hMemory4);
                  }
                }
                if ((int) structure2.index != (int) structure1.refIdx)
                {
                  if (num4 < structure2.prevLen)
                  {
                    num5 = (int) structure2.prevLen - (int) num4;
                    num4 = (uint) ((int) num3 - (int) uint32_4 - (int) structure1.invLen - num5);
                    flag = true;
                  }
                  else
                  {
                    num4 -= structure2.prevLen;
                    flag = false;
                  }
                  IntPtr hMemory5 = PviMarshal.AllocHGlobal((IntPtr) Marshal.SizeOf(typeof (ARLoggerEntry)));
                  for (int ofs = 0; ofs < Marshal.SizeOf(typeof (ARLoggerEntry)); ++ofs)
                    Marshal.WriteByte(hMemory5, ofs, numArray[(long) ofs + (long) uint32_4 - (long) uint32_3 + (long) num4]);
                  structure2 = (ARLoggerEntry) Marshal.PtrToStructure(hMemory5, typeof (ARLoggerEntry));
                  PviMarshal.FreeHGlobal(ref hMemory5);
                }
                else
                  break;
              }
            }
            loggerCollection.Add(logger);
          }
          binaryReader.Close();
          input.Close();
        }
        else
          return this.propParent is Service ? this.LoadServiceBasedARL(file) : this.LoadLocalARL(file);
      }
      catch
      {
        return 11;
      }
      return num1;
    }

    private string ValidateTimestampString(string timeStamp)
    {
      string str = timeStamp;
      int length = timeStamp.LastIndexOf('.');
      if (0 < length)
      {
        if (timeStamp.Substring(length + 1).Length == 0)
          str = timeStamp.Substring(0, length) + ".000" + timeStamp.Substring(length + 1);
        else if (1 == timeStamp.Substring(length + 1).Length)
          str = timeStamp.Substring(0, length) + ".00" + timeStamp.Substring(length + 1);
        else if (2 == timeStamp.Substring(length + 1).Length)
          str = timeStamp.Substring(0, length) + ".0" + timeStamp.Substring(length + 1);
      }
      else
        str += ".000000";
      return str;
    }

    private void LoadARLEntries(
      LoggerCollection loggerCollection,
      XmlTextReader xmlReader,
      bool isServiceBased,
      bool onlyOneLogger)
    {
      ulong ulKey = 0;
      int severityCode = 0;
      int customerCode = 0;
      int facilityCode = 0;
      LoggerXMLInterpreter loggerXmlInterpreter = new LoggerXMLInterpreter();
      if (!isServiceBased)
        ulKey = 0UL;
      do
      {
        if (string.Compare(xmlReader.Name, "LoggerEntries") == 0 && xmlReader.IsStartElement())
        {
          this.propContentVersion = 0U;
          string attribute = xmlReader.GetAttribute("Version");
          if (attribute != null && 0 < attribute.Length)
            this.propContentVersion = System.Convert.ToUInt32(attribute);
        }
        else
        {
          ++ulKey;
          if (string.Compare(xmlReader.Name, "Entry") == 0 && xmlReader.IsStartElement())
          {
            string attribute1 = xmlReader.GetAttribute("LoggerModule");
            Logger logger;
            if (isServiceBased)
            {
              if ((logger = loggerCollection[attribute1]) == null)
              {
                logger = new Logger((Service) this.propParent, attribute1);
                logger.GlobalMerge = true;
                loggerCollection.Add(logger);
              }
            }
            else
              logger = (Logger) this.propParent;
            string attribute2 = xmlReader.GetAttribute("Level");
            string attribute3 = xmlReader.GetAttribute("Time");
            string attribute4 = xmlReader.GetAttribute("ErrorNumber");
            string attribute5 = xmlReader.GetAttribute("ErrorInfo");
            string attribute6 = xmlReader.GetAttribute("Task");
            string attribute7 = xmlReader.GetAttribute("RecordId");
            string attribute8 = xmlReader.GetAttribute("AdditionalDataFormat");
            string attribute9 = xmlReader.GetAttribute("BinaryFormat");
            string attribute10 = xmlReader.GetAttribute("OriginRecordId");
            string attribute11 = xmlReader.GetAttribute("EventId");
            string str1 = this.ValidateTimestampString(attribute3);
            LoggerEntry logEntry = new LoggerEntry((object) logger, attribute1, System.Convert.ToDateTime(str1, (IFormatProvider) DateTimeFormatInfo.InvariantInfo), attribute1.CompareTo("$LOG285$") == 0);
            logEntry.propErrorNumber = Pvi.ToUInt32(attribute4);
            if (attribute8 != null && 0 < attribute8.Length)
              logEntry._AdditionalDataFormat = System.Convert.ToInt32(attribute8);
            if (attribute9 != null && 0 < attribute9.Length)
              logEntry.propEventDataType = (EventDataTypes) System.Convert.ToInt32(attribute9.ToString());
            if (attribute10 != null && 0 < attribute10.Length)
              logEntry.propOriginRecordId = System.Convert.ToInt32(attribute10);
            if (attribute7 != null && 0 < attribute7.Length)
              logEntry._RecordId = Pvi.ToUInt32(attribute7);
            if (attribute11 != null && 0 < attribute11.Length)
            {
              logEntry.propEventId = System.Convert.ToInt32(attribute11);
              logEntry.propErrorNumber = loggerXmlInterpreter.DecodeV1010EventID(logEntry.propEventId, ref severityCode, ref customerCode, ref facilityCode);
              logEntry.propLevelType = (LevelType) severityCode;
              logEntry.propCustomerCode = customerCode;
              logEntry.propFacilityCode = facilityCode;
            }
            if (attribute5 != null && 0 < attribute5.Length)
              logEntry.propErrorInfo = Pvi.ToUInt32(attribute5);
            logEntry.propTask = attribute6;
            logEntry.propInternID = logEntry.propID;
            if (isServiceBased)
            {
              logEntry.UpdateUKey();
              logger.LoggerEntries.propContentVersion = logger.propContentVersion = this.propContentVersion;
              logger.LoggerEntries.Add((object) logEntry.UniqueKey, (object) logEntry);
            }
            else
            {
              logEntry.UpdateUKey(ulKey);
              this.Add((object) logEntry.UniqueKey, (object) logEntry);
            }
            switch (attribute2)
            {
              case "Success":
                logEntry.propLevelType = LevelType.Success;
                break;
              case "Warning":
                logEntry.propLevelType = LevelType.Warning;
                break;
              case "Debug":
                logEntry.propLevelType = LevelType.Debug;
                break;
              case "Info":
                logEntry.propLevelType = LevelType.Info;
                break;
              case "Fatal":
                logEntry.propLevelType = LevelType.Fatal;
                break;
              default:
                logEntry.propLevelType = LevelType.Info;
                break;
            }
            if (!xmlReader.IsEmptyElement)
            {
              do
              {
                if (string.Compare(xmlReader.Name.ToLower(), "ascii") == 0)
                {
                  if (logEntry.EventDataType == EventDataTypes.ArLoggerAPI)
                    logEntry.propEventData = (object) xmlReader.ReadElementString();
                  else
                    logEntry.propErrorText = xmlReader.ReadElementString();
                }
                if (string.Compare(xmlReader.Name.ToLower(), "binary") == 0 && xmlReader.IsStartElement())
                {
                  string[] strArray = xmlReader.ReadElementString().Trim(' ').Split(' ');
                  logEntry.propBinary = new byte[strArray.Length];
                  for (int index = 0; index < strArray.Length; ++index)
                  {
                    if (strArray[index].Length == 0)
                      logEntry.propBinary[index] = (byte) 0;
                    else
                      logEntry.propBinary[index] = System.Convert.ToByte(strArray[index]);
                  }
                  if (this.propContentVersion >= 4112U && 3 != logEntry._AdditionalDataFormat)
                    loggerXmlInterpreter.DecodeV1010AdditionalData(logEntry, logEntry._AdditionalDataFormat, logEntry.Binary.Length, logEntry.Binary);
                }
                if (string.Compare(xmlReader.Name.ToLower(), "exception") == 0)
                {
                  string attribute12 = xmlReader.GetAttribute("Type");
                  string attribute13 = xmlReader.GetAttribute("BacktraceCount");
                  string attribute14 = xmlReader.GetAttribute("DataLength");
                  string attribute15 = xmlReader.GetAttribute("ArVersion");
                  logEntry.propException = new Exception();
                  switch (attribute12)
                  {
                    case "Processor":
                      logEntry.propException.propType = ExceptionType.Processor;
                      break;
                    case "System":
                      logEntry.propException.propType = ExceptionType.System;
                      break;
                    default:
                      logEntry.propException.propType = ExceptionType.System;
                      break;
                  }
                  logEntry.propException.propBacktraceCount = System.Convert.ToUInt32(attribute13);
                  logEntry.propException.propARVersion = attribute15;
                  logEntry.propRuntimeVersion = attribute15;
                  logEntry.propException.propDataLength = System.Convert.ToUInt32(attribute14);
                  do
                  {
                    if (string.Compare(xmlReader.Name.ToLower(), "processordata") == 0 && xmlReader.IsStartElement())
                    {
                      string attribute16 = xmlReader.GetAttribute("ProgramCounter");
                      string attribute17 = xmlReader.GetAttribute("EFlags");
                      string attribute18 = xmlReader.GetAttribute("ErrorCode");
                      logEntry.propException.propProcessorData = new ProcessorData();
                      logEntry.propException.propProcessorData.propProgramCounter = System.Convert.ToUInt32(attribute16);
                      logEntry.propException.propProcessorData.propEFlags = System.Convert.ToUInt32(attribute17);
                      logEntry.propException.propProcessorData.propErrorCode = System.Convert.ToUInt32(attribute18);
                    }
                    if (string.Compare(xmlReader.Name.ToLower(), "taskdata") == 0 && xmlReader.IsStartElement())
                    {
                      string attribute19 = xmlReader.GetAttribute("ID");
                      string attribute20 = xmlReader.GetAttribute("Priority");
                      string attribute21 = xmlReader.GetAttribute("Name");
                      string attribute22 = xmlReader.GetAttribute("StackBegin");
                      string attribute23 = xmlReader.GetAttribute("StackEnd");
                      string attribute24 = xmlReader.GetAttribute("RegisterEAX");
                      string attribute25 = xmlReader.GetAttribute("RegisterEBX");
                      string attribute26 = xmlReader.GetAttribute("RegisterECX");
                      string attribute27 = xmlReader.GetAttribute("RegisterEDX");
                      string attribute28 = xmlReader.GetAttribute("RegisterESI");
                      string attribute29 = xmlReader.GetAttribute("RegisterEDI");
                      string attribute30 = xmlReader.GetAttribute("RegisterEIP");
                      string attribute31 = xmlReader.GetAttribute("RegisterEBP");
                      string attribute32 = xmlReader.GetAttribute("RegisterESP");
                      string attribute33 = xmlReader.GetAttribute("RegisterEFLAGS");
                      string attribute34 = xmlReader.GetAttribute("StackSize");
                      logEntry.propException.propTaskData = new TaskData();
                      logEntry.propException.propTaskData.propId = System.Convert.ToUInt32(attribute19);
                      logEntry.propException.propTaskData.propPriority = System.Convert.ToUInt32(attribute20);
                      logEntry.propException.propTaskData.propName = attribute21;
                      logEntry.propException.propTaskData.propStackBegin = System.Convert.ToUInt32(attribute22);
                      logEntry.propException.propTaskData.propStackEnd = System.Convert.ToUInt32(attribute23);
                      logEntry.propException.propTaskData.propRegisterEax = System.Convert.ToUInt32(attribute24);
                      logEntry.propException.propTaskData.propRegisterEbx = System.Convert.ToUInt32(attribute25);
                      logEntry.propException.propTaskData.propRegisterEcx = System.Convert.ToUInt32(attribute26);
                      logEntry.propException.propTaskData.propRegisterEdx = System.Convert.ToUInt32(attribute27);
                      logEntry.propException.propTaskData.propRegisterEsi = System.Convert.ToUInt32(attribute28);
                      logEntry.propException.propTaskData.propRegisterEdi = System.Convert.ToUInt32(attribute29);
                      logEntry.propException.propTaskData.propRegisterEip = System.Convert.ToUInt32(attribute30);
                      logEntry.propException.propTaskData.propRegisterEbp = System.Convert.ToUInt32(attribute31);
                      logEntry.propException.propTaskData.propRegisterEsp = System.Convert.ToUInt32(attribute32);
                      logEntry.propException.propTaskData.propRegisterEflags = System.Convert.ToUInt32(attribute33);
                      logEntry.propException.propTaskData.propStackSize = System.Convert.ToUInt32(attribute34);
                    }
                    if (string.Compare(xmlReader.Name.ToLower(), "memorydata") == 0 && xmlReader.IsStartElement())
                    {
                      logEntry.propException.propMemoryData = new MemoryData();
                      while (string.Compare(xmlReader.Name.ToLower(), "memorydata") != 0 || xmlReader.IsStartElement())
                      {
                        if (string.Compare(xmlReader.Name.ToLower(), "pc") == 0 && xmlReader.IsStartElement())
                        {
                          string[] strArray = xmlReader.ReadElementString().Trim(' ').Split(' ');
                          logEntry.propException.propMemoryData.propMemPc = new byte[strArray.Length];
                          for (int index = 0; index < strArray.Length; ++index)
                            logEntry.propException.propMemoryData.propMemPc[index] = System.Convert.ToByte(strArray[index]);
                        }
                        if (string.Compare(xmlReader.Name.ToLower(), "esp") == 0 && xmlReader.IsStartElement())
                        {
                          string[] strArray = xmlReader.ReadElementString().Trim(' ').Split(' ');
                          logEntry.propException.propMemoryData.propMemESP = new byte[strArray.Length];
                          for (int index = 0; index < strArray.Length; ++index)
                            logEntry.propException.propMemoryData.propMemESP[index] = System.Convert.ToByte(strArray[index]);
                        }
                        if (string.Compare(xmlReader.Name.ToLower(), "memorydata") == 0 && !xmlReader.IsStartElement() || !xmlReader.Read())
                          break;
                      }
                    }
                    if (string.Compare(xmlReader.Name.ToLower(), "backtrace") == 0 && xmlReader.IsStartElement())
                    {
                      string attribute35 = xmlReader.GetAttribute("Address");
                      string attribute36 = xmlReader.GetAttribute("FunctionName");
                      string attribute37 = xmlReader.GetAttribute("ParamCount");
                      Backtrace backtrace1;
                      if (logEntry.propException.propBacktrace == null)
                      {
                        logEntry.propException.propBacktrace = new Backtrace();
                        backtrace1 = logEntry.propException.propBacktrace;
                      }
                      else
                      {
                        Backtrace backtrace2 = logEntry.propException.propBacktrace;
                        for (Backtrace backtrace3 = backtrace2.propNextBacktrace; backtrace3 != null; backtrace3 = backtrace3.NextBacktrace)
                          backtrace2 = backtrace3;
                        backtrace2.propNextBacktrace = new Backtrace();
                        backtrace1 = backtrace2.propNextBacktrace;
                      }
                      backtrace1.propAddress = System.Convert.ToUInt32(attribute35);
                      backtrace1.propFunctionName = attribute36;
                      backtrace1.propParamCount = System.Convert.ToUInt32(attribute37);
                      do
                      {
                        if (xmlReader.NodeType == XmlNodeType.Element)
                        {
                          if (string.Compare(xmlReader.Name, "Backtrace") != 0 || xmlReader.IsStartElement())
                          {
                            if (string.Compare(xmlReader.Name.ToLower(), "parameters") == 0 && xmlReader.IsStartElement())
                            {
                              string str2 = xmlReader.ReadElementString();
                              if (str2.Length > 0)
                              {
                                string[] strArray = str2.Trim(' ').Split(' ');
                                backtrace1.propParameter = new uint[(IntPtr) backtrace1.propParamCount];
                                for (int index = 0; index < strArray.Length; ++index)
                                  backtrace1.propParameter[index] = System.Convert.ToUInt32(strArray[index]);
                              }
                            }
                            if (string.Compare(xmlReader.Name.ToLower(), "functioninfo") == 0 && xmlReader.IsStartElement())
                            {
                              string attribute38 = xmlReader.GetAttribute("ModuleName");
                              string attribute39 = xmlReader.GetAttribute("CodeOffset");
                              backtrace1.propFunctionInfo = new FunctionInfo();
                              backtrace1.propFunctionInfo.codeOffset = System.Convert.ToUInt32(attribute39);
                              backtrace1.propFunctionInfo.moduleName = attribute38;
                            }
                            if (string.Compare(xmlReader.Name.ToLower(), "callstack") == 0 && xmlReader.IsStartElement())
                            {
                              string attribute40 = xmlReader.GetAttribute("ModuleName");
                              string attribute41 = xmlReader.GetAttribute("CodeOffset");
                              backtrace1.propCallstack = new Callstack();
                              backtrace1.propCallstack.propCodeOffset = System.Convert.ToUInt32(attribute41);
                              backtrace1.propCallstack.propModuleName = attribute40;
                            }
                            if (string.Compare(xmlReader.Name.ToLower(), "pcinfo") == 0 && xmlReader.IsStartElement())
                            {
                              string attribute42 = xmlReader.GetAttribute("ModuleName");
                              string attribute43 = xmlReader.GetAttribute("CodeOffset");
                              backtrace1.propPCInfo = new PCInfo();
                              backtrace1.propPCInfo.propCodeOffset = System.Convert.ToUInt32(attribute43);
                              backtrace1.propPCInfo.propModuleName = attribute42;
                            }
                          }
                          else
                            break;
                        }
                      }
                      while ((string.Compare(xmlReader.Name, "Backtrace") != 0 || xmlReader.IsStartElement()) && xmlReader.Read());
                    }
                  }
                  while ((string.Compare(xmlReader.Name, "Exception") != 0 || xmlReader.IsStartElement()) && xmlReader.Read());
                }
              }
              while ((string.Compare(xmlReader.Name, "Entry") != 0 || xmlReader.IsStartElement()) && xmlReader.Read());
            }
          }
        }
      }
      while (xmlReader.Read() && (onlyOneLogger ? (string.Compare(xmlReader.Name, "Logger") != 0 ? 1 : 0) : (!onlyOneLogger ? 1 : 0)) != 0);
    }

    private int LoadServiceBasedARL(string file)
    {
      int num = 0;
      XmlTextReader xmlReader = (XmlTextReader) null;
      XmlSanitizerStream input = (XmlSanitizerStream) null;
      LoggerCollection loggerCollection = new LoggerCollection(CollectionType.ArrayList, this.propParent, file);
      try
      {
        input = new XmlSanitizerStream(file);
        xmlReader = new XmlTextReader((TextReader) input);
        this.LoadARLEntries(loggerCollection, xmlReader, true, false);
      }
      catch
      {
        if (xmlReader != null)
        {
          xmlReader.Close();
          input.Close();
          xmlReader = (XmlTextReader) null;
        }
        num = 12054;
      }
      finally
      {
        if (xmlReader != null)
        {
          xmlReader.Close();
          input.Close();
        }
      }
      return num;
    }

    private int LoadLocalARL(string file)
    {
      int num = 0;
      XmlTextReader xmlReader = (XmlTextReader) null;
      try
      {
        this.CleanUp(false);
        xmlReader = new XmlTextReader(file);
        this.LoadARLEntries((LoggerCollection) null, xmlReader, false, false);
      }
      catch
      {
        if (xmlReader != null)
        {
          xmlReader.Close();
          xmlReader = (XmlTextReader) null;
        }
        num = 12054;
      }
      finally
      {
        xmlReader?.Close();
      }
      return num;
    }

    public int Save(ref StringBuilder xmlTextBlock)
    {
      int num1 = 0;
      if (this.Count == 0)
        return num1;
      foreach (LoggerEntry loggerEntry in (IEnumerable) this.Values)
      {
        string str1 = string.Format("{0:000000}", (object) (loggerEntry.DateTime.Ticks % 10000000L / 10L));
        if (loggerEntry.ErrorText != null)
        {
          string str2 = loggerEntry.LoggerModuleName;
          if (loggerEntry.propParent is Logger)
            str2 = ((Base) loggerEntry.propParent).Name;
          xmlTextBlock.Append(string.Format("<Entry Level=\"{0}\" Time=\"{1}.{2}\" ErrorNumber=\"{3}\" ErrorInfo=\"{4}\" Task=\"{5}\"", (object) loggerEntry.LevelType.ToString(), (object) loggerEntry.DateTime.ToString("s", (IFormatProvider) DateTimeFormatInfo.InvariantInfo), (object) str1, (object) loggerEntry.ErrorNumber.ToString(), (object) loggerEntry.ErrorInfo.ToString(), (object) loggerEntry.propTask));
          if (this.propContentVersion >= 4112U)
          {
            if (0 < loggerEntry.EventID)
              xmlTextBlock.Append(string.Format(" EventId=\"{0}\"", (object) loggerEntry.EventID.ToString()));
            if (0U < loggerEntry.propInternID || loggerEntry._RecordId != 0U)
              xmlTextBlock.Append(string.Format(" RecordId=\"{0}\"", 0U < loggerEntry._RecordId ? (object) loggerEntry._RecordId.ToString() : (object) loggerEntry.propInternID.ToString()));
            if (0 < loggerEntry.OriginRecordId)
              xmlTextBlock.Append(string.Format(" OriginRecordId=\"{0}\"", (object) loggerEntry.OriginRecordId.ToString()));
            if (0 < loggerEntry.CustomerCode)
              xmlTextBlock.Append(string.Format(" CustomerCode=\"{0}\"", (object) loggerEntry.CustomerCode.ToString()));
            if (0 < loggerEntry.FacilityCode)
              xmlTextBlock.Append(string.Format(" FacilityCode=\"{0}\"", (object) loggerEntry.FacilityCode.ToString()));
            if (0 < loggerEntry.AdditionalDataFormat)
              xmlTextBlock.Append(string.Format(" AdditionalDataFormat=\"{0}\"", (object) loggerEntry.AdditionalDataFormat.ToString()));
            if (loggerEntry.EventDataType != EventDataTypes.Undefined && loggerEntry.EventDataType != EventDataTypes.EmptyEventData)
              xmlTextBlock.Append(string.Format(" BinaryFormat=\"{0}\"", (object) loggerEntry.EventDataType.ToString()));
          }
          xmlTextBlock.Append(string.Format(" LoggerModule=\"{0}\">\r\n", (object) str2));
          char[] charArray = (loggerEntry.EventDataType != EventDataTypes.ArLoggerAPI ? loggerEntry.ErrorText.Replace("&", "&amp;") : loggerEntry.EventData.ToString()).ToCharArray();
          string str3 = "";
          for (int index = 0; index < charArray.Length; ++index)
          {
            char ch = charArray[index];
            string str4 = ch.ToString();
            if (' ' > ch || '~' < ch)
              str4 = string.Format("&#{0};", (object) (byte) ch);
            str3 += str4;
          }
          string str5 = str3.Replace("<", "&lt;").Replace(">", "&gt;").Replace("\"", "&quot;").Replace("'", "&apos;").Replace("\u0012", "&#18;").Replace("\r", "&#13;").Replace("\u007F", "&#127;").Replace("\u0015", "&#21;").Replace("\u001B", "&#27;").Replace("\n", "&#10;");
          xmlTextBlock.Append(string.Format("<ASCII>{0}</ASCII>\r\n", (object) str5));
          if (loggerEntry.Exception != null)
          {
            xmlTextBlock.Append(string.Format("<Exception Type=\"{0}\" BacktraceCount=\"{1}\" DataLength=\"{2}\" ArVersion=\"{3}\">\r\n", (object) loggerEntry.Exception.Type, (object) loggerEntry.Exception.BacktraceCount.ToString(), (object) loggerEntry.Exception.DataLength.ToString(), (object) loggerEntry.Exception.ArVersion));
            if (loggerEntry.Exception.ProcessorData != null)
              xmlTextBlock.Append(string.Format("<ProcessorData ProgramCounter=\"{0}\" EFlags=\"{1}\" ErrorCode=\"{2}\"/>\r\n", (object) loggerEntry.Exception.ProcessorData.ProgramCounter.ToString(), (object) loggerEntry.Exception.ProcessorData.EFlags.ToString(), (object) loggerEntry.Exception.ProcessorData.ErrorCode.ToString()));
            if (loggerEntry.Exception.TaskData != null)
              xmlTextBlock.Append(string.Format("<TaskData ID=\"{0}\" Priority=\"{1}\" Name=\"{2}\" StackBegin=\"{3}\" StackEnd=\"{4}\" StackSize=\"{5}\" RegisterEAX=\"{6}\" RegisterEBX=\"{7}\" RegisterECX=\"{8}\" RegisterEDX=\"{9}\" RegisterESI=\"{10}\" RegisterEDI=\"{11}\" RegisterEIP=\"{12}\" RegisterESP=\"{13}\" RegisterEBP=\"{14}\" RegisterEFLAGS=\"{15}\"/>\r\n", (object) loggerEntry.Exception.TaskData.Id.ToString(), (object) loggerEntry.Exception.TaskData.Priority.ToString(), (object) loggerEntry.Exception.TaskData.Name, (object) loggerEntry.Exception.TaskData.StackBegin.ToString(), (object) loggerEntry.Exception.TaskData.StackEnd.ToString(), (object) loggerEntry.Exception.TaskData.StackSize.ToString(), (object) loggerEntry.Exception.TaskData.RegisterEAX.ToString(), (object) loggerEntry.Exception.TaskData.RegisterEBX.ToString(), (object) loggerEntry.Exception.TaskData.RegisterECX.ToString(), (object) loggerEntry.Exception.TaskData.RegisterEDX.ToString(), (object) loggerEntry.Exception.TaskData.RegisterESI.ToString(), (object) loggerEntry.Exception.TaskData.RegisterEDI.ToString(), (object) loggerEntry.Exception.TaskData.RegisterEIP.ToString(), (object) loggerEntry.Exception.TaskData.RegisterESP.ToString(), (object) loggerEntry.Exception.TaskData.RegisterEBP.ToString(), (object) loggerEntry.Exception.TaskData.RegisterEFLAGS.ToString()));
            if (loggerEntry.Exception.MemoryData != null)
            {
              xmlTextBlock.Append("<MemoryData>\r\n");
              xmlTextBlock.Append("<PC>");
              int num2 = 0;
              foreach (byte num3 in loggerEntry.Exception.MemoryData.PC)
              {
                xmlTextBlock.Append(num3.ToString());
                ++num2;
                if (num2 != loggerEntry.Exception.MemoryData.PC.Length)
                  xmlTextBlock.Append(" ");
                else
                  break;
              }
              xmlTextBlock.Append("</PC>\r\n");
              xmlTextBlock.Append("<ESP>");
              int num4 = 0;
              foreach (byte num5 in loggerEntry.Exception.MemoryData.ESP)
              {
                xmlTextBlock.Append(num5.ToString());
                ++num4;
                if (num4 != loggerEntry.Exception.MemoryData.ESP.Length)
                  xmlTextBlock.Append(" ");
                else
                  break;
              }
              xmlTextBlock.Append("</ESP>\r\n");
              xmlTextBlock.Append("</MemoryData>\r\n");
            }
            Backtrace backtrace1 = loggerEntry.Exception.Backtrace;
            while (backtrace1 != null)
            {
              Backtrace backtrace2 = backtrace1;
              backtrace1 = backtrace2.NextBacktrace;
              xmlTextBlock.Append(string.Format("<Backtrace Address=\"{0}\" FunctionName=\"{1}\" ParamCount=\"{2}\">\r\n", (object) backtrace2.Address.ToString(), (object) backtrace2.FunctionName, (object) backtrace2.Paramcount.ToString()));
              if (backtrace2.Parameter != null)
              {
                xmlTextBlock.Append("<Parameters>");
                for (int index = 0; index < backtrace2.Parameter.Length; ++index)
                {
                  xmlTextBlock.Append(backtrace2.Parameter[index].ToString());
                  if (index != backtrace2.Parameter.Length - 1)
                    xmlTextBlock.Append(" ");
                  else
                    break;
                }
                xmlTextBlock.Append("</Parameters>\r\n");
              }
              if (backtrace2.FunctionInfo != null)
                xmlTextBlock.Append(string.Format("<FunctionInfo ModuleName=\"{0}\" CodeOffset=\"{1}\"/>\r\n", (object) backtrace2.FunctionInfo.ModuleName, (object) backtrace2.FunctionInfo.CodeOffset));
              if (backtrace2.propCallstack != null)
              {
                string str6 = backtrace2.Callstack.ModuleName.Replace("<", "&lt;").Replace(">", "&gt;").Replace("\"", "&quot;").Replace("'", "&apos;").Replace("\u0012", "&#18;").Replace("\r", "&#13;").Replace("\u007F", "&#127;").Replace("\u0015", "&#21;").Replace("\u001B", "&#27;").Replace("\n", "&#10;");
                xmlTextBlock.Append(string.Format("<Callstack ModuleName=\"{0}\" CodeOffset=\"{1}\"/>\r\n", (object) str6, (object) backtrace2.Callstack.CodeOffset));
              }
              if (backtrace2.PCInfo != null)
                xmlTextBlock.Append(string.Format("<PCInfo ModuleName=\"{0}\" CodeOffset=\"{1}\"/>\r\n", (object) backtrace2.PCInfo.ModuleName, (object) backtrace2.PCInfo.CodeOffset));
              xmlTextBlock.Append("</Backtrace>\r\n");
            }
            xmlTextBlock.Append("</Exception>\r\n");
            if (loggerEntry.Binary != null && loggerEntry.Binary.Length != 0)
            {
              xmlTextBlock.Append("<Binary>");
              int num6 = 0;
              foreach (byte num7 in loggerEntry.Binary)
              {
                xmlTextBlock.Append(num7.ToString());
                ++num6;
                if (num6 != loggerEntry.Binary.Length)
                  xmlTextBlock.Append(" ");
                else
                  break;
              }
              xmlTextBlock.Append("</Binary>\r\n");
            }
          }
          else
          {
            byte[] binary = loggerEntry.Binary;
            if (binary != null && binary.Length != 0)
            {
              xmlTextBlock.Append("<Binary>");
              int num8 = 0;
              foreach (byte num9 in binary)
              {
                xmlTextBlock.Append(num9.ToString());
                ++num8;
                if (num8 != binary.Length)
                  xmlTextBlock.Append(" ");
                else
                  break;
              }
              xmlTextBlock.Append("</Binary>\r\n");
            }
          }
          xmlTextBlock.Append("</Entry>\r\n");
        }
        else
        {
          string str7 = !(loggerEntry.propParent is Logger) ? "NA" : ((Base) loggerEntry.propParent).Name;
          xmlTextBlock.Append(string.Format("<Entry Level=\"{0}\" Time=\"{1}.{2}\" ErrorNumber=\"{3}\" Task=\"{4}\" LoggerModule=\"{5}\"/>\r\n", (object) loggerEntry.LevelType.ToString(), (object) loggerEntry.DateTime.ToString("s", (IFormatProvider) DateTimeFormatInfo.InvariantInfo), (object) str1, (object) loggerEntry.ErrorNumber.ToString(), (object) loggerEntry.propTask, (object) str7));
        }
      }
      return num1;
    }

    public int SaveAs(string file, LogExportFormat fileFormat)
    {
      int num;
      switch (fileFormat)
      {
        case LogExportFormat.HTML:
          num = this.SaveAsHTML(file);
          break;
        case LogExportFormat.CSV:
          num = this.SaveAsCSV(file);
          break;
        default:
          num = this.SaveAsARL(file);
          break;
      }
      return num;
    }

    private string GetXMLString(string ascii) => ascii.Replace("&", "&amp;").Replace("&amp;amp", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;").Replace("\"", "&quot;").Replace("'", "&apos;").Replace("Ä", "&#196;").Replace("Ö", "&#214;").Replace("Ü", "&#220;").Replace("ä", "&#228;").Replace("ö", "&#246;").Replace("ü", "&#252;").Replace("ß", "&#223;");

    private int SaveAsHTML(string file)
    {
      int num1 = 0;
      StringBuilder stringBuilder = new StringBuilder();
      try
      {
        string str1 = "<html>\r\n";
        stringBuilder.Append(str1);
        string str2 = "<head>\r\n";
        stringBuilder.Append(str2);
        string str3 = string.Format("<title>{0}</title>\r\n", (object) file);
        stringBuilder.Append(str3);
        string str4 = "</head>\r\n";
        stringBuilder.Append(str4);
        string str5 = "<body>\r\n";
        stringBuilder.Append(str5);
        string str6 = string.Format("<h3>{0} (V0x{1:X}) - {2} Entries:</h3>\r\n", (object) file, (object) this.ContentVersion, (object) this.Count);
        stringBuilder.Append(str6);
        string str7 = "<table border=\"1\" cellpadding=\"2\" cellspacing=\"1\" style=\"border-collapse: collapse\" bordercolor=\"#111111\">\r\n";
        stringBuilder.Append(str7);
        string str8 = "<tr bgcolor=\"#E5E5E5\">\r\n";
        stringBuilder.Append(str8);
        string str9 = "<td align=\"center\"><b>TimeStamp</b></td>\r\n";
        stringBuilder.Append(str9);
        string str10 = "<td align=\"center\"><b>Error</b></td>\r\n";
        stringBuilder.Append(str10);
        string str11 = "<td align=\"center\"><b>Level</b></td>\r\n";
        stringBuilder.Append(str11);
        string str12 = "<td align=\"center\"><b>Task</b></td>\r\n";
        stringBuilder.Append(str12);
        if (this.propContentVersion >= 4112U)
        {
          string str13 = "<td align=\"center\"><b>EventId</b></td>\r\n";
          stringBuilder.Append(str13);
          string str14 = "<td align=\"center\"><b>RecordId</b></td>\r\n";
          stringBuilder.Append(str14);
          string str15 = "<td align=\"center\"><b>OriginRecordId</b></td>\r\n";
          stringBuilder.Append(str15);
          string str16 = "<td align=\"center\"><b>CustomerCode</b></td>\r\n";
          stringBuilder.Append(str16);
          string str17 = "<td align=\"center\"><b>FacilityCode</b></td>\r\n";
          stringBuilder.Append(str17);
          string str18 = "<td align=\"center\"><b>AdditionalDataFormat</b></td>\r\n";
          stringBuilder.Append(str18);
          string str19 = "<td align=\"center\"><b>BinaryFormat</b></td>\r\n";
          stringBuilder.Append(str19);
        }
        string str20 = "<td align=\"center\"><b>Binary</b></td>\r\n";
        stringBuilder.Append(str20);
        string str21 = "<td align=\"center\"><b>ASCII</b></td>\r\n";
        stringBuilder.Append(str21);
        string str22 = "<td align=\"center\"><b>Logger</b></td>\r\n";
        stringBuilder.Append(str22);
        string str23 = "</tr>\r\n";
        stringBuilder.Append(str23);
        if (0 < this.Count)
        {
          foreach (LoggerEntry loggerEntry in (IEnumerable) this.Values)
          {
            string str24;
            switch (loggerEntry.LevelType)
            {
              case LevelType.Success:
              case LevelType.Fatal:
              case (LevelType) 129:
                str24 = " bgcolor=\"FFD7D7\"";
                break;
              case LevelType.Warning:
              case (LevelType) 130:
                str24 = " bgcolor=\"#FFFFCE\"";
                break;
              default:
                str24 = "";
                break;
            }
            string str25 = string.Format("<tr{0}>\r\n", (object) str24);
            stringBuilder.Append(str25);
            string str26 = string.Format("{0}    <td align=\"right\">{1}</td>\r\n    <td align=\"right\">{2}</td>\r\n    <td align=\"right\">{3}</td>\r\n", (object) loggerEntry.ToStringHTM(this.propContentVersion), (object) loggerEntry.BinaryToString(), (object) this.GetXMLString(loggerEntry.ErrorText), (object) loggerEntry.LoggerModuleName);
            stringBuilder.Append(str26);
            string errorDescription = loggerEntry.ErrorDescription;
            if (errorDescription != null && 0 < errorDescription.Length)
            {
              string str27 = string.Format("<tr><td align=\"right\" colspan=\"7\">{0}</td></tr>", (object) errorDescription);
              stringBuilder.Append(str27);
            }
            if (loggerEntry.Exception != null)
            {
              string str28 = "<tr>\r\n<td align=\"right\"></td>\r\n<td align=\"right\" colspan=\"6\">\r\n";
              stringBuilder.Append(str28);
              string str29 = "<table border=\"1\" cellpadding=\"2\" cellspacing=\"1\" style=\"border-collapse: collapse\" bordercolorlight=\"#C0C0C0\" bordercolordark=\"#808080\">\r\n";
              stringBuilder.Append(str29);
              string str30 = string.Format("<tr>\r\n<td style=\"border-collapse: collapse\" bordercolor=\"#C0C0C0\">Exception</td><td style=\"border-collapse: collapse\" bordercolor=\"#C0C0C0\">{0}</td></tr>\r\n", (object) loggerEntry.Exception.Type.ToString());
              stringBuilder.Append(str30);
              string stringHtm1 = loggerEntry.Exception.ToStringHTM();
              stringBuilder.Append(stringHtm1);
              if (loggerEntry.Exception.ProcessorData != null)
              {
                string stringHtm2 = loggerEntry.Exception.ProcessorData.ToStringHTM();
                stringBuilder.Append(stringHtm2);
              }
              if (loggerEntry.Exception.TaskData != null)
              {
                string stringHtm3 = loggerEntry.Exception.TaskData.ToStringHTM();
                stringBuilder.Append(stringHtm3);
              }
              if (loggerEntry.Exception.MemoryData != null)
              {
                string stringHtm4 = loggerEntry.Exception.MemoryData.ToStringHTM();
                stringBuilder.Append(stringHtm4);
              }
              int num2 = 0;
              for (Backtrace backtrace = loggerEntry.Exception.Backtrace; backtrace != null; backtrace = backtrace.NextBacktrace)
              {
                ++num2;
                string str31 = string.Format("<tr>\r\n<td align=\"left\" valign=\"top\" style=\"border-collapse: collapse\" bordercolor=\"#C0C0C0\">Backtrace #{0}</td>\r\n<td bordercolor=\"#C0C0C0\">\r\n", (object) num2);
                stringBuilder.Append(str31);
                string str32 = "<table border=\"1\" cellpadding=\"2\" cellspacing=\"1\" style=\"border-collapse: collapse\" bordercolor=\"#FFFFFF\" bordercolorlight=\"#C0C0C0\" bordercolordark=\"#808080\">\r\n";
                stringBuilder.Append(str32);
                string stringHtm5 = backtrace.ToStringHTM();
                stringBuilder.Append(stringHtm5);
                if (backtrace.FunctionInfo != null)
                {
                  string stringHtm6 = backtrace.FunctionInfo.ToStringHTM();
                  stringBuilder.Append(stringHtm6);
                }
                if (backtrace.Callstack != null)
                {
                  string stringHtm7 = backtrace.Callstack.ToStringHTM();
                  stringBuilder.Append(stringHtm7);
                }
                if (backtrace.PCInfo != null)
                {
                  string stringHtm8 = backtrace.PCInfo.ToStringHTM();
                  stringBuilder.Append(stringHtm8);
                }
                string str33 = "</table>\r\n</td>\r\n</tr>\r\n";
                stringBuilder.Append(str33);
              }
              string str34 = "</table>\r\n</td>\r\n</tr>\r\n";
              stringBuilder.Append(str34);
            }
          }
        }
        string str35 = "</table>\r\n";
        stringBuilder.Append(str35);
        string str36 = "</body>\r\n";
        stringBuilder.Append(str36);
        string str37 = "</html>\r\n";
        stringBuilder.Append(str37);
        FileStream fileStream = new FileStream(file, FileMode.Create, FileAccess.Write);
        StreamWriter streamWriter = new StreamWriter((Stream) fileStream);
        streamWriter.Write(stringBuilder.ToString());
        streamWriter.Close();
        fileStream.Close();
      }
      catch (System.Exception ex)
      {
        num1 = -2;
      }
      return num1;
    }

    private int SaveAsCSV(string file)
    {
      int num = 0;
      StringBuilder stringBuilder = new StringBuilder();
      try
      {
        string str1 = "TimeStamp;Error;Level;Info;Task;";
        if (this.propContentVersion >= 4112U)
          str1 += "EventId;RecordId;OriginRecordId;CustomerCode;FacilityCode;AdditionalDataFormat;BinaryFormat;";
        string str2 = str1 + "Binary;ASCII;Logger;" + "Exception;BackTraces;DataLen;ARVersion;" + "ProgramCounter;EFlags;ErrorCode;" + "TaskID;Priority;TaskName;StackBegin;StackEnd;StackSize;RegisterEAX;RegisterEBX;RegisterECX;RegisterEDX;RegisterESI;RegisterEDI;RegisterEIP;RegisterESP;RegisterEBP;RegisterEFLAGS;" + "PCMemory;ESPMemory;" + "BackTraceAddress;BackTraceFunction;BackTraceInfo;BackTraceTask;" + "BackTraceParameters;" + "FnModule;FnCodeOffset;" + "CallStackModule;CallStackCodeOffset;" + "PCInfoModule;PCInfoCodeOffset;" + "Help\r\n";
        stringBuilder.Append(str2);
        if (0 < this.Count)
        {
          foreach (LoggerEntry loggerEntry in (IEnumerable) this.Values)
          {
            string errorText = loggerEntry.ErrorText;
            if (loggerEntry.EventDataType == EventDataTypes.ArLoggerAPI)
              errorText = loggerEntry.EventData.ToString();
            string str3 = string.Format("{0}{1};{2};{3};", (object) loggerEntry.ToStringCSV(this.propContentVersion), (object) loggerEntry.BinaryToString(), (object) errorText, (object) loggerEntry.LoggerModuleName);
            stringBuilder.Append(str3);
            string str4 = loggerEntry.ErrorDescription;
            if (loggerEntry.Exception == null)
            {
              string str5 = "\"\";\"\";\"\";\"\";" + "\"\";\"\";\"\";" + "\"\";\"\";\"\";\"\";\"\";\"\";\"\";\"\";\"\";\"\";\"\";\"\";\"\";\"\";\"\";\"\";" + "\"\";\"\";" + "\"\";\"\";\"\";\"\";" + "\"\";" + "\"\";\"\";" + "\"\";\"\";" + "\"\";\"\";\"" + (str4 == null ? str4 : "") + "\"\r\n";
              stringBuilder.Append(str5);
            }
            else
            {
              string stringCsv1 = loggerEntry.Exception.ToStringCSV();
              stringBuilder.Append(stringCsv1);
              if (loggerEntry.Exception.ProcessorData != null)
              {
                string stringCsv2 = loggerEntry.Exception.ProcessorData.ToStringCSV();
                stringBuilder.Append(stringCsv2);
              }
              else
              {
                string str6 = "\"\";\"\";\"\";";
                stringBuilder.Append(str6);
              }
              if (loggerEntry.Exception.TaskData != null)
              {
                string stringCsv3 = loggerEntry.Exception.TaskData.ToStringCSV();
                stringBuilder.Append(stringCsv3);
              }
              else
              {
                string str7 = "\"\";\"\";\"\";\"\";\"\";\"\";\"\";\"\";\"\";\"\";\"\";\"\";\"\";\"\";\"\";\"\";";
                stringBuilder.Append(str7);
              }
              if (loggerEntry.Exception.MemoryData != null)
              {
                string stringCsv4 = loggerEntry.Exception.MemoryData.ToStringCSV();
                stringBuilder.Append(stringCsv4);
              }
              else
              {
                string str8 = "\"\";\"\";";
                stringBuilder.Append(str8);
              }
              Backtrace backtrace = loggerEntry.Exception.Backtrace;
              if (backtrace != null)
              {
                string str9 = "";
                for (; backtrace != null; backtrace = backtrace.NextBacktrace)
                {
                  stringBuilder.Append(str9);
                  string stringCsv5 = backtrace.ToStringCSV();
                  stringBuilder.Append(stringCsv5);
                  string str10 = "\"";
                  stringBuilder.Append(str10);
                  for (uint index = 0; index < backtrace.Paramcount; ++index)
                  {
                    string str11 = string.Format("{0} ", (object) backtrace.Parameter[(IntPtr) index]);
                    stringBuilder.Append(str11);
                  }
                  string str12 = "\";";
                  stringBuilder.Append(str12);
                  if (backtrace.FunctionInfo != null)
                  {
                    string stringCsv6 = backtrace.FunctionInfo.ToStringCSV();
                    stringBuilder.Append(stringCsv6);
                  }
                  else
                  {
                    string str13 = "\"\";\"\";";
                    stringBuilder.Append(str13);
                  }
                  if (backtrace.Callstack != null)
                  {
                    string stringCsv7 = backtrace.Callstack.ToStringCSV();
                    stringBuilder.Append(stringCsv7);
                  }
                  else
                  {
                    string str14 = "\"\";\"\";";
                    stringBuilder.Append(str14);
                  }
                  if (backtrace.PCInfo != null)
                  {
                    string stringCsv8 = backtrace.PCInfo.ToStringCSV();
                    stringBuilder.Append(stringCsv8);
                  }
                  else
                  {
                    string str15 = "\"\";\"\";";
                    stringBuilder.Append(str15);
                  }
                  string str16 = string.Format("\"{0}\"\r\n", (object) str4);
                  str4 = "";
                  stringBuilder.Append(str16);
                  str9 = "\"\";\"\";\"\";\"\";\"\";\"\";\"\";\"\";" + "\"\";\"\";\"\";\"\";" + "\"\";\"\";\"\";" + "\"\";\"\";\"\";\"\";\"\";\"\";\"\";\"\";\"\";\"\";\"\";\"\";\"\";\"\";\"\";\"\";" + "\"\";\"\";";
                }
              }
              else
              {
                string str17 = "\"\";\"\";\"\";\"\";" + "\"\";" + "\"\";\"\";" + "\"\";\"\";" + "\"\";\"\";" + str4 + "\r\n";
                stringBuilder.Append(str17);
              }
            }
          }
        }
        FileStream fileStream = new FileStream(file, FileMode.Create, FileAccess.Write);
        StreamWriter streamWriter = new StreamWriter((Stream) fileStream);
        streamWriter.Write(stringBuilder.ToString());
        streamWriter.Close();
        fileStream.Close();
      }
      catch (System.Exception ex)
      {
        num = -2;
      }
      return num;
    }

    public int Save(string file) => this.SaveAsARL(file);

    private int SaveAsARL(string file)
    {
      int num = 0;
      StringBuilder xmlTextBlock = new StringBuilder();
      xmlTextBlock.Append("<?xml version=\"1.0\" encoding=\"utf-8\" ?>\r\n<?AutomationStudio Version=\"2.6\"?>\r\n<?AutomationRuntimeIOSystem Version=\"1.0\"?>\r\n<?LoggerControl Version=\"1.0\"?>\r\n");
      xmlTextBlock.Append(string.Format("<LoggerEntries Count=\"{0}\" Version=\"{1}\">\r\n", (object) this.Count, (object) this.ContentVersion.ToString()));
      this.Save(ref xmlTextBlock);
      xmlTextBlock.Append("</LoggerEntries>");
      FileStream fileStream = new FileStream(file, FileMode.Create, FileAccess.Write);
      StreamWriter streamWriter = new StreamWriter((Stream) fileStream);
      streamWriter.Write(xmlTextBlock.ToString());
      streamWriter.Close();
      fileStream.Close();
      return num;
    }

    public override void Clear()
    {
      LoggerEventArgs e = (LoggerEventArgs) null;
      if (this.Parent is Logger && ((Base) this.Parent).Service != null && ((Base) this.Parent).Service.LoggerEntries != null)
      {
        if (!((Base) this.Parent).propDisposed)
          e = new LoggerEventArgs("EntriesRemoved", "EntriesRemoved", 0, ((Base) this.Parent).Service.Language, Action.LoggerGlobalRemoved, new LoggerEntryCollection("EntriesRemoved", this));
        if (0 < this.Count)
        {
          foreach (LoggerEntryBase loggerEntryBase in (IEnumerable) this.Values)
          {
            if (((Base) this.Parent).Service.LoggerEntries.ContainsKey((object) loggerEntryBase.UniqueKey))
              ((Base) this.Parent).Service.LoggerEntries.Remove(loggerEntryBase.UniqueKey);
          }
        }
        if (!((Base) this.Parent).propDisposed)
          ((Logger) this.Parent).CallOnEntriesRemoved(e);
      }
      if (this.arrayOfLoggerEntries != null)
        this.arrayOfLoggerEntries.Clear();
      base.Clear();
    }

    private int getArrayIndex(LoggerEntryBase lentry) => this.propParent is Service ? lentry.propSArrayIndex : lentry.propArrayIndex;

    private void setArrayIndex(LoggerEntryBase lentry, int index)
    {
      if (this.propParent is Service)
        lentry.propSArrayIndex = index;
      else
        lentry.propArrayIndex = index;
    }

    public override void Add(object key, object value) => base.Add(key, value);

    public override bool ContainsKey(object key)
    {
      switch (key)
      {
        case LoggerEntryBase _:
          return base.ContainsKey((object) ((LoggerEntryBase) key).UniqueKey);
        case string _:
          return base.ContainsKey((object) System.Convert.ToUInt64(key));
        default:
          return base.ContainsKey(key);
      }
    }

    public int Add(LoggerEntryBase entry, bool addKeyOnly)
    {
      if (this.propParent is Logger && ((Logger) this.propParent).GlobalMerge)
        ((Base) this.propParent).Service.LoggerEntries.Add(entry, addKeyOnly);
      this.SetArrayIndex(entry);
      this.arrayOfLoggerEntries.Add((object) entry);
      base.Add((object) entry.UniqueKey, (object) entry);
      return 0;
    }

    public int Add(LoggerEntryBase entry)
    {
      if (entry == null || base.ContainsKey((object) entry.UniqueKey))
        return -1;
      if (this.propParent is Logger && ((Logger) this.propParent).GlobalMerge)
        ((Base) this.propParent).Service.LoggerEntries.Add(entry);
      this.SetArrayIndex(entry);
      this.arrayOfLoggerEntries.Add((object) entry);
      base.Add((object) entry.UniqueKey, (object) entry);
      return 0;
    }

    private string GetEntryIDString(uint id) => string.Format("{0:0000000000}", (object) id);

    [CLSCompliant(false)]
    public void Remove(ulong key)
    {
      if (!base.ContainsKey((object) key))
        return;
      int arrayIndex = this.GetArrayIndex((LoggerEntryBase) this[(object) key]);
      if (arrayIndex < this.arrayOfLoggerEntries.Count)
      {
        this.arrayOfLoggerEntries.RemoveAt(arrayIndex);
        this.UpdateArrayIndices(arrayIndex);
      }
      base.Remove((object) key);
    }

    public override void Remove(string key)
    {
      ulong uint64 = System.Convert.ToUInt64(key);
      if (!base.ContainsKey((object) uint64))
        return;
      int arrayIndex = this.GetArrayIndex((LoggerEntryBase) this[(object) uint64]);
      if (arrayIndex < this.arrayOfLoggerEntries.Count)
      {
        this.arrayOfLoggerEntries.RemoveAt(arrayIndex);
        this.UpdateArrayIndices(arrayIndex);
      }
      base.Remove((object) uint64);
    }

    public override void Remove(object key)
    {
      if (key is LoggerEntryBase)
      {
        int arrayIndex = this.GetArrayIndex((LoggerEntryBase) key);
        base.Remove((object) ((LoggerEntryBase) key).UniqueKey);
        if (arrayIndex >= this.arrayOfLoggerEntries.Count)
          return;
        this.arrayOfLoggerEntries.RemoveAt(arrayIndex);
        this.UpdateArrayIndices(arrayIndex);
      }
      else
      {
        ulong uint64 = System.Convert.ToUInt64(key);
        if (!base.ContainsKey((object) uint64))
          return;
        int arrayIndex = this.GetArrayIndex((LoggerEntryBase) this[(object) uint64]);
        if (arrayIndex < this.arrayOfLoggerEntries.Count)
        {
          this.arrayOfLoggerEntries.RemoveAt(arrayIndex);
          this.UpdateArrayIndices(arrayIndex);
        }
        base.Remove((object) uint64);
      }
    }

    protected virtual void SetArrayIndex(LoggerEntryBase eEntry) => eEntry.propArrayIndex = this.arrayOfLoggerEntries.Count;

    protected virtual int GetArrayIndex(LoggerEntryBase eEntry) => eEntry.propArrayIndex;

    protected virtual void UpdateArrayIndices(int idxRemoved)
    {
      for (int index = idxRemoved; index < this.arrayOfLoggerEntries.Count; ++index)
        --((LoggerEntryBase) this.arrayOfLoggerEntries[index]).propArrayIndex;
    }

    public LoggerEntry this[int index] => index < this.arrayOfLoggerEntries.Count ? (LoggerEntry) this.arrayOfLoggerEntries[index] : this[(ulong) index];

    [CLSCompliant(false)]
    public LoggerEntry this[ulong index] => (LoggerEntry) this[(object) index];

    public string XMLData => this.propXMLData;

    internal void Initialize(string xmlData) => this.propXMLData = xmlData;
  }
}
