// Decompiled with JetBrains decompiler
// Type: BR.AN.PviServices.Value
// Assembly: BR.AN.PVIServices, Version=1.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DA853F0A-C070-4E30-B7A7-51D955FB85F6
// Assembly location: C:\Users\ss_six\Desktop\FXE\FXE software integration package\COMET_FXE-PVIServices_Example\Pluto-PVI-trial_01\packages\IToolS.BandR.3.5.58\lib\net20\BR.AN.PVIServices.dll

using System;
using System.Collections;
using System.Runtime.InteropServices;
using System.Text;

namespace BR.AN.PviServices
{
  [CLSCompliant(false)]
  public class Value : IDisposable, IConvertible, IFormattable
  {
    internal bool isAssigned;
    private int propByteOffset;
    internal uint propUInt32Val;
    internal bool propHasOwnDataPtr;
    private bool propTypePreset;
    internal sbyte propIsEnum;
    internal DerivationBase propDerivedFrom;
    internal sbyte propIsDerived;
    internal sbyte propIsBitString;
    internal EnumArray propEnumerations;
    internal bool propArryOne;
    internal ArrayDimensionArray propDimensions;
    internal object propObjValue;
    internal byte[] propByteField;
    internal Variable propParent;
    internal DataType propDataType;
    internal int propArrayMinIndex;
    internal int propArrayMaxIndex;
    internal int propTypeLength;
    internal int propArrayLength;
    internal bool propDisposed;
    internal IntPtr pData;
    internal int propDataSize;

    private void InitMembers(
      Variable parentVar,
      IntPtr dataPtr,
      DataType type,
      int typeLen,
      int arrayLen)
    {
      this.InitMembers(parentVar, dataPtr, type, typeLen);
      this.propByteOffset = 0;
      this.propArrayLength = arrayLen;
      this.propDataSize = typeLen * arrayLen;
      this.propUInt32Val = 0U;
    }

    private void InitMembers(Variable parentVar, IntPtr dataPtr, DataType type, int typeLen)
    {
      this.propDisposed = false;
      this.propArryOne = false;
      this.propDimensions = (ArrayDimensionArray) null;
      this.propIsEnum = (sbyte) -1;
      this.propIsDerived = (sbyte) -1;
      this.propDerivedFrom = (DerivationBase) null;
      this.propIsBitString = (sbyte) -1;
      this.propEnumerations = (EnumArray) null;
      this.propArrayLength = 1;
      this.propByteField = (byte[]) null;
      this.propDataSize = typeLen;
      this.propDataType = type;
      this.isAssigned = false;
      this.propParent = parentVar;
      this.propTypeLength = typeLen;
      this.pData = dataPtr;
    }

    private void Initialize(
      Variable parentVar,
      object val,
      DataType type,
      int typeLen,
      int arrayLen)
    {
      this.propHasOwnDataPtr = true;
      this.InitMembers(parentVar, IntPtr.Zero, type, typeLen, arrayLen);
      this.propObjValue = val;
      if (type != DataType.Unknown)
        this.pData = PviMarshal.AllocHGlobal(this.DataSize);
      this.Assign(val);
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
      if (this.propDisposed)
        return;
      this.Dispose(true);
      GC.SuppressFinalize((object) this);
    }

    internal virtual void Dispose(bool disposing)
    {
      if (this.propDisposed)
        return;
      this.OnDisposing(disposing);
      if (disposing)
        this.propDisposed = true;
      if (this.propHasOwnDataPtr && IntPtr.Zero != this.pData)
        PviMarshal.FreeHGlobal(ref this.pData);
      this.pData = IntPtr.Zero;
      this.propByteField = (byte[]) null;
      this.propObjValue = (object) null;
      this.propParent = (Variable) null;
    }

    ~Value() => this.Dispose(false);

    internal Value() => this.Initialize((Variable) null, (object) null, DataType.Unknown, 0, 1);

    internal Value(Value parentVal, int[] indices)
    {
      this.propHasOwnDataPtr = false;
      int byteOffset = Variable.CalculateByteOffset(indices, parentVal);
      this.propByteOffset = byteOffset;
      bool flag = true;
      Service service;
      if (parentVal.propParent != null)
      {
        service = parentVal.propParent.Service;
        flag = false;
      }
      else
        service = new Service();
      switch (parentVal.propDataType)
      {
        case DataType.Boolean:
          this.InitMembers(parentVal.propParent, parentVal.pData, DataType.Boolean, parentVal.propTypeLength);
          this.propObjValue = parentVal.propByteField[byteOffset] != (byte) 0 ? (object) true : (object) false;
          break;
        case DataType.SByte:
          this.InitMembers(parentVal.propParent, parentVal.pData, DataType.SByte, parentVal.propTypeLength);
          this.propObjValue = (object) (sbyte) parentVal.propByteField[byteOffset];
          break;
        case DataType.Int16:
          this.InitMembers(parentVal.propParent, parentVal.pData, DataType.Int16, parentVal.propTypeLength);
          this.propObjValue = (object) service.toInt16(parentVal.propByteField, this.propByteOffset);
          break;
        case DataType.Int32:
          this.InitMembers(parentVal.propParent, parentVal.pData, DataType.Int32, parentVal.propTypeLength);
          this.propObjValue = (object) service.toInt32(parentVal.propByteField, this.propByteOffset);
          break;
        case DataType.Int64:
          this.InitMembers(parentVal.propParent, parentVal.pData, DataType.Int64, parentVal.propTypeLength);
          this.propObjValue = (object) service.toInt64(parentVal.propByteField, this.propByteOffset);
          break;
        case DataType.Byte:
          this.InitMembers(parentVal.propParent, parentVal.pData, DataType.Byte, parentVal.propTypeLength);
          this.propObjValue = (object) parentVal.propByteField[byteOffset];
          break;
        case DataType.UInt16:
        case DataType.WORD:
          this.InitMembers(parentVal.propParent, parentVal.pData, DataType.UInt16, parentVal.propTypeLength);
          this.propObjValue = (object) service.toUInt16(parentVal.propByteField, this.propByteOffset);
          break;
        case DataType.UInt32:
        case DataType.DWORD:
          this.InitMembers(parentVal.propParent, parentVal.pData, DataType.UInt32, parentVal.propTypeLength);
          this.propObjValue = (object) service.toUInt32(parentVal.propByteField, this.propByteOffset);
          break;
        case DataType.UInt64:
          this.InitMembers(parentVal.propParent, parentVal.pData, DataType.UInt64, parentVal.propTypeLength);
          this.propObjValue = (object) service.toUInt64(parentVal.propByteField, this.propByteOffset);
          break;
        case DataType.Single:
          this.InitMembers(parentVal.propParent, parentVal.pData, DataType.Single, parentVal.propTypeLength);
          this.propObjValue = (object) service.toSingle(parentVal.propByteField, this.propByteOffset);
          break;
        case DataType.Double:
          this.InitMembers(parentVal.propParent, parentVal.pData, DataType.Double, parentVal.propTypeLength);
          this.propObjValue = (object) service.toDouble(parentVal.propByteField, this.propByteOffset);
          break;
        case DataType.TimeSpan:
          this.InitMembers(parentVal.propParent, parentVal.pData, DataType.TimeSpan, parentVal.propTypeLength);
          this.propObjValue = (object) service.toTimeSpan(parentVal.propByteField, this.propByteOffset);
          break;
        case DataType.DateTime:
        case DataType.DT:
          this.InitMembers(parentVal.propParent, parentVal.pData, DataType.DateTime, parentVal.propTypeLength);
          this.propObjValue = (object) service.toDateTime(parentVal.propByteField, this.propByteOffset);
          break;
        case DataType.String:
          this.InitMembers(parentVal.propParent, parentVal.pData, DataType.String, parentVal.propTypeLength);
          this.propObjValue = (object) service.toString(parentVal.propByteField, this.propByteOffset, parentVal.propTypeLength);
          break;
        case DataType.WString:
          this.InitMembers(parentVal.propParent, parentVal.pData, DataType.WString, parentVal.propTypeLength);
          this.propObjValue = (object) PviMarshal.ToWString(parentVal.propByteField, this.propByteOffset, parentVal.propTypeLength);
          break;
        case DataType.TimeOfDay:
        case DataType.TOD:
          this.InitMembers(parentVal.propParent, parentVal.pData, DataType.TimeOfDay, parentVal.propTypeLength);
          this.propObjValue = (object) service.toTimeSpan(parentVal.propByteField, this.propByteOffset);
          break;
        case DataType.Date:
          this.InitMembers(parentVal.propParent, parentVal.pData, DataType.Date, parentVal.propTypeLength);
          this.propObjValue = (object) service.toDateTime(parentVal.propByteField, this.propByteOffset);
          break;
        case DataType.UInt8:
          this.InitMembers(parentVal.propParent, parentVal.pData, DataType.UInt8, parentVal.propTypeLength);
          this.propObjValue = (object) parentVal.propByteField[byteOffset];
          break;
        default:
          this.InitMembers(parentVal.propParent, parentVal.pData, DataType.Unknown, 0);
          break;
      }
      if (!flag)
        return;
      service.Dispose();
    }

    internal Value(Value parentVal, int arrayIndex)
    {
      this.propHasOwnDataPtr = false;
      this.propByteOffset = arrayIndex * parentVal.propTypeLength;
      Service service = parentVal.propParent == null ? new Service() : parentVal.propParent.Service;
      switch (parentVal.propDataType)
      {
        case DataType.Boolean:
          this.InitMembers(parentVal.propParent, parentVal.pData, DataType.Boolean, parentVal.propTypeLength);
          if (parentVal.propByteField[arrayIndex] == (byte) 0)
          {
            this.propObjValue = (object) false;
            break;
          }
          this.propObjValue = (object) true;
          break;
        case DataType.SByte:
          this.InitMembers(parentVal.propParent, parentVal.pData, DataType.SByte, parentVal.propTypeLength);
          this.propObjValue = (object) (sbyte) parentVal.propByteField[arrayIndex];
          break;
        case DataType.Int16:
          this.InitMembers(parentVal.propParent, parentVal.pData, DataType.Int16, parentVal.propTypeLength);
          this.propObjValue = (object) service.toInt16(parentVal.propByteField, this.propByteOffset);
          break;
        case DataType.Int32:
          this.InitMembers(parentVal.propParent, parentVal.pData, DataType.Int32, parentVal.propTypeLength);
          this.propObjValue = (object) service.toInt32(parentVal.propByteField, this.propByteOffset);
          break;
        case DataType.Int64:
          this.InitMembers(parentVal.propParent, parentVal.pData, DataType.Int64, parentVal.propTypeLength);
          this.propObjValue = (object) service.toInt64(parentVal.propByteField, this.propByteOffset);
          break;
        case DataType.Byte:
          this.InitMembers(parentVal.propParent, parentVal.pData, DataType.Byte, parentVal.propTypeLength);
          this.propObjValue = (object) parentVal.propByteField[arrayIndex];
          break;
        case DataType.UInt16:
        case DataType.WORD:
          this.InitMembers(parentVal.propParent, parentVal.pData, DataType.UInt16, parentVal.propTypeLength);
          this.propObjValue = (object) service.toUInt16(parentVal.propByteField, this.propByteOffset);
          break;
        case DataType.UInt32:
        case DataType.DWORD:
          this.InitMembers(parentVal.propParent, parentVal.pData, DataType.UInt32, parentVal.propTypeLength);
          this.propObjValue = (object) service.toUInt32(parentVal.propByteField, this.propByteOffset);
          break;
        case DataType.UInt64:
          this.InitMembers(parentVal.propParent, parentVal.pData, DataType.UInt64, parentVal.propTypeLength);
          this.propObjValue = (object) service.toUInt64(parentVal.propByteField, this.propByteOffset);
          break;
        case DataType.Single:
          this.InitMembers(parentVal.propParent, parentVal.pData, DataType.Single, parentVal.propTypeLength);
          this.propObjValue = (object) service.toSingle(parentVal.propByteField, this.propByteOffset);
          break;
        case DataType.Double:
          this.InitMembers(parentVal.propParent, parentVal.pData, DataType.Double, parentVal.propTypeLength);
          this.propObjValue = (object) service.toDouble(parentVal.propByteField, this.propByteOffset);
          break;
        case DataType.TimeSpan:
          this.InitMembers(parentVal.propParent, parentVal.pData, DataType.TimeSpan, parentVal.propTypeLength);
          this.propObjValue = (object) service.toTimeSpan(parentVal.propByteField, this.propByteOffset);
          break;
        case DataType.DateTime:
        case DataType.DT:
          this.InitMembers(parentVal.propParent, parentVal.pData, DataType.DateTime, parentVal.propTypeLength);
          this.propObjValue = (object) service.toDateTime(parentVal.propByteField, this.propByteOffset);
          break;
        case DataType.String:
          this.InitMembers(parentVal.propParent, parentVal.pData, DataType.String, parentVal.propTypeLength);
          this.propObjValue = (object) service.toString(parentVal.propByteField, this.propByteOffset, parentVal.propTypeLength);
          break;
        case DataType.WString:
          this.InitMembers(parentVal.propParent, parentVal.pData, DataType.WString, parentVal.propTypeLength);
          this.propObjValue = (object) PviMarshal.ToWString(parentVal.propByteField, this.propByteOffset, parentVal.propTypeLength);
          break;
        case DataType.TimeOfDay:
        case DataType.TOD:
          this.InitMembers(parentVal.propParent, parentVal.pData, DataType.TimeOfDay, parentVal.propTypeLength);
          this.propObjValue = (object) service.toTimeSpan(parentVal.propByteField, this.propByteOffset);
          break;
        case DataType.Date:
          this.InitMembers(parentVal.propParent, parentVal.pData, DataType.Date, parentVal.propTypeLength);
          this.propObjValue = (object) service.toDateTime(parentVal.propByteField, this.propByteOffset);
          break;
        case DataType.UInt8:
          this.InitMembers(parentVal.propParent, parentVal.pData, DataType.UInt8, parentVal.propTypeLength);
          this.propObjValue = (object) parentVal.propByteField[arrayIndex];
          break;
        default:
          this.InitMembers(parentVal.propParent, parentVal.pData, DataType.Unknown, 0);
          break;
      }
    }

    public Value(bool value) => this.Initialize((Variable) null, (object) value, DataType.Boolean, 1, 1);

    internal Value(bool value, Variable parentVar) => this.Initialize(parentVar, (object) value, DataType.Boolean, 1, 1);

    public Value(bool[] value) => this.SetByteField(value);

    internal unsafe void SetByteField(bool[] value)
    {
      this.Initialize((Variable) null, (object) value, DataType.Boolean, 1, value.Length);
      this.propByteField = new byte[this.propArrayLength * this.propTypeLength];
      fixed (byte* numPtr = &this.propByteField[0])
      {
        for (int index = 0; index < this.propArrayLength; ++index)
          ((bool*) numPtr)[index] = value[index];
      }
    }

    public Value(sbyte value) => this.Initialize((Variable) null, (object) value, DataType.SByte, 0, 1);

    internal Value(sbyte value, Variable parentVar) => this.Initialize(parentVar, (object) value, DataType.SByte, 0, 1);

    public Value(sbyte[] value) => this.SetByteField(value);

    internal unsafe void SetByteField(sbyte[] value)
    {
      this.Initialize((Variable) null, (object) value, DataType.SByte, 1, value.Length);
      this.propByteField = new byte[this.propArrayLength * this.propTypeLength];
      fixed (byte* numPtr = &this.propByteField[0])
      {
        for (int index = 0; index < this.propArrayLength; ++index)
          ((sbyte*) numPtr)[index] = value[index];
      }
    }

    public Value(short value) => this.Initialize((Variable) null, (object) value, DataType.Int16, 2, 1);

    internal Value(short value, Variable parentVar) => this.Initialize(parentVar, (object) value, DataType.Int16, 2, 1);

    public Value(short[] value) => this.SetByteField(value);

    internal unsafe void SetByteField(short[] value)
    {
      this.Initialize((Variable) null, (object) value, DataType.Int16, 2, value.Length);
      this.propByteField = new byte[this.propArrayLength * this.propTypeLength];
      fixed (byte* numPtr = &this.propByteField[0])
      {
        *(short*) numPtr = value[0];
        for (int index = 1; index < this.propArrayLength; ++index)
          ((short*) numPtr)[index] = value[index];
      }
    }

    public Value(int value) => this.Initialize((Variable) null, (object) value, DataType.Int32, 4, 1);

    internal Value(int value, Variable parentVar) => this.Initialize(parentVar, (object) value, DataType.Int32, 4, 1);

    public Value(int[] value) => this.SetByteField(value);

    internal unsafe void SetByteField(int[] value)
    {
      this.Initialize((Variable) null, (object) value, DataType.Int32, 4, value.Length);
      this.propByteField = new byte[this.propArrayLength * this.propTypeLength];
      fixed (byte* numPtr = &this.propByteField[0])
      {
        *(int*) numPtr = value[0];
        for (int index = 1; index < this.propArrayLength; ++index)
          ((int*) numPtr)[index] = value[index];
      }
    }

    internal unsafe void SetByteField(long[] value)
    {
      this.Initialize((Variable) null, (object) value, DataType.Int64, 8, value.Length);
      this.propByteField = new byte[this.propArrayLength * this.propTypeLength];
      fixed (byte* numPtr = &this.propByteField[0])
      {
        *(long*) numPtr = value[0];
        for (int index = 1; index < this.propArrayLength; ++index)
          ((long*) numPtr)[index] = value[index];
      }
    }

    internal Value(byte value, Variable parentVar) => this.Initialize(parentVar, (object) value, DataType.Byte, 1, 1);

    public Value(byte value) => this.Initialize((Variable) null, (object) value, DataType.Byte, 1, 1);

    public Value(byte[] value)
    {
      this.isAssigned = false;
      this.SetByteField(value);
    }

    internal unsafe void SetByteField(byte[] value)
    {
      this.Initialize((Variable) null, (object) value, DataType.Byte, 1, value.Length);
      this.propByteField = new byte[this.propArrayLength * this.propTypeLength];
      fixed (byte* numPtr = &this.propByteField[0])
      {
        *numPtr = value[0];
        for (int index = 1; index < this.propArrayLength; ++index)
          numPtr[index] = value[index];
      }
    }

    public Value(ushort value) => this.Initialize((Variable) null, (object) value, DataType.UInt16, 2, 1);

    internal Value(ushort value, Variable parentVar) => this.Initialize(parentVar, (object) value, DataType.UInt16, 2, 1);

    public Value(ushort[] value) => this.SetByteField(value);

    internal unsafe void SetByteField(ushort[] value)
    {
      this.Initialize((Variable) null, (object) value, DataType.UInt16, 2, value.Length);
      this.propByteField = new byte[this.propArrayLength * this.propTypeLength];
      fixed (byte* numPtr = &this.propByteField[0])
      {
        *(ushort*) numPtr = value[0];
        for (int index = 1; index < this.propArrayLength; ++index)
          ((ushort*) numPtr)[index] = value[index];
      }
    }

    public Value(uint value) => this.Initialize((Variable) null, (object) value, DataType.UInt32, 4, 1);

    internal Value(uint value, Variable parentVar) => this.Initialize(parentVar, (object) value, DataType.UInt32, 4, 1);

    public Value(uint[] value) => this.SetByteField(value);

    public Value(ulong value) => this.Initialize((Variable) null, (object) value, DataType.UInt64, 8, 1);

    internal Value(ulong value, Variable parentVar) => this.Initialize(parentVar, (object) value, DataType.UInt64, 8, 1);

    public Value(ulong[] value) => this.SetByteField(value);

    internal unsafe void SetByteField(uint[] value)
    {
      this.Initialize((Variable) null, (object) value, DataType.UInt32, 4, value.Length);
      this.propByteField = new byte[this.propArrayLength * this.propTypeLength];
      fixed (byte* numPtr = &this.propByteField[0])
      {
        *(uint*) numPtr = value[0];
        for (int index = 1; index < this.propArrayLength; ++index)
          ((uint*) numPtr)[index] = value[index];
      }
    }

    internal unsafe void SetByteField(ulong[] value)
    {
      this.Initialize((Variable) null, (object) value, DataType.UInt64, 8, value.Length);
      this.propByteField = new byte[this.propArrayLength * this.propTypeLength];
      fixed (byte* numPtr = &this.propByteField[0])
      {
        *(ulong*) numPtr = value[0];
        for (int index = 1; index < this.propArrayLength; ++index)
          ((ulong*) numPtr)[index] = value[index];
      }
    }

    public Value(float value) => this.Initialize((Variable) null, (object) value, DataType.Single, 4, 1);

    internal Value(float value, Variable parentVar) => this.Initialize(parentVar, (object) value, DataType.Single, 4, 1);

    public Value(float[] value) => this.SetByteField(value);

    internal unsafe void SetByteField(float[] value)
    {
      this.Initialize((Variable) null, (object) value, DataType.Single, 4, value.Length);
      this.propByteField = new byte[this.propArrayLength * this.propTypeLength];
      fixed (byte* numPtr = &this.propByteField[0])
      {
        *(float*) numPtr = value[0];
        for (int index = 1; index < this.propArrayLength; ++index)
          ((float*) numPtr)[index] = value[index];
      }
    }

    public Value(double value) => this.Initialize((Variable) null, (object) value, DataType.Double, 8, 1);

    internal Value(double value, Variable parentVar) => this.Initialize(parentVar, (object) value, DataType.Double, 8, 1);

    public Value(double[] value) => this.SetByteField(value);

    internal unsafe void SetByteField(double[] value)
    {
      this.Initialize((Variable) null, (object) value, DataType.Double, 8, value.Length);
      this.propByteField = new byte[this.propArrayLength * this.propTypeLength];
      fixed (byte* numPtr = &this.propByteField[0])
      {
        *(double*) numPtr = value[0];
        for (int index = 1; index < this.propArrayLength; ++index)
          ((double*) numPtr)[index] = value[index];
      }
    }

    public Value(TimeSpan value) => this.Initialize((Variable) null, (object) value, DataType.TimeSpan, 4, 1);

    internal Value(TimeSpan value, Variable parentVar) => this.Initialize(parentVar, (object) value, DataType.TimeSpan, 4, 1);

    public Value(TimeSpan[] value) => this.SetByteField(value);

    internal unsafe void SetByteField(TimeSpan[] value)
    {
      this.Initialize((Variable) null, (object) value, DataType.TimeSpan, 4, value.Length);
      this.propByteField = new byte[this.propArrayLength * this.propTypeLength];
      fixed (byte* numPtr = &this.propByteField[0])
      {
        *(uint*) numPtr = (uint) value[0].Ticks / 10000U;
        for (int index = 1; index < this.propArrayLength; ++index)
          ((uint*) numPtr)[index] = (uint) value[index].Ticks / 10000U;
      }
    }

    public Value(DateTime value) => this.Initialize((Variable) null, (object) value, DataType.DateTime, 4, 1);

