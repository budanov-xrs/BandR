// Decompiled with JetBrains decompiler
// Type: BR.AN.PviServices.EventDataTypes
// Assembly: BR.AN.PVIServices, Version=1.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DA853F0A-C070-4E30-B7A7-51D955FB85F6
// Assembly location: C:\Users\ss_six\Desktop\FXE\FXE software integration package\COMET_FXE-PVIServices_Example\Pluto-PVI-trial_01\packages\IToolS.BandR.3.5.58\lib\net20\BR.AN.PVIServices.dll

namespace BR.AN.PviServices
{
  public enum EventDataTypes
  {
    Undefined = 0,
    ASCIIStrings = 1,
    ANSIStrings = 16, // 0x00000010
    UInt8 = 64, // 0x00000040
    Int8 = 65, // 0x00000041
    UInt16 = 66, // 0x00000042
    Int16 = 67, // 0x00000043
    UInt32 = 68, // 0x00000044
    Int32 = 69, // 0x00000045
    UInt64 = 70, // 0x00000046
    Int64 = 71, // 0x00000047
    UInt8BE = 72, // 0x00000048
    Int8BE = 73, // 0x00000049
    UInt16BE = 74, // 0x0000004A
    Int16BE = 75, // 0x0000004B
    UInt32BE = 76, // 0x0000004C
    Int32BE = 77, // 0x0000004D
    UInt64BE = 78, // 0x0000004E
    Int64BE = 79, // 0x0000004F
    AsciiChar = 80, // 0x00000050
    AnsiChar = 81, // 0x00000051
    UTF16Char = 82, // 0x00000052
    UTF16CharBE = 83, // 0x00000053
    UTF32Char = 84, // 0x00000054
    UTF32CharBE = 85, // 0x00000055
    BooleanFalse = 96, // 0x00000060
    BooleanTrue = 97, // 0x00000061
    MemAddress = 100, // 0x00000064
    MemAddressBE = 101, // 0x00000065
    Float32 = 103, // 0x00000067
    Float32BE = 104, // 0x00000068
    Double64 = 105, // 0x00000069
    Double64BE = 105, // 0x00000069
    MBCSStrings = 136, // 0x00000088
    UTF16Strings = 138, // 0x0000008A
    UTF16StringsBE = 139, // 0x0000008B
    UTF32Strings = 140, // 0x0000008C
    UTF32StringsBE = 141, // 0x0000008D
    BytesLigttleEndian = 240, // 0x000000F0
    BytesBigEndian = 242, // 0x000000F2
    ArLoggerAPI = 65534, // 0x0000FFFE
    EmptyEventData = 65535, // 0x0000FFFF
  }
}
