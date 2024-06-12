// Decompiled with JetBrains decompiler
// Type: BR.AN.PviServices.MemoryType
// Assembly: BR.AN.PVIServices, Version=1.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DA853F0A-C070-4E30-B7A7-51D955FB85F6
// Assembly location: C:\Users\ss_six\Desktop\FXE\FXE software integration package\COMET_FXE-PVIServices_Example\Pluto-PVI-trial_01\packages\IToolS.BandR.3.5.58\lib\net20\BR.AN.PVIServices.dll

namespace BR.AN.PviServices
{
  public enum MemoryType
  {
    SystemRom = 0,
    SystemRam = 1,
    UserRom = 2,
    UserRam = 3,
    MemCard = 4,
    FixRam = 5,
    Dram = 65, // 0x00000041
    Permanent = 240, // 0x000000F0
    SysInternal = 241, // 0x000000F1
    Remanent = 242, // 0x000000F2
    SystemSettings = 243, // 0x000000F3
    TransferModule = 244, // 0x000000F4
    Os = 61441, // 0x0000F001
    Tmp = 61442, // 0x0000F002
    Io = 61443, // 0x0000F003
    GlobalAnalog = 61444, // 0x0000F004
    GlobalDigital = 61445, // 0x0000F005
    NOTValid = 65535, // 0x0000FFFF
  }
}