    internal Value(DateTime value, Variable parentVar) => this.Initialize(parentVar, (object) value, DataType.DateTime, 4, 1);

    public Value(DateTime[] value) => this.SetByteField(value);

    internal unsafe void SetByteField(DateTime[] value)
    {
      this.Initialize((Variable) null, (object) value, DataType.DateTime, 4, value.Length);
      this.propByteField = new byte[this.propArrayLength * this.propTypeLength];
      fixed (byte* numPtr = &this.propByteField[0])
      {
        *(uint*) numPtr = (uint) value[0].Ticks / 10000U;
        for (int index = 1; index < this.propArrayLength; ++index)
          ((uint*) numPtr)[index] = (uint) value[index].Ticks / 10000U;
      }
    }

    public Value(string value) => this.Initialize((Variable) null, (object) value, DataType.String, value.Length, 1);

    internal Value(string value, Variable parentVar) => this.Initialize(parentVar, (object) value, DataType.String, value.Length, 1);

    public Value(string[] value) => this.SetByteField(value);

    internal unsafe void SetByteField(string[] value)
    {
      this.Initialize((Variable) null, (object) value, DataType.String, value[0].Length, value.Length);
      this.propByteField = new byte[this.propArrayLength * this.propTypeLength];
      fixed (byte* numPtr1 = &this.propByteField[0])
      {
        string str1 = value[0];
        byte* numPtr2 = numPtr1 + this.propTypeLength;
        *numPtr2 = System.Convert.ToByte(str1[0]);
        for (int index = 1; index < this.propTypeLength - 1; ++index)
          numPtr2[index] = System.Convert.ToByte(str1[index]);
        *(numPtr2 + this.propTypeLength - 1) = (byte) 0;
        for (int index1 = 1; index1 < this.propArrayLength; ++index1)
        {
          string str2 = value[index1];
          byte* numPtr3 = numPtr1 + ((IntPtr) index1 * this.propTypeLength).ToInt64();
          *numPtr3 = System.Convert.ToByte(str2[0]);
          for (int index2 = 1; index2 < this.propTypeLength - 1; ++index2)
            numPtr3[index2] = System.Convert.ToByte(str2[index2]);
          *(numPtr3 + this.propTypeLength - 1) = (byte) 0;
        }
      }
    }

    public Value(object value)
    {
      this.Initialize((Variable) null, value, DataType.Unknown, 0, 1);
      this.Assign(value);
    }

    internal Value(object value, Variable parentVar)
    {
      this.Initialize(parentVar, value, DataType.Unknown, 0, 1);
      this.Assign(value);
    }

    private int BinaryDataLen => this.propArrayLength * this.propTypeLength;

    private bool IsBoolTrue(object value)
    {
      if (value == null)
        return false;
      string str = value.ToString();
      if (str.Length == 0)
        return false;
      string lower = str.ToLower();
      return lower.CompareTo("false") != 0 && lower.CompareTo("0") != 0;
    }

    private void AssignArrayValues(object values)
    {
      if (this.DataType == DataType.Unknown)
        this.isAssigned = false;
      else if (values is Array)
      {
        if (DataType.Structure == this.DataType)
        {
          if (this.Parent.StructureMembers == null)
            return;
          for (int index = 0; index < ((Array) values).Length; ++index)
            this.Parent.Value[this.Parent.StructureMembers[index].ToString()].Assign(((Array) values).GetValue(index));
        }
        else if (this.Parent != null && (Value) null != this.Parent.Value)
        {
          for (int index = 0; index < ((Array) values).Length; ++index)
            this.Parent.Value[index].Assign(((Array) values).GetValue(index));
        }
        else
        {
          if (this.propByteField == null || this.propByteField.Length != this.propArrayLength * this.propTypeLength)
            this.propByteField = new byte[this.propArrayLength * this.propTypeLength];
          for (int index = 0; index < ((Array) values).Length; ++index)
            this.Assign(((Array) values).GetValue(index), index);
        }
      }
      else if (DataType.Structure == this.DataType)
      {
        if (this.Parent.StructureMembers == null)
          return;
        this.Parent.Value[this.Parent.StructureMembers[0].ToString()].Assign(values);
      }
      else if (this.Parent != null && (Value) null != this.Parent.Value)
      {
        this.Parent.Value[0].Assign(values);
      }
      else
      {
        if (this.propByteField == null || this.propByteField.Length != this.propArrayLength * this.propTypeLength)
          this.propByteField = new byte[this.propArrayLength * this.propTypeLength];
        this[0].Assign(values);
      }
    }

    private void AssignStrArray(string[] values)
    {
      if (this.DataType == DataType.Unknown)
        this.isAssigned = false;
      else if (DataType.Structure == this.DataType)
      {
        if (this.Parent.StructureMembers == null)
          return;
        for (int index = 0; index < values.Length; ++index)
          this.Parent.Value[this.Parent.StructureMembers[index].ToString()].Assign((object) values[index]);
      }
      else if (this.Parent != null && (Value) null != this.Parent.Value)
      {
        for (int index = 0; index < values.Length; ++index)
          this.Parent.Value[index].Assign((object) values[index]);
      }
      else
      {
        if (this.propByteField == null || this.propByteField.Length != this.propArrayLength * this.propTypeLength)
          this.propByteField = new byte[this.propArrayLength * this.propTypeLength];
        for (int index = 0; index < values.Length; ++index)
          this[index].Assign((object) values[index]);
      }
    }

    private void SetPG2000StringData(string strVal)
    {
      if (this.BinaryDataLen < strVal.Length)
      {
        strVal = strVal.Substring(0, this.BinaryDataLen);
        this.Parent.propSendChangedEvent = true;
      }
      if (this.propParent != null)
        this.propParent.ResizePviDataPtr(this.BinaryDataLen);
      if (IntPtr.Zero == this.pData)
      {
        this.pData = PviMarshal.AllocHGlobal(this.BinaryDataLen);
        this.propHasOwnDataPtr = true;
      }
      for (int index = 0; index < this.propDataSize; ++index)
      {
        if (index < strVal.Length)
          Marshal.WriteByte(this.pData, index, (byte) strVal[index]);
        else
          Marshal.WriteByte(this.pData, index, (byte) 0);
      }
    }

    private bool CheckBuffers(ulong uiValue)
    {
      if (this.propParent != null)
        this.propParent.Value.isAssigned = true;
      this.isAssigned = true;
      if (1 < this.propArrayLength)
      {
        if (this.IsPG2000String)
          this.SetPG2000StringData(uiValue.ToString());
        else
          this.AssignArrayValues((object) uiValue);
        return false;
      }
      if (IntPtr.Zero == this.pData)
      {
        if (this.DataSize == 0)
        {
          if (this.propParent != null)
            this.propParent.Value.isAssigned = false;
          this.isAssigned = false;
          return false;
        }
        this.pData = PviMarshal.AllocHGlobal(this.DataSize);
        this.propHasOwnDataPtr = true;
      }
      if (this.IsPG2000String)
      {
        this.SetPG2000StringData(uiValue.ToString());
        return false;
      }
      return !(IntPtr.Zero == this.pData);
    }

    private void AssignValue(float value)
    {
      switch (this.DataType)
      {
        case DataType.Single:
          this.propDataSize = Marshal.SizeOf(typeof (float));
          if (this.propParent != null)
          {
            this.propParent.Service.cpyFltToBuffer((object) value);
            for (int index = 0; index < 4; ++index)
              Marshal.WriteByte(this.pData, this.propByteOffset + index, this.propParent.Service.ByteBuffer[index]);
            break;
          }
          Marshal.Copy(new float[1]
          {
            System.Convert.ToSingle(value)
          }, 0, this.pData, 1);
          break;
        case DataType.Double:
          this.propDataSize = Marshal.SizeOf(typeof (double));
          if (this.propParent != null)
          {
            this.propParent.Service.cpyDblToBuffer((object) value);
            for (int index = 0; index < 8; ++index)
              Marshal.WriteByte(this.pData, this.propByteOffset + index, this.propParent.Service.ByteBuffer[index]);
            break;
          }
          Marshal.Copy(new double[1]
          {
            System.Convert.ToDouble(value)
          }, 0, this.pData, 1);
          break;
        default:
          this.AssignValue(System.Convert.ToUInt64(value));
          break;
      }
    }

    private void AssignValue(double value)
    {
      switch (this.DataType)
      {
        case DataType.Single:
          this.propDataSize = Marshal.SizeOf(typeof (float));
          if (this.propParent != null)
          {
            this.propParent.Service.cpyFltToBuffer((object) value);
            for (int index = 0; index < 4; ++index)
              Marshal.WriteByte(this.pData, this.propByteOffset + index, this.propParent.Service.ByteBuffer[index]);
            break;
          }
          Marshal.Copy(new float[1]
          {
            System.Convert.ToSingle(value)
          }, 0, this.pData, 1);
          break;
        case DataType.Double:
          this.propDataSize = Marshal.SizeOf(typeof (double));
          if (this.propParent != null)
          {
            this.propParent.Service.cpyDblToBuffer((object) value);
            for (int index = 0; index < 8; ++index)
              Marshal.WriteByte(this.pData, this.propByteOffset + index, this.propParent.Service.ByteBuffer[index]);
            break;
          }
          Marshal.Copy(new double[1]
          {
            System.Convert.ToDouble(value)
          }, 0, this.pData, 1);
          break;
        default:
          this.AssignValue(System.Convert.ToUInt64(value));
          break;
      }
    }

    private void AssignValue(ulong value)
    {
      switch (this.DataType)
      {
        case DataType.Boolean:
          this.propDataSize = Marshal.SizeOf(typeof (sbyte));
          if (0UL == value)
          {
            Marshal.WriteByte(this.pData, this.propByteOffset, (byte) 0);
            break;
          }
          Marshal.WriteByte(this.pData, this.propByteOffset, (byte) 1);
          break;
        case DataType.SByte:
          this.propDataSize = Marshal.SizeOf(typeof (sbyte));
          PviMarshal.WriteSByte(this.pData, this.propByteOffset, (sbyte) value);
          break;
        case DataType.Int16:
          this.propDataSize = Marshal.SizeOf(typeof (short));
          Marshal.WriteInt16(this.pData, this.propByteOffset, (short) value);
          break;
        case DataType.Int32:
          this.propDataSize = Marshal.SizeOf(typeof (int));
          Marshal.WriteInt32(this.pData, this.propByteOffset, (int) value);
          break;
        case DataType.Int64:
          this.propDataSize = Marshal.SizeOf(typeof (long));
          PviMarshal.WriteInt64(this.pData, this.propByteOffset, (long) value);
          break;
        case DataType.Byte:
        case DataType.UInt8:
          this.propDataSize = Marshal.SizeOf(typeof (byte));
          Marshal.WriteByte(this.pData, this.propByteOffset, (byte) value);
          break;
        case DataType.UInt16:
        case DataType.WORD:
          this.propDataSize = Marshal.SizeOf(typeof (ushort));
          PviMarshal.WriteUInt16(this.pData, this.propByteOffset, (ushort) value);
          break;
        case DataType.UInt32:
        case DataType.DWORD:
          this.propDataSize = Marshal.SizeOf(typeof (uint));
          PviMarshal.WriteUInt32(this.pData, this.propByteOffset, (uint) value);
          break;
        case DataType.UInt64:
          this.propDataSize = Marshal.SizeOf(typeof (ulong));
          PviMarshal.WriteUInt64(this.pData, this.propByteOffset, value);
          break;
        case DataType.Single:
          this.propDataSize = Marshal.SizeOf(typeof (float));
          if (this.propParent != null)
          {
            this.propParent.Service.cpyFltToBuffer((object) value);
            for (int index = 0; index < 4; ++index)
              Marshal.WriteByte(this.pData, this.propByteOffset + index, this.propParent.Service.ByteBuffer[index]);
            break;
          }
          Marshal.Copy(new float[1]
          {
            System.Convert.ToSingle(value)
          }, 0, this.pData, 1);
          break;
        case DataType.Double:
          this.propDataSize = Marshal.SizeOf(typeof (double));
          if (this.propParent != null)
          {
            this.propParent.Service.cpyDblToBuffer((object) value);
            for (int index = 0; index < 8; ++index)
              Marshal.WriteByte(this.pData, this.propByteOffset + index, this.propParent.Service.ByteBuffer[index]);
            break;
          }
          Marshal.Copy(new double[1]
          {
            System.Convert.ToDouble(value)
          }, 0, this.pData, 1);
          break;
        case DataType.TimeSpan:
        case DataType.TimeOfDay:
        case DataType.TOD:
          this.propDataSize = Marshal.SizeOf(typeof (int));
          Marshal.WriteInt32(this.pData, this.propByteOffset, Pvi.GetTimeSpanInt32((object) value));
          break;
        case DataType.DateTime:
        case DataType.DT:
          this.propDataSize = Marshal.SizeOf(typeof (int));
          PviMarshal.WriteUInt32(this.pData, this.propByteOffset, Pvi.GetDateTimeUInt32((object) value));
          break;
        case DataType.String:
          byte val1 = 0;
          this.propDataSize = this.DataSize;
          for (int index = 0; index < this.DataSize; ++index)
            Marshal.WriteByte(this.pData, this.propByteOffset + index, val1);
          if (0 < value.ToString().Length)
          {
            string str = value.ToString();
            for (int index = 0; index < str.Length && index < this.DataSize; ++index)
            {
              byte val2 = (byte) str[index];
              Marshal.WriteByte(this.pData, this.propByteOffset + index, val2);
            }
            break;
          }
          break;
        case DataType.WString:
          byte val3 = 0;
          this.propDataSize = this.DataSize;
          for (int index = 0; index < this.DataSize; ++index)
            Marshal.WriteByte(this.pData, this.propByteOffset + index, val3);
          if (0 < value.ToString().Length)
          {
            byte[] destination = new byte[this.DataSize];
            IntPtr hglobalUni = PviMarshal.StringToHGlobalUni(value.ToString());
            if (value.ToString().Length * 2 > this.DataSize)
            {
              Marshal.Copy(hglobalUni, destination, 0, this.propDataSize - 2);
              destination[this.DataSize - 2] = (byte) 0;
              destination[this.DataSize - 1] = (byte) 0;
            }
            else
              Marshal.Copy(hglobalUni, destination, 0, value.ToString().Length * 2);
            for (int index = 0; index < destination.Length; ++index)
              Marshal.WriteByte(this.pData, this.propByteOffset + index, destination[index]);
            PviMarshal.FreeHGlobal(ref hglobalUni);
            break;
          }
          break;
        case DataType.Date:
          this.propDataSize = Marshal.SizeOf(typeof (int));
          PviMarshal.WriteUInt32(this.pData, this.propByteOffset, Pvi.GetDateUInt32((object) value));
          break;
        default:
          if (this.propParent != null)
            this.propParent.Value.isAssigned = true;
          this.isAssigned = false;
          return;
      }
      this.propArrayLength = 1;
    }

    public void Assign(uint value)
    {
      if (!this.CheckBuffers((ulong) value))
        return;
      this.AssignValue((ulong) value);
      this.propObjValue = (object) value;
    }

    public void Assign(int value)
    {
      if (!this.CheckBuffers((ulong) value))
        return;
      this.AssignValue((ulong) value);
      this.propObjValue = (object) value;
    }

    public void Assign(ushort value)
    {
      if (!this.CheckBuffers((ulong) value))
        return;
      this.AssignValue((ulong) value);
      this.propObjValue = (object) value;
    }

    public void Assign(short value)
    {
      if (!this.CheckBuffers((ulong) value))
        return;
      this.AssignValue((ulong) value);
      this.propObjValue = (object) value;
    }

    public void Assign(byte value)
    {
      if (!this.CheckBuffers((ulong) value))
        return;
      this.AssignValue((ulong) value);
      this.propObjValue = (object) value;
    }

    public void Assign(sbyte value)
    {
      if (!this.CheckBuffers((ulong) value))
        return;
      this.AssignValue((ulong) value);
      this.propObjValue = (object) value;
    }

    public void Assign(float value)
    {
      if (!this.CheckBuffers((ulong) value))
        return;
      this.AssignValue(value);
      this.propObjValue = (object) value;
    }

    public void Assign(double value)
    {
      if (!this.CheckBuffers((ulong) value))
        return;
      this.AssignValue(value);
      this.propObjValue = (object) value;
    }

    public void Assign(bool value)
    {
      ulong uiValue = 0;
      if (value)
        uiValue = 1UL;
      if (!this.CheckBuffers(uiValue))
        return;
      this.AssignValue(uiValue);
      this.propObjValue = (object) value;
    }

    public void Assign(object value)
    {
      object values = value;
      if (values == null)
        return;
      if (this.propParent != null)
        this.propParent.Value.isAssigned = true;
      this.isAssigned = true;
      if (1 < this.propArrayLength)
      {
        if (this.IsPG2000String)
        {
          string strVal = "";
          if (values is Array)
          {
            for (int index = 0; index < ((Array) values).Length; ++index)
              strVal += ((Array) values).GetValue(index).ToString();
          }
          else
            strVal = values.ToString();
          this.SetPG2000StringData(strVal);
        }
        else
          this.AssignArrayValues(values);
      }
      else
      {
        if (IntPtr.Zero == this.pData)
        {
          if (this.DataSize == 0)
          {
            if (this.propParent != null)
              this.propParent.Value.isAssigned = false;
            this.isAssigned = false;
            return;
          }
          this.pData = PviMarshal.AllocHGlobal(this.DataSize);
          this.propHasOwnDataPtr = true;
        }
        if (this.IsPG2000String)
        {
          this.SetPG2000StringData(values.ToString());
        }
        else
        {
          if (values is string && (sbyte) 1 == this.propIsEnum && this.propEnumerations != null)
            values = this.propEnumerations.EnumValue((string) value) ?? value;
          if (IntPtr.Zero == this.pData)
            return;
          switch (this.DataType)
          {
            case DataType.Boolean:
              this.propDataSize = Marshal.SizeOf(typeof (sbyte));
              if (values is bool flag)
              {
                if (!flag)
                {
                  Marshal.WriteByte(this.pData, this.propByteOffset, (byte) 0);
                  break;
                }
                Marshal.WriteByte(this.pData, this.propByteOffset, (byte) 1);
                break;
              }
              if (!this.IsBoolTrue(values))
              {
                Marshal.WriteByte(this.pData, this.propByteOffset, (byte) 0);
                break;
              }
              Marshal.WriteByte(this.pData, this.propByteOffset, (byte) 1);
              break;
            case DataType.SByte:
              this.propDataSize = Marshal.SizeOf(typeof (sbyte));
              PviMarshal.WriteSByte(this.pData, this.propByteOffset, PviMarshal.toSByte(values));
              break;
            case DataType.Int16:
              this.propDataSize = Marshal.SizeOf(typeof (short));
              Marshal.WriteInt16(this.pData, this.propByteOffset, PviMarshal.toInt16(values));
              break;
            case DataType.Int32:
              this.propDataSize = Marshal.SizeOf(typeof (int));
              Marshal.WriteInt32(this.pData, this.propByteOffset, PviMarshal.toInt32(values));
              break;
            case DataType.Int64:
              this.propDataSize = Marshal.SizeOf(typeof (long));
              PviMarshal.WriteInt64(this.pData, this.propByteOffset, PviMarshal.toInt64(values));
              break;
            case DataType.Byte:
            case DataType.UInt8:
              this.propDataSize = Marshal.SizeOf(typeof (byte));
              Marshal.WriteByte(this.pData, this.propByteOffset, PviMarshal.toByte(values));
              break;
            case DataType.UInt16:
            case DataType.WORD:
              this.propDataSize = Marshal.SizeOf(typeof (ushort));
              PviMarshal.WriteUInt16(this.pData, this.propByteOffset, PviMarshal.toUInt16(values));
              break;
            case DataType.UInt32:
            case DataType.DWORD:
              this.propDataSize = Marshal.SizeOf(typeof (uint));
              PviMarshal.WriteUInt32(this.pData, this.propByteOffset, PviMarshal.toUInt32(values));
              break;
            case DataType.UInt64:
              this.propDataSize = Marshal.SizeOf(typeof (ulong));
              PviMarshal.WriteUInt64(this.pData, this.propByteOffset, PviMarshal.toUInt64(values));
              break;
            case DataType.Single:
              this.propDataSize = Marshal.SizeOf(typeof (float));
              if (this.propParent != null)
              {
                this.propParent.Service.cpyFltToBuffer(values);
                for (int index = 0; index < 4; ++index)
                  Marshal.WriteByte(this.pData, this.propByteOffset + index, this.propParent.Service.ByteBuffer[index]);
                break;
              }
              Marshal.Copy(new float[1]
              {
                System.Convert.ToSingle(values)
              }, 0, this.pData, 1);
              break;
            case DataType.Double:
              this.propDataSize = Marshal.SizeOf(typeof (double));
              if (this.propParent != null)
              {
                this.propParent.Service.cpyDblToBuffer(values);
                for (int index = 0; index < 8; ++index)
                  Marshal.WriteByte(this.pData, this.propByteOffset + index, this.propParent.Service.ByteBuffer[index]);
                break;
              }
              Marshal.Copy(new double[1]
              {
                System.Convert.ToDouble(values)
              }, 0, this.pData, 1);
              break;
            case DataType.TimeSpan:
            case DataType.TimeOfDay:
            case DataType.TOD:
              this.propDataSize = Marshal.SizeOf(typeof (int));
              Marshal.WriteInt32(this.pData, this.propByteOffset, Pvi.GetTimeSpanInt32(values));
              break;
            case DataType.DateTime:
            case DataType.DT:
              this.propDataSize = Marshal.SizeOf(typeof (int));
              PviMarshal.WriteUInt32(this.pData, this.propByteOffset, Pvi.GetDateTimeUInt32(values));
              break;
            case DataType.String:
              byte val1 = 0;
              this.propDataSize = this.DataSize;
              for (int index = 0; index < this.DataSize; ++index)
                Marshal.WriteByte(this.pData, this.propByteOffset + index, val1);
              if (values != null && 0 < values.ToString().Length)
              {
                string str = values.ToString();
                for (int index = 0; index < str.Length && index < this.DataSize; ++index)
                {
                  byte val2 = (byte) str[index];
                  Marshal.WriteByte(this.pData, this.propByteOffset + index, val2);
                }
                break;
              }
              break;
            case DataType.WString:
              byte val3 = 0;
              this.propDataSize = this.DataSize;
              for (int index = 0; index < this.DataSize; ++index)
                Marshal.WriteByte(this.pData, this.propByteOffset + index, val3);
              if (values != null && 0 < values.ToString().Length)
              {
                byte[] destination = new byte[this.DataSize];
                IntPtr hglobalUni = PviMarshal.StringToHGlobalUni(values.ToString());
                if (values.ToString().Length * 2 > this.DataSize)
                {
                  Marshal.Copy(hglobalUni, destination, 0, this.propDataSize - 2);
                  destination[this.DataSize - 2] = (byte) 0;
                  destination[this.DataSize - 1] = (byte) 0;
                }
                else
                  Marshal.Copy(hglobalUni, destination, 0, values.ToString().Length * 2);
                for (int index = 0; index < destination.Length; ++index)
                  Marshal.WriteByte(this.pData, this.propByteOffset + index, destination[index]);
                PviMarshal.FreeHGlobal(ref hglobalUni);
                break;
              }
              break;
            case DataType.Date:
              this.propDataSize = Marshal.SizeOf(typeof (int));
              PviMarshal.WriteUInt32(this.pData, this.propByteOffset, Pvi.GetDateUInt32(values));
              break;
            default:
              if (this.propParent != null)
                this.propParent.Value.isAssigned = true;
              this.isAssigned = false;
              return;
          }
          this.propArrayLength = 1;
          this.propObjValue = values;
        }
      }
    }

