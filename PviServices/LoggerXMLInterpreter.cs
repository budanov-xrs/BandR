// Decompiled with JetBrains decompiler
// Type: BR.AN.PviServices.LoggerXMLInterpreter
// Assembly: BR.AN.PVIServices, Version=1.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DA853F0A-C070-4E30-B7A7-51D955FB85F6
// Assembly location: C:\Users\ss_six\Desktop\FXE\FXE software integration package\COMET_FXE-PVIServices_Example\Pluto-PVI-trial_01\packages\IToolS.BandR.3.5.58\lib\net20\BR.AN.PVIServices.dll

using System;
using System.Collections;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Xml;

namespace BR.AN.PviServices
{
  internal class LoggerXMLInterpreter
  {
    internal LoggerXMLInterpreter()
    {
    }

    internal int ParseV1000Content(
      Logger logParent,
      string xmlData,
      ref LoggerEntryCollection eventEntries)
    {
      XmlReader xmlReader = (XmlReader) null;
      LoggerEntry entry = (LoggerEntry) null;
      int v1000Content = 0;
      try
      {
        xmlReader = XmlReader.Create((TextReader) new StringReader(xmlData));
        int content = (int) xmlReader.MoveToContent();
        while (!xmlReader.EOF)
        {
          if (xmlReader.NodeType == XmlNodeType.EndElement)
            goto label_25;
label_2:
          if (xmlReader.NodeType == XmlNodeType.Element)
          {
            switch (xmlReader.Name)
            {
              case "Entry":
                string attribute1 = xmlReader.GetAttribute("Timestamp");
                if (attribute1 == null || attribute1.Length == 0)
                  attribute1 = xmlReader.GetAttribute("Time");
                entry = new LoggerEntry((object) logParent, attribute1);
                string attribute2 = xmlReader.GetAttribute("Error");
                if (!string.IsNullOrEmpty(attribute2))
                  entry.propErrorNumber = System.Convert.ToUInt32(attribute2);
                string attribute3 = xmlReader.GetAttribute("Level");
                if (!string.IsNullOrEmpty(attribute3))
                  entry.propLevelType = (LevelType) System.Convert.ToUInt16(attribute3);
                string attribute4 = xmlReader.GetAttribute("TaskName");
                if (!string.IsNullOrEmpty(attribute4))
                  entry.propTask = attribute4;
                string attribute5 = xmlReader.GetAttribute("ID");
                if (!string.IsNullOrEmpty(attribute5))
                {
                  entry.propInternID = System.Convert.ToUInt32(attribute5);
                  entry.UpdateUKey();
                }
                string attribute6 = xmlReader.GetAttribute("Info");
                if (!string.IsNullOrEmpty(attribute6))
                {
                  if (attribute6.ToLower().IndexOf("x") == -1)
                    entry.propErrorInfo = System.Convert.ToUInt32(attribute6);
                  else
                    entry.propErrorInfo = System.Convert.ToUInt32(attribute6, 16);
                }
                eventEntries.Add((LoggerEntryBase) entry);
                break;
              case "Text":
                string str = xmlReader.ReadElementString();
                entry.propErrorText = str;
                break;
              case "Binary":
                string hex = xmlReader.ReadElementString();
                entry.propBinary = HexConvert.ToBytesNoSwap(hex);
                if (((int) entry.propErrorInfo & 2) != 0 || ((int) entry.propErrorInfo & 1) != 0)
                {
                  entry.GetExceptionData();
                  break;
                }
                break;
            }
          }
          xmlReader.Read();
          continue;
label_25:
          if (!(xmlReader.Name == "Logger"))
            goto label_2;
          else
            break;
        }
      }
      catch
      {
        v1000Content = 12054;
      }
      finally
      {
        xmlReader?.Close();
      }
      return v1000Content;
    }

