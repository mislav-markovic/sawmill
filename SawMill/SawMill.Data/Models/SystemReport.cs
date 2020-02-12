using System;
using System.Collections.Generic;

namespace SawMill.Data.Models
{
  public class SystemReport
  {
    public int SystemReportId { get; set; }
    public int SystemId { get; set; }
    public virtual System System { get; set; }
    public DateTime Timestamp { get; set; }
    public virtual ICollection<SystemReportAlertGroup> SystemReportAlertGroup { get; set; }
    public virtual ICollection<ComponentReport> ComponentReports { get; set; }
  }
}