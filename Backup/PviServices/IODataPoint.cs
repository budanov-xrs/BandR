// Decompiled with JetBrains decompiler
// Type: BR.AN.PviServices.IODataPoint
// Assembly: BR.AN.PVIServices, Version=1.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DA853F0A-C070-4E30-B7A7-51D955FB85F6
// Assembly location: C:\Users\ss_six\Desktop\FXE\FXE software integration package\COMET_FXE-PVIServices_Example\Pluto-PVI-trial_01\packages\IToolS.BandR.3.5.58\lib\net20\BR.AN.PVIServices.dll

using System;
using System.Xml;

namespace BR.AN.PviServices
{
  public class IODataPoint : Base
  {
    private IOVariable propForceVar;
    private IOVariable propConsumerVar;
    private IOVariable propProducerVar;
    private int propIOConnection;
    private int propIOActivation;
    private int propIOValidated;
    private bool propDataValid;
    private int propMissingValueEvent;
    private bool propSimulated;
    private Value propPviValue;
    private Value propInternalValue;
    private Value propPhysicalValue;
    private Value propForceValue;
    private Base propOwner;
    private Cpu propCpu;
    private int propRefreshTime;
    private bool propActive;

    public IODataPoint(Cpu cpu, string name)
      : base((Base) cpu)
    {
      this.propMissingValueEvent = 0;
      this.propDataValid = false;
      this.propInternalValue = new Value();
      this.propActive = true;
      this.propName = name;
      this.propCpu = cpu;
      this.propCpu.IODataPoints.Add(this);
      this.propRefreshTime = 100;
      this.propPviValue = new Value(0);
      this.propPhysicalValue = new Value(0);
      this.propForceValue = new Value(0);
      this.propOwner = (Base) cpu;
      this.propIOConnection = 0;
      this.propIOActivation = 0;
      this.propIOValidated = 0;
      this.propForceVar = new IOVariable(cpu, name, IOVariableTypes.FORCE);
      this.propConsumerVar = new IOVariable(cpu, name, IOVariableTypes.VALUE);
      this.propProducerVar = new IOVariable(cpu, name, IOVariableTypes.PHYSICAL);
      this.propSimulated = false;
      this.AddIOVariableEvents();
    }

    private void AddIOVariableEvents()
    {
      this.propForceVar.Error += new PviEventHandler(this.ForceVar_Error);
      this.propForceVar.Disconnected += new PviEventHandler(this.ForceVar_Disconnected);
      this.propForceVar.DataValidated += new PviEventHandler(this.ForceVar_DataValidated);
      this.propForceVar.Deactivated += new PviEventHandler(this.ForceVar_Deactivated);
      this.propForceVar.Connected += new PviEventHandler(this.ForceVar_Connected);
      this.propForceVar.Activated += new PviEventHandler(this.ForceVar_Activated);
      this.propForceVar.ValueChanged += new VariableEventHandler(this.ForceVar_ValueChanged);
      this.propForceVar.ValueRead += new PviEventHandler(this.ForceVar_ValueRead);
      this.propForceVar.ValueWritten += new PviEventHandler(this.ForceVar_ValueWritten);
      this.propConsumerVar.Error += new PviEventHandler(this.ConsumerVar_Error);
      this.propConsumerVar.Disconnected += new PviEventHandler(this.ConsumerVar_Disconnected);
      this.propConsumerVar.DataValidated += new PviEventHandler(this.ConsumerVar_DataValidated);
      this.propConsumerVar.Deactivated += new PviEventHandler(this.ConsumerVar_Deactivated);
      this.propConsumerVar.Connected += new PviEventHandler(this.ConsumerVar_Connected);
      this.propConsumerVar.Activated += new PviEventHandler(this.ConsumerVar_Activated);
      this.propConsumerVar.ValueChanged += new VariableEventHandler(this.ConsumerVar_ValueChanged);
      this.propConsumerVar.ForcedOff += new PviEventHandler(this.ConsumerVar_ForcedOff);
      this.propConsumerVar.ForcedOn += new PviEventHandler(this.ConsumerVar_ForcedOn);
      this.propConsumerVar.ValueRead += new PviEventHandler(this.ConsumerVar_ValueRead);
      this.propConsumerVar.StatusChanged += new PviValueEventHandler(this.ConsumerVar_StatusChanged);
      this.propProducerVar.Error += new PviEventHandler(this.ProducerVar_Error);
      this.propProducerVar.Disconnected += new PviEventHandler(this.ProducerVar_Disconnected);
      this.propProducerVar.DataValidated += new PviEventHandler(this.ProducerVar_DataValidated);
      this.propProducerVar.Deactivated += new PviEventHandler(this.ProducerVar_Deactivated);
      this.propProducerVar.Connected += new PviEventHandler(this.ProducerVar_Connected);
      this.propProducerVar.Activated += new PviEventHandler(this.ProducerVar_Activated);
      this.propProducerVar.ValueChanged += new VariableEventHandler(this.ProducerVar_ValueChanged);
      this.propProducerVar.ValueRead += new PviEventHandler(this.ProducerVar_ValueRead);
    }

