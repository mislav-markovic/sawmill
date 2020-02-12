using System.Collections.Generic;

namespace SawMill.Data.Models
{
  public class System
  {
    public System()
    {
      Component = new HashSet<Component>();
      AlertGroup = new List<AlertGroup>();
      SystemReports = new List<SystemReport>();
    }

    public System(
      int systemId,
      string systemName,
      string systemDescription,
      ICollection<Component> component)
    {
      SystemId = systemId;
      SystemName = systemName;
      SystemDescription = systemDescription;
      Component = component;
    }

    public System(int systemId, string systemName, string systemDescription) : this(systemId, systemName,
      systemDescription, null)
    {
    }

    public int SystemId { get; set; }
    public string SystemName { get; set; }
    public string SystemDescription { get; set; }

    public virtual ICollection<Component> Component { get; set; }
    public virtual ICollection<AlertGroup> AlertGroup { get; set; }
    public virtual ICollection<SystemReport> SystemReports { get; set; }
  }
}