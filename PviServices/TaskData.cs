// Decompiled with JetBrains decompiler
// Type: BR.AN.PviServices.TaskData
// Assembly: BR.AN.PVIServices, Version=1.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DA853F0A-C070-4E30-B7A7-51D955FB85F6
// Assembly location: C:\Users\ss_six\Desktop\FXE\FXE software integration package\COMET_FXE-PVIServices_Example\Pluto-PVI-trial_01\packages\IToolS.BandR.3.5.58\lib\net20\BR.AN.PVIServices.dll

using System;
using System.Collections;

namespace BR.AN.PviServices
{
  public class TaskData
  {
    internal uint propId;
    internal uint propPriority;
    internal string propName;
    internal uint propStackBegin;
    internal uint propStackEnd;
    internal uint propStackSize;
    internal uint propRegisterEax;
    internal uint propRegisterEbx;
    internal uint propRegisterEcx;
    internal uint propRegisterEdx;
    internal uint propRegisterEsi;
    internal uint propRegisterEdi;
    internal uint propRegisterEip;
    internal uint propRegisterEsp;
    internal uint propRegisterEbp;
    internal uint propRegisterEflags;

    internal TaskData()
    {
    }

    internal TaskData(ExceptionTaskInfo taskInfo)
    {
      this.propId = taskInfo.taskId;
      this.propPriority = taskInfo.taskPrio;
      this.propName = taskInfo.taskName;
      this.propStackBegin = taskInfo.stackBottom;
      this.propStackEnd = taskInfo.stackEnd;
      this.propStackSize = taskInfo.stackSize;
      this.propRegisterEax = taskInfo.eax;
      this.propRegisterEbx = taskInfo.ebx;
      this.propRegisterEcx = taskInfo.ecx;
      this.propRegisterEdx = taskInfo.edx;
      this.propRegisterEsi = taskInfo.esi;
      this.propRegisterEdi = taskInfo.edi;
      this.propRegisterEip = taskInfo.eip;
      this.propRegisterEsp = taskInfo.esp;
      this.propRegisterEbp = taskInfo.ebp;
      this.propRegisterEflags = taskInfo.eflags;
    }

    public override string ToString() => "ID=\"" + this.propId.ToString() + "\" Name=\"" + this.propName.ToString() + "\" Priority=\"" + this.propPriority.ToString() + "\" Eax=\"" + this.propRegisterEax.ToString() + "\" Ebp=\"" + this.propRegisterEbp.ToString() + "\" Ebx=\"" + this.propRegisterEbx.ToString() + "\" Ecx=\"" + this.propRegisterEcx.ToString() + "\" Edi=\"" + this.propRegisterEdi.ToString() + "\" Edx=\"" + this.propRegisterEdx.ToString() + "\" Eflags=\"" + this.propRegisterEflags.ToString() + "\" Eip=\"" + this.propRegisterEip.ToString() + "\" Esi=\"" + this.propRegisterEsi.ToString() + "\" Esp=\"" + this.propRegisterEsp.ToString() + "\" StackBegin=\"" + this.propStackBegin.ToString() + "\" StackEnd=\"" + this.propStackEnd.ToString() + "\" StackSize=\"" + this.propStackSize.ToString() + "\"";

