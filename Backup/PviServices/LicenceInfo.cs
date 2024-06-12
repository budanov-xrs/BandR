// Decompiled with JetBrains decompiler
// Type: BR.AN.PviServices.LicenceInfo
// Assembly: BR.AN.PVIServices, Version=1.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DA853F0A-C070-4E30-B7A7-51D955FB85F6
// Assembly location: C:\Users\ss_six\Desktop\FXE\FXE software integration package\COMET_FXE-PVIServices_Example\Pluto-PVI-trial_01\packages\IToolS.BandR.3.5.58\lib\net20\BR.AN.PVIServices.dll

using System;
using System.Runtime.InteropServices;
using System.Text;

namespace BR.AN.PviServices
{
  public class LicenceInfo : IDisposable
  {
    private Service propService;
    private byte[] porpLCName;
    internal bool propDisposed;
    private string propLicenseName;
    private string propInfo;
    private RuntimeStates propRuntimeState;

    public LicenceInfo(Service service)
    {
      this.propDisposed = false;
      this.porpLCName = new byte[73];
      this.propService = service;
      this.propLicenseName = "NO LICENCE";
      this.propInfo = "";
      this.propRuntimeState = RuntimeStates.Undefined;
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
      this.porpLCName = (byte[]) null;
      this.propInfo = (string) null;
      this.propLicenseName = (string) null;
      this.propService = (Service) null;
    }

    public event DisposeEventHandler Disposing;

    internal virtual void OnDisposing(bool disposing)
    {
      if (this.Disposing == null)
        return;
      this.Disposing((object) this, new DisposeEventArgs(disposing));
    }

    public string LicenceName => this.propLicenseName;

    public string Info => this.propInfo;

    public RuntimeStates RuntimeState => this.propRuntimeState;

    public int Update(int responseInfo) => this.propService.UpdateLicenceInfo();

    internal void UpdateRuntimeState(IntPtr pRuntimeInfo)
    {
      int num1 = 8;
      this.propLicenseName = "";
      if (IntPtr.Zero == pRuntimeInfo)
      {
        this.propLicenseName = "NO LICENCE";
        this.propRuntimeState = RuntimeStates.Undefined;
      }
      else
      {
        for (int index = 0; index < this.porpLCName.Length; ++index)
          this.porpLCName[index] = (byte) 0;
        byte num2 = Marshal.ReadByte(pRuntimeInfo);
        Marshal.Copy(pRuntimeInfo, this.porpLCName, 0, 73);
        for (int index = 8; index < this.porpLCName.Length; ++index)
        {
          if (this.porpLCName[index] == (byte) 0)
          {
            num1 = index;
            break;
          }
        }
        this.propLicenseName = Encoding.ASCII.GetString(this.porpLCName, 8, num1 - 8);
        switch (num2)
        {
          case 1:
            this.propRuntimeState = RuntimeStates.Trial;
            break;
          case 2:
            this.propRuntimeState = RuntimeStates.Runtime;
            break;
          case 4:
            this.propRuntimeState = RuntimeStates.Locked;
            break;
          default:
            this.propRuntimeState = RuntimeStates.Undefined;
            break;
        }
      }
    }

    public override string ToString() => this.propRuntimeState.ToString() + ";" + this.LicenceName;
  }
}