    internal override void reCreateState()
    {
      if (!this.reCreateActive)
        return;
      this.propConnectionState = ConnectionStates.Unininitialized;
      this.propReCreateActive = true;
      this.reCreateActive = false;
      this.propForceVar.reCreateActive = false;
      this.propConsumerVar.reCreateActive = false;
      this.propProducerVar.reCreateActive = false;
      this.propLinkId = 0U;
      this.Connect();
    }

    public override void Connect()
    {
      if (ConnectionStates.Connected == this.propConnectionState || ConnectionStates.ConnectedError == this.propConnectionState)
      {
        this.Fire_ConnectedEvent((object) this, new PviEventArgs(this.Name, this.Address, this.propErrorCode, this.Service.Language, Action.IODataPointConnect, this.Service));
      }
      else
      {
        if (ConnectionStates.Connecting == this.propConnectionState)
          return;
        this.propConnectionState = ConnectionStates.Connecting;
        this.propProducerVar.Active = this.propActive;
        this.propConsumerVar.Active = this.propActive;
        this.propForceVar.Active = this.propActive;
        this.propReturnValue = 0;
        this.propProducerVar.Connect();
        this.propReturnValue = this.propProducerVar.ReturnValue;
      }
    }

    public override void Disconnect()
    {
      this.propIOConnection = 7;
      this.propConnectionState = ConnectionStates.Disconnecting;
      this.propReturnValue = 0;
      this.propForceVar.Disconnect();
      this.propReturnValue = this.propForceVar.ReturnValue;
      this.propConsumerVar.Disconnect();
      this.propReturnValue = this.propConsumerVar.ReturnValue;
      this.propProducerVar.Disconnect();
      this.propReturnValue = this.propProducerVar.ReturnValue;
    }

    public override void Disconnect(bool noResponse)
    {
      if (!noResponse)
      {
        this.Disconnect();
      }
      else
      {
        this.propConnectionState = ConnectionStates.Disconnecting;
        this.propIOConnection = 7;
        this.propReturnValue = 0;
        if (this.propForceVar != null)
        {
          this.propForceVar.Disconnect(noResponse);
          this.propReturnValue = this.propForceVar.ReturnValue;
        }
        if (this.propConsumerVar != null)
        {
          this.propConsumerVar.Disconnect(noResponse);
          this.propReturnValue = this.propConsumerVar.ReturnValue;
        }
        if (this.propProducerVar != null)
        {
          this.propProducerVar.Disconnect(noResponse);
          this.propReturnValue = this.propProducerVar.ReturnValue;
        }
        this.propConnectionState = ConnectionStates.Disconnected;
        this.propIOConnection = 0;
      }
    }

    public int Disconnect(int internalAction)
    {
      int num = 0;
      if (this.propForceVar != null)
        num = this.propForceVar.Disconnect((uint) internalAction);
      if (this.propConsumerVar != null)
        num = this.propConsumerVar.Disconnect((uint) internalAction);
      if (this.propProducerVar != null)
        num = this.propProducerVar.Disconnect((uint) internalAction);
      return num;
    }

