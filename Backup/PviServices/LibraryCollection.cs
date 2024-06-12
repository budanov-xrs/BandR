// Decompiled with JetBrains decompiler
// Type: BR.AN.PviServices.LibraryCollection
// Assembly: BR.AN.PVIServices, Version=1.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DA853F0A-C070-4E30-B7A7-51D955FB85F6
// Assembly location: C:\Users\ss_six\Desktop\FXE\FXE software integration package\COMET_FXE-PVIServices_Example\Pluto-PVI-trial_01\packages\IToolS.BandR.3.5.58\lib\net20\BR.AN.PVIServices.dll

using System;
using System.Collections;
using System.Xml;

namespace BR.AN.PviServices
{
  public class LibraryCollection : BaseCollection
  {
    public LibraryCollection(Cpu cpu, string name)
      : base((object) cpu, name)
    {
      this.propCollectionType = CollectionType.HashTable;
    }

    public virtual void Add(Library library) => this.Add((object) library.Name, (object) library);

    public virtual void Upload()
    {
      string request = "LIB=* QU=0";
      this.Service.BuildRequestBuffer(request);
      this.ReadArgumentRequest(this.Service.hPvi, ((Base) this.propParent).LinkId, AccessTypes.LibraryList, this.Service.RequestBuffer, request.Length, 1202U, this.InternId);
    }

    public override void Connect() => this.Connect(ConnectionType.CreateAndLink);

    public override void Connect(ConnectionType connectionType)
    {
      this.Fire_Error((object) this, new PviEventArgs(this.propName, "", 12058, this.Service.Language, Action.LibrariesConnect, this.Service));
      this.OnCollectionConnected(new CollectionEventArgs(this.propName, "", 12058, this.Service.Language, Action.LibrariesConnect, (BaseCollection) null));
    }

    internal override void OnPviRead(
      int errorCode,
      PVIReadAccessTypes accessType,
      PVIDataStates dataState,
      IntPtr pData,
      uint dataLen,
      int option)
    {
      if (PVIReadAccessTypes.LibraryList == accessType)
      {
        if (errorCode == 0 && dataLen > 0U)
        {
          string stringAnsi = PviMarshal.PtrToStringAnsi(pData, dataLen);
          if (stringAnsi != "")
          {
            string[] strArray = stringAnsi.Split("\t".ToCharArray());
            for (int index = 0; index < strArray.Length; ++index)
            {
              string str = strArray[index].Substring(0, strArray[index].IndexOf(" "));
              if (!((Cpu) this.propParent).Libraries.ContainsKey((object) str))
              {
                Library library = new Library((Cpu) this.propParent, str);
                int num = strArray[index].IndexOf("TYP=");
                library.propType = (LibraryType) System.Convert.ToByte(strArray[index].Substring(num + 4));
              }
            }
          }
          this.OnUploaded(new PviEventArgs(this.propName, "", errorCode, this.Service.Language, Action.LibraryUpload, this.Service));
        }
        else
        {
          this.OnUploaded(new PviEventArgs(this.propName, "", errorCode, this.Service.Language, Action.LibraryUpload, this.Service));
          this.OnError(new PviEventArgs(this.propName, "", errorCode, this.Service.Language, Action.LibraryUpload, this.Service));
        }
      }
      else
        base.OnPviRead(errorCode, accessType, dataState, pData, dataLen, option);
    }

    protected virtual void OnError(PviEventArgs e) => this.Fire_Error((object) this, e);

    internal override int ToXMLTextWriter(ref XmlTextWriter writer, ConfigurationFlags flags)
    {
      int xmlTextWriter1 = 0;
      if (this.Count > 0)
      {
        writer.WriteStartElement(this.GetType().Name);
        foreach (object obj in (IEnumerable) this.Values)
        {
          writer.WriteStartElement("Library");
          int xmlTextWriter2 = ((Base) obj).ToXMLTextWriter(ref writer, flags);
          if (xmlTextWriter2 != 0)
            xmlTextWriter1 = xmlTextWriter2;
          writer.WriteEndElement();
        }
        writer.WriteEndElement();
      }
      return xmlTextWriter1;
    }

    public override Service Service => ((Base) this.propParent).Service;

    internal override void Dispose(bool disposing, bool removeFromCollection)
    {
      if (this.propDisposed)
        return;
      object propParent = this.propParent;
      object propUserData = this.propUserData;
      string propName = this.propName;
      this.CleanUp(disposing);
      this.propParent = propParent;
      this.propUserData = propUserData;
      this.propName = propName;
      base.Dispose(disposing, removeFromCollection);
      this.propParent = (object) null;
      this.propUserData = (object) null;
      this.propName = (string) null;
    }

    internal void CleanUp(bool disposing)
    {
      ArrayList arrayList = new ArrayList();
      this.propCounter = 0;
      if (this.Values != null)
      {
        foreach (Library library in (IEnumerable) this.Values)
        {
          arrayList.Add((object) library);
          if (library.LinkId != 0U)
            library.Disconnect(0);
        }
      }
      for (int index = 0; index < arrayList.Count; ++index)
        ((Base) arrayList[index]).Dispose(disposing, true);
      this.Clear();
    }

    public Library this[string name] => this.propCollectionType == CollectionType.HashTable ? (Library) this[(object) name] : (Library) null;
  }
}
