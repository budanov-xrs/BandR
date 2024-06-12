// Decompiled with JetBrains decompiler
// Type: BR.AN.PviServices.RedundancySwitchPossibility
// Assembly: BR.AN.PVIServices, Version=1.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DA853F0A-C070-4E30-B7A7-51D955FB85F6
// Assembly location: C:\Users\ss_six\Desktop\FXE\FXE software integration package\COMET_FXE-PVIServices_Example\Pluto-PVI-trial_01\packages\IToolS.BandR.3.5.58\lib\net20\BR.AN.PVIServices.dll

namespace BR.AN.PviServices
{
  public enum RedundancySwitchPossibility
  {
    Impossible = 0,
    MajorBump = 16, // 0x00000010
    MinorBump = 32, // 0x00000020
    Bumpless = 255, // 0x000000FF
  }
}