    public void ReadValue(IOVariableTypes vType)
    {
      switch (vType)
      {
        case IOVariableTypes.VALUE:
          this.propConsumerVar.ReadValue();
          break;
        case IOVariableTypes.FORCE:
          this.propForceVar.ReadValue();
          break;
        default:
          this.propProducerVar.ReadValue();
          break;
      }
    }

    internal override void Dispose(bool disposing, bool removeFromCollection)
    {
      if (this.propDisposed)
        return;
      Base propParent = this.propParent;
      string propLinkName = this.propLinkName;
      string propLogicalName = this.propLogicalName;
      object propUserData = this.propUserData;
      string propName = this.propName;
      string propAddress = this.propAddress;
      base.Dispose(disposing, removeFromCollection);
      if (!disposing)
        return;
      this.propParent = propParent;
      this.propLinkName = propLinkName;
      this.propLogicalName = propLogicalName;
      this.propUserData = propUserData;
      this.propName = propName;
      this.propAddress = propAddress;
      if (removeFromCollection && this.Parent is Cpu)
        ((Cpu) this.Parent).IODataPoints.Remove(this.Name);
      if ((Value) null != this.propForceValue)
      {
        this.propForceValue.Dispose();
        this.propForceValue = (Value) null;
      }
      if ((Value) null != this.propInternalValue)
      {
        this.propInternalValue.Dispose();
        this.propInternalValue = (Value) null;
      }
      if ((Value) null != this.propPhysicalValue)
      {
        this.propPhysicalValue.Dispose();
        this.propPhysicalValue = (Value) null;
      }
      if (this.propConsumerVar != null)
      {
        this.propConsumerVar.Dispose();
        this.propConsumerVar = (IOVariable) null;
      }
      if (this.propForceVar != null)
      {
        this.propForceVar.Dispose();
        this.propForceVar = (IOVariable) null;
      }
      if (this.propProducerVar != null)
      {
        this.propProducerVar.Dispose();
        this.propProducerVar = (IOVariable) null;
      }
      if ((Value) null != this.propPviValue)
      {
        this.propPviValue.Dispose();
        this.propPviValue = (Value) null;
      }
      if (this.propOwner != null)
        this.propOwner = (Base) null;
      this.propCpu = (Cpu) null;
      this.propParent = (Base) null;
      this.propLinkName = (string) null;
      this.propLogicalName = (string) null;
      this.propUserData = (object) null;
      this.propName = (string) null;
      this.propAddress = (string) null;
    }

    public override void Remove()
    {
      if (this.propForceVar != null)
        this.propForceVar.Remove();
      if (this.propConsumerVar != null)
        this.propConsumerVar.Remove();
      if (this.propProducerVar != null)
        this.propProducerVar.Remove();
      base.Remove();
    }

    public void ReadValue() => this.ReadValue(IOVariableTypes.PHYSICAL);