    public void Assign(object value, int index)
    {
      object obj = value;
      if (obj == null || index >= this.ArrayLength)
        return;
      if (this.propParent != null)
        this.propParent.Value.isAssigned = true;
      this.isAssigned = true;
      int index1 = index * this.TypeLength;
      if (IntPtr.Zero == this.pData)
      {
        if (this.DataSize == 0)
        {
          if (this.propParent != null)
            this.propParent.Value.isAssigned = false;
          this.isAssigned = false;
          return;
        }
        this.pData = PviMarshal.AllocHGlobal(this.DataSize);
        this.propHasOwnDataPtr = true;
      }
      if (this.IsPG2000String)
      {
        this.SetPG2000StringData(obj.ToString());
      }
      else
      {
        if (obj is string && (sbyte) 1 == this.propIsEnum && this.propEnumerations != null)
          obj = this.propEnumerations.EnumValue((string) value) ?? value;
        if (IntPtr.Zero == this.pData)
          return;
        switch (this.DataType)
        {
          case DataType.Boolean:
            this.propDataSize = Marshal.SizeOf(typeof (sbyte));
            this.propByteField[index1] = (byte) 0;
            if (obj is bool flag)
            {
              if (!flag)
              {
                Marshal.WriteByte(this.pData, index1, (byte) 0);
                break;
              }
              Marshal.WriteByte(this.pData, index1, (byte) 1);
              this.propByteField[index1] = (byte) 1;
              break;
            }
            if (!this.IsBoolTrue(obj))
            {
              Marshal.WriteByte(this.pData, index1, (byte) 0);
              break;
            }
            Marshal.WriteByte(this.pData, index1, (byte) 1);
            this.propByteField[index1] = (byte) 1;
            break;
          case DataType.SByte:
            this.propDataSize = Marshal.SizeOf(typeof (sbyte));
            PviMarshal.WriteSByte(this.pData, index1, PviMarshal.toSByte(obj));
            break;
          case DataType.Int16:
            this.propDataSize = Marshal.SizeOf(typeof (short));
            Marshal.WriteInt16(this.pData, index1, PviMarshal.toInt16(obj));
            break;
          case DataType.Int32:
            this.propDataSize = Marshal.SizeOf(typeof (int));
            Marshal.WriteInt32(this.pData, index1, PviMarshal.toInt32(obj));
            break;
          case DataType.Int64:
            this.propDataSize = Marshal.SizeOf(typeof (long));
            PviMarshal.WriteInt64(this.pData, index1, PviMarshal.toInt64(obj));
            break;
          case DataType.Byte:
          case DataType.UInt8:
            this.propDataSize = Marshal.SizeOf(typeof (byte));
            Marshal.WriteByte(this.pData, index1, PviMarshal.toByte(obj));
            break;
          case DataType.UInt16:
          case DataType.WORD:
            this.propDataSize = Marshal.SizeOf(typeof (ushort));
            PviMarshal.WriteUInt16(this.pData, index1, PviMarshal.toUInt16(obj));
            break;
          case DataType.UInt32:
          case DataType.DWORD:
            this.propDataSize = Marshal.SizeOf(typeof (uint));
            PviMarshal.WriteUInt32(this.pData, index1, PviMarshal.toUInt32(obj));
            break;
          case DataType.UInt64:
            this.propDataSize = Marshal.SizeOf(typeof (ulong));
            PviMarshal.WriteUInt64(this.pData, index1, PviMarshal.toUInt64(obj));
            break;
          case DataType.Single:
            this.propDataSize = Marshal.SizeOf(typeof (float));
            if (this.propParent != null)
            {
              this.propParent.Service.cpyFltToBuffer(obj);
              for (int index2 = 0; index2 < 4; ++index2)
                Marshal.WriteByte(this.pData, index1 + index2, this.propParent.Service.ByteBuffer[index2]);
              break;
            }
            Marshal.Copy(new float[1]
            {
              System.Convert.ToSingle(obj)
            }, 0, this.pData, 1);
            break;
          case DataType.Double:
            this.propDataSize = Marshal.SizeOf(typeof (double));
            if (this.propParent != null)
            {
              this.propParent.Service.cpyDblToBuffer(obj);
              for (int index3 = 0; index3 < 8; ++index3)
                Marshal.WriteByte(this.pData, index1 + index3, this.propParent.Service.ByteBuffer[index3]);
              break;
            }
            Marshal.Copy(new double[1]
            {
              System.Convert.ToDouble(obj)
            }, 0, this.pData, 1);
            break;
          case DataType.TimeSpan:
          case DataType.TimeOfDay:
          case DataType.TOD:
            this.propDataSize = Marshal.SizeOf(typeof (int));
            Marshal.WriteInt32(this.pData, index1, Pvi.GetTimeSpanInt32(obj));
            break;
          case DataType.DateTime:
          case DataType.DT:
            this.propDataSize = Marshal.SizeOf(typeof (int));
            PviMarshal.WriteUInt32(this.pData, index1, Pvi.GetDateTimeUInt32(obj));
            break;
          case DataType.String:
            byte val1 = 0;
            this.propDataSize = this.DataSize;
            for (int index4 = 0; index4 < this.DataSize; ++index4)
              Marshal.WriteByte(this.pData, index1 + index4, val1);
            if (obj != null && 0 < obj.ToString().Length)
            {
              string str = obj.ToString();
              for (int index5 = 0; index5 < str.Length && index5 < this.DataSize; ++index5)
              {
                byte val2 = (byte) str[index5];
                Marshal.WriteByte(this.pData, index1 + index5, val2);
              }
              break;
            }
            break;
          case DataType.WString:
            byte val3 = 0;
            this.propDataSize = this.DataSize;
            for (int index6 = 0; index6 < this.DataSize; ++index6)
              Marshal.WriteByte(this.pData, index1 + index6, val3);
            if (obj != null && 0 < obj.ToString().Length)
            {
              byte[] destination = new byte[this.DataSize];
              IntPtr hglobalUni = PviMarshal.StringToHGlobalUni(obj.ToString());
              if (obj.ToString().Length * 2 > this.DataSize)
              {
                Marshal.Copy(hglobalUni, destination, 0, this.propDataSize - 2);
                destination[this.DataSize - 2] = (byte) 0;
                destination[this.DataSize - 1] = (byte) 0;
              }
              else
                Marshal.Copy(hglobalUni, destination, 0, obj.ToString().Length * 2);
              for (int index7 = 0; index7 < destination.Length; ++index7)
                Marshal.WriteByte(this.pData, index1 + index7, destination[index7]);
              PviMarshal.FreeHGlobal(ref hglobalUni);
              break;
            }
            break;
          case DataType.Date:
            this.propDataSize = Marshal.SizeOf(typeof (int));
            PviMarshal.WriteUInt32(this.pData, index1, Pvi.GetDateUInt32(obj));
            break;
          default:
            if (this.propParent != null)
              this.propParent.Value.isAssigned = true;
            this.isAssigned = false;
            return;
        }
        PviMarshal.Copy(this.pData, index1, index1, ref this.propByteField, this.propDataSize);
      }
    }

    public Value(object[] value)
    {
      this.isAssigned = false;
      this.Assign(value);
    }

    public void Assign(object[] value)
    {
      switch (this.DataType)
      {
        case DataType.Boolean:
          bool[] destinationArray1 = new bool[value.Length];
          if (1 < value.Length)
            Array.Copy((Array) value, 0, (Array) destinationArray1, 0, value.Length);
          else
            destinationArray1.SetValue((object) value, 0);
          this.SetByteField(destinationArray1);
          break;
        case DataType.SByte:
          sbyte[] destinationArray2 = new sbyte[value.Length];
          if (1 < value.Length)
            Array.Copy((Array) value, 0, (Array) destinationArray2, 0, value.Length);
          else
            destinationArray2.SetValue((object) value, 0);
          this.SetByteField(destinationArray2);
          break;
        case DataType.Int16:
          short[] destinationArray3 = new short[value.Length];
          if (1 < value.Length)
            Array.Copy((Array) value, 0, (Array) destinationArray3, 0, value.Length);
          else
            destinationArray3.SetValue((object) value, 0);
          this.SetByteField(destinationArray3);
          break;
        case DataType.Int32:
          int[] destinationArray4 = new int[value.Length];
          if (1 < value.Length)
            Array.Copy((Array) value, 0, (Array) destinationArray4, 0, value.Length);
          else
            destinationArray4.SetValue((object) value, 0);
          this.SetByteField(destinationArray4);
          break;
        case DataType.Int64:
          long[] destinationArray5 = new long[value.Length];
          if (1 < value.Length)
            Array.Copy((Array) value, 0, (Array) destinationArray5, 0, value.Length);
          else
            destinationArray5.SetValue((object) value, 0);
          this.SetByteField(destinationArray5);
          break;
        case DataType.Byte:
        case DataType.UInt8:
          byte[] destinationArray6 = new byte[value.Length];
          if (1 < value.Length)
            Array.Copy((Array) value, 0, (Array) destinationArray6, 0, value.Length);
          else
            destinationArray6.SetValue((object) value, 0);
          this.SetByteField(destinationArray6);
          break;
        case DataType.UInt16:
        case DataType.WORD:
          ushort[] destinationArray7 = new ushort[value.Length];
          if (1 < value.Length)
            Array.Copy((Array) value, 0, (Array) destinationArray7, 0, value.Length);
          else
            destinationArray7.SetValue((object) value, 0);
          this.SetByteField(destinationArray7);
          break;
        case DataType.UInt32:
        case DataType.DWORD:
          uint[] destinationArray8 = new uint[value.Length];
          if (1 < value.Length)
            Array.Copy((Array) value, 0, (Array) destinationArray8, 0, value.Length);
          else
            destinationArray8.SetValue((object) value, 0);
          this.SetByteField(destinationArray8);
          break;
        case DataType.UInt64:
          ulong[] destinationArray9 = new ulong[value.Length];
          if (1 < value.Length)
            Array.Copy((Array) value, 0, (Array) destinationArray9, 0, value.Length);
          else
            destinationArray9.SetValue((object) value, 0);
          this.SetByteField(destinationArray9);
          break;
        case DataType.Single:
          float[] destinationArray10 = new float[value.Length];
          if (1 < value.Length)
            Array.Copy((Array) value, 0, (Array) destinationArray10, 0, value.Length);
          else
            destinationArray10.SetValue((object) value, 0);
          this.SetByteField(destinationArray10);
          break;
        case DataType.Double:
          double[] destinationArray11 = new double[value.Length];
          if (1 < value.Length)
            Array.Copy((Array) value, 0, (Array) destinationArray11, 0, value.Length);
          else
            destinationArray11.SetValue((object) value, 0);
          this.SetByteField(destinationArray11);
          break;
        case DataType.TimeSpan:
        case DataType.TimeOfDay:
        case DataType.TOD:
          TimeSpan[] destinationArray12 = new TimeSpan[value.Length];
          if (1 < value.Length)
            Array.Copy((Array) value, 0, (Array) destinationArray12, 0, value.Length);
          else
            destinationArray12.SetValue((object) value, 0);
          this.SetByteField(destinationArray12);
          break;
        case DataType.DateTime:
        case DataType.Date:
        case DataType.DT:
          DateTime[] destinationArray13 = new DateTime[value.Length];
          if (1 < value.Length)
            Array.Copy((Array) value, 0, (Array) destinationArray13, 0, value.Length);
          else
            destinationArray13.SetValue((object) value, 0);
          this.SetByteField(destinationArray13);
          break;
        case DataType.String:
          string[] destinationArray14 = new string[value.Length];
          if (1 < value.Length)
            Array.Copy((Array) value, 0, (Array) destinationArray14, 0, value.Length);
          else
            destinationArray14.SetValue((object) value, 0);
          this.SetByteField(destinationArray14);
          break;
        default:
          throw new InvalidOperationException();
      }
    }

    private void CopyToSystemDataTypeArray(
      IntPtr pNewData,
      Array dataArray,
      ref ArrayList changedMembers)
    {
      if (this.propDataType == DataType.Unknown)
        return;
      switch (this.propDataType)
      {
        case DataType.Boolean:
          if (!(IntPtr.Zero != pNewData))
            break;
          for (int ofs = 0; ofs < this.propArrayLength; ++ofs)
          {
            if (Marshal.ReadByte(pNewData, ofs) == (byte) 0)
            {
              if (((bool[]) dataArray)[ofs])
                changedMembers.Add((object) ("[" + (object) ofs + "]"));
              ((bool[]) dataArray)[ofs] = false;
            }
            else
            {
              if (!((bool[]) dataArray)[ofs])
                changedMembers.Add((object) ("[" + (object) ofs + "]"));
              ((bool[]) dataArray)[ofs] = true;
            }
          }
          break;
        case DataType.SByte:
          if (!(IntPtr.Zero != pNewData))
            break;
          for (int ofs = 0; ofs < this.propArrayLength; ++ofs)
          {
            sbyte num = (sbyte) Marshal.ReadByte(pNewData, ofs);
            if ((int) ((sbyte[]) dataArray)[ofs] != (int) num)
              changedMembers.Add((object) ("[" + (object) ofs + "]"));
            ((sbyte[]) dataArray)[ofs] = num;
          }
          break;
        case DataType.Int16:
          if (!(IntPtr.Zero != pNewData))
            break;
          for (int index = 0; index < this.propArrayLength; ++index)
          {
            short num = Marshal.ReadInt16(pNewData, index * 2);
            if ((int) ((short[]) dataArray)[index] != (int) num)
              changedMembers.Add((object) ("[" + (object) index + "]"));
            ((short[]) dataArray)[index] = num;
          }
          break;
        case DataType.Int32:
          if (!(IntPtr.Zero != pNewData))
            break;
          for (int index = 0; index < this.propArrayLength; ++index)
          {
            int num = Marshal.ReadInt32(pNewData, index * 4);
            if (((int[]) dataArray)[index] != num)
              changedMembers.Add((object) ("[" + (object) index + "]"));
            ((int[]) dataArray)[index] = num;
          }
          break;
        case DataType.Int64:
          if (!(IntPtr.Zero != pNewData))
            break;
          for (int index = 0; index < this.propArrayLength; ++index)
          {
            long num = PviMarshal.ReadInt64(pNewData, index * 8);
            if (((long[]) dataArray)[index] != num)
              changedMembers.Add((object) ("[" + (object) index + "]"));
            ((long[]) dataArray)[index] = num;
          }
          break;
        case DataType.Byte:
        case DataType.UInt8:
          if (!(IntPtr.Zero != pNewData))
            break;
          for (int ofs = 0; ofs < this.propArrayLength; ++ofs)
          {
            byte num = Marshal.ReadByte(pNewData, ofs);
            if ((int) ((byte[]) dataArray)[ofs] != (int) num)
              changedMembers.Add((object) ("[" + (object) ofs + "]"));
            ((byte[]) dataArray)[ofs] = num;
          }
          break;
        case DataType.UInt16:
        case DataType.WORD:
          if (!(IntPtr.Zero != pNewData))
            break;
          for (int index = 0; index < this.propArrayLength; ++index)
          {
            ushort num = (ushort) Marshal.ReadInt16(pNewData, index * 2);
            if ((int) ((ushort[]) dataArray)[index] != (int) num)
              changedMembers.Add((object) ("[" + (object) index + "]"));
            ((ushort[]) dataArray)[index] = num;
          }
          break;
        case DataType.UInt32:
        case DataType.DWORD:
          if (!(IntPtr.Zero != pNewData))
            break;
          for (int index = 0; index < this.propArrayLength; ++index)
          {
            uint num = (uint) Marshal.ReadInt32(pNewData, index * 4);
            if ((int) ((uint[]) dataArray)[index] != (int) num)
              changedMembers.Add((object) ("[" + (object) index + "]"));
            ((uint[]) dataArray)[index] = num;
          }
          break;
        case DataType.UInt64:
          if (!(IntPtr.Zero != pNewData))
            break;
          for (int index = 0; index < this.propArrayLength; ++index)
          {
            ulong num = (ulong) PviMarshal.ReadInt64(pNewData, index * 8);
            if ((long) ((ulong[]) dataArray)[index] != (long) num)
              changedMembers.Add((object) ("[" + (object) index + "]"));
            ((ulong[]) dataArray)[index] = num;
          }
          break;
        case DataType.Single:
          if (!(IntPtr.Zero != pNewData))
            break;
          float[] destination1 = new float[this.propArrayLength];
          Marshal.Copy(pNewData, destination1, 0, this.propArrayLength);
          for (int index = 0; index < this.propArrayLength; ++index)
          {
            if ((double) ((float[]) dataArray)[index] != (double) destination1[index])
              changedMembers.Add((object) ("[" + (object) index + "]"));
            ((float[]) dataArray)[index] = destination1[index];
          }
          break;
        case DataType.Double:
          if (!(IntPtr.Zero != pNewData))
            break;
          double[] destination2 = new double[this.propArrayLength];
          Marshal.Copy(pNewData, destination2, 0, this.propArrayLength);
          for (int index = 0; index < this.propArrayLength; ++index)
          {
            if (((double[]) dataArray)[index] != destination2[index])
              changedMembers.Add((object) ("[" + (object) index + "]"));
            ((double[]) dataArray)[index] = destination2[index];
          }
          break;
        case DataType.String:
          if (!(IntPtr.Zero != pNewData))
            break;
          byte[] numArray = new byte[this.DataSize];
          Marshal.Copy(pNewData, numArray, 0, this.DataSize);
          IntPtr hMemory = PviMarshal.AllocHGlobal(this.propTypeLength);
          for (int index = 0; index < this.propArrayLength; ++index)
          {
            Marshal.Copy(numArray, this.propTypeLength * index, hMemory, this.propTypeLength);
            string str = PviMarshal.PtrToStringAnsi(hMemory, this.propTypeLength);
            int length = str.IndexOf("\0");
            if (-1 != length)
              str = str.Substring(0, length);
            if (str.CompareTo(((string[]) dataArray)[index]) != 0)
              changedMembers.Add((object) ("[" + (object) index + "]"));
            ((string[]) dataArray)[index] = str;
          }
          PviMarshal.FreeHGlobal(ref hMemory);
          break;
      }
    }

    private Array ToSystemDataTypeArray(byte[] bytes, int offset)
    {
      if (bytes != null && 0 < this.propArrayLength)
      {
        IntPtr zero = IntPtr.Zero;
        if (this.propDataType != DataType.Unknown)
        {
          switch (this.propDataType)
          {
            case DataType.Boolean:
              bool[] systemDataTypeArray1 = new bool[this.propArrayLength];
              for (int index = 0; index < this.propArrayLength; ++index)
                systemDataTypeArray1[index] = bytes[offset + index] != (byte) 0;
              return (Array) systemDataTypeArray1;
            case DataType.SByte:
              sbyte[] systemDataTypeArray2 = new sbyte[this.propArrayLength];
              for (int index = 0; index < this.propArrayLength; ++index)
                systemDataTypeArray2.SetValue((object) (sbyte) bytes[offset + index], index);
              return (Array) systemDataTypeArray2;
            case DataType.Int16:
              short[] destination1 = new short[this.propArrayLength];
              IntPtr hMemory1 = PviMarshal.AllocHGlobal(this.propArrayLength * 2);
              Marshal.Copy(bytes, offset, hMemory1, this.propArrayLength * 2);
              Marshal.Copy(hMemory1, destination1, 0, this.propArrayLength);
              PviMarshal.FreeHGlobal(ref hMemory1);
              return (Array) destination1;
            case DataType.Int32:
              int[] destination2 = new int[this.propArrayLength];
              IntPtr hMemory2 = PviMarshal.AllocHGlobal(this.propArrayLength * 4);
              Marshal.Copy(bytes, offset, hMemory2, this.propArrayLength * 4);
              Marshal.Copy(hMemory2, destination2, 0, this.propArrayLength);
              PviMarshal.FreeHGlobal(ref hMemory2);
              return (Array) destination2;
            case DataType.Byte:
            case DataType.UInt8:
              byte[] systemDataTypeArray3 = new byte[this.propArrayLength];
              for (int index = 0; index < this.propArrayLength; ++index)
                systemDataTypeArray3.SetValue((object) bytes[offset + index], index);
              return (Array) systemDataTypeArray3;
            case DataType.UInt16:
            case DataType.WORD:
              ushort[] systemDataTypeArray4 = new ushort[this.propArrayLength];
              IntPtr hMemory3 = PviMarshal.AllocHGlobal(this.propArrayLength * 2);
              Marshal.Copy(bytes, offset, hMemory3, this.propArrayLength * 2);
              for (int index = 0; index < this.propArrayLength; ++index)
                systemDataTypeArray4.SetValue((object) (ushort) Marshal.ReadInt16(hMemory3, index * 2), index);
              PviMarshal.FreeHGlobal(ref hMemory3);
              return (Array) systemDataTypeArray4;
            case DataType.UInt32:
            case DataType.DWORD:
              uint[] systemDataTypeArray5 = new uint[this.propArrayLength];
              IntPtr hMemory4 = PviMarshal.AllocHGlobal(this.propArrayLength * 4);
              Marshal.Copy(bytes, offset, hMemory4, this.propArrayLength * 4);
              for (int index = 0; index < this.propArrayLength; ++index)
                systemDataTypeArray5.SetValue((object) (uint) Marshal.ReadInt32(hMemory4, index * 4), index);
              PviMarshal.FreeHGlobal(ref hMemory4);
              return (Array) systemDataTypeArray5;
            case DataType.Single:
              float[] destination3 = new float[this.propArrayLength];
              IntPtr hMemory5 = PviMarshal.AllocHGlobal(this.propArrayLength * 4);
              Marshal.Copy(bytes, offset, hMemory5, this.propArrayLength * 4);
              Marshal.Copy(hMemory5, destination3, 0, this.propArrayLength);
              PviMarshal.FreeHGlobal(ref hMemory5);
              return (Array) destination3;
            case DataType.Double:
              double[] destination4 = new double[this.propArrayLength];
              IntPtr hMemory6 = PviMarshal.AllocHGlobal(this.propArrayLength * 8);
              Marshal.Copy(bytes, offset, hMemory6, this.propArrayLength * 8);
              Marshal.Copy(hMemory6, destination4, 0, this.propArrayLength);
              PviMarshal.FreeHGlobal(ref hMemory6);
              return (Array) destination4;
            case DataType.TimeSpan:
            case DataType.TimeOfDay:
            case DataType.TOD:
              TimeSpan[] systemDataTypeArray6 = new TimeSpan[this.propArrayLength];
              IntPtr hMemory7 = PviMarshal.AllocHGlobal(this.propArrayLength * 4);
              Marshal.Copy(bytes, offset, hMemory7, this.propArrayLength * 4);
              for (int index = 0; index < this.propArrayLength; ++index)
                systemDataTypeArray6.SetValue((object) new TimeSpan(10000L * (long) Marshal.ReadInt32(hMemory7, index * 4)), index);
              PviMarshal.FreeHGlobal(ref hMemory7);
              return (Array) systemDataTypeArray6;
            case DataType.DateTime:
            case DataType.Date:
            case DataType.DT:
              DateTime[] systemDataTypeArray7 = new DateTime[this.propArrayLength];
              IntPtr hMemory8 = PviMarshal.AllocHGlobal(this.propArrayLength * 4);
              Marshal.Copy(bytes, offset, hMemory8, this.propArrayLength * 4);
              for (int index = 0; index < this.propArrayLength; ++index)
                systemDataTypeArray7.SetValue((object) Pvi.UInt32ToDateTime((uint) Marshal.ReadInt32(hMemory8, index * 4)), index);
              PviMarshal.FreeHGlobal(ref hMemory8);
              return (Array) systemDataTypeArray7;
            case DataType.String:
              string[] systemDataTypeArray8 = new string[this.propArrayLength];
              for (int index = 0; index < this.propArrayLength; ++index)
                systemDataTypeArray8.SetValue((object) PviMarshal.ToAnsiString(bytes, offset + this.propTypeLength * index, this.propTypeLength), index);
              return (Array) systemDataTypeArray8;
            case DataType.WString:
              string[] systemDataTypeArray9 = new string[this.propArrayLength];
              for (int index = 0; index < this.propArrayLength; ++index)
              {
                string wstring = PviMarshal.ToWString(bytes, offset + this.propTypeLength * index, this.propTypeLength);
                systemDataTypeArray9.SetValue((object) wstring, index);
              }
              return (Array) systemDataTypeArray9;
          }
        }
      }
      return (Array) null;
    }

