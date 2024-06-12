// Decompiled with JetBrains decompiler
// Type: BR.AN.PviServices.IOVariable
// Assembly: BR.AN.PVIServices, Version=1.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DA853F0A-C070-4E30-B7A7-51D955FB85F6
// Assembly location: C:\Users\ss_six\Desktop\FXE\FXE software integration package\COMET_FXE-PVIServices_Example\Pluto-PVI-trial_01\packages\IToolS.BandR.3.5.58\lib\net20\BR.AN.PVIServices.dll

using System;
using System.Runtime.InteropServices;

namespace BR.AN.PviServices
{
  internal class IOVariable : Variable
  {
    private Cpu propCpu;
    private IOVariableTypes propIOType;
    private bool propForce;
    private bool propStatusGot;

    public event PviValueEventHandler StatusChanged;

    public IOVariable(Cpu cpu, string ioName, IOVariableTypes ioType)
      : base(cpu)
    {
      this.propStatusGot = false;
      this.propForce = false;
      this.propIOType = ioType;
      this.propCpu = cpu;
      switch (ioType)
      {
        case IOVariableTypes.PHYSICAL:
          this.propName = "P+" + ioName;
          break;
        case IOVariableTypes.VALUE:
          this.propName = "C+" + ioName;
          break;
        default:
          this.propName = "F+" + ioName;
          break;
      }
      this.Initialize((Base) cpu, (Base) cpu, true, true);
      this.Init(this.propName);
    }

    public IOVariableTypes IOType => this.propIOType;

    internal bool Force
    {
      get => this.propForce;
      set
      {
        this.propForce = value;
        if (this.propForce)
          this.WriteRequest(this.Service.hPvi, this.propLinkId, AccessTypes.ForceOn, IntPtr.Zero, 0, 815U);
        else
          this.WriteRequest(this.Service.hPvi, this.propLinkId, AccessTypes.ForceOff, IntPtr.Zero, 0, 816U);
      }
    }

    internal override int Disconnect(uint internalAction, bool noResponse)
    {
      this.propStatusGot = false;
      return base.Disconnect(internalAction, noResponse);
    }

    private int InternalWriteIOValue()
    {
      int errorCode = this.WriteRequest(this.Service.hPvi, this.propLinkId, AccessTypes.Data, this.propPviValue.pData, this.propPviValue.propDataSize, 818U);
      if (errorCode != 0)
        this.OnError(new PviEventArgs(this.propName, this.propAddress, errorCode, this.Service.Language, Action.VariableValueWrite, this.Service));
      return errorCode;
    }

    internal void WriteIOValue(Value val)
    {
      if (this.IsConnected)
      {
        Value val1 = val;
        if (this.Convert != null && this.propPviValue.propObjValue != null)
          val1 = this.Convert.ValueToPviValue(val);
        switch (this.propPviValue.propDataType)
        {
          case DataType.Boolean:
            this.ResizePviDataPtr(1);
            if (val1 == false || val1.ToInt32((IFormatProvider) null) == 0)
            {
              Marshal.WriteByte(this.propPviValue.pData, (byte) 0);
              break;
            }
            Marshal.WriteByte(this.propPviValue.pData, (byte) 1);
            break;
          case DataType.SByte:
            if (CastModes.PG2000String == (this.CastMode & CastModes.PG2000String) && 1 < this.Value.ArrayLength)
            {
              this.propPviValue.Assign((object) val1.ToString());
              break;
            }
            this.ResizePviDataPtr(1);
            Marshal.WriteByte(this.propPviValue.pData, (byte) (sbyte) val1);
            break;
          case DataType.Int16:
            this.ResizePviDataPtr(2);
            Marshal.WriteInt16(this.propPviValue.pData, (short) val1);
            break;
          case DataType.Int32:
            this.ResizePviDataPtr(4);
            Marshal.WriteInt32(this.propPviValue.pData, (int) val1);
            break;
          case DataType.Int64:
            this.ResizePviDataPtr(8);
            PviMarshal.WriteInt64(this.propPviValue.pData, (long) val1);
            break;
          case DataType.Byte:
          case DataType.UInt8:
            if (CastModes.PG2000String == (this.CastMode & CastModes.PG2000String) && 1 < this.Value.ArrayLength)
            {
              this.propPviValue.Assign((object) val1.ToString());
              break;
            }
            this.ResizePviDataPtr(1);
            Marshal.WriteByte(this.propPviValue.pData, (byte) val1);
            break;
          case DataType.UInt16:
          case DataType.WORD:
            this.ResizePviDataPtr(2);
            Marshal.WriteInt16(this.propPviValue.pData, (short) (ushort) val1);
            break;
          case DataType.UInt32:
          case DataType.DWORD:
            this.ResizePviDataPtr(4);
            Marshal.WriteInt32(this.propPviValue.pData, (int) (uint) val1);
            break;
          case DataType.UInt64:
            this.ResizePviDataPtr(8);
            PviMarshal.WriteInt64(this.propPviValue.pData, (long) (ulong) val1);
            break;
          case DataType.Single:
            this.ResizePviDataPtr(4);
            Marshal.Copy(new float[1]{ (float) val1 }, 0, this.propPviValue.pData, 1);
            break;
          case DataType.Double:
            this.ResizePviDataPtr(8);
            Marshal.Copy(new double[1]{ (double) val1 }, 0, this.propPviValue.pData, 1);
            break;
        }
        this.InternalWriteIOValue();
      }
      else
      {
        if (this.Service.WaitForParentConnection)
          this.Requests |= Actions.SetValue;
        if (this.Convert != null)
          this.propInternalValue = this.Convert.ValueToPviValue(val);
        else
          this.propInternalValue = val;
      }
    }