    internal uint DecodeV1010EventID(
      int eventID,
      ref int severityCode,
      ref int customerCode,
      ref int facilityCode)
    {
      uint num = (uint) (eventID & (int) ushort.MaxValue);
      int[] source = new int[2];
      byte[] destination = new byte[4];
      source[0] = eventID;
      IntPtr hMemory = PviMarshal.AllocHGlobal(4);
      Marshal.Copy(source, 0, hMemory, 1);
      Marshal.Copy(hMemory, destination, 0, 4);
      severityCode = (int) destination[3] >> 6;
      customerCode = 32 == ((int) destination[3] & 32) ? 1 : 0;
      facilityCode = (((int) destination[3] & 15) << 8) + (int) destination[2];
      if (1 == facilityCode && customerCode == 0)
        num = (uint) (eventID & 131071);
      else if (facilityCode == 0 || 1 == customerCode)
        num = (uint) (eventID & 268435455);
      PviMarshal.FreeHGlobal(ref hMemory);
      return num;
    }

    private void Geti386ExceptionData(LoggerEntry logEntry, byte[] binData)
    {
      logEntry.propBinary = binData;
      logEntry.GetExceptionData();
    }

    private void GetARMExceptionData(LoggerEntry logEntry, byte[] binData)
    {
      logEntry.propBinary = binData;
      logEntry.GetExceptionData();
    }

    private void GetStringData(LoggerEntry logEntry, byte[] binData)
    {
      int index1 = 0;
      if (0 >= binData.Length)
        return;
      logEntry.propEventData = (object) new ArrayList();
      logEntry.propEventDataType = EventDataTypes.ASCIIStrings;
      string str;
      for (; index1 < binData.Length; index1 += str.Length + 1)
      {
        str = Encoding.ASCII.GetString(binData, index1, binData.Length - (index1 + 1));
        string[] strArray = str.Split(new char[1]);
        for (int index2 = 0; index2 < strArray.GetLength(0); ++index2)
          ((ArrayList) logEntry.propEventData).Add((object) strArray.GetValue(index2).ToString());
      }
    }

    private int GetStrsEntryCount(byte[] binData, ref int iOffset)
    {
      iOffset = 0;
      int strsEntryCount;
      if (128 != ((int) binData[1] & 128))
      {
        iOffset = 2;
        strsEntryCount = (int) binData[1];
      }
      else if (240 == ((int) binData[1] & 240))
      {
        iOffset = 5;
        strsEntryCount = (((int) binData[1] & 7) << 18) + (((int) binData[2] & 63) << 12) + (((int) binData[3] & 63) << 6) + ((int) binData[4] & 63);
      }
      else if (224 == ((int) binData[1] & 224))
      {
        iOffset = 4;
        strsEntryCount = (((int) binData[1] & 15) << 12) + (((int) binData[2] & 63) << 6) + ((int) binData[3] & 63);
      }
      else if (192 != ((int) binData[1] & 192))
      {
        iOffset = 3;
        strsEntryCount = (((int) binData[1] & 31) << 6) + ((int) binData[2] & 63);
      }
      else
        strsEntryCount = 0;
      return strsEntryCount;
    }

