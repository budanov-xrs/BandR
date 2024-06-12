// Decompiled with JetBrains decompiler
// Type: BR.AN.PviServices.ModuleType
// Assembly: BR.AN.PVIServices, Version=1.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DA853F0A-C070-4E30-B7A7-51D955FB85F6
// Assembly location: C:\Users\ss_six\Desktop\FXE\FXE software integration package\COMET_FXE-PVIServices_Example\Pluto-PVI-trial_01\packages\IToolS.BandR.3.5.58\lib\net20\BR.AN.PVIServices.dll

namespace BR.AN.PviServices
{
  public enum ModuleType
  {
    Update = 0,
    Contents = 1,
    Contents2 = 2,
    MemoryExtension = 3,
    BootModule = 7,
    PlcTask = 17, // 0x00000011
    SystemTask = 18, // 0x00000012
    UserTask = 19, // 0x00000013
    TimerTask = 20, // 0x00000014
    InterruptTask = 21, // 0x00000015
    ExceptionTask = 22, // 0x00000016
    Startup = 31, // 0x0000001F
    AvtLib = 33, // 0x00000021
    SysLib = 34, // 0x00000022
    HwLib = 35, // 0x00000023
    ComLib = 36, // 0x00000024
    MathLib = 37, // 0x00000025
    Lib = 38, // 0x00000026
    InstLib = 39, // 0x00000027
    AddLib = 40, // 0x00000028
    Io = 49, // 0x00000031
    IoMap = 50, // 0x00000032
    DataModule = 65, // 0x00000041
    Table = 66, // 0x00000042
    AsFwHw = 67, // 0x00000043
    AsSafetyModule = 68, // 0x00000044
    NcDriver = 69, // 0x00000045
    ACP10 = 70, // 0x00000046
    ProfilerE = 75, // 0x0000004B
    ProfilerData = 76, // 0x0000004C
    TracerDefinition = 77, // 0x0000004D
    TracerData = 78, // 0x0000004E
    NcUpdate = 79, // 0x0000004F
    History = 81, // 0x00000051
    Error = 82, // 0x00000052
    Logger = 83, // 0x00000053
    Noc = 97, // 0x00000061
    Merker = 113, // 0x00000071
    TkLoc = 114, // 0x00000072
    PlcConfig = 129, // 0x00000081
    ComConfig = 130, // 0x00000082
    PpConfig = 131, // 0x00000083
    IOConfig = 132, // 0x00000084
    OpcConfig = 133, // 0x00000085
    OpcUaConfig = 134, // 0x00000086
    Config = 143, // 0x0000008F
    Exception = 145, // 0x00000091
    Interrupt = 146, // 0x00000092
    Device = 161, // 0x000000A1
    Exe = 224, // 0x000000E0
    Probe = 237, // 0x000000ED
    ProbeIo = 238, // 0x000000EE
    OsExe = 239, // 0x000000EF
    Data = 240, // 0x000000F0
    Unknown = 255, // 0x000000FF
  }
}
