using System.Collections.Generic;

namespace SawMill.Data.Models
{
  public class Component
  {
    public Component()
    {
      Alerts = new HashSet<Alert>();
      NormalizedLog = new HashSet<NormalizedLog>();
      RawLog = new HashSet<RawLog>();
      ComponentReports = new List<ComponentReport>();
    }

    public int ComponentId { get; set; }
    public string ComponentName { get; set; }
    public string ComponentDescription { get; set; }
    public int? SystemId { get; set; }
    public int? ParsingRulesId { get; set; }

    public virtual ParsingRules ParsingRules { get; set; }
    public virtual System System { get; set; }
    public virtual ICollection<Alert> Alerts { get; set; }
    public virtual ICollection<NormalizedLog> NormalizedLog { get; set; }
    public virtual ICollection<RawLog> RawLog { get; set; }
    public virtual ICollection<ComponentReport> ComponentReports { get; set; }
  }
}