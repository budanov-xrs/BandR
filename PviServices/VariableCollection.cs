// Decompiled with JetBrains decompiler
// Type: BR.AN.PviServices.VariableCollection
// Assembly: BR.AN.PVIServices, Version=1.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DA853F0A-C070-4E30-B7A7-51D955FB85F6
// Assembly location: C:\Users\ss_six\Desktop\FXE\FXE software integration package\COMET_FXE-PVIServices_Example\Pluto-PVI-trial_01\packages\IToolS.BandR.3.5.58\lib\net20\BR.AN.PVIServices.dll

using System;
using System.Collections;
using System.Xml;

namespace BR.AN.PviServices
{
  public class VariableCollection : BaseCollection
  {
    private int propActiveCount;
    private int propDeactiveCount;
    private int propDatavalidCount;
    private VariableCollection propErrorVariables;
    private VariableCollection propValidVariables;
    private VariableCollection propDataValidVariables;
    private VariableCollection propActiveVariables;
    private VariableCollection propDeactiveVariables;
    private bool propWriteValueAutomatic;
    private double propHysteresis;
    private Scaling propScaling;
    private bool propPolling;
    private Access propVariableAccess;

    public VariableCollection(object parent, string name)
      : base(parent, name)
    {
      this.propParent = parent;
    }

    internal VariableCollection(CollectionType colType, object parent, string name)
      : base(colType, parent, name)
    {
      this.propParent = parent;
    }

    public override void Connect(ConnectionType connectionType)
    {
      this.propValidCount = 0;
      this.propErrorCount = 0;
      this.propSentCount = 0;
      this.Requests |= Actions.Connect;
      int num = 0;
      if (ConnectionStates.Connected == this.propConnectionState)
      {
        this.OnCollectionConnected((CollectionEventArgs) new VariableCollectionEventArgs(this.propName, "", 0, this.Service.Language, Action.VariablesConnect, this.propValidVariables));
      }
      else
      {
        if (this.propValidVariables != null)
        {
          this.propValidVariables.Clear();
        }
        else
        {
          this.propValidVariables = new VariableCollection(this.Parent, "Valid variables");
          this.propValidVariables.propInternalCollection = true;
        }
        if (this.propErrorVariables != null)
        {
          this.propErrorVariables.Clear();
        }
        else
        {
          this.propErrorVariables = new VariableCollection(this.Parent, "Error variables");
          this.propErrorVariables.propInternalCollection = true;
        }
        if (this.propDataValidVariables != null)
        {
          this.propDataValidVariables.Clear();
        }
        else
        {
          this.propDataValidVariables = new VariableCollection(this.Parent, "Valid variables");
          this.propDataValidVariables.propInternalCollection = true;
        }
        if (this.Count == 0)
        {
          this.Requests &= Actions.GetList | Actions.GetValue | Actions.SetValue | Actions.GetForce | Actions.SetForce | Actions.Start | Actions.Stop | Actions.RunCycle | Actions.GetType | Actions.SetType | Actions.GetLength | Actions.SetRefresh | Actions.CreateLink | Actions.Upload | Actions.Download | Actions.SetActive | Actions.SetHysteresis | Actions.Disconnect | Actions.SetInitValue | Actions.GetLBType | Actions.ModuleInfo | Actions.ListSNMPVariables | Actions.Connected | Actions.Uploading | Actions.GetCpuInfo | Actions.ReadPVFormat | Actions.FireActivated | Actions.ReadIndex | Actions.FireDataValidated | Actions.FireValueChanged | Actions.Link;
          this.OnCollectionConnected((CollectionEventArgs) new VariableCollectionEventArgs(this.propName, "", 0, this.Service.Language, Action.VariablesConnect, this.propValidVariables));
        }
        else
        {
          if (this.propCollectionType != CollectionType.HashTable)
            return;
          foreach (Variable variable in (IEnumerable) this.Values)
          {
            ++this.propSentCount;
            if (variable.IsConnected)
            {
              ++num;
              this.propValidVariables.Add(variable);
              ++this.propValidCount;
              if (num + this.propErrorCount == this.Count)
                this.OnCollectionConnected((CollectionEventArgs) new VariableCollectionEventArgs(this.propName, "", 0, this.Service.Language, Action.VariablesConnect, this.propValidVariables));
            }
            else
            {
              if (this.propActive)
                variable.Active = true;
              variable.Connect(connectionType);
            }
          }
        }
      }
    }

    protected internal override void OnCollectionConnected(CollectionEventArgs e) => base.OnCollectionConnected(e);

    public void Disconnect(bool noResponse)
    {
      if (noResponse)
      {
        this.DisconnectObjects(true);
        this.propValidCount = 0;
        this.propErrorCount = 0;
        this.propSentCount = 0;
        this.propDatavalidCount = 0;
        this.propConnectionState = ConnectionStates.Disconnected;
      }
      else
        this.Disconnect();
    }

    public void Disconnect()
    {
      this.propValidCount = 0;
      this.propErrorCount = 0;
      this.propSentCount = 0;
      this.propDatavalidCount = 0;
      if (this.Values == null || this.Count == 0)
      {
        this.OnCollectionDisconnected((CollectionEventArgs) new VariableCollectionEventArgs(this.propName, "", 0, this.Service.Language, Action.VariablesDisconnect, this));
      }
      else
      {
        this.Requests |= Actions.Disconnect;
        if (this.propValidVariables != null)
        {
          this.propValidVariables.Clear();
        }
        else
        {
          this.propValidVariables = new VariableCollection(this.Parent, "Valid variables");
          this.propValidVariables.propInternalCollection = true;
        }
        if (this.propErrorVariables != null)
        {
          this.propErrorVariables.Clear();
        }
        else
        {
          this.propErrorVariables = new VariableCollection(this.Parent, "Error variables");
          this.propErrorVariables.propInternalCollection = true;
        }
        if (this.Count == 0)
        {
          this.Requests &= Actions.Connect | Actions.GetList | Actions.GetValue | Actions.SetValue | Actions.GetForce | Actions.SetForce | Actions.Start | Actions.Stop | Actions.RunCycle | Actions.GetType | Actions.SetType | Actions.GetLength | Actions.SetRefresh | Actions.CreateLink | Actions.Upload | Actions.Download | Actions.SetActive | Actions.SetHysteresis | Actions.SetInitValue | Actions.GetLBType | Actions.ModuleInfo | Actions.ListSNMPVariables | Actions.Connected | Actions.Uploading | Actions.GetCpuInfo | Actions.ReadPVFormat | Actions.FireActivated | Actions.ReadIndex | Actions.FireDataValidated | Actions.FireValueChanged | Actions.Link;
          this.OnCollectionDisconnected((CollectionEventArgs) new VariableCollectionEventArgs(this.propName, "", 0, this.Service.Language, Action.VariablesDisconnect, this.propValidVariables));
        }
        else
          this.DisconnectObjects(false);
      }
    }

