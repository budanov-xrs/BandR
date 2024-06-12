// Decompiled with JetBrains decompiler
// Type: BR.AN.PviServices.InternAction
// Assembly: BR.AN.PVIServices, Version=1.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DA853F0A-C070-4E30-B7A7-51D955FB85F6
// Assembly location: C:\Users\ss_six\Desktop\FXE\FXE software integration package\COMET_FXE-PVIServices_Example\Pluto-PVI-trial_01\packages\IToolS.BandR.3.5.58\lib\net20\BR.AN.PVIServices.dll

namespace BR.AN.PviServices
{
  internal enum InternAction
  {
    LinkForHysteresis = 2712, // 0x00000A98
    LinkForScaling = 2713, // 0x00000A99
    UnlinkForHysteresis = 2714, // 0x00000A9A
    UnlinkForScaling = 2715, // 0x00000A9B
    UnlinkForSetDataType = 2716, // 0x00000A9C
    UnlinkForGetDataType = 2717, // 0x00000A9D
    LinkForSetDataType = 2718, // 0x00000A9E
    LineConnect = 2800, // 0x00000AF0
    LineDisconnect = 2801, // 0x00000AF1
    LineEvent = 2802, // 0x00000AF2
    DeviceConnect = 2803, // 0x00000AF3
    DeviceDisconnect = 2804, // 0x00000AF4
    DeviceEvent = 2805, // 0x00000AF5
    StationConnect = 2806, // 0x00000AF6
    StationDisconnect = 2807, // 0x00000AF7
    StationEvent = 2808, // 0x00000AF8
    VariableReadInternal = 2809, // 0x00000AF9
    VariableReadFormatInternal = 2810, // 0x00000AFA
    VariableExtendedTypInfo = 2811, // 0x00000AFB
    VariableReadStatus = 2812, // 0x00000AFC
    VariableUnlinkStructConnect = 2813, // 0x00000AFD
    LinkForUpload = 2820, // 0x00000B04
    UnlinkForUpload = 2821, // 0x00000B05
    ChangeConnection = 3000, // 0x00000BB8
    ChangeStationConnection = 3001, // 0x00000BB9
    ChangeDeviceConnection = 3002, // 0x00000BBA
    ChangeLineConnection = 3003, // 0x00000BBB
    LoggerReadInfo = 4460, // 0x0000116C
  }
}
