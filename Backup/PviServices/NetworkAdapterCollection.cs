// Decompiled with JetBrains decompiler
// Type: BR.AN.PviServices.NetworkAdapterCollection
// Assembly: BR.AN.PVIServices, Version=1.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DA853F0A-C070-4E30-B7A7-51D955FB85F6
// Assembly location: C:\Users\ss_six\Desktop\FXE\FXE software integration package\COMET_FXE-PVIServices_Example\Pluto-PVI-trial_01\packages\IToolS.BandR.3.5.58\lib\net20\BR.AN.PVIServices.dll

using System;
using System.Collections;

namespace BR.AN.PviServices
{
  public class NetworkAdapterCollection : SNMPCollectionBase
  {
    private Variable propMACAddresses;
    private int propRefreshTime;

    internal NetworkAdapterCollection(string name, SimpleNetworkManagementProtocol parentObj)
      : base(name, (SNMPBase) parentObj)
    {
      int retVal = 0;
      this.propMACAddresses = new Variable(parentObj, "MacAddresses");
      this.propMACAddresses.propVariableAccess = Access.Read;
      this.propMACAddresses.propRefreshTime = 0;
      this.propMACAddresses.UpdateDataFormat("VT=string VL=1024 VN=1", Action.NONE, 0, true, ref retVal);
      this.propMACAddresses.Value.TypePreset = true;
      this.propMACAddresses.propActive = true;
      this.propMACAddresses.ValueChanged += new VariableEventHandler(this.MACAddresses_ValueChanged);
      this.propRefreshTime = 0;
    }

    private void MACAddresses_ValueChanged(object sender, VariableEventArgs e)
    {
      int retVal = 0;
      if (this.Service == null)
        return;
      CollectionErrorEventArgs e1 = new CollectionErrorEventArgs(this.Name, e.ErrorCode, this.Service.Language, Action.SNMPListStations);
      if (e.ErrorCode == 0)
      {
        string str1 = ((Variable) sender).Value.ToString();
        if (str1.Length >= ((Variable) sender).Value.DataSize)
        {
          this.propMACAddresses.Value.TypePreset = true;
          this.propMACAddresses.propPviValue.propTypeLength = 1024 + ((Variable) sender).Value.DataSize;
          this.propMACAddresses.UpdateDataFormat("VT=string VL=" + this.propMACAddresses.propPviValue.propTypeLength.ToString() + " VN=1", Action.VariableInternFormat, 0, false, true, ref retVal);
          return;
        }
        string str2 = str1.Trim();
        Hashtable hashtable = new Hashtable();
        if (0 < str2.Length)
        {
          string[] strArray = str2.Split('\t');
          for (int index = 0; index < strArray.Length; ++index)
          {
            string str3 = strArray.GetValue(index).ToString();
            if (!this.ContainsKey(str3))
            {
              hashtable.Add((object) str3, (object) str3);
              NetworkAdapter networkAdapter = new NetworkAdapter(str3, this.Parent);
              e1.NewItems.Add((object) str3);
              this.Add(str3, (object) networkAdapter);
            }
            else
              this[str3].propState = SNMPConnectionStates.Connected;
          }
        }
        if (0 < this.Count)
        {
          foreach (NetworkAdapter networkAdapter in (IEnumerable) this.Values)
          {
            if (!hashtable.ContainsKey((object) networkAdapter.Name))
            {
              networkAdapter.propState = SNMPConnectionStates.Unpluged;
              e1.ChangedItems.Add((object) networkAdapter.Name);
            }
          }
        }
      }
      this.OnChanged(e1);
    }

    internal override void OnPviRead(
      int errorCode,
      PVIReadAccessTypes accessType,
      PVIDataStates dataState,
      IntPtr pData,
      uint dataLen,
      int option)
    {
      if (accessType == PVIReadAccessTypes.SNMPListLocalVariables)
      {
        if (errorCode == 0)
          return;
        this.OnSearchCompleted(new ErrorEventArgs(this.Name, "", errorCode, this.Service.Language, Action.SNMPListLocalVariables));
      }
      else
        base.OnPviRead(errorCode, accessType, dataState, pData, dataLen, option);
    }

    public int RefreshTime
    {
      get => this.propRefreshTime;
      set
      {
        this.propRefreshTime = value;
        if (this.propParent == null || this.Service == null)
          return;
        if (value != 0 && 2 * ((SimpleNetworkManagementProtocol) this.propParent).ResponseTimeout > value)
          this.propRefreshTime = 2 * ((SimpleNetworkManagementProtocol) this.propParent).ResponseTimeout;
        if (ConnectionStates.Connected == this.propMACAddresses.propConnectionState)
        {
          this.propMACAddresses.RefreshTime = this.propRefreshTime;
        }
        else
        {
          this.propMACAddresses.propRefreshTime = this.propRefreshTime;
          if (0 >= this.propRefreshTime)
            return;
          if (!this.Service.SNMP.propIsConnected)
            this.Service.SNMP.Connect();
          this.propMACAddresses.Connect();
        }
      }
    }

    public NetworkAdapter this[string indexer] => (NetworkAdapter) this.propItems[(object) indexer];

    public int Search()
    {
      int errorCode = 0;
      if (this.propRequesting)
        return -1;
      this.propRequestCount = 0;
      if (0 < this.Count)
      {
        foreach (NetworkAdapter networkAdapter in (IEnumerable) this.Values)
        {
          ++this.propRequestCount;
          networkAdapter.SearchCompleted += new ErrorEventHandler(this.Adapter_SearchCompleted);
          errorCode = networkAdapter.Search();
          if (errorCode != 0)
          {
            --this.propRequestCount;
            networkAdapter.SearchCompleted -= new ErrorEventHandler(this.Adapter_SearchCompleted);
            this.OnError(new ErrorEventArgs(this.Name, "", errorCode, this.Service.Language, Action.SNMPListLocalVariables, networkAdapter.MACAddress));
            errorCode = 0;
          }
        }
      }
      if (this.propRequestCount == 0)
        this.OnSearchCompleted(new ErrorEventArgs(this.Name, "", 0, this.Service.Language, Action.SNMPListLocalVariables));
      return errorCode;
    }

    private void Adapter_SearchCompleted(object sender, ErrorEventArgs e)
    {
      ((SNMPBase) sender).SearchCompleted -= new ErrorEventHandler(this.Adapter_SearchCompleted);
      --this.propRequestCount;
      if (e.ErrorCode != 0)
        this.OnError(new ErrorEventArgs(this.Name, "", e.ErrorCode, this.Service.Language, e.Action, ((NetworkAdapter) sender).MACAddress));
      if (this.propRequestCount != 0)
        return;
      this.propRequesting = false;
      this.OnSearchCompleted(new ErrorEventArgs(this.Name, "", e.ErrorCode, this.Service.Language, e.Action));
    }

    public override void Cleanup()
    {
      if (0 < this.Count)
      {
        foreach (SNMPBase snmpBase in (IEnumerable) this.Values)
          snmpBase.Cleanup();
      }
      base.Cleanup();
    }
  }
}
