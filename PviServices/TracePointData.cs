// Decompiled with JetBrains decompiler
// Type: BR.AN.PviServices.TracePointData
// Assembly: BR.AN.PVIServices, Version=1.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DA853F0A-C070-4E30-B7A7-51D955FB85F6
// Assembly location: C:\Users\ss_six\Desktop\FXE\FXE software integration package\COMET_FXE-PVIServices_Example\Pluto-PVI-trial_01\packages\IToolS.BandR.3.5.58\lib\net20\BR.AN.PVIServices.dll

using System;

namespace BR.AN.PviServices
{
  public class TracePointData : IDisposable
  {
    private bool disposed;
    private IECDataTypes propPVFormat;
    private uint propPVLength;
    private byte[] propDataBytes;

    private void InitMembers()
    {
      this.propPVFormat = IECDataTypes.UNDEFINED;
      this.propPVLength = 0U;
      this.propDataBytes = (byte[]) null;
    }

    public TracePointData() => this.InitMembers();

    [CLSCompliant(false)]
    public TracePointData(
      uint iecFormat,
      uint typeLenght,
      IntPtr pData,
      uint dataLen,
      ref int dataOffset)
    {
      this.InitMembers();
      this.propPVFormat = (IECDataTypes) iecFormat;
      this.propPVLength = typeLenght;
      this.propDataBytes = new byte[(IntPtr) typeLenght];
      for (int index = 0; index < (int) typeLenght; ++index)
        this.propDataBytes[index] = PviMarshal.ReadByte(pData, ref dataOffset);
    }

    public IECDataTypes IECType => this.propPVFormat;

    [CLSCompliant(false)]
    public uint TypeLength => this.propPVLength;

    public byte[] DataBytes => this.propDataBytes;

    internal void UpdateFormat(uint formatType, uint typeLength)
    {
      this.propPVFormat = (IECDataTypes) formatType;
      this.propPVLength = typeLength;
    }

    internal void UpdateData(IECDataTypes formatType, uint typeLength, byte[] dataBytes)
    {
      this.propPVFormat = formatType;
      this.propPVLength = typeLength;
      this.propDataBytes = new byte[(IntPtr) typeLength];
      for (int index = 0; index < (int) typeLength; ++index)
        this.propDataBytes[index] = (byte) dataBytes.GetValue(index);
    }

    public object DataTo(TypeCode conversionType)
    {
      switch (conversionType)
      {
        case TypeCode.Char:
          return (object) char.MinValue;
        case TypeCode.SByte:
          return (object) (sbyte) this.propDataBytes[0];
        case TypeCode.Byte:
          return (object) this.propDataBytes[0];
        case TypeCode.Int16:
          return (object) (short) 0;
        case TypeCode.UInt16:
          return (object) (ushort) 0;
        case TypeCode.Int32:
          return (object) 0;
        case TypeCode.UInt32:
          return (object) 0U;
        case TypeCode.Int64:
          return (object) 0L;
        case TypeCode.UInt64:
          return (object) 0UL;
        case TypeCode.Single:
          return (object) 0.0f;
        case TypeCode.Double:
          return (object) 0.0;
        case TypeCode.DateTime:
          return (object) new DateTime();
        default:
          throw new ArgumentOutOfRangeException("\"" + conversionType.ToString() + "\" is NOT supported!");
      }
    }

    public void Dispose()
    {
      this.Dispose(true);
      GC.SuppressFinalize((object) this);
    }

    protected virtual void Dispose(bool disposing)
    {
      if (this.disposed)
        return;
      if (disposing)
        this.propDataBytes = (byte[]) null;
      this.disposed = true;
    }

    ~TracePointData() => this.Dispose(false);

    public override string ToString()
    {
      string str = "0x";
      if (this.propDataBytes != null)
      {
        for (int index = 0; index < this.propDataBytes.GetLength(0); ++index)
          str += string.Format("{0:X2}", (object) System.Convert.ToInt32(this.propDataBytes.GetValue(index).ToString()));
      }
      return "Format=\"" + this.propPVFormat.ToString() + "\" Length=\"" + (object) this.propPVLength + "\" Data=\"" + str + "\"";
    }
  }
}
