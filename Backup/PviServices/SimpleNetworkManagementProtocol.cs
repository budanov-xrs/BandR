// Decompiled with JetBrains decompiler
// Type: BR.AN.PviServices.SimpleNetworkManagementProtocol
// Assembly: BR.AN.PVIServices, Version=1.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DA853F0A-C070-4E30-B7A7-51D955FB85F6
// Assembly location: C:\Users\ss_six\Desktop\FXE\FXE software integration package\COMET_FXE-PVIServices_Example\Pluto-PVI-trial_01\packages\IToolS.BandR.3.5.58\lib\net20\BR.AN.PVIServices.dll

using System;
using System.Collections;
using System.Xml;

namespace BR.AN.PviServices
{
  public class SimpleNetworkManagementProtocol : SNMPBase
  {
    private uint propLineLinkID;
    private string propLineName;
    private string propDeviceName;
    private int propResponseTimeout;
    private NetworkAdapterCollection propNetworkAdapters;
    private SNMPVariableCollection propVariables;

    internal SimpleNetworkManagementProtocol(Service serviceObj)
      : base("snmp", serviceObj)
    {
      this.propLineName = "LnSNMP";
      this.propDeviceName = "snmp";
      this.propResponseTimeout = 1000;
      this.propNetworkAdapters = new NetworkAdapterCollection(nameof (NetworkAdapters), this);
      this.propVariables = new SNMPVariableCollection("SNMPGlobal_" + this.Name, (SNMPBase) this);
    }

    internal override int ToXMLTextWriter(ref XmlTextWriter writer, ConfigurationFlags flags)
    {
      writer.WriteStartElement("SNMP");
      writer.WriteAttributeString("LINE", this.propLineName);
      writer.WriteAttributeString("DEVICE", this.propDeviceName);
      writer.WriteAttributeString("ResponseTimeout", this.propResponseTimeout.ToString());
      writer.WriteEndElement();
      return 0;
    }

    public override int FromXmlTextReader(ref XmlTextReader reader, ConfigurationFlags flags)
    {
      string str = "";
      string attribute1 = reader.GetAttribute("LINE");
      if (attribute1 != null && attribute1.Length > 0)
        this.propLineName = attribute1;
      str = "";
      string attribute2 = reader.GetAttribute("DEVICE");
      if (attribute2 != null && attribute2.Length > 0)
        this.propDeviceName = attribute2;
      str = "";
      string attribute3 = reader.GetAttribute("ResponseTimeout");
      if (attribute3 != null && attribute3.Length > 0)
      {
        int result = 0;
        if (PviParse.TryParseInt32(attribute3, out result))
          this.propResponseTimeout = result;
      }
      return 0;
    }

    internal override void OnPviRead(
      int errorCode,
      PVIReadAccessTypes accessType,
      PVIDataStates dataState,
      IntPtr pData,
      uint dataLen,
      int option)
    {
      switch (accessType)
      {
        case PVIReadAccessTypes.ChildTypes:
          if (errorCode != 0)
          {
            this.OnSearchCompleted(new ErrorEventArgs(this.Name, "", errorCode, this.Service.Language, Action.SNMPListStations));
            break;
          }
          this.OnStationsListUpdate(pData, dataLen);
          break;
        case PVIReadAccessTypes.Variables:
          if (errorCode != 0)
          {
            this.OnSearchCompleted(new ErrorEventArgs(this.Name, "", errorCode, this.Service.Language, Action.SNMPListGlobalVariables));
            break;
          }
          this.OnGlobalVariablesUpdate(pData, dataLen);
          break;
        default:
          base.OnPviRead(errorCode, accessType, dataState, pData, dataLen, option);
          break;
      }
    }