    private int GetPrefixedStrings(LoggerEntry logEntry, byte[] binData, byte binFormat)
    {
      int iOffset = 0;
      if ((byte) 136 != binFormat && (byte) 138 != binFormat && (byte) 139 != binFormat && (byte) 140 != binFormat && (byte) 141 != binFormat)
        return 1;
      if (0 < this.GetStrsEntryCount(binData, ref iOffset))
      {
        switch (binFormat)
        {
          case 136:
            logEntry.propEventData = (object) new ArrayList();
            logEntry.propEventDataType = EventDataTypes.MBCSStrings;
            string str1;
            for (; iOffset < binData.Length; iOffset += str1.Length + 1)
            {
              str1 = Encoding.Default.GetString(binData, iOffset, binData.Length - iOffset);
              ((ArrayList) logEntry.propEventData).Add((object) str1);
            }
            break;
          case 138:
            logEntry.propEventData = (object) new ArrayList();
            logEntry.propEventDataType = EventDataTypes.UTF16Strings;
            string str2;
            for (; iOffset < binData.Length; iOffset += str2.Length + 1)
            {
              str2 = Encoding.Unicode.GetString(binData, iOffset, binData.Length - 1);
              ((ArrayList) logEntry.propEventData).Add((object) str2);
            }
            break;
          case 139:
            logEntry.propEventData = (object) new ArrayList();
            logEntry.propEventDataType = EventDataTypes.UTF16StringsBE;
            string str3;
            for (; iOffset < binData.Length; iOffset += str3.Length + 1)
            {
              str3 = Encoding.Unicode.GetString(binData, iOffset, binData.Length - 1);
              ((ArrayList) logEntry.propEventData).Add((object) str3);
            }
            break;
          case 140:
            logEntry.propEventData = (object) new ArrayList();
            logEntry.propEventDataType = EventDataTypes.UTF32Strings;
            string str4;
            for (; iOffset < binData.Length; iOffset += str4.Length + 1)
            {
              str4 = Encoding.UTF32.GetString(binData, iOffset, binData.Length - 1);
              ((ArrayList) logEntry.propEventData).Add((object) str4);
            }
            break;
          case 141:
            logEntry.propEventData = (object) new ArrayList();
            logEntry.propEventDataType = EventDataTypes.UTF32Strings;
            string str5;
            for (; iOffset < binData.Length; iOffset += str5.Length + 1)
            {
              str5 = Encoding.UTF32.GetString(binData, iOffset, binData.Length - 1);
              ((ArrayList) logEntry.propEventData).Add((object) str5);
            }
            break;
          default:
            return 1;
        }
      }
      return 0;
    }

    private int GetPrefixedFloatingPoints(LoggerEntry logEntry, byte[] binData, byte binFormat)
    {
      switch (binFormat)
      {
        case 92:
          if (4 < binData.GetLength(0))
          {
            logEntry.propEventData = (object) PviMarshal.ToFloat32(binData[1], binData[2], binData[3], binData[4]);
            logEntry.propEventDataType = EventDataTypes.Float32;
            break;
          }
          break;
        case 93:
          if (4 < binData.GetLength(0))
          {
            logEntry.propEventData = (object) PviMarshal.ToFloat32(binData[4], binData[3], binData[2], binData[1]);
            logEntry.propEventDataType = EventDataTypes.Float32BE;
            break;
          }
          break;
        case 94:
          if (4 < binData.GetLength(0))
          {
            logEntry.propEventData = (object) PviMarshal.ToDouble64(binData[1], binData[2], binData[3], binData[4], binData[5], binData[6], binData[7], binData[8]);
            logEntry.propEventDataType = EventDataTypes.Double64;
            break;
          }
          break;
        case 95:
          if (4 < binData.GetLength(0))
          {
            logEntry.propEventData = (object) PviMarshal.ToDouble64(binData[8], binData[7], binData[6], binData[5], binData[4], binData[3], binData[2], binData[1]);
            logEntry.propEventDataType = EventDataTypes.Double64;
            break;
          }
          break;
        default:
          return 1;
      }
      return 0;
    }

    private int GetPrefixedSpecials(LoggerEntry logEntry, byte[] binData, byte binFormat)
    {
      switch (binFormat)
      {
        case 96:
          logEntry.propEventData = (object) false;
          logEntry.propEventDataType = EventDataTypes.BooleanFalse;
          break;
        case 97:
          logEntry.propEventData = (object) true;
          logEntry.propEventDataType = EventDataTypes.BooleanTrue;
          break;
        case 100:
          if (4 < binData.GetLength(0))
          {
            logEntry.propEventData = (object) PviMarshal.ToInt32(binData[1], binData[2], binData[3], binData[4]);
            logEntry.propEventDataType = EventDataTypes.MemAddress;
            break;
          }
          break;
        case 101:
          if (4 < binData.GetLength(0))
          {
            logEntry.propEventData = (object) PviMarshal.ToInt32(binData[4], binData[3], binData[2], binData[1]);
            logEntry.propEventDataType = EventDataTypes.MemAddressBE;
            break;
          }
          break;
        default:
          return 1;
      }
      return 0;
    }

