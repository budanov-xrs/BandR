﻿// Decompiled with JetBrains decompiler
// Type: BR.AN.PviServices.PlcFamily
// Assembly: BR.AN.PVIServices, Version=1.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DA853F0A-C070-4E30-B7A7-51D955FB85F6
// Assembly location: C:\Users\ss_six\Desktop\FXE\FXE software integration package\COMET_FXE-PVIServices_Example\Pluto-PVI-trial_01\packages\IToolS.BandR.3.5.58\lib\net20\BR.AN.PVIServices.dll

using System.Runtime.InteropServices;

namespace BR.AN.PviServices
{
  [ComVisible(true)]
  public enum PlcFamily
  {
    None = -1, // 0xFFFFFFFF
    System2010 = 0,
    System2005 = 1,
    System2003 = 2,
    LogicScanner = 3,
    Simulation = 4,
    PowerPanel = 5,
    PanelC300 = 8,
    PanelC200 = 9,
    X20 = 10, // 0x0000000A
  }
}
