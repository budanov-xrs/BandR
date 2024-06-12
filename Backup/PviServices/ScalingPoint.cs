// Decompiled with JetBrains decompiler
// Type: BR.AN.PviServices.ScalingPoint
// Assembly: BR.AN.PVIServices, Version=1.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DA853F0A-C070-4E30-B7A7-51D955FB85F6
// Assembly location: C:\Users\ss_six\Desktop\FXE\FXE software integration package\COMET_FXE-PVIServices_Example\Pluto-PVI-trial_01\packages\IToolS.BandR.3.5.58\lib\net20\BR.AN.PVIServices.dll

using System;
using System.Xml;

namespace BR.AN.PviServices
{
  [CLSCompliant(false)]
  public class ScalingPoint
  {
    private Value xValue;
    private Value yValue;

    public ScalingPoint(Value xValue, Value yValue)
    {
      this.xValue = xValue;
      this.yValue = yValue;
    }

    public Value XValue => this.xValue;

    public Value YValue => this.yValue;

    internal int ToXMLTextWriter(ref XmlTextWriter writer, ConfigurationFlags flags)
    {
      writer.WriteAttributeString("XValue", (string) this.xValue);
      writer.WriteAttributeString("YValue", (string) this.yValue);
      return 0;
    }

    public int FromXmlTextReader(
      ref XmlTextReader reader,
      ConfigurationFlags flags,
      ScalingPoint point)
    {
      if (point == null)
        return -1;
      double result = 0.0;
      string attribute1 = reader.GetAttribute("XValue");
      if (attribute1 != null && attribute1.Length > 0 && PviParse.TryParseDouble(attribute1, out result))
        point.xValue = (Value) result;
      string attribute2 = reader.GetAttribute("YValue");
      if (attribute2 != null && attribute2.Length > 0 && PviParse.TryParseDouble(attribute2, out result))
        point.yValue = (Value) result;
      return 0;
    }
  }
}
