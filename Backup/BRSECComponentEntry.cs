// Decompiled with JetBrains decompiler
// Type: BR.AN.BRSECComponentEntry
// Assembly: BR.AN.PVIServices, Version=1.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DA853F0A-C070-4E30-B7A7-51D955FB85F6
// Assembly location: C:\Users\ss_six\Desktop\FXE\FXE software integration package\COMET_FXE-PVIServices_Example\Pluto-PVI-trial_01\packages\IToolS.BandR.3.5.58\lib\net20\BR.AN.PVIServices.dll

using System.Runtime.InteropServices;

namespace BR.AN
{
  [StructLayout(LayoutKind.Sequential, Size = 79)]
  public struct BRSECComponentEntry
  {
    public char RequiresBRIPC;
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 13)]
    public string OrderId;
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 65)]
    public string LicenseText;
  }
}
