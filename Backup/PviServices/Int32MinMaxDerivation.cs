// Decompiled with JetBrains decompiler
// Type: BR.AN.PviServices.Int32MinMaxDerivation
// Assembly: BR.AN.PVIServices, Version=1.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DA853F0A-C070-4E30-B7A7-51D955FB85F6
// Assembly location: C:\Users\ss_six\Desktop\FXE\FXE software integration package\COMET_FXE-PVIServices_Example\Pluto-PVI-trial_01\packages\IToolS.BandR.3.5.58\lib\net20\BR.AN.PVIServices.dll

namespace BR.AN.PviServices
{
  public class Int32MinMaxDerivation : DerivationBase
  {
    private int propMinimum;
    private int propMaximum;

    internal Int32MinMaxDerivation(string typeName, DataType basicType, params string[] values)
      : base(typeName, basicType)
    {
      this.propMinimum = int.MinValue;
      this.propMaximum = int.MaxValue;
      if (2 >= values.Length)
        return;
      this.propMinimum = System.Convert.ToInt32(values.GetValue(1));
      this.propMaximum = System.Convert.ToInt32(values.GetValue(2));
    }

    public int Minimum => this.propMinimum;

    public int Maximum => this.propMaximum;

    public override string DerivationParameters()
    {
      string str;
      if (this.propDerivedFrom == null)
        str = "v," + this.propMinimum.ToString() + "," + this.propMaximum.ToString();
      else
        str = "v," + this.propMinimum.ToString() + "," + this.propMaximum.ToString() + ";" + this.propDerivedFrom.DerivationParameters();
      return str;
    }
  }
}
