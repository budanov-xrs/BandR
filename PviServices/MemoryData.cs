// Decompiled with JetBrains decompiler
// Type: BR.AN.PviServices.MemoryData
// Assembly: BR.AN.PVIServices, Version=1.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DA853F0A-C070-4E30-B7A7-51D955FB85F6
// Assembly location: C:\Users\ss_six\Desktop\FXE\FXE software integration package\COMET_FXE-PVIServices_Example\Pluto-PVI-trial_01\packages\IToolS.BandR.3.5.58\lib\net20\BR.AN.PVIServices.dll

using System.Collections;

namespace BR.AN.PviServices
{
  public class MemoryData
  {
    internal byte[] propMemPc;
    internal byte[] propMemESP;

    internal MemoryData()
    {
    }

    internal MemoryData(ExceptionMemoryInfo memoryInfo)
    {
      this.propMemPc = memoryInfo.memPc;
      this.propMemESP = memoryInfo.memEsp;
    }

    public override string ToString()
    {
      string str1 = "";
      for (int index = 0; index < this.propMemESP.GetLength(0); ++index)
        str1 += string.Format("{0:X2}", this.propMemESP.GetValue(index));
      string str2 = "";
      for (int index = 0; index < this.propMemPc.GetLength(0); ++index)
        str2 += string.Format("{0:X2}", this.propMemPc.GetValue(index));
      return "ESP=\"" + str1 + "\" PC=\"" + str2 + "\"";
    }

    private string PCToString()
    {
      string str = "";
      for (int index = 0; index < this.propMemPc.GetLength(0); ++index)
        str += string.Format("{0:X2}", this.propMemPc.GetValue(index));
      return str;
    }

    private string ESPToString()
    {
      string str = "";
      for (int index = 0; index < this.propMemESP.GetLength(0); ++index)
        str += string.Format("{0:X2}", this.propMemESP.GetValue(index));
      return str;
    }

    internal virtual string ToStringHTM()
    {
      string stringHtm = "";
      ArrayList arrayList = new ArrayList();
      arrayList.Add((object) ("<tr><td align=\"left\" valign=\"top\" bordercolor=\"#C0C0C0\">MemoryData</td>" + "<td bordercolor=\"#C0C0C0\" style=\"border-collapse: collapse\"><table border=\"1\" cellpadding=\"2\" cellspacing=\"1\" " + "style=\"border-collapse: collapse\" bordercolor=\"#FFFFFF\" id=\"AutoNumber4\" bordercolorlight=\"#C0C0C0\" bordercolordark=\"#808080\">"));
      arrayList.Add((object) string.Format("<tr><td align=\"left\" valign=\"top\">PC</td><td>{0}</td></tr>", (object) this.PCToString()));
      arrayList.Add((object) string.Format("<tr><td align=\"left\" valign=\"top\">ESP</td><td>{0}</td></tr></table>\r\n</td>\r\n</tr>\r\n", (object) this.ESPToString()));
      for (int index = 0; index < arrayList.Count; ++index)
        stringHtm += arrayList[index].ToString();
      return stringHtm;
    }

    internal virtual string ToStringCSV() => "\"" + this.PCToString() + "\";\"" + this.ESPToString() + "\"";

    public byte[] PC => this.propMemPc;

    public byte[] ESP => this.propMemESP;
  }
}
