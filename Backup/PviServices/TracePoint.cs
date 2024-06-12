// Decompiled with JetBrains decompiler
// Type: BR.AN.PviServices.TracePoint
// Assembly: BR.AN.PVIServices, Version=1.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DA853F0A-C070-4E30-B7A7-51D955FB85F6
// Assembly location: C:\Users\ss_six\Desktop\FXE\FXE software integration package\COMET_FXE-PVIServices_Example\Pluto-PVI-trial_01\packages\IToolS.BandR.3.5.58\lib\net20\BR.AN.PVIServices.dll

using System;
using System.Runtime.InteropServices;

namespace BR.AN.PviServices
{
  public class TracePoint : Base
  {
    private TraceVariableCollection propTraceVariables;
    private ulong propOffset;
    private Variable.ROIoptions propROI;

    public TraceVariableCollection TraceVariables => this.propTraceVariables;

    public TracePoint(Task task, string name)
      : base((Base) task, name)
    {
      this.propROI = Variable.ROIoptions.OFF;
      if (0 < task.TracePoints.Contains(name))
        throw new ArgumentException("There exists already a Tracepoint with the same name!", name);
      this.propTraceVariables = new TraceVariableCollection(this);
      task.TracePoints.Add(this);
      this.propOffset = 0UL;
    }

    public int AddTraceVariable(string nameOfTraceVariable) => this.propTraceVariables.Add(nameOfTraceVariable);

    internal override void Dispose(bool disposing, bool removeFromCollection)
    {
      if (this.propDisposed)
        return;
      if (disposing)
      {
        this.propTraceVariables.Dispose();
        this.propTraceVariables = (TraceVariableCollection) null;
      }
      base.Dispose(disposing, removeFromCollection);
    }

    private TraceDataCollection UpdateTraceData(IntPtr pData, uint dataLength)
    {
      int srcOffset = 0;
      int index = 0;
      TraceDataCollection traceDataCollection = (TraceDataCollection) null;
      int[] dataDest1 = new int[2];
      if (IntPtr.Zero != pData && this.propTraceVariables != null)
      {
        traceDataCollection = new TraceDataCollection();
        if (dataLength < 8U)
        {
          byte[] numArray = new byte[(IntPtr) dataLength];
          Marshal.Copy(pData, numArray, 0, (int) dataLength);
          uint dataType = (uint) numArray[0];
          traceDataCollection.Add(new TraceData(numArray, (IECDataTypes) dataType));
        }
        else
        {
          while ((long) srcOffset < (long) dataLength)
          {
            PviMarshal.Copy(pData, srcOffset, ref dataDest1, 2);
            srcOffset += 8;
            uint dataType = PviMarshal.toUInt32(dataDest1.GetValue(0));
            byte[] dataDest2;
            if (dataType > 25U)
            {
              dataType = 0U;
              dataDest2 = new byte[1]{ (byte) 0 };
            }
            else
            {
              uint uint32 = PviMarshal.toUInt32(dataDest1.GetValue(1));
              if (uint32 <= (uint) int.MaxValue)
              {
                int destElements = int.Parse(uint32.ToString());
                dataDest2 = new byte[destElements];
                PviMarshal.Copy(pData, srcOffset, ref dataDest2, destElements);
                srcOffset += destElements;
              }
              else
              {
                dataType = 0U;
                dataDest2 = new byte[1]{ (byte) 0 };
              }
            }
            traceDataCollection.Add(new TraceData(dataDest2, (IECDataTypes) dataType));
            this.propTraceVariables[index]?.SetDataBytes((int) dataType, dataDest2);
            ++index;
          }
        }
      }
      return traceDataCollection;
    }

    public override string FullName
    {
      get
      {
        if (this.Name == null)
          return "";
        return this.propParent != null ? this.propParent.FullName + "." + this.Name : this.Name;
      }
    }

    public override string PviPathName => this.Name != null && 0 < this.Name.Length ? this.Parent.PviPathName + "/\"" + this.propName + "\" OT=Pvar" : this.Parent.PviPathName;

    public override void Connect()
    {
      this.propReturnValue = 0;
      if (ConnectionStates.Connected == this.propConnectionState || ConnectionStates.ConnectedError == this.propConnectionState)
      {
        this.Fire_ConnectedEvent((object) this, new PviEventArgs(this.Name, this.Address, this.propErrorCode, this.Service.Language, Action.VariableConnect, this.Service));
      }
      else
      {
        if (ConnectionStates.Connecting == this.propConnectionState)
          return;
        this.propLinkParam = "EV=eudf";
        this.propObjectParam = "CD=" + this.GetConnectionDescription();
        if (this.propROI != Variable.ROIoptions.OFF)
          this.propObjectParam = this.propObjectParam.Replace("\"" + this.propAddress + "\"", "\"/RO=" + this.propAddress + " /ROI=" + ((int) this.propROI).ToString() + "\"");
        string linkName = this.LinkName;
        this.propConnectionState = ConnectionStates.Connecting;
        this.propReturnValue = this.XCreateRequest(this.Service.hPvi, linkName, ObjectType.POBJ_PVAR, this.propObjectParam, 550U, this.propLinkParam, 501U);
        if (this.propReturnValue == 0)
          return;
        this.OnError(new PviEventArgs(this.propName, this.propAddress, this.propReturnValue, this.Service.Language, Action.VariableConnect, this.Service));
      }
    }

