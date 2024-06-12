// Decompiled with JetBrains decompiler
// Type: BR.AN.PviServices.Variable
// Assembly: BR.AN.PVIServices, Version=1.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DA853F0A-C070-4E30-B7A7-51D955FB85F6
// Assembly location: C:\Users\ss_six\Desktop\FXE\FXE software integration package\COMET_FXE-PVIServices_Example\Pluto-PVI-trial_01\packages\IToolS.BandR.3.5.58\lib\net20\BR.AN.PVIServices.dll

using System;
using System.Collections;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Xml;

namespace BR.AN.PviServices
{
  public class Variable : Base
  {
    protected StructMemberCollection mapNameToMember;
    private bool propWaitingOnReadEvent;
    internal int propErrorState;
    private bool isMemberVar;
    private bool propReadingState;
    private bool isStruct;
    private IntPtr pWriteData;
    private IntPtr pReadData;
    private int propStrDataLen;
    private byte[] propWriteByteField;
    private byte[] propReadByteField;
    internal bool propSendChangedEvent;
    private bool propExpandMembers;
    private Value propInitialValue;
    private Variable.ROIoptions propROI;
    private string propInitValue;
    private CastModes propCastMode;
    private int propBitOffset;
    private string[] propChangedStructMembers;
    private string propStructName;
    internal Value propInternalValue;
    internal byte[] propInternalByteField;
    internal Base propOwner;
    internal Value propPviValue;
    internal bool propDataValid;
    internal int propRefreshTime;
    internal double propHysteresis;
    internal MemberCollection propMembers;
    internal bool propForceValue;
    internal VariableAttribute propAttribute;
    internal bool propActive;
    internal bool isServiceUploaded;
    internal bool propWriteValueAutomatic;
    internal Scope propScope;
    internal IConvert propConvert;
    internal int propAlignment;
    internal int propOffset;
    internal int propInternalOffset;
    internal MemberCollection propUserMembers;
    internal bool propReadOnly;
    internal ConnectionType propPVState;
    internal bool propSendUploadEvent;
    internal Hashtable propUserCollections;
    internal Access propVariableAccess;
    internal bool propPolling;
    internal ScalingPointCollection propScalingPoints;
    internal IODataPointCollection propIODataPoints;
    internal Scaling propScaling;
    internal string propUserTag;
    internal bool propWaitForUserTag;
    internal bool propStatusRead;
    private uint propPviInternStructElement;

    private void InitializeMembers()
    {
      this.isStruct = false;
      this.propReadingState = false;
      this.isMemberVar = false;
      this.propWaitingOnReadEvent = false;
      this.propWriteByteField = (byte[]) null;
      this.propReadByteField = (byte[]) null;
      this.propErrorState = 0;
      this.isServiceUploaded = false;
      this.propExpandMembers = false;
      this.pWriteData = IntPtr.Zero;
      this.pReadData = IntPtr.Zero;
      this.propStrDataLen = 0;
      this.propBitOffset = -1;
      this.propSendChangedEvent = false;
      this.propChangedStructMembers = new string[0];
      this.propStructName = (string) null;
      this.propInitValue = (string) null;
      this.propCastMode = CastModes.DEFAULT;
      this.mapNameToMember = (StructMemberCollection) null;
      this.propOwner = (Base) null;
      this.propDataValid = false;
      this.propRefreshTime = 100;
      this.propOffset = 0;
      this.propInternalOffset = 0;
    }

    protected void Initialize(Base pObj, Base oObj, bool expandMembers, bool automaticWrite)
    {
      this.propROI = Variable.ROIoptions.OFF;
      this.propAlignment = 1;
      this.propInitialValue = (Value) null;
      this.InitializeMembers();
      this.propExpandMembers = expandMembers;
      this.propWriteValueAutomatic = automaticWrite;
      this.propOwner = oObj;
    }

    private void ResetReadDataPtr(int newSize)
    {
      if (this.propStrDataLen < newSize)
      {
        if (this.pReadData == IntPtr.Zero)
          PviMarshal.FreeHGlobal(ref this.pReadData);
        this.propStrDataLen = newSize;
        this.pReadData = PviMarshal.AllocHGlobal(this.propStrDataLen);
        this.propReadByteField = (byte[]) null;
        this.propReadByteField = new byte[this.propStrDataLen];
        if (this.propPviValue.propByteField != null)
          return;
        this.propPviValue.propByteField = new byte[this.propPviValue.DataSize];
      }
      else
      {
        if (this.propReadByteField == null)
          this.propReadByteField = new byte[this.propPviValue.DataSize];
        for (int index = 0; index < this.propReadByteField.Length; ++index)
          this.propReadByteField.SetValue((object) (byte) 0, index);
      }
    }

    private void ResetWriteDataPtr(Variable var, int newSize) => this.ResetWriteDataPtr(var, newSize, false);

    private void ResetWriteDataPtr(Variable var, int newSize, bool setZero)
    {
      if (var.propStrDataLen < newSize || var.propWriteByteField == null)
      {
        if (var.pWriteData != IntPtr.Zero)
          PviMarshal.FreeHGlobal(ref var.pWriteData);
        var.propStrDataLen = newSize;
        var.pWriteData = PviMarshal.AllocHGlobal(var.propStrDataLen);
        var.propWriteByteField = (byte[]) null;
        var.propWriteByteField = new byte[var.propStrDataLen];
        if (var.propPviValue.propByteField == null)
          var.propPviValue.propByteField = new byte[var.propPviValue.DataSize];
      }
      if (!setZero)
        return;
      for (int index = 0; index < var.propWriteByteField.Length; ++index)
        var.propWriteByteField[index] = (byte) 0;
    }

    internal void ResizePviDataPtr(int newSize)
    {
      if (this.propPviValue.propDataSize >= newSize && !(this.propPviValue.pData == IntPtr.Zero))
        return;
      PviMarshal.FreeHGlobal(ref this.propPviValue.pData);
      this.propPviValue.propDataSize = newSize;
      this.propPviValue.pData = PviMarshal.AllocHGlobal(this.propPviValue.propDataSize);
      this.propPviValue.propHasOwnDataPtr = true;
    }

    internal Variable() => this.Initialize((Base) null, (Base) null, true, true);

    internal Variable(Cpu cpu)
      : base((Base) cpu)
    {
      this.Initialize((Base) null, (Base) null, true, true);
    }

    public Variable(Service service, string name)
      : base((Base) service, name)
    {
      if (service != null && service.Variables[name] != null)
        throw new ArgumentException("There is already an object in \"" + service.Name + ".Variables\" which has the same name! Use a different name and the same address or use the object from \"" + service.Name + ".Variables\".", name);
      this.Initialize((Base) service, (Base) service, true, true);
      this.Init(name);
      service.Variables.Add(this);
    }

    internal Variable(SimpleNetworkManagementProtocol snmp, string name)
      : base((Base) snmp.Service, name)
    {
      this.InitializeMembers();
      this.propSNMPParent = (SNMPBase) snmp;
      this.Init(name);
    }

    internal Variable(NetworkAdapter nwAdapter, string name)
      : base((Base) nwAdapter.Service, name)
    {
      this.InitializeMembers();
      this.propSNMPParent = (SNMPBase) nwAdapter;
      this.Init(name);
    }

    public Variable(Service service, string name, bool isUploded)
      : base((Base) service, name)
    {
      if (service != null && service.Variables[name] != null)
        throw new ArgumentException("There is already an object in \"" + service.Name + ".Variables\" which has the same name! Use a different name and the same address or use the object from \"" + service.Name + ".Variables\".", name);
      this.Initialize((Base) service, (Base) service, true, true);
      this.Init(name, isUploded);
      service.Variables.Add(this);
    }

    public Variable(Service service, bool expandMembers, string name)
      : base((Base) service, name)
    {
      if (service != null && service.Variables[name] != null)
        throw new ArgumentException("There is already an object in \"" + service.Name + ".Variables\" which has the same name! Use a different name and the same address or use the object from \"" + service.Name + ".Variables\".", name);
      this.Initialize((Base) service, (Base) service, expandMembers, true);
      this.Init(name);
      service.Variables.Add(this);
    }

    public Variable(Cpu cpu, string name)
      : base((Base) cpu, name)
    {
      if (cpu != null && cpu.Variables[name] != null)
        throw new ArgumentException("There is already an object in \"" + cpu.Name + ".Variables\" which has the same name! Use a different name and the same address or use the object from \"" + cpu.Name + ".Variables\".", name);
      this.Initialize((Base) cpu, (Base) cpu, true, true);
      this.Init(name);
      cpu.Variables.Add(this);
    }

    public Variable(Cpu cpu, bool expandMembers, string name)
      : base((Base) cpu, name)
    {
      if (cpu != null && cpu.Variables[name] != null)
        throw new ArgumentException("There is already an object in \"" + cpu.Name + ".Variables\" which has the same name! Use a different name and the same address or use the object from \"" + cpu.Name + ".Variables\".", name);
      this.Initialize((Base) cpu, (Base) cpu, expandMembers, true);
      this.Init(name);
      cpu.Variables.Add(this);
    }

    public Variable(Task task, string name)
      : base((Base) task, name)
    {
      if (task != null && task.Variables[name] != null)
        throw new ArgumentException("There is already an object in \"" + task.Name + ".Variables\" which has the same name! Use a different name and the same address or use the object from \"" + task.Name + ".Variables\".", name);
      this.Initialize((Base) task, (Base) task, true, true);
      this.Init(name);
      task.Variables.Add(this);
    }

    public Variable(Task task, bool expandMembers, string name)
      : base((Base) task, name)
    {
      if (task != null && task.Variables[name] != null)
        throw new ArgumentException("There is already an object in \"" + task.Name + ".Variables\" which has the same name! Use a different name and the same address or use the object from \"" + task.Name + ".Variables\".", name);
      this.Initialize((Base) task, (Base) task, expandMembers, true);
      this.Init(name);
      task.Variables.Add(this);
    }

    public Variable(Variable variable, string name)
      : base((Base) variable, name)
    {
      if (variable != null && variable.propMembers[name] != null)
        throw new ArgumentException("There is already an object in \"" + variable.Name + ".Members\" which has the same name! Use the object from \"" + variable.Name + ".Members\".", name);
      this.Initialize(variable.Parent, (Base) variable, true, variable.WriteValueAutomatic);
      this.Init(name);
      this.propWriteValueAutomatic = variable.WriteValueAutomatic;
      if (variable.Members == null)
        variable.propMembers = new MemberCollection(variable, variable.Address);
      variable.AddMember(this);
    }

    public Variable(Variable variable, bool expandMembers, string name)
      : base((Base) variable, name)
    {
      if (variable != null && variable.propMembers[name] != null)
        throw new ArgumentException("There is already an object in \"" + variable.Name + ".Members\" which has the same name! Use the object from \"" + variable.Name + ".Members\".", name);
      this.Initialize(variable.Parent, (Base) variable, expandMembers, variable.WriteValueAutomatic);
      this.Init(name);
      this.propWriteValueAutomatic = variable.WriteValueAutomatic;
      variable.AddMember(this);
    }

    public Variable(bool isMember, Variable variable, string name, bool addToVColls)
      : base((Base) variable, name, addToVColls)
    {
      this.Initialize(variable.Parent, (Base) variable, true, variable.WriteValueAutomatic);
      this.Init(name);
      this.propWriteValueAutomatic = variable.WriteValueAutomatic;
      this.isMemberVar = isMember;
      variable.AddMember(this);
      this.propPviValue.InitializeExtendedAttributes();
    }

    internal Variable(
      string name,
      Variable parentVar,
      bool addToVCollections,
      int offset,
      int alignment,
      Scope vScope)
      : base((Base) parentVar, name, addToVCollections)
    {
      this.Initialize(parentVar.Parent, (Base) parentVar, true, parentVar.WriteValueAutomatic);
      this.Init(name);
      this.propWriteValueAutomatic = parentVar.WriteValueAutomatic;
      this.isMemberVar = true;
      this.propAlignment = alignment;
      this.propScope = vScope;
      this.Address = parentVar.Address + name;
      this.propPviValue.propTypeLength = parentVar.Value.TypeLength;
      this.propPviValue.SetArrayLength(1);
      if (this.propPviValue.ArrayMinIndex == 0)
        this.propPviValue.propArrayMaxIndex = this.propPviValue.ArrayLength - 1;
      this.propOffset = offset;
      this.propPviValue.SetDataType(parentVar.Value.DataType);
      this.propStructName = parentVar.propStructName;
      this.propPviValue.propDerivedFrom = parentVar.Value.DerivedFrom;
      this.propPviValue.propEnumerations = parentVar.Value.Enumerations;
      this.propPviValue.SetDataType(parentVar.Value.DataType);
      parentVar.AddMember(this);
      this.propPviValue.InitializeExtendedAttributes();
    }

    internal void CloneVariable(
      Variable varClone,
      Variable root,
      Variable parentVar,
      bool addToVCollections,
      bool bAddToAll)
    {
      int propOffset = this.propOffset;
      if (bAddToAll)
        this.AddStructMembers(parentVar, this);
      else
        root.AddStructMember(this.GetStructMemberName(root), this);
      this.AddToParentCollection(this, this.propParent, addToVCollections);
      if (DataType.Structure == this.propPviValue.DataType)
      {
        this.CreateNestedStructClone(root, parentVar, this, varClone, ref propOffset, 0, addToVCollections, bAddToAll);
      }
      else
      {
        this.propAlignment = varClone.propAlignment;
        this.propScope = root.propScope;
        this.propPviValue.Clone(varClone.propPviValue);
        this.propStructName = varClone.propStructName;
        this.propOffset = propOffset;
        if (!this.propPviValue.IsOfTypeArray)
          return;
        this.CreateNestedStructClone(root, parentVar, this, varClone, ref propOffset, 0, addToVCollections, bAddToAll);
      }
    }

    public Variable(Variable variable, string name, bool addToVColls)
      : base((Base) variable, name, addToVColls)
    {
      if (variable != null && variable.propMembers[name] != null)
        throw new ArgumentException("There is already an object in \"" + variable.Name + ".Members\" which has the same name! Use the object from \"" + variable.Name + ".Members\".", name);
      this.Initialize(variable.Parent, (Base) variable, true, variable.WriteValueAutomatic);
      this.Init(name);
      this.propWriteValueAutomatic = variable.WriteValueAutomatic;
      variable.AddMember(this);
    }

    public Variable(Variable variable, bool expandMembers, string name, bool memberOnly)
      : base((Base) variable, name, memberOnly)
    {
      if (variable != null && variable.propMembers[name] != null)
        throw new ArgumentException("There is already an object in \"" + variable.Name + ".Members\" which has the same name! Use the object from \"" + variable.Name + ".Members\".", name);
      this.Initialize(variable.Parent, (Base) variable, expandMembers, variable.WriteValueAutomatic);
      this.Init(name);
      this.propWriteValueAutomatic = variable.WriteValueAutomatic;
      variable.AddMember(this);
    }

    internal Variable(Cpu cpu, bool expandMembers, string name, VariableCollection collection)
      : base((Base) cpu, name)
    {
      this.Initialize((Base) cpu, (Base) cpu, expandMembers, true);
      this.Init(name);
      collection.Add(this);
    }

    internal Variable(Task task, bool expandMembers, string name, VariableCollection collection)
      : base((Base) task, name)
    {
      this.Initialize((Base) task, (Base) task, expandMembers, true);
      this.Init(name);
      collection.Add(this);
    }

    internal Variable(Base parentObj, bool expandMembers, string name, string address)
      : base(parentObj, name)
    {
      this.Initialize(parentObj, parentObj, expandMembers, true);
      string[] strArray = address.Split('.');
      int num1 = strArray[0].IndexOf("[");
      string name1 = num1 == -1 ? strArray[0] : (System.Convert.ToInt32(strArray[0].Substring(num1 + 1, strArray[0].IndexOf("]") - num1 - 1)) <= 0 ? strArray[0] : strArray[0].Substring(0, strArray[0].IndexOf("[")));
      Variable variable;
      if (parentObj is Cpu)
      {
        Cpu cpu = (Cpu) parentObj;
        if ((variable = cpu.Variables[name1]) == null)
        {
          if (num1 != -1)
          {
            variable = new Variable(cpu, strArray[0].Substring(0, strArray[0].IndexOf("[")));
            variable.propAlignment = this.propAlignment;
            variable.propPviValue.propArrayLength = System.Convert.ToInt32(strArray[0].Substring(num1 + 1, strArray[0].IndexOf("]") - num1 - 1));
          }
          else
          {
            variable = new Variable(cpu, strArray[0]);
            variable.propAlignment = this.propAlignment;
          }
          variable.propPviValue.SetDataType(DataType.Structure);
        }
      }
      else
      {
        Task task = (Task) parentObj;
        if ((variable = task.Variables[name1]) == null)
        {
          if (num1 != -1)
          {
            variable = new Variable(task, strArray[0].Substring(0, strArray[0].IndexOf("[")));
            variable.propAlignment = this.propAlignment;
            variable.propPviValue.propArrayLength = System.Convert.ToInt32(strArray[0].Substring(num1 + 1, strArray[0].IndexOf("]") - num1 - 1));
          }
          else
          {
            variable = new Variable(task, strArray[0]);
            variable.propAlignment = this.propAlignment;
          }
          variable.propPviValue.SetDataType(DataType.Structure);
        }
      }
      object obj = (object) variable;
      Variable member = variable;
      for (int index = 1; index < strArray.Length; ++index)
      {
        int num2 = 0;
        int num3 = strArray[index].IndexOf("[");
        string name2;
        if (num3 != -1)
        {
          num2 = System.Convert.ToInt32(strArray[index].Substring(num3 + 1, strArray[index].IndexOf("]") - num3 - 1));
          name2 = num2 <= 0 ? strArray[index] : strArray[index].Substring(0, strArray[index].IndexOf("["));
        }
        else
          name2 = strArray[index];
        if (member.Members == null || (member = member.Members[name2]) == null)
        {
          if (index != strArray.Length - 1)
          {
            member = new Variable((Variable) obj, name2);
            member.propAlignment = this.propAlignment;
            ((Variable) obj).Members.Add(member);
            member.propPviValue.propArrayLength = num2;
            member.propPviValue.SetDataType(DataType.Structure);
          }
          else
          {
            this.propName = name2;
            this.propParent = (Base) obj;
            this.Init(this.propName);
            this.propPviValue.propArrayLength = num2;
            ((Variable) this.propParent).Members.Add(this);
            member = this;
          }
        }
        obj = (object) member;
      }
    }

    internal void Init(string name) => this.Init(name, false);

    internal void Init(string name, bool isServiceUploadedVar)
    {
      this.propScope = Scope.UNDEFINED;
      this.propReadingFormat = false;
      this.propLinkId = 0U;
      this.propMembers = (MemberCollection) null;
      this.propUserMembers = (MemberCollection) null;
      this.propPviValue = new Value();
      this.propPviValue.Parent = this;
      this.propPviValue.propDataType = DataType.Unknown;
      this.propPviValue.propArrayMinIndex = 0;
      this.propPviValue.propArrayMaxIndex = 0;
      this.propPviValue.propDataSize = 0;
      this.propDataValid = false;
      this.propRefreshTime = 100;
      this.propPviValue.Parent = this;
      this.propWriteValueAutomatic = true;
      this.propReadOnly = false;
      this.propPVState = ConnectionType.None;
      this.propInternalValue = (Value) null;
      this.propVariableAccess = Access.ReadAndWrite;
      this.propReadOnly = false;
      this.propPolling = true;
      this.propScalingPoints = new ScalingPointCollection();
      this.propScalingPoints.propParent = this;
      this.propIODataPoints = (IODataPointCollection) null;
      if (this.propAddToLogicalObjects)
      {
        switch (this.Service.LogicalObjectsUsage)
        {
          case LogicalObjectsUsage.FullName:
            this.Service.AddLogicalObject(this.FullName, (object) this);
            break;
          case LogicalObjectsUsage.ObjectName:
            this.Service.AddLogicalObject(name, (object) this);
            break;
          case LogicalObjectsUsage.ObjectNameWithType:
            this.Service.AddLogicalObject(this.PviPathName, (object) this);
            break;
        }
      }
      this.isServiceUploaded = isServiceUploadedVar;
      if (!isServiceUploadedVar)
        return;
      this.propAddress = name;
    }

    internal void AddMember(Variable newVar)
    {
      if (this.propMembers == null)
        this.propMembers = new MemberCollection(this, this.Name + ".Variables");
      this.propMembers.Add(newVar);
    }

    internal void AddStructMembers(Variable parentVar, Variable memberVar)
    {
      Base owner = parentVar.Owner;
      if (owner is Variable)
        this.AddStructMembers((Variable) owner, memberVar);
      parentVar.AddStructMember(memberVar.GetStructMemberName(parentVar), memberVar);
    }

    internal void AddStructMember(string name, Variable memberVar)
    {
      if (this.mapNameToMember == null)
        this.mapNameToMember = new StructMemberCollection();
      this.mapNameToMember.Add((object) name, (object) memberVar);
    }

    public void SetTypeInfo(DataType type)
    {
      this.propPviValue.propDataType = type;
      this.propBitOffset = -1;
      this.propPviValue.propArrayLength = 0;
      this.propPviValue.TypePreset = true;
    }

    public void SetTypeInfo(int bitOffset, DataType type)
    {
      this.propPviValue.propDataType = type;
      this.propBitOffset = bitOffset;
      this.propPviValue.propArrayLength = 0;
      this.propPviValue.TypePreset = true;
    }

    public void SetTypeInfo(DataType type, int arraySize)
    {
      this.propPviValue.propDataType = type;
      this.propBitOffset = -1;
      this.propPviValue.propArrayLength = arraySize;
      this.propPviValue.TypePreset = true;
    }

    public void SetTypeInfo(DataType type, int arraySize, int bitOffset)
    {
      this.propPviValue.propDataType = type;
      this.propBitOffset = bitOffset;
      this.propPviValue.propArrayLength = arraySize;
      this.propPviValue.TypePreset = true;
    }

    internal override void reCreateState()
    {
      if (!this.reCreateActive)
        return;
      this.reCreateActive = false;
      this.propLinkId = 0U;
      if (this.propActive)
        this.Requests |= Actions.SetActive | Actions.FireActivated;
      this.propConnectionState = ConnectionStates.Unininitialized;
      this.Connect();
    }

    public override int ChangeConnection()
    {
      string connectionDescription = this.GetConnectionDescription();
      return this.WriteRequest(this.Service.hPvi, this.LinkId, AccessTypes.Connect, connectionDescription.Substring(0, connectionDescription.IndexOf(" ")), 3000U, this.propInternID);
    }

    public override void Connect()
    {
      this.propReturnValue = 0;
      this.Connect(this.ConnectionType);
    }

    public override void Connect(ConnectionType conType)
    {
      this.propReturnValue = 0;
      this.Connect(conType, 0);
    }

    internal override void Connect(bool forceConnect)
    {
      this.propReturnValue = 0;
      this.Connect(true, this.ConnectionType, 0U);
    }

    protected virtual string GetLinkParameters(
      ConnectionType conType,
      string dt,
      string fs,
      string lp,
      string va,
      string cm,
      string vL,
      string vN)
    {
      if (this.propSNMPParent != null)
        return this.Address.CompareTo("MacAddresses") != 0 ? "EV=" : (!this.propActive ? (!this.Service.UserTagEvents ? "VT=string" + vL + " VN=1 EV=ef" : "VT=string" + vL + " VN=1 EV=euf") : (!this.Service.UserTagEvents ? "VT=string" + vL + " VN=1 EV=efd" : "VT=string" + vL + " VN=1 EV=eufd"));
      string eventMaskParameters = this.GetEventMaskParameters(conType);
      string linkParameters;
      if (0 < dt.Length)
        linkParameters = dt + vL + vN + " " + eventMaskParameters + lp + cm;
      else
        linkParameters = eventMaskParameters + lp + cm;
      if (va != null && 0 < va.Length)
        linkParameters = "VT=boolean " + eventMaskParameters + lp + va;
      if (this.ConnectionType == ConnectionType.Link)
        linkParameters += fs;
      string str = "";
      if (this.propHysteresis > 0.0)
        str = string.Format(" {0}{1}", (object) "HY=", (object) this.propHysteresis.ToString((IFormatProvider) CultureInfo.InvariantCulture));
      if (!this.Service.IsStatic || this.ConnectionType == ConnectionType.Link)
      {
        if ((this.Requests & Actions.SetRefresh) != Actions.NONE)
          this.Requests &= Actions.Connect | Actions.GetList | Actions.GetValue | Actions.SetValue | Actions.GetForce | Actions.SetForce | Actions.Start | Actions.Stop | Actions.RunCycle | Actions.GetType | Actions.SetType | Actions.GetLength | Actions.CreateLink | Actions.Upload | Actions.Download | Actions.SetActive | Actions.SetHysteresis | Actions.Disconnect | Actions.SetInitValue | Actions.GetLBType | Actions.ModuleInfo | Actions.ListSNMPVariables | Actions.Connected | Actions.Uploading | Actions.GetCpuInfo | Actions.ReadPVFormat | Actions.FireActivated | Actions.ReadIndex | Actions.FireDataValidated | Actions.FireValueChanged | Actions.Link;
        if (this.propActive)
        {
          if (0 < dt.Length)
            linkParameters = dt + vL + vN + " " + eventMaskParameters + lp + cm;
          else
            linkParameters = eventMaskParameters + lp + cm;
          if (va != null && 0 < va.Length)
            linkParameters = "VT=boolean " + eventMaskParameters + lp + va;
          if (this.ConnectionType == ConnectionType.Link)
            linkParameters = linkParameters + fs + str;
        }
        this.Requests &= Actions.Connect | Actions.GetList | Actions.GetValue | Actions.SetValue | Actions.GetForce | Actions.SetForce | Actions.Start | Actions.Stop | Actions.RunCycle | Actions.GetType | Actions.SetType | Actions.GetLength | Actions.SetRefresh | Actions.CreateLink | Actions.Upload | Actions.Download | Actions.SetHysteresis | Actions.Disconnect | Actions.SetInitValue | Actions.GetLBType | Actions.ModuleInfo | Actions.ListSNMPVariables | Actions.Connected | Actions.Uploading | Actions.GetCpuInfo | Actions.ReadPVFormat | Actions.FireActivated | Actions.ReadIndex | Actions.FireDataValidated | Actions.FireValueChanged | Actions.Link;
      }
      if (this.isStruct)
        linkParameters += " VT=struct VL=0 AL=1";
      return linkParameters;
    }

    protected virtual string GetEventMaskParameters(ConnectionType conType, bool useParamMarker)
    {
      string str = "";
      string eventMaskParameters;
      if (useParamMarker)
      {
        eventMaskParameters = "EV=";
        if (ConnectionType.Create != conType)
          eventMaskParameters = !this.Service.UserTagEvents ? eventMaskParameters + "ef" : eventMaskParameters + "euf";
      }
      else if (this.propErrorState != 0)
      {
        eventMaskParameters = !this.Service.UserTagEvents ? str + "ef" : str + "euf";
        this.propErrorState = 0;
      }
      else
        eventMaskParameters = !this.Service.UserTagEvents ? str + "e" : str + "eu";
      if ((!this.Service.IsStatic || ConnectionType.Link == conType) && this.propActive && this.Access != Access.No && Access.Write != this.Access)
        eventMaskParameters += "d";
      return eventMaskParameters;
    }

    private string GetEventMaskParameters(ConnectionType conType) => this.GetEventMaskParameters(conType, true);

    protected virtual string GetObjectParameters(
      string rf,
      string hy,
      string at,
      string fs,
      string ut,
      string dt,
      string vL,
      string vN)
    {
      string str1 = "";
      string str2 = "";
      string str3 = "";
      string str4 = "";
      string str5 = dt;
      if (0 < dt.Length)
      {
        str5.Trim();
        str5 = " " + str5;
        str3 = vL;
        str4 = vN;
      }
      if (this.propInitValue != null && this.propInitValue.Length != 0)
        str1 = " DV=" + this.propInitValue.ToString();
      string objectParameters1;
      if (this.propSNMPParent != null)
      {
        string name = this.propSNMPParent.Name;
        if (this.Address.CompareTo("MacAddresses") == 0 && this.Value.DataType == DataType.Unknown)
        {
          string objectParameters2;
          if (LogicalObjectsUsage.ObjectNameWithType == this.Service.LogicalObjectsUsage)
            objectParameters2 = string.Format("\"{0}\" VT=string VL=1024 VN=1 {1}{2}", (object) this.propAddress, (object) "RF=", (object) rf);
          else
            objectParameters2 = string.Format("\"{0}\"/\"{1}\" VT=string VL=1024 VN=1 {2}{3}", (object) name, (object) this.propAddress, (object) "RF=", (object) rf);
          return objectParameters2;
        }
        if (ConnectionType.Link == this.ConnectionType)
        {
          if (LogicalObjectsUsage.ObjectNameWithType == this.Service.LogicalObjectsUsage)
            objectParameters1 = string.Format("\"{0}\"{1}{2} {3}{4}{5}{6}{7}{8}", (object) this.propAddress, (object) str3, (object) str4, (object) "RF=", (object) rf, (object) hy, (object) at, (object) ut, (object) str1);
          else
            objectParameters1 = string.Format("\"{0}\"/\"{1}\"{2}{3} {4}{5}{6}{7}{8}{9}", (object) name, (object) this.propAddress, (object) str3, (object) str4, (object) "RF=", (object) rf, (object) hy, (object) at, (object) ut, (object) str1);
        }
        else if (0 < str5.Length)
        {
          if (LogicalObjectsUsage.ObjectNameWithType == this.Service.LogicalObjectsUsage)
            objectParameters1 = string.Format("\"{0}\"{1}{2}{3}{4} {5}{6}{7}{8}{9}{10}{11}", (object) this.propAddress, (object) str5, (object) str3, (object) str4, (object) str2, (object) "RF=", (object) rf, (object) hy, (object) at, (object) fs, (object) ut, (object) str1);
          else
            objectParameters1 = string.Format("\"{0}\"/\"{1}\"{2}{3}{4}{5} {6}{7}{8}{9}{10}{11}{12}", (object) name, (object) this.propAddress, (object) str5, (object) str3, (object) str4, (object) str2, (object) "RF=", (object) rf, (object) hy, (object) at, (object) fs, (object) ut, (object) str1);
        }
        else if (LogicalObjectsUsage.ObjectNameWithType == this.Service.LogicalObjectsUsage)
          objectParameters1 = string.Format("\"{0}\"{1}{2}{3} {4}{5}{6}{7}{8}{9}{10}", (object) this.propAddress, (object) str3, (object) str4, (object) str2, (object) "RF=", (object) rf, (object) hy, (object) at, (object) fs, (object) ut, (object) str1);
        else
          objectParameters1 = string.Format("\"{0}\"/\"{1}\"{2}{3}{4} {5}{6}{7}{8}{9}{10}{11}", (object) name, (object) this.propAddress, (object) str3, (object) str4, (object) str2, (object) "RF=", (object) rf, (object) hy, (object) at, (object) fs, (object) ut, (object) str1);
      }
      else if (this.propParent is Service)
        objectParameters1 = string.Format("\"{0}\" {1}{2}{3}{4}{5}{6}{7}{8}{9}{10}", (object) this.propAddress, (object) "RF=", (object) rf, (object) hy, (object) at, (object) fs, (object) ut, (object) str5, (object) str4, (object) str3, (object) str1);
      else if (this.propParent is Variable)
      {
        string linkName = this.propParent.Parent.LinkName;
        if (ConnectionType.Link == this.ConnectionType)
          objectParameters1 = string.Format("\"{0}\"/\"{1}\"{2}{3} {4}{5}{6}{7}{8}{9}", (object) linkName, (object) this.propAddress, (object) str3, (object) str4, (object) "RF=", (object) rf, (object) hy, (object) at, (object) ut, (object) str1);
        else
          objectParameters1 = string.Format("\"{0}\"/\"{1}\"{2}{3} {4}{5}{6}{7}{8}{9}{10}", (object) linkName, (object) this.propAddress, (object) str3, (object) str4, (object) "RF=", (object) rf, (object) hy, (object) at, (object) fs, (object) ut, (object) str1);
      }
      else if (this.propParent is Cpu)
      {
        string str6 = this.propParent.LinkName;
        if (((Cpu) this.propParent).Connection.DeviceType >= DeviceType.TcpIpMODBUS)
        {
          str6 = ((Cpu) this.propParent).Connection.pviStationObj.Name;
          if (0 < this.propAddress.Length)
          {
            if (0 < vN.Length)
            {
              if (0 < vL.Length)
                str3.Insert(1, "/");
              str4.Insert(1, "/");
            }
            string str7 = dt;
            if (0 < dt.Length)
              str7 = "/" + dt;
            str2 = string.Format(" /{0}{1} {2}", (object) "VA=", (object) this.propAddress, (object) str7);
          }
        }
        if (ConnectionType.Link == this.ConnectionType)
        {
          if (LogicalObjectsUsage.ObjectNameWithType == this.Service.LogicalObjectsUsage)
            objectParameters1 = string.Format("\"{0}\"{1}{2} {3}{4}{5}{6}{7}{8}", (object) this.propAddress, (object) str4, (object) str3, (object) "RF=", (object) rf, (object) hy, (object) at, (object) ut, (object) str1);
          else
            objectParameters1 = string.Format("\"{0}\"/\"{1}\"{2}{3} {4}{5}{6}{7}{8}{9}", (object) str6, (object) this.propAddress, (object) str4, (object) str3, (object) "RF=", (object) rf, (object) hy, (object) at, (object) ut, (object) str1);
        }
        else if (LogicalObjectsUsage.ObjectNameWithType == this.Service.LogicalObjectsUsage)
          objectParameters1 = string.Format("\"{0}\"{1}{2}{3} {4}{5}{6}{7}{8}{9}{10}", (object) this.propAddress, (object) str2, (object) str4, (object) str3, (object) "RF=", (object) rf, (object) hy, (object) at, (object) fs, (object) ut, (object) str1);
        else
          objectParameters1 = string.Format("\"{0}\"/\"{1}{2}\"{3}{4} {5}{6}{7}{8}{9}{10}{11}", (object) str6, (object) this.propAddress, (object) str2, (object) str4, (object) str3, (object) "RF=", (object) rf, (object) hy, (object) at, (object) fs, (object) ut, (object) str1);
      }
      else if (LogicalObjectsUsage.ObjectNameWithType == this.Service.LogicalObjectsUsage)
        objectParameters1 = string.Format("\"{0}\"{1}{2} {3}{4}{5}{6}{7}{8}{9}", (object) this.propAddress, (object) str4, (object) str3, (object) "RF=", (object) rf, (object) hy, (object) at, (object) fs, (object) ut, (object) str1);
      else
        objectParameters1 = string.Format("\"{0}\"/\"{1}\"{2}{3} {4}{5}{6}{7}{8}{9}{10}", (object) this.propParent.LinkName, (object) this.propAddress, (object) str4, (object) str3, (object) "RF=", (object) rf, (object) hy, (object) at, (object) fs, (object) ut, (object) str1);
      if (this.propROI != Variable.ROIoptions.OFF)
      {
        string newValue = "\"/RO=" + this.propAddress + " /ROI=" + ((int) this.propROI).ToString() + "\"";
        string oldValue = "\"" + this.propAddress + "\"";
        objectParameters1 = objectParameters1.Replace(oldValue, newValue);
      }
      return objectParameters1;
    }

    private string GetDataTypParameters()
    {
      string dataTypParameters = "";
      if (this.propParent is Service && this.propPviValue.DataType != DataType.Unknown && this.propPviValue.DataType != DataType.Structure && !this.propPviValue.IsOfTypeArray)
        dataTypParameters = this.propScaling == null || this.propScaling.ScalingPoints.Count <= 0 ? string.Format("{0}{1}", (object) "VT=", (object) this.GetPviDataTypeText(this.propPviValue.DataType)) : (this.propScaling.ScalingPoints.propUserDataType == DataType.Unknown || this.propPviValue.DataType != DataType.Unknown ? string.Format("{0}{1}", (object) "VT=", (object) this.GetPviDataTypeText(this.propPviValue.DataType)) : string.Format("{0}{1}", (object) "VT=", (object) this.GetPviDataTypeText(this.propScaling.ScalingPoints.propUserDataType)));
      return dataTypParameters;
    }

    private string GetVAParameters()
    {
      string vaParameters = "";
      if (-1 < this.propBitOffset)
        vaParameters = string.Format(" {0}{1}", (object) "VA=", (object) this.propBitOffset.ToString());
      return vaParameters;
    }

    private string GetVNParameter()
    {
      string vnParameter = "";
      if (1 < this.propPviValue.ArrayLength)
        vnParameter = string.Format(" {0}{1}", (object) "VN=", (object) this.propPviValue.ArrayLength.ToString());
      return vnParameter;
    }

    private string GetVLParameter()
    {
      string vlParameter = "";
      if (DataType.String == this.propPviValue.DataType)
        vlParameter = this.propPviValue.propTypeLength != 0 || !(this.propParent is Service) ? string.Format(" {0}{1}", (object) "VL=", (object) this.propPviValue.propTypeLength.ToString()) : string.Format(" {0}32", (object) "VL=");
      else if (1 < this.propPviValue.DataSize)
        vlParameter = string.Format(" {0}{1}", (object) "VL=", (object) this.propPviValue.DataSize.ToString());
      return vlParameter;
    }

    private string GetHysteresisParameters()
    {
      string hysteresisParameters = "";
      if (this.propHysteresis > 0.0)
        hysteresisParameters = string.Format(" {0}{1}", (object) "HY=", (object) this.propHysteresis.ToString((IFormatProvider) CultureInfo.InvariantCulture));
      return hysteresisParameters;
    }

