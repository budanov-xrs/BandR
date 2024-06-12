// Decompiled with JetBrains decompiler
// Type: BR.AN.PviServices.ConfigurationFlags
// Assembly: BR.AN.PVIServices, Version=1.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DA853F0A-C070-4E30-B7A7-51D955FB85F6
// Assembly location: C:\Users\ss_six\Desktop\FXE\FXE software integration package\COMET_FXE-PVIServices_Example\Pluto-PVI-trial_01\packages\IToolS.BandR.3.5.58\lib\net20\BR.AN.PVIServices.dll

using System;

namespace BR.AN.PviServices
{
  [Flags]
  public enum ConfigurationFlags
  {
    None = 0,
    Values = 1,
    ConnectionState = 4,
    ActiveState = 8,
    RefreshTime = 16, // 0x00000010
    VariableMembers = 32, // 0x00000020
    IOAttributes = 512, // 0x00000200
    Scope = 1024, // 0x00000400
    All = Scope | IOAttributes | VariableMembers | RefreshTime | ActiveState | ConnectionState | Values, // 0x0000063D
  }
}
