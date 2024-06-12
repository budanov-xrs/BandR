// Decompiled with JetBrains decompiler
// Type: BR.AN.PviServices.VariableAttribute
// Assembly: BR.AN.PVIServices, Version=1.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DA853F0A-C070-4E30-B7A7-51D955FB85F6
// Assembly location: C:\Users\ss_six\Desktop\FXE\FXE software integration package\COMET_FXE-PVIServices_Example\Pluto-PVI-trial_01\packages\IToolS.BandR.3.5.58\lib\net20\BR.AN.PVIServices.dll

using System;

namespace BR.AN.PviServices
{
  [Flags]
  internal enum VariableAttribute
  {
    None = 0,
    Input = 1,
    Output = 2,
    Constant = 4,
    Variable = 8,
  }
}
