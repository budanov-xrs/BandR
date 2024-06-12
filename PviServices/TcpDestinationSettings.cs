// Decompiled with JetBrains decompiler
// Type: BR.AN.PviServices.TcpDestinationSettings
// Assembly: BR.AN.PVIServices, Version=1.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DA853F0A-C070-4E30-B7A7-51D955FB85F6
// Assembly location: C:\Users\ss_six\Desktop\FXE\FXE software integration package\COMET_FXE-PVIServices_Example\Pluto-PVI-trial_01\packages\IToolS.BandR.3.5.58\lib\net20\BR.AN.PVIServices.dll

namespace BR.AN.PviServices
{
  public class TcpDestinationSettings
  {
    private string propStringData;
    private string propDestinationIPAddress;
    private int propRemotePortNumber;
    private int propDestinationStationNumber;
    private string propRoutingInformation;

    public TcpDestinationSettings() => this.Init();

    private void Init()
    {
      this.propStringData = "";
      this.propDestinationIPAddress = "";
      this.propRemotePortNumber = 0;
      this.propDestinationStationNumber = 0;
      this.propRoutingInformation = "";
    }

    internal void Parse(string strData)
    {
      this.Init();
      if (0 >= strData.Length)
        return;
      this.propStringData = strData;
      string[] strArray = strData.Split(' ');
      for (int index = 0; index < strArray.Length; ++index)
      {
        string strConnection = strArray.GetValue(index).ToString();
        if (!DeviceBase.UpdateParameterFromString("/DAIP=", strConnection, ref this.propDestinationIPAddress) && !DeviceBase.UpdateParameterFromString("/IP=", strConnection, ref this.propDestinationIPAddress) && !DeviceBase.UpdateParameterFromString("/REPO=", strConnection, ref this.propRemotePortNumber) && !DeviceBase.UpdateParameterFromString("/PT=", strConnection, ref this.propRemotePortNumber) && !DeviceBase.UpdateParameterFromString("/DA=", strConnection, ref this.propDestinationStationNumber))
          DeviceBase.UpdateParameterFromString("/CN=", strConnection, ref this.propRoutingInformation);
      }
    }

    public string DestinationIPAddress => this.propDestinationIPAddress;

    public int RemotePortNumber => this.propRemotePortNumber;

    public int DestinationStationNumber => this.propDestinationStationNumber;

    public string RoutingInformation => this.propRoutingInformation;

    public override string ToString() => this.propStringData;
  }
}
