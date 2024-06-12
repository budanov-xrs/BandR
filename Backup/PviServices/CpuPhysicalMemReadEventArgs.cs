// Decompiled with JetBrains decompiler
// Type: BR.AN.PviServices.CpuPhysicalMemReadEventArgs
// Assembly: BR.AN.PVIServices, Version=1.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DA853F0A-C070-4E30-B7A7-51D955FB85F6
// Assembly location: C:\Users\ss_six\Desktop\FXE\FXE software integration package\COMET_FXE-PVIServices_Example\Pluto-PVI-trial_01\packages\IToolS.BandR.3.5.58\lib\net20\BR.AN.PVIServices.dll

using System;
using System.ComponentModel;

namespace BR.AN.PviServices
{
  [Browsable(false)]
  [EditorBrowsable(EditorBrowsableState.Never)]
  public class CpuPhysicalMemReadEventArgs : PviEventArgs
  {
    private IntPtr propDataPtr;
    private int propDataLen;

    [Browsable(false)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public CpuPhysicalMemReadEventArgs(
      string name,
      string address,
      int error,
      string language,
      Action action,
      IntPtr pData,
      int dataLen)
      : base(name, address, error, language, action)
    {
      this.propDataPtr = pData;
      this.propDataLen = dataLen;
    }

    [Browsable(false)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public int DataLength => this.propDataLen;

    [EditorBrowsable(EditorBrowsableState.Never)]
    [Browsable(false)]
    public IntPtr DataPtr => this.propDataPtr;
  }
}
