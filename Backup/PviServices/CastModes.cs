// Decompiled with JetBrains decompiler
// Type: BR.AN.PviServices.CastModes
// Assembly: BR.AN.PVIServices, Version=1.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DA853F0A-C070-4E30-B7A7-51D955FB85F6
// Assembly location: C:\Users\ss_six\Desktop\FXE\FXE software integration package\COMET_FXE-PVIServices_Example\Pluto-PVI-trial_01\packages\IToolS.BandR.3.5.58\lib\net20\BR.AN.PVIServices.dll

using System;

namespace BR.AN.PviServices
{
  [Flags]
  public enum CastModes
  {
    DEFAULT = 0,
    PG2000String = 1,
    DecimalConversion = 2,
    RangeCheck = 4,
    FloatConversion = 8,
    StringTermination = 16, // 0x00000010
  }
}
