// Decompiled with JetBrains decompiler
// Type: BR.AN.PviServices.PVIWriteAccessTypes
// Assembly: BR.AN.PVIServices, Version=1.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DA853F0A-C070-4E30-B7A7-51D955FB85F6
// Assembly location: C:\Users\ss_six\Desktop\FXE\FXE software integration package\COMET_FXE-PVIServices_Example\Pluto-PVI-trial_01\packages\IToolS.BandR.3.5.58\lib\net20\BR.AN.PVIServices.dll

namespace BR.AN.PviServices
{
  internal enum PVIWriteAccessTypes
  {
    EventMask = 5,
    Connection = 10, // 0x0000000A
    Data = 11, // 0x0000000B
    State = 12, // 0x0000000C
    BasicAttributes = 13, // 0x0000000D
    ExtendedAttributes = 14, // 0x0000000E
    Refresh = 15, // 0x0000000F
    Hysteresis = 16, // 0x00000010
    ConversionFunction = 18, // 0x00000012
    Download = 21, // 0x00000015
    DateNTime = 22, // 0x00000016
    Delete_PLC_Memory = 23, // 0x00000017
    StreamDownLoad = 27, // 0x0000001B
    CpuModuleDelete = 29, // 0x0000001D
    Cancel = 128, // 0x00000080
    UserTag = 129, // 0x00000081
    Snapshot = 240, // 0x000000F0
    WritePhysicalMemory = 260, // 0x00000104
    TTService = 268, // 0x0000010C
    DeleteDiagModule = 274, // 0x00000112
    StartModule = 276, // 0x00000114
    StopModule = 277, // 0x00000115
    ResumeModule = 278, // 0x00000116
    BurnModule = 279, // 0x00000117
    DeleteModule = 280, // 0x00000118
    ClearMemory = 281, // 0x00000119
    ForceOn = 282, // 0x0000011A
    ForceOff = 283, // 0x0000011B
    SavePath = 290, // 0x00000122
    GlobalForceOFF = 297, // 0x00000129
    ANSL_RedundancyControl = 440, // 0x000001B8
    ANSL_TracePointsRegister = 450, // 0x000001C2
    ANSL_TracePointsUnregister = 451, // 0x000001C3
    ANSL_COMMAND_Data = 460, // 0x000001CC
  }
}