    protected override void OnConnected(PviEventArgs e)
    {
      base.OnConnected(new PviEventArgs(this.Name, this.Address, e.ErrorCode, this.Service.Language, Action.IODataPointConnect, this.Service));
      if ((this.Requests & Actions.SetValue) != Actions.NONE)
      {
        this.propForceVar.Value = this.propInternalValue;
        this.Requests &= Actions.Connect | Actions.GetList | Actions.GetValue | Actions.GetForce | Actions.SetForce | Actions.Start | Actions.Stop | Actions.RunCycle | Actions.GetType | Actions.SetType | Actions.GetLength | Actions.SetRefresh | Actions.CreateLink | Actions.Upload | Actions.Download | Actions.SetActive | Actions.SetHysteresis | Actions.Disconnect | Actions.SetInitValue | Actions.GetLBType | Actions.ModuleInfo | Actions.ListSNMPVariables | Actions.Connected | Actions.Uploading | Actions.GetCpuInfo | Actions.ReadPVFormat | Actions.FireActivated | Actions.ReadIndex | Actions.FireDataValidated | Actions.FireValueChanged | Actions.Link;
      }
      if (Actions.SetActive == (this.Requests & Actions.SetActive))
        this.Requests &= Actions.Connect | Actions.GetList | Actions.GetValue | Actions.SetValue | Actions.GetForce | Actions.SetForce | Actions.Start | Actions.Stop | Actions.RunCycle | Actions.GetType | Actions.SetType | Actions.GetLength | Actions.SetRefresh | Actions.CreateLink | Actions.Upload | Actions.Download | Actions.SetHysteresis | Actions.Disconnect | Actions.SetInitValue | Actions.GetLBType | Actions.ModuleInfo | Actions.ListSNMPVariables | Actions.Connected | Actions.Uploading | Actions.GetCpuInfo | Actions.ReadPVFormat | Actions.FireActivated | Actions.ReadIndex | Actions.FireDataValidated | Actions.FireValueChanged | Actions.Link;
      if (Actions.SetForce == (this.Requests & Actions.SetForce))
      {
        this.Requests &= Actions.Connect | Actions.GetList | Actions.GetValue | Actions.SetValue | Actions.GetForce | Actions.Start | Actions.Stop | Actions.RunCycle | Actions.GetType | Actions.SetType | Actions.GetLength | Actions.SetRefresh | Actions.CreateLink | Actions.Upload | Actions.Download | Actions.SetActive | Actions.SetHysteresis | Actions.Disconnect | Actions.SetInitValue | Actions.GetLBType | Actions.ModuleInfo | Actions.ListSNMPVariables | Actions.Connected | Actions.Uploading | Actions.GetCpuInfo | Actions.ReadPVFormat | Actions.FireActivated | Actions.ReadIndex | Actions.FireDataValidated | Actions.FireValueChanged | Actions.Link;
        this.OnForcedOn(0);
      }
      else
      {
        if (!this.propConsumerVar.Force)
          return;
        this.OnForcedOn(0);
      }
    }

    protected override void OnDisconnected(PviEventArgs e)
    {
      if (this.Service == null)
        return;
      base.OnDisconnected(new PviEventArgs(this.Name, this.Address, e.ErrorCode, this.Service.Language, Action.IODataPointDisconnect, this.Service));
    }

    protected virtual void OnForcedOn(int error)
    {
      if (ConnectionStates.Connected == this.propConnectionState)
      {
        if (this.ForcedOn == null)
          return;
        this.ForcedOn((object) this, new PviEventArgs(this.Name, this.Address, error, this.Service.Language, Action.IODataPointForceOn, this.Service));
      }
      else
        this.Requests |= Actions.SetForce;
    }

    protected virtual void OnForcedOff(int error)
    {
      if (ConnectionStates.Connecting == this.propConnectionState)
      {
        this.Requests |= Actions.GetForce;
      }
      else
      {
        if (this.ForcedOff == null)
          return;
        this.ForcedOff((object) this, new PviEventArgs(this.Name, this.Address, error, this.Service.Language, Action.IODataPointForceOff, this.Service));
      }
    }

    protected virtual void OnValueWritten(int error)
    {
      if (this.ForceValueWritten == null)
        return;
      this.ForceValueWritten((object) this, new PviEventArgs(this.Name, this.Address, error, this.Service.Language, Action.VariableValueWrite, this.Service));
    }

    protected virtual void OnForceValueChanged(Variable sender, int error)
    {
      if (this.propDataValid)
      {
        if (this.ForceValueChanged == null)
          return;
        this.ForceValueChanged((object) this, new PviEventArgs(this.propName, this.propAddress, error, this.Service.Language, Action.ForceValueChangedEvent, this.Service));
      }
      else
        this.propMissingValueEvent |= 4;
    }

    protected virtual void OnPhysicalValueChanged(Variable sender, int error)
    {
      if ((this.Direction != Direction.Input || IOVariableTypes.PHYSICAL != ((IOVariable) sender).IOType) && (Direction.Output != this.Direction || IOVariableTypes.VALUE != ((IOVariable) sender).IOType))
        return;
      this.propPhysicalValue = sender.Value;
      if (this.propDataValid)
      {
        if (this.PhysicalValueChanged == null)
          return;
        this.PhysicalValueChanged((object) this, new PviEventArgs(this.propName, this.propAddress, error, this.Service.Language, Action.PhysicalValueChangedEvent, this.Service));
      }
      else
        this.propMissingValueEvent |= 1;
    }

