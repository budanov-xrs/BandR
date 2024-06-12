// Decompiled with JetBrains decompiler
// Type: BR.AN.PviServices.DataType
// Assembly: BR.AN.PVIServices, Version=1.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DA853F0A-C070-4E30-B7A7-51D955FB85F6
// Assembly location: C:\Users\ss_six\Desktop\FXE\FXE software integration package\COMET_FXE-PVIServices_Example\Pluto-PVI-trial_01\packages\IToolS.BandR.3.5.58\lib\net20\BR.AN.PVIServices.dll

namespace BR.AN.PviServices
{
  public enum DataType
  {
    Unknown = 0,
    Boolean = 1,
    SByte = 2,
    Int16 = 3,
    Int32 = 4,
    Int64 = 5,
    Byte = 7,
    UInt16 = 8,
    UInt32 = 9,
    UInt64 = 10, // 0x0000000A
    Single = 12, // 0x0000000C
    Double = 13, // 0x0000000D
    TimeSpan = 14, // 0x0000000E
    DateTime = 15, // 0x0000000F
    String = 16, // 0x00000010
    Structure = 17, // 0x00000011
    WString = 18, // 0x00000012
    TimeOfDay = 19, // 0x00000013
    Date = 20, // 0x00000014
    WORD = 21, // 0x00000015
    DWORD = 22, // 0x00000016
    Data = 23, // 0x00000017
    LWORD = 24, // 0x00000018
    UInt8 = 25, // 0x00000019
    TOD = 26, // 0x0000001A
    DT = 27, // 0x0000001B
  }
}