    private string GetCastModeParameters()
    {
      string castModeParameters = "";
      if (this.propCastMode != CastModes.DEFAULT)
        castModeParameters = " CM=" + ((int) this.propCastMode).ToString();
      return castModeParameters;
    }

    private string GetAttributeParameters()
    {
      string attributeParameters = string.Format(" {0}", (object) "AT=");
      if (Access.ReadAndWrite == this.propVariableAccess)
        attributeParameters += "rw";
      else if (this.propVariableAccess == Access.No)
      {
        attributeParameters = string.Format(" {0}", (object) "AT=");
      }
      else
      {
        if (Access.Read == (this.propVariableAccess & Access.Read))
          attributeParameters += "r";
        if (Access.Write == (this.propVariableAccess & Access.Write))
          attributeParameters += "w";
        if (Access.DIRECT == (this.propVariableAccess & Access.DIRECT))
          attributeParameters += "d";
        if (Access.FASTECHO == (this.propVariableAccess & Access.FASTECHO))
          attributeParameters += "h";
        if (Access.EVENT == (this.propVariableAccess & Access.EVENT))
          attributeParameters += "e";
      }
      return attributeParameters;
    }

    private string GetUserTagParameters()
    {
      string userTagParameters = "";
      if (this.propUserTag != null)
        userTagParameters = string.Format(" {0}{1}", (object) "UT=", (object) this.propUserTag);
      return userTagParameters;
    }

    private string GetScalingFunctionParameters()
    {
      string functionParameters = "";
      if (this.propPviValue.DataType != DataType.Structure && !this.propPviValue.IsOfTypeArray)
      {
        if (this.propScaling != null && this.propScaling.ScalingType == ScalingType.Factor)
          this.propScaling.Factor = this.propScaling.propFactor;
        if (this.propScaling != null && this.propScaling.ScalingPoints.Count > 0)
        {
          functionParameters = string.Format(" {0}", (object) "FS=");
          foreach (ScalingPoint scalingPoint in (ArrayList) this.propScaling.ScalingPoints)
          {
            string str1 = scalingPoint.XValue.ToString((IFormatProvider) CultureInfo.InvariantCulture);
            string str2 = scalingPoint.YValue.ToString((IFormatProvider) CultureInfo.InvariantCulture);
            if (DataType.Single == this.propPviValue.propDataType || DataType.Double == this.propPviValue.propDataType)
            {
              if (-1 == str1.IndexOf('.'))
                str1 += ".0";
              if (-1 == str2.IndexOf('.'))
                str2 += ".0";
            }
            functionParameters += string.Format("{0},{1};", (object) str1, (object) str2);
          }
        }
      }
      return functionParameters;
    }

    private string UpdateAddress(ConnectionType conType)
    {
      string str = this.propAddress;
      if (this.propAddress == null || this.propAddress.Length == 0 && ConnectionType.Link != conType)
      {
        str = this.propName;
        for (Base @base = !(this.propOwner is Variable) || !((Variable) this.propOwner).propPviValue.IsOfTypeArray ? this.propOwner : ((Variable) this.propOwner).propOwner; @base is Variable; @base = ((Variable) @base).propOwner)
          str = @base.propAddress.Length != 0 ? @base.propAddress + "." + str : @base.propName + "." + str;
      }
      return str;
    }

    private string GetLTParameters()
    {
      string ltParameters = "";
      if (this.propHysteresis > 0.0 || this.propScaling != null && this.propScaling.ScalingPoints.Count > 0 && this.propPviValue.DataType != DataType.Structure && !this.propPviValue.IsOfTypeArray)
        ltParameters = string.Format(" {0}{1}", (object) "LT=", (object) "prc");
      return ltParameters;
    }

    private string GetDataTypParameter()
    {
      string dataTypParameter = "";
      if (this.propPviValue.TypePreset)
      {
        switch (this.propPviValue.propDataType)
        {
          case DataType.Boolean:
            dataTypParameter = "VT=boolean";
            break;
          case DataType.SByte:
            dataTypParameter = "VT=i8";
            break;
          case DataType.Int16:
            dataTypParameter = "VT=i16";
            break;
          case DataType.Int32:
            dataTypParameter = "VT=i32";
            break;
          case DataType.Int64:
            dataTypParameter = "VT=i64";
            break;
          case DataType.Byte:
          case DataType.UInt8:
            dataTypParameter = "VT=u8";
            break;
          case DataType.UInt16:
            dataTypParameter = "VT=u16";
            break;
          case DataType.UInt32:
            dataTypParameter = "VT=u32";
            break;
          case DataType.UInt64:
            dataTypParameter = "VT=u64";
            break;
          case DataType.Single:
            dataTypParameter = "VT=f32";
            break;
          case DataType.Double:
            dataTypParameter = "VT=f64";
            break;
          case DataType.TimeSpan:
            dataTypParameter = "VT=time";
            break;
          case DataType.DateTime:
          case DataType.DT:
            dataTypParameter = "VT=dt";
            break;
          case DataType.String:
            dataTypParameter = "VT=string";
            break;
          case DataType.WString:
            dataTypParameter = "VT=wstring";
            break;
          case DataType.TimeOfDay:
          case DataType.TOD:
            dataTypParameter = "VT=tod";
            break;
          case DataType.Date:
            dataTypParameter = "VT=date";
            break;
          case DataType.WORD:
            dataTypParameter = "VT=WORD";
            break;
          case DataType.DWORD:
            dataTypParameter = "VT=DWORD";
            break;
        }
      }
      return dataTypParameter;
    }

    protected override string GetConnectionDescription()
    {
      string attributeParameters = this.GetAttributeParameters();
      string userTagParameters = this.GetUserTagParameters();
      string hysteresisParameters = this.GetHysteresisParameters();
      string vnParameter = this.GetVNParameter();
      string vlParameter = this.GetVLParameter();
      string dt = this.GetDataTypParameter();
      string functionParameters = this.GetScalingFunctionParameters();
      this.propAddress = this.UpdateAddress(ConnectionType.Create);
      if (this.propParent is Service && this.propSNMPParent == null)
        dt = this.GetDataTypParameters();
      return this.GetObjectParameters(this.propRefreshTime.ToString(), hysteresisParameters, attributeParameters, functionParameters, userTagParameters, dt, vlParameter, vnParameter);
    }

    public virtual void Connect(ConnectionType conType, int internalAction) => this.Connect(false, conType, (uint) internalAction);

    private void Connect(bool forceConnection, ConnectionType conType, uint internalAction)
    {
      this.propReturnValue = 0;
      if (this.reCreateActive || this.LinkId != 0U)
        return;
      if (this.LinkId != 0U && (ConnectionStates.Connected == this.propConnectionState || ConnectionStates.ConnectedError == this.propConnectionState))
      {
        this.Fire_ConnectedEvent((object) this, new PviEventArgs(this.Name, this.Address, this.propErrorCode, this.Service.Language, Action.VariableConnect, this.Service));
      }
      else
      {
        if (!forceConnection && ConnectionStates.Unininitialized < this.propConnectionState && ConnectionStates.Disconnecting > this.propConnectionState || ConnectionStates.Connecting == this.propConnectionState)
          return;
        if ((Value) null != this.propPviValue && !this.propPviValue.TypePreset)
        {
          this.propPviValue.propDataType = DataType.Unknown;
          this.propDataValid = false;
          this.propWaitForUserTag = false;
          this.isObjectConnected = false;
        }
        this.ConnectionType = conType;
        if (this.propParent is Variable)
        {
          if (ConnectionStates.Connected == this.propConnectionState || ConnectionStates.ConnectedError == this.propConnectionState)
          {
            this.Call_Connected(new PviEventArgs(this.Name, this.Address, 12002, this.Service.Language, Action.VariableConnect, this.Service));
            this.propReturnValue = 12002;
          }
        }
        else if (ConnectionStates.Connected == this.propConnectionState || ConnectionStates.ConnectedError == this.propConnectionState)
        {
          this.Call_Connected(new PviEventArgs(this.Name, this.Address, 12002, this.Service.Language, Action.VariableConnect, this.Service));
          this.propReturnValue = 12002;
        }
        this.propWaitForUserTag = !string.IsNullOrEmpty(this.propUserTag);
        this.propAddress = this.UpdateAddress(conType);
        if (this.propParent is Cpu && !this.propParent.IsConnected && this.propParent.ErrorCode == 0)
        {
          if (!forceConnection)
          {
            if (this.Service.WaitForParentConnection)
            {
              this.propReturnValue = 0;
              this.Requests |= Actions.Connect;
              return;
            }
          }
          else if (Actions.Connect == (this.Requests & Actions.Connect))
            this.Requests &= Actions.GetList | Actions.GetValue | Actions.SetValue | Actions.GetForce | Actions.SetForce | Actions.Start | Actions.Stop | Actions.RunCycle | Actions.GetType | Actions.SetType | Actions.GetLength | Actions.SetRefresh | Actions.CreateLink | Actions.Upload | Actions.Download | Actions.SetActive | Actions.SetHysteresis | Actions.Disconnect | Actions.SetInitValue | Actions.GetLBType | Actions.ModuleInfo | Actions.ListSNMPVariables | Actions.Connected | Actions.Uploading | Actions.GetCpuInfo | Actions.ReadPVFormat | Actions.FireActivated | Actions.ReadIndex | Actions.FireDataValidated | Actions.FireValueChanged | Actions.Link;
        }
        if (this.propParent is Task && !this.propParent.IsConnected && this.propParent.ErrorCode == 0)
        {
          if (!forceConnection)
          {
            if (this.Service.WaitForParentConnection)
            {
              this.propReturnValue = 0;
              this.Requests |= Actions.Connect;
              return;
            }
          }
          else if (Actions.Connect == (this.Requests & Actions.Connect))
            this.Requests &= Actions.GetList | Actions.GetValue | Actions.SetValue | Actions.GetForce | Actions.SetForce | Actions.Start | Actions.Stop | Actions.RunCycle | Actions.GetType | Actions.SetType | Actions.GetLength | Actions.SetRefresh | Actions.CreateLink | Actions.Upload | Actions.Download | Actions.SetActive | Actions.SetHysteresis | Actions.Disconnect | Actions.SetInitValue | Actions.GetLBType | Actions.ModuleInfo | Actions.ListSNMPVariables | Actions.Connected | Actions.Uploading | Actions.GetCpuInfo | Actions.ReadPVFormat | Actions.FireActivated | Actions.ReadIndex | Actions.FireDataValidated | Actions.FireValueChanged | Actions.Link;
        }
        this.propConnectionState = ConnectionStates.Connecting;
        string dt = this.GetDataTypParameter();
        if (this.propPviValue.DataType == DataType.Unknown)
        {
          if (this.ConnectionType == ConnectionType.Link && (this.propHysteresis > 0.0 || this.propScaling != null && this.propScaling.ScalingPoints.Count > 0))
          {
            this.propReturnValue = this.XLinkRequest(this.Service.hPvi, this.LinkName, 711U, "EV=f", 709U);
            return;
          }
        }
        else if (this.propParent is Service && this.propSNMPParent == null)
          dt = this.GetDataTypParameters();
        if (this.Service.IsStatic)
          this.Requests |= Actions.Link;
        string functionParameters = this.GetScalingFunctionParameters();
        string ltParameters = this.GetLTParameters();
        string vaParameters = this.GetVAParameters();
        string castModeParameters = this.GetCastModeParameters();
        string vnParameter = this.GetVNParameter();
        string vlParameter = this.GetVLParameter();
        this.propLinkParam = this.GetLinkParameters(conType, dt, functionParameters, ltParameters, vaParameters, castModeParameters, vlParameter, vnParameter);
        this.propObjectParam = "CD=" + this.GetConnectionDescription();
        string objectName = this.GetObjectName();
        if (!this.Service.IsStatic && this.ConnectionType == ConnectionType.CreateAndLink)
          this.propReturnValue = this.XCreateRequest(this.Service.hPvi, objectName, ObjectType.POBJ_PVAR, this.propObjectParam, 550U, this.propLinkParam, 501U);
        else if (ConnectionType.Link != this.ConnectionType && ConnectionType.Create != this.propPVState)
        {
          this.propReturnValue = this.XCreateRequest(this.Service.hPvi, objectName, ObjectType.POBJ_PVAR, this.propObjectParam, 0U, "", 501U);
        }
        else
        {
          uint action = 701;
          if (internalAction != 0U)
            action = internalAction;
          this.propReturnValue = this.PviLinkObject(action);
        }
        if (this.propReturnValue == 0)
          return;
        this.OnError(new PviEventArgs(this.propName, this.propAddress, this.propReturnValue, this.Service.Language, Action.VariableConnect, this.Service));
      }
    }

    protected override string GetObjectName() => this.propSNMPParent == null ? (!this.isServiceUploaded ? this.LinkName : "@Pvi/" + this.Name) : this.propSNMPParent.FullName + "." + this.Name;

    public virtual void Upload() => this.Upload(true);

    internal virtual void Upload(bool bSendEvent)
    {
      this.propSendUploadEvent = bSendEvent;
      if (ConnectionStates.Connected != this.propConnectionState && ConnectionStates.ConnectedError != this.propConnectionState)
      {
        this.Requests |= Actions.Upload;
        this.Connect();
      }
      else
      {
        if (this.Members != null && this.Members.Count > 0)
        {
          this.propUserMembers = new MemberCollection();
          this.Members.CopyTo(this.propUserMembers);
          this.Members.CleanUp(false);
        }
        int errorCode = !(this.Parent is Service) ? this.ReadRequest(this.Service.hPvi, this.propLinkId, AccessTypes.TypeExtern, 700U) : this.ReadRequest(this.Service.hPvi, this.propLinkId, AccessTypes.TypeIntern, 700U);
        if (errorCode == 0)
          return;
        this.OnError(new PviEventArgs(this.Name, this.Address, errorCode, this.Service.Language, Action.NONE, this.Service));
      }
    }

    protected override void OnConnected(PviEventArgs e)
    {
      if (ConnectionStates.Connecting < this.propConnectionState && ConnectionStates.ConnectedError != this.propConnectionState)
      {
        if (ConnectionStates.Disconnected != this.propConnectionState)
          return;
        this.FireConnectedEvents(e);
      }
      else
      {
        if (this.propErrorState != 0)
        {
          if (this.propActive)
          {
            this.Activate();
            this.propErrorState = 0;
          }
          else
          {
            if (this.Cpu != null)
            {
              if (!this.Cpu.IsSG4Target)
              {
                this.Requests |= Actions.ReadPVFormat;
                this.Read_State(this.propLinkId, 2812U);
                return;
              }
              this.Read_FormatEX(this.propLinkId);
              return;
            }
            this.Read_FormatEX(this.propLinkId);
            return;
          }
        }
        if ((e.ErrorCode == 0 || 12002 == e.ErrorCode) && this.propInitValue != null && this.propInitValue.Length != 0)
        {
          this.propPviValue.Assign((object) this.propInitValue);
          this.Requests |= Actions.SetInitValue;
        }
        this.FireConnectedEvents(e);
      }
    }

    private void FireConnectedEvents(PviEventArgs e)
    {
      if (0.0 != this.propHysteresis && (this.propPviValue.DataType == DataType.Single || this.propPviValue.DataType == DataType.Double) && ConnectionStates.Connecting == this.propConnectionState && -1 == this.propHysteresis.ToString().IndexOf('.') && -1 == this.propHysteresis.ToString().IndexOf(','))
      {
        this.propConnectionState = ConnectionStates.Connected;
        this.Hysteresis = this.propHysteresis;
        this.propConnectionState = ConnectionStates.Connecting;
      }
      base.OnConnected(e);
      if (this.Parent is Cpu)
        ((Cpu) this.Parent).Variables.OnConnected(this, e);
      else if (this.Parent is Task)
        ((Task) this.Parent).Variables.OnConnected(this, e);
      else if (this.Parent is Service && this.propSNMPParent == null)
        ((Service) this.Parent).Variables.OnConnected(this, e);
      if (this.propUserCollections != null && this.propUserCollections.Count > 0)
      {
        foreach (VariableCollection variableCollection in (IEnumerable) this.propUserCollections.Values)
          variableCollection.OnConnected(this, e);
      }
      if (e.ErrorCode == 0)
      {
        this.CheckActiveRequests(e);
        if (this.Service != null && this.Service.IsStatic && ConnectionType.Link == (this.ConnectionType & ConnectionType.Link) && Actions.Link == (this.Requests & Actions.Link))
        {
          this.Requests &= Actions.Connect | Actions.GetList | Actions.GetValue | Actions.SetValue | Actions.GetForce | Actions.SetForce | Actions.Start | Actions.Stop | Actions.RunCycle | Actions.GetType | Actions.SetType | Actions.GetLength | Actions.SetRefresh | Actions.CreateLink | Actions.Upload | Actions.Download | Actions.SetActive | Actions.SetHysteresis | Actions.Disconnect | Actions.SetInitValue | Actions.GetLBType | Actions.ModuleInfo | Actions.ListSNMPVariables | Actions.Connected | Actions.Uploading | Actions.GetCpuInfo | Actions.ReadPVFormat | Actions.FireActivated | Actions.ReadIndex | Actions.FireDataValidated | Actions.FireValueChanged;
          if (this.propActive && -1 == this.propLinkParam.IndexOf("d") && this.Access != Access.No && Access.Write != this.Access)
          {
            if (-1 != this.propLinkParam.IndexOf("VT="))
              this.propLinkParam = this.propLinkParam.Replace("f VT", "fd VT");
            else
              this.propLinkParam += "d";
          }
          this.propReturnValue = this.XLinkRequest(this.Service.hPvi, this.GetObjectName(), 550U, this.propLinkParam, 701U);
        }
      }
      if (!((Value) null != this.propPviValue) || this.Active || this.propPviValue.DataType == DataType.Unknown || this.propSNMPParent != null)
        return;
      this.DeactivateInternal();
    }

    protected void CheckActiveRequests(PviEventArgs e)
    {
      if (this.propLinkId == 0U)
        return;
      if ((this.Requests & Actions.SetActive) != Actions.NONE)
      {
        this.Active = this.propActive;
        this.Requests &= Actions.Connect | Actions.GetList | Actions.GetValue | Actions.SetValue | Actions.GetForce | Actions.SetForce | Actions.Start | Actions.Stop | Actions.RunCycle | Actions.GetType | Actions.SetType | Actions.GetLength | Actions.SetRefresh | Actions.CreateLink | Actions.Upload | Actions.Download | Actions.SetHysteresis | Actions.Disconnect | Actions.SetInitValue | Actions.GetLBType | Actions.ModuleInfo | Actions.ListSNMPVariables | Actions.Connected | Actions.Uploading | Actions.GetCpuInfo | Actions.ReadPVFormat | Actions.FireActivated | Actions.ReadIndex | Actions.FireDataValidated | Actions.FireValueChanged | Actions.Link;
      }
      if ((this.Requests & Actions.SetRefresh) != Actions.NONE)
      {
        this.RefreshTime = this.RefreshTime;
        this.Requests &= Actions.Connect | Actions.GetList | Actions.GetValue | Actions.SetValue | Actions.GetForce | Actions.SetForce | Actions.Start | Actions.Stop | Actions.RunCycle | Actions.GetType | Actions.SetType | Actions.GetLength | Actions.CreateLink | Actions.Upload | Actions.Download | Actions.SetActive | Actions.SetHysteresis | Actions.Disconnect | Actions.SetInitValue | Actions.GetLBType | Actions.ModuleInfo | Actions.ListSNMPVariables | Actions.Connected | Actions.Uploading | Actions.GetCpuInfo | Actions.ReadPVFormat | Actions.FireActivated | Actions.ReadIndex | Actions.FireDataValidated | Actions.FireValueChanged | Actions.Link;
      }
      int num = (int) (this.Requests & Actions.SetHysteresis);
      if ((this.Requests & Actions.SetInitValue) != Actions.NONE)
      {
        this.Requests &= Actions.Connect | Actions.GetList | Actions.GetValue | Actions.SetValue | Actions.GetForce | Actions.SetForce | Actions.Start | Actions.Stop | Actions.RunCycle | Actions.GetType | Actions.SetType | Actions.GetLength | Actions.SetRefresh | Actions.CreateLink | Actions.Upload | Actions.Download | Actions.SetActive | Actions.SetHysteresis | Actions.Disconnect | Actions.GetLBType | Actions.ModuleInfo | Actions.ListSNMPVariables | Actions.Connected | Actions.Uploading | Actions.GetCpuInfo | Actions.ReadPVFormat | Actions.FireActivated | Actions.ReadIndex | Actions.FireDataValidated | Actions.FireValueChanged | Actions.Link;
        this.WriteInitialValue();
      }
      if ((this.Requests & Actions.SetValue) != Actions.NONE)
      {
        this.propPviValue.Assign((object) this.propInternalValue.ToString());
        if (this.WriteValueAutomatic)
          this.WriteValue();
        this.propInternalValue.Dispose();
        this.propInternalValue = (Value) null;
        this.Requests &= Actions.Connect | Actions.GetList | Actions.GetValue | Actions.GetForce | Actions.SetForce | Actions.Start | Actions.Stop | Actions.RunCycle | Actions.GetType | Actions.SetType | Actions.GetLength | Actions.SetRefresh | Actions.CreateLink | Actions.Upload | Actions.Download | Actions.SetActive | Actions.SetHysteresis | Actions.Disconnect | Actions.SetInitValue | Actions.GetLBType | Actions.ModuleInfo | Actions.ListSNMPVariables | Actions.Connected | Actions.Uploading | Actions.GetCpuInfo | Actions.ReadPVFormat | Actions.FireActivated | Actions.ReadIndex | Actions.FireDataValidated | Actions.FireValueChanged | Actions.Link;
      }
      if ((this.Requests & Actions.GetValue) != Actions.NONE)
      {
        this.Requests &= Actions.Connect | Actions.GetList | Actions.SetValue | Actions.GetForce | Actions.SetForce | Actions.Start | Actions.Stop | Actions.RunCycle | Actions.GetType | Actions.SetType | Actions.GetLength | Actions.SetRefresh | Actions.CreateLink | Actions.Upload | Actions.Download | Actions.SetActive | Actions.SetHysteresis | Actions.Disconnect | Actions.SetInitValue | Actions.GetLBType | Actions.ModuleInfo | Actions.ListSNMPVariables | Actions.Connected | Actions.Uploading | Actions.GetCpuInfo | Actions.ReadPVFormat | Actions.FireActivated | Actions.ReadIndex | Actions.FireDataValidated | Actions.FireValueChanged | Actions.Link;
        this.ReadInternalValue();
      }
      if ((this.Requests & Actions.Upload) != Actions.NONE)
      {
        this.Upload();
        this.Requests &= Actions.Connect | Actions.GetList | Actions.GetValue | Actions.SetValue | Actions.GetForce | Actions.SetForce | Actions.Start | Actions.Stop | Actions.RunCycle | Actions.GetType | Actions.SetType | Actions.GetLength | Actions.SetRefresh | Actions.CreateLink | Actions.Download | Actions.SetActive | Actions.SetHysteresis | Actions.Disconnect | Actions.SetInitValue | Actions.GetLBType | Actions.ModuleInfo | Actions.ListSNMPVariables | Actions.Connected | Actions.Uploading | Actions.GetCpuInfo | Actions.ReadPVFormat | Actions.FireActivated | Actions.ReadIndex | Actions.FireDataValidated | Actions.FireValueChanged | Actions.Link;
      }
      if (Actions.FireActivated == (this.Requests & Actions.FireActivated))
        this.OnActivated(e);
      if (Actions.FireDataValidated == (this.Requests & Actions.FireDataValidated))
        this.OnDataValidated(e);
      if (Actions.FireValueChanged != (this.Requests & Actions.FireValueChanged))
        return;
      this.Requests &= Actions.Connect | Actions.GetList | Actions.GetValue | Actions.SetValue | Actions.GetForce | Actions.SetForce | Actions.Start | Actions.Stop | Actions.RunCycle | Actions.GetType | Actions.SetType | Actions.GetLength | Actions.SetRefresh | Actions.CreateLink | Actions.Upload | Actions.Download | Actions.SetActive | Actions.SetHysteresis | Actions.Disconnect | Actions.SetInitValue | Actions.GetLBType | Actions.ModuleInfo | Actions.ListSNMPVariables | Actions.Connected | Actions.Uploading | Actions.GetCpuInfo | Actions.ReadPVFormat | Actions.FireActivated | Actions.ReadIndex | Actions.FireDataValidated | Actions.Link;
      this.OnValueChanged(new VariableEventArgs(this.Name, this.Address, e.ErrorCode, this.Service.Language, e.Action, new string[0]));
    }

    public override void Disconnect()
    {
      this.propReturnValue = 0;
      this.propReadingFormat = false;
      this.propReadingState = false;
      this.Disconnect(502U);
    }

    public override void Disconnect(bool noResponse)
    {
      this.propReturnValue = 0;
      this.propReadingFormat = false;
      this.propReadingState = false;
      this.Disconnect(502U, noResponse);
    }

    internal int Disconnect(uint internalAction) => this.Disconnect(internalAction, false);

    internal virtual int Disconnect(uint internalAction, bool noResponse)
    {
      this.propNoDisconnectedEvent = noResponse;
      this.propWaitingOnReadEvent = false;
      int num = 12004;
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
      {
        this.propConnectionState = ConnectionStates.Unininitialized;
        this.propReadingFormat = false;
        this.propReadingState = false;
      }
      return num;
    }

    protected override void OnDisconnected(PviEventArgs e)
    {
      this.propReadingState = false;
      this.propReadingFormat = false;
      if (ConnectionStates.Connected != this.propConnectionState && ConnectionStates.ConnectedError != this.propConnectionState)
      {
        this.OnConnected(e);
        this.isObjectConnected = false;
        if (this.propConnectionState != ConnectionStates.Disconnecting)
          this.propConnectionState = ConnectionStates.Connected;
      }
      base.OnDisconnected(e);
      if (this.Parent is Cpu && ((Cpu) this.Parent).Variables != null)
        ((Cpu) this.Parent).Variables.OnDisconnected(this, e);
      else if (this.Parent is Task && ((Task) this.Parent).Variables != null)
        ((Task) this.Parent).Variables.OnDisconnected(this, e);
      if (this.propParent is Service && ((Service) this.Parent).Variables != null && this.propSNMPParent == null)
        ((Service) this.propParent).Variables.OnDisconnected(this, e);
      if (this.propUserCollections != null && this.propUserCollections.Count > 0)
      {
        foreach (VariableCollection variableCollection in (IEnumerable) this.propUserCollections.Values)
        {
          variableCollection.OnDisconnected(this, e);
          if (this.propUserCollections == null)
            break;
        }
      }
      this.propPVState = ConnectionType.None;
      this.propDataValid = false;
      if (!this.propDisposed && this.mapNameToMember != null)
      {
        for (int index = 0; index < this.mapNameToMember.Count; ++index)
        {
          object obj = this.mapNameToMember[index];
          if (obj is Variable)
            ((Variable) obj).propDataValid = false;
        }
      }
      this.propStatusRead = false;
      if (Actions.Connect == (this.Requests & Actions.Connect))
      {
        this.Requests = Actions.NONE;
        this.Connect(ConnectionType.CreateAndLink);
      }
      else
        this.propConnectionState = ConnectionStates.Unininitialized;
    }

    protected internal override void OnError(PviEventArgs e)
    {
      base.OnError(e);
      if (this.Parent is Cpu)
      {
        if (this.Parent != null && ((Cpu) this.Parent).Variables != null)
          ((Cpu) this.Parent).Variables.OnError(this, e);
      }
      else if (this.Parent is Task && this.Parent != null && ((Task) this.Parent).Variables != null)
        ((Task) this.Parent).Variables.OnError(this, e);
      if (this.propParent is Service && this.propSNMPParent == null)
        ((Service) this.propParent).Variables.OnError(this, e);
      if (this.propUserCollections != null && this.propUserCollections.Count > 0)
      {
        foreach (VariableCollection variableCollection in (IEnumerable) this.propUserCollections.Values)
          variableCollection.OnError(this, e);
      }
      this.propStatusRead = false;
    }

    protected virtual void OnDataValidated(PviEventArgs e)
    {
      if (Actions.FireDataValidated == (this.Requests & Actions.FireDataValidated))
        this.Requests &= Actions.Connect | Actions.GetList | Actions.GetValue | Actions.SetValue | Actions.GetForce | Actions.SetForce | Actions.Start | Actions.Stop | Actions.RunCycle | Actions.GetType | Actions.SetType | Actions.GetLength | Actions.SetRefresh | Actions.CreateLink | Actions.Upload | Actions.Download | Actions.SetActive | Actions.SetHysteresis | Actions.Disconnect | Actions.SetInitValue | Actions.GetLBType | Actions.ModuleInfo | Actions.ListSNMPVariables | Actions.Connected | Actions.Uploading | Actions.GetCpuInfo | Actions.ReadPVFormat | Actions.FireActivated | Actions.ReadIndex | Actions.FireValueChanged | Actions.Link;
      if (this.propDataValid)
        return;
      this.propDataValid = true;
      if (this.mapNameToMember != null)
      {
        for (int index = 0; index < this.mapNameToMember.Count; ++index)
        {
          object obj = this.mapNameToMember[index];
          if (obj is Variable)
            ((Variable) obj).propDataValid = true;
        }
      }
      if (this.DataValidated != null)
        this.DataValidated((object) this, e);
      if (this.Parent is Cpu)
        ((Cpu) this.Parent).Variables.OnDataValidated(this, e);
      else if (this.Parent is Task)
        ((Task) this.Parent).Variables.OnDataValidated(this, e);
      else if (this.Parent is Service && this.propSNMPParent == null)
        ((Service) this.Parent).Variables.OnDataValidated(this, e);
      if (this.propUserCollections == null || this.propUserCollections.Count <= 0)
        return;
      foreach (VariableCollection variableCollection in (IEnumerable) this.propUserCollections.Values)
        variableCollection.OnDataValidated(this, e);
    }

    protected virtual void OnValueChanged(VariableEventArgs e)
    {
      if (ConnectionStates.Connected != this.propConnectionState)
      {
        this.Requests |= Actions.FireDataValidated | Actions.FireValueChanged;
      }
      else
      {
        this.Fire_ValueChanged((object) this, e);
        if (this.Parent is Task && ((Task) this.Parent).Variables != null)
          ((Task) this.Parent).Variables.OnValueChanged(this, e);
        else if (this.Parent is Cpu && ((Cpu) this.Parent).Variables != null)
          ((Cpu) this.Parent).Variables.OnValueChanged(this, e);
        if (this.Parent is Service && ((Service) this.Parent).Variables != null)
          ((Service) this.Parent).Variables.OnValueChanged(this, e);
        if (this.propUserCollections == null || this.propUserCollections.Count <= 0)
          return;
        foreach (VariableCollection variableCollection in (IEnumerable) this.propUserCollections.Values)
          variableCollection.OnValueChanged(this, e);
      }
    }

    protected virtual void OnValueWritten(PviEventArgs e)
    {
      this.Fire_ValueWritten((object) this, e);
      if (this.Parent is Cpu)
        ((Cpu) this.Parent).Variables.OnValueWritten(this, e);
      else if (this.Parent is Task)
        ((Task) this.Parent).Variables.OnValueWritten(this, e);
      if (this.propParent is Service)
        ((Service) this.propParent).Variables.OnValueWritten(this, e);
      if (this.propUserCollections == null || this.propUserCollections.Count <= 0)
        return;
      foreach (VariableCollection variableCollection in (IEnumerable) this.propUserCollections.Values)
        variableCollection.OnValueWritten(this, e);
    }

    protected virtual void OnValueRead(PviEventArgs e)
    {
      this.propErrorCode = e.ErrorCode;
      this.Fire_ValueRead((object) this, e);
      if (this.Parent is Cpu)
        ((Cpu) this.Parent).Variables.OnValueRead(this, e);
      else if (this.Parent is Task)
        ((Task) this.Parent).Variables.OnValueRead(this, e);
      if (this.propParent is Service && this.propSNMPParent == null)
        ((Service) this.propParent).Variables.OnValueRead(this, e);
      if (this.propUserCollections == null || this.propUserCollections.Count <= 0)
        return;
      foreach (VariableCollection variableCollection in (IEnumerable) this.propUserCollections.Values)
        variableCollection.OnValueRead(this, e);
    }

    internal void Fire_ValueChanged(object sender, VariableEventArgs e)
    {
      this.OnDataValidated((PviEventArgs) e);
      if (this.ValueChanged == null)
        return;
      this.ValueChanged(sender, e);
    }

    internal void Fire_ValueWritten(object sender, PviEventArgs e)
    {
      if (this.ValueWritten == null)
        return;
      this.ValueWritten(sender, e);
    }

    internal void Fire_ValueRead(object sender, PviEventArgs e)
    {
      this.propWaitingOnReadEvent = false;
      if (e.ErrorCode == 0)
        this.OnDataValidated(e);
      if (this.ValueRead == null)
        return;
      this.ValueRead(sender, e);
    }

    internal void Fire_Activated(object sender, PviEventArgs e)
    {
      this.propActive = true;
      if (e.ErrorCode != 0)
      {
        this.Requests |= Actions.SetActive | Actions.FireActivated;
      }
      else
      {
        if (this.Activated == null)
          return;
        this.Activated(sender, e);
      }
    }

    internal void Fire_Deactivated(object sender, PviEventArgs e)
    {
      this.propReadingFormat = false;
      this.propReadingState = false;
      this.propActive = false;
      this.Requests = Actions.NONE;
      if (this.Deactivated == null)
        return;
      this.Deactivated(sender, e);
    }

    protected virtual void OnExtendedTypeInfoRead(PviEventArgs e)
    {
      if (this.ExtendedTypeInfoRead == null)
        return;
      this.ExtendedTypeInfoRead((object) this, e);
    }

    protected virtual void OnActivated(PviEventArgs e)
    {
      if (Actions.FireActivated != (this.Requests & Actions.FireActivated))
        return;
      this.Requests &= Actions.Connect | Actions.GetList | Actions.GetValue | Actions.SetValue | Actions.GetForce | Actions.SetForce | Actions.Start | Actions.Stop | Actions.RunCycle | Actions.GetType | Actions.SetType | Actions.GetLength | Actions.SetRefresh | Actions.CreateLink | Actions.Upload | Actions.Download | Actions.SetActive | Actions.SetHysteresis | Actions.Disconnect | Actions.SetInitValue | Actions.GetLBType | Actions.ModuleInfo | Actions.ListSNMPVariables | Actions.Connected | Actions.Uploading | Actions.GetCpuInfo | Actions.ReadPVFormat | Actions.ReadIndex | Actions.FireDataValidated | Actions.FireValueChanged | Actions.Link;
      this.Fire_Activated((object) this, e);
      if (this.propParent is Cpu)
        ((Cpu) this.propParent).Variables.OnActivated(this, e);
      else if (this.propParent is Task)
        ((Task) this.propParent).Variables.OnActivated(this, e);
      else if (this.propParent is Service)
        ((Service) this.propParent).Variables.OnActivated(this, e);
      if (this.propUserCollections != null && this.propUserCollections.Count > 0)
      {
        foreach (VariableCollection variableCollection in (IEnumerable) this.propUserCollections.Values)
          variableCollection.OnActivated(this, e);
      }
      if (this.propPviValue.DataType != DataType.Unknown && (DataType.Structure != this.propPviValue.DataType && 1 >= this.propPviValue.ArrayLength || !this.propExpandMembers || this.StructureMembers == null || this.StructureMembers.Count != 0))
        return;
      this.Read_FormatEX(this.propLinkId);
    }

    protected virtual void OnDeactivated(PviEventArgs e)
    {
      this.Fire_Deactivated((object) this, e);
      if (this.propParent is Cpu)
        ((Cpu) this.propParent).Variables.OnDeactivated(this, e);
      else if (this.propParent is Task)
        ((Task) this.propParent).Variables.OnDeactivated(this, e);
      else if (this.propParent is Service)
        ((Service) this.propParent).Variables.OnDeactivated(this, e);
      if (this.propUserCollections == null || this.propUserCollections.Count <= 0)
        return;
      foreach (VariableCollection variableCollection in (IEnumerable) this.propUserCollections.Values)
        variableCollection.OnDeactivated(this, e);
    }

    protected override void OnPropertyChanged(PviEventArgs e)
    {
      base.OnPropertyChanged(e);
      if (this.propParent is Cpu)
        ((Cpu) this.propParent).Variables.OnPropertyChanged(this, e);
      else if (this.propParent is Task)
        ((Task) this.propParent).Variables.OnPropertyChanged(this, e);
      else if (this.propParent is Service)
        ((Service) this.propParent).Variables.OnPropertyChanged(this, e);
      if (this.propUserCollections == null || this.propUserCollections.Count <= 0)
        return;
      foreach (VariableCollection variableCollection in (IEnumerable) this.propUserCollections.Values)
        variableCollection.OnPropertyChanged(this, e);
    }

    protected virtual void OnUploaded(PviEventArgs e)
    {
      if (e.ErrorCode != 0)
      {
        if (!this.propActive)
          return;
        this.Requests |= Actions.Upload;
      }
      else
      {
        if (this.Uploaded != null)
          this.Uploaded((object) this, e);
        if (this.propMembers == null || 0 >= this.propMembers.Count)
          return;
        foreach (Variable variable in (IEnumerable) this.propMembers.Values)
        {
          if ((variable.Requests & Actions.Connect) != Actions.NONE)
            variable.Connect();
        }
      }
    }

    protected virtual void OnForcedOn(PviEventArgs e)
    {
      if (this.ForcedOn == null)
        return;
      this.ForcedOn((object) this, e);
    }

    protected virtual void OnForcedOff(PviEventArgs e)
    {
      if (this.ForcedOff == null)
        return;
      this.ForcedOff((object) this, e);
    }

