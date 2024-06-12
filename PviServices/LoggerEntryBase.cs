// Decompiled with JetBrains decompiler
// Type: BR.AN.PviServices.LoggerEntryBase
// Assembly: BR.AN.PVIServices, Version=1.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DA853F0A-C070-4E30-B7A7-51D955FB85F6
// Assembly location: C:\Users\ss_six\Desktop\FXE\FXE software integration package\COMET_FXE-PVIServices_Example\Pluto-PVI-trial_01\packages\IToolS.BandR.3.5.58\lib\net20\BR.AN.PVIServices.dll

using System;

namespace BR.AN.PviServices
{
  public class LoggerEntryBase : IDisposable
  {
    internal string propRuntimeVersion;
    internal int propFacilityCode;
    internal int propCustomerCode;
    internal int propEventId;
    private ulong propUKey;
    protected string propLoggerModuleName;
    internal LevelType propLevelType;
    internal DateTime propDateTime;
    internal uint propErrorInfo;
    internal uint propErrorNumber;
    internal string propErrorText;
    internal byte[] propBinary;
    internal Exception propException;
    internal string propTask;
    internal uint propID;
    internal uint propInternID;
    internal int propArrayIndex;
    internal int propSArrayIndex;
    internal bool propDisposed;
    internal object propParent;
    private string propCurrentCulture = string.Empty;
    private string propErrorDescription = string.Empty;

    private void Initialize(object parent, DateTime dateTime, bool addKeyOnly, bool reverseOrder)
    {
      this.propFacilityCode = 0;
      this.propEventId = 0;
      this.propLoggerModuleName = "";
      if (parent is Logger)
        this.propLoggerModuleName = ((Base) parent).Name;
      this.propRuntimeVersion = "";
      this.propDisposed = false;
      this.propUKey = 0UL;
      this.propArrayIndex = 0;
      this.propSArrayIndex = 0;
      this.propException = (Exception) null;
      this.propParent = parent;
      this.propDateTime = dateTime;
      this.propID = 0U;
      this.propLevelType = LevelType.Success;
      this.propErrorNumber = 0U;
      this.propErrorText = "";
      this.propBinary = (byte[]) null;
      this.propTask = "";
      this.propErrorInfo = 0U;
      if (!(this.propParent is Logger))
        return;
      if (reverseOrder)
        this.propID = --this.Service.EntryIDDec;
      else
        this.propID = ++this.Service.EntryIDInc;
    }

    internal LoggerEntryBase() => this.Initialize((object) null, new DateTime(0L), false, false);

    [CLSCompliant(false)]
    public LoggerEntryBase(DateTime dateTime) => this.Initialize((object) null, dateTime, false, false);

    [CLSCompliant(false)]
    public LoggerEntryBase(object parent, DateTime dateTime) => this.Initialize(parent, dateTime, false, false);

    [CLSCompliant(false)]
    public LoggerEntryBase(object parent, string strDT)
    {
      string str1 = "0";
      string str2 = strDT.Replace(',', '.');
      int length = str2.IndexOf(".");
      long num = 0;
      if (str2.Length - length - 1 > 1)
        str1 = str2.Substring(length + 1, str2.Length - length - 1);
      string str3 = str2.Substring(0, length);
      DateTime dateTime1;
      if (str3.IndexOf('-') != -1)
      {
        num = (long) System.Convert.ToInt32(str1);
        string[] strArray = str3.Split('-');
        dateTime1 = strArray.Length != 6 ? new DateTime() : new DateTime(System.Convert.ToInt32(strArray.GetValue(0).ToString()), System.Convert.ToInt32(strArray.GetValue(1).ToString()), System.Convert.ToInt32(strArray.GetValue(2).ToString()), System.Convert.ToInt32(strArray.GetValue(3).ToString()), System.Convert.ToInt32(strArray.GetValue(4).ToString()), System.Convert.ToInt32(strArray.GetValue(5).ToString()));
      }
      else
      {
        if (str1 != "0")
          num = System.Convert.ToInt64(str1) / 1000L;
        dateTime1 = Pvi.UInt32ToDateTime(System.Convert.ToUInt32(str3));
      }
      DateTime dateTime2 = dateTime1.AddTicks(num);
      this.Initialize(parent, dateTime2, false, false);
    }

