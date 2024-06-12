// Decompiled with JetBrains decompiler
// Type: BR.AN.PviServices.ResponseInfo
// Assembly: BR.AN.PVIServices, Version=1.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DA853F0A-C070-4E30-B7A7-51D955FB85F6
// Assembly location: C:\Users\ss_six\Desktop\FXE\FXE software integration package\COMET_FXE-PVIServices_Example\Pluto-PVI-trial_01\packages\IToolS.BandR.3.5.58\lib\net20\BR.AN.PVIServices.dll

using System;
using System.Runtime.InteropServices;

namespace BR.AN.PviServices
{
  [StructLayout(LayoutKind.Sequential, Pack = 1)]
  public struct ResponseInfo
  {
    [CLSCompliant(false)]
    public uint LinkId;
    public int Mode;
    public int Type;
    public int Error;
    public int Status;

    public ResponseInfo(int linkID, int mode, int type, int error, int status)
    {
      this.LinkId = (uint) linkID;
      this.Mode = mode;
      this.Type = type;
      this.Error = error;
      this.Status = status;
    }
  }
}
