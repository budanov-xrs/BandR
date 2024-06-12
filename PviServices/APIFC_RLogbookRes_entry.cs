// Decompiled with JetBrains decompiler
// Type: BR.AN.PviServices.APIFC_RLogbookRes_entry
// Assembly: BR.AN.PVIServices, Version=1.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DA853F0A-C070-4E30-B7A7-51D955FB85F6
// Assembly location: C:\Users\ss_six\Desktop\FXE\FXE software integration package\COMET_FXE-PVIServices_Example\Pluto-PVI-trial_01\packages\IToolS.BandR.3.5.58\lib\net20\BR.AN.PVIServices.dll

using System.Runtime.InteropServices;

namespace BR.AN.PviServices
{
  [StructLayout(LayoutKind.Sequential, Pack = 1)]
  internal struct APIFC_RLogbookRes_entry
  {
    public ushort errcode;
    public uint errinfo;
    public char errtask_0;
    public char errtask_1;
    public char errtask_2;
    public char errtask_3;
    public APIFCerrtyp errtyp;
    public byte timestamp_0;
    public byte timestamp_1;
    public byte timestamp_2;
    public byte timestamp_3;
    public byte timestamp_4;
    public byte res;
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
    public string errstring;
  }
}
