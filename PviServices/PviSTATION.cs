// Decompiled with JetBrains decompiler
// Type: BR.AN.PviServices.PviSTATION
// Assembly: BR.AN.PVIServices, Version=1.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DA853F0A-C070-4E30-B7A7-51D955FB85F6
// Assembly location: C:\Users\ss_six\Desktop\FXE\FXE software integration package\COMET_FXE-PVIServices_Example\Pluto-PVI-trial_01\packages\IToolS.BandR.3.5.58\lib\net20\BR.AN.PVIServices.dll

namespace BR.AN.PviServices
{
  internal class PviSTATION : Base
  {
    public PviSTATION(PviDEVICE device, string name)
      : base((Base) device, name)
    {
    }

    public override string FullName
    {
      get
      {
        if (this.Name != null && 0 < this.Name.Length)
          return this.Parent.FullName + "." + this.Name;
        return this.Parent != null ? this.Parent.FullName : "";
      }
    }

    public override string PviPathName => this.Name != null && 0 < this.Name.Length ? this.Parent.PviPathName + "/" + this.Name : this.Parent.PviPathName;

    public void SetName(string newName) => this.propName = newName;

    public void Initialize(string parentName, string objParameters)
    {
      if (LogicalObjectsUsage.ObjectNameWithType == this.Service.LogicalObjectsUsage)
        this.propObjectParam = "CD=" + objParameters;
      else
        this.propObjectParam = "CD=\"" + parentName + "\"/" + objParameters;
    }

    public int PviConnect()
    {
      string pObjName = this.AddressEx;
      if (this.Service != null && LogicalObjectsUsage.ObjectNameWithType == this.Service.LogicalObjectsUsage)
        pObjName = this.PviPathName;
      return this.XCreateRequest(this.Service.hPvi, pObjName, ObjectType.POBJ_STATION, this.ObjectParam, 2808U, "", 2806U);
    }

    public int PviDisconnect()
    {
      this.propConnectionState = ConnectionStates.Disconnecting;
      int errorCode;
      if (this.LinkId != 0U)
      {
        errorCode = this.UnlinkRequest(2807U);
        if (errorCode != 0)
          this.OnError(new PviEventArgs(this.Name, this.Address, errorCode, this.Service.Language, Action.CpuDisconnect, this.Service));
        this.propLinkId = 0U;
      }
      else
      {
        this.propConnectionState = ConnectionStates.Disconnected;
        errorCode = 4808;
      }
      return errorCode;
    }

    internal int TurnOffEvents()
    {
      string request = "";
      this.Service.BuildRequestBuffer(request);
      return this.Write(this.Service.hPvi, this.LinkId, AccessTypes.EventMask, this.Service.RequestBuffer, request.Length);
    }

    internal int TurnOnEvents()
    {
      string request = "eds";
      this.Service.BuildRequestBuffer(request);
      return this.WriteRequest(this.Service.hPvi, this.LinkId, AccessTypes.EventMask, this.Service.RequestBuffer, request.Length, this.propInternID);
    }
  }
}