    protected virtual void OnValueChanged(Variable sender, int error)
    {
      this.propPviValue = sender.Value;
      if (Direction.Output == this.Direction)
        this.propPhysicalValue = sender.Value;
      if (this.propDataValid)
      {
        if (this.ValueChanged != null)
          this.ValueChanged((object) this, new PviEventArgs(this.propName, this.propAddress, error, this.Service.Language, Action.VariableValueChangedEvent, this.Service));
        if (Direction.Output != this.Direction)
          return;
        this.OnPhysicalValueChanged(sender, error);
      }
      else
        this.propMissingValueEvent |= 2;
    }

    protected virtual void OnValueRead(Variable sender, PviEventArgs e, IOVariableTypes ioVType)
    {
      switch (ioVType)
      {
        case IOVariableTypes.PHYSICAL:
          if (this.Direction == Direction.Input)
          {
            this.propPhysicalValue = sender.Value;
            break;
          }
          break;
        case IOVariableTypes.VALUE:
          this.propPviValue = sender.Value;
          if (Direction.Output == this.Direction)
          {
            this.propPhysicalValue = sender.Value;
            break;
          }
          break;
        default:
          this.propForceValue = sender.Value;
          break;
      }
      if (this.ValueRead == null)
        return;
      this.ValueRead((object) this, new PviEventArgs(this.Name, this.Address, e.ErrorCode, this.Service.Language, e.Action, this.Service));
    }

    [CLSCompliant(false)]
    public Value PhysicalValue => this.propPhysicalValue;

    [CLSCompliant(false)]
    public Value ForceValue
    {
      get => this.propForceValue;
      set => this.propForceVar.WriteIOValue(value);
    }

    public int RefreshTime
    {
      get => this.propRefreshTime;
      set
      {
        this.propRefreshTime = value;
        this.propForceVar.RefreshTime = this.propRefreshTime;
        this.propProducerVar.RefreshTime = this.propRefreshTime;
        this.propConsumerVar.RefreshTime = this.propRefreshTime;
      }
    }

    [CLSCompliant(false)]
    public Value Value => this.propPviValue;

    public bool Force
    {
      get => this.propConsumerVar.Force;
      set => this.propConsumerVar.Force = value;
    }

    public override string FullName
    {
      get
      {
        if (this.Name != null && 0 < this.Name.Length)
          return this.propOwner.FullName + "." + this.Name;
        return this.propOwner != null ? this.propOwner.FullName : "";
      }
    }

    public override string PviPathName => this.Name != null && 0 < this.Name.Length ? this.propOwner.PviPathName + "/\"" + this.propName + "\" OT=Pvar" : this.propOwner.PviPathName;

    public bool Simulated => this.propSimulated;

    public Direction Direction => -1 != this.Name.IndexOf("%Q") ? Direction.Output : Direction.Input;

    public event PviEventHandler ForcedOn;

    public event PviEventHandler ForcedOff;

    public event PviEventHandler ForceValueWritten;

    public event PviEventHandler ValueChanged;

    public event PviEventHandler PhysicalValueChanged;

    public event PviEventHandler ForceValueChanged;

    public event PviEventHandler ValueRead;

    public bool Active
    {
      get => this.propActive;
      set => this.SetActive(value);
    }

    private void SetActive(bool value)
    {
      if (value == this.propActive)
        return;
      this.propActive = value;
      this.propForceVar.Active = this.propActive;
      this.propConsumerVar.Active = this.propActive;
      this.propProducerVar.Active = this.propActive;
    }

    private void ForceVar_ValueChanged(object sender, VariableEventArgs e)
    {
      this.propForceValue = ((Variable) sender).Value;
      this.OnForceValueChanged((Variable) sender, e.ErrorCode);
    }

    private void ForceVar_ValueRead(object sender, PviEventArgs e)
    {
      this.propForceValue = ((Variable) sender).Value;
      this.OnValueRead((Variable) sender, e, IOVariableTypes.FORCE);
    }

    private void ForceVar_ValueWritten(object sender, PviEventArgs e)
    {
      this.propForceValue = ((Variable) sender).Value;
      this.OnValueWritten(e.ErrorCode);
    }

