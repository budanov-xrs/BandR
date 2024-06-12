// Decompiled with JetBrains decompiler
// Type: BR.AN.PviServices.ScalingPointCollection
// Assembly: BR.AN.PVIServices, Version=1.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DA853F0A-C070-4E30-B7A7-51D955FB85F6
// Assembly location: C:\Users\ss_six\Desktop\FXE\FXE software integration package\COMET_FXE-PVIServices_Example\Pluto-PVI-trial_01\packages\IToolS.BandR.3.5.58\lib\net20\BR.AN.PVIServices.dll

using System;
using System.Collections;
using System.Xml;

namespace BR.AN.PviServices
{
  [CLSCompliant(false)]
  public class ScalingPointCollection : ArrayList
  {
    internal DataType propUserDataType;
    internal int propUserTypeLength;
    internal Variable propParent;

    public ScalingPointCollection() => this.propUserDataType = DataType.Unknown;

    public virtual void Add(ScalingPoint scalingPoint)
    {
      if (this.Count == 0)
      {
        if (scalingPoint.YValue.DataType == DataType.String || scalingPoint.YValue.DataType == DataType.Boolean || scalingPoint.YValue.DataType == DataType.DT || scalingPoint.YValue.DataType == DataType.DateTime || scalingPoint.YValue.DataType == DataType.Date || scalingPoint.YValue.DataType == DataType.TOD || scalingPoint.YValue.DataType == DataType.TimeOfDay || scalingPoint.YValue.DataType == DataType.TimeSpan)
          throw new InvalidOperationException();
        if (scalingPoint.YValue.DataType == DataType.Single)
        {
          this.propUserDataType = DataType.Double;
          this.propUserTypeLength = 8;
        }
        else
        {
          this.propUserDataType = scalingPoint.YValue.DataType;
          this.propUserTypeLength = scalingPoint.YValue.TypeLength;
        }
      }
      this.Add((object) scalingPoint);
    }

    internal int ToXMLTextWriter(ref XmlTextWriter writer, ConfigurationFlags flags)
    {
      if (this.Count > 0)
      {
        writer.WriteStartElement("ScalingPoints");
        for (int index = 0; index < this.Count; ++index)
        {
          writer.WriteStartElement("ScalingPoint" + index.ToString());
          ((ScalingPoint) this[index]).ToXMLTextWriter(ref writer, flags);
          writer.WriteEndElement();
        }
        writer.WriteEndElement();
      }
      return 0;
    }

    public int FromXmlTextReader(
      ref XmlTextReader reader,
      ConfigurationFlags flags,
      ScalingPointCollection pointCollection)
    {
      ScalingPoint scalingPoint = new ScalingPoint((Value) 0, (Value) 0);
      while (reader.Read() && reader.NodeType != XmlNodeType.EndElement)
      {
        scalingPoint.FromXmlTextReader(ref reader, flags, scalingPoint);
        pointCollection.Add(scalingPoint);
      }
      reader.Read();
      return 0;
    }
  }
}
