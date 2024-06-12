// Decompiled with JetBrains decompiler
// Type: BR.AN.BRLicenseInfo
// Assembly: BR.AN.PVIServices, Version=1.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DA853F0A-C070-4E30-B7A7-51D955FB85F6
// Assembly location: C:\Users\ss_six\Desktop\FXE\FXE software integration package\COMET_FXE-PVIServices_Example\Pluto-PVI-trial_01\packages\IToolS.BandR.3.5.58\lib\net20\BR.AN.PVIServices.dll

using System;
using System.Collections;
using System.Runtime.InteropServices;

namespace BR.AN
{
  public class BRLicenseInfo : IDisposable
  {
    private ArrayList propLCInfos;
    private int propSearchStep;
    internal CBFindKeys callBackFindKeys;
    internal CBFindModules callBackFindMods;
    internal CBFindBRIpc callBackFindBRIpc;
    internal bool propDisposed;
    private BuRIPCStates propBRIPCState;

    [DllImport("BrSecDll.dll")]
    internal static extern int Br_ReadDsKey(CBFindKeys pCallback, IntPtr pCBData);

    [DllImport("BrSecDll.dll")]
    internal static extern int Br_ReadLcMod(
      CBFindModules pCallback,
      IntPtr pCBData,
      string searchPath);

    [DllImport("BrSecDll.dll")]
    internal static extern int Br_FindBrIPC(
      CBFindBRIpc pCallback,
      IntPtr pCBData,
      long appId,
      long appSubId);

    public event LCInfosEventHandler Found;

    public BRLicenseInfo()
    {
      this.propDisposed = false;
      this.propLCInfos = new ArrayList(1);
      this.propSearchStep = 0;
      this.propBRIPCState = BuRIPCStates.INVALID;
      this.callBackFindKeys = new CBFindKeys(this.CallBackFindKeys);
      this.callBackFindMods = new CBFindModules(this.CallBackFindLicModules);
      this.callBackFindBRIpc = new CBFindBRIpc(this.CallBackFindBRIpc);
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
      this.callBackFindBRIpc = (CBFindBRIpc) null;
      this.callBackFindKeys = (CBFindKeys) null;
      this.callBackFindMods = (CBFindModules) null;
      this.Disposing = (DisposeEventHandler) null;
      this.Found = (LCInfosEventHandler) null;
      this.propLCInfos.Clear();
      this.propLCInfos = (ArrayList) null;
    }

    public event DisposeEventHandler Disposing;

    internal virtual void OnDisposing(bool disposing)
    {
      if (this.Disposing == null)
        return;
      this.Disposing((object) this, new DisposeEventArgs(disposing));
    }

    public BuRIPCStates BRIPCState => this.propBRIPCState;

    protected internal bool CallBackFindKeys(
      int pCBData,
      [MarshalAs(UnmanagedType.LPStr)] string keySerial,
      [MarshalAs(UnmanagedType.LPStr)] string keyPort,
      [MarshalAs(UnmanagedType.LPStr)] string dsID,
      [MarshalAs(UnmanagedType.LPArray, SizeConst = 10)] BRSECComponentEntry[] pSecInfos)
    {
      ArrayList activations = new ArrayList(1);
      for (int index = 0; index < pSecInfos.GetLength(0); ++index)
      {
        BRSECComponentEntry brsecComponentEntry = (BRSECComponentEntry) pSecInfos.GetValue(index);
        if (brsecComponentEntry.OrderId.Length != 0 && brsecComponentEntry.LicenseText.Length != 0)
        {
          LicComponets licComponets = new LicComponets(brsecComponentEntry.OrderId, brsecComponentEntry.LicenseText, brsecComponentEntry.RequiresBRIPC != char.MinValue);
          activations.Add((object) licComponets);
        }
        else
          break;
      }
      this.OnCBFindKeys(new LCInfo(dsID, keyPort, "", keySerial, activations));
      return true;
    }

