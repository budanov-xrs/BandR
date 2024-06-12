// Decompiled with JetBrains decompiler
// Type: BR.AN.PviServices.SyncReadData
// Assembly: BR.AN.PVIServices, Version=1.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DA853F0A-C070-4E30-B7A7-51D955FB85F6
// Assembly location: C:\Users\ss_six\Desktop\FXE\FXE software integration package\COMET_FXE-PVIServices_Example\Pluto-PVI-trial_01\packages\IToolS.BandR.3.5.58\lib\net20\BR.AN.PVIServices.dll

using System;

namespace BR.AN.PviServices
{
  public class SyncReadData
  {
    private IntPtr propPtrData;
    private int propDataLength;
    private IntPtr propPtrArgData;
    private int propArgDataLength;

    public SyncReadData(int dataLength)
    {
      this.propPtrData = IntPtr.Zero;
      if (0 < dataLength)
        this.propPtrData = PviMarshal.AllocHGlobal(dataLength);
      this.propDataLength = dataLength;
      this.propPtrArgData = IntPtr.Zero;
      this.propArgDataLength = 0;
    }

    public void FreeBuffers() => PviMarshal.FreeHGlobal(ref this.propPtrData);

    public IntPtr PtrData
    {
      get => this.propPtrData;
      set => this.propPtrData = value;
    }

    public int DataLength
    {
      get => this.propDataLength;
      set => this.propDataLength = value;
    }

    public IntPtr PtrArgData
    {
      get => this.propPtrArgData;
      set => this.propPtrArgData = value;
    }

    public int ArgDataLength
    {
      get => this.propArgDataLength;
      set => this.propArgDataLength = value;
    }
  }
}
