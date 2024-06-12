// Decompiled with JetBrains decompiler
// Type: BR.AN.PviServices.PviCBEvents
// Assembly: BR.AN.PVIServices, Version=1.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DA853F0A-C070-4E30-B7A7-51D955FB85F6
// Assembly location: C:\Users\ss_six\Desktop\FXE\FXE software integration package\COMET_FXE-PVIServices_Example\Pluto-PVI-trial_01\packages\IToolS.BandR.3.5.58\lib\net20\BR.AN.PVIServices.dll

using System;

namespace BR.AN.PviServices
{
  public abstract class PviCBEvents : PInvokePvicom
  {
    internal virtual void OnPviCreated(int errorCode, uint linkID)
    {
    }

    internal virtual void OnPviLinked(int errorCode, uint linkID, int option)
    {
    }

    internal virtual void OnPviEvent(
      int errorCode,
      EventTypes eventType,
      PVIDataStates dataState,
      IntPtr pData,
      uint dataLen,
      int option)
    {
    }

    internal virtual void OnPviRead(
      int errorCode,
      PVIReadAccessTypes accessType,
      PVIDataStates dataState,
      IntPtr pData,
      uint dataLen,
      int option)
    {
    }

    internal virtual void OnPviWritten(
      int errorCode,
      PVIWriteAccessTypes accessType,
      PVIDataStates dataState,
      int option,
      IntPtr pData,
      uint dataLen)
    {
    }

    internal virtual void OnPviUnLinked(int errorCode, int option)
    {
    }

    internal virtual void OnPviDeleted(int errorCode)
    {
    }

    internal virtual void OnPviChangedLink(int errorCode)
    {
    }

    internal virtual void OnPviCancelled(int errorCode, int type)
    {
    }
  }
}
