// Decompiled with JetBrains decompiler
// Type: BR.AN.PviServices.Service
// Assembly: BR.AN.PVIServices, Version=1.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DA853F0A-C070-4E30-B7A7-51D955FB85F6
// Assembly location: C:\Users\ss_six\Desktop\FXE\FXE software integration package\COMET_FXE-PVIServices_Example\Pluto-PVI-trial_01\packages\IToolS.BandR.3.5.58\lib\net20\BR.AN.PVIServices.dll

using System;
using System.Collections;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Resources;
using System.Runtime.InteropServices;
using System.Text;
using System.Xml;

namespace BR.AN.PviServices
{
  public class Service : Base
  {
    private const string COREDLL = "version.dll";
    internal uint propPendingObjectBrowserEvents;
    private bool propConnectionInterupted;
    private uint propModuleUIDBuilder;
    private Utilities propUtils;
    private SimpleNetworkManagementProtocol propSNMP;
    private PviObjectBrowser propServicePviObj;
    private ArrayList propClientNames;
    private LicenceInfo propLicenseInfo;
    private int propRetryTime;
    private int propProcessTimeout;
    private int propMessageLimitation;
    private int propPVIAutoStart;
    private IntPtr propWindowHandle;
    private bool propAddMembersToVariableCollection;
    private bool propExtendedTypeInfoAutoUpdateForVariables;
    private bool propAddStructMembersToMembersToo;
    private bool propUserTagEvents;
    internal PviCallback callback;
    internal PviCallback cbCreate;
    internal PviCallback cbLink;
    internal PviCallback cbLinkA;
    internal PviCallback cbLinkB;
    internal PviCallback cbLinkC;
    internal PviCallback cbLinkD;
    internal PviCallback cbLinkE;
    internal PviCallback cbEvent;
    internal PviCallback cbEventA;
    internal PviCallback cbRead;
    internal PviCallback cbReadA;
    internal PviCallback cbReadB;
    internal PviCallback cbReadC;
    internal PviCallback cbReadD;
    internal PviCallback cbReadE;
    internal PviCallback cbReadF;
    internal PviCallback cbReadG;
    internal PviCallback cbReadS;
    internal PviCallback cbReadT;
    internal PviCallback cbWrite;
    internal PviCallback cbWriteA;
    internal PviCallback cbWriteB;
    internal PviCallback cbWriteC;
    internal PviCallback cbWriteD;
    internal PviCallback cbWriteE;
    internal PviCallback cbWriteF;
    internal PviCallback cbUnlink;
    internal PviCallback cbUnlinkA;
    internal PviCallback cbUnlinkB;
    internal PviCallback cbUnlinkC;
    internal PviCallback cbUnlinkD;
    internal PviCallback cbDelete;
    internal PviCallback cbChangeLink;
    internal PviCallback cbSNMP;
    internal IntPtr RequestBuffer;
    private char[] propEventChars;
    internal uint hPvi;
    private ServiceCollection propServices;
    private int propTimeout;
    private string propLanguage;
    private bool propErrorexception;
    private bool propErrorevent;
    private bool isStatic;
    private string propServer;
    private int propPort;
    private CpuCollection propCpus;
    private CollectionType propCollectionType;
    private LogicalCollection propLogicalObjects;
    private Cpu propCpu;
    private LogicalObjectsUsage propLogicalUsage;
    private EventMessageType propEventMsgType;
    private VariableCollection propVariables;
    internal PlcMessageWindow propMSGWindow;
    internal Hashtable InternIDs;
    internal uint ActID;
    internal Version propUserTagMinVersion;
    private bool propWaitForParentConnection;
    internal LoggerEntryCollection propLoggerEntries;
    private ArrayList propLoggerCollections;
    internal uint EntryIDInc;
    internal uint EntryIDDec = uint.MaxValue;
    private IntPtr iptr2Byte;
    private IntPtr iptr4Byte;
    private IntPtr marshStrPtr;
    private IntPtr iptr8Byte;
    private float[] marshF32;
    private double[] marshF64;
    private long uint64Val;
    private byte[] byteBuffer;

    internal Utilities Utilities => this.propUtils;

    public SimpleNetworkManagementProtocol SNMP => this.propSNMP;

    public TraceWriter Trace => this.Services.Trace;

    [DllImport("version.dll", SetLastError = true)]
    private static extern int GetFileVersionInfoSize(string lptstrFilename, out IntPtr lpdwHandle);

    [DllImport("version.dll", SetLastError = true)]
    private static extern bool GetFileVersionInfo(
      string lptstrFilename,
      IntPtr dwHandle,
      int dwLen,
      IntPtr lpData);

    [DllImport("version.dll", SetLastError = true)]
    private static extern bool VerQueryValue(
      IntPtr pBlock,
      string lpSubBlock,
      out IntPtr lplpBuffer,
      out int puLen);

    private int GetNativeDLLVersions(ref string productVersion, ref string fileVersion)
    {
      int nativeDllVersions = 0;
      string str = Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetName().CodeBase) + "\\" + Assembly.GetExecutingAssembly().GetName().Name + ".DLL";
      if (-1 != str.IndexOf("file:\\"))
        str = str.Substring(6);
      if (!File.Exists(str))
        return 2;
      IntPtr lpdwHandle;
      int fileVersionInfoSize = Service.GetFileVersionInfoSize(str, out lpdwHandle);
      IntPtr num = Marshal.AllocHGlobal(fileVersionInfoSize);
      try
      {
        if (!Service.GetFileVersionInfo(str, lpdwHandle, fileVersionInfoSize, num))
          return 3;
        IntPtr lplpBuffer;
        Service.VerQueryValue(num, "\\", out lplpBuffer, out int _);
        Service.VS_FIXEDFILEINFO structure = (Service.VS_FIXEDFILEINFO) Marshal.PtrToStructure(lplpBuffer, typeof (Service.VS_FIXEDFILEINFO));
        Version version1 = new Version((int) structure.dwFileVersionMS >> 16, (int) structure.dwFileVersionMS & (int) ushort.MaxValue, (int) structure.dwFileVersionLS >> 16, (int) structure.dwFileVersionLS & (int) ushort.MaxValue);
        Version version2 = new Version((int) structure.dwProductVersionMS >> 16, (int) structure.dwProductVersionMS & (int) ushort.MaxValue, (int) structure.dwFileVersionLS >> 16);
        fileVersion = version1.ToString();
        productVersion = version2.ToString();
      }
      finally
      {
        Marshal.FreeHGlobal(num);
      }
      return nativeDllVersions;
    }

