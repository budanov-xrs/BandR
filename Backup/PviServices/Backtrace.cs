// Decompiled with JetBrains decompiler
// Type: BR.AN.PviServices.Backtrace
// Assembly: BR.AN.PVIServices, Version=1.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DA853F0A-C070-4E30-B7A7-51D955FB85F6
// Assembly location: C:\Users\ss_six\Desktop\FXE\FXE software integration package\COMET_FXE-PVIServices_Example\Pluto-PVI-trial_01\packages\IToolS.BandR.3.5.58\lib\net20\BR.AN.PVIServices.dll

using System;
using System.Collections;

namespace BR.AN.PviServices
{
  public class Backtrace
  {
    internal Backtrace propNextBacktrace;
    internal uint propTaskIdx;
    internal uint propInfo;
    internal uint propParamCount;
    internal uint propAddress;
    internal string propFunctionName;
    internal uint[] propParameter;
    internal Callstack propCallstack;
    internal FunctionInfo propFunctionInfo;
    internal PCInfo propPCInfo;

    internal Backtrace() => this.propTaskIdx = uint.MaxValue;

    internal Backtrace(ExceptionTraceRecord traceRecord)
    {
      this.propTaskIdx = uint.MaxValue;
      this.propInfo = traceRecord.brmInfoFlag;
      this.propParamCount = traceRecord.paramCnt;
      this.propAddress = traceRecord.funcAddr;
      this.propFunctionName = traceRecord.funcName;
      this.propParameter = new uint[(IntPtr) traceRecord.paramCnt];
    }

    public override string ToString()
    {
      string str = "";
      for (int index = 0; (long) index < (long) this.propParamCount; ++index)
        str += string.Format("{0:X8} ", (object) this.propParameter[index]);
      return "Address=\"" + (object) this.propAddress + (this.propCallstack != null ? (object) ("\" CallStack=\"" + this.propCallstack.ToString()) : (object) "") + (this.propFunctionInfo != null ? (object) ("\" FunctionInfo=\"" + this.propFunctionInfo.ToString()) : (object) "") + (this.propFunctionName != null ? (object) ("\" FunctionName=\"" + this.propFunctionName) : (object) "") + "\" Info=\"0x" + string.Format("{0:X8}", (object) this.propInfo) + "\" Parameter=\"" + (0U < this.propParamCount ? (object) str : (object) this.propParamCount.ToString()) + "\"" + (this.propPCInfo != null ? (object) (" " + this.propPCInfo.ToString()) : (object) "") + " TaskIdx=\"0x" + string.Format("{0:X8}", (object) this.propTaskIdx) + "\"";
    }

    internal virtual string ToStringHTM()
    {
      string stringHtm = "";
      string str = "";
      ArrayList arrayList = new ArrayList();
      arrayList.Add((object) string.Format("<tr>\r\n<td align=\"left\" valign=\"top\">Address</td>\r\n<td>{0}</td>\r\n</tr>\r\n", (object) this.Address));
      arrayList.Add((object) string.Format("<tr>\r\n<td align=\"left\" valign=\"top\">FunctionName</td>\r\n<td>{0}</td>\r\n</tr>\r\n", (object) this.FunctionName));
      arrayList.Add((object) string.Format("<tr>\r\n<td align=\"left\" valign=\"top\">Info</td>\r\n<td>{0}</td>\r\n</tr>\r\n", (object) this.Info));
      arrayList.Add((object) string.Format("<tr>\r\n<td align=\"left\" valign=\"top\">TaskIndex</td>\r\n<td>{0}</td>\r\n</tr>\r\n", (object) this.TaskIndex));
      arrayList.Add((object) "<tr>\r\n<td align=\"left\" valign=\"top\">Parameters</td>\r\n<td>\r\n");
      for (int index = 0; (long) index < (long) this.propParamCount; ++index)
        str = index != 0 ? str + string.Format("<br>{0:X8}", (object) this.propParameter[index]) : string.Format("{0:X8}", (object) this.propParameter[index]);
      arrayList.Add((object) (str + "</td>\r\n</tr>\r\n"));
      for (int index = 0; index < arrayList.Count; ++index)
        stringHtm += arrayList[index].ToString();
      return stringHtm;
    }

    internal virtual string ToStringCSV() => string.Format("\"{0}\";\"{1}\";\"{2:X8}\";\"{3:X8}\";", (object) this.Address, (object) this.FunctionName, (object) this.Info, (object) this.TaskIndex);

    public bool ReplaceFunctionName(string pFunctionName)
    {
      if (this.propFunctionName == null || 0 >= this.propFunctionName.Length || pFunctionName == null || 0 >= pFunctionName.Length)
        return false;
      this.propFunctionName = pFunctionName;
      return true;
    }

    public Backtrace NextBacktrace => this.propNextBacktrace;

    [CLSCompliant(false)]
    public uint Paramcount => this.propParamCount;

    [CLSCompliant(false)]
    public uint Address => this.propAddress;

    [CLSCompliant(false)]
    public string FunctionName => this.propFunctionName;

    [CLSCompliant(false)]
    public uint[] Parameter => this.propParameter;

    [CLSCompliant(false)]
    public uint Info => this.propInfo;

    [CLSCompliant(false)]
    public uint TaskIndex => this.propTaskIdx;

    public Callstack Callstack => this.propCallstack;

    public FunctionInfo FunctionInfo => this.propFunctionInfo;

    public PCInfo PCInfo => this.propPCInfo;
  }
}
