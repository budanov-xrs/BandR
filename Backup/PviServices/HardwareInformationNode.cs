// Decompiled with JetBrains decompiler
// Type: BR.AN.PviServices.HardwareInformationNode
// Assembly: BR.AN.PVIServices, Version=1.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DA853F0A-C070-4E30-B7A7-51D955FB85F6
// Assembly location: C:\Users\ss_six\Desktop\FXE\FXE software integration package\COMET_FXE-PVIServices_Example\Pluto-PVI-trial_01\packages\IToolS.BandR.3.5.58\lib\net20\BR.AN.PVIServices.dll

using System.Xml;

namespace BR.AN.PviServices
{
  public class HardwareInformationNode
  {
    public HardwareInformationNode(XmlReader xmlTReader)
    {
      bool flag = true;
      while (xmlTReader.AttributeCount == 0 && flag)
        flag = xmlTReader.Read();
      string attribute1 = xmlTReader.GetAttribute(nameof (Name));
      if (!string.IsNullOrEmpty(attribute1))
        this.Name = attribute1;
      string attribute2 = xmlTReader.GetAttribute(nameof (Path));
      if (!string.IsNullOrEmpty(attribute2))
        this.Path = attribute2;
      string attribute3 = xmlTReader.GetAttribute(nameof (State));
      if (string.IsNullOrEmpty(attribute3))
        return;
      this.State = attribute3;
    }

    public string Name { get; set; }

    public string Path { get; set; }

    public string State { get; set; }
  }
}
