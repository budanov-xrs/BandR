// Decompiled with JetBrains decompiler
// Type: BR.AN.PviServices.DerivationBase
// Assembly: BR.AN.PVIServices, Version=1.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DA853F0A-C070-4E30-B7A7-51D955FB85F6
// Assembly location: C:\Users\ss_six\Desktop\FXE\FXE software integration package\COMET_FXE-PVIServices_Example\Pluto-PVI-trial_01\packages\IToolS.BandR.3.5.58\lib\net20\BR.AN.PVIServices.dll

namespace BR.AN.PviServices
{
  public class DerivationBase
  {
    private string propName;
    private DataType propBasicType;
    protected DerivationBase propDerivedFrom;

    internal DerivationBase(string typeName, DataType basicType)
    {
      this.propName = typeName;
      this.propBasicType = basicType;
      this.propDerivedFrom = (DerivationBase) null;
    }

    public string Name => this.propName;

    public DataType DataType => this.propBasicType;

    public DerivationBase DerivedFrom => this.propDerivedFrom;

    internal void SetDerivation(DerivationBase derivation) => this.propDerivedFrom = derivation;

    public virtual string ToPviString() => this.DerivationPath() != null && 0 < this.DerivationPath().Length ? "VS=" + this.DerivationParameters() + " TN=" + this.DerivationPath() : "VS=" + this.DerivationParameters();

    public string DerivationPath() => this.propDerivedFrom == null ? this.propName : this.propName + "," + this.propDerivedFrom.DerivationPath();

    public virtual string DerivationParameters() => this.propDerivedFrom != null ? "v;" + this.propDerivedFrom.DerivationParameters() : "v";

    public override string ToString() => this.ToPviString();
  }
}
