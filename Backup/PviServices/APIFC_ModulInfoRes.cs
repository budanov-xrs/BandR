// Decompiled with JetBrains decompiler
// Type: BR.AN.PviServices.APIFC_ModulInfoRes
// Assembly: BR.AN.PVIServices, Version=1.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DA853F0A-C070-4E30-B7A7-51D955FB85F6
// Assembly location: C:\Users\ss_six\Desktop\FXE\FXE software integration package\COMET_FXE-PVIServices_Example\Pluto-PVI-trial_01\packages\IToolS.BandR.3.5.58\lib\net20\BR.AN.PVIServices.dll

using System.Runtime.InteropServices;

namespace BR.AN.PviServices
{
  [StructLayout(LayoutKind.Sequential, Pack = 1)]
  internal struct APIFC_ModulInfoRes
  {
    public ushort dm_index;
    public byte instP_valid;
    public byte instP_value;
    public DomainState dm_state;
    public ProgramState pi_state;
    public byte pi_count;
    public uint address;
    public uint length;
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 36)]
    public string name;
    public byte version;
    public byte revision;
    public uint erz_time;
    public uint and_time;
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 36)]
    public string erz_name;
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 36)]
    public string and_name;
    public TaskClassType task_class;
    public byte install_no;
    public ushort pv_idx;
    public ushort pv_cnt;
    public uint mem_ana_adr;
    public uint mem_dig_adr;
    public MemoryType mem_location;
    public ModuleType type;
  }
}