    private object NonSimpleToType(Type objType, IFormatProvider provider) => DataType.Structure == this.propDataType ? this.propParent.Members.FirstSimpleTyped.Value.ToType(objType, provider) : this.propParent.propPviValue[0].ToType(objType, provider);

    public bool ToBoolean(IFormatProvider provider)
    {
      bool flag = false;
      if (1 < this.propArrayLength || DataType.Structure == this.propDataType)
        return (bool) this.NonSimpleToType(flag.GetType(), provider);
      if (this.propObjValue == null)
        return false;
      switch (this.propDataType)
      {
        case DataType.Boolean:
          return this.propObjValue is Array ? ((IConvertible) ((Array) this.propObjValue).GetValue(0)).ToBoolean(provider) : ((IConvertible) this.propObjValue).ToBoolean(provider);
        case DataType.SByte:
        case DataType.Int16:
        case DataType.Int32:
        case DataType.Int64:
        case DataType.Byte:
        case DataType.UInt16:
        case DataType.UInt32:
        case DataType.UInt64:
        case DataType.Single:
        case DataType.Double:
        case DataType.String:
        case DataType.WORD:
        case DataType.DWORD:
        case DataType.UInt8:
          return this.propObjValue is Array ? ((Array) this.propObjValue).GetValue(0).ToString().CompareTo("0") != 0 : this.propObjValue.ToString().CompareTo("0") != 0;
        default:
          return ((IConvertible) this.propObjValue).ToBoolean(provider);
      }
    }

    public sbyte ToSByte(IFormatProvider provider)
    {
      sbyte num = 0;
      if (1 < this.propArrayLength || DataType.Structure == this.propDataType)
        return (sbyte) this.NonSimpleToType(num.GetType(), provider);
      switch (this.propDataType)
      {
        case DataType.Boolean:
          if (this.propObjValue is Array)
          {
            if ((bool) ((Array) this.propObjValue).GetValue(0))
              return 1;
          }
          else if ((bool) this.propObjValue)
            return 1;
          return 0;
        case DataType.SByte:
        case DataType.Int16:
        case DataType.Int32:
        case DataType.Int64:
        case DataType.Byte:
        case DataType.UInt16:
        case DataType.UInt32:
        case DataType.UInt64:
        case DataType.Single:
        case DataType.Double:
        case DataType.String:
        case DataType.WORD:
        case DataType.DWORD:
        case DataType.UInt8:
          return this.propObjValue is Array ? ((IConvertible) ((Array) this.propObjValue).GetValue(0)).ToSByte(provider) : ((IConvertible) this.propObjValue).ToSByte(provider);
        default:
          return ((IConvertible) this.propObjValue).ToSByte(provider);
      }
    }

    public short ToInt16(IFormatProvider provider)
    {
      short num = 0;
      if (1 < this.propArrayLength || DataType.Structure == this.propDataType)
        return (short) this.NonSimpleToType(num.GetType(), provider);
      switch (this.propDataType)
      {
        case DataType.Boolean:
          if (this.propObjValue is Array)
          {
            if ((bool) ((Array) this.propObjValue).GetValue(0))
              return 1;
          }
          else if ((bool) this.propObjValue)
            return 1;
          return 0;
        case DataType.SByte:
        case DataType.Int16:
        case DataType.Int32:
        case DataType.Int64:
        case DataType.Byte:
        case DataType.UInt16:
        case DataType.UInt32:
        case DataType.UInt64:
        case DataType.Single:
        case DataType.Double:
        case DataType.String:
        case DataType.WORD:
        case DataType.DWORD:
        case DataType.UInt8:
          return this.propObjValue is Array ? ((IConvertible) ((Array) this.propObjValue).GetValue(0)).ToInt16(provider) : ((IConvertible) this.propObjValue).ToInt16(provider);
        default:
          return ((IConvertible) this.propObjValue).ToInt16(provider);
      }
    }

    public int ToInt32(IFormatProvider provider)
    {
      int num = 0;
      if (1 < this.propArrayLength || DataType.Structure == this.propDataType)
        return (int) this.NonSimpleToType(num.GetType(), provider);
      switch (this.propDataType)
      {
        case DataType.Boolean:
          if (this.propObjValue is Array)
          {
            if ((bool) ((Array) this.propObjValue).GetValue(0))
              return 1;
          }
          else if ((bool) this.propObjValue)
            return 1;
          return 0;
        case DataType.SByte:
        case DataType.Int16:
        case DataType.Int32:
        case DataType.Int64:
        case DataType.Byte:
        case DataType.UInt16:
        case DataType.UInt32:
        case DataType.UInt64:
        case DataType.Single:
        case DataType.Double:
        case DataType.WORD:
        case DataType.DWORD:
        case DataType.UInt8:
          return this.propObjValue is Array ? ((IConvertible) ((Array) this.propObjValue).GetValue(0)).ToInt32(provider) : ((IConvertible) this.propObjValue).ToInt32(provider);
        case DataType.TimeSpan:
        case DataType.TimeOfDay:
        case DataType.TOD:
          return PviMarshal.toInt32((object) this.propUInt32Val);
        case DataType.DateTime:
        case DataType.Date:
        case DataType.DT:
          return PviMarshal.toInt32((object) this.propUInt32Val);
        case DataType.String:
          if ("true" == this.propObjValue.ToString().ToLower())
            return 1;
          return "false" == this.propObjValue.ToString().ToLower() ? 0 : ((IConvertible) this.propObjValue).ToInt32(provider);
        default:
          return ((IConvertible) this.propObjValue).ToInt32(provider);
      }
    }

    public byte ToByte(IFormatProvider provider)
    {
      byte num = 0;
      if (1 < this.propArrayLength || DataType.Structure == this.propDataType)
        return (byte) this.NonSimpleToType(num.GetType(), provider);
      switch (this.propDataType)
      {
        case DataType.Boolean:
          if (this.propObjValue is Array)
          {
            if ((bool) ((Array) this.propObjValue).GetValue(0))
              return 1;
          }
          else if ((bool) this.propObjValue)
            return 1;
          return 0;
        case DataType.SByte:
        case DataType.Int16:
        case DataType.Int32:
        case DataType.Int64:
        case DataType.Byte:
        case DataType.UInt16:
        case DataType.UInt32:
        case DataType.UInt64:
        case DataType.Single:
        case DataType.Double:
        case DataType.WORD:
        case DataType.DWORD:
        case DataType.UInt8:
          return this.propObjValue is Array ? ((IConvertible) ((Array) this.propObjValue).GetValue(0)).ToByte(provider) : ((IConvertible) this.propObjValue).ToByte(provider);
        case DataType.String:
          return (byte) this.propObjValue.ToString()[0];
        default:
          return ((IConvertible) this.propObjValue).ToByte(provider);
      }
    }

    public ushort ToUInt16(IFormatProvider provider)
    {
      ushort num = 0;
      if (1 < this.propArrayLength || DataType.Structure == this.propDataType)
        return (ushort) this.NonSimpleToType(num.GetType(), provider);
      switch (this.propDataType)
      {
        case DataType.Boolean:
          if (this.propObjValue is Array)
          {
            if ((bool) ((Array) this.propObjValue).GetValue(0))
              return 1;
          }
          else if ((bool) this.propObjValue)
            return 1;
          return 0;
        case DataType.SByte:
        case DataType.Int16:
        case DataType.Int32:
        case DataType.Int64:
        case DataType.Byte:
        case DataType.UInt16:
        case DataType.UInt32:
        case DataType.UInt64:
        case DataType.Single:
        case DataType.Double:
        case DataType.String:
        case DataType.WORD:
        case DataType.DWORD:
        case DataType.UInt8:
          return this.propObjValue is Array ? ((IConvertible) ((Array) this.propObjValue).GetValue(0)).ToUInt16(provider) : ((IConvertible) this.propObjValue).ToUInt16(provider);
        default:
          throw new InvalidCastException();
      }
    }

    public uint ToUInt32(IFormatProvider provider)
    {
      uint num = 0;
      if (1 < this.propArrayLength || DataType.Structure == this.propDataType)
        return (uint) this.NonSimpleToType(num.GetType(), provider);
      switch (this.propDataType)
      {
        case DataType.Boolean:
          if (this.propObjValue is Array)
          {
            if ((bool) ((Array) this.propObjValue).GetValue(0))
              return 1;
          }
          else if ((bool) this.propObjValue)
            return 1;
          return 0;
        case DataType.SByte:
        case DataType.Int16:
        case DataType.Int32:
        case DataType.Int64:
        case DataType.Byte:
        case DataType.UInt16:
        case DataType.UInt32:
        case DataType.UInt64:
        case DataType.Single:
        case DataType.Double:
        case DataType.String:
        case DataType.WORD:
        case DataType.DWORD:
        case DataType.UInt8:
          return this.propObjValue is Array ? ((IConvertible) ((Array) this.propObjValue).GetValue(0)).ToUInt32(provider) : ((IConvertible) this.propObjValue).ToUInt32(provider);
        case DataType.TimeSpan:
        case DataType.TimeOfDay:
        case DataType.TOD:
          return this.propUInt32Val;
        case DataType.DateTime:
        case DataType.Date:
        case DataType.DT:
          return this.propUInt32Val;
        default:
          throw new InvalidOperationException();
      }
    }

    public ulong ToUInt64(IFormatProvider provider)
    {
      ulong num = 0;
      if (1 < this.propArrayLength || DataType.Structure == this.propDataType)
        return (ulong) this.NonSimpleToType(num.GetType(), provider);
      switch (this.propDataType)
      {
        case DataType.Boolean:
          if (this.propObjValue is Array)
          {
            if ((bool) ((Array) this.propObjValue).GetValue(0))
              return 1;
          }
          else if ((bool) this.propObjValue)
            return 1;
          return 0;
        case DataType.SByte:
        case DataType.Int16:
        case DataType.Int32:
        case DataType.Int64:
        case DataType.Byte:
        case DataType.UInt16:
        case DataType.UInt32:
        case DataType.UInt64:
        case DataType.Single:
        case DataType.Double:
        case DataType.String:
        case DataType.WORD:
        case DataType.DWORD:
        case DataType.UInt8:
          return this.propObjValue is Array ? ((IConvertible) ((Array) this.propObjValue).GetValue(0)).ToUInt64(provider) : ((IConvertible) this.propObjValue).ToUInt64(provider);
        case DataType.TimeSpan:
        case DataType.TimeOfDay:
        case DataType.TOD:
          return System.Convert.ToUInt64(this.propUInt32Val);
        case DataType.DateTime:
        case DataType.Date:
        case DataType.DT:
          return System.Convert.ToUInt64(this.propUInt32Val);
        default:
          throw new InvalidOperationException();
      }
    }

    public float ToSingle(IFormatProvider provider)
    {
      float num = 0.0f;
      if (1 < this.propArrayLength || DataType.Structure == this.propDataType)
        return (float) this.NonSimpleToType(num.GetType(), provider);
      switch (this.propDataType)
      {
        case DataType.Boolean:
          if (this.propObjValue is Array)
          {
            if ((bool) ((Array) this.propObjValue).GetValue(0))
              return 1f;
          }
          else if ((bool) this.propObjValue)
            return 1f;
          return 0.0f;
        case DataType.SByte:
        case DataType.Int16:
        case DataType.Int32:
        case DataType.Int64:
        case DataType.Byte:
        case DataType.UInt16:
        case DataType.UInt32:
        case DataType.UInt64:
        case DataType.Single:
        case DataType.Double:
        case DataType.String:
        case DataType.WORD:
        case DataType.DWORD:
        case DataType.UInt8:
          return this.propObjValue is Array ? ((IConvertible) ((Array) this.propObjValue).GetValue(0)).ToSingle(provider) : ((IConvertible) this.propObjValue).ToSingle(provider);
        case DataType.TimeSpan:
        case DataType.TimeOfDay:
        case DataType.TOD:
          return System.Convert.ToSingle(this.propUInt32Val);
        case DataType.DateTime:
        case DataType.Date:
        case DataType.DT:
          return System.Convert.ToSingle(this.propUInt32Val);
        default:
          return ((IConvertible) this.propObjValue).ToSingle(provider);
      }
    }

    public object ToSystemDataTypeValue(IFormatProvider provider)
    {
      if (1 < this.propArrayLength || DataType.Structure == this.propDataType)
        throw new InvalidCastException();
      switch (this.propDataType)
      {
        case DataType.Boolean:
          return this.propObjValue.ToString().ToLower().CompareTo("0") == 0 || this.propObjValue.ToString().ToLower().CompareTo("false") == 0 ? (object) false : (object) true;
        case DataType.SByte:
          return (object) ((IConvertible) this.propObjValue).ToSByte(provider);
        case DataType.Int16:
          return (object) ((IConvertible) this.propObjValue).ToInt16(provider);
        case DataType.Int32:
          return (object) ((IConvertible) this.propObjValue).ToInt32(provider);
        case DataType.Int64:
          return (object) ((IConvertible) this.propObjValue).ToInt64(provider);
        case DataType.Byte:
        case DataType.UInt8:
          return (object) ((IConvertible) this.propObjValue).ToByte(provider);
        case DataType.UInt16:
        case DataType.WORD:
          return (object) ((IConvertible) this.propObjValue).ToUInt16(provider);
        case DataType.UInt32:
        case DataType.DWORD:
          return (object) ((IConvertible) this.propObjValue).ToUInt32(provider);
        case DataType.UInt64:
          return (object) ((IConvertible) this.propObjValue).ToUInt64(provider);
        case DataType.Single:
          return (object) ((IConvertible) this.propObjValue).ToSingle(provider);
        case DataType.Double:
          return (object) ((IConvertible) this.propObjValue).ToDouble(provider);
        case DataType.String:
          return (object) this.propObjValue.ToString();
        default:
          throw new InvalidCastException();
      }
    }

    public double ToDouble(IFormatProvider provider)
    {
      double num = 0.0;
      if (1 < this.propArrayLength || DataType.Structure == this.propDataType)
        return (double) this.NonSimpleToType(num.GetType(), provider);
      switch (this.propDataType)
      {
        case DataType.Boolean:
        case DataType.SByte:
        case DataType.Int16:
        case DataType.Int32:
        case DataType.Int64:
        case DataType.Byte:
        case DataType.UInt16:
        case DataType.UInt32:
        case DataType.UInt64:
        case DataType.Single:
        case DataType.Double:
        case DataType.String:
        case DataType.WORD:
        case DataType.DWORD:
        case DataType.UInt8:
          return this.propObjValue is Array ? ((IConvertible) ((Array) this.propObjValue).GetValue(0)).ToDouble(provider) : ((IConvertible) this.propObjValue).ToDouble(provider);
        case DataType.TimeSpan:
        case DataType.TimeOfDay:
        case DataType.TOD:
          return System.Convert.ToDouble(this.propUInt32Val);
        case DataType.DateTime:
        case DataType.Date:
        case DataType.DT:
          return System.Convert.ToDouble(this.propUInt32Val);
        default:
          return ((IConvertible) this.propObjValue).ToDouble(provider);
      }
    }

    public TimeSpan ToTimeSpan(IFormatProvider provider)
    {
      TimeSpan timeSpan = new TimeSpan(0L);
      if (1 < this.propArrayLength || DataType.Structure == this.propDataType)
        return (TimeSpan) this.NonSimpleToType(timeSpan.GetType(), provider);
      if (this.propObjValue == null)
        return new TimeSpan(0L);
      switch (this.propDataType)
      {
        case DataType.Boolean:
        case DataType.SByte:
        case DataType.Int16:
        case DataType.Int32:
        case DataType.Int64:
        case DataType.Byte:
        case DataType.UInt16:
        case DataType.UInt32:
        case DataType.UInt64:
        case DataType.Single:
        case DataType.Double:
        case DataType.String:
        case DataType.WORD:
        case DataType.DWORD:
        case DataType.UInt8:
          return this.propObjValue is Array ? new TimeSpan((long) ((IConvertible) ((Array) this.propObjValue).GetValue(0)).ToInt32(provider)) : new TimeSpan((long) ((IConvertible) this.propObjValue).ToInt32(provider));
        case DataType.TimeSpan:
          return (TimeSpan) this.propObjValue;
        default:
          throw new InvalidCastException();
      }
    }

    public DateTime ToDateTime(IFormatProvider provider)
    {
      DateTime dateTime = new DateTime(0L);
      if (1 < this.propArrayLength || DataType.Structure == this.propDataType)
        return (DateTime) this.NonSimpleToType(dateTime.GetType(), provider);
      if (this.propObjValue == null)
        return new DateTime(0L);
      switch (this.propDataType)
      {
        case DataType.Boolean:
        case DataType.SByte:
        case DataType.Int16:
        case DataType.Int32:
        case DataType.Int64:
        case DataType.Byte:
        case DataType.UInt16:
        case DataType.UInt32:
        case DataType.UInt64:
        case DataType.Single:
        case DataType.Double:
        case DataType.String:
        case DataType.WORD:
        case DataType.DWORD:
        case DataType.UInt8:
          return this.propObjValue is Array ? new DateTime((long) ((IConvertible) ((Array) this.propObjValue).GetValue(0)).ToUInt32(provider)) : new DateTime((long) ((IConvertible) this.propObjValue).ToUInt32(provider));
        case DataType.TimeSpan:
        case DataType.TimeOfDay:
        case DataType.TOD:
          return 0 < ((TimeSpan) this.propObjValue).Days ? new DateTime(((TimeSpan) this.propObjValue).Ticks) : System.Convert.ToDateTime(this.propObjValue.ToString());
        case DataType.DateTime:
        case DataType.Date:
        case DataType.DT:
          return (DateTime) this.propObjValue;
        default:
          return new DateTime((long) ((IConvertible) this.propObjValue).ToUInt32(provider));
      }
    }

    public string ToString(IFormatProvider provider)
    {
      if (this.propObjValue == null)
        return this.ToString((string) null, provider);
      if (this.propDataType == DataType.TimeSpan)
      {
        string str = ((TimeSpan) this.propObjValue).Days.ToString() + "." + ((TimeSpan) this.propObjValue).Hours.ToString() + ":" + ((TimeSpan) this.propObjValue).Minutes.ToString() + ":" + ((TimeSpan) this.propObjValue).Seconds.ToString();
        if (0 < ((TimeSpan) this.propObjValue).Milliseconds)
          str = str + "." + ((TimeSpan) this.propObjValue).Milliseconds.ToString("000");
        return str;
      }
      if (this.propDataType == DataType.TimeOfDay || this.propDataType == DataType.TOD)
      {
        string str = ((TimeSpan) this.propObjValue).Hours.ToString() + ":" + ((TimeSpan) this.propObjValue).Minutes.ToString() + ":" + ((TimeSpan) this.propObjValue).Seconds.ToString();
        if (0 < ((TimeSpan) this.propObjValue).Milliseconds)
          str = str + "." + ((TimeSpan) this.propObjValue).Milliseconds.ToString("000");
        return str;
      }
      if (this.propDataType == DataType.DateTime || this.propDataType == DataType.DT)
        return this.propObjValue.ToString();
      return this.propDataType == DataType.Date ? DateTime.Parse(this.propObjValue.ToString()).ToShortDateString() : ((IConvertible) this.propObjValue).ToString(provider);
    }

    public string BinaryToStringUNI()
    {
      IntPtr hMemory;
      if (this.propByteField != null)
      {
        hMemory = PviMarshal.AllocHGlobal(this.propByteField.Length);
        Marshal.Copy(this.propByteField, 0, hMemory, this.propByteField.Length);
      }
      else
        hMemory = this.pData;
      string stringUni = Marshal.PtrToStringUni(hMemory);
      if (this.propByteField != null)
        PviMarshal.FreeHGlobal(ref hMemory);
      return stringUni;
    }

    public string BinaryToAnsiString()
    {
      string ansiString = "";
      if (this.propByteField != null)
        ansiString = PviMarshal.ToAnsiString(this.propByteField);
      return ansiString;
    }

    public virtual string ToEnum()
    {
      string str;
      if (this.propObjValue != null)
      {
        this.ToInt32((IFormatProvider) null);
        str = this.ToString((IFormatProvider) null);
        if ((sbyte) 1 == this.propIsEnum && this.propEnumerations != null)
          str = this.propEnumerations.EnumName((object) this.ToString((IFormatProvider) null)) ?? this.ToString((IFormatProvider) null);
      }
      else
        str = this.ToString((IFormatProvider) null);
      return str;
    }