    [CLSCompliant(false)]
    public LoggerEntryBase(object parent, DateTime dateTime, bool addKeyOnly, bool reverseOrder) => this.Initialize(parent, dateTime, addKeyOnly, reverseOrder);

    ~LoggerEntryBase() => this.Dispose(false);

    public event DisposeEventHandler Disposing;

    internal virtual void OnDisposing(bool disposing)
    {
      if (this.Disposing == null)
        return;
      this.Disposing((object) this, new DisposeEventArgs(disposing));
    }

    public void Dispose()
    {
      if (this.propDisposed)
        return;
      this.Dispose(true);
      GC.SuppressFinalize((object) this);
    }

    internal virtual void Dispose(bool disposing)
    {
      if (this.propDisposed)
        return;
      this.OnDisposing(disposing);
      if (!disposing)
        return;
      this.propDisposed = true;
      if (this.propBinary != null)
        this.propBinary = (byte[]) null;
      this.propErrorText = (string) null;
      this.propException = (Exception) null;
      this.propParent = (object) null;
      this.propTask = (string) null;
      this.propParent = (object) null;
    }

    public override string ToString()
    {
      string str = "";
      if (this.propBinary != null)
      {
        for (int index = 0; index < this.propBinary.GetLength(0); ++index)
          str += string.Format("{0:X2} ", this.propBinary.GetValue(index));
      }
      return "ID=\"" + this.propID.ToString() + "\" DateTime=\"" + this.propDateTime.ToString("s") + "." + string.Format("{0:000}", (object) this.propDateTime.Millisecond) + "\" ErrorNumber=\"" + this.propErrorNumber.ToString() + "\" Binary=\"" + str + "\" CodeOffset=\"0x" + string.Format("{0:X8}", (object) this.CodeOffset) + "\" ErrorDescription=\"" + this.ErrorDescription + "\" ErrorInfo=\"0x" + string.Format("{0:X8}", (object) this.ErrorInfo) + "\" ErrorText=\"" + this.propErrorText + "\" LevelType=\"" + this.propLevelType.ToString() + "\" Task=\"" + this.propTask + "\"" + (this.propException != null ? this.propException.ToString() : "");
    }

    internal string LevelToString(int levelType, int error)
    {
      string str;
      switch (levelType)
      {
        case 0:
          str = "Success";
          break;
        case 1:
        case 3:
          str = error == 0 ? "Info" : "Fatal";
          break;
        case 2:
          str = "Warning";
          break;
        case 4:
          str = "Debug";
          break;
        case 129:
          str = "FatalUser";
          break;
        case 130:
          str = "WarningUser";
          break;
        default:
          str = "Info";
          break;
      }
      return str;
    }

    internal string DateTimeToString() => string.Format("{0}-{1:00}-{2:00}T{3:00}:{4:00}:{5:00}.{6:000}", (object) this.DateTime.Year, (object) this.DateTime.Month, (object) this.DateTime.Day, (object) this.DateTime.Hour, (object) this.DateTime.Minute, (object) this.DateTime.Second, (object) this.DateTime.Millisecond);

    internal virtual string ToStringHTM(uint contentVersion)
    {
      string str = "<td align=\"right\">";
      return string.Format("<td align=\"center\" width=\"220\">{0}</td>\r\n{1}{2}</td>\r\n{3}{4}</td>\r\n{5}{6}</td>\r\n", (object) this.DateTimeToString(), (object) str, (object) this.ErrorNumber, (object) str, (object) this.LevelToString((int) this.LevelType, (int) this.ErrorNumber), (object) str, (object) this.Task);
    }

    internal virtual string ToStringCSV(uint contentVersion) => string.Format("\"{0}\";\"{1}\";\"{2}\";\"{3}\";\"{4}\";", (object) this.DateTimeToString(), (object) this.ErrorNumber.ToString(), (object) this.LevelToString((int) this.LevelType, (int) this.ErrorNumber), (object) this.ErrorInfo, (object) this.Task);

