// Decompiled with JetBrains decompiler
// Type: BR.AN.PviServices.TaskClassCollection
// Assembly: BR.AN.PVIServices, Version=1.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DA853F0A-C070-4E30-B7A7-51D955FB85F6
// Assembly location: C:\Users\ss_six\Desktop\FXE\FXE software integration package\COMET_FXE-PVIServices_Example\Pluto-PVI-trial_01\packages\IToolS.BandR.3.5.58\lib\net20\BR.AN.PVIServices.dll

using System;
using System.Collections;
using System.Runtime.InteropServices;

namespace BR.AN.PviServices
{
  public class TaskClassCollection : BaseCollection
  {
    internal TaskClassCollection(Base parent, string name)
      : base(CollectionType.ArrayList, (object) parent, name)
    {
    }

    internal void Add(TaskClass taskClass) => this.propArrayList.Add((object) taskClass);

    protected void OnError(CollectionEventArgs e)
    {
      if (!(this.propParent is Cpu))
        return;
      ((Base) this.propParent).OnError((PviEventArgs) e);
    }

    public void Upload()
    {
      if (this.propParent is Cpu && !((Base) this.propParent).IsConnected && this.Service.WaitForParentConnection)
      {
        this.Requests |= Actions.Upload;
      }
      else
      {
        int error = this.ReadArgumentRequest(((Base) this.propParent).Service.hPvi, ((Base) this.propParent).LinkId, AccessTypes.TaskClass, IntPtr.Zero, 0, 617U, this.InternId);
        if (error == 0)
          return;
        this.OnError(new CollectionEventArgs(((Base) this.propParent).Name, ((Base) this.propParent).Address, error, this.Service.Language, Action.TaskClassesUpload, (BaseCollection) null));
      }
    }

    internal override void OnPviRead(
      int errorCode,
      PVIReadAccessTypes accessType,
      PVIDataStates dataState,
      IntPtr pData,
      uint dataLen,
      int option)
    {
      if (PVIReadAccessTypes.TaskClasses == accessType)
      {
        if (errorCode == 0)
        {
          int num1 = 44;
          int num2 = (int) ((long) dataLen / (long) num1);
          this.Clear();
          for (int index = 0; index < num2; ++index)
            this.Add(new TaskClass((APIFC_TkInfoRes) Marshal.PtrToStructure((IntPtr) ((int) pData + index * num1), typeof (APIFC_TkInfoRes))));
          this.OnUploaded(new PviEventArgs(this.propName, "", errorCode, this.Service.Language, Action.TaskClassesUpload, this.Service));
        }
        else
        {
          this.OnUploaded(new PviEventArgs(this.propName, "", errorCode, this.Service.Language, Action.TaskClassesUpload, this.Service));
          this.OnError(new CollectionEventArgs(this.propName, "", errorCode, this.Service.Language, Action.TaskClassesUpload, (BaseCollection) null));
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
      base.Dispose(disposing, false);
      this.propParent = (object) null;
      this.propUserData = (object) null;
      this.propName = (string) null;
    }

    internal void CleanUp(bool disposing)
    {
      this.propCounter = 0;
      if (this.Values != null)
      {
        foreach (TaskClass taskClass in (IEnumerable) this.Values)
          taskClass.Dispose(disposing, true);
      }
      this.Clear();
    }

    public TaskClass this[int index] => (TaskClass) this.propArrayList[index];
  }
}
