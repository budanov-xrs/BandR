// Decompiled with JetBrains decompiler
// Type: BR.AN.PviServices.Access
// Assembly: BR.AN.PVIServices, Version=1.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DA853F0A-C070-4E30-B7A7-51D955FB85F6
// Assembly location: C:\Users\ss_six\Desktop\FXE\FXE software integration package\COMET_FXE-PVIServices_Example\Pluto-PVI-trial_01\packages\IToolS.BandR.3.5.58\lib\net20\BR.AN.PVIServices.dll

using System;

namespace BR.AN.PviServices
{
  [Flags]
  public enum Access
  {
    No = 0,
    Read = 1,
    Write = 2,
    ReadAndWrite = Write | Read, // 0x00000003
    EVENT = 4,
    DIRECT = 8,
    FASTECHO = 16, // 0x00000010
  }
}