    private int DisconnectObjects(bool noResponse)
    {
      int errorCode = 0;
      int num = 0;
      if (this.propCollectionType == CollectionType.HashTable)
      {
        if (this.Values == null)
          return 0;
        this.propDisconnectedCount = this.Count;
        errorCode = 0;
        if (this.propValidVariables != null)
          this.propValidVariables.Clear();
        foreach (Variable variable in (IEnumerable) this.Values)
        {
          if (!noResponse)
          {
            ++num;
            if (!variable.IsConnected && this.propValidVariables != null)
            {
              ++this.propValidCount;
              this.propValidVariables.Add(variable);
            }
          }
          errorCode = variable.Disconnect(602U, noResponse);
          ++this.propSentCount;
          if (errorCode != 0)
          {
            if (!noResponse)
              variable.FireDisconnected(errorCode, Action.VariablesDisconnect);
            --this.propDisconnectedCount;
            if (this.propDisconnectedCount == 0)
            {
              if (!noResponse)
              {
                this.OnCollectionDisconnected((CollectionEventArgs) new VariableCollectionEventArgs(this.propName, "", 0, this.Service.Language, Action.VariablesDisconnect, this.propValidVariables));
                break;
              }
              break;
            }
          }
          if (num == this.Count)
            break;
        }
      }
      return errorCode;
    }

    protected internal override void OnCollectionDisconnected(CollectionEventArgs e) => base.OnCollectionDisconnected(e);

    public override void Add(object key, object value)
    {
      Variable variable = (Variable) value;
      if (!this.ContainsKey(key))
        base.Add(key, (object) variable);
      if (this.propParent is Cpu)
        variable.propParent = (Base) this.propParent;
      if (this.propInternalCollection)
        return;
      if (variable.propUserCollections == null)
        variable.propUserCollections = new Hashtable();
      if (variable.propUserCollections.ContainsKey(this.Name))
        return;
      variable.propUserCollections.Add(this.Name, (object) this);
    }

    public virtual void Add(Variable variable)
    {
      if (variable == null || variable.Name == null)
        return;
      if (!this.ContainsKey((object) variable.Name))
        base.Add((object) variable.Name, (object) variable);
      if (this.propParent is Cpu)
        variable.propParent = (Base) this.propParent;
      if (this.propInternalCollection)
        return;
      if (variable.propUserCollections == null)
        variable.propUserCollections = new Hashtable();
      if (variable.propUserCollections.ContainsKey(this.Name))
        return;
      variable.propUserCollections.Add(this.Name, (object) this);
    }

    public override void Remove(string key)
    {
      if (!this.ContainsKey((object) key))
        return;
      base.Remove(key);
      if (this.propValidVariables != null)
        this.propValidVariables.Remove(key);
      if (this.propErrorVariables != null)
        this.propErrorVariables.Remove(key);
      if (this.propDataValidVariables != null)
        this.propDataValidVariables.Remove(key);
      if (this.propActiveVariables != null)
        this.propActiveVariables.Remove(key);
      if (this.propDeactiveVariables == null)
        return;
      this.propDeactiveVariables.Remove(key);
    }

    public virtual void Remove(Variable variable)
    {
      if (!this.ContainsKey((object) variable.Name))
        return;
      this.Remove(variable.Name);
    }

    public void WriteScaling()
    {
      this.propValidCount = 0;
      this.propSentCount = 0;
      this.propErrorCount = 0;
      if (this.propValidVariables != null)
      {
        this.propValidVariables.Clear();
      }
      else
      {
        this.propValidVariables = new VariableCollection(this.Parent, "Valid variables");
        this.propValidVariables.propInternalCollection = true;
      }
      if (this.propErrorVariables != null)
      {
        this.propErrorVariables.Clear();
      }
      else
      {
        this.propErrorVariables = new VariableCollection(this.Parent, "Error variables");
        this.propErrorVariables.propInternalCollection = true;
      }
      if (this.propCollectionType != CollectionType.HashTable || 0 >= this.Count)
        return;
      foreach (Variable variable in (IEnumerable) this.Values)
      {
        ++this.propSentCount;
        variable.WriteScaling();
      }
    }

