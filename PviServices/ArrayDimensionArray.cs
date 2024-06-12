// Decompiled with JetBrains decompiler
// Type: BR.AN.PviServices.ArrayDimensionArray
// Assembly: BR.AN.PVIServices, Version=1.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DA853F0A-C070-4E30-B7A7-51D955FB85F6
// Assembly location: C:\Users\ss_six\Desktop\FXE\FXE software integration package\COMET_FXE-PVIServices_Example\Pluto-PVI-trial_01\packages\IToolS.BandR.3.5.58\lib\net20\BR.AN.PVIServices.dll

using System.Collections;

namespace BR.AN.PviServices
{
  public class ArrayDimensionArray
  {
    private ArrayList propItems;

    internal ArrayDimensionArray() => this.propItems = new ArrayList();

    internal void Clear() => this.propItems.Clear();

    internal ArrayDimensionArray Clone()
    {
      ArrayDimensionArray arrayDimensionArray = new ArrayDimensionArray();
      for (int index = 0; index < this.propItems.Count; ++index)
        arrayDimensionArray.Add(new ArrayDimension((ArrayDimension) this.propItems[index]));
      return arrayDimensionArray;
    }

    internal int Add(params string[] dims)
    {
      if (2 < dims.Length)
        return this.propItems.Add((object) new ArrayDimension(System.Convert.ToInt32(dims.GetValue(2).ToString()), System.Convert.ToInt32(dims.GetValue(1).ToString())));
      return 1 < dims.Length ? this.propItems.Add((object) new ArrayDimension(System.Convert.ToInt32(dims.GetValue(1).ToString()))) : -1;
    }

    internal int Add(ArrayDimension arrayDimItem) => this.propItems.Add((object) arrayDimItem);

    public int Count => this.propItems.Count;

    public ArrayDimension this[int index] => (ArrayDimension) this.propItems[index];

    public override string ToString()
    {
      string str = "";
      for (int index = 0; index < this.propItems.Count; ++index)
        str = index != 0 ? str + ";" + this.propItems[index].ToString() : this.propItems[index].ToString();
      return str;
    }
  }
}