    public override string ToString() => this.IsPG2000String ? this.BinaryToAnsiString() : this.ToString((IFormatProvider) null);

    public bool IsPG2000String => this.propParent != null && CastModes.PG2000String == (this.propParent.CastMode & CastModes.PG2000String) && 1 < this.propArrayLength;

    private string StructMembersToString(string format, IFormatProvider provider)
    {
      string str;
      switch (this.propDataType)
      {
        case DataType.Boolean:
          str = "";
          if (this.IsOfTypeArray)
          {
            if (this.propParent.Members != null && 0 < this.propParent.Members.Count)
            {
              for (int index = 0; index < this.propParent.Members.Count; ++index)
                str = str + this.propParent.Members[index].Value.ToString(format, provider) + ";";
              break;
            }
            if (this.propObjValue != null)
            {
              str = ((IFormattable) this.propObjValue).ToString(format, provider);
              break;
            }
            break;
          }
          if (this.propObjValue != null)
          {
            if (TypeCode.Boolean == Type.GetTypeCode(this.propObjValue.GetType()))
            {
              str = ((bool) this.propObjValue).ToString(provider);
              break;
            }
            str = "false";
            if (this.propObjValue.ToString().CompareTo("0") != 0)
            {
              str = "true";
              break;
            }
            break;
          }
          break;
        case DataType.SByte:
        case DataType.Int16:
        case DataType.Int32:
        case DataType.Int64:
        case DataType.Byte:
        case DataType.UInt16:
        case DataType.UInt32:
        case DataType.UInt64:
        case DataType.Single:
        case DataType.Double:
        case DataType.DateTime:
        case DataType.Date:
        case DataType.WORD:
        case DataType.DWORD:
        case DataType.UInt8:
        case DataType.DT:
          str = "";
          if (this.IsOfTypeArray)
          {
            if (this.propParent.Members != null && 0 < this.propParent.Members.Count)
            {
              for (int index = 0; index < this.propParent.Members.Count; ++index)
                str = str + this.propParent.Members[index].Value.ToString(format, provider) + ";";
              break;
            }
            if (this.propObjValue != null)
            {
              str = ((IFormattable) this.propObjValue).ToString(format, provider);
              break;
            }
            break;
          }
          if (this.propObjValue != null)
          {
            str = ((IFormattable) this.propObjValue).ToString(format, provider);
            break;
          }
          break;
        case DataType.TimeSpan:
        case DataType.TimeOfDay:
        case DataType.TOD:
          str = "";
          if (1 < this.propArrayLength && this.propParent != null && this.propParent.Members != null)
          {
            if (0 < this.propParent.Members.Count)
            {
              for (int index = 0; index < this.propParent.Members.Count; ++index)
                str = str + this.propParent.Members[index].Value.ToString(format, provider) + ";";
              break;
            }
            if (this.propObjValue != null)
            {
              str = ((IFormattable) this.propObjValue).ToString(format, provider);
              break;
            }
            break;
          }
          if (this.propObjValue != null)
          {
            str = this.propObjValue.ToString();
            break;
          }
          break;
        case DataType.String:
          str = "";
          if (this.propObjValue != null)
          {
            str = this.propObjValue.ToString();
            break;
          }
          if (1 < this.propArrayLength)
          {
            for (int index = 0; index < this.propArrayLength; ++index)
              str = str + this.propParent.Value[index].ToString(format, provider) + ";";
            break;
          }
          if (this.propObjValue != null)
          {
            str = ((IFormattable) this.propObjValue).ToString(format, provider);
            break;
          }
          break;
        case DataType.Structure:
          str = "";
          if (this.propParent.Members != null)
          {
            for (int index = 0; index < this.propParent.Members.Count; ++index)
              str = str + this.propParent.Members[index].Value.ToString(format, provider) + ";";
            break;
          }
          if (this.propByteField != null)
          {
            for (int index = 0; index < this.propByteField.GetLength(0); ++index)
              str = str + this.propByteField.GetValue(index).ToString() + ";";
            break;
          }
          break;
        case DataType.WString:
          str = "";
          if (this.propObjValue != null)
          {
            str = this.propObjValue.ToString();
            break;
          }
          if (1 < this.propArrayLength)
          {
            for (int index = 0; index < this.propArrayLength; ++index)
              str = str + this.propParent.Value[index].ToString(format, provider) + ";";
            break;
          }
          if (this.propObjValue != null)
          {
            str = ((IFormattable) this.propObjValue).ToString(format, provider);
            break;
          }
          break;
        default:
          str = (string) null;
          break;
      }
      return str;
    }

    public string ToStringUNI(string format, IFormatProvider provider) => this.BinaryToStringUNI();

    public string ToAnsiString(string format, IFormatProvider provider) => this.BinaryToAnsiString();

    public string ToString(string format, IFormatProvider provider)
    {
      string str = (string) null;
      if (this.IsPG2000String)
        return this.BinaryToAnsiString();
      if (this.IsOfTypeArray && DataType.Structure != this.propDataType)
      {
        Array arrayData = this.ArrayData;
        if (arrayData != null)
        {
          for (int index = 0; index < this.propArrayLength; ++index)
            str = str + arrayData.GetValue(index).ToString() + ";";
        }
        else
          str = this.StructMembersToString(format, provider);
      }
      else
        str = this.StructMembersToString(format, provider);
      if (str != null && 0 < str.Length)
      {
        for (; str.Length - 1 == str.LastIndexOf(";"); str = str.Substring(0, str.Length - 1))
        {
          if (1 == str.Length)
            return "";
        }
      }
      return str;
    }

    public char ToChar(IFormatProvider provider)
    {
      char minValue = char.MinValue;
      if (1 < this.propArrayLength || DataType.Structure == this.propDataType)
        return (char) this.NonSimpleToType(minValue.GetType(), provider);
      switch (this.propDataType)
      {
        case DataType.Boolean:
          if (this.propObjValue is Array)
          {
            if ((bool) ((Array) this.propObjValue).GetValue(0))
              return '\u0001';
          }
          else if ((bool) this.propObjValue)
            return '\u0001';
          return char.MinValue;
        case DataType.SByte:
        case DataType.Int16:
        case DataType.Int32:
        case DataType.Int64:
        case DataType.Byte:
        case DataType.UInt16:
        case DataType.UInt32:
        case DataType.UInt64:
        case DataType.Single:
        case DataType.Double:
        case DataType.WORD:
        case DataType.DWORD:
        case DataType.UInt8:
          return this.propObjValue is Array ? ((IConvertible) ((Array) this.propObjValue).GetValue(0)).ToChar(provider) : ((IConvertible) this.propObjValue).ToChar(provider);
        case DataType.String:
          return this.propObjValue.ToString()[0];
        default:
          return ((IConvertible) this.propObjValue).ToChar(provider);
      }
    }

    public Decimal ToDecimal(IFormatProvider provider)
    {
      Decimal num = 0M;
      if (1 < this.propArrayLength || DataType.Structure == this.propDataType)
        return (Decimal) this.NonSimpleToType(num.GetType(), provider);
      switch (this.propDataType)
      {
        case DataType.Boolean:
        case DataType.SByte:
        case DataType.Int16:
        case DataType.Int32:
        case DataType.Int64:
        case DataType.Byte:
        case DataType.UInt16:
        case DataType.UInt32:
        case DataType.UInt64:
        case DataType.Single:
        case DataType.Double:
        case DataType.WORD:
        case DataType.DWORD:
        case DataType.UInt8:
          return ((IConvertible) this.propObjValue).ToDecimal(provider);
        case DataType.String:
          return (Decimal) this.propObjValue.ToString()[0];
        default:
          return ((IConvertible) this.propObjValue).ToDecimal(provider);
      }
    }

    public long ToInt64(IFormatProvider provider)
    {
      long num = 0;
      if (1 < this.propArrayLength || DataType.Structure == this.propDataType)
        return (long) this.NonSimpleToType(num.GetType(), provider);
      switch (this.propDataType)
      {
        case DataType.Boolean:
          if (this.propObjValue is Array)
          {
            if ((bool) ((Array) this.propObjValue).GetValue(0))
              return 1;
          }
          else if ((bool) this.propObjValue)
            return 1;
          return 0;
        case DataType.SByte:
        case DataType.Int16:
        case DataType.Int32:
        case DataType.Int64:
        case DataType.Byte:
        case DataType.UInt16:
        case DataType.UInt32:
        case DataType.UInt64:
        case DataType.Single:
        case DataType.Double:
        case DataType.WORD:
        case DataType.DWORD:
        case DataType.UInt8:
          return this.propObjValue is Array ? ((IConvertible) ((Array) this.propObjValue).GetValue(0)).ToInt64(provider) : ((IConvertible) this.propObjValue).ToInt64(provider);
        case DataType.TimeSpan:
        case DataType.TimeOfDay:
        case DataType.TOD:
          return System.Convert.ToInt64(((TimeSpan) this.propObjValue).Ticks);
        case DataType.DateTime:
        case DataType.Date:
        case DataType.DT:
          return System.Convert.ToInt64(this.propUInt32Val);
        case DataType.String:
          return (long) this.propObjValue.ToString()[0];
        default:
          return ((IConvertible) this.propObjValue).ToInt64(provider);
      }
    }

    public TypeCode GetTypeCode()
    {
      switch (this.propDataType)
      {
        case DataType.Boolean:
          return TypeCode.Boolean;
        case DataType.SByte:
          return TypeCode.SByte;
        case DataType.Int16:
          return TypeCode.Int16;
        case DataType.Int32:
          return TypeCode.Int32;
        case DataType.Int64:
          return TypeCode.Int64;
        case DataType.Byte:
        case DataType.UInt8:
          return TypeCode.Byte;
        case DataType.UInt16:
        case DataType.WORD:
          return TypeCode.UInt16;
        case DataType.UInt32:
        case DataType.DWORD:
          return TypeCode.UInt32;
        case DataType.UInt64:
          return TypeCode.UInt64;
        case DataType.Single:
          return TypeCode.Single;
        case DataType.Double:
          return TypeCode.Double;
        case DataType.TimeSpan:
        case DataType.TimeOfDay:
        case DataType.TOD:
          return TypeCode.DateTime;
        case DataType.DateTime:
        case DataType.Date:
        case DataType.DT:
          return TypeCode.DateTime;
        case DataType.String:
          return TypeCode.String;
        case DataType.Structure:
          return TypeCode.Object;
        default:
          return TypeCode.Empty;
      }
    }

    public object ToType(Type conversionType, IFormatProvider provider)
    {
      if (1 < this.propArrayLength || DataType.Structure == this.propDataType)
        return this.NonSimpleToType(conversionType, provider);
      switch (this.propDataType)
      {
        case DataType.Boolean:
          if (this.propObjValue is Array)
          {
            if ((bool) ((Array) this.propObjValue).GetValue(0))
              return (object) 1;
          }
          else if ((bool) this.propObjValue)
            return (object) 1;
          return (object) 0;
        case DataType.SByte:
        case DataType.Int16:
        case DataType.Int32:
        case DataType.Int64:
        case DataType.Byte:
        case DataType.UInt16:
        case DataType.UInt32:
        case DataType.UInt64:
        case DataType.Single:
        case DataType.Double:
        case DataType.String:
        case DataType.WORD:
        case DataType.DWORD:
        case DataType.UInt8:
          return ((IConvertible) this.propObjValue).ToType(conversionType, provider);
        case DataType.TimeSpan:
        case DataType.DateTime:
        case DataType.TimeOfDay:
        case DataType.Date:
        case DataType.TOD:
        case DataType.DT:
          return this.propObjValue;
        default:
          throw new InvalidCastException();
      }
    }

    public override int GetHashCode() => base.GetHashCode();

    public override bool Equals(object value)
    {
      if ((object) (value as Value) != null)
      {
        if (((Value) value).DataType == this.DataType)
        {
          if (this.propObjValue != null && ((Value) value).propObjValue != null)
            return this.propObjValue.Equals(((Value) value).propObjValue);
        }
        else
        {
          if (1 == this.propArrayLength && 1 == ((Value) value).propArrayLength && this.propObjValue != null && ((Value) value).propObjValue != null)
            return this.propObjValue.Equals(((Value) value).propObjValue);
          if (1 < this.propArrayLength && 1 < ((Value) value).propArrayLength && this.propArrayLength == ((Value) value).propArrayLength)
          {
            for (int index = 0; index < this.propArrayLength; ++index)
            {
              if (this[index] != ((Value) value)[index])
                return false;
            }
            return true;
          }
        }
      }
      return false;
    }

    public bool Equals(Value value)
    {
      if (value.DataType == this.DataType)
      {
        if (this.propObjValue != null && value.propObjValue != null)
          return this.propObjValue.Equals(value.propObjValue);
      }
      else
      {
        if (1 == this.propArrayLength && 1 == value.propArrayLength && this.propObjValue != null && value.propObjValue != null)
          return this.propObjValue.Equals(value.propObjValue);
        if (1 < this.propArrayLength && 1 < value.propArrayLength && this.propArrayLength == value.propArrayLength)
        {
          for (int index = 0; index < this.propArrayLength; ++index)
          {
            if (this[index] != value[index])
              return false;
          }
          return true;
        }
      }
      return false;
    }

    public bool Equals(bool value) => this.propObjValue != null && this.propObjValue.Equals((object) value);

    public bool Equals(float value) => this.propObjValue != null && this.propObjValue.Equals((object) value);

    public bool Equals(double value) => this.propObjValue != null && this.propObjValue.Equals((object) value);

    public bool Equals(sbyte value) => this.propObjValue != null && this.propObjValue.Equals((object) value);

    public bool Equals(short value) => this.propObjValue != null && this.propObjValue.Equals((object) value);

    public bool Equals(int value) => this.propObjValue != null && this.propObjValue.Equals((object) value);

    public bool Equals(long value) => this.propObjValue != null && this.propObjValue.Equals((object) value);

    public bool Equals(byte value) => this.propObjValue != null && this.propObjValue.Equals((object) value);

    public bool Equals(ushort value) => this.propObjValue != null && this.propObjValue.Equals((object) value);

    public bool Equals(uint value) => this.propObjValue != null && this.propObjValue.Equals((object) value);

    public bool Equals(ulong value) => this.propObjValue != null && this.propObjValue.Equals((object) value);

    internal void SetArrayLength(int length) => this.propArrayLength = length;

    internal void SetMinMax()
    {
      if (this.propParent.Scaling == null || this.propParent.Scaling.ScalingType == ScalingType.LimitValues || this.propParent.Scaling.ScalingType == ScalingType.LimitValuesAndFactor)
        return;
      switch (this.propDataType)
      {
        case DataType.SByte:
          this.propParent.Scaling.propMinValue = (Value) sbyte.MinValue;
          this.propParent.Scaling.propMaxValue = (Value) sbyte.MaxValue;
          break;
        case DataType.Int16:
          this.propParent.Scaling.propMinValue = (Value) short.MinValue;
          this.propParent.Scaling.propMaxValue = (Value) short.MaxValue;
          break;
        case DataType.Int32:
          this.propParent.Scaling.propMinValue = (Value) int.MinValue;
          this.propParent.Scaling.propMaxValue = (Value) int.MaxValue;
          break;
        case DataType.Int64:
          this.propParent.Scaling.propMinValue = (Value) long.MinValue;
          this.propParent.Scaling.propMaxValue = (Value) long.MaxValue;
          break;
        case DataType.Byte:
        case DataType.UInt8:
          this.propParent.Scaling.propMinValue = (Value) (byte) 0;
          this.propParent.Scaling.propMaxValue = (Value) byte.MaxValue;
          break;
        case DataType.UInt16:
        case DataType.WORD:
          this.propParent.Scaling.propMinValue = (Value) (ushort) 0;
          this.propParent.Scaling.propMaxValue = (Value) ushort.MaxValue;
          break;
        case DataType.UInt32:
        case DataType.DWORD:
          this.propParent.Scaling.propMinValue = (Value) 0U;
          this.propParent.Scaling.propMaxValue = (Value) uint.MaxValue;
          break;
        case DataType.UInt64:
          this.propParent.Scaling.propMinValue = (Value) 0UL;
          this.propParent.Scaling.propMaxValue = (Value) ulong.MaxValue;
          break;
        case DataType.Single:
          this.propParent.Scaling.propMinValue = (Value) float.MinValue;
          this.propParent.Scaling.propMaxValue = (Value) float.MaxValue;
          break;
        case DataType.Double:
          this.propParent.Scaling.propMinValue = (Value) float.MinValue;
          this.propParent.Scaling.propMaxValue = (Value) float.MaxValue;
          break;
      }
    }

    internal bool TypePreset
    {
      get => this.propTypePreset;
      set => this.propTypePreset = value;
    }

    private void PresetDataType(DataType type)
    {
      bool flag = false;
      if (this.propDataType != DataType.Unknown)
        flag = true;
      if (type == DataType.Unknown)
      {
        this.propTypePreset = false;
      }
      else
      {
        this.propTypePreset = true;
        this.propDataType = type;
        this.propArrayMinIndex = 0;
        this.propArrayMaxIndex = 0;
        switch (type - 2)
        {
          case DataType.Unknown:
          case DataType.Int64:
          case DataType.Data:
            this.propArrayLength = 1;
            this.propTypeLength = 1;
            break;
          case DataType.Boolean:
          case DataType.SByte | DataType.Int32:
          case DataType.TimeOfDay:
            this.propArrayLength = 1;
            this.propTypeLength = 2;
            break;
          case DataType.SByte:
          case DataType.Byte:
          case DataType.UInt64:
          case DataType.Date:
            this.propArrayLength = 1;
            this.propTypeLength = 4;
            break;
          case DataType.Int16:
          case DataType.UInt16:
          case DataType.Int16 | DataType.UInt16:
            this.propArrayLength = 1;
            this.propTypeLength = 8;
            break;
        }
        this.SetMinMax();
        if (!flag || !this.propParent.IsConnected || this.propParent.ConnectionType != ConnectionType.Link)
          return;
        this.propParent.Disconnect(2716U);
        this.propParent.Connect(ConnectionType.Link, 2718);
      }
    }

    internal bool SetArrayIndex(string typeDescription)
    {
      int num1 = 0;
      int startIndex1 = typeDescription.IndexOf("VS=");
      int num2 = typeDescription.IndexOf(' ', startIndex1);
      int startIndex2 = startIndex1 + "VS=".Length;
      string str = -1 != num2 ? typeDescription.Substring(startIndex2, num2 - startIndex2) : typeDescription.Substring(startIndex2);
      string[] strArray1 = str.Split(';');
      if (1 == strArray1.Length && strArray1.GetValue(0).ToString().IndexOf("a") == 0)
      {
        string[] strArray2 = str.Split(',');
        if (3 == strArray2.Length)
        {
          this.propArrayMinIndex = System.Convert.ToInt32(strArray2[1]);
          this.propArrayMaxIndex = System.Convert.ToInt32(strArray2[2]);
          this.propArryOne = true;
          return true;
        }
        if (1 == strArray2.Length)
        {
          this.propArrayMinIndex = 0;
          this.propArrayMaxIndex = 0;
          this.propArryOne = true;
          return true;
        }
      }
      else
      {
        this.InitializeArrayDimensions();
        for (int index = 0; index < strArray1.Length; ++index)
        {
          string[] strArray3 = strArray1.GetValue(index).ToString().Split(',');
          if ("a".CompareTo(strArray3.GetValue(0).ToString()) == 0 && -1 != this.ArrayDimensions.Add(strArray3))
            ++num1;
        }
      }
      return false;
    }

    internal bool SetDataType(DataType type)
    {
      bool flag = false;
      if (this.propDataType != DataType.Unknown)
        flag = true;
      this.propDataType = type;
      if (this.propParent == null)
        return flag;
      this.SetMinMax();
      if (flag && this.propParent.IsConnected && this.propParent.ConnectionType == ConnectionType.Link)
      {
        this.propParent.Disconnect(2716U);
        this.propParent.Connect(ConnectionType.Link, 2718);
      }
      return flag;
    }

    public static Value operator +(Value value1, Value value2)
    {
      switch (value1.propDataType)
      {
        case DataType.SByte:
          return new Value((int) (sbyte) value1.propObjValue + (int) (byte) value2, value1.Parent);
        case DataType.Int16:
          return new Value((int) (short) value1.propObjValue + (int) (short) value2, value1.Parent);
        case DataType.Int32:
          return new Value((int) value1.propObjValue + (int) value2, value1.Parent);
        case DataType.Int64:
          return new Value((float) ((long) value1.propObjValue + (long) value2), value1.Parent);
        case DataType.Byte:
        case DataType.UInt8:
          return new Value((int) (byte) value1.propObjValue + (int) (byte) value2, value1.Parent);
        case DataType.UInt16:
        case DataType.WORD:
          return new Value((int) (ushort) value1.propObjValue + (int) (ushort) value2, value1.Parent);
        case DataType.UInt32:
        case DataType.DWORD:
          return new Value((uint) value1.propObjValue + (uint) value2, value1.Parent);
        case DataType.UInt64:
          return new Value((ulong) value1.propObjValue + (ulong) value2, value1.Parent);
        case DataType.Single:
          return new Value((float) value1.propObjValue + (float) value2, value1.Parent);
        case DataType.Double:
          return new Value((double) value1.propObjValue + (double) value2, value1.Parent);
        case DataType.TimeSpan:
          return new Value((TimeSpan) value1.propObjValue + (TimeSpan) value2, value1.Parent);
        case DataType.String:
          return new Value(value1.propObjValue.ToString() + value2.propObjValue.ToString(), value1.Parent);
        case DataType.TimeOfDay:
        case DataType.TOD:
          return new Value((TimeSpan) value1.propObjValue + (TimeSpan) value2, value1.Parent);
        default:
          throw new InvalidOperationException();
      }
    }

    public static Value operator /(Value value1, Value value2)
    {
      double num = (double) value2;
      switch (value1.propDataType)
      {
        case DataType.SByte:
          return new Value((sbyte) ((double) (sbyte) value1.propObjValue / num), value1.Parent);
        case DataType.Int16:
          return new Value((short) ((double) (short) value1.propObjValue / num), value1.Parent);
        case DataType.Int32:
          return new Value((int) ((double) (int) value1.propObjValue / num), value1.Parent);
        case DataType.Int64:
          return new Value((float) (long) ((double) (long) value1.propObjValue / num), value1.Parent);
        case DataType.Byte:
        case DataType.UInt8:
          return new Value((byte) ((double) (byte) value1.propObjValue / num), value1.Parent);
        case DataType.UInt16:
        case DataType.WORD:
          return new Value((ushort) ((double) (ushort) value1.propObjValue / num), value1.Parent);
        case DataType.UInt32:
        case DataType.DWORD:
          return new Value((uint) ((double) (uint) value1.propObjValue / num), value1.Parent);
        case DataType.UInt64:
          return new Value((ulong) ((double) (ulong) value1.propObjValue / num), value1.Parent);
        case DataType.Single:
          return new Value((float) value1.propObjValue / (float) num, value1.Parent);
        case DataType.Double:
          return new Value((double) value1.propObjValue / num, value1.Parent);
        default:
          throw new InvalidOperationException();
      }
    }

