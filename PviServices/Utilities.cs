// Decompiled with JetBrains decompiler
// Type: BR.AN.PviServices.Utilities
// Assembly: BR.AN.PVIServices, Version=1.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DA853F0A-C070-4E30-B7A7-51D955FB85F6
// Assembly location: C:\Users\ss_six\Desktop\FXE\FXE software integration package\COMET_FXE-PVIServices_Example\Pluto-PVI-trial_01\packages\IToolS.BandR.3.5.58\lib\net20\BR.AN.PVIServices.dll

using System;
using System.Reflection;
using System.Resources;

namespace BR.AN.PviServices
{
  public class Utilities : IDisposable
  {
    private ResourceManager propRM;
    private ResourceManager propRMPCC;
    internal bool propDisposed;
    private string propActiveCulture;

    public Utilities()
    {
      this.propDisposed = true;
      this.propRM = (ResourceManager) null;
      this.propRMPCC = (ResourceManager) null;
      this.propActiveCulture = "undefined";
    }

    public void Dispose()
    {
      if (this.propDisposed)
        return;
      this.Dispose(true);
      GC.SuppressFinalize((object) this);
    }

    internal virtual void Dispose(bool disposing)
    {
      if (this.propDisposed)
        return;
      this.OnDisposing(disposing);
      if (!disposing)
        return;
      this.propDisposed = true;
      this.propActiveCulture = (string) null;
      this.propRM = (ResourceManager) null;
      this.propRMPCC = (ResourceManager) null;
    }

    public event DisposeEventHandler Disposing;

    internal virtual void OnDisposing(bool disposing)
    {
      if (this.Disposing == null)
        return;
      this.Disposing((object) this, new DisposeEventArgs(disposing));
    }

    public string ActiveCulture
    {
      get => this.propActiveCulture;
      set
      {
        this.propActiveCulture = value;
        if (!(this.propActiveCulture != ""))
          return;
        this.propRM = (ResourceManager) null;
        this.propRMPCC = (ResourceManager) null;
        if (this.propActiveCulture.ToLower().IndexOf("de") == 0)
        {
          this.propRM = new ResourceManager("BR.AN.PviServices.Resources.de.PviErrors", Assembly.GetExecutingAssembly());
          this.propRMPCC = new ResourceManager("BR.AN.PviServices.Resources.de.PccLog", Assembly.GetExecutingAssembly());
        }
        else if (this.propActiveCulture.ToLower().IndexOf("fr") == 0)
        {
          this.propRM = new ResourceManager("BR.AN.PviServices.Resources.fr.PviErrors", Assembly.GetExecutingAssembly());
          this.propRMPCC = new ResourceManager("BR.AN.PviServices.Resources.fr.PccLog", Assembly.GetExecutingAssembly());
        }
        else
        {
          if (this.propActiveCulture.ToLower().IndexOf("en") != 0)
            return;
          this.propRM = new ResourceManager("BR.AN.PviServices.Resources.en.PviErrors", Assembly.GetExecutingAssembly());
          this.propRMPCC = new ResourceManager("BR.AN.PviServices.Resources.en.PccLog", Assembly.GetExecutingAssembly());
        }
      }
    }

    public string GetErrorText(int error)
    {
      if (this.propRM != null)
      {
        try
        {
          return this.propRM.GetString(string.Format("{0:0000}", (object) error)) ?? this.propRMPCC.GetString(string.Format("{0:0000}", (object) error));
        }
        catch (System.Exception ex)
        {
        }
      }
      return error.ToString();
    }

    public string GetErrorText(int error, string culture)
    {
      string errorText = "";
      if (culture != null && culture != "")
      {
        if (culture.CompareTo(this.propActiveCulture) == 0)
          return this.GetErrorText(error);
        try
        {
          errorText = (culture.ToLower().IndexOf("de") != 0 ? (culture.ToLower().IndexOf("fr") != 0 ? new ResourceManager("BR.AN.PviServices.Resources.en.PviErrors", Assembly.GetExecutingAssembly()) : new ResourceManager("BR.AN.PviServices.Resources.fr.PviErrors", Assembly.GetExecutingAssembly())) : new ResourceManager("BR.AN.PviServices.Resources.de.PviErrors", Assembly.GetExecutingAssembly())).GetString(string.Format("{0:0000}", (object) error));
          if (errorText == null)
            errorText = (culture.ToLower().IndexOf("de") != 0 ? (culture.ToLower().IndexOf("fr") != 0 ? new ResourceManager("BR.AN.PviServices.Resources.en.PccLog", Assembly.GetExecutingAssembly()) : new ResourceManager("BR.AN.PviServices.Resources.fr.PccLog", Assembly.GetExecutingAssembly())) : new ResourceManager("BR.AN.PviServices.Resources.de.PccLog", Assembly.GetExecutingAssembly())).GetString(string.Format("{0:0000}", (object) error));
        }
        catch (System.Exception ex)
        {
          string message = ex.Message;
        }
      }
      return errorText;
    }

    public string GetErrorTextPCC(int error)
    {
      if (this.propRMPCC != null)
      {
        try
        {
          return this.propRMPCC.GetString(string.Format("{0:0000}", (object) error));
        }
        catch (System.Exception ex)
        {
          string message = ex.Message;
        }
      }
      return error.ToString();
    }

    [CLSCompliant(false)]
    public string GetErrorTextPCC(uint error)
    {
      if (this.propRMPCC != null)
      {
        try
        {
          return this.propRMPCC.GetString(string.Format("{0:0000}", (object) error));
        }
        catch (System.Exception ex)
        {
          string message = ex.Message;
        }
      }
      return error.ToString();
    }
  }
}
