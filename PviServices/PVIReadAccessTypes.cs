// Decompiled with JetBrains decompiler
// Type: BR.AN.PviServices.PVIReadAccessTypes
// Assembly: BR.AN.PVIServices, Version=1.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DA853F0A-C070-4E30-B7A7-51D955FB85F6
// Assembly location: C:\Users\ss_six\Desktop\FXE\FXE software integration package\COMET_FXE-PVIServices_Example\Pluto-PVI-trial_01\packages\IToolS.BandR.3.5.58\lib\net20\BR.AN.PVIServices.dll

namespace BR.AN.PviServices
{
  internal enum PVIReadAccessTypes
  {
    Type = 1,
    Version = 2,
    Error = 3,
    EventMask = 5,
    ChildObjects = 6,
    ChildTypes = 7,
    Connection = 10, // 0x0000000A
    Data = 11, // 0x0000000B
    State = 12, // 0x0000000C
    BasicAttributes = 13, // 0x0000000D
    ExtendedAttributes = 14, // 0x0000000E
    Refresh = 15, // 0x0000000F
    Hysteresis = 16, // 0x00000010
    DefaultValue = 17, // 0x00000011
    ConversionFunction = 18, // 0x00000012
    ExtendedInternalAttributes = 19, // 0x00000013
    Upload = 20, // 0x00000014
    DateNTime = 22, // 0x00000016
    PLC_Memory = 24, // 0x00000018
    ModuleType = 25, // 0x00000019
    StreamUpload = 26, // 0x0000001A
    ModuleData = 28, // 0x0000001C
    Lines = 30, // 0x0000001E
    Devices = 31, // 0x0000001F
    Stations = 32, // 0x00000020
    Cpus = 33, // 0x00000021
    Modules = 34, // 0x00000022
    Tasks = 35, // 0x00000023
    Variables = 36, // 0x00000024
    PLC_Info = 50, // 0x00000032
    UserTag = 129, // 0x00000081
    License = 200, // 0x000000C8
    Clients = 210, // 0x000000D2
    Snapshot = 240, // 0x000000F0
    CPUInfo = 256, // 0x00000100
    TaskClasses = 257, // 0x00000101
    ModuleList = 258, // 0x00000102
    ReadPhysicalMemory = 259, // 0x00000103
    CpuMemoryInfo = 267, // 0x0000010B
    TTService = 268, // 0x0000010C
    ReadErrorLogBook = 269, // 0x0000010D
    UploadHardWareInfo = 271, // 0x0000010F
    DiagnoseModuleList = 273, // 0x00000111
    SavePath = 290, // 0x00000122
    ResolveNodeNumber = 291, // 0x00000123
    LinkNodeList = 296, // 0x00000128
    LibraryList = 306, // 0x00000132
    LIC_GetLicenseStatus = 351, // 0x0000015F
    ANSL_CpuInfo = 400, // 0x00000190
    ANSL_ModuleInfo = 401, // 0x00000191
    ANSL_TaskInfo = 402, // 0x00000192
    ANSL_ModuleList = 403, // 0x00000193
    ANSL_MemoryInfo = 404, // 0x00000194
    ANSL_HardwareInfo = 405, // 0x00000195
    ANSL_RedundancyInfo = 406, // 0x00000196
    ANSL_CpuExtendedInfo = 407, // 0x00000197
    ANSL_ApplicationInfo = 409, // 0x00000199
    ANSL_LoggerModuleInfo = 416, // 0x000001A0
    ANSL_LoggerModuleData = 417, // 0x000001A1
    ANSL_TracePointsReadData = 452, // 0x000001C4
    ANSL_LISTConnections = 500, // 0x000001F4
    Diagnostics = 777, // 0x00000309
    SNMPListStations = 1400, // 0x00000578
    SNMPListGlobalVariables = 1401, // 0x00000579
    SNMPListLocalVariables = 1402, // 0x0000057A
    SNMPConnectLine = 1403, // 0x0000057B
    SNMPConnectDevice = 1404, // 0x0000057C
    SNMPConnectStation = 1405, // 0x0000057D
    SNMPConnectVariable = 1406, // 0x0000057E
  }
}