    private void OnStationsListUpdate(IntPtr pData, uint dataLen)
    {
      if (this.propNetworkAdapters != null)
        this.propNetworkAdapters.InitConnect((SNMPBase) this);
      Hashtable hashtable = new Hashtable();
      string stringAnsi = PviMarshal.PtrToStringAnsi(pData, dataLen);
      string str1 = "";
      for (int index = 0; (long) index < (long) dataLen; ++index)
      {
        if ('\t' != stringAnsi[index] && stringAnsi[index] != char.MinValue)
        {
          str1 += (string) (object) stringAnsi[index];
        }
        else
        {
          int num = str1.IndexOf("OT=Station");
          if (-1 != num)
          {
            string str2 = str1.Substring(0, num - 1);
            if (!hashtable.ContainsKey((object) str2))
              hashtable.Add((object) str2, (object) str2);
            if (!this.propNetworkAdapters.ContainsKey(str2))
            {
              NetworkAdapter networkAdapter = new NetworkAdapter(str2, (SNMPBase) this);
              this.propNetworkAdapters.Add(str2, (object) networkAdapter);
            }
            else
              this.propNetworkAdapters[str2].propState = SNMPConnectionStates.Connected;
          }
          str1 = "";
        }
      }
      if (this.propNetworkAdapters != null && 0 < this.propNetworkAdapters.Count)
      {
        foreach (NetworkAdapter networkAdapter in (IEnumerable) this.propNetworkAdapters.Values)
        {
          if (!hashtable.ContainsKey((object) networkAdapter.Name))
            networkAdapter.propState = SNMPConnectionStates.Unpluged;
        }
      }
      if (Actions.ListSNMPVariables == (this.propRequestQueue & Actions.ListSNMPVariables))
      {
        this.propRequestQueue ^= Actions.ListSNMPVariables;
        int snmpVariables = this.GetSNMPVariables((int) this.propLinkID, 1401);
        if (snmpVariables == 0)
          return;
        this.OnSearchCompleted(new ErrorEventArgs(this.Name, "", snmpVariables, this.Service.Language, Action.SNMPListGlobalVariables, "global SNMP variables"));
      }
      else
        this.OnSearchCompleted(new ErrorEventArgs(this.Name, "", 0, this.Service.Language, Action.SNMPListGlobalVariables));
    }

    private void OnGlobalVariablesUpdate(IntPtr pData, uint dataLen)
    {
      if (this.propVariables != null)
      {
        this.propVariables.Cleanup();
        this.propVariables.InitConnect((SNMPBase) this);
      }
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
          if (str.CompareTo("MacAddresses") == 0)
            variable.RefreshTime = 20 * this.propResponseTimeout;
          this.propVariables.Add(str, (object) variable);
          str = "";
        }
      }
      this.OnSearchCompleted(new ErrorEventArgs(this.Name, "", 0, this.Service.Language, Action.SNMPListGlobalVariables));
    }

    [PviKeyWord("/RT")]
    public int ResponseTimeout
    {
      get => this.propResponseTimeout;
      set
      {
        int propResponseTimeout = this.propResponseTimeout;
        this.propResponseTimeout = value;
      }
    }

    public NetworkAdapterCollection NetworkAdapters => this.propNetworkAdapters;

    public SNMPVariableCollection Variables => this.propVariables;

    public int Search()
    {
      int num;
      if (this.propIsConnected)
      {
        num = this.GetMACStations();
        if (num == 0)
          this.propRequestQueue |= Actions.ListSNMPVariables;
      }
      else
      {
        num = this.ConnectPviObjects();
        if (num == 0 || 12002 == num)
        {
          this.propIsConnected = true;
          num = this.GetMACStations();
          if (num == 0)
            this.propRequestQueue |= Actions.ListSNMPVariables;
        }
      }
      return num;
    }

    internal override int Connect()
    {
      this.propNetworkAdapters.InitConnect((SNMPBase) this);
      this.propVariables.InitConnect((SNMPBase) this);
      int num = this.ConnectPviObjects();
      if (num == 0 || 12002 == num)
        this.propIsConnected = true;
      return num;
    }

    private int ConnectPviObjects()
    {
      int num = this.ConnectPviObject(true, this.propLineName, "CD=\"Pvi\"/\"" + this.propLineName + "\"", "", ObjectType.POBJ_LINE, 1403, out this.propLineLinkID);
      if (num == 0 || 12002 == num)
        num = this.ConnectPviObject(true, this.propDeviceName, string.Format("CD=\"{0}\"/\"/IF={1} /RT={2}\"", (object) this.propLineName, (object) this.propDeviceName, (object) this.propResponseTimeout), "", ObjectType.POBJ_DEVICE, 1404, out this.propLinkID);
      return num;
    }

    private int GetMACStations() => this.Service.EventMessageType != EventMessageType.CallBack ? PInvokePvicom.PviComMsgReadRequest(this.Service.hPvi, this.propLinkID, AccessTypes.ListExtern, this.Service.WindowHandle, 1400U, this.propServiceArrayIndex) : PInvokePvicom.PviComReadRequest(this.Service.hPvi, this.propLinkID, AccessTypes.ListExtern, this.Service.cbRead, 4294967294U, this.propServiceArrayIndex);

    public override void Cleanup()
    {
      if (this.propVariables != null)
        this.propVariables.Cleanup();
      if (this.propNetworkAdapters != null)
        this.propNetworkAdapters.Cleanup();
      this.CancelRequest();
      this.UnlinkPviObject(this.propLineLinkID);
      this.propIsConnected = false;
      base.Cleanup();
    }

    public override string ToString() => "LINE=\"" + this.propLineName + "\" DEVICE=\"" + this.propDeviceName + "\" ResponseTimeout=\"" + this.propResponseTimeout.ToString() + "\"";
  }
}
