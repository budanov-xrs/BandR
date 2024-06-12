// Decompiled with JetBrains decompiler
// Type: BR.AN.PviServices.ModuleInfoDecription
// Assembly: BR.AN.PVIServices, Version=1.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DA853F0A-C070-4E30-B7A7-51D955FB85F6
// Assembly location: C:\Users\ss_six\Desktop\FXE\FXE software integration package\COMET_FXE-PVIServices_Example\Pluto-PVI-trial_01\packages\IToolS.BandR.3.5.58\lib\net20\BR.AN.PVIServices.dll

using System;
using System.Xml;

namespace BR.AN.PviServices
{
  internal class ModuleInfoDecription
  {
    internal ushort dm_index;
    internal ushort pi_index;
    internal byte instP_valid;
    internal byte instP_value;
    internal DomainState dm_state;
    internal ProgramState pi_state;
    internal byte pi_count;
    internal uint address;
    internal uint length;
    internal string name;
    internal byte version;
    internal byte revision;
    internal uint erz_time;
    internal uint and_time;
    internal string erz_name;
    internal string and_name;
    internal TaskClassType task_class;
    internal byte install_no;
    internal ushort pv_idx;
    internal ushort pv_cnt;
    internal uint mem_ana_adr;
    internal uint mem_dig_adr;
    internal MemoryType mem_location;
    internal ModuleType type;
    internal uint createTime;
    internal uint andTime;
    internal uint progInvocation;
    internal int modListed;

    internal ModuleInfoDecription() => this.Initialize();

    internal void Initialize()
    {
      this.dm_index = (ushort) 0;
      this.pi_index = (ushort) 0;
      this.instP_valid = (byte) 0;
      this.instP_value = (byte) 0;
      this.dm_state = DomainState.NonExistent;
      this.pi_state = ProgramState.NonExistent;
      this.pi_count = (byte) 0;
      this.address = 0U;
      this.length = 0U;
      this.name = "";
      this.version = (byte) 0;
      this.revision = (byte) 0;
      this.erz_time = 0U;
      this.and_time = 0U;
      this.erz_name = "";
      this.and_name = "";
      this.task_class = TaskClassType.NotValid;
      this.install_no = (byte) 0;
      this.pv_idx = (ushort) 0;
      this.pv_cnt = (ushort) 0;
      this.mem_ana_adr = 0U;
      this.mem_dig_adr = 0U;
      this.mem_location = MemoryType.SystemRom;
      this.type = ModuleType.Unknown;
      this.createTime = 0U;
      this.andTime = 0U;
      this.progInvocation = 0U;
    }

    internal void Init(APIFC_ModulInfoRes apifcInfo)
    {
      this.Initialize();
      this.dm_index = apifcInfo.dm_index;
      this.instP_valid = apifcInfo.instP_valid;
      this.instP_value = apifcInfo.instP_value;
      this.dm_state = apifcInfo.dm_state;
      this.pi_state = apifcInfo.pi_state;
      this.pi_count = apifcInfo.pi_count;
      this.address = apifcInfo.address;
      this.length = apifcInfo.length;
      this.name = apifcInfo.name;
      this.version = apifcInfo.version;
      this.revision = apifcInfo.revision;
      this.erz_time = apifcInfo.erz_time;
      this.and_time = apifcInfo.and_time;
      this.erz_name = apifcInfo.erz_name;
      this.and_name = apifcInfo.and_name;
      this.task_class = apifcInfo.task_class;
      this.install_no = apifcInfo.install_no;
      this.pv_idx = apifcInfo.pv_idx;
      this.pv_cnt = apifcInfo.pv_cnt;
      this.mem_ana_adr = apifcInfo.mem_ana_adr;
      this.mem_dig_adr = apifcInfo.mem_dig_adr;
      this.mem_location = apifcInfo.mem_location;
      this.type = apifcInfo.type;
      this.createTime = apifcInfo.erz_time;
      this.andTime = apifcInfo.and_time;
    }