    public void WriteValues()
    {
      this.propValidCount = 0;
      this.propSentCount = 0;
      this.propErrorCount = 0;
      this.Requests |= Actions.SetValue;
      if (this.propValidVariables != null)
      {
        this.propValidVariables.Clear();
      }
      else
      {
        this.propValidVariables = new VariableCollection(this.Parent, "Valid variables");
        this.propValidVariables.propInternalCollection = true;
      }
      if (this.propErrorVariables != null)
      {
        this.propErrorVariables.Clear();
      }
      else
      {
        this.propErrorVariables = new VariableCollection(this.Parent, "Error variables");
        this.propErrorVariables.propInternalCollection = true;
      }
      if (this.Count == 0)
      {
        this.Requests &= Actions.Connect | Actions.GetList | Actions.GetValue | Actions.GetForce | Actions.SetForce | Actions.Start | Actions.Stop | Actions.RunCycle | Actions.GetType | Actions.SetType | Actions.GetLength | Actions.SetRefresh | Actions.CreateLink | Actions.Upload | Actions.Download | Actions.SetActive | Actions.SetHysteresis | Actions.Disconnect | Actions.SetInitValue | Actions.GetLBType | Actions.ModuleInfo | Actions.ListSNMPVariables | Actions.Connected | Actions.Uploading | Actions.GetCpuInfo | Actions.ReadPVFormat | Actions.FireActivated | Actions.ReadIndex | Actions.FireDataValidated | Actions.FireValueChanged | Actions.Link;
        this.OnCollectionValuesWritten((CollectionEventArgs) new VariableCollectionEventArgs(this.propName, "", 0, this.Service.Language, Action.VariableValueWrite, this.propValidVariables));
      }
      else
      {
        if (this.propCollectionType != CollectionType.HashTable)
          return;
        foreach (Variable variable in (IEnumerable) this.Values)
        {
          ++this.propSentCount;
          int errorCode = variable.WriteValue();
          if (errorCode != 0)
          {
            this.OnError(variable, new PviEventArgs(variable.propName, variable.propAddress, errorCode, this.Service.Language, Action.VariableValueWrite, this.Service));
            this.OnValueWritten(variable, new PviEventArgs(variable.Name, variable.Address, errorCode, this.Service.Language, Action.VariableValueWrite, this.Service));
          }
        }
      }
    }

    protected internal virtual void OnConnected(Variable variable, PviEventArgs e)
    {
      ++this.propValidCount;
      if (this.propValidVariables == null)
      {
        this.propValidVariables = new VariableCollection(this.Parent, "Valid variables");
        this.propValidVariables.propInternalCollection = true;
      }
      if (!this.propValidVariables.ContainsKey((object) variable.Name))
        this.propValidVariables.Add(variable);
      this.Fire_Connected((object) variable, e);
      if (this.propValidCount + this.propErrorCount != this.propSentCount || (this.Requests & Actions.Connect) == Actions.NONE)
        return;
      this.Requests &= Actions.GetList | Actions.GetValue | Actions.SetValue | Actions.GetForce | Actions.SetForce | Actions.Start | Actions.Stop | Actions.RunCycle | Actions.GetType | Actions.SetType | Actions.GetLength | Actions.SetRefresh | Actions.CreateLink | Actions.Upload | Actions.Download | Actions.SetActive | Actions.SetHysteresis | Actions.Disconnect | Actions.SetInitValue | Actions.GetLBType | Actions.ModuleInfo | Actions.ListSNMPVariables | Actions.Connected | Actions.Uploading | Actions.GetCpuInfo | Actions.ReadPVFormat | Actions.FireActivated | Actions.ReadIndex | Actions.FireDataValidated | Actions.FireValueChanged | Actions.Link;
      this.OnCollectionConnected((CollectionEventArgs) new VariableCollectionEventArgs(this.propName, "", 0, this.Service.Language, Action.VariablesConnect, this.propValidVariables));
      if (this.propErrorCount <= 0)
        return;
      this.OnCollectionError((CollectionEventArgs) new VariableCollectionEventArgs(this.propName, "", 0, this.Service.Language, Action.VariablesConnect, this.propErrorVariables));
    }

    protected internal virtual void OnDisconnected(Variable variable, PviEventArgs e)
    {
      this.propConnectionState = ConnectionStates.Disconnected;
      ++this.propValidCount;
      if (this.propValidVariables == null)
      {
        this.propValidVariables = new VariableCollection(this.Parent, "Valid variables");
        this.propValidVariables.propInternalCollection = true;
      }
      if (!this.propValidVariables.ContainsKey((object) variable.Name))
        this.propValidVariables.Add(variable);
      this.Fire_Disconnected((object) variable, e);
      if (this.propValidCount != this.propSentCount || (this.Requests & Actions.Disconnect) == Actions.NONE)
        return;
      this.Requests &= Actions.Connect | Actions.GetList | Actions.GetValue | Actions.SetValue | Actions.GetForce | Actions.SetForce | Actions.Start | Actions.Stop | Actions.RunCycle | Actions.GetType | Actions.SetType | Actions.GetLength | Actions.SetRefresh | Actions.CreateLink | Actions.Upload | Actions.Download | Actions.SetActive | Actions.SetHysteresis | Actions.SetInitValue | Actions.GetLBType | Actions.ModuleInfo | Actions.ListSNMPVariables | Actions.Connected | Actions.Uploading | Actions.GetCpuInfo | Actions.ReadPVFormat | Actions.FireActivated | Actions.ReadIndex | Actions.FireDataValidated | Actions.FireValueChanged | Actions.Link;
      this.OnCollectionDisconnected((CollectionEventArgs) new VariableCollectionEventArgs(this.propName, "", 0, this.Service.Language, Action.VariablesDisconnect, this.propValidVariables));
      if (this.propErrorCount <= 0)
        return;
      this.OnCollectionError((CollectionEventArgs) new VariableCollectionEventArgs(this.propName, "", 0, this.Service.Language, Action.VariablesDisconnect, this.propErrorVariables));
    }

