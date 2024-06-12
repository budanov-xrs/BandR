// Decompiled with JetBrains decompiler
// Type: BR.AN.LCInfo
// Assembly: BR.AN.PVIServices, Version=1.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DA853F0A-C070-4E30-B7A7-51D955FB85F6
// Assembly location: C:\Users\ss_six\Desktop\FXE\FXE software integration package\COMET_FXE-PVIServices_Example\Pluto-PVI-trial_01\packages\IToolS.BandR.3.5.58\lib\net20\BR.AN.PVIServices.dll

using System;
using System.Collections;

namespace BR.AN
{
  public class LCInfo : IDisposable
  {
    internal bool propDisposed;
    private string propSerialNumber;
    private string propLCName;
    private string propLCInfo;
    private string propLCPort;
    private ArrayList propActivations;

    public LCInfo() => this.Initialize("", "", "", "", new ArrayList(1));

    internal LCInfo(string name, string port, string info, string serial, ArrayList activations) => this.Initialize(name, port, info, serial, activations);

    private void Initialize(
      string name,
      string port,
      string info,
      string serial,
      ArrayList activations)
    {
      this.propDisposed = false;
      this.propLCName = name;
      this.propLCPort = port;
      this.propLCInfo = info;
      this.propSerialNumber = serial;
      this.propActivations = activations;
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
      this.propActivations.Clear();
      this.propActivations = (ArrayList) null;
      this.propLCInfo = (string) null;
      this.propLCName = (string) null;
      this.propLCPort = (string) null;
      this.propSerialNumber = (string) null;
    }

    public event DisposeEventHandler Disposing;

    internal virtual void OnDisposing(bool disposing)
    {
      if (this.Disposing == null)
        return;
      this.Disposing((object) this, new DisposeEventArgs(disposing));
    }

    public string SerialNumber => this.propSerialNumber;

    public string Name => this.propLCName;

    public string Info => this.propLCInfo;

    public string Port => this.propLCPort;

    public ArrayList Activations => this.propActivations;
  }
}