    public int GetAssemblyVersions(ref string productVersion, ref string fileVersion)
    {
      FileVersionInfo versionInfo = FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location);
      productVersion = versionInfo.ProductVersion;
      fileVersion = versionInfo.FileVersion;
      return this.GetNativeDLLVersions(ref productVersion, ref fileVersion);
    }

    internal Service()
    {
      this.InitEmpty();
      this.InitBuffers();
      this.InternIDs = new Hashtable(1000);
      this.propUserTagMinVersion = new Version("2.50");
      this.propUserTagEvents = true;
    }

    public Service(EventMessageType commMethod, string name)
      : base(name)
    {
      this.Init(commMethod, name, (ServiceCollection) null);
    }

    public Service(EventMessageType commMethod, string name, ServiceCollection services)
      : base(name)
    {
      this.Init(commMethod, name, services);
    }

    public Service(string name)
      : base(name)
    {
      this.Init(EventMessageType.CallBack, name, (ServiceCollection) null);
    }

    public Service(string name, ServiceCollection services)
      : base(name)
    {
      this.Init(EventMessageType.CallBack, name, services);
    }

    internal void InitEmpty()
    {
      this.propAddStructMembersToMembersToo = false;
      this.propEventMsgType = EventMessageType.CallBack;
      this.propExtendedTypeInfoAutoUpdateForVariables = false;
      this.propAddMembersToVariableCollection = false;
      this.ActID = 0U;
      this.hPvi = 0U;
      this.propUtils = (Utilities) null;
      this.propClientNames = (ArrayList) null;
      this.propWindowHandle = IntPtr.Zero;
      this.propLicenseInfo = new LicenceInfo(this);
      this.propConnectionInterupted = false;
      this.propErrorexception = false;
      this.propErrorevent = true;
      this.propTimeout = 0;
      this.propRetryTime = 0;
      this.propProcessTimeout = 0;
      this.propMessageLimitation = 1;
      this.propPVIAutoStart = 1;
      this.propLanguage = "en-us";
      this.propWaitForParentConnection = true;
      this.callback = (PviCallback) null;
      this.cbCreate = (PviCallback) null;
      this.cbLink = (PviCallback) null;
      this.cbLinkA = (PviCallback) null;
      this.cbLinkB = (PviCallback) null;
      this.cbLinkC = (PviCallback) null;
      this.cbLinkD = (PviCallback) null;
      this.cbLinkE = (PviCallback) null;
      this.cbEvent = (PviCallback) null;
      this.cbEventA = (PviCallback) null;
      this.cbRead = (PviCallback) null;
      this.cbReadA = (PviCallback) null;
      this.cbReadB = (PviCallback) null;
      this.cbReadC = (PviCallback) null;
      this.cbReadD = (PviCallback) null;
      this.cbReadE = (PviCallback) null;
      this.cbReadF = (PviCallback) null;
      this.cbReadG = (PviCallback) null;
      this.cbReadS = (PviCallback) null;
      this.cbReadT = (PviCallback) null;
      this.cbWrite = (PviCallback) null;
      this.cbWriteA = (PviCallback) null;
      this.cbWriteB = (PviCallback) null;
      this.cbWriteC = (PviCallback) null;
      this.cbWriteD = (PviCallback) null;
      this.cbWriteE = (PviCallback) null;
      this.cbWriteF = (PviCallback) null;
      this.cbUnlink = (PviCallback) null;
      this.cbUnlinkA = (PviCallback) null;
      this.cbUnlinkB = (PviCallback) null;
      this.cbUnlinkC = (PviCallback) null;
      this.cbUnlinkD = (PviCallback) null;
      this.cbDelete = (PviCallback) null;
      this.cbChangeLink = (PviCallback) null;
      this.cbSNMP = (PviCallback) null;
      this.propMSGWindow = (PlcMessageWindow) null;
      this.RequestBuffer = IntPtr.Zero;
      this.propEventChars = (char[]) null;
      this.byteBuffer = (byte[]) null;
      this.iptr2Byte = IntPtr.Zero;
      this.iptr4Byte = IntPtr.Zero;
      this.iptr8Byte = IntPtr.Zero;
      this.marshStrPtr = IntPtr.Zero;
      this.marshF32 = (float[]) null;
      this.marshF64 = (double[]) null;
      this.propWindowHandle = IntPtr.Zero;
      this.propCollectionType = CollectionType.HashTable;
      this.propCpus = (CpuCollection) null;
      this.propVariables = (VariableCollection) null;
      this.propCpu = (Cpu) null;
      this.propLoggerEntries = (LoggerEntryCollection) null;
      this.propLoggerCollections = (ArrayList) null;
      this.propServices = (ServiceCollection) null;
      this.LogicalObjectsUsage = LogicalObjectsUsage.FullName;
      this.propLogicalObjects = (LogicalCollection) null;
      this.propSNMP = (SimpleNetworkManagementProtocol) null;
    }

    internal void InitBuffers()
    {
      this.RequestBuffer = PviMarshal.AllocHGlobal(256);
      this.propEventChars = new char[256];
      this.byteBuffer = new byte[8];
      this.iptr2Byte = PviMarshal.AllocHGlobal(2);
      this.iptr4Byte = PviMarshal.AllocHGlobal(4);
      this.iptr8Byte = PviMarshal.AllocHGlobal(8);
      this.marshStrPtr = IntPtr.Zero;
      this.marshF32 = new float[1];
      this.marshF64 = new double[1];
    }

    internal void Init(EventMessageType commMethod, string name, ServiceCollection services)
    {
      this.InitEmpty();
      this.InternIDs = new Hashtable(1000);
      this.propUserTagMinVersion = new Version("2.50");
      this.propUserTagEvents = true;
      this.propService = this;
      this.propModuleUIDBuilder = 1U;
      this.propEventMsgType = commMethod;
      ServiceCollection.Services.Add((object) this.Name, (object) this);
      this.propUtils = new Utilities();
      this.propUtils.ActiveCulture = this.propLanguage;
      this.InitBuffers();
      this.propClientNames = new ArrayList(1);
      this.propLicenseInfo = new LicenceInfo(this);
      this.propUtils.ActiveCulture = this.propLanguage;
      this.propMSGWindow = new PlcMessageWindow(this);
      this.callback = new PviCallback(this.Callback);
      this.cbCreate = new PviCallback(this.PVICB_Create);
      this.cbLink = new PviCallback(this.PVICB_Link);
      this.cbLinkA = new PviCallback(this.PVICB_LinkA);
      this.cbLinkB = new PviCallback(this.PVICB_LinkB);
      this.cbLinkC = new PviCallback(this.PVICB_LinkC);
      this.cbLinkD = new PviCallback(this.PVICB_LinkD);
      this.cbLinkE = new PviCallback(this.PVICB_LinkE);
      this.cbEvent = new PviCallback(this.PVICB_Event);
      this.cbEventA = new PviCallback(this.PVICB_EventA);
      this.cbRead = new PviCallback(this.PVICB_Read);
      this.cbReadA = new PviCallback(this.PVICB_ReadA);
      this.cbReadB = new PviCallback(this.PVICB_ReadB);
      this.cbReadC = new PviCallback(this.PVICB_ReadC);
      this.cbReadD = new PviCallback(this.PVICB_ReadD);
      this.cbReadE = new PviCallback(this.PVICB_ReadE);
      this.cbReadF = new PviCallback(this.PVICB_ReadF);
      this.cbReadG = new PviCallback(this.PVICB_ReadG);
      this.cbReadS = new PviCallback(this.PVICB_ReadS);
      this.cbReadT = new PviCallback(this.PVICB_ReadT);
      this.cbWrite = new PviCallback(this.PVICB_Write);
      this.cbWriteA = new PviCallback(this.PVICB_WriteA);
      this.cbWriteB = new PviCallback(this.PVICB_WriteB);
      this.cbWriteC = new PviCallback(this.PVICB_WriteC);
      this.cbWriteD = new PviCallback(this.PVICB_WriteD);
      this.cbWriteE = new PviCallback(this.PVICB_WriteE);
      this.cbWriteF = new PviCallback(this.PVICB_WriteF);
      this.cbUnlink = new PviCallback(this.PVICB_Unlink);
      this.cbUnlinkA = new PviCallback(this.PVICB_UnlinkA);
      this.cbUnlinkB = new PviCallback(this.PVICB_UnlinkB);
      this.cbUnlinkC = new PviCallback(this.PVICB_UnlinkC);
      this.cbUnlinkD = new PviCallback(this.PVICB_UnlinkD);
      this.cbDelete = new PviCallback(this.PVICB_Delete);
      this.cbChangeLink = new PviCallback(this.PVICB_ChangeLink);
      this.cbSNMP = new PviCallback(this.PVICB_SNMP);
      this.propMSGWindow.CreateControl();
      this.propWindowHandle = this.propMSGWindow.Handle;
      this.propCollectionType = CollectionType.HashTable;
      this.propCpus = new CpuCollection(CollectionType.HashTable, (object) this, "");
      this.propCpus.propInternalCollection = true;
      this.propVariables = new VariableCollection(CollectionType.HashTable, (object) this, this.Name + ".Variables");
      this.propCpu = (Cpu) null;
      this.propLoggerEntries = (LoggerEntryCollection) new ServiceLoggerEntryCollection((Base) this, this.Name + ".LogEntries");
      this.propLoggerCollections = new ArrayList(1);
      this.propServices = services;
      if (this.propServices != null)
      {
        this.propServices.Add(this);
        this.LogicalObjectsUsage = this.propServices.LogicalObjectsUsage;
      }
      else
        this.LogicalObjectsUsage = LogicalObjectsUsage.FullName;
      this.propLogicalObjects = new LogicalCollection((object) this, this.Name + ".Logicals");
      this.propSNMP = new SimpleNetworkManagementProtocol(this);
    }

    internal bool ContainsIDKey(uint internID)
    {
      if (this.InternIDs != null)
        this.InternIDs.ContainsKey((object) internID);
      return false;
    }

    internal bool AddID(object objTarget, ref uint internID)
    {
      if (this.InternIDs == null)
        return false;
      if (this.ActID == uint.MaxValue)
      {
        uint key = 1;
        while (this.InternIDs.ContainsKey((object) key))
          ++key;
        internID = key;
        this.InternIDs.Add((object) key, objTarget);
        return true;
      }
      internID = ++this.ActID;
      this.InternIDs.Add((object) internID, objTarget);
      return true;
    }

    internal void RemoveID(uint internID)
    {
      if (this.InternIDs == null)
        return;
      this.InternIDs.Remove((object) internID);
    }

    internal int Initialize(
      ref uint pviHandle,
      int ComTimeout,
      int RetryTimeMessage,
      string pInitParam,
      IntPtr pRes2)
    {
      byte[] bytes = new StringMarshal().GetBytes(pInitParam);
      return this.EventMessageType != EventMessageType.CallBack ? PInvokePvicom.PviComMsgInitialize(out pviHandle, ComTimeout, RetryTimeMessage, bytes, pRes2) : PInvokePvicom.PviComInitialize(out pviHandle, ComTimeout, RetryTimeMessage, pInitParam, pRes2);
    }

    internal void BuildRequestBuffer(string request)
    {
      this.propEventChars = request.ToCharArray();
      for (int index = 0; index < this.propEventChars.GetLength(0); ++index)
        Marshal.WriteByte(this.Service.RequestBuffer, index, (byte) (char) this.propEventChars.GetValue(index));
    }

    public string GetErrorText(int errNo) => this.propUtils.GetErrorText(errNo);

    internal int SetGlobEventMsg(
      uint pviHandle,
      uint globalEvents,
      uint eventMsgNo,
      uint eventParam)
    {
      return this.EventMessageType != EventMessageType.CallBack ? PInvokePvicom.PviComMsgSetGlobEventMsg(pviHandle, globalEvents, this.WindowHandle, eventMsgNo, eventParam) : PInvokePvicom.PviComSetGlobEventMsg(pviHandle, globalEvents, this.callback, eventMsgNo, eventParam);
    }

    internal static int Deinitialize(uint hPvi) => 8 != IntPtr.Size ? PInvokePvicom.PviXDeinitialize(hPvi) : PInvokePvicom.Pvi64XDeinitialize(hPvi);

    public virtual void Connect(string server, int port)
    {
      this.propServer = server;
      this.propPort = port;
      this.Connect();
    }

    private void DisposeMSGWnd(int mode)
    {
      if (this.propMSGWindow.InvokeRequired)
      {
        this.propMSGWindow.Invoke((Delegate) new Service.DisposeMSGWindowCB(this.DisposeMSGWnd), (object) mode);
      }
      else
      {
        this.propMSGWindow.Dispose();
        this.propMSGWindow = (PlcMessageWindow) null;
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
      if (disposing)
      {
        this.propParent = propParent;
        this.propLinkName = propLinkName;
        this.propLogicalName = propLogicalName;
        this.propUserData = propUserData;
        this.propName = propName;
        this.propAddress = propAddress;
        if (removeFromCollection && this.Services != null)
          this.Services.Remove(this.Name);
        this.byteBuffer = (byte[]) null;
        if (this.propCpus != null)
        {
          this.propCpus.Dispose(disposing, removeFromCollection);
          this.propCpus = (CpuCollection) null;
        }
        if (IntPtr.Zero != this.iptr2Byte)
        {
          PviMarshal.FreeHGlobal(ref this.iptr2Byte);
          this.iptr2Byte = IntPtr.Zero;
        }
        if (IntPtr.Zero != this.iptr4Byte)
        {
          PviMarshal.FreeHGlobal(ref this.iptr4Byte);
          this.iptr4Byte = IntPtr.Zero;
        }
        if (IntPtr.Zero != this.iptr8Byte)
        {
          PviMarshal.FreeHGlobal(ref this.iptr8Byte);
          this.iptr8Byte = IntPtr.Zero;
        }
        this.propLanguage = (string) null;
        if (this.propLoggerCollections != null)
        {
          this.propLoggerCollections.Clear();
          this.propLoggerCollections = (ArrayList) null;
        }
        if (this.propLoggerEntries != null)
        {
          this.propLoggerEntries.Dispose(disposing, removeFromCollection);
          this.propLoggerEntries = (LoggerEntryCollection) null;
        }
        if (this.propVariables != null)
        {
          this.propVariables.Dispose(disposing, removeFromCollection);
          this.propVariables = (VariableCollection) null;
        }
        if (this.propServicePviObj != null)
          this.propServicePviObj = (PviObjectBrowser) null;
        if (this.propSNMP != null)
        {
          this.propSNMP.Cleanup();
          this.propSNMP = (SimpleNetworkManagementProtocol) null;
        }
        if (this.propUtils != null)
        {
          this.propUtils.Dispose(disposing);
          this.propUtils = (Utilities) null;
        }
        if (this.propMSGWindow != null)
          this.DisposeMSGWnd(0);
        if (this.propLicenseInfo != null)
        {
          this.propLicenseInfo.Dispose(disposing);
          this.propLicenseInfo = (LicenceInfo) null;
        }
        if (this.propLogicalObjects != null)
        {
          this.propLogicalObjects.Dispose(disposing, removeFromCollection);
          this.propLogicalObjects = (LogicalCollection) null;
        }
        if (this.marshF32 != null)
          this.marshF32 = (float[]) null;
        if (this.marshF64 != null)
          this.marshF64 = (double[]) null;
        if (IntPtr.Zero != this.marshStrPtr)
        {
          PviMarshal.FreeHGlobal(ref this.marshStrPtr);
          this.marshStrPtr = IntPtr.Zero;
        }
        PviMarshal.FreeHGlobal(ref this.marshStrPtr);
        if (this.propClientNames != null)
        {
          this.propClientNames.Clear();
          this.propClientNames = (ArrayList) null;
        }
        this.propCpu = (Cpu) null;
        this.propParent = (Base) null;
        this.propUserTagMinVersion = (Version) null;
        this.propServer = (string) null;
        this.propServices = (ServiceCollection) null;
        this.propWindowHandle = IntPtr.Zero;
        this.propEventChars = (char[]) null;
        if (IntPtr.Zero != this.RequestBuffer)
        {
          PviMarshal.FreeHGlobal(ref this.RequestBuffer);
          this.RequestBuffer = IntPtr.Zero;
        }
      }
      this.callback = new PviCallback(this.Callback);
      this.cbCreate = (PviCallback) null;
      this.cbLink = (PviCallback) null;
      this.cbLinkA = (PviCallback) null;
      this.cbLinkB = (PviCallback) null;
      this.cbLinkC = (PviCallback) null;
      this.cbLinkD = (PviCallback) null;
      this.cbLinkE = (PviCallback) null;
      this.cbEvent = (PviCallback) null;
      this.cbEventA = (PviCallback) null;
      this.cbRead = (PviCallback) null;
      this.cbReadA = (PviCallback) null;
      this.cbReadB = (PviCallback) null;
      this.cbReadC = (PviCallback) null;
      this.cbReadD = (PviCallback) null;
      this.cbReadE = (PviCallback) null;
      this.cbReadF = (PviCallback) null;
      this.cbReadG = (PviCallback) null;
      this.cbReadS = (PviCallback) null;
      this.cbReadT = (PviCallback) null;
      this.cbWrite = (PviCallback) null;
      this.cbWriteA = (PviCallback) null;
      this.cbWriteB = (PviCallback) null;
      this.cbWriteC = (PviCallback) null;
      this.cbWriteD = (PviCallback) null;
      this.cbWriteE = (PviCallback) null;
      this.cbWriteF = (PviCallback) null;
      this.cbUnlink = (PviCallback) null;
      this.cbUnlinkA = (PviCallback) null;
      this.cbUnlinkB = (PviCallback) null;
      this.cbUnlinkC = (PviCallback) null;
      this.cbUnlinkD = (PviCallback) null;
      this.cbDelete = (PviCallback) null;
      this.cbChangeLink = (PviCallback) null;
      this.cbSNMP = (PviCallback) null;
      Service.Deinitialize(this.hPvi);
      this.propParent = (Base) null;
      this.propLinkName = (string) null;
      this.propLogicalName = (string) null;
      this.propUserData = (object) null;
      this.propName = (string) null;
      this.propAddress = (string) null;
      this.byteBuffer = (byte[]) null;
      this.callback = (PviCallback) null;
      this.propErrorText = (string) null;
      this.propEventChars = (char[]) null;
      if (this.InternIDs != null)
        this.InternIDs.Clear();
      this.InternIDs = (Hashtable) null;
      this.propLanguage = (string) null;
      this.propLinkName = (string) null;
      this.propLinkParam = (string) null;
      this.propLogicalName = (string) null;
      this.LicencInfoUpdated = (PviEventHandler) null;
      this.PVIObjectsAttached = (PVIObjectsAttachedEventHandler) null;
      if (this.propMSGWindow != null)
        this.DisposeMSGWnd(0);
      this.propSNMP = (SimpleNetworkManagementProtocol) null;
      this.propServer = (string) null;
      this.propUserData = (object) null;
      this.propUserTagMinVersion = (Version) null;
    }

    public override void Connect() => this.Connect(ConnectionType.CreateAndLink);

    public override void Connect(ConnectionType connectionType)
    {
      string str1 = "";
      string str2 = "";
      if (ConnectionStates.Connecting == this.propConnectionState)
        return;
      this.propConnectionState = ConnectionStates.Connecting;
      if (this.IsConnected)
      {
        this.propErrorCode = this.propReturnValue = 12002;
        this.OnError(new PviEventArgs(this.propName, this.propAddress, 12002, this.Service.Language, Action.ServiceConnect, this));
      }
      else
      {
        string str3 = "LM=" + this.propMessageLimitation.ToString();
        if (1 != this.propPVIAutoStart)
          str2 = " AS=" + this.propPVIAutoStart.ToString();
        if (0 < this.propProcessTimeout)
          str1 = " PT=" + this.propProcessTimeout.ToString();
        string pInitParam;
        if (this.Server != null && this.Server.Length > 0)
          pInitParam = string.Format("{0} IP={1} PN={2}{3}{4}", (object) str3, (object) this.Server, (object) this.Port, (object) str1, (object) str2);
        else
          pInitParam = string.Format("{0}{1}{2}", (object) str3, (object) str1, (object) str2);
        StringBuilder s = new StringBuilder(256);
        int num = PInvokePvicom.GetActiveWindow();
        for (int parent = PInvokePvicom.GetParent(num); parent != 0; parent = PInvokePvicom.GetParent(num))
          num = parent;
        PInvokePvicom.GetWindowText(num, s, s.Capacity + 1);
        string str4 = s.ToString();
        switch (PInvokePvicom.GetWindowContextHelpId(num))
        {
          case 131570:
          case 131571:
            if (-1 != str4.IndexOf("Automation Studio"))
            {
              APIFC_PviSecure structure = new APIFC_PviSecure();
              structure.first = 1381322324;
              structure.second = -1381322325;
              IntPtr hMemory = PviMarshal.AllocHGlobal((IntPtr) Marshal.SizeOf(typeof (APIFC_PviSecure)));
              Marshal.StructureToPtr((object) structure, hMemory, false);
              PInvokePvicom.PviComServerClient(hMemory, 0);
              PviMarshal.FreeHGlobal(ref hMemory);
              break;
            }
            break;
        }
        if (EventMessageType.WindowMessage == this.Service.EventMessageType)
          this.AddID((object) this, ref this.propInternID);
        this.propReturnValue = this.Initialize(ref this.hPvi, this.propTimeout, this.propRetryTime, pInitParam, new IntPtr(0));
        if (this.Server != null && this.Server.Length > 0)
          this.propReturnValue = this.XLinkRequest(this.Service.hPvi, "Pvi", 99U, "", 98U);
        if (this.propReturnValue == 0)
        {
          if (this.EventMessageType == EventMessageType.CallBack)
          {
            this.propReturnValue = this.SetGlobEventMsg(this.hPvi, 240U, 4294967294U, 101U);
            this.propReturnValue = this.SetGlobEventMsg(this.hPvi, 241U, 4294967294U, 102U);
            this.propReturnValue = this.SetGlobEventMsg(this.hPvi, 242U, 4294967294U, 103U);
          }
          else
          {
            this.propReturnValue = this.SetGlobEventMsg(this.hPvi, 240U, 101U, 101U);
            this.propReturnValue = this.SetGlobEventMsg(this.hPvi, 241U, 102U, 102U);
            this.propReturnValue = this.SetGlobEventMsg(this.hPvi, 242U, 103U, 103U);
          }
        }
        if (this.propReturnValue == 0)
          return;
        this.OnError(new PviEventArgs(this.Name, this.Address, this.propReturnValue, this.Service.Language, Action.ServiceConnect, this));
      }
    }

    public int GetPVIVersionInfo(ref Hashtable versionInfos)
    {
      int num1 = 4096;
      IntPtr num2 = PviMarshal.AllocCoTaskMem(num1);
      versionInfos.Clear();
      PInvokePvicom.PviComGetVersionInfo(num2, num1);
      PviMarshal.GetVersionInfos(num2, num1, ref versionInfos, "§LOCAL§ ");
      if (PInvokePvicom.PviComRead(this.hPvi, this.LinkId, AccessTypes.Version, IntPtr.Zero, 0, num2, num1) == 0)
        PviMarshal.GetVersionInfos(num2, num1, ref versionInfos);
      int pviVersionInfo = PInvokePvicom.PviComRead(this.hPvi, this.LinkId, AccessTypes.PVIVersion, IntPtr.Zero, 0, num2, num1);
      if (pviVersionInfo == 0)
        PviMarshal.GetVersionInfos(num2, num1, ref versionInfos, "§PVI Setup§");
      return pviVersionInfo;
    }

    private void PviDisconnect(uint pviHandle)
    {
      if (this.propMSGWindow != null && this.propMSGWindow.InvokeRequired)
        this.propMSGWindow.Invoke((Delegate) new Service.SetPviDisconnectCallback(this.PviDisconnect), (object) pviHandle);
      else if (PInvokePvicom.PostMessage(this.WindowHandle, Base.MakeWindowMessage(Action.ServiceDisconnectPOSTMSG), (int) pviHandle, (int) this.propInternID))
        this.propReturnValue = 0;
      else
        this.DisconnectEx(this.hPvi);
    }

    public override void Disconnect()
    {
      if (ConnectionStates.Disconnecting == this.propConnectionState)
        return;
      if (this.propConnectionState == ConnectionStates.Unininitialized)
      {
        this.PviDisconnect(this.hPvi);
      }
      else
      {
        this.propConnectionState = ConnectionStates.Disconnecting;
        if (ConnectionStates.Disconnected == this.propConnectionState || this.Cpus == null || ConnectionStates.Disconnected == this.Cpus.propConnectionState)
        {
          this.OnDisconnected(new PviEventArgs(this.Name, this.Address, 0, this.Language, Action.ServiceDisconnect, this));
        }
        else
        {
          int propConnectionState = (int) this.propConnectionState;
          this.propConnectionState = ConnectionStates.Disconnecting;
          this.PviDisconnect(this.hPvi);
        }
      }
    }

    public override void Disconnect(bool noResponse)
    {
      if (ConnectionStates.Connected < this.propConnectionState)
        return;
      this.propNoDisconnectedEvent = noResponse;
      this.Disconnect();
    }

    internal int DisconnectEx(uint pviHandle)
    {
      this.propReturnValue = Service.Deinitialize(pviHandle);
      this.hPvi = 0U;
      this.OnDisconnected(new PviEventArgs(this.Name, this.Address, this.propReturnValue, this.Language, Action.ServiceDisconnect, this));
      if (this.propReturnValue != 0)
        this.OnError(new PviEventArgs(this.Name, this.Address, this.propReturnValue, this.Language, Action.ServiceDisconnect, this));
      return this.propReturnValue;
    }

    protected override void OnDisconnected(PviEventArgs e)
    {
      if (this.InternIDs != null)
      {
        foreach (PviCBEvents pviCbEvents in (IEnumerable) this.InternIDs.Values)
        {
          if (pviCbEvents != null && pviCbEvents is Base)
            ((Base) pviCbEvents).UpdateServiceCreateState();
        }
      }
      base.OnDisconnected(e);
    }

    protected override void OnConnected(PviEventArgs e)
    {
      if (e.ErrorCode == 0 && Action.ServiceArrange == e.Action)
      {
        this.RefreshPviClientsList();
        if (0 < this.propCpus.Count)
        {
          lock (this.propCpus.SyncRoot)
          {
            foreach (Cpu cpu in (IEnumerable) this.propCpus.Values)
            {
              if (Actions.Connect == (cpu.Requests & Actions.Connect))
                cpu.Connect();
              else if (cpu.reCreateActive)
                cpu.reCreateState();
            }
          }
        }
        e.propAction = Action.ConnectedEvent;
      }
      if (this.propConnectionInterupted && this.IsConnected)
        return;
      base.OnConnected(e);
    }

    public int AttachPVIObjects()
    {
      if (this.propServicePviObj != null)
      {
        this.propServicePviObj.Deinit();
        this.propServicePviObj = (PviObjectBrowser) null;
      }
      this.propServicePviObj = new PviObjectBrowser("@Pvi", false, (Base) this, (PviObjectBrowser) null);
      this.propPendingObjectBrowserEvents = 0U;
      this.ConnectionType = ConnectionType.Link;
      return this.propServicePviObj.Link();
    }

    public event PVIObjectsAttachedEventHandler PVIObjectsAttached;

    protected internal void OnPVIObjectsAttached(PviEventArgs e)
    {
      --this.propPendingObjectBrowserEvents;
      if (this.propPendingObjectBrowserEvents != 0U || this.PVIObjectsAttached == null)
        return;
      if (e.propErrorCode != 0)
        this.OnError(e);
      this.PVIObjectsAttached((object) this, e);
    }

    private int CreatePviLinkObject()
    {
      int pviLinkObject = 0;
      string str1 = "@Pvi";
      string str2 = "";
      if (this.propLinkId == 0U)
        pviLinkObject = this.Service.EventMessageType != EventMessageType.CallBack ? this.XMLink(this.hPvi, out this.propLinkId, str1, 6U, 6U, str2) : PInvokePvicom.PviComLink(this.hPvi, out this.propLinkId, str1, this.callback, 6U, 6U, str2);
      return pviLinkObject;
    }

    public int RefreshPviClientsList()
    {
      int length = 20;
      this.propClientNames.Clear();
      IntPtr zero = IntPtr.Zero;
      IntPtr hMemory = PviMarshal.AllocHGlobal(length);
      int num = this.CreatePviLinkObject();
      if (num == 0)
      {
        for (num = PInvokePvicom.PviComRead(this.hPvi, this.propLinkId, AccessTypes.Clients, zero, 0, hMemory, length); num == 0 && Marshal.ReadByte(hMemory, length - 1) != (byte) 0; num = PInvokePvicom.PviComRead(this.hPvi, this.propLinkId, AccessTypes.Clients, zero, 0, hMemory, length))
        {
          PviMarshal.FreeHGlobal(ref hMemory);
          length += 100;
          hMemory = PviMarshal.AllocHGlobal(length);
        }
        if (num == 0)
        {
          byte[] numArray = new byte[length];
          Marshal.Copy(hMemory, numArray, 0, length);
          string str = Encoding.ASCII.GetString(numArray);
          string[] strArray = str.Substring(0, str.IndexOf(char.MinValue)).Split('\t');
          for (int index = 0; index < strArray.Length; ++index)
            this.propClientNames.Add((object) strArray.GetValue(index).ToString());
        }
      }
      PviMarshal.FreeHGlobal(ref hMemory);
      return num;
    }

    public int UpdateLicenceInfo()
    {
      int num = this.CreatePviLinkObject();
      if (num == 0)
        num = this.Service.EventMessageType != EventMessageType.CallBack ? PInvokePvicom.PviComMsgReadRequest(this.hPvi, this.propLinkId, AccessTypes.License, this.WindowHandle, 104U, this.propInternID) : PInvokePvicom.PviComReadRequest(this.hPvi, this.propLinkId, AccessTypes.License, this.callback, 4294967294U, 104U);
      return num;
    }

    public ArrayList ClientNames => this.propClientNames;

    public LicenceInfo LicenceInfo => this.propLicenseInfo;

    public event PviEventHandler LicencInfoUpdated;

    protected virtual void OnLicencInfoUpdated(LicenceInfo licInfo, int error)
    {
      if (this.LicencInfoUpdated == null)
        return;
      this.LicencInfoUpdated((object) this, new PviEventArgs(this.Name, this.Address, error, this.propLanguage, Action.LicenceInfoUpdate, this));
    }

    internal bool PVICB_Create(
      int wParam,
      int lParam,
      IntPtr pData,
      uint dataLen,
      ref ResponseInfo info)
    {
      PviCBEvents internId = (PviCBEvents) this.InternIDs[(object) (uint) lParam];
      internId?.OnPviCreated(info.Error, info.LinkId);
      Base @base = internId as Base;
      return true;
    }

    internal bool PVICB_Link(
      int wParam,
      int lParam,
      IntPtr pData,
      uint dataLen,
      ref ResponseInfo info)
    {
      return this.PVI_LinkResponse(0, wParam, lParam, pData, dataLen, ref info);
    }

    internal bool PVICB_LinkA(
      int wParam,
      int lParam,
      IntPtr pData,
      uint dataLen,
      ref ResponseInfo info)
    {
      return this.PVI_LinkResponse(1, wParam, lParam, pData, dataLen, ref info);
    }

    internal bool PVICB_LinkB(
      int wParam,
      int lParam,
      IntPtr pData,
      uint dataLen,
      ref ResponseInfo info)
    {
      return this.PVI_LinkResponse(2, wParam, lParam, pData, dataLen, ref info);
    }

    internal bool PVICB_LinkC(
      int wParam,
      int lParam,
      IntPtr pData,
      uint dataLen,
      ref ResponseInfo info)
    {
      return this.PVI_LinkResponse(3, wParam, lParam, pData, dataLen, ref info);
    }

    internal bool PVICB_LinkD(
      int wParam,
      int lParam,
      IntPtr pData,
      uint dataLen,
      ref ResponseInfo info)
    {
      return this.PVI_LinkResponse(4, wParam, lParam, pData, dataLen, ref info);
    }

    internal bool PVICB_LinkE(
      int wParam,
      int lParam,
      IntPtr pData,
      uint dataLen,
      ref ResponseInfo info)
    {
      return this.PVI_LinkResponse(5, wParam, lParam, pData, dataLen, ref info);
    }

    internal bool PVICB_Event(
      int wParam,
      int lParam,
      IntPtr pData,
      uint dataLen,
      ref ResponseInfo info)
    {
      return this.PVI_Event(0, wParam, lParam, pData, dataLen, ref info);
    }

    internal bool PVICB_EventA(
      int wParam,
      int lParam,
      IntPtr pData,
      uint dataLen,
      ref ResponseInfo info)
    {
      return this.PVI_Event(1, wParam, lParam, pData, dataLen, ref info);
    }

    internal bool PVICB_Read(
      int wParam,
      int lParam,
      IntPtr pData,
      uint dataLen,
      ref ResponseInfo info)
    {
      return this.PVI_ReadResponse(0, wParam, lParam, pData, dataLen, ref info);
    }

    internal bool PVICB_ReadA(
      int wParam,
      int lParam,
      IntPtr pData,
      uint dataLen,
      ref ResponseInfo info)
    {
      return this.PVI_ReadResponse(1, wParam, lParam, pData, dataLen, ref info);
    }

    internal bool PVICB_ReadB(
      int wParam,
      int lParam,
      IntPtr pData,
      uint dataLen,
      ref ResponseInfo info)
    {
      return this.PVI_ReadResponse(2, wParam, lParam, pData, dataLen, ref info);
    }

    internal bool PVICB_ReadC(
      int wParam,
      int lParam,
      IntPtr pData,
      uint dataLen,
      ref ResponseInfo info)
    {
      return this.PVI_ReadResponse(3, wParam, lParam, pData, dataLen, ref info);
    }

    internal bool PVICB_ReadD(
      int wParam,
      int lParam,
      IntPtr pData,
      uint dataLen,
      ref ResponseInfo info)
    {
      return this.PVI_ReadResponse(4, wParam, lParam, pData, dataLen, ref info);
    }

    internal bool PVICB_ReadE(
      int wParam,
      int lParam,
      IntPtr pData,
      uint dataLen,
      ref ResponseInfo info)
    {
      return this.PVI_ReadResponse(5, wParam, lParam, pData, dataLen, ref info);
    }

    internal bool PVICB_ReadF(
      int wParam,
      int lParam,
      IntPtr pData,
      uint dataLen,
      ref ResponseInfo info)
    {
      return this.PVI_ReadResponse(6, wParam, lParam, pData, dataLen, ref info);
    }

    internal bool PVICB_ReadG(
      int wParam,
      int lParam,
      IntPtr pData,
      uint dataLen,
      ref ResponseInfo info)
    {
      return this.PVI_ReadResponse(7, wParam, lParam, pData, dataLen, ref info);
    }

    internal bool PVICB_ReadS(
      int wParam,
      int lParam,
      IntPtr pData,
      uint dataLen,
      ref ResponseInfo info)
    {
      return this.PVI_ReadResponse(19, wParam, lParam, pData, dataLen, ref info);
    }

    internal bool PVICB_ReadT(
      int wParam,
      int lParam,
      IntPtr pData,
      uint dataLen,
      ref ResponseInfo info)
    {
      return this.PVI_ReadResponse(20, wParam, lParam, pData, dataLen, ref info);
    }

    internal bool PVICB_Write(
      int wParam,
      int lParam,
      IntPtr pData,
      uint dataLen,
      ref ResponseInfo info)
    {
      return this.PVI_WriteResponse(0, wParam, lParam, pData, dataLen, ref info);
    }

    internal bool PVICB_WriteA(
      int wParam,
      int lParam,
      IntPtr pData,
      uint dataLen,
      ref ResponseInfo info)
    {
      return this.PVI_WriteResponse(1, wParam, lParam, pData, dataLen, ref info);
    }

    internal bool PVICB_WriteB(
      int wParam,
      int lParam,
      IntPtr pData,
      uint dataLen,
      ref ResponseInfo info)
    {
      return this.PVI_WriteResponse(2, wParam, lParam, pData, dataLen, ref info);
    }

    internal bool PVICB_WriteC(
      int wParam,
      int lParam,
      IntPtr pData,
      uint dataLen,
      ref ResponseInfo info)
    {
      return this.PVI_WriteResponse(3, wParam, lParam, pData, dataLen, ref info);
    }

    internal bool PVICB_WriteD(
      int wParam,
      int lParam,
      IntPtr pData,
      uint dataLen,
      ref ResponseInfo info)
    {
      return this.PVI_WriteResponse(4, wParam, lParam, pData, dataLen, ref info);
    }

    internal bool PVICB_WriteE(
      int wParam,
      int lParam,
      IntPtr pData,
      uint dataLen,
      ref ResponseInfo info)
    {
      return this.PVI_WriteResponse(5, wParam, lParam, pData, dataLen, ref info);
    }

    internal bool PVICB_WriteF(
      int wParam,
      int lParam,
      IntPtr pData,
      uint dataLen,
      ref ResponseInfo info)
    {
      return this.PVI_WriteResponse(6, wParam, lParam, pData, dataLen, ref info);
    }

    internal bool PVICB_Unlink(
      int wParam,
      int lParam,
      IntPtr pData,
      uint dataLen,
      ref ResponseInfo info)
    {
      return this.PVI_UnlinkResponse(0, wParam, lParam, pData, dataLen, ref info);
    }

    internal bool PVICB_UnlinkA(
      int wParam,
      int lParam,
      IntPtr pData,
      uint dataLen,
      ref ResponseInfo info)
    {
      return this.PVI_UnlinkResponse(1, wParam, lParam, pData, dataLen, ref info);
    }

    internal bool PVICB_UnlinkB(
      int wParam,
      int lParam,
      IntPtr pData,
      uint dataLen,
      ref ResponseInfo info)
    {
      return this.PVI_UnlinkResponse(2, wParam, lParam, pData, dataLen, ref info);
    }

    internal bool PVICB_UnlinkC(
      int wParam,
      int lParam,
      IntPtr pData,
      uint dataLen,
      ref ResponseInfo info)
    {
      return this.PVI_UnlinkResponse(3, wParam, lParam, pData, dataLen, ref info);
    }

    internal bool PVICB_UnlinkD(
      int wParam,
      int lParam,
      IntPtr pData,
      uint dataLen,
      ref ResponseInfo info)
    {
      return this.PVI_UnlinkResponse(4, wParam, lParam, pData, dataLen, ref info);
    }

    internal bool PVICB_Delete(
      int wParam,
      int lParam,
      IntPtr pData,
      uint dataLen,
      ref ResponseInfo info)
    {
      ((PviCBEvents) this.InternIDs[(object) (uint) lParam])?.OnPviDeleted(info.Error);
      return true;
    }

    internal bool PVICB_ChangeLink(
      int wParam,
      int lParam,
      IntPtr pData,
      uint dataLen,
      ref ResponseInfo info)
    {
      return true;
    }

    internal bool PVICB_SNMP(
      int wParam,
      int lParam,
      IntPtr pData,
      uint dataLen,
      ref ResponseInfo info)
    {
      PviCBEvents internId = (PviCBEvents) this.InternIDs[(object) (uint) lParam];
      if (internId != null)
      {
        switch (info.Mode)
        {
        }
      }
      SNMPBase snmpBase = internId as SNMPBase;
      return true;
    }

    [CLSCompliant(false)]
    protected internal bool Callback(
      int wParam,
      int lParam,
      IntPtr pData,
      uint dataLen,
      ref ResponseInfo info)
    {
      AccessModes mode = (AccessModes) info.Mode;
      AccessTypes type = (AccessTypes) info.Type;
      this.propErrorCode = info.Error;
      if (this.propDisposed)
        return true;
      if (info.LinkId != 0U)
        this.propLinkId = info.LinkId;
      int error = info.Error;
      switch (mode)
      {
        case AccessModes.Event:
          switch ((EventTypes) info.Type)
          {
            case EventTypes.Error:
              this.OnError(new PviEventArgs(this.Name, this.Address, info.Error, this.Language, Action.ErrorEvent, this));
              return true;
            case EventTypes.ServiceConnect:
              this.OnConnected(new PviEventArgs(this.Name, this.Address, info.Error, this.Language, Action.ServiceConnect, this));
              if (info.Error != 0)
              {
                this.OnError(new PviEventArgs(this.Name, this.Address, info.Error, this.Language, Action.ServiceConnect, this));
                this.propConnectionInterupted = true;
              }
              return true;
            case EventTypes.ServiceDisconnect:
              this.propLinkId = 0U;
              if (this.IsConnected)
              {
                this.OnDisconnected(new PviEventArgs(this.Name, this.Address, info.Error, this.Language, Action.ServiceDisconnect, this));
                this.propConnectionInterupted = true;
              }
              else
              {
                this.OnConnected(new PviEventArgs(this.Name, this.Address, info.Error, this.Language, Action.ServiceConnect, this));
                this.OnError(new PviEventArgs(this.Name, this.Address, info.Error, this.Language, Action.ServiceConnect, this));
                this.propConnectionInterupted = true;
              }
              this.CallDisconnectedForChildObjects(info.Error, this.Language, Action.ServiceDisconnect);
              return true;
            case EventTypes.ServiceArrange:
              if (!this.propConnectionInterupted)
              {
                this.OnConnected(new PviEventArgs(this.Name, this.Address, info.Error, this.Language, Action.ServiceArrange, this));
              }
              else
              {
                if (this.hPvi == 0U)
                  this.Connect();
                this.OnConnected(new PviEventArgs(this.Name, this.Address, info.Error, this.Language, Action.ServiceArrange, this));
                this.propConnectionInterupted = false;
                if (0 < this.Cpus.Count)
                {
                  foreach (Base @base in (IEnumerable) this.Cpus.Values)
                    @base.reCreateState();
                }
              }
              this.propConnectionState = ConnectionStates.Connected;
              return true;
          }
          break;
        case AccessModes.Read:
          switch (type)
          {
            case AccessTypes.Version:
              if (info.Error == 0 && 0U < dataLen)
              {
                string str = "";
                string ansiString = PviMarshal.ToAnsiString(pData, dataLen);
                str = ansiString.IndexOf("\r\n") == -1 ? ansiString.Substring(ansiString.IndexOf("\n") - 4, 4) : ansiString.Substring(ansiString.IndexOf("\r\n") - 4, 4);
                break;
              }
              break;
            case AccessTypes.License:
              this.propLicenseInfo.UpdateRuntimeState(pData);
              this.OnLicencInfoUpdated(this.propLicenseInfo, info.Error);
              break;
          }
          break;
        case AccessModes.Link:
          this.ReadRequest(this.Service.hPvi, info.LinkId, AccessTypes.Version, 110U);
          break;
      }
      return false;
    }

    private void CallDisconnectedForChildObjects(
      int errorCode,
      string languageName,
      Action actionCode)
    {
      if (this.InternIDs == null)
        return;
      for (int key = 0; key < this.InternIDs.Count; ++key)
      {
        PviCBEvents internId = (PviCBEvents) this.InternIDs[(object) key];
        switch (internId)
        {
          case Cpu _:
          case Module _:
          case Variable _:
          case IODataPoint _:
            internId.OnPviEvent(errorCode, EventTypes.Error, PVIDataStates.InheratedError, IntPtr.Zero, 0U, 0);
            break;
        }
      }
    }

    protected internal override void OnError(PviEventArgs e) => base.OnError((object) this, e);

    protected internal override void OnError(object sender, PviEventArgs e)
    {
      if (e.ErrorCode == 0)
        return;
      base.OnError(sender, e);
    }

    public int SaveConfiguration(string fileName, ConfigurationFlags flags)
    {
      int num = 0;
      FileStream w = new FileStream(fileName, FileMode.Create, FileAccess.Write);
      XmlTextWriter writer = new XmlTextWriter((Stream) w, Encoding.UTF8);
      writer.Formatting = Formatting.Indented;
      writer.WriteStartDocument();
      writer.WriteRaw("<?PviServices Version=1.0?>");
      writer.WriteStartElement("Services");
      this.ToXMLTextWriter(ref writer, flags);
      writer.WriteEndElement();
      writer.Close();
      w.Close();
      return num;
    }

    public virtual int SaveConfiguration(string fileName)
    {
      ConfigurationFlags flags = (ConfigurationFlags) (0 | 4 | 8 | 16 | 32 | 1 | 1024 | 512);
      return this.SaveConfiguration(fileName, flags);
    }

    internal override int ToXMLTextWriter(ref XmlTextWriter writer, ConfigurationFlags flags)
    {
      writer.WriteStartElement(nameof (Service));
      int xmlTextWriter1 = base.ToXMLTextWriter(ref writer, flags);
      if (this.propTimeout != 0)
        writer.WriteAttributeString("Timeout", this.propTimeout.ToString());
      if (1 != this.propPVIAutoStart)
        writer.WriteAttributeString("PviAutoStart", this.propPVIAutoStart.ToString());
      if (0 < this.propProcessTimeout)
        writer.WriteAttributeString("ProcessTimeout", this.propProcessTimeout.ToString());
      if (1 != this.propMessageLimitation)
        writer.WriteAttributeString("MessageLimitation", this.propMessageLimitation.ToString());
      if (0 < this.propRetryTime)
        writer.WriteAttributeString("RetryTime", this.propRetryTime.ToString());
      if (!this.propWaitForParentConnection)
        writer.WriteAttributeString("WaitForParent", this.propWaitForParentConnection.ToString());
      if (!(this.propLanguage == "en-us"))
        writer.WriteAttributeString("Language", this.Language);
      if (this.Port != 0)
        writer.WriteAttributeString("Port", this.Port.ToString());
      if (this.propServer != null && this.propServer.Length > 0)
        writer.WriteAttributeString("Server", this.propServer.ToString());
      writer.WriteAttributeString("IsStatic", this.IsStatic.ToString());
      switch (this.propLogicalUsage)
      {
        case LogicalObjectsUsage.None:
          writer.WriteAttributeString("LogicalUsage", "None");
          break;
        case LogicalObjectsUsage.FullName:
          writer.WriteAttributeString("LogicalUsage", "FullName");
          break;
        case LogicalObjectsUsage.ObjectName:
          writer.WriteAttributeString("LogicalUsage", "ObjectName");
          break;
        case LogicalObjectsUsage.ObjectNameWithType:
          writer.WriteAttributeString("LogicalUsage", "ObjectNameWithType");
          break;
      }
      if (this.propExtendedTypeInfoAutoUpdateForVariables)
        writer.WriteAttributeString("ExtendedTypeInfoAutoUpdateForVariables", this.propExtendedTypeInfoAutoUpdateForVariables.ToString());
      int xmlTextWriter2 = this.Variables.ToXMLTextWriter(ref writer, flags);
      if (xmlTextWriter2 != 0)
        xmlTextWriter1 = xmlTextWriter2;
      if (0 < this.Cpus.Count)
      {
        foreach (Base @base in (IEnumerable) this.Cpus.Values)
        {
          int xmlTextWriter3 = @base.ToXMLTextWriter(ref writer, flags);
          if (xmlTextWriter3 != 0)
            xmlTextWriter1 = xmlTextWriter3;
        }
      }
      writer.WriteEndElement();
      return xmlTextWriter1;
    }

    internal int AddLogicalObject(string logical, object obj)
    {
      int num = 0;
      if (this.propServices == null)
      {
        if (this.propLogicalObjects == null)
          this.propLogicalObjects = new LogicalCollection((object) this, this.Name + ".Logicals");
        if (this.propLogicalObjects.ContainsKey((object) logical))
          return -1;
        this.propLogicalObjects.Add((object) logical, obj);
        if (obj is Base)
          ((Base) obj).propLogicalName = logical;
        return num;
      }
      if (this.propServices.ContainsLogicalObject(logical))
        return -1;
      if (obj is Base)
        ((Base) obj).propLogicalName = logical;
      return this.propServices.AddLogicalObject(logical, obj);
    }

    internal void RemoveLogicalObject(string logical)
    {
      if (this.propServices == null && this.propLogicalObjects != null)
        this.propLogicalObjects.Remove(logical);
      else
        this.propServices.RemoveLogicalObject(logical);
    }

    private int FromXmlTextReader(XmlTextReader reader, object parent, ConfigurationFlags flags)
    {
      string str = "";
      bool flag = false;
      int num = 0;
      if (reader == null)
        return -1;
      if (reader.Name != nameof (Service))
      {
        int content = (int) reader.MoveToContent();
        reader.Read();
      }
      while (reader.NodeType != XmlNodeType.EndElement && reader.Name != "Services")
      {
        if (string.Compare(reader.Name, nameof (Service)) == 0)
        {
          this.FromXmlTextReader(ref reader, flags, (Base) this);
          str = "";
          string attribute1 = reader.GetAttribute("Connected");
          if (attribute1 != null && attribute1.Length > 0 && attribute1.ToLower() == "true")
            flag = true;
          this.isStatic = reader.GetAttribute("IsStatic").Equals("true");
          str = "";
          string attribute2 = reader.GetAttribute("Timeout");
          if (attribute2 != null && attribute2.Length > 0)
            PviParse.TryParseInt32(attribute2, out this.propTimeout);
          str = "";
          string attribute3 = reader.GetAttribute("PviAutoStart");
          if (attribute3 != null && attribute3.Length > 0)
            PviParse.TryParseInt32(attribute3, out this.propPVIAutoStart);
          str = "";
          string attribute4 = reader.GetAttribute("ProcessTimeout");
          if (attribute4 != null && attribute4.Length > 0)
            PviParse.TryParseInt32(attribute4, out this.propProcessTimeout);
          str = "";
          string attribute5 = reader.GetAttribute("MessageLimitation");
          this.propMessageLimitation = 1;
          if (attribute5 != null && attribute5.Length > 0)
            PviParse.TryParseInt32(attribute5, out this.propMessageLimitation);
          str = "";
          string attribute6 = reader.GetAttribute("RetryTime");
          this.propRetryTime = 0;
          if (attribute6 != null && attribute6.Length > 0)
            PviParse.TryParseInt32(attribute6, out this.propRetryTime);
          str = "";
          string attribute7 = reader.GetAttribute("WaitForParent");
          if (attribute7 != null && attribute7.ToLower().CompareTo("false") == 0)
            this.propWaitForParentConnection = false;
          str = "";
          string attribute8 = reader.GetAttribute("Language");
          if (attribute8 != null && attribute8.Length > 0)
            this.Language = attribute8;
          str = "";
          string attribute9 = reader.GetAttribute("Server");
          if (attribute9 != null && attribute9.Length > 0)
            this.Server = attribute9;
          str = "";
          string attribute10 = reader.GetAttribute("Port");
          if (attribute10 != null && attribute10.Length > 0)
            PviParse.TryParseInt32(attribute10, out this.propPort);
          str = "";
          string attribute11 = reader.GetAttribute("LogicalUsage");
          if (attribute11 != null && attribute11.Length > 0)
          {
            switch (attribute11)
            {
              case "None":
                this.propLogicalUsage = LogicalObjectsUsage.None;
                break;
              case "FullName":
                this.propLogicalUsage = LogicalObjectsUsage.FullName;
                break;
              case "ObjectName":
                this.propLogicalUsage = LogicalObjectsUsage.ObjectName;
                break;
              case "ObjectNameWithType":
                this.propLogicalUsage = LogicalObjectsUsage.ObjectNameWithType;
                break;
            }
          }
          str = "";
          string attribute12 = reader.GetAttribute("ExtendedTypeInfoAutoUpdateForVariables");
          this.propExtendedTypeInfoAutoUpdateForVariables = false;
          if (attribute12 != null && 0 < attribute12.Length && "true" == attribute12.ToLower())
            this.propExtendedTypeInfoAutoUpdateForVariables = true;
          reader.Read();
        }
        else if (reader.Name.ToLower().CompareTo("variablecollection") == 0)
          this.Variables.FromXmlTextReader(ref reader, flags, (Base) this);
        else if (reader.Name.ToLower().CompareTo("variable") == 0)
          this.Variables.FromXmlTextReader(ref reader, flags, (Base) this);
        else if (reader.Name.ToLower().CompareTo("cpu") == 0)
        {
          string attribute = reader.GetAttribute("Name");
          if (attribute != null && attribute.Length > 0)
          {
            this.propCpu = new Cpu(this, attribute);
            this.propCpu.FromXmlTextReader(ref reader, flags, (Base) this.propCpu);
          }
          if (reader.Name.ToLower().CompareTo("service") == 0 && reader.NodeType == XmlNodeType.EndElement)
            break;
        }
        else
          reader.Read();
      }
      if (flag)
      {
        if (this.propServer != null && this.propServer.Length > 0 && this.propPort > 0)
          this.Connect(this.propServer, this.propPort);
        this.Connect();
      }
      return num;
    }

    public virtual int LoadConfiguration(string fileName)
    {
      ConfigurationFlags flags = (ConfigurationFlags) (0 | 4 | 8 | 16 | 32 | 1 | 1024 | 512);
      return this.LoadConfiguration(fileName, flags);
    }

    public virtual int LoadConfiguration(StreamReader stream)
    {
      ConfigurationFlags flags = (ConfigurationFlags) (0 | 4 | 8 | 16 | 32 | 1 | 1024 | 512);
      return this.LoadConfiguration(stream, flags);
    }

    protected internal int LoadConfiguration(XmlTextReader reader, ConfigurationFlags flags) => this.FromXmlTextReader(reader, (object) this, flags);

    protected virtual int LoadConfiguration(string fileName, ConfigurationFlags flags)
    {
      if (!File.Exists(fileName))
        return -1;
      XmlTextReader reader = new XmlTextReader(fileName);
      reader.WhitespaceHandling = WhitespaceHandling.None;
      int num = this.LoadConfiguration(reader, flags);
      reader.Close();
      return num;
    }

    protected virtual int LoadConfiguration(StreamReader stream, ConfigurationFlags flags)
    {
      if (stream == null)
        return -1;
      return this.LoadConfiguration(new XmlTextReader((TextReader) stream)
      {
        WhitespaceHandling = WhitespaceHandling.None
      }, flags);
    }

    public override void Remove()
    {
      if (this.propSNMP != null)
        this.propSNMP.Cleanup();
      base.Remove();
      if (this.Services != null && this.Name != null)
        this.Services.Remove(this.Name);
      if (this.Name == null)
        return;
      ServiceCollection.Services.Remove((object) this.Name);
    }

    public void RemoveArchive(string path)
    {
      LoggerCollection loggerCollection1 = (LoggerCollection) null;
      foreach (LoggerCollection loggerCollection2 in this.LoggerCollections)
      {
        if (loggerCollection2.Name.Equals((object) path))
        {
          loggerCollection1 = loggerCollection2;
          break;
        }
      }
      this.LoggerCollections.Remove((object) loggerCollection1);
    }

    internal static string GetErrorText(int error, string language)
    {
      uint num = (uint) error;
      string errorText = "";
      if (language != "")
      {
        try
        {
          ResourceManager resourceManager1 = language.ToLower().IndexOf("de") != 0 ? (language.ToLower().IndexOf("fr") != 0 ? new ResourceManager("BR.AN.PviServices.Resources.en.PviErrors", Assembly.GetExecutingAssembly()) : new ResourceManager("BR.AN.PviServices.Resources.fr.PviErrors", Assembly.GetExecutingAssembly())) : new ResourceManager("BR.AN.PviServices.Resources.de.PviErrors", Assembly.GetExecutingAssembly());
          errorText = num < (uint) int.MaxValue ? resourceManager1.GetString(string.Format("{0:0000}", (object) num)) : resourceManager1.GetString(num.ToString());
          if (errorText == null)
          {
            ResourceManager resourceManager2 = language.ToLower().IndexOf("de") != 0 ? (language.ToLower().IndexOf("fr") != 0 ? new ResourceManager("BR.AN.PviServices.Resources.en.PccLog", Assembly.GetExecutingAssembly()) : new ResourceManager("BR.AN.PviServices.Resources.fr.PccLog", Assembly.GetExecutingAssembly())) : new ResourceManager("BR.AN.PviServices.Resources.de.PccLog", Assembly.GetExecutingAssembly());
            errorText = num < (uint) int.MaxValue ? resourceManager2.GetString(string.Format("{0:0000}", (object) num)) : resourceManager2.GetString(num.ToString());
          }
        }
        catch (System.Exception ex)
        {
        }
      }
      return errorText;
    }

    internal static string GetErrorTextEx(string errorNumber, string language)
    {
      string errorTextEx = "";
      if (language != "")
      {
        try
        {
          errorTextEx = (language.ToLower().IndexOf("de") != 0 ? (language.ToLower().IndexOf("fr") != 0 ? new ResourceManager("BR.AN.PviServices.Resources.en.PccLog", Assembly.GetExecutingAssembly()) : new ResourceManager("BR.AN.PviServices.Resources.fr.PccLog", Assembly.GetExecutingAssembly())) : new ResourceManager("BR.AN.PviServices.Resources.de.PccLog", Assembly.GetExecutingAssembly())).GetString(errorNumber);
        }
        catch (System.Exception ex)
        {
        }
      }
      return errorTextEx;
    }

    internal static string GetErrorTextPCC(int error, string language) => Service.GetErrorTextEx(error.ToString(), language);

    internal static string GetErrorTextPCC(uint error, string language) => Service.GetErrorTextEx(string.Format("{0:0000}", (object) error), language);

    internal EventMessageType EventMessageType => this.propEventMsgType;

    public LogicalCollection LogicalObjects => this.propLogicalObjects;

    internal uint ModuleUID => this.propModuleUIDBuilder++;

    public virtual int Timeout
    {
      get => this.propTimeout;
      set => this.propTimeout = value;
    }

    public int RetryTime
    {
      get => this.propRetryTime;
      set => this.propRetryTime = value;
    }

    public int ProcessTimeout
    {
      get => this.propProcessTimeout;
      set => this.propProcessTimeout = value;
    }

    public int MessageLimitation
    {
      get => this.propMessageLimitation;
      set => this.propMessageLimitation = value;
    }

    public int PVIAutoStart
    {
      get => this.propPVIAutoStart;
      set => this.propPVIAutoStart = value;
    }

    internal IntPtr WindowHandle => this.propWindowHandle;

    public string Language
    {
      get => this.propLanguage;
      set
      {
        this.propLanguage = value;
        this.propUtils.ActiveCulture = this.propLanguage;
      }
    }

    internal virtual bool ErrorException
    {
      get => this.propErrorexception;
      set => this.propErrorexception = value;
    }

    internal virtual bool ErrorEvent
    {
      get => this.propErrorevent;
      set => this.propErrorevent = value;
    }

    public virtual int Port
    {
      get => this.propPort;
      set => this.propPort = value;
    }

    public virtual string Server
    {
      get => this.propServer;
      set => this.propServer = value;
    }

    public CpuCollection Cpus => this.propCpus;

    internal CollectionType CollectionType => this.propCollectionType;

    public bool IsStatic
    {
      get => this.isStatic;
      set => this.isStatic = value;
    }

    public bool AddMembersToVariableCollection
    {
      get => this.propAddMembersToVariableCollection;
      set => this.propAddMembersToVariableCollection = value;
    }

    public override string FullName => this.Name;

    public override string PviPathName => "@Pvi";

    public bool ExtendedTypeInfoAutoUpdateForVariables
    {
      get => this.propExtendedTypeInfoAutoUpdateForVariables;
      set => this.propExtendedTypeInfoAutoUpdateForVariables = value;
    }

    public bool AddStructMembersToMembersToo
    {
      get => this.propAddStructMembersToMembersToo;
      set => this.propAddStructMembersToMembersToo = value;
    }

    public LogicalObjectsUsage LogicalObjectsUsage
    {
      get => this.propLogicalUsage;
      set => this.propLogicalUsage = value;
    }

    public bool UserTagEvents
    {
      get => this.propUserTagEvents;
      set => this.propUserTagEvents = value;
    }

    public VariableCollection Variables => this.propVariables;

    internal ServiceCollection Services
    {
      get => this.propServices;
      set => this.propServices = value;
    }

    public bool WaitForParentConnection
    {
      get => this.propWaitForParentConnection;
      set => this.propWaitForParentConnection = value;
    }

    public LoggerEntryCollection LoggerEntries => this.propLoggerEntries;

    public ArrayList LoggerCollections => this.propLoggerCollections;

    internal byte[] ByteBuffer => this.byteBuffer;

    internal void cpyDblToBuffer(object value)
    {
      Marshal.Copy(new double[1]{ System.Convert.ToDouble(value) }, 0, this.iptr8Byte, 1);
      Marshal.Copy(this.iptr8Byte, this.byteBuffer, 0, 8);
    }

    internal void cpyFltToBuffer(object value)
    {
      Marshal.Copy(new float[1]{ System.Convert.ToSingle(value) }, 0, this.iptr4Byte, 1);
      Marshal.Copy(this.iptr4Byte, this.byteBuffer, 0, 4);
    }

    internal short toInt16(byte[] bBuffer, int byteOffset)
    {
      Marshal.Copy(bBuffer, byteOffset, this.iptr2Byte, 2);
      return Marshal.ReadInt16(this.iptr2Byte);
    }

    internal ushort toUInt16(byte[] bBuffer, int byteOffset)
    {
      Marshal.Copy(bBuffer, byteOffset, this.iptr2Byte, 2);
      return (ushort) Marshal.ReadInt16(this.iptr2Byte);
    }

    internal int toInt32(byte[] bBuffer, int byteOffset)
    {
      Marshal.Copy(bBuffer, byteOffset, this.iptr4Byte, 4);
      return Marshal.ReadInt32(this.iptr4Byte);
    }

    internal uint toUInt32(byte[] bBuffer, int byteOffset)
    {
      Marshal.Copy(bBuffer, byteOffset, this.iptr4Byte, 4);
      return (uint) Marshal.ReadInt32(this.iptr4Byte);
    }

    internal long toInt64(byte[] bBuffer, int byteOffset)
    {
      Marshal.Copy(bBuffer, byteOffset, this.iptr8Byte, 8);
      return PviMarshal.ReadInt64(this.iptr8Byte);
    }

    internal ulong toUInt64(byte[] bBuffer, int byteOffset)
    {
      Marshal.Copy(bBuffer, byteOffset, this.iptr8Byte, 8);
      return (ulong) PviMarshal.ReadInt64(this.iptr8Byte);
    }

    internal float toSingle(byte[] bBuffer, int byteOffset)
    {
      Marshal.Copy(bBuffer, byteOffset, this.iptr4Byte, 4);
      Marshal.Copy(this.iptr4Byte, this.marshF32, 0, 1);
      return this.marshF32[0];
    }

    internal double toDouble(byte[] bBuffer, int byteOffset)
    {
      Marshal.Copy(bBuffer, byteOffset, this.iptr8Byte, 8);
      Marshal.Copy(this.iptr8Byte, this.marshF64, 0, 1);
      return this.marshF64[0];
    }

    internal TimeSpan toTimeSpan(byte[] bBuffer, int byteOffset)
    {
      Marshal.Copy(bBuffer, byteOffset, this.iptr4Byte, 4);
      this.uint64Val = (long) Marshal.ReadInt32(this.iptr4Byte);
      return new TimeSpan(10000L * this.uint64Val);
    }

    internal DateTime toDateTime(byte[] bBuffer, int byteOffset)
    {
      Marshal.Copy(bBuffer, byteOffset, this.iptr4Byte, 4);
      return Pvi.UInt32ToDateTime((uint) Marshal.ReadInt32(this.iptr4Byte));
    }

    internal string toString(byte[] bBuffer, int byteOffset, int strLen)
    {
      string str = (string) null;
      for (int index = byteOffset; index < byteOffset + strLen; ++index)
      {
        byte num = (byte) bBuffer.GetValue(index);
        if (num != (byte) 0)
        {
          str += (string) (object) (char) num;
          if (index + 1 > bBuffer.GetLength(0))
            break;
        }
        else
          break;
      }
      return str;
    }

    internal bool PVI_UnlinkResponse(
      int fnNumber,
      int wParam,
      int lParam,
      IntPtr pData,
      uint dataLen,
      ref ResponseInfo info)
    {
      Base internId = (Base) this.InternIDs[(object) (uint) lParam];
      if (internId != null)
      {
        if (info.Type == 128)
          internId.OnPviCancelled(info.Error, 8);
        else
          internId.OnPviUnLinked(info.Error, fnNumber);
      }
      return true;
    }

    internal bool PVI_Event(
      int fnNumber,
      int wParam,
      int lParam,
      IntPtr pData,
      uint dataLen,
      ref ResponseInfo info)
    {
      ((PviCBEvents) this.InternIDs[(object) (uint) lParam])?.OnPviEvent(info.Error, (EventTypes) info.Type, (PVIDataStates) info.Status, pData, dataLen, fnNumber);
      return true;
    }

    internal bool PVI_LinkResponse(
      int fnNumber,
      int wParam,
      int lParam,
      IntPtr pData,
      uint dataLen,
      ref ResponseInfo info)
    {
      PviCBEvents internId = (PviCBEvents) this.InternIDs[(object) (uint) lParam];
      if (internId != null)
      {
        if (info.Type == 128)
          internId.OnPviCancelled(info.Error, 7);
        else
          internId.OnPviLinked(info.Error, info.LinkId, fnNumber);
      }
      return true;
    }

    internal bool PVI_ReadResponse(
      int fnNumber,
      int wParam,
      int lParam,
      IntPtr pData,
      uint dataLen,
      ref ResponseInfo info)
    {
      PviCBEvents internId = (PviCBEvents) this.InternIDs[(object) (uint) lParam];
      if (internId != null)
      {
        if (info.Type == 128)
          internId.OnPviCancelled(info.Error, 9);
        else
          internId.OnPviRead(info.Error, (PVIReadAccessTypes) info.Type, (PVIDataStates) info.Status, pData, dataLen, fnNumber);
      }
      return true;
    }

    internal bool PVI_WriteResponse(
      int fnNumber,
      int wParam,
      int lParam,
      IntPtr pData,
      uint dataLen,
      ref ResponseInfo info)
    {
      PviCBEvents internId = (PviCBEvents) this.InternIDs[(object) (uint) lParam];
      if (internId != null)
      {
        if (info.Type == 128)
          internId.OnPviCancelled(info.Error, 10);
        else
          internId.OnPviWritten(info.Error, (PVIWriteAccessTypes) info.Type, (PVIDataStates) info.Status, fnNumber, pData, dataLen);
      }
      return true;
    }

    internal static bool IsRemoteError(int errorCode) => 12040 == errorCode || 12059 == errorCode || 12094 == errorCode || 12095 == errorCode;

    private struct VS_FIXEDFILEINFO
    {
      public uint dwSignature;
      public uint dwStrucVersion;
      public uint dwFileVersionMS;
      public uint dwFileVersionLS;
      public uint dwProductVersionMS;
      public uint dwProductVersionLS;
      public uint dwFileFlagsMask;
      public uint dwFileFlags;
      public uint dwFileOS;
      public uint dwFileType;
      public uint dwFileSubtype;
      public uint dwFileDateMS;
      public uint dwFileDateLS;
    }

    private delegate void DisposeMSGWindowCB(int mode);

    internal delegate void SetPviDisconnectCallback(uint hPvi);
  }
}