    protected override string GetLinkParameters(
      ConnectionType conType,
      string dt,
      string fs,
      string lp,
      string va,
      string cm,
      string vL,
      string vN)
    {
      string linkParameters;
      switch (this.propIOType)
      {
        case IOVariableTypes.PHYSICAL:
          linkParameters = "EV=edf";
          break;
        case IOVariableTypes.VALUE:
          linkParameters = "EV=sedf";
          break;
        default:
          linkParameters = "EV=sedf";
          break;
      }
      return linkParameters;
    }

    protected override string GetEventMaskParameters(ConnectionType conType, bool useParamMarker)
    {
      string eventMaskParameters = "";
      if (useParamMarker)
        eventMaskParameters = "EV=";
      if (ConnectionType.Create != conType)
        eventMaskParameters = !this.Service.UserTagEvents ? eventMaskParameters + "elfs" : eventMaskParameters + "eulfs";
      if ((!this.Service.IsStatic || ConnectionType.Link == conType) && this.propActive)
        eventMaskParameters += "d";
      return eventMaskParameters;
    }

    protected override string GetObjectParameters(
      string rf,
      string hy,
      string at,
      string fs,
      string ut,
      string dt,
      string vL,
      string vN)
    {
      string objectParameters;
      switch (this.propIOType)
      {
        case IOVariableTypes.PHYSICAL:
          objectParameters = LogicalObjectsUsage.ObjectNameWithType != this.Service.LogicalObjectsUsage ? string.Format("\"{0}\"/\"{1} AT=r RF={2}\"", (object) this.propCpu.LinkName, (object) this.Name, (object) this.RefreshTime) : string.Format("\"{0}\" AT=r RF={1}", (object) this.Name, (object) this.RefreshTime);
          break;
        case IOVariableTypes.VALUE:
          objectParameters = LogicalObjectsUsage.ObjectNameWithType != this.Service.LogicalObjectsUsage ? string.Format("\"{0}\"/\"{1} AT=r RF={2}\"", (object) this.propCpu.LinkName, (object) this.Name, (object) this.RefreshTime) : string.Format("\"{0}\" AT=r RF={1}", (object) this.Name, (object) this.RefreshTime);
          break;
        default:
          objectParameters = LogicalObjectsUsage.ObjectNameWithType != this.Service.LogicalObjectsUsage ? string.Format("\"{0}\"/\"{1} RF={2}\"", (object) this.propCpu.LinkName, (object) this.Name, (object) this.RefreshTime) : string.Format("\"{0}\" RF={1}", (object) this.Name, (object) this.RefreshTime);
          break;
      }
      return objectParameters;
    }

