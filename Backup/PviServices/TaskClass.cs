// Decompiled with JetBrains decompiler
// Type: BR.AN.PviServices.TaskClass
// Assembly: BR.AN.PVIServices, Version=1.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DA853F0A-C070-4E30-B7A7-51D955FB85F6
// Assembly location: C:\Users\ss_six\Desktop\FXE\FXE software integration package\COMET_FXE-PVIServices_Example\Pluto-PVI-trial_01\packages\IToolS.BandR.3.5.58\lib\net20\BR.AN.PVIServices.dll

using System;
using System.Xml;

namespace BR.AN.PviServices
{
  public class TaskClass : IDisposable
  {
    private string propName;
    internal bool propDisposed;
    internal object propUserData;
    private TaskClassType propNumber;
    private ProgramState propState;

    public event DisposeEventHandler Disposing;

    internal virtual void OnDisposing(bool disposing)
    {
      if (this.Disposing == null)
        return;
      this.Disposing((object) this, new DisposeEventArgs(disposing));
    }

    public void Dispose()
    {
      if (this.propDisposed)
        return;
      this.Dispose(true, true);
      GC.SuppressFinalize((object) this);
    }

    internal virtual void Dispose(bool disposing, bool removeFromCollection)
    {
      if (this.propDisposed)
        return;
      this.OnDisposing(disposing);
      if (!disposing)
        return;
      this.propDisposed = true;
      this.propName = (string) null;
      this.propUserData = (object) null;
    }

    internal TaskClass(APIFC_TkInfoRes taskClassInfo)
    {
      this.propDisposed = false;
      this.propName = taskClassInfo.name.ToString();
      this.propNumber = taskClassInfo.number;
      this.propState = taskClassInfo.state;
    }

    internal int ToXMLTextWriter(ref XmlTextWriter writer, ConfigurationFlags flags)
    {
      writer.WriteStartElement(nameof (TaskClass));
      writer.WriteAttributeString("Name", this.propName.ToString());
      if (this.propState != ProgramState.Running)
        writer.WriteAttributeString("State", this.propState.ToString());
      writer.WriteAttributeString("Type", this.propNumber.ToString());
      writer.WriteEndElement();
      return 0;
    }

    internal static int FromXmlTextReader(
      ref XmlTextReader reader,
      ConfigurationFlags flags,
      ref APIFC_TkInfoRes taskClassInfo)
    {
      string str = "";
      string attribute1 = reader.GetAttribute("State");
      if (attribute1 != null && attribute1.Length > 0)
      {
        switch (attribute1.ToLower())
        {
          case "idle":
            taskClassInfo.state = ProgramState.Idle;
            break;
          case "nonexistent":
            taskClassInfo.state = ProgramState.NonExistent;
            break;
          case "resetting":
            taskClassInfo.state = ProgramState.Resetting;
            break;
          case "resuming":
            taskClassInfo.state = ProgramState.Resuming;
            break;
          case "running":
            taskClassInfo.state = ProgramState.Running;
            break;
          case "starting":
            taskClassInfo.state = ProgramState.Starting;
            break;
          case "stopped":
            taskClassInfo.state = ProgramState.Stopped;
            break;
          case "stopping":
            taskClassInfo.state = ProgramState.Stopping;
            break;
          case "unrunnable":
            taskClassInfo.state = ProgramState.Unrunnable;
            break;
        }
      }
      string attribute2 = reader.GetAttribute("Type");
      if (attribute2 != null && attribute2.Length > 0)
      {
        switch (attribute2.ToLower())
        {
          case "cyclic1":
            taskClassInfo.number = TaskClassType.Cyclic1;
            break;
          case "cyclic2":
            taskClassInfo.number = TaskClassType.Cyclic2;
            break;
          case "cyclic3":
            taskClassInfo.number = TaskClassType.Cyclic3;
            break;
          case "cyclic4":
            taskClassInfo.number = TaskClassType.Cyclic4;
            break;
          case "cyclic5":
            taskClassInfo.number = TaskClassType.Cyclic5;
            break;
          case "cyclic6":
            taskClassInfo.number = TaskClassType.Cyclic6;
            break;
          case "cyclic7":
            taskClassInfo.number = TaskClassType.Cyclic7;
            break;
          case "cyclic8":
            taskClassInfo.number = TaskClassType.Cyclic8;
            break;
          case "timer1":
            taskClassInfo.number = TaskClassType.Timer1;
            break;
          case "timer2":
            taskClassInfo.number = TaskClassType.Timer2;
            break;
          case "timer3":
            taskClassInfo.number = TaskClassType.Timer3;
            break;
          case "timer4":
            taskClassInfo.number = TaskClassType.Timer4;
            break;
          case "exception":
            taskClassInfo.number = TaskClassType.Exception;
            break;
          case "interrupt":
            taskClassInfo.number = TaskClassType.Interrupt;
            break;
          case "notvalid":
            taskClassInfo.number = TaskClassType.NotValid;
            break;
        }
      }
      str = "";
      string attribute3 = reader.GetAttribute("Name");
      if (attribute3 != null && attribute3.Length > 0)
        taskClassInfo.name = attribute3;
      reader.Read();
      return 0;
    }

    public string Name => this.propName;

    public TaskClassType Type => this.propNumber;

    public ProgramState State => this.propState;

    public object UserData
    {
      get => this.propUserData;
      set => this.propUserData = value;
    }
  }
}
