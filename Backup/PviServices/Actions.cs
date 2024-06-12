// Decompiled with JetBrains decompiler
// Type: BR.AN.PviServices.Actions
// Assembly: BR.AN.PVIServices, Version=1.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DA853F0A-C070-4E30-B7A7-51D955FB85F6
// Assembly location: C:\Users\ss_six\Desktop\FXE\FXE software integration package\COMET_FXE-PVIServices_Example\Pluto-PVI-trial_01\packages\IToolS.BandR.3.5.58\lib\net20\BR.AN.PVIServices.dll

using System;

namespace BR.AN.PviServices
{
  [Flags]
  internal enum Actions : uint
  {
    NONE = 0,
    Connect = 1,
    GetList = 2,
    GetValue = 4,
    SetValue = 8,
    GetForce = 16, // 0x00000010
    SetForce = 32, // 0x00000020
    Start = 64, // 0x00000040
    Stop = 128, // 0x00000080
    RunCycle = 256, // 0x00000100
    GetType = 512, // 0x00000200
    SetType = 1024, // 0x00000400
    GetLength = 2048, // 0x00000800
    SetRefresh = 4096, // 0x00001000
    CreateLink = 8192, // 0x00002000
    Upload = 16384, // 0x00004000
    Download = 32768, // 0x00008000
    SetActive = 65536, // 0x00010000
    SetHysteresis = 131072, // 0x00020000
    Disconnect = 262144, // 0x00040000
    SetInitValue = 524288, // 0x00080000
    GetLBType = 1048576, // 0x00100000
    ModuleInfo = 2097152, // 0x00200000
    ListSNMPVariables = 4194304, // 0x00400000
    Connected = 8388608, // 0x00800000
    Uploading = 16777216, // 0x01000000
    GetCpuInfo = 33554432, // 0x02000000
    ReadPVFormat = 67108864, // 0x04000000
    FireActivated = 134217728, // 0x08000000
    ReadIndex = 268435456, // 0x10000000
    FireDataValidated = 536870912, // 0x20000000
    FireValueChanged = 1073741824, // 0x40000000
    Link = 2147483648, // 0x80000000
  }
}