    public event PviEventHandler Deactivated;

    private void OnDeactivated(int error)
    {
      this.propActive = false;
      if (this.Deactivated == null)
        return;
      this.Deactivated((object) this, new PviEventArgs(this.Name, this.Address, error, this.Service.Language, Action.VariableDeactivate, this.Service));
    }

    public event PviEventHandler Activated;

    private void OnActivated(int error)
    {
      this.propActive = true;
      if (this.Activated == null)
        return;
      this.Activated((object) this, new PviEventArgs(this.Name, this.Address, error, this.Service.Language, Action.VariableActivate, this.Service));
    }

    public event PviEventHandler DataValidated;

    protected virtual void OnDataValidated()
    {
      if (this.DataValidated != null)
        this.DataValidated((object) this, new PviEventArgs(this.Name, this.Address, 0, this.Service.Language, Action.VariablesDataValid, this.Service));
      if (Actions.FireActivated != (this.Requests & Actions.FireActivated))
        return;
      this.Requests &= Actions.Connect | Actions.GetList | Actions.GetValue | Actions.SetValue | Actions.GetForce | Actions.SetForce | Actions.Start | Actions.Stop | Actions.RunCycle | Actions.GetType | Actions.SetType | Actions.GetLength | Actions.SetRefresh | Actions.CreateLink | Actions.Upload | Actions.Download | Actions.SetActive | Actions.SetHysteresis | Actions.Disconnect | Actions.SetInitValue | Actions.GetLBType | Actions.ModuleInfo | Actions.ListSNMPVariables | Actions.Connected | Actions.Uploading | Actions.GetCpuInfo | Actions.ReadPVFormat | Actions.ReadIndex | Actions.FireDataValidated | Actions.FireValueChanged | Actions.Link;
      this.OnActivated(0);
    }

    private void CheckValidState()
    {
      if (7 != this.propIOValidated || this.propDataValid || 7 != this.propIOConnection)
        return;
      this.propDataValid = true;
      this.OnDataValidated();
      if (2 == (2 & this.propMissingValueEvent))
      {
        this.propMissingValueEvent ^= 2;
        this.OnValueChanged((Variable) this.propConsumerVar, 0);
      }
      if (4 == (4 & this.propMissingValueEvent))
      {
        this.propMissingValueEvent ^= 4;
        this.OnForceValueChanged((Variable) this.propForceVar, 0);
      }
      if (1 != (1 & this.propMissingValueEvent))
        return;
      this.propMissingValueEvent ^= 1;
      if (this.Direction == Direction.Input)
        this.OnPhysicalValueChanged((Variable) this.propProducerVar, 0);
      else
        this.OnPhysicalValueChanged((Variable) this.propConsumerVar, 0);
    }

    private void CheckActiveState(bool doActivate, PviEventArgs e)
    {
      if (doActivate)
      {
        if (7 == this.propIOActivation && 7 == this.propIOValidated && 7 == this.propIOConnection)
          this.OnActivated(e.ErrorCode);
        else
          this.Requests |= Actions.FireActivated;
      }
      else
      {
        if (this.propIOActivation != 0)
          return;
        this.OnDeactivated(e.ErrorCode);
      }
    }

    private void CheckConnectionState(bool doConnect, PviEventArgs e)
    {
      if (doConnect)
      {
        if (7 != this.propIOConnection)
          return;
        this.OnConnected(e);
        if (this.propMissingValueEvent == 0)
          return;
        this.CheckValidState();
      }
      else
      {
        if (this.propIOConnection != 0)
          return;
        this.OnDisconnected(e);
      }
    }

    private void ForceVar_Activated(object sender, PviEventArgs e)
    {
      this.propIOActivation |= 4;
      this.CheckActiveState(true, e);
    }

    private void ForceVar_Connected(object sender, PviEventArgs e)
    {
      this.propIOConnection |= 4;
      this.CheckConnectionState(true, e);
      if (e.ErrorCode != 0)
        return;
      this.propProducerVar.Active = this.propActive;
      this.propConsumerVar.Active = this.propActive;
      this.propForceVar.Active = this.propActive;
    }