    protected internal virtual void OnError(Variable variable, PviEventArgs e)
    {
      if (this.propErrorVariables == null)
      {
        this.propErrorVariables = new VariableCollection(this.Parent, "Error variables");
        this.propErrorVariables.propInternalCollection = true;
      }
      if (variable != null && !this.propErrorVariables.ContainsKey((object) variable.Name))
        this.propErrorVariables.Add(variable);
      this.Fire_Error((object) variable, e);
      if (this.propValidCount + this.propErrorCount != this.Count)
        return;
      if (e.Action == Action.VariableConnect)
        this.OnCollectionError((CollectionEventArgs) new VariableCollectionEventArgs(this.propName, "", 0, this.Service.Language, Action.VariablesConnect, this.propErrorVariables));
      else if (e.Action == Action.VariableDisconnect)
        this.OnCollectionError((CollectionEventArgs) new VariableCollectionEventArgs(this.propName, "", 0, this.Service.Language, Action.VariablesDisconnect, this.propErrorVariables));
      else if (e.Action == Action.VariablesDisconnect)
        this.OnCollectionError((CollectionEventArgs) new VariableCollectionEventArgs(this.propName, "", 0, this.Service.Language, Action.VariablesDisconnect, this.propErrorVariables));
      else if (e.Action == Action.VariableActivate)
        this.OnCollectionError((CollectionEventArgs) new VariableCollectionEventArgs(this.propName, "", 0, this.Service.Language, Action.VariablesActivate, this.propErrorVariables));
      else if (e.Action == Action.VariableDeactivate)
        this.OnCollectionError((CollectionEventArgs) new VariableCollectionEventArgs(this.propName, "", 0, this.Service.Language, Action.VariablesDeactivate, this.propErrorVariables));
      else
        this.OnCollectionError((CollectionEventArgs) new VariableCollectionEventArgs(this.propName, "", 0, this.Service.Language, Action.NONE, this.propErrorVariables));
      this.propErrorVariables.Clear();
      if (this.propValidCount > 0 && e.Action == Action.VariableConnect)
        this.OnCollectionConnected((CollectionEventArgs) new VariableCollectionEventArgs(this.propName, "", 0, this.Service.Language, Action.VariablesConnect, this.propValidVariables));
      else if (this.propValidCount > 0 && e.Action == Action.VariableDisconnect)
        this.OnCollectionDisconnected((CollectionEventArgs) new VariableCollectionEventArgs(this.propName, "", 0, this.Service.Language, Action.VariablesDisconnect, this.propValidVariables));
      else if (this.propValidCount > 0 && e.Action == Action.VariablesDisconnect)
        this.OnCollectionDisconnected((CollectionEventArgs) new VariableCollectionEventArgs(this.propName, "", 0, this.Service.Language, Action.VariablesDisconnect, this.propValidVariables));
      else if (this.propValidCount > 0 && e.Action == Action.VariableActivate)
      {
        this.OnCollectionActivated((CollectionEventArgs) new VariableCollectionEventArgs(this.propName, "", 0, this.Service.Language, Action.VariablesActivate, this.propValidVariables));
      }
      else
      {
        if (this.propValidCount <= 0 || e.Action != Action.VariableDeactivate)
          return;
        this.OnCollectionDeactivated((CollectionEventArgs) new VariableCollectionEventArgs(this.propName, "", 0, this.Service.Language, Action.VariablesDeactivate, this.propValidVariables));
      }
    }

    protected internal virtual void OnValueChanged(Variable variable, VariableEventArgs e)
    {
      if (this.ValueChanged == null)
        return;
      this.ValueChanged((object) variable, e);
    }

    protected internal virtual void OnDataValidated(Variable variable, PviEventArgs e)
    {
      ++this.propDatavalidCount;
      if (this.propDataValidVariables == null)
      {
        this.propDataValidVariables = new VariableCollection(this.Parent, "Data validated variables");
        this.propDataValidVariables.propInternalCollection = true;
      }
      if (!this.propDataValidVariables.ContainsKey((object) variable.Name))
        this.propDataValidVariables.Add(variable);
      if (this.DataValidated != null)
        this.DataValidated((object) variable, e);
      if (this.propDatavalidCount + this.propErrorCount != this.Count)
        return;
      this.OnCollectionDataValidated((CollectionEventArgs) new VariableCollectionEventArgs(this.propName, "", 0, this.Service.Language, Action.VariablesDataValid, this.propDataValidVariables));
    }

    protected internal virtual void OnActivated(Variable variable, PviEventArgs e)
    {
      ++this.propActiveCount;
      if (this.propActiveVariables == null)
      {
        this.propActiveVariables = new VariableCollection(this.Parent, "Active variables");
        this.propActiveVariables.propInternalCollection = true;
      }
      if (!this.propActiveVariables.ContainsKey((object) variable.Name))
        this.propActiveVariables.Add(variable);
      if (this.Activated != null)
        this.Activated((object) variable, e);
      if (this.propActiveCount + this.propErrorCount != this.propSentCount || (this.Requests & Actions.SetActive) == Actions.NONE)
        return;
      this.Requests &= Actions.Connect | Actions.GetList | Actions.GetValue | Actions.SetValue | Actions.GetForce | Actions.SetForce | Actions.Start | Actions.Stop | Actions.RunCycle | Actions.GetType | Actions.SetType | Actions.GetLength | Actions.SetRefresh | Actions.CreateLink | Actions.Upload | Actions.Download | Actions.SetHysteresis | Actions.Disconnect | Actions.SetInitValue | Actions.GetLBType | Actions.ModuleInfo | Actions.ListSNMPVariables | Actions.Connected | Actions.Uploading | Actions.GetCpuInfo | Actions.ReadPVFormat | Actions.FireActivated | Actions.ReadIndex | Actions.FireDataValidated | Actions.FireValueChanged | Actions.Link;
      this.OnCollectionActivated((CollectionEventArgs) new VariableCollectionEventArgs(this.propName, "", 0, this.Service.Language, Action.VariablesActivate, this.propActiveVariables));
    }

    protected internal virtual void OnDeactivated(Variable variable, PviEventArgs e)
    {
      ++this.propDeactiveCount;
      if (this.propDeactiveVariables == null)
      {
        this.propDeactiveVariables = new VariableCollection(this.Parent, "Deactive variables");
        this.propDeactiveVariables.propInternalCollection = true;
      }
      if (!this.propDeactiveVariables.ContainsKey((object) variable.Name))
        this.propDeactiveVariables.Add(variable);
      if (this.Deactivated != null)
        this.Deactivated((object) variable, e);
      if (this.propDeactiveCount + this.propErrorCount != this.propSentCount || (this.Requests & Actions.SetActive) == Actions.NONE)
        return;
      this.Requests &= Actions.Connect | Actions.GetList | Actions.GetValue | Actions.SetValue | Actions.GetForce | Actions.SetForce | Actions.Start | Actions.Stop | Actions.RunCycle | Actions.GetType | Actions.SetType | Actions.GetLength | Actions.SetRefresh | Actions.CreateLink | Actions.Upload | Actions.Download | Actions.SetHysteresis | Actions.Disconnect | Actions.SetInitValue | Actions.GetLBType | Actions.ModuleInfo | Actions.ListSNMPVariables | Actions.Connected | Actions.Uploading | Actions.GetCpuInfo | Actions.ReadPVFormat | Actions.FireActivated | Actions.ReadIndex | Actions.FireDataValidated | Actions.FireValueChanged | Actions.Link;
      this.OnCollectionDeactivated((CollectionEventArgs) new VariableCollectionEventArgs(this.propName, "", 0, this.Service.Language, Action.VariablesDeactivate, this.propDeactiveVariables));
    }

