// Decompiled with JetBrains decompiler
// Type: BR.AN.PviServices.PviParse
// Assembly: BR.AN.PVIServices, Version=1.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DA853F0A-C070-4E30-B7A7-51D955FB85F6
// Assembly location: C:\Users\ss_six\Desktop\FXE\FXE software integration package\COMET_FXE-PVIServices_Example\Pluto-PVI-trial_01\packages\IToolS.BandR.3.5.58\lib\net20\BR.AN.PVIServices.dll

using System;

namespace BR.AN.PviServices
{
  internal class PviParse
  {
    public static bool TryParseInt32(string s, out int result)
    {
      try
      {
        result = int.Parse(s);
        return true;
      }
      catch
      {
        result = 0;
        return false;
      }
    }

    public static bool TryParseInt16(string s, out short result)
    {
      try
      {
        result = short.Parse(s);
        return true;
      }
      catch
      {
        result = (short) 0;
        return false;
      }
    }

    public static bool TryParseUInt16(string s, out ushort result)
    {
      try
      {
        result = ushort.Parse(s);
        return true;
      }
      catch
      {
        result = (ushort) 0;
        return false;
      }
    }

    public static bool TryParseUInt32(string s, out uint result)
    {
      try
      {
        result = uint.Parse(s);
        return true;
      }
      catch
      {
        result = 0U;
        return false;
      }
    }

    public static bool TryParseDateTime(string s, out DateTime result)
    {
      try
      {
        result = DateTime.Parse(s);
        return true;
      }
      catch
      {
        result = DateTime.MinValue;
        return false;
      }
    }

    public static bool TryParseDouble(string s, out double result)
    {
      try
      {
        result = double.Parse(s);
        return true;
      }
      catch
      {
        result = 0.0;
        return false;
      }
    }

    public static bool TryParseByte(string s, out byte result)
    {
      try
      {
        result = byte.Parse(s);
        return true;
      }
      catch
      {
        result = (byte) 0;
        return false;
      }
    }
  }
}
