// Decompiled with JetBrains decompiler
// Type: BR.AN.PviServices.ArrayDerivation
// Assembly: BR.AN.PVIServices, Version=1.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DA853F0A-C070-4E30-B7A7-51D955FB85F6
// Assembly location: C:\Users\ss_six\Desktop\FXE\FXE software integration package\COMET_FXE-PVIServices_Example\Pluto-PVI-trial_01\packages\IToolS.BandR.3.5.58\lib\net20\BR.AN.PVIServices.dll

namespace BR.AN.PviServices
{
  public class ArrayDerivation : DerivationBase
  {
    private int propMinIndex;
    private int propMaxIndex;

    internal ArrayDerivation(string typeName, DataType basicType, ArrayDimension arrayDim)
      : base(typeName, basicType)
    {
      this.propMinIndex = arrayDim.StartIndex;
      this.propMaxIndex = arrayDim.EndIndex;
    }

    public int MinIndex => this.propMinIndex;

    public int MaxIndex => this.propMaxIndex;

    public override string DerivationParameters()
    {
      string str;
      if (this.propDerivedFrom == null)
        str = "v;a," + this.propMinIndex.ToString() + "," + this.propMaxIndex.ToString();
      else
        str = "v;a," + this.propMinIndex.ToString() + "," + this.propMaxIndex.ToString() + ";" + this.propDerivedFrom.DerivationParameters();
      return str;
    }
  }
}