    protected internal virtual void OnValueRead(Variable variable, PviEventArgs e)
    {
      ++this.propValidCount;
      if (this.propValidVariables == null)
      {
        this.propValidVariables = new VariableCollection(this.Parent, "Valid variables");
        this.propValidVariables.propInternalCollection = true;
      }
      if (!this.propValidVariables.ContainsKey((object) variable.Name))
        this.propValidVariables.Add(variable);
      if (this.ValueRead != null)
        this.ValueRead((object) variable, e);
      if (this.propValidCount + this.propErrorCount != this.propSentCount || (this.Requests & Actions.GetValue) == Actions.NONE)
        return;
      this.Requests &= Actions.Connect | Actions.GetList | Actions.SetValue | Actions.GetForce | Actions.SetForce | Actions.Start | Actions.Stop | Actions.RunCycle | Actions.GetType | Actions.SetType | Actions.GetLength | Actions.SetRefresh | Actions.CreateLink | Actions.Upload | Actions.Download | Actions.SetActive | Actions.SetHysteresis | Actions.Disconnect | Actions.SetInitValue | Actions.GetLBType | Actions.ModuleInfo | Actions.ListSNMPVariables | Actions.Connected | Actions.Uploading | Actions.GetCpuInfo | Actions.ReadPVFormat | Actions.FireActivated | Actions.ReadIndex | Actions.FireDataValidated | Actions.FireValueChanged | Actions.Link;
      this.OnCollectionValuesRead((CollectionEventArgs) new VariableCollectionEventArgs(this.propName, "", 0, this.Service.Language, Action.VariablesValuesRead, this.propValidVariables));
      if (this.propErrorCount <= 0)
        return;
      this.OnCollectionError((CollectionEventArgs) new VariableCollectionEventArgs(this.propName, "", 0, this.Service.Language, Action.VariablesValuesRead, this.propErrorVariables));
    }

    protected internal virtual void OnValueWritten(Variable variable, PviEventArgs e)
    {
      ++this.propValidCount;
      if (this.propValidVariables == null)
      {
        this.propValidVariables = new VariableCollection(this.Parent, "Valid variables");
        this.propValidVariables.propInternalCollection = true;
      }
      if (!this.propValidVariables.ContainsKey((object) variable.Name))
        this.propValidVariables.Add(variable);
      if (this.ValueWritten != null)
        this.ValueWritten((object) variable, e);
      if (this.propValidCount + this.propErrorCount != this.propSentCount || (this.Requests & Actions.SetValue) == Actions.NONE)
        return;
      this.Requests &= Actions.Connect | Actions.GetList | Actions.GetValue | Actions.GetForce | Actions.SetForce | Actions.Start | Actions.Stop | Actions.RunCycle | Actions.GetType | Actions.SetType | Actions.GetLength | Actions.SetRefresh | Actions.CreateLink | Actions.Upload | Actions.Download | Actions.SetActive | Actions.SetHysteresis | Actions.Disconnect | Actions.SetInitValue | Actions.GetLBType | Actions.ModuleInfo | Actions.ListSNMPVariables | Actions.Connected | Actions.Uploading | Actions.GetCpuInfo | Actions.ReadPVFormat | Actions.FireActivated | Actions.ReadIndex | Actions.FireDataValidated | Actions.FireValueChanged | Actions.Link;
      this.OnCollectionValuesWritten((CollectionEventArgs) new VariableCollectionEventArgs(this.propName, "", 0, this.Service.Language, Action.VariableValueWrite, this.propValidVariables));
      if (this.propErrorCount <= 0)
        return;
      this.OnCollectionError((CollectionEventArgs) new VariableCollectionEventArgs(this.propName, "", 0, this.Service.Language, Action.VariablesValuesRead, this.propErrorVariables));
    }

    protected internal virtual void OnPropertyChanged(Variable variable, PviEventArgs e)
    {
      ++this.propValidCount;
      if (this.propValidVariables == null)
      {
        this.propValidVariables = new VariableCollection(this.Parent, "Valid variables");
        this.propValidVariables.propInternalCollection = true;
      }
      if (!this.propValidVariables.ContainsKey((object) variable.Name))
        this.propValidVariables.Add(variable);
      if (this.PropertyChanged != null)
        this.PropertyChanged((object) variable, e);
      if (this.propValidCount + this.propErrorCount != this.propSentCount)
        return;
      this.OnCollectionPropertyChanged((CollectionEventArgs) new VariableCollectionEventArgs(this.propName, "", 0, this.Service.Language, e.Action, this.propValidVariables));
      if (this.propErrorCount <= 0)
        return;
      this.OnCollectionError((CollectionEventArgs) new VariableCollectionEventArgs(this.propName, "", 0, this.Service.Language, e.Action, this.propErrorVariables));
    }

    protected internal virtual void OnCollectionDataValidated(CollectionEventArgs e)
    {
      if (this.CollectionDataValidated == null)
        return;
      this.CollectionDataValidated((object) this, e);
    }

    protected internal virtual void OnCollectionActivated(CollectionEventArgs e)
    {
      if (this.CollectionActivated == null)
        return;
      this.CollectionActivated((object) this, e);
    }

    protected internal virtual void OnCollectionDeactivated(CollectionEventArgs e)
    {
      if (this.CollectionDeactivated == null)
        return;
      this.CollectionDeactivated((object) this, e);
    }

    protected internal virtual void OnCollectionValuesRead(CollectionEventArgs e)
    {
      if (this.CollectionValuesRead == null)
        return;
      this.CollectionValuesRead((object) this, e);
    }

    protected internal virtual void OnCollectionValuesWritten(CollectionEventArgs e)
    {
      if (this.CollectionValuesWritten == null)
        return;
      this.CollectionValuesWritten((object) this, e);
    }

