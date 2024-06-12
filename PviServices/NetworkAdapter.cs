// Decompiled with JetBrains decompiler
// Type: BR.AN.PviServices.NetworkAdapter
// Assembly: BR.AN.PVIServices, Version=1.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DA853F0A-C070-4E30-B7A7-51D955FB85F6
// Assembly location: C:\Users\ss_six\Desktop\FXE\FXE software integration package\COMET_FXE-PVIServices_Example\Pluto-PVI-trial_01\packages\IToolS.BandR.3.5.58\lib\net20\BR.AN.PVIServices.dll

using System;

namespace BR.AN.PviServices
{
  public class NetworkAdapter : SNMPBase
  {
    private SNMPVariableCollection propVariables;
    internal SNMPConnectionStates propState;
    private string propMACAddress;

    internal NetworkAdapter(string macAdr, SNMPBase parentObj)
      : base(macAdr, parentObj)
    {
      this.propState = SNMPConnectionStates.Connected;
      this.propMACAddress = macAdr;
      this.propVariables = new SNMPVariableCollection("SNMPLocal_" + this.Name, (SNMPBase) this);
    }

    internal override void OnPviRead(
      int errorCode,
      PVIReadAccessTypes accessType,
      PVIDataStates dataState,
      IntPtr pData,
      uint dataLen,
      int option)
    {
      if (accessType == PVIReadAccessTypes.Variables)
      {
        if (errorCode == 0)
          this.OnLocalVariablesUpdate(pData, dataLen);
        else
          this.OnSearchCompleted(new ErrorEventArgs(this.Name, "", errorCode, this.Service.Language, Action.SNMPListLocalVariables));
      }
      else
        base.OnPviRead(errorCode, accessType, dataState, pData, dataLen, option);
    }

    private void OnLocalVariablesUpdate(IntPtr pData, uint dataLen)
    {
      if (this.propVariables != null)
      {
        this.propVariables.Cleanup();
        this.propVariables.InitConnect((SNMPBase) this);
      }
      if (this.Service != null)
      {
        string stringAnsi = PviMarshal.PtrToStringAnsi(pData, dataLen);
        string str = "";
        for (int index = 0; (long) index < (long) dataLen; ++index)
        {
          if ('\t' != stringAnsi[index] && stringAnsi[index] != char.MinValue)
          {
            str += (string) (object) stringAnsi[index];
          }
          else
          {
            Variable variable = new Variable(this, str);
            this.propVariables.Add(str, (object) variable);
            str = "";
          }
        }
        this.OnSearchCompleted(new ErrorEventArgs(this.Name, "", 0, this.Service.Language, Action.SNMPListLocalVariables));
      }
      else
        this.OnSearchCompleted(new ErrorEventArgs(this.Name, "", 4808, "en", Action.SNMPListLocalVariables));
    }

    public SNMPVariableCollection Variables => this.propVariables;

    public SNMPConnectionStates State => this.propState;

    public string MACAddress => this.propMACAddress;

    public int Search()
    {
      int num = -1;
      this.propVariables.InitConnect((SNMPBase) this);
      if (this.propParent != null)
      {
        string objDesc = "CD=\"" + this.propParent.Name + "\"/\"/CN=" + this.MACAddress + "\"";
        if (!this.propIsConnected)
          num = this.ConnectPviObject(true, this.propMACAddress, objDesc, "", ObjectType.POBJ_STATION, 1403, out this.propLinkID);
        if (num == 0 || 12002 == num)
          num = this.GetSNMPVariables((int) this.propLinkID, 1402);
      }
      else
        num = -2;
      return num;
    }

    public override void Cleanup()
    {
      if (this.propVariables != null)
        this.propVariables.Cleanup();
      if (this.Service != null)
        PInvokePvicom.PviComUnlinkRequest(this.Service.hPvi, this.propLinkID, (PviCallback) null, 0U, 0U);
      base.Cleanup();
    }

    public override int Disconnect(bool synchronous)
    {
      if (!synchronous)
        return base.Disconnect(synchronous);
      this.propIsConnected = false;
      return this.UnlinkPviObject(this.propLinkID);
    }
  }
}
