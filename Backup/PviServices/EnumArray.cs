// Decompiled with JetBrains decompiler
// Type: BR.AN.PviServices.EnumArray
// Assembly: BR.AN.PVIServices, Version=1.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DA853F0A-C070-4E30-B7A7-51D955FB85F6
// Assembly location: C:\Users\ss_six\Desktop\FXE\FXE software integration package\COMET_FXE-PVIServices_Example\Pluto-PVI-trial_01\packages\IToolS.BandR.3.5.58\lib\net20\BR.AN.PVIServices.dll

using System.Collections;

namespace BR.AN.PviServices
{
  public class EnumArray
  {
    private ArrayList propItems;
    private ArrayList propNames;
    private ArrayList propValues;
    private string propName;

    internal EnumArray(string name)
    {
      this.propItems = new ArrayList();
      this.propNames = new ArrayList();
      this.propValues = new ArrayList();
      this.propName = name;
    }

    internal EnumArray Clone() => new EnumArray(this.propName)
    {
      propItems = (ArrayList) this.propItems.Clone(),
      propNames = (ArrayList) this.propNames.Clone(),
      propValues = (ArrayList) this.propNames.Clone()
    };

    internal void Clear()
    {
      this.propItems.Clear();
      this.propNames.Clear();
      this.propValues.Clear();
    }

    public string Name => this.propName;

    internal int AddEnum(EnumBase enumVal)
    {
      for (int index = 0; index < this.propItems.Count; ++index)
      {
        if (((EnumBase) this.propItems[index]).Name.CompareTo(enumVal.Name) == 0)
          return -1;
      }
      if (enumVal.Value == null)
        enumVal.SetEnumValue(this.propValues[this.propValues.Count - 1]);
      this.propNames.Add((object) enumVal.Name);
      this.propValues.Add(enumVal.Value);
      return this.propItems.Add((object) enumVal);
    }

    public object EnumValue(string name)
    {
      for (int index = 0; index < this.propNames.Count; ++index)
      {
        if (name.CompareTo(this.propNames[index].ToString()) == 0)
          return this.propValues[index];
      }
      return (object) null;
    }

    public string EnumName(object value)
    {
      for (int index = 0; index < this.propValues.Count; ++index)
      {
        if (value.ToString().CompareTo(this.propValues[index].ToString()) == 0)
          return (string) this.propNames[index];
      }
      return (string) null;
    }

    public ArrayList Names => this.propNames;

    public ArrayList Values => this.propValues;

    public EnumBase this[int index] => (EnumBase) this.propItems[index];

    public int Count => this.propItems.Count;

    public virtual string ToPviString()
    {
      string pviString = "";
      for (int index = 0; index < this.propValues.Count; ++index)
      {
        if (index == 0)
          pviString = pviString + "e," + this.propValues[index].ToString() + "," + this.propNames[index].ToString();
        else
          pviString = pviString + ";e," + this.propValues[index].ToString() + "," + this.propNames[index].ToString();
      }
      return pviString;
    }

    public override string ToString() => this.ToPviString();
  }
}