    protected internal virtual void OnCollectionPropertyChanged(CollectionEventArgs e)
    {
      if (this.CollectionPropertyChanged == null)
        return;
      this.CollectionPropertyChanged((object) this, e);
    }

    public void Upload()
    {
      if (this.propParent is Cpu && !((Base) this.propParent).IsConnected && this.Service.WaitForParentConnection)
        this.Requests |= Actions.Upload;
      else if (this.propParent is Service && !((Base) this.propParent).IsConnected && this.Service.WaitForParentConnection)
        this.Requests |= Actions.Upload;
      else if (this.propParent is Task && !((Base) this.propParent).IsConnected && this.Service.WaitForParentConnection)
      {
        this.Requests |= Actions.Upload;
      }
      else
      {
        int error = !(this.propParent is Cpu) ? (!(this.propParent is Task) ? (!(this.propParent is Service) ? -1 : this.ReadArgumentRequest(((Base) this.propParent).Service.hPvi, ((Base) this.propParent).LinkId, AccessTypes.List, IntPtr.Zero, 0, 614U, this.InternId)) : this.ReadArgumentRequest(((Base) this.propParent).Service.hPvi, ((Base) this.propParent).LinkId, AccessTypes.ListVariable, IntPtr.Zero, 0, 614U, this.InternId)) : this.ReadArgumentRequest(((Base) this.propParent).Service.hPvi, ((Base) this.propParent).LinkId, AccessTypes.ListVariable, IntPtr.Zero, 0, 614U, this.InternId);
        if (error == 0)
          return;
        this.OnError((Variable) null, (PviEventArgs) new VariableCollectionEventArgs(((Base) this.propParent).Name, ((Base) this.propParent).Address, error, this.Service.Language, Action.VariablesUpload, (VariableCollection) null));
      }
    }

    protected internal override void OnUploaded(PviEventArgs e) => base.OnUploaded(e);