    [CLSCompliant(false)]
    public ulong Offset
    {
      get => this.propOffset;
      set => this.propOffset = value;
    }

    public Variable.ROIoptions RuntimeObjectIndex
    {
      get => this.propROI;
      set => this.propROI = value;
    }

    protected override string GetConnectionDescription()
    {
      string str1 = "\"TP+";
      if (this.propTraceVariables.Count == 0)
        this.propTraceVariables.Add(this.Name);
      string str2 = str1 + this.propOffset.ToString();
      for (int index = 0; index < this.propTraceVariables.Count; ++index)
      {
        TracePointVariable propTraceVariable = this.propTraceVariables[index];
        str2 = str2 + ";" + propTraceVariable.Name;
      }
      return str2 + "\"" + " AT=e";
    }

    public override void Disconnect()
    {
      this.propReturnValue = 0;
      this.Disconnect(931U, false);
    }

    public override void Disconnect(bool noResponse)
    {
      this.propReturnValue = 0;
      this.Disconnect(931U, noResponse);
    }

    internal int Disconnect(uint internalAction, bool noResponse)
    {
      int num = 12004;
      this.propNoDisconnectedEvent = noResponse;
      this.propConnectionState = ConnectionStates.Disconnecting;
      if (this.propLinkId != 0U)
      {
        if (this.Service != null)
        {
          if (this.propNoDisconnectedEvent)
          {
            num = this.Unlink();
            this.propConnectionState = ConnectionStates.Unininitialized;
          }
          else
            num = this.UnlinkRequest(internalAction);
        }
        else
        {
          this.propLinkId = 0U;
          num = 0;
        }
      }
      if (this.propNoDisconnectedEvent)
        this.propConnectionState = ConnectionStates.Unininitialized;
      return num;
    }

    internal override void OnPviEvent(
      int errorCode,
      EventTypes eventType,
      PVIDataStates dataState,
      IntPtr pData,
      uint dataLen,
      int option)
    {
      int propErrorCode = this.propErrorCode;
      this.propErrorCode = errorCode;
      if (errorCode != 0 && EventTypes.Error != eventType)
      {
        base.OnPviEvent(errorCode, eventType, dataState, pData, dataLen, option);
      }
      else
      {
        switch (eventType)
        {
          case EventTypes.Error:
            if (errorCode != 0)
            {
              if (this.IsConnected)
                this.OnDisconnected(new PviEventArgs(this.Name, this.Address, errorCode, this.Service.Language, Action.TracePointDisConnect, this.Service));
              else if (ConnectionStates.Connecting == this.propConnectionState)
                this.OnConnected(new PviEventArgs(this.Name, this.Address, errorCode, this.Service.Language, Action.TracePointConnect, this.Service));
              this.OnError(new PviEventArgs(this.Name, this.Address, errorCode, this.Service.Language, Action.ErrorEvent, this.Service));
              break;
            }
            this.OnConnected(new PviEventArgs(this.Name, this.Address, errorCode, this.Service.Language, Action.TracePointConnect, this.Service));
            break;
          case EventTypes.Data:
            this.OnTraceDataChanged(errorCode, Action.TracePointDataChanged, pData, dataLen);
            break;
          case EventTypes.Dataform:
            if (this.IsConnected || ConnectionStates.Connecting != this.propConnectionState)
              break;
            this.OnConnected(new PviEventArgs(this.Name, this.Address, errorCode, this.Service.Language, Action.TracePointConnect, this.Service));
            break;
          default:
            base.OnPviEvent(errorCode, eventType, dataState, pData, dataLen, option);
            break;
        }
      }
    }

    public event TraceDataEventHandler TraceDataChanged;

    [CLSCompliant(false)]
    protected virtual void OnTraceDataChanged(
      int error,
      Action action,
      IntPtr pData,
      uint dataLength)
    {
      TraceDataCollection traceDataCollection = (TraceDataCollection) null;
      TraceDataCollection traceDataCol = this.UpdateTraceData(pData, dataLength);
      if (this.TraceDataChanged != null)
        this.TraceDataChanged((object) this, new TraceDataEventArgs(this.Name, this.Address, error, this.Service.Language, action, traceDataCol));
      traceDataCol.Dispose();
      traceDataCollection = (TraceDataCollection) null;
    }
  }
}
