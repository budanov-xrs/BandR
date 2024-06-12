// Decompiled with JetBrains decompiler
// Type: BR.AN.PviServices.CpuTTServiceEventArgs
// Assembly: BR.AN.PVIServices, Version=1.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DA853F0A-C070-4E30-B7A7-51D955FB85F6
// Assembly location: C:\Users\ss_six\Desktop\FXE\FXE software integration package\COMET_FXE-PVIServices_Example\Pluto-PVI-trial_01\packages\IToolS.BandR.3.5.58\lib\net20\BR.AN.PVIServices.dll

using System;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace BR.AN.PviServices
{
  [Browsable(false)]
  [EditorBrowsable(EditorBrowsableState.Never)]
  public class CpuTTServiceEventArgs : PviEventArgs
  {
    private ushort propTTGroup;
    private byte propTTServiceID;
    private byte[] propTTData;

    [Browsable(false)]
    [CLSCompliant(false)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public CpuTTServiceEventArgs(
      string name,
      string address,
      int error,
      string language,
      Action action,
      ushort ttGroup,
      byte ttServiceID,
      IntPtr pData,
      byte ttDataLen)
      : base(name, address, error, language, action)
    {
      this.propTTGroup = ttGroup;
      this.propTTServiceID = ttServiceID;
      this.propTTData = new byte[(int) ttDataLen];
      if ((byte) 0 >= ttDataLen)
        return;
      Marshal.Copy(new IntPtr(pData.ToInt64() + 4L), this.propTTData, 0, (int) ttDataLen);
    }

    [EditorBrowsable(EditorBrowsableState.Never)]
    [Browsable(false)]
    [CLSCompliant(false)]
    public ushort TTGroup => this.propTTGroup;

    [Browsable(false)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public byte TTServiceID => this.propTTServiceID;

    [Browsable(false)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public byte[] TTData => this.propTTData;
  }
}