    public static Value operator *(Value value1, Value value2)
    {
      double num = (double) value2;
      switch (value1.propDataType)
      {
        case DataType.SByte:
          return new Value((sbyte) ((double) (sbyte) value1.propObjValue * num), value1.Parent);
        case DataType.Int16:
          return new Value((short) ((double) (short) value1.propObjValue * num), value1.Parent);
        case DataType.Int32:
          return new Value((int) ((double) (int) value1.propObjValue * num), value1.Parent);
        case DataType.Int64:
          return new Value((float) (long) ((double) (long) value1.propObjValue * num), value1.Parent);
        case DataType.Byte:
        case DataType.UInt8:
          return new Value((byte) ((double) (byte) value1.propObjValue * num), value1.Parent);
        case DataType.UInt16:
        case DataType.WORD:
          return new Value((ushort) ((double) (ushort) value1.propObjValue * num), value1.Parent);
        case DataType.UInt32:
        case DataType.DWORD:
          return new Value((uint) ((double) (uint) value1.propObjValue * num), value1.Parent);
        case DataType.UInt64:
          return new Value((ulong) ((double) (ulong) value1.propObjValue * num), value1.Parent);
        case DataType.Single:
          return new Value((float) value1.propObjValue * (float) num, value1.Parent);
        case DataType.Double:
          return new Value((double) value1.propObjValue * num, value1.Parent);
        default:
          throw new InvalidOperationException();
      }
    }

    public static Value operator -(Value value1, Value value2)
    {
      switch (value1.propDataType)
      {
        case DataType.SByte:
          return new Value((int) (sbyte) value1.propObjValue - (int) (byte) value2, value1.Parent);
        case DataType.Int16:
          return new Value((int) (short) value1.propObjValue - (int) (short) value2, value1.Parent);
        case DataType.Int32:
          return new Value((int) value1.propObjValue - (int) value2, value1.Parent);
        case DataType.Int64:
          return new Value((float) ((long) value1.propObjValue - (long) value2), value1.Parent);
        case DataType.Byte:
        case DataType.UInt8:
          return new Value((int) (byte) value1.propObjValue - (int) (byte) value2, value1.Parent);
        case DataType.UInt16:
        case DataType.WORD:
          return new Value((int) (ushort) value1.propObjValue - (int) (ushort) value2, value1.Parent);
        case DataType.UInt32:
        case DataType.DWORD:
          return new Value((uint) value1.propObjValue - (uint) value2, value1.Parent);
        case DataType.UInt64:
          return new Value((ulong) value1.propObjValue - (ulong) value2, value1.Parent);
        case DataType.Single:
          return new Value((float) value1.propObjValue - (float) value2, value1.Parent);
        case DataType.Double:
          return new Value((double) value1.propObjValue - (double) value2, value1.Parent);
        case DataType.TimeSpan:
          return new Value((TimeSpan) value1.propObjValue - (TimeSpan) value2, value1.Parent);
        case DataType.DateTime:
        case DataType.DT:
          return new Value((DateTime) value1.propObjValue - (DateTime) value2, value1.Parent);
        case DataType.TimeOfDay:
        case DataType.TOD:
          return new Value((TimeSpan) value1.propObjValue - (TimeSpan) value2, value1.Parent);
        case DataType.Date:
          return new Value((DateTime) value1.propObjValue - (DateTime) value2, value1.Parent);
        default:
          throw new InvalidOperationException();
      }
    }

    public static Value operator ++(Value value)
    {
      switch (value.propDataType)
      {
        case DataType.Boolean:
        case DataType.SByte:
        case DataType.Int16:
        case DataType.Int32:
        case DataType.Int64:
        case DataType.Byte:
        case DataType.UInt16:
        case DataType.UInt32:
        case DataType.UInt64:
        case DataType.Single:
        case DataType.Double:
        case DataType.TimeSpan:
        case DataType.DateTime:
        case DataType.TimeOfDay:
        case DataType.Date:
        case DataType.WORD:
        case DataType.DWORD:
        case DataType.UInt8:
        case DataType.TOD:
        case DataType.DT:
          return value + (Value) 1;
        default:
          throw new InvalidOperationException();
      }
    }

    public static Value operator --(Value value)
    {
      switch (value.propDataType)
      {
        case DataType.Boolean:
        case DataType.SByte:
        case DataType.Int16:
        case DataType.Int32:
        case DataType.Int64:
        case DataType.Byte:
        case DataType.UInt16:
        case DataType.UInt32:
        case DataType.UInt64:
        case DataType.Single:
        case DataType.Double:
        case DataType.TimeSpan:
        case DataType.DateTime:
        case DataType.TimeOfDay:
        case DataType.Date:
        case DataType.WORD:
        case DataType.DWORD:
        case DataType.UInt8:
        case DataType.TOD:
        case DataType.DT:
          return value - (Value) 1;
        default:
          throw new InvalidOperationException();
      }
    }

    public static implicit operator bool(Value value)
    {
      if (1 < value.propArrayLength || value.propObjValue == null)
        return value.ToBoolean((IFormatProvider) null);
      if (value.propObjValue is bool)
        return (bool) value.propObjValue;
      return value.propObjValue.ToString().CompareTo("0") != 0;
    }

    public static implicit operator bool[](Value value)
    {
      if (value.propArrayLength <= 0 || value.propDataType == DataType.DateTime || value.propDataType == DataType.TimeSpan || value.propDataType == DataType.Structure)
        throw new InvalidCastException();
      bool[] flagArray = new bool[value.propArrayLength];
      for (int index = 0; index < value.propArrayLength; ++index)
        flagArray[index] = (bool) value[index];
      return flagArray;
    }

    public static implicit operator sbyte(Value value)
    {
      if (value.propArrayLength > 1 || value.propObjValue == null)
        return value.ToSByte((IFormatProvider) null);
      switch (value.propDataType)
      {
        case DataType.Boolean:
          return value.propObjValue.ToString().ToLower().CompareTo("true") != 0 ? (sbyte) 0 : (sbyte) 1;
        case DataType.SByte:
          return (sbyte) value.propObjValue;
        case DataType.Int16:
          return (sbyte) (short) value.propObjValue;
        case DataType.Int32:
          return (sbyte) (int) value.propObjValue;
        case DataType.Int64:
          return (sbyte) (long) value.propObjValue;
        case DataType.Byte:
          return (sbyte) (byte) value.propObjValue;
        case DataType.UInt16:
          return (sbyte) (ushort) value.propObjValue;
        case DataType.UInt32:
          return (sbyte) (uint) value.propObjValue;
        case DataType.UInt64:
          return (sbyte) (ulong) value.propObjValue;
        case DataType.Single:
          return (sbyte) (float) value.propObjValue;
        case DataType.Double:
          return (sbyte) (double) value.propObjValue;
        case DataType.TimeSpan:
          return System.Convert.ToSByte(((TimeSpan) value.propObjValue).Ticks);
        case DataType.DateTime:
          return System.Convert.ToSByte(((DateTime) value.propObjValue).Ticks);
        case DataType.String:
          return (sbyte) value.propObjValue.ToString()[0];
        case DataType.TimeOfDay:
          return System.Convert.ToSByte(((TimeSpan) value.propObjValue).Ticks);
        case DataType.Date:
          return System.Convert.ToSByte(((DateTime) value.propObjValue).Ticks);
        case DataType.WORD:
          return (sbyte) (ushort) value.propObjValue;
        case DataType.DWORD:
          return (sbyte) (uint) value.propObjValue;
        case DataType.UInt8:
          return (sbyte) (byte) value.propObjValue;
        case DataType.TOD:
          return System.Convert.ToSByte(((TimeSpan) value.propObjValue).Ticks);
        case DataType.DT:
          return System.Convert.ToSByte(((DateTime) value.propObjValue).Ticks);
        default:
          throw new InvalidCastException();
      }
    }

    public static implicit operator sbyte[](Value value)
    {
      if (value.propArrayLength <= 0 || value.propDataType == DataType.DateTime || value.propDataType == DataType.TimeSpan || value.propDataType == DataType.Structure)
        throw new InvalidCastException();
      sbyte[] numArray = new sbyte[value.propArrayLength];
      for (int index = 0; index < value.propArrayLength; ++index)
        numArray[index] = (sbyte) value[index];
      return numArray;
    }

    public static implicit operator short(Value value)
    {
      if (value.propArrayLength > 1 || value.propObjValue == null)
        return value.ToInt16((IFormatProvider) null);
      switch (value.propDataType)
      {
        case DataType.Boolean:
          return value.propObjValue.ToString().ToLower().CompareTo("true") != 0 ? (short) 0 : (short) 1;
        case DataType.SByte:
          return (short) (sbyte) value.propObjValue;
        case DataType.Int16:
          return (short) value.propObjValue;
        case DataType.Int32:
          return (short) (int) value.propObjValue;
        case DataType.Int64:
          return (short) (long) value.propObjValue;
        case DataType.Byte:
          return (short) (byte) value.propObjValue;
        case DataType.UInt16:
          return (short) (ushort) value.propObjValue;
        case DataType.UInt32:
          return (short) (uint) value.propObjValue;
        case DataType.UInt64:
          return (short) (ulong) value.propObjValue;
        case DataType.Single:
          return (short) (float) value.propObjValue;
        case DataType.Double:
          return (short) (double) value.propObjValue;
        case DataType.TimeSpan:
          return System.Convert.ToInt16(((TimeSpan) value.propObjValue).Ticks);
        case DataType.DateTime:
          return System.Convert.ToInt16(((DateTime) value.propObjValue).Ticks);
        case DataType.TimeOfDay:
          return System.Convert.ToInt16(((TimeSpan) value.propObjValue).Ticks);
        case DataType.Date:
          return System.Convert.ToInt16(((DateTime) value.propObjValue).Ticks);
        case DataType.WORD:
          return (short) (ushort) value.propObjValue;
        case DataType.DWORD:
          return (short) (uint) value.propObjValue;
        case DataType.UInt8:
          return (short) (byte) value.propObjValue;
        case DataType.TOD:
          return System.Convert.ToInt16(((TimeSpan) value.propObjValue).Ticks);
        case DataType.DT:
          return System.Convert.ToInt16(((DateTime) value.propObjValue).Ticks);
        default:
          return System.Convert.ToInt16(value.propObjValue.ToString());
      }
    }

    public static implicit operator short[](Value value)
    {
      if (value.propArrayLength <= 0 || value.propDataType == DataType.DateTime || value.propDataType == DataType.TimeSpan || value.propDataType == DataType.Structure)
        throw new InvalidCastException();
      short[] numArray = new short[value.propArrayLength];
      for (int index = 0; index < value.propArrayLength; ++index)
        numArray[index] = (short) value[index];
      return numArray;
    }

    public static implicit operator int(Value value)
    {
      if (value.propArrayLength > 1 || value.propObjValue == null)
        return value.ToInt32((IFormatProvider) null);
      switch (value.propDataType)
      {
        case DataType.Boolean:
          return value.propObjValue.ToString().ToLower().CompareTo("true") != 0 ? 0 : 1;
        case DataType.SByte:
          return (int) (sbyte) value.propObjValue;
        case DataType.Int16:
          return (int) (short) value.propObjValue;
        case DataType.Int32:
          return (int) value.propObjValue;
        case DataType.Int64:
          return (int) (long) value.propObjValue;
        case DataType.Byte:
          return (int) (byte) value.propObjValue;
        case DataType.UInt16:
          return (int) (ushort) value.propObjValue;
        case DataType.UInt32:
          return (int) (uint) value.propObjValue;
        case DataType.UInt64:
          return (int) (ulong) value.propObjValue;
        case DataType.Single:
          return (int) (float) value.propObjValue;
        case DataType.Double:
          return (int) (double) value.propObjValue;
        case DataType.TimeSpan:
          return (int) ((TimeSpan) value.propObjValue).Ticks;
        case DataType.DateTime:
          return (int) ((DateTime) value.propObjValue).Ticks;
        case DataType.String:
          return System.Convert.ToInt32(value.propObjValue.ToString());
        case DataType.TimeOfDay:
          return (int) ((TimeSpan) value.propObjValue).Ticks;
        case DataType.Date:
          return (int) ((DateTime) value.propObjValue).Ticks;
        case DataType.WORD:
          return (int) (ushort) value.propObjValue;
        case DataType.DWORD:
          return (int) (ushort) value.propObjValue;
        case DataType.UInt8:
          return (int) (byte) value.propObjValue;
        case DataType.TOD:
          return (int) ((TimeSpan) value.propObjValue).Ticks;
        case DataType.DT:
          return (int) ((DateTime) value.propObjValue).Ticks;
        default:
          return System.Convert.ToInt32(value.propObjValue.ToString());
      }
    }

    public static implicit operator int[](Value value)
    {
      if (value.propArrayLength <= 0 || value.propDataType == DataType.DateTime || value.propDataType == DataType.TimeSpan || value.propDataType == DataType.Structure)
        throw new InvalidOperationException();
      int[] numArray = new int[value.propArrayLength];
      for (int index = 0; index < value.propArrayLength; ++index)
        numArray[index] = (int) value[index];
      return numArray;
    }

    public static implicit operator long(Value value)
    {
      if (value.propArrayLength > 1 || value.propObjValue == null)
        return value.ToInt64((IFormatProvider) null);
      switch (value.propDataType)
      {
        case DataType.Boolean:
          return value.propObjValue.ToString().ToLower().CompareTo("true") != 0 ? 0L : 1L;
        case DataType.SByte:
          return (long) (sbyte) value.propObjValue;
        case DataType.Int16:
          return (long) (short) value.propObjValue;
        case DataType.Int32:
          return (long) (int) value.propObjValue;
        case DataType.Int64:
          return (long) value.propObjValue;
        case DataType.Byte:
          return (long) (byte) value.propObjValue;
        case DataType.UInt16:
          return (long) (ushort) value.propObjValue;
        case DataType.UInt32:
          return (long) (uint) value.propObjValue;
        case DataType.UInt64:
          return (long) (ulong) value.propObjValue;
        case DataType.Single:
          return (long) (float) value.propObjValue;
        case DataType.Double:
          return (long) (double) value.propObjValue;
        case DataType.TimeSpan:
          return System.Convert.ToInt64(((TimeSpan) value.propObjValue).Ticks);
        case DataType.DateTime:
          return System.Convert.ToInt64(((DateTime) value.propObjValue).Ticks);
        case DataType.String:
          return System.Convert.ToInt64(value.propObjValue.ToString());
        case DataType.TimeOfDay:
          return System.Convert.ToInt64(((TimeSpan) value.propObjValue).Ticks);
        case DataType.Date:
          return System.Convert.ToInt64(((DateTime) value.propObjValue).Ticks);
        case DataType.WORD:
          return (long) (ushort) value.propObjValue;
        case DataType.DWORD:
          return (long) (uint) value.propObjValue;
        case DataType.UInt8:
          return (long) (byte) value.propObjValue;
        case DataType.TOD:
          return System.Convert.ToInt64(((TimeSpan) value.propObjValue).Ticks);
        case DataType.DT:
          return System.Convert.ToInt64(((DateTime) value.propObjValue).Ticks);
        default:
          return System.Convert.ToInt64(value.propObjValue.ToString());
      }
    }

    public static implicit operator long[](Value value)
    {
      if (value.propArrayLength <= 0 || value.propDataType == DataType.DateTime || value.propDataType == DataType.TimeSpan || value.propDataType == DataType.Structure)
        throw new InvalidOperationException();
      long[] numArray = new long[value.propArrayLength];
      for (int index = 0; index < value.propArrayLength; ++index)
        numArray[index] = (long) value[index];
      return numArray;
    }

    public static implicit operator byte(Value value)
    {
      if (value.propArrayLength > 1 || value.propObjValue == null)
        return value.ToByte((IFormatProvider) null);
      switch (value.propDataType)
      {
        case DataType.Boolean:
          return value.propObjValue.ToString().ToLower().CompareTo("true") != 0 ? (byte) 0 : (byte) 1;
        case DataType.SByte:
          return (byte) (sbyte) value.propObjValue;
        case DataType.Int16:
          return (byte) (short) value.propObjValue;
        case DataType.Int32:
          return (byte) (int) value.propObjValue;
        case DataType.Int64:
          return (byte) (long) value.propObjValue;
        case DataType.Byte:
          return (byte) value.propObjValue;
        case DataType.UInt16:
          return (byte) (ushort) value.propObjValue;
        case DataType.UInt32:
          return (byte) (uint) value.propObjValue;
        case DataType.UInt64:
          return (byte) (ulong) value.propObjValue;
        case DataType.Single:
          return (byte) (float) value.propObjValue;
        case DataType.Double:
          return (byte) (double) value.propObjValue;
        case DataType.TimeSpan:
          return System.Convert.ToByte(((TimeSpan) value.propObjValue).Ticks);
        case DataType.DateTime:
          return System.Convert.ToByte(((DateTime) value.propObjValue).Ticks);
        case DataType.TimeOfDay:
          return System.Convert.ToByte(((TimeSpan) value.propObjValue).Ticks);
        case DataType.Date:
          return System.Convert.ToByte(((DateTime) value.propObjValue).Ticks);
        case DataType.WORD:
          return (byte) (ushort) value.propObjValue;
        case DataType.DWORD:
          return (byte) (uint) value.propObjValue;
        case DataType.UInt8:
          return (byte) value.propObjValue;
        case DataType.TOD:
          return System.Convert.ToByte(((TimeSpan) value.propObjValue).Ticks);
        case DataType.DT:
          return System.Convert.ToByte(((DateTime) value.propObjValue).Ticks);
        default:
          return System.Convert.ToByte(value.propObjValue.ToString());
      }
    }

    public static implicit operator byte[](Value value)
    {
      if (value.propArrayLength <= 0 || value.propDataType == DataType.DateTime || value.propDataType == DataType.TimeSpan || value.propDataType == DataType.Structure)
        throw new InvalidOperationException();
      byte[] numArray = new byte[value.propArrayLength];
      for (int index = 0; index < value.propArrayLength; ++index)
        numArray[index] = (byte) value[index];
      return numArray;
    }

    public static implicit operator ushort(Value value)
    {
      if (value.propArrayLength > 1 || value.propObjValue == null)
        return value.ToUInt16((IFormatProvider) null);
      switch (value.propDataType)
      {
        case DataType.Boolean:
          return value.propObjValue.ToString().ToLower().CompareTo("true") != 0 ? (ushort) 0 : (ushort) 1;
        case DataType.SByte:
          return (ushort) (sbyte) value.propObjValue;
        case DataType.Int16:
          return (ushort) (short) value.propObjValue;
        case DataType.Int32:
          return (ushort) (int) value.propObjValue;
        case DataType.Int64:
          return (ushort) (long) value.propObjValue;
        case DataType.Byte:
          return (ushort) (byte) value.propObjValue;
        case DataType.UInt16:
          return (ushort) value.propObjValue;
        case DataType.UInt32:
          return (ushort) (uint) value.propObjValue;
        case DataType.UInt64:
          return (ushort) (ulong) value.propObjValue;
        case DataType.Single:
          return (ushort) (float) value.propObjValue;
        case DataType.Double:
          return (ushort) (double) value.propObjValue;
        case DataType.TimeSpan:
          return System.Convert.ToUInt16(((TimeSpan) value.propObjValue).Ticks);
        case DataType.DateTime:
          return System.Convert.ToUInt16(((DateTime) value.propObjValue).Ticks);
        case DataType.TimeOfDay:
          return System.Convert.ToUInt16(((TimeSpan) value.propObjValue).Ticks);
        case DataType.Date:
          return System.Convert.ToUInt16(((DateTime) value.propObjValue).Ticks);
        case DataType.WORD:
          return (ushort) value.propObjValue;
        case DataType.DWORD:
          return (ushort) (uint) value.propObjValue;
        case DataType.UInt8:
          return (ushort) (byte) value.propObjValue;
        case DataType.TOD:
          return System.Convert.ToUInt16(((TimeSpan) value.propObjValue).Ticks);
        case DataType.DT:
          return System.Convert.ToUInt16(((DateTime) value.propObjValue).Ticks);
        default:
          return System.Convert.ToUInt16(value.propObjValue.ToString());
      }
    }

    public static implicit operator ushort[](Value value)
    {
      if (value.propArrayLength <= 0 || value.propDataType == DataType.DateTime || value.propDataType == DataType.TimeSpan || value.propDataType == DataType.Structure)
        throw new InvalidOperationException();
      ushort[] numArray = new ushort[value.propArrayLength];
      for (int index = 0; index < value.propArrayLength; ++index)
        numArray[index] = (ushort) value[index];
      return numArray;
    }

    public static implicit operator uint(Value value)
    {
      if (value.propArrayLength > 1 || value.propObjValue == null)
        return value.ToUInt32((IFormatProvider) null);
      switch (value.propDataType)
      {
        case DataType.Boolean:
          return value.propObjValue.ToString().ToLower().CompareTo("true") != 0 ? 0U : 1U;
        case DataType.SByte:
          return (uint) (sbyte) value.propObjValue;
        case DataType.Int16:
          return (uint) (short) value.propObjValue;
        case DataType.Int32:
          return (uint) (int) value.propObjValue;
        case DataType.Int64:
          return (uint) (long) value.propObjValue;
        case DataType.Byte:
          return (uint) (byte) value.propObjValue;
        case DataType.UInt16:
          return (uint) (ushort) value.propObjValue;
        case DataType.UInt32:
          return (uint) value.propObjValue;
        case DataType.UInt64:
          return (uint) (ulong) value.propObjValue;
        case DataType.Single:
          return (uint) (float) value.propObjValue;
        case DataType.Double:
          return (uint) (double) value.propObjValue;
        case DataType.TimeSpan:
          return System.Convert.ToUInt32(((TimeSpan) value.propObjValue).Ticks);
        case DataType.DateTime:
          return System.Convert.ToUInt32(((DateTime) value.propObjValue).Ticks);
        case DataType.TimeOfDay:
          return System.Convert.ToUInt32(((TimeSpan) value.propObjValue).Ticks);
        case DataType.Date:
          return System.Convert.ToUInt32(((DateTime) value.propObjValue).Ticks);
        case DataType.WORD:
          return (uint) (ushort) value.propObjValue;
        case DataType.DWORD:
          return (uint) value.propObjValue;
        case DataType.UInt8:
          return (uint) (byte) value.propObjValue;
        case DataType.TOD:
          return System.Convert.ToUInt32(((TimeSpan) value.propObjValue).Ticks);
        case DataType.DT:
          return System.Convert.ToUInt32(((DateTime) value.propObjValue).Ticks);
        default:
          return System.Convert.ToUInt32(value.propObjValue.ToString());
      }
    }

