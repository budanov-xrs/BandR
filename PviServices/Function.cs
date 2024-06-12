// Decompiled with JetBrains decompiler
// Type: BR.AN.PviServices.Function
// Assembly: BR.AN.PVIServices, Version=1.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DA853F0A-C070-4E30-B7A7-51D955FB85F6
// Assembly location: C:\Users\ss_six\Desktop\FXE\FXE software integration package\COMET_FXE-PVIServices_Example\Pluto-PVI-trial_01\packages\IToolS.BandR.3.5.58\lib\net20\BR.AN.PVIServices.dll

namespace BR.AN.PviServices
{
  public class Function
  {
    private string propName;
    private Library propLibrary;
    internal byte propReference;

    public Function(Library library, string name)
    {
      this.propName = name;
      library.Functions.Add((object) name, (object) this);
      this.propLibrary = library;
    }

    public string Name => this.propName;

    public Library Library => this.propLibrary;

    public byte Reference => this.propReference;
  }
}