    internal virtual string ToStringHTM()
    {
      string stringHtm = "";
      ArrayList arrayList = new ArrayList();
      arrayList.Add((object) ("<tr><td align=\"left\" valign=\"top\" bordercolor=\"#C0C0C0\">TaskData</td>" + "<td style=\"border-collapse: collapse\" bordercolor=\"#C0C0C0\"><table border=\"1\" cellpadding=\"2\" cellspacing=\"1\" " + "style=\"border-collapse: collapse\" bordercolor=\"#FFFFFF\" id=\"AutoNumber3\" bordercolorlight=\"#C0C0C0\" bordercolordark=\"#808080\">"));
      arrayList.Add((object) string.Format("<tr><td align=\"left\" valign=\"top\">Id</td><td>{0}</td></tr>", (object) this.Id));
      arrayList.Add((object) string.Format("<tr><td align=\"left\" valign=\"top\">Priority</td><td>{0}</td></tr>", (object) this.Priority));
      arrayList.Add((object) string.Format("<tr><td align=\"left\" valign=\"top\">Name</td><td>{0}</td></tr>", (object) this.Name));
      arrayList.Add((object) string.Format("<tr><td align=\"left\" valign=\"top\">StackBegin</td><td>{0}</td></tr>", (object) this.StackBegin));
      arrayList.Add((object) string.Format("<tr><td align=\"left\" valign=\"top\">StackEnd</td><td>{0}</td></tr>", (object) this.StackEnd));
      arrayList.Add((object) string.Format("<tr><td align=\"left\" valign=\"top\">StackSize</td><td>{0}</td></tr>", (object) this.StackSize));
      arrayList.Add((object) string.Format("<tr><td align=\"left\" valign=\"top\">RegisterEAX</td><td>{0}</td></tr>", (object) this.RegisterEAX));
      arrayList.Add((object) string.Format("<tr><td align=\"left\" valign=\"top\">RegisterEBX</td><td>{0}</td></tr>", (object) this.RegisterEBX));
      arrayList.Add((object) string.Format("<tr><td align=\"left\" valign=\"top\">RegisterECX</td><td>{0}</td></tr>", (object) this.RegisterECX));
      arrayList.Add((object) string.Format("<tr><td align=\"left\" valign=\"top\">RegisterEDX</td><td>{0}</td></tr>", (object) this.RegisterEDX));
      arrayList.Add((object) string.Format("<tr><td align=\"left\" valign=\"top\">RegisterESI</td><td>{0}</td></tr>", (object) this.RegisterESI));
      arrayList.Add((object) string.Format("<tr><td align=\"left\" valign=\"top\">RegisterEDI</td><td>{0}</td></tr>", (object) this.RegisterEDI));
      arrayList.Add((object) string.Format("<tr><td align=\"left\" valign=\"top\">RegisterEIP</td><td>{0}</td></tr>", (object) this.RegisterEIP));
      arrayList.Add((object) string.Format("<tr><td align=\"left\" valign=\"top\">RegisterESP</td><td>{0}</td></tr>", (object) this.RegisterESP));
      arrayList.Add((object) string.Format("<tr><td align=\"left\" valign=\"top\">RegisterEBP</td><td>{0}</td></tr>", (object) this.RegisterEBP));
      arrayList.Add((object) string.Format("<tr><td align=\"left\" valign=\"top\">RegisterEFlags</td><td>{0}</td></tr></table>\r\n</td>\r\n</tr>\r\n", (object) this.RegisterEFLAGS));
      for (int index = 0; index < arrayList.Count; ++index)
        stringHtm += arrayList[index].ToString();
      return stringHtm;
    }

    internal virtual string ToStringCSV() => string.Format("\"{0}\";\"{1}\";\"{2}\";\"{3}\";\"{4}\";\"{5}\";\"{6}\";\"{7}\";\"{8}\";\"{9}\";\"{10}\";\"{11}\";\"{12}\";\"{13}\";\"{14}\";\"{15}\";", (object) this.Id, (object) this.Priority, (object) this.Name, (object) this.StackBegin, (object) this.StackEnd, (object) this.StackSize, (object) this.RegisterEAX, (object) this.RegisterEBX, (object) this.RegisterECX, (object) this.RegisterEDX, (object) this.RegisterESI, (object) this.RegisterEDI, (object) this.RegisterEIP, (object) this.RegisterESP, (object) this.RegisterEBP, (object) this.RegisterEFLAGS);

    [CLSCompliant(false)]
    public uint Id => this.propId;

    [CLSCompliant(false)]
    public uint Priority => this.propPriority;

    [CLSCompliant(false)]
    public string Name => this.propName;

    [CLSCompliant(false)]
    public uint StackBegin => this.propStackBegin;

    [CLSCompliant(false)]
    public uint StackEnd => this.propStackEnd;

    [CLSCompliant(false)]
    public uint RegisterEAX => this.propRegisterEax;

    [CLSCompliant(false)]
    public uint RegisterEBX => this.propRegisterEbx;

    [CLSCompliant(false)]
    public uint RegisterECX => this.propRegisterEcx;

    [CLSCompliant(false)]
    public uint RegisterEDX => this.propRegisterEdx;

    [CLSCompliant(false)]
    public uint RegisterESI => this.propRegisterEsi;

    [CLSCompliant(false)]
    public uint RegisterEDI => this.propRegisterEdi;

    [CLSCompliant(false)]
    public uint RegisterEIP => this.propRegisterEip;

    [CLSCompliant(false)]
    public uint RegisterEBP => this.propRegisterEbp;

    [CLSCompliant(false)]
    public uint RegisterESP => this.propRegisterEsp;

    [CLSCompliant(false)]
    public uint RegisterEFLAGS => this.propRegisterEflags;

    [CLSCompliant(false)]
    public uint StackSize => this.propStackSize;
  }
}
