// Decompiled with JetBrains decompiler
// Type: BR.AN.PviServices.TraceData
// Assembly: BR.AN.PVIServices, Version=1.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DA853F0A-C070-4E30-B7A7-51D955FB85F6
// Assembly location: C:\Users\ss_six\Desktop\FXE\FXE software integration package\COMET_FXE-PVIServices_Example\Pluto-PVI-trial_01\packages\IToolS.BandR.3.5.58\lib\net20\BR.AN.PVIServices.dll

using System;

namespace BR.AN.PviServices
{
  public class TraceData : IDisposable
  {
    private IECDataTypes propDataType;
    private byte[] propData;

    internal TraceData(byte[] dataBytes, IECDataTypes dataType)
    {
      this.propDataType = dataType;
      this.propData = dataBytes;
    }

    public IECDataTypes DataType => this.propDataType;

    public byte[] Data => this.propData;

    public override string ToString()
    {
      string str = "";
      if (this.propData != null)
      {
        for (int index = 0; index < this.propData.GetLength(0); ++index)
          str += string.Format("{0:X2}", this.propData.GetValue(index));
      }
      return "\" Type=\"" + this.propDataType.ToString() + "\" DataLength=\"" + (this.propData != null ? this.propData.GetLength(0).ToString() : "0") + "\"\" Data=\"" + str + "\"";
    }

    public void Dispose()
    {
      this.propDataType = IECDataTypes.UNDEFINED;
      if (this.propData == null)
        return;
      this.propData = (byte[]) null;
    }
  }
}