    private int GetPrefixedCharacters(LoggerEntry logEntry, byte[] binData, byte binFormat)
    {
      switch (binFormat)
      {
        case 80:
          if (1 < binData.GetLength(0))
          {
            logEntry.propEventData = (object) System.Convert.ToChar(binData[1]);
            logEntry.propEventDataType = EventDataTypes.AsciiChar;
            break;
          }
          break;
        case 81:
          if (1 < binData.GetLength(0))
          {
            logEntry.propEventData = (object) System.Convert.ToChar(binData[1]);
            logEntry.propEventDataType = EventDataTypes.AnsiChar;
            break;
          }
          break;
        case 82:
          if (2 < binData.GetLength(0))
          {
            byte[] bytes = new byte[2]
            {
              binData[1],
              binData[2]
            };
            logEntry.propEventData = (object) Encoding.Unicode.GetString(bytes, 0, 2);
            logEntry.propEventDataType = EventDataTypes.UTF16Char;
            break;
          }
          break;
        case 83:
          if (2 < binData.GetLength(0))
          {
            byte[] bytes = new byte[2]
            {
              binData[2],
              binData[1]
            };
            logEntry.propEventData = (object) Encoding.Unicode.GetString(bytes, 0, 2);
            logEntry.propEventDataType = EventDataTypes.UTF16CharBE;
            break;
          }
          break;
        case 84:
          if (4 < binData.GetLength(0))
          {
            byte[] bytes = new byte[4]
            {
              binData[1],
              binData[2],
              binData[3],
              binData[4]
            };
            logEntry.propEventData = (object) Encoding.UTF32.GetString(bytes);
            logEntry.propEventDataType = EventDataTypes.UTF32Char;
            break;
          }
          break;
        case 85:
          if (4 < binData.GetLength(0))
          {
            byte[] bytes = new byte[4]
            {
              binData[4],
              binData[3],
              binData[2],
              binData[1]
            };
            logEntry.propEventData = (object) Encoding.UTF32.GetString(bytes);
            logEntry.propEventDataType = EventDataTypes.UTF32Char;
            break;
          }
          break;
        default:
          return 1;
      }
      return 0;
    }

