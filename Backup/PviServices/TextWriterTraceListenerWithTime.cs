// Decompiled with JetBrains decompiler
// Type: BR.AN.PviServices.TextWriterTraceListenerWithTime
// Assembly: BR.AN.PVIServices, Version=1.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DA853F0A-C070-4E30-B7A7-51D955FB85F6
// Assembly location: C:\Users\ss_six\Desktop\FXE\FXE software integration package\COMET_FXE-PVIServices_Example\Pluto-PVI-trial_01\packages\IToolS.BandR.3.5.58\lib\net20\BR.AN.PVIServices.dll

using System;
using System.Diagnostics;
using System.IO;

namespace BR.AN.PviServices
{
  public class TextWriterTraceListenerWithTime : TextWriterTraceListener
  {
    public TextWriterTraceListenerWithTime()
    {
    }

    public TextWriterTraceListenerWithTime(Stream stream)
      : base(stream)
    {
    }

    public TextWriterTraceListenerWithTime(string path)
      : base(path)
    {
    }

    public TextWriterTraceListenerWithTime(TextWriter writer)
      : base(writer)
    {
    }

    public TextWriterTraceListenerWithTime(Stream stream, string name)
      : base(stream, name)
    {
    }

    public TextWriterTraceListenerWithTime(string path, string name)
      : base(path, name)
    {
    }

    public TextWriterTraceListenerWithTime(TextWriter writer, string name)
      : base(writer, name)
    {
    }

    public override void WriteLine(string message)
    {
      this.Write(DateTime.Now.ToString("yyyy:MM:dd HH:mm:ss.ffff "));
      base.WriteLine(message);
    }
  }
}