    internal int Read_State(uint linkId, uint uiAction)
    {
      if (this.propReadingState)
        return 0;
      this.propReadingState = true;
      return this.ReadRequest(this.Service.hPvi, linkId, AccessTypes.Status, uiAction);
    }

    private void ExtractExtendedTypeInfo(int errorCode, IntPtr pData, uint dataLen)
    {
      ArrayList changedMembers = new ArrayList();
      if (0U < dataLen)
      {
        this.propPviValue.InitializeExtendedAttributes();
        string pviText = PviMarshal.PtrToStringAnsi(pData, dataLen);
        int length = pviText.IndexOf("\0");
        if (-1 != length)
          pviText = pviText.Substring(0, length);
        string[] strArray = pviText.Split(' ');
        for (int index = 0; index < strArray.Length; ++index)
        {
          strArray.GetValue(index).ToString();
          this.GetVSParameters(pviText, ref this.propPviValue, this.propPviValue.DataType);
        }
      }
      else
        this.OnError(new PviEventArgs(this.Name, this.Address, errorCode, this.Service.Language, Action.VariableExtendedTypeInfoRead, this.Service));
      if ((Value) null == this.propInitialValue)
        this.propInitialValue = new Value();
      this.ConvertSimpleValues(pData, dataLen, ref changedMembers, ref this.propInitialValue);
      this.OnExtendedTypeInfoRead(new PviEventArgs(this.Name, this.Address, errorCode, this.Service.Language, Action.VariableExtendedTypeInfoRead, this.Service));
    }

    private bool DataFormat_Changed(int errorCode, IntPtr pData, uint dataLen, Action cbAction)
    {
      bool flag = true;
      int retVal = 0;
      if (0U < dataLen)
      {
        string stringAnsi = PviMarshal.PtrToStringAnsi(pData, dataLen);
        this.GetScope(stringAnsi, ref this.propScope);
        flag = this.UpdateDataFormat(stringAnsi, cbAction, errorCode, false, ref retVal);
        if (retVal != 0)
          this.OnError(new PviEventArgs(this.Name, this.Address, retVal, this.Service.Language, cbAction, this.Service));
      }
      else
      {
        this.propReadingFormat = false;
        this.OnError(new PviEventArgs(this.Name, this.Address, errorCode, this.Service.Language, cbAction, this.Service));
        this.Requests |= Actions.ReadPVFormat;
      }
      return flag;
    }

    internal bool GetExtendedAttributes(
      string strFormat,
      ref bool isMDimArray,
      ref Value pPviValue)
    {
      bool extendedAttributes = false;
      pPviValue.propIsEnum = (sbyte) 0;
      pPviValue.propIsBitString = (sbyte) 0;
      pPviValue.propIsDerived = (sbyte) 0;
      string[] strArray = strFormat.Split('{');
      isMDimArray = false;
      if (-1 != strFormat.IndexOf("VS="))
      {
        extendedAttributes = true;
        string pviText = strArray[0];
        if (1 < strArray.GetLength(0))
        {
          pviText.Trim();
          if (pviText.Length == 0)
            pviText = strArray[1];
        }
        if (-1 != pviText.IndexOf("VS=e") || -1 != pviText.IndexOf(";e"))
        {
          pPviValue.propIsEnum = (sbyte) 1;
          if (-1 != pviText.IndexOf("SN="))
          {
            if (pPviValue.propEnumerations != null)
            {
              pPviValue.propEnumerations.Clear();
              pPviValue.propEnumerations = (EnumArray) null;
            }
            pPviValue.propEnumerations = new EnumArray(this.GetSNParameter(pviText));
          }
        }
        if (-1 != pviText.IndexOf("VS=b") || -1 != pviText.IndexOf(";b"))
          pPviValue.propIsBitString = (sbyte) 1;
        if (-1 != pviText.IndexOf("VS=a") || -1 != pviText.IndexOf(";a"))
          isMDimArray = true;
        if (-1 != pviText.IndexOf("VS=v") || -1 != pviText.IndexOf(";v"))
          pPviValue.propIsDerived = (sbyte) 1;
      }
      return extendedAttributes;
    }

    private bool ExtendedTypeInfoAutoUpdateEnabled() => this.Service != null && this.Service.ExtendedTypeInfoAutoUpdateForVariables && ((sbyte) 1 == this.propPviValue.propIsEnum || (sbyte) 1 == this.propPviValue.propIsDerived || (sbyte) 1 == this.propPviValue.propIsBitString);

    internal bool UpdateDataFormat(
      string strFormat,
      Action cbAction,
      int errorCode,
      bool initOnly,
      ref int retVal)
    {
      return this.UpdateDataFormat(strFormat, cbAction, errorCode, initOnly, false, ref retVal);
    }

    internal bool UpdateDataFormat(
      string strFormat,
      Action cbAction,
      int errorCode,
      bool initOnly,
      bool createNew,
      ref int retVal)
    {
      bool isMDimArray = false;
      this.propReadingFormat = false;
      int length = strFormat.IndexOf("\0");
      try
      {
        if (-1 != length)
          strFormat = strFormat.Substring(0, length);
        if (0 < strFormat.Length)
        {
          this.propPviValue.InitializeExtendedAttributes();
          bool extendedAttributes = this.GetExtendedAttributes(strFormat, ref isMDimArray, ref this.propPviValue);
          if (isMDimArray)
            this.propPviValue.SetArrayIndex(strFormat);
          DataType dataType = Variable.GetDataType(strFormat, this.Value.IsBitString, ref this.propPviValue.propTypeLength);
          this.GetVSParameters(strFormat, ref this.propPviValue, dataType);
          if (Action.VariableReadFormatInternal != cbAction)
          {
            if (this.Parent is Service)
            {
              if (DataType.Structure == dataType || extendedAttributes || this.ExtendedTypeInfoAutoUpdateEnabled() || (sbyte) 1 == this.propPviValue.propIsEnum && (this.propPviValue.propEnumerations == null || this.propPviValue.propEnumerations.Count == 0))
              {
                this.propPviValue.propDataType = DataType.Unknown;
                if (!this.isStruct && DataType.Structure == dataType)
                {
                  this.isStruct = true;
                  if (this.UnlinkRequest(2813U) == 0)
                    return false;
                }
                if (this.Read_FormatEX(this.propLinkId) == 0)
                  return false;
              }
            }
            else if (DataType.Structure == dataType || extendedAttributes || this.ExtendedTypeInfoAutoUpdateEnabled() || (sbyte) 1 == this.propPviValue.propIsEnum && (this.propPviValue.propEnumerations == null || this.propPviValue.propEnumerations.Count == 0))
            {
              this.propPviValue.propDataType = DataType.Unknown;
              if (!this.isStruct && DataType.Structure == dataType)
              {
                this.isStruct = true;
                if (this.UnlinkRequest(2813U) == 0)
                  return false;
              }
              this.Read_FormatEX(this.propLinkId);
              return false;
            }
          }
          int arrayLength = Variable.GetArrayLength(strFormat);
          if (this.propScaling != null && this.propPviValue.DataType == DataType.Unknown && dataType != DataType.Structure && arrayLength <= 1)
          {
            bool flag = this.propPviValue.SetDataType(Variable.GetDataType(strFormat, this.Value.IsBitString, ref this.propPviValue.propTypeLength));
            this.propPviValue.propTypeLength = Variable.GetDataTypeLength(strFormat, this.propPviValue.propTypeLength);
            this.propPviValue.SetArrayLength(Variable.GetArrayLength(strFormat));
            if (this.propPviValue.ArrayMinIndex == 0)
              this.propPviValue.propArrayMaxIndex = this.propPviValue.ArrayLength - 1;
            if (!flag)
              this.WriteScaling();
          }
          else if (this.propPviValue.DataType == DataType.Unknown || this.IsConnected)
          {
            this.propPviValue.SetDataType(Variable.GetDataType(strFormat, this.Value.IsBitString, ref this.propPviValue.propTypeLength));
            this.propPviValue.propTypeLength = Variable.GetDataTypeLength(strFormat, this.propPviValue.propTypeLength);
            this.propPviValue.SetArrayLength(Variable.GetArrayLength(strFormat));
            if (this.propPviValue.ArrayMinIndex == 0)
              this.propPviValue.propArrayMaxIndex = this.propPviValue.ArrayLength - 1;
            if (this.propPviValue.IsOfTypeArray && DataType.Structure != this.propPviValue.DataType)
            {
              this.propExpandMembers = false;
              this.CreateMembers(strFormat, ref this.propPviValue.propTypeLength, false);
            }
          }
          else if (!this.IsConnected)
          {
            if (!this.propPviValue.TypePreset)
            {
              this.propPviValue.SetDataType(Variable.GetDataType(strFormat, this.Value.IsBitString, ref this.propPviValue.propTypeLength));
              this.propPviValue.propTypeLength = Variable.GetDataTypeLength(strFormat, this.propPviValue.propTypeLength);
              this.propPviValue.SetArrayLength(Variable.GetArrayLength(strFormat));
              if (this.propPviValue.ArrayMinIndex == 0)
                this.propPviValue.propArrayMaxIndex = this.propPviValue.ArrayLength - 1;
            }
            else
              this.propPviValue.DataType = this.propPviValue.DataType;
            if (this.propPviValue.IsOfTypeArray && DataType.Structure != this.propPviValue.DataType)
            {
              this.propExpandMembers = false;
              this.CreateMembers(strFormat, ref this.propPviValue.propTypeLength, false);
            }
          }
          this.propPviValue.SetArrayLength(Variable.GetArrayLength(strFormat));
          if (this.propPviValue.ArrayMinIndex == 0)
            this.propPviValue.propArrayMaxIndex = this.propPviValue.ArrayLength - 1;
          if (Action.VariableInternLink == cbAction || Action.VariableInternFormat == cbAction)
          {
            if (createNew)
            {
              this.DeleteRequest(true);
              this.Requests = Actions.Connect;
            }
            else
            {
              this.propNoDisconnectedEvent = true;
              this.Disconnect(2717U);
              this.propNoDisconnectedEvent = false;
              this.Connect(ConnectionType.Link);
            }
            return true;
          }
          if (!initOnly)
          {
            if (this.propPviValue.propDataType != DataType.Structure)
            {
              if (!this.propWaitForUserTag && (!(this is IOVariable) || this.propStatusRead))
              {
                if (this.Cpu != null && !this.Cpu.IsSG4Target)
                  this.Read_State(this.propLinkId, 2812U);
                else
                  this.OnConnected(new PviEventArgs(this.Name, this.Address, errorCode, this.Service.Language, Action.VariableConnect, this.Service));
              }
            }
            else if (ConnectionStates.Connected != this.propConnectionState)
            {
              if (this.Parent is Service)
              {
                this.GetVSParameters(strFormat, ref this.propPviValue, this.propPviValue.DataType);
                if (this.propPviValue.propDataType == DataType.Structure)
                {
                  this.propPviInternStructElement = 0U;
                  this.CreateMembers(strFormat, ref this.propPviValue.propTypeLength, this.propExpandMembers);
                }
                else if (this.propPviValue.IsOfTypeArray)
                  this.CreateMembersNOExpand(strFormat, ref this.propPviValue.propTypeLength, this.Service.AddMembersToVariableCollection);
                this.OnConnected(new PviEventArgs(this.Name, this.Address, errorCode, this.Service.Language, Action.VariableConnect, this.Service));
                if (Actions.GetValue == (this.Requests & Actions.GetValue))
                {
                  this.Requests &= Actions.Connect | Actions.GetList | Actions.SetValue | Actions.GetForce | Actions.SetForce | Actions.Start | Actions.Stop | Actions.RunCycle | Actions.GetType | Actions.SetType | Actions.GetLength | Actions.SetRefresh | Actions.CreateLink | Actions.Upload | Actions.Download | Actions.SetActive | Actions.SetHysteresis | Actions.Disconnect | Actions.SetInitValue | Actions.GetLBType | Actions.ModuleInfo | Actions.ListSNMPVariables | Actions.Connected | Actions.Uploading | Actions.GetCpuInfo | Actions.ReadPVFormat | Actions.FireActivated | Actions.ReadIndex | Actions.FireDataValidated | Actions.FireValueChanged | Actions.Link;
                  this.ReadInternalValue();
                }
              }
              else
                retVal = this.InterpretTypInfo(errorCode, strFormat, (int) cbAction);
            }
            if (Actions.SetActive == (this.Requests & Actions.SetActive))
            {
              this.Requests &= Actions.Connect | Actions.GetList | Actions.GetValue | Actions.SetValue | Actions.GetForce | Actions.SetForce | Actions.Start | Actions.Stop | Actions.RunCycle | Actions.GetType | Actions.SetType | Actions.GetLength | Actions.SetRefresh | Actions.CreateLink | Actions.Upload | Actions.Download | Actions.SetHysteresis | Actions.Disconnect | Actions.SetInitValue | Actions.GetLBType | Actions.ModuleInfo | Actions.ListSNMPVariables | Actions.Connected | Actions.Uploading | Actions.GetCpuInfo | Actions.ReadPVFormat | Actions.FireActivated | Actions.ReadIndex | Actions.FireDataValidated | Actions.FireValueChanged | Actions.Link;
              this.Active = this.propActive;
            }
          }
          return true;
        }
      }
      catch (OutOfMemoryException ex)
      {
        string message = ex.Message;
        this.CleanupMemory();
        retVal = 14;
      }
      catch (System.Exception ex)
      {
        string message = ex.Message;
        retVal = 14;
      }
      return false;
    }

    private Access GetAccessType(string attribDesc)
    {
      Access accessType = this.propVariableAccess;
      int num1 = attribDesc.IndexOf("AT=");
      if (-1 < num1)
      {
        int num2 = attribDesc.IndexOf(" ", 1 + num1);
        string str = -1 != num2 ? attribDesc.Substring(num1 + 3, num2 - 3 - num1).ToLower() : attribDesc.Substring(num1 + 3);
        str.Trim();
        if (0 < str.Length)
        {
          accessType = Access.No;
          if (-1 != str.IndexOf("r"))
            accessType |= Access.Read;
          if (-1 != str.IndexOf("w"))
            accessType |= Access.Write;
          if (-1 != str.IndexOf("e"))
            accessType |= Access.EVENT;
          if (-1 != str.IndexOf("d"))
            accessType |= Access.DIRECT;
          if (-1 != str.IndexOf("h"))
            accessType |= Access.FASTECHO;
        }
      }
      return accessType;
    }

    [CLSCompliant(false)]
    protected void UpdateValueData(IntPtr pData, uint dataLen, int error)
    {
      int num = 0;
      ArrayList changedMembers = new ArrayList(1);
      this.propChangedStructMembers = new string[changedMembers.Count];
      if (0U < dataLen)
      {
        num = this.ConvertPviValue(pData, dataLen, ref changedMembers);
        this.propChangedStructMembers = new string[changedMembers.Count];
        for (int index = 0; index < changedMembers.Count; ++index)
          this.propChangedStructMembers.SetValue(changedMembers[index], index);
      }
      if (this.propActive)
        this.OnValueChanged(new VariableEventArgs(this.Name, this.Address, num != 0 ? num : error, this.Service.Language, Action.VariableValueChangedEvent, this.propChangedStructMembers));
    }

    private void ValidateConnection(int errorCode)
    {
      if ((!this.Service.IsStatic || ConnectionType.Link == this.ConnectionType) && !this.IsConnected)
      {
        if (this is IOVariable)
          this.Read_State(this.LinkId, 557U);
        else if (!this.propReadingState && errorCode == 0 && this.propPviValue.DataType != DataType.Unknown && !this.propWaitForUserTag)
          this.OnConnected(new PviEventArgs(this.Name, this.Address, errorCode, this.Service.Language, Action.ConnectedEvent, this.Service));
      }
      if (this.propReadingState || this.propPviValue.DataType == DataType.Unknown || DataType.Structure == this.propPviValue.DataType || this.IsConnected || this.propWaitForUserTag)
        return;
      this.OnConnected(new PviEventArgs(this.Name, this.Address, errorCode, this.Service.Language, Action.ConnectedEvent, this.Service));
    }

    internal override void OnPviCreated(int errorCode, uint linkID)
    {
      this.propErrorCode = errorCode;
      if (this.propErrorCode == 0 || 12002 == this.propErrorCode)
      {
        if (ConnectionType.CreateAndLink == this.ConnectionType)
        {
          this.propLinkId = linkID;
          if (this.Service.IsStatic)
          {
            this.propPVState = ConnectionType.Create;
            this.propReturnValue = this.PviLinkObject(701U);
          }
          else
          {
            if (this.propSNMPParent == null)
              return;
            this.Read_FormatEX(this.propLinkId);
          }
        }
        else
          this.OnConnected(new PviEventArgs(this.Name, this.Address, errorCode, this.Service.Language, Action.VariableConnect, this.Service));
      }
      else
      {
        if (Service.IsRemoteError(errorCode))
          this.Requests |= Actions.Connect;
        this.OnDisconnected(new PviEventArgs(this.Name, this.Address, errorCode, this.Service.Language, Action.VariableConnect, this.Service));
        this.OnError(new PviEventArgs(this.Name, this.Address, errorCode, this.Service.Language, Action.VariableConnect, this.Service));
      }
    }

    internal override void OnPviLinked(int errorCode, uint linkID, int option)
    {
      this.propErrorCode = errorCode;
      if (this.propErrorCode == 0 || 12002 == this.propErrorCode)
      {
        this.propLinkId = linkID;
        if (1 == option)
        {
          if (errorCode == 0)
            return;
          this.Service.OnPVIObjectsAttached(new PviEventArgs(this.propName, this.propAddress, errorCode, this.Service.Language, Action.TaskReadVariablesList));
        }
        else if (!this.IsConnected && !(this.Parent is Service) && 2 != option)
        {
          if (this is IOVariable)
          {
            this.Read_State(linkID, 557U);
          }
          else
          {
            if (this.propPviValue.propDataType != DataType.Unknown)
              return;
            this.Read_FormatEX(this.propLinkId);
          }
        }
        else if (!this.IsConnected && !(this.Parent is Service))
        {
          this.Read_FormatEX(this.propLinkId);
        }
        else
        {
          PviEventArgs e;
          switch (option)
          {
            case 3:
              e = new PviEventArgs(this.Name, this.Address, errorCode, this.Service.Language, Action.VariableScalingChange, this.Service);
              this.OnPropertyChanged(e);
              break;
            case 4:
              e = new PviEventArgs(this.Name, this.Address, errorCode, this.Service.Language, Action.VariableSetHysteresis, this.Service);
              this.OnPropertyChanged(e);
              break;
            case 5:
              e = new PviEventArgs(this.Name, this.Address, errorCode, this.Service.Language, Action.VariableSetType, this.Service);
              this.OnPropertyChanged(e);
              break;
            default:
              e = new PviEventArgs(this.Name, this.Address, errorCode, this.Service.Language, Action.VariableLink, this.Service);
              break;
          }
          this.CheckActiveRequests(e);
        }
      }
      else
        this.OnError(new PviEventArgs(this.Name, this.Address, errorCode, this.Service.Language, Action.VariableConnect, this.Service));
    }

