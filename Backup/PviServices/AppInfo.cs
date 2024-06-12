// Decompiled with JetBrains decompiler
// Type: BR.AN.PviServices.AppInfo
// Assembly: BR.AN.PVIServices, Version=1.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DA853F0A-C070-4E30-B7A7-51D955FB85F6
// Assembly location: C:\Users\ss_six\Desktop\FXE\FXE software integration package\COMET_FXE-PVIServices_Example\Pluto-PVI-trial_01\packages\IToolS.BandR.3.5.58\lib\net20\BR.AN.PVIServices.dll

using System.IO;
using System.Xml;

namespace BR.AN.PviServices
{
  public class AppInfo
  {
    public AppInfo(string xmlData)
    {
      XmlReader xmlReader = (XmlReader) null;
      try
      {
        xmlReader = XmlReader.Create((TextReader) new StringReader(xmlData));
        int content = (int) xmlReader.MoveToContent();
        do
        {
          if (xmlReader.NodeType == XmlNodeType.Element)
          {
            switch (xmlReader.Name)
            {
              case "ApplicationInformation":
                string attribute1 = xmlReader.GetAttribute("ProjectId");
                if (!string.IsNullOrEmpty(attribute1))
                  this.Version = attribute1;
                string attribute2 = xmlReader.GetAttribute("AutomationRuntime");
                if (!string.IsNullOrEmpty(attribute2))
                  this.OsVersion = attribute2;
                string attribute3 = xmlReader.GetAttribute("ProjectName");
                if (!string.IsNullOrEmpty(attribute3))
                {
                  this.ConfigurationId = attribute3;
                  break;
                }
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

    public string Version { set; get; }

    public string OsVersion { set; get; }

    public string ConfigurationId { set; get; }
  }
}