    protected internal bool CallBackFindLicModules(
      int pCBData,
      [MarshalAs(UnmanagedType.LPStr)] string licName,
      [MarshalAs(UnmanagedType.LPStr)] string licInfo,
      [MarshalAs(UnmanagedType.LPStr)] string licID,
      [MarshalAs(UnmanagedType.LPArray, SizeConst = 10)] BRSECComponentEntry[] pSecInfos)
    {
      ArrayList activations = new ArrayList(1);
      for (int index = 0; index < pSecInfos.GetLength(0); ++index)
      {
        BRSECComponentEntry brsecComponentEntry = (BRSECComponentEntry) pSecInfos.GetValue(index);
        if (brsecComponentEntry.OrderId.Length != 0 && brsecComponentEntry.LicenseText.Length != 0)
        {
          LicComponets licComponets = new LicComponets(brsecComponentEntry.OrderId, brsecComponentEntry.LicenseText, brsecComponentEntry.RequiresBRIPC != char.MinValue);
          activations.Add((object) licComponets);
        }
        else
          break;
      }
      this.OnCBFindKeys(new LCInfo(licID, "DLL", licInfo, licName, activations));
      return true;
    }

    protected internal bool CallBackFindBRIpc(int pCBData, int appID, int appSubId)
    {
      if (pCBData == 0)
        this.OnCBFindBRIPc(appID, appSubId);
      else
        this.propBRIPCState = BuRIPCStates.RunningOnABuRIPC;
      return true;
    }

    protected void OnCBFindKeys(LCInfo lcInfo)
    {
      if (lcInfo != null)
        this.propLCInfos.Add((object) lcInfo);
      --this.propSearchStep;
      if (this.propSearchStep != 0)
        return;
      this.Fire_LCsFound(this.propLCInfos);
    }

    protected void OnCBFindBRIPc(int appID, int appSubId)
    {
      this.propBRIPCState = BuRIPCStates.RunningOnABuRIPC;
      --this.propSearchStep;
      if (this.propSearchStep != 0)
        return;
      this.Fire_LCsFound((ArrayList) null);
    }

    private void Fire_LCsFound(ArrayList lcInfos)
    {
      if (this.Found == null)
        return;
      this.Found((object) this, new LCEventArgs(lcInfos));
    }

    public int Search()
    {
      int num1 = 0;
      this.propSearchStep = 3;
      this.propLCInfos.Clear();
      try
      {
        if (BRLicenseInfo.Br_FindBrIPC(this.callBackFindBRIpc, IntPtr.Zero, 0L, 0L) == 0)
        {
          this.propBRIPCState = BuRIPCStates.NoBuRIPC;
          this.OnCBFindKeys((LCInfo) null);
        }
        int num2 = BRLicenseInfo.Br_ReadDsKey(this.callBackFindKeys, IntPtr.Zero);
        if (0 < num2)
          num1 += num2;
        else
          this.OnCBFindKeys((LCInfo) null);
        int num3 = BRLicenseInfo.Br_ReadLcMod(this.callBackFindMods, IntPtr.Zero, (string) null);
        if (0 < num3)
          num1 += num3;
        else
          this.OnCBFindKeys((LCInfo) null);
      }
      catch
      {
        num1 = -1;
      }
      return num1;
    }

    public int Search(string moduleFileName)
    {
      int num1 = 0;
      this.propSearchStep = 3;
      this.propLCInfos.Clear();
      try
      {
        if (BRLicenseInfo.Br_FindBrIPC(this.callBackFindBRIpc, IntPtr.Zero, 0L, 0L) == 0)
        {
          this.propBRIPCState = BuRIPCStates.NoBuRIPC;
          this.OnCBFindKeys((LCInfo) null);
        }
        int num2 = BRLicenseInfo.Br_ReadDsKey(this.callBackFindKeys, IntPtr.Zero);
        if (0 < num2)
          num1 += num2;
        else
          this.OnCBFindKeys((LCInfo) null);
        int num3 = BRLicenseInfo.Br_ReadLcMod(this.callBackFindMods, IntPtr.Zero, moduleFileName);
        if (0 < num3)
          num1 += num3;
        else
          this.OnCBFindKeys((LCInfo) null);
      }
      catch
      {
        num1 = -1;
      }
      return num1;
    }

    public bool SearchBuRIpc()
    {
      try
      {
        if (BRLicenseInfo.Br_FindBrIPC(this.callBackFindBRIpc, IntPtr.Zero, -1L, -1L) == 0)
        {
          this.propBRIPCState = BuRIPCStates.NoBuRIPC;
          return false;
        }
      }
      catch
      {
        return false;
      }
      return true;
    }
  }
}