    public static implicit operator uint[](Value value)
    {
      if (value.propArrayLength <= 0 || value.propDataType == DataType.DateTime || value.propDataType == DataType.TimeSpan || value.propDataType == DataType.Structure)
        throw new InvalidOperationException();
      uint[] numArray = new uint[value.propArrayLength];
      for (int index = 0; index < value.propArrayLength; ++index)
        numArray[index] = (uint) value[index];
      return numArray;
    }

    public static implicit operator ulong(Value value)
    {
      if (value.propArrayLength > 1 || value.propObjValue == null)
        return value.ToUInt64((IFormatProvider) null);
      switch (value.propDataType)
      {
        case DataType.Boolean:
          return value.propObjValue.ToString().ToLower().CompareTo("true") != 0 ? 0UL : 1UL;
        case DataType.SByte:
          return (ulong) (sbyte) value.propObjValue;
        case DataType.Int16:
          return (ulong) (short) value.propObjValue;
        case DataType.Int32:
          return (ulong) (int) value.propObjValue;
        case DataType.Int64:
          return (ulong) (long) value.propObjValue;
        case DataType.Byte:
          return (ulong) (byte) value.propObjValue;
        case DataType.UInt16:
          return (ulong) (ushort) value.propObjValue;
        case DataType.UInt32:
          return (ulong) (uint) value.propObjValue;
        case DataType.UInt64:
          return (ulong) value.propObjValue;
        case DataType.Single:
          return (ulong) (float) value.propObjValue;
        case DataType.Double:
          return (ulong) (double) value.propObjValue;
        case DataType.TimeSpan:
          return System.Convert.ToUInt64(((TimeSpan) value.propObjValue).Ticks);
        case DataType.DateTime:
          return System.Convert.ToUInt64(((DateTime) value.propObjValue).Ticks);
        case DataType.TimeOfDay:
          return System.Convert.ToUInt64(((TimeSpan) value.propObjValue).Ticks);
        case DataType.Date:
          return System.Convert.ToUInt64(((DateTime) value.propObjValue).Ticks);
        case DataType.WORD:
          return (ulong) (ushort) value.propObjValue;
        case DataType.DWORD:
          return (ulong) (uint) value.propObjValue;
        case DataType.UInt8:
          return (ulong) (byte) value.propObjValue;
        case DataType.TOD:
          return System.Convert.ToUInt64(((TimeSpan) value.propObjValue).Ticks);
        case DataType.DT:
          return System.Convert.ToUInt64(((DateTime) value.propObjValue).Ticks);
        default:
          return System.Convert.ToUInt64(value.propObjValue.ToString());
      }
    }

    public static implicit operator ulong[](Value value)
    {
      if (value.propArrayLength <= 0 || value.propDataType == DataType.DateTime || value.propDataType == DataType.TimeSpan || value.propDataType == DataType.Structure)
        throw new InvalidOperationException();
      ulong[] numArray = new ulong[value.propArrayLength];
      for (int index = 0; index < value.propArrayLength; ++index)
        numArray[index] = (ulong) value[index];
      return numArray;
    }

    public static implicit operator float(Value value)
    {
      if (value.propArrayLength > 1 || value.propObjValue == null)
        return value.ToSingle((IFormatProvider) null);
      switch (value.propDataType)
      {
        case DataType.Boolean:
          return value.propObjValue.ToString().ToLower().CompareTo("true") != 0 ? 0.0f : 1f;
        case DataType.SByte:
          return (float) (sbyte) value.propObjValue;
        case DataType.Int16:
          return (float) (short) value.propObjValue;
        case DataType.Int32:
          return (float) (int) value.propObjValue;
        case DataType.Int64:
          return (float) (long) value.propObjValue;
        case DataType.Byte:
          return (float) (byte) value.propObjValue;
        case DataType.UInt16:
          return (float) (ushort) value.propObjValue;
        case DataType.UInt32:
          return (float) (uint) value.propObjValue;
        case DataType.UInt64:
          return (float) (ulong) value.propObjValue;
        case DataType.Single:
          return (float) value.propObjValue;
        case DataType.Double:
          return (float) (double) value.propObjValue;
        case DataType.TimeSpan:
          return System.Convert.ToSingle(((TimeSpan) value.propObjValue).Ticks);
        case DataType.DateTime:
          return System.Convert.ToSingle(((DateTime) value.propObjValue).Ticks);
        case DataType.TimeOfDay:
          return System.Convert.ToSingle(((TimeSpan) value.propObjValue).Ticks);
        case DataType.Date:
          return System.Convert.ToSingle(((DateTime) value.propObjValue).Ticks);
        case DataType.WORD:
          return (float) (ushort) value.propObjValue;
        case DataType.DWORD:
          return (float) (uint) value.propObjValue;
        case DataType.UInt8:
          return (float) (byte) value.propObjValue;
        case DataType.TOD:
          return System.Convert.ToSingle(((TimeSpan) value.propObjValue).Ticks);
        case DataType.DT:
          return System.Convert.ToSingle(((DateTime) value.propObjValue).Ticks);
        default:
          return System.Convert.ToSingle(value.propObjValue.ToString());
      }
    }

    public static implicit operator float[](Value value)
    {
      if (value.propArrayLength <= 0 || value.propDataType == DataType.DateTime || value.propDataType == DataType.TimeSpan || value.propDataType == DataType.Structure)
        throw new InvalidOperationException();
      float[] numArray = new float[value.propArrayLength];
      for (int index = 0; index < value.propArrayLength; ++index)
        numArray[index] = (float) value[index];
      return numArray;
    }

    public static implicit operator double(Value value)
    {
      if (value.propArrayLength > 1 || value.propObjValue == null)
        return value.ToDouble((IFormatProvider) null);
      switch (value.propDataType)
      {
        case DataType.Boolean:
          return value.propObjValue.ToString().ToLower().CompareTo("true") != 0 ? 0.0 : 1.0;
        case DataType.SByte:
          return (double) (sbyte) value.propObjValue;
        case DataType.Int16:
          return (double) (short) value.propObjValue;
        case DataType.Int32:
          return (double) (int) value.propObjValue;
        case DataType.Int64:
          return (double) (long) value.propObjValue;
        case DataType.Byte:
          return (double) (byte) value.propObjValue;
        case DataType.UInt16:
          return (double) (ushort) value.propObjValue;
        case DataType.UInt32:
          return (double) (uint) value.propObjValue;
        case DataType.UInt64:
          return (double) (ulong) value.propObjValue;
        case DataType.Single:
          return (double) (float) value.propObjValue;
        case DataType.Double:
          return (double) value.propObjValue;
        case DataType.TimeSpan:
          return System.Convert.ToDouble(((TimeSpan) value.propObjValue).Ticks);
        case DataType.DateTime:
          return System.Convert.ToDouble(((DateTime) value.propObjValue).Ticks);
        case DataType.TimeOfDay:
          return System.Convert.ToDouble(((TimeSpan) value.propObjValue).Ticks);
        case DataType.Date:
          return System.Convert.ToDouble(((DateTime) value.propObjValue).Ticks);
        case DataType.WORD:
          return (double) (ushort) value.propObjValue;
        case DataType.DWORD:
          return (double) (uint) value.propObjValue;
        case DataType.UInt8:
          return (double) (byte) value.propObjValue;
        case DataType.TOD:
          return System.Convert.ToDouble(((TimeSpan) value.propObjValue).Ticks);
        case DataType.DT:
          return System.Convert.ToDouble(((DateTime) value.propObjValue).Ticks);
        default:
          return System.Convert.ToDouble(value.propObjValue.ToString());
      }
    }

    public static implicit operator double[](Value value)
    {
      if (value.propArrayLength <= 0 || value.propDataType == DataType.DateTime || value.propDataType == DataType.TimeSpan || value.propDataType == DataType.Structure)
        throw new InvalidOperationException();
      double[] numArray = new double[value.propArrayLength];
      for (int index = 0; index < value.propArrayLength; ++index)
        numArray[index] = (double) value[index];
      return numArray;
    }

    public static implicit operator TimeSpan(Value value)
    {
      if (value.propArrayLength > 1 || value.propObjValue == null)
        return value.ToTimeSpan((IFormatProvider) null);
      switch (value.propDataType)
      {
        case DataType.Boolean:
          return value.propObjValue.ToString().ToLower().CompareTo("true") != 0 ? new TimeSpan(0L) : new TimeSpan(1L);
        case DataType.SByte:
          return new TimeSpan((long) (sbyte) value.propObjValue);
        case DataType.Int16:
          return new TimeSpan((long) (short) value.propObjValue);
        case DataType.Int32:
          return new TimeSpan((long) (int) value.propObjValue);
        case DataType.Int64:
          return new TimeSpan((long) (int) value.propObjValue);
        case DataType.Byte:
          return new TimeSpan((long) (byte) value.propObjValue);
        case DataType.UInt16:
          return new TimeSpan((long) (ushort) value.propObjValue);
        case DataType.UInt32:
          return new TimeSpan((long) (uint) value.propObjValue);
        case DataType.UInt64:
          return new TimeSpan((long) (uint) value.propObjValue);
        case DataType.Single:
          return new TimeSpan((long) (uint) value.propObjValue);
        case DataType.Double:
          return new TimeSpan((long) (uint) value.propObjValue);
        case DataType.TimeSpan:
          return (TimeSpan) value.propObjValue;
        case DataType.DateTime:
          return new TimeSpan((long) (uint) ((DateTime) value.propObjValue).Ticks);
        case DataType.TimeOfDay:
          return (TimeSpan) value.propObjValue;
        case DataType.Date:
          return new TimeSpan((long) (uint) ((DateTime) value.propObjValue).Ticks);
        case DataType.WORD:
          return new TimeSpan((long) (ushort) value.propObjValue);
        case DataType.DWORD:
          return new TimeSpan((long) (uint) value.propObjValue);
        case DataType.UInt8:
          return new TimeSpan((long) (byte) value.propObjValue);
        case DataType.TOD:
          return (TimeSpan) value.propObjValue;
        case DataType.DT:
          return new TimeSpan((long) (uint) ((DateTime) value.propObjValue).Ticks);
        default:
          throw new InvalidCastException();
      }
    }

    public static implicit operator TimeSpan[](Value value)
    {
      if (value.propArrayLength <= 0 || value.propDataType == DataType.DateTime || value.propDataType == DataType.Structure)
        throw new InvalidCastException();
      TimeSpan[] timeSpanArray = new TimeSpan[value.propArrayLength];
      for (int index = 0; index < value.propArrayLength; ++index)
        timeSpanArray[index] = (TimeSpan) value[index];
      return timeSpanArray;
    }

    public static implicit operator DateTime(Value value)
    {
      if (value.propArrayLength > 1 || value.propObjValue == null)
        return value.ToDateTime((IFormatProvider) null);
      switch (value.propDataType)
      {
        case DataType.Boolean:
          return value.propObjValue.ToString().ToLower().CompareTo("true") != 0 ? new DateTime(0L) : new DateTime(1L);
        case DataType.SByte:
          return new DateTime((long) (sbyte) value.propObjValue);
        case DataType.Int16:
          return new DateTime((long) (short) value.propObjValue);
        case DataType.Int32:
          return new DateTime((long) (int) value.propObjValue);
        case DataType.Int64:
          return new DateTime((long) (int) value.propObjValue);
        case DataType.Byte:
          return new DateTime((long) (byte) value.propObjValue);
        case DataType.UInt16:
          return new DateTime((long) (ushort) value.propObjValue);
        case DataType.UInt32:
          return new DateTime((long) (uint) value.propObjValue);
        case DataType.UInt64:
          return new DateTime((long) (uint) value.propObjValue);
        case DataType.Single:
          return new DateTime((long) (uint) value.propObjValue);
        case DataType.Double:
          return new DateTime((long) (uint) value.propObjValue);
        case DataType.TimeSpan:
          return new DateTime((long) (uint) ((TimeSpan) value.propObjValue).Ticks);
        case DataType.DateTime:
          return (DateTime) value.propObjValue;
        case DataType.TimeOfDay:
          return new DateTime((long) (uint) ((TimeSpan) value.propObjValue).Ticks);
        case DataType.Date:
          return (DateTime) value.propObjValue;
        case DataType.WORD:
          return new DateTime((long) (ushort) value.propObjValue);
        case DataType.DWORD:
          return new DateTime((long) (uint) value.propObjValue);
        case DataType.UInt8:
          return new DateTime((long) (byte) value.propObjValue);
        case DataType.TOD:
          return new DateTime((long) (uint) ((TimeSpan) value.propObjValue).Ticks);
        case DataType.DT:
          return (DateTime) value.propObjValue;
        default:
          throw new InvalidCastException();
      }
    }

    public static implicit operator DateTime[](Value value)
    {
      if (value.propArrayLength <= 0 || value.propDataType == DataType.Structure)
        throw new InvalidCastException();
      DateTime[] dateTimeArray = new DateTime[value.propArrayLength];
      for (int index = 0; index < value.propArrayLength; ++index)
        dateTimeArray[index] = (DateTime) value[index];
      return dateTimeArray;
    }

    public static implicit operator string(Value value) => value.propDataType == DataType.Structure ? value.ToString((IFormatProvider) null) : value.ToString();

    public static implicit operator string[](Value value)
    {
      if (value.propArrayLength <= 0 || value.propDataType == DataType.Structure)
        throw new InvalidOperationException();
      string[] strArray = new string[value.propArrayLength];
      for (int index = 0; index < value.propArrayLength; ++index)
        strArray[index] = value[index].ToString();
      return strArray;
    }

    public static implicit operator Value(bool value) => new Value(value);

    public static implicit operator Value(bool[] value) => new Value(value);

    public static implicit operator Value(sbyte value) => new Value(value);

    public static implicit operator Value(sbyte[] value) => new Value(value);

    public static implicit operator Value(short value) => new Value(value);

    public static implicit operator Value(short[] value) => new Value(value);

    public static implicit operator Value(int value) => new Value(value);

    public static implicit operator Value(int[] value) => new Value(value);

    public static implicit operator Value(long value) => new Value((float) value);

    public static implicit operator Value(long[] value) => new Value((object) value);

    public static implicit operator Value(byte value) => new Value(value);

    public static implicit operator Value(byte[] value) => new Value(value);

    public static implicit operator Value(ushort value) => new Value(value);

    public static implicit operator Value(ushort[] value) => new Value(value);

    public static implicit operator Value(uint value) => new Value(value);

    public static implicit operator Value(uint[] value) => new Value(value);

    public static implicit operator Value(ulong value) => new Value(value);

    public static implicit operator Value(ulong[] value) => new Value(value);

    public static implicit operator Value(float value) => new Value(value);

    public static implicit operator Value(float[] value) => new Value(value);

    public static implicit operator Value(double value) => new Value(value);

    public static implicit operator Value(double[] value) => new Value(value);

    public static implicit operator Value(TimeSpan value) => new Value(value);

    public static implicit operator Value(TimeSpan[] value) => new Value(value);

    public static implicit operator Value(DateTime value) => new Value(value);

    public static implicit operator Value(DateTime[] value) => new Value(value);

    public static implicit operator Value(string value) => new Value(value);

    public static implicit operator Value(string[] value) => new Value(value);

    public static bool operator ==(Value value1, Value value2)
    {
      if ((object) value1 != null && (object) value2 != null)
        return value1.Equals(value2);
      return (object) value1 == null && (object) value2 == null;
    }

    public static bool operator ==(Value value1, bool value2) => (object) value1 != null && value1.Equals(value2);

    public static bool operator ==(Value value1, float value2) => (object) value1 != null && value1.Equals(value2);

    public static bool operator ==(Value value1, double value2) => (object) value1 != null && value1.Equals(value2);

    public static bool operator ==(Value value1, sbyte value2) => (object) value1 != null && value1.Equals(value2);

    public static bool operator ==(Value value1, short value2) => (object) value1 != null && value1.Equals(value2);

    public static bool operator ==(Value value1, int value2) => (object) value1 != null && value1.Equals(value2);

    public static bool operator ==(Value value1, long value2) => (object) value1 != null && value1.Equals(value2);

    public static bool operator ==(Value value1, byte value2) => (object) value1 != null && value1.Equals(value2);

    public static bool operator ==(Value value1, ushort value2) => (object) value1 != null && value1.Equals(value2);

    public static bool operator ==(Value value1, uint value2) => (object) value1 != null && value1.Equals(value2);

    public static bool operator ==(Value value1, ulong value2) => (object) value1 != null && value1.Equals(value2);

    public static bool operator !=(Value value1, Value value2)
    {
      if ((object) value1 != null && (object) value2 != null)
        return !value1.Equals(value2);
      return (object) value1 != null || (object) value2 != null;
    }

    public static bool operator !=(Value value1, byte value2) => (object) value1 == null || !value1.Equals(value2);

    public static bool operator !=(Value value1, ushort value2) => (object) value1 == null || !value1.Equals(value2);

    public static bool operator !=(Value value1, uint value2) => (object) value1 == null || !value1.Equals(value2);

    public static bool operator !=(Value value1, ulong value2) => (object) value1 == null || !value1.Equals(value2);

    public static bool operator !=(Value value1, bool value2) => (object) value1 == null || !value1.Equals(value2);

    public static bool operator !=(Value value1, float value2) => (object) value1 == null || !value1.Equals(value2);

    public static bool operator !=(Value value1, double value2) => (object) value1 == null || !value1.Equals(value2);

    public static bool operator !=(Value value1, sbyte value2) => (object) value1 == null || !value1.Equals(value2);

    public static bool operator !=(Value value1, short value2) => (object) value1 == null || !value1.Equals(value2);

    public static bool operator !=(Value value1, int value2) => (object) value1 == null || !value1.Equals(value2);

    public static bool operator !=(Value value1, long value2) => (object) value1 == null || !value1.Equals(value2);

    public int DataSize => this.propArrayLength * this.propTypeLength;

    public bool IsOfTypeArray => this.propArryOne || 1 < this.propArrayLength;

    public int ArrayMinIndex => this.propArrayMinIndex;

    public int ArrayMaxIndex => this.propArrayMaxIndex;

    public DataType DataType
    {
      get => this.propDataType;
      set => this.PresetDataType(value);
    }

    public IECDataTypes IECDataType
    {
      get
      {
        switch (this.propDataType)
        {
          case DataType.Boolean:
            return IECDataTypes.BOOL;
          case DataType.SByte:
            return IECDataTypes.SINT;
          case DataType.Int16:
            return IECDataTypes.INT;
          case DataType.Int32:
            return IECDataTypes.DINT;
          case DataType.Int64:
            return IECDataTypes.LINT;
          case DataType.Byte:
            return IECDataTypes.BYTE;
          case DataType.UInt16:
            return IECDataTypes.UINT;
          case DataType.UInt32:
            return IECDataTypes.UDINT;
          case DataType.UInt64:
            return IECDataTypes.ULINT;
          case DataType.Single:
            return IECDataTypes.REAL;
          case DataType.Double:
            return IECDataTypes.LREAL;
          case DataType.TimeSpan:
            return IECDataTypes.TIME;
          case DataType.DateTime:
            return IECDataTypes.DATE_AND_TIME;
          case DataType.String:
            return IECDataTypes.STRING;
          case DataType.Structure:
            return IECDataTypes.STRUCT;
          case DataType.WString:
            return IECDataTypes.WSTRING;
          case DataType.TimeOfDay:
            return IECDataTypes.TIME_OF_DAY;
          case DataType.Date:
            return IECDataTypes.DATE;
          case DataType.WORD:
            return IECDataTypes.WORD;
          case DataType.DWORD:
            return IECDataTypes.DWORD;
          case DataType.LWORD:
            return IECDataTypes.LWORD;
          case DataType.UInt8:
            return IECDataTypes.USINT;
          case DataType.TOD:
            return IECDataTypes.TOD;
          case DataType.DT:
            return IECDataTypes.DT;
          default:
            return IECDataTypes.UNDEFINED;
        }
      }
    }

    public TypeCode SystemDataType
    {
      get
      {
        switch (this.propDataType)
        {
          case DataType.Boolean:
            return TypeCode.Boolean;
          case DataType.SByte:
            return TypeCode.SByte;
          case DataType.Int16:
            return TypeCode.Int16;
          case DataType.Int32:
            return TypeCode.Int32;
          case DataType.Int64:
            return TypeCode.Int64;
          case DataType.Byte:
            return TypeCode.Byte;
          case DataType.UInt16:
            return TypeCode.UInt16;
          case DataType.UInt32:
            return TypeCode.UInt32;
          case DataType.UInt64:
            return TypeCode.UInt64;
          case DataType.Single:
            return TypeCode.Single;
          case DataType.Double:
            return TypeCode.Double;
          case DataType.TimeSpan:
            return TypeCode.DateTime;
          case DataType.DateTime:
            return TypeCode.DateTime;
          case DataType.String:
            return TypeCode.String;
          case DataType.Structure:
            return TypeCode.Object;
          case DataType.TimeOfDay:
            return TypeCode.DateTime;
          case DataType.Date:
            return TypeCode.DateTime;
          case DataType.WORD:
            return TypeCode.UInt16;
          case DataType.DWORD:
            return TypeCode.UInt32;
          case DataType.UInt8:
            return TypeCode.Byte;
          case DataType.TOD:
            return TypeCode.DateTime;
          case DataType.DT:
            return TypeCode.DateTime;
          default:
            return TypeCode.Empty;
        }
      }
    }

    public int TypeLength
    {
      get => this.propTypeLength;
      set => this.propTypeLength = value;
    }

    public int ArrayLength => this.propArrayLength;

    public ArrayDimensionArray ArrayDimensions => this.propDimensions;

    internal int CalculateArrayOffset(int dim, int offset)
    {
      int arrayOffset = offset;
      if (this.propDimensions != null && dim < this.propDimensions.Count)
        arrayOffset = offset + Math.Abs(this.propDimensions[dim].StartIndex);
      return arrayOffset;
    }

    public sbyte IsEnum => this.propIsEnum;

    public DerivationBase DerivedFrom => this.propDerivedFrom;

    public sbyte IsDerived => this.propIsDerived;

    public sbyte IsBitString => this.propIsBitString;

    public EnumArray Enumerations => this.propEnumerations;

    internal void InitializeArrayDimensions()
    {
      if (this.propDimensions != null)
        return;
      this.propDimensions = new ArrayDimensionArray();
    }

    internal void InitializeExtendedAttributes()
    {
      this.propDimensions = (ArrayDimensionArray) null;
      this.propArryOne = false;
      this.propIsDerived = (sbyte) 0;
      this.propDerivedFrom = (DerivationBase) null;
      this.propIsEnum = (sbyte) 0;
      if (this.propEnumerations != null)
        this.propEnumerations.Clear();
      this.propEnumerations = (EnumArray) null;
      this.propIsBitString = (sbyte) 0;
    }

