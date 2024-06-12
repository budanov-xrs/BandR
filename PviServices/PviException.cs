// Decompiled with JetBrains decompiler
// Type: BR.AN.PviServices.PviException
// Assembly: BR.AN.PVIServices, Version=1.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DA853F0A-C070-4E30-B7A7-51D955FB85F6
// Assembly location: C:\Users\ss_six\Desktop\FXE\FXE software integration package\COMET_FXE-PVIServices_Example\Pluto-PVI-trial_01\packages\IToolS.BandR.3.5.58\lib\net20\BR.AN.PVIServices.dll

namespace BR.AN.PviServices
{
  public class PviException : System.Exception
  {
    private int error;
    private object sender;
    private PviEventArgs pviEvent;

    public PviException(string message, int error, object sender, PviEventArgs pviEvent)
      : base(message)
    {
      this.error = error;
      this.sender = sender;
      this.pviEvent = pviEvent;
    }

    public virtual int Error
    {
      get => this.error;
      set => this.error = value;
    }

    public virtual object Sender => this.sender;

    public virtual PviEventArgs PlcEvent => this.pviEvent;
  }
}
