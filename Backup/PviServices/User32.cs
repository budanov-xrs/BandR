// Decompiled with JetBrains decompiler
// Type: BR.AN.PviServices.User32
// Assembly: BR.AN.PVIServices, Version=1.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DA853F0A-C070-4E30-B7A7-51D955FB85F6
// Assembly location: C:\Users\ss_six\Desktop\FXE\FXE software integration package\COMET_FXE-PVIServices_Example\Pluto-PVI-trial_01\packages\IToolS.BandR.3.5.58\lib\net20\BR.AN.PVIServices.dll

using Microsoft.Win32.SafeHandles;
using System.Runtime.InteropServices;

namespace BR.AN.PviServices
{
  internal class User32
  {
    private const uint INFINITE = 4294967295;
    private const uint WAIT_ABANDONED = 128;
    private const uint WAIT_OBJECT_0 = 0;
    private const uint WAIT_TIMEOUT = 258;

    [DllImport("kernel32.dll", SetLastError = true)]
    internal static extern uint WaitForSingleObject(SafeWaitHandle hHandle, uint dwMilliseconds);
  }
}
