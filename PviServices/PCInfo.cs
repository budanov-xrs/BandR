// Decompiled with JetBrains decompiler
// Type: BR.AN.PviServices.PCInfo
// Assembly: BR.AN.PVIServices, Version=1.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DA853F0A-C070-4E30-B7A7-51D955FB85F6
// Assembly location: C:\Users\ss_six\Desktop\FXE\FXE software integration package\COMET_FXE-PVIServices_Example\Pluto-PVI-trial_01\packages\IToolS.BandR.3.5.58\lib\net20\BR.AN.PVIServices.dll

using System;

namespace BR.AN.PviServices
{
  public class PCInfo
  {
    internal string propModuleName;
    internal uint propCodeOffset;

    internal PCInfo()
    {
    }

    internal PCInfo(ExceptionBRModulePC pcInfo)
    {
      this.propModuleName = pcInfo.brmName;
      this.propCodeOffset = pcInfo.codeOffset;
    }

    public override string ToString() => "CodeOffset=\"0x" + string.Format("{0:X8}", (object) this.propCodeOffset) + (this.propModuleName != null ? "\" ModuleName=\"" + this.propModuleName : "") + "\"";

    internal virtual string ToStringHTM() => string.Format("<tr>\r\n<td align=\"left\" valign=\"top\">PCInfo</td>\r\n<td >\r\n<table border=\"1\" cellpadding=\"2\" cellspacing=\"1\" style=\"border-collapse: collapse\" bordercolorlight=\"#C0C0C0\" bordercolordark=\"#808080\">{0}{1}\r\n", (object) string.Format("<tr>\r\n<td>ModuleName</td>\r\n<td>{0}</td>\r\n</tr>", (object) this.ModuleName), (object) string.Format("<tr>\r\n<td>ModuleName</td>\r\n<td>{0}</td>\r\n</tr>", (object) this.CodeOffset)) + "</table>\r\n</td>\r\n</tr>\r\n";

    internal virtual string ToStringCSV() => string.Format("\"{0}\";\"{1:X8}\";", this.propModuleName != null ? (object) this.propModuleName : (object) "", (object) this.CodeOffset);

    public bool ReplaceModuleName(string pModuleName)
    {
      if (this.propModuleName == null || 0 >= this.propModuleName.Length || pModuleName == null || 0 >= pModuleName.Length)
        return false;
      this.propModuleName = pModuleName;
      return true;
    }

    public string ModuleName => this.propModuleName;

    [CLSCompliant(false)]
    public uint CodeOffset => this.propCodeOffset;
  }
}
