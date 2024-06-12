// Decompiled with JetBrains decompiler
// Type: BR.AN.PviServices.MemoryInformation
// Assembly: BR.AN.PVIServices, Version=1.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DA853F0A-C070-4E30-B7A7-51D955FB85F6
// Assembly location: C:\Users\ss_six\Desktop\FXE\FXE software integration package\COMET_FXE-PVIServices_Example\Pluto-PVI-trial_01\packages\IToolS.BandR.3.5.58\lib\net20\BR.AN.PVIServices.dll

using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace BR.AN.PviServices
{
  public class MemoryInformation
  {
    public List<Drive> Drives { get; set; }

    public List<MemoryInfo> Memories { get; set; }

    public MemoryInformation(string memoryInformationXmlString)
    {
      XmlReader xmlReader = (XmlReader) null;
      try
      {
        xmlReader = XmlReader.Create((TextReader) new StringReader(memoryInformationXmlString));
        int content = (int) xmlReader.MoveToContent();
        do
        {
          if (xmlReader.NodeType == XmlNodeType.Element)
          {
            switch (xmlReader.Name)
            {
              case "Drive":
                this.ParseDrivesList(xmlReader.ReadSubtree());
                break;
              case "Memory":
                this.ParseMemoriesList(xmlReader.ReadSubtree());
                break;
            }
          }
        }
        while (xmlReader.Read());
      }
      catch
      {
      }
      finally
      {
        xmlReader?.Close();
      }
    }

    private void ParseDrivesList(XmlReader xmlTReader)
    {
      this.Drives = new List<Drive>();
      int content = (int) xmlTReader.MoveToContent();
      while (xmlTReader.Read())
      {
        if (xmlTReader.HasAttributes)
          this.Drives.Add(new Drive(xmlTReader.ReadSubtree()));
      }
    }

    private void ParseMemoriesList(XmlReader xmlTReader)
    {
      this.Memories = new List<MemoryInfo>();
      int content = (int) xmlTReader.MoveToContent();
      while (xmlTReader.Read())
      {
        if (xmlTReader.HasAttributes)
          this.Memories.Add(new MemoryInfo(xmlTReader.ReadSubtree()));
      }
    }
  }
}
