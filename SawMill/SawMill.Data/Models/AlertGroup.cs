using System.Collections.Generic;

namespace SawMill.Data.Models
{
  public class AlertGroup
  {
    public AlertGroup()
    {
      AlertGroupAlert = new List<AlertGroupAlert>();
      AlertGroupValues = new List<AlertGroupValue>();
      SystemReportAlertGroups = new List<SystemReportAlertGroup>();
    }

    public int AlertGroupId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public long CorrelationWindow { get; set; }
    public int SystemId { get; set; }
    public virtual System System { get; set; }
    public virtual ICollection<AlertGroupAlert> AlertGroupAlert { get; set; }
    public virtual ICollection<AlertGroupValue> AlertGroupValues { get; set; }
    public virtual ICollection<SystemReportAlertGroup> SystemReportAlertGroups { get; set; }
  }
}