    internal void UpdateAPIFCModulInfoRes(ref APIFC_ModulInfoRes apifcInfo)
    {
      apifcInfo.dm_index = this.dm_index;
      apifcInfo.instP_valid = this.instP_valid;
      apifcInfo.instP_value = this.instP_value;
      apifcInfo.dm_state = this.dm_state;
      apifcInfo.pi_state = this.pi_state;
      apifcInfo.pi_count = this.pi_count;
      apifcInfo.address = this.address;
      apifcInfo.length = this.length;
      apifcInfo.name = this.name;
      apifcInfo.version = this.version;
      apifcInfo.revision = this.revision;
      apifcInfo.erz_time = this.erz_time;
      apifcInfo.and_time = this.and_time;
      apifcInfo.erz_name = this.erz_name;
      apifcInfo.and_name = this.and_name;
      apifcInfo.task_class = this.task_class;
      apifcInfo.install_no = this.install_no;
      apifcInfo.pv_idx = this.pv_idx;
      apifcInfo.pv_cnt = this.pv_cnt;
      apifcInfo.mem_ana_adr = this.mem_ana_adr;
      apifcInfo.mem_dig_adr = this.mem_dig_adr;
      apifcInfo.mem_location = this.mem_location;
      apifcInfo.type = this.type;
      apifcInfo.erz_time = this.createTime;
      apifcInfo.and_time = this.andTime;
    }