    private int GetPrefixedIntegers(LoggerEntry logEntry, byte[] binData, byte binFormat)
    {
      switch (binFormat)
      {
        case 64:
          if (1 < binData.GetLength(0))
          {
            logEntry.propEventData = (object) System.Convert.ToByte(binData[1]);
            logEntry.propEventDataType = EventDataTypes.UInt8;
            break;
          }
          break;
        case 65:
          if (1 < binData.GetLength(0))
          {
            logEntry.propEventData = (object) (sbyte) System.Convert.ToByte(binData[1]);
            logEntry.propEventDataType = EventDataTypes.Int8;
            break;
          }
          break;
        case 66:
          if (2 < binData.GetLength(0))
          {
            logEntry.propEventData = (object) PviMarshal.ToUInt16(binData[1], binData[2]);
            logEntry.propEventDataType = EventDataTypes.UInt16;
            break;
          }
          break;
        case 67:
          if (2 < binData.GetLength(0))
          {
            logEntry.propEventData = (object) PviMarshal.ToInt16(binData[1], binData[2]);
            logEntry.propEventDataType = EventDataTypes.Int16;
            break;
          }
          break;
        case 68:
          if (4 < binData.GetLength(0))
          {
            logEntry.propEventData = (object) PviMarshal.ToUInt32(binData[1], binData[2], binData[3], binData[4]);
            logEntry.propEventDataType = EventDataTypes.UInt32;
            break;
          }
          break;
        case 69:
          if (4 < binData.GetLength(0))
          {
            logEntry.propEventData = (object) PviMarshal.ToInt32(binData[1], binData[2], binData[3], binData[4]);
            logEntry.propEventDataType = EventDataTypes.Int32;
            break;
          }
          break;
        case 70:
          if (8 < binData.GetLength(0))
          {
            logEntry.propEventData = (object) PviMarshal.ToUInt64(binData[1], binData[2], binData[3], binData[4], binData[5], binData[6], binData[7], binData[8]);
            logEntry.propEventDataType = EventDataTypes.UInt64;
            break;
          }
          break;
        case 71:
          if (8 < binData.GetLength(0))
          {
            logEntry.propEventData = (object) PviMarshal.ToInt64(binData[1], binData[2], binData[3], binData[4], binData[5], binData[6], binData[7], binData[8]);
            logEntry.propEventDataType = EventDataTypes.Int64;
            break;
          }
          break;
        case 72:
          if (1 < binData.GetLength(0))
          {
            logEntry.propEventData = (object) System.Convert.ToByte(binData[1]);
            logEntry.propEventDataType = EventDataTypes.UInt8BE;
            break;
          }
          break;
        case 73:
          if (1 < binData.GetLength(0))
          {
            logEntry.propEventData = (object) (sbyte) System.Convert.ToByte(binData[1]);
            logEntry.propEventDataType = EventDataTypes.Int8BE;
            break;
          }
          break;
        case 74:
          if (2 < binData.GetLength(0))
          {
            logEntry.propEventData = (object) PviMarshal.ToUInt16(binData[2], binData[1]);
            logEntry.propEventDataType = EventDataTypes.UInt16BE;
            break;
          }
          break;
        case 75:
          if (2 < binData.GetLength(0))
          {
            logEntry.propEventData = (object) PviMarshal.ToInt16(binData[2], binData[1]);
            logEntry.propEventDataType = EventDataTypes.Int16BE;
            break;
          }
          break;
        case 76:
          if (4 < binData.GetLength(0))
          {
            logEntry.propEventData = (object) PviMarshal.ToUInt32(binData[4], binData[3], binData[2], binData[1]);
            logEntry.propEventDataType = EventDataTypes.UInt32BE;
            break;
          }
          break;
        case 77:
          if (4 < binData.GetLength(0))
          {
            logEntry.propEventData = (object) PviMarshal.ToInt32(binData[4], binData[3], binData[2], binData[1]);
            logEntry.propEventDataType = EventDataTypes.Int32BE;
            break;
          }
          break;
        case 78:
          if (8 < binData.GetLength(0))
          {
            logEntry.propEventData = (object) PviMarshal.ToUInt64(binData[8], binData[7], binData[6], binData[5], binData[4], binData[3], binData[2], binData[1]);
            logEntry.propEventDataType = EventDataTypes.UInt64BE;
            break;
          }
          break;
        case 79:
          if (8 < binData.GetLength(0))
          {
            logEntry.propEventData = (object) PviMarshal.ToInt64(binData[8], binData[7], binData[6], binData[5], binData[4], binData[3], binData[2], binData[1]);
            logEntry.propEventDataType = EventDataTypes.Int64BE;
            break;
          }
          break;
        default:
          return 1;
      }
      return 0;
    }

    private void GetArLoggerData(LoggerEntry logEntry, byte[] binData)
    {
      logEntry.propEventDataType = EventDataTypes.ArLoggerAPI;
      if (0 >= binData.Length)
        return;
      logEntry.propEventData = (object) PviMarshal.ToAnsiString(binData);
      logEntry.propBinary = new byte[binData.Length - (1 + logEntry.propEventData.ToString().Length)];
      int index1 = 0;
      for (int index2 = logEntry.propEventData.ToString().Length + 1; index2 < binData.Length; ++index2)
      {
        logEntry.propBinary.SetValue(binData.GetValue(index2), index1);
        ++index1;
      }
    }