    public void ReadValues()
    {
      this.propValidCount = 0;
      this.propSentCount = 0;
      this.propErrorCount = 0;
      this.Requests |= Actions.GetValue;
      if (this.propValidVariables != null)
      {
        this.propValidVariables.Clear();
      }
      else
      {
        this.propValidVariables = new VariableCollection(this.Parent, "Valid variables");
        this.propValidVariables.propInternalCollection = true;
      }
      if (this.propErrorVariables != null)
      {
        this.propErrorVariables.Clear();
      }
      else
      {
        this.propErrorVariables = new VariableCollection(this.Parent, "Error variables");
        this.propErrorVariables.propInternalCollection = true;
      }
      if (this.propCollectionType != CollectionType.HashTable || 0 >= this.Count)
        return;
      foreach (Variable variable in (IEnumerable) this.Values)
      {
        ++this.propSentCount;
        variable.ReadValue();
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
      string[] strArray = (string[]) null;
      bool isMDimArray = false;
      if (PVIReadAccessTypes.Variables == accessType)
      {
        if (errorCode == 0 && dataLen > 0U)
        {
          Variable variable1 = (Variable) null;
          string stringAnsi = PviMarshal.PtrToStringAnsi(pData, dataLen);
          strArray = (string[]) null;
          if (stringAnsi != null && 1 < stringAnsi.Length)
          {
            foreach (string str in stringAnsi.Split("\t".ToCharArray()))
            {
              int length = str.IndexOf("\0");
              if (-1 != length)
                str = str.Substring(0, length);
              if (this.propParent is Cpu && !((Cpu) this.propParent).Variables.ContainsKey((object) Variable.GetVariableName(str)))
                variable1 = new Variable((Cpu) this.propParent, true, Variable.GetVariableName(str), this);
              if (this.propParent is Task)
              {
                if (Scope.Global != Variable.EvaluateScope(str))
                {
                  if (!((Task) this.propParent).Variables.ContainsKey((object) Variable.GetVariableName(str)))
                  {
                    variable1 = new Variable((Task) this.propParent, true, Variable.GetVariableName(str), this);
                    variable1.propScope = Scope.Local;
                  }
                }
                else
                {
                  Variable variable2;
                  if ((variable2 = ((Module) this.propParent).propCpu.Variables[Variable.GetVariableName(str)]) == null)
                  {
                    variable1 = new Variable(((Module) this.propParent).Cpu, true, Variable.GetVariableName(str), ((Module) this.propParent).Cpu.Variables);
                    variable1.propScope = Scope.Global;
                    ((Task) this.propParent).propGlobals.Add(variable1);
                  }
                  else if (variable2.Name != null && !((Task) this.propParent).propGlobals.ContainsKey((object) variable2.Name))
                    ((Task) this.propParent).propGlobals.Add(variable2);
                }
              }
              if (variable1 != null)
              {
                variable1.GetScope(str, ref variable1.propScope);
                variable1.GetExtendedAttributes(str, ref isMDimArray, ref variable1.propPviValue);
                if (isMDimArray)
                  variable1.propPviValue.SetArrayIndex(str);
                variable1.propPviValue.SetDataType(Variable.GetDataType(str, variable1.Value.IsBitString, ref variable1.propPviValue.propTypeLength));
                variable1.propPviValue.SetArrayLength(Variable.GetArrayLength(str));
                if (variable1.propPviValue.ArrayMinIndex == 0)
                  variable1.propPviValue.propArrayMaxIndex = variable1.propPviValue.ArrayLength - 1;
                variable1.propPviValue.propTypeLength = Variable.GetDataTypeLength(str, variable1.propPviValue.propTypeLength);
              }
              variable1 = (Variable) null;
            }
          }
          this.OnUploaded(new PviEventArgs(this.propName, "", errorCode, this.Service.Language, Action.VariablesUpload, this.Service));
        }
        else
        {
          this.OnUploaded(new PviEventArgs(this.propName, "", errorCode, this.Service.Language, Action.VariablesUpload, this.Service));
          this.OnError((Variable) null, (PviEventArgs) new VariableCollectionEventArgs(this.propName, "", errorCode, this.Service.Language, Action.VariablesUpload, (VariableCollection) null));
        }
      }
      else if (PVIReadAccessTypes.ChildObjects == accessType)
      {
        if (errorCode == 0 && dataLen > 0U)
        {
          string str = PviMarshal.PtrToStringAnsi(pData, dataLen);
          int length = str.IndexOf("\0");
          if (-1 != length)
            str = str.Substring(0, length);
          strArray = (string[]) null;
          if (str != "")
          {
            foreach (string pviString in str.Split("\t".ToCharArray()))
            {
              if (pviString.Split(" ".ToCharArray())[1].CompareTo("OT=Pvar") == 0 && this.propParent is Service && !((Service) this.propParent).Variables.ContainsKey((object) Variable.GetVariableName(pviString)))
              {
                Variable variable = new Variable((Service) this.propParent, Variable.GetVariableName(pviString), true);
              }
            }
          }
          this.OnUploaded(new PviEventArgs(this.propName, "", errorCode, this.Service.Language, Action.VariablesUpload, this.Service));
        }
        else
        {
          this.OnUploaded(new PviEventArgs(this.propName, "", errorCode, this.Service.Language, Action.VariablesUpload, this.Service));
          this.OnError((Variable) null, (PviEventArgs) new VariableCollectionEventArgs(this.propName, "", errorCode, this.Service.Language, Action.VariablesUpload, (VariableCollection) null));
        }
      }
      else
        base.OnPviRead(errorCode, accessType, dataState, pData, dataLen, option);
    }

    internal override int ToXMLTextWriter(ref XmlTextWriter writer, ConfigurationFlags flags)
    {
      int xmlTextWriter1 = 0;
      if (this.Count > 0)
      {
        writer.WriteStartElement(this.GetType().Name);
        foreach (Base @base in (IEnumerable) this.Values)
        {
          int xmlTextWriter2 = @base.ToXMLTextWriter(ref writer, flags);
          if (xmlTextWriter2 != 0)
            xmlTextWriter1 = xmlTextWriter2;
        }
        writer.WriteEndElement();
      }
      return xmlTextWriter1;
    }

    internal override void Dispose(bool disposing, bool removeFromCollection)
    {
      if (this.propDisposed)
        return;
      if (this.propErrorVariables != null)
        this.propErrorVariables.Dispose(disposing, removeFromCollection);
      if (this.propValidVariables != null)
        this.propValidVariables.Dispose(disposing, removeFromCollection);
      if (this.propDataValidVariables != null)
        this.propDataValidVariables.Dispose(disposing, removeFromCollection);
      if (this.propActiveVariables != null)
        this.propActiveVariables.Dispose(disposing, removeFromCollection);
      if (this.propDeactiveVariables != null)
        this.propDeactiveVariables.Dispose(disposing, removeFromCollection);
      this.propScaling = (Scaling) null;
      this.CleanUp(disposing);
      base.Dispose(disposing, removeFromCollection);
    }

    public override void Clear()
    {
      if (!this.propInternalCollection)
      {
        foreach (Variable variable in (IEnumerable) this.Values)
        {
          if (variable.propUserCollections != null)
            variable.propUserCollections.Remove(this.Name);
        }
      }
      base.Clear();
    }

    internal void CleanUp(bool disposing)
    {
      this.propCounter = 0;
      ArrayList arrayList = new ArrayList();
      if (this.Values != null)
      {
        foreach (Variable variable in (IEnumerable) this.Values)
          arrayList.Add((object) variable);
        for (int index = 0; index < arrayList.Count; ++index)
        {
          Variable variable = (Variable) arrayList[index];
          if (variable.LinkId != 0U)
            variable.Disconnect(0U);
          variable.Dispose(disposing, true);
        }
      }
      arrayList.Clear();
      this.Clear();
    }

    public Variable this[string name] => (Variable) this[(object) name];

    internal bool DataValid => this.Count == this.propDatavalidCount;

    public bool Active
    {
      get => this.propActive;
      set
      {
        this.Requests |= Actions.SetActive;
        this.propActive = value;
        this.propActiveCount = 0;
        this.propDeactiveCount = 0;
        this.propErrorCount = 0;
        this.propSentCount = 0;
        if (this.propActiveVariables != null)
          this.propActiveVariables.Clear();
        if (this.propDeactiveVariables != null)
          this.propDeactiveVariables.Clear();
        if (this.propErrorVariables != null)
          this.propErrorVariables.Clear();
        if (this.Count == 0)
        {
          this.Requests &= Actions.Connect | Actions.GetList | Actions.GetValue | Actions.SetValue | Actions.GetForce | Actions.SetForce | Actions.Start | Actions.Stop | Actions.RunCycle | Actions.GetType | Actions.SetType | Actions.GetLength | Actions.SetRefresh | Actions.CreateLink | Actions.Upload | Actions.Download | Actions.SetHysteresis | Actions.Disconnect | Actions.SetInitValue | Actions.GetLBType | Actions.ModuleInfo | Actions.ListSNMPVariables | Actions.Connected | Actions.Uploading | Actions.GetCpuInfo | Actions.ReadPVFormat | Actions.FireActivated | Actions.ReadIndex | Actions.FireDataValidated | Actions.FireValueChanged | Actions.Link;
          if (value)
            this.OnCollectionActivated((CollectionEventArgs) new VariableCollectionEventArgs(this.propName, "", 0, this.Service.Language, Action.VariablesActivate, this.propActiveVariables));
          else
            this.OnCollectionDeactivated((CollectionEventArgs) new VariableCollectionEventArgs(this.propName, "", 0, this.Service.Language, Action.VariablesDeactivate, this.propDeactiveVariables));
        }
        else
        {
          if (this.propCollectionType != CollectionType.HashTable)
            return;
          foreach (Variable variable in (IEnumerable) this.Values)
          {
            if (this.propActive)
            {
              ++this.propSentCount;
              if (variable.Active)
              {
                if (variable.ErrorCode != 0)
                  ++this.propErrorCount;
                else
                  ++this.propActiveCount;
              }
              else if (this.propValidVariables != null)
                variable.Active = this.propActive;
            }
            else
            {
              if (!variable.Active)
              {
                if (variable.ErrorCode != 0)
                  ++this.propErrorCount;
                else
                  ++this.propDeactiveCount;
              }
              else if (this.propValidVariables != null)
                variable.Active = this.propActive;
              ++this.propSentCount;
            }
          }
          if (this.propSentCount != this.propErrorCount)
            return;
          if (value)
            this.OnCollectionActivated((CollectionEventArgs) new VariableCollectionEventArgs(this.propName, "", 0, this.Service.Language, Action.VariablesActivate, this.propActiveVariables));
          else
            this.OnCollectionDeactivated((CollectionEventArgs) new VariableCollectionEventArgs(this.propName, "", 0, this.Service.Language, Action.VariablesDeactivate, this.propDeactiveVariables));
        }
      }
    }

    public int RefreshTime
    {
      get => this.propRefreshTime;
      set
      {
        this.propRefreshTime = value;
        this.propValidCount = 0;
        this.propSentCount = 0;
        this.propErrorCount = 0;
        if (this.propValidVariables != null)
        {
          this.propValidVariables.Clear();
        }
        else
        {
          this.propValidVariables = new VariableCollection(this.Parent, "Valid variables");
          this.propValidVariables.propInternalCollection = true;
        }
        if (this.propErrorVariables != null)
        {
          this.propErrorVariables.Clear();
        }
        else
        {
          this.propErrorVariables = new VariableCollection(this.Parent, "Error variables");
          this.propErrorVariables.propInternalCollection = true;
        }
        if (this.propCollectionType != CollectionType.HashTable || 0 >= this.Count)
          return;
        foreach (Variable variable in (IEnumerable) this.Values)
        {
          ++this.propSentCount;
          variable.RefreshTime = this.propRefreshTime;
        }
      }
    }

    public bool WriteValueAutomatic
    {
      get => this.propWriteValueAutomatic;
      set
      {
        this.propWriteValueAutomatic = value;
        if (0 >= this.Count)
          return;
        foreach (Variable variable in (IEnumerable) this.Values)
          variable.WriteValueAutomatic = this.propWriteValueAutomatic;
      }
    }

    public double Hysteresis
    {
      get => this.propHysteresis;
      set
      {
        this.propHysteresis = value;
        this.propValidCount = 0;
        this.propSentCount = 0;
        this.propErrorCount = 0;
        if (this.propValidVariables != null)
        {
          this.propValidVariables.Clear();
        }
        else
        {
          this.propValidVariables = new VariableCollection(this.Parent, "Valid variables");
          this.propValidVariables.propInternalCollection = true;
        }
        if (this.propErrorVariables != null)
        {
          this.propErrorVariables.Clear();
        }
        else
        {
          this.propErrorVariables = new VariableCollection(this.Parent, "Error variables");
          this.propErrorVariables.propInternalCollection = true;
        }
        if (this.propCollectionType != CollectionType.HashTable || 0 >= this.Count)
          return;
        foreach (Variable variable in (IEnumerable) this.Values)
        {
          ++this.propSentCount;
          variable.Hysteresis = this.propHysteresis;
        }
      }
    }

    [CLSCompliant(false)]
    public Scaling Scaling
    {
      get => this.propScaling;
      set
      {
        this.propScaling = value;
        if (this.propCollectionType != CollectionType.HashTable || 0 >= this.Count)
          return;
        foreach (Variable variable in (IEnumerable) this.Values)
          variable.propScaling = this.propScaling;
      }
    }

    public bool Polling
    {
      get => this.propPolling;
      set
      {
        this.propPolling = value;
        this.propValidCount = 0;
        this.propSentCount = 0;
        this.propErrorCount = 0;
        if (this.propValidVariables != null)
        {
          this.propValidVariables.Clear();
        }
        else
        {
          this.propValidVariables = new VariableCollection(this.Parent, "Valid variables");
          this.propValidVariables.propInternalCollection = true;
        }
        if (this.propErrorVariables != null)
        {
          this.propErrorVariables.Clear();
        }
        else
        {
          this.propErrorVariables = new VariableCollection(this.Parent, "Error variables");
          this.propErrorVariables.propInternalCollection = true;
        }
        if (this.propCollectionType != CollectionType.HashTable || 0 >= this.Count)
          return;
        foreach (Variable variable in (IEnumerable) this.Values)
        {
          ++this.propSentCount;
          variable.Polling = this.propPolling;
        }
      }
    }

    public Access Access
    {
      get => this.propVariableAccess;
      set
      {
        this.propVariableAccess = value;
        this.propValidCount = 0;
        this.propSentCount = 0;
        this.propErrorCount = 0;
        if (this.propValidVariables != null)
        {
          this.propValidVariables.Clear();
        }
        else
        {
          this.propValidVariables = new VariableCollection(this.Parent, "Valid variables");
          this.propValidVariables.propInternalCollection = true;
        }
        if (this.propErrorVariables != null)
        {
          this.propErrorVariables.Clear();
        }
        else
        {
          this.propErrorVariables = new VariableCollection(this.Parent, "Error variables");
          this.propErrorVariables.propInternalCollection = true;
        }
        if (this.propCollectionType != CollectionType.HashTable || 0 >= this.Count)
          return;
        foreach (Variable variable in (IEnumerable) this.Values)
        {
          ++this.propSentCount;
          variable.Access = this.propVariableAccess;
        }
      }
    }

    public event CollectionEventHandler CollectionDataValidated;

    public event CollectionEventHandler CollectionActivated;

    public event CollectionEventHandler CollectionDeactivated;

    public event CollectionEventHandler CollectionValuesRead;

    public event CollectionEventHandler CollectionValuesWritten;

    public event CollectionEventHandler CollectionPropertyChanged;

    public event PviEventHandler DataValidated;

    public event PviEventHandler Activated;

    public event PviEventHandler Deactivated;

    public event PviEventHandler ValueRead;

    public event PviEventHandler ValueWritten;

    public event VariableEventHandler ValueChanged;

    public event PviEventHandler PropertyChanged;
  }
}
