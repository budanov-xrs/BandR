// Decompiled with JetBrains decompiler
// Type: BR.AN.PviServices.ProgressInfo
// Assembly: BR.AN.PVIServices, Version=1.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DA853F0A-C070-4E30-B7A7-51D955FB85F6
// Assembly location: C:\Users\ss_six\Desktop\FXE\FXE software integration package\COMET_FXE-PVIServices_Example\Pluto-PVI-trial_01\packages\IToolS.BandR.3.5.58\lib\net20\BR.AN.PVIServices.dll

using System;
using System.Runtime.InteropServices;

namespace BR.AN.PviServices
{
  [CLSCompliant(true)]
  [StructLayout(LayoutKind.Sequential, Pack = 1)]
  public struct ProgressInfo
  {
    public int Access;
    public int Percent;
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
    public string Info;
  }
}