    internal virtual string BinaryToString()
    {
      string str = "";
      if (this.propBinary != null)
      {
        if (0 < this.propBinary.GetLength(0))
        {
          str = string.Format("{0:X2}", this.propBinary.GetValue(0));
          for (int index = 1; index < this.propBinary.GetLength(0); ++index)
            str += string.Format(" {0:X2}", this.propBinary.GetValue(index));
        }
      }
      else
        str = "00 00 00 00";
      return str;
    }

    public LevelType LevelType => this.propLevelType;

    public DateTime DateTime => this.propDateTime;

    [CLSCompliant(false)]
    public uint ErrorNumber => this.propErrorNumber;

    public long ErrorInfo => (long) this.propErrorInfo;

    [CLSCompliant(false)]
    public uint ModuleIndex => this.propException != null && this.propException.Backtrace != null ? this.propException.Backtrace.propInfo : 0U;

    [CLSCompliant(false)]
    public uint TaskIndex => this.propException != null && this.propException.Backtrace != null ? this.propException.Backtrace.propTaskIdx : uint.MaxValue;

    [CLSCompliant(false)]
    public uint CodeOffset => this.propException != null && this.propException.Backtrace != null && this.propException.Backtrace.PCInfo != null ? this.propException.Backtrace.PCInfo.CodeOffset : 0U;

    public string Task => this.propTask;

    private string GetErrorDescription(string language, int eNum) => this.Service == null ? Service.GetErrorTextPCC(eNum, language) : this.Service.Utilities.GetErrorTextPCC(eNum);

    private string GetErrorDescription(string language, uint eNum) => this.Service == null ? Service.GetErrorTextPCC(eNum, language) : this.Service.Utilities.GetErrorTextPCC(eNum);

    private void MakeErrorDescriptionText(string newLanguage)
    {
      this.propCurrentCulture = newLanguage;
      if (1 < this.propFacilityCode || this.propCustomerCode != 0)
        this.propErrorDescription = this.GetErrorDescription(this.propCurrentCulture, this.propEventId);
      else
        this.propErrorDescription = this.GetErrorDescription(this.propCurrentCulture, this.propErrorNumber);
    }

    public virtual string ErrorDescription
    {
      get
      {
        string str = "en";
        if (this.Service != null)
          str = this.Service.Language;
        if (this.propErrorDescription == string.Empty)
        {
          this.MakeErrorDescriptionText(str);
          return this.propErrorDescription;
        }
        if (this.propCurrentCulture.CompareTo(str) == 0)
          return this.propErrorDescription;
        this.propCurrentCulture = str;
        this.MakeErrorDescriptionText(str);
        return this.propErrorDescription;
      }
    }

    public string ErrorText => this.propErrorText;

    public byte[] Binary => this.propBinary;

    public Exception Exception => this.propException;

    [CLSCompliant(false)]
    public uint ID => this.propID;

    public string Key => this.propUKey.ToString();

    [CLSCompliant(false)]
    public ulong UniqueKey => this.propUKey;

    [CLSCompliant(false)]
    public uint InternId => this.propInternID;

    internal void UpdateUKey()
    {
      if (this.propParent is Module)
      {
        this.propUKey = (ulong) ((Module) this.propParent).ModUID << 32;
        this.propUKey += (ulong) this.propInternID;
      }
      else
      {
        if (!(this.propParent is Cpu))
          return;
        this.propUKey = (ulong) ((Cpu) this.propParent).ModUID << 32;
        this.propUKey += (ulong) this.propInternID;
      }
    }

    internal void UpdateUKey(ulong ulKey) => this.propUKey = ulKey;

    public Logger LoggerModule => (Logger) this.propParent;

    public string LoggerModuleName => this.propLoggerModuleName;

    public Service Service
    {
      get
      {
        if (this.propParent is Service)
          return (Service) this.propParent;
        if (this.propParent is Cpu)
          return ((Base) this.propParent).Service;
        return this.propParent is Logger ? ((Base) this.propParent).Service : (Service) null;
      }
    }
  }
}
