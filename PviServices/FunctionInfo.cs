// Decompiled with JetBrains decompiler
// Type: BR.AN.PviServices.FunctionInfo
// Assembly: BR.AN.PVIServices, Version=1.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DA853F0A-C070-4E30-B7A7-51D955FB85F6
// Assembly location: C:\Users\ss_six\Desktop\FXE\FXE software integration package\COMET_FXE-PVIServices_Example\Pluto-PVI-trial_01\packages\IToolS.BandR.3.5.58\lib\net20\BR.AN.PVIServices.dll

using System;

namespace BR.AN.PviServices
{
  public class FunctionInfo
  {
    internal string moduleName;
    internal uint codeOffset;

    internal FunctionInfo()
    {
    }

    internal FunctionInfo(ExceptionBRModuleFunction functionInfo)
    {
      this.moduleName = functionInfo.brmName;
      this.codeOffset = functionInfo.codeOffset;
    }

    public override string ToString() => "CodeOffset=\"0x" + string.Format("{0:X8}", (object) this.codeOffset) + "\" ModuleName=\"" + this.moduleName.ToString() + "\"";

    internal virtual string ToStringHTM() => string.Format("<tr>\r\n<td align=\"left\" valign=\"top\">FunctionInfo</td>\r\n<td >\r\n<table border=\"1\" cellpadding=\"2\" cellspacing=\"1\" bordercolorlight=\"#C0C0C0\" bordercolordark=\"#808080\" style=\"border-collapse: collapse\">{0}{1}\r\n", (object) string.Format("<tr>\r\n<td>ModuleName</td>\r\n<td>{0}</td>\r\n</tr>", (object) this.ModuleName), (object) string.Format("<tr>\r\n<td>CodeOffset</td>\r\n<td>{0}</td>\r\n</tr>", (object) this.CodeOffset)) + "</table>\r\n</td>\r\n</tr>\r\n";

    internal virtual string ToStringCSV() => string.Format("\"{0}\";\"{1:X8}\";", this.moduleName != null ? (object) this.moduleName : (object) "", (object) this.CodeOffset);

    public string ModuleName => this.moduleName;

    [CLSCompliant(false)]
    public uint CodeOffset => this.codeOffset;
  }
}
