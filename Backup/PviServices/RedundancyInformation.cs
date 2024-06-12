// Decompiled with JetBrains decompiler
// Type: BR.AN.PviServices.RedundancyInformation
// Assembly: BR.AN.PVIServices, Version=1.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DA853F0A-C070-4E30-B7A7-51D955FB85F6
// Assembly location: C:\Users\ss_six\Desktop\FXE\FXE software integration package\COMET_FXE-PVIServices_Example\Pluto-PVI-trial_01\packages\IToolS.BandR.3.5.58\lib\net20\BR.AN.PVIServices.dll

using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace BR.AN.PviServices
{
  public class RedundancyInformation
  {
    public RedundancyPriority RifModeSwitchPosition { get; set; }

    public RedundancyPriority CpuRedundancyPriotity { get; set; }

    public RedundancyState CpuRedundancyState { get; set; }

    public DateTime LongPushTimeStamp { get; set; }

    public DateTime ShortPushTimeStamp { get; set; }

    public RedundancySwitchPossibility CpuRedundancySwitchState { get; set; }

    public RedundancyLinkState CpuRedundancyLink { get; set; }

    public string ConfigurationId { get; set; }

    public RedundancyRRadMappingStates RRadMappingStates { get; set; }

    public RedundancyRRadStates RRadStates { get; set; }

    public RedundantCpuIpConfigurations IpConfigurations { get; set; }

    public RedundancyInformation(string redundancyInformationXmlString)
    {
      XmlReader xmlTReader = (XmlReader) null;
      try
      {
        this.IpConfigurations = new RedundantCpuIpConfigurations();
        xmlTReader = XmlReader.Create((TextReader) new StringReader(redundancyInformationXmlString));
        int content = (int) xmlTReader.MoveToContent();
        do
        {
          if (xmlTReader.NodeType == XmlNodeType.Element)
          {
            switch (xmlTReader.Name)
            {
              case "CpuRedInfo":
                this.ParseRootNode(xmlTReader);
                break;
              case "PriIpConf":
                this.IpConfigurations.Primary = new List<IpAdressConfiguration>();
                RedundancyInformation.AddAllConfiguration(xmlTReader.ReadSubtree(), this.IpConfigurations.Primary);
                break;
              case "SecIpConf":
                this.IpConfigurations.Secundary = new List<IpAdressConfiguration>();
                RedundancyInformation.AddAllConfiguration(xmlTReader.ReadSubtree(), this.IpConfigurations.Secundary);
                break;
              case "CluIpConf":
                this.IpConfigurations.Cluster = new IpAdressConfiguration(xmlTReader.ReadSubtree());
                break;
              case "ActIpConf":
                this.IpConfigurations.Active = new IpAdressConfiguration(xmlTReader.ReadSubtree());
                break;
              case "InactIpConf":
                this.IpConfigurations.Inactive = new IpAdressConfiguration(xmlTReader.ReadSubtree());
                break;
              case "LocalIp":
                this.IpConfigurations.Local = new IpAdressConfiguration(xmlTReader.ReadSubtree());
                break;
              case "PartnerIp":
                this.IpConfigurations.Partner = new IpAdressConfiguration(xmlTReader.ReadSubtree());
                break;
            }
          }
        }
        while (xmlTReader.Read());
      }
      catch
      {
      }
      finally
      {
        xmlTReader?.Close();
      }
    }

    private static void AddAllConfiguration(XmlReader xmlTReader, List<IpAdressConfiguration> list)
    {
      int content = (int) xmlTReader.MoveToContent();
      while (xmlTReader.Read())
      {
        if (xmlTReader.HasAttributes)
          list.Add(new IpAdressConfiguration(xmlTReader.ReadSubtree()));
      }
    }

    private void ParseRootNode(XmlReader xmlTReader)
    {
      string attribute1 = xmlTReader.GetAttribute("RIFModeSwitch");
      if (!string.IsNullOrEmpty(attribute1))
        this.RifModeSwitchPosition = (RedundancyPriority) Enum.Parse(typeof (RedundancyPriority), attribute1, true);
      string attribute2 = xmlTReader.GetAttribute("LongPushTimeStamp");
      if (!string.IsNullOrEmpty(attribute2))
      {
        try
        {
          this.LongPushTimeStamp = DateTime.Parse(attribute2);
        }
        catch
        {
        }
      }
      string attribute3 = xmlTReader.GetAttribute("ShortPushTimeStamp");
      if (!string.IsNullOrEmpty(attribute3))
      {
        try
        {
          this.ShortPushTimeStamp = DateTime.Parse(attribute3);
        }
        catch
        {
        }
      }
      string attribute4 = xmlTReader.GetAttribute("CpuModeSwitch");
      if (!string.IsNullOrEmpty(attribute4))
        this.CpuRedundancyPriotity = (RedundancyPriority) Enum.Parse(typeof (RedundancyPriority), attribute4, true);
      string attribute5 = xmlTReader.GetAttribute("CpuProcessCtrlState");
      if (!string.IsNullOrEmpty(attribute5))
        this.CpuRedundancyState = (RedundancyState) Enum.Parse(typeof (RedundancyState), attribute5, true);
      string attribute6 = xmlTReader.GetAttribute("SwitchoverLevel");
      if (!string.IsNullOrEmpty(attribute6))
        this.CpuRedundancySwitchState = (RedundancySwitchPossibility) Enum.Parse(typeof (RedundancySwitchPossibility), attribute6, true);
      string attribute7 = xmlTReader.GetAttribute("RIFLinkActive");
      if (!string.IsNullOrEmpty(attribute7))
        this.CpuRedundancyLink = (RedundancyLinkState) Enum.Parse(typeof (RedundancyLinkState), attribute7, true);
      string attribute8 = xmlTReader.GetAttribute("ProjectName");
      if (!string.IsNullOrEmpty(attribute8))
        this.ConfigurationId = attribute8;
      string attribute9 = xmlTReader.GetAttribute("RRADMapping");
      if (!string.IsNullOrEmpty(attribute9))
        this.RRadMappingStates = (RedundancyRRadMappingStates) Enum.Parse(typeof (RedundancyRRadMappingStates), attribute9, true);
      string attribute10 = xmlTReader.GetAttribute("RRADState");
      if (string.IsNullOrEmpty(attribute10))
        return;
      this.RRadStates = (RedundancyRRadStates) Enum.Parse(typeof (RedundancyRRadStates), attribute10, true);
    }
  }
}