    private void ForceVar_Deactivated(object sender, PviEventArgs e)
    {
      this.propIOActivation ^= 4;
      this.CheckActiveState(false, e);
    }

    private void ForceVar_DataValidated(object sender, PviEventArgs e)
    {
      this.propIOValidated |= 4;
      this.CheckValidState();
    }

    private void ForceVar_Disconnected(object sender, PviEventArgs e)
    {
      this.propIOConnection ^= 4;
      this.CheckConnectionState(false, e);
    }

    private void ForceVar_Error(object sender, PviEventArgs e) => this.OnError(e, IOVariableTypes.FORCE);

    private void ConsumerVar_ValueChanged(object sender, VariableEventArgs e) => this.OnValueChanged((Variable) sender, e.ErrorCode);

    private void ConsumerVar_ValueRead(object sender, PviEventArgs e) => this.OnValueRead((Variable) sender, e, IOVariableTypes.VALUE);

    private void ConsumerVar_StatusChanged(object sender, object newValue)
    {
      string str = newValue.ToString();
      int num = str.IndexOf("IO");
      this.propSimulated = false;
      if (-1 == num || -1 == str.IndexOf("s", num + 3))
        return;
      this.propSimulated = true;
    }

    private void ConsumerVar_Activated(object sender, PviEventArgs e)
    {
      this.propIOActivation |= 2;
      this.CheckActiveState(true, e);
    }

    private void ConsumerVar_Connected(object sender, PviEventArgs e)
    {
      this.propIOConnection |= 2;
      if (e.ErrorCode != 0)
        return;
      this.propForceVar.Connect();
      this.propReturnValue = this.propForceVar.ReturnValue;
    }

    private void ConsumerVar_Deactivated(object sender, PviEventArgs e)
    {
      this.propIOActivation ^= 2;
      this.CheckActiveState(false, e);
    }

    private void ConsumerVar_DataValidated(object sender, PviEventArgs e)
    {
      this.propIOValidated |= 2;
      this.CheckValidState();
    }

    private void ConsumerVar_Disconnected(object sender, PviEventArgs e)
    {
      this.propIOConnection ^= 2;
      this.CheckConnectionState(false, e);
    }

    private void ConsumerVar_Error(object sender, PviEventArgs e) => this.OnError(e, IOVariableTypes.VALUE);

    private void ProducerVar_ValueChanged(object sender, VariableEventArgs e) => this.OnPhysicalValueChanged((Variable) sender, e.ErrorCode);

    private void ProducerVar_ValueRead(object sender, PviEventArgs e) => this.OnValueRead((Variable) sender, e, IOVariableTypes.PHYSICAL);

    private void ProducerVar_Activated(object sender, PviEventArgs e)
    {
      this.propIOActivation |= 1;
      this.CheckActiveState(true, e);
    }

    private void ProducerVar_Connected(object sender, PviEventArgs e)
    {
      this.propIOConnection |= 1;
      if (e.ErrorCode != 0)
        return;
      this.propConsumerVar.Connect();
      this.propReturnValue = this.propConsumerVar.ReturnValue;
    }

    private void ProducerVar_Deactivated(object sender, PviEventArgs e)
    {
      this.propIOActivation ^= 1;
      this.CheckActiveState(false, e);
    }

    private void ProducerVar_DataValidated(object sender, PviEventArgs e)
    {
      this.propIOValidated |= 1;
      this.CheckValidState();
    }

    private void ProducerVar_Disconnected(object sender, PviEventArgs e)
    {
      this.propIOConnection ^= 1;
      this.CheckConnectionState(false, e);
    }

    private void ProducerVar_Error(object sender, PviEventArgs e) => this.OnError(e, IOVariableTypes.PHYSICAL);

