// Decompiled with JetBrains decompiler
// Type: BR.AN.PviServices.Drive
// Assembly: BR.AN.PVIServices, Version=1.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DA853F0A-C070-4E30-B7A7-51D955FB85F6
// Assembly location: C:\Users\ss_six\Desktop\FXE\FXE software integration package\COMET_FXE-PVIServices_Example\Pluto-PVI-trial_01\packages\IToolS.BandR.3.5.58\lib\net20\BR.AN.PVIServices.dll

using System.Xml;

namespace BR.AN.PviServices
{
  public class Drive
  {
    public Drive(XmlReader xmlTReader)
    {
      bool flag = true;
      while (xmlTReader.AttributeCount == 0 && flag)
        flag = xmlTReader.Read();
      this.DriveNumber = xmlTReader.Name;
      string attribute1 = xmlTReader.GetAttribute("ID");
      if (!string.IsNullOrEmpty(attribute1))
        this.Id = attribute1;
      string attribute2 = xmlTReader.GetAttribute(nameof (Name));
      if (!string.IsNullOrEmpty(attribute2))
        this.Name = attribute2;
      string attribute3 = xmlTReader.GetAttribute(nameof (Size));
      if (!string.IsNullOrEmpty(attribute3))
      {
        try
        {
          this.Size = long.Parse(attribute3);
        }
        catch
        {
        }
      }
      string attribute4 = xmlTReader.GetAttribute(nameof (UsedSize));
      if (string.IsNullOrEmpty(attribute4))
        return;
      try
      {
        this.UsedSize = long.Parse(attribute4);
      }
      catch
      {
      }
    }

    public string DriveNumber { get; set; }

    public string Name { get; set; }

    public string Id { get; set; }

    public long Size { get; set; }

    public long UsedSize { get; set; }
  }
}
