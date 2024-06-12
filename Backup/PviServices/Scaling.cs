// Decompiled with JetBrains decompiler
// Type: BR.AN.PviServices.Scaling
// Assembly: BR.AN.PVIServices, Version=1.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DA853F0A-C070-4E30-B7A7-51D955FB85F6
// Assembly location: C:\Users\ss_six\Desktop\FXE\FXE software integration package\COMET_FXE-PVIServices_Example\Pluto-PVI-trial_01\packages\IToolS.BandR.3.5.58\lib\net20\BR.AN.PVIServices.dll

using System;
using System.Xml;

namespace BR.AN.PviServices
{
  [CLSCompliant(false)]
  public class Scaling
  {
    internal ScalingType propScalingType;
    private ScalingPointCollection propScalingPoints;
    internal Value propFactor;
    internal Value propMinValue;
    internal Value propMaxValue;

    public Scaling()
    {
      this.Init();
      this.Factor = (Value) 1;
      this.propScalingType = ScalingType.Factor;
    }

    public Scaling(Value factor)
    {
      this.Init();
      this.Factor = factor;
      this.propScalingType = ScalingType.Factor;
    }

    public Scaling(Value minValue, Value maxValue)
    {
      this.propMinValue = minValue;
      this.propMaxValue = maxValue;
      this.Factor = (Value) 1;
      this.propScalingType = ScalingType.LimitValues;
    }

    public Scaling(Value minValue, Value maxValue, Value factor)
    {
      this.propMinValue = minValue;
      this.propMaxValue = maxValue;
      this.Factor = factor;
      this.propScalingType = ScalingType.LimitValuesAndFactor;
    }

    public Scaling(ScalingPointCollection scalingPoints)
    {
      this.Init();
      this.propScalingPoints = scalingPoints;
      this.propScalingType = ScalingType.ScalingPoints;
    }

    internal void Init()
    {
      this.propMaxValue = (Value) int.MaxValue;
      this.propMinValue = (Value) int.MinValue;
    }

    internal int ToXMLTextWriter(ref XmlTextWriter writer, ConfigurationFlags flags)
    {
      int xmlTextWriter1 = 0;
      if (this.propFactor != 1)
        writer.WriteAttributeString("Factor", this.propFactor.ToString());
      if (this.propMinValue != int.MinValue)
        writer.WriteAttributeString("MinValue", this.propMinValue.ToString());
      if (this.propMaxValue != int.MaxValue)
        writer.WriteAttributeString("MaxValue", this.propMaxValue.ToString());
      if (this.propScalingType != ScalingType.ScalingPoints)
        writer.WriteAttributeString("ScalingType", this.propScalingType.ToString());
      int xmlTextWriter2 = this.propScalingPoints.ToXMLTextWriter(ref writer, flags);
      if (xmlTextWriter2 != 0)
        xmlTextWriter1 = xmlTextWriter2;
      return xmlTextWriter1;
    }

    public int FromXmlTextReader(
      ref XmlTextReader reader,
      ConfigurationFlags flags,
      Scaling tmpScaling)
    {
      string str = "";
      string attribute1 = reader.GetAttribute("Factor");
      if (attribute1 != null && attribute1.Length > 0)
        tmpScaling.propFactor = (Value) attribute1;
      str = "";
      string attribute2 = reader.GetAttribute("MinValue");
      if (attribute2 != null && attribute2.Length > 0)
        tmpScaling.propMinValue = (Value) attribute2;
      str = "";
      string attribute3 = reader.GetAttribute("MaxValue");
      if (attribute3 != null && attribute3.Length > 0)
        tmpScaling.propMaxValue = (Value) attribute3;
      str = "";
      string attribute4 = reader.GetAttribute("ScalingType");
      if (attribute4 != null && attribute4.Length > 0)
      {
        switch (attribute4.ToLower())
        {
          case "callback":
            tmpScaling.propScalingType = ScalingType.Callback;
            break;
          case "factor":
            tmpScaling.propScalingType = ScalingType.Factor;
            break;
          case "limitvalues":
            tmpScaling.propScalingType = ScalingType.LimitValues;
            break;
          case "limitvaluesandfactor":
            tmpScaling.propScalingType = ScalingType.LimitValuesAndFactor;
            break;
          case "scalingpoints":
            tmpScaling.propScalingType = ScalingType.ScalingPoints;
            break;
        }
      }
      reader.Read();
      if (reader.Name == "ScalingPoints")
      {
        if (tmpScaling.ScalingPoints == null)
          tmpScaling.ScalingPoints = new ScalingPointCollection();
        tmpScaling.ScalingPoints.FromXmlTextReader(ref reader, flags, tmpScaling.ScalingPoints);
      }
      return 0;
    }

    internal ScalingType ScalingType => this.propScalingType;

    public ScalingPointCollection ScalingPoints
    {
      get => this.propScalingPoints;
      set
      {
        this.propScalingType = ScalingType.ScalingPoints;
        this.propScalingPoints = value;
      }
    }

    public Value Factor
    {
      get => this.propFactor;
      set
      {
        this.propFactor = value;
        this.propScalingType = this.propScalingType != ScalingType.LimitValues ? (this.propScalingType != ScalingType.LimitValuesAndFactor ? ScalingType.Factor : ScalingType.LimitValuesAndFactor) : ScalingType.LimitValuesAndFactor;
        this.propScalingPoints = new ScalingPointCollection()
        {
          new ScalingPoint((Value) this.propMinValue.ToDouble((IFormatProvider) null), (Value) this.propMinValue.ToDouble((IFormatProvider) null) * this.propFactor),
          new ScalingPoint((Value) this.propMaxValue.ToDouble((IFormatProvider) null), (Value) this.propMaxValue.ToDouble((IFormatProvider) null) * this.propFactor)
        };
      }
    }

    public Value MinValue
    {
      get => this.propMinValue;
      set
      {
        this.propMinValue = value;
        if (this.propScalingType == ScalingType.Factor)
          this.propScalingType = ScalingType.LimitValuesAndFactor;
        else if (this.propScalingType == ScalingType.LimitValuesAndFactor)
          this.propScalingType = ScalingType.LimitValuesAndFactor;
        else
          this.propScalingType = ScalingType.LimitValues;
      }
    }

    public Value MaxValue
    {
      get => this.propMaxValue;
      set
      {
        this.propMaxValue = value;
        if (this.propScalingType == ScalingType.Factor)
          this.propScalingType = ScalingType.LimitValuesAndFactor;
        else if (this.propScalingType == ScalingType.LimitValuesAndFactor)
          this.propScalingType = ScalingType.LimitValuesAndFactor;
        else
          this.propScalingType = ScalingType.LimitValues;
      }
    }
  }
}
