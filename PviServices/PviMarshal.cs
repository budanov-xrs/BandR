// Decompiled with JetBrains decompiler
// Type: BR.AN.PviServices.PviMarshal
// Assembly: BR.AN.PVIServices, Version=1.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DA853F0A-C070-4E30-B7A7-51D955FB85F6
// Assembly location: C:\Users\ss_six\Desktop\FXE\FXE software integration package\COMET_FXE-PVIServices_Example\Pluto-PVI-trial_01\packages\IToolS.BandR.3.5.58\lib\net20\BR.AN.PVIServices.dll

using System;
using System.Collections;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace BR.AN.PviServices
{
  internal class PviMarshal
  {
    private const uint LMEM_FIXED = 0;
    private const uint LMEM_ZEROINIT = 64;
    private const uint LPTR = 64;

    [DllImport("coredll.dll")]
    internal static extern IntPtr LocalAlloc(uint uFlags, uint uBytes);

    [DllImport("coredll.dll")]
    internal static extern IntPtr LocalFree(IntPtr hMem);

    internal static string PtrToStringAnsi(IntPtr cb, int size) => Marshal.PtrToStringAnsi(cb, size);

    internal static string PtrToStringAnsi(IntPtr cb, uint size) => PviMarshal.PtrToStringAnsi(cb, (int) size);

    internal static string ToAnsiStringNoTermination(IntPtr cb, int size)
    {
      string stringNoTermination = "";
      if (IntPtr.Zero != cb)
      {
        byte[] destination = new byte[size];
        Marshal.Copy(cb, destination, 0, size);
        for (int index = 0; index < size; ++index)
        {
          byte num = destination[index];
          if (num != (byte) 0)
            stringNoTermination += (string) (object) (char) num;
          else
            break;
        }
      }
      return stringNoTermination;
    }

    internal static string ToAnsiString(IntPtr cb, int size) => PviMarshal.ToAnsiString(cb, (uint) size);

    internal static string ToAnsiString(IntPtr cb, uint size)
    {
      string ansiString = "";
      if (IntPtr.Zero != cb && 0U < size)
      {
        byte[] destination = new byte[(IntPtr) size];
        Marshal.Copy(cb, destination, 0, (int) size);
        for (int index = 0; (long) index < (long) size; ++index)
        {
          byte num = destination[index];
          if (num != (byte) 0)
            ansiString += (string) (object) (char) num;
          else
            break;
        }
      }
      return ansiString;
    }

    internal static void GetVersionInfos(IntPtr pVersion, int dataLen, ref Hashtable vInfos) => PviMarshal.GetVersionInfos(pVersion, dataLen, ref vInfos, "");

    internal static void GetVersionInfos(
      IntPtr pVersion,
      int dataLen,
      ref Hashtable vInfos,
      string addToKey)
    {
      if (0 >= dataLen)
        return;
      string[] strArray = PviMarshal.ToAnsiString(pVersion, dataLen).Split('\n');
      for (int index = 0; index < strArray.GetLength(0); ++index)
      {
        string str1 = strArray.GetValue(index).ToString();
        if (0 < str1.Length)
        {
          int length = str1.LastIndexOf(' ');
          if (-1 != length)
          {
            string key = addToKey + str1.Substring(0, length);
            string str2 = str1.Substring(length + 1);
            vInfos.Add((object) key, (object) str2);
          }
          else
          {
            string key = addToKey;
            string str3 = str1;
            vInfos.Add((object) key, (object) str3);
          }
        }
      }
    }

    internal static string ToWString(IntPtr pBuffer, uint bufferLen) => PviMarshal.ToWString(pBuffer, (int) bufferLen);

    internal static string ToWString(IntPtr pBuffer, int bufferLen) => Marshal.PtrToStringUni(pBuffer, bufferLen / 2 - 1);

    internal static string ToWString(byte[] bBuffer, int byteOffset, int strLen)
    {
      IntPtr hMemory = PviMarshal.AllocHGlobal(strLen);
      Marshal.Copy(bBuffer, byteOffset, hMemory, strLen);
      string stringUni = Marshal.PtrToStringUni(hMemory);
      PviMarshal.FreeHGlobal(ref hMemory);
      return stringUni;
    }

    internal static string ToAnsiString(byte[] bytes) => PviMarshal.ToAnsiString(bytes, 0, bytes.GetLength(0));

    internal static string ToAnsiString(byte[] bytes, int offset, int len)
    {
      string ansiString = "";
      if (bytes != null)
      {
        for (int index = offset; index < offset + len && index < bytes.GetLength(0); ++index)
        {
          byte num = (byte) bytes.GetValue(index);
          if (num != (byte) 0)
            ansiString += (string) (object) (char) num;
          else
            break;
        }
      }
      return ansiString;
    }

    internal static string PtrToStringAnsi(IntPtr cb) => Marshal.PtrToStringAnsi(cb);

    internal static IntPtr AllocCoTaskMem(int cb)
    {
      IntPtr num = new IntPtr();
      return Marshal.AllocHGlobal(cb);
    }

    internal static IntPtr AllocHGlobal(IntPtr cb)
    {
      IntPtr num = new IntPtr();
      return Marshal.AllocHGlobal(cb);
    }

    internal static IntPtr AllocHGlobal(int cb)
    {
      IntPtr num = new IntPtr();
      return 0 < cb ? Marshal.AllocHGlobal(cb) : IntPtr.Zero;
    }

    internal static IntPtr AllocHGlobal(uint cb)
    {
      IntPtr num = new IntPtr();
      return Marshal.AllocHGlobal((int) cb);
    }

    internal static void FreeHGlobal(ref IntPtr hMemory)
    {
      if (hMemory != IntPtr.Zero)
        Marshal.FreeHGlobal(hMemory);
      hMemory = IntPtr.Zero;
    }

    internal static long ReadInt64(IntPtr ptr) => PviMarshal.ReadInt64(ptr, 0);

    internal static long ReadInt64(IntPtr ptr, int offset) => Marshal.ReadInt64(ptr, offset);

    internal static ulong ReadUInt64(IntPtr ptr, ref int offset)
    {
      long num = Marshal.ReadInt64(ptr, offset);
      offset += 8;
      return (ulong) num;
    }

    internal static byte ReadByte(IntPtr ptr, ref int offset)
    {
      byte num = Marshal.ReadByte(ptr, offset);
      ++offset;
      return num;
    }

    internal static ushort ReadUInt16(IntPtr ptr, ref int offset)
    {
      short num = Marshal.ReadInt16(ptr, offset);
      offset += 2;
      return (ushort) num;
    }

    internal static uint ReadUInt32(IntPtr ptr, ref int offset)
    {
      int num = Marshal.ReadInt32(ptr, offset);
      offset += 4;
      return (uint) num;
    }

    internal static uint WmMsgToUInt32(IntPtr ptr)
    {
      int uint32 = 0;
      if (IntPtr.Zero != ptr)
        uint32 = (int) ptr;
      return (uint) uint32;
    }

    internal static void WmMsgToInt32(ref Message msg, ref int iWParam, ref int iLParam)
    {
      iWParam = (int) msg.WParam;
      iLParam = (int) msg.LParam;
    }

    internal static void WriteSByte(IntPtr ptr, sbyte val) => PviMarshal.WriteSByte(ptr, 0, val);

    internal static void WriteSByte(IntPtr ptr, int offset, sbyte val) => Marshal.WriteByte(ptr, offset, (byte) val);

    internal static void WriteUInt16(IntPtr ptr, ushort val) => PviMarshal.WriteUInt16(ptr, 0, val);

    internal static void WriteUInt16(IntPtr ptr, int offset, ushort val) => Marshal.WriteInt16(ptr, offset, (short) val);

    internal static void WriteUInt32(IntPtr ptr, uint val) => PviMarshal.WriteUInt32(ptr, 0, val);

    internal static void WriteUInt32(IntPtr ptr, int offset, uint val) => Marshal.WriteInt32(ptr, offset, (int) val);

    internal static void WriteUInt64(IntPtr ptr, ulong val) => PviMarshal.WriteUInt64(ptr, 0, val);

    internal static void WriteUInt64(IntPtr ptr, int offset, ulong val) => PviMarshal.WriteInt64(ptr, offset, (long) val);

    internal static void WriteInt64(IntPtr ptr, long val) => PviMarshal.WriteInt64(ptr, 0, val);

    internal static void WriteInt64(IntPtr ptr, int offset, long val) => Marshal.WriteInt64(ptr, offset, val);

    internal static void WriteSingle(IntPtr ptr, int offset, float val)
    {
      float[] source = new float[1]{ val };
      byte[] destination = new byte[4];
      IntPtr hMemory = PviMarshal.AllocHGlobal(4);
      Marshal.Copy(source, 0, hMemory, 1);
      Marshal.Copy(hMemory, destination, 0, 4);
      for (int index = 0; index < 4; ++index)
        Marshal.WriteByte(ptr, offset + index, destination[index]);
      PviMarshal.FreeHGlobal(ref hMemory);
    }

    internal static void WriteSingle(IntPtr ptr, int offset, Value val)
    {
      if (IntPtr.Zero != val.pData && 4 == val.DataSize)
      {
        byte[] destination = new byte[4];
        Marshal.Copy(val.pData, destination, 0, 4);
        for (int index = 0; index < 4; ++index)
          Marshal.WriteByte(ptr, offset + index, destination[index]);
      }
      else
        PviMarshal.WriteSingle(ptr, offset, System.Convert.ToSingle(val.ToString()));
    }

    internal static void WriteDouble(IntPtr ptr, int offset, double val)
    {
      double[] source = new double[1]{ val };
      byte[] destination = new byte[8];
      IntPtr hMemory = PviMarshal.AllocHGlobal(8);
      Marshal.Copy(source, 0, hMemory, 1);
      Marshal.Copy(hMemory, destination, 0, 8);
      for (int index = 0; index < 8; ++index)
        Marshal.WriteByte(ptr, offset + index, destination[index]);
      PviMarshal.FreeHGlobal(ref hMemory);
    }

    internal static void WriteDouble(IntPtr ptr, int offset, Value val)
    {
      if (IntPtr.Zero != val.pData && 8 == val.DataSize)
      {
        byte[] destination = new byte[8];
        Marshal.Copy(val.pData, destination, 0, 8);
        for (int index = 0; index < 8; ++index)
          Marshal.WriteByte(ptr, offset + index, destination[index]);
      }
      else
        PviMarshal.WriteDouble(ptr, offset, System.Convert.ToDouble(val.ToString()));
    }

    internal static void WriteString(IntPtr ptr, int offset, string val)
    {
      for (int index = 0; index < val.Length; ++index)
        Marshal.WriteByte(ptr, offset + index, (byte) val[index]);
    }

    internal static int HighDWord(long value)
    {
      long[] source = new long[2]{ value, 0L };
      IntPtr hMemory = PviMarshal.AllocHGlobal(8);
      Marshal.Copy(source, 0, hMemory, 1);
      int num = Marshal.ReadInt32(hMemory, 4);
      PviMarshal.FreeHGlobal(ref hMemory);
      return num;
    }

    internal static uint HighDWord(ulong value)
    {
      long[] source = new long[2]{ (long) value, 0L };
      IntPtr hMemory = PviMarshal.AllocHGlobal(8);
      Marshal.Copy(source, 0, hMemory, 1);
      uint num = (uint) Marshal.ReadInt32(hMemory, 4);
      PviMarshal.FreeHGlobal(ref hMemory);
      return num;
    }

    internal static ushort HighWord(uint value)
    {
      int[] source = new int[2]{ (int) value, 0 };
      IntPtr hMemory = PviMarshal.AllocHGlobal(4);
      Marshal.Copy(source, 0, hMemory, 1);
      ushort num = (ushort) Marshal.ReadInt16(hMemory, 2);
      PviMarshal.FreeHGlobal(ref hMemory);
      return num;
    }

    internal static short HighWord(int value)
    {
      int[] source = new int[2]{ value, 0 };
      IntPtr hMemory = PviMarshal.AllocHGlobal(4);
      Marshal.Copy(source, 0, hMemory, 1);
      short num = Marshal.ReadInt16(hMemory, 2);
      PviMarshal.FreeHGlobal(ref hMemory);
      return num;
    }

    internal static short LowWord(int value)
    {
      int[] source = new int[2]{ value, 0 };
      IntPtr hMemory = PviMarshal.AllocHGlobal(4);
      Marshal.Copy(source, 0, hMemory, 1);
      short num = Marshal.ReadInt16(hMemory, 0);
      PviMarshal.FreeHGlobal(ref hMemory);
      return num;
    }

    internal static ushort LowWord(uint value)
    {
      int[] source = new int[2]{ (int) value, 0 };
      IntPtr hMemory = PviMarshal.AllocHGlobal(4);
      Marshal.Copy(source, 0, hMemory, 1);
      ushort num = (ushort) Marshal.ReadInt16(hMemory, 0);
      PviMarshal.FreeHGlobal(ref hMemory);
      return num;
    }

    internal static uint LowDWord(ulong value)
    {
      long[] source = new long[2]{ (long) value, 0L };
      IntPtr hMemory = PviMarshal.AllocHGlobal(8);
      Marshal.Copy(source, 0, hMemory, 1);
      uint num = (uint) Marshal.ReadInt32(hMemory, 0);
      PviMarshal.FreeHGlobal(ref hMemory);
      return num;
    }

    internal static int LowDWord(long value)
    {
      long[] source = new long[2]{ value, 0L };
      IntPtr hMemory = PviMarshal.AllocHGlobal(8);
      Marshal.Copy(source, 0, hMemory, 1);
      int num = Marshal.ReadInt32(hMemory, 0);
      PviMarshal.FreeHGlobal(ref hMemory);
      return num;
    }

    internal static ulong ToDWord(ushort hH, ushort lH, ushort hL, ushort lL)
    {
      short[] source = new short[4]
      {
        (short) lL,
        (short) hL,
        (short) lH,
        (short) hH
      };
      IntPtr hMemory = PviMarshal.AllocHGlobal(8);
      Marshal.Copy(source, 0, hMemory, 4);
      ulong dword = (ulong) PviMarshal.ReadInt64(hMemory);
      PviMarshal.FreeHGlobal(ref hMemory);
      return dword;
    }

    internal static long ToDWord(short hH, short lH, short hL, short lL)
    {
      short[] source = new short[4]{ lL, hL, lH, hH };
      IntPtr hMemory = PviMarshal.AllocHGlobal(8);
      Marshal.Copy(source, 0, hMemory, 4);
      long dword = PviMarshal.ReadInt64(hMemory);
      PviMarshal.FreeHGlobal(ref hMemory);
      return dword;
    }

    internal static ulong ToDWord(uint high, uint low)
    {
      int[] source = new int[2]{ (int) low, (int) high };
      IntPtr hMemory = PviMarshal.AllocHGlobal(8);
      Marshal.Copy(source, 0, hMemory, 2);
      ulong dword = (ulong) PviMarshal.ReadInt64(hMemory);
      PviMarshal.FreeHGlobal(ref hMemory);
      return dword;
    }

    internal static long ToDWord(int high, int low)
    {
      int[] source = new int[2]{ low, high };
      IntPtr hMemory = PviMarshal.AllocHGlobal(8);
      Marshal.Copy(source, 0, hMemory, 2);
      long dword = PviMarshal.ReadInt64(hMemory);
      PviMarshal.FreeHGlobal(ref hMemory);
      return dword;
    }

    internal static ushort ToUInt16(byte bH, byte bL)
    {
      int offset = 0;
      byte[] source = new byte[2]{ bH, bL };
      IntPtr hMemory = PviMarshal.AllocHGlobal(2);
      Marshal.Copy(source, 0, hMemory, 2);
      ushort uint16 = PviMarshal.ReadUInt16(hMemory, ref offset);
      PviMarshal.FreeHGlobal(ref hMemory);
      return uint16;
    }

    internal static short ToInt16(byte bH, byte bL)
    {
      int ofs = 0;
      byte[] source = new byte[2]{ bH, bL };
      IntPtr hMemory = PviMarshal.AllocHGlobal(2);
      Marshal.Copy(source, 0, hMemory, 2);
      short int16 = Marshal.ReadInt16(hMemory, ofs);
      PviMarshal.FreeHGlobal(ref hMemory);
      return int16;
    }

    internal static uint ToUInt32(byte bHH, byte bLH, byte bHL, byte bLL)
    {
      int offset = 0;
      byte[] source = new byte[4]{ bHH, bLH, bHL, bLL };
      IntPtr hMemory = PviMarshal.AllocHGlobal(4);
      Marshal.Copy(source, 0, hMemory, 4);
      uint uint32 = PviMarshal.ReadUInt32(hMemory, ref offset);
      PviMarshal.FreeHGlobal(ref hMemory);
      return uint32;
    }

    internal static int ToInt32(byte bHH, byte bLH, byte bHL, byte bLL)
    {
      byte[] source = new byte[4]{ bHH, bLH, bHL, bLL };
      IntPtr hMemory = PviMarshal.AllocHGlobal(4);
      Marshal.Copy(source, 0, hMemory, 4);
      int int32 = Marshal.ReadInt32(hMemory, 0);
      PviMarshal.FreeHGlobal(ref hMemory);
      return int32;
    }

    internal static float ToFloat32(byte bHH, byte bLH, byte bHL, byte bLL)
    {
      float[] destination = new float[1];
      byte[] source = new byte[4]{ bHH, bLH, bHL, bLL };
      IntPtr hMemory = PviMarshal.AllocHGlobal(4);
      Marshal.Copy(source, 0, hMemory, 4);
      Marshal.Copy(hMemory, destination, 0, 1);
      PviMarshal.FreeHGlobal(ref hMemory);
      return destination[0];
    }

    internal static ulong ToUInt64(
      byte bHHH,
      byte bLHH,
      byte bHLH,
      byte bLLH,
      byte bHHL,
      byte bLHL,
      byte bHLL,
      byte bLLL)
    {
      int offset = 0;
      byte[] source = new byte[8]
      {
        bHHH,
        bLHH,
        bHLH,
        bLLH,
        bHHL,
        bLHL,
        bHLL,
        bLLL
      };
      IntPtr hMemory = PviMarshal.AllocHGlobal(8);
      Marshal.Copy(source, 0, hMemory, 8);
      ulong uint64 = PviMarshal.ReadUInt64(hMemory, ref offset);
      PviMarshal.FreeHGlobal(ref hMemory);
      return uint64;
    }

    internal static long ToInt64(
      byte bHHH,
      byte bLHH,
      byte bHLH,
      byte bLLH,
      byte bHHL,
      byte bLHL,
      byte bHLL,
      byte bLLL)
    {
      byte[] source = new byte[8]
      {
        bHHH,
        bLHH,
        bHLH,
        bLLH,
        bHHL,
        bLHL,
        bHLL,
        bLLL
      };
      IntPtr hMemory = PviMarshal.AllocHGlobal(8);
      Marshal.Copy(source, 0, hMemory, 8);
      long int64 = Marshal.ReadInt64(hMemory, 0);
      PviMarshal.FreeHGlobal(ref hMemory);
      return int64;
    }

    internal static double ToDouble64(
      byte bHHH,
      byte bLHH,
      byte bHLH,
      byte bLLH,
      byte bHHL,
      byte bLHL,
      byte bHLL,
      byte bLLL)
    {
      double[] destination = new double[1];
      byte[] source = new byte[8]
      {
        bHHH,
        bLHH,
        bHLH,
        bLLH,
        bHHL,
        bLHL,
        bHLL,
        bLLL
      };
      IntPtr hMemory = PviMarshal.AllocHGlobal(8);
      Marshal.Copy(source, 0, hMemory, 8);
      Marshal.Copy(hMemory, destination, 0, 1);
      PviMarshal.FreeHGlobal(ref hMemory);
      return destination[0];
    }

    internal static byte toByte(object value)
    {
      byte num = 0;
      long intVal = Pvi.GetIntVal(value);
      if (0L > intVal)
      {
        if (-129L < intVal)
          num = (byte) ((ulong) intVal + 256UL);
      }
      else
      {
        num = byte.MaxValue;
        if (256L > intVal)
          num = (byte) intVal;
      }
      return num;
    }

    internal static ushort toUInt16(object value)
    {
      uint16 = (ushort) 0;
      if (!(value is ushort uint16))
      {
        long intVal = Pvi.GetIntVal(value);
        if (0L > intVal)
        {
          if (-32769L < intVal)
            uint16 = (ushort) ((ulong) intVal + (ulong) short.MaxValue);
        }
        else
        {
          uint16 = ushort.MaxValue;
          if (65536L > intVal)
            uint16 = (ushort) intVal;
        }
      }
      return uint16;
    }

    internal static uint toUInt32(object value)
    {
      uint32 = 0U;
      switch (value)
      {
        case uint uint32:
label_21:
          return uint32;
        case int num1:
          uint32 = (uint) num1;
          goto label_21;
        case short num2:
          uint32 = (uint) num2;
          goto label_21;
        case long num3:
          uint32 = (uint) num3;
          goto label_21;
        case ulong num4:
          uint32 = (uint) num4;
          goto label_21;
        case ushort num5:
          uint32 = (uint) num5;
          goto label_21;
        case sbyte num6:
          uint32 = (uint) num6;
          goto label_21;
        case byte num7:
          uint32 = (uint) num7;
          goto label_21;
        case float num8:
          uint32 = (uint) num8;
          goto label_21;
        case double num9:
          uint32 = (uint) num9;
          goto label_21;
        default:
          if ((object) (value as Value) != null)
          {
            uint32 = PviMarshal.toUInt32(((Value) value).propObjValue);
            goto label_21;
          }
          else if (value is DateTime dateTime)
          {
            uint32 = PviMarshal.toUInt32((object) dateTime.Ticks);
            goto label_21;
          }
          else if (-1 != value.ToString().IndexOf(':'))
          {
            uint32 = PviMarshal.toUInt32((object) DateTime.Parse(value.ToString()));
            goto label_21;
          }
          else
          {
            long intVal = Pvi.GetIntVal(value);
            if (0L > intVal)
            {
              if (-2147483649L < intVal)
              {
                uint32 = (uint) ((ulong) intVal + 2147483648UL);
                goto label_21;
              }
              else
                goto label_21;
            }
            else
            {
              uint32 = uint.MaxValue;
              if (4294967296L > intVal)
              {
                uint32 = (uint) intVal;
                goto label_21;
              }
              else
                goto label_21;
            }
          }
      }
    }

    internal static sbyte toSByte(object value)
    {
      num = sbyte.MinValue;
      if (!(value is sbyte num))
      {
        long intVal = Pvi.GetIntVal(value);
        if (-129L < intVal)
          num = (long) sbyte.MaxValue >= intVal ? (sbyte) intVal : sbyte.MaxValue;
      }
      return num;
    }

    internal static short toInt16(object value)
    {
      int16 = short.MinValue;
      if (!(value is short int16))
      {
        long intVal = Pvi.GetIntVal(value);
        if (-32769L < intVal)
          int16 = (long) short.MaxValue >= intVal ? (short) intVal : short.MaxValue;
      }
      return int16;
    }

    internal static int toInt32(object value)
    {
      int32 = int.MinValue;
      switch (value)
      {
        case int int32:
label_17:
          return int32;
        case long num1:
          int32 = (int) num1;
          goto label_17;
        case short num2:
          int32 = (int) num2;
          goto label_17;
        case ulong num3:
          int32 = (int) num3;
          goto label_17;
        case uint num4:
          int32 = (int) num4;
          goto label_17;
        case ushort num5:
          int32 = (int) num5;
          goto label_17;
        case sbyte num6:
          int32 = (int) num6;
          goto label_17;
        case byte num7:
          int32 = (int) num7;
          goto label_17;
        case float num8:
          int32 = (int) num8;
          goto label_17;
        case double num9:
          int32 = (int) num9;
          goto label_17;
        default:
          if ((object) (value as Value) != null)
          {
            int32 = PviMarshal.toInt32(((Value) value).propObjValue);
            goto label_17;
          }
          else
          {
            long[] source = new long[2];
            int[] destination = new int[2];
            source[0] = Pvi.GetIntVal(value);
            if (-2147483649L < source[0])
            {
              if ((long) int.MaxValue < source[0])
              {
                IntPtr hMemory = PviMarshal.AllocHGlobal(8);
                Marshal.Copy(source, 0, hMemory, 1);
                Marshal.Copy(hMemory, destination, 0, 1);
                int32 = destination[0];
                PviMarshal.FreeHGlobal(ref hMemory);
              }
              else
                int32 = (int) source[0];
            }
            goto label_17;
          }
      }
    }

    internal static byte[] UInt64ToBytes(ulong u64Val)
    {
      long[] source = new long[2];
      byte[] destination = new byte[8];
      source[0] = (long) u64Val;
      IntPtr hMemory = PviMarshal.AllocHGlobal(8);
      Marshal.Copy(source, 0, hMemory, 1);
      Marshal.Copy(hMemory, destination, 0, 8);
      PviMarshal.FreeHGlobal(ref hMemory);
      return destination;
    }

    internal static long toInt64(object value) => Pvi.GetIntVal(value);

    internal static ulong toUInt64(object value) => Pvi.GetUIntVal(value);

    internal static APIFC_ModulInfoRes PtrToModulInfoStructure(IntPtr ptr, System.Type structureType) => (APIFC_ModulInfoRes) Marshal.PtrToStructure(ptr, structureType);

    internal static APIFC_DiagModulInfoRes PtrToDiagModulInfoStructure(
      IntPtr ptr,
      System.Type structureType)
    {
      return (APIFC_DiagModulInfoRes) Marshal.PtrToStructure(ptr, structureType);
    }

    internal static ProgressInfo PtrToProgressInfoStructure(IntPtr ptr, System.Type structureType) => (ProgressInfo) Marshal.PtrToStructure(ptr, structureType);

    internal static IntPtr StringToHGlobal(string str) => str.Length < 0 ? IntPtr.Zero : Marshal.StringToHGlobalAnsi(str);

    internal static IntPtr StringToHGlobalUni(string str) => str.Length < 0 ? IntPtr.Zero : Marshal.StringToHGlobalUni(str);

    internal static void Copy(
      IntPtr ptrSource,
      int srcOffset,
      ref int[] dataDest,
      int destElements)
    {
      for (int index = 0; index < destElements; ++index)
        dataDest[index] = Marshal.ReadInt32(ptrSource, srcOffset + 4 * index);
    }

    internal static void Copy(
      IntPtr ptrSource,
      int srcOffset,
      ref byte[] dataDest,
      int destElements)
    {
      for (int index = 0; index < destElements; ++index)
        dataDest[index] = Marshal.ReadByte(ptrSource, srcOffset + index);
    }

    internal static void Copy(
      IntPtr ptrSource,
      int srcOffset,
      int destOffset,
      ref byte[] dataDest,
      int destElements)
    {
      for (int index = 0; index < destElements; ++index)
        dataDest[destOffset + index] = Marshal.ReadByte(ptrSource, srcOffset + index);
    }

    internal static void Copy(ushort ui16Src, int destOffset, ref byte[] dataDest)
    {
      short[] source = new short[2]
      {
        (short) ui16Src,
        (short) 0
      };
      IntPtr num = Marshal.AllocHGlobal(2);
      Marshal.Copy(source, 0, num, 1);
      PviMarshal.Copy(num, 0, destOffset, ref dataDest, 2);
      Marshal.FreeHGlobal(num);
    }

    internal static void Copy(uint ui32Src, int destOffset, ref byte[] dataDest)
    {
      int[] source = new int[2]{ (int) ui32Src, 0 };
      IntPtr num = Marshal.AllocHGlobal(4);
      Marshal.Copy(source, 0, num, 1);
      PviMarshal.Copy(num, 0, destOffset, ref dataDest, 4);
      Marshal.FreeHGlobal(num);
    }

    internal static void Copy(uint ui32Src, IntPtr dataDest)
    {
      int[] source = new int[2]{ (int) ui32Src, 0 };
      dataDest = Marshal.AllocHGlobal(4);
      Marshal.Copy(source, 0, dataDest, 1);
    }

    internal static void Copy(IntPtr dataSrc, ref uint ui32Dest)
    {
      int[] destination = new int[2];
      Marshal.Copy(dataSrc, destination, 0, 1);
      ui32Dest = (uint) destination[0];
    }

    internal static uint TimeToUInt32(string plcTimeCode)
    {
      string[] strArray = plcTimeCode.Split('-');
      DateTime dt = new DateTime(1900, 1, 1);
      byte num1 = System.Convert.ToByte(strArray[0], 16);
      byte num2 = System.Convert.ToByte(strArray[1], 16);
      byte num3 = System.Convert.ToByte(strArray[2], 16);
      byte num4 = System.Convert.ToByte(strArray[3], 16);
      byte num5 = System.Convert.ToByte(strArray[4], 16);
      int num6 = (int) num1 >> 1;
      dt = dt.AddYears(num6);
      int num7 = (((int) num1 & 1) << 3) + ((int) num2 >> 5);
      dt = dt.AddMonths(num7 - 1);
      int num8 = (int) num2 & 31;
      dt = dt.AddDays((double) (num8 - 1));
      int num9 = (int) num3 >> 3;
      dt = dt.AddHours((double) num9);
      int num10 = (((int) num3 & 7) << 3) + ((int) num4 >> 5);
      dt = dt.AddMinutes((double) num10);
      int num11 = (((int) num4 & 31) << 1) + ((int) num5 >> 7);
      dt = dt.AddSeconds((double) num11);
      int num12 = (int) num5 & (int) sbyte.MaxValue;
      dt = dt.AddMilliseconds((double) num12);
      return Pvi.DateTimeToUInt32(dt);
    }

    internal static int ToInt32(string strValue) => -1 == strValue.ToLower().IndexOf("0x") ? System.Convert.ToInt32(strValue) : System.Convert.ToInt32(strValue, 16);

    internal static uint ToUInt32(string strValue) => -1 == strValue.ToLower().IndexOf("0x") ? System.Convert.ToUInt32(strValue) : System.Convert.ToUInt32(strValue, 16);

    internal static byte ToByte(string strValue) => -1 == strValue.ToLower().IndexOf("0x") ? System.Convert.ToByte(strValue) : System.Convert.ToByte(strValue, 16);

    internal static bool HexCharToByte(char hexChar, ref byte byteVal)
    {
      byteVal = PviMarshal.Convert.ToByte(hexChar);
      switch (hexChar)
      {
        case '0':
          byteVal = (byte) 0;
          break;
        case '1':
          byteVal = (byte) 1;
          break;
        case '2':
          byteVal = (byte) 2;
          break;
        case '3':
          byteVal = (byte) 3;
          break;
        case '4':
          byteVal = (byte) 4;
          break;
        case '5':
          byteVal = (byte) 5;
          break;
        case '6':
          byteVal = (byte) 6;
          break;
        case '7':
          byteVal = (byte) 7;
          break;
        case '8':
          byteVal = (byte) 8;
          break;
        case '9':
          byteVal = (byte) 9;
          break;
        case 'A':
          byteVal = (byte) 10;
          break;
        case 'B':
          byteVal = (byte) 11;
          break;
        case 'C':
          byteVal = (byte) 12;
          break;
        case 'D':
          byteVal = (byte) 13;
          break;
        case 'E':
          byteVal = (byte) 14;
          break;
        case 'F':
          byteVal = (byte) 15;
          break;
        default:
          return false;
      }
      return true;
    }

    internal class Convert
    {
      internal static byte ToByte(sbyte value) => (sbyte) 0 <= value ? (byte) value : (byte) (256U + (uint) value);

      internal static byte ToByte(char value) => char.MinValue <= value ? (byte) value : (byte) (256U + (uint) value);

      internal static ushort BytesToUShort(byte value1, byte value2)
      {
        byte[] numArray = new byte[2]{ value1, value2 };
        if (BitConverter.IsLittleEndian)
          Array.Reverse((Array) numArray);
        return BitConverter.ToUInt16(numArray, 0);
      }

      internal static short ToInt16(ushort value) => (ushort) short.MaxValue >= value ? (short) value : (short) ((int) short.MaxValue - (int) value);

      internal static int ToInt32(uint value) => (uint) int.MaxValue >= value ? (int) value : int.MaxValue - (int) value;
    }
  }
}