    private void GetPrefixedData(LoggerEntry logEntry, byte[] binData)
    {
      int index1 = 0;
      if (0 >= binData.Length)
        return;
      byte binFormat = binData[0];
      if (binFormat < (byte) 16)
      {
        if ((byte) 0 >= binData[0])
          return;
        logEntry.propEventData = (object) new ArrayList();
        logEntry.propEventDataType = EventDataTypes.ASCIIStrings;
        string str;
        for (; index1 < binData.Length; index1 += str.Length + 1)
        {
          str = Encoding.ASCII.GetString(binData, index1, binData.Length - 1);
          ((ArrayList) logEntry.propEventData).Add((object) str);
        }
      }
      else if (binFormat > (byte) 16 && binFormat < (byte) 32)
      {
        if ((byte) 0 >= binData[0])
          return;
        logEntry.propEventData = (object) new ArrayList();
        logEntry.propEventDataType = EventDataTypes.ANSIStrings;
        string str;
        for (; index1 < binData.Length; index1 += str.Length + 1)
        {
          str = Encoding.ASCII.GetString(binData, index1, binData.Length - 1);
          ((ArrayList) logEntry.propEventData).Add((object) str);
        }
      }
      else if (binFormat > (byte) 127 && binFormat < (byte) 136)
      {
        int num = (int) binData[0] - 128;
        if (0 >= num)
          return;
        logEntry.propEventData = (object) new ArrayList();
        logEntry.propEventDataType = EventDataTypes.BytesLigttleEndian;
        for (int index2 = 1; index2 < num && index2 < binData.GetLength(0); ++index2)
          ((ArrayList) logEntry.propEventData).Add((object) binData[index2]);
      }
      else
      {
        int num = this.GetPrefixedIntegers(logEntry, binData, binFormat);
        if (num != 0)
          num = this.GetPrefixedCharacters(logEntry, binData, binFormat);
        if (num != 0)
          num = this.GetPrefixedSpecials(logEntry, binData, binFormat);
        if (num != 0)
          num = this.GetPrefixedFloatingPoints(logEntry, binData, binFormat);
        if (num != 0)
          num = this.GetPrefixedStrings(logEntry, binData, binFormat);
        if (num == 0)
          return;
        logEntry.propBinary = binData;
      }
    }

    internal void DecodeV1010AdditionalData(
      LoggerEntry logEntry,
      int additionalDataFormat,
      int numOfBytes,
      byte[] byteBuffer)
    {
      try
      {
        logEntry.propBinary = byteBuffer;
        switch (additionalDataFormat)
        {
          case 1:
            this.GetStringData(logEntry, byteBuffer);
            break;
          case 2:
            this.GetPrefixedData(logEntry, byteBuffer);
            break;
          case 3:
            this.GetArLoggerData(logEntry, byteBuffer);
            break;
          case 254:
            this.Geti386ExceptionData(logEntry, byteBuffer);
            break;
          case (int) byte.MaxValue:
            this.GetARMExceptionData(logEntry, byteBuffer);
            break;
          default:
            logEntry.propBinary = byteBuffer;
            break;
        }
      }
      catch
      {
        logEntry.propBinary = byteBuffer;
      }
    }

    internal void DecodeV1010AdditionalData(
      LoggerEntry logEntry,
      int additionalDataFormat,
      string attrVal)
    {
      try
      {
        byte[] bytesNoSwap = HexConvert.ToBytesNoSwap(attrVal);
        if (bytesNoSwap == null)
          return;
        this.DecodeV1010AdditionalData(logEntry, additionalDataFormat, bytesNoSwap.Length, bytesNoSwap);
      }
      catch
      {
        logEntry.propBinary = (byte[]) null;
      }
    }

