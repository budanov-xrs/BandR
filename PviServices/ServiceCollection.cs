// Decompiled with JetBrains decompiler
// Type: BR.AN.PviServices.ServiceCollection
// Assembly: BR.AN.PVIServices, Version=1.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DA853F0A-C070-4E30-B7A7-51D955FB85F6
// Assembly location: C:\Users\ss_six\Desktop\FXE\FXE software integration package\COMET_FXE-PVIServices_Example\Pluto-PVI-trial_01\packages\IToolS.BandR.3.5.58\lib\net20\BR.AN.PVIServices.dll

using System.Collections;
using System.IO;
using System.Text;
using System.Xml;

namespace BR.AN.PviServices
{
  public class ServiceCollection : BaseCollection
  {
    private TraceWriter propTracer;
    public static Hashtable Services = new Hashtable();
    private LogicalCollection propLogicalObjects;
    private LogicalObjectsUsage propLogicalUsage;

    public TraceWriter Trace
    {
      get
      {
        if (this.propTracer == null)
          this.propTracer = new TraceWriter();
        return this.propTracer;
      }
    }

    public ServiceCollection()
    {
      this.propCollectionType = CollectionType.HashTable;
      this.propLogicalObjects = new LogicalCollection((object) this, "Logicals");
      this.LogicalObjectsUsage = LogicalObjectsUsage.FullName;
    }

    public int LoadConfiguration(string fileName)
    {
      ConfigurationFlags flags = (ConfigurationFlags) (0 | 4 | 8 | 16 | 32 | 1 | 1024 | 512);
      return this.LoadConfiguration(fileName, flags);
    }

    protected int LoadConfiguration(StreamReader stream, ConfigurationFlags flags)
    {
      XmlTextReader reader = new XmlTextReader((TextReader) stream);
      int content = (int) reader.MoveToContent();
      if (string.Compare(reader.Name, "Services") == 0)
      {
        string attribute = reader.GetAttribute("LogicalUsage");
        if (attribute != null)
        {
          if (attribute.Equals("None"))
            this.propLogicalUsage = LogicalObjectsUsage.None;
          if (attribute.Equals("FullName"))
            this.propLogicalUsage = LogicalObjectsUsage.FullName;
          if (attribute.Equals("ObjectName"))
            this.propLogicalUsage = LogicalObjectsUsage.ObjectName;
          if (attribute.Equals("ObjectNameWithType"))
            this.propLogicalUsage = LogicalObjectsUsage.ObjectNameWithType;
        }
      }
      reader.Read();
      while (reader.NodeType != XmlNodeType.EndElement)
      {
        if (string.Compare(reader.Name, "Service") == 0)
        {
          string attribute = reader.GetAttribute("Name");
          if (!this.ContainsKey((object) attribute))
          {
            Service service = new Service(attribute, this);
            service.LoadConfiguration(reader, flags);
            if (this.Contains((object) service))
              this.Add(service);
            reader.Read();
          }
          else
            reader.Skip();
        }
      }
      reader.Close();
      return 0;
    }

    public int LoadConfiguration(StreamReader stream)
    {
      ConfigurationFlags flags = (ConfigurationFlags) (0 | 4 | 8 | 16 | 32 | 1 | 1024 | 512);
      return this.LoadConfiguration(stream, flags);
    }