    internal int ReadFromXML(XmlTextReader xmlTReader)
    {
      int num1 = 0;
      this.Initialize();
      try
      {
        string attribute1 = xmlTReader.GetAttribute("Name");
        if (attribute1 != null && 0 < attribute1.Length)
          this.name = attribute1;
        string attribute2 = xmlTReader.GetAttribute("Listed");
        this.modListed = attribute2 == null || 0 >= attribute2.Length ? 3 : System.Convert.ToInt32(attribute2);
        string attribute3 = xmlTReader.GetAttribute("Size");
        if (attribute3 != null && 0 < attribute3.Length)
          this.length = -1 != attribute3.IndexOf('x') || -1 != attribute3.IndexOf('X') ? System.Convert.ToUInt32(attribute3, 16) : System.Convert.ToUInt32(attribute3);
        string attribute4 = xmlTReader.GetAttribute("MemType");
        if (attribute4 != null && 0 < attribute4.Length)
          this.mem_location = -1 != attribute4.IndexOf('x') || -1 != attribute4.IndexOf('X') ? (MemoryType) System.Convert.ToUInt32(attribute4, 16) : (MemoryType) System.Convert.ToUInt32(attribute4);
        string attribute5 = xmlTReader.GetAttribute("Version");
        if (attribute5 != null && 0 < attribute5.Length)
          this.version = -1 != attribute5.IndexOf('x') || -1 != attribute5.IndexOf('X') ? System.Convert.ToByte(attribute5, 16) : System.Convert.ToByte(attribute5);
        string attribute6 = xmlTReader.GetAttribute("Revision");
        if (attribute6 != null && 0 < attribute6.Length)
          this.revision = -1 != attribute6.IndexOf('x') || -1 != attribute6.IndexOf('X') ? System.Convert.ToByte(attribute6, 16) : System.Convert.ToByte(attribute6);
        string attribute7 = xmlTReader.GetAttribute("ModulType");
        if (attribute7 != null && 0 < attribute7.Length)
          this.type = -1 != attribute7.IndexOf('x') || -1 != attribute7.IndexOf('X') ? (ModuleType) System.Convert.ToUInt32(attribute7, 16) : (ModuleType) System.Convert.ToUInt32(attribute7);
        string attribute8 = xmlTReader.GetAttribute("TaskClass");
        if (attribute8 != null && 0 < attribute8.Length)
          this.task_class = -1 != attribute8.IndexOf('x') || -1 != attribute8.IndexOf('X') ? (TaskClassType) System.Convert.ToUInt32(attribute8, 16) : (TaskClassType) System.Convert.ToUInt32(attribute8);
        string attribute9 = xmlTReader.GetAttribute("InstallNo");
        if (attribute9 != null && 0 < attribute9.Length)
          this.install_no = -1 != attribute9.IndexOf('x') || -1 != attribute9.IndexOf('X') ? System.Convert.ToByte(attribute9, 16) : System.Convert.ToByte(attribute9);
        string attribute10 = xmlTReader.GetAttribute("ModulState");
        if (attribute10 != null && 0 < attribute10.Length)
          this.pi_state = -1 != attribute10.IndexOf('x') || -1 != attribute10.IndexOf('X') ? (ProgramState) System.Convert.ToUInt32(attribute10, 16) : (ProgramState) System.Convert.ToUInt32(attribute10);
        string attribute11 = xmlTReader.GetAttribute("DomainOvIndex");
        if (attribute11 != null && 0 < attribute11.Length)
          this.dm_index = -1 != attribute11.IndexOf('x') || -1 != attribute11.IndexOf('X') ? System.Convert.ToUInt16(attribute11, 16) : System.Convert.ToUInt16(attribute11);
        string attribute12 = xmlTReader.GetAttribute("InvocationOvIndex");
        if (attribute12 != null && 0 < attribute12.Length)
          this.pi_index = -1 != attribute12.IndexOf('x') || -1 != attribute12.IndexOf('X') ? System.Convert.ToUInt16(attribute12, 16) : System.Convert.ToUInt16(attribute12);
        string attribute13 = xmlTReader.GetAttribute("DomainModulState");
        if (attribute13 != null && 0 < attribute13.Length)
          this.dm_state = -1 != attribute13.IndexOf('x') || -1 != attribute13.IndexOf('X') ? (DomainState) System.Convert.ToUInt32(attribute13, 16) : (DomainState) System.Convert.ToUInt32(attribute13);
        string attribute14 = xmlTReader.GetAttribute("ProgInvocation");
        if (attribute14 != null && 0 < attribute14.Length)
          this.progInvocation = -1 != attribute14.IndexOf('x') || -1 != attribute14.IndexOf('X') ? System.Convert.ToUInt32(attribute14, 16) : System.Convert.ToUInt32(attribute14);
        string attribute15 = xmlTReader.GetAttribute("Time");
        if (attribute15 != null && 0 < attribute15.Length)
        {
          if (-1 == attribute15.IndexOf('-') && -1 == attribute15.IndexOf('.'))
          {
            this.and_time = System.Convert.ToUInt32(attribute15);
            this.createTime = this.erz_time = this.and_time;
          }
          else
          {
            string str1 = attribute15.Replace('-', '/');
            int num2 = str1.IndexOf('/', 0);
            int num3 = str1.IndexOf('/', num2 + 1);
            int length = str1.IndexOf('/', num3 + 1);
            string str2 = str1.Substring(length + 1);
            this.and_time = Pvi.DateTimeToUInt32(DateTime.Parse(str1.Substring(0, length) + " " + str2.Replace('/', ':')));
            this.createTime = this.erz_time = this.and_time;
          }
        }
        string attribute16 = xmlTReader.GetAttribute("RawTimeErzT5");
        if (attribute16 != null && 0 < attribute16.Length && -1 == attribute16.IndexOf('x') && -1 == attribute16.IndexOf('X'))
          this.erz_time = this.createTime = PviMarshal.TimeToUInt32(attribute16);
        string attribute17 = xmlTReader.GetAttribute("RawTimeAenT5");
        if (attribute17 != null && 0 < attribute17.Length && -1 == attribute17.IndexOf('x') && -1 == attribute17.IndexOf('X'))
          this.and_time = this.andTime = PviMarshal.TimeToUInt32(attribute17);
        string attribute18 = xmlTReader.GetAttribute("Address");
        if (attribute18 != null)
        {
          if (0 < attribute18.Length)
            this.address = -1 != attribute18.IndexOf('x') || -1 != attribute18.IndexOf('X') ? System.Convert.ToUInt32(attribute18, 16) : System.Convert.ToUInt32(attribute18);
        }
      }
      catch
      {
        num1 = 12054;
      }
      return num1;
    }
  }
}
