// Decompiled with JetBrains decompiler
// Type: BR.AN.PviServices.Library
// Assembly: BR.AN.PVIServices, Version=1.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DA853F0A-C070-4E30-B7A7-51D955FB85F6
// Assembly location: C:\Users\ss_six\Desktop\FXE\FXE software integration package\COMET_FXE-PVIServices_Example\Pluto-PVI-trial_01\packages\IToolS.BandR.3.5.58\lib\net20\BR.AN.PVIServices.dll

using System;
using System.Collections;
using System.Xml;

namespace BR.AN.PviServices
{
  public class Library : Base
  {
    internal Hashtable propFunctions;
    internal LibraryType propType;
    internal Cpu propCpu;

    public Library(Cpu cpu, string name)
      : base((Base) cpu, name)
    {
      this.propFunctions = new Hashtable();
      this.propCpu = cpu;
      this.propParent = (Base) cpu;
      this.propCpu.Libraries.Add(this);
    }

    public int Disconnect(int internalAction) => this.UnlinkRequest((uint) internalAction);

    public virtual void UploadFunctions()
    {
      string request = "LIB=" + this.propName + " QU=0";
      this.Service.BuildRequestBuffer(request);
      int errorCode = this.ReadArgumentRequest(this.Service.hPvi, this.propParent.LinkId, AccessTypes.LibraryList, this.Service.RequestBuffer, request.Length, 614U, this.Parent.InternId);
      if (errorCode == 0)
        return;
      this.OnError(new PviEventArgs(this.Name, this.Address, errorCode, this.Service.Language, Action.LibFunctionsUpload, this.Service));
    }

    internal override void OnPviRead(
      int errorCode,
      PVIReadAccessTypes accessType,
      PVIDataStates dataState,
      IntPtr pData,
      uint dataLen,
      int option)
    {
      this.propErrorCode = errorCode;
      if (accessType == PVIReadAccessTypes.LibraryList)
      {
        if (this.ErrorCode == 0 && 0U < dataLen)
        {
          string stringAnsi = PviMarshal.PtrToStringAnsi(pData, dataLen);
          if (!(stringAnsi != "") || 1 >= stringAnsi.Length)
            return;
          string[] strArray = stringAnsi.Split("\t".ToCharArray());
          for (int index = 0; index < strArray.Length; ++index)
          {
            string str = strArray[index].Substring(0, strArray[index].IndexOf(" "));
            if (!this.propFunctions.ContainsKey((object) str))
            {
              Function function = new Function(this, str);
              int num = strArray[index].IndexOf("REF=");
              function.propReference = System.Convert.ToByte(strArray[index].Substring(num + 4));
            }
          }
          this.OnFunctionsUploaded(new PviEventArgs(this.propName, "", errorCode, this.Service.Language, Action.LibFunctionsUpload, this.Service));
        }
        else
        {
          if (this.ErrorCode == 0)
            return;
          this.OnFunctionsUploaded(new PviEventArgs(this.propName, "", errorCode, this.Service.Language, Action.LibFunctionsUpload, this.Service));
          this.OnError(new PviEventArgs(this.Name, this.Address, this.ErrorCode, this.Service.Language, Action.LibFunctionsUpload, this.Service));
        }
      }
      else
        base.OnPviRead(errorCode, accessType, dataState, pData, dataLen, option);
    }

    protected virtual void OnFunctionsUploaded(PviEventArgs e)
    {
      if (this.FunctionsUploaded == null)
        return;
      this.FunctionsUploaded((object) this, e);
    }

    internal override int ToXMLTextWriter(ref XmlTextWriter writer, ConfigurationFlags flags)
    {
      base.ToXMLTextWriter(ref writer, flags);
      writer.WriteAttributeString("Type", this.propType.ToString());
      if (this.propFunctions.Count > 0)
      {
        writer.WriteStartElement("Functions");
        foreach (Function function in (IEnumerable) this.propFunctions.Values)
        {
          writer.WriteStartElement("Function");
          writer.WriteAttributeString("Name", function.Name);
          if (function.propReference != (byte) 0)
            writer.WriteAttributeString("Reference", function.Reference.ToString());
          writer.WriteEndElement();
        }
        writer.WriteEndElement();
      }
      return 0;
    }

    public override int FromXmlTextReader(
      ref XmlTextReader reader,
      ConfigurationFlags flags,
      Base baseObj)
    {
      int num = base.FromXmlTextReader(ref reader, flags, baseObj);
      Library library = (Library) baseObj;
      if (library == null)
        return -1;
      string str = "";
      string attribute1 = reader.GetAttribute("Type");
      if (attribute1 != null && attribute1.Length > 0)
      {
        switch (attribute1.ToLower())
        {
          case "extern":
            library.propType = LibraryType.Extern;
            break;
          case "intern":
            library.propType = LibraryType.Intern;
            break;
        }
      }
      reader.Read();
      if (reader.Name == "Functions")
      {
        while (reader.Read() && reader.NodeType != XmlNodeType.EndElement)
        {
          string name = "";
          byte result = 0;
          str = "";
          string attribute2 = reader.GetAttribute("Name");
          if (attribute2 != null && attribute2.Length > 0)
            name = attribute2;
          str = "";
          string attribute3 = reader.GetAttribute("Reference");
          if (attribute3 != null && attribute3.Length > 0)
            PviParse.TryParseByte(attribute3, out result);
          if (name != "")
          {
            Function function = new Function(library, name);
          }
        }
      }
      reader.Read();
      if (reader.Name == nameof (Library) && reader.NodeType == XmlNodeType.EndElement)
        reader.Read();
      return num;
    }

    public Hashtable Functions => this.propFunctions;

    public LibraryType Type => this.propType;

    public override string FullName
    {
      get
      {
        if (this.propName != null && 0 < this.Name.Length)
          return this.Parent.FullName + "." + this.Name;
        return this.Parent != null ? this.Parent.FullName : "";
      }
    }

    public override string PviPathName => this.Name != null && 0 < this.Name.Length ? this.Parent.PviPathName + "/\"" + this.propName + "\"" : this.Parent.PviPathName;

    public Cpu Cpu => this.propCpu;

    public event PviEventHandler FunctionsUploaded;
  }
}
