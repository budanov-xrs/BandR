// Decompiled with JetBrains decompiler
// Type: BR.AN.PviServices.AccessTypes
// Assembly: BR.AN.PVIServices, Version=1.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DA853F0A-C070-4E30-B7A7-51D955FB85F6
// Assembly location: C:\Users\ss_six\Desktop\FXE\FXE software integration package\COMET_FXE-PVIServices_Example\Pluto-PVI-trial_01\packages\IToolS.BandR.3.5.58\lib\net20\BR.AN.PVIServices.dll

namespace BR.AN.PviServices
{
  internal enum AccessTypes
  {
    None = 0,
    ObjectType = 1,
    Version = 2,
    Error = 3,
    EventMask = 5,
    List = 6,
    ListExtern = 7,
    Connect = 10, // 0x0000000A
    Data = 11, // 0x0000000B
    Status = 12, // 0x0000000C
    Type = 13, // 0x0000000D
    TypeExtern = 14, // 0x0000000E
    Refresh = 15, // 0x0000000F
    Hysteresis = 16, // 0x00000010
    Replace = 17, // 0x00000011
    Function = 18, // 0x00000012
    TypeIntern = 19, // 0x00000013
    Upload = 20, // 0x00000014
    Download = 21, // 0x00000015
    DateTime = 22, // 0x00000016
    MemoryDelete = 23, // 0x00000017
    MemoryInfo = 24, // 0x00000018
    ModuleType = 25, // 0x00000019
    UploadStream = 26, // 0x0000001A
    DownloadStream = 27, // 0x0000001B
    ModuleData = 28, // 0x0000001C
    CpuModuleDelete = 29, // 0x0000001D
    ListLine = 30, // 0x0000001E
    ListDevice = 31, // 0x0000001F
    ListStation = 32, // 0x00000020
    ListCpu = 33, // 0x00000021
    ListModule = 34, // 0x00000022
    ListTask = 35, // 0x00000023
    ListVariable = 36, // 0x00000024
    CpuInfo = 50, // 0x00000032
    Cancel = 128, // 0x00000080
    UserTag = 129, // 0x00000081
    License = 200, // 0x000000C8
    Global = 201, // 0x000000C9
    Config = 202, // 0x000000CA
    Clients = 210, // 0x000000D2
    PVIVersion = 211, // 0x000000D3
    Snapshot = 240, // 0x000000F0
    DataLogger = 241, // 0x000000F1
    PviParameter = 242, // 0x000000F2
    StopPviManager = 243, // 0x000000F3
    MessageBox = 250, // 0x000000FA
    Info = 256, // 0x00000100
    TaskClass = 257, // 0x00000101
    ModuleList = 258, // 0x00000102
    ReadPhysicalMemory = 259, // 0x00000103
    WritePhysicalMemory = 260, // 0x00000104
    Reset = 261, // 0x00000105
    VariableList = 262, // 0x00000106
    StopCpu = 263, // 0x00000107
    StartCpu = 264, // 0x00000108
    StopTaskClass = 265, // 0x00000109
    StartTaskClass = 266, // 0x0000010A
    CpuMemoryInfo = 267, // 0x0000010B
    TTService = 268, // 0x0000010C
    ReadError = 269, // 0x0000010D
    HardwareUpload = 271, // 0x0000010F
    LoadDiagnose = 272, // 0x00000110
    ReadDiagModList = 273, // 0x00000111
    DeleteDiagModule = 274, // 0x00000112
    InstallModule = 275, // 0x00000113
    StartModule = 276, // 0x00000114
    StopModule = 277, // 0x00000115
    ResumeModule = 278, // 0x00000116
    BurnModule = 279, // 0x00000117
    DeleteModule = 280, // 0x00000118
    ClearMemory = 281, // 0x00000119
    ForceOn = 282, // 0x0000011A
    ForceOff = 283, // 0x0000011B
    GetRtc = 284, // 0x0000011C
    SetRtc = 285, // 0x0000011D
    GetErrorText = 286, // 0x0000011E
    DisableCon = 287, // 0x0000011F
    EnableCon = 288, // 0x00000120
    ModulePath = 289, // 0x00000121
    SavePath = 290, // 0x00000122
    ResolveNodeNumber = 291, // 0x00000123
    LinkNodeList = 296, // 0x00000128
    LinkNodeForceOff = 297, // 0x00000129
    LibraryList = 306, // 0x00000132
    LIC_ListDongles = 346, // 0x0000015A
    LIC_ListOfExistingLicenses = 347, // 0x0000015B
    LIC_ListOfRequiredLicenses = 348, // 0x0000015C
    LIC_ReadContext = 349, // 0x0000015D
    LIC_UpdateLicense = 350, // 0x0000015E
    LIC_GetLicenseStatus = 351, // 0x0000015F
    LIC_BlinkDongle = 352, // 0x00000160
    ANSL_CpuInfo = 400, // 0x00000190
    ANSL_ModuleInfo = 401, // 0x00000191
    ANSL_TaskInfo = 402, // 0x00000192
    ANSL_ModuleList = 403, // 0x00000193
    ANSL_MemoryInfo = 404, // 0x00000194
    ANSL_HardwareInfo = 405, // 0x00000195
    ANSL_RedundancyInfo = 406, // 0x00000196
    ANSL_CpuExtendedInfo = 407, // 0x00000197
    ANSL_TaskClassInfo = 408, // 0x00000198
    ANSL_ApplicationInfo = 409, // 0x00000199
    ANSL_LoggerModuleInfo = 416, // 0x000001A0
    ANSL_LoggerModuleData = 417, // 0x000001A1
    ANSL_RedundancyControl = 440, // 0x000001B8
    ANSL_TracePointsRegister = 450, // 0x000001C2
    ANSL_TracePointsUnregister = 451, // 0x000001C3
    ANSL_TracePointsReadData = 452, // 0x000001C4
    ANSL_COMMAND_Data = 460, // 0x000001CC
    ANSL_LISTConnections = 500, // 0x000001F4
  }
}