    internal override void OnPviEvent(
      int errorCode,
      EventTypes eventType,
      PVIDataStates dataState,
      IntPtr pData,
      uint dataLen,
      int option)
    {
      int retVal = 0;
      int propErrorCode = this.propErrorCode;
      this.propErrorCode = errorCode;
      ConnectionStates propConnectionState = this.propConnectionState;
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
              if (12033 == errorCode && this.propSNMPParent != null)
              {
                this.propPviValue.TypePreset = true;
                if (1 == option)
                {
                  this.UpdateDataFormat("VT=string VL=1024 VN=1", Action.VariableInternLink, 0, false, true, ref retVal);
                  break;
                }
                this.UpdateDataFormat("VT=string VL=1024 VN=1", Action.VariableFormatChanged, 0, false, true, ref retVal);
                break;
              }
              if (this.IsConnected)
              {
                this.OnDisconnected(new PviEventArgs(this.Name, this.Address, errorCode, this.Service.Language, Action.DisconnectedEvent, this.Service));
                if (ConnectionStates.Connected == propConnectionState)
                  this.propConnectionState = ConnectionStates.ConnectedError;
              }
              else if (ConnectionStates.Connecting == this.propConnectionState)
                this.OnConnected(new PviEventArgs(this.Name, this.Address, errorCode, this.Service.Language, Action.DisconnectedEvent, this.Service));
              this.OnError(new PviEventArgs(this.Name, this.Address, errorCode, this.Service.Language, Action.ErrorEvent, this.Service));
              break;
            }
            if (this is IOVariable)
            {
              if (!this.IsConnected || this.propParent is Service)
                break;
              this.Read_State(this.LinkId, 557U);
              break;
            }
            if (this.propPviValue.DataType == DataType.Unknown || Actions.ReadPVFormat == (this.Requests & Actions.ReadPVFormat))
            {
              if (this.Cpu != null)
              {
                if (!this.Cpu.IsSG4Target)
                {
                  this.Requests |= Actions.ReadPVFormat;
                  this.Read_State(this.propLinkId, 2812U);
                  break;
                }
                this.Read_FormatEX(this.propLinkId);
                break;
              }
              this.Read_FormatEX(this.propLinkId);
              break;
            }
            if (this.Cpu != null && !this.Cpu.IsSG4Target)
            {
              this.Read_State(this.propLinkId, 2812U);
              break;
            }
            this.OnConnected(new PviEventArgs(this.Name, this.Address, errorCode, this.Service.Language, Action.VariableConnect, this.Service));
            break;
          case EventTypes.Data:
            this.ValidateConnection(errorCode);
            if (this.ExpandMembers)
            {
              if (this.propPviValue.DataType == DataType.Unknown)
              {
                this.Requests |= Actions.GetValue;
                this.Read_FormatEX(this.propLinkId);
                break;
              }
              if (this.propPviValue.DataType != DataType.Unknown)
              {
                this.UpdateValueData(pData, dataLen, errorCode);
                break;
              }
              this.ReadInternalValue();
              break;
            }
            if (this.propPviValue.DataType == DataType.Unknown)
            {
              this.Requests |= Actions.GetValue;
              this.Read_FormatEX(this.propLinkId);
              break;
            }
            ArrayList changedMembers = new ArrayList(1);
            if (0U < dataLen)
              retVal = this.ConvertPviValue(pData, dataLen, ref changedMembers);
            this.propChangedStructMembers = new string[changedMembers.Count];
            for (int index = 0; index < changedMembers.Count; ++index)
              this.propChangedStructMembers.SetValue(changedMembers[index], index);
            if (retVal != 0)
            {
              this.OnError(new PviEventArgs(this.Name, this.Address, retVal, this.Service.Language, Action.VariableValueChangedEvent, this.Service));
              this.OnValueChanged(new VariableEventArgs(this.Name, this.Address, retVal, this.Service.Language, Action.VariableValueChangedEvent, this.propChangedStructMembers));
              break;
            }
            this.OnValueChanged(new VariableEventArgs(this.Name, this.Address, errorCode, this.Service.Language, Action.VariableValueChangedEvent, this.propChangedStructMembers));
            break;
          case EventTypes.Status:
            if (0U >= dataLen)
              break;
            string ansiString1 = PviMarshal.ToAnsiString(pData, dataLen);
            if (Variable.GetForceStatus(ansiString1, ref this.propForceValue))
            {
              string address = this.Address;
              if (this.propParent is Task)
              {
                string str = this.propParent.Address + ":" + this.Address;
              }
            }
            this.propAttribute = Variable.GetAttribute(ansiString1);
            this.propStatusRead = true;
            if (this.propPviValue.DataType == DataType.Unknown)
            {
              this.Read_FormatEX(this.propLinkId);
              break;
            }
            this.OnConnected(new PviEventArgs(this.Name, this.Address, errorCode, this.Service.Language, Action.VariableConnect, this.Service));
            break;
          case EventTypes.Dataform:
            if (1 == option)
            {
              this.DataFormat_Changed(errorCode, pData, dataLen, Action.VariableInternLink);
              break;
            }
            this.DataFormat_Changed(errorCode, pData, dataLen, Action.VariableFormatChanged);
            break;
          case EventTypes.UserTag:
            if (this.ErrorCode != 0)
              break;
            string propUserTag = this.propUserTag;
            string ansiString2 = PviMarshal.ToAnsiString(pData, dataLen);
            this.propUserTag = 0U >= dataLen ? (string) null : ansiString2;
            if (this.propWaitForUserTag)
            {
              if (this.propPviValue.DataType == DataType.Structure && this.Members != null && this.Members.Count > 0 || this.propParent is Service && this.ConnectionType != ConnectionType.Link)
                this.OnConnected(new PviEventArgs(this.Name, this.Address, errorCode, this.Service.Language, Action.VariableConnect, this.Service));
              this.propWaitForUserTag = false;
              break;
            }
            if (!this.IsConnected || propUserTag == null || propUserTag.Equals(ansiString2))
              break;
            this.OnPropertyChanged(new PviEventArgs(this.Name, this.Address, errorCode, this.Service.Language, Action.VariableSetUserTag, this.Service));
            break;
          default:
            base.OnPviEvent(errorCode, eventType, dataState, pData, dataLen, option);
            break;
        }
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
      int num = 0;
      this.propErrorCode = errorCode;
      if (errorCode != 0)
      {
        switch (accessType)
        {
          case PVIReadAccessTypes.Data:
            this.OnValueRead(new PviEventArgs(this.Name, this.Address, errorCode, this.Service.Language, Action.VariableValueRead, this.Service));
            break;
          case PVIReadAccessTypes.State:
            this.OnConnected(new PviEventArgs(this.Name, this.Address, errorCode, this.Service.Language, Action.VariableConnect, this.Service));
            break;
          case PVIReadAccessTypes.BasicAttributes:
          case PVIReadAccessTypes.ExtendedAttributes:
          case PVIReadAccessTypes.ExtendedInternalAttributes:
            this.propErrorState = errorCode;
            switch (option)
            {
              case 1:
                this.OnUploaded(new PviEventArgs(this.Name, this.Address, errorCode, this.Service.Language, Action.VariableUpload, this.Service));
                return;
              case 2:
                if (this.Parent is Service)
                {
                  this.ReadRequest(this.Service.hPvi, this.LinkId, AccessTypes.Type, 2810U);
                  return;
                }
                this.propPviValue.DataType = DataType.Unknown;
                this.propReadingFormat = false;
                this.OnError(new PviEventArgs(this.Name, this.Address, errorCode, this.Service.Language, Action.VariableExtendedTypeInfoRead, this.Service));
                return;
              case 5:
                this.OnExtendedTypeInfoRead(new PviEventArgs(this.Name, this.Address, errorCode, this.Service.Language, Action.VariableExtendedTypeInfoRead, this.Service));
                return;
              default:
                base.OnPviRead(errorCode, accessType, dataState, pData, dataLen, option);
                return;
            }
          case PVIReadAccessTypes.LinkNodeList:
            this.IODataPoints.OnPviRead(errorCode, PVIReadAccessTypes.LinkNodeList, dataState, pData, dataLen, option);
            break;
          default:
            base.OnPviRead(errorCode, accessType, dataState, pData, dataLen, option);
            break;
        }
      }
      else
      {
        switch (accessType)
        {
          case PVIReadAccessTypes.Data:
            this.ValidateConnection(errorCode);
            ArrayList changedMembers = new ArrayList(1);
            if (0U < dataLen && this.propPviValue.propDataType != DataType.Unknown)
              num = this.ConvertPviValue(pData, dataLen, ref changedMembers);
            this.propChangedStructMembers = new string[changedMembers.Count];
            for (int index = 0; index < changedMembers.Count; ++index)
              this.propChangedStructMembers.SetValue(changedMembers[index], index);
            if (5 == option && this.Active)
            {
              this.OnValueChanged(new VariableEventArgs(this.Name, this.Address, num != 0 ? num : errorCode, this.Service.Language, Action.VariableValueChangedEvent, this.propChangedStructMembers));
              break;
            }
            this.OnValueRead(new PviEventArgs(this.Name, this.Address, num != 0 ? num : errorCode, this.Service.Language, Action.VariableValueRead, this.Service));
            break;
          case PVIReadAccessTypes.State:
            if (0U >= dataLen)
              break;
            string ansiString = PviMarshal.ToAnsiString(pData, dataLen);
            this.propReadingState = false;
            if (Variable.GetForceStatus(ansiString, ref this.propForceValue))
            {
              string address = this.Address;
              if (this.propParent is Task)
              {
                string str = this.propParent.Address + ":" + this.Address;
              }
            }
            this.propStatusRead = true;
            this.propAttribute = Variable.GetAttribute(ansiString);
            if (this.propPviValue.DataType != DataType.Unknown)
            {
              this.OnConnected(new PviEventArgs(this.Name, this.Address, errorCode, this.Service.Language, Action.VariableConnect, this.Service));
              break;
            }
            if (Actions.ReadPVFormat != (this.Requests & Actions.ReadPVFormat))
              break;
            this.Read_FormatEX(this.propLinkId);
            break;
          case PVIReadAccessTypes.BasicAttributes:
          case PVIReadAccessTypes.ExtendedAttributes:
          case PVIReadAccessTypes.ExtendedInternalAttributes:
            this.propErrorState = errorCode;
            switch (option)
            {
              case 1:
                if (0U < dataLen)
                {
                  string pviText = PVIReadAccessTypes.BasicAttributes != accessType ? PviMarshal.PtrToStringAnsi(pData, dataLen) : PviMarshal.ToAnsiString(pData, dataLen);
                  int errorCode1 = this.InterpretTypInfo(errorCode, pviText, 700);
                  if (this.propSendUploadEvent)
                  {
                    if (errorCode1 != 0)
                    {
                      this.OnError(new PviEventArgs(this.Name, this.Address, errorCode1, this.Service.Language, Action.VariableUpload, this.Service));
                      this.OnUploaded(new PviEventArgs(this.Name, this.Address, errorCode1, this.Service.Language, Action.VariableUpload, this.Service));
                      return;
                    }
                    this.OnUploaded(new PviEventArgs(this.Name, this.Address, errorCode, this.Service.Language, Action.VariableUpload, this.Service));
                    return;
                  }
                  if (errorCode1 == 0)
                    return;
                  this.OnError(new PviEventArgs(this.Name, this.Address, errorCode1, this.Service.Language, Action.VariableConnect, this.Service));
                  this.OnConnected(new PviEventArgs(this.Name, this.Address, errorCode1, this.Service.Language, Action.VariableConnect, this.Service));
                  return;
                }
                this.OnError(new PviEventArgs(this.Name, this.Address, errorCode, this.Service.Language, Action.VariableExtendedTypeInfoRead, this.Service));
                return;
              case 2:
                this.DataFormat_Changed(errorCode, pData, dataLen, Action.VariableReadFormatInternal);
                return;
              case 5:
                this.ExtractExtendedTypeInfo(errorCode, pData, dataLen);
                return;
              default:
                this.propReadingFormat = false;
                if (0U < dataLen)
                {
                  string stringAnsi = PviMarshal.PtrToStringAnsi(pData, dataLen);
                  this.InterpretTypInfo(errorCode, stringAnsi, 516);
                  return;
                }
                this.OnError(new PviEventArgs(this.Name, this.Address, errorCode, this.Service.Language, Action.VariableExtendedTypeInfoRead, this.Service));
                return;
            }
          case PVIReadAccessTypes.LinkNodeList:
            this.IODataPoints.OnPviRead(errorCode, PVIReadAccessTypes.LinkNodeList, dataState, pData, dataLen, option);
            break;
          default:
            base.OnPviRead(errorCode, accessType, dataState, pData, dataLen, option);
            break;
        }
      }
    }

    private int InterpretTypInfo(int errorCode, string pviText, int actionCode)
    {
      bool isMDimArray = false;
      int retVal = 0;
      try
      {
        this.propPviValue.InitializeExtendedAttributes();
        int length = pviText.IndexOf("\0");
        if (-1 != length)
          pviText = pviText.Substring(0, length);
        this.GetScope(pviText, ref this.propScope);
        this.GetExtendedAttributes(pviText, ref isMDimArray, ref this.propPviValue);
        if (isMDimArray)
          this.propPviValue.SetArrayIndex(pviText);
        this.propPviValue.SetDataType(Variable.GetDataType(pviText, this.Value.IsBitString, ref this.propPviValue.propTypeLength));
        this.propPviValue.SetArrayLength(Variable.GetArrayLength(pviText));
        if (this.propPviValue.ArrayMinIndex == 0)
          this.propPviValue.propArrayMaxIndex = this.propPviValue.ArrayLength - 1;
        this.GetVSParameters(pviText, ref this.propPviValue, this.propPviValue.DataType);
        this.propPviValue.propTypeLength = Variable.GetDataTypeLength(pviText, this.propPviValue.propTypeLength);
        if (this.propPviValue.propDataType == DataType.Structure)
          this.CreateMembers(pviText, ref this.propPviValue.propTypeLength, this.propExpandMembers);
        else if (this.propPviValue.IsOfTypeArray)
        {
          this.CreateMembersNOExpand(pviText, ref this.propPviValue.propTypeLength, this.Service.AddMembersToVariableCollection);
        }
        else
        {
          this.UpdateDataFormat(pviText, (Action) actionCode, errorCode, false, ref retVal);
          return retVal;
        }
        if (this.propSendUploadEvent)
          return retVal;
        if (!this.propWaitForUserTag && !this.IsConnected)
        {
          this.OnConnected(new PviEventArgs(this.Name, this.Address, errorCode, this.Service.Language, Action.VariableConnect, this.Service));
          if (Actions.GetValue == (this.Requests & Actions.GetValue))
          {
            this.Requests &= Actions.Connect | Actions.GetList | Actions.SetValue | Actions.GetForce | Actions.SetForce | Actions.Start | Actions.Stop | Actions.RunCycle | Actions.GetType | Actions.SetType | Actions.GetLength | Actions.SetRefresh | Actions.CreateLink | Actions.Upload | Actions.Download | Actions.SetActive | Actions.SetHysteresis | Actions.Disconnect | Actions.SetInitValue | Actions.GetLBType | Actions.ModuleInfo | Actions.ListSNMPVariables | Actions.Connected | Actions.Uploading | Actions.GetCpuInfo | Actions.ReadPVFormat | Actions.FireActivated | Actions.ReadIndex | Actions.FireDataValidated | Actions.FireValueChanged | Actions.Link;
            this.ReadInternalValue();
          }
        }
        else if (Actions.GetValue == (this.Requests & Actions.GetValue))
        {
          this.Requests &= Actions.Connect | Actions.GetList | Actions.SetValue | Actions.GetForce | Actions.SetForce | Actions.Start | Actions.Stop | Actions.RunCycle | Actions.GetType | Actions.SetType | Actions.GetLength | Actions.SetRefresh | Actions.CreateLink | Actions.Upload | Actions.Download | Actions.SetActive | Actions.SetHysteresis | Actions.Disconnect | Actions.SetInitValue | Actions.GetLBType | Actions.ModuleInfo | Actions.ListSNMPVariables | Actions.Connected | Actions.Uploading | Actions.GetCpuInfo | Actions.ReadPVFormat | Actions.FireActivated | Actions.ReadIndex | Actions.FireDataValidated | Actions.FireValueChanged | Actions.Link;
          this.ReadInternalValue();
        }
      }
      catch (OutOfMemoryException ex)
      {
        string message = ex.Message;
        this.CleanupMemory();
        retVal = 14;
      }
      catch (System.Exception ex)
      {
        string message = ex.Message;
        retVal = 14;
      }
      return retVal;
    }

    internal override void OnPviWritten(
      int errorCode,
      PVIWriteAccessTypes accessType,
      PVIDataStates dataState,
      int option,
      IntPtr pData,
      uint dataLen)
    {
      this.propErrorCode = errorCode;
      switch (accessType)
      {
        case PVIWriteAccessTypes.EventMask:
          switch (option)
          {
            case 1:
              this.OnActivated(new PviEventArgs(this.Name, this.Address, errorCode, this.Service.Language, Action.VariableActivate, this.Service));
              return;
            case 2:
              return;
            default:
              this.OnDeactivated(new PviEventArgs(this.Name, this.Address, errorCode, this.Service.Language, Action.VariableDeactivate, this.Service));
              return;
          }
        case PVIWriteAccessTypes.Data:
          this.ValidateConnection(errorCode);
          if (!this.propActive && !this.propPviValue.isAssigned && this.PVRoot.propWriteByteField != null)
            this.PVRoot.propPviValue.propByteField = (byte[]) this.PVRoot.propWriteByteField.Clone();
          this.OnValueWritten(new PviEventArgs(this.Name, this.Address, errorCode, this.Service.Language, Action.VariableValueWrite, this.Service));
          if (!this.propSendChangedEvent)
            break;
          this.propSendChangedEvent = false;
          if (errorCode != 0)
            break;
          this.ReadInternalValue();
          break;
        case PVIWriteAccessTypes.BasicAttributes:
          this.OnPropertyChanged(new PviEventArgs(this.Name, this.Address, errorCode, this.Service.Language, Action.VariableSetType, this.Service));
          break;
        case PVIWriteAccessTypes.Refresh:
          this.OnPropertyChanged(new PviEventArgs(this.Name, this.Address, errorCode, this.Service.Language, Action.VariableSetRefreshTime, this.Service));
          break;
        case PVIWriteAccessTypes.Hysteresis:
          this.OnPropertyChanged(new PviEventArgs(this.Name, this.Address, errorCode, this.Service.Language, Action.VariableSetHysteresis, this.Service));
          break;
        case PVIWriteAccessTypes.ConversionFunction:
          this.OnPropertyChanged(new PviEventArgs(this.Name, this.Address, errorCode, this.Service.Language, Action.VariableScalingChange));
          break;
        case PVIWriteAccessTypes.UserTag:
          this.OnPropertyChanged(new PviEventArgs(this.Name, this.Address, errorCode, this.Service.Language, Action.UserTag, this.Service));
          break;
        case PVIWriteAccessTypes.ForceOn:
          this.OnForcedOn(new PviEventArgs(this.Name, this.Address, errorCode, this.Service.Language, Action.VariableSetForce, this.Service));
          break;
        case PVIWriteAccessTypes.ForceOff:
          this.OnForcedOff(new PviEventArgs(this.Name, this.Address, errorCode, this.Service.Language, Action.VariableSetForce, this.Service));
          break;
        default:
          base.OnPviWritten(errorCode, accessType, dataState, option, pData, dataLen);
          break;
      }
    }

    internal override void OnPviUnLinked(int errorCode, int option)
    {
      this.propErrorCode = errorCode;
      switch (option)
      {
        case 1:
        case 2:
          if (ConnectionStates.Disconnecting == this.propConnectionState)
          {
            this.propLinkId = 0U;
            if (this.Service == null)
              break;
            if (2 == option)
            {
              this.OnDisconnected(new PviEventArgs(this.Name, this.Address, errorCode, this.Service.Language, Action.VariablesDisconnect, this.Service));
              break;
            }
            this.OnDisconnected(new PviEventArgs(this.Name, this.Address, errorCode, this.Service.Language, Action.VariableDisconnect, this.Service));
            break;
          }
          if (this.Service == null)
            break;
          this.OnError(new PviEventArgs(this.Name, this.Address, errorCode, this.Service.Language, Action.VariableDisconnect, this.Service));
          break;
        case 4:
          this.propConnectionState = ConnectionStates.Unininitialized;
          this.propLinkId = 0U;
          this.Connect();
          break;
      }
    }

    internal override void OnPviDeleted(int errorCode)
    {
      this.propErrorCode = errorCode;
      base.OnPviDeleted(errorCode);
    }

    internal override void OnPviChangedLink(int errorCode)
    {
      this.propErrorCode = errorCode;
      base.OnPviChangedLink(errorCode);
    }

    internal static Value GetValueFromString(string strValue, DataType dataType)
    {
      switch (dataType)
      {
        case DataType.Boolean:
          return new Value(System.Convert.ToBoolean(strValue));
        case DataType.SByte:
          return new Value(System.Convert.ToSByte(strValue));
        case DataType.Int16:
          return new Value(System.Convert.ToInt16(strValue));
        case DataType.Int32:
          return new Value(System.Convert.ToInt32(strValue));
        case DataType.Int64:
          return new Value((float) System.Convert.ToInt64(strValue));
        case DataType.Byte:
          return new Value(System.Convert.ToByte(strValue));
        case DataType.UInt16:
          return new Value(System.Convert.ToUInt16(strValue));
        case DataType.UInt32:
          return new Value(System.Convert.ToUInt32(strValue));
        case DataType.UInt64:
          return new Value(System.Convert.ToUInt64(strValue));
        case DataType.Single:
          return new Value(System.Convert.ToSingle(strValue));
        case DataType.Double:
          return new Value(System.Convert.ToDouble(strValue));
        case DataType.TimeSpan:
          return new Value(System.Convert.ToDateTime(strValue));
        case DataType.DateTime:
          return new Value(System.Convert.ToDateTime(strValue));
        case DataType.String:
          return new Value(strValue);
        case DataType.TimeOfDay:
          return new Value(System.Convert.ToDateTime(strValue));
        case DataType.Date:
          return new Value(System.Convert.ToDateTime(strValue));
        case DataType.WORD:
          return new Value(System.Convert.ToUInt16(strValue));
        case DataType.DWORD:
          return new Value(System.Convert.ToUInt32(strValue));
        case DataType.UInt8:
          return new Value(System.Convert.ToByte(strValue));
        case DataType.TOD:
          return new Value(System.Convert.ToDateTime(strValue));
        case DataType.DT:
          return new Value(System.Convert.ToDateTime(strValue));
        default:
          return (Value) null;
      }
    }

    private unsafe int ConvertComplexValue(IntPtr pData, ref ArrayList changedMembers)
    {
      int num1 = 0;
      if (this.propPviValue.DataType == DataType.Structure)
      {
        for (int index1 = 0; index1 < this.Members.Count; ++index1)
        {
          byte* numPtr = (byte*) ((IntPtr) pData.ToPointer() + this.Members[index1].propOffset);
          if (this.propPviValue.ArrayLength > 0)
          {
            for (int index2 = 0; index2 < this.propPviValue.ArrayLength; ++index2)
            {
              for (int index3 = 0; index3 < this.Members[index1].propPviValue.propTypeLength * this.Members[index1].propPviValue.propArrayLength; ++index3)
              {
                if ((int) (numPtr + index3)[((IntPtr) index2 * this.propPviValue.propTypeLength).ToInt64()] != (int) this.propPviValue.propByteField[this.Members[index1].propOffset + index3 + index2 * this.propPviValue.propTypeLength] || !this.propDataValid)
                {
                  if (this.Members[index1].propPviValue.DataType == DataType.Structure)
                  {
                    string str1 = (string) null;
                    if (this.propPviValue.ArrayLength > 1)
                      str1 = string.Format("{0}[{1}].", (object) this.Name, (object) index2.ToString());
                    Variable member = this.Members[index1];
                    int num2 = index3 % member.propPviValue.propTypeLength;
                    Variable memberByOffset = member.GetMemberByOffset(index3 % member.propPviValue.propTypeLength);
                    if (memberByOffset != null)
                    {
                      string str2 = memberByOffset.Name;
                      Variable variable = memberByOffset;
                      do
                      {
                        variable = (Variable) variable.Owner;
                        string str3 = str2.Insert(0, ".");
                        if (variable.propPviValue.ArrayLength > 1)
                        {
                          int num3 = (!(this.Members[index1].Name == variable.Name) ? (variable.propOffset >= num2 ? num2 % ((Variable) variable.propOwner).propPviValue.propTypeLength : (num2 - variable.propOffset) % ((Variable) variable.propOwner).propPviValue.propTypeLength) : index3 % ((Variable) variable.propOwner).propPviValue.propTypeLength) / variable.propPviValue.TypeLength;
                          str3 = str3.Insert(0, string.Format("[{0}]", (object) num3.ToString()));
                        }
                        str2 = str3.Insert(0, variable.Name);
                      }
                      while (this.Members[index1].Name != variable.Name);
                      if (str1 != null)
                        str2 = str2.Insert(0, str1);
                      bool flag = false;
                      if (memberByOffset.propPviValue.ArrayLength > 1)
                      {
                        int num4 = ((Variable) memberByOffset.propOwner).propOffset > num2 || !(memberByOffset.propOwner.Name != variable.Name) ? System.Convert.ToInt32((num2 - memberByOffset.propOffset) % ((Variable) memberByOffset.propOwner).propPviValue.TypeLength / memberByOffset.propPviValue.propTypeLength) : System.Convert.ToInt32((num2 - ((Variable) memberByOffset.propOwner).propOffset - memberByOffset.propOffset) % ((Variable) memberByOffset.propOwner).propPviValue.TypeLength / memberByOffset.propPviValue.propTypeLength);
                        if (num4 > -1 && num4 < memberByOffset.propPviValue.ArrayLength)
                          str2 = string.Format("{0}[{1}]", (object) str2, (object) num4.ToString());
                      }
                      foreach (string str4 in changedMembers)
                      {
                        if (str2.Equals(str4))
                        {
                          flag = true;
                          break;
                        }
                      }
                      if (!flag)
                        changedMembers[num1++] = (object) str2;
                    }
                  }
                  else
                  {
                    string str5 = this.Members[index1].Name;
                    bool flag = false;
                    int int32 = System.Convert.ToInt32(index3 / this.Members[index1].propPviValue.propTypeLength);
                    if (int32 > -1)
                    {
                      if (this.Members[index1].propPviValue.ArrayLength > 1)
                      {
                        if (this.propPviValue.ArrayLength > 1)
                          str5 = string.Format("{0}[{1}].{2}[{3}]", (object) this.Name, (object) index2.ToString(), (object) this.Members[index1].Name, (object) int32.ToString());
                        else
                          str5 = string.Format("{0}[{1}]", (object) this.Members[index1].Name, (object) int32.ToString());
                      }
                      else
                        str5 = this.propPviValue.ArrayLength <= 1 ? string.Format("{0}", (object) this.Members[index1].Name) : string.Format("{0}[{1}].{2}", (object) this.Name, (object) index2.ToString(), (object) this.Members[index1].Name);
                    }
                    foreach (string str6 in changedMembers)
                    {
                      if (str5.Equals(str6))
                      {
                        flag = true;
                        break;
                      }
                    }
                    if (!flag)
                      changedMembers[num1++] = (object) str5;
                  }
                  this.propPviValue.propByteField[this.Members[index1].propOffset + index3 + index2 * this.propPviValue.propTypeLength] = (numPtr + index3)[((IntPtr) index2 * this.propPviValue.propTypeLength).ToInt64()];
                }
              }
            }
          }
        }
      }
      else
      {
        for (int index4 = 0; index4 < this.propPviValue.propArrayLength; ++index4)
        {
          byte* numPtr = (byte*) ((IntPtr) pData.ToPointer() + (IntPtr) index4 * this.propPviValue.propTypeLength);
          bool flag = false;
          for (int index5 = 0; index5 < this.propPviValue.propTypeLength; ++index5)
          {
            if ((int) numPtr[index5] != (int) this.propPviValue.propByteField[index4 * this.propPviValue.propTypeLength + index5] || !this.propDataValid)
            {
              this.propPviValue.propByteField[index4 * this.propPviValue.propTypeLength + index5] = numPtr[index5];
              flag = true;
            }
          }
          if (flag)
            changedMembers[num1++] = (object) index4.ToString();
        }
      }
      PviMarshal.FreeHGlobal(ref this.propPviValue.pData);
      return 0;
    }

    private void ConvertStructAndArrayValues(
      IntPtr pData,
      uint dataLen,
      ref ArrayList changedMembers)
    {
      if (this.propPviValue.propByteField == null || this.propPviValue.propByteField.Length != this.propPviValue.propArrayLength * this.propPviValue.propTypeLength)
        this.propPviValue.propByteField = new byte[this.propPviValue.propArrayLength * this.propPviValue.propTypeLength];
      Marshal.Copy(pData, this.propPviValue.propByteField, 0, (int) dataLen);
      if (!this.propPviValue.isAssigned)
      {
        if (IntPtr.Zero == this.propPviValue.pData)
        {
          this.propPviValue.pData = PviMarshal.AllocHGlobal(this.propPviValue.DataSize);
          this.propPviValue.propHasOwnDataPtr = true;
        }
        Marshal.Copy(this.propPviValue.propByteField, 0, this.propPviValue.pData, this.propPviValue.DataSize);
      }
      if (DataType.Structure != this.propPviValue.DataType)
      {
        if (this.propPviValue.IsOfTypeArray)
        {
          for (int arrayMinIndex = this.propPviValue.ArrayMinIndex; arrayMinIndex < this.propPviValue.ArrayMaxIndex + 1; ++arrayMinIndex)
          {
            if (this.HasDataChanged(this.propPviValue.propByteField, (arrayMinIndex - this.propPviValue.ArrayMinIndex) * this.propPviValue.TypeLength, this.propPviValue.TypeLength))
              changedMembers.Add((object) ("[" + (object) arrayMinIndex + "]"));
          }
        }
        else if (this.HasDataChanged(this.propPviValue.propByteField, 0, this.propPviValue.TypeLength))
          changedMembers.Add((object) "[0]");
      }
      else if (this.ExpandMembers && this.propPviValue.DataType != DataType.Unknown && this.mapNameToMember != null)
      {
        for (int index = 0; index < this.mapNameToMember.Count; ++index)
        {
          Variable variable = (Variable) this.mapNameToMember[index];
          if (DataType.Structure != variable.Value.DataType && !variable.Value.IsOfTypeArray)
          {
            if (this.HasDataChanged(this.propPviValue.propByteField, variable.propOffset, variable.Value.TypeLength))
              changedMembers.Add((object) variable.StructMemberName);
            variable.InternalSetValue(pData, dataLen, variable.propOffset);
          }
          else
            variable.Value.UpdateByteField(pData, dataLen, variable.propOffset);
        }
      }
      if (this.propInternalByteField == null)
        this.propInternalByteField = new byte[this.propPviValue.propByteField.Length];
      Marshal.Copy(pData, this.propInternalByteField, 0, (int) dataLen);
    }

    private int ConvertPviValue(IntPtr pData, uint dataLen, ref ArrayList changedMembers)
    {
      int num = 0;
      try
      {
        if (1 < this.propPviValue.ArrayLength || this.propPviValue.propDataType == DataType.Structure)
          this.ConvertStructAndArrayValues(pData, dataLen, ref changedMembers);
        else
          this.ConvertSimpleValues(pData, dataLen, ref changedMembers, ref this.propPviValue);
      }
      catch (OutOfMemoryException ex)
      {
        string message = ex.Message;
        this.CleanupMemory();
        num = 14;
      }
      catch (System.Exception ex)
      {
        string message = ex.Message;
        num = 14;
      }
      return num;
    }

    private void ConvertSimpleValues(
      IntPtr pData,
      uint dataLen,
      ref ArrayList changedMembers,
      ref Value ppValue)
    {
      if (0 < this.BitOffset)
      {
        ppValue.propObjValue = (object) System.Convert.ToBoolean(Marshal.ReadByte(pData));
      }
      else
      {
        switch (this.propPviValue.propDataType)
        {
          case DataType.Boolean:
            byte num1 = Marshal.ReadByte(pData);
            ppValue.propObjValue = (object) System.Convert.ToBoolean(num1);
            break;
          case DataType.SByte:
            ppValue.propObjValue = (object) (sbyte) Marshal.ReadByte(pData);
            break;
          case DataType.Int16:
            ppValue.propObjValue = (object) Marshal.ReadInt16(pData);
            break;
          case DataType.Int32:
            ppValue.propObjValue = (object) Marshal.ReadInt32(pData);
            break;
          case DataType.Int64:
            ppValue.propObjValue = (object) PviMarshal.ReadInt64(pData);
            break;
          case DataType.Byte:
          case DataType.UInt8:
            ppValue.propObjValue = (object) Marshal.ReadByte(pData);
            break;
          case DataType.UInt16:
          case DataType.WORD:
            ppValue.propObjValue = (object) (ushort) Marshal.ReadInt16(pData);
            break;
          case DataType.UInt32:
          case DataType.DWORD:
            ppValue.propObjValue = (object) (uint) Marshal.ReadInt32(pData);
            break;
          case DataType.UInt64:
          case DataType.LWORD:
            ppValue.propObjValue = (object) (ulong) PviMarshal.ReadInt64(pData);
            break;
          case DataType.Single:
            float[] destination1 = new float[1];
            Marshal.Copy(pData, destination1, 0, 1);
            ppValue.propObjValue = (object) destination1[0];
            break;
          case DataType.Double:
            double[] destination2 = new double[1];
            Marshal.Copy(pData, destination2, 0, 1);
            ppValue.propObjValue = (object) destination2[0];
            break;
          case DataType.TimeSpan:
          case DataType.TimeOfDay:
          case DataType.TOD:
            uint num2 = (uint) Marshal.ReadInt32(pData);
            ppValue.propUInt32Val = num2;
            long ticks = (long) num2 * 10000L;
            ppValue.propObjValue = (object) new TimeSpan(ticks);
            break;
          case DataType.DateTime:
          case DataType.Date:
          case DataType.DT:
            uint timeValue = (uint) Marshal.ReadInt32(pData);
            DateTime dateTime = new DateTime(55L);
            ppValue.propUInt32Val = timeValue;
            ppValue.propObjValue = (object) Pvi.UInt32ToDateTime(timeValue);
            break;
          case DataType.String:
            string ansiString = PviMarshal.ToAnsiString(pData, dataLen);
            ppValue.propObjValue = (object) ansiString;
            break;
          case DataType.WString:
            string wstring = PviMarshal.ToWString(pData, dataLen);
            ppValue.propObjValue = (object) wstring;
            break;
        }
      }
    }

    private bool HasDataChanged(byte[] bCompare, int offset, int size)
    {
      if (this.propInternalByteField == null)
        return true;
      for (int index = offset; index < offset + size; ++index)
      {
        if ((int) bCompare[index] != (int) this.propInternalByteField[index])
          return true;
      }
      return false;
    }

    internal virtual void CheckStatus(object state)
    {
    }

    internal static string GetDataTypeString(DataType dataType)
    {
      switch (dataType)
      {
        case DataType.Boolean:
          return "Boolean";
        case DataType.SByte:
          return "SByte";
        case DataType.Int16:
          return "Int16";
        case DataType.Int32:
          return "Int32";
        case DataType.Int64:
          return "Int64";
        case DataType.Byte:
          return "BYTE";
        case DataType.UInt16:
          return "UInt16";
        case DataType.UInt32:
          return "UInt32";
        case DataType.UInt64:
          return "UInt64";
        case DataType.Single:
          return "Single";
        case DataType.Double:
          return "Double";
        case DataType.TimeSpan:
          return "Timespan";
        case DataType.DateTime:
          return "DateTime";
        case DataType.String:
          return "String";
        case DataType.Structure:
          return "Structure";
        case DataType.WString:
          return "WString";
        case DataType.TimeOfDay:
          return "TIME_OF_DAY";
        case DataType.Date:
          return "DATE";
        case DataType.WORD:
          return "WORD";
        case DataType.DWORD:
          return "DWORD";
        case DataType.LWORD:
          return "LWORD";
        case DataType.UInt8:
          return "USINT";
        case DataType.TOD:
          return "TOD";
        case DataType.DT:
          return "DT";
        default:
          return "";
      }
    }

    internal static DataType GetDataTypeFromString(string name)
    {
      if (name.IndexOf("Structure") != -1)
        return DataType.Structure;
      switch (name)
      {
        case "Boolean":
          return DataType.Boolean;
        case "SByte":
          return DataType.SByte;
        case "Int16":
          return DataType.Int16;
        case "Int32":
          return DataType.Int32;
        case "Int64":
          return DataType.Int64;
        case "Byte":
          return DataType.Byte;
        case "BYTE":
          return DataType.Byte;
        case "USINT":
          return DataType.UInt8;
        case "UInt8":
          return DataType.UInt8;
        case "UInt16":
          return DataType.UInt16;
        case "UINT":
          return DataType.UInt16;
        case "WORD":
          return DataType.WORD;
        case "LWORD":
          return DataType.LWORD;
        case "UInt32":
          return DataType.UInt32;
        case "DWORD":
          return DataType.DWORD;
        case "UInt64":
          return DataType.UInt64;
        case "Single":
          return DataType.Single;
        case "Double":
          return DataType.Double;
        case "TimeSpan":
          return DataType.TimeSpan;
        case "TimeOfDay":
          return DataType.TimeOfDay;
        case "TIME_OF_DAY":
          return DataType.TimeOfDay;
        case "TOD":
          return DataType.TOD;
        case "DateTime":
          return DataType.DateTime;
        case "Date":
          return DataType.Date;
        case "DATE_AND_TIME":
          return DataType.DateTime;
        case "DT":
          return DataType.DT;
        case "DATE":
          return DataType.Date;
        case "D":
          return DataType.Date;
        case "String":
          return DataType.String;
        case "WString":
          return DataType.WString;
        case "WSTRING":
          return DataType.WString;
        default:
          return DataType.Unknown;
      }
    }

    internal static DataType GetDataType(string pviString, sbyte isBitString, ref int typeLength)
    {
      DataType dataType = DataType.Unknown;
      if (pviString.Length == 0)
        return DataType.Unknown;
      int num = pviString.IndexOf("VT=");
      if (num != -1)
      {
        if (string.Compare(pviString, num + 3, "i8", 0, "i8".Length) == 0)
          dataType = DataType.SByte;
        else if (string.Compare(pviString, num + 3, "i16", 0, "i16".Length) == 0)
        {
          dataType = DataType.Int16;
          typeLength = 2;
        }
        else if (string.Compare(pviString, num + 3, "i32", 0, "i32".Length) == 0)
        {
          dataType = DataType.Int32;
          typeLength = 4;
        }
        else if (string.Compare(pviString, num + 3, "i64", 0, "i64".Length) == 0)
        {
          dataType = DataType.Int64;
          typeLength = 8;
        }
        else if (string.Compare(pviString, num + 3, "u8", 0, "u8".Length) == 0)
        {
          dataType = DataType.UInt8;
          typeLength = 1;
          if ((sbyte) 1 == isBitString)
            dataType = DataType.Byte;
        }
        else if (string.Compare(pviString, num + 3, "byte", 0, "u8".Length) == 0)
        {
          dataType = DataType.Byte;
          typeLength = 1;
        }
        else if (string.Compare(pviString, num + 3, "u16", 0, "u16".Length) == 0)
        {
          dataType = DataType.UInt16;
          typeLength = 2;
          if ((sbyte) 1 == isBitString)
            dataType = DataType.WORD;
        }
        else if (string.Compare(pviString, num + 3, "WORD", 0, "WORD".Length) == 0)
        {
          dataType = DataType.WORD;
          typeLength = 2;
        }
        else if (string.Compare(pviString, num + 3, "u32", 0, "u32".Length) == 0)
        {
          dataType = DataType.UInt32;
          typeLength = 4;
          if ((sbyte) 1 == isBitString)
            dataType = DataType.DWORD;
        }
        else if (string.Compare(pviString, num + 3, "DWORD", 0, "DWORD".Length) == 0)
        {
          dataType = DataType.DWORD;
          typeLength = 4;
        }
        else if (string.Compare(pviString, num + 3, "u64", 0, "u64".Length) == 0)
        {
          dataType = DataType.UInt64;
          typeLength = 8;
          if ((sbyte) 1 == isBitString)
            dataType = DataType.LWORD;
        }
        else if (string.Compare(pviString, num + 3, "f32", 0, "f32".Length) == 0)
        {
          dataType = DataType.Single;
          typeLength = 4;
        }
        else if (string.Compare(pviString, num + 3, "f64", 0, "f64".Length) == 0)
        {
          dataType = DataType.Double;
          typeLength = 8;
        }
        else if (string.Compare(pviString, num + 3, "string", 0, "string".Length) == 0)
          dataType = DataType.String;
        else if (string.Compare(pviString, num + 3, "wstring", 0, "wstring".Length) == 0)
          dataType = DataType.WString;
        else if (string.Compare(pviString, num + 3, "boolean", 0, "boolean".Length) == 0)
        {
          dataType = DataType.Boolean;
          typeLength = 1;
        }
        else if (string.Compare(pviString, num + 3, "struct", 0, "struct".Length) == 0)
          dataType = DataType.Structure;
        else if (string.Compare(pviString, num + 3, "dt", 0, "dt".Length) == 0)
        {
          dataType = DataType.DT;
          typeLength = 4;
        }
        else if (string.Compare(pviString, num + 3, "date", 0, "date".Length) == 0)
        {
          dataType = DataType.Date;
          typeLength = 4;
        }
        else if (string.Compare(pviString, num + 3, "time", 0, "time".Length) == 0)
        {
          dataType = DataType.TimeSpan;
          typeLength = 4;
        }
        else if (string.Compare(pviString, num + 3, "tod", 0, "tod".Length) == 0)
        {
          dataType = DataType.TOD;
          typeLength = 4;
        }
        else
          dataType = string.Compare(pviString, num + 3, "data", 0, "data".Length) != 0 ? DataType.Unknown : DataType.Data;
      }
      return dataType;
    }

    internal static int GetDataTypeLength(string pviString, int defaultTypeLen)
    {
      int dataTypeLength = defaultTypeLen;
      if (0 < pviString.Length)
      {
        int num1 = pviString.IndexOf("VL=");
        if (-1 != num1)
        {
          int num2 = pviString.IndexOf(" ", num1 + 3);
          pviString = num2 == -1 ? pviString.Substring(num1 + 3, pviString.Length - num1 - 3) : pviString.Substring(num1 + 3, num2 - num1 - 3);
          dataTypeLength = System.Convert.ToInt32(pviString);
        }
      }
      return dataTypeLength;
    }

    internal static int GetArrayLength(string pviString)
    {
      if (pviString.Length == 0)
        return -1;
      int num1 = pviString.IndexOf("VN=");
      int num2 = pviString.IndexOf(" ", num1 + 3);
      pviString = num2 == -1 ? pviString.Substring(num1 + 3, pviString.Length - num1 - 3) : pviString.Substring(num1 + 3, num2 - num1 - 3);
      int length = pviString.IndexOf('}');
      if (-1 != length)
        pviString = pviString.Substring(0, length);
      return System.Convert.ToInt32(pviString);
    }

    internal static string GetVariableName(string pviString)
    {
      int length = pviString.IndexOf(" ");
      return length != -1 ? pviString.Substring(0, length) : (string) null;
    }

    internal static bool GetForceStatus(string pviString, ref bool forceValue)
    {
      if (pviString.Length == 0)
        return false;
      int num = pviString.IndexOf("FC=");
      if (num == -1)
        return false;
      pviString = pviString.Substring(num + 3, 1);
      forceValue = System.Convert.ToBoolean(System.Convert.ToByte(pviString));
      return true;
    }

    internal static Access GetAccessStatus(string pviString)
    {
      if (pviString.Length == 0)
        return Access.No;
      int num = pviString.IndexOf("IO=");
      if (num == -1)
        return Access.No;
      pviString = pviString.Substring(num + 3, 1);
      return -1 != pviString.IndexOf("r") ? Access.Read : Access.Write;
    }

    internal static VariableAttribute GetAttribute(string pviString)
    {
      VariableAttribute attribute = VariableAttribute.None;
      string str = "";
      if (pviString.Length == 0)
        return VariableAttribute.None;
      int num1 = pviString.IndexOf("IO=");
      if (num1 != -1)
        str = pviString.Substring(num1 + 3, 1);
      if (-1 != str.IndexOf("w"))
        attribute |= VariableAttribute.Output;
      if (-1 != str.IndexOf("r"))
        attribute |= VariableAttribute.Input;
      int startIndex = pviString.IndexOf("ST=");
      int num2 = pviString.IndexOf(" ", startIndex);
      string strA = num2 == -1 ? pviString.Substring(startIndex + 3) : pviString.Substring(startIndex + 3, num2 - startIndex - 3);
      if (string.Compare(strA, "Const") == 0)
        attribute |= VariableAttribute.Constant;
      else if (string.Compare(strA, "Var") == 0)
        attribute |= VariableAttribute.Variable;
      return attribute;
    }

    internal void GetScope(string pviString, ref Scope sopeValue)
    {
      int num = pviString.IndexOf("SC=");
      pviString = pviString.Substring(num + 3, 1);
      if (string.Compare(pviString, "g") == 0)
        sopeValue = Scope.Global;
      else if (string.Compare(pviString, "l") == 0)
      {
        sopeValue = Scope.Local;
      }
      else
      {
        if (string.Compare(pviString, "d") != 0)
          return;
        sopeValue = Scope.Dynamic;
      }
    }

    internal static Scope EvaluateScope(string pviString)
    {
      int num = pviString.IndexOf("SC=");
      pviString = pviString.Substring(num + 3, 1);
      if (string.Compare(pviString, "g") == 0)
        return Scope.Global;
      if (string.Compare(pviString, "l") == 0)
        return Scope.Local;
      return string.Compare(pviString, "d") == 0 ? Scope.Dynamic : Scope.UNDEFINED;
    }

    internal int CreateMembers(string pviText, ref int typeLength, bool expandMembers) => !expandMembers ? this.CreateMembersNOExpand(pviText, ref typeLength, this.Service.AddMembersToVariableCollection) : this.CreateMembersExpanded(pviText, ref typeLength, this.Service.AddMembersToVariableCollection);

    private int CreateMembersNOExpand(string pviText, ref int typeLength, bool addToVCollections)
    {
      int membersNoExpand = 0;
      string[] strArray = pviText.Split('{');
      if (this.propMembers != null)
        this.propMembers.CleanUp(false);
      if (this.mapNameToMember != null)
      {
        this.mapNameToMember.Clear();
        this.mapNameToMember = (StructMemberCollection) null;
      }
      this.propPviValue.propArrayLength = Variable.GetArrayLength(strArray[0]);
      this.propAlignment = Variable.GetAlignment(strArray[0]);
      this.propPviValue.propTypeLength = Variable.GetDataTypeLength(strArray[0], this.propPviValue.propTypeLength);
      this.propOffset = 0;
      this.propInternalOffset = 0;
      if (CastModes.PG2000String == (this.CastMode & CastModes.PG2000String) && this.propPviValue.IsOfTypeArray)
        return -1;
      if (addToVCollections)
        this.AddComplexVariableItem();
      return membersNoExpand;
    }

    internal void AddComplexVariableItem()
    {
      if (1 < this.propPviValue.propArrayLength)
      {
        for (int arrayMinIndex = this.propPviValue.ArrayMinIndex; arrayMinIndex < this.propPviValue.ArrayMaxIndex + 1; ++arrayMinIndex)
        {
          string str = "[" + arrayMinIndex.ToString() + "]";
          string vAddress = this.Address + str;
          Variable memberVar = this.LookupInParentCollection(str, vAddress, this);
          if (memberVar == null)
          {
            memberVar = new Variable(this, str, true);
            memberVar.Address = vAddress;
            this.AddToParentCollection(memberVar, this.propParent, this.Service.AddMembersToVariableCollection);
          }
          memberVar.propAlignment = this.propAlignment;
          memberVar.propScope = this.propScope;
          memberVar.Address = this.Address + str;
          memberVar.propPviValue.propTypeLength = this.propPviValue.propTypeLength;
          memberVar.propPviValue.SetArrayLength(1);
          if (memberVar.propPviValue.ArrayMinIndex == 0)
            memberVar.propPviValue.propArrayMaxIndex = memberVar.propPviValue.ArrayLength - 1;
          memberVar.propOffset = this.propOffset + arrayMinIndex * memberVar.propPviValue.propTypeLength;
          memberVar.propPviValue.propArryOne = this.propPviValue.propArryOne;
          memberVar.propPviValue.propDimensions = this.propPviValue.propDimensions;
          memberVar.propPviValue.SetDataType(this.propPviValue.DataType);
        }
      }
      else
      {
        if (!this.propPviValue.IsOfTypeArray)
          return;
        string str = "[0]";
        string vAddress = this.Address + str;
        Variable memberVar = this.LookupInParentCollection(str, vAddress, this);
        if (memberVar == null)
        {
          memberVar = new Variable(this, str, true);
          memberVar.Address = vAddress;
          this.AddToParentCollection(memberVar, this.propParent, this.Service.AddMembersToVariableCollection);
        }
        memberVar.propAlignment = this.propAlignment;
        memberVar.propScope = this.propScope;
        memberVar.propPviValue.propTypeLength = this.propPviValue.propTypeLength;
        memberVar.propPviValue.SetArrayLength(1);
        if (memberVar.propPviValue.ArrayMinIndex == 0)
          memberVar.propPviValue.propArrayMaxIndex = memberVar.propPviValue.ArrayLength - 1;
        memberVar.propOffset = this.propOffset;
        memberVar.propPviValue.propArryOne = this.propPviValue.propArryOne;
        memberVar.propPviValue.propDimensions = this.propPviValue.propDimensions;
        memberVar.propPviValue.SetDataType(this.propPviValue.DataType);
      }
    }

    private int CreateMembersExpanded(string pviText, ref int typeLength, bool addToVCollections)
    {
      int membersExpanded = 0;
      int num = 1;
      Variable variable1 = this;
      int byteOffset = 0;
      bool isMDimArray = false;
      string[] typeInfo = pviText.Split('{');
      if (this.propMembers != null)
        this.propMembers.CleanUp(false);
      if (this.mapNameToMember != null)
      {
        this.mapNameToMember.Clear();
        this.mapNameToMember = (StructMemberCollection) null;
      }
      this.propPviValue.propArrayLength = Variable.GetArrayLength(typeInfo[0]);
      if (CastModes.PG2000String == (this.CastMode & CastModes.PG2000String) && this.propPviValue.IsOfTypeArray)
        return -1;
      this.propAlignment = Variable.GetAlignment(typeInfo[0]);
      this.GetVSParameters(typeInfo[0], ref this.propPviValue, this.propPviValue.DataType);
      this.propStructName = this.GetSNParameter(typeInfo[0]);
      this.propPviValue.propTypeLength = Variable.GetDataTypeLength(typeInfo[0], this.propPviValue.propTypeLength);
      this.propOffset = 0;
      this.propInternalOffset = 0;
      if (this.propPviValue.IsOfTypeArray)
      {
        int infoIdx = 0;
        this.CreateStructArray(this, this, typeInfo, this.propOffset, ref byteOffset, ref infoIdx, addToVCollections);
        byteOffset = this.propPviValue.propTypeLength;
      }
      else
      {
        for (int infoIdx = 1; infoIdx < typeInfo.Length; ++infoIdx)
        {
          string[] strArray = Variable.GetStructElementName(typeInfo[infoIdx]).Split('.');
          int length = strArray.Length;
          if (num > length)
          {
            for (int index = num; index > strArray.Length; --index)
              variable1 = (Variable) variable1.propOwner;
          }
          num = strArray.Length;
          string str = strArray[strArray.Length - 1];
          if (str.Length == 0)
          {
            str = this.propPviInternStructElement.ToString();
            ++this.propPviInternStructElement;
          }
          string vAddress = variable1.Address + "." + str;
          if (this.propUserMembers != null && this.propUserMembers.Count > 0)
          {
            if (this.propUserMembers.ContainsKey((object) str))
            {
              Variable propUserMember = this.propUserMembers[str];
              Variable variable2 = propUserMember;
              variable2.propOwner = (Base) variable1;
              variable2.propPviValue.SetDataType(Variable.GetDataType(typeInfo[infoIdx], variable2.Value.IsBitString, ref variable2.propPviValue.propTypeLength));
              variable2.GetVSParameters(typeInfo[infoIdx], ref variable2.propPviValue, variable2.propPviValue.DataType);
              variable2.propPviValue.propTypeLength = Variable.GetDataTypeLength(typeInfo[infoIdx], variable2.propPviValue.propTypeLength);
              variable2.propPviValue.SetArrayLength(Variable.GetArrayLength(typeInfo[infoIdx]));
              if (variable2.propPviValue.ArrayMinIndex == 0)
                variable2.propPviValue.propArrayMaxIndex = variable2.propPviValue.ArrayLength - 1;
              variable2.propOffset = byteOffset;
              byteOffset = variable2.propOffset + variable2.propPviValue.propTypeLength * variable2.propPviValue.ArrayLength;
              if (variable2.propPviValue.DataType == DataType.Structure)
              {
                variable2.propStructName = this.GetSNParameter(typeInfo[infoIdx]);
                variable1 = variable2;
              }
              this.AddToParentCollection(variable2, this.propParent, addToVCollections);
              this.AddMember(variable2);
              this.propUserMembers.Remove(propUserMember.Name);
            }
            else
            {
              Variable memberVar = this.LookupInParentCollection(str, vAddress, this);
              if (memberVar == null)
              {
                memberVar = new Variable(true, variable1, str, addToVCollections);
                memberVar.Address = vAddress;
                this.AddToParentCollection(memberVar, this.propParent, addToVCollections);
              }
              memberVar.propAlignment = this.propAlignment;
              memberVar.propScope = this.propScope;
              memberVar.propOwner = (Base) variable1;
              memberVar.propPviValue.SetDataType(Variable.GetDataType(typeInfo[infoIdx], memberVar.Value.IsBitString, ref memberVar.propPviValue.propTypeLength));
              memberVar.GetVSParameters(typeInfo[infoIdx], ref memberVar.propPviValue, memberVar.propPviValue.DataType);
              memberVar.propPviValue.propTypeLength = Variable.GetDataTypeLength(typeInfo[infoIdx], memberVar.propPviValue.propTypeLength);
              memberVar.propPviValue.SetArrayLength(Variable.GetArrayLength(typeInfo[infoIdx]));
              if (memberVar.propPviValue.ArrayMinIndex == 0)
                memberVar.propPviValue.propArrayMaxIndex = memberVar.propPviValue.ArrayLength - 1;
              memberVar.propOffset = byteOffset;
              byteOffset = memberVar.propOffset + memberVar.propPviValue.propTypeLength * memberVar.propPviValue.ArrayLength;
              if (memberVar.propPviValue.DataType == DataType.Structure)
              {
                memberVar.propStructName = this.GetSNParameter(typeInfo[infoIdx]);
                variable1 = memberVar;
              }
            }
          }
          else
          {
            Variable variable3 = this.LookupInParentCollection(str, vAddress, this);
            if (variable3 == null)
            {
              variable3 = new Variable(true, variable1, str, addToVCollections);
              variable3.Address = vAddress;
              this.AddToParentCollection(variable3, this.propParent, addToVCollections);
            }
            variable3.propAlignment = this.propAlignment;
            variable3.propScope = this.propScope;
            variable3.propOwner = (Base) variable1;
            if (this.Service != null && this.Service.AddStructMembersToMembersToo)
              this.AddStructMembers(this, variable3);
            else
              this.AddStructMember(variable3.StructMemberName, variable3);
            this.GetExtendedAttributes(typeInfo[infoIdx], ref isMDimArray, ref variable3.propPviValue);
            if (isMDimArray)
              variable3.propPviValue.SetArrayIndex(typeInfo[infoIdx]);
            variable3.propPviValue.SetDataType(Variable.GetDataType(typeInfo[infoIdx], variable3.Value.IsBitString, ref variable3.propPviValue.propTypeLength));
            variable3.GetVSParameters(typeInfo[infoIdx], ref variable3.propPviValue, variable3.propPviValue.DataType);
            variable3.propPviValue.propTypeLength = Variable.GetDataTypeLength(typeInfo[infoIdx], variable3.propPviValue.propTypeLength);
            variable3.propPviValue.SetArrayLength(Variable.GetArrayLength(typeInfo[infoIdx]));
            if (variable3.propPviValue.ArrayMinIndex == 0)
              variable3.propPviValue.propArrayMaxIndex = variable3.propPviValue.ArrayLength - 1;
            variable3.propOffset = byteOffset;
            if (DataType.Structure != variable3.propPviValue.DataType)
              byteOffset = variable3.propOffset + variable3.propPviValue.propTypeLength * variable3.propPviValue.ArrayLength;
            if (variable1.propMembers == null)
              variable1.propMembers = new MemberCollection(variable1, "");
            variable1.propMembers.Add(variable3);
            if (DataType.Structure == variable3.propPviValue.DataType)
              variable3.propStructName = this.GetSNParameter(typeInfo[infoIdx]);
            if (variable3.propPviValue.IsOfTypeArray)
            {
              this.CreateStructArray(this, variable3, typeInfo, variable3.propOffset, ref byteOffset, ref infoIdx, addToVCollections);
              --infoIdx;
            }
            else if (variable3.propPviValue.DataType == DataType.Structure)
            {
              ++infoIdx;
              if (infoIdx < typeInfo.GetLength(0))
              {
                this.CreateNestedStruct(this, variable3, typeInfo, variable3.propOffset, length, ref infoIdx, addToVCollections);
                byteOffset = variable3.propOffset + variable3.propPviValue.propTypeLength * variable3.propPviValue.ArrayLength;
                --infoIdx;
              }
              else
                break;
            }
          }
        }
      }
      if (this.propUserMembers != null)
        this.propUserMembers.CopyTo(this.Members);
      typeLength = byteOffset;
      return membersExpanded;
    }

    private Variable LookupInParentCollection(string vName, string vAddress, Variable vOwner)
    {
      Variable variable = (Variable) null;
      Base parent = vOwner.Parent;
      string name = vAddress;
      if (parent is Cpu)
        variable = ((Cpu) parent).Variables[name];
      else if (this.propParent is Task)
        variable = ((Task) parent).Variables[name];
      return variable != null && variable.Name.CompareTo(vName) == 0 ? variable : (Variable) null;
    }

    internal bool ContainedInParentCollection()
    {
      Base propParent = this.propParent;
      while (true)
      {
        switch (propParent)
        {
          case Variable _:
            propParent = propParent.propParent;
            continue;
          case Cpu _:
            goto label_3;
          case Task _:
            goto label_4;
          default:
            goto label_5;
        }
      }
label_3:
      return ((Cpu) propParent).Variables.ContainsKey((object) this.AddressEx);
label_4:
      return ((Task) propParent).Variables.ContainsKey((object) this.AddressEx);
label_5:
      return false;
    }

    private void AddToParentCollection(Variable memberVar, Base parentObj, bool addToVCollections)
    {
      if (!addToVCollections)
        return;
      if (parentObj is Cpu)
      {
        if (((Cpu) parentObj).Variables.ContainsKey((object) memberVar.AddressEx))
          return;
        ((Cpu) parentObj).Variables.Add((object) memberVar.AddressEx, (object) memberVar);
      }
      else
      {
        if (!(this.propParent is Task) || ((Task) parentObj).Variables.ContainsKey((object) memberVar.AddressEx))
          return;
        ((Task) parentObj).Variables.Add((object) memberVar.AddressEx, (object) memberVar);
      }
    }

    private void CreateNestedStruct(
      Variable root,
      Variable varParent,
      string[] typeInfo,
      int offset,
      int nesting,
      ref int infoIdx,
      bool addToVCollections)
    {
      int byteOffset = offset;
      bool isMDimArray = false;
      if (infoIdx >= typeInfo.GetLength(0))
        return;
      string[] strArray = Variable.GetStructElementName(typeInfo[infoIdx]).Split('.');
      int length = strArray.Length;
      while (infoIdx < typeInfo.Length && length > nesting)
      {
        string str = strArray[length - 1];
        if (str.Length == 0)
        {
          str = this.propPviInternStructElement.ToString();
          ++this.propPviInternStructElement;
        }
        string vAddress = varParent.Address + "." + str;
        Variable variable = this.LookupInParentCollection(str, vAddress, this);
        if (variable == null)
        {
          variable = new Variable(true, varParent, str, addToVCollections);
          variable.Address = vAddress;
          this.AddToParentCollection(variable, this.propParent, addToVCollections);
        }
        variable.propAlignment = this.propAlignment;
        variable.propScope = root.propScope;
        this.GetExtendedAttributes(typeInfo[infoIdx], ref isMDimArray, ref variable.propPviValue);
        if (isMDimArray)
          variable.propPviValue.SetArrayIndex(typeInfo[infoIdx]);
        variable.propPviValue.SetDataType(Variable.GetDataType(typeInfo[infoIdx], variable.Value.IsBitString, ref variable.propPviValue.propTypeLength));
        variable.GetVSParameters(typeInfo[infoIdx], ref variable.propPviValue, variable.propPviValue.DataType);
        variable.propPviValue.propTypeLength = Variable.GetDataTypeLength(typeInfo[infoIdx], variable.propPviValue.propTypeLength);
        variable.propPviValue.SetArrayLength(Variable.GetArrayLength(typeInfo[infoIdx]));
        if (variable.propPviValue.ArrayMinIndex == 0)
          variable.propPviValue.propArrayMaxIndex = variable.propPviValue.ArrayLength - 1;
        variable.propOffset = byteOffset;
        if (DataType.Structure == variable.propPviValue.DataType)
          variable.propStructName = this.GetSNParameter(typeInfo[infoIdx]);
        if (this.Service != null && this.Service.AddStructMembersToMembersToo)
          this.AddStructMembers(varParent, variable);
        else
          root.AddStructMember(variable.GetStructMemberName(root), variable);
        if (variable.propPviValue.IsOfTypeArray)
          this.CreateStructArray(root, variable, typeInfo, variable.propOffset, ref byteOffset, ref infoIdx, addToVCollections);
        else if (variable.propPviValue.DataType == DataType.Structure)
        {
          ++infoIdx;
          this.CreateNestedStruct(root, variable, typeInfo, variable.propOffset, length, ref infoIdx, addToVCollections);
          byteOffset = variable.propOffset + variable.propPviValue.propTypeLength * variable.propPviValue.ArrayLength;
        }
        else
        {
          byteOffset = variable.propOffset + variable.propPviValue.propTypeLength * variable.propPviValue.ArrayLength;
          ++infoIdx;
        }
        if (infoIdx < typeInfo.Length)
        {
          strArray = Variable.GetStructElementName(typeInfo[infoIdx]).Split('.');
          length = strArray.Length;
        }
      }
      varParent.propPviValue.propTypeLength = byteOffset - offset;
    }

    private void CreateNestedStructClone(
      Variable root,
      Variable varParent,
      Variable varOwner,
      Variable varClone,
      ref int offset,
      int nesting,
      bool addToVCollections,
      bool addStructMembersToMembersToo)
    {
      int nesting1 = nesting;
      if (varClone.Members == null)
        return;
      varOwner.propMembers = new MemberCollection(varOwner, varOwner.Address);
      foreach (Variable varClone1 in (IEnumerable) varClone.Members.Values)
      {
        if (-1 == varClone1.Name.IndexOf('.'))
        {
          string vAddress = varClone1.Name.IndexOf('[') != 0 ? varOwner.Address + "." + varClone1.Name : varOwner.Address + varClone1.Name;
          Variable variable = this.LookupInParentCollection(varClone1.Name, vAddress, this);
          if (variable == null)
          {
            variable = new Variable(true, varOwner, varClone1.Name, addToVCollections);
            variable.Address = vAddress;
            this.AddToParentCollection(variable, varOwner.propParent, addToVCollections);
          }
          variable.propAlignment = varOwner.propAlignment;
          variable.propScope = root.propScope;
          variable.propPviValue.Clone(varClone1.propPviValue);
          variable.propStructName = varClone1.propStructName;
          variable.propOffset = offset;
          if (addStructMembersToMembersToo)
            this.AddStructMembers(varOwner, variable);
          else
            root.AddStructMember(variable.GetStructMemberName(root), variable);
          if (variable.propPviValue.IsOfTypeArray || variable.propPviValue.DataType == DataType.Structure)
          {
            ++nesting1;
            this.CreateNestedStructClone(root, varOwner, variable, varClone1, ref offset, nesting1, addToVCollections, addStructMembersToMembersToo);
          }
          else
            offset = variable.propOffset + variable.propPviValue.propTypeLength;
        }
      }
    }

    private void AddNextDimensionItem(
      string strDims,
      ArrayDimensionArray aDims,
      int numDim,
      int totalDims,
      ref ArrayList lstOfItems)
    {
      if (numDim < totalDims)
      {
        for (int startIndex = aDims[numDim].StartIndex; startIndex < aDims[numDim].EndIndex + 1; ++startIndex)
          this.AddNextDimensionItem(strDims + startIndex.ToString() + ",", aDims, numDim + 1, totalDims, ref lstOfItems);
      }
      else
      {
        string str = strDims.Substring(0, strDims.Length - 1);
        lstOfItems.Add((object) ("[" + str + "]"));
      }
    }

    private void AddDimensionIndexs(ArrayDimensionArray aDims, ref ArrayList lstOfItems)
    {
      ArrayList arrayList1 = new ArrayList();
      ArrayList arrayList2 = new ArrayList();
      for (int index = 0; index < aDims.Count; ++index)
      {
        arrayList1.Add((object) aDims[index].StartIndex);
        arrayList2.Add((object) aDims[index].EndIndex);
      }
      for (int startIndex = aDims[0].StartIndex; startIndex < aDims[0].EndIndex + 1; ++startIndex)
        this.AddNextDimensionItem(startIndex.ToString() + ",", aDims, 1, aDims.Count, ref lstOfItems);
    }

    private void CreateStructArray(
      Variable root,
      Variable parentVar,
      string[] typeInfo,
      int offset,
      ref int byteOffset,
      ref int infoIdx,
      bool addToVCollections)
    {
      int propTypeLength1 = parentVar.propPviValue.propTypeLength;
      int nesting = 0;
      if (root != parentVar)
        nesting = Variable.GetStructElementName(typeInfo[infoIdx]).Split('.').Length;
      ++infoIdx;
      if (1 < parentVar.propPviValue.propArrayLength)
      {
        ArrayList lstOfItems = new ArrayList();
        if (parentVar.propPviValue.ArrayDimensions != null)
        {
          this.AddDimensionIndexs(parentVar.propPviValue.ArrayDimensions, ref lstOfItems);
        }
        else
        {
          for (int arrayMinIndex = parentVar.propPviValue.ArrayMinIndex; arrayMinIndex <= parentVar.propPviValue.ArrayMaxIndex; ++arrayMinIndex)
            lstOfItems.Add((object) ("[" + arrayMinIndex.ToString() + "]"));
        }
        Variable structArrayElement = this.CreateStructArrayElement(root, typeInfo, ref infoIdx, offset, nesting, parentVar, addToVCollections);
        int propTypeLength2 = structArrayElement.propPviValue.propTypeLength;
        parentVar.propPviValue.propTypeLength = propTypeLength2;
        for (int arrayIdx = 1; arrayIdx < lstOfItems.Count; ++arrayIdx)
          this.CreateStructArrayElementClone(arrayIdx, structArrayElement, root, typeInfo, offset, nesting, parentVar, addToVCollections);
        if (DataType.Structure != structArrayElement.propPviValue.DataType)
          ;
      }
      else
      {
        string str = "[0]";
        string vAddress = parentVar.Address + str;
        Variable variable = this.LookupInParentCollection(str, vAddress, this);
        if (variable == null)
        {
          variable = new Variable(true, parentVar, str, addToVCollections);
          variable.Address = vAddress;
          this.AddToParentCollection(variable, this.propParent, addToVCollections);
        }
        variable.propAlignment = this.propAlignment;
        variable.propScope = root.propScope;
        variable.propPviValue.propTypeLength = parentVar.Value.TypeLength;
        variable.propPviValue.SetArrayLength(1);
        if (variable.propPviValue.ArrayMinIndex == 0)
          variable.propPviValue.propArrayMaxIndex = variable.propPviValue.ArrayLength - 1;
        variable.propOffset = offset;
        variable.propPviValue.propArryOne = false;
        variable.propPviValue.SetDataType(parentVar.Value.DataType);
        if (DataType.Structure == variable.propPviValue.DataType)
          variable.propStructName = this.GetSNParameter(typeInfo[infoIdx - 1]);
        if (this.Service != null && this.Service.AddStructMembersToMembersToo)
          this.AddStructMembers(root, variable);
        else
          root.AddStructMember(variable.GetStructMemberName(root), variable);
        if (variable.propPviValue.DataType == DataType.Structure)
          this.CreateNestedStruct(root, variable, typeInfo, variable.propOffset, nesting, ref infoIdx, addToVCollections);
        parentVar.propPviValue.propTypeLength = variable.propPviValue.propTypeLength * variable.propPviValue.propArrayLength;
      }
      if (root == parentVar)
        return;
      byteOffset = offset + parentVar.propPviValue.propTypeLength * parentVar.propPviValue.propArrayLength;
    }

    private void CreateStructArrayElementClone(
      int arrayIdx,
      Variable cloneRoot,
      Variable root,
      string[] typeInfo,
      int offset,
      int nesting,
      Variable parentVar,
      bool addToVCollections)
    {
      bool bAddToAll = false;
      int propTypeLength = cloneRoot.propPviValue.propTypeLength;
      string name = "[" + arrayIdx.ToString() + "]";
      int offset1 = offset + arrayIdx * propTypeLength;
      Variable variable = new Variable(name, parentVar, addToVCollections, offset1, this.propAlignment, root.propScope);
      if (this.Service != null && this.Service.AddStructMembersToMembersToo)
        bAddToAll = true;
      variable.CloneVariable(cloneRoot, root, parentVar, addToVCollections, bAddToAll);
    }

    private Variable CreateStructArrayElement(
      Variable root,
      string[] typeInfo,
      ref int rootInfoIdx,
      int offset,
      int nesting,
      Variable parentVar,
      bool addToVCollections)
    {
      string str = "[0]";
      string vAddress = parentVar.Address + str;
      Variable structArrayElement = this.LookupInParentCollection(str, vAddress, this);
      if (structArrayElement == null)
      {
        structArrayElement = new Variable(true, parentVar, str, addToVCollections);
        structArrayElement.Address = vAddress;
        this.AddToParentCollection(structArrayElement, this.propParent, addToVCollections);
      }
      structArrayElement.propAlignment = this.propAlignment;
      structArrayElement.propScope = root.propScope;
      structArrayElement.propPviValue.propTypeLength = parentVar.Value.TypeLength;
      structArrayElement.propPviValue.SetArrayLength(1);
      if (structArrayElement.propPviValue.ArrayMinIndex == 0)
        structArrayElement.propPviValue.propArrayMaxIndex = structArrayElement.propPviValue.ArrayLength - 1;
      structArrayElement.propOffset = offset;
      structArrayElement.propPviValue.SetDataType(parentVar.Value.DataType);
      structArrayElement.propStructName = parentVar.propStructName;
      structArrayElement.propPviValue.propDerivedFrom = parentVar.Value.DerivedFrom;
      structArrayElement.propPviValue.propEnumerations = parentVar.Value.Enumerations;
      structArrayElement.propPviValue.SetDataType(parentVar.Value.DataType);
      if (this.Service != null && this.Service.AddStructMembersToMembersToo)
        this.AddStructMembers(parentVar, structArrayElement);
      else
        root.AddStructMember(structArrayElement.GetStructMemberName(root), structArrayElement);
      if (structArrayElement.propPviValue.DataType == DataType.Structure)
        this.CreateNestedStruct(root, structArrayElement, typeInfo, structArrayElement.propOffset, nesting, ref rootInfoIdx, addToVCollections);
      return structArrayElement;
    }

    internal static string GetStructElementName(string pviText)
    {
      int num = pviText.IndexOf(" ");
      return pviText.Substring(1, num - 1);
    }

    internal static string GetStructureName(string pviText)
    {
      int startIndex = pviText.IndexOf("SN=") + 3;
      int num = pviText.IndexOf(" ", startIndex);
      return pviText.Substring(startIndex, num - startIndex);
    }

    internal string GetSNParameter(string pviText)
    {
      string snParameter = (string) null;
      int startIndex = pviText.IndexOf("SN=");
      if (-1 != startIndex)
      {
        int num = pviText.IndexOf(" ", startIndex);
        if (-1 == num)
          num = pviText.Length;
        snParameter = pviText.Substring(startIndex + 3, num - 3 - startIndex);
      }
      return snParameter;
    }

    private void GetVSParameters(string pviText, ref Value ppVal, DataType basicType)
    {
      string[] strArray1 = (string[]) null;
      int index1 = 0;
      string pviText1 = pviText;
      int length1 = pviText1.IndexOf("\0");
      if (-1 != length1)
        pviText1 = pviText1.Substring(0, length1);
      if (-1 != pviText1.IndexOf('{'))
        pviText1 = pviText1.Substring(0, pviText1.IndexOf('{'));
      int num1 = pviText1.IndexOf("VS=");
      if (-1 == num1)
        return;
      int num2 = pviText1.IndexOf("TN=");
      if (-1 != num2)
      {
        string str = pviText1.Substring(num2 + 3).Split(' ').GetValue(0).ToString();
        str.Trim();
        if (str.Length == 1 + str.LastIndexOf(','))
          str = str.Substring(0, str.Length - 1);
        strArray1 = str.Split(',');
        ppVal.propIsDerived = (sbyte) 1;
      }
      string str1 = pviText1.Substring(num1 + 3).Split(' ').GetValue(0).ToString();
      int length2 = str1.IndexOf('}');
      if (-1 != length2)
        str1 = str1.Substring(0, length2);
      string[] strArray2 = str1.Split(';');
      for (int index2 = 0; index2 < strArray2.Length; ++index2)
      {
        string str2 = strArray2.GetValue(index2).ToString();
        string[] strArray3 = str2.Split(',');
        if (1 == strArray3.Length && "v".CompareTo(strArray3.GetValue(0).ToString()) == 0)
        {
          ppVal.propIsDerived = (sbyte) 1;
          if (strArray1 != null && index1 < strArray1.Length)
          {
            ppVal.SetDerivation(new DerivationBase(strArray1.GetValue(index1).ToString(), basicType));
            ++index1;
          }
          else
            ppVal.SetDerivation(new DerivationBase("", basicType));
        }
        else if (0 < strArray3.Length)
        {
          if ("a".CompareTo(strArray3.GetValue(0).ToString()) == 0)
          {
            if (1 == strArray2.Length)
              ppVal.propArryOne = ppVal.SetArrayIndex("VS=" + str2);
          }
          else if ("v".CompareTo(strArray3.GetValue(0).ToString()) == 0)
          {
            ppVal.propIsDerived = (sbyte) 1;
            if (strArray1 != null && index1 < strArray1.Length)
            {
              ppVal.SetDerivation((DerivationBase) new Int32MinMaxDerivation(strArray1.GetValue(index1).ToString(), basicType, strArray3));
              ++index1;
            }
            else
              ppVal.SetDerivation((DerivationBase) new Int32MinMaxDerivation("", basicType, strArray3));
          }
          else if ("e".CompareTo(strArray3.GetValue(0).ToString()) == 0)
          {
            ppVal.propIsEnum = (sbyte) 1;
            if (1 < strArray3.Length)
            {
              if (ppVal.propEnumerations == null)
              {
                if (strArray1 != null && index1 < strArray1.Length)
                {
                  ppVal.propEnumerations = new EnumArray(strArray1.GetValue(index1).ToString());
                  ++index1;
                }
                else
                  ppVal.propEnumerations = new EnumArray(this.GetSNParameter(pviText1));
              }
              ppVal.propEnumerations.AddEnum((EnumBase) new Int32Enum(strArray3));
            }
          }
          else if ("b".CompareTo(strArray3.GetValue(0).ToString()) == 0)
            ppVal.propIsBitString = (sbyte) 1;
        }
      }
      if (ppVal.propDimensions == null || 1 != ppVal.propDimensions.Count)
        return;
      ppVal.propArrayMaxIndex = ppVal.propDimensions[0].EndIndex;
      ppVal.propArrayMinIndex = ppVal.propDimensions[0].StartIndex;
      ppVal.propArryOne = true;
      ppVal.propDimensions = (ArrayDimensionArray) null;
    }

    internal static int GetAlignment(string pviText) => Variable.GetAttributeValue(pviText, "AL=");

    internal static int GetOffset(
      string pviText,
      int alignment,
      int byteOffset,
      int typeLength,
      ref int internalOffset)
    {
      int offset = byteOffset;
      bool attribIsContained = false;
      internalOffset = Variable.GetAttributeValue(pviText, "VO=", ref attribIsContained);
      if (!attribIsContained)
        internalOffset = byteOffset;
      return offset;
    }

    private static int GetAttributeValue(string pviText, string attribute)
    {
      bool attribIsContained = false;
      return Variable.GetAttributeValue(pviText, attribute, ref attribIsContained);
    }

    private static int GetAttributeValue(
      string pviText,
      string attribute,
      ref bool attribIsContained)
    {
      attribIsContained = false;
      if (-1 == pviText.IndexOf(attribute))
        return 0;
      int startIndex = pviText.IndexOf(attribute) + attribute.Length;
      int num1 = pviText.IndexOf(" ", startIndex);
      int num2 = pviText.IndexOf("}", startIndex);
      if (-1 != num1 && num1 < num2)
        num2 = num1;
      if (-1 == num2)
        num2 = num1 >= pviText.Length ? pviText.Length : num1;
      attribIsContained = true;
      return System.Convert.ToInt32(pviText.Substring(startIndex, num2 - startIndex));
    }

    public void ReadValue() => this.ReadValueEx();

    public int ReadExtendedTypeInfo() => !(this.Parent is Service) ? this.ReadRequest(this.Service.hPvi, this.LinkId, AccessTypes.TypeExtern, 2811U) : this.ReadRequest(this.Service.hPvi, this.LinkId, AccessTypes.TypeIntern, 2811U);

    public int ReadValueEx()
    {
      int errorCode;
      if (this.propPviValue.DataType == DataType.Unknown)
      {
        this.Requests |= Actions.GetValue;
        errorCode = this.Read_FormatEX(this.propLinkId);
      }
      else
        errorCode = this.ReadRequest(this.Service.hPvi, this.propLinkId, AccessTypes.Data, 505U);
      if (errorCode != 0)
        this.OnError(new PviEventArgs(this.Name, this.Address, errorCode, this.Service.Language, Action.VariableValueRead, this.Service));
      return errorCode;
    }

    public int ReadValue(bool synchronous)
    {
      ArrayList changes = (ArrayList) null;
      return this.ReadValue(synchronous, ref changes);
    }

    public int ReadValue(bool synchronous, ref ArrayList changes)
    {
      int error;
      if (!synchronous)
      {
        error = 0;
        if (!this.propWaitingOnReadEvent)
        {
          this.propWaitingOnReadEvent = true;
          if (this.propPviValue.DataType == DataType.Unknown)
            this.Read_FormatEX(this.propLinkId);
          error = this.ReadRequest(this.Service.hPvi, this.propLinkId, AccessTypes.Data, 505U);
        }
      }
      else if (this.propPviValue.DataType == DataType.Unknown)
      {
        this.Read_FormatEX(this.propLinkId);
        error = 12012;
      }
      else
      {
        SyncReadData readData = new SyncReadData(this.propPviValue.DataSize);
        error = this.Read(this.Service.hPvi, this.propLinkId, AccessTypes.Data, readData);
        if (error == 0)
        {
          changes = new ArrayList(1);
          error = this.ConvertPviValue(readData.PtrData, (uint) readData.DataLength, ref changes);
          this.propChangedStructMembers = new string[changes.Count];
          for (int index = 0; index < changes.Count; ++index)
            this.propChangedStructMembers.SetValue(changes[index], index);
          this.OnDataValidated((PviEventArgs) new VariableEventArgs(this.Name, this.Address, error, this.Service.Language, Action.VariableValueRead, this.propChangedStructMembers));
        }
        readData.FreeBuffers();
      }
      return error;
    }

    internal void ReadInternalValue()
    {
      int errorCode = this.ReadRequest(this.Service.hPvi, this.propLinkId, AccessTypes.Data, 2809U);
      if (errorCode == 0)
        return;
      this.OnError(new PviEventArgs(this.Name, this.Address, errorCode, this.Service.Language, Action.VariableValueRead, this.Service));
    }

    internal Variable GetMemberByOffset(int offset)
    {
      if (offset < 0)
        return (Variable) null;
      foreach (Variable member in (BaseCollection) this.Members)
      {
        if (member.propOffset == offset && member.propPviValue.DataType != DataType.Structure)
          return member;
        if (member.propPviValue.DataType == DataType.Structure && member.propPviValue.ArrayLength * member.propPviValue.propTypeLength + member.propOffset > offset)
        {
          Variable memberByOffset;
          if (member.propPviValue.ArrayLength > 1)
          {
            int offset1 = (offset - member.propOffset) % member.propPviValue.propTypeLength;
            memberByOffset = member.GetMemberByOffset(offset1);
          }
          else
            memberByOffset = member.GetMemberByOffset(offset - member.propOffset);
          if (memberByOffset != null)
            return memberByOffset;
        }
        if (member.propPviValue.DataType != DataType.Structure)
        {
          if (offset > member.propOffset && offset < member.propOffset + member.propPviValue.TypeLength * member.propPviValue.ArrayLength)
            return member;
          if (offset < this.propOffset + this.propPviValue.propTypeLength * this.propPviValue.propArrayLength)
          {
            int num = offset % this.propPviValue.propTypeLength;
            if (num >= member.propOffset && num < member.propOffset + member.propPviValue.TypeLength * member.propPviValue.propArrayLength)
              return member;
          }
        }
      }
      return (Variable) null;
    }

    private int WriteInitialValue()
    {
      int errorCode;
      if (DataType.String == this.propPviValue.DataType)
      {
        this.ResetWriteDataPtr(this, this.propPviValue.TypeLength, true);
        if (this.propPviValue.propDataSize > this.propPviValue.TypeLength)
        {
          Marshal.Copy(this.propPviValue.pData, this.propWriteByteField, 0, this.propPviValue.TypeLength);
        }
        else
        {
          this.ResizePviDataPtr(this.propPviValue.DataSize);
          Marshal.Copy(this.propPviValue.pData, this.propWriteByteField, 0, this.propPviValue.propDataSize);
        }
        Marshal.Copy(this.propWriteByteField, 0, this.pWriteData, this.propStrDataLen);
        this.propPviValue.isAssigned = false;
        errorCode = this.WriteRequest(this.Service.hPvi, this.propLinkId, AccessTypes.Data, this.pWriteData, this.propStrDataLen, 556U);
      }
      else if (this.propPviValue.IsOfTypeArray && CastModes.PG2000String == (this.CastMode & CastModes.PG2000String))
      {
        if (!this.propPviValue.isAssigned)
        {
          this.ResetWriteDataPtr(this, this.propPviValue.DataSize, true);
          this.propWriteByteField = (byte[]) this.propPviValue.propByteField.Clone();
          for (int index = 0; index < this.StructureMembers.Count; ++index)
          {
            if (this.StructureMembers.IsVirtual)
            {
              Marshal.Copy(this.propPviValue.DataPtr, this.propWriteByteField, this.propPviValue.TypeLength * index, this.propPviValue.TypeLength);
            }
            else
            {
              Variable structureMember = (Variable) this.StructureMembers[index];
              if (0 < structureMember.propPviValue.propDataSize)
              {
                structureMember.propPviValue.isAssigned = false;
                if (1 <= structureMember.propPviValue.ArrayLength && DataType.Structure != structureMember.Value.DataType)
                  Marshal.Copy(structureMember.propPviValue.DataPtr, this.propWriteByteField, structureMember.propOffset, structureMember.Value.propDataSize);
              }
            }
          }
          Marshal.Copy(this.propWriteByteField, 0, this.pWriteData, this.propStrDataLen);
          this.propPviValue.isAssigned = false;
          errorCode = this.WriteRequest(this.Service.hPvi, this.propLinkId, AccessTypes.Data, this.pWriteData, this.propStrDataLen, 556U);
        }
        else
        {
          this.ResetWriteDataPtr(this, this.propPviValue.ArrayLength);
          if (this.propPviValue.propDataSize > this.propPviValue.ArrayLength)
            Marshal.Copy(this.propPviValue.pData, this.propWriteByteField, 0, this.propPviValue.ArrayLength);
          else
            Marshal.Copy(this.propPviValue.pData, this.propWriteByteField, 0, this.propPviValue.propDataSize);
          Marshal.Copy(this.propWriteByteField, 0, this.pWriteData, this.propStrDataLen);
          this.propPviValue.isAssigned = false;
          errorCode = this.WriteRequest(this.Service.hPvi, this.propLinkId, AccessTypes.Data, this.pWriteData, this.propStrDataLen, 556U);
        }
      }
      else if (this.propPviValue.IsOfTypeArray || DataType.Structure == this.propPviValue.DataType)
      {
        this.ResetWriteDataPtr(this, this.propPviValue.DataSize);
        this.propWriteByteField = (byte[]) this.propPviValue.propByteField.Clone();
        for (int index = 0; index < this.StructureMembers.Count; ++index)
        {
          if (this.StructureMembers.IsVirtual)
          {
            Marshal.Copy(this.propPviValue.DataPtr, this.propWriteByteField, this.propPviValue.TypeLength * index, this.propPviValue.TypeLength);
          }
          else
          {
            Variable structureMember = (Variable) this.StructureMembers[index];
            if (0 < structureMember.propPviValue.propDataSize)
            {
              structureMember.propPviValue.isAssigned = false;
              if (1 <= structureMember.propPviValue.ArrayLength && DataType.Structure != structureMember.propPviValue.DataType)
                Marshal.Copy(structureMember.propPviValue.DataPtr, this.propWriteByteField, structureMember.propOffset, structureMember.propPviValue.propDataSize);
            }
            structureMember.propPviValue.isAssigned = false;
          }
        }
        Marshal.Copy(this.propWriteByteField, 0, this.pWriteData, this.propStrDataLen);
        this.propPviValue.isAssigned = false;
        errorCode = this.WriteRequest(this.Service.hPvi, this.propLinkId, AccessTypes.Data, this.pWriteData, this.propStrDataLen, 556U);
      }
      else
        errorCode = this.WriteRequest(this.Service.hPvi, this.propLinkId, AccessTypes.Data, this.propPviValue.pData, this.propPviValue.propDataSize, 556U);
      PviMarshal.FreeHGlobal(ref this.propPviValue.pData);
      if (this.propInternalByteField != null)
        this.propPviValue.propByteField = this.propInternalByteField;
      if (errorCode != 0)
        this.OnError(new PviEventArgs(this.propName, this.propAddress, errorCode, this.Service.Language, Action.VariableValueWrite, this.Service));
      return errorCode;
    }

    private int WriteValueFromPtr(bool sync)
    {
      this.ResetWriteDataPtr(this, this.propPviValue.DataSize);
      this.propInternalByteField = (byte[]) this.propPviValue.propByteField.Clone();
      this.propPviValue.isAssigned = false;
      return !sync ? this.WriteRequest(this.Service.hPvi, this.propLinkId, AccessTypes.Data, this.propPviValue.DataPtr, this.propPviValue.DataSize, 506U) : this.Write(this.Service.hPvi, this.LinkId, AccessTypes.Data, this.propPviValue.DataPtr, this.propPviValue.DataSize);
    }

    internal void UpdateAssignedData(IntPtr pD, int byteOffset, int dSize)
    {
      if (this.propPviValue.propByteField == null)
        this.propPviValue.propByteField = new byte[this.propPviValue.DataSize];
      if (IntPtr.Zero == this.propPviValue.pData)
      {
        this.propPviValue.pData = PviMarshal.AllocHGlobal(this.propPviValue.propByteField.Length);
        this.propPviValue.propHasOwnDataPtr = true;
      }
      Marshal.Copy(this.propPviValue.pData, this.propPviValue.propByteField, byteOffset, dSize);
    }

    internal int WriteValue(Array values, int offset)
    {
      if (Access.Read == this.Access)
        return 12034;
      this.ResetWriteDataPtr(this.PVRoot, this.PVRoot.propPviValue.DataSize);
      this.PVRoot.propInternalByteField = (byte[]) this.PVRoot.propPviValue.propByteField.Clone();
      Marshal.Copy(this.PVRoot.propInternalByteField, 0, this.PVRoot.pWriteData, this.PVRoot.propStrDataLen);
      if (values.Length == 0)
        return -2;
      switch (values.GetValue(0))
      {
        case float _:
          if (0 < offset)
          {
            int num = values.Length * 4;
            IntPtr hMemory = PviMarshal.AllocHGlobal(num);
            this.PVRoot.propWriteByteField = (byte[]) this.PVRoot.propPviValue.propByteField.Clone();
            Marshal.Copy((float[]) values, 0, hMemory, values.Length);
            Marshal.Copy(hMemory, this.PVRoot.propWriteByteField, offset, num);
            Marshal.Copy(this.PVRoot.propWriteByteField, 0, this.PVRoot.pWriteData, this.PVRoot.propStrDataLen);
            PviMarshal.FreeHGlobal(ref hMemory);
            break;
          }
          Marshal.Copy((float[]) values, 0, this.pWriteData, values.Length);
          break;
        case double _:
          if (0 < offset)
          {
            int num = values.Length * 8;
            IntPtr hMemory = PviMarshal.AllocHGlobal(num);
            this.PVRoot.propWriteByteField = (byte[]) this.PVRoot.propPviValue.propByteField.Clone();
            Marshal.Copy((double[]) values, 0, hMemory, values.Length);
            Marshal.Copy(hMemory, this.PVRoot.propWriteByteField, offset, num);
            Marshal.Copy(this.PVRoot.propWriteByteField, 0, this.PVRoot.pWriteData, this.PVRoot.propStrDataLen);
            PviMarshal.FreeHGlobal(ref hMemory);
            break;
          }
          Marshal.Copy((double[]) values, 0, this.PVRoot.pWriteData, values.Length);
          break;
        case int _:
          for (int index = 0; index < values.Length; ++index)
            Marshal.WriteInt32(this.PVRoot.pWriteData, offset + 4 * index, (int) values.GetValue(index));
          break;
        case short _:
          for (int index = 0; index < values.Length; ++index)
            Marshal.WriteInt16(this.PVRoot.pWriteData, offset + 2 * index, (short) values.GetValue(index));
          break;
        case byte _:
          for (int index = 0; index < values.Length; ++index)
            Marshal.WriteByte(this.PVRoot.pWriteData, offset + index, (byte) values.GetValue(index));
          break;
        case char _:
          for (int index = 0; index < values.Length; ++index)
            Marshal.WriteByte(this.PVRoot.pWriteData, offset + index, PviMarshal.Convert.ToByte((char) values.GetValue(index)));
          break;
        case sbyte _:
          for (int index = 0; index < values.Length; ++index)
            Marshal.WriteByte(this.PVRoot.pWriteData, offset + index, PviMarshal.Convert.ToByte((sbyte) values.GetValue(index)));
          break;
        case ushort _:
          for (int index = 0; index < values.Length; ++index)
            PviMarshal.WriteUInt16(this.PVRoot.pWriteData, offset + 2 * index, (ushort) values.GetValue(index));
          break;
        case short _:
          for (int index = 0; index < values.Length; ++index)
            Marshal.WriteInt16(this.PVRoot.pWriteData, offset + 2 * index, (short) values.GetValue(index));
          break;
        case int _:
          for (int index = 0; index < values.Length; ++index)
            Marshal.WriteInt32(this.PVRoot.pWriteData, offset + 4 * index, (int) values.GetValue(index));
          break;
        case uint _:
          for (int index = 0; index < values.Length; ++index)
            PviMarshal.WriteUInt32(this.PVRoot.pWriteData, offset + 4 * index, (uint) values.GetValue(index));
          break;
        case long _:
          for (int index = 0; index < values.Length; ++index)
            PviMarshal.WriteInt64(this.PVRoot.pWriteData, offset + 8 * index, (long) values.GetValue(index));
          break;
        case ulong _:
          for (int index = 0; index < values.Length; ++index)
            PviMarshal.WriteInt64(this.PVRoot.pWriteData, offset + 8 * index, (long) values.GetValue(index));
          break;
        case DateTime _:
          for (int index = 0; index < values.Length; ++index)
            PviMarshal.WriteUInt32(this.PVRoot.pWriteData, offset + 4 * index, Pvi.DateTimeToUInt32((DateTime) values.GetValue(index)));
          break;
        case TimeSpan _:
          for (int index = 0; index < values.Length; ++index)
            Marshal.WriteInt32(this.PVRoot.pWriteData, offset + 4 * index, (int) (((TimeSpan) values.GetValue(index)).Ticks / 10000L));
          break;
        case bool _:
          for (int index = 0; index < values.Length; ++index)
          {
            byte val = 1;
            if (!(bool) values.GetValue(index))
              val = (byte) 0;
            Marshal.WriteByte(this.PVRoot.pWriteData, offset + index, val);
          }
          break;
        case string _:
          for (int index1 = 0; index1 < values.Length; ++index1)
          {
            string str = (string) values.GetValue(index1);
            char[] chArray = (char[]) null;
            chArray = new char[str.Length];
            char[] charArray = str.ToCharArray(0, str.Length);
            for (int index2 = 0; index2 < charArray.Length; ++index2)
              Marshal.WriteByte(this.PVRoot.pWriteData, offset + this.propPviValue.propTypeLength * index1 + index2, (byte) charArray[index2]);
            for (int length = charArray.Length; length < this.propPviValue.propTypeLength; ++length)
              Marshal.WriteByte(this.PVRoot.pWriteData, offset + this.propPviValue.propTypeLength * index1 + length, (byte) 0);
          }
          break;
      }
      this.propPviValue.isAssigned = false;
      if (!this.PVRoot.propActive)
        Marshal.Copy(this.PVRoot.pWriteData, this.PVRoot.propWriteByteField, 0, this.PVRoot.propStrDataLen);
      return this.WriteRequest(this.Service.hPvi, this.PVRoot.LinkId, AccessTypes.Data, this.PVRoot.pWriteData, this.PVRoot.propStrDataLen, 506U);
    }

    internal int WriteValue(Array values) => this.WriteValue(values, 0);

    public int WriteValue() => this.SendWriteValue(false);

    private int ValueCBWriteRequest(IntPtr pData, int dataLen, uint msgID, bool sync)
    {
      if (this.propLinkId == 0U)
        return 2;
      return sync ? this.Write(this.Service.hPvi, this.LinkId, AccessTypes.Data, pData, dataLen) : this.WriteRequest(this.Service.hPvi, this.propLinkId, AccessTypes.Data, pData, dataLen, 506U);
    }

    private int SendWriteValue(bool sync)
    {
      int errorCode;
      if (Access.Read == this.Access)
      {
        errorCode = 12034;
      }
      else
      {
        if (this.propPviValue.DataType == DataType.Unknown)
          return 12036;
        if (this.propForceValue)
          errorCode = this.WriteValueForced(true);
        else if (!this.propExpandMembers || DataType.Structure != this.propPviValue.DataType && this.propPviValue.IsOfTypeArray)
          errorCode = this.WriteValueFromPtr(sync);
        else if (DataType.String == this.propPviValue.DataType)
        {
          this.ResetWriteDataPtr(this, this.propPviValue.TypeLength, true);
          if (this.propPviValue.propDataSize > this.propStrDataLen)
          {
            Marshal.Copy(this.propPviValue.pData, this.propWriteByteField, 0, this.propPviValue.TypeLength);
          }
          else
          {
            this.ResizePviDataPtr(this.propPviValue.DataSize);
            Marshal.Copy(this.propPviValue.pData, this.propWriteByteField, 0, this.propPviValue.propDataSize);
          }
          Marshal.Copy(this.propWriteByteField, 0, this.pWriteData, this.propStrDataLen);
          this.propPviValue.isAssigned = false;
          errorCode = this.ValueCBWriteRequest(this.pWriteData, this.propStrDataLen, this.propInternID, sync);
        }
        else if (this.propPviValue.IsOfTypeArray && CastModes.PG2000String == (this.CastMode & CastModes.PG2000String))
        {
          if (!this.propPviValue.isAssigned)
          {
            this.ResetWriteDataPtr(this, this.propPviValue.DataSize, true);
            this.propInternalByteField = (byte[]) this.propPviValue.propByteField.Clone();
            for (int index = 0; index < this.StructureMembers.Count; ++index)
            {
              if (this.StructureMembers.IsVirtual)
              {
                Marshal.Copy(this.propPviValue.DataPtr, this.propWriteByteField, this.propPviValue.TypeLength * index, this.propPviValue.TypeLength);
              }
              else
              {
                Variable structureMember = (Variable) this.StructureMembers[index];
                if (0 < structureMember.Value.propDataSize)
                {
                  structureMember.Value.isAssigned = false;
                  if (1 <= structureMember.Value.ArrayLength && DataType.Structure != structureMember.Value.DataType)
                  {
                    Marshal.Copy(structureMember.Value.DataPtr, this.propWriteByteField, structureMember.propOffset, structureMember.Value.propDataSize);
                    structureMember.Value.propDataSize = 0;
                  }
                }
                structureMember.Value.isAssigned = false;
              }
            }
            Marshal.Copy(this.propWriteByteField, 0, this.pWriteData, this.propStrDataLen);
            this.propPviValue.isAssigned = false;
            errorCode = this.ValueCBWriteRequest(this.pWriteData, this.propStrDataLen, this.propInternID, sync);
          }
          else
          {
            this.ResetWriteDataPtr(this, this.propPviValue.ArrayLength);
            if (this.propPviValue.propDataSize > this.propStrDataLen)
              Marshal.Copy(this.propPviValue.pData, this.propWriteByteField, 0, this.propStrDataLen);
            else
              Marshal.Copy(this.propPviValue.pData, this.propWriteByteField, 0, this.propPviValue.propDataSize);
            Marshal.Copy(this.propWriteByteField, 0, this.pWriteData, this.propStrDataLen);
            this.propPviValue.isAssigned = false;
            errorCode = this.ValueCBWriteRequest(this.pWriteData, this.propStrDataLen, this.propInternID, sync);
          }
        }
        else if (this.propPviValue.IsOfTypeArray || DataType.Structure == this.propPviValue.DataType)
        {
          this.ResetWriteDataPtr(this, this.propPviValue.DataSize);
          this.propInternalByteField = (byte[]) this.propPviValue.propByteField.Clone();
          this.propWriteByteField = (byte[]) this.propPviValue.propByteField.Clone();
          for (int index = 0; index < this.StructureMembers.Count; ++index)
          {
            if (this.StructureMembers.IsVirtual)
            {
              Marshal.Copy(this.propPviValue.DataPtr, this.propWriteByteField, this.propPviValue.TypeLength * index, this.propPviValue.TypeLength);
            }
            else
            {
              Variable structureMember = (Variable) this.StructureMembers[index];
              if ((structureMember.Value.isAssigned && (0 < structureMember.Value.propDataSize || 1 < structureMember.Value.ArrayLength) || Access.Write == this.Access && 0 < structureMember.Value.propDataSize) && DataType.Structure != structureMember.Value.DataType && 0 < structureMember.Value.DataSize)
              {
                if (IntPtr.Zero != structureMember.Value.DataPtr)
                  Marshal.Copy(structureMember.Value.DataPtr, this.propWriteByteField, structureMember.propOffset, structureMember.Value.DataSize);
                else if (IntPtr.Zero != this.Value.DataPtr)
                {
                  Marshal.Copy(this.Value.DataPtr, this.propWriteByteField, structureMember.propOffset, structureMember.Value.DataSize);
                }
                else
                {
                  for (int propOffset = structureMember.propOffset; propOffset < structureMember.propOffset + structureMember.Value.DataSize && propOffset < this.propWriteByteField.GetLength(0); ++propOffset)
                    this.propWriteByteField[propOffset] = (byte) 0;
                }
                if (Access.Write != this.Access)
                  structureMember.Value.propDataSize = 0;
              }
              structureMember.Value.isAssigned = false;
            }
          }
          Marshal.Copy(this.propWriteByteField, 0, this.pWriteData, this.propStrDataLen);
          this.propPviValue.isAssigned = false;
          errorCode = this.ValueCBWriteRequest(this.pWriteData, this.propStrDataLen, this.propInternID, sync);
        }
        else
          errorCode = this.ValueCBWriteRequest(this.propPviValue.pData, this.propPviValue.propDataSize, this.propInternID, sync);
      }
      if (errorCode > 0)
        this.OnError(new PviEventArgs(this.propName, this.propAddress, errorCode, this.Service.Language, Action.VariableValueWrite, this.Service));
      return errorCode;
    }

    public int WriteValue(bool synchronous) => Access.Read != this.Access ? this.SendWriteValue(synchronous) : 12034;

    internal int WriteValueForced(bool force)
    {
      int num;
      if (force)
      {
        num = this.WriteRequest(this.Service.hPvi, this.propLinkId, AccessTypes.ForceOn, this.propPviValue.pData, this.propPviValue.propDataSize, 508U);
        if (this.propInternalByteField != null)
          this.propPviValue.propByteField = this.propInternalByteField;
      }
      else
      {
        num = this.WriteRequest(this.Service.hPvi, this.propLinkId, AccessTypes.ForceOff, IntPtr.Zero, 0, 508U);
        if (this.propInternalByteField != null)
          this.propPviValue.propByteField = this.propInternalByteField;
      }
      return num;
    }

    internal unsafe int SetArrayIndexValue(int index, Value value)
    {
      this.ResizePviDataPtr(this.propPviValue.propArrayLength * this.propPviValue.propTypeLength);
      Marshal.Copy(this.propPviValue.propByteField, 0, this.propPviValue.pData, this.propPviValue.propDataSize);
      byte* numPtr = (byte*) ((IntPtr) this.propPviValue.pData.ToPointer() + (IntPtr) index * this.propPviValue.propTypeLength);
      switch (this.propPviValue.propDataType)
      {
        case DataType.Boolean:
          *numPtr = (byte) (bool) value;
          break;
        case DataType.SByte:
          *numPtr = (byte) (sbyte) value;
          break;
        case DataType.Int16:
          *(short*) numPtr = (short) value;
          break;
        case DataType.Int32:
          *(int*) numPtr = (int) value;
          break;
        case DataType.Int64:
          *(long*) numPtr = (long) value;
          break;
        case DataType.Byte:
          *numPtr = (byte) value;
          break;
        case DataType.UInt16:
          *(short*) numPtr = (short) (ushort) value;
          break;
        case DataType.UInt32:
          *(int*) numPtr = (int) (uint) value;
          break;
        case DataType.UInt64:
          *(long*) numPtr = (long) (ulong) value;
          break;
        case DataType.Single:
          *(float*) numPtr = (float) value;
          break;
        case DataType.Double:
          *(double*) numPtr = (double) value;
          break;
        case DataType.TimeSpan:
          *(int*) numPtr = (int) (uint) value;
          break;
        case DataType.DateTime:
          *(int*) numPtr = (int) (uint) value;
          break;
        case DataType.String:
          string str = value.ToString();
          for (int index1 = 0; index1 < this.propPviValue.propTypeLength - 1; ++index1)
            numPtr[index1] = str.Length <= index1 ? (byte) 0 : System.Convert.ToByte(str[index1]);
          *(numPtr + this.propPviValue.propTypeLength - 1) = (byte) 0;
          break;
        case DataType.TimeOfDay:
          *(int*) numPtr = (int) (uint) value;
          break;
        case DataType.Date:
          *(int*) numPtr = (int) (uint) value;
          break;
        case DataType.WORD:
          *(short*) numPtr = (short) (ushort) value;
          break;
        case DataType.DWORD:
          *(int*) numPtr = (int) (uint) value;
          break;
        case DataType.UInt8:
          *numPtr = (byte) value;
          break;
        case DataType.TOD:
          *(int*) numPtr = (int) (uint) value;
          break;
        case DataType.DT:
          *(int*) numPtr = (int) (uint) value;
          break;
        default:
          return 0;
      }
      return this.WriteValueAutomatic ? this.WriteValue() : 0;
    }

    internal void InternalSetValue(IntPtr pData, uint dataLen, int offset) => this.InternalSetValue(pData, dataLen, offset, this.propPviValue.propTypeLength);

    internal void InternalSetValue(IntPtr pData, uint dataLen, int offset, int len)
    {
      if ((long) offset >= (long) dataLen)
        return;
      switch (this.propPviValue.propDataType)
      {
        case DataType.Boolean:
          byte num = Marshal.ReadByte(pData, offset);
          this.propPviValue.propObjValue = (object) true;
          if (num != (byte) 0)
            break;
          this.propPviValue.propObjValue = (object) false;
          break;
        case DataType.SByte:
          this.propPviValue.propObjValue = (object) (sbyte) Marshal.ReadByte(pData, offset);
          break;
        case DataType.Int16:
          this.propPviValue.propObjValue = (object) Marshal.ReadInt16(pData, offset);
          break;
        case DataType.Int32:
          this.propPviValue.propObjValue = (object) Marshal.ReadInt32(pData, offset);
          break;
        case DataType.Int64:
          this.propPviValue.propObjValue = (object) PviMarshal.ReadInt64(pData, offset);
          break;
        case DataType.Byte:
        case DataType.UInt8:
          this.propPviValue.propObjValue = (object) Marshal.ReadByte(pData, offset);
          break;
        case DataType.UInt16:
        case DataType.WORD:
          this.propPviValue.propObjValue = (object) (ushort) Marshal.ReadInt16(pData, offset);
          break;
        case DataType.UInt32:
        case DataType.DWORD:
          this.propPviValue.propObjValue = (object) (uint) Marshal.ReadInt32(pData, offset);
          break;
        case DataType.UInt64:
          this.propPviValue.propObjValue = (object) (ulong) PviMarshal.ReadInt64(pData, offset);
          break;
        case DataType.Single:
          this.ResetReadDataPtr(4);
          for (int index = 0; index < 4; ++index)
            this.propReadByteField[index] = Marshal.ReadByte(pData, offset + index);
          Marshal.Copy(this.propReadByteField, 0, this.pReadData, this.propStrDataLen);
          float[] destination1 = new float[1];
          Marshal.Copy(this.pReadData, destination1, 0, 1);
          this.propPviValue.propObjValue = (object) destination1[0];
          break;
        case DataType.Double:
          this.ResetReadDataPtr(8);
          for (int index = 0; index < 8; ++index)
            this.propReadByteField[index] = Marshal.ReadByte(pData, offset + index);
          Marshal.Copy(this.propReadByteField, 0, this.pReadData, this.propStrDataLen);
          double[] destination2 = new double[1];
          Marshal.Copy(this.pReadData, destination2, 0, 1);
          this.propPviValue.propObjValue = (object) destination2[0];
          break;
        case DataType.TimeSpan:
        case DataType.TimeOfDay:
        case DataType.TOD:
          this.propPviValue.propObjValue = (object) new TimeSpan((long) (uint) Marshal.ReadInt32(pData, offset) * 10000L);
          break;
        case DataType.DateTime:
        case DataType.Date:
        case DataType.DT:
          DateTime dateTime = new DateTime(55L);
          this.propPviValue.propObjValue = (object) Pvi.UInt32ToDateTime((uint) Marshal.ReadInt32(pData, offset));
          break;
        case DataType.String:
          this.ResetReadDataPtr(len);
          for (int index = 0; index < len && (long) dataLen >= (long) (offset + index); ++index)
            this.propReadByteField[index] = Marshal.ReadByte(pData, offset + index);
          this.propPviValue.propObjValue = (object) PviMarshal.ToAnsiString(this.propReadByteField);
          break;
        case DataType.WString:
          this.ResetReadDataPtr(len);
          for (int index = 0; index < len && (long) dataLen >= (long) (offset + index); ++index)
            this.propReadByteField[index] = Marshal.ReadByte(pData, offset + index);
          this.propPviValue.propObjValue = (object) PviMarshal.ToWString(this.propReadByteField, 0, len);
          break;
      }
    }

    private string GetOldStyleValueName(string varName)
    {
      int startIndex = varName.IndexOf('[', 1);
      int num = varName.IndexOf('.', 0);
      string oldStyleValueName = varName;
      if (-1 != startIndex)
      {
        oldStyleValueName = varName.Substring(startIndex);
        if (-1 != num && startIndex > num)
          oldStyleValueName = varName.Substring(num + 1);
      }
      else if (num == 0)
        oldStyleValueName = varName.Substring(num + 1);
      return oldStyleValueName;
    }

    private Value GetValueUseOldStyleName(string varName)
    {
      string oldStyleValueName = this.GetOldStyleValueName(varName);
      if (this.mapNameToMember != null && this.mapNameToMember.ContainsKey((object) oldStyleValueName))
        return this.mapNameToMember[oldStyleValueName].Value;
      if (!this.propExpandMembers)
      {
        if (-1 == oldStyleValueName.IndexOf('['))
        {
          if (-1 == oldStyleValueName.IndexOf(','))
            return this.propPviValue[System.Convert.ToInt32(oldStyleValueName)];
          string[] strArray = oldStyleValueName.Substring(0, oldStyleValueName.Length - 1).Split(',');
          if (1 >= strArray.Length)
            return (Value) null;
          int[] numArray = new int[strArray.Length];
          for (int index = 0; index < strArray.Length; ++index)
            numArray.SetValue((object) System.Convert.ToInt32(strArray.GetValue(index).ToString()), index);
          return this.propPviValue[numArray];
        }
        if (oldStyleValueName.IndexOf('[') != 0)
          return new Value(-1);
        string[] strArray1 = oldStyleValueName.Substring(1, oldStyleValueName.IndexOf(']') - 1).Split(',');
        if (1 >= strArray1.Length)
          return this.propPviValue[System.Convert.ToInt32(oldStyleValueName.Substring(1, oldStyleValueName.IndexOf(']') - 1))];
        int[] numArray1 = new int[strArray1.Length];
        for (int index = 0; index < strArray1.Length; ++index)
          numArray1.SetValue((object) System.Convert.ToInt32(strArray1.GetValue(index).ToString()), index);
        return this.propPviValue[numArray1];
      }
      if (-1 == oldStyleValueName.IndexOf('['))
        return this.propPviValue[System.Convert.ToInt32(oldStyleValueName)];
      if (oldStyleValueName.IndexOf('[') != 0)
        return new Value(-1);
      string[] strArray2 = oldStyleValueName.Substring(1, oldStyleValueName.Length - 2).Split(',');
      if (1 >= strArray2.Length)
        return this.propPviValue[System.Convert.ToInt32(oldStyleValueName.Substring(1, oldStyleValueName.Length - 2))];
      int[] indices = new int[strArray2.Length];
      for (int index = 0; index < strArray2.Length; ++index)
        indices.SetValue((object) System.Convert.ToInt32(strArray2.GetValue(index).ToString()), index);
      if (this.Members != null)
      {
        int index = Variable.CalculateIndex(indices, this.propPviValue);
        if (this.Members.ContainsKey((object) ("[" + index.ToString() + "]")))
          return this.Members["[" + index.ToString() + "]"].Value;
      }
      return this.propPviValue[indices];
    }

    internal Value GetStructureMemberValue(int index) => this.GetStructureMemberValue("[" + index.ToString() + "]");

    private int GetFlatIndex(
      string varName,
      ArrayDimensionArray aDims,
      ref string preVarName,
      ref string remVarName)
    {
      int flatIndex = 0;
      bool flag = false;
      int num1 = 0;
      preVarName = "";
      remVarName = "";
      string[] strArray1 = varName.Split(']');
      for (int index1 = 0; index1 < strArray1.Length; ++index1)
      {
        string str1 = strArray1.GetValue(index1).ToString();
        if (flag)
          remVarName += str1;
        else if (-1 == str1.IndexOf(','))
        {
          ref string local = ref preVarName;
          local = local + str1 + "]";
        }
        else
        {
          flag = true;
          string[] strArray2 = str1.Split(',');
          int[] numArray = new int[strArray2.Length];
          for (int index2 = 0; index2 < strArray2.Length; ++index2)
          {
            string str2 = strArray2.GetValue(index2).ToString().Replace('[', ' ').Replace(',', ' ').Trim();
            numArray[index2] = System.Convert.ToInt32(str2);
          }
          int index3;
          for (index3 = 0; index3 < numArray.Length - 1; ++index3)
          {
            int num2 = (int) numArray.GetValue(index3);
            if (index3 < aDims.Count)
            {
              num1 = 1;
              for (int index4 = index3 + 1; index4 < aDims.Count; ++index4)
                num1 *= aDims[index4].NumOfElements;
            }
            flatIndex += (num2 - aDims[index3].StartIndex) * num1;
          }
          int num3 = (int) numArray.GetValue(index3);
          flatIndex += num3;
          if (index3 < aDims.Count)
            flatIndex -= aDims[index3].StartIndex;
        }
      }
      return flatIndex;
    }

    internal Value GetStructureMemberValue(string varName)
    {
      string remVarName = "";
      string preVarName = "";
      Variable variable1 = this;
      if (this.mapNameToMember != null)
      {
        if (this.mapNameToMember.ContainsKey((object) varName))
          return this.mapNameToMember[varName].Value;
        if (this.propPviValue.ArrayDimensions != null && -1 != varName.IndexOf(','))
        {
          int flatIndex = this.GetFlatIndex(varName, this.propPviValue.ArrayDimensions, ref preVarName, ref remVarName);
          string name1 = preVarName + "[" + flatIndex.ToString() + "]" + remVarName;
          Variable variable2;
          if (-1 != name1.IndexOf(','))
          {
            preVarName = name1.Substring(0, 1 + name1.IndexOf(']', name1.IndexOf(',')));
            string name2 = preVarName.Substring(0, preVarName.LastIndexOf('['));
            variable2 = this.mapNameToMember[name2];
            if (variable2 != null)
            {
              string varName1 = name1.Substring(name2.Length);
              return variable2.GetStructureMemberValue(varName1);
            }
          }
          else
            variable2 = this.mapNameToMember[name1];
          return variable2?.Value;
        }
        int num1 = varName.LastIndexOf(']');
        if (variable1.Value.IsOfTypeArray)
          return this.GetValueUseOldStyleName(varName);
        if (num1 + 1 == varName.Length)
        {
          int num2 = varName.LastIndexOf('[');
          string str = varName.Substring(0, num2);
          if (this.mapNameToMember.ContainsKey((object) str))
            return this.mapNameToMember[str].GetStructureMemberValue(varName.Substring(num2));
        }
        return (Value) null;
      }
      if (this.Members != null)
        variable1 = this.Members[varName];
      if (variable1 == null)
        variable1 = this;
      return !variable1.Value.IsOfTypeArray ? variable1.Value : this.GetValueUseOldStyleName(varName);
    }

    private void ValidateDataPtr(int size)
    {
      if (!(IntPtr.Zero == this.propPviValue.pData))
        return;
      this.propPviValue.pData = PviMarshal.AllocHGlobal(size);
      this.propPviValue.propHasOwnDataPtr = true;
      this.propPviValue.propDataSize = size;
    }

    internal int setValue(Variable owner, Value newValue)
    {
      int num = -1;
      int size = 0;
      switch (this.propPviValue.DataType)
      {
        case DataType.Boolean:
          size = Marshal.SizeOf(typeof (byte));
          this.ValidateDataPtr(size);
          if ((Value) false == newValue || newValue.ToInt32((IFormatProvider) null) == 0)
          {
            if (IntPtr.Zero == this.propPviValue.pData)
            {
              Marshal.WriteByte(this.Value.pData, this.propOffset, (byte) 0);
              this.Value.isAssigned = true;
              break;
            }
            Marshal.WriteByte(this.propPviValue.pData, (byte) 0);
            this.propPviValue.isAssigned = true;
            break;
          }
          if (IntPtr.Zero == this.propPviValue.pData)
          {
            Marshal.WriteByte(this.Value.pData, this.propOffset, (byte) 1);
            this.Value.isAssigned = true;
            break;
          }
          Marshal.WriteByte(this.propPviValue.pData, (byte) 1);
          this.propPviValue.isAssigned = true;
          break;
        case DataType.SByte:
          size = Marshal.SizeOf(typeof (sbyte));
          this.ValidateDataPtr(size);
          if (IntPtr.Zero == this.propPviValue.pData)
          {
            if ((sbyte) 1 == this.Value.IsEnum && DataType.String == newValue.DataType)
            {
              if (this.Value.propEnumerations.Names.Contains((object) newValue.ToString()))
              {
                Marshal.WriteByte(this.Value.pData, this.propOffset, (byte) System.Convert.ToSByte(this.Value.propEnumerations.EnumValue(newValue.ToString()).ToString()));
                this.Value.isAssigned = true;
              }
            }
            else
            {
              Marshal.WriteByte(this.Value.pData, this.propOffset, (byte) newValue);
              this.Value.isAssigned = true;
            }
          }
          else if ((sbyte) 1 == this.propPviValue.IsEnum && DataType.String == newValue.DataType)
          {
            if (this.propPviValue.propEnumerations.Names.Contains((object) newValue.ToString()))
            {
              Marshal.WriteByte(this.propPviValue.pData, (byte) System.Convert.ToSByte(this.propPviValue.propEnumerations.EnumValue(newValue.ToString()).ToString()));
              this.propPviValue.isAssigned = true;
            }
          }
          else
          {
            Marshal.WriteByte(this.propPviValue.pData, (byte) newValue);
            this.propPviValue.isAssigned = true;
          }
          if (IntPtr.Zero == this.propPviValue.pData)
          {
            Marshal.WriteByte(this.Value.pData, this.propOffset, (byte) (sbyte) newValue);
            this.Value.isAssigned = true;
            break;
          }
          Marshal.WriteByte(this.propPviValue.pData, (byte) (sbyte) newValue);
          this.propPviValue.isAssigned = true;
          break;
        case DataType.Int16:
          size = Marshal.SizeOf(typeof (short));
          this.ValidateDataPtr(size);
          if (IntPtr.Zero == this.propPviValue.pData)
          {
            if ((sbyte) 1 == this.Value.IsEnum && DataType.String == newValue.DataType)
            {
              if (this.Value.propEnumerations.Names.Contains((object) newValue.ToString()))
              {
                Marshal.WriteInt16(this.Value.pData, this.propOffset, System.Convert.ToInt16(this.Value.propEnumerations.EnumValue(newValue.ToString()).ToString()));
                this.Value.isAssigned = true;
                break;
              }
              break;
            }
            Marshal.WriteInt16(this.Value.pData, this.propOffset, (short) newValue);
            this.Value.isAssigned = true;
            break;
          }
          if ((sbyte) 1 == this.propPviValue.IsEnum && DataType.String == newValue.DataType)
          {
            if (this.propPviValue.propEnumerations.Names.Contains((object) newValue.ToString()))
            {
              Marshal.WriteInt16(this.propPviValue.pData, System.Convert.ToInt16(this.propPviValue.propEnumerations.EnumValue(newValue.ToString()).ToString()));
              this.propPviValue.isAssigned = true;
              break;
            }
            break;
          }
          Marshal.WriteInt16(this.propPviValue.pData, (short) newValue);
          this.propPviValue.isAssigned = true;
          break;
        case DataType.Int32:
          size = Marshal.SizeOf(typeof (int));
          this.ValidateDataPtr(size);
          if (IntPtr.Zero == this.propPviValue.pData)
          {
            if ((sbyte) 1 == this.Value.IsEnum && DataType.String == newValue.DataType)
            {
              if (this.Value.propEnumerations.Names.Contains((object) newValue.ToString()))
              {
                Marshal.WriteInt32(this.Value.pData, this.propOffset, System.Convert.ToInt32(this.Value.propEnumerations.EnumValue(newValue.ToString()).ToString()));
                this.Value.isAssigned = true;
                break;
              }
              break;
            }
            Marshal.WriteInt32(this.Value.pData, this.propOffset, (int) newValue);
            this.Value.isAssigned = true;
            break;
          }
          if ((sbyte) 1 == this.propPviValue.IsEnum && DataType.String == newValue.DataType)
          {
            if (this.propPviValue.propEnumerations.Names.Contains((object) newValue.ToString()))
            {
              Marshal.WriteInt32(this.propPviValue.pData, System.Convert.ToInt32(this.propPviValue.propEnumerations.EnumValue(newValue.ToString()).ToString()));
              this.propPviValue.isAssigned = true;
              break;
            }
            break;
          }
          Marshal.WriteInt32(this.propPviValue.pData, (int) newValue);
          this.propPviValue.isAssigned = true;
          break;
        case DataType.Int64:
          size = Marshal.SizeOf(typeof (long));
          this.ValidateDataPtr(size);
          if (IntPtr.Zero == this.propPviValue.pData)
          {
            if ((sbyte) 1 == this.Value.IsEnum && DataType.String == newValue.DataType)
            {
              if (this.Value.propEnumerations.Names.Contains((object) newValue.ToString()))
              {
                PviMarshal.WriteInt64(this.Value.pData, this.propOffset, System.Convert.ToInt64(this.Value.propEnumerations.EnumValue(newValue.ToString()).ToString()));
                this.Value.isAssigned = true;
                break;
              }
              break;
            }
            PviMarshal.WriteInt64(this.Value.pData, this.propOffset, (long) newValue);
            this.Value.isAssigned = true;
            break;
          }
          if ((sbyte) 1 == this.propPviValue.IsEnum && DataType.String == newValue.DataType)
          {
            if (this.propPviValue.propEnumerations.Names.Contains((object) newValue.ToString()))
            {
              PviMarshal.WriteInt64(this.propPviValue.pData, System.Convert.ToInt64(this.propPviValue.propEnumerations.EnumValue(newValue.ToString()).ToString()));
              this.propPviValue.isAssigned = true;
              break;
            }
            break;
          }
          PviMarshal.WriteInt64(this.propPviValue.pData, (long) newValue);
          this.propPviValue.isAssigned = true;
          break;
        case DataType.Byte:
        case DataType.UInt8:
          size = Marshal.SizeOf(typeof (byte));
          this.ValidateDataPtr(size);
          if (IntPtr.Zero == this.propPviValue.pData)
          {
            if ((sbyte) 1 == this.Value.IsEnum && DataType.String == newValue.DataType)
            {
              if (this.Value.propEnumerations.Names.Contains((object) newValue.ToString()))
              {
                Marshal.WriteByte(this.Value.pData, this.propOffset, System.Convert.ToByte(this.Value.propEnumerations.EnumValue(newValue.ToString()).ToString()));
                this.Value.isAssigned = true;
                break;
              }
              break;
            }
            Marshal.WriteByte(this.Value.pData, this.propOffset, (byte) newValue);
            this.Value.isAssigned = true;
            break;
          }
          if ((sbyte) 1 == this.propPviValue.IsEnum && DataType.String == newValue.DataType)
          {
            if (this.propPviValue.propEnumerations.Names.Contains((object) newValue.ToString()))
            {
              Marshal.WriteByte(this.propPviValue.pData, System.Convert.ToByte(this.propPviValue.propEnumerations.EnumValue(newValue.ToString()).ToString()));
              this.propPviValue.isAssigned = true;
              break;
            }
            break;
          }
          Marshal.WriteByte(this.propPviValue.pData, (byte) newValue);
          this.propPviValue.isAssigned = true;
          break;
        case DataType.UInt16:
        case DataType.WORD:
          size = Marshal.SizeOf(typeof (ushort));
          this.ValidateDataPtr(size);
          if (IntPtr.Zero == this.propPviValue.pData)
          {
            if ((sbyte) 1 == this.Value.IsEnum && DataType.String == newValue.DataType)
            {
              if (this.Value.propEnumerations.Names.Contains((object) newValue.ToString()))
              {
                Marshal.WriteInt16(this.Value.pData, this.propOffset, (short) System.Convert.ToUInt16(this.Value.propEnumerations.EnumValue(newValue.ToString()).ToString()));
                this.Value.isAssigned = true;
                break;
              }
              break;
            }
            Marshal.WriteInt16(this.Value.pData, this.propOffset, (short) (ushort) newValue);
            this.Value.isAssigned = true;
            break;
          }
          if ((sbyte) 1 == this.propPviValue.IsEnum && DataType.String == newValue.DataType)
          {
            if (this.propPviValue.propEnumerations.Names.Contains((object) newValue.ToString()))
            {
              Marshal.WriteInt16(this.propPviValue.pData, (short) System.Convert.ToUInt16(this.propPviValue.propEnumerations.EnumValue(newValue.ToString()).ToString()));
              this.propPviValue.isAssigned = true;
              break;
            }
            break;
          }
          Marshal.WriteInt16(this.propPviValue.pData, (short) (ushort) newValue);
          this.propPviValue.isAssigned = true;
          break;
        case DataType.UInt32:
        case DataType.DWORD:
          size = Marshal.SizeOf(typeof (uint));
          this.ValidateDataPtr(size);
          if (IntPtr.Zero == this.propPviValue.pData)
          {
            if ((sbyte) 1 == this.Value.IsEnum && DataType.String == newValue.DataType)
            {
              if (this.Value.propEnumerations.Names.Contains((object) newValue.ToString()))
              {
                Marshal.WriteInt32(this.Value.pData, this.propOffset, (int) System.Convert.ToUInt32(this.Value.propEnumerations.EnumValue(newValue.ToString()).ToString()));
                this.Value.isAssigned = true;
                break;
              }
              break;
            }
            Marshal.WriteInt32(this.Value.pData, this.propOffset, (int) (uint) newValue);
            this.Value.isAssigned = true;
            break;
          }
          if ((sbyte) 1 == this.propPviValue.IsEnum && DataType.String == newValue.DataType)
          {
            if (this.propPviValue.propEnumerations.Names.Contains((object) newValue.ToString()))
            {
              Marshal.WriteInt32(this.propPviValue.pData, (int) System.Convert.ToUInt32(this.propPviValue.propEnumerations.EnumValue(newValue.ToString()).ToString()));
              this.propPviValue.isAssigned = true;
              break;
            }
            break;
          }
          Marshal.WriteInt32(this.propPviValue.pData, (int) (uint) newValue);
          this.propPviValue.isAssigned = true;
          break;
        case DataType.UInt64:
          size = Marshal.SizeOf(typeof (ulong));
          this.ValidateDataPtr(size);
          if (IntPtr.Zero == this.propPviValue.pData)
          {
            if ((sbyte) 1 == this.Value.IsEnum && DataType.String == newValue.DataType)
            {
              if (this.Value.propEnumerations.Names.Contains((object) newValue.ToString()))
              {
                PviMarshal.WriteInt64(this.Value.pData, this.propOffset, (long) System.Convert.ToUInt64(this.Value.propEnumerations.EnumValue(newValue.ToString()).ToString()));
                this.Value.isAssigned = true;
                break;
              }
              break;
            }
            PviMarshal.WriteInt64(this.Value.pData, this.propOffset, (long) (ulong) newValue);
            this.Value.isAssigned = true;
            break;
          }
          if ((sbyte) 1 == this.propPviValue.IsEnum && DataType.String == newValue.DataType)
          {
            if (this.propPviValue.propEnumerations.Names.Contains((object) newValue.ToString()))
            {
              PviMarshal.WriteInt64(this.propPviValue.pData, (long) System.Convert.ToUInt64(this.propPviValue.propEnumerations.EnumValue(newValue.ToString()).ToString()));
              this.propPviValue.isAssigned = true;
              break;
            }
            break;
          }
          PviMarshal.WriteInt64(this.propPviValue.pData, (long) (ulong) newValue);
          this.propPviValue.isAssigned = true;
          break;
        case DataType.Single:
          size = Marshal.SizeOf(typeof (float));
          this.ValidateDataPtr(size);
          if (IntPtr.Zero == this.propPviValue.pData)
          {
            if ((sbyte) 1 == this.Value.IsEnum && DataType.String == newValue.DataType)
            {
              if (this.Value.propEnumerations.Names.Contains((object) newValue.ToString()))
              {
                PviMarshal.WriteSingle(this.Value.pData, this.propOffset, System.Convert.ToSingle(this.Value.propEnumerations.EnumValue(newValue.ToString()).ToString()));
                this.Value.isAssigned = true;
                break;
              }
              break;
            }
            PviMarshal.WriteSingle(this.Value.pData, this.propOffset, newValue.ToSingle((IFormatProvider) null));
            this.Value.isAssigned = true;
            break;
          }
          if ((sbyte) 1 == this.propPviValue.IsEnum && DataType.String == newValue.DataType)
          {
            if (this.propPviValue.propEnumerations.Names.Contains((object) newValue.ToString()))
            {
              PviMarshal.WriteSingle(this.propPviValue.pData, 0, System.Convert.ToSingle(this.propPviValue.propEnumerations.EnumValue(newValue.ToString()).ToString()));
              this.propPviValue.isAssigned = true;
              break;
            }
            break;
          }
          PviMarshal.WriteSingle(this.propPviValue.pData, 0, newValue.ToSingle((IFormatProvider) null));
          this.propPviValue.isAssigned = true;
          break;
        case DataType.Double:
          size = Marshal.SizeOf(typeof (double));
          this.ValidateDataPtr(size);
          if (IntPtr.Zero == this.propPviValue.pData)
          {
            if ((sbyte) 1 == this.Value.IsEnum && DataType.String == newValue.DataType)
            {
              if (this.Value.propEnumerations.Names.Contains((object) newValue.ToString()))
              {
                PviMarshal.WriteDouble(this.Value.pData, this.propOffset, System.Convert.ToDouble(this.Value.propEnumerations.EnumValue(newValue.ToString()).ToString()));
                this.Value.isAssigned = true;
                break;
              }
              break;
            }
            PviMarshal.WriteDouble(this.Value.pData, this.propOffset, newValue.ToDouble((IFormatProvider) null));
            this.Value.isAssigned = true;
            break;
          }
          if ((sbyte) 1 == this.propPviValue.IsEnum && DataType.String == newValue.DataType)
          {
            if (this.propPviValue.propEnumerations.Names.Contains((object) newValue.ToString()))
            {
              PviMarshal.WriteDouble(this.propPviValue.pData, 0, System.Convert.ToDouble(this.propPviValue.propEnumerations.EnumValue(newValue.ToString()).ToString()));
              this.propPviValue.isAssigned = true;
              break;
            }
            break;
          }
          PviMarshal.WriteDouble(this.propPviValue.pData, 0, newValue.ToDouble((IFormatProvider) null));
          this.propPviValue.isAssigned = true;
          break;
        case DataType.TimeSpan:
        case DataType.TimeOfDay:
        case DataType.TOD:
          size = Marshal.SizeOf(typeof (int));
          this.ValidateDataPtr(size);
          if (IntPtr.Zero == this.propPviValue.pData)
          {
            if (DataType.DateTime == newValue.DataType)
            {
              Marshal.WriteInt32(this.Value.pData, this.propOffset, Pvi.GetTimeSpanInt32((object) (DateTime) newValue.propObjValue));
              this.Value.isAssigned = true;
              break;
            }
            if (DataType.TimeSpan == newValue.DataType)
            {
              Marshal.WriteInt32(this.Value.pData, this.propOffset, Pvi.GetTimeSpanInt32((object) (TimeSpan) newValue.propObjValue));
              this.Value.isAssigned = true;
              break;
            }
            Marshal.WriteInt32(this.Value.pData, this.propOffset, (int) newValue / 10000);
            this.Value.isAssigned = true;
            break;
          }
          if (DataType.DateTime == newValue.DataType)
          {
            Marshal.WriteInt32(this.propPviValue.pData, Pvi.GetTimeSpanInt32((object) (DateTime) newValue.propObjValue));
            this.propPviValue.isAssigned = true;
            break;
          }
          if (DataType.TimeSpan == newValue.DataType)
          {
            Marshal.WriteInt32(this.propPviValue.pData, Pvi.GetTimeSpanInt32((object) (TimeSpan) newValue.propObjValue));
            this.propPviValue.isAssigned = true;
            break;
          }
          Marshal.WriteInt32(this.propPviValue.pData, (int) newValue / 10000);
          this.propPviValue.isAssigned = true;
          break;
        case DataType.DateTime:
        case DataType.Date:
        case DataType.DT:
          size = Marshal.SizeOf(typeof (int));
          this.ValidateDataPtr(size);
          if (IntPtr.Zero == this.propPviValue.pData)
          {
            if (DataType.DateTime == newValue.DataType)
            {
              Marshal.WriteInt32(this.Value.pData, this.propOffset, (int) Pvi.DateTimeToUInt32((DateTime) newValue.propObjValue));
              this.Value.isAssigned = true;
              break;
            }
            if (DataType.TimeSpan == newValue.DataType)
            {
              Marshal.WriteInt32(this.Value.pData, this.propOffset, (int) Pvi.DateTimeToUInt32(new DateTime(((TimeSpan) newValue.propObjValue).Ticks)));
              this.Value.isAssigned = true;
              break;
            }
            Marshal.WriteInt32(this.Value.pData, this.propOffset, (int) newValue);
            this.Value.isAssigned = true;
            break;
          }
          if (DataType.DateTime == newValue.DataType)
          {
            Marshal.WriteInt32(this.propPviValue.pData, (int) Pvi.DateTimeToUInt32((DateTime) newValue.propObjValue));
            this.propPviValue.isAssigned = true;
            break;
          }
          if (DataType.TimeSpan == newValue.DataType)
          {
            Marshal.WriteInt32(this.propPviValue.pData, (int) Pvi.DateTimeToUInt32(new DateTime(((TimeSpan) newValue.propObjValue).Ticks)));
            this.propPviValue.isAssigned = true;
            break;
          }
          Marshal.WriteInt32(this.propPviValue.pData, (int) newValue);
          this.propPviValue.isAssigned = true;
          break;
        case DataType.String:
          size = this.propPviValue.DataSize;
          this.ValidateDataPtr(size);
          this.propPviValue.propDataSize = this.propPviValue.DataSize;
          if ((Value) null != newValue)
          {
            string str = newValue.ToString();
            for (int index = 0; index < this.propPviValue.propDataSize; ++index)
            {
              if (index < str.Length)
              {
                if (IntPtr.Zero == this.propPviValue.pData)
                {
                  Marshal.WriteByte(this.Value.pData, this.propOffset + index, (byte) str[index]);
                  this.Value.isAssigned = true;
                }
                else
                {
                  Marshal.WriteByte(this.propPviValue.pData, index, (byte) str[index]);
                  this.propPviValue.isAssigned = true;
                }
              }
              else if (IntPtr.Zero == this.propPviValue.pData)
              {
                Marshal.WriteByte(this.Value.pData, this.propOffset + index, (byte) 0);
                this.Value.isAssigned = true;
              }
              else
              {
                Marshal.WriteByte(this.propPviValue.pData, index, (byte) 0);
                this.propPviValue.isAssigned = true;
              }
            }
            break;
          }
          for (int ofs = 0; ofs < this.propPviValue.propDataSize; ++ofs)
          {
            if (IntPtr.Zero == this.propPviValue.pData)
            {
              Marshal.WriteByte(this.Value.pData, this.propOffset + ofs, (byte) 0);
              this.Value.isAssigned = true;
            }
            else
            {
              Marshal.WriteByte(this.propPviValue.pData, ofs, (byte) 0);
              this.propPviValue.isAssigned = true;
            }
          }
          break;
        case DataType.WString:
          size = this.propPviValue.DataSize;
          this.ValidateDataPtr(size);
          this.propPviValue.propDataSize = this.propPviValue.DataSize;
          if ((Value) null != newValue)
          {
            byte[] destination;
            if (DataType.WString == newValue.DataType)
            {
              destination = new byte[newValue.DataSize];
              Marshal.Copy(newValue.pData, destination, 0, newValue.DataSize);
            }
            else
            {
              string str = newValue.ToString();
              destination = new byte[str.Length * 2 + 2];
              int index1 = 0;
              for (int index2 = 0; index2 < str.Length; ++index2)
              {
                destination.SetValue((object) (byte) str[index2], index1);
                int index3 = index1 + 1;
                destination.SetValue((object) (byte) 0, index3);
                index1 = index3 + 1;
              }
            }
            for (int index = 0; index < this.propPviValue.propDataSize; ++index)
            {
              if (index < destination.Length)
              {
                if (IntPtr.Zero == this.propPviValue.pData)
                {
                  Marshal.WriteByte(this.Value.pData, this.propOffset + index, (byte) destination.GetValue(index));
                  this.Value.isAssigned = true;
                }
                else
                {
                  Marshal.WriteByte(this.propPviValue.pData, index, (byte) destination.GetValue(index));
                  this.propPviValue.isAssigned = true;
                }
              }
              else if (IntPtr.Zero == this.propPviValue.pData)
              {
                Marshal.WriteByte(this.Value.pData, this.propOffset + index, (byte) 0);
                this.Value.isAssigned = true;
              }
              else
              {
                Marshal.WriteByte(this.propPviValue.pData, index, (byte) 0);
                this.propPviValue.isAssigned = true;
              }
            }
            break;
          }
          for (int ofs = 0; ofs < this.propPviValue.propDataSize; ++ofs)
          {
            if (IntPtr.Zero == this.propPviValue.pData)
            {
              Marshal.WriteByte(this.Value.pData, this.propOffset + ofs, (byte) 0);
              this.Value.isAssigned = true;
            }
            else
            {
              Marshal.WriteByte(this.propPviValue.pData, ofs, (byte) 0);
              this.propPviValue.isAssigned = true;
            }
          }
          break;
      }
      if (0 < size)
      {
        this.propPviValue.propDataSize = size;
        num = 0;
      }
      return num;
    }

    protected void UpdatedStructValue()
    {
      this.propPviValue.propDataSize = this.propPviValue.propByteField.Length;
      this.ResizePviDataPtr(this.propPviValue.propByteField.Length);
      Marshal.Copy(this.propPviValue.propByteField, 0, this.propPviValue.pData, this.propPviValue.propByteField.Length);
    }

    protected void UpdatingStructValue()
    {
      this.propPviValue.propDataSize = this.propPviValue.propByteField.Length;
      this.propInternalByteField = (byte[]) this.propPviValue.propByteField.Clone();
    }

    internal static int CalculateByteOffset(int[] indices, Value pPviValue) => Variable.CalculateIndex(indices, pPviValue) * pPviValue.propTypeLength;

    internal static int CalculateIndex(int[] indices, Value pPviValue)
    {
      int num1 = 0;
      int index1;
      if (pPviValue.propDimensions != null && indices.Length <= pPviValue.propDimensions.Count)
      {
        int index2;
        for (index2 = 0; index2 < indices.Length - 1; ++index2)
        {
          int num2 = indices[index2] - pPviValue.propDimensions[index2].StartIndex;
          int num3 = 1;
          for (int index3 = index2 + 1; index3 < pPviValue.propDimensions.Count; ++index3)
            num3 = pPviValue.propDimensions[index3].NumOfElements * num3;
          num1 += num2 * num3;
        }
        int num4 = indices.Length != pPviValue.propDimensions.Count ? indices[index2] : indices[index2] - pPviValue.propDimensions[index2].StartIndex;
        index1 = num1 + num4;
        if (pPviValue.ArrayLength <= num4)
          index1 = indices[0];
      }
      else
      {
        int num5 = indices[0] - pPviValue.ArrayMinIndex;
        for (int index4 = 1; index4 < indices.Length; ++index4)
          num5 += indices[index4];
        if (pPviValue.ArrayLength <= num5)
          num5 = indices[0];
        index1 = num5;
      }
      return index1;
    }

    internal int SetStructureMemberValue(Value value, params int[] indices)
    {
      int num = -1;
      string str = "";
      if (Access.Read == this.Access)
      {
        if (this.WriteValueAutomatic)
        {
          for (int index = 0; index < indices.Length; ++index)
            str = str + "[" + (System.Convert.ToInt32(indices.GetValue(index).ToString()) - this.propPviValue.ArrayMinIndex).ToString() + "]";
          this.OnValueWritten(new PviEventArgs(this.Name + str, this.Address + str, 12034, this.Service.Language, Action.VariableValueWrite, this.Service));
        }
        return 12034;
      }
      if (1 < indices.Length && (this.propPviValue.ArrayDimensions == null || this.propPviValue.ArrayDimensions.Count == 0))
      {
        for (int index = 0; index < indices.Length; ++index)
          this.Value[System.Convert.ToInt32(indices.GetValue(index).ToString())].Assign(value.ToSystemDataTypeValue((IFormatProvider) null));
        if (this.WriteValueAutomatic)
          num = this.WriteValue();
        return num;
      }
      if (IntPtr.Zero == this.propPviValue.pData)
      {
        this.propPviValue.pData = PviMarshal.AllocHGlobal(this.Value.DataSize);
        this.propPviValue.propHasOwnDataPtr = true;
      }
      if (!this.propPviValue.IsOfTypeArray || this.propPviValue.propArryOne && this.Value.ArrayLength == value.ArrayLength)
      {
        this.Value = value;
        return 0;
      }
      int byteOffset = Variable.CalculateByteOffset(indices, this.propPviValue);
      switch (this.propPviValue.DataType)
      {
        case DataType.Boolean:
          if (!(bool) value)
          {
            Marshal.WriteByte(this.propPviValue.pData, byteOffset, (byte) 0);
            break;
          }
          Marshal.WriteByte(this.propPviValue.pData, byteOffset, (byte) 1);
          break;
        case DataType.SByte:
          PviMarshal.WriteSByte(this.propPviValue.pData, byteOffset, PviMarshal.toSByte((object) value));
          break;
        case DataType.Int16:
          Marshal.WriteInt16(this.propPviValue.pData, byteOffset, PviMarshal.toInt16((object) value));
          break;
        case DataType.Int32:
          Marshal.WriteInt32(this.propPviValue.pData, byteOffset, PviMarshal.toInt32((object) value));
          break;
        case DataType.Int64:
          PviMarshal.WriteInt64(this.propPviValue.pData, byteOffset, PviMarshal.toInt64((object) value));
          break;
        case DataType.Byte:
        case DataType.UInt8:
          Marshal.WriteByte(this.propPviValue.pData, byteOffset, PviMarshal.toByte((object) value));
          break;
        case DataType.UInt16:
        case DataType.WORD:
          PviMarshal.WriteUInt16(this.propPviValue.pData, byteOffset, PviMarshal.toUInt16((object) value));
          break;
        case DataType.UInt32:
        case DataType.DWORD:
          PviMarshal.WriteUInt32(this.propPviValue.pData, byteOffset, PviMarshal.toUInt32((object) value));
          break;
        case DataType.UInt64:
          PviMarshal.WriteUInt64(this.propPviValue.pData, byteOffset, PviMarshal.toUInt64((object) value));
          break;
        case DataType.Single:
          this.Service.cpyFltToBuffer((object) value);
          Marshal.WriteByte(this.propPviValue.pData, byteOffset, this.Service.ByteBuffer[0]);
          Marshal.WriteByte(this.propPviValue.pData, byteOffset + 1, this.Service.ByteBuffer[1]);
          Marshal.WriteByte(this.propPviValue.pData, byteOffset + 2, this.Service.ByteBuffer[2]);
          Marshal.WriteByte(this.propPviValue.pData, byteOffset + 3, this.Service.ByteBuffer[3]);
          break;
        case DataType.Double:
          this.Service.cpyDblToBuffer((object) value);
          Marshal.WriteByte(this.propPviValue.pData, byteOffset, this.Service.ByteBuffer[0]);
          Marshal.WriteByte(this.propPviValue.pData, byteOffset + 1, this.Service.ByteBuffer[1]);
          Marshal.WriteByte(this.propPviValue.pData, byteOffset + 2, this.Service.ByteBuffer[2]);
          Marshal.WriteByte(this.propPviValue.pData, byteOffset + 3, this.Service.ByteBuffer[3]);
          Marshal.WriteByte(this.propPviValue.pData, byteOffset + 4, this.Service.ByteBuffer[4]);
          Marshal.WriteByte(this.propPviValue.pData, byteOffset + 5, this.Service.ByteBuffer[5]);
          Marshal.WriteByte(this.propPviValue.pData, byteOffset + 6, this.Service.ByteBuffer[6]);
          Marshal.WriteByte(this.propPviValue.pData, byteOffset + 7, this.Service.ByteBuffer[7]);
          break;
        case DataType.TimeSpan:
        case DataType.TimeOfDay:
        case DataType.TOD:
          Marshal.WriteInt32(this.propPviValue.pData, byteOffset, Pvi.GetTimeSpanInt32((object) value));
          break;
        case DataType.DateTime:
        case DataType.Date:
        case DataType.DT:
          PviMarshal.WriteUInt32(this.propPviValue.pData, byteOffset, Pvi.GetDateTimeUInt32((object) value));
          break;
        case DataType.String:
          for (int ofs = 0; ofs < this.propPviValue.propTypeLength; ++ofs)
          {
            Marshal.WriteByte(this.propPviValue.pData, byteOffset + ofs, (byte) 0);
            if (ofs < value.TypeLength)
            {
              byte val = Marshal.ReadByte(value.pData, ofs);
              Marshal.WriteByte(this.propPviValue.pData, byteOffset + ofs, val);
            }
          }
          break;
        case DataType.WString:
          Value obj;
          if (DataType.WString == value.DataType)
          {
            obj = value;
          }
          else
          {
            obj = new Value();
            obj.propDataType = DataType.WString;
            obj.propTypeLength = value.ToString().Length * 2;
            obj.Assign((object) value.ToString());
          }
          for (int ofs = 0; ofs < this.propPviValue.propTypeLength; ++ofs)
          {
            Marshal.WriteByte(this.propPviValue.pData, byteOffset + ofs, (byte) 0);
            if (ofs < obj.TypeLength)
            {
              byte val = Marshal.ReadByte(obj.pData, ofs);
              Marshal.WriteByte(this.propPviValue.pData, byteOffset + ofs, val);
            }
          }
          break;
      }
      if (this.WriteValueAutomatic)
        num = this.WriteValue();
      else
        this.Value.isAssigned = true;
      return num;
    }

    internal int SetStructureMemberValue(string varName, Value value)
    {
      int num1 = 0;
      if (Access.Read == this.Access)
      {
        if (this.WriteValueAutomatic)
        {
          if (varName.IndexOf("[") == 0)
            this.OnValueWritten(new PviEventArgs(this.Name + "." + varName, this.Address + "." + varName, 12034, this.Service.Language, Action.VariableValueWrite, this.Service));
          else
            this.OnValueWritten(new PviEventArgs(this.Name + varName, this.Address + varName, 12034, this.Service.Language, Action.VariableValueWrite, this.Service));
        }
        return 12034;
      }
      if (IntPtr.Zero == this.propPviValue.pData)
      {
        this.propPviValue.pData = PviMarshal.AllocHGlobal(this.Value.DataSize);
        this.propPviValue.propHasOwnDataPtr = true;
      }
      if (this.StructureMembers != null && this.StructureMembers.ContainsKey((object) varName))
      {
        int num2 = this.StructureMembers[varName].setValue(this, value);
        if (num2 == 0 && this.WriteValueAutomatic)
          num2 = this.WriteValue();
        return num2;
      }
      string oldStyleValueName = this.GetOldStyleValueName(varName);
      if (this.StructureMembers != null && this.StructureMembers.ContainsKey((object) oldStyleValueName))
      {
        int num3 = this.StructureMembers[oldStyleValueName].setValue(this, value);
        if (num3 == 0 && this.WriteValueAutomatic)
          num3 = this.WriteValue();
        return num3;
      }
      if (!this.propPviValue.IsOfTypeArray)
      {
        this.Value = value;
        return 0;
      }
      if (this.propPviValue.IsOfTypeArray)
      {
        int num4;
        if (-1 == oldStyleValueName.IndexOf("["))
          num4 = System.Convert.ToInt32(oldStyleValueName) * this.propPviValue.TypeLength;
        else if (oldStyleValueName.IndexOf('[') == 0)
        {
          string[] strArray = oldStyleValueName.Substring(1, oldStyleValueName.Length - 2).Split(',');
          if (1 < strArray.Length)
          {
            int[] numArray = new int[strArray.Length];
            for (int index = 0; index < strArray.Length; ++index)
              numArray.SetValue((object) System.Convert.ToInt32(strArray.GetValue(index).ToString()), index);
            return this.SetStructureMemberValue(value, numArray);
          }
          System.Convert.ToInt32(oldStyleValueName.Substring(1, oldStyleValueName.Length - 2));
          num4 = System.Convert.ToInt32(oldStyleValueName.Substring(1, oldStyleValueName.Length - 2)) * this.propPviValue.TypeLength;
        }
        else
          num4 = System.Convert.ToInt32(oldStyleValueName.Substring(1, oldStyleValueName.Length - 2)) * this.propPviValue.TypeLength;
        switch (this.propPviValue.DataType)
        {
          case DataType.Boolean:
            if (!(bool) value)
            {
              Marshal.WriteByte(this.propPviValue.pData, num4, (byte) 0);
              break;
            }
            Marshal.WriteByte(this.propPviValue.pData, num4, (byte) 1);
            break;
          case DataType.SByte:
            PviMarshal.WriteSByte(this.propPviValue.pData, num4, PviMarshal.toSByte((object) value));
            break;
          case DataType.Int16:
            Marshal.WriteInt16(this.propPviValue.pData, num4, PviMarshal.toInt16((object) value));
            break;
          case DataType.Int32:
            Marshal.WriteInt32(this.propPviValue.pData, num4, PviMarshal.toInt32((object) value));
            break;
          case DataType.Int64:
            PviMarshal.WriteInt64(this.propPviValue.pData, num4, PviMarshal.toInt64((object) value));
            break;
          case DataType.Byte:
          case DataType.UInt8:
            Marshal.WriteByte(this.propPviValue.pData, num4, PviMarshal.toByte((object) value));
            break;
          case DataType.UInt16:
          case DataType.WORD:
            PviMarshal.WriteUInt16(this.propPviValue.pData, num4, PviMarshal.toUInt16((object) value));
            break;
          case DataType.UInt32:
          case DataType.DWORD:
            PviMarshal.WriteUInt32(this.propPviValue.pData, num4, PviMarshal.toUInt32((object) value));
            break;
          case DataType.UInt64:
            PviMarshal.WriteUInt64(this.propPviValue.pData, num4, PviMarshal.toUInt64((object) value));
            break;
          case DataType.Single:
            this.Service.cpyFltToBuffer((object) value);
            Marshal.WriteByte(this.propPviValue.pData, num4, this.Service.ByteBuffer[0]);
            Marshal.WriteByte(this.propPviValue.pData, num4 + 1, this.Service.ByteBuffer[1]);
            Marshal.WriteByte(this.propPviValue.pData, num4 + 2, this.Service.ByteBuffer[2]);
            Marshal.WriteByte(this.propPviValue.pData, num4 + 3, this.Service.ByteBuffer[3]);
            break;
          case DataType.Double:
            this.Service.cpyDblToBuffer((object) value);
            Marshal.WriteByte(this.propPviValue.pData, num4, this.Service.ByteBuffer[0]);
            Marshal.WriteByte(this.propPviValue.pData, num4 + 1, this.Service.ByteBuffer[1]);
            Marshal.WriteByte(this.propPviValue.pData, num4 + 2, this.Service.ByteBuffer[2]);
            Marshal.WriteByte(this.propPviValue.pData, num4 + 3, this.Service.ByteBuffer[3]);
            Marshal.WriteByte(this.propPviValue.pData, num4 + 4, this.Service.ByteBuffer[4]);
            Marshal.WriteByte(this.propPviValue.pData, num4 + 5, this.Service.ByteBuffer[5]);
            Marshal.WriteByte(this.propPviValue.pData, num4 + 6, this.Service.ByteBuffer[6]);
            Marshal.WriteByte(this.propPviValue.pData, num4 + 7, this.Service.ByteBuffer[7]);
            break;
          case DataType.TimeSpan:
          case DataType.TimeOfDay:
          case DataType.TOD:
            Marshal.WriteInt32(this.propPviValue.pData, num4, Pvi.GetTimeSpanInt32((object) value));
            break;
          case DataType.DateTime:
          case DataType.Date:
          case DataType.DT:
            PviMarshal.WriteUInt32(this.propPviValue.pData, num4, Pvi.GetDateTimeUInt32((object) value));
            break;
          case DataType.String:
            for (int ofs = 0; ofs < this.propPviValue.propTypeLength; ++ofs)
            {
              Marshal.WriteByte(this.propPviValue.pData, num4 + ofs, (byte) 0);
              if (ofs < value.TypeLength)
              {
                byte val = Marshal.ReadByte(value.pData, ofs);
                Marshal.WriteByte(this.propPviValue.pData, num4 + ofs, val);
              }
            }
            break;
          case DataType.WString:
            Value obj;
            if (DataType.WString == value.DataType)
            {
              obj = value;
            }
            else
            {
              obj = new Value();
              obj.propDataType = DataType.WString;
              obj.propTypeLength = value.ToString().Length * 2;
              obj.Assign((object) value.ToString());
            }
            for (int ofs = 0; ofs < this.propPviValue.propTypeLength; ++ofs)
            {
              Marshal.WriteByte(this.propPviValue.pData, num4 + ofs, (byte) 0);
              if (ofs < obj.TypeLength)
              {
                byte val = Marshal.ReadByte(obj.pData, ofs);
                Marshal.WriteByte(this.propPviValue.pData, num4 + ofs, val);
              }
            }
            break;
        }
        if (this.WriteValueAutomatic)
          num1 = this.WriteValue();
        return num1;
      }
      if (this.WriteValueAutomatic)
        this.OnValueWritten(new PviEventArgs(this.Name, this.Address, 120025, this.Service.Language, Action.VariableValueWrite, this.Service));
      return -1;
    }

    internal void CleanupMemory()
    {
      this.propReadingFormat = false;
      if ((Value) null != this.propPviValue)
      {
        if (this.propPviValue.propHasOwnDataPtr && IntPtr.Zero != this.propPviValue.pData)
        {
          PviMarshal.FreeHGlobal(ref this.propPviValue.pData);
          this.propPviValue.pData = IntPtr.Zero;
        }
        this.propPviValue.propByteField = (byte[]) null;
        this.propPviValue.propObjValue = (object) null;
        this.propPviValue = (Value) null;
      }
      if (this.pReadData == IntPtr.Zero)
      {
        PviMarshal.FreeHGlobal(ref this.pReadData);
        this.pReadData = IntPtr.Zero;
      }
      if (this.pWriteData == IntPtr.Zero)
      {
        PviMarshal.FreeHGlobal(ref this.pWriteData);
        this.pWriteData = IntPtr.Zero;
      }
      this.propInternalByteField = (byte[]) null;
      if ((Value) null != this.propInternalValue)
      {
        if (this.propInternalValue.propHasOwnDataPtr && IntPtr.Zero != this.propInternalValue.pData)
        {
          PviMarshal.FreeHGlobal(ref this.propInternalValue.pData);
          this.propInternalValue.pData = IntPtr.Zero;
        }
        this.propInternalValue.propByteField = (byte[]) null;
        this.propInternalValue.propObjValue = (object) null;
        this.propInternalValue = (Value) null;
      }
      if (this.propIODataPoints != null)
        this.propIODataPoints = (IODataPointCollection) null;
      if (this.mapNameToMember != null)
      {
        this.mapNameToMember.CleanUp(true);
        this.mapNameToMember = (StructMemberCollection) null;
      }
      if (this.propMembers != null)
      {
        this.propMembers.Dispose(true, true);
        this.propMembers = (MemberCollection) null;
      }
      if (this.propUserMembers != null)
      {
        this.propUserMembers.Dispose(true, true);
        this.propUserMembers = (MemberCollection) null;
      }
      this.propChangedStructMembers = (string[]) null;
      if (this.propScalingPoints != null)
      {
        this.propScalingPoints.Clear();
        this.propScalingPoints = (ScalingPointCollection) null;
      }
      if (this.propWriteByteField != null)
        this.propWriteByteField = (byte[]) null;
      if (this.propReadByteField != null)
        this.propReadByteField = (byte[]) null;
      if (this.propUserCollections != null)
      {
        this.propUserCollections.Clear();
        this.propUserCollections = (Hashtable) null;
      }
      if (this.propUserMembers == null)
        return;
      this.propUserMembers.CleanUp(true);
      this.propUserMembers = (MemberCollection) null;
    }

    internal override void Dispose(bool disposing, bool removeFromCollection)
    {
      if (this.propDisposed)
        return;
      this.propReadingFormat = false;
      if (disposing)
      {
        if ((Value) null != this.propPviValue)
        {
          this.propPviValue.Dispose(disposing);
          this.propPviValue = (Value) null;
        }
        if (this.pReadData == IntPtr.Zero)
        {
          PviMarshal.FreeHGlobal(ref this.pReadData);
          this.pReadData = IntPtr.Zero;
        }
        if (this.pWriteData == IntPtr.Zero)
        {
          PviMarshal.FreeHGlobal(ref this.pWriteData);
          this.pWriteData = IntPtr.Zero;
        }
        this.propInternalByteField = (byte[]) null;
        if ((Value) null != this.propInternalValue)
        {
          this.propInternalValue.Dispose(disposing);
          this.propInternalValue = (Value) null;
        }
        if (this.propIODataPoints != null)
        {
          this.propIODataPoints.Dispose(disposing, removeFromCollection);
          this.propIODataPoints = (IODataPointCollection) null;
        }
        if (this.mapNameToMember != null)
        {
          this.mapNameToMember.CleanUp(disposing);
          this.mapNameToMember = (StructMemberCollection) null;
        }
        if (this.propMembers != null)
        {
          this.propMembers.Dispose(disposing, removeFromCollection);
          this.propMembers = (MemberCollection) null;
        }
        if (this.propUserMembers != null)
        {
          this.propUserMembers.Dispose(disposing, removeFromCollection);
          this.propUserMembers = (MemberCollection) null;
        }
        this.propChangedStructMembers = (string[]) null;
        if (this.propScalingPoints != null)
        {
          this.propScalingPoints.Clear();
          this.propScalingPoints = (ScalingPointCollection) null;
        }
        if (this.propWriteByteField != null)
          this.propWriteByteField = (byte[]) null;
        if (this.propReadByteField != null)
          this.propReadByteField = (byte[]) null;
        if (this.propUserCollections != null)
        {
          this.propUserCollections.Clear();
          this.propUserCollections = (Hashtable) null;
        }
        if (this.propUserMembers != null)
        {
          this.propUserMembers.CleanUp(disposing);
          this.propUserMembers = (MemberCollection) null;
        }
        if (removeFromCollection)
        {
          if (this.Parent is Service && ((Service) this.Parent).Variables != null)
            ((Service) this.Parent).Variables.Remove(this.Name);
          if (this.Parent is Cpu)
            ((Cpu) this.Parent).Variables.Remove(this.Name);
          if (this.Parent is Task)
            ((Task) this.Parent).Variables.Remove(this.Name);
          if (this.Parent is Variable)
            ((Variable) this.Parent).Members.Remove(this.Name);
        }
      }
      base.Dispose(disposing, removeFromCollection);
    }

    public override void Remove()
    {
      base.Remove();
      if (this.propMembers != null)
        this.propMembers.CleanUp(false);
      if (this.Parent is Cpu)
        ((Cpu) this.Parent).Variables.Remove(this.Name);
      else if (this.Parent is Task)
        ((Task) this.Parent).Variables.Remove(this.Name);
      if (this.propUserCollections == null || 0 >= this.propUserCollections.Count)
        return;
      foreach (VariableCollection variableCollection in (IEnumerable) this.propUserCollections.Values)
        variableCollection.Remove(this);
    }

    internal void Remove(object coll)
    {
      base.Remove();
      if (this.propMembers != null)
        this.propMembers.CleanUp(false);
      if (this.Parent is Cpu && ((Cpu) this.Parent).Variables != coll)
        ((Cpu) this.Parent).Variables.Remove(this.Name);
      else if (this.Parent is Task && ((Task) this.Parent).Variables != coll)
        ((Task) this.Parent).Variables.Remove(this.Name);
      if (this.propUserCollections == null || 0 >= this.propUserCollections.Count)
        return;
      foreach (VariableCollection variableCollection in (IEnumerable) this.propUserCollections.Values)
      {
        if (variableCollection != coll)
          variableCollection.Remove(this);
      }
    }

    internal string GetPviDataTypeText(DataType dataType)
    {
      switch (dataType)
      {
        case DataType.SByte:
          return "i8";
        case DataType.Int16:
          return "i16";
        case DataType.Int32:
          return "i32";
        case DataType.Int64:
          return "i64";
        case DataType.Byte:
          return "u8";
        case DataType.UInt16:
          return "u16";
        case DataType.UInt32:
          return "u32";
        case DataType.UInt64:
          return "u64";
        case DataType.Single:
          return "f32";
        case DataType.Double:
          return "f64";
        case DataType.String:
          return "string";
        case DataType.WORD:
          return "WORD";
        case DataType.DWORD:
          return "DWORD";
        case DataType.UInt8:
          return "u8";
        default:
          return "";
      }
    }

    public void WriteScaling()
    {
      int errorCode = 0;
      IntPtr zero = IntPtr.Zero;
      if (this.Scaling.ScalingType == ScalingType.LimitValuesAndFactor)
      {
        this.Scaling.ScalingPoints = new ScalingPointCollection()
        {
          new ScalingPoint(this.Scaling.MinValue, this.Scaling.MinValue * this.Scaling.Factor),
          new ScalingPoint(this.Scaling.MaxValue, this.Scaling.MaxValue * this.Scaling.Factor)
        };
        this.Scaling.propScalingType = ScalingType.LimitValuesAndFactor;
      }
      string request = "";
      if (this.propScaling != null && this.propScaling.ScalingPoints.Count > 0)
      {
        request = "";
        foreach (ScalingPoint scalingPoint in (ArrayList) this.propScaling.ScalingPoints)
        {
          string str1 = scalingPoint.XValue.ToString((IFormatProvider) CultureInfo.InvariantCulture);
          string str2 = scalingPoint.YValue.ToString((IFormatProvider) CultureInfo.InvariantCulture);
          if (DataType.Single == this.propPviValue.propDataType || DataType.Double == this.propPviValue.propDataType)
          {
            if (-1 == str1.IndexOf('.'))
              str1 += ".0";
            if (-1 == str2.IndexOf('.'))
              str2 += ".0";
          }
          request += string.Format("{0},{1};", (object) str1, (object) str2);
        }
      }
      this.Service.BuildRequestBuffer(request);
      if (this.ConnectionType == ConnectionType.Link)
      {
        if (ConnectionStates.Connected == this.propConnectionState || ConnectionStates.ConnectedError == this.propConnectionState)
        {
          this.Disconnect(2715U);
          this.Connect(ConnectionType.Link, 2713);
          return;
        }
      }
      else if (this.propLinkId != 0U)
        errorCode = this.WriteRequest(this.Service.hPvi, this.propLinkId, AccessTypes.Function, this.Service.RequestBuffer, request.Length, 553U);
      if (errorCode == 0)
        return;
      this.OnError(new PviEventArgs(this.propName, this.propAddress, errorCode, this.Service.Language, Action.VariableScalingChange, this.Service));
    }

    public override int FromXmlTextReader(
      ref XmlTextReader reader,
      ConfigurationFlags flags,
      Base baseObj)
    {
      Variable baseObj1 = (Variable) baseObj;
      if (baseObj1 == null)
        return -1;
      base.FromXmlTextReader(ref reader, flags, (Base) baseObj1);
      string str = "";
      string attribute1 = reader.GetAttribute("DataType");
      if (attribute1 != null && attribute1.Length > 0)
        baseObj1.propPviValue.SetDataType(Variable.GetDataTypeFromString(attribute1));
      str = "";
      string attribute2 = reader.GetAttribute("Length");
      if (attribute2 != null && attribute2.Length > 0)
      {
        short result = 0;
        if (PviParse.TryParseInt16(attribute2, out result))
          baseObj1.propPviValue.SetArrayLength((int) result);
      }
      str = "";
      string attribute3 = reader.GetAttribute("Value");
      if (attribute3 != null && attribute3.Length > 0 && (ConfigurationFlags.Values & flags) != ConfigurationFlags.None)
        baseObj1.Value.Assign((object) attribute3);
      str = "";
      string attribute4 = reader.GetAttribute("Active");
      if (attribute4 != null && attribute4.Length > 0 && (ConfigurationFlags.ActiveState & flags) != ConfigurationFlags.None && attribute4.ToLower() == "true")
        baseObj1.SetActive(true);
      str = "";
      string attribute5 = reader.GetAttribute("RefreshTime");
      if (attribute5 != null && attribute5.Length > 0 && (ConfigurationFlags.RefreshTime & flags) != ConfigurationFlags.None)
      {
        int result = 0;
        if (PviParse.TryParseInt32(attribute5, out result))
          baseObj1.propRefreshTime = result;
      }
      str = "";
      string attribute6 = reader.GetAttribute("Access");
      if (attribute6 != null && attribute6.Length > 0)
      {
        switch (attribute6.ToLower())
        {
          case "no":
            baseObj1.Access = Access.No;
            break;
          case "read":
            baseObj1.Access = Access.Read;
            break;
          case "write":
            baseObj1.Access = Access.Write;
            break;
          case "readandwrite":
            baseObj1.Access = Access.ReadAndWrite;
            break;
          case "event":
            baseObj1.Access = Access.EVENT;
            break;
          case "direct":
            baseObj1.Access = Access.DIRECT;
            break;
          case "fastecho":
            baseObj1.Access = Access.FASTECHO;
            break;
        }
      }
      str = "";
      string attribute7 = reader.GetAttribute("CastMode");
      if (attribute7 != null && attribute7.Length > 0)
      {
        switch (attribute7.ToLower())
        {
          case "decimalconversion":
            baseObj1.propCastMode = CastModes.DecimalConversion;
            break;
          case "default":
            baseObj1.propCastMode = CastModes.DEFAULT;
            break;
          case "floatconversion":
            baseObj1.propCastMode = CastModes.FloatConversion;
            break;
          case "pg2000string":
            baseObj1.propCastMode = CastModes.PG2000String;
            break;
          case "rangecheck":
            baseObj1.propCastMode = CastModes.RangeCheck;
            break;
          case "stringtermination":
            baseObj1.propCastMode = CastModes.StringTermination;
            break;
        }
      }
      str = "";
      string attribute8 = reader.GetAttribute("StructName");
      if (attribute8 != null && attribute8.Length > 0)
        baseObj1.propStructName = attribute8;
      str = "";
      string attribute9 = reader.GetAttribute("StructMemberName");
      if (attribute9 != null && attribute9.Length > 0)
        baseObj1.propName = attribute9;
      str = "";
      string attribute10 = reader.GetAttribute("UserTag");
      if (attribute10 != null && attribute10.Length > 0)
        baseObj1.propUserTag = attribute10;
      str = "";
      string attribute11 = reader.GetAttribute("Scope");
      if (attribute11 != null && attribute11.Length > 0 && (ConfigurationFlags.Scope & flags) != ConfigurationFlags.None)
      {
        switch (attribute11)
        {
          case "Global":
            baseObj1.propScope = Scope.Global;
            break;
          case "Local":
            baseObj1.propScope = Scope.Local;
            break;
          case "Dynamic":
            baseObj1.propScope = Scope.Dynamic;
            break;
          default:
            baseObj1.propScope = Scope.UNDEFINED;
            break;
        }
      }
      str = "";
      string attribute12 = reader.GetAttribute("BitOffset");
      if (attribute12 != null && attribute12.Length > 0)
      {
        int result = 0;
        if (PviParse.TryParseInt32(attribute12, out result))
          baseObj1.propBitOffset = result;
      }
      str = "";
      string attribute13 = reader.GetAttribute("Hysteresis");
      if (attribute13 != null && attribute13.Length > 0)
        PviParse.TryParseDouble(attribute13, out baseObj1.propHysteresis);
      str = "";
      string attribute14 = reader.GetAttribute("InitValue");
      if (attribute14 != null && attribute14.Length > 0)
        baseObj1.propInitValue = attribute14;
      str = "";
      string attribute15 = reader.GetAttribute("ExpandMembers");
      if (attribute15 != null && attribute15.Length > 0)
      {
        switch (attribute15.ToLower())
        {
          case "true":
            baseObj1.propExpandMembers = true;
            break;
          case "false":
            baseObj1.propExpandMembers = false;
            break;
        }
      }
      str = "";
      string attribute16 = reader.GetAttribute("Polling");
      if (attribute16 != null && string.Compare(attribute16.ToLower(), "false") == 0)
        baseObj1.propPolling = false;
      str = "";
      string attribute17 = reader.GetAttribute("DataValid");
      if (attribute17 != null && string.Compare(attribute17.ToLower(), "true") == 0)
        baseObj1.propDataValid = true;
      str = "";
      string attribute18 = reader.GetAttribute("WriteValueAutomatic");
      if (attribute18 != null && string.Compare(attribute18.ToLower(), "false") == 0)
        baseObj1.propWriteValueAutomatic = false;
      str = "";
      string attribute19 = reader.GetAttribute("Attribute");
      if (attribute19 != null && attribute19.Length > 0 && (ConfigurationFlags.IOAttributes & flags) != ConfigurationFlags.None)
      {
        string attribute20 = reader.GetAttribute("Force");
        switch (attribute19.ToLower())
        {
          case "input":
            baseObj1.propAttribute = VariableAttribute.Input;
            if (attribute20 != null && attribute20.Length > 0 && attribute20.ToLower() == "true")
            {
              baseObj1.propForceValue = true;
              break;
            }
            break;
          case "output":
            baseObj1.propAttribute = VariableAttribute.Output;
            if (attribute20 != null && attribute20.Length > 0 && attribute20.ToLower() == "true")
            {
              baseObj1.propForceValue = true;
              break;
            }
            break;
          case "constant":
            baseObj1.propAttribute = VariableAttribute.Constant;
            break;
          case "variable":
            baseObj1.propAttribute = VariableAttribute.Variable;
            break;
          case "none":
            baseObj1.propAttribute = VariableAttribute.None;
            break;
        }
        baseObj1.propWriteValueAutomatic = false;
      }
      if (ConnectionStates.Connected == baseObj1.propConnectionState || ConnectionStates.ConnectedError == baseObj1.propConnectionState)
        baseObj1.Requests |= Actions.Connect;
      if (reader.GetAttribute("ScalingType") != null || reader.GetAttribute("MaxValue") != null || reader.GetAttribute("MinValue") != null || reader.GetAttribute("Factor") != null)
      {
        if (baseObj1.Scaling == null)
          baseObj1.Scaling = new Scaling();
        baseObj1.Scaling.ScalingPoints.Clear();
        baseObj1.Scaling.FromXmlTextReader(ref reader, flags, baseObj1.Scaling);
      }
      else
        reader.Read();
      return 0;
    }

    public void ReadMemberVariables(
      ref XmlTextReader reader,
      ConfigurationFlags flags,
      Variable var)
    {
      if ((flags & ConfigurationFlags.VariableMembers) != ConfigurationFlags.None)
      {
        do
        {
          string attribute = reader.GetAttribute("Name");
          if (attribute != null && attribute.Length > 0)
          {
            Variable variable = new Variable(true, var, attribute, true);
            variable.propAlignment = this.propAlignment;
            this.FromXmlTextReader(ref reader, flags, (Base) variable);
            if (!var.Members.Contains((object) variable))
              var.Members.Add(variable);
          }
        }
        while (reader.Read() && reader.NodeType != XmlNodeType.EndElement);
      }
      else
        reader.Skip();
      reader.Read();
    }

    internal override int ToXMLTextWriter(ref XmlTextWriter writer, ConfigurationFlags flags) => this.SaveVariableConfiguration(ref writer, flags, this);

    private int SaveVariableConfiguration(
      ref XmlTextWriter writer,
      ConfigurationFlags flags,
      Variable var)
    {
      writer.WriteStartElement(nameof (Variable));
      if (var.StructMemberName != null && var.StructMemberName.Length > 0)
      {
        writer.WriteAttributeString("Name", var.StructMemberName);
        if (var.PVRoot.propAddress + "." + var.StructMemberName != var.propAddress)
          writer.WriteAttributeString("Address", var.propAddress);
      }
      else if (var.Name != null && var.Name.Length > 0)
      {
        writer.WriteAttributeString("Name", var.Name);
        if (var.propAddress != null && var.propAddress.Length > 0 && var.Name != var.propAddress)
          writer.WriteAttributeString("Address", var.propAddress);
      }
      if (var.propUserData is string && var.propUserData != null && ((string) var.propUserData).Length > 0)
        writer.WriteAttributeString("UserData", var.propUserData.ToString());
      if (var.propLinkName != null && var.propLinkName.Length > 0)
        writer.WriteAttributeString("LinkName", var.propLinkName);
      writer.WriteAttributeString("Connected", this.propConnectionState.ToString());
      if (var.propPviValue.DataType != DataType.Unknown)
        writer.WriteAttributeString("DataType", Variable.GetDataTypeString(var.propPviValue.DataType));
      if (var.propPviValue.IsOfTypeArray)
        writer.WriteAttributeString("ArraySize", var.propPviValue.ArrayLength.ToString());
      if (var.propPviValue.DataType == DataType.String)
        writer.WriteAttributeString("StringLength", var.propPviValue.TypeLength.ToString());
      if (var.Active && (flags & ConfigurationFlags.ActiveState) != ConfigurationFlags.None)
        writer.WriteAttributeString("Active", "true");
      if (var.Scaling != null)
        var.Scaling.ToXMLTextWriter(ref writer, flags);
      if ((flags & ConfigurationFlags.IOAttributes) != ConfigurationFlags.None)
      {
        if ((var.propAttribute & VariableAttribute.Variable) != VariableAttribute.None)
        {
          if ((var.propAttribute & VariableAttribute.Input) != VariableAttribute.None)
          {
            writer.WriteAttributeString("Attribute", "Input");
            writer.WriteAttributeString("Force", var.ForceValue.ToString());
          }
          else if ((var.propAttribute & VariableAttribute.Output) != VariableAttribute.None)
          {
            writer.WriteAttributeString("Attribute", "Output");
            writer.WriteAttributeString("Force", var.ForceValue.ToString());
          }
        }
        else if ((var.propAttribute & VariableAttribute.Constant) != VariableAttribute.None)
          writer.WriteAttributeString("Attribute", "Constant");
      }
      if (var != var.PVRoot && var.PVRoot.ConnectionType != var.ConnectionType || var == var.PVRoot)
      {
        if ((flags & ConfigurationFlags.Scope) != ConfigurationFlags.None)
        {
          if (var.Scope == Scope.Global)
            writer.WriteAttributeString("Scope", "Global");
          else if (var.Scope == Scope.Local)
            writer.WriteAttributeString("Scope", "Local");
          else if (var.Scope == Scope.Dynamic)
            writer.WriteAttributeString("Scope", "Dynamic");
          else
            writer.WriteAttributeString("Scope", "UNDEFINED");
        }
        if ((ConfigurationFlags.ConnectionState & flags) != ConfigurationFlags.None && var.ConnectionType != ConnectionType.CreateAndLink)
          writer.WriteAttributeString("ConnectionType", var.ConnectionType.ToString());
        if (var.propErrorCode > 0)
          writer.WriteAttributeString("ErrorCode", var.propErrorCode.ToString());
        if (var.RefreshTime != 100 && (flags & ConfigurationFlags.RefreshTime) != ConfigurationFlags.None)
          writer.WriteAttributeString("RefreshTime", var.RefreshTime.ToString());
        if (var.propVariableAccess != Access.ReadAndWrite)
          writer.WriteAttributeString("Access", var.propVariableAccess.ToString());
        if (var.CastMode != CastModes.DEFAULT)
          writer.WriteAttributeString("CastMode", var.CastMode.ToString());
        if (var.StructName != null && var.StructName.Length > 0)
          writer.WriteAttributeString("StructName", var.StructName);
        if (var.propUserTag != null && var.propUserTag.Length > 0)
          writer.WriteAttributeString("UserTag", var.propUserTag);
        if (-1 != var.BitOffset)
          writer.WriteAttributeString("BitOffset", var.BitOffset.ToString());
        if (0.0 != var.Hysteresis)
          writer.WriteAttributeString("Hysteresis", var.Hysteresis.ToString());
        if (var.InitValue != null && var.InitValue.Length > 0)
          writer.WriteAttributeString("InitValue", var.InitValue);
        if (!var.ExpandMembers && (Variable.GetDataTypeString(var.propPviValue.DataType) == DataType.Structure.ToString() || var.propPviValue.propArrayLength > 0))
          writer.WriteAttributeString("ExpandMembers", var.ExpandMembers.ToString());
        if (!var.Polling)
          writer.WriteAttributeString("Polling", var.Polling.ToString());
        if (var.DataValid)
          writer.WriteAttributeString("DataValid", var.DataValid.ToString());
        if (!var.WriteValueAutomatic)
          writer.WriteAttributeString("WriteValueAutomatic", var.WriteValueAutomatic.ToString());
      }
      if ((flags & ConfigurationFlags.VariableMembers) != ConfigurationFlags.None && var.mapNameToMember != null && var.mapNameToMember.Count > 0)
      {
        writer.WriteStartElement("Members");
        for (int index = 0; index < var.mapNameToMember.Count; ++index)
        {
          Variable var1 = (Variable) var.mapNameToMember[index];
          this.SaveVariableConfiguration(ref writer, flags, var1);
        }
        writer.WriteEndElement();
      }
      if (var.Value != (Value) null && var.Value.ToString() != null && var.Value.ToString() != "" && (flags & ConfigurationFlags.Values) != ConfigurationFlags.None && var.Value.propDataType != DataType.Structure && var.Value.propArrayLength < 2)
        writer.WriteAttributeString("Value", var.Value.ToString());
      writer.WriteEndElement();
      return 0;
    }

    public event VariableEventHandler ValueChanged;

    public event PviEventHandler ValueWritten;

    public event PviEventHandler ValueRead;

    public event PviEventHandler ExtendedTypeInfoRead;

    public event PviEventHandler DataValidated;

    public event PviEventHandler Activated;

    public event PviEventHandler Deactivated;

    public event PviEventHandler Uploaded;

    public event PviEventHandler ForcedOn;

    public event PviEventHandler ForcedOff;

    public Variable this[params int[] indices] => this.GetVariable(indices);

    public Variable this[int index] => this.GetVariable(index);

    private Variable GetVariable(params int[] indices)
    {
      if (this.Members == null)
        return (Variable) null;
      string name = this.Name;
      for (int index = 0; index < indices.Length; ++index)
        name = name + "[" + indices.GetValue(index).ToString() + "]";
      return this.Members[name];
    }

    public Variable this[string varName]
    {
      get
      {
        string[] strArray = varName.Split('.');
        if (strArray.Length > 1)
        {
          Variable variable = this;
          for (int index = 0; index < strArray.Length; ++index)
          {
            if (variable != null)
              variable = variable.Members[strArray[index]];
          }
          return variable;
        }
        return this.Members != null ? this.Members[varName] : throw new InvalidOperationException();
      }
    }

    internal Base Owner
    {
      get => this.propOwner;
      set => this.propOwner = value;
    }

    public string OwnerName => this.propOwner != null ? this.propOwner.Address : (string) null;

    public bool ExpandMembers
    {
      get => this.propExpandMembers;
      set => this.propExpandMembers = value;
    }

    public int RefreshTime
    {
      get => this.propRefreshTime;
      set
      {
        this.propRefreshTime = value;
        if (ConnectionStates.Connected == this.propConnectionState)
        {
          Marshal.WriteInt32(this.Service.RequestBuffer, this.propRefreshTime);
          int errorCode = this.WriteRequest(this.Service.hPvi, this.LinkId, AccessTypes.Refresh, this.Service.RequestBuffer, Marshal.SizeOf(typeof (int)), 512U);
          if (errorCode == 0)
            return;
          this.OnError(new PviEventArgs(this.propName, this.propAddress, errorCode, this.Service.Language, Action.VariableSetRefreshTime, this.Service));
        }
        else
        {
          if (!this.Service.WaitForParentConnection)
            return;
          this.Requests |= Actions.SetRefresh;
        }
      }
    }

    public double Hysteresis
    {
      get => this.propHysteresis;
      set
      {
        this.propHysteresis = value;
        if ((ConnectionStates.Connected == this.propConnectionState || ConnectionStates.ConnectedError == this.propConnectionState) && this.ConnectionType == ConnectionType.Link)
        {
          this.Disconnect(2714U);
          this.Connect(ConnectionType.Link, 2712);
        }
        else if (ConnectionStates.Connected == this.propConnectionState)
        {
          string request = value.ToString((IFormatProvider) CultureInfo.InvariantCulture);
          if ((this.propPviValue.DataType == DataType.Single || this.propPviValue.DataType == DataType.Double) && -1 == request.IndexOf('.'))
            request += ".0";
          this.Service.BuildRequestBuffer(request);
          this.propPviValue.propDataSize = request.Length;
          int errorCode = this.WriteRequest(this.Service.hPvi, this.propLinkId, AccessTypes.Hysteresis, this.Service.RequestBuffer, request.Length, 514U);
          if (errorCode == 0)
            return;
          this.OnError(new PviEventArgs(this.propName, this.propAddress, errorCode, this.Service.Language, Action.VariableSetHysteresis, this.Service));
        }
        else
        {
          if (!this.Service.WaitForParentConnection)
            return;
          this.Requests |= Actions.SetHysteresis;
        }
      }
    }

    public override bool IsConnected
    {
      get
      {
        if (base.IsConnected)
          return true;
        return this.propParent is Variable ? this.propParent.IsConnected : base.IsConnected;
      }
    }

    public bool ReadOnly
    {
      get => this.propReadOnly;
      set
      {
        this.propReadOnly = value;
        if (this.propReadOnly)
        {
          this.propVariableAccess |= Access.Read;
          if (Access.Write != (this.propVariableAccess & Access.Write))
            return;
          this.propVariableAccess &= ~Access.Write;
        }
        else
          this.propVariableAccess |= Access.Write;
      }
    }

    public IECDataTypes IECDataType => (Value) null != this.propPviValue ? this.propPviValue.IECDataType : IECDataTypes.UNDEFINED;

    [CLSCompliant(false)]
    public Value Value
    {
      get => this.propExpandMembers ? (this.Convert == null || this.propPviValue.propArrayLength > 1 || this.propPviValue.propObjValue == null ? (this.IsConnected || (Value) null == this.propInternalValue ? this.propPviValue : this.propInternalValue) : (this.IsConnected ? this.Convert.PviValueToValue(this.propPviValue) : this.Convert.PviValueToValue(this.propInternalValue))) : (this.IsConnected || (Value) null == this.propInternalValue ? this.propPviValue : this.propInternalValue);
      set
      {
        int num = 0;
        if (this.propReadOnly)
        {
          this.OnError(new PviEventArgs(this.propName, this.propAddress, 12034, this.Service.Language, Action.VariableConnect, this.Service));
          this.OnValueWritten(new PviEventArgs(this.Name, this.Address, 12034, this.Service.Language, Action.VariableValueWrite, this.Service));
          num = 12034;
        }
        else if (this.IsConnected)
        {
          Value val = value;
          if (this.Convert != null && this.propPviValue.propObjValue != null)
            val = this.Convert.ValueToPviValue(value);
          if (val.propArrayLength > 1)
          {
            if (val.DataType != this.propPviValue.propDataType || val.propArrayLength != this.propPviValue.propArrayLength)
              num = 12034;
            this.ResizePviDataPtr(this.propPviValue.propArrayLength * this.propPviValue.propTypeLength);
            this.ResizePviDataPtr(this.propPviValue.propDataSize);
            if (val.propByteField.Length > this.propPviValue.propDataSize)
              Marshal.Copy(val.propByteField, 0, this.propPviValue.pData, this.propPviValue.propDataSize);
            else
              Marshal.Copy(val.propByteField, 0, this.propPviValue.pData, val.propByteField.Length);
            if (this.WriteValueAutomatic)
            {
              int errorCode = this.WriteValue();
              if (errorCode != 0)
              {
                this.OnError(new PviEventArgs(this.propName, this.propAddress, errorCode, this.Service.Language, Action.VariableValueWrite, this.Service));
                this.OnValueWritten(new PviEventArgs(this.Name, this.Address, errorCode, this.Service.Language, Action.VariableValueWrite, this.Service));
              }
            }
          }
          else
          {
            switch (this.propPviValue.propDataType)
            {
              case DataType.Boolean:
                this.ResizePviDataPtr(1);
                if (val == false || val.ToInt32((IFormatProvider) null) == 0)
                {
                  Marshal.WriteByte(this.propPviValue.pData, (byte) 0);
                  break;
                }
                Marshal.WriteByte(this.propPviValue.pData, (byte) 1);
                break;
              case DataType.SByte:
                if (CastModes.PG2000String == (this.CastMode & CastModes.PG2000String) && this.propPviValue.IsOfTypeArray)
                {
                  this.propPviValue.Assign((object) val.ToString());
                  break;
                }
                this.ResizePviDataPtr(1);
                if ((sbyte) 1 == this.propPviValue.IsEnum && DataType.String == val.DataType)
                {
                  if (this.propPviValue.propEnumerations.Names.Contains((object) val.ToString()))
                  {
                    Marshal.WriteByte(this.propPviValue.pData, (byte) System.Convert.ToSByte(this.propPviValue.propEnumerations.EnumValue(val.ToString()).ToString()));
                    break;
                  }
                  break;
                }
                Marshal.WriteByte(this.propPviValue.pData, (byte) val.ToSByte((IFormatProvider) null));
                break;
              case DataType.Int16:
                this.ResizePviDataPtr(2);
                if ((sbyte) 1 == this.propPviValue.IsEnum && DataType.String == val.DataType)
                {
                  if (this.propPviValue.propEnumerations.Names.Contains((object) val.ToString()))
                  {
                    Marshal.WriteInt16(this.propPviValue.pData, System.Convert.ToInt16(this.propPviValue.propEnumerations.EnumValue(val.ToString()).ToString()));
                    break;
                  }
                  break;
                }
                Marshal.WriteInt16(this.propPviValue.pData, val.ToInt16((IFormatProvider) null));
                break;
              case DataType.Int32:
                this.ResizePviDataPtr(4);
                if ((sbyte) 1 == this.propPviValue.IsEnum && DataType.String == val.DataType)
                {
                  if (this.propPviValue.propEnumerations.Names.Contains((object) val.ToString()))
                  {
                    Marshal.WriteInt32(this.propPviValue.pData, System.Convert.ToInt32(this.propPviValue.propEnumerations.EnumValue(val.ToString()).ToString()));
                    break;
                  }
                  break;
                }
                Marshal.WriteInt32(this.propPviValue.pData, val.ToInt32((IFormatProvider) null));
                break;
              case DataType.Int64:
                this.ResizePviDataPtr(8);
                if ((sbyte) 1 == this.propPviValue.IsEnum && DataType.String == val.DataType)
                {
                  if (this.propPviValue.propEnumerations.Names.Contains((object) val.ToString()))
                  {
                    PviMarshal.WriteInt64(this.propPviValue.pData, System.Convert.ToInt64(this.propPviValue.propEnumerations.EnumValue(val.ToString()).ToString()));
                    break;
                  }
                  break;
                }
                PviMarshal.WriteInt64(this.propPviValue.pData, val.ToInt64((IFormatProvider) null));
                break;
              case DataType.Byte:
              case DataType.UInt8:
                if (CastModes.PG2000String == (this.CastMode & CastModes.PG2000String) && this.propPviValue.IsOfTypeArray)
                {
                  this.propPviValue.Assign((object) val.ToString());
                  break;
                }
                this.ResizePviDataPtr(1);
                if ((sbyte) 1 == this.propPviValue.IsEnum && DataType.String == val.DataType)
                {
                  if (this.propPviValue.propEnumerations.Names.Contains((object) val.ToString()))
                  {
                    Marshal.WriteByte(this.propPviValue.pData, System.Convert.ToByte(this.propPviValue.propEnumerations.EnumValue(val.ToString()).ToString()));
                    break;
                  }
                  break;
                }
                Marshal.WriteByte(this.propPviValue.pData, val.ToByte((IFormatProvider) null));
                break;
              case DataType.UInt16:
              case DataType.WORD:
                this.ResizePviDataPtr(2);
                if ((sbyte) 1 == this.propPviValue.IsEnum && DataType.String == val.DataType)
                {
                  if (this.propPviValue.propEnumerations.Names.Contains((object) val.ToString()))
                  {
                    Marshal.WriteInt16(this.propPviValue.pData, (short) System.Convert.ToUInt16(this.propPviValue.propEnumerations.EnumValue(val.ToString()).ToString()));
                    break;
                  }
                  break;
                }
                Marshal.WriteInt16(this.propPviValue.pData, (short) val.ToUInt16((IFormatProvider) null));
                break;
              case DataType.UInt32:
              case DataType.DWORD:
                this.ResizePviDataPtr(4);
                if ((sbyte) 1 == this.propPviValue.IsEnum && DataType.String == val.DataType)
                {
                  if (this.propPviValue.propEnumerations.Names.Contains((object) val.ToString()))
                  {
                    Marshal.WriteInt32(this.propPviValue.pData, (int) System.Convert.ToUInt32(this.propPviValue.propEnumerations.EnumValue(val.ToString()).ToString()));
                    break;
                  }
                  break;
                }
                Marshal.WriteInt32(this.propPviValue.pData, (int) val.ToUInt32((IFormatProvider) null));
                break;
              case DataType.UInt64:
                this.ResizePviDataPtr(8);
                if ((sbyte) 1 == this.propPviValue.IsEnum && DataType.String == val.DataType)
                {
                  if (this.propPviValue.propEnumerations.Names.Contains((object) val.ToString()))
                  {
                    long uint64 = (long) System.Convert.ToUInt64(this.propPviValue.propEnumerations.EnumValue(val.ToString()).ToString());
                    PviMarshal.WriteInt64(this.propPviValue.pData, (long) val);
                    break;
                  }
                  break;
                }
                PviMarshal.WriteInt64(this.propPviValue.pData, (long) val.ToUInt64((IFormatProvider) null));
                break;
              case DataType.Single:
                this.ResizePviDataPtr(4);
                float[] source1 = new float[1];
                if ((sbyte) 1 == this.propPviValue.IsEnum && DataType.String == val.DataType)
                {
                  if (this.propPviValue.propEnumerations.Names.Contains((object) val.ToString()))
                    source1[0] = System.Convert.ToSingle(this.propPviValue.propEnumerations.EnumValue(val.ToString()).ToString());
                  else
                    break;
                }
                else
                  source1[0] = val.ToSingle((IFormatProvider) null);
                Marshal.Copy(source1, 0, this.propPviValue.pData, 1);
                break;
              case DataType.Double:
                this.ResizePviDataPtr(8);
                double[] source2 = new double[1];
                if ((sbyte) 1 == this.propPviValue.IsEnum && DataType.String == val.DataType)
                {
                  if (this.propPviValue.propEnumerations.Names.Contains((object) val.ToString()))
                    source2[0] = System.Convert.ToDouble(this.propPviValue.propEnumerations.EnumValue(val.ToString()).ToString());
                  else
                    break;
                }
                else
                  source2[0] = val.ToDouble((IFormatProvider) null);
                Marshal.Copy(source2, 0, this.propPviValue.pData, 1);
                break;
              case DataType.TimeSpan:
              case DataType.TimeOfDay:
              case DataType.TOD:
                this.ResizePviDataPtr(4);
                Marshal.WriteInt32(this.propPviValue.pData, Pvi.GetTimeSpanInt32(val.propObjValue));
                break;
              case DataType.DateTime:
              case DataType.Date:
              case DataType.DT:
                this.ResizePviDataPtr(4);
                Marshal.WriteInt32(this.propPviValue.pData, (int) Pvi.GetDateTimeUInt32(val.propObjValue));
                break;
              case DataType.String:
                if (val.propParent == null)
                  val.propParent = this;
                this.ResizePviDataPtr(this.propPviValue.DataSize);
                this.ResetWriteDataPtr(this, this.propPviValue.DataSize, true);
                if (0 < val.ToString().Length)
                {
                  if (val.ToString().Length < this.propPviValue.DataSize)
                  {
                    Marshal.Copy(val.pData, this.propWriteByteField, 0, val.ToString().Length);
                    this.propWriteByteField.SetValue((object) (byte) 0, val.ToString().Length);
                  }
                  else
                    Marshal.Copy(val.pData, this.propWriteByteField, 0, this.propPviValue.DataSize);
                }
                Marshal.Copy(this.propWriteByteField, 0, this.propPviValue.pData, this.propWriteByteField.Length);
                break;
              case DataType.WString:
                if (val.propParent == null)
                  val.propParent = this;
                this.ResizePviDataPtr(this.propPviValue.DataSize);
                this.ResetWriteDataPtr(this, this.propPviValue.DataSize, true);
                if (0 < val.ToString().Length)
                {
                  if (DataType.WString == val.DataType)
                  {
                    if (val.ToStringUNI((string) null, (IFormatProvider) null).Length < this.propPviValue.DataSize)
                    {
                      Marshal.Copy(val.pData, this.propWriteByteField, 0, val.ToStringUNI((string) null, (IFormatProvider) null).Length);
                      this.propWriteByteField.SetValue((object) (byte) 0, val.ToStringUNI((string) null, (IFormatProvider) null).Length);
                    }
                    else
                      Marshal.Copy(val.pData, this.propWriteByteField, 0, this.propPviValue.DataSize);
                  }
                  else
                  {
                    this.propPviValue.Assign((object) val.ToString());
                    Marshal.Copy(this.propPviValue.pData, this.propWriteByteField, 0, this.propPviValue.DataSize);
                    this.propWriteByteField.SetValue((object) (byte) 0, val.ToString().Length * 2);
                    this.propWriteByteField.SetValue((object) (byte) 0, val.ToString().Length * 2 + 1);
                  }
                }
                Marshal.Copy(this.propWriteByteField, 0, this.propPviValue.pData, this.propWriteByteField.Length);
                break;
            }
            if (this.WriteValueAutomatic && !this.isMemberVar)
            {
              int errorCode = this.WriteValue();
              if (0 >= errorCode)
                return;
              this.OnError(new PviEventArgs(this.propName, this.propAddress, errorCode, this.Service.Language, Action.VariableValueWrite, this.Service));
              this.OnValueWritten(new PviEventArgs(this.Name, this.Address, errorCode, this.Service.Language, Action.VariableValueWrite, this.Service));
            }
            else
              this.Value.isAssigned = true;
          }
        }
        else
        {
          if (this.Service.WaitForParentConnection)
            this.Requests |= Actions.SetValue;
          if (this.Convert != null)
            this.propInternalValue = this.Convert.ValueToPviValue(value);
          else
            this.propInternalValue = value;
        }
      }
    }

    private Value InitializeValue(Value baseVal, Value newVal) => newVal;

    [CLSCompliant(false)]
    public Value InitialValue => this.propInitialValue;

    public TypeCode GetMemberDataType(string name)
    {
      if (this.mapNameToMember != null && this.mapNameToMember.ContainsKey((object) name))
        return this.mapNameToMember[name].Value.SystemDataType;
      return this.Value.IsOfTypeArray ? this.Value.SystemDataType : TypeCode.Empty;
    }

    public int GetMemberDataSize(string name)
    {
      if (this.mapNameToMember != null && this.mapNameToMember.ContainsKey((object) name))
        return this.mapNameToMember[name].Value.DataSize;
      return this.Value.IsOfTypeArray ? this.Value.TypeLength : 0;
    }

    public int GetMemberArrayLength(string name)
    {
      if (this.mapNameToMember.ContainsKey((object) name))
        return this.mapNameToMember[name].Value.ArrayLength;
      return this.Value.IsOfTypeArray ? 1 : 0;
    }

    public MemberCollection Members => this.propMembers;

    public StructMemberCollection StructureMembers => this.mapNameToMember;

    public bool ForceValue
    {
      get => this.propForceValue;
      set
      {
        int errorCode = 0;
        if (this.propForceValue == value)
          return;
        this.propForceValue = value;
        if (this.propForceValue)
          this.propPviValue = this.Value;
        else
          errorCode = this.WriteValueForced(false);
        if (errorCode == 0)
          return;
        this.OnError(new PviEventArgs(this.propName, this.propAddress, errorCode, this.Service.Language, Action.VariableValueWrite, this.Service));
      }
    }

    private void SetActive(bool value)
    {
      this.propActive = value;
      if (this.propLinkId != 0U)
      {
        if (!this.propActive)
          this.Deactivate();
        else
          this.Activate();
      }
      else if (this.propActive)
      {
        this.Requests |= Actions.SetActive | Actions.FireActivated;
      }
      else
      {
        if (Actions.FireActivated == (this.Requests & Actions.FireActivated))
          this.Requests &= Actions.Connect | Actions.GetList | Actions.GetValue | Actions.SetValue | Actions.GetForce | Actions.SetForce | Actions.Start | Actions.Stop | Actions.RunCycle | Actions.GetType | Actions.SetType | Actions.GetLength | Actions.SetRefresh | Actions.CreateLink | Actions.Upload | Actions.Download | Actions.SetActive | Actions.SetHysteresis | Actions.Disconnect | Actions.SetInitValue | Actions.GetLBType | Actions.ModuleInfo | Actions.ListSNMPVariables | Actions.Connected | Actions.Uploading | Actions.GetCpuInfo | Actions.ReadPVFormat | Actions.ReadIndex | Actions.FireDataValidated | Actions.FireValueChanged | Actions.Link;
        if (Actions.SetActive != (this.Requests & Actions.SetActive))
          return;
        this.Requests &= Actions.Connect | Actions.GetList | Actions.GetValue | Actions.SetValue | Actions.GetForce | Actions.SetForce | Actions.Start | Actions.Stop | Actions.RunCycle | Actions.GetType | Actions.SetType | Actions.GetLength | Actions.SetRefresh | Actions.CreateLink | Actions.Upload | Actions.Download | Actions.SetHysteresis | Actions.Disconnect | Actions.SetInitValue | Actions.GetLBType | Actions.ModuleInfo | Actions.ListSNMPVariables | Actions.Connected | Actions.Uploading | Actions.GetCpuInfo | Actions.ReadPVFormat | Actions.FireActivated | Actions.ReadIndex | Actions.FireDataValidated | Actions.FireValueChanged | Actions.Link;
      }
    }

    private int Activate()
    {
      int errorCode = 0;
      string eventMaskParameters = this.GetEventMaskParameters(ConnectionType.Link, false);
      if (this.propLinkId != 0U)
      {
        this.Service.BuildRequestBuffer(eventMaskParameters);
        this.Requests |= Actions.FireActivated;
        errorCode = this.WriteRequest(this.Service.hPvi, this.propLinkId, AccessTypes.EventMask, this.Service.RequestBuffer, eventMaskParameters.Length, 503U);
        if (errorCode != 0)
        {
          this.OnError(new PviEventArgs(this.propName, this.propAddress, errorCode, this.Service.Language, Action.VariableActivate, this.Service));
          this.OnActivated(new PviEventArgs(this.propName, this.propAddress, errorCode, this.Service.Language, Action.VariableActivate, this.Service));
        }
      }
      else
        this.Requests |= Actions.SetActive | Actions.FireActivated;
      return errorCode;
    }

    protected void DeactivateInternal()
    {
      Marshal.WriteByte(this.Service.RequestBuffer, (byte) 0);
      this.WriteRequest(this.Service.hPvi, this.propLinkId, AccessTypes.EventMask, this.Service.RequestBuffer, 0, 0U);
    }

    private int Deactivate()
    {
      Marshal.WriteByte(this.Service.RequestBuffer, (byte) 0);
      int errorCode = this.WriteRequest(this.Service.hPvi, this.propLinkId, AccessTypes.EventMask, this.Service.RequestBuffer, 0, 504U);
      if (errorCode != 0)
        this.OnError(new PviEventArgs(this.propName, this.propAddress, errorCode, this.Service.Language, Action.VariableDeactivate, this.Service));
      return errorCode;
    }

    public bool Active
    {
      get => this.propActive;
      set => this.SetActive(value);
    }

    public bool WriteValueAutomatic
    {
      get => this.propWriteValueAutomatic;
      set
      {
        this.propWriteValueAutomatic = value;
        if (!this.propExpandMembers || this.propMembers == null)
          return;
        for (int index = 0; index < this.propMembers.Count; ++index)
          this.propMembers[index].WriteValueAutomatic = this.propWriteValueAutomatic;
      }
    }

    public Scope Scope => this.propScope;

    public bool DataValid => this.propDataValid;

    public Variable.ROIoptions RuntimeObjectIndex
    {
      get => this.propROI;
      set => this.propROI = value;
    }

    public override string FullName
    {
      get
      {
        if (this.Name == null)
          return "";
        if (this.Name.Length > 0)
        {
          if (this.Owner is Variable && (Value) null != ((Variable) this.Owner).Value && ((Variable) this.Owner).Value.IsOfTypeArray)
            return this.Owner.FullName + this.Name;
          if (this.Owner != null)
            return this.Owner.FullName + "." + this.Name;
          return this.propSNMPParent != null ? this.propSNMPParent.FullName + "." + this.Name : this.Name;
        }
        return this.Owner != null ? this.Owner.FullName + "." + this.Name : this.Name;
      }
    }

    public override string PviPathName
    {
      get
      {
        if (this.propOwner is Variable)
          return this.Parent.PviPathName + "/\"" + this.propAddress + "\" OT=Pvar";
        return this.Name != null && 0 < this.Name.Length ? this.Parent.PviPathName + "/\"" + this.propName + "\" OT=Pvar" : this.Parent.PviPathName;
      }
    }

    private string getMemberName()
    {
      if (!(this.propOwner is Variable))
        return "";
      if (((Variable) this.propOwner).Value.IsOfTypeArray)
      {
        string structMemberName = ((Variable) this.propOwner).StructMemberName;
        return 0 < structMemberName.Length && structMemberName.Length - 1 == structMemberName.LastIndexOf(']') ? ((Variable) this.propOwner).StructMemberName + "." + this.Name : ((Variable) this.propOwner).StructMemberName + this.Name;
      }
      return ((Variable) this.propOwner).StructMemberName.Length == 0 ? this.Name : ((Variable) this.propOwner).StructMemberName + "." + this.Name;
    }

    internal virtual string GetStructMemberName(Variable root)
    {
      if (this.Address.IndexOf(root.Address, 0) != 0)
        return this.Address;
      return root.Value.IsOfTypeArray ? this.Address.Substring(root.Address.Length) : this.Address.Substring(root.Address.Length + 1);
    }

    public virtual string StructMemberName => this.getMemberName();

    internal Variable PVRoot
    {
      get
      {
        if (this.propParent == null)
          return (Variable) null;
        return this.propParent is Variable ? ((Variable) this.propParent).PVRoot : this;
      }
    }

    public override Base Parent
    {
      get
      {
        if (this.propParent is Cpu || this.propParent is Task || this.propParent is Service)
          return this.propParent;
        return this.propParent != null ? this.propParent.Parent : (Base) null;
      }
    }

    [CLSCompliant(false)]
    public IConvert Convert
    {
      get => this.propConvert;
      set => this.propConvert = value;
    }

    public string InitValue
    {
      get => this.propInitValue;
      set => this.propInitValue = value;
    }

    public CastModes CastMode
    {
      get => this.propCastMode;
      set => this.propCastMode = value;
    }

    public int BitOffset
    {
      get => this.propBitOffset;
      set => this.propBitOffset = value;
    }

    public string[] ChangedStructMembers => this.propChangedStructMembers;

    public string StructName => this.propStructName;

    internal Cpu Cpu
    {
      get
      {
        if (this.propParent is Cpu)
          return (Cpu) this.propParent;
        if (this.propParent is Task)
          return ((Module) this.propParent).Cpu;
        return this.propParent is Variable ? ((Variable) this.propParent).Cpu : (Cpu) null;
      }
    }

    public int DataAlignment => this.propAlignment;

    public Access Access
    {
      get => this.propVariableAccess;
      set
      {
        if (value == this.propVariableAccess)
          return;
        this.propVariableAccess = value;
        this.propReadOnly = false;
        if (Access.Read == (this.propVariableAccess & Access.Read) && Access.Write != (this.propVariableAccess & Access.Write))
          this.propReadOnly = true;
        this.propPolling = true;
        if (Access.EVENT == (value & Access.EVENT))
          this.propPolling = false;
        if (!this.IsConnected)
          return;
        string attributeParameters = this.GetAttributeParameters();
        string eventMaskParameters = this.GetEventMaskParameters(ConnectionType.Link, false);
        this.Service.BuildRequestBuffer(attributeParameters);
        this.WriteRequest(this.Service.hPvi, this.propLinkId, AccessTypes.Type, this.Service.RequestBuffer, attributeParameters.Length, 551U);
        this.Service.BuildRequestBuffer(eventMaskParameters);
        this.WriteRequest(this.Service.hPvi, this.propLinkId, AccessTypes.EventMask, this.Service.RequestBuffer, eventMaskParameters.Length, 555U);
      }
    }

    public bool Polling
    {
      get => this.propPolling;
      set
      {
        if (this.propPolling == value)
          return;
        this.propPolling = value;
        this.propVariableAccess |= Access.EVENT;
        if (this.propPolling)
          this.propVariableAccess &= ~Access.EVENT;
        if (!this.IsConnected)
          return;
        string attributeParameters = this.GetAttributeParameters();
        this.Service.BuildRequestBuffer(attributeParameters);
        this.WriteRequest(this.Service.hPvi, this.propLinkId, AccessTypes.Type, this.Service.RequestBuffer, attributeParameters.Length, 552U);
      }
    }

    [CLSCompliant(false)]
    public Scaling Scaling
    {
      get => this.propScaling;
      set => this.propScaling = value;
    }

    public IODataPointCollection IODataPoints
    {
      get
      {
        if (this.propIODataPoints == null)
          this.propIODataPoints = new IODataPointCollection((Base) this, this.propAddress);
        return this.propIODataPoints;
      }
    }

    public string UserTag
    {
      get => this.propUserTag;
      set
      {
        IntPtr zero = IntPtr.Zero;
        if (this.propUserTag == value)
          return;
        this.propUserTag = value;
        if (!this.IsConnected)
          return;
        IntPtr hglobal = PviMarshal.StringToHGlobal(this.propUserTag);
        this.WriteRequest(this.Service.hPvi, this.propLinkId, AccessTypes.UserTag, hglobal, this.propUserTag.Length, 554U);
        PviMarshal.FreeHGlobal(ref hglobal);
      }
    }

    [Flags]
    public enum ROIoptions
    {
      OFF = 0,
      NonZeroBasedArrayIndex = 1,
    }
  }
}
