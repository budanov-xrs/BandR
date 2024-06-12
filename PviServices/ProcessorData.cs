// Decompiled with JetBrains decompiler
// Type: BR.AN.PviServices.ProcessorData
// Assembly: BR.AN.PVIServices, Version=1.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DA853F0A-C070-4E30-B7A7-51D955FB85F6
// Assembly location: C:\Users\ss_six\Desktop\FXE\FXE software integration package\COMET_FXE-PVIServices_Example\Pluto-PVI-trial_01\packages\IToolS.BandR.3.5.58\lib\net20\BR.AN.PVIServices.dll

using System;

namespace BR.AN.PviServices
{
  public class ProcessorData
  {
    internal uint propProgramCounter;
    internal uint propEFlags;
    internal uint propErrorCode;

    internal ProcessorData()
    {
    }

    internal ProcessorData(ExceptionProcessorInfo processorInfo)
    {
      this.propProgramCounter = processorInfo.pc;
      this.propEFlags = processorInfo.eFlags;
      this.propErrorCode = processorInfo.excErrFrameCode;
    }

    public override string ToString() => "EFlags=\"" + this.propEFlags.ToString() + "\" ErrorCode=\"" + this.propErrorCode.ToString() + "\" ProgramCounter=\"0x" + string.Format("{0:X8}", (object) this.propProgramCounter) + "\"";

    internal virtual string ToStringHTM() => string.Format("{0}{1}{2}{3}", (object) ("<tr><td align=\"left\" valign=\"top\" bordercolor=\"#C0C0C0\">ProcessorData</td>" + "<td bordercolor=\"#C0C0C0\"><table border=\"1\" cellpadding=\"2\" cellspacing=\"1\" " + "style=\"border-collapse: collapse\" bordercolor=\"#FFFFFF\" id=\"AutoNumber7\" bordercolorlight=\"#C0C0C0\" bordercolordark=\"#808080\">"), (object) string.Format("<tr><td align=\"left\" valign=\"top\">ProgramCounter</td><td>{0}</td></tr>", (object) this.ProgramCounter), (object) string.Format("<tr><td align=\"left\" valign=\"top\">EFlags</td><td>{0}</td></tr>", (object) this.EFlags), (object) string.Format("<tr><td align=\"left\" valign=\"top\">ErrorCode</td><td>{0}</td></tr></table>\r\n</td>\r\n</tr>\r\n", (object) this.ErrorCode));

    internal virtual string ToStringCSV() => string.Format("\"{0}\";\"{1}\";\"{2}\";", (object) this.ProgramCounter, (object) this.EFlags, (object) this.ErrorCode);

    [CLSCompliant(false)]
    public uint ProgramCounter => this.propProgramCounter;

    [CLSCompliant(false)]
    public uint EFlags => this.propEFlags;

    [CLSCompliant(false)]
    public uint ErrorCode => this.propErrorCode;
  }
}
