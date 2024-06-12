// Decompiled with JetBrains decompiler
// Type: BR.AN.PviServices.MemoryCollection
// Assembly: BR.AN.PVIServices, Version=1.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DA853F0A-C070-4E30-B7A7-51D955FB85F6
// Assembly location: C:\Users\ss_six\Desktop\FXE\FXE software integration package\COMET_FXE-PVIServices_Example\Pluto-PVI-trial_01\packages\IToolS.BandR.3.5.58\lib\net20\BR.AN.PVIServices.dll

using System;
using System.Collections;
using System.Runtime.InteropServices;
using System.Xml;

namespace BR.AN.PviServices
{
  public class MemoryCollection : BaseCollection
  {
    public MemoryCollection(Cpu parent, string name)
      : base(CollectionType.ArrayList, (object) parent, name)
    {
    }

    public void Add(Memory memory) => this.propArrayList.Add((object) memory);

    protected void OnError(CollectionEventArgs e)
    {
      if (!(this.propParent is Cpu))
        return;
      ((Base) this.propParent).OnError((PviEventArgs) e);
    }

    public void Upload()
    {
      if (this.propParent is Cpu && !((Base) this.propParent).IsConnected && this.Service.WaitForParentConnection)
        this.Requests |= Actions.Upload;
      int error = this.ReadArgumentRequest(((Base) this.propParent).Service.hPvi, ((Base) this.propParent).LinkId, AccessTypes.CpuMemoryInfo, IntPtr.Zero, 0, 613U, this.InternId);
      if (error == 0)
        return;
      this.OnError(new CollectionEventArgs(((Base) this.propParent).Name, ((Base) this.propParent).Address, error, this.Service.Language, Action.MemoriesUpload, (BaseCollection) null));
    }

    internal override void OnPviRead(
      int errorCode,
      PVIReadAccessTypes accessType,
      PVIDataStates dataState,
      IntPtr pData,
      uint dataLen,
      int option)
    {
      if (PVIReadAccessTypes.CpuMemoryInfo == accessType)
      {
        if (errorCode == 0)
        {
          uint num1 = 22;
          uint num2 = dataLen / num1;
          this.Clear();
          for (uint index = 0; index < num2; ++index)
            this.Add(new Memory((Cpu) this.propParent, (APIFC_CPmemInfoRes) Marshal.PtrToStructure((IntPtr) (long) (uint) ((int) pData + (int) index * (int) num1), typeof (APIFC_CPmemInfoRes))));
          this.OnUploaded(new PviEventArgs(this.propName, "", errorCode, this.Service.Language, Action.MemoriesUpload, this.Service));
        }
        else
        {
          this.OnUploaded(new PviEventArgs(this.propName, "", errorCode, this.Service.Language, Action.MemoriesUpload, this.Service));
          this.OnError(new CollectionEventArgs(this.propName, "", errorCode, this.Service.Language, Action.MemoriesUpload, (BaseCollection) null));
        }
      }
      else
        base.OnPviRead(errorCode, accessType, dataState, pData, dataLen, option);
    }

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
        foreach (Memory memory in (IEnumerable) this.Values)
          arrayList.Add((object) memory);
      }
      for (int index = 0; index < arrayList.Count; ++index)
        ((Memory) arrayList[index]).Dispose(disposing);
      this.Clear();
    }

    internal override int ToXMLTextWriter(ref XmlTextWriter writer, ConfigurationFlags flags)
    {
      int xmlTextWriter1 = 0;
      if (this.Values.Count > 0)
      {
        writer.WriteStartElement(this.GetType().Name);
        foreach (Memory memory in (IEnumerable) this.Values)
        {
          int xmlTextWriter2 = memory.ToXMLTextWriter(ref writer, flags);
          if (xmlTextWriter2 != 0)
            xmlTextWriter1 = xmlTextWriter2;
        }
        writer.WriteEndElement();
      }
      return xmlTextWriter1;
    }

    public Memory this[int index] => (Memory) this.propArrayList[index];

    public Memory this[MemoryType type]
    {
      get
      {
        if (this.propParent is Cpu && 0 < ((Cpu) this.propParent).Memories.Count)
        {
          foreach (Memory memory in (IEnumerable) ((Cpu) this.propParent).Memories.Values)
          {
            if (memory.Type == type)
              return memory;
          }
        }
        return (Memory) null;
      }
    }
  }
}
