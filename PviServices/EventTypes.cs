// Decompiled with JetBrains decompiler
// Type: BR.AN.PviServices.EventTypes
// Assembly: BR.AN.PVIServices, Version=1.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DA853F0A-C070-4E30-B7A7-51D955FB85F6
// Assembly location: C:\Users\ss_six\Desktop\FXE\FXE software integration package\COMET_FXE-PVIServices_Example\Pluto-PVI-trial_01\packages\IToolS.BandR.3.5.58\lib\net20\BR.AN.PVIServices.dll

namespace BR.AN.PviServices
{
  internal enum EventTypes
  {
    Error = 3,
    Disconnect = 9,
    Connection = 10, // 0x0000000A
    Data = 11, // 0x0000000B
    Status = 12, // 0x0000000C
    Dataform = 13, // 0x0000000D
    Proceeding = 128, // 0x00000080
    UserTag = 129, // 0x00000081
    ServiceConnect = 240, // 0x000000F0
    ServiceDisconnect = 241, // 0x000000F1
    ServiceArrange = 242, // 0x000000F2
    EVENT_LINEBASE = 255, // 0x000000FF
    ModuleChanged = 256, // 0x00000100
    EventDebugger = 257, // 0x00000101
    ModuleDeleted = 258, // 0x00000102
    ModuleListChangedXML = 403, // 0x00000193
    RedundancyCtrlEventXML = 440, // 0x000001B8
    TracePointsDataChanged = 452, // 0x000001C4
  }
}
