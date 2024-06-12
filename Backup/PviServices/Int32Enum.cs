// Decompiled with JetBrains decompiler
// Type: BR.AN.PviServices.Int32Enum
// Assembly: BR.AN.PVIServices, Version=1.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DA853F0A-C070-4E30-B7A7-51D955FB85F6
// Assembly location: C:\Users\ss_six\Desktop\FXE\FXE software integration package\COMET_FXE-PVIServices_Example\Pluto-PVI-trial_01\packages\IToolS.BandR.3.5.58\lib\net20\BR.AN.PVIServices.dll

namespace BR.AN.PviServices
{
  public class Int32Enum : EnumBase
  {
    internal Int32Enum(params string[] values)
      : base((string) null, (object) null)
    {
      if (2 < values.Length)
      {
        this.propName = values[2];
        this.propValue = (object) System.Convert.ToInt32(values[1]);
      }
      else
      {
        if (1 >= values.Length)
          return;
        this.propName = values[1];
      }
    }

    internal override void SetEnumValue(object value) => this.propValue = (object) (1 + (int) value);

    public override string ToString() => "e," + this.propValue.ToString() + "," + this.propName;
  }
}
