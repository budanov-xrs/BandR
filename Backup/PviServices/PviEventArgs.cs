// Decompiled with JetBrains decompiler
// Type: BR.AN.PviServices.PviEventArgs
// Assembly: BR.AN.PVIServices, Version=1.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DA853F0A-C070-4E30-B7A7-51D955FB85F6
// Assembly location: C:\Users\ss_six\Desktop\FXE\FXE software integration package\COMET_FXE-PVIServices_Example\Pluto-PVI-trial_01\packages\IToolS.BandR.3.5.58\lib\net20\BR.AN.PVIServices.dll

using System;

namespace BR.AN.PviServices
{
  public class PviEventArgs : EventArgs
  {
    internal string propErrorText = string.Empty;
    internal string propCurLanguage = string.Empty;
    internal Service propService;
    internal string propName;
    internal string propAddress;
    internal int propErrorCode;
    internal string propLanguage;
    internal Action propAction;

    public PviEventArgs(
      string name,
      string address,
      int errorCode,
      string language,
      Action action)
    {
      this.propName = name;
      this.propAddress = address;
      this.propErrorCode = errorCode;
      this.propLanguage = language;
      this.propAction = action;
    }

    internal PviEventArgs(
      string name,
      string address,
      int errorCode,
      string language,
      Action action,
      Service service)
    {
      this.propName = name;
      this.propAddress = address;
      this.propErrorCode = errorCode;
      this.propLanguage = language;
      this.propAction = action;
      this.propService = service;
    }

    public string Name => this.propName;

    public string Address => this.propAddress;

    public int ErrorCode => this.propErrorCode;

    internal void SetErrorCode(int errorCode) => this.propErrorCode = errorCode;

    public string ErrorText
    {
      get
      {
        if (this.propErrorText == string.Empty)
        {
          this.propCurLanguage = this.propLanguage;
          this.propErrorText = this.propService != null ? this.propService.Utilities.GetErrorText(this.propErrorCode, this.propLanguage) : Service.GetErrorText(this.propErrorCode, this.propLanguage);
          return this.propErrorText == null ? "" : this.propErrorText;
        }
        if (this.propCurLanguage.CompareTo(this.propLanguage) == 0)
          return this.propErrorText;
        this.propCurLanguage = this.propLanguage;
        this.propErrorText = this.propService != null ? this.propService.Utilities.GetErrorText(this.propErrorCode, this.propLanguage) : Service.GetErrorText(this.propErrorCode, this.propLanguage);
        return this.propErrorText == null ? "" : this.propErrorText;
      }
    }

    public Action Action => this.propAction;
  }
}
