// Decompiled with JetBrains decompiler
// Type: BR.AN.LCEventArgs
// Assembly: BR.AN.PVIServices, Version=1.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DA853F0A-C070-4E30-B7A7-51D955FB85F6
// Assembly location: C:\Users\ss_six\Desktop\FXE\FXE software integration package\COMET_FXE-PVIServices_Example\Pluto-PVI-trial_01\packages\IToolS.BandR.3.5.58\lib\net20\BR.AN.PVIServices.dll

using System;
using System.Collections;

namespace BR.AN
{
  public class LCEventArgs : EventArgs
  {
    private int propErrorCode;
    private ArrayList propLicenseInfos;

    public LCEventArgs() => this.Initialize(0);

    internal LCEventArgs(int errorCode) => this.Initialize(errorCode);

    internal LCEventArgs(ArrayList listOfLCInfos)
    {
      this.Initialize(0);
      this.propLicenseInfos = listOfLCInfos;
    }

    private void Initialize(int errorCode)
    {
      this.propErrorCode = errorCode;
      this.propLicenseInfos = new ArrayList(1);
    }

    public int ErrorCode => this.propErrorCode;

    public ArrayList LicenseInfos => this.propLicenseInfos;
  }
}