    internal int ParseV1010Content(
      Logger logParent,
      string xmlData,
      ref LoggerEntryCollection eventEntries)
    {
      XmlReader xmlReader = (XmlReader) null;
      LoggerEntry loggerEntry = (LoggerEntry) null;
      int severityCode = 0;
      int customerCode = 0;
      int facilityCode = 0;
      byte[] numArray = new byte[2048];
      try
      {
        xmlReader = XmlReader.Create((TextReader) new StringReader(xmlData));
        int content = (int) xmlReader.MoveToContent();
        while (!xmlReader.EOF)
        {
          if (xmlReader.NodeType == XmlNodeType.EndElement)
            goto label_22;
label_2:
          if (xmlReader.NodeType == XmlNodeType.Element)
          {
            switch (xmlReader.Name)
            {
              case "Entry":
                string attribute1 = xmlReader.GetAttribute("TimestampUtc");
                loggerEntry = new LoggerEntry((object) logParent, attribute1);
                string attribute2 = xmlReader.GetAttribute("EventId");
                if (!string.IsNullOrEmpty(attribute2))
                {
                  loggerEntry.propEventId = System.Convert.ToInt32(attribute2);
                  loggerEntry.propErrorNumber = this.DecodeV1010EventID(loggerEntry.propEventId, ref severityCode, ref customerCode, ref facilityCode);
                  loggerEntry.propLevelType = (LevelType) severityCode;
                  loggerEntry.propCustomerCode = customerCode;
                  loggerEntry.propFacilityCode = facilityCode;
                }
                string attribute3 = xmlReader.GetAttribute("ObjectId");
                if (!string.IsNullOrEmpty(attribute3))
                  loggerEntry.propTask = attribute3;
                string attribute4 = xmlReader.GetAttribute("RecordId");
                if (!string.IsNullOrEmpty(attribute4))
                {
                  loggerEntry.propInternID = System.Convert.ToUInt32(attribute4);
                  loggerEntry.UpdateUKey();
                }
                string attribute5 = xmlReader.GetAttribute("AddDataFormat");
                loggerEntry._AdditionalDataFormat = 0;
                if (!string.IsNullOrEmpty(attribute5))
                  loggerEntry._AdditionalDataFormat = System.Convert.ToInt32(attribute5);
                string attribute6 = xmlReader.GetAttribute("OriginRecordId");
                if (!string.IsNullOrEmpty(attribute6))
                  loggerEntry.propOriginRecordId = System.Convert.ToInt32(attribute6);
                eventEntries.Add((LoggerEntryBase) loggerEntry);
                break;
              case "AddData":
                if (loggerEntry != null)
                {
                  if (xmlReader.CanReadBinaryContent && xmlReader.NodeType != XmlNodeType.Element)
                  {
                    int numOfBytes = xmlReader.ReadElementContentAsBinHex(numArray, 0, numArray.Length);
                    this.DecodeV1010AdditionalData(loggerEntry, loggerEntry.AdditionalDataFormat, numOfBytes, numArray);
                    break;
                  }
                  string attrVal = xmlReader.ReadElementString();
                  this.DecodeV1010AdditionalData(loggerEntry, loggerEntry.AdditionalDataFormat, attrVal);
                  break;
                }
                break;
            }
          }
          xmlReader.Read();
          continue;
label_22:
          if (!(xmlReader.Name == "Logger"))
            goto label_2;
          else
            break;
        }
      }
      catch
      {
        return 12054;
      }
      finally
      {
        xmlReader?.Close();
      }
      return 0;
    }

    internal int ParseXMLContent(
      Logger logParent,
      string xmlData,
      ref LoggerEntryCollection eventEntries,
      ref uint readVersion)
    {
      uint num1 = 0;
      int num2 = xmlData.IndexOf("Version=\"");
      if (num2 > 0)
        num1 = System.Convert.ToUInt32(xmlData.Substring(num2 + 9, xmlData.IndexOf('"', num2 + 9) - (num2 + 9)));
      else if (-1 != xmlData.IndexOf("Entry ID=\""))
        num1 = 4096U;
      else if (-1 != xmlData.IndexOf("Entry RecordId=\""))
        num1 = 4112U;
      int xmlContent;
      switch (num1)
      {
        case 4096:
          xmlContent = this.ParseV1000Content(logParent, xmlData, ref eventEntries);
          break;
        case 4112:
          xmlContent = this.ParseV1010Content(logParent, xmlData, ref eventEntries);
          break;
        default:
          xmlContent = 12054;
          break;
      }
      readVersion = num1;
      return xmlContent;
    }
  }
}
