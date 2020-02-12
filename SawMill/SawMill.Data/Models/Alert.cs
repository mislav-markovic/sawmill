using System.Collections.Generic;

namespace SawMill.Data.Models
{
  public class Alert
  {
    public int AlertId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public long Timespan { get; set; }
    public int Threshold { get; set; }
    public string Value { get; set; }
    public int GeneralRuleId { get; set; }
    public bool HasConstantValue { get; set; }
    public virtual GeneralRule GeneralRule { get; set; }
    public int ComponentId { get; set; }
    public virtual Component Component { get; set; }


    public virtual ICollection<AlertGroupAlert> AlertGroupAlerts { get; set; }
    public virtual ICollection<AlertValue> AlertValues { get; set; }
    public virtual ICollection<ComponentReportAlert> ComponentReportAlerts { get; set; }
  }
}