    private void OnError(PviEventArgs e, IOVariableTypes ioVType)
    {
      switch (ioVType)
      {
        case IOVariableTypes.PHYSICAL:
          if (e.ErrorCode != 4808)
          {
            this.OnError(new PviEventArgs(this.Name, this.Address, e.ErrorCode, this.Service.Language, e.Action, this.Service));
            break;
          }
          break;
        case IOVariableTypes.VALUE:
          if (e.ErrorCode != 4808)
          {
            this.OnError(new PviEventArgs(this.Name, this.Address, e.ErrorCode, this.Service.Language, e.Action, this.Service));
            break;
          }
          break;
        default:
          this.OnError(new PviEventArgs(this.Name, this.Address, e.ErrorCode, this.Service.Language, e.Action, this.Service));
          break;
      }
      if (!Service.IsRemoteError(e.ErrorCode) || this.propConnectionState == ConnectionStates.Unininitialized || ConnectionStates.Disconnected == this.propConnectionState || ConnectionStates.Disconnecting == this.propConnectionState)
        return;
      this.reCreateActive = true;
    }

    private void ConsumerVar_ForcedOff(object sender, PviEventArgs e) => this.OnForcedOff(e.ErrorCode);

    private void ConsumerVar_ForcedOn(object sender, PviEventArgs e) => this.OnForcedOn(e.ErrorCode);

    internal override int ToXMLTextWriter(ref XmlTextWriter writer, ConfigurationFlags flags)
    {
      base.ToXMLTextWriter(ref writer, flags);
      if (!this.propActive)
        writer.WriteAttributeString("Active", this.propActive.ToString());
      if (this.Direction == Direction.Output)
        writer.WriteAttributeString("Direction", this.Direction.ToString());
      if (this.propForceValue != (Value) null && this.propForceValue.ToString() != "" && this.propForceValue.ToString() != "0")
        writer.WriteAttributeString("ForceValue", this.propForceValue.ToString());
      writer.WriteAttributeString("RefreshTime", this.propRefreshTime.ToString());
      if (this.propPhysicalValue != (Value) null && this.propPhysicalValue.ToString() != "" && this.propPhysicalValue.ToString() != "0")
        writer.WriteAttributeString("PhysicalValue", this.propPhysicalValue.ToString());
      if (this.propPviValue != (Value) null && this.propPviValue.ToString() != "" && this.propPviValue.ToString() != "0")
        writer.WriteAttributeString("PviValue", this.propPviValue.ToString());
      if (this.Simulated)
        writer.WriteAttributeString("Simulated", this.Simulated.ToString());
      return 0;
    }

    public override int FromXmlTextReader(
      ref XmlTextReader reader,
      ConfigurationFlags flags,
      Base baseObj)
    {
      int num = base.FromXmlTextReader(ref reader, flags, baseObj);
      IODataPoint ioDataPoint = (IODataPoint) baseObj;
      if (ioDataPoint == null)
        return -1;
      int result1 = 0;
      uint result2 = 0;
      string str = "";
      string attribute1 = reader.GetAttribute("Active");
      if (attribute1 != null && attribute1.Length > 0 && attribute1.ToLower() == "false")
        ioDataPoint.propActive = false;
      str = "";
      string attribute2 = reader.GetAttribute("ForceValue");
      if (attribute2 != null && attribute2.Length > 0)
        ioDataPoint.propForceValue.Assign((object) attribute2);
      str = "";
      string attribute3 = reader.GetAttribute("PhysicalValue");
      if (attribute3 != null && attribute3.Length > 0)
        ioDataPoint.propPhysicalValue.Assign((object) attribute3);
      str = "";
      string attribute4 = reader.GetAttribute("PviValue");
      if (attribute4 != null && attribute4.Length > 0)
        ioDataPoint.propPviValue.Assign((object) attribute4);
      str = "";
      string attribute5 = reader.GetAttribute("Simulated");
      if (attribute5 != null && attribute5.Length > 0 && attribute5.ToLower() == "true")
        ioDataPoint.propSimulated = true;
      str = "";
      string attribute6 = reader.GetAttribute("InternID");
      if (attribute6 != null && attribute6.Length > 0 && PviParse.TryParseUInt32(attribute6, out result2))
        ioDataPoint.propInternID = result2;
      str = "";
      string attribute7 = reader.GetAttribute("RefreshTime");
      if (attribute7 != null && attribute7.Length > 0 && PviParse.TryParseInt32(attribute7, out result1))
        ioDataPoint.propRefreshTime = result1;
      reader.Read();
      return num;
    }
  }
}