    internal override void OnPviEvent(
      int errorCode,
      EventTypes eventType,
      PVIDataStates dataState,
      IntPtr pData,
      uint dataLen,
      int option)
    {
      switch (eventType)
      {
        case EventTypes.Data:
          this.UpdateValueData(pData, dataLen, errorCode);
          break;
        case EventTypes.Status:
          if (errorCode != 0 || dataLen <= 0U)
            break;
          string str = PviMarshal.PtrToStringAnsi(pData, dataLen);
          bool propForce = this.propForce;
          int length = str.IndexOf("\0");
          if (-1 != length)
            str = str.Substring(0, length);
          Variable.GetForceStatus(str, ref this.propForce);
          this.OnStatusChanged(str);
          if (this.IsConnected)
          {
            if (!propForce && this.propForce)
            {
              this.OnForcedOn(new PviEventArgs(this.Name, this.Address, errorCode, this.Service.Language, Action.IODataPointForceOn, this.Service));
              break;
            }
            if (!propForce || this.propForce)
              break;
            this.OnForcedOff(new PviEventArgs(this.Name, this.Address, errorCode, this.Service.Language, Action.IODataPointForceOff, this.Service));
            break;
          }
          if (this.propPviValue.DataType == DataType.Unknown)
          {
            this.Read_FormatEX(this.propLinkId);
            break;
          }
          if (ConnectionStates.Connecting != this.propConnectionState)
            break;
          this.OnConnected(new PviEventArgs(this.Name, this.Address, errorCode, this.Service.Language, Action.VariableConnect, this.Service));
          break;
        case EventTypes.Dataform:
          base.OnPviEvent(errorCode, eventType, dataState, pData, dataLen, option);
          if (IOVariableTypes.PHYSICAL != this.IOType && IOVariableTypes.FORCE != this.IOType && IOVariableTypes.VALUE != this.IOType)
            break;
          this.OnConnected(new PviEventArgs(this.Name, this.Address, errorCode, this.Service.Language, Action.VariableConnect, this.Service));
          break;
        default:
          base.OnPviEvent(errorCode, eventType, dataState, pData, dataLen, option);
          break;
      }
    }

    internal override void OnPviWritten(
      int errorCode,
      PVIWriteAccessTypes accessType,
      PVIDataStates dataState,
      int option,
      IntPtr pData,
      uint dataLen)
    {
      switch (accessType)
      {
        case PVIWriteAccessTypes.ForceOn:
          if (errorCode != 0 || 1 != option)
            break;
          this.OnForcedOn(new PviEventArgs(this.Name, this.Address, errorCode, this.Service.Language, Action.IODataPointForceOn, this.Service));
          break;
        case PVIWriteAccessTypes.ForceOff:
          if (errorCode != 0 || 2 != option)
            break;
          this.OnForcedOff(new PviEventArgs(this.Name, this.Address, errorCode, this.Service.Language, Action.IODataPointForceOff, this.Service));
          break;
        default:
          base.OnPviWritten(errorCode, accessType, dataState, option, pData, dataLen);
          break;
      }
    }

    protected override void OnValueChanged(VariableEventArgs e) => this.Fire_ValueChanged((object) this, e);

    protected override void OnValueWritten(PviEventArgs e) => this.Fire_ValueWritten((object) this, e);

    protected override void OnValueRead(PviEventArgs e) => this.Fire_ValueRead((object) this, e);

    internal override void OnPviRead(
      int errorCode,
      PVIReadAccessTypes accessType,
      PVIDataStates dataState,
      IntPtr pData,
      uint dataLen,
      int option)
    {
      if (errorCode == 0 && PVIReadAccessTypes.State == accessType)
        this.OnPviEvent(errorCode, EventTypes.Status, dataState, pData, dataLen, option);
      else
        base.OnPviRead(errorCode, accessType, dataState, pData, dataLen, option);
    }

    protected override void OnActivated(PviEventArgs e)
    {
      if (Actions.FireActivated != (this.Requests & Actions.FireActivated))
        return;
      this.Requests &= Actions.Connect | Actions.GetList | Actions.GetValue | Actions.SetValue | Actions.GetForce | Actions.SetForce | Actions.Start | Actions.Stop | Actions.RunCycle | Actions.GetType | Actions.SetType | Actions.GetLength | Actions.SetRefresh | Actions.CreateLink | Actions.Upload | Actions.Download | Actions.SetActive | Actions.SetHysteresis | Actions.Disconnect | Actions.SetInitValue | Actions.GetLBType | Actions.ModuleInfo | Actions.ListSNMPVariables | Actions.Connected | Actions.Uploading | Actions.GetCpuInfo | Actions.ReadPVFormat | Actions.ReadIndex | Actions.FireDataValidated | Actions.FireValueChanged | Actions.Link;
      this.Fire_Activated((object) this, e);
      if (this.propPviValue.DataType != DataType.Unknown && DataType.Structure != this.propPviValue.DataType && 1 >= this.propPviValue.ArrayLength)
        return;
      this.Read_FormatEX(this.propLinkId);
    }

    protected override void OnDeactivated(PviEventArgs e) => this.Fire_Deactivated((object) this, e);

    protected virtual void OnStatusChanged(string statutsText)
    {
      this.propStatusGot = true;
      if (this.StatusChanged == null)
        return;
      this.StatusChanged((object) this, (object) statutsText);
    }

    protected override void OnConnected(PviEventArgs e)
    {
      if (!this.Fire_Connected(e))
        return;
      this.CheckActiveRequests(e);
      if (!this.propStatusGot && IOVariableTypes.VALUE == this.IOType)
        this.Read_State(this.LinkId, 557U);
      if (this.Active)
        return;
      this.DeactivateInternal();
    }
  }
}
