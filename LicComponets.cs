// Decompiled with JetBrains decompiler
// Type: BR.AN.LicComponets
// Assembly: BR.AN.PVIServices, Version=1.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DA853F0A-C070-4E30-B7A7-51D955FB85F6
// Assembly location: C:\Users\ss_six\Desktop\FXE\FXE software integration package\COMET_FXE-PVIServices_Example\Pluto-PVI-trial_01\packages\IToolS.BandR.3.5.58\lib\net20\BR.AN.PVIServices.dll

using System;

namespace BR.AN
{
  public class LicComponets : IDisposable
  {
    internal bool propDisposed;
    private string propOrderID;
    private string propLicenseText;
    private bool propRequiresBRIPC;

    public LicComponets() => this.Initialize("", "", false);

    internal LicComponets(string orderID, string licText, bool requiresBRIPC) => this.Initialize(orderID, licText, requiresBRIPC);

    private void Initialize(string orderID, string licText, bool requiresBRIPC)
    {
      this.propDisposed = false;
      this.propOrderID = orderID;
      this.propLicenseText = licText;
      this.propRequiresBRIPC = requiresBRIPC;
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
      this.propLicenseText = (string) null;
      this.propOrderID = (string) null;
    }

    public event DisposeEventHandler Disposing;

    internal virtual void OnDisposing(bool disposing)
    {
      if (this.Disposing == null)
        return;
      this.Disposing((object) this, new DisposeEventArgs(disposing));
    }

    public string OrderID => this.propOrderID;

    public string LicenseText => this.propLicenseText;

    public bool RequiresBRIPC => this.propRequiresBRIPC;
  }
}
