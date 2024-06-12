// Decompiled with JetBrains decompiler
// Type: BR.AN.PviServices.Exception
// Assembly: BR.AN.PVIServices, Version=1.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DA853F0A-C070-4E30-B7A7-51D955FB85F6
// Assembly location: C:\Users\ss_six\Desktop\FXE\FXE software integration package\COMET_FXE-PVIServices_Example\Pluto-PVI-trial_01\packages\IToolS.BandR.3.5.58\lib\net20\BR.AN.PVIServices.dll

using System;

namespace BR.AN.PviServices
{
  public class Exception
  {
    internal uint propBacktraceCount;
    internal uint propDataLength;
    internal string propARVersion;
    internal ProcessorData propProcessorData;
    internal TaskData propTaskData;
    internal MemoryData propMemoryData;
    internal Backtrace propBacktrace;
    internal ExceptionType propType;

    public Exception()
    {
    }

    internal Exception(ExceptionHeader header)
    {
      this.propBacktraceCount = header.traceRec;
      this.propDataLength = header.excInfoSize;
      this.propARVersion = header.arVersion;
    }

    public override string ToString() => "ArVersion=\"" + this.propARVersion + "\" Type=\"" + this.propType.ToString() + "\" DataLength=\"" + this.propDataLength.ToString() + "\" BacktraceCount=\"" + this.propBacktraceCount.ToString() + "\"" + (this.propBacktrace != null ? " " + this.propBacktrace.ToString() : "") + (this.propMemoryData != null ? " " + this.propMemoryData.ToString() : "") + (this.propProcessorData != null ? " " + this.propProcessorData.ToString() : "") + (this.propTaskData != null ? " " + this.propTaskData.ToString() : "");

    internal virtual string ToStringHTM() => string.Format("{0}{1}{2}{3}", (object) string.Format("<tr>\r\n<td style=\"border-collapse: collapse\" bordercolor=\"#C0C0C0\">Exception</td>\r\n<td bordercolor=\"#C0C0C0\">{0}</td></tr>\r\n", (object) this.Type), (object) string.Format("<tr>\r\n<td style=\"border-collapse: collapse\" bordercolor=\"#C0C0C0\">BacktraceCount</td>\r\n<td bordercolor=\"#C0C0C0\">{0}</td></tr>\r\n", (object) this.BacktraceCount), (object) string.Format("<tr>\r\n<td style=\"border-collapse: collapse\" bordercolor=\"#C0C0C0\">DataLength</td>\r\n<td bordercolor=\"#C0C0C0\">{0}</td></tr>\r\n", (object) this.DataLength), (object) string.Format("<tr>\r\n<td style=\"border-collapse: collapse\" bordercolor=\"#C0C0C0\">ArVersion</td>\r\n<td bordercolor=\"#C0C0C0\">{0}</td></tr>\r\n", (object) this.ArVersion));

    internal virtual string ToStringCSV() => string.Format("\"{0}\";\"{1}\";\"{2}\";\"{3}\";", (object) this.Type, (object) this.BacktraceCount, (object) this.DataLength, (object) this.ArVersion);

    [CLSCompliant(false)]
    public uint BacktraceCount => this.propBacktraceCount;

    [CLSCompliant(false)]
    public uint DataLength => this.propDataLength;

    public string ArVersion => this.propARVersion;

    public ProcessorData ProcessorData => this.propProcessorData;

    public TaskData TaskData => this.propTaskData;

    public MemoryData MemoryData => this.propMemoryData;

    public Backtrace Backtrace => this.propBacktrace;

    public ExceptionType Type => this.propType;
  }
}
