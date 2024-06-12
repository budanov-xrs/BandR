// Decompiled with JetBrains decompiler
// Type: BR.AN.PviServices.MemoryInfo
// Assembly: BR.AN.PVIServices, Version=1.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DA853F0A-C070-4E30-B7A7-51D955FB85F6
// Assembly location: C:\Users\ss_six\Desktop\FXE\FXE software integration package\COMET_FXE-PVIServices_Example\Pluto-PVI-trial_01\packages\IToolS.BandR.3.5.58\lib\net20\BR.AN.PVIServices.dll

using System.Xml;

namespace BR.AN.PviServices
{
  public class MemoryInfo
  {
    public MemoryInfo(XmlReader xmlTReader)
    {
      bool flag = true;
      while (xmlTReader.AttributeCount == 0 && flag)
        flag = xmlTReader.Read();
      this.Name = xmlTReader.Name;
      string attribute1 = xmlTReader.GetAttribute(nameof (Type));
      if (!string.IsNullOrEmpty(attribute1))
      {
        try
        {
          this.Type = int.Parse(attribute1);
        }
        catch
        {
        }
      }
      string attribute2 = xmlTReader.GetAttribute(nameof (Size));
      if (!string.IsNullOrEmpty(attribute2))
      {
        try
        {
          this.Size = long.Parse(attribute2);
        }
        catch
        {
        }
      }
      string attribute3 = xmlTReader.GetAttribute(nameof (UsedSize));
      if (!string.IsNullOrEmpty(attribute3))
      {
        try
        {
          this.UsedSize = long.Parse(attribute3);
        }
        catch
        {
        }
      }
      string attribute4 = xmlTReader.GetAttribute(nameof (MaxBlockSize));
      if (string.IsNullOrEmpty(attribute4))
        return;
      try
      {
        this.MaxBlockSize = long.Parse(attribute4);
      }
      catch
      {
      }
    }

    public string Name { get; set; }

    public int Type { get; set; }

    public long Size { get; set; }

    public long UsedSize { get; set; }

    public long MaxBlockSize { get; set; }
  }
}
