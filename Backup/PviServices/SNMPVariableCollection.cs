// Decompiled with JetBrains decompiler
// Type: BR.AN.PviServices.SNMPVariableCollection
// Assembly: BR.AN.PVIServices, Version=1.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DA853F0A-C070-4E30-B7A7-51D955FB85F6
// Assembly location: C:\Users\ss_six\Desktop\FXE\FXE software integration package\COMET_FXE-PVIServices_Example\Pluto-PVI-trial_01\packages\IToolS.BandR.3.5.58\lib\net20\BR.AN.PVIServices.dll

using System;
using System.Collections;
using System.Collections.Generic;

namespace BR.AN.PviServices
{
  public class SNMPVariableCollection : SNMPCollectionBase
  {
    private int propWriteErrors;
    private int propReadErrors;
    private bool propFilteredVariableList;

    internal SNMPVariableCollection(string name, SNMPBase parentObj)
      : base(name, parentObj)
    {
      this.propWriteErrors = 0;
      this.propReadErrors = 0;
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

    public Variable this[string indexer] => (Variable) this.propItems[(object) indexer];

    public override int Disconnect() => this.Disconnect(true);

    public override int Disconnect(bool synchronous)
    {
      if (0 < this.Count)
      {
        foreach (Variable variable in (IEnumerable) this.Values)
        {
          variable.CancelRequest(true);
          variable.Unlink();
        }
      }
      return 0;
    }

    public override void Cleanup()
    {
      if (0 < this.Count)
      {
        foreach (Variable variable in (IEnumerable) this.Values)
        {
          variable.CancelRequest(true);
          variable.Unlink();
          variable.Remove();
          variable.Dispose();
        }
      }
      base.Cleanup();
    }

    public int ReadFiltered(List<string> filter)
    {
      this.propFilteredVariableList = true;
      return this.ReadInternal(filter);
    }

    private int ReadInternal(List<string> filter)
    {
      int errorCode = 0;
      if (this.propRequesting)
        return -1;
      this.propReadErrors = 0;
      this.propRequestCount = 0;
      if (0 < this.Count)
      {
        foreach (Variable variable in (IEnumerable) this.Values)
        {
          if (filter == null || filter.Contains(variable.Name))
          {
            ++this.propRequestCount;
            variable.ValueRead += new PviEventHandler(this.tmpVar_ValueRead);
            if (variable.IsConnected)
            {
              errorCode = variable.ReadValueEx();
              if (errorCode != 0)
              {
                variable.ValueRead -= new PviEventHandler(this.tmpVar_ValueRead);
                --this.propRequestCount;
                ++this.propReadErrors;
                this.OnError(new ErrorEventArgs(this.Name, "", errorCode, this.Service.Language, Action.VariableRead, variable.Name));
              }
            }
            else
            {
              variable.Requests |= Actions.GetValue;
              variable.Connected += new PviEventHandler(this.tmpVar_Connected);
              variable.Connect();
            }
          }
        }
      }
      if (this.propRequestCount == 0)
      {
        this.propRequesting = false;
        this.OnValuesRead(new ErrorEventArgs(this.Name, "", this.propReadErrors, this.Service.Language, Action.VariablesValuesRead));
      }
      return errorCode;
    }

    public int Read()
    {
      this.propFilteredVariableList = false;
      return this.ReadInternal((List<string>) null);
    }

    private void tmpVar_Connected(object sender, PviEventArgs e) => ((Base) sender).Connected -= new PviEventHandler(this.tmpVar_Connected);

    private void tmpVar_ValueRead(object sender, PviEventArgs e)
    {
      ((Variable) sender).ValueRead -= new PviEventHandler(this.tmpVar_ValueRead);
      --this.propRequestCount;
      if (e.ErrorCode != 0)
      {
        ++this.propReadErrors;
        this.OnError(new ErrorEventArgs(this.Name, "", e.ErrorCode, this.Service.Language, e.Action, ((Base) sender).Name));
      }
      if (this.propRequestCount != 0)
        return;
      this.propRequesting = false;
      this.OnValuesRead(new ErrorEventArgs(this.Name, "", this.propReadErrors, this.Service.Language, Action.VariablesValuesRead));
    }

    public int Write()
    {
      int errorCode = 0;
      if (this.propRequesting)
        return -1;
      this.propWriteErrors = 0;
      this.propRequestCount = 0;
      if (0 < this.Count)
      {
        foreach (Variable variable in (IEnumerable) this.Values)
        {
          ++this.propRequestCount;
          variable.ValueWritten += new PviEventHandler(this.tmpVar_ValueWritten);
          errorCode = variable.WriteValue();
          if (errorCode != 0)
          {
            variable.ValueWritten -= new PviEventHandler(this.tmpVar_ValueWritten);
            --this.propRequestCount;
            ++this.propWriteErrors;
            this.OnError(new ErrorEventArgs(this.Name, "", errorCode, this.Service.Language, Action.VariableWrite, variable.Name));
          }
        }
      }
      if (this.propRequestCount == 0)
        this.OnValuesWritten(new ErrorEventArgs(this.Name, "", this.propWriteErrors, this.Service.Language, Action.VariablesValuesWrite));
      return errorCode;
    }

    private void tmpVar_ValueWritten(object sender, PviEventArgs e)
    {
      ((Variable) sender).ValueWritten -= new PviEventHandler(this.tmpVar_ValueWritten);
      --this.propRequestCount;
      if (e.ErrorCode != 0)
      {
        ++this.propWriteErrors;
        this.OnError(new ErrorEventArgs(this.Name, "", e.ErrorCode, this.Service.Language, e.Action));
      }
      if (this.propRequestCount != 0)
        return;
      this.propRequesting = false;
      this.OnValuesWritten(new ErrorEventArgs(this.Name, "", this.propWriteErrors, this.Service.Language, Action.VariablesValuesWrite));
    }

    public event ErrorEventHandler ValuesRead;

    public event ErrorEventHandler ValuesReadFiltered;

    protected virtual void OnValuesRead(ErrorEventArgs e)
    {
      this.propRequesting = false;
      if (this.propFilteredVariableList)
      {
        if (this.ValuesReadFiltered == null)
          return;
        this.ValuesReadFiltered((object) this, e);
      }
      else
      {
        if (this.ValuesRead == null)
          return;
        this.ValuesRead((object) this, e);
      }
    }

    public event ErrorEventHandler ValuesWritten;

    protected virtual void OnValuesWritten(ErrorEventArgs e)
    {
      this.propRequesting = false;
      if (this.ValuesWritten == null)
        return;
      this.ValuesWritten((object) this, e);
    }
  }
}