    protected int LoadConfiguration(string fileName, ConfigurationFlags flags)
    {
      XmlTextReader reader = new XmlTextReader(fileName);
      reader.WhitespaceHandling = WhitespaceHandling.None;
      int content = (int) reader.MoveToContent();
      if (string.Compare(reader.Name, "Services") == 0)
      {
        string attribute = reader.GetAttribute("LogicalUsage");
        if (attribute != null)
        {
          if (attribute.Equals("None"))
            this.propLogicalUsage = LogicalObjectsUsage.None;
          if (attribute.Equals("FullName"))
            this.propLogicalUsage = LogicalObjectsUsage.FullName;
          if (attribute.Equals("ObjectName"))
            this.propLogicalUsage = LogicalObjectsUsage.ObjectName;
          if (attribute.Equals("ObjectNameWithType"))
            this.propLogicalUsage = LogicalObjectsUsage.ObjectNameWithType;
        }
      }
      reader.Read();
      while (reader.NodeType != XmlNodeType.EndElement)
      {
        if (string.Compare(reader.Name, "Service") == 0)
        {
          string attribute = reader.GetAttribute("Name");
          if (!this.ContainsKey((object) attribute))
          {
            Service service = new Service(attribute, this);
            service.LoadConfiguration(reader, flags);
            if (this.Contains((object) service))
              this.Add(service);
            if (!reader.Read())
              break;
          }
          else
            reader.Skip();
        }
        else
          reader.Read();
      }
      reader.Close();
      return 0;
    }

    public int SaveConfiguration(string fileName)
    {
      ConfigurationFlags flags = (ConfigurationFlags) (0 | 4 | 8 | 16 | 32 | 1 | 1024 | 512);
      return this.SaveConfiguration(fileName, flags);
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
      writer.WriteAttributeString("LogicalUsage", this.propLogicalUsage.ToString());
      if (0 < this.Count)
      {
        foreach (Base @base in (IEnumerable) this.Values)
          @base.ToXMLTextWriter(ref writer, flags);
      }
      writer.WriteEndElement();
      writer.Close();
      w.Close();
      return num;
    }

    public void Add(Service service)
    {
      service.Services = this;
      service.LogicalObjectsUsage = this.LogicalObjectsUsage;
      this.Add((object) service.Name, (object) service);
    }

    public override void Connect()
    {
      if (0 >= this.Count)
        return;
      foreach (Service service in (IEnumerable) this.Values)
      {
        service.Connected += new PviEventHandler(this.ServiceConnected);
        service.Error += new PviEventHandler(this.ServiceError);
        service.Connect();
      }
    }

    public void Disconnect()
    {
      this.propCounter = 0;
      this.propErrorCount = 0;
      if (0 >= this.Count)
        return;
      foreach (Service service in (IEnumerable) this.Values)
      {
        service.Disconnect();
        service.Disconnected += new PviEventHandler(this.ServiceDisconnected);
      }
    }

    private void ServiceConnected(object sender, PviEventArgs e)
    {
      ++this.propCounter;
      ((Base) sender).Connected -= new PviEventHandler(this.ServiceConnected);
      int propCounter = this.propCounter;
      int count = this.Count;
    }

    private void ServiceDisconnected(object sender, PviEventArgs e)
    {
      ++this.propCounter;
      ((Base) sender).Disconnected -= new PviEventHandler(this.ServiceDisconnected);
      int propCounter = this.propCounter;
      int count = this.Count;
    }

    private void ServiceError(object sender, PviEventArgs e)
    {
      ++this.propErrorCount;
      ((Base) sender).Error -= new PviEventHandler(this.ServiceError);
    }

    internal bool ContainsLogicalObject(string logical) => this.propLogicalObjects != null && this.propLogicalObjects.ContainsKey((object) logical);

    internal int AddLogicalObject(string logical, object obj)
    {
      int num = 0;
      if (this.propLogicalObjects == null)
        this.propLogicalObjects = new LogicalCollection((object) this, "Logicals");
      this.propLogicalObjects.Add((object) logical, obj);
      return num;
    }

    internal void RemoveLogicalObject(string logical)
    {
      if (this.propLogicalObjects == null)
        return;
      this.propLogicalObjects.Remove(logical);
    }

    public LogicalCollection LogicalObjects => this.propLogicalObjects;

    public LogicalObjectsUsage LogicalObjectsUsage
    {
      get => this.propLogicalUsage;
      set => this.propLogicalUsage = value;
    }

    public Service this[string name] => this.propCollectionType == CollectionType.HashTable ? (Service) this[(object) name] : (Service) null;
  }
}