    public Array ArrayData
    {
      get => this.propParent != null && this.propParent.PVRoot.propPviValue.propByteField != null ? this.ToSystemDataTypeArray(this.propParent.PVRoot.propPviValue.propByteField, this.propParent.propOffset) : this.ToSystemDataTypeArray(this.propByteField, 0);
      set => this.propParent.WriteValue(value, this.propParent.propOffset);
    }

    internal IntPtr DataPtr => this.pData;

    public Value this[string varName]
    {
      get => this.propParent.GetStructureMemberValue(varName);
      set
      {
        this.propParent.Value[varName].Assign(value.propObjValue);
        if (!this.propParent.WriteValueAutomatic)
          return;
        this.propParent.WriteValue();
      }
    }

    private void SetInByteBuffer(int index, Value value)
    {
      switch (this.propDataType)
      {
        case DataType.Boolean:
          this.propObjValue = (object) value.ToBoolean((IFormatProvider) null);
          break;
        case DataType.SByte:
          this.propObjValue = (object) value.ToSByte((IFormatProvider) null);
          break;
        case DataType.Int16:
          this.propObjValue = (object) value.ToBoolean((IFormatProvider) null);
          break;
        case DataType.Int32:
          this.propObjValue = (object) value.ToInt32((IFormatProvider) null);
          break;
        case DataType.Int64:
          this.propObjValue = (object) value.ToInt64((IFormatProvider) null);
          break;
        case DataType.Byte:
        case DataType.UInt8:
          this.propObjValue = (object) value.ToByte((IFormatProvider) null);
          break;
        case DataType.UInt16:
        case DataType.WORD:
          this.propObjValue = (object) value.ToUInt16((IFormatProvider) null);
          break;
        case DataType.UInt32:
        case DataType.DWORD:
          this.propObjValue = (object) value.ToUInt32((IFormatProvider) null);
          break;
        case DataType.UInt64:
          this.propObjValue = (object) value.ToUInt64((IFormatProvider) null);
          break;
        case DataType.Single:
          this.propObjValue = (object) value.ToSingle((IFormatProvider) null);
          break;
        case DataType.Double:
          this.propObjValue = (object) value.ToDouble((IFormatProvider) null);
          break;
        case DataType.TimeSpan:
        case DataType.TimeOfDay:
        case DataType.TOD:
          this.propObjValue = (object) value.ToTimeSpan((IFormatProvider) null);
          break;
        case DataType.DateTime:
        case DataType.Date:
        case DataType.DT:
          this.propObjValue = (object) value.ToDateTime((IFormatProvider) null);
          break;
        case DataType.String:
          this.propObjValue = (object) value.ToString((IFormatProvider) null);
          break;
      }
    }

    private Value GetFromByteBuffer(params int[] indices)
    {
      if (!this.IsOfTypeArray)
        return this;
      if (this.propByteField == null)
        this.propByteField = new byte[this.DataSize];
      if (IntPtr.Zero == this.pData)
      {
        this.pData = PviMarshal.AllocHGlobal(this.DataSize);
        this.propHasOwnDataPtr = true;
      }
      return new Value(this, indices);
    }

    public Value this[params int[] indices]
    {
      get => this.GetFromByteBuffer(indices);
      set => this.propParent.SetStructureMemberValue(value, indices);
    }

    public Value this[int index]
    {
      get => this.GetFromByteBuffer(index);
      set => this.propParent.SetStructureMemberValue(value, index);
    }

    public Variable Parent
    {
      get => this.propParent;
      set => this.propParent = value;
    }

    public string ToIECString() => this.ToIECString(10);

    public string ToIECString(int baseOfRepresentation) => this.IsPG2000String ? this.BinaryToAnsiString() : this.ConvertToIECString(baseOfRepresentation);

    private string ConvertToIECString(int baseOfRepresentation)
    {
      if (this.propObjValue != null)
      {
        switch (baseOfRepresentation)
        {
          case 2:
            return this.ToIECBinaryString();
          case 8:
            return this.ToIECOctalString();
          case 16:
            return this.ToIECHexadecimalString();
          default:
            return this.ToIECDecimalString();
        }
      }
      else
      {
        switch (baseOfRepresentation)
        {
          case 2:
            return this.ToIECBinaryStringEx();
          case 8:
            return this.ToIECOctalStringEx();
          case 16:
            return this.ToIECHexadecimalStringEx();
          default:
            return this.ToIECDecimalStringEx();
        }
      }
    }

    private string ToIECBinaryString()
    {
      StringBuilder stringBuilder = new StringBuilder();
      switch (this.propDataType)
      {
        case DataType.Boolean:
          if (this.propObjValue.ToString().ToLower().CompareTo("0") == 0 || this.propObjValue.ToString().ToLower().CompareTo("false") == 0)
          {
            stringBuilder.Append("+2#0");
            break;
          }
          stringBuilder.Append("+2#1");
          break;
        case DataType.SByte:
          stringBuilder.Append((sbyte) 0 > (sbyte) this.propObjValue ? "-2#" : "+2#");
          stringBuilder.Append(System.Convert.ToString((short) (sbyte) this.propObjValue, 2).PadLeft(8, '0'));
          break;
        case DataType.Int16:
          stringBuilder.Append((short) 0 > (short) this.propObjValue ? "-2#" : "+2#");
          stringBuilder.Append(System.Convert.ToString((short) this.propObjValue, 2).PadLeft(16, '0'));
          break;
        case DataType.Int32:
          stringBuilder.Append(0 > (int) this.propObjValue ? "-2#" : "+2#");
          stringBuilder.Append(System.Convert.ToString((int) this.propObjValue, 2).PadLeft(32, '0'));
          break;
        case DataType.Int64:
          stringBuilder.Append(0L > (long) this.propObjValue ? "-2#" : "+2#");
          stringBuilder.Append(System.Convert.ToString((long) this.propObjValue, 2).PadLeft(64, '0'));
          break;
        case DataType.Byte:
        case DataType.UInt8:
          stringBuilder.Append("+2#");
          stringBuilder.Append(System.Convert.ToString((byte) this.propObjValue, 2).PadLeft(8, '0'));
          break;
        case DataType.UInt16:
        case DataType.WORD:
          stringBuilder.Append("+2#");
          stringBuilder.Append(System.Convert.ToString((int) (ushort) this.propObjValue, 2).PadLeft(16, '0'));
          break;
        case DataType.UInt32:
        case DataType.DWORD:
          stringBuilder.Append("+2#");
          stringBuilder.Append(System.Convert.ToString((long) (uint) this.propObjValue, 2).PadLeft(32, '0'));
          break;
        case DataType.UInt64:
          stringBuilder.Append("+2#");
          string empty = string.Empty;
          byte[] bytes = PviMarshal.UInt64ToBytes((ulong) this.propObjValue);
          string str = (empty + System.Convert.ToString(bytes[0], 2).PadLeft(8, '0') + System.Convert.ToString(bytes[1], 2).PadLeft(8, '0') + System.Convert.ToString(bytes[2], 2).PadLeft(8, '0') + System.Convert.ToString(bytes[3], 2).PadLeft(8, '0') + System.Convert.ToString(bytes[4], 2).PadLeft(8, '0') + System.Convert.ToString(bytes[5], 2).PadLeft(8, '0') + System.Convert.ToString(bytes[6], 2).PadLeft(8, '0') + System.Convert.ToString(bytes[7], 2).PadLeft(8, '0')).PadLeft(64, '0');
          stringBuilder.Append(str);
          break;
        case DataType.Single:
          throw new NotSupportedException();
        case DataType.Double:
          throw new NotSupportedException();
        case DataType.TimeSpan:
        case DataType.TimeOfDay:
        case DataType.TOD:
          stringBuilder.Append("+2#");
          stringBuilder.Append(System.Convert.ToString(((TimeSpan) this.propObjValue).Ticks, 2).PadLeft(32, '0'));
          break;
        case DataType.DateTime:
        case DataType.Date:
        case DataType.DT:
          stringBuilder.Append("+2#");
          stringBuilder.Append(System.Convert.ToString(((DateTime) this.propObjValue).Ticks, 2).PadLeft(32, '0'));
          break;
        case DataType.String:
          throw new NotSupportedException();
        default:
          stringBuilder.Append("???");
          break;
      }
      return stringBuilder.ToString();
    }

    private string ToIECOctalString()
    {
      StringBuilder stringBuilder = new StringBuilder();
      switch (this.propDataType)
      {
        case DataType.Boolean:
          if (this.propObjValue.ToString().ToLower().CompareTo("0") == 0 || this.propObjValue.ToString().ToLower().CompareTo("false") == 0)
          {
            stringBuilder.Append("+8#0");
            break;
          }
          stringBuilder.Append("+8#1");
          break;
        case DataType.SByte:
          stringBuilder.Append((sbyte) 0 > (sbyte) this.propObjValue ? "-8#" : "+8#");
          stringBuilder.Append(System.Convert.ToString((short) (sbyte) this.propObjValue, 8).PadLeft(3, '0'));
          break;
        case DataType.Int16:
          stringBuilder.Append((short) 0 > (short) this.propObjValue ? "-8#" : "+8#");
          stringBuilder.Append(System.Convert.ToString((short) this.propObjValue, 8).PadLeft(6, '0'));
          break;
        case DataType.Int32:
          stringBuilder.Append(0 > (int) this.propObjValue ? "-8#" : "+8#");
          stringBuilder.Append(System.Convert.ToString((int) this.propObjValue, 8).PadLeft(12, '0'));
          break;
        case DataType.Int64:
          stringBuilder.Append(0L > (long) this.propObjValue ? "-8#" : "+8#");
          stringBuilder.Append(System.Convert.ToString((long) this.propObjValue, 8).PadLeft(24, '0'));
          break;
        case DataType.Byte:
        case DataType.UInt8:
          stringBuilder.Append("+8#");
          stringBuilder.Append(System.Convert.ToString((byte) this.propObjValue, 8).PadLeft(3, '0'));
          break;
        case DataType.UInt16:
        case DataType.WORD:
          stringBuilder.Append("+8#");
          stringBuilder.Append(System.Convert.ToString((int) (ushort) this.propObjValue, 8).PadLeft(6, '0'));
          break;
        case DataType.UInt32:
        case DataType.DWORD:
          stringBuilder.Append("+8#");
          stringBuilder.Append(System.Convert.ToString((long) (uint) this.propObjValue, 8).PadLeft(12, '0'));
          break;
        case DataType.UInt64:
          stringBuilder.Append("+8#");
          string empty = string.Empty;
          byte[] bytes = PviMarshal.UInt64ToBytes((ulong) this.propObjValue);
          string str = (empty + System.Convert.ToString(bytes[0], 8).PadLeft(3, '0') + System.Convert.ToString(bytes[1], 8).PadLeft(3, '0') + System.Convert.ToString(bytes[2], 8).PadLeft(3, '0') + System.Convert.ToString(bytes[3], 8).PadLeft(3, '0') + System.Convert.ToString(bytes[4], 8).PadLeft(3, '0') + System.Convert.ToString(bytes[5], 8).PadLeft(3, '0') + System.Convert.ToString(bytes[6], 8).PadLeft(3, '0') + System.Convert.ToString(bytes[7], 8).PadLeft(3, '0')).PadLeft(24, '0');
          stringBuilder.Append(str);
          break;
        case DataType.Single:
          throw new NotSupportedException();
        case DataType.Double:
          throw new NotSupportedException();
        case DataType.TimeSpan:
        case DataType.TimeOfDay:
        case DataType.TOD:
          stringBuilder.Append("+8#");
          stringBuilder.Append(System.Convert.ToString(((TimeSpan) this.propObjValue).Ticks, 8).PadLeft(12, '0'));
          break;
        case DataType.DateTime:
        case DataType.Date:
        case DataType.DT:
          stringBuilder.Append("+8#");
          stringBuilder.Append(System.Convert.ToString(((DateTime) this.propObjValue).Ticks, 8).PadLeft(12, '0'));
          break;
        case DataType.String:
          throw new NotSupportedException();
        default:
          stringBuilder.Append("???");
          break;
      }
      return stringBuilder.ToString();
    }

    private string ToIECHexadecimalString()
    {
      StringBuilder stringBuilder = new StringBuilder();
      switch (this.propDataType)
      {
        case DataType.Boolean:
          if (this.propObjValue.ToString().ToLower().CompareTo("0") == 0 || this.propObjValue.ToString().ToLower().CompareTo("false") == 0)
          {
            stringBuilder.Append("+16#0");
            break;
          }
          stringBuilder.Append("+16#1");
          break;
        case DataType.SByte:
          stringBuilder.Append((sbyte) 0 > (sbyte) this.propObjValue ? "-16#" : "+16#");
          stringBuilder.Append(System.Convert.ToString((short) (sbyte) this.propObjValue, 16).ToUpper().PadLeft(2, '0'));
          break;
        case DataType.Int16:
          stringBuilder.Append((short) 0 > (short) this.propObjValue ? "-16#" : "+16#");
          stringBuilder.Append(System.Convert.ToString((short) this.propObjValue, 16).PadLeft(4, '0'));
          break;
        case DataType.Int32:
          stringBuilder.Append(0 > (int) this.propObjValue ? "-16#" : "+16#");
          stringBuilder.Append(System.Convert.ToString((int) this.propObjValue, 16).PadLeft(8, '0'));
          break;
        case DataType.Int64:
          stringBuilder.Append(0L > (long) this.propObjValue ? "-16#" : "+16#");
          stringBuilder.Append(System.Convert.ToString((long) this.propObjValue, 16).ToUpper().PadLeft(16, '0'));
          break;
        case DataType.Byte:
        case DataType.UInt8:
          stringBuilder.Append("+16#");
          stringBuilder.Append(System.Convert.ToString((byte) this.propObjValue, 16).ToUpper().PadLeft(2, '0'));
          break;
        case DataType.UInt16:
        case DataType.WORD:
          stringBuilder.Append("+16#");
          stringBuilder.Append(System.Convert.ToString((int) (ushort) this.propObjValue, 16).ToUpper().PadLeft(4, '0'));
          break;
        case DataType.UInt32:
        case DataType.DWORD:
          stringBuilder.Append("+16#");
          stringBuilder.Append(System.Convert.ToString((long) (uint) this.propObjValue, 16).ToUpper().PadLeft(8, '0'));
          break;
        case DataType.UInt64:
          stringBuilder.Append("+16#");
          string empty = string.Empty;
          byte[] bytes = PviMarshal.UInt64ToBytes((ulong) this.propObjValue);
          string str = (empty + System.Convert.ToString(bytes[0], 16).ToUpper().PadLeft(2, '0') + System.Convert.ToString(bytes[1], 16).ToUpper().PadLeft(2, '0') + System.Convert.ToString(bytes[2], 16).ToUpper().PadLeft(2, '0') + System.Convert.ToString(bytes[3], 16).ToUpper().PadLeft(2, '0') + System.Convert.ToString(bytes[4], 16).ToUpper().PadLeft(2, '0') + System.Convert.ToString(bytes[5], 16).ToUpper().PadLeft(2, '0') + System.Convert.ToString(bytes[6], 16).ToUpper().PadLeft(2, '0') + System.Convert.ToString(bytes[7], 16).ToUpper().PadLeft(2, '0')).PadLeft(16, '0');
          stringBuilder.Append(str);
          break;
        case DataType.Single:
          throw new NotSupportedException();
        case DataType.Double:
          throw new NotSupportedException();
        case DataType.TimeSpan:
        case DataType.TimeOfDay:
        case DataType.TOD:
          stringBuilder.Append("+16#");
          stringBuilder.Append(System.Convert.ToString(((TimeSpan) this.propObjValue).Ticks, 16).ToUpper().PadLeft(8, '0'));
          break;
        case DataType.DateTime:
        case DataType.Date:
        case DataType.DT:
          stringBuilder.Append("+16#");
          stringBuilder.Append(System.Convert.ToString(((DateTime) this.propObjValue).Ticks, 16).ToUpper().PadLeft(8, '0'));
          break;
        case DataType.String:
          throw new NotSupportedException();
        default:
          stringBuilder.Append("???");
          break;
      }
      return stringBuilder.ToString();
    }

    private string ToIECDecimalString()
    {
      if (this.propDataType == DataType.TimeSpan)
      {
        int num = 0;
        string iecDecimalString = "T#";
        if (0 < ((TimeSpan) this.propObjValue).Days)
        {
          iecDecimalString = iecDecimalString + ((TimeSpan) this.propObjValue).Days.ToString("00") + "d";
          ++num;
        }
        if (0 < ((TimeSpan) this.propObjValue).Hours || 0 < ((TimeSpan) this.propObjValue).Minutes || 0 < ((TimeSpan) this.propObjValue).Seconds || 0 < ((TimeSpan) this.propObjValue).Milliseconds)
        {
          if (0 < num)
            iecDecimalString += "_";
          iecDecimalString = iecDecimalString + ((TimeSpan) this.propObjValue).Hours.ToString("00") + "h";
          ++num;
        }
        if (0 < ((TimeSpan) this.propObjValue).Minutes || 0 < ((TimeSpan) this.propObjValue).Seconds || 0 < ((TimeSpan) this.propObjValue).Milliseconds)
        {
          if (0 < num)
            iecDecimalString += "_";
          iecDecimalString = iecDecimalString + ((TimeSpan) this.propObjValue).Minutes.ToString("00") + "m";
          ++num;
        }
        if (0 < ((TimeSpan) this.propObjValue).Seconds || 0 < ((TimeSpan) this.propObjValue).Milliseconds)
        {
          if (0 < num)
            iecDecimalString += "_";
          iecDecimalString = iecDecimalString + ((TimeSpan) this.propObjValue).Seconds.ToString("00") + "s";
        }
        if (0 < ((TimeSpan) this.propObjValue).Milliseconds)
        {
          if (0 < num)
            iecDecimalString += "_";
          iecDecimalString = iecDecimalString + ((TimeSpan) this.propObjValue).Milliseconds.ToString("000") + "ms";
        }
        else if (0.0 == ((TimeSpan) this.propObjValue).TotalMilliseconds)
          iecDecimalString += "0ms";
        return iecDecimalString;
      }
      if (this.propDataType == DataType.TimeOfDay || this.propDataType == DataType.TOD)
      {
        string iecDecimalString = "TOD#" + ((TimeSpan) this.propObjValue).Hours.ToString("00") + ":" + ((TimeSpan) this.propObjValue).Minutes.ToString("00") + ":" + ((TimeSpan) this.propObjValue).Seconds.ToString("00");
        if (0 < ((TimeSpan) this.propObjValue).Milliseconds)
          iecDecimalString = iecDecimalString + "." + ((TimeSpan) this.propObjValue).Milliseconds.ToString("000");
        return iecDecimalString;
      }
      if (this.propDataType == DataType.DateTime || this.propDataType == DataType.DT)
      {
        string iecDecimalString = "DT#" + ((DateTime) this.propObjValue).Year.ToString("0000") + "-" + ((DateTime) this.propObjValue).Month.ToString("00") + "-" + ((DateTime) this.propObjValue).Day.ToString("00") + "-" + ((DateTime) this.propObjValue).Hour.ToString("00") + ":" + ((DateTime) this.propObjValue).Minute.ToString("00") + ":" + ((DateTime) this.propObjValue).Second.ToString("00");
        if (0 < ((DateTime) this.propObjValue).Millisecond)
          iecDecimalString = iecDecimalString + "." + ((DateTime) this.propObjValue).Millisecond.ToString("000");
        return iecDecimalString;
      }
      if (this.propDataType != DataType.Date)
        return this.ToString((IFormatProvider) null);
      return "D#" + ((DateTime) this.propObjValue).Year.ToString("0000") + "-" + ((DateTime) this.propObjValue).Month.ToString("00") + "-" + ((DateTime) this.propObjValue).Day.ToString("00");
    }

    private string ToIECBinaryStringEx() => this.ToString((IFormatProvider) null);

    private string ToIECOctalStringEx() => this.ToString((IFormatProvider) null);

    private string ToIECHexadecimalStringEx() => this.ToString((IFormatProvider) null);

    private string ToIECDecimalStringEx() => this.ToString((IFormatProvider) null);

    internal void SetDerivation(DerivationBase derivation)
    {
      DerivationBase derivationBase = this.propDerivedFrom;
      if (this.propDerivedFrom == null)
      {
        this.propDerivedFrom = derivation;
      }
      else
      {
        for (DerivationBase derivedFrom = this.propDerivedFrom.DerivedFrom; derivedFrom != null; derivedFrom = derivationBase.DerivedFrom)
          derivationBase = derivedFrom;
        derivationBase.SetDerivation(derivation);
      }
    }

    internal void Clone(Value cloneValue)
    {
      this.propDisposed = cloneValue.propDisposed;
      this.propArryOne = cloneValue.propArryOne;
      this.propDimensions = cloneValue.propDimensions;
      this.propIsEnum = cloneValue.propIsEnum;
      this.propIsDerived = cloneValue.propIsDerived;
      this.propDerivedFrom = cloneValue.propDerivedFrom;
      this.propIsBitString = cloneValue.propIsBitString;
      this.propEnumerations = cloneValue.propEnumerations;
      this.propArrayLength = cloneValue.propArrayLength;
      this.propDataSize = cloneValue.propDataSize;
      this.propDataType = cloneValue.propDataType;
      this.isAssigned = cloneValue.isAssigned;
      this.propTypeLength = cloneValue.propTypeLength;
      this.pData = cloneValue.pData;
      this.propArrayMaxIndex = cloneValue.propArrayMaxIndex;
      this.propArrayMinIndex = cloneValue.propArrayMinIndex;
      this.propByteField = (byte[]) null;
      if (cloneValue.propByteField != null)
        this.propByteField = (byte[]) cloneValue.propByteField.Clone();
      this.propByteOffset = cloneValue.propByteOffset;
      this.propDimensions = (ArrayDimensionArray) null;
      if (cloneValue.propDimensions != null)
        this.propDimensions = cloneValue.propDimensions.Clone();
      this.propEnumerations = (EnumArray) null;
      if (cloneValue.propEnumerations != null)
        this.propEnumerations = cloneValue.propEnumerations.Clone();
      this.propHasOwnDataPtr = cloneValue.propHasOwnDataPtr;
      this.propTypePreset = cloneValue.propTypePreset;
      this.propUInt32Val = cloneValue.propUInt32Val;
    }

    internal unsafe void UpdateByteField(IntPtr pData, uint dataLen, int ownerOffset)
    {
      if (this.propByteField == null)
        return;
      byte* numPtr = (byte*) ((IntPtr) pData.ToPointer() + ownerOffset);
      if (this.ArrayLength <= 0)
        return;
      for (int index = 0; index < this.propByteField.Length && (long) index < (long) dataLen - (long) ownerOffset; ++index)
        this.propByteField[index] = numPtr[index];
    }
  }
}
