// Decompiled with JetBrains decompiler
// Type: BR.AN.PviServices.IpAdressConfiguration
// Assembly: BR.AN.PVIServices, Version=1.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DA853F0A-C070-4E30-B7A7-51D955FB85F6
// Assembly location: C:\Users\ss_six\Desktop\FXE\FXE software integration package\COMET_FXE-PVIServices_Example\Pluto-PVI-trial_01\packages\IToolS.BandR.3.5.58\lib\net20\BR.AN.PVIServices.dll

using System.Net;
using System.Xml;

namespace BR.AN.PviServices
{
  public class IpAdressConfiguration
  {
    public IpAdressConfiguration(XmlReader xmlTReader)
    {
      bool flag = true;
      while (xmlTReader.AttributeCount == 0 && flag)
        flag = xmlTReader.Read();
      string attribute1 = xmlTReader.GetAttribute("Name");
      if (!string.IsNullOrEmpty(attribute1))
        this.DeviceName = attribute1;
      string attribute2 = xmlTReader.GetAttribute("Addr");
      if (!string.IsNullOrEmpty(attribute2))
        this.IpAdress = IPAddress.Parse(attribute2);
      string attribute3 = xmlTReader.GetAttribute("SubNetMask");
      if (!string.IsNullOrEmpty(attribute3))
        this.SubnetMask = IPAddress.Parse(attribute3);
      string attribute4 = xmlTReader.GetAttribute(nameof (HostName));
      if (!string.IsNullOrEmpty(attribute4))
        this.HostName = attribute4;
      string attribute5 = xmlTReader.GetAttribute("BaudRate");
      if (!string.IsNullOrEmpty(attribute5))
        this.Baudrate = int.Parse(attribute5);
      string attribute6 = xmlTReader.GetAttribute(nameof (AnslPort));
      if (!string.IsNullOrEmpty(attribute6))
        this.AnslPort = int.Parse(attribute6);
      string attribute7 = xmlTReader.GetAttribute("CluAddr");
      if (string.IsNullOrEmpty(attribute7))
        return;
      this.ClusterIpAdress = IPAddress.Parse(attribute7);
    }

    public string DeviceName { get; set; }

    public IPAddress IpAdress { get; set; }

    public IPAddress SubnetMask { get; set; }

    public string HostName { get; set; }

    public int Baudrate { get; set; }

    public int AnslPort { get; set; }

    public IPAddress ClusterIpAdress { get; set; }
  }
